using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Fiscalapi.XmlDownloader.Common.Models
{
    public class MetaItem
    {
        /// <summary>
        /// Folio de la factura - UUID
        /// </summary>
        public string InvoiceUuid { get; set; }

        /// <summary>
        /// RFC del emisor del comprobante - RfcEmisor
        /// </summary>
        [Required]
        [StringLength(13)]
        public string IssuerTin { get; set; }

        /// <summary>
        /// Nombre o razón social del emisor - NombreEmisor
        /// </summary>
        [Required]
        public string IssuerName { get; set; }

        /// <summary>
        /// RFC del receptor del comprobante - RfcReceptor
        /// </summary>
        [Required]
        [StringLength(13)]
        public string RecipientTin { get; set; }

        /// <summary>
        /// Nombre o razón social del receptor - NombreReceptor
        /// </summary>
        [Required]
        public string RecipientName { get; set; }

        /// <summary>
        /// RFC del Proveedor Autorizado de Certificación (PAC) - RfcPac
        /// </summary>
        [Required]
        [StringLength(13)]
        public string PacTin { get; set; }

        /// <summary>
        /// Fecha y hora de emisión del comprobante - FechaEmision
        /// </summary>
        public DateTime InvoiceDate { get; set; }

        /// <summary>
        /// Fecha y hora de certificación por el SAT - FechaCertificacionSat
        /// </summary>
        public DateTime? SatCertificationDate { get; set; }

        /// <summary>
        /// Monto total del comprobante - Monto
        /// </summary>
        [Range(0, double.MaxValue)]
        public decimal Amount { get; set; }

        /// <summary>
        /// Tipo de comprobante (I = Ingreso, E = Egreso, T = Traslado, N = Nómina, P = Pago) - EfectoComprobante
        /// </summary>
        [Required]
        [StringLength(1)]
        public string InvoiceType { get; set; }

        /// <summary>
        /// Estatus del comprobante (1 = Vigente, 0 = Cancelado) - Estatus
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Fecha de cancelación del comprobante (si aplica) - FechaCancelacion
        /// </summary>
        public DateTime? CancellationDate { get; set; }


        /// <summary>
        /// Constructor con parámetros
        /// </summary> 
        public MetaItem(string invoiceUuid, string issuerTin, string issuerName, string recipientTin,
            string recipientName, string pacTin, DateTime invoiceDate,
            DateTime? satCertificationDate, decimal amount, string invoiceType,
            int status, DateTime? cancellationDate = null)
        {
            InvoiceUuid = invoiceUuid;
            IssuerTin = issuerTin;
            IssuerName = issuerName;
            RecipientTin = recipientTin;
            RecipientName = recipientName;
            PacTin = pacTin;
            InvoiceDate = invoiceDate;
            SatCertificationDate = satCertificationDate;
            Amount = amount;
            InvoiceType = invoiceType;
            Status = status;
            CancellationDate = cancellationDate;
        }

        /// <summary>
        /// Método para parsear una línea de texto separada por ~ al objeto MetaItem
        /// </summary>
        [Obsolete("Use TryCreateFromString(); será eliminado en v7.")]
        public static MetaItem CreateFromString(string metadataTextLine)
        {
            if (TryCreateFromString(metadataTextLine, out var metaItem, out var error))
                return metaItem!;

            throw new ArgumentException(error, nameof(metadataTextLine));
        }

        /// <summary>
        /// Parsea una línea de metadata separada por ~ de forma tolerante y con cultura
        /// invariante. Nunca lanza: una línea malformada devuelve false con un error
        /// descriptivo.
        /// </summary>
        /// <param name="metadataTextLine">Línea del archivo .txt de metadata.</param>
        /// <param name="metaItem">Metadata parseada, o null si la línea es inválida.</param>
        /// <param name="error">Descripción del problema cuando el parseo falla.</param>
        public static bool TryCreateFromString(string? metadataTextLine, out MetaItem? metaItem, out string? error)
        {
            metaItem = null;
            error = null;

            if (string.IsNullOrWhiteSpace(metadataTextLine))
            {
                error = "La línea está vacía.";
                return false;
            }

            var fields = metadataTextLine.Split('~');

            if (fields.Length < 11)
            {
                error = $"Formato inválido: se requieren al menos 11 campos y se encontraron {fields.Length}.";
                return false;
            }

            if (!DateTime.TryParse(fields[6], CultureInfo.InvariantCulture, DateTimeStyles.None, out var invoiceDate))
            {
                error = $"Campo 7 (fecha de emisión) inválido: '{fields[6]}'.";
                return false;
            }

            DateTime? satCertificationDate = null;
            if (!string.IsNullOrEmpty(fields[7]))
            {
                if (!DateTime.TryParse(fields[7], CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsed))
                {
                    error = $"Campo 8 (fecha de certificación) inválido: '{fields[7]}'.";
                    return false;
                }

                satCertificationDate = parsed;
            }

            if (!decimal.TryParse(fields[8], NumberStyles.Number, CultureInfo.InvariantCulture, out var amount))
            {
                error = $"Campo 9 (monto) inválido: '{fields[8]}'.";
                return false;
            }

            if (!int.TryParse(fields[10], NumberStyles.Integer, CultureInfo.InvariantCulture, out var status))
            {
                error = $"Campo 11 (estatus) inválido: '{fields[10]}'.";
                return false;
            }

            DateTime? cancellationDate = null;
            if (fields.Length > 11 && !string.IsNullOrEmpty(fields[11]))
            {
                if (!DateTime.TryParse(fields[11], CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsed))
                {
                    error = $"Campo 12 (fecha de cancelación) inválido: '{fields[11]}'.";
                    return false;
                }

                cancellationDate = parsed;
            }

            metaItem = new MetaItem(
                invoiceUuid: fields[0],
                issuerTin: fields[1],
                issuerName: fields[2],
                recipientTin: fields[3],
                recipientName: fields[4],
                pacTin: fields[5],
                invoiceDate: invoiceDate,
                satCertificationDate: satCertificationDate,
                amount: amount,
                invoiceType: fields[9],
                status: status,
                cancellationDate: cancellationDate
            );
            return true;
        }

        /// <summary>
        /// Convierte el objeto a string con formato separado por ~ (cultura invariante,
        /// round-trip con TryCreateFromString)
        /// </summary>
        public override string ToString()
        {
            return $"{InvoiceUuid}~{IssuerTin}~{IssuerName}~{RecipientTin}~{RecipientName}~{PacTin}~" +
                   $"{InvoiceDate:yyyy-MM-dd HH:mm:ss}~{SatCertificationDate?.ToString("yyyy-MM-dd HH:mm:ss")}~" +
                   $"{Amount.ToString(CultureInfo.InvariantCulture)}~{InvoiceType}~{Status}~" +
                   $"{CancellationDate?.ToString("yyyy-MM-dd HH:mm:ss")}";
        }

        /// <summary>
        /// Indica si el comprobante está vigente
        /// </summary>
        public bool IsActive => Status == 1;

        /// <summary>
        /// Indica si el comprobante está cancelado
        /// </summary>
        public bool IsCancelled => Status == 0;


        /// <summary>
        /// Raw content in Base64 format.
        /// </summary>
        [XmlIgnore]
        public string? Base64Content { get; set; }
    }
}