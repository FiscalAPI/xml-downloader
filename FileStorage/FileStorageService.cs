using System.IO.Compression;

namespace Fiscalapi.XmlDownloader.FileStorage
{
    /// <summary>
    /// Implementation I/O local file system operations.
    /// </summary>
    public class FileStorageService : IFileStorageService
    {
        /// <summary>
        /// Check if directory exists using synchronous Directory.Exists
        /// </summary>
        /// <param name="directoryPath">Full path to the directory</param>
        /// <returns>True if directory exists</returns>
        public bool DirectoryExists(string directoryPath)
        {
            return Directory.Exists(directoryPath);
        }

        /// <summary>
        /// Clean directory contents synchronously - no native async APIs available
        /// </summary>
        /// <param name="directoryPath">Full path to the directory to clean</param>
        /// <param name="cancellationToken">Cancellation token</param>
        public void CleanDirectory(string directoryPath, CancellationToken cancellationToken = default)
        {
            if (!Directory.Exists(directoryPath))
                return;

            var directoryInfo = new DirectoryInfo(directoryPath);

            // Delete all files
            foreach (var file in directoryInfo.GetFiles())
            {
                cancellationToken.ThrowIfCancellationRequested();
                file.Delete();
            }

            // Delete all subdirectories
            foreach (var dir in directoryInfo.GetDirectories())
            {
                cancellationToken.ThrowIfCancellationRequested();
                dir.Delete(true);
            }
        }

        /// <summary>
        /// Create directory synchronously using Directory.CreateDirectory
        /// </summary>
        /// <param name="directoryPath">Full path to the directory to create</param>
        public void CreateDirectoryIfNotExist(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }

        /// <summary>
        /// Check if file exists using synchronous File.Exists
        /// </summary>
        /// <param name="fullFilePath">Full path to the file</param>
        /// <returns>True if file exists</returns>
        public bool FileExists(string fullFilePath)
        {
            return File.Exists(fullFilePath);
        }

        /// <summary>
        /// Extract ZIP file asynchronously using ZipFile.ExtractToDirectory
        /// </summary>
        /// <param name="fullFilePath">Full path to the ZIP file</param>
        /// <param name="extractToPath">Path to extract files to</param>
        /// <param name="cancellationToken">Cancellation token</param>
        public void ExtractZipFile(string fullFilePath, string? extractToPath,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(fullFilePath))
                throw new ArgumentException("ZIP file path cannot be null or empty", nameof(fullFilePath));

            if (string.IsNullOrWhiteSpace(extractToPath))
                throw new ArgumentException("Extract path cannot be null or empty", nameof(extractToPath));

            if (!File.Exists(fullFilePath))
                throw new FileNotFoundException($"ZIP file not found: {fullFilePath}");


            using var archive = ZipFile.OpenRead(fullFilePath);
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
        }

        /// <summary>
        /// Get files from a directory with optional file extension filter.
        /// </summary>
        /// <param name="directoryPath">Full path to the directory</param>
        /// <param name="fileExtension">File extension filter (optional)</param>
        /// <returns>List of file details</returns>
        public List<FileDetails> GetFiles(string directoryPath, string? fileExtension = null)
        {
            if (!Directory.Exists(directoryPath))
                return [];

            var searchPattern = string.IsNullOrEmpty(fileExtension) ? "*.*" : $"*.{fileExtension.TrimStart('.')}";
            var files = Directory.GetFiles(directoryPath, searchPattern, SearchOption.TopDirectoryOnly);

            return files.Select(filePath =>
            {
                var fileInfo = new FileInfo(filePath);
                return new FileDetails
                {
                    FileName = fileInfo.Name,
                    FilePath = filePath,
                    CreatedAt = fileInfo.CreationTime,
                    ModifiedAt = fileInfo.LastWriteTime,
                    Size = fileInfo.Length
                };
            }).ToList();
        }

        /// <summary>
        /// Write binary data asynchronously
        /// </summary>
        /// <param name="fullFilePath">Full path to the file</param>
        /// <param name="data">Binary data to write</param>
        /// <param name="cancellationToken">Cancellation token</param>
        public async Task WriteFileAsync(string fullFilePath, byte[] data,
            CancellationToken cancellationToken = default)
        {
            var directory = Path.GetDirectoryName(fullFilePath);
            if (string.IsNullOrEmpty(directory) || !Directory.Exists(directory))
            {
                throw new DirectoryNotFoundException($"Directory does not exist: {directory}");
            }

            await File.WriteAllBytesAsync(fullFilePath, data, cancellationToken);
        }

        /// <summary>
        /// Write base64 data by decoding it and writing as binary data.
        /// </summary>
        /// <param name="fullFilePath">Full path to the file</param>
        /// <param name="base64Data">Base64 encoded data to write</param>
        /// <param name="cancellationToken">Cancellation token</param>
        public async Task WriteFileAsync(string fullFilePath, string base64Data,
            CancellationToken cancellationToken = default)
        {
            var data = Convert.FromBase64String(base64Data);
            await WriteFileAsync(fullFilePath, data, cancellationToken);
        }

        /// <summary>
        /// Read file as byte array asynchronously using File.ReadAllBytesAsync
        /// </summary>
        /// <param name="fullFilePath">Full path to the file</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>File content as byte array</returns>
        public async Task<byte[]> ReadFileAsync(string fullFilePath, CancellationToken cancellationToken = default)
        {
            return await File.ReadAllBytesAsync(fullFilePath, cancellationToken);
        }

        /// <summary>
        /// Read file content as string asynchronously using File.ReadAllTextAsync
        /// </summary>
        /// <param name="fullFilePath">Full path to the file</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>File content as string</returns>
        public async Task<string> ReadFileContentAsync(string fullFilePath,
            CancellationToken cancellationToken = default)
        {
            return await File.ReadAllTextAsync(fullFilePath, cancellationToken);
        }

        /// <summary>
        /// Delete file synchronously using File.Delete - no async version available
        /// </summary>
        /// <param name="fullFilePath">Full path to the file</param>
        /// <returns>True if file was deleted successfully</returns>
        public void DeleteFile(string fullFilePath)
        {
            if (File.Exists(fullFilePath))
                File.Delete(fullFilePath);
        }
    }
}