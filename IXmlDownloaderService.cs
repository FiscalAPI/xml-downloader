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
    /// Authenticates the user with SAT and retrieves an authentication token using the provided certificate and key in Base64 format.
    /// <param name="base64Cer">Base64 Fiel Certificate</param>
    /// <param name="base64Key">Base64 Fiel PrivateKey</param>
    /// <param name="password">Plain Fiel PrivateKey's Password Phrase</param>
    /// <param name="cancellationToken"></param>
    /// <returns>AuthResponse</returns>
    /// </summary>
    Task<AuthResponse> AuthenticateAsync(string base64Cer, string base64Key, string password,
        CancellationToken cancellationToken = default);


    /// <summary>
    /// Authenticates the user with SAT and retrieves an authentication token given the Fiel credential.
    /// </summary>
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
    /// Retrieves a list of Comprobantes from a package represented by its extracted directory path.
    /// </summary>
    /// <param name="fullZipFilePath">Package .zip file path</param>
    /// <param name="extractToPath"></param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>List of Comprobantes objects</returns>
    IAsyncEnumerable<Comprobante> GetComprobantesAsync(string fullZipFilePath, string extractToPath,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a list of Comprobantes from a package represented by its byte array.
    /// </summary>
    /// <param name="packageBytes">Package Bytes</param>
    /// <param name="cancellationToken">cancellationToken</param>
    /// <returns>List of Comprobantes objects</returns>
    IAsyncEnumerable<Comprobante> GetComprobantesAsync(byte[] packageBytes,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a list of Comprobantes from a package represented by its DownloadResponse.
    /// </summary>
    /// <param name="downloadResponse">DownloadResponse</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>List of Comprobantes objects</returns>
    IAsyncEnumerable<Comprobante> GetComprobantesAsync(DownloadResponse downloadResponse,
        CancellationToken cancellationToken = default);
}