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

using Fiscalapi.XmlDownloader.Common.Http;

namespace Fiscalapi.XmlDownloader.Download.Models;

/// <summary>
/// Response from package download service
/// </summary>
public class DownloadResponse : BaseResponse
{
    private string? _base64Package;
    private byte[]? _packageBytes;

    /// <summary>
    /// Base64 encoded package containing the downloaded data.
    /// </summary>
    public string? Base64Package
    {
        get => _base64Package;
        set
        {
            _base64Package = value;
            _packageBytes = null;
        }
    }

    /// <summary>
    /// Bytes of the package containing the downloaded data. Decoded once per instance
    /// (SAT packages can weigh hundreds of MB). Throws <see cref="FormatException"/>
    /// if <see cref="Base64Package"/> is not valid base64.
    /// </summary>
    public byte[] PackageBytes => _packageBytes ??= string.IsNullOrWhiteSpace(_base64Package)
        ? Array.Empty<byte>()
        : Convert.FromBase64String(_base64Package);

    /// <summary>
    /// Size of the package in bytes
    /// </summary>
    public int PackageSize => PackageBytes.Length;
}