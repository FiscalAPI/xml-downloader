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


using Fiscalapi.Credentials.Common;
using Fiscalapi.XmlDownloader.Common.Enums;
using Fiscalapi.XmlDownloader.Common.Models;
using Cfdi33Models = Fiscalapi.XmlDownloader.Common.Models.Cfdi33;

namespace Fiscalapi.XmlDownloader.Common;

/// <summary>
/// Parser central de CFDIs: detecta la versión por el namespace del elemento raíz,
/// deserializa con el modelo correcto (los 3.3 se mapean a la forma 4.0) y devuelve
/// un resultado por documento. Nunca lanza excepciones.
/// </summary>
public static class ComprobanteParser
{
    /// <summary>
    /// Parsea un CFDI (3.3 o 4.0) a <see cref="ComprobanteResult"/>. Cualquier fallo
    /// (namespace desconocido, XML malformado, error de deserialización) produce un
    /// resultado con <c>Succeeded = false</c>, el XML crudo y el error; nunca lanza.
    /// </summary>
    /// <param name="xml">Contenido XML del documento.</param>
    /// <param name="sourceName">Nombre de la entrada del ZIP o ruta del archivo de origen.</param>
    public static ComprobanteResult Parse(string xml, string sourceName)
    {
        var version = CfdiVersionDetector.Detect(xml);

        if (version == CfdiVersion.Unknown)
        {
            return new ComprobanteResult
            {
                SourceName = sourceName,
                Version = version,
                RawXml = xml,
                ErrorMessage =
                    "Namespace raíz no reconocido: no es CFDI 3.3 (cfd/3) ni 4.0 (cfd/4). " +
                    "¿Documento de Retenciones o XML malformado?"
            };
        }

        try
        {
            var warnings = new List<string>();
            Comprobante? comprobante;

            if (version == CfdiVersion.Cfdi40)
            {
                comprobante = XmlSerializerService.Deserialize<Comprobante>(xml);
            }
            else
            {
                var source = XmlSerializerService.Deserialize<Cfdi33Models.Comprobante>(xml);
                comprobante = source is null ? null : Cfdi33Models.Cfdi33Mapper.ToComprobante(source, warnings);
            }

            if (comprobante is null)
            {
                return new ComprobanteResult
                {
                    SourceName = sourceName,
                    Version = version,
                    RawXml = xml,
                    ErrorMessage = "La deserialización devolvió null."
                };
            }

            comprobante.Base64Content = xml.EncodeToBase64();
            warnings.AddRange(comprobante.TryDeserializeComplements());

            return new ComprobanteResult
            {
                SourceName = sourceName,
                Version = version,
                Comprobante = comprobante,
                RawXml = xml,
                Warnings = warnings
            };
        }
        catch (Exception ex)
        {
            return new ComprobanteResult
            {
                SourceName = sourceName,
                Version = version,
                RawXml = xml,
                Error = ex,
                ErrorMessage = ex.GetBaseException().Message
            };
        }
    }
}
