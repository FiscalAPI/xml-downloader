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
using Fiscalapi.XmlDownloader.Verify.Models;
using Fiscalapi.XmlDownloader.Verify.Models.Sat;
using Microsoft.Extensions.Logging;

namespace Fiscalapi.XmlDownloader.Verify;

public static class VerifyResponseService
{
    /// <summary>
    /// Builds a VerifyResponse from the SAT response.
    /// </summary>
    public static VerifyResponse Build(SatResponse satResponse, ILogger logger)
    {
        if (satResponse.IsSuccessStatusCode)
        {
            var envelope = XmlSerializerService.Deserialize<VerifyEnvelope>(satResponse.RawResponse!);

            /*
         *<s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
               <s:Body xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
                   xmlns:xsd="http://www.w3.org/2001/XMLSchema">
                   <VerificaSolicitudDescargaResponse xmlns="http://DescargaMasivaTerceros.sat.gob.mx">
                       <VerificaSolicitudDescargaResult
                           CodEstatus="5000"
                           EstadoSolicitud="3"
                           CodigoEstadoSolicitud="5000"
                           NumeroCFDIs="0"
                           Mensaje="Solicitud Aceptada">
                           <IdsPaquetes>4e80345d-917f-40bb-a98f4a73939343c5_01</IdsPaquetes>
                           <IdsPaquetes>4e80345d-917f-40bb-a98f4a73939343c5_02</IdsPaquetes>
                           <IdsPaquetes>4e80345d-917f-40bb-a98f4a73939343c5_03</IdsPaquetes>
                           <IdsPaquetes>4e80345d-917f-40bb-a98f4a73939343c5_04</IdsPaquetes>
                           <IdsPaquetes>4e80345d-917f-40bb-a98f4a73939343c5_05</IdsPaquetes>
                           <IdsPaquetes>4e80345d-917f-40bb-a98f4a73939343c5_06</IdsPaquetes>
                       </VerificaSolicitudDescargaResult>
                   </VerificaSolicitudDescargaResponse>
               </s:Body>
           </s:Envelope>
         */

            //CodEstatus="5000"
            var codEstatus = envelope?.Body.VerifyDownloadRequestResponse
                .VerifyDownloadRequestResult.CodEstatus;

            var status = !string.IsNullOrWhiteSpace(codEstatus)
                ? codEstatus.ToEnumElement<SatStatus>()
                : SatStatus.Unknown;

            // EstadoSolicitud="3"
            var estadoSolicitud = envelope?.Body.VerifyDownloadRequestResponse
                .VerifyDownloadRequestResult.EstadoSolicitud;


            var requestStatus = !string.IsNullOrWhiteSpace(estadoSolicitud)
                ? estadoSolicitud.ToEnumElement<RequestStatus>()
                : RequestStatus.Desconocido;

            //  NumeroCFDIs="0"
            var numeroCfdis = envelope?.Body?.VerifyDownloadRequestResponse
                .VerifyDownloadRequestResult?.NumeroCFDIs ?? "0";

            numeroCfdis = numeroCfdis.Trim().Equals("") ? "0" : numeroCfdis;

            var invoiceCount = int.Parse(numeroCfdis);

            //Mensaje
            var mensaje = envelope?.Body?.VerifyDownloadRequestResponse?
                .VerifyDownloadRequestResult?.Mensaje ?? "";

            // IdsPaquetes
            var idsPaquetes = envelope?.Body?.VerifyDownloadRequestResponse?
                .VerifyDownloadRequestResult?.PackageIds ?? [];


            return new VerifyResponse
            {
                Succeeded = true,
                SatStatus = status,
                SatStatusCode = status.ToEnumCode(),
                SatMessage = mensaje,
                RequestStatus = requestStatus,
                InvoiceCount = invoiceCount,
                PackageIds = idsPaquetes,
                RawRequest = satResponse.RawRequest,
                RawResponse = satResponse.RawResponse
            };
        }

        return new VerifyResponse
        {
            Succeeded = false,
            SatStatusCode = "UnknownError",
            SatMessage = $"StatusCode: {satResponse.HttpStatusCode} Message: {satResponse.ReasonPhrase}",
            RawRequest = satResponse.RawRequest,
            RawResponse = satResponse.RawResponse,
        };
    }
}