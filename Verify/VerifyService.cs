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
using Fiscalapi.Credentials.Core;
using Fiscalapi.XmlDownloader.Auth.Models;
using Fiscalapi.XmlDownloader.Common;
using Fiscalapi.XmlDownloader.Common.Http;
using Fiscalapi.XmlDownloader.Verify.Models;
using Microsoft.Extensions.Logging;

namespace Fiscalapi.XmlDownloader.Verify;

/// <summary>
/// Service for verifying download requests with SAT.
/// </summary>
public class VerifyService : SatService, IVerifyService
{
    /// <summary>
    /// Constructor for dependency injection scenarios
    /// </summary>
    /// <param name="httpClient">HttpClient instance</param>
    /// <param name="logger">Logger instance</param>
    public VerifyService(HttpClient httpClient, ILogger<VerifyService> logger) 
        : base(httpClient, logger)
    {
    }

    /// <summary>
    /// Constructor for direct instantiation scenarios
    /// </summary>
    /// <param name="logger">Logger instance</param>
    public VerifyService(ILogger<VerifyService> logger) 
        : base(logger)
    {
    }

    /// <summary>
    /// Verifies a download request with SAT using the provided credential, authentication token, and request ID.
    /// </summary>
    /// <param name="credential">Fiel</param>
    /// <param name="authToken">Authentication token</param>
    /// <param name="requestId">Request ID</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <param name="logger">Logger</param>
    /// <returns>VerifyResponse</returns>
    public async Task<VerifyResponse> VerifyAsync(ICredential credential, Token authToken,
        string requestId, ILogger logger, CancellationToken cancellationToken = default)
    {
        var toDigest = CreateDigest(requestId, credential.Certificate.Rfc);
        var signature = CreateSignature(credential, toDigest);
        var requestXml = BuildEnvelope(requestId, credential.Certificate.Rfc, signature);

        var satResponse = await SendRequestAsync(
            url: SatUrl.VerifyUrl,
            action: SatUrl.VerifyAction,
            payload: requestXml,
            token: authToken.Value,
            cancellationToken: cancellationToken);

        var verifyResponse = VerifyResponseService.Build(satResponse, logger);
        return verifyResponse;
    }

    private static string CreateDigest(string requestId, string rfcSolicitante)
    {
        var xml = XmlTemplates.VerifyToDigest
            .Replace(XmlPlaceholders.RequestId, requestId)
            .Replace(XmlPlaceholders.RequesterTin, rfcSolicitante);

        return xml.Clean();
    }

    private static string CreateSignature(ICredential credential, string toDigest)
    {
        toDigest = toDigest.Clean();
        var digestValue = credential.CreateHash(toDigest);

        // Create the SignedInfo element with the digest value
        var signedInfo = XmlTemplates.SignedInfo
            .Replace(XmlPlaceholders.Base64Digested, digestValue)
            .Replace(XmlPlaceholders.ReferenceUri, "");

        var cleanedSignedInfo = signedInfo.Clean();
        var signatureValue = credential.SignData(cleanedSignedInfo).ToBase64String();

        var cleanSignedInfoForTemplate = cleanedSignedInfo.Replace(
            @"<SignedInfo xmlns=""http://www.w3.org/2000/09/xmldsig#"">",
            "<SignedInfo>");

        var keyInfo = XmlTemplates.X509KeyInfo
            .Replace(XmlPlaceholders.X509IssuerName, credential.Certificate.Issuer)
            .Replace(XmlPlaceholders.X509SerialNumber, credential.Certificate.SerialNumber)
            .Replace(XmlPlaceholders.Base64Cer, credential.Certificate.RawDataBytes.ToBase64String());

        var signature = XmlTemplates.SignatureTemplate
            .Replace(XmlPlaceholders.SignedInfo, cleanSignedInfoForTemplate)
            .Replace(XmlPlaceholders.Base64Signature, signatureValue)
            .Replace(XmlPlaceholders.KeyInfo, keyInfo);

        return signature;
    }

    private static string BuildEnvelope(string requestId, string rfcSolicitante, string signature) //BuildEnvelope
    {
        var envelope = XmlTemplates.VerifyEnvelope
            .Replace(XmlPlaceholders.RequestId, requestId)
            .Replace(XmlPlaceholders.RequesterTin, rfcSolicitante)
            .Replace(XmlPlaceholders.SignatureData, signature);

        return envelope.Clean();
    }
}