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

namespace Fiscalapi.XmlDownloader.Auth.Models;

public class Token
{
    /// <summary>
    /// The JWT token string.
    /// </summary>
    public string? Value { get; set; }

    /// <summary>
    /// The timestamp indicating when the token becomes valid.
    /// </summary>
    public DateTime? ValidFrom { get; set; }

    /// <summary>
    /// The timestamp indicating when the token expires.
    /// </summary>
    public DateTime? ValidTo { get; set; }

    /// <summary>
    /// RFC associated with the token. The tax identification number (TIN) 
    /// </summary>
    public string? Tin { get; set; }

    /// <summary>
    /// Checks if the token is currently valid based on the current time.
    /// </summary>
    public bool IsValid =>
        ValidFrom.HasValue &&
        ValidTo.HasValue &&
        !string.IsNullOrWhiteSpace(Value) &&
        !string.IsNullOrWhiteSpace(Tin) &&
        DateTime.UtcNow >= ValidFrom.Value &&
        DateTime.UtcNow <= ValidTo.Value;
}