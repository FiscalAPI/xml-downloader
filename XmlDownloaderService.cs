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

    public Task<AuthResponse> AuthenticateAsync(ICredential credential, ServiceEndpoints? endpoints = null,
        CancellationToken cancellationToken = default)
    {
        // Set service endpoints before authenticating (default to CFDI if not specified)
        _serviceEndpoints = endpoints ?? ServiceEndpoints.Cfdi();
        Credential = credential;
        return AuthenticateAsync(credential, cancellationToken);
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
    /// Retrieves per-document results from a package represented by its .zip file path.
    /// A single unreadable document never aborts the enumeration; only an unreadable ZIP
    /// throws <see cref="CorruptPackageException"/>.
    /// </summary>
    /// <param name="fullFilePath">Package .zip file path</param>
    /// <param name="extractToPath">Path where to extract the zip file</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>A <see cref="IAsyncEnumerable{ComprobanteResult}"/> of per-document results.</returns>
    /// <exception cref="CorruptPackageException">The whole ZIP is unreadable (corrupt or empty).</exception>
    public async IAsyncEnumerable<ComprobanteResult> GetComprobantesAsync(string fullFilePath, string extractToPath,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        ExtractZipFileOrThrow(fullFilePath, extractToPath, cancellationToken);

        var files = _storageService.GetFiles(extractToPath, FileStorageSettings.CfdiExtension);

        foreach (var file in files)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var (xml, readError) = await TryReadFileAsync(file.FilePath, cancellationToken);

            if (xml is null)
            {
                _logger.LogWarning("No se pudo leer el archivo {File}: {Error}", file.FilePath, readError);
                yield return new ComprobanteResult
                {
                    SourceName = file.FilePath,
                    ErrorMessage = readError
                };
                continue;
            }

            yield return ParseComprobante(xml, file.FilePath);
        }
    }

    /// <summary>
    /// Retrieves per-document results from a package represented by its byte array.
    /// A single unreadable document never aborts the enumeration; only an unreadable ZIP
    /// throws <see cref="CorruptPackageException"/>.
    /// </summary>
    /// <param name="packageBytes">Package Bytes</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>A <see cref="IAsyncEnumerable{ComprobanteResult}"/> of per-document results.</returns>
    /// <exception cref="CorruptPackageException">The whole ZIP is unreadable (corrupt or empty).</exception>
    public async IAsyncEnumerable<ComprobanteResult> GetComprobantesAsync(byte[] packageBytes,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        using var zipArchive = OpenZipArchive(packageBytes);

        foreach (var entry in zipArchive.Entries)
        {
            cancellationToken.ThrowIfCancellationRequested();

            //verify if the entry is an XML file
            if (!entry.Name.EndsWith(FileStorageSettings.CfdiExtension, StringComparison.OrdinalIgnoreCase))
                continue;

            var (xml, readError) = await TryReadEntryAsync(entry, cancellationToken);

            if (xml is null)
            {
                _logger.LogWarning("No se pudo leer la entrada {Entry} del paquete: {Error}",
                    entry.FullName, readError);
                yield return new ComprobanteResult
                {
                    SourceName = entry.FullName,
                    ErrorMessage = readError
                };
                continue;
            }

            yield return ParseComprobante(xml, entry.FullName);
        }
    }

    /// <summary>
    /// Retrieves per-document results from a package represented by its DownloadResponse.
    /// A single unreadable document never aborts the enumeration.
    /// </summary>
    /// <param name="downloadResponse">DownloadResponse</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>A <see cref="IAsyncEnumerable{ComprobanteResult}"/> of per-document results.</returns>
    /// <exception cref="CorruptPackageException">The ZIP is unreadable, empty or the base64 is invalid.</exception>
    public async IAsyncEnumerable<ComprobanteResult> GetComprobantesAsync(DownloadResponse downloadResponse,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var packageBytes = MaterializePackageBytes(downloadResponse);

        await foreach (var result in GetComprobantesAsync(packageBytes, cancellationToken))
        {
            yield return result;
        }
    }


    /// <summary>
    /// Retrieves per-line metadata results from a package represented by its .zip file path.
    /// A single malformed line never aborts the enumeration; only an unreadable ZIP
    /// throws <see cref="CorruptPackageException"/>.
    /// </summary>
    /// <param name="fullFilePath">Package .zip file path</param>
    /// <param name="extractToPath">Path where to extract the zip file</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>A <see cref="IAsyncEnumerable{MetaItemResult}"/> of per-line results.</returns>
    /// <exception cref="CorruptPackageException">The whole ZIP is unreadable (corrupt or empty).</exception>
    public async IAsyncEnumerable<MetaItemResult> GetMetadataAsync(string fullFilePath, string extractToPath,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        ExtractZipFileOrThrow(fullFilePath, extractToPath, cancellationToken);

        // Load file details - look for .txt files instead of XML
        var files = _storageService.GetFiles(extractToPath, FileStorageSettings.MetaExtension);

        foreach (var file in files)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var (txtContent, readError) = await TryReadFileAsync(file.FilePath, cancellationToken);

            if (txtContent is null)
            {
                _logger.LogWarning("No se pudo leer el archivo {File}: {Error}", file.FilePath, readError);
                yield return new MetaItemResult
                {
                    SourceName = file.FilePath,
                    ErrorMessage = readError
                };
                continue;
            }

            foreach (var result in ParseMetadataLines(txtContent, file.FilePath))
            {
                cancellationToken.ThrowIfCancellationRequested();
                yield return result;
            }
        }
    }

    /// <summary>
    /// Retrieves per-line metadata results from a package represented by its byte array.
    /// A single malformed line never aborts the enumeration; only an unreadable ZIP
    /// throws <see cref="CorruptPackageException"/>.
    /// </summary>
    /// <param name="packageBytes">Package Bytes</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>A <see cref="IAsyncEnumerable{MetaItemResult}"/> of per-line results.</returns>
    /// <exception cref="CorruptPackageException">The whole ZIP is unreadable (corrupt or empty).</exception>
    public async IAsyncEnumerable<MetaItemResult> GetMetadataAsync(byte[] packageBytes,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        using var zipArchive = OpenZipArchive(packageBytes);

        foreach (var entry in zipArchive.Entries)
        {
            cancellationToken.ThrowIfCancellationRequested();

            // Verify if the entry is a TXT file
            if (!entry.Name.EndsWith(FileStorageSettings.MetaExtension, StringComparison.OrdinalIgnoreCase))
                continue;

            var (txtContent, readError) = await TryReadEntryAsync(entry, cancellationToken);

            if (txtContent is null)
            {
                _logger.LogWarning("No se pudo leer la entrada {Entry} del paquete: {Error}",
                    entry.FullName, readError);
                yield return new MetaItemResult
                {
                    SourceName = entry.FullName,
                    ErrorMessage = readError
                };
                continue;
            }

            foreach (var result in ParseMetadataLines(txtContent, entry.FullName))
            {
                cancellationToken.ThrowIfCancellationRequested();
                yield return result;
            }
        }
    }

    /// <summary>
    /// Retrieves per-line metadata results from a package represented by its DownloadResponse.
    /// A single malformed line never aborts the enumeration.
    /// </summary>
    /// <param name="downloadResponse">DownloadResponse</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>A <see cref="IAsyncEnumerable{MetaItemResult}"/> of per-line results.</returns>
    /// <exception cref="CorruptPackageException">The ZIP is unreadable, empty or the base64 is invalid.</exception>
    public async IAsyncEnumerable<MetaItemResult> GetMetadataAsync(DownloadResponse downloadResponse,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var packageBytes = MaterializePackageBytes(downloadResponse);

        await foreach (var result in GetMetadataAsync(packageBytes, cancellationToken))
        {
            yield return result;
        }
    }

    /// <summary>
    /// Opens the package as a ZipArchive. Empty or corrupt packages are a terminal failure
    /// of the whole package, signaled with <see cref="CorruptPackageException"/>.
    /// Extracted to a non-iterator method because C# forbids yield inside try/catch.
    /// </summary>
    private ZipArchive OpenZipArchive(byte[] packageBytes)
    {
        ArgumentNullException.ThrowIfNull(packageBytes);

        if (packageBytes.Length == 0)
        {
            _logger.LogError("El paquete está vacío (0 bytes): no existe un ZIP que abrir");
            throw new CorruptPackageException(0,
                "El paquete está vacío (0 bytes): el SAT no entregó contenido.");
        }

        try
        {
            return new ZipArchive(new MemoryStream(packageBytes), ZipArchiveMode.Read);
        }
        catch (InvalidDataException ex)
        {
            _logger.LogError(ex, "El paquete ZIP está corrupto ({PackageSize} bytes)", packageBytes.Length);
            throw new CorruptPackageException(packageBytes.Length,
                "El paquete ZIP está corrupto o no es un archivo ZIP válido.", ex);
        }
    }

    /// <summary>
    /// Extracts the package zip converting an unreadable ZIP into <see cref="CorruptPackageException"/>.
    /// </summary>
    private void ExtractZipFileOrThrow(string fullFilePath, string extractToPath,
        CancellationToken cancellationToken)
    {
        try
        {
            _storageService.ExtractZipFile(
                fullFilePath: fullFilePath,
                extractToPath: extractToPath,
                logger: _logger,
                cancellationToken: cancellationToken
            );
        }
        catch (InvalidDataException ex)
        {
            long packageSize = 0;
            try
            {
                packageSize = new FileInfo(fullFilePath).Length;
            }
            catch (Exception)
            {
                // best effort: the size is diagnostic only
            }

            _logger.LogError(ex, "El paquete ZIP {File} está corrupto ({PackageSize} bytes)",
                fullFilePath, packageSize);
            throw new CorruptPackageException(packageSize,
                "El paquete ZIP está corrupto o no es un archivo ZIP válido.", ex);
        }
    }

    /// <summary>
    /// Reads a single zip entry. A bad CRC or IO error on one entry yields a failed result
    /// instead of killing the whole package.
    /// </summary>
    private static async Task<(string? Content, string? Error)> TryReadEntryAsync(ZipArchiveEntry entry,
        CancellationToken cancellationToken)
    {
        try
        {
            await using var entryStream = entry.Open();
            using var reader = new StreamReader(entryStream);
            return (await reader.ReadToEndAsync(cancellationToken), null);
        }
        catch (Exception ex) when (ex is InvalidDataException or IOException)
        {
            return (null, $"No se pudo leer la entrada del ZIP: {ex.Message}");
        }
    }

    /// <summary>
    /// Reads a file from disk protecting the enumeration from IO failures on a single file.
    /// </summary>
    private async Task<(string? Content, string? Error)> TryReadFileAsync(string filePath,
        CancellationToken cancellationToken)
    {
        try
        {
            return (await _storageService.ReadFileContentAsync(filePath, cancellationToken, _logger), null);
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            return (null, $"No se pudo leer el archivo: {ex.Message}");
        }
    }

    /// <summary>
    /// Parses one CFDI logging failures and warnings. Never throws.
    /// </summary>
    private ComprobanteResult ParseComprobante(string xml, string sourceName)
    {
        var result = ComprobanteParser.Parse(xml, sourceName);

        if (!result.Succeeded)
            _logger.LogWarning("CFDI ilegible en {Source}: {Error}", sourceName, result.ErrorMessage);
        else if (result.Warnings.Count > 0)
            _logger.LogWarning("CFDI {Source} con advertencias: {Warnings}",
                sourceName, string.Join(" | ", result.Warnings));

        return result;
    }

    /// <summary>
    /// Parses the lines of a metadata .txt. Line 1 is the header; LineNumber is the physical
    /// 1-based line within the file. Malformed lines yield failed results.
    /// </summary>
    private IEnumerable<MetaItemResult> ParseMetadataLines(string txtContent, string sourceName)
    {
        // Split WITHOUT removing empty entries so LineNumber matches the physical line.
        var lines = txtContent.Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.None);

        // Skip the header line (first line contains column names)
        for (var i = 1; i < lines.Length; i++)
        {
            var line = lines[i];

            if (string.IsNullOrWhiteSpace(line))
                continue;

            if (MetaItem.TryCreateFromString(line, out var metaItem, out var error))
            {
                metaItem!.Base64Content = line.EncodeToBase64();
                yield return new MetaItemResult
                {
                    SourceName = sourceName,
                    LineNumber = i + 1,
                    MetaItem = metaItem,
                    RawLine = line
                };
            }
            else
            {
                _logger.LogWarning("Línea de metadata inválida en {Source} línea {Line}: {Error}",
                    sourceName, i + 1, error);
                yield return new MetaItemResult
                {
                    SourceName = sourceName,
                    LineNumber = i + 1,
                    RawLine = line,
                    ErrorMessage = error
                };
            }
        }
    }

    /// <summary>
    /// Materializes the package bytes ONCE (base64 decode is expensive on SAT packages).
    /// Invalid base64 is a terminal failure of the whole package.
    /// </summary>
    private static byte[] MaterializePackageBytes(DownloadResponse downloadResponse)
    {
        if (downloadResponse is null)
        {
            throw new ArgumentNullException(nameof(downloadResponse), "Download response cannot be null.");
        }

        try
        {
            return downloadResponse.PackageBytes;
        }
        catch (FormatException ex)
        {
            throw new CorruptPackageException(0,
                "El contenido de Base64Package no es base64 válido.", ex);
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