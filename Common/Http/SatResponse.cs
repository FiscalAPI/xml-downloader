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

using System.Net;

namespace XmlDownloader.Common.Http;

/// <summary>
/// Raw SAT Response
/// </summary>
public class SatResponse
{
    public bool IsSuccessStatusCode { get; set; }
    public HttpStatusCode HttpStatusCode { get; set; }
    public string? ReasonPhrase { get; set; }
    public string? RawRequest { get; set; }
    public string? RawResponse { get; set; }
}