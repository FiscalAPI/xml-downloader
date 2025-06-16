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

using XmlDownloader.Common;
using XmlDownloader.Common.Enums;
using XmlDownloader.Common.Http;
using XmlDownloader.Download.Models;
using XmlDownloader.Download.Models.Sat;

namespace XmlDownloader.Download;

public static class DownloadResponseService
{
    public static DownloadResponse Build(SatResponse satResponse)
    {
        if (satResponse.IsSuccessStatusCode)
        {
            /*
             *<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
                     <s:Header>
                           <h:respuesta CodEstatus="5000" Mensaje="Solicitud Aceptada"
                                 xmlns:h="http://DescargaMasivaTerceros.sat.gob.mx"
                                 xmlns="http://DescargaMasivaTerceros.sat.gob.mx"
                                 xmlns:xsd="http://www.w3.org/2001/XMLSchema"
                                 xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"/>
                     </s:Header>
                     <s:Body xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
                           xmlns:xsd="http://www.w3.org/2001/XMLSchema">
                           <RespuestaDescargaMasivaTercerosSalida xmlns="http://DescargaMasivaTerceros.sat.gob.mx">
                                 <Paquete>base64...</Paquete>
                           </RespuestaDescargaMasivaTercerosSalida>
                     </s:Body>
               </s:Envelope>
             */
            var envelope = XmlSerializerService.Deserialize<DownloadEnvelope>(satResponse.RawResponse!);


            // CodEstatus
            var codEstatus = envelope?.Header?.HeaderResponse?.CodEstatus ?? "000";
            var status = codEstatus.ToEnumElement<SatStatus>();

            // Mensaje
            var mensaje = envelope?.Header?.HeaderResponse?.Mensaje ?? "";


            return new DownloadResponse
            {
                Succeeded = true,
                SatStatus = status,
                SatStatusCode = codEstatus,
                SatMessage = mensaje,
                Base64Package = envelope?.Body.BodyData?.Base64Package,
                RawRequest = satResponse.RawRequest,
                RawResponse = satResponse.RawResponse
            };
        }


        return new DownloadResponse
        {
            Succeeded = false,
            SatStatus = SatStatus.Unknown,
            SatStatusCode = "UnknownError",
            SatMessage = $"StatusCode: {satResponse.HttpStatusCode} Message: {satResponse.ReasonPhrase}",
            RawRequest = satResponse.RawRequest,
            RawResponse = satResponse.RawResponse
        };
    }
}