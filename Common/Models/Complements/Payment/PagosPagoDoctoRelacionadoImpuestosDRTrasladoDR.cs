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
public class PagosPagoDoctoRelacionadoImpuestosDRTrasladoDR
{
    private decimal baseDRField;

    private c_Impuesto impuestoDRField;

    private c_TipoFactor tipoFactorDRField;

    private decimal tasaOCuotaDRField;

    private bool tasaOCuotaDRFieldSpecified;

    private decimal importeDRField;

    private bool importeDRFieldSpecified;

    /// <remarks/>
    [XmlAttribute()]
    public decimal BaseDR
    {
        get { return this.baseDRField; }
        set { this.baseDRField = value; }
    }

    /// <remarks/>
    [XmlAttribute()]
    public c_Impuesto ImpuestoDR
    {
        get { return this.impuestoDRField; }
        set { this.impuestoDRField = value; }
    }

    /// <remarks/>
    [XmlAttribute()]
    public c_TipoFactor TipoFactorDR
    {
        get { return this.tipoFactorDRField; }
        set { this.tipoFactorDRField = value; }
    }

    /// <remarks/>
    [XmlAttribute()]
    public decimal TasaOCuotaDR
    {
        get { return this.tasaOCuotaDRField; }
        set { this.tasaOCuotaDRField = value; }
    }

    /// <remarks/>
    [XmlIgnore()]
    public bool TasaOCuotaDRSpecified
    {
        get { return this.tasaOCuotaDRFieldSpecified; }
        set { this.tasaOCuotaDRFieldSpecified = value; }
    }

    /// <remarks/>
    [XmlAttribute()]
    public decimal ImporteDR
    {
        get { return this.importeDRField; }
        set { this.importeDRField = value; }
    }

    /// <remarks/>
    [XmlIgnore()]
    public bool ImporteDRSpecified
    {
        get { return this.importeDRFieldSpecified; }
        set { this.importeDRFieldSpecified = value; }
    }
}