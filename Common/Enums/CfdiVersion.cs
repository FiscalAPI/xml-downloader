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


namespace Fiscalapi.XmlDownloader.Common.Enums;

/// <summary>
/// Versión del CFDI detectada a partir del namespace del elemento raíz del XML.
/// </summary>
public enum CfdiVersion
{
    /// <summary>
    /// Namespace raíz no reconocido: XML malformado, Retenciones u otro documento que no es CFDI 3.3/4.0.
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// CFDI 3.3 (namespace http://www.sat.gob.mx/cfd/3). Vigente hasta el 31-mar-2023;
    /// presente de forma garantizada en descargas masivas históricas.
    /// </summary>
    Cfdi33 = 33,

    /// <summary>
    /// CFDI 4.0 (namespace http://www.sat.gob.mx/cfd/4).
    /// </summary>
    Cfdi40 = 40
}
