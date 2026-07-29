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
public class ComprobanteReceptor {
    
    private string rfcField;
    
    private string nombreField;
    
    private string domicilioFiscalReceptorField;
    
    private c_Pais residenciaFiscalField;
    
    private bool residenciaFiscalFieldSpecified;
    
    private string numRegIdTribField;
    
    private c_RegimenFiscal regimenFiscalReceptorField;

    private bool regimenFiscalReceptorFieldSpecified;

    private c_UsoCFDI usoCFDIField;

    private bool usoCFDIFieldSpecified;
    
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
    public string DomicilioFiscalReceptor {
        get {
            return domicilioFiscalReceptorField;
        }
        set {
            domicilioFiscalReceptorField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public c_Pais ResidenciaFiscal {
        get {
            return residenciaFiscalField;
        }
        set {
            residenciaFiscalField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool ResidenciaFiscalSpecified {
        get {
            return residenciaFiscalFieldSpecified;
        }
        set {
            residenciaFiscalFieldSpecified = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string NumRegIdTrib {
        get {
            return numRegIdTribField;
        }
        set {
            numRegIdTribField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public c_RegimenFiscal RegimenFiscalReceptor {
        get {
            return regimenFiscalReceptorField;
        }
        set {
            regimenFiscalReceptorField = value;
        }
    }

    /// <summary>
    /// Indica si el atributo RegimenFiscalReceptor está presente. Al deserializar un CFDI 4.0
    /// el XmlSerializer lo establece en true automáticamente; en CFDIs 3.3 mapeados queda en
    /// false (el atributo no existe en 3.3), lo que evita interpretar el default del enum
    /// como un régimen real.
    /// </summary>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool RegimenFiscalReceptorSpecified {
        get {
            return regimenFiscalReceptorFieldSpecified;
        }
        set {
            regimenFiscalReceptorFieldSpecified = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public c_UsoCFDI UsoCFDI {
        get {
            return usoCFDIField;
        }
        set {
            usoCFDIField = value;
        }
    }

    /// <summary>
    /// Indica si el atributo UsoCFDI está presente. Al deserializar un CFDI 4.0 el
    /// XmlSerializer lo establece en true automáticamente; en CFDIs 3.3 mapeados puede quedar
    /// en false cuando el código no existe en el catálogo 4.0.
    /// </summary>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool UsoCFDISpecified {
        get {
            return usoCFDIFieldSpecified;
        }
        set {
            usoCFDIFieldSpecified = value;
        }
    }
}