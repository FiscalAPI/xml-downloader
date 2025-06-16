
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
public class ComprobanteConceptoParte {
    
    private ComprobanteConceptoParteInformacionAduanera[] informacionAduaneraField;
    
    private string claveProdServField;
    
    private string noIdentificacionField;
    
    private decimal cantidadField;
    
    private string unidadField;
    
    private string descripcionField;
    
    private decimal valorUnitarioField;
    
    private bool valorUnitarioFieldSpecified;
    
    private decimal importeField;
    
    private bool importeFieldSpecified;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("InformacionAduanera")]
    public ComprobanteConceptoParteInformacionAduanera[] InformacionAduanera {
        get {
            return informacionAduaneraField;
        }
        set {
            informacionAduaneraField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string ClaveProdServ {
        get {
            return claveProdServField;
        }
        set {
            claveProdServField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string NoIdentificacion {
        get {
            return noIdentificacionField;
        }
        set {
            noIdentificacionField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public decimal Cantidad {
        get {
            return cantidadField;
        }
        set {
            cantidadField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Unidad {
        get {
            return unidadField;
        }
        set {
            unidadField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Descripcion {
        get {
            return descripcionField;
        }
        set {
            descripcionField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public decimal ValorUnitario {
        get {
            return valorUnitarioField;
        }
        set {
            valorUnitarioField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool ValorUnitarioSpecified {
        get {
            return valorUnitarioFieldSpecified;
        }
        set {
            valorUnitarioFieldSpecified = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public decimal Importe {
        get {
            return importeField;
        }
        set {
            importeField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool ImporteSpecified {
        get {
            return importeFieldSpecified;
        }
        set {
            importeFieldSpecified = value;
        }
    }
}