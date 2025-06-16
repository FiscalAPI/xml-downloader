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

namespace XmlDownloader.Common.Models.Complements.Payment;

/// <remarks/>
[Serializable()]
[XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/Pagos20")]
public class PagosPagoImpuestosPRetencionP
{
    private c_Impuesto impuestoPField;

    private decimal importePField;

    /// <remarks/>
    [XmlAttribute()]
    public c_Impuesto ImpuestoP
    {
        get { return this.impuestoPField; }
        set { this.impuestoPField = value; }
    }

    /// <remarks/>
    [XmlAttribute()]
    public decimal ImporteP
    {
        get { return this.importePField; }
        set { this.importePField = value; }
    }
}