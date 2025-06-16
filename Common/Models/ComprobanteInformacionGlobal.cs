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
public class ComprobanteInformacionGlobal {
    
    private c_Periodicidad periodicidadField;
    
    private c_Meses mesesField;
    
    private short añoField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public c_Periodicidad Periodicidad {
        get {
            return periodicidadField;
        }
        set {
            periodicidadField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public c_Meses Meses {
        get {
            return mesesField;
        }
        set {
            mesesField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public short Año {
        get {
            return añoField;
        }
        set {
            añoField = value;
        }
    }
}