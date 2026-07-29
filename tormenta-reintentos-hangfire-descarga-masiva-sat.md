# Plan de remediación — `Fiscalapi.XmlDownloader` v5.0.17 → v6.0.0
## Tormenta de reintentos Hangfire / Descarga masiva SAT (parte NuGet)

> **Referencia obligada:** auditoría completa del incidente en `C:\Users\JesusMendoza\source\repos\fapi-back\hangfire.md` (informe técnico del 28-jul-2026 + adenda forense §7 + investigación de complementos §8, ambas del 29-jul).
> **Plan hermano:** `C:\Users\JesusMendoza\source\repos\fapi-back\tormenta-reintentos-hangfire-descarga-masiva-sat.md` (consumidor). Ambos planes comparten contrato de API (sección "Contrato con el consumidor"); implementar este primero.

---

## 1. Contexto

El 28-jul-2026 producción se congeló ~26 min por una tormenta de reintentos de Hangfire (206,878 jobs Failed, ~9.57M filas). Tres de los cuatro errores determinísticos que alimentan la tormenta **nacen en este paquete**:

| Defecto | Ubicación | Evidencia de la auditoría |
|---|---|---|
| `DownloadResponse.Succeeded = true` con solo HTTP 200, sin validar `CodEstatus` del SAT ni contenido de `<Paquete>` | `Download\DownloadResponseService.cs:60-69` | §7.1: 65 de 85 paquetes pendientes en BD con `Base64Package` vacío; el "Central Directory corrupt" es un ZIP que **nunca existió** (EOCD sobre stream de 0 bytes) |
| `ZipArchive` sin try/catch; deserialización anclada **solo a CFDI 4.0** | `XmlDownloaderService.cs:441/:549`; `Common\Models\Comprobante.cs:25-26` (namespace `cfd/4` fijo, serializer sgen precompilado) | §7.2: paquete `420a4648` atorado desde 26-feb-2026 con 240 XMLs (141 CFDI 3.3 + 99 CFDI 4.0); un solo 3.3 bloquea también las 99 facturas 4.0 válidas. CFDI 3.3 fue válido hasta 31-mar-2023: **toda descarga histórica lo trae garantizado** |
| `MetaItem.CreateFromString` aborta el paquete completo por una línea | `Common\Models\MetaItem.cs:110-137` | 141 `ArgumentException` "Se requieren al menos 11 campos" en 10 min; `DateTime.Parse`/`decimal.Parse`/`int.Parse` con cultura del hilo |

Patrón común: **API todo-o-nada** — una entrada mala aborta el `IAsyncEnumerable` completo y la excepción escapa al consumidor, que reintenta eternamente un fallo determinístico.

## 2. Comportamiento ACTUAL con complementos no modelados (nómina, carta porte, etc.) — validado en código y en producción (auditoría §8)

Pregunta explícita del equipo: *¿qué pasa hoy cuando se descarga un CFDI con complemento de carta porte o nómina, si solo existen modelos de Pagos 2.0 y TFD?*

1. **La deserialización base NO falla.** `ComprobanteComplemento.Any` es `[XmlAnyElement] XmlElement[]` (`Common\Models\ComprobanteComplemento.cs:28-33`): cualquier complemento — nómina 1.2, carta porte 2.0/3.0/3.1, comercio exterior, INE, etc. — entra como `XmlElement` crudo. Un CFDI de nómina o carta porte **no truena el pipeline por sí mismo**. Confirmado en el ensamblado desplegado (auditoría §8.1): solo existen los namespaces de serialización `cfd/4`, `Pagos20` y `TimbreFiscalDigital`; `CartaPorte` no existe ni como string.
2. **Solo se tipan 2 complementos.** `DeserializeComplements()` (`Common\Models\Comprobante.cs:351-372`) recorre `Complemento.Any` y solo materializa las propiedades de `AllComplements` (`Common\Models\AllComplements.cs`): `[Complement("tfd")] TimbreFiscalDigital` y `[Complement("pago20")] Pagos`. Para cualquier otro complemento `targetProperty == null` → `continue`: **se ignora silenciosamente a nivel tipado**. No hay log, no hay warning.
3. **El crudo no se pierde (confirmado con datos de producción, auditoría §8.3).** fapi-back persiste TODOS los elementos de `Complemento.Any` como filas `XmlComplement { ComplementName = LocalName, Base64ComplementValue = OuterXml→base64 }` (`fapi-back\...\XmlProfile.cs:339-356`). En producción: 205,641 filas — TimbreFiscalDigital 173,057 (= total de `Xmls`, exacto), Pagos 14,640 (= CFDIs tipo P), **Nomina 12,878 (= CFDIs tipo N, 1:1), CartaPorte 2,566**, y ~2,500 de otros (ImpuestosLocales, Aerolineas, Divisas, etc.). Cero blobs vacíos o truncados; muestras decodificadas íntegras (CartaPorte 3.1 con `IdCCP`/`Ubicaciones`, Nómina 1.2 con percepciones/deducciones). **No hay pérdida silenciosa en CFDI 4.0.** fapi-back solo consume tipado el TFD (para `Xml.Uuid`); ni siquiera usa `Pago20`.
4. **Lo que SÍ se pierde (auditoría §8.4):**
   - Los complementos sin modelo son **datos opacos**: no consultables relacionalmente (sin columnas para percepciones de nómina, `IdCCP`, ubicaciones, etc.). Explotarlos exigiría modelos + backfill sobre los blobs ya guardados — la materia prima está completa.
   - **ComercioExterior: 0 filas** pese a observarse CFDIs con `cce11` en los paquetes — esos CFDIs son 3.3 y por el bug de §7.2 **el CFDI completo nunca se procesa**. Ahí la pérdida no es del complemento sino del documento entero; el soporte dual 3.3/4.0 de este plan es lo que la corrige (al procesarse los 3.3, sus complementos empezarán a aterrizar como blobs igual que los demás).
5. **Defectos reales del mecanismo:**
   - El match es por **prefijo del elemento** (`element.Prefix == "tfd"` / `"pago20"`). El prefijo XML es decorativo y lo elige el emisor: un TFD con prefijo no estándar → no matchea → `AllComplements.TimbreFiscalDigital` null → el consumidor pierde el `Uuid`. A la inversa, un elemento cuyo prefijo casualmente sea `tfd`/`pago20` pero con contenido distinto → `serializer.Deserialize` **lanza sin try/catch** → aborta el documento y, con la API todo-o-nada actual, **el paquete completo**.
   - En CFDI 3.3 el complemento de pagos es **Pagos 1.0** (namespace `http://www.sat.gob.mx/Pagos`, prefijo típico `pago10`): nunca matchea `pago20` → queda solo crudo. Aceptable (el consumidor no usa Pagos tipado), pero debe quedar documentado.

**Conclusión:** el catch-all + blob es una red de seguridad correcta y validada con producción; los defectos a corregir aquí son (a) match por prefijo en vez de namespace y (b) deserialización de complemento sin try/catch. **Fuera de alcance de esta remediación** (decisión de producto diferida, auditoría §8.5): agregar modelos tipados de Nómina/CartaPorte y el backfill de los 12,878 + 2,566 blobs existentes — la nueva mecánica por namespace de la sección 4.6 deja el terreno listo para sumar modelos sin tocar el pipeline.

## 3. Decisiones ya tomadas (no re-litigar)

- **Soporte dual CFDI 3.3 + 4.0** de raíz — los 3.3 son datos legítimos del SAT (históricos pre-abr-2023).
- **API por-documento**: resultado éxito/fallo por entrada del ZIP; nunca todo-o-nada.
- **ZIP corrupto** = excepción tipada terminal (`CorruptPackageException`), distinta de fallos por-documento.
- Breaking changes de API permitidos (fapi-back es el consumidor principal y se actualiza a la par) → **major bump a 6.0.0**.
- **Los modelos CFDI 3.3 YA ESTÁN en el repo**: `C:\Users\JesusMendoza\source\repos\xml-downloader\Common\Models\Cfdi33\` (`Cfdi33.cs` generado con xsd.exe desde `cfdv33.xsd`, incluido en la misma carpeta). No escribir modelos a mano — partir de ese archivo.
- Sin tests unitarios en el plan (verificación manual del usuario, política del proyecto).

## 4. Plan de implementación

### 4.1 Detección de versión
- Nuevo `Common/Enums/CfdiVersion.cs`: `enum CfdiVersion { Unknown = 0, Cfdi33 = 33, Cfdi40 = 40 }`.
- Nuevo `Common/CfdiVersionDetector.cs`: constantes `Cfdi33Namespace = "http://www.sat.gob.mx/cfd/3"` / `Cfdi40Namespace = "http://www.sat.gob.mx/cfd/4"`; método `Detect(string xml)` que lee **solo el elemento raíz** con `XmlReader` (`DtdProcessing.Prohibit`) y resuelve por `reader.NamespaceURI`. XML malformado (`XmlException`) → `CfdiVersion.Unknown`, **nunca lanza**.

### 4.2 Modelo CFDI 3.3 (ya presente) — `Common/Models/Cfdi33/`
- `Cfdi33.cs`: clase `Comprobante` (partial) en namespace C# `Fiscalapi.XmlDownloader.Cfdi33`, anclada a `cfd/3`, con subtipos y enums propios del XSD (incl. `c_CodigoPostal`; un CP histórico fuera del enum lanza al deserializar ese documento — contenido por el resultado por-documento de 4.4).
- Colisión de nombres con el modelo 4.0 (`Comprobante`, `ComprobanteComplemento`, enums duplicados): usar alias `using Cfdi33Models = Fiscalapi.XmlDownloader.Cfdi33;` en los puntos de consumo.
- Nota estructural: en el generado 3.3, `Complemento` es **array** (`ComprobanteComplemento[]`).

### 4.3 Unificación 3.3 → modelo `Comprobante` (4.0)
Decisión de arquitectura: el modelo unificado ES el `Comprobante` 4.0 existente (superconjunto estructural de 3.3 para todo lo que consume fapi-back; su `XmlProfile` queda casi intacto).
- Nuevo `Common/Models/Cfdi33/Cfdi33Mapper.cs` (estático, sin AutoMapper): `Comprobante ToComprobante(Cfdi33Models.Comprobante source)`.
  - Copia 1:1 de atributos comunes preservando `Version = "3.3"` y los flags `Specified`.
  - Enums de catálogo duplicados entre namespaces: convertir por código SAT con los helpers existentes (`ToEnumCode`/`ToEnumElement`).
  - `CfdiRelacionados` (0..1 en 3.3) → array de un elemento en 4.0.
  - Receptor: `DomicilioFiscalReceptor = null`, `RegimenFiscalReceptor` sin especificar (ver flags en 4.3.1). En 3.3 no existen.
  - Impuestos nivel comprobante: traslados 3.3 no tienen `Base` → `Base = 0` (documentar en XML-doc).
  - `Complemento[]` (array 3.3) → aplanar concatenando todos los `Any` en el único `ComprobanteComplemento.Any` del 4.0. El TFD comparte namespace entre versiones: el UUID se extrae igual.
  - Sin `Exportacion`/`ObjetoImp`/`InformacionGlobal`/`ACuentaTerceros` → null / `Specified = false`.
- **4.3.1 Flags `Specified` nuevos en el modelo 4.0** (para que el consumidor distinga "no presente" de "default de enum" — evita persistir un "601" fantasma en CFDIs 3.3): `ComprobanteReceptor.RegimenFiscalReceptorSpecified`, `ComprobanteReceptor.UsoCFDISpecified`, `Comprobante.ExportacionSpecified`, `ComprobanteConcepto.ObjetoImpSpecified` (patrón `[XmlIgnore] bool`; al deserializar 4.0 el propio `XmlSerializer` los pone en `true` → round-trip 4.0 intacto).

### 4.4 Resultados por documento
- Nuevo `Common/Models/PackageItemResult.cs`:
  ```csharp
  public class ComprobanteResult
  {
      public bool Succeeded => Comprobante is not null;
      public string SourceName { get; init; } = string.Empty; // entrada del zip o ruta
      public CfdiVersion Version { get; init; }
      public Comprobante? Comprobante { get; init; }          // unificado (forma 4.0)
      public string? RawXml { get; init; }                    // siempre presente, incluso en fallo
      public string? ErrorMessage { get; init; }
      public Exception? Error { get; init; }
      public List<string> Warnings { get; init; } = new();    // p.ej. complementos ilegibles
  }

  public class MetaItemResult
  {
      public bool Succeeded => MetaItem is not null;
      public string SourceName { get; init; } = string.Empty;
      public int LineNumber { get; init; }                    // 1-based dentro del .txt
      public MetaItem? MetaItem { get; init; }
      public string? RawLine { get; init; }
      public string? ErrorMessage { get; init; }
  }
  ```
- Nuevo `Common/CorruptPackageException.cs`: ZIP completo ilegible ("Central Directory corrupt" real o stream vacío) = fallo terminal del **paquete**, señalizado con excepción tipada (permite un `catch` específico en el consumidor sin ensuciar el happy path del `IAsyncEnumerable`). Incluye `PackageSize` e inner exception.

### 4.5 Parser central — nuevo `Common/ComprobanteParser.cs`
```csharp
public static ComprobanteResult Parse(string xml, string sourceName)
```
Detecta versión → deserializa con el modelo correcto (`Comprobante` 4.0 o `Cfdi33Models.Comprobante` + `Cfdi33Mapper`) → asigna `Base64Content` → `TryDeserializeComplements()` (warnings). `Unknown` o cualquier excepción → `ComprobanteResult` fallido con el XML crudo y el error. **Nunca lanza.**

### 4.6 Complementos: match por namespace + tolerancia (fix de la sección 2)
- Cambiar el atributo `[Complement]` para declarar el **namespace URI** (`http://www.sat.gob.mx/TimbreFiscalDigital`, `http://www.sat.gob.mx/Pagos20`) y matchear por `element.NamespaceURI` en `DeserializeComplements` — el prefijo es decorativo, el namespace es la única fuente de verdad. Con esto el UUID del TFD se extrae siempre.
- `Comprobante.cs`: nuevo `TryDeserializeComplements()` con try/catch **por complemento** que acumula `List<string>` de warnings (`"{Namespace}: {mensaje}"`); un complemento ilegible degrada a warning + crudo conservado en `Any`, jamás aborta el documento. `DeserializeComplements()` existente queda delegando, `[Obsolete]`.
- Documentar en XML-doc de `AllComplements`: los complementos sin modelo (nómina, carta porte, CCE, INE, Pagos 1.0 de 3.3...) viajan crudos en `Complemento.Any` — es responsabilidad del consumidor persistirlos crudos si los necesita (fapi-back ya lo hace).

### 4.7 Metadata tolerante — `Common/Models/MetaItem.cs`
- Nuevo `TryCreateFromString(string line, out MetaItem? metaItem, out string? error)`: split por `'~'`; `< 11` campos → error descriptivo con el conteo; `DateTime.TryParse`/`decimal.TryParse`/`int.TryParse` con `CultureInfo.InvariantCulture`; fechas opcionales vacías → null, con formato inválido → error. **Nunca lanza.**
- `CreateFromString` queda `[Obsolete]` delegando. Corregir `ToString()` para emitir `Amount` con cultura invariante (round-trip).

### 4.8 Nueva superficie de servicio — `IXmlDownloaderService` / `XmlDownloaderService` (breaking)
Los 6 métodos de lectura de paquetes cambian de retorno:
```csharp
IAsyncEnumerable<ComprobanteResult> GetComprobantesAsync(string fullZipFilePath, string extractToPath, CancellationToken ct = default);
IAsyncEnumerable<ComprobanteResult> GetComprobantesAsync(byte[] packageBytes, CancellationToken ct = default);
IAsyncEnumerable<ComprobanteResult> GetComprobantesAsync(DownloadResponse downloadResponse, CancellationToken ct = default);
IAsyncEnumerable<MetaItemResult> GetMetadataAsync(string fullFilePath, string extractToPath, CancellationToken ct = default);
IAsyncEnumerable<MetaItemResult> GetMetadataAsync(byte[] packageBytes, CancellationToken ct = default);
IAsyncEnumerable<MetaItemResult> GetMetadataAsync(DownloadResponse downloadResponse, CancellationToken ct = default);
```
Implementación (restricción yield+try/catch resuelta extrayendo métodos no-iteradores):
- Apertura del ZIP en método privado `OpenZipArchive(...)`: `InvalidDataException` (incluye el caso de 0 bytes) → `CorruptPackageException` con tamaño del paquete.
- Lectura de **cada entrada** protegida (`TryReadEntryAsync`): un CRC malo en una entrada = `ComprobanteResult` fallido con `RawXml = null`, no paquete caído.
- Por XML: `yield return ComprobanteParser.Parse(xml, entry.FullName)`.
- Por línea de metadata: `TryCreateFromString` → `MetaItemResult` (con `LineNumber` y `RawLine`).
- `LogWarning` por ítem fallido; `LogError` antes de lanzar `CorruptPackageException`.
- Misma cirugía en la variante por ruta (`ExtractZipFile` / `FileStorageService.OpenRead`).

### 4.9 `DownloadResponse.Succeeded` con semántica real (defecto dominante — §7.1 de la auditoría)
`Download\DownloadResponseService.cs`: hoy `Succeeded = true` con solo `IsSuccessStatusCode` (HTTP 200), aunque el SAT responda `CodEstatus ≠ 5000` o `<Paquete>` vacío. Corregir:
```csharp
var hasContent = !string.IsNullOrWhiteSpace(envelope?.Body.BodyData?.Base64Package);
var satOk = status == SatStatus.RequestSucceeded; // CodEstatus 5000
Succeeded = satOk && hasContent;   // éxito = el SAT entregó el paquete real
SatMessage = hasContent ? mensaje : $"{mensaje} (sin contenido en <Paquete>)";
```
"Paquete no disponible/no listo" pasa a `Succeeded = false` con el estatus SAT visible — el consumidor ya tiene rama para ese caso (no persiste, la solicitud sigue en polling). Este fix es el que impide que vuelvan a nacer paquetes vacíos en BD.

### 4.10 Versionado
`XmlDownloader.csproj`: `Version` → **6.0.0**. README: sección "Migración v5 → v6" con la tabla de breaking changes:
1. `GetComprobantesAsync` → `IAsyncEnumerable<ComprobanteResult>` (antes `Comprobante`).
2. `GetMetadataAsync` → `IAsyncEnumerable<MetaItemResult>` (antes `MetaItem`).
3. ZIP ilegible lanza `CorruptPackageException` (antes `InvalidDataException` de BCL).
4. `Succeeded` de `DownloadResponse` = CodEstatus 5000 **y** contenido no vacío (antes: solo HTTP 200).
5. `MetaItem.CreateFromString` y `Comprobante.DeserializeComplements` quedan `[Obsolete]`; parse con cultura invariante.
6. Match de complementos por namespace URI (antes por prefijo).
7. Flags `Specified` nuevos en `Comprobante`/`ComprobanteReceptor`/`ComprobanteConcepto`.

## 5. Orden de implementación

1. `CfdiVersion` + `CfdiVersionDetector` (sin dependencias).
2. Flags `Specified` nuevos en modelos 4.0 (4.3.1).
3. `Cfdi33Mapper` (sobre los modelos ya presentes en `Common/Models/Cfdi33/`).
4. `ComprobanteResult` / `MetaItemResult` / `CorruptPackageException`.
5. `MetaItem.TryCreateFromString` + invariant culture.
6. `[Complement]` por namespace + `TryDeserializeComplements`.
7. `ComprobanteParser`.
8. `IXmlDownloaderService` + `XmlDownloaderService` (firmas nuevas y manejo de ZIP).
9. `DownloadResponseService.Succeeded` (4.9).
10. `csproj` 6.0.0 + README de migración. `dotnet build` + pack.

## 6. Contrato con el consumidor (sincronizado con el plan de fapi-back)

Nombres exactos que fapi-back consumirá: `ComprobanteResult` (propiedades `Succeeded`, `SourceName`, `Version`, `Comprobante`, `RawXml`, `ErrorMessage`, `Warnings`), `MetaItemResult` (`Succeeded`, `SourceName`, `LineNumber`, `MetaItem`, `RawLine`, `ErrorMessage`), `CorruptPackageException`, `CfdiVersion`. Cualquier cambio de nombre debe reflejarse en el plan hermano antes de codificar.

## 7. Riesgos

- **Valores de catálogo históricos ausentes en los enums del XSD 3.3** (regímenes derogados, `c_CodigoPostal` desactualizado): la deserialización de ese documento falla → queda como `ComprobanteResult` fallido con XML crudo, diagnosticable, sin tumbar el paquete. Si aparecen en volumen, relajar ese atributo a `string` en el modelo 33 (decisión diferida, documentar).
- **Retenciones** (namespace propio): fuera de alcance; `CfdiVersionDetector` devuelve `Unknown` → ítem fallido explícito, no crash (el consumidor además filtra Retenciones antes de procesar CFDIs).
- **Re-serialización 4.0** con los flags nuevos: verificada inocua (al deserializar, `XmlSerializer` marca `Specified=true` para atributos presentes).

## 8. Verificación manual (la ejecuta el usuario)

1. `dotnet build` limpio del paquete.
2. Con el `Base64Package` real del paquete `420a4648` (240 XMLs: 141×3.3 + 99×4.0, ver auditoría §7.2): `GetComprobantesAsync` devuelve 240 resultados `Succeeded=true`, `Version` correcta por documento, UUID del TFD presente en todos.
3. Con el paquete de 0 bytes (caso §7.1): lanza `CorruptPackageException`, no `InvalidDataException`.
4. Con un CFDI con complemento de **nómina** y uno de **carta porte**: `Succeeded=true`, complemento intacto en `Complemento.Any` (crudo), sin warnings de error; TFD extraído aunque el prefijo no sea `tfd`.
5. Con un .txt de metadata con una línea malformada: esa línea sale como `MetaItemResult` fallido con `LineNumber`/`RawLine`; el resto parsea.
6. Simular respuesta SAT HTTP 200 con `CodEstatus≠5000` / `<Paquete>` vacío: `DownloadResponse.Succeeded = false`.

## 9. Archivos críticos

`XmlDownloaderService.cs`, `IXmlDownloaderService.cs`, `Download/DownloadResponseService.cs`, `Common/Models/Comprobante.cs`, `Common/Models/AllComplements.cs` (+ atributo `Complement`), `Common/Models/MetaItem.cs`, `Common/Models/Cfdi33/Cfdi33.cs` (ya presente) y nuevo `Common/Models/Cfdi33/Cfdi33Mapper.cs`, nuevos `Common/Enums/CfdiVersion.cs`, `Common/CfdiVersionDetector.cs`, `Common/ComprobanteParser.cs`, `Common/CorruptPackageException.cs`, `Common/Models/PackageItemResult.cs`, `XmlDownloader.csproj`.
