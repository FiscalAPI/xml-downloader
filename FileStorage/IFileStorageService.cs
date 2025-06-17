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

namespace Fiscalapi.XmlDownloader.FileStorage;

/// <summary>
/// Interface for cross-platform file system operations
/// Designed to support multiple implementations like local file system, cloud storage, etc.
/// </summary>
public interface IFileStorageService
{
    /// <summary>
    /// Check if a file exists at the specified path
    /// </summary>
    bool FileExists(string filePath);

    /// <summary>
    /// Check if a directory exists at the specified path
    /// </summary>
    bool DirectoryExists(string directoryPath);

    /// <summary>
    /// Delete all files and folders recursively from the specified directory
    /// </summary>
    Task CleanDirectoryAsync(string directoryPath, CancellationToken cancellationToken = default);

    /// <summary>
    /// Extract a ZIP file to the specified destination path or default directory if not specified
    /// </summary>
    /// <param name="zipFilePath">Package .zip path</param>
    /// <param name="extractToPath">Path to write unzip files</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>Completion Task</returns>
    Task ExtractZipFileAsync(string zipFilePath, string? extractToPath = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Ensure required directories exist based on configuration settings
    /// </summary>
    Task EnsureDirectoriesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Get a list of files from the specified directory with optional file extension filter
    /// </summary>
    Task<List<FileDetails>> GetFilesAsync(string directoryPath, string? fileExtension = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Write binary data to a file
    /// </summary>
    Task WriteFileAsync(string filePath, byte[] data, CancellationToken cancellationToken = default);

    /// <summary>
    /// Write base64 encoded data to a file
    /// </summary>
    Task WriteFileAsync(string filePath, string base64Data, CancellationToken cancellationToken = default);

    /// <summary>
    /// Read file content as byte array
    /// </summary>
    Task<byte[]> ReadFileAsync(string filePath, CancellationToken cancellationToken = default);

    /// <summary>
    /// Read file content as string
    /// </summary>
    Task<string> ReadFileContentAsync(string filePath, CancellationToken cancellationToken = default);

    /// <summary>
    /// Copy a file from source to destination
    /// </summary>
    Task<bool> CopyFileAsync(string sourceFilePath, string destinationFilePath, bool overwrite = false,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Move a file from source to destination
    /// </summary>
    Task<bool> MoveFileAsync(string sourceFilePath, string destinationFilePath,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete a file
    /// </summary>
    Task<bool> DeleteFileAsync(string filePath, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get file size in bytes
    /// </summary>
    Task<long> GetFileSizeAsync(string filePath, CancellationToken cancellationToken = default);

    /// <summary>
    /// Create a directory if it doesn't exist
    /// </summary>
    Task CreateDirectoryAsync(string directoryPath, CancellationToken cancellationToken = default);
}