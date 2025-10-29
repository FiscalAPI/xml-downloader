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

using Fiscalapi.Credentials.Core;
using Fiscalapi.XmlDownloader.Auth.Models;
using Fiscalapi.XmlDownloader.Auth.Models.Sat;
using Fiscalapi.XmlDownloader.Common;
using Fiscalapi.XmlDownloader.Common.Enums;
using Fiscalapi.XmlDownloader.Common.Http;
using Microsoft.Extensions.Logging;

namespace Fiscalapi.XmlDownloader.Auth;

public static class AuthResponseService
{
    public static AuthResponse Build(SatResponse satResponse, ICredential credential, ILogger logger)
    {
        /*
         *<s:envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/" xmlns:u="http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd">
               <s:header>
                   <o:security s:mustunderstand="1" xmlns:o="http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd">
                       <u:timestamp u:id="_0">
                           <u:created>2023-07-14T15:42:58.786Z</u:created>
                           <u:expires>2023-07-14T15:47:58.786Z</u:expires>
                       </u:timestamp>
                   </o:security>
               </s:header>
               <s:body>
                   <autenticaresponse xmlns="http://DescargaMasivaTerceros.gob.mx">
                     <autenticaresult>token...</autenticaresult>
                   </autenticaresponse>
               </s:body>
           </s:envelope>

            <s:envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/">
               <s:body>
                   <s:fault>
                       <faultcode xmlns:a="http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd">a:InvalidSecurity</faultcode>
                       <faultstring xml:lang="en-US">An error occurred when verifying security for the message.</faultstring>
                   </s:fault>
               </s:body>
           </s:envelope>
         */
        if (satResponse.IsSuccessStatusCode)
        {
            var envelope = XmlSerializerService.Deserialize<AuthEnvelope>(satResponse.RawResponse!);
            var tokenValue = envelope?.Body?.AutenticaResponse?.AutenticaResult;
            
            var authResponse = new AuthResponse
            {
                Succeeded = true,
                SatStatus = SatStatus.RequestSucceeded,
                SatStatusCode = SatStatus.RequestSucceeded.ToEnumCode(),
                SatMessage = "",
                TokenValue = tokenValue,
                ValidFrom = envelope?.Header?.Security?.Timestamp?.Created,
                ValidTo = envelope?.Header?.Security?.Timestamp?.Expires,
                RawRequest = satResponse.RawRequest,
                RawResponse = satResponse.RawResponse,
                Tin = credential.Certificate.Rfc
            };

            // Critical logging: Verify token was received
            if (string.IsNullOrWhiteSpace(tokenValue))
            {
                logger.LogWarning(
                    "Authentication succeeded but token is missing. RFC: {Rfc}, HasEnvelope: {HasEnvelope}, HasBody: {HasBody}, HasAutenticaResponse: {HasAutenticaResponse}",
                    credential.Certificate.Rfc,
                    envelope != null,
                    envelope?.Body != null,
                    envelope?.Body?.AutenticaResponse != null);
            }
            else
            {
                logger.LogInformation(
                    "Authentication succeeded. RFC: {Rfc}, TokenLength: {TokenLength}, ValidFrom: {ValidFrom}, ValidTo: {ValidTo}",
                    credential.Certificate.Rfc,
                    tokenValue.Length,
                    authResponse.ValidFrom,
                    authResponse.ValidTo);
            }

            return authResponse;
        }

        if (satResponse.RawResponse is not null && satResponse.RawResponse.ToLowerInvariant().Contains("fault"))
        {
            var faultEnvelope = XmlSerializerService.Deserialize<AuthFaultEnvelope>(satResponse.RawResponse);
            logger.LogError(
                "Authentication failed with SOAP fault. RFC: {Rfc}, FaultCode: {FaultCode}, FaultMessage: {FaultMessage}",
                credential.Certificate.Rfc,
                faultEnvelope?.Body?.Fault?.FaultCode,
                faultEnvelope?.Body?.Fault?.FaultMessage);
            
            return new AuthResponse
            {
                Succeeded = false,
                SatStatus = SatStatus.Unknown,
                SatStatusCode = faultEnvelope?.Body?.Fault?.FaultCode,
                SatMessage = faultEnvelope?.Body?.Fault?.FaultMessage,
                RawRequest = satResponse.RawRequest,
                RawResponse = satResponse.RawResponse,
            };
        }

        logger.LogError(
            "Authentication failed with unexpected status. RFC: {Rfc}, HttpStatusCode: {StatusCode}, ReasonPhrase: {ReasonPhrase}",
            credential.Certificate.Rfc,
            satResponse.HttpStatusCode,
            satResponse.ReasonPhrase);

        return new AuthResponse
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