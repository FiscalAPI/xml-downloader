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
using Fiscalapi.Credentials.Core;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using XmlDownloader.Auth;
using XmlDownloader.Auth.Models;
using XmlDownloader.Common;
using XmlDownloader.Common.Models;
using XmlDownloader.Download;
using XmlDownloader.Download.Models;
using XmlDownloader.FileStorage;
using XmlDownloader.Query;
using XmlDownloader.Query.Models;
using XmlDownloader.Verify;
using XmlDownloader.Verify.Models;

namespace XmlDownloader;

public class XmlDownloaderService : IXmlDownloaderService
{
    public bool IsDebugEnabled { get; set; }
    public Token? Token { get; set; }
    public ICredential? Credential { get; set; }

    private readonly ILogger<XmlDownloaderService> _logger;
    private readonly FileStorageSettings _settings;
    private readonly IAuthService _authService;
    private readonly IQueryService _queryService;
    private readonly IVerifyService _verifyService;
    private readonly IDownloadService _downloadService;
    private readonly IFileStorageService _storageService;

    // Default constructor for dependency injection
    public XmlDownloaderService(
        IAuthService authService,
        IQueryService queryService,
        IVerifyService verifyService,
        IDownloadService downloadService,
        IFileStorageService storageService,
        IOptions<FileStorageSettings> options,
        ILogger<XmlDownloaderService> logger
    )
    {
        _authService = authService;
        _queryService = queryService;
        _verifyService = verifyService;
        _downloadService = downloadService;
        _storageService = storageService;
        _settings = options.Value;
        _logger = logger;
    }

    // Default constructor no dependency injection 
    public XmlDownloaderService()
    {
        _authService = new AuthService();
        _queryService = new QueryService();
        _verifyService = new VerifyService();
        _downloadService = new DownloadService();
        _storageService = new FileStorageService();
        _settings = new FileStorageSettings();
        _settings.InitializeDefaultDirectories();

        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole().SetMinimumLevel(LogLevel.Information);
        });

        _logger = loggerFactory.CreateLogger<XmlDownloaderService>();
    }


    public async Task<AuthResponse> AuthenticateAsync(string base64Cer, string base64Key, string password,
        CancellationToken cancellationToken = default)
    {
        ICertificate certificate = new Certificate(base64Cer);
        IPrivateKey privateKey = new PrivateKey(base64Key, password);
        ICredential credential = new Credential(certificate, privateKey);

        Credential = credential;

        return await AuthenticateAsync(credential, cancellationToken);
    }


    public async Task<AuthResponse> AuthenticateAsync(ICredential credential,
        CancellationToken cancellationToken = default)
    {
        var authResponse = await _authService.AuthenticateAsync(credential, cancellationToken);

        Token = authResponse.Token;
        Credential = credential;
        return authResponse;
    }

    public async Task<QueryResponse> CreateRequestAsync(QueryParameters parameters,
        CancellationToken cancellationToken = default)
    {
        EnsureAuthToken();

        return await _queryService.CreateAsync(
            credential: Credential!,
            authToken: Token!,
            parameters: parameters,
            cancellationToken: cancellationToken
        );
    }

    public Task<VerifyResponse> VerifyAsync(string requestId, CancellationToken cancellationToken = default)
    {
        EnsureAuthToken();

        return _verifyService.VerifyAsync(
            credential: Credential!,
            authToken: Token!,
            requestId: requestId,
            cancellationToken: cancellationToken
        );
    }

    public Task<DownloadResponse> DownloadAsync(string packageId, CancellationToken cancellationToken = default)
    {
        EnsureAuthToken();

        return _downloadService.DownloadAsync(
            credential: Credential!,
            authToken: Token!,
            packageId: packageId,
            cancellationToken: cancellationToken
        );
    }

    public async Task WritePackageAsync(string path, byte[] bytes, CancellationToken cancellationToken = default)
    {
        await _storageService.WriteFileAsync(path, bytes, cancellationToken);
    }

    public async Task WritePackageAsync(string path, string base64Package,
        CancellationToken cancellationToken = default)
    {
        await _storageService.WriteFileAsync(path, base64Package, cancellationToken);
    }

    public async Task<byte[]> ReadFileAsync(string filePath, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return await _storageService.ReadFileAsync(filePath, cancellationToken);
    }


    public async IAsyncEnumerable<Comprobante> GetComprobantesAsync(string zipFilePath,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        //Unzip the package to a default directory
        await _storageService.ExtractZipFileAsync(
            zipFilePath: zipFilePath,
            extractToPath: _settings.TempDirectory,
            cancellationToken: cancellationToken
        );

        //Load file details 
        var files = await _storageService.GetFilesAsync(
            _settings.TempDirectory,
            _settings.CfdiExtension,
            cancellationToken);

        //Serialize each XML file to Comprobante object
        foreach (var file in files)
        {
            cancellationToken.ThrowIfCancellationRequested();


            var xml = await _storageService.ReadFileContentAsync(file.FilePath, cancellationToken);


            var comprobante = XmlSerializerService.Deserialize<Comprobante>(xml);

            if (comprobante is not null)
                yield return comprobante;
        }
    }

    public async IAsyncEnumerable<Comprobante> GetComprobantesAsync(byte[] packageBytes,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        using var memoryStream = new MemoryStream(packageBytes);
        using var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Read);

        foreach (var entry in zipArchive.Entries)
        {
            cancellationToken.ThrowIfCancellationRequested();

            //verify if the entry is an XML file
            if (!entry.Name.EndsWith(_settings.CfdiExtension, StringComparison.OrdinalIgnoreCase))
                continue;

            // Read the entry stream
            await using var entryStream = entry.Open();
            using var reader = new StreamReader(entryStream);

            var xml = await reader.ReadToEndAsync(cancellationToken);

            var comprobante = XmlSerializerService.Deserialize<Comprobante>(xml);

            if (comprobante is null) continue;
            comprobante.DeserializeComplements();
            yield return comprobante;
        }
    }

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


    private void EnsureAuthToken()
    {
        if (Token == null)
        {
            throw new InvalidOperationException("Authentication token is required. Please authenticate first.");
        }

        if (Credential == null)
        {
            throw new InvalidOperationException("Credential is required. Please authenticate first.");
        }
    }
}