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
using Fiscalapi.XmlDownloader.Common.Models.Complements.Payment;
using Fiscalapi.XmlDownloader.Common.Models.Complements.Signature;

namespace Fiscalapi.XmlDownloader.Common.Models;

/// <summary>
/// Complementos del CFDI materializados como objetos tipados. Solo se tipan TFD y Pagos 2.0;
/// cualquier otro complemento (nómina, carta porte, comercio exterior, INE, Pagos 1.0 de los
/// CFDI 3.3 — namespace http://www.sat.gob.mx/Pagos —, impuestos locales, etc.) viaja crudo
/// como XmlElement en <c>Comprobante.Complemento.Any</c>: es responsabilidad del consumidor
/// persistirlo si lo necesita. El match se hace por namespace URI del elemento.
/// </summary>
public class AllComplements
{
    /// <summary>
    /// Complemento del comprobante que contiene el Timbre Fiscal Digital.
    /// </summary>
    [Complement("http://www.sat.gob.mx/TimbreFiscalDigital")]
    public TimbreFiscalDigital? TimbreFiscalDigital { get; set; }

    /// <summary>
    /// Complemento del comprobante con el complemento de pagos 2.0
    /// </summary>
    [Complement("http://www.sat.gob.mx/Pagos20")]
    public Pagos? Pago20 { get; set; }
}