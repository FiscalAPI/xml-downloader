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

using System.Runtime.InteropServices;

namespace Fiscalapi.XmlDownloader.FileStorage;

/// <summary>
/// Configuration settings for file management operations
/// </summary>
public class FileStorageSettings
{
    /// <summary>
    /// Package file extension
    /// </summary>
    public string PackageExtension { get; set; } = ".zip";

    /// <summary>
    /// XML file extension for CFDI documents
    /// </summary>
    public string CfdiExtension { get; set; } = ".xml";

    /// <summary>
    /// Metadata file extension
    /// </summary>
    public string MetadataExtension { get; set; } = ".txt";

    /// <summary>
    /// Folder to perform temporary operations like folder extractions
    /// </summary>
    public string TempDirectory { get; set; } = string.Empty;

    /// <summary>
    /// Folder where packages will be saved
    /// </summary>
    public string PackagesDirectory { get; set; } = string.Empty;

    /// <summary>
    /// Initialize default directories based on current operating system 
    /// </summary>
    public void InitializeDefaultDirectories()
    {
        if (string.IsNullOrEmpty(TempDirectory))
        {
            //Windows: C:\Users\[Username]\AppData\Local\Fiscalapi\Temp
            //Linux  : /home/[username]/.fiscalapi/temp
            TempDirectory = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Fiscalapi",
                    "Temp")
                : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".fiscalapi", "temp");
        }

        if (string.IsNullOrEmpty(PackagesDirectory))
        {
            //Windows: C:\Users\[Username]\AppData\Local\Fiscalapi\Packages
            //Linux  : /home/[username]/.fiscalapi/packages
            PackagesDirectory = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Fiscalapi",
                    "Packages")
                : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".fiscalapi",
                    "packages");
        }
    }
}