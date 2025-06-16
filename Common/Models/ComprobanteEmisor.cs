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


namespace XmlDownloader.Common.Models;

/// <remarks/>
[Serializable()]


[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.sat.gob.mx/cfd/4")]
public class ComprobanteEmisor {
    
    private string rfcField;
    
    private string nombreField;
    
    private c_RegimenFiscal regimenFiscalField;
    
    private string facAtrAdquirenteField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Rfc {
        get {
            return rfcField;
        }
        set {
            rfcField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Nombre {
        get {
            return nombreField;
        }
        set {
            nombreField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public c_RegimenFiscal RegimenFiscal {
        get {
            return regimenFiscalField;
        }
        set {
            regimenFiscalField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string FacAtrAdquirente {
        get {
            return facAtrAdquirenteField;
        }
        set {
            facAtrAdquirenteField = value;
        }
    }
}