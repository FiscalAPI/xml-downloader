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


namespace Fiscalapi.XmlDownloader.Common.Models;

/// <remarks/>
[Serializable()]


[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.sat.gob.mx/cfd/4")]
public class ComprobanteConcepto {
    
    private ComprobanteConceptoImpuestos impuestosField;
    
    private ComprobanteConceptoACuentaTerceros aCuentaTercerosField;
    
    private ComprobanteConceptoInformacionAduanera[] informacionAduaneraField;
    
    private ComprobanteConceptoCuentaPredial[] cuentaPredialField;
    
    private ComprobanteConceptoComplementoConcepto complementoConceptoField;
    
    private ComprobanteConceptoParte[] parteField;
    
    private string claveProdServField;
    
    private string noIdentificacionField;
    
    private decimal cantidadField;
    
    private string claveUnidadField;
    
    private string unidadField;
    
    private string descripcionField;
    
    private decimal valorUnitarioField;
    
    private decimal importeField;
    
    private decimal descuentoField;
    
    private bool descuentoFieldSpecified;
    
    private c_ObjetoImp objetoImpField;
    
    /// <remarks/>
    public ComprobanteConceptoImpuestos Impuestos {
        get {
            return impuestosField;
        }
        set {
            impuestosField = value;
        }
    }
    
    /// <remarks/>
    public ComprobanteConceptoACuentaTerceros ACuentaTerceros {
        get {
            return aCuentaTercerosField;
        }
        set {
            aCuentaTercerosField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("InformacionAduanera")]
    public ComprobanteConceptoInformacionAduanera[] InformacionAduanera {
        get {
            return informacionAduaneraField;
        }
        set {
            informacionAduaneraField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("CuentaPredial")]
    public ComprobanteConceptoCuentaPredial[] CuentaPredial {
        get {
            return cuentaPredialField;
        }
        set {
            cuentaPredialField = value;
        }
    }
    
    /// <remarks/>
    public ComprobanteConceptoComplementoConcepto ComplementoConcepto {
        get {
            return complementoConceptoField;
        }
        set {
            complementoConceptoField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("Parte")]
    public ComprobanteConceptoParte[] Parte {
        get {
            return parteField;
        }
        set {
            parteField = value;
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
    public string ClaveUnidad {
        get {
            return claveUnidadField;
        }
        set {
            claveUnidadField = value;
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
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public decimal Descuento {
        get {
            return descuentoField;
        }
        set {
            descuentoField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool DescuentoSpecified {
        get {
            return descuentoFieldSpecified;
        }
        set {
            descuentoFieldSpecified = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public c_ObjetoImp ObjetoImp {
        get {
            return objetoImpField;
        }
        set {
            objetoImpField = value;
        }
    }
}