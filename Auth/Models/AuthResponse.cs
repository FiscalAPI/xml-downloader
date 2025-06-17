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

namespace Fiscalapi.XmlDownloader.Auth.Models;

/// <summary>
/// Response from the authentication service
/// </summary>
public class AuthResponse : BaseResponse
{
    /// <summary>
    /// The JWT token returned in a successful response.
    /// </summary>
    public string? TokenValue { get; set; }

    /// <summary>
    /// The timestamp indicating when the token becomes valid.
    /// </summary>
    public DateTime? ValidFrom { get; set; }

    /// <summary>
    /// The timestamp indicating when the token expires.
    /// </summary>
    public DateTime? ValidTo { get; set; }

    /// <summary>
    /// RFC associated with the Auth response. The tax identification number (TIN) 
    /// </summary>
    public string? Tin { get; set; }

    /// <summary>
    /// Get token details as a structured object
    /// </summary>
    public Token? Token
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(TokenValue))
            {
                return new Token
                {
                    Value = TokenValue,
                    ValidFrom = ValidFrom,
                    ValidTo = ValidTo,
                    Tin = Tin
                };
            }

            return null;
        }
    }
}