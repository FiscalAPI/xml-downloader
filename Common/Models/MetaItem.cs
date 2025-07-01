using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public static MetaItem CreateFromString(string metadataTextLine)
        {
            if (string.IsNullOrWhiteSpace(metadataTextLine))
                throw new ArgumentException("Los datos no pueden estar vacíos", nameof(metadataTextLine));

            var fields = metadataTextLine.Split('~');

            if (fields.Length < 11)
                throw new ArgumentException("Formato de datos inválido. Se requieren al menos 11 campos",
                    nameof(metadataTextLine));

            return new MetaItem(
                invoiceUuid: fields[0],
                issuerTin: fields[1],
                issuerName: fields[2],
                recipientTin: fields[3],
                recipientName: fields[4],
                pacTin: fields[5],
                invoiceDate: DateTime.Parse(fields[6]),
                satCertificationDate: string.IsNullOrEmpty(fields[7]) ? null : DateTime.Parse(fields[7]),
                amount: decimal.Parse(fields[8]),
                invoiceType: fields[9],
                status: int.Parse(fields[10]),
                cancellationDate: fields.Length > 11 && !string.IsNullOrEmpty(fields[11])
                    ? DateTime.Parse(fields[11])
                    : null
            );
        }

        /// <summary>
        /// Convierte el objeto a string con formato separado por ~
        /// </summary>
        public override string ToString()
        {
            return $"{InvoiceUuid}~{IssuerTin}~{IssuerName}~{RecipientTin}~{RecipientName}~{PacTin}~" +
                   $"{InvoiceDate:yyyy-MM-dd HH:mm:ss}~{SatCertificationDate?.ToString("yyyy-MM-dd HH:mm:ss")}~" +
                   $"{Amount}~{InvoiceType}~{Status}~{CancellationDate?.ToString("yyyy-MM-dd HH:mm:ss")}";
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