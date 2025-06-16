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
public class ComprobanteConceptoACuentaTerceros {
    
    private string rfcACuentaTercerosField;
    
    private string nombreACuentaTercerosField;
    
    private c_RegimenFiscal regimenFiscalACuentaTercerosField;
    
    private string domicilioFiscalACuentaTercerosField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string RfcACuentaTerceros {
        get {
            return rfcACuentaTercerosField;
        }
        set {
            rfcACuentaTercerosField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string NombreACuentaTerceros {
        get {
            return nombreACuentaTercerosField;
        }
        set {
            nombreACuentaTercerosField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public c_RegimenFiscal RegimenFiscalACuentaTerceros {
        get {
            return regimenFiscalACuentaTercerosField;
        }
        set {
            regimenFiscalACuentaTercerosField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string DomicilioFiscalACuentaTerceros {
        get {
            return domicilioFiscalACuentaTercerosField;
        }
        set {
            domicilioFiscalACuentaTercerosField = value;
        }
    }
}