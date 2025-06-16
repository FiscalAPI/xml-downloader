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

namespace XmlDownloader.Verify.Models;

/// <summary>
/// Response from request verification service
/// </summary>
public class VerifyResponse : BaseResponse
{
    /// <summary>
    /// Current status of the request
    /// </summary>
    public RequestStatus RequestStatus { get; set; }

    /// <summary>
    /// Number of CFDIs found for the request
    /// </summary>
    public int InvoiceCount { get; set; }

    /// <summary>
    /// List of package IDs available for download
    /// </summary>
    public List<string> PackageIds { get; set; } = new();

    /// <summary>
    /// Indicates if the request is ready for download
    /// </summary>
    public bool IsReadyToDownload => RequestStatus == RequestStatus.Terminada;
}