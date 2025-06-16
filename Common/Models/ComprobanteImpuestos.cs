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
public class ComprobanteImpuestos {
    
    private ComprobanteImpuestosRetencion[] retencionesField;
    
    private ComprobanteImpuestosTraslado[] trasladosField;
    
    private decimal totalImpuestosRetenidosField;
    
    private bool totalImpuestosRetenidosFieldSpecified;
    
    private decimal totalImpuestosTrasladadosField;
    
    private bool totalImpuestosTrasladadosFieldSpecified;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlArrayItemAttribute("Retencion", IsNullable=false)]
    public ComprobanteImpuestosRetencion[] Retenciones {
        get {
            return retencionesField;
        }
        set {
            retencionesField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlArrayItemAttribute("Traslado", IsNullable=false)]
    public ComprobanteImpuestosTraslado[] Traslados {
        get {
            return trasladosField;
        }
        set {
            trasladosField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public decimal TotalImpuestosRetenidos {
        get {
            return totalImpuestosRetenidosField;
        }
        set {
            totalImpuestosRetenidosField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool TotalImpuestosRetenidosSpecified {
        get {
            return totalImpuestosRetenidosFieldSpecified;
        }
        set {
            totalImpuestosRetenidosFieldSpecified = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public decimal TotalImpuestosTrasladados {
        get {
            return totalImpuestosTrasladadosField;
        }
        set {
            totalImpuestosTrasladadosField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool TotalImpuestosTrasladadosSpecified {
        get {
            return totalImpuestosTrasladadosFieldSpecified;
        }
        set {
            totalImpuestosTrasladadosFieldSpecified = value;
        }
    }
}