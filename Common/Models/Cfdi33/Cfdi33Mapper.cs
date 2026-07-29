/*
 * ============================================================================
 * Mozilla Public License 2.0 (MPL-2.0)
 * Autor: FISCAL API S. DE R.L. DE C.V. - https://fiscalapi.com
 * ============================================================================
 *
 * Este código está sujeto a los términos de la Mozilla Public License v2.0.
 * Licencia completa: https://mozilla.org/MPL/2.0
 *
 * AVISO: Este software se proporciona "tal como está" sin garantías de ningún
 * tipo. Al usar, modificar o distribuir este código debe mantener esta
 * atribución y las referencias al autor.
 *
 * ============================================================================
 */


using System.Collections.Concurrent;
using System.Reflection;
using System.Xml.Serialization;
using V40 = Fiscalapi.XmlDownloader.Common.Models;

namespace Fiscalapi.XmlDownloader.Common.Models.Cfdi33;

/// <summary>
/// Convierte un CFDI 3.3 al modelo unificado 4.0 (<see cref="V40.Comprobante"/>).
/// El mapeo nunca falla el documento: los códigos de catálogo del 3.3 (strings crudos)
/// que no existan en los enums del catálogo 4.0 degradan a warning — en atributos
/// opcionales se omiten (<c>Specified = false</c>) y en atributos requeridos queda el
/// valor default del enum. El XML original siempre viaja en <c>ComprobanteResult.RawXml</c>.
/// </summary>
public static class Cfdi33Mapper
{
    private static readonly ConcurrentDictionary<Type, Dictionary<string, object>> Cache = new();

    /// <summary>
    /// Mapea un CFDI 3.3 a la forma 4.0 preservando <c>Version = "3.3"</c>.
    /// Los atributos exclusivos de 4.0 quedan null o con <c>Specified = false</c>.
    /// Nota: los traslados a nivel comprobante quedan con <c>Base = 0</c> (el atributo no existe en 3.3).
    /// </summary>
    /// <param name="source">Comprobante 3.3 deserializado.</param>
    /// <param name="warnings">Acumulador de advertencias no fatales del mapeo.</param>
    public static V40.Comprobante ToComprobante(Comprobante source, List<string> warnings)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(warnings);

        var (formaPago, formaPagoSpecified) =
            MapOptional<c_FormaPago>(source.FormaPago, "Comprobante.FormaPago", warnings);
        var (metodoPago, metodoPagoSpecified) =
            MapOptional<c_MetodoPago>(source.MetodoPago, "Comprobante.MetodoPago", warnings);

        return new V40.Comprobante
        {
            Version = source.Version,
            Serie = source.Serie,
            Folio = source.Folio,
            Fecha = source.Fecha,
            Sello = source.Sello,
            NoCertificado = source.NoCertificado,
            Certificado = source.Certificado,
            CondicionesDePago = source.CondicionesDePago,
            SubTotal = source.SubTotal,
            Descuento = source.Descuento,
            DescuentoSpecified = source.DescuentoSpecified,
            Moneda = MapRequired<c_Moneda>(source.Moneda, "Comprobante.Moneda", warnings),
            TipoCambio = source.TipoCambio,
            TipoCambioSpecified = source.TipoCambioSpecified,
            Total = source.Total,
            TipoDeComprobante =
                MapRequired<c_TipoDeComprobante>(source.TipoDeComprobante, "Comprobante.TipoDeComprobante", warnings),
            FormaPago = formaPago,
            FormaPagoSpecified = formaPagoSpecified,
            MetodoPago = metodoPago,
            MetodoPagoSpecified = metodoPagoSpecified,
            ExportacionSpecified = false,
            LugarExpedicion = source.LugarExpedicion,
            Confirmacion = source.Confirmacion,
            InformacionGlobal = null,
            CfdiRelacionados = MapCfdiRelacionados(source.CfdiRelacionados, warnings),
            Emisor = MapEmisor(source.Emisor, warnings),
            Receptor = MapReceptor(source.Receptor, warnings),
            Conceptos = source.Conceptos?.Select(c => MapConcepto(c, warnings)).ToArray(),
            Impuestos = MapImpuestos(source.Impuestos, warnings),
            Complemento = FlattenComplemento(source.Complemento),
            Addenda = source.Addenda?.Any is { Length: > 0 }
                ? new V40.ComprobanteAddenda { Any = source.Addenda.Any }
                : null
        };
    }

    private static V40.ComprobanteCfdiRelacionados[]? MapCfdiRelacionados(
        ComprobanteCfdiRelacionados? source, List<string> warnings)
    {
        if (source is null) return null;

        return new[]
        {
            new V40.ComprobanteCfdiRelacionados
            {
                TipoRelacion =
                    MapRequired<c_TipoRelacion>(source.TipoRelacion, "CfdiRelacionados.TipoRelacion", warnings),
                CfdiRelacionado = source.CfdiRelacionado?
                    .Select(r => new V40.ComprobanteCfdiRelacionadosCfdiRelacionado { UUID = r.UUID })
                    .ToArray()
            }
        };
    }

    private static V40.ComprobanteEmisor? MapEmisor(ComprobanteEmisor? source, List<string> warnings)
    {
        if (source is null) return null;

        return new V40.ComprobanteEmisor
        {
            Rfc = source.Rfc,
            Nombre = source.Nombre,
            RegimenFiscal = MapRequired<c_RegimenFiscal>(source.RegimenFiscal, "Emisor.RegimenFiscal", warnings),
            FacAtrAdquirente = null
        };
    }

    private static V40.ComprobanteReceptor? MapReceptor(ComprobanteReceptor? source, List<string> warnings)
    {
        if (source is null) return null;

        var (residenciaFiscal, residenciaFiscalSpecified) =
            MapOptional<c_Pais>(source.ResidenciaFiscal, "Receptor.ResidenciaFiscal", warnings);
        var (usoCfdi, usoCfdiSpecified) =
            MapOptional<c_UsoCFDI>(source.UsoCFDI, "Receptor.UsoCFDI", warnings);

        return new V40.ComprobanteReceptor
        {
            Rfc = source.Rfc,
            Nombre = source.Nombre,
            DomicilioFiscalReceptor = null,
            ResidenciaFiscal = residenciaFiscal,
            ResidenciaFiscalSpecified = residenciaFiscalSpecified,
            NumRegIdTrib = source.NumRegIdTrib,
            RegimenFiscalReceptorSpecified = false,
            UsoCFDI = usoCfdi,
            UsoCFDISpecified = usoCfdiSpecified
        };
    }

    private static V40.ComprobanteConcepto MapConcepto(ComprobanteConcepto source, List<string> warnings)
    {
        return new V40.ComprobanteConcepto
        {
            ClaveProdServ = source.ClaveProdServ,
            NoIdentificacion = source.NoIdentificacion,
            Cantidad = source.Cantidad,
            ClaveUnidad = source.ClaveUnidad,
            Unidad = source.Unidad,
            Descripcion = source.Descripcion,
            ValorUnitario = source.ValorUnitario,
            Importe = source.Importe,
            Descuento = source.Descuento,
            DescuentoSpecified = source.DescuentoSpecified,
            ObjetoImpSpecified = false,
            ACuentaTerceros = null,
            Impuestos = MapConceptoImpuestos(source.Impuestos, warnings),
            InformacionAduanera = source.InformacionAduanera?
                .Select(a => new V40.ComprobanteConceptoInformacionAduanera { NumeroPedimento = a.NumeroPedimento })
                .ToArray(),
            CuentaPredial = source.CuentaPredial is null
                ? null
                : new[] { new V40.ComprobanteConceptoCuentaPredial { Numero = source.CuentaPredial.Numero } },
            ComplementoConcepto = source.ComplementoConcepto?.Any is { Length: > 0 }
                ? new V40.ComprobanteConceptoComplementoConcepto { Any = source.ComplementoConcepto.Any }
                : null,
            Parte = source.Parte?.Select(p => MapParte(p)).ToArray()
        };
    }

    private static V40.ComprobanteConceptoImpuestos? MapConceptoImpuestos(
        ComprobanteConceptoImpuestos? source, List<string> warnings)
    {
        if (source is null) return null;

        return new V40.ComprobanteConceptoImpuestos
        {
            Traslados = source.Traslados?.Select(t => new V40.ComprobanteConceptoImpuestosTraslado
            {
                Base = t.Base,
                Impuesto = MapRequired<c_Impuesto>(t.Impuesto, "Concepto.Traslado.Impuesto", warnings),
                TipoFactor = MapRequired<c_TipoFactor>(t.TipoFactor, "Concepto.Traslado.TipoFactor", warnings),
                TasaOCuota = t.TasaOCuota,
                TasaOCuotaSpecified = t.TasaOCuotaSpecified,
                Importe = t.Importe,
                ImporteSpecified = t.ImporteSpecified
            }).ToArray(),
            Retenciones = source.Retenciones?.Select(r => new V40.ComprobanteConceptoImpuestosRetencion
            {
                Base = r.Base,
                Impuesto = MapRequired<c_Impuesto>(r.Impuesto, "Concepto.Retencion.Impuesto", warnings),
                TipoFactor = MapRequired<c_TipoFactor>(r.TipoFactor, "Concepto.Retencion.TipoFactor", warnings),
                TasaOCuota = r.TasaOCuota,
                Importe = r.Importe
            }).ToArray()
        };
    }

    private static V40.ComprobanteConceptoParte MapParte(ComprobanteConceptoParte source)
    {
        return new V40.ComprobanteConceptoParte
        {
            ClaveProdServ = source.ClaveProdServ,
            NoIdentificacion = source.NoIdentificacion,
            Cantidad = source.Cantidad,
            Unidad = source.Unidad,
            Descripcion = source.Descripcion,
            ValorUnitario = source.ValorUnitario,
            ValorUnitarioSpecified = source.ValorUnitarioSpecified,
            Importe = source.Importe,
            ImporteSpecified = source.ImporteSpecified,
            InformacionAduanera = source.InformacionAduanera?
                .Select(a => new V40.ComprobanteConceptoParteInformacionAduanera
                {
                    NumeroPedimento = a.NumeroPedimento
                })
                .ToArray()
        };
    }

    private static V40.ComprobanteImpuestos? MapImpuestos(ComprobanteImpuestos? source, List<string> warnings)
    {
        if (source is null) return null;

        return new V40.ComprobanteImpuestos
        {
            Retenciones = source.Retenciones?.Select(r => new V40.ComprobanteImpuestosRetencion
            {
                Impuesto = MapRequired<c_Impuesto>(r.Impuesto, "Impuestos.Retencion.Impuesto", warnings),
                Importe = r.Importe
            }).ToArray(),
            // En 3.3 el traslado a nivel comprobante no tiene atributo Base: queda en 0.
            // TasaOCuota e Importe son requeridos en 3.3 y opcionales en 4.0 → Specified = true.
            Traslados = source.Traslados?.Select(t => new V40.ComprobanteImpuestosTraslado
            {
                Base = 0,
                Impuesto = MapRequired<c_Impuesto>(t.Impuesto, "Impuestos.Traslado.Impuesto", warnings),
                TipoFactor = MapRequired<c_TipoFactor>(t.TipoFactor, "Impuestos.Traslado.TipoFactor", warnings),
                TasaOCuota = t.TasaOCuota,
                TasaOCuotaSpecified = true,
                Importe = t.Importe,
                ImporteSpecified = true
            }).ToArray(),
            TotalImpuestosRetenidos = source.TotalImpuestosRetenidos,
            TotalImpuestosRetenidosSpecified = source.TotalImpuestosRetenidosSpecified,
            TotalImpuestosTrasladados = source.TotalImpuestosTrasladados,
            TotalImpuestosTrasladadosSpecified = source.TotalImpuestosTrasladadosSpecified
        };
    }

    private static V40.ComprobanteComplemento? FlattenComplemento(ComprobanteComplemento[]? source)
    {
        var elements = source?
            .Where(c => c?.Any is { Length: > 0 })
            .SelectMany(c => c.Any)
            .ToArray();

        return elements is { Length: > 0 }
            ? new V40.ComprobanteComplemento { Any = elements }
            : null;
    }

    /// <summary>
    /// Mapea un código SAT requerido en 4.0: código desconocido → default del enum + warning.
    /// </summary>
    private static TEnum MapRequired<TEnum>(string? code, string attribute, List<string> warnings)
        where TEnum : struct, Enum
    {
        if (TryToCatalogEnum<TEnum>(code, out var value))
            return value;

        var fallback = default(TEnum);
        if (!string.IsNullOrEmpty(code))
            warnings.Add(
                $"{attribute}: código '{code}' no existe en {typeof(TEnum).Name} (catálogo 4.0); " +
                $"queda el valor default '{fallback}'.");
        return fallback;
    }

    /// <summary>
    /// Mapea un código SAT opcional en 4.0 (con flag Specified): ausente → omitido sin warning;
    /// desconocido → omitido con warning.
    /// </summary>
    private static (TEnum Value, bool Specified) MapOptional<TEnum>(string? code, string attribute,
        List<string> warnings) where TEnum : struct, Enum
    {
        if (string.IsNullOrEmpty(code))
            return (default, false);

        if (TryToCatalogEnum<TEnum>(code, out var value))
            return (value, true);

        warnings.Add(
            $"{attribute}: código '{code}' no existe en {typeof(TEnum).Name} (catálogo 4.0); se omite.");
        return (default, false);
    }

    /// <summary>
    /// Conversión código SAT → enum de catálogo con la misma semántica que XmlSerializer:
    /// la clave es <c>[XmlEnum("...")]</c> cuando existe y el nombre del miembro en caso contrario.
    /// (Distinto de <c>ToEnumElement</c>, que usa <c>[EnumCode]</c> y es exclusivo de los enums
    /// de negocio como SatStatus.)
    /// </summary>
    private static bool TryToCatalogEnum<TEnum>(string? code, out TEnum value) where TEnum : struct, Enum
    {
        value = default;
        if (string.IsNullOrEmpty(code))
            return false;

        var map = Cache.GetOrAdd(typeof(TEnum), static type =>
        {
            var result = new Dictionary<string, object>(StringComparer.Ordinal);
            foreach (var member in Enum.GetValues(type))
            {
                var name = member.ToString()!;
                var xmlEnum = type.GetMember(name).FirstOrDefault()?
                    .GetCustomAttribute<XmlEnumAttribute>()?.Name;
                result[xmlEnum ?? name] = member;
            }

            return result;
        });

        if (!map.TryGetValue(code, out var found))
            return false;

        value = (TEnum)found;
        return true;
    }
}
