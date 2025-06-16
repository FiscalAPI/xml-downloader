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

using XmlDownloader.Common.Attributes;

namespace XmlDownloader.Verify.Models;

/// <summary>
/// Estado de la solicitud de descarga
/// </summary>
public enum RequestStatus
{
    /// <summary>
    /// Desconocido
    /// </summary>
    [EnumCode("0")] Desconocido = 0,


    /// <summary>
    /// Aceptada
    /// </summary>
    [EnumCode("1")] Aceptada = 1,

    /// <summary>
    /// En Proceso
    /// </summary>
    [EnumCode("2")] EnProceso = 2,

    /// <summary>
    /// Terminada
    /// </summary>
    [EnumCode("3")] Terminada = 3,

    /// <summary>
    /// Error
    /// </summary>
    [EnumCode("4")] Error = 4,

    /// <summary>
    /// Rechazada
    /// </summary>
    [EnumCode("5")] Rechazada = 5,

    /// <summary>
    /// Vencida (sucede 72 horas después de que se generó el paquete de descarga)
    /// </summary>
    [EnumCode("6")] Vencida = 6
}