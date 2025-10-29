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
using Microsoft.Extensions.Logging;

namespace Fiscalapi.XmlDownloader.Auth;

/// <summary>
/// SAT Authentication Service.
/// </summary>
public class AuthService : SatService, IAuthService
{
    /// <summary>
    /// Authenticates with SAT using the provided credential and returns the authentication token
    /// </summary>
    public async Task<AuthResponse> AuthenticateAsync(ICredential credential,
        CancellationToken cancellationToken = default, ILogger? logger = null)
    {
        // Generate Sat XML security token ID
        var uuid = CreateSecurityToken();

        // Create digest and signature using unified template
        var digest = CreateDigest(credential);
        var signature = CreateSignature(digest, credential, uuid);

        // Build SOAP envelope
        var authXml = BuildEnvelope(digest, uuid, credential.Certificate.RawDataBytes.ToBase64String(),
            signature);

        // Send request
        var satResponse = await SendRequestAsync(
            url: SatUrl.AuthUrl,
            action: SatUrl.AuthAction,
            payload: authXml,
            cancellationToken: cancellationToken);

        // Map response 
        var authResponse = AuthResponseService.Build(satResponse, credential);

        return authResponse;
    }

    /// <summary>
    /// Creates the digest for authentication
    /// </summary>
    private static Digest CreateDigest(ICredential credential)
    {
        var created = DateTime.UtcNow;
        var expires = created.AddSeconds(300);

        var timestampXml = XmlTemplates.TimestampTemplate
            .Replace(XmlPlaceholders.Created, created.ToSatFormat())
            .Replace(XmlPlaceholders.Expires, expires.ToSatFormat());

        var base64Hash = credential.CreateHash(timestampXml.Clean());

        return new Digest
        {
            Created = created,
            Expires = expires,
            Base64Digested = base64Hash
        };
    }

    /// <summary>
    /// Creates the complete signature
    /// </summary>
    private static string CreateSignature(Digest digest, ICredential credential, string uuid)
    {
        // Create SignedInfo
        var signedInfo = XmlTemplates.SignedInfo
            .Replace(XmlPlaceholders.Base64Digested, digest.Base64Digested)
            .Replace(XmlPlaceholders.ReferenceUri, "#_0"); // Auth siempre usa #_0

        // Sign the SignedInfo
        var signatureValue = credential.SignData(signedInfo.Clean()).ToBase64String();

        // Remove namespace from SignedInfo for embedding
        var cleanSignedInfo = signedInfo.Replace(
            @"<SignedInfo xmlns=""http://www.w3.org/2000/09/xmldsig#"">",
            "<SignedInfo>");

        // Create KeyInfo 
        var keyInfo = XmlTemplates.SecurityTokenReferenceKeyInfo
            .Replace(XmlPlaceholders.Uuid, uuid);

        // Build complete signature
        var signature = XmlTemplates.SignatureTemplate
            .Replace(XmlPlaceholders.SignedInfo, cleanSignedInfo)
            .Replace(XmlPlaceholders.Base64Signature, signatureValue)
            .Replace(XmlPlaceholders.KeyInfo, keyInfo);

        return signature;
    }

    /// <summary>
    /// Builds the complete authentication envelope
    /// </summary>
    private static string BuildEnvelope(Digest digest, string uuid, string certificate, string signature)
    {
        var binarySecurityToken = XmlTemplates.BinarySecurityToken
            .Replace(XmlPlaceholders.Uuid, uuid)
            .Replace(XmlPlaceholders.Base64Cer, certificate);

        var authEnvelope = XmlTemplates.AuthEnvelope
            .Replace(XmlPlaceholders.Created, digest.Created.ToSatFormat())
            .Replace(XmlPlaceholders.Expires, digest.Expires.ToSatFormat())
            .Replace(XmlPlaceholders.Uuid, uuid)
            .Replace(XmlPlaceholders.Base64Cer, certificate)
            .Replace(XmlPlaceholders.SignatureData, signature);

        return authEnvelope.Clean();
    }

    /// <summary>
    /// Creates a unique XML security token ID
    /// </summary>
    private static string CreateSecurityToken()
    {
        var uuid = Guid.NewGuid().ToString("D").ToLowerInvariant();

        return $"uuid-{uuid}-4";
    }
}