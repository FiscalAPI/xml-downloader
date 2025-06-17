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

using Fiscalapi.XmlDownloader.Common;
using Fiscalapi.XmlDownloader.Common.Enums;
using Fiscalapi.XmlDownloader.Common.Http;
using Fiscalapi.XmlDownloader.Query.Models;
using Fiscalapi.XmlDownloader.Query.Models.Sat;

namespace Fiscalapi.XmlDownloader.Query;

public static class QueryResponseService
{
    public static QueryResponse Build(SatResponse satResponse)
    {
        if (satResponse.IsSuccessStatusCode)
        {
            /*
             *<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
                   <s:Body xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
                       xmlns:xsd="http://www.w3.org/2001/XMLSchema">
                       <SolicitaDescargaEmitidosResponse xmlns="http://DescargaMasivaTerceros.sat.gob.mx">
                           <SolicitaDescargaEmitidosResult
                                IdSolicitud="05e4038d-1f0d-4617-87d1-232fdd93bcc5"
                                RfcSolicitante="AXT940727FP8"
                                CodEstatus="5000"
                                Mensaje="Solicitud Aceptada"/>
                       </SolicitaDescargaEmitidosResponse>
                   </s:Body>
               </s:Envelope>

            <s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
                  <s:Body xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
                     xmlns:xsd="http://www.w3.org/2001/XMLSchema">
                     <SolicitaDescargaRecibidosResponse xmlns="http://DescargaMasivaTerceros.sat.gob.mx">
                        <SolicitaDescargaRecibidosResult
                                IdSolicitud="a8129420-4f22-42b4-9a7f-0c193d89d09a"
                                RfcSolicitante="AXT940727FP8"
                                CodEstatus="5000"
                                Mensaje="Solicitud Aceptada"/>
                     </SolicitaDescargaRecibidosResponse>
                  </s:Body>
               </s:Envelope>

            <s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
                   <s:Body xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
                       xmlns:xsd="http://www.w3.org/2001/XMLSchema">
                       <SolicitaDescargaFolioResponse xmlns="http://DescargaMasivaTerceros.sat.gob.mx">
                           <SolicitaDescargaFolioResult
                                    IdSolicitud="ad9f0b1e-d398-45a1-8fd0-0dd5a4f66945"
                                    RfcSolicitante="AXT940727FP8"
                                    CodEstatus="5000"
                                    Mensaje="Solicitud Aceptada"/>
                       </SolicitaDescargaFolioResponse>
                   </s:Body>
               </s:Envelope>
             */
            var envelope = XmlSerializerService.Deserialize<QueryEnvelope>(satResponse.RawResponse!);


            var result = envelope?.Body.SolicitaDescargaEmitidosResponse?.SolicitaDescargaEmitidosResult ??
                         envelope?.Body.SolicitaDescargaRecibidosResponse?.SolicitaDescargaRecibidosResult ??
                         envelope?.Body.SolicitaDescargaFolioResponse?.SolicitaDescargaFolioResult;

            // IdSolicitud
            var idSolicitud = result?.IdSolicitud ?? "";


            // RfcSolicitante
            var rfcSolicitante = result?.RfcSolicitante ?? "";


            // CodEstatus
            var status = !string.IsNullOrWhiteSpace(result?.CodEstatus)
                ? result.CodEstatus.ToEnumElement<SatStatus>()
                : SatStatus.Unknown;

            // Mensaje
            var mensaje = result?.Mensaje ?? "";


            return new QueryResponse
            {
                Succeeded = true,
                SatStatus = status,
                SatStatusCode = result?.CodEstatus,
                SatMessage = mensaje,
                RequestId = idSolicitud,
                RequesterRfc = rfcSolicitante,
                RawRequest = satResponse.RawRequest,
                RawResponse = satResponse.RawResponse,
            };
        }

        if (satResponse.RawResponse != null && satResponse.RawResponse.ToLower().Contains("fault"))
        {
            var envelope = XmlSerializerService.Deserialize<QueryEnvelope>(satResponse.RawResponse);
            return new QueryResponse
            {
                Succeeded = false,
                SatStatus = SatStatus.Unknown,
                SatStatusCode = envelope?.Body?.Fault?.FaultCode,
                SatMessage = envelope?.Body?.Fault?.FaultMessage,
                RawRequest = satResponse.RawRequest,
                RawResponse = satResponse.RawResponse,
            };
        }

        return new QueryResponse
        {
            Succeeded = false,
            SatStatus = SatStatus.Unknown,
            SatStatusCode = "UnknownError",
            SatMessage = $"StatusCode: {satResponse.HttpStatusCode} Message: {satResponse.ReasonPhrase}",
            RawRequest = satResponse.RawRequest,
            RawResponse = satResponse.RawResponse,
        };
    }
}