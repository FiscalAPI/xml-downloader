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
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Fiscalapi.XmlDownloader.FileStorage;

/// <summary>
/// Local file system implementation
/// </summary>
public class FileStorageService : IFileStorageService, IDisposable
{
    private readonly ILogger<FileStorageService> _logger;
    private readonly FileStorageSettings _settings;

    // Dependency injection constructor
    public FileStorageService(ILogger<FileStorageService> logger, IOptions<FileStorageSettings> settings)
    {
        _settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _settings.InitializeDefaultDirectories();
    }

    // No dependency injection constructor
    public FileStorageService()
    {
        _settings = new FileStorageSettings();
        _settings.InitializeDefaultDirectories();
        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole().SetMinimumLevel(LogLevel.Information);
        });

        _logger = loggerFactory.CreateLogger<FileStorageService>();
    }

    public bool FileExists(string filePath)
    {
        return !string.IsNullOrWhiteSpace(filePath) && File.Exists(filePath);
    }

    public bool DirectoryExists(string directoryPath)
    {
        return !string.IsNullOrWhiteSpace(directoryPath) && Directory.Exists(directoryPath);
    }

    public async Task CleanDirectoryAsync(string directoryPath, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(directoryPath))
            throw new ArgumentException("Directory path cannot be null or empty", nameof(directoryPath));

        if (!Directory.Exists(directoryPath))
        {
            _logger.LogWarning("Directory does not exist: {DirectoryPath}", directoryPath);
            return;
        }

        await Task.Run(() =>
        {
            var directoryInfo = new DirectoryInfo(directoryPath);

            // Delete all files
            foreach (var file in directoryInfo.GetFiles())
            {
                cancellationToken.ThrowIfCancellationRequested();
                file.Delete();
            }

            // Delete all subdirectories
            foreach (var subDir in directoryInfo.GetDirectories())
            {
                cancellationToken.ThrowIfCancellationRequested();
                subDir.Delete(true);
            }
        }, cancellationToken);

        _logger.LogInformation("Directory cleaned successfully: {DirectoryPath}", directoryPath);
    }

    public async Task ExtractZipFileAsync(string zipFilePath, string? extractToPath = null,
        CancellationToken cancellationToken = default)
    {
        extractToPath ??= _settings.TempDirectory;

        if (string.IsNullOrWhiteSpace(zipFilePath))
            throw new ArgumentException("ZIP file path cannot be null or empty", nameof(zipFilePath));

        if (string.IsNullOrWhiteSpace(extractToPath))
            throw new ArgumentException("Extract path cannot be null or empty", nameof(extractToPath));

        if (!File.Exists(zipFilePath))
            throw new FileNotFoundException($"ZIP file not found: {zipFilePath}");

        await CreateDirectoryAsync(extractToPath, cancellationToken);

        await Task.Run(() =>
        {
            using var archive = ZipFile.OpenRead(zipFilePath);
            foreach (var entry in archive.Entries)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var destinationPath = Path.Combine(extractToPath, entry.FullName);

                // Ensure the directory exists
                var directoryPath = Path.GetDirectoryName(destinationPath);
                if (!string.IsNullOrEmpty(directoryPath) && !Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                // Extract file if it's not a directory
                if (!string.IsNullOrEmpty(entry.Name))
                {
                    entry.ExtractToFile(destinationPath, overwrite: true);
                }
            }
        }, cancellationToken);

        _logger.LogInformation("ZIP file extracted successfully from {ZipFilePath} to {ExtractToPath}", zipFilePath,
            extractToPath);
    }

    public async Task EnsureDirectoriesAsync(CancellationToken cancellationToken = default)
    {
        var directories = new[]
        {
            _settings.TempDirectory,
            _settings.PackagesDirectory
        };

        foreach (var directory in directories)
        {
            if (!string.IsNullOrWhiteSpace(directory))
            {
                await CreateDirectoryAsync(directory, cancellationToken);
            }
        }

        _logger.LogInformation("Required directories ensured successfully");
    }

    public async Task<List<FileDetails>> GetFilesAsync(string directoryPath, string? fileExtension = null,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(directoryPath))
            throw new ArgumentException("Directory path cannot be null or empty", nameof(directoryPath));

        if (!Directory.Exists(directoryPath))
            return [];

        return await Task.Run(() =>
        {
            var files = new List<FileDetails>();
            var directoryInfo = new DirectoryInfo(directoryPath);

            var searchPattern = string.IsNullOrWhiteSpace(fileExtension) ? "*" : $"*{fileExtension}";

            foreach (var file in directoryInfo.GetFiles(searchPattern, SearchOption.TopDirectoryOnly))
            {
                cancellationToken.ThrowIfCancellationRequested();

                files.Add(new FileDetails
                {
                    FileName = file.Name,
                    FilePath = file.FullName,
                    Size = file.Length,
                    CreatedAt = file.CreationTime,
                    ModifiedAt = file.LastWriteTime
                });
            }

            return files;
        }, cancellationToken);
    }

    public async Task WriteFileAsync(string filePath, byte[] data, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("File path cannot be null or empty", nameof(filePath));

        if (data == null)
            throw new ArgumentNullException(nameof(data));

        var directoryPath = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrEmpty(directoryPath))
        {
            await CreateDirectoryAsync(directoryPath, cancellationToken);
        }

        await File.WriteAllBytesAsync(filePath, data, cancellationToken);
        _logger.LogDebug("File written successfully: {FilePath}, Size: {Size} bytes", filePath, data.Length);
    }

    public async Task WriteFileAsync(string filePath, string base64Data,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("File path cannot be null or empty", nameof(filePath));

        if (string.IsNullOrWhiteSpace(base64Data))
            throw new ArgumentException("Base64 data cannot be null or empty", nameof(base64Data));

        var data = Convert.FromBase64String(base64Data);
        await WriteFileAsync(filePath, data, cancellationToken);
    }

    public async Task<byte[]> ReadFileAsync(string filePath, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("File path cannot be null or empty", nameof(filePath));

        if (!File.Exists(filePath))
            throw new FileNotFoundException($"File not found: {filePath}");

        return await File.ReadAllBytesAsync(filePath, cancellationToken);
    }

    public async Task<string> ReadFileContentAsync(string filePath, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("File path cannot be null or empty", nameof(filePath));

        if (!File.Exists(filePath))
            throw new FileNotFoundException($"File not found: {filePath}");

        return await File.ReadAllTextAsync(filePath, Encoding.UTF8, cancellationToken);
    }

    public async Task<bool> CopyFileAsync(string sourceFilePath, string destinationFilePath, bool overwrite = false,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(sourceFilePath))
            throw new ArgumentException("Source file path cannot be null or empty", nameof(sourceFilePath));

        if (string.IsNullOrWhiteSpace(destinationFilePath))
            throw new ArgumentException("Destination file path cannot be null or empty",
                nameof(destinationFilePath));

        if (!File.Exists(sourceFilePath))
            return false;

        var destinationDirectory = Path.GetDirectoryName(destinationFilePath);
        if (!string.IsNullOrEmpty(destinationDirectory))
        {
            await CreateDirectoryAsync(destinationDirectory, cancellationToken);
        }

        await Task.Run(() => File.Copy(sourceFilePath, destinationFilePath, overwrite), cancellationToken);
        _logger.LogDebug("File copied successfully from {SourcePath} to {DestinationPath}", sourceFilePath,
            destinationFilePath);
        return true;
    }

    public async Task<bool> MoveFileAsync(string sourceFilePath, string destinationFilePath,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(sourceFilePath))
            throw new ArgumentException("Source file path cannot be null or empty", nameof(sourceFilePath));

        if (string.IsNullOrWhiteSpace(destinationFilePath))
            throw new ArgumentException("Destination file path cannot be null or empty",
                nameof(destinationFilePath));

        if (!File.Exists(sourceFilePath))
            return false;

        var destinationDirectory = Path.GetDirectoryName(destinationFilePath);
        if (!string.IsNullOrEmpty(destinationDirectory))
        {
            await CreateDirectoryAsync(destinationDirectory, cancellationToken);
        }

        await Task.Run(() => File.Move(sourceFilePath, destinationFilePath), cancellationToken);
        _logger.LogDebug("File moved successfully from {SourcePath} to {DestinationPath}", sourceFilePath,
            destinationFilePath);
        return true;
    }

    public async Task<bool> DeleteFileAsync(string filePath, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            return false;

        if (!File.Exists(filePath))
            return true;

        await Task.Run(() => File.Delete(filePath), cancellationToken);
        _logger.LogDebug("File deleted successfully: {FilePath}", filePath);
        return true;
    }

    public async Task<long> GetFileSizeAsync(string filePath, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("File path cannot be null or empty", nameof(filePath));

        if (!File.Exists(filePath))
            throw new FileNotFoundException($"File not found: {filePath}");

        return await Task.Run(() =>
        {
            var fileInfo = new FileInfo(filePath);
            return fileInfo.Length;
        }, cancellationToken);
    }

    public async Task CreateDirectoryAsync(string directoryPath, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(directoryPath))
            return;

        if (Directory.Exists(directoryPath))
            return;

        await Task.Run(() => Directory.CreateDirectory(directoryPath), cancellationToken);
        _logger.LogDebug("Directory created successfully: {DirectoryPath}", directoryPath);
    }

    protected virtual void Dispose(bool disposing)
    {
        // Ya no hay recursos que liberar específicamente
        // Si en el futuro se agregan, se mantendrá este patrón
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}