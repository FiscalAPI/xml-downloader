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

namespace XmlDownloader;

public class XmlDownloaderSettings
{
    /// <summary>
    /// Package file extension
    /// </summary>
    public static string PackageExtension { get; set; } = ".zip";

    /// <summary>
    /// CFDI file extension
    /// </summary>
    public static string CfdiExtension { get; set; } = ".xml";

    /// <summary>
    /// Metadata file extension
    /// </summary>
    public static string MetaExtension { get; set; } = ".txt";

    /// <summary>
    /// Folder to perform temp operations like folder extractions
    /// </summary>
    public static string TempDirectory { get; set; } = string.Empty;

    /// <summary>
    /// Folder where packages will be saved on.
    /// </summary>
    public static string PackagesDirectory { get; set; } = string.Empty;
}