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

namespace Fiscalapi.XmlDownloader.Common.Enums;

/// <summary>
/// Mensajes recibidos desde todas las operaciones del Servicio de Descarga Masiva
/// </summary>
public enum SatStatus
{
    /// <summary>
    /// Desconocido.
    /// </summary>
    [EnumCode("000")] Unknown = 000,

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
    [EnumCode("302")] InvalidSignature = 302,

    /// <summary>
    /// Sello no corresponde con el RFC del solicitante.
    /// El sello digital no corresponde con el RFC del usuario que realiza la operación.
    /// </summary>
    [EnumCode("303")] SignatureAndRfcMismatch = 303,

    /// <summary>
    /// Certificado Revocado o Caduco.
    /// El certificado puede ser inválido por múltiples razones como son el tipo, la vigencia, etc.
    /// </summary>
    [EnumCode("304")] CertificateExpiredOrRevoked = 304,

    /// <summary>
    /// Certificado Inválido.
    /// El certificado puede ser inválido por múltiples razones como son el tipo, la vigencia, etc.
    /// </summary>
    [EnumCode("305")] InvalidCertificate = 305,

    /// <summary>
    /// Error no controlado
    /// </summary>
    [EnumCode("404")] UncontrolledError = 404,

    /// <summary>
    /// Solicitud recibida con éxito.
    /// La solicitud fue recibida y procesada correctamente por el servicio.
    /// </summary>
    [EnumCode("5000")] RequestSucceeded = 5000,

    /// <summary>
    /// Tercero no autorizado.
    /// Se da cuando se trata de descargar comprobantes que no son propios.
    /// </summary>
    [EnumCode("5001")] Unauthorized = 5001,

    /// <summary>
    /// Se han agotado las solicitudes de por vida.
    /// Se ha alcanzado el límite de solicitudes con el mismo criterio.
    /// </summary>
    [EnumCode("5002")] CriteriaLimitExceeded = 5002,

    /// <summary>
    /// Tope máximo de elementos de la consulta.
    /// La solicitud sobrepasa el máximo de resultados por tipo de solicitud (Metadata y CFDI). 
    /// </summary>
    [EnumCode("5003")] ResultsLimitExceeded = 5003,

    /// <summary>
    /// No se encontró la información.
    /// No se encontró la información solicitada (solicitud de descarga o paquete según el contexto).
    /// </summary>
    [EnumCode("5004")] InformationNotFound = 5004,

    /// <summary>
    /// Ya se tiene una solicitud registrada.
    /// Ya existe una solicitud activa con los mismos criterios.
    /// </summary>
    [EnumCode("5005")] DuplicateRequest = 5005,

    /// <summary>
    /// No existe el paquete solicitado.
    /// Los paquetes solo tienen un periodo de vida de 72 horas.
    /// </summary>
    [EnumCode("5007")] PackageExpired = 5007,

    /// <summary>
    /// Máximo de descargas permitidas.
    /// Un paquete solo puede descargarse un total de 2 veces, una vez agotadas, ya no se podrá volver a descargar.
    /// </summary>
    [EnumCode("5008")] DownloadsExceeded = 5008,

    /// <summary>
    /// Límite de descargas por folio por día.
    /// Se ha alcanzado o sobrepasado el límite de descargas diarias por folio.
    /// </summary>
    [EnumCode("5011")] DailyDownloadExceeded = 5011,

    /// <summary>
    /// No se permite la descarga de xml que se encuentren cancelados
    /// </summary>
    [EnumCode("5012")] CancelledXmlNotAllowed = 5012
}