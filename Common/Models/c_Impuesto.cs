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
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.sat.gob.mx/sitio_internet/cfd/catalogos")]
public enum c_Impuesto {
    
    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("001")]
    Item001,
    
    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("002")]
    Item002,
    
    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("003")]
    Item003,
}