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

namespace Fiscalapi.XmlDownloader.Common.Models.Complements.Payment;

/// <remarks/>
[Serializable()]
[XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/Pagos20")]
public class PagosPagoDoctoRelacionadoImpuestosDR
{
    private PagosPagoDoctoRelacionadoImpuestosDRRetencionDR[] retencionesDRField;

    private PagosPagoDoctoRelacionadoImpuestosDRTrasladoDR[] trasladosDRField;

    /// <remarks/>
    [XmlArrayItem("RetencionDR", IsNullable = false)]
    public PagosPagoDoctoRelacionadoImpuestosDRRetencionDR[] RetencionesDR
    {
        get { return this.retencionesDRField; }
        set { this.retencionesDRField = value; }
    }

    /// <remarks/>
    [XmlArrayItem("TrasladoDR", IsNullable = false)]
    public PagosPagoDoctoRelacionadoImpuestosDRTrasladoDR[] TrasladosDR
    {
        get { return this.trasladosDRField; }
        set { this.trasladosDRField = value; }
    }
}