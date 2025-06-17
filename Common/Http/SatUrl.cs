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

namespace Fiscalapi.XmlDownloader.Common.Http;

/// <summary>
/// Sat web services urls and it's actions
/// </summary>
public static class SatUrl
{
    #region Auth

    public const string AuthUrl =
        "https://cfdidescargamasivasolicitud.clouda.sat.gob.mx/Autenticacion/Autenticacion.svc";

    public const string AuthAction = "http://DescargaMasivaTerceros.gob.mx/IAutenticacion/Autentica";

    #endregion

    #region Request

    public const string RequestUrl =
        "https://cfdidescargamasivasolicitud.clouda.sat.gob.mx/SolicitaDescargaService.svc";

    public const string RequestIssuedAction =
        "http://DescargaMasivaTerceros.sat.gob.mx/ISolicitaDescargaService/SolicitaDescargaEmitidos";

    public const string RequestReceivedAction =
        "http://DescargaMasivaTerceros.sat.gob.mx/ISolicitaDescargaService/SolicitaDescargaRecibidos";

    public const string RequestUuidAction =
        "http://DescargaMasivaTerceros.sat.gob.mx/ISolicitaDescargaService/SolicitaDescargaFolio";

    #endregion

    #region Verify

    public const string VerifyUrl =
        "https://cfdidescargamasivasolicitud.clouda.sat.gob.mx/VerificaSolicitudDescargaService.svc";

    public const string VerifyAction =
        "http://DescargaMasivaTerceros.sat.gob.mx/IVerificaSolicitudDescargaService/VerificaSolicitudDescarga";

    #endregion

    #region Download

    public const string DownloadUrl =
        "https://cfdidescargamasiva.clouda.sat.gob.mx/DescargaMasivaService.svc";

    public const string DownloadAction =
        "http://DescargaMasivaTerceros.sat.gob.mx/IDescargaMasivaTercerosService/Descargar";

    #endregion
}