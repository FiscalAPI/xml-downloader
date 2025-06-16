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

using XmlDownloader.Common.Enums;

namespace XmlDownloader.Common.Http;

/// <summary>
/// Base response class containing common properties for all SAT web service responses
/// </summary>
public abstract class BaseResponse
{
    /// <summary>
    /// Indicates whether the response was successful.
    /// </summary>
    public bool Succeeded { get; set; }

    /// <summary>
    /// Raw request and response XML strings for debugging purposes.
    /// </summary>
    public string? RawRequest { get; set; }

    /// <summary>
    /// Raw response XML string as received from the SAT service.
    /// </summary>
    public string? RawResponse { get; set; }


    /// <summary>
    /// Operation status code from the SAT service
    /// </summary>
    public SatStatus SatStatus { get; set; }

    /// <summary>
    /// Sat 'CodEstatus' received from the service
    /// </summary>
    public string? SatStatusCode { get; set; }

    /// <summary>
    /// Sar 'Mensaje' from the SAT service
    /// </summary>
    public string? SatMessage { get; set; }
}