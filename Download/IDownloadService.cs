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
using Fiscalapi.XmlDownloader.Auth.Models;
using Fiscalapi.XmlDownloader.Common.Http;
using Fiscalapi.XmlDownloader.Download.Models;
using Microsoft.Extensions.Logging;

namespace Fiscalapi.XmlDownloader.Download;

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
    /// <param name="endpoints">Service endpoints to use for download</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <param name="logger">Logger</param>
    /// <returns>DownloadResponse</returns>
    Task<DownloadResponse> DownloadAsync(ICredential credential, Token authToken, string packageId,
         ServiceEndpoints endpoints, ILogger logger, CancellationToken cancellationToken = default);
}