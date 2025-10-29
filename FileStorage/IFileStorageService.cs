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

using Microsoft.Extensions.Logging;

namespace Fiscalapi.XmlDownloader.FileStorage;

/// <summary>
/// Interface for file system operations.
/// </summary>
public interface IFileStorageService
{
    /// <summary>
    /// Check if a directory exists at the specified path
    /// </summary>
    /// <param name="directoryPath">Full path to the directory</param>
    /// <returns>True if directory exists</returns>
    bool DirectoryExists(string directoryPath);

    /// <summary>
    /// Delete all files and folders recursively from the specified directory
    /// </summary>
    /// <param name="directoryPath">Full path to the directory to clean</param>
    /// <param name="cancellationToken">Cancellation token</param>
    void CleanDirectory(string directoryPath, CancellationToken cancellationToken = default);

    /// <summary>
    /// Create a directory if it doesn't exist
    /// </summary>
    /// <param name="directoryPath">Full path to the directory to create</param>
    void CreateDirectoryIfNotExist(string directoryPath);

    /// <summary>
    /// Check if a file exists at the specified path
    /// </summary>
    /// <param name="fullFilePath">Full path to the file</param>
    /// <returns>True if file exists</returns>
    bool FileExists(string fullFilePath);

    /// <summary>
    /// Extract a ZIP file to the specified destination path
    /// </summary>
    /// <param name="fullFilePath">Full path to the ZIP file</param>
    /// <param name="extractToPath">Path to extract files to</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <param name="logger">Logger</param>
    /// <returns>Completion Task</returns>
    void ExtractZipFile(string fullFilePath, string extractToPath,
        CancellationToken cancellationToken = default, ILogger? logger = null);

    /// <summary>
    /// Get a list of files from the specified directory with optional file extension filter
    /// </summary>
    /// <param name="directoryPath">Full path to the directory</param>
    /// <param name="fileExtension">File extension filter (optional)</param>
    /// <returns>List of file details</returns>
    List<FileDetails> GetFiles(string directoryPath, string? fileExtension = null);

    /// <summary>
    /// Write binary data to a file
    /// </summary>
    /// <param name="fullFilePath">Full path to the file</param>
    /// <param name="data">Binary data to write</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <param name="logger">Logger</param>
    Task WriteFileAsync(string fullFilePath, byte[] data, CancellationToken cancellationToken = default,
        ILogger? logger = null);

    /// <summary>
    /// Write base64 encoded data to a file
    /// </summary>
    /// <param name="fullFilePath">Full path to the file</param>
    /// <param name="base64Data">Base64 encoded data to write</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <param name="logger">Logger</param>
    Task WriteFileAsync(string fullFilePath, string base64Data, CancellationToken cancellationToken = default,
        ILogger? logger = null);

    /// <summary>
    /// Read file content as byte array
    /// </summary>
    /// <param name="fullFilePath">Full path to the file</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <param name="logger">Logger</param>
    /// <returns>File content as byte array</returns>
    Task<byte[]> ReadFileAsync(string fullFilePath, CancellationToken cancellationToken = default,
        ILogger? logger = null);

    /// <summary>
    /// Read file content as string
    /// </summary>
    /// <param name="fullFilePath">Full path to the file</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <param name="logger">Logger</param>
    /// <returns>File content as string</returns>
    Task<string> ReadFileContentAsync(string fullFilePath, CancellationToken cancellationToken = default,
        ILogger? logger = null);

    /// <summary>
    /// Delete a file
    /// </summary>
    /// <param name="fullFilePath">Full path to the file</param>
    /// <returns>True if file was deleted successfully</returns>
    void DeleteFile(string fullFilePath);
}