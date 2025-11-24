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

using Fiscalapi.XmlDownloader.Common.Enums;
using Fiscalapi.XmlDownloader.Common.Http;

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


    public void Validate(ServiceEndpoints endpoints)
    {
        // Check: No se permite la descarga de retenciones en el servicio de CFDI | Metadatos.
        if (RequestType == QueryType.Retenciones && endpoints.ServiceType != ServiceType.Retenciones)
        {
            throw new InvalidOperationException(
                $"No se permite la descarga de retenciones en el servicio de CFDI | Metadatos.");
        }

        // Check: No se permite la descarga de CFDI | Metadatos en el servicio de retenciones.
        if (RequestType is QueryType.CFDI or QueryType.Metadata && endpoints.ServiceType != ServiceType.Cfdi)
        {
            throw new InvalidOperationException(
                $"No se permite la descarga de CFDI | Metadatos en el servicio de retenciones.");
        }

        // Check: No se permite la descarga de retenciones canceladas
        if (RequestType == QueryType.Retenciones && InvoiceStatus == InvoiceStatus.Cancelado)
        {
            throw new InvalidOperationException($"No se permite la descarga de retenciones cancelados.");
        }

        // Check: No se permite la descarga de CFDI recibidos cancelados.
        if (RequestType == QueryType.CFDI && RecipientTin is not null && InvoiceStatus == InvoiceStatus.Cancelado)
        {
            throw new InvalidOperationException($"No se permite la descarga de CFDI recibidos cancelados.");
        }
    }

    // Method to know what ServiceType is using this QueryParameters
    public ServiceEndpoints GetServiceEndpoints()
    {
        return RequestType switch
        {
            QueryType.CFDI => ServiceEndpoints.Cfdi(),
            QueryType.Metadata => ServiceEndpoints.Cfdi(),
            QueryType.Retenciones => ServiceEndpoints.Retenciones(),
            _ => throw new InvalidOperationException("Tipo de solicitud no válido.")
        };
    }

    public bool IsCfdiServiceType()
    {
        return RequestType switch
        {
            QueryType.CFDI => true,
            QueryType.Metadata => true,
            QueryType.Retenciones => false,
            _ => throw new InvalidOperationException("Tipo de solicitud no válido.")
        };
    }

    public bool IsRetencionesServiceType()
    {
        return RequestType switch
        {
            QueryType.CFDI => false,
            QueryType.Metadata => false,
            QueryType.Retenciones => true,
            _ => throw new InvalidOperationException("Tipo de solicitud no válido.")
        };
    }
}