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

using XmlDownloader.Common.Attributes;

namespace XmlDownloader.Query.Models;

public enum InvoiceComplement
{
    [EnumCode("")] Any,
    [EnumCode("acreditamientoieps10")] AcreditamientoIeps10,
    [EnumCode("aerolineas")] Aerolineas,
    [EnumCode("certificadodedestruccion")] CertificadoDeDestruccion,
    [EnumCode("cfdiregistrofiscal")] CfdiRegistroFiscal,
    [EnumCode("comercioexterior10")] ComercioExterior10,
    [EnumCode("comercioexterior11")] ComercioExterior11,
    [EnumCode("comprobante")] Comprobante,
    [EnumCode("consumodecombustibles")] ConsumoDecombustibles,
    [EnumCode("consumodecombustibles11")] ConsumoDecombustibles11,
    [EnumCode("detallista")] Detallista,
    [EnumCode("divisas")] Divisas,
    [EnumCode("donat11")] Donat11,
    [EnumCode("ecc11")] Ecc11,
    [EnumCode("ecc12")] Ecc12,
    [EnumCode("gastoshidrocarburos10")] GastosHidrocarburos10,
    [EnumCode("iedu")] Iedu,
    [EnumCode("implocal")] ImpLocal,
    [EnumCode("ine11")] Ine11,
    [EnumCode("ingresoshidrocarburos")] IngresosHidrocarburos,
    [EnumCode("leyendasfisc")] LeyendasFisc,
    [EnumCode("nomina11")] Nomina11,
    [EnumCode("nomina12")] Nomina12,
    [EnumCode("notariospublicos")] NotariosPublicos,
    [EnumCode("obrasarteantiguedades")] ObrasArteAntiguedades,
    [EnumCode("pagoenespecie")] PagoEnEspecie,
    [EnumCode("pagos10")] Pagos10,
    [EnumCode("pfic")] Pfic,

    [EnumCode("renovacionysustitucionvehiculos")]
    RenovacionYSustitucionVehiculos,

    [EnumCode("servicioparcialconstruccion")]
    ServicioParcialConstruccion,
    [EnumCode("spei")] Spei,
    [EnumCode("terceros11")] Terceros11,

    [EnumCode("turistapasajeroextranjero")]
    TuristaPasajeroExtranjero,
    [EnumCode("valesdedespensa")] ValesDeDespensa,
    [EnumCode("vehiculousado")] VehiculoUsado,
    [EnumCode("ventavehiculos11")] VentaVehiculos11
}