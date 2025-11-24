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
using Fiscalapi.XmlDownloader.Query.Models;
using Microsoft.Extensions.Logging;

namespace Fiscalapi.XmlDownloader.Query;

/// <summary>
/// Service for creating download requests from the SAT.
/// </summary>
public interface IQueryService
{
    /// <summary>
    /// Creates a download request based on the provided parameters.
    /// </summary>
    /// <param name="credential">Fiel</param>
    /// <param name="authToken">Authentication token</param>
    /// <param name="parameters">Request parameters</param>
    /// <param name="endpoints">Service endpoints to use for the query</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <param name="logger">Logger</param>
    /// <returns>QueryResponse</returns>
    Task<QueryResponse> CreateAsync(ICredential credential, Token authToken,
        QueryParameters parameters, ServiceEndpoints endpoints, ILogger logger, CancellationToken cancellationToken = default);
}