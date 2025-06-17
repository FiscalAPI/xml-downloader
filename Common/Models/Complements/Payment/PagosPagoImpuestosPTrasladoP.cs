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
public class PagosPagoImpuestosPTrasladoP
{
    private decimal basePField;

    private c_Impuesto impuestoPField;

    private c_TipoFactor tipoFactorPField;

    private decimal tasaOCuotaPField;

    private bool tasaOCuotaPFieldSpecified;

    private decimal importePField;

    private bool importePFieldSpecified;

    /// <remarks/>
    [XmlAttribute()]
    public decimal BaseP
    {
        get { return this.basePField; }
        set { this.basePField = value; }
    }

    /// <remarks/>
    [XmlAttribute()]
    public c_Impuesto ImpuestoP
    {
        get { return this.impuestoPField; }
        set { this.impuestoPField = value; }
    }

    /// <remarks/>
    [XmlAttribute()]
    public c_TipoFactor TipoFactorP
    {
        get { return this.tipoFactorPField; }
        set { this.tipoFactorPField = value; }
    }

    /// <remarks/>
    [XmlAttribute()]
    public decimal TasaOCuotaP
    {
        get { return this.tasaOCuotaPField; }
        set { this.tasaOCuotaPField = value; }
    }

    /// <remarks/>
    [XmlIgnore()]
    public bool TasaOCuotaPSpecified
    {
        get { return this.tasaOCuotaPFieldSpecified; }
        set { this.tasaOCuotaPFieldSpecified = value; }
    }

    /// <remarks/>
    [XmlAttribute()]
    public decimal ImporteP
    {
        get { return this.importePField; }
        set { this.importePField = value; }
    }

    /// <remarks/>
    [XmlIgnore()]
    public bool ImportePSpecified
    {
        get { return this.importePFieldSpecified; }
        set { this.importePFieldSpecified = value; }
    }
}