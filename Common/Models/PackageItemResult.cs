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


using Fiscalapi.XmlDownloader.Common.Enums;

namespace Fiscalapi.XmlDownloader.Common.Models;

/// <summary>
/// Resultado por documento al leer los XML de un paquete de descarga masiva.
/// Un documento ilegible produce un resultado fallido (con el XML crudo y el error);
/// nunca aborta la enumeración del paquete completo.
/// </summary>
public class ComprobanteResult
{
    /// <summary>
    /// Indica si el documento se procesó correctamente.
    /// </summary>
    public bool Succeeded => Comprobante is not null;

    /// <summary>
    /// Nombre de la entrada del ZIP o ruta del archivo de origen.
    /// </summary>
    public string SourceName { get; init; } = string.Empty;

    /// <summary>
    /// Versión del CFDI detectada (Unknown si el namespace raíz no es cfd/3 ni cfd/4).
    /// </summary>
    public CfdiVersion Version { get; init; }

    /// <summary>
    /// Comprobante unificado en forma 4.0. Los CFDI 3.3 se mapean preservando <c>Version="3.3"</c>.
    /// Null cuando el documento no pudo procesarse.
    /// </summary>
    public Comprobante? Comprobante { get; init; }

    /// <summary>
    /// XML crudo del documento. Presente también en fallos de parseo
    /// (null solo cuando la entrada del ZIP no pudo leerse).
    /// </summary>
    public string? RawXml { get; init; }

    /// <summary>
    /// Mensaje de error cuando <see cref="Succeeded"/> es false.
    /// </summary>
    public string? ErrorMessage { get; init; }

    /// <summary>
    /// Excepción original cuando <see cref="Succeeded"/> es false, si aplica.
    /// </summary>
    public Exception? Error { get; init; }

    /// <summary>
    /// Advertencias no fatales: complementos ilegibles, códigos de catálogo desconocidos
    /// en CFDI 3.3, etc. El documento sigue siendo válido.
    /// </summary>
    public List<string> Warnings { get; init; } = new();
}

/// <summary>
/// Resultado por línea al leer los archivos de metadata (.txt) de un paquete.
/// Una línea malformada produce un resultado fallido; nunca aborta el archivo completo.
/// </summary>
public class MetaItemResult
{
    /// <summary>
    /// Indica si la línea se parseó correctamente.
    /// </summary>
    public bool Succeeded => MetaItem is not null;

    /// <summary>
    /// Nombre de la entrada del ZIP o ruta del archivo de origen.
    /// </summary>
    public string SourceName { get; init; } = string.Empty;

    /// <summary>
    /// Número de línea física (1-based) dentro del archivo .txt.
    /// </summary>
    public int LineNumber { get; init; }

    /// <summary>
    /// Metadata parseada. Null cuando la línea no pudo procesarse.
    /// </summary>
    public MetaItem? MetaItem { get; init; }

    /// <summary>
    /// Línea de texto original.
    /// </summary>
    public string? RawLine { get; init; }

    /// <summary>
    /// Mensaje de error cuando <see cref="Succeeded"/> es false.
    /// </summary>
    public string? ErrorMessage { get; init; }
}
