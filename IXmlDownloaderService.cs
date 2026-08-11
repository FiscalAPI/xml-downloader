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
using Fiscalapi.XmlDownloader.Common.Enums;
using Fiscalapi.XmlDownloader.Common.Http;
using Fiscalapi.XmlDownloader.Common.Models;
using Fiscalapi.XmlDownloader.Download.Models;
using Fiscalapi.XmlDownloader.Query.Models;
using Fiscalapi.XmlDownloader.Verify.Models;

namespace Fiscalapi.XmlDownloader;

public interface IXmlDownloaderService
{
    /// <summary>
    /// Indicates whether the service is in debug mode to log raw requests and responses.
    /// </summary>
    public bool IsDebugEnabled { get; set; }

    /// <summary>
    /// Fiel used for authentication with the SAT service.
    /// </summary>
    public ICredential? Credential { get; set; }

    /// <summary>
    /// Authentication token used for all requests to the SAT service.
    /// </summary>
    public Token? Token { get; set; }

    /// <summary>
    /// Service endpoints currently configured for the service (CFDI or Retenciones).
    /// </summary>
    public ServiceEndpoints ServiceEndpoints { get; }

    /// <summary>
    /// Sets the service type (CFDI or Retenciones) and switches endpoints accordingly.
    /// If the current token is from a different service type, it will be invalidated to force re-authentication.
    /// If Credential is set, re-authentication will be performed automatically.
    /// </summary>
    /// <param name="serviceType">The service type to switch to</param>
    /// <param name="forceReauthenticate">If true, invalidates the current token even if it matches the service type</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Task representing the asynchronous operation</returns>
    Task SetServiceType(ServiceType serviceType, bool forceReauthenticate = false, CancellationToken cancellationToken = default);


    /// <summary>
    /// Authenticates the user with SAT and retrieves an authentication token using the provided certificate and key in Base64 format.
    /// </summary>
    /// <remarks>
    /// Contrato: en fallo lógico del SAT (HTTP 500, CodEstatus de error) este método NO lanza
    /// excepción; regresa <see cref="AuthResponse"/> con <c>Succeeded=false</c> y <c>Token</c> en
    /// null. El llamador debe revisar <c>Succeeded</c> y <c>Token</c>. Solo los fallos de
    /// transporte/parsing (HTTP, XML) se propagan como excepción.
    /// </remarks>
    /// <param name="base64Cer">Base64 Fiel Certificate</param>
    /// <param name="base64Key">Base64 Fiel PrivateKey</param>
    /// <param name="password">Plain Fiel PrivateKey's Password Phrase</param>
    /// <param name="endpoints">Service endpoints to use for authentication. Defaults to CFDI endpoints if null.</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>AuthResponse</returns>
    Task<AuthResponse> AuthenticateAsync(string base64Cer, string base64Key, string password,
        ServiceEndpoints? endpoints = null, CancellationToken cancellationToken = default);
    /// <summary>
    /// Authenticates the user with SAT and retrieves an authentication token given the Fiel credential.
    /// </summary>
    /// <remarks>
    /// Contrato: en fallo lógico del SAT NO lanza; regresa <c>Succeeded=false</c> con
    /// <c>Token</c> en null. Revisa <c>Succeeded</c> antes de continuar.
    /// </remarks>
    /// <param name="credential"></param>
    /// <param name="endpoints">Service endpoints to use for authentication. Defaults to CFDI endpoints if null.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>AuthResponse</returns>
    Task<AuthResponse> AuthenticateAsync(ICredential credential, ServiceEndpoints? endpoints = null, CancellationToken cancellationToken = default);


    /// <summary>
    /// Authenticates the user to CFDI endpoints with SAT and retrieves an authentication token given the Fiel credential.
    /// </summary>
    /// <remarks>
    /// Contrato: en fallo lógico del SAT NO lanza; regresa <c>Succeeded=false</c> con
    /// <c>Token</c> en null. Revisa <c>Succeeded</c> antes de continuar.
    /// </remarks>
    /// <param name="credential"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>AuthResponse</returns>
    Task<AuthResponse> AuthenticateAsync(ICredential credential, CancellationToken cancellationToken = default);


    /// <summary>
    /// Creates a 'download request' to the SAT with the specified query parameters.
    /// </summary>
    /// <param name="parameters"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>QueryResponse</returns>
    Task<QueryResponse> CreateRequestAsync(QueryParameters parameters, CancellationToken cancellationToken = default);

    /// <summary>
    /// Verifies the status of a previously created request using its request ID.
    /// </summary>
    /// <param name="requestId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>VerifyResponse</returns>
    Task<VerifyResponse> VerifyAsync(string requestId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Downloads a package from SAT using the specified package ID.
    /// </summary>
    /// <param name="packageId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>DownloadResponse</returns>
    Task<DownloadResponse> DownloadAsync(string packageId, CancellationToken cancellationToken = default);


    /// <summary>
    /// Writes the downloaded package bytes to a file at the specified path.
    /// </summary>
    /// <param name="fullFilePath">Disk path where package will be written</param>
    /// <param name="bytes">Package data in bytes</param>
    /// <param name="cancellationToken">cancellationToken</param>
    /// <returns>Completed Task</returns>
    Task WritePackageAsync(string fullFilePath, byte[] bytes, CancellationToken cancellationToken = default);


    /// <summary>
    /// Writes the downloaded package data to a file at the specified path.
    /// </summary>
    /// <param name="fullFilePath">Disk path where package will be written</param>
    /// <param name="base64Package">Package data in base 64</param>
    /// <param name="cancellationToken">cancellationToken</param>
    /// <returns>Completed Task</returns>
    Task WritePackageAsync(string fullFilePath, string base64Package, CancellationToken cancellationToken = default);


    /// <summary>
    /// Reads a file from the disk at the specified path and returns its content as a byte array.
    /// </summary>
    /// <param name="fullFilePath">File path</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>data in bytes</returns>
    Task<byte[]> ReadFileAsync(string fullFilePath, CancellationToken cancellationToken = default);


    /// <summary>
    /// Retrieves per-document results from a package represented by its .zip file path.
    /// Unreadable documents yield failed results (<c>Succeeded = false</c>) with the raw XML
    /// and error; the enumeration is never aborted by a single document. Supports CFDI 3.3
    /// and 4.0 (3.3 is mapped to the unified 4.0 model preserving <c>Version = "3.3"</c>).
    /// </summary>
    /// <param name="fullZipFilePath">Package .zip file path</param>
    /// <param name="extractToPath">Path where to extract the zip file</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>A <see cref="IAsyncEnumerable{ComprobanteResult}"/> of per-document results.</returns>
    /// <exception cref="CorruptPackageException">The whole ZIP is unreadable (corrupt or empty).</exception>
    IAsyncEnumerable<ComprobanteResult> GetComprobantesAsync(string fullZipFilePath, string extractToPath,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves per-document results from a package represented by its byte array.
    /// Unreadable documents yield failed results (<c>Succeeded = false</c>) with the raw XML
    /// and error; the enumeration is never aborted by a single document. Supports CFDI 3.3
    /// and 4.0 (3.3 is mapped to the unified 4.0 model preserving <c>Version = "3.3"</c>).
    /// </summary>
    /// <param name="packageBytes">Package Bytes</param>
    /// <param name="cancellationToken">cancellationToken</param>
    /// <returns>A <see cref="IAsyncEnumerable{ComprobanteResult}"/> of per-document results.</returns>
    /// <exception cref="CorruptPackageException">The whole ZIP is unreadable (corrupt or empty).</exception>
    IAsyncEnumerable<ComprobanteResult> GetComprobantesAsync(byte[] packageBytes,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves per-document results from a package represented by its DownloadResponse.
    /// Unreadable documents yield failed results (<c>Succeeded = false</c>) with the raw XML
    /// and error; the enumeration is never aborted by a single document.
    /// </summary>
    /// <param name="downloadResponse">DownloadResponse</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>A <see cref="IAsyncEnumerable{ComprobanteResult}"/> of per-document results.</returns>
    /// <exception cref="CorruptPackageException">The ZIP is unreadable, empty or the base64 is invalid.</exception>
    IAsyncEnumerable<ComprobanteResult> GetComprobantesAsync(DownloadResponse downloadResponse,
        CancellationToken cancellationToken = default);


    /// <summary>
    /// Retrieves per-line metadata results from a package represented by its .zip file path.
    /// Malformed lines yield failed results with <c>LineNumber</c>/<c>RawLine</c>; the
    /// enumeration is never aborted by a single line.
    /// </summary>
    /// <param name="fullFilePath">Package .zip file path</param>
    /// <param name="extractToPath">Path where to extract the zip file</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>A <see cref="IAsyncEnumerable{MetaItemResult}"/> of per-line results.</returns>
    /// <exception cref="CorruptPackageException">The whole ZIP is unreadable (corrupt or empty).</exception>
    IAsyncEnumerable<MetaItemResult> GetMetadataAsync(string fullFilePath, string extractToPath,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves per-line metadata results from a package represented by its byte array.
    /// Malformed lines yield failed results with <c>LineNumber</c>/<c>RawLine</c>; the
    /// enumeration is never aborted by a single line.
    /// </summary>
    /// <param name="packageBytes">Package Bytes</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>A <see cref="IAsyncEnumerable{MetaItemResult}"/> of per-line results.</returns>
    /// <exception cref="CorruptPackageException">The whole ZIP is unreadable (corrupt or empty).</exception>
    IAsyncEnumerable<MetaItemResult> GetMetadataAsync(byte[] packageBytes,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves per-line metadata results from a package represented by its DownloadResponse.
    /// Malformed lines yield failed results with <c>LineNumber</c>/<c>RawLine</c>; the
    /// enumeration is never aborted by a single line.
    /// </summary>
    /// <param name="downloadResponse">DownloadResponse</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>A <see cref="IAsyncEnumerable{MetaItemResult}"/> of per-line results.</returns>
    /// <exception cref="CorruptPackageException">The ZIP is unreadable, empty or the base64 is invalid.</exception>
    IAsyncEnumerable<MetaItemResult> GetMetadataAsync(DownloadResponse downloadResponse,
        CancellationToken cancellationToken = default);
}