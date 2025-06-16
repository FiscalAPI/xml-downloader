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

namespace XmlDownloader.Auth.Models.Sat;

// Envelope for Success Response
[XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
public class AuthEnvelope
{
    [XmlElement(ElementName = "Header")] public AuthHeader? Header { get; set; }

    [XmlElement(ElementName = "Body")] public AuthBody? Body { get; set; }
}