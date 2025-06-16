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

using XmlDownloader.Common.Http;

namespace XmlDownloader.Download.Models;

/// <summary>
/// Response from package download service
/// </summary>
public class DownloadResponse : BaseResponse
{
    /// <summary>
    /// Base64 encoded package containing the downloaded data.
    /// </summary>
    public string? Base64Package { get; set; }

    /// <summary>
    /// Bytes of the package containing the downloaded data.
    /// </summary>
    public byte[] PackageBytes => !string.IsNullOrWhiteSpace(Base64Package)
        ? Convert.FromBase64String(Base64Package)
        : Array.Empty<byte>();

    /// <summary>
    /// Size of the package in bytes
    /// </summary>
    public int PackageSize => PackageBytes.Length;
}