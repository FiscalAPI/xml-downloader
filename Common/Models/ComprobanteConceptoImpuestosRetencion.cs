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
public class ComprobanteConceptoImpuestosRetencion {
    
    private decimal baseField;
    
    private c_Impuesto impuestoField;
    
    private c_TipoFactor tipoFactorField;
    
    private decimal tasaOCuotaField;
    
    private decimal importeField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public decimal Base {
        get {
            return baseField;
        }
        set {
            baseField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public c_Impuesto Impuesto {
        get {
            return impuestoField;
        }
        set {
            impuestoField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public c_TipoFactor TipoFactor {
        get {
            return tipoFactorField;
        }
        set {
            tipoFactorField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public decimal TasaOCuota {
        get {
            return tasaOCuotaField;
        }
        set {
            tasaOCuotaField = value;
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
}