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

using System.IO.Compression;
using System.Runtime.CompilerServices;
using Fiscalapi.Credentials.Common;
using Fiscalapi.Credentials.Core;
using Fiscalapi.XmlDownloader.Auth;
using Fiscalapi.XmlDownloader.Auth.Models;
using Fiscalapi.XmlDownloader.Common;
using Fiscalapi.XmlDownloader.Common.Enums;
using Fiscalapi.XmlDownloader.Common.Http;
using Fiscalapi.XmlDownloader.Common.Models;
using Fiscalapi.XmlDownloader.Download;
using Fiscalapi.XmlDownloader.Download.Models;
using Fiscalapi.XmlDownloader.FileStorage;
using Fiscalapi.XmlDownloader.Query;
using Fiscalapi.XmlDownloader.Query.Models;
using Fiscalapi.XmlDownloader.Verify;
using Fiscalapi.XmlDownloader.Verify.Models;
using Microsoft.Extensions.Logging;

namespace Fiscalapi.XmlDownloader;

public class XmlDownloaderService : IXmlDownloaderService, IDisposable
{
    public bool IsDebugEnabled { get; set; }
    public Token? Token { get; set; }
    public ICredential? Credential { get; set; }

    private ServiceEndpoints _serviceEndpoints;
    public ServiceEndpoints ServiceEndpoints => _serviceEndpoints;

    private readonly ILogger<XmlDownloaderService> _logger;
    private readonly IAuthService _authService;
    private readonly IQueryService _queryService;
    private readonly IVerifyService _verifyService;
    private readonly IDownloadService _downloadService;
    private readonly IFileStorageService _storageService;
    
    private readonly bool _ownsServices;
    private bool _disposed;

    /// <summary>
    /// Default constructor for dependency injection
    /// </summary>
    /// <param name="authService">Authentication service</param>
    /// <param name="queryService">Query service</param>
    /// <param name="verifyService">Verify service</param>
    /// <param name="downloadService">Download service</param>
    /// <param name="storageService">File storage service</param>
    /// <param name="logger">Logger instance</param>
    public XmlDownloaderService(
        IAuthService authService,
        IQueryService queryService,
        IVerifyService verifyService,
        IDownloadService downloadService,
        IFileStorageService storageService,
        ILogger<XmlDownloaderService> logger
    )
    {
        _authService = authService;
        _queryService = queryService;
        _verifyService = verifyService;
        _downloadService = downloadService;
        _storageService = storageService;
        _logger = logger;
        _ownsServices = false;
        _serviceEndpoints = ServiceEndpoints.Cfdi(); // Default to CFDI
    }

    /// <summary>
    /// Default constructor no dependency injection 
    /// </summary>
    public XmlDownloaderService()
    {
        // Create logger factory for non-DI scenario
        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole().SetMinimumLevel(LogLevel.Information);
        });
        
        // Create logger for this service
        _logger = loggerFactory.CreateLogger<XmlDownloaderService>();

        // Create services with their own specific loggers in non-DI scenario
        _authService = new AuthService(loggerFactory.CreateLogger<AuthService>());
        _queryService = new QueryService(loggerFactory.CreateLogger<QueryService>());
        _verifyService = new VerifyService(loggerFactory.CreateLogger<VerifyService>());
        _downloadService = new DownloadService(loggerFactory.CreateLogger<DownloadService>());
        _storageService = new FileStorageService();
        _serviceEndpoints = ServiceEndpoints.Cfdi(); // Default to CFDI
        _ownsServices = true;
    }

    /// <summary>
    /// Sets the service type (CFDI or Retenciones) and switches endpoints accordingly.
    /// If the current token is from a different service type, it will be invalidated to force re-authentication.
    /// If Credential is set, re-authentication will be performed automatically.
    /// </summary>
    /// <param name="serviceType">The service type to switch to</param>
    /// <param name="forceReauthenticate">If true, invalidates the current token even if it matches the service type</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Task representing the asynchronous operation</returns>
    public async Task SetServiceType(ServiceType serviceType, bool forceReauthenticate = false, CancellationToken cancellationToken = default)
    {
        var newEndpoints = serviceType switch
        {
            ServiceType.Cfdi => ServiceEndpoints.Cfdi(),
            ServiceType.Retenciones => ServiceEndpoints.Retenciones(),
            _ => throw new ArgumentException($"Unknown service type: {serviceType}", nameof(serviceType))
        };

        // Check if we're switching to a different service type
        var isSwitchingServiceType = _serviceEndpoints.ServiceType != newEndpoints.ServiceType;

        // Update endpoints atomically
        _serviceEndpoints = newEndpoints;


        // If switching service type or force re-authenticate, invalidate token
        var needsReauthentication = isSwitchingServiceType || forceReauthenticate;
        
        if (needsReauthentication)
        {
            if (Token != null)
            {
                _logger.LogInformation(
                    "Invalidating token due to service type change. Old ServiceType: {OldServiceType}, New ServiceType: {NewServiceType}, ForceReauthenticate: {ForceReauthenticate}",
                    _serviceEndpoints.ServiceType,
                    newEndpoints.ServiceType,
                    forceReauthenticate);
            }
            
            Token = null; // Invalidate token to force re-authentication

            // If Credential is available, re-authenticate automatically
            if (Credential != null)
            {
                _logger.LogInformation(
                    "Re-authenticating automatically after service type change. ServiceType: {ServiceType}, RFC: {Rfc}",
                    newEndpoints.ServiceType,
                    Credential.Certificate.Rfc);

                try
                {
                    var authResponse = await _authService.AuthenticateAsync(
                        credential: Credential,
                        endpoints: _serviceEndpoints,
                        logger: _logger,
                        cancellationToken: cancellationToken);

                    Token = authResponse.Token;

                    if (!authResponse.Succeeded)
                    {
                        _logger.LogError(
                            "Automatic re-authentication failed after service type change. RFC: {Rfc}, Status: {StatusCode}, Message: {Message}",
                            Credential.Certificate.Rfc,
                            authResponse.SatStatusCode,
                            authResponse.SatMessage);
                    }
                    else if (Token == null)
                    {
                        _logger.LogError(
                            "Automatic re-authentication succeeded but token is null. RFC: {Rfc}",
                            Credential.Certificate.Rfc);
                    }
                    else
                    {
                        _logger.LogInformation(
                            "Automatic re-authentication succeeded. RFC: {Rfc}, ServiceType: {ServiceType}",
                            Credential.Certificate.Rfc,
                            newEndpoints.ServiceType);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex,
                        "Error during automatic re-authentication after service type change. RFC: {Rfc}, ServiceType: {ServiceType}",
                        Credential.Certificate.Rfc,
                        newEndpoints.ServiceType);
                     throw;
                }
            }
        }
    }

    /// <summary>
    /// Authenticate with a certificate and private key
    /// </summary>
    /// <param name="base64Cer">Base64 encoded certificate</param>
    /// <param name="base64Key">Base64 encoded private key</param>
    /// <param name="password">Plain text password for the private key</param>
    /// <param name="endpoints">Service endpoints to use for authentication. Defaults to CFDI endpoints if null.</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="AuthResponse"/> object containing the authentication token.</returns>
    public async Task<AuthResponse> AuthenticateAsync(string base64Cer, string base64Key, string password,
        ServiceEndpoints? endpoints = null, CancellationToken cancellationToken = default)
    {
        // Set service endpoints before authenticating (default to CFDI if not specified)
        _serviceEndpoints = endpoints ?? ServiceEndpoints.Cfdi();

        ICertificate certificate = new Certificate(base64Cer);
        IPrivateKey privateKey = new PrivateKey(base64Key, password);
        ICredential credential = new Credential(certificate, privateKey);

        Credential = credential;

        return await AuthenticateAsync(credential, cancellationToken);
    }

    /// <summary>
    /// Authenticate with a FiscalAPI credential.
    /// See https://github.com/FiscalAPI/fiscalapi-credentials-net for more information.
    /// </summary>
    /// <param name="credential">Credential represent a FIEL certificate and private key</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="AuthResponse"/> object containing the authentication token.</returns>
    public async Task<AuthResponse> AuthenticateAsync(ICredential credential,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting authentication for RFC: {Rfc}", credential.Certificate.Rfc);
        
        // Configure algorithm for XML downloader (SHA1) before authenticating
        credential.ConfigureAlgorithmForXmlDownloader();
        
        var authResponse = await _authService.AuthenticateAsync(
            credential: credential,
            endpoints: _serviceEndpoints,
            logger: _logger,
            cancellationToken: cancellationToken);

        Token = authResponse.Token;
        Credential = credential;
        
        // Critical logging: Authentication result
        if (!authResponse.Succeeded)
        {
            _logger.LogError("Authentication failed. RFC: {Rfc}, Status: {StatusCode}, Message: {Message}",
                credential.Certificate.Rfc,
                authResponse.SatStatusCode,
                authResponse.SatMessage);
        }
        else if (Token == null)
        {
            _logger.LogError(
                "Authentication succeeded but token is null. RFC: {Rfc}, TokenValue is null or empty: {IsNullOrEmpty}",
                credential.Certificate.Rfc,
                string.IsNullOrWhiteSpace(authResponse.TokenValue));
        }
        else
        {
            _logger.LogInformation("Authentication succeeded. RFC: {Rfc}, Token is valid", credential.Certificate.Rfc);
        }
        
        return authResponse;
    }

    /// <summary>
    /// Create a download request to the SAT using the given parameters.
    /// </summary>
    /// <param name="parameters">Query parameters</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="QueryResponse"/> object containing the request ID.</returns>
    public async Task<QueryResponse> CreateRequestAsync(QueryParameters parameters,
        CancellationToken cancellationToken = default)
    {
        // Critical logging: Check token before proceeding
        if (Token == null || Credential == null)
        {
            _logger.LogError("Cannot create download request: Token or Credential is null. Token is null: {TokenIsNull}, Credential is null: {CredentialIsNull}",
                Token == null,
                Credential == null);
        }
        
        EnsureAuthToken();

        _logger.LogInformation("Creating download request for RFC: {Rfc}, ServiceType: {ServiceType}", 
            Credential!.Certificate.Rfc, _serviceEndpoints.ServiceType);
        
        return await _queryService.CreateAsync(
            credential: Credential!,
            authToken: Token!,
            parameters: parameters,
            endpoints: _serviceEndpoints,
            logger: _logger,
            cancellationToken: cancellationToken
        );
    }

    /// <summary>
    /// Verify a download request using the given request ID.
    /// </summary>
    /// <param name="requestId">Request ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="VerifyResponse"/> object containing the package ID.</returns>
    public Task<VerifyResponse> VerifyAsync(string requestId, CancellationToken cancellationToken = default)
    {
        EnsureAuthToken();

        return _verifyService.VerifyAsync(
            credential: Credential!,
            authToken: Token!,
            requestId: requestId,
            endpoints: _serviceEndpoints,
            logger: _logger,
            cancellationToken: cancellationToken
        );
    }

    /// <summary>
    /// Download a package from the SAT using the given package ID resolved from a verify request.
    /// </summary>
    /// <param name="packageId">Package ID resolved from a verify request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="DownloadResponse"/> object containing the package data.</returns>
    public Task<DownloadResponse> DownloadAsync(string packageId, CancellationToken cancellationToken = default)
    {
        EnsureAuthToken();

        return _downloadService.DownloadAsync(
            credential: Credential!,
            authToken: Token!,
            packageId: packageId,
            endpoints: _serviceEndpoints,
            logger: _logger,
            cancellationToken: cancellationToken
        );
    }

    /// <summary>
    /// Writes a package .zip file to the specified file path.
    /// </summary>
    /// <param name="fullFilePath">Full path including directory and file name and extension</param>
    /// <param name="bytes">Package bytes</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task WritePackageAsync(string fullFilePath, byte[] bytes,
        CancellationToken cancellationToken = default)
    {
        await _storageService.WriteFileAsync(fullFilePath, bytes, cancellationToken, _logger);
    }

    /// <summary>
    /// Writes a package .zip file to the specified file path.
    /// The package is a .zip file containing the CFDIs or Metadata (.txt) files.
    /// </summary>
    /// <param name="fullFilePath">Full path including directory and file name and extension</param>
    /// <param name="base64Package">Base64 encoded package</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task WritePackageAsync(string fullFilePath, string base64Package,
        CancellationToken cancellationToken = default)
    {
        await _storageService.WriteFileAsync(fullFilePath, base64Package, cancellationToken, _logger);
    }

    /// <summary>
    /// Reads a file from the specified file path.
    /// </summary>
    /// <param name="fullFilePath">File path</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>File content in bytes</returns>
    public async Task<byte[]> ReadFileAsync(string fullFilePath, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return await _storageService.ReadFileAsync(fullFilePath, cancellationToken, _logger);
    }

    /// <summary>
    /// Retrieves a list of Comprobantes from a package represented by its extracted directory path.
    /// </summary>
    /// <param name="fullFilePath">Package .zip file path</param>
    /// <param name="extractToPath"></param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>A <see cref="IAsyncEnumerable{Comprobante}"/> of <see cref="Comprobante"/> objects.</returns>
    public async IAsyncEnumerable<Comprobante> GetComprobantesAsync(string fullFilePath, string extractToPath,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        //Unzip the package to a default directory
        _storageService.ExtractZipFile(
            fullFilePath: fullFilePath,
            extractToPath: extractToPath,
            logger: _logger,
            cancellationToken: cancellationToken
        );

        //Load file details 
        var files = _storageService.GetFiles(extractToPath, FileStorageSettings.CfdiExtension);

        //Serialize each XML file to Comprobante object
        foreach (var file in files)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var xml = await _storageService.ReadFileContentAsync(file.FilePath, cancellationToken, _logger);
            var comprobante = XmlSerializerService.Deserialize<Comprobante>(xml);

            if (comprobante is null) continue;

            comprobante.Base64Content = xml.EncodeToBase64();
            comprobante.DeserializeComplements();
            yield return comprobante;
        }
    }

    /// <summary>
    /// Retrieves a list of Comprobantes from a package represented by its byte array.
    /// </summary>
    /// <param name="packageBytes">Package Bytes</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>A <see cref="IAsyncEnumerable{Comprobante}"/> of <see cref="Comprobante"/> objects.</returns>
    public async IAsyncEnumerable<Comprobante> GetComprobantesAsync(byte[] packageBytes,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        using var memoryStream = new MemoryStream(packageBytes);
        using var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Read);

        foreach (var entry in zipArchive.Entries)
        {
            cancellationToken.ThrowIfCancellationRequested();

            //verify if the entry is an XML file
            if (!entry.Name.EndsWith(FileStorageSettings.CfdiExtension, StringComparison.OrdinalIgnoreCase))
                continue;

            // Read the entry stream
            await using var entryStream = entry.Open();
            using var reader = new StreamReader(entryStream);

            var xml = await reader.ReadToEndAsync(cancellationToken);

            var comprobante = XmlSerializerService.Deserialize<Comprobante>(xml);

            if (comprobante is null) continue;

            comprobante.Base64Content = xml.EncodeToBase64();
            comprobante.DeserializeComplements();
            yield return comprobante;
        }
    }

    /// <summary>
    /// Retrieves a list of Comprobantes from a package represented by its DownloadResponse.
    /// </summary>
    /// <param name="downloadResponse">DownloadResponse</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>A <see cref="IAsyncEnumerable{Comprobante}"/> of <see cref="Comprobante"/> objects.</returns>
    public async IAsyncEnumerable<Comprobante> GetComprobantesAsync(DownloadResponse downloadResponse,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        if (downloadResponse is null)
        {
            throw new ArgumentNullException(nameof(downloadResponse), "Download response cannot be null.");
        }

        if (downloadResponse.PackageSize <= 0)
        {
            throw new ArgumentException("Package ID cannot be null or empty.", nameof(downloadResponse.PackageBytes));
        }

        cancellationToken.ThrowIfCancellationRequested();

        await foreach (var comprobante in GetComprobantesAsync(downloadResponse.PackageBytes, cancellationToken))
        {
            yield return comprobante;
        }
    }


    /// <summary>
    /// Retrieves a list of MetaItems from a package represented by its extracted directory path.
    /// </summary>
    /// <param name="fullFilePath">Package .zip file path</param>
    /// <param name="extractToPath">Path where to extract the zip file</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>A <see cref="IAsyncEnumerable{MetaItem}"/> of <see cref="MetaItem"/> objects.</returns>
    public async IAsyncEnumerable<MetaItem> GetMetadataAsync(string fullFilePath, string extractToPath,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        // Unzip the package to a default directory
        _storageService.ExtractZipFile(
            fullFilePath: fullFilePath,
            extractToPath: extractToPath,
            cancellationToken: cancellationToken,
            logger: _logger
        );

        // Load file details - look for .txt files instead of XML
        var files = _storageService.GetFiles(extractToPath, FileStorageSettings.MetaExtension);

        // Process each TXT file to extract MetaItem objects
        foreach (var file in files)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var txtContent = await _storageService.ReadFileContentAsync(file.FilePath, cancellationToken, _logger);
            var lines = txtContent.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            // Skip the header line (first line contains column names)
            for (var i = 1; i < lines.Length; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (string.IsNullOrWhiteSpace(lines[i]))
                    continue;

                var metaItem = MetaItem.CreateFromString(lines[i]);
                metaItem.Base64Content = lines[i].EncodeToBase64();
                yield return metaItem;
            }
        }
    }

    /// <summary>
    /// Retrieves a list of MetaItems from a package represented by its byte array.
    /// </summary>
    /// <param name="packageBytes">Package Bytes</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>A <see cref="IAsyncEnumerable{MetaItem}"/> of <see cref="MetaItem"/> objects.</returns>
    public async IAsyncEnumerable<MetaItem> GetMetadataAsync(byte[] packageBytes,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        using var memoryStream = new MemoryStream(packageBytes);
        using var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Read);

        foreach (var entry in zipArchive.Entries)
        {
            cancellationToken.ThrowIfCancellationRequested();

            // Verify if the entry is a TXT file
            if (!entry.Name.EndsWith(FileStorageSettings.MetaExtension, StringComparison.OrdinalIgnoreCase))
                continue;

            // Read the entry stream
            await using var entryStream = entry.Open();
            using var reader = new StreamReader(entryStream);
            var txtContent = await reader.ReadToEndAsync(cancellationToken);

            var lines = txtContent.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            // Skip the header line (first line contains column names)
            for (var i = 1; i < lines.Length; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (string.IsNullOrWhiteSpace(lines[i]))
                    continue;


                var metaItem = MetaItem.CreateFromString(lines[i]);
                metaItem.Base64Content = lines[i].EncodeToBase64();

                yield return metaItem;
            }
        }
    }

    /// <summary>
    /// Retrieves a list of MetaItems from a package represented by its DownloadResponse.
    /// </summary>
    /// <param name="downloadResponse">DownloadResponse</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>A <see cref="IAsyncEnumerable{MetaItem}"/> of <see cref="MetaItem"/> objects.</returns>
    public async IAsyncEnumerable<MetaItem> GetMetadataAsync(DownloadResponse downloadResponse,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        if (downloadResponse is null)
        {
            throw new ArgumentNullException(nameof(downloadResponse), "Download response cannot be null.");
        }

        if (downloadResponse.PackageSize <= 0)
        {
            throw new ArgumentException("Package size must be greater than zero.",
                nameof(downloadResponse.PackageSize));
        }

        if (downloadResponse.PackageBytes is null || downloadResponse.PackageBytes.Length == 0)
        {
            throw new ArgumentException("Package bytes cannot be null or empty.",
                nameof(downloadResponse.PackageBytes));
        }

        cancellationToken.ThrowIfCancellationRequested();

        await foreach (var metaItem in GetMetadataAsync(downloadResponse.PackageBytes, cancellationToken))
        {
            yield return metaItem;
        }
    }


    /// <summary>
    /// Ensures that the authentication token and credential are not null.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when the authentication token or credential is null.</exception>
    private void EnsureAuthToken()
    {
        if (Token == null)
        {
            _logger.LogError("EnsureAuthToken failed: Token is null. Credential is null: {CredentialIsNull}",
                Credential == null);
            throw new InvalidOperationException("Authentication token is required. Please authenticate first.");
        }

        if (Credential == null)
        {
            _logger.LogError("EnsureAuthToken failed: Credential is null. Token is null: {TokenIsNull}",
                Token == null);
            throw new InvalidOperationException("Credential is required. Please authenticate first.");
        }
    }

    /// <summary>
    /// Disposes the service, releasing resources.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Disposes the service, releasing resources.
    /// </summary>
    /// <param name="disposing">Whether the call is from Dispose or finalizer</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing && _ownsServices)
        {
            // Dispose services in non-DI scenarios where we own them
            if (_authService is IDisposable authDisposable)
                authDisposable.Dispose();
            if (_queryService is IDisposable queryDisposable)
                queryDisposable.Dispose();
            if (_verifyService is IDisposable verifyDisposable)
                verifyDisposable.Dispose();
            if (_downloadService is IDisposable downloadDisposable)
                downloadDisposable.Dispose();
            if (_storageService is IDisposable storageDisposable)
                storageDisposable.Dispose();
        }

        _disposed = true;
    }
}