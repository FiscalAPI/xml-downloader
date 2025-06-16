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
public class PagosPago
{
    private PagosPagoDoctoRelacionado[] doctoRelacionadoField;

    private PagosPagoImpuestosP impuestosPField;

    private System.DateTime fechaPagoField;

    private c_FormaPago formaDePagoPField;

    private c_Moneda monedaPField;

    private decimal tipoCambioPField;

    private bool tipoCambioPFieldSpecified;

    private decimal montoField;

    private string numOperacionField;

    private string rfcEmisorCtaOrdField;

    private string nomBancoOrdExtField;

    private string ctaOrdenanteField;

    private string rfcEmisorCtaBenField;

    private string ctaBeneficiarioField;

    private c_TipoCadenaPago tipoCadPagoField;

    private bool tipoCadPagoFieldSpecified;

    private byte[] certPagoField;

    private string cadPagoField;

    private byte[] selloPagoField;

    /// <remarks/>
    [XmlElement("DoctoRelacionado")]
    public PagosPagoDoctoRelacionado[] DoctoRelacionado
    {
        get { return this.doctoRelacionadoField; }
        set { this.doctoRelacionadoField = value; }
    }

    /// <remarks/>
    public PagosPagoImpuestosP ImpuestosP
    {
        get { return this.impuestosPField; }
        set { this.impuestosPField = value; }
    }

    /// <remarks/>
    [XmlAttribute()]
    public System.DateTime FechaPago
    {
        get { return this.fechaPagoField; }
        set { this.fechaPagoField = value; }
    }

    /// <remarks/>
    [XmlAttribute()]
    public c_FormaPago FormaDePagoP
    {
        get { return this.formaDePagoPField; }
        set { this.formaDePagoPField = value; }
    }

    /// <remarks/>
    [XmlAttribute()]
    public c_Moneda MonedaP
    {
        get { return this.monedaPField; }
        set { this.monedaPField = value; }
    }

    /// <remarks/>
    [XmlAttribute()]
    public decimal TipoCambioP
    {
        get { return this.tipoCambioPField; }
        set { this.tipoCambioPField = value; }
    }

    /// <remarks/>
    [XmlIgnore()]
    public bool TipoCambioPSpecified
    {
        get { return this.tipoCambioPFieldSpecified; }
        set { this.tipoCambioPFieldSpecified = value; }
    }

    /// <remarks/>
    [XmlAttribute()]
    public decimal Monto
    {
        get { return this.montoField; }
        set { this.montoField = value; }
    }

    /// <remarks/>
    [XmlAttribute()]
    public string NumOperacion
    {
        get { return this.numOperacionField; }
        set { this.numOperacionField = value; }
    }

    /// <remarks/>
    [XmlAttribute()]
    public string RfcEmisorCtaOrd
    {
        get { return this.rfcEmisorCtaOrdField; }
        set { this.rfcEmisorCtaOrdField = value; }
    }

    /// <remarks/>
    [XmlAttribute()]
    public string NomBancoOrdExt
    {
        get { return this.nomBancoOrdExtField; }
        set { this.nomBancoOrdExtField = value; }
    }

    /// <remarks/>
    [XmlAttribute()]
    public string CtaOrdenante
    {
        get { return this.ctaOrdenanteField; }
        set { this.ctaOrdenanteField = value; }
    }

    /// <remarks/>
    [XmlAttribute()]
    public string RfcEmisorCtaBen
    {
        get { return this.rfcEmisorCtaBenField; }
        set { this.rfcEmisorCtaBenField = value; }
    }

    /// <remarks/>
    [XmlAttribute()]
    public string CtaBeneficiario
    {
        get { return this.ctaBeneficiarioField; }
        set { this.ctaBeneficiarioField = value; }
    }

    /// <remarks/>
    [XmlAttribute()]
    public c_TipoCadenaPago TipoCadPago
    {
        get { return this.tipoCadPagoField; }
        set { this.tipoCadPagoField = value; }
    }

    /// <remarks/>
    [XmlIgnore()]
    public bool TipoCadPagoSpecified
    {
        get { return this.tipoCadPagoFieldSpecified; }
        set { this.tipoCadPagoFieldSpecified = value; }
    }

    /// <remarks/>
    [XmlAttribute(DataType = "base64Binary")]
    public byte[] CertPago
    {
        get { return this.certPagoField; }
        set { this.certPagoField = value; }
    }

    /// <remarks/>
    [XmlAttribute()]
    public string CadPago
    {
        get { return this.cadPagoField; }
        set { this.cadPagoField = value; }
    }

    /// <remarks/>
    [XmlAttribute(DataType = "base64Binary")]
    public byte[] SelloPago
    {
        get { return this.selloPagoField; }
        set { this.selloPagoField = value; }
    }
}