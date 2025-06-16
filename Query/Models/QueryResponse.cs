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

using XmlDownloader.Common.Http;

namespace XmlDownloader.Query.Models;

/// <summary>
/// Response from download request operations (Emitidos, Recibidos, Folio)
/// </summary>
public class QueryResponse : BaseResponse
{
    /// <summary>
    /// Unique identifier for the download request
    /// </summary>
    public string? RequestId { get; set; }

    /// <summary>
    /// RFC Solicitante
    /// </summary>
    public string? RequesterRfc { get; set; }
}