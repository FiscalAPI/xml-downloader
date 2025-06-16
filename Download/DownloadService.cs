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
using XmlDownloader.Auth.Models;
using XmlDownloader.Common;
using XmlDownloader.Common.Http;
using XmlDownloader.Download.Models;

namespace XmlDownloader.Download;

/// <summary>
/// Service for downloading packages from SAT.
/// </summary>
public class DownloadService : SatService, IDownloadService
{
    /// <summary>
    /// Downloads a package from SAT using the provided credential, authentication token and package ID.s
    /// </summary>
    /// <param name="credential">Fiel</param>
    /// <param name="authToken">Authentication token</param>
    /// <param name="packageId">PackageID</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>DownloadResponse</returns>
    public async Task<DownloadResponse> DownloadAsync(ICredential credential, Token authToken, string packageId,
        CancellationToken cancellationToken = default)
    {
        var toDigest = CreateDigest(packageId, credential.Certificate.Rfc);
        var signature = CreateSignature(credential, toDigest);
        var requestXml = BuildEnvelope(packageId, credential.Certificate.Rfc, signature);

        var satResponse = await SendRequestAsync(
            url: SatUrl.DownloadUrl,
            action: SatUrl.DownloadAction,
            payload: requestXml,
            token: authToken.Value,
            cancellationToken: cancellationToken);

        var downloadResponse = DownloadResponseService.Build(satResponse);

        return downloadResponse;
    }

    private string CreateDigest(string packageId, string requesterTin)
    {
        var xml = XmlTemplates.DownloadToDigest
            .Replace(XmlPlaceholders.PackageId, packageId)
            .Replace(XmlPlaceholders.RequesterTin, requesterTin);

        return xml.Clean();
    }

    private string CreateSignature(ICredential credential, string toDigest)
    {
        toDigest = toDigest.Clean();
        var digestValue = credential.CreateHash(toDigest);

        // Create signed info
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

    private string BuildEnvelope(string packageId, string requesterTin, string signature)
    {
        var envelope = XmlTemplates.DownloadEnvelope
            .Replace(XmlPlaceholders.PackageId, packageId)
            .Replace(XmlPlaceholders.RequesterTin, requesterTin)
            .Replace(XmlPlaceholders.SignatureData, signature);

        return envelope.Clean();
    }
}