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

using System.Xml.Serialization;

namespace Fiscalapi.XmlDownloader.Common;

/// <summary>
/// Service for serializing and deserializing XML data.
/// </summary>
public static class XmlSerializerService
{
    public static T? Deserialize<T>(string xml) where T : class
    {
        var serializer = new XmlSerializer(typeof(T));
        using var reader = new StringReader(xml);
        return serializer.Deserialize(reader) as T;
    }

    public static string Serialize<T>(T obj)
    {
        var serializer = new XmlSerializer(typeof(T));
        using var stringWriter = new StringWriter();
        serializer.Serialize(stringWriter, obj);
        return stringWriter.ToString();
    }
}