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
using Fiscalapi.XmlDownloader.Verify.Models;

namespace Fiscalapi.XmlDownloader.Verify;

/// <summary>
/// Service interface for verifying download requests with SAT.
/// </summary>
public interface IVerifyService
{
    /// <summary>
    /// Verifies a download request with SAT using the provided credential, authentication token, and request ID.
    /// </summary>
    /// <param name="credential">Fiel</param>
    /// <param name="authToken">Authentication token</param>
    /// <param name="requestId">Request ID</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>VerifyResponse</returns>
    Task<VerifyResponse> VerifyAsync(ICredential credential, Token authToken, string requestId,
        CancellationToken cancellationToken = default);
}