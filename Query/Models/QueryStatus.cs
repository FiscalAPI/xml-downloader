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

using Fiscalapi.XmlDownloader.Common.Attributes;

namespace Fiscalapi.XmlDownloader.Query.Models;

/// <summary>
/// Mensajes recibidos desde las operaciones "SolicitaDescargaEmitidos", "SolicitaDescargaRecibidos" y "SolicitaDescargaFolio" del Servicio de Solicitud Descarga Masiva
/// </summary>
public enum QueryStatus
{
    /// <summary>
    /// Usuario No Válido
    /// </summary>
    [EnumCode("300")] InvalidUser = 300,

    /// <summary>
    /// XML Mal Formado.
    /// Este código de error se regresa cuando el request posee información inválida, ejemplo: un RFC de receptor no válido.
    /// </summary>
    [EnumCode("301")] InvalidXml = 301,

    /// <summary>
    /// Sello Mal Formado
    /// </summary>
    [EnumCode("302")] InvalidSignatureFormat = 302,

    /// <summary>
    /// Sello no corresponde con RfcEmisor (SolicitaDescargaEmitidos, SolicitaDescargaRecibidos, SolicitaDescargaFolio)
    /// </summary>
    [EnumCode("303")] SignatureAndRfcMismatch = 303,


    /// <summary>
    /// Certificado Revocado o Caduco.
    /// El certificado puede ser inválido por múltiples razones como son el tipo, la vigencia, etc.
    /// </summary>
    [EnumCode("304")] ExpiredCert = 304,

    /// <summary>
    /// Certificado Inválido.
    /// El certificado puede ser inválido por múltiples razones como son el tipo, la vigencia, etc.
    /// </summary>
    [EnumCode("305")] InvalidCert = 305,

    /// <summary>
    /// Solicitud de descarga recibida con éxito
    /// </summary>
    [EnumCode("5000")] Success = 5000,

    /// <summary>
    /// Tercero no autorizado.
    /// Se da cuando se trata de descargar comprobantes que no son propios.
    /// </summary>
    [EnumCode("5001")] UnauthorizedThirdParty = 5001,

    /// <summary>
    /// Se han agotado las solicitudes de por vida.
    /// Se ha alcanzado el límite de solicitudes, con el mismo criterio.
    /// </summary>
    [EnumCode("5002")] RequestConfigurationLimitReached = 5002,

    /// <summary>
    /// Ya se tiene una solicitud registrada.
    /// Ya existe una solicitud activa con los mismos criterios.
    /// </summary>
    [EnumCode("5005")] DuplicateRequest = 5005,

    /// <summary>
    /// No se permite la descarga de xml que se encuentren cancelados.
    /// Específico para la operación SolicitaDescargaFolio.
    /// </summary>
    [EnumCode("5012")] NotAllowed = 5012,

    /// <summary>
    /// Error no controlado
    /// </summary>
    [EnumCode("404")] UncontrolledError = 404
}