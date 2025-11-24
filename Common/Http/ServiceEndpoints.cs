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

namespace Fiscalapi.XmlDownloader.Common.Http;

/// <summary>
/// Clase inmutable que encapsula todos los endpoints del servicio de descarga masiva del SAT.
/// Use ServiceEndpoints.Cfdi() para "CFDI regulares"
/// Use ServiceEndpoints.Retenciones() para "CFDI de retenciones e información de pagos"
/// </summary>
public sealed class ServiceEndpoints
{
    /// <summary>
    /// URL del servicio de autenticación
    /// </summary>
    public string AuthUrl { get; }

    /// <summary>
    /// URL del servicio de solicitud de descarga
    /// </summary>
    public string QueryUrl { get; }

    /// <summary>
    /// URL del servicio de verificación de solicitud
    /// </summary>
    public string VerifyUrl { get; }

    /// <summary>
    /// URL del servicio de descarga de paquetes
    /// </summary>
    public string DownloadUrl { get; }

    /// <summary>
    /// Tipo de servicio (CFDI o Retenciones)
    /// </summary>
    public ServiceType ServiceType { get; }

    // SOAP Actions (constantes que no cambian entre CFDI y Retenciones)
    public const string AuthAction = "http://DescargaMasivaTerceros.gob.mx/IAutenticacion/Autentica";
    public const string RequestIssuedAction = "http://DescargaMasivaTerceros.sat.gob.mx/ISolicitaDescargaService/SolicitaDescargaEmitidos";
    public const string RequestReceivedAction = "http://DescargaMasivaTerceros.sat.gob.mx/ISolicitaDescargaService/SolicitaDescargaRecibidos";
    public const string RequestUuidAction = "http://DescargaMasivaTerceros.sat.gob.mx/ISolicitaDescargaService/SolicitaDescargaFolio";
    public const string VerifyAction = "http://DescargaMasivaTerceros.sat.gob.mx/IVerificaSolicitudDescargaService/VerificaSolicitudDescarga";
    public const string DownloadAction = "http://DescargaMasivaTerceros.sat.gob.mx/IDescargaMasivaTercerosService/Descargar";

    /// <summary>
    /// Constructor privado para crear instancias inmutables
    /// </summary>
    private ServiceEndpoints(string authUrl, string queryUrl, string verifyUrl, string downloadUrl, ServiceType serviceType)
    {
        AuthUrl = authUrl ?? throw new ArgumentNullException(nameof(authUrl));
        QueryUrl = queryUrl ?? throw new ArgumentNullException(nameof(queryUrl));
        VerifyUrl = verifyUrl ?? throw new ArgumentNullException(nameof(verifyUrl));
        DownloadUrl = downloadUrl ?? throw new ArgumentNullException(nameof(downloadUrl));
        ServiceType = serviceType;
    }

    /// <summary>
    /// Crea una instancia con los endpoints para "CFDI regulares"
    /// </summary>
    public static ServiceEndpoints Cfdi()
    {
        return new ServiceEndpoints(
            authUrl: "https://cfdidescargamasivasolicitud.clouda.sat.gob.mx/Autenticacion/Autenticacion.svc",
            queryUrl: "https://cfdidescargamasivasolicitud.clouda.sat.gob.mx/SolicitaDescargaService.svc",
            verifyUrl: "https://cfdidescargamasivasolicitud.clouda.sat.gob.mx/VerificaSolicitudDescargaService.svc",
            downloadUrl: "https://cfdidescargamasiva.clouda.sat.gob.mx/DescargaMasivaService.svc",
            serviceType: ServiceType.Cfdi
        );
    }

    /// <summary>
    /// Crea una instancia con los endpoints para "CFDI de retenciones e información de pagos"
    /// </summary>
    public static ServiceEndpoints Retenciones()
    {
        return new ServiceEndpoints(
            authUrl: "https://retendescargamasivasolicitud.clouda.sat.gob.mx/Autenticacion/Autenticacion.svc",
            queryUrl: "https://retendescargamasivasolicitud.clouda.sat.gob.mx/SolicitaDescargaService.svc",
            verifyUrl: "https://retendescargamasivasolicitud.clouda.sat.gob.mx/VerificaSolicitudDescargaService.svc",
            downloadUrl: "https://retendescargamasiva.clouda.sat.gob.mx/DescargaMasivaService.svc",
            serviceType: ServiceType.Retenciones
        );
    }
}

