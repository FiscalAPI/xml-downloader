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


namespace Fiscalapi.XmlDownloader.Common;

/// <summary>
/// El paquete ZIP completo es ilegible: archivo vacío (0 bytes), base64 corrupto o
/// central directory dañado. Es un fallo terminal del paquete, distinto de los fallos
/// por documento que se reportan como resultados con <c>Succeeded = false</c>.
/// </summary>
public class CorruptPackageException : Exception
{
    /// <summary>
    /// Tamaño en bytes del paquete que no pudo abrirse.
    /// </summary>
    public long PackageSize { get; }

    public CorruptPackageException(long packageSize, string message, Exception? innerException = null)
        : base(message, innerException)
    {
        PackageSize = packageSize;
    }
}
