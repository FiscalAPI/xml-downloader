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
[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sat.gob.mx/sitio_internet/cfd/catalogos")]
public enum c_FormaPago
{
    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("01")]
    Item01,

    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("02")]
    Item02,

    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("03")]
    Item03,

    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("04")]
    Item04,

    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("05")]
    Item05,

    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("06")]
    Item06,

    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("08")]
    Item08,

    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("12")]
    Item12,

    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("13")]
    Item13,

    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("14")]
    Item14,

    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("15")]
    Item15,

    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("17")]
    Item17,

    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("23")]
    Item23,

    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("24")]
    Item24,

    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("25")]
    Item25,

    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("26")]
    Item26,

    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("27")]
    Item27,

    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("28")]
    Item28,

    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("29")]
    Item29,

    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("30")]
    Item30,

    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("31")]
    Item31,

    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("99")]
    Item99,
}