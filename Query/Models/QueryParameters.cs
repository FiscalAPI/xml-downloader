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

namespace Fiscalapi.XmlDownloader.Query.Models;

public class QueryParameters
{
    // 'Folio' attribute
    public string? InvoiceUuid { get; set; }

    // 'RfcSolicitante' attribute
    public string? RequesterTin { get; set; }

    // 'TipoSolicitud' attribute
    public QueryType RequestType { get; set; }

    // 'TipoComprobante' attribute
    public InvoiceType InvoiceType { get; set; }

    // 'FechaInicial' attribute
    public DateTime StartDate { get; set; }

    // 'FechaFinal' attribute
    public DateTime EndDate { get; set; }

    // 'RfcReceptor' attribute
    public string? RecipientTin { get; set; }

    // 'RfcEmisor' attribute
    public string? IssuerTin { get; set; }

    // 'EstadoComprobante' attribute 
    public InvoiceStatus InvoiceStatus { get; set; }

    // 'Complemento' attribute
    public InvoiceComplement InvoiceComplement { get; set; }

    // 'RfcACuentaTerceros' attribute
    public string? ThirdPartyTin { get; set; }
}