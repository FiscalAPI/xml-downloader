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
using Fiscalapi.XmlDownloader.Query.Models;
using Microsoft.Extensions.Logging;

namespace Fiscalapi.XmlDownloader.Query;

/// <summary>
/// Service for creating download requests from the SAT.
/// </summary>
public class QueryService : SatService, IQueryService
{
    /// <summary>
    /// Constructor for dependency injection scenarios
    /// </summary>
    /// <param name="httpClient">HttpClient instance</param>
    /// <param name="logger">Logger instance</param>
    public QueryService(HttpClient httpClient, ILogger<QueryService> logger) 
        : base(httpClient, logger)
    {
    }

    /// <summary>
    /// Constructor for direct instantiation scenarios
    /// </summary>
    /// <param name="logger">Logger instance</param>
    public QueryService(ILogger<QueryService> logger) 
        : base(logger)
    {
    }

    /// <summary>
    /// Creates a download request based on the provided parameters.
    /// </summary>
    /// <param name="credential">Fiel</param>
    /// <param name="authToken">Authentication token</param>
    /// <param name="parameters">Request parameters</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <param name="logger">Logger</param>
    /// <returns>QueryResponse</returns>
    public async Task<QueryResponse> CreateAsync(ICredential credential, Token authToken,
        QueryParameters parameters, ILogger logger, CancellationToken cancellationToken = default)
    {
        var operationType = DetermineOperationType(parameters);
        var toDigest = CreateDigest(parameters, operationType);
        var signature = CreateSignature(credential, toDigest);
        var requestXml = BuildEnvelope(parameters, signature, operationType);

        var satResponse = await SendRequestAsync(
            url: SatUrl.RequestUrl,
            action: GetSoapActionByOperation(operationType),
            payload: requestXml,
            token: authToken.Value,
            cancellationToken: cancellationToken);

        var requestResponse = QueryResponseService.Build(satResponse, logger);
        return requestResponse;
    }


    /// <summary>
    /// Creates the digest
    /// </summary>
    private string CreateDigest(QueryParameters parameters, DownloadType operationType)
    {
        var extraXml = string.Empty;
        var requestAttributes = BuildRequestAttributes(parameters, operationType, ref extraXml);

        // Filtrar atributos vacíos y ordenarlos alfabéticamente
        var filteredAttributes = requestAttributes
            .Where(kvp => !string.IsNullOrEmpty(kvp.Value))
            .OrderBy(kvp => kvp.Key)
            .ToList();

        var attributes = string.Join(" ", filteredAttributes.Select(kvp =>
            $@"{kvp.Key}=""{kvp.Value}"""));

        var digestTemplate = operationType switch
        {
            DownloadType.Emitidos => XmlTemplates.EmitidosToDigest,
            DownloadType.Recibidos => XmlTemplates.RecibidosToDigest,
            DownloadType.Uuid => XmlTemplates.FolioToDigest,
            _ => throw new InvalidOperationException($"Unsupported operation type: {operationType}")
        };

        var xml = digestTemplate
            .Replace(XmlPlaceholders.RequestAttributes, attributes)
            .Replace(XmlPlaceholders.ExtraXml, extraXml);

        return xml.Clean();
    }

    /// <summary>
    /// Creates the complete signature using unified SignedInfo template
    /// </summary>
    private string CreateSignature(ICredential credential, string toDigest)
    {
        toDigest = toDigest.Clean();
        var digestValue = credential.CreateHash(toDigest);

        // Create SignedInfo
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

    /// <summary>
    /// Builds the complete request envelope BuildEnvelope
    /// </summary>
    private string BuildEnvelope(QueryParameters parameters, string signature,
        DownloadType operationType)
    {
        var extraXml = string.Empty;
        var requestAttributes = BuildRequestAttributes(parameters, operationType, ref extraXml);

        // Filter empty attributes and sort them alphabetically
        var filteredAttributes = requestAttributes
            .Where(kvp => !string.IsNullOrEmpty(kvp.Value))
            .OrderBy(kvp => kvp.Key)
            .ToList();

        var attributes = string.Join(" ", filteredAttributes.Select(kvp =>
            $@"{kvp.Key}=""{kvp.Value}"""));

        var nodeName = operationType switch
        {
            DownloadType.Emitidos => "SolicitaDescargaEmitidos",
            DownloadType.Recibidos => "SolicitaDescargaRecibidos",
            DownloadType.Uuid => "SolicitaDescargaFolio",
            _ => throw new InvalidOperationException($"Unsupported operation type: {operationType}")
        };

        var envelope = XmlTemplates.QueryEnvelope
            .Replace(XmlPlaceholders.NodeName, nodeName)
            .Replace(XmlPlaceholders.RequestAttributes, attributes)
            .Replace(XmlPlaceholders.ExtraXml, extraXml)
            .Replace(XmlPlaceholders.SignatureData, signature);

        return envelope.Clean();
    }

    /// <summary>
    /// Builds request attributes
    /// </summary>
    private Dictionary<string, string> BuildRequestAttributes(QueryParameters parameters,
        DownloadType operationType, ref string extraXml)
    {
        var requestAttributes = new Dictionary<string, string>();

        switch (operationType)
        {
            case DownloadType.Emitidos:
                BuildEmitidosAttributes(parameters, requestAttributes, ref extraXml);
                break;

            case DownloadType.Recibidos:
                BuildRecibidosAttributes(parameters, requestAttributes);
                break;

            case DownloadType.Uuid:
                BuildFolioAttributes(parameters, requestAttributes);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(operationType), operationType, null);
        }

        return requestAttributes;
    }

    /// <summary>
    /// Builds attributes for SolicitaDescargaEmitidos 
    /// </summary>
    private void BuildEmitidosAttributes(QueryParameters parameters,
        IDictionary<string, string> requestAttributes, ref string extraXml)
    {
        requestAttributes["Complemento"] = parameters.InvoiceComplement.ToEnumCode();
        requestAttributes["EstadoComprobante"] = parameters.InvoiceStatus.ToEnumCode();
        requestAttributes["FechaInicial"] = parameters.StartDate.ToSatFormat();
        requestAttributes["FechaFinal"] = parameters.EndDate.ToSatFormat();

        if (!string.IsNullOrWhiteSpace(parameters.IssuerTin))
            requestAttributes["RfcEmisor"] = parameters.IssuerTin.ToUpperInvariant();

        if (!string.IsNullOrWhiteSpace(parameters.RequesterTin))
            requestAttributes["RfcSolicitante"] = parameters.RequesterTin.ToUpperInvariant();

        requestAttributes["TipoComprobante"] = parameters.InvoiceType.ToEnumCode();
        requestAttributes["TipoSolicitud"] = parameters.RequestType.ToEnumCode();

        if (!string.IsNullOrWhiteSpace(parameters.ThirdPartyTin))
            requestAttributes["RfcACuentaTerceros"] = parameters.ThirdPartyTin.ToUpperInvariant();

        // Build RfcReceptores XML if present
        if (!string.IsNullOrWhiteSpace(parameters.RecipientTin))
        {
            var rfcReceptorItem = XmlTemplates.RfcReceptorTemplate
                .Replace(XmlPlaceholders.RecipientTin, parameters.RecipientTin.ToUpperInvariant());

            extraXml = XmlTemplates.RfcReceptoresTemplate
                .Replace(XmlPlaceholders.RfcReceptorItems, rfcReceptorItem);
        }
    }

    /// <summary>
    /// Builds attributes for SolicitaDescargaRecibidos 
    /// </summary>
    private void BuildRecibidosAttributes(QueryParameters parameters,
        IDictionary<string, string> requestAttributes)
    {
        requestAttributes["Complemento"] = parameters.InvoiceComplement.ToEnumCode();
        requestAttributes["EstadoComprobante"] = parameters.InvoiceStatus.ToEnumCode();
        requestAttributes["FechaInicial"] = parameters.StartDate.ToSatFormat();
        requestAttributes["FechaFinal"] = parameters.EndDate.ToSatFormat();

        if (!string.IsNullOrWhiteSpace(parameters.IssuerTin))
            requestAttributes["RfcEmisor"] = parameters.IssuerTin.ToUpperInvariant();

        if (!string.IsNullOrWhiteSpace(parameters.RequesterTin))
            requestAttributes["RfcSolicitante"] = parameters.RequesterTin.ToUpperInvariant();

        requestAttributes["TipoComprobante"] = parameters.InvoiceType.ToEnumCode();

        requestAttributes["TipoSolicitud"] = parameters.RequestType.ToEnumCode();

        if (!string.IsNullOrWhiteSpace(parameters.RecipientTin))
            requestAttributes["RfcReceptor"] = parameters.RecipientTin.ToUpperInvariant();

        if (!string.IsNullOrWhiteSpace(parameters.ThirdPartyTin))
            requestAttributes["RfcACuentaTerceros"] = parameters.ThirdPartyTin.ToUpperInvariant();
    }

    /// <summary>
    /// Builds attributes for SolicitaDescargaFolio
    /// </summary>
    private static void BuildFolioAttributes(QueryParameters parameters,
        IDictionary<string, string> requestAttributes)
    {
        if (!string.IsNullOrWhiteSpace(parameters.InvoiceUuid))
            requestAttributes["Folio"] = parameters.InvoiceUuid;

        if (!string.IsNullOrWhiteSpace(parameters.RequesterTin))
            requestAttributes["RfcSolicitante"] = parameters.RequesterTin.ToUpperInvariant();
    }

    private static string GetSoapActionByOperation(DownloadType operationType)
    {
        return operationType switch
        {
            DownloadType.Emitidos => SatUrl.RequestIssuedAction,
            DownloadType.Recibidos => SatUrl.RequestReceivedAction,
            DownloadType.Uuid => SatUrl.RequestUuidAction,
            _ => throw new InvalidOperationException($"Unsupported operation type: {operationType}")
        };
    }

    private static DownloadType DetermineOperationType(QueryParameters parameters)
    {
        if (!string.IsNullOrEmpty(parameters.InvoiceUuid))
            return DownloadType.Uuid;

        return !string.IsNullOrEmpty(parameters.IssuerTin)
            ? DownloadType.Emitidos
            : DownloadType.Recibidos;
    }
}