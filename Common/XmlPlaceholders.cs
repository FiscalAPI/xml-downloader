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
/// Placeholders used in XML templates for SAT web services.
/// </summary>
public static class XmlPlaceholders
{
    #region Common Placeholders

    public const string Base64Cer = $"___{nameof(Base64Cer)}___";
    public const string Base64Digested = $"___{nameof(Base64Digested)}___";
    public const string Base64Signature = $"___{nameof(Base64Signature)}___";
    public const string X509IssuerName = $"___{nameof(X509IssuerName)}___";
    public const string X509SerialNumber = $"___{nameof(X509SerialNumber)}___";

    public const string SignedInfo = $"___{nameof(SignedInfo)}___";
    public const string KeyInfo = $"___{nameof(KeyInfo)}___";
    public const string SignatureData = $"___{nameof(SignatureData)}___";
    public const string ReferenceUri = $"___{nameof(ReferenceUri)}___";

    #endregion

    #region Auth Specific Placeholders

    public const string Created = $"___{nameof(Created)}___";
    public const string Expires = $"___{nameof(Expires)}___";
    public const string Uuid = $"___{nameof(Uuid)}___";

    #endregion

    #region Query Specific Placeholders

    public const string RequestAttributes = $"___{nameof(RequestAttributes)}___";
    public const string ExtraXml = $"___{nameof(ExtraXml)}___";
    public const string NodeName = $"___{nameof(NodeName)}___";
    public const string RecipientTin = $"___{nameof(RecipientTin)}___";
    public const string RfcReceptorItems = $"___{nameof(RfcReceptorItems)}___";

    #endregion

    #region Verify Service Placeholders

    public const string RequestId = $"___{nameof(RequestId)}___";
    public const string RequesterTin = $"___{nameof(RequesterTin)}___";

    #endregion

    #region Download Service Placeholders

    public const string PackageId = $"___{nameof(PackageId)}___";

    #endregion
}