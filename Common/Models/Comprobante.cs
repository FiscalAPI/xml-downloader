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


using System.Reflection;
using System.Xml.Serialization;
using XmlDownloader.Common.Attributes;

namespace XmlDownloader.Common.Models;

[Serializable()]
[XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/cfd/4")]
[XmlRoot(Namespace = "http://www.sat.gob.mx/cfd/4", IsNullable = false)]
public class Comprobante
{
    private ComprobanteInformacionGlobal informacionGlobalField;

    private ComprobanteCfdiRelacionados[] cfdiRelacionadosField;

    private ComprobanteEmisor emisorField;

    private ComprobanteReceptor receptorField;

    private ComprobanteConcepto[] conceptosField;

    private ComprobanteImpuestos impuestosField;

    private ComprobanteComplemento complementoField;

    private ComprobanteAddenda addendaField;

    private string versionField;

    private string serieField;

    private string folioField;

    private DateTime fechaField;

    private string selloField;

    private c_FormaPago formaPagoField;

    private bool formaPagoFieldSpecified;

    private string noCertificadoField;

    private string certificadoField;

    private string condicionesDePagoField;

    private decimal subTotalField;

    private decimal descuentoField;

    private bool descuentoFieldSpecified;

    private c_Moneda monedaField;

    private decimal tipoCambioField;

    private bool tipoCambioFieldSpecified;

    private decimal totalField;

    private c_TipoDeComprobante tipoDeComprobanteField;

    private c_Exportacion exportacionField;

    private c_MetodoPago metodoPagoField;

    private bool metodoPagoFieldSpecified;

    private string lugarExpedicionField;

    private string confirmacionField;

    public Comprobante()
    {
        versionField = "4.0";
    }

    /// <remarks/>
    public ComprobanteInformacionGlobal InformacionGlobal
    {
        get { return informacionGlobalField; }
        set { informacionGlobalField = value; }
    }

    /// <remarks/>
    [XmlElement("CfdiRelacionados")]
    public ComprobanteCfdiRelacionados[] CfdiRelacionados
    {
        get { return cfdiRelacionadosField; }
        set { cfdiRelacionadosField = value; }
    }

    /// <remarks/>
    public ComprobanteEmisor Emisor
    {
        get { return emisorField; }
        set { emisorField = value; }
    }

    /// <remarks/>
    public ComprobanteReceptor Receptor
    {
        get { return receptorField; }
        set { receptorField = value; }
    }

    /// <remarks/>
    [XmlArrayItem("Concepto", IsNullable = false)]
    public ComprobanteConcepto[] Conceptos
    {
        get { return conceptosField; }
        set { conceptosField = value; }
    }

    /// <remarks/>
    public ComprobanteImpuestos Impuestos
    {
        get { return impuestosField; }
        set { impuestosField = value; }
    }

    /// <remarks/>
    public ComprobanteComplemento Complemento
    {
        get { return complementoField; }
        set { complementoField = value; }
    }

    /// <remarks/>
    public ComprobanteAddenda Addenda
    {
        get { return addendaField; }
        set { addendaField = value; }
    }

    /// <remarks/>
    [XmlAttribute()]
    public string Version
    {
        get { return versionField; }
        set { versionField = value; }
    }

    /// <remarks/>
    [XmlAttribute()]
    public string Serie
    {
        get { return serieField; }
        set { serieField = value; }
    }

    /// <remarks/>
    [XmlAttribute()]
    public string Folio
    {
        get { return folioField; }
        set { folioField = value; }
    }

    /// <remarks/>
    [XmlAttribute()]
    public DateTime Fecha
    {
        get { return fechaField; }
        set { fechaField = value; }
    }

    /// <remarks/>
    [XmlAttribute()]
    public string Sello
    {
        get { return selloField; }
        set { selloField = value; }
    }

    /// <remarks/>
    [XmlAttribute()]
    public c_FormaPago FormaPago
    {
        get { return formaPagoField; }
        set { formaPagoField = value; }
    }

    /// <remarks/>
    [XmlIgnore()]
    public bool FormaPagoSpecified
    {
        get { return formaPagoFieldSpecified; }
        set { formaPagoFieldSpecified = value; }
    }

    /// <remarks/>
    [XmlAttribute()]
    public string NoCertificado
    {
        get { return noCertificadoField; }
        set { noCertificadoField = value; }
    }

    /// <remarks/>
    [XmlAttribute()]
    public string Certificado
    {
        get { return certificadoField; }
        set { certificadoField = value; }
    }

    /// <remarks/>
    [XmlAttribute()]
    public string CondicionesDePago
    {
        get { return condicionesDePagoField; }
        set { condicionesDePagoField = value; }
    }

    /// <remarks/>
    [XmlAttribute()]
    public decimal SubTotal
    {
        get { return subTotalField; }
        set { subTotalField = value; }
    }

    /// <remarks/>
    [XmlAttribute()]
    public decimal Descuento
    {
        get { return descuentoField; }
        set { descuentoField = value; }
    }

    /// <remarks/>
    [XmlIgnore()]
    public bool DescuentoSpecified
    {
        get { return descuentoFieldSpecified; }
        set { descuentoFieldSpecified = value; }
    }

    /// <remarks/>
    [XmlAttribute()]
    public c_Moneda Moneda
    {
        get { return monedaField; }
        set { monedaField = value; }
    }

    /// <remarks/>
    [XmlAttribute()]
    public decimal TipoCambio
    {
        get { return tipoCambioField; }
        set { tipoCambioField = value; }
    }

    /// <remarks/>
    [XmlIgnore()]
    public bool TipoCambioSpecified
    {
        get { return tipoCambioFieldSpecified; }
        set { tipoCambioFieldSpecified = value; }
    }

    /// <remarks/>
    [XmlAttribute()]
    public decimal Total
    {
        get { return totalField; }
        set { totalField = value; }
    }

    /// <remarks/>
    [XmlAttribute()]
    public c_TipoDeComprobante TipoDeComprobante
    {
        get { return tipoDeComprobanteField; }
        set { tipoDeComprobanteField = value; }
    }

    /// <remarks/>
    [XmlAttribute()]
    public c_Exportacion Exportacion
    {
        get { return exportacionField; }
        set { exportacionField = value; }
    }

    /// <remarks/>
    [XmlAttribute()]
    public c_MetodoPago MetodoPago
    {
        get { return metodoPagoField; }
        set { metodoPagoField = value; }
    }

    /// <remarks/>
    [XmlIgnore()]
    public bool MetodoPagoSpecified
    {
        get { return metodoPagoFieldSpecified; }
        set { metodoPagoFieldSpecified = value; }
    }

    /// <remarks/>
    [XmlAttribute()]
    public string LugarExpedicion
    {
        get { return lugarExpedicionField; }
        set { lugarExpedicionField = value; }
    }

    /// <remarks/>
    [XmlAttribute()]
    public string Confirmacion
    {
        get { return confirmacionField; }
        set { confirmacionField = value; }
    }

    /// <summary>
    /// All 'CFDI Complementos' deserialized at runtime automatically.
    /// </summary>
    [XmlIgnore]
    public AllComplements AllComplements { get; set; } = new();




    public void DeserializeComplements()
    {
        if (Complemento?.Any == null) return;


        var properties = typeof(AllComplements).GetProperties()
            .Where(p => p.GetCustomAttribute<ComplementAttribute>() != null)
            .ToList();

        foreach (var element in Complemento.Any)
        {
            var targetProperty = properties.FirstOrDefault(p =>
                p.GetCustomAttribute<ComplementAttribute>()?.Prefix == element.Prefix);

            if (targetProperty == null) continue;

            var serializer = new XmlSerializer(targetProperty.PropertyType);
            using var reader = new StringReader(element.OuterXml);
            var value = serializer.Deserialize(reader);
            targetProperty.SetValue(AllComplements, value);
        }
    }
}