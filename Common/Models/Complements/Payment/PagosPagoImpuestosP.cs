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
public class PagosPagoImpuestosP
{
    private PagosPagoImpuestosPRetencionP[] retencionesPField;

    private PagosPagoImpuestosPTrasladoP[] trasladosPField;

    /// <remarks/>
    [XmlArrayItem("RetencionP", IsNullable = false)]
    public PagosPagoImpuestosPRetencionP[] RetencionesP
    {
        get { return this.retencionesPField; }
        set { this.retencionesPField = value; }
    }

    /// <remarks/>
    [XmlArrayItem("TrasladoP", IsNullable = false)]
    public PagosPagoImpuestosPTrasladoP[] TrasladosP
    {
        get { return this.trasladosPField; }
        set { this.trasladosPField = value; }
    }
}