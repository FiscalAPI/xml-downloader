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
/// XML envelope building blocks used for SAT web services.
/// </summary>
public static class XmlTemplates
{
    #region Comon Templates

    /// <summary>
    /// Template SignedInfo for Auth and Query
    /// </summary>
    public const string SignedInfo =
        $"""
         <SignedInfo xmlns="http://www.w3.org/2000/09/xmldsig#">
             <CanonicalizationMethod Algorithm="http://www.w3.org/2001/10/xml-exc-c14n#"></CanonicalizationMethod>
             <SignatureMethod Algorithm="http://www.w3.org/2000/09/xmldsig#rsa-sha1"></SignatureMethod>
             <Reference URI="{XmlPlaceholders.ReferenceUri}">
                 <Transforms>
                     <Transform Algorithm="http://www.w3.org/2001/10/xml-exc-c14n#"></Transform>
                 </Transforms>
                 <DigestMethod Algorithm="http://www.w3.org/2000/09/xmldsig#sha1"></DigestMethod>
                 <DigestValue>{XmlPlaceholders.Base64Digested}</DigestValue>
             </Reference>
         </SignedInfo>
         """;


    /// <summary>
    /// Template para KeyInfo con X509Data for Query, Verify and Download.
    /// </summary>
    public const string X509KeyInfo =
        $"""
         <KeyInfo>
             <X509Data>
                 <X509IssuerSerial>
                     <X509IssuerName>{XmlPlaceholders.X509IssuerName}</X509IssuerName>
                     <X509SerialNumber>{XmlPlaceholders.X509SerialNumber}</X509SerialNumber>
                 </X509IssuerSerial>
                 <X509Certificate>{XmlPlaceholders.Base64Cer}</X509Certificate>
             </X509Data>
         </KeyInfo>
         """;

    /// <summary>
    /// Template base para Signature for all services.
    /// </summary>
    public const string SignatureTemplate =
        $"""
         <Signature xmlns="http://www.w3.org/2000/09/xmldsig#">
             {XmlPlaceholders.SignedInfo}
             <SignatureValue>{XmlPlaceholders.Base64Signature}</SignatureValue>
             {XmlPlaceholders.KeyInfo}
         </Signature>
         """;

    #endregion

    #region Service 1: Auth Templates

    /// <summary>
    /// Template timestamp for Auth.
    /// </summary>
    public const string TimestampTemplate =
        $"""
         <u:Timestamp xmlns:u="http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd" u:Id="_0">
             <u:Created>{XmlPlaceholders.Created}</u:Created>
             <u:Expires>{XmlPlaceholders.Expires}</u:Expires>
         </u:Timestamp>
         """;

    /// <summary>
    /// Template  BinarySecurityToken for Auth.
    /// </summary>
    public const string BinarySecurityToken =
        $"""
         <o:BinarySecurityToken u:Id="{XmlPlaceholders.Uuid}"
                               ValueType="http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509v3"
                               EncodingType="http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary">
             {XmlPlaceholders.Base64Cer}
         </o:BinarySecurityToken>
         """;

    /// <summary>
    /// Template KeyInfo with SecurityTokenReference for Auth.
    /// </summary>
    public const string SecurityTokenReferenceKeyInfo =
        $"""
         <KeyInfo>
             <o:SecurityTokenReference>
                 <o:Reference URI="#{XmlPlaceholders.Uuid}" ValueType="http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509v3"/>
             </o:SecurityTokenReference>
         </KeyInfo>
         """;

    /// <summary>
    /// Template completo para el envelope de Autenticación
    /// </summary>
    public const string AuthEnvelope =
        $"""
         <s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/"
                     xmlns:u="http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd">
             <s:Header>
                 <o:Security xmlns:o="http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd" s:mustUnderstand="1">
                     {TimestampTemplate}
                     {BinarySecurityToken}
                     {XmlPlaceholders.SignatureData}
                 </o:Security>
             </s:Header>
             <s:Body>
                 <Autentica xmlns="http://DescargaMasivaTerceros.gob.mx"/>
             </s:Body>
         </s:Envelope>
         """;

    #endregion

    #region Service 2: Query Templates

    /// <summary>
    /// Template for RfcReceptores for 'SolicitaDescargaEmitidos'.
    /// </summary>
    public const string RfcReceptoresTemplate =
        $"""
         <des:RfcReceptores>
             {XmlPlaceholders.RfcReceptorItems}
         </des:RfcReceptores>
         """;

    /// <summary>
    /// Template for a single RfcReceptor
    /// </summary>
    public const string RfcReceptorTemplate =
        $"""
         <des:RfcReceptor>{XmlPlaceholders.RecipientTin}</des:RfcReceptor>
         """;

    /// <summary>
    /// Template para digest de SolicitaDescargaEmitidos
    /// </summary>
    public const string EmitidosToDigest =
        $"""
         <des:SolicitaDescargaEmitidos xmlns:des="http://DescargaMasivaTerceros.sat.gob.mx">
             <des:solicitud {XmlPlaceholders.RequestAttributes}>
                 {XmlPlaceholders.ExtraXml}
             </des:solicitud>
         </des:SolicitaDescargaEmitidos>
         """;

    /// <summary>
    /// Template para digest de SolicitaDescargaRecibidos
    /// </summary>
    public const string RecibidosToDigest =
        $"""
         <des:SolicitaDescargaRecibidos xmlns:des="http://DescargaMasivaTerceros.sat.gob.mx">
             <des:solicitud {XmlPlaceholders.RequestAttributes}>
                 {XmlPlaceholders.ExtraXml}
             </des:solicitud>
         </des:SolicitaDescargaRecibidos>
         """;

    /// <summary>
    /// Template para digest de SolicitaDescargaFolio
    /// </summary>
    public const string FolioToDigest =
        $"""
         <des:SolicitaDescargaFolio xmlns:des="http://DescargaMasivaTerceros.sat.gob.mx">
             <des:solicitud {XmlPlaceholders.RequestAttributes}>
                 {XmlPlaceholders.ExtraXml}
             </des:solicitud>
         </des:SolicitaDescargaFolio>
         """;

    /// <summary>
    /// Template genérico para envelope de Query
    /// </summary>
    public const string QueryEnvelope =
        $"""
         <s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/"
                     xmlns:des="http://DescargaMasivaTerceros.sat.gob.mx"
                     xmlns:xd="http://www.w3.org/2000/09/xmldsig#">
             <s:Header/>
             <s:Body>
                 <des:{XmlPlaceholders.NodeName}>
                     <des:solicitud {XmlPlaceholders.RequestAttributes}>
                         {XmlPlaceholders.ExtraXml}
                         {XmlPlaceholders.SignatureData}
                     </des:solicitud>
                 </des:{XmlPlaceholders.NodeName}>
             </s:Body>
         </s:Envelope>
         """;

    #endregion

    #region Service 3: Verify Templates

    /// <summary>
    /// Template para digest de VerificaSolicitudDescarga
    /// </summary>
    public const string VerifyToDigest =
        $"""
         <des:VerificaSolicitudDescarga xmlns:des="http://DescargaMasivaTerceros.sat.gob.mx">
             <des:solicitud IdSolicitud="{XmlPlaceholders.RequestId}" RfcSolicitante="{XmlPlaceholders.RequesterTin}"></des:solicitud>
         </des:VerificaSolicitudDescarga>
         """;

    /// <summary>
    /// Template para envelope de VerificaSolicitudDescarga
    /// </summary>
    public const string VerifyEnvelope =
        $"""
         <s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/"
                     xmlns:des="http://DescargaMasivaTerceros.sat.gob.mx"
                     xmlns:xd="http://www.w3.org/2000/09/xmldsig#">
             <s:Header/>
             <s:Body>
                 <des:VerificaSolicitudDescarga>
                     <des:solicitud IdSolicitud="{XmlPlaceholders.RequestId}" RfcSolicitante="{XmlPlaceholders.RequesterTin}">
                         {XmlPlaceholders.SignatureData}
                     </des:solicitud>
                 </des:VerificaSolicitudDescarga>
             </s:Body>
         </s:Envelope>
         """;

    #endregion

    #region Service 4: Download Templates

    /// <summary>
    /// DownloadToDigest Template para digest.
    /// </summary>
    public const string DownloadToDigest =
        $"""
         <des:PeticionDescargaMasivaTercerosEntrada xmlns:des="http://DescargaMasivaTerceros.sat.gob.mx">
             <des:peticionDescarga IdPaquete="{XmlPlaceholders.PackageId}" RfcSolicitante="{XmlPlaceholders.RequesterTin}"></des:peticionDescarga>
         </des:PeticionDescargaMasivaTercerosEntrada>
         """;

    /// <summary>
    /// DownloadEnvelope Template para envelope.
    /// </summary>
    public const string DownloadEnvelope =
        $"""
         <s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/"
                     xmlns:des="http://DescargaMasivaTerceros.sat.gob.mx"
                     xmlns:xd="http://www.w3.org/2000/09/xmldsig#">
             <s:Header/>
             <s:Body>
                 <des:PeticionDescargaMasivaTercerosEntrada>
                     <des:peticionDescarga IdPaquete="{XmlPlaceholders.PackageId}" RfcSolicitante="{XmlPlaceholders.RequesterTin}">
                         {XmlPlaceholders.SignatureData}
                     </des:peticionDescarga>
                 </des:PeticionDescargaMasivaTercerosEntrada>
             </s:Body>
         </s:Envelope>
         """;

    #endregion

    #region Template Building Helper Methods

    /// <summary>
    /// Construye un template de digest genérico basado en el tipo de operación
    /// </summary>
    /// <param name="nodeName">Nombre del nodo (ej: SolicitaDescargaEmitidos)</param>
    /// <param name="elementName">Nombre del elemento (ej: solicitud, peticionDescarga)</param>
    /// <param name="attributes">Placeholders de atributos</param>
    /// <param name="extraXml">XML adicional interno</param>
    /// <returns>Template de digest</returns>
    public static string BuildDigestTemplate(string nodeName, string elementName = "solicitud",
        string attributes = XmlPlaceholders.RequestAttributes, string extraXml = XmlPlaceholders.ExtraXml)
    {
        return $"""
                <des:{nodeName} xmlns:des="http://DescargaMasivaTerceros.sat.gob.mx">
                    <des:{elementName} {attributes}>
                        {extraXml}
                    </des:{elementName}>
                </des:{nodeName}>
                """;
    }

    /// <summary>
    /// Construye un template de envelope genérico
    /// </summary>
    /// <param name="nodeName">Nombre del nodo del servicio</param>
    /// <param name="elementName">Nombre del elemento (solicitud, peticionDescarga)</param>
    /// <param name="attributes">Placeholders de atributos</param>
    /// <param name="extraXml">XML adicional</param>
    /// <returns>Template de envelope</returns>
    public static string BuildEnvelopeTemplate(string nodeName, string elementName = "solicitud",
        string attributes = XmlPlaceholders.RequestAttributes, string extraXml = XmlPlaceholders.ExtraXml)
    {
        return $"""
                <s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/"
                            xmlns:des="http://DescargaMasivaTerceros.sat.gob.mx"
                            xmlns:xd="http://www.w3.org/2000/09/xmldsig#">
                    <s:Header/>
                    <s:Body>
                        <des:{nodeName}>
                            <des:{elementName} {attributes}>
                                {extraXml}
                                {XmlPlaceholders.SignatureData}
                            </des:{elementName}>
                        </des:{nodeName}>
                    </s:Body>
                </s:Envelope>
                """;
    }

    #endregion
}