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


using System.Xml;
using Fiscalapi.XmlDownloader.Common.Enums;

namespace Fiscalapi.XmlDownloader.Common;

/// <summary>
/// Detecta la versión de un CFDI leyendo únicamente el namespace del elemento raíz.
/// </summary>
public static class CfdiVersionDetector
{
    /// <summary>
    /// Namespace del CFDI 3.3.
    /// </summary>
    public const string Cfdi33Namespace = "http://www.sat.gob.mx/cfd/3";

    /// <summary>
    /// Namespace del CFDI 4.0.
    /// </summary>
    public const string Cfdi40Namespace = "http://www.sat.gob.mx/cfd/4";

    /// <summary>
    /// Detecta la versión del CFDI a partir del namespace del elemento raíz.
    /// Nunca lanza excepciones: XML nulo, vacío o malformado, así como documentos con
    /// namespace raíz distinto (p. ej. Retenciones), devuelven <see cref="CfdiVersion.Unknown"/>.
    /// </summary>
    /// <param name="xml">Contenido XML del documento.</param>
    /// <returns>La versión detectada o <see cref="CfdiVersion.Unknown"/>.</returns>
    public static CfdiVersion Detect(string? xml)
    {
        if (string.IsNullOrWhiteSpace(xml))
            return CfdiVersion.Unknown;

        try
        {
            var settings = new XmlReaderSettings { DtdProcessing = DtdProcessing.Prohibit };
            using var reader = XmlReader.Create(new StringReader(xml), settings);
            reader.MoveToContent();

            return reader.NamespaceURI switch
            {
                Cfdi33Namespace => CfdiVersion.Cfdi33,
                Cfdi40Namespace => CfdiVersion.Cfdi40,
                _ => CfdiVersion.Unknown
            };
        }
        catch (XmlException)
        {
            return CfdiVersion.Unknown;
        }
    }
}
