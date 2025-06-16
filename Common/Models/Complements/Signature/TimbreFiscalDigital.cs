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

namespace XmlDownloader.Common.Models.Complements.Signature;

[Serializable()]
[XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/TimbreFiscalDigital")]
[XmlRoot(Namespace = "http://www.sat.gob.mx/TimbreFiscalDigital", IsNullable = false)]
public class TimbreFiscalDigital
{
    private string versionField;

    private string uUIDField;

    private DateTime fechaTimbradoField;

    private string rfcProvCertifField;

    private string leyendaField;

    private string selloCFDField;

    private string noCertificadoSATField;

    private string selloSATField;

    public TimbreFiscalDigital()
    {
        versionField = "1.1";
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
    public string UUID
    {
        get { return uUIDField; }
        set { uUIDField = value; }
    }

    /// <remarks/>
    [XmlAttribute()]
    public DateTime FechaTimbrado
    {
        get { return fechaTimbradoField; }
        set { fechaTimbradoField = value; }
    }

    /// <remarks/>
    [XmlAttribute()]
    public string RfcProvCertif
    {
        get { return rfcProvCertifField; }
        set { rfcProvCertifField = value; }
    }

    /// <remarks/>
    [XmlAttribute()]
    public string Leyenda
    {
        get { return leyendaField; }
        set { leyendaField = value; }
    }

    /// <remarks/>
    [XmlAttribute()]
    public string SelloCFD
    {
        get { return selloCFDField; }
        set { selloCFDField = value; }
    }

    /// <remarks/>
    [XmlAttribute()]
    public string NoCertificadoSAT
    {
        get { return noCertificadoSATField; }
        set { noCertificadoSATField = value; }
    }

    /// <remarks/>
    [XmlAttribute()]
    public string SelloSAT
    {
        get { return selloSATField; }
        set { selloSATField = value; }
    }
}