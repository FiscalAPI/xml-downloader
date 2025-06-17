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
public class PagosPagoDoctoRelacionado
{
    private PagosPagoDoctoRelacionadoImpuestosDR impuestosDRField;

    private string idDocumentoField;

    private string serieField;

    private string folioField;

    private c_Moneda monedaDRField;

    private decimal equivalenciaDRField;

    private bool equivalenciaDRFieldSpecified;

    private string numParcialidadField;

    private decimal impSaldoAntField;

    private decimal impPagadoField;

    private decimal impSaldoInsolutoField;

    private c_ObjetoImp objetoImpDRField;

    /// <remarks/>
    public PagosPagoDoctoRelacionadoImpuestosDR ImpuestosDR
    {
        get { return this.impuestosDRField; }
        set { this.impuestosDRField = value; }
    }

    /// <remarks/>
    [XmlAttribute()]
    public string IdDocumento
    {
        get { return this.idDocumentoField; }
        set { this.idDocumentoField = value; }
    }

    /// <remarks/>
    [XmlAttribute()]
    public string Serie
    {
        get { return this.serieField; }
        set { this.serieField = value; }
    }

    /// <remarks/>
    [XmlAttribute()]
    public string Folio
    {
        get { return this.folioField; }
        set { this.folioField = value; }
    }

    /// <remarks/>
    [XmlAttribute()]
    public c_Moneda MonedaDR
    {
        get { return this.monedaDRField; }
        set { this.monedaDRField = value; }
    }

    /// <remarks/>
    [XmlAttribute()]
    public decimal EquivalenciaDR
    {
        get { return this.equivalenciaDRField; }
        set { this.equivalenciaDRField = value; }
    }

    /// <remarks/>
    [XmlIgnore()]
    public bool EquivalenciaDRSpecified
    {
        get { return this.equivalenciaDRFieldSpecified; }
        set { this.equivalenciaDRFieldSpecified = value; }
    }

    /// <remarks/>
    [XmlAttribute(DataType = "integer")]
    public string NumParcialidad
    {
        get { return this.numParcialidadField; }
        set { this.numParcialidadField = value; }
    }

    /// <remarks/>
    [XmlAttribute()]
    public decimal ImpSaldoAnt
    {
        get { return this.impSaldoAntField; }
        set { this.impSaldoAntField = value; }
    }

    /// <remarks/>
    [XmlAttribute()]
    public decimal ImpPagado
    {
        get { return this.impPagadoField; }
        set { this.impPagadoField = value; }
    }

    /// <remarks/>
    [XmlAttribute()]
    public decimal ImpSaldoInsoluto
    {
        get { return this.impSaldoInsolutoField; }
        set { this.impSaldoInsolutoField = value; }
    }

    /// <remarks/>
    [XmlAttribute()]
    public c_ObjetoImp ObjetoImpDR
    {
        get { return this.objetoImpDRField; }
        set { this.objetoImpDRField = value; }
    }
}