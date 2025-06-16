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

using Fiscalapi.Credentials.Core;
using XmlDownloader.Auth.Models;
using XmlDownloader.Download.Models;

namespace XmlDownloader.Download;

/// <summary>
/// Service interface for downloading packages from SAT.
/// </summary>
public interface IDownloadService
{
    /// <summary>
    /// Downloads a package from SAT using the provided credential, authentication token and package ID.s
    /// </summary>
    /// <param name="credential">Fiel</param>
    /// <param name="authToken">Authentication token</param>
    /// <param name="packageId">PackageID</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>DownloadResponse</returns>
    Task<DownloadResponse> DownloadAsync(ICredential credential, Token authToken, string packageId,
        CancellationToken cancellationToken = default);
}