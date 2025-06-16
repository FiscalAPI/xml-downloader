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

namespace XmlDownloader.Auth;

/// <summary>
/// Interface for the SAT Authentication Service.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Authenticates with SAT using the provided credential and returns the authentication token.
    /// </summary>
    /// <param name="credential"></param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns></returns>
    Task<AuthResponse> AuthenticateAsync(ICredential credential, CancellationToken cancellationToken = default);
}