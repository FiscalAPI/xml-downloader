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

using System.Xml.Serialization;

namespace Fiscalapi.XmlDownloader.Common.Models.Complements.Payment;

/// <remarks/>
[Serializable()]
[XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/Pagos20")]
[XmlRoot(Namespace = "http://www.sat.gob.mx/Pagos20", IsNullable = false)]
public class Pagos
{
    private PagosTotales totalesField;

    private PagosPago[] pagoField;

    private string versionField;

    public Pagos()
    {
        this.versionField = "2.0";
    }

    /// <remarks/>
    public PagosTotales Totales
    {
        get { return this.totalesField; }
        set { this.totalesField = value; }
    }

    /// <remarks/>
    [XmlElement("Pago")]
    public PagosPago[] Pago
    {
        get { return this.pagoField; }
        set { this.pagoField = value; }
    }

    /// <remarks/>
    [XmlAttribute()]
    public string Version
    {
        get { return this.versionField; }
        set { this.versionField = value; }
    }
}