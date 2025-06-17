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

public class AllComplements
{
    /// <summary>
    /// Complemento del comprobante que contiene el Timbre Fiscal Digital.
    /// </summary>
    [Complement("tfd")]
    public TimbreFiscalDigital? TimbreFiscalDigital { get; set; }

    /// <summary>
    /// Complemento del comprobante con el complemento de pagos 2.0
    /// </summary>
    [Complement("pago20")]
    public Pagos Pago20 { get; set; }
}