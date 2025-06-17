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

using System.Xml.Serialization;

namespace Fiscalapi.XmlDownloader.Verify.Models.Sat;

/// <summary>
/// Main result containing download request verification data
/// </summary>
public class VerifyDownloadRequestResult
{
    /// <summary>
    /// Status code of the verification request
    /// </summary>
    [XmlAttribute("CodEstatus")]
    public string? CodEstatus { get; set; }

    /// <summary>
    /// State of the download request (1=Accepted, 2=InProcess, 3=Finished, 4=Error, 5=Rejected, 6=Expired)
    /// </summary>
    [XmlAttribute("EstadoSolicitud")]
    public string EstadoSolicitud { get; set; }

    /// <summary>
    /// Status code of the download request state
    /// </summary>
    [XmlAttribute("CodigoEstadoSolicitud")]
    public string? CodigoEstadoSolicitud { get; set; }

    /// <summary>
    /// Number of CFDIs that make up the download request
    /// </summary>
    [XmlAttribute("NumeroCFDIs")]
    public string? NumeroCFDIs { get; set; }

    /// <summary>
    /// Description message for the status code
    /// </summary>
    [XmlAttribute("Mensaje")]
    public string Mensaje { get; set; }

    /// <summary>
    /// List of package identifiers (only returned when request status is finished)
    /// </summary>
    [XmlElement("IdsPaquetes")]
    public List<string> PackageIds { get; set; } = [];
}