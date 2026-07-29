using System.Xml.Serialization;

namespace Fiscalapi.XmlDownloader.Common.Models.Cfdi33 {
    /// <remarks/>
    
    [Serializable()]
    
    
    [XmlType(AnonymousType=true, Namespace="http://www.sat.gob.mx/cfd/3")]
    [XmlRoot(Namespace="http://www.sat.gob.mx/cfd/3", IsNullable=false)]
    public partial class Comprobante {
        
        private ComprobanteCfdiRelacionados cfdiRelacionadosField;
        
        private ComprobanteEmisor emisorField;
        
        private ComprobanteReceptor receptorField;
        
        private ComprobanteConcepto[] conceptosField;
        
        private ComprobanteImpuestos impuestosField;
        
        private ComprobanteComplemento[] complementoField;
        
        private ComprobanteAddenda addendaField;
        
        private string versionField;
        
        private string serieField;
        
        private string folioField;
        
        private System.DateTime fechaField;
        
        private string selloField;
        
        private string formaPagoField;
        
        private string noCertificadoField;
        
        private string certificadoField;
        
        private string condicionesDePagoField;
        
        private decimal subTotalField;
        
        private decimal descuentoField;
        
        private bool descuentoFieldSpecified;
        
        private string monedaField;
        
        private decimal tipoCambioField;
        
        private bool tipoCambioFieldSpecified;
        
        private decimal totalField;
        
        private string tipoDeComprobanteField;
        
        private string metodoPagoField;
        
        private string lugarExpedicionField;
        
        private string confirmacionField;
        
        public Comprobante() {
            this.versionField = "3.3";
        }
        
        /// <remarks/>
        public ComprobanteCfdiRelacionados CfdiRelacionados {
            get {
                return this.cfdiRelacionadosField;
            }
            set {
                this.cfdiRelacionadosField = value;
            }
        }
        
        /// <remarks/>
        public ComprobanteEmisor Emisor {
            get {
                return this.emisorField;
            }
            set {
                this.emisorField = value;
            }
        }
        
        /// <remarks/>
        public ComprobanteReceptor Receptor {
            get {
                return this.receptorField;
            }
            set {
                this.receptorField = value;
            }
        }
        
        /// <remarks/>
        [XmlArrayItem("Concepto", IsNullable=false)]
        public ComprobanteConcepto[] Conceptos {
            get {
                return this.conceptosField;
            }
            set {
                this.conceptosField = value;
            }
        }
        
        /// <remarks/>
        public ComprobanteImpuestos Impuestos {
            get {
                return this.impuestosField;
            }
            set {
                this.impuestosField = value;
            }
        }
        
        /// <remarks/>
        [XmlElement("Complemento")]
        public ComprobanteComplemento[] Complemento {
            get {
                return this.complementoField;
            }
            set {
                this.complementoField = value;
            }
        }
        
        /// <remarks/>
        public ComprobanteAddenda Addenda {
            get {
                return this.addendaField;
            }
            set {
                this.addendaField = value;
            }
        }
        
        /// <remarks/>
        [XmlAttribute()]
        public string Version {
            get {
                return this.versionField;
            }
            set {
                this.versionField = value;
            }
        }
        
        /// <remarks/>
        [XmlAttribute()]
        public string Serie {
            get {
                return this.serieField;
            }
            set {
                this.serieField = value;
            }
        }
        
        /// <remarks/>
        [XmlAttribute()]
        public string Folio {
            get {
                return this.folioField;
            }
            set {
                this.folioField = value;
            }
        }
        
        /// <remarks/>
        [XmlAttribute()]
        public System.DateTime Fecha {
            get {
                return this.fechaField;
            }
            set {
                this.fechaField = value;
            }
        }
        
        /// <remarks/>
        [XmlAttribute()]
        public string Sello {
            get {
                return this.selloField;
            }
            set {
                this.selloField = value;
            }
        }
        
        /// <remarks/>
        [XmlAttribute()]
        public string FormaPago {
            get {
                return this.formaPagoField;
            }
            set {
                this.formaPagoField = value;
            }
        }
        
        /// <remarks/>
        [XmlAttribute()]
        public string NoCertificado {
            get {
                return this.noCertificadoField;
            }
            set {
                this.noCertificadoField = value;
            }
        }
        
        /// <remarks/>
        [XmlAttribute()]
        public string Certificado {
            get {
                return this.certificadoField;
            }
            set {
                this.certificadoField = value;
            }
        }
        
        /// <remarks/>
        [XmlAttribute()]
        public string CondicionesDePago {
            get {
                return this.condicionesDePagoField;
            }
            set {
                this.condicionesDePagoField = value;
            }
        }
        
        /// <remarks/>
        [XmlAttribute()]
        public decimal SubTotal {
            get {
                return this.subTotalField;
            }
            set {
                this.subTotalField = value;
            }
        }
        
        /// <remarks/>
        [XmlAttribute()]
        public decimal Descuento {
            get {
                return this.descuentoField;
            }
            set {
                this.descuentoField = value;
            }
        }
        
        /// <remarks/>
        [XmlIgnore()]
        public bool DescuentoSpecified {
            get {
                return this.descuentoFieldSpecified;
            }
            set {
                this.descuentoFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [XmlAttribute()]
        public string Moneda {
            get {
                return this.monedaField;
            }
            set {
                this.monedaField = value;
            }
        }
        
        /// <remarks/>
        [XmlAttribute()]
        public decimal TipoCambio {
            get {
                return this.tipoCambioField;
            }
            set {
                this.tipoCambioField = value;
            }
        }
        
        /// <remarks/>
        [XmlIgnore()]
        public bool TipoCambioSpecified {
            get {
                return this.tipoCambioFieldSpecified;
            }
            set {
                this.tipoCambioFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [XmlAttribute()]
        public decimal Total {
            get {
                return this.totalField;
            }
            set {
                this.totalField = value;
            }
        }
        
        /// <remarks/>
        [XmlAttribute()]
        public string TipoDeComprobante {
            get {
                return this.tipoDeComprobanteField;
            }
            set {
                this.tipoDeComprobanteField = value;
            }
        }
        
        /// <remarks/>
        [XmlAttribute()]
        public string MetodoPago {
            get {
                return this.metodoPagoField;
            }
            set {
                this.metodoPagoField = value;
            }
        }
        
        /// <remarks/>
        [XmlAttribute()]
        public string LugarExpedicion {
            get {
                return this.lugarExpedicionField;
            }
            set {
                this.lugarExpedicionField = value;
            }
        }
        
        /// <remarks/>
        [XmlAttribute()]
        public string Confirmacion {
            get {
                return this.confirmacionField;
            }
            set {
                this.confirmacionField = value;
            }
        }
    }
    
    /// <remarks/>
    
    [Serializable()]
    
    
    [XmlType(AnonymousType=true, Namespace="http://www.sat.gob.mx/cfd/3")]
    public partial class ComprobanteCfdiRelacionados {
        
        private ComprobanteCfdiRelacionadosCfdiRelacionado[] cfdiRelacionadoField;
        
        private string tipoRelacionField;
        
        /// <remarks/>
        [XmlElement("CfdiRelacionado")]
        public ComprobanteCfdiRelacionadosCfdiRelacionado[] CfdiRelacionado {
            get {
                return this.cfdiRelacionadoField;
            }
            set {
                this.cfdiRelacionadoField = value;
            }
        }
        
        /// <remarks/>
        [XmlAttribute()]
        public string TipoRelacion {
            get {
                return this.tipoRelacionField;
            }
            set {
                this.tipoRelacionField = value;
            }
        }
    }
    
    /// <remarks/>
    
    [Serializable()]
    
    
    [XmlType(AnonymousType=true, Namespace="http://www.sat.gob.mx/cfd/3")]
    public partial class ComprobanteCfdiRelacionadosCfdiRelacionado {
        
        private string uUIDField;
        
        /// <remarks/>
        [XmlAttribute()]
        public string UUID {
            get {
                return this.uUIDField;
            }
            set {
                this.uUIDField = value;
            }
        }
    }
    
  
    /// <remarks/>
    
    [Serializable()]
    
    
    [XmlType(AnonymousType=true, Namespace="http://www.sat.gob.mx/cfd/3")]
    public partial class ComprobanteEmisor {
        
        private string rfcField;
        
        private string nombreField;
        
        private string regimenFiscalField;
        
        /// <remarks/>
        [XmlAttribute()]
        public string Rfc {
            get {
                return this.rfcField;
            }
            set {
                this.rfcField = value;
            }
        }
        
        /// <remarks/>
        [XmlAttribute()]
        public string Nombre {
            get {
                return this.nombreField;
            }
            set {
                this.nombreField = value;
            }
        }
        
        /// <remarks/>
        [XmlAttribute()]
        public string RegimenFiscal {
            get {
                return this.regimenFiscalField;
            }
            set {
                this.regimenFiscalField = value;
            }
        }
    }
    
  
    /// <remarks/>
    
    [Serializable()]
    
    
    [XmlType(AnonymousType=true, Namespace="http://www.sat.gob.mx/cfd/3")]
    public partial class ComprobanteReceptor {
        
        private string rfcField;
        
        private string nombreField;
        
        private string residenciaFiscalField;
        
        private string numRegIdTribField;
        
        private string usoCFDIField;
        
        /// <remarks/>
        [XmlAttribute()]
        public string Rfc {
            get {
                return this.rfcField;
            }
            set {
                this.rfcField = value;
            }
        }
        
        /// <remarks/>
        [XmlAttribute()]
        public string Nombre {
            get {
                return this.nombreField;
            }
            set {
                this.nombreField = value;
            }
        }
        
        /// <remarks/>
        [XmlAttribute()]
        public string ResidenciaFiscal {
            get {
                return this.residenciaFiscalField;
            }
            set {
                this.residenciaFiscalField = value;
            }
        }
        
        /// <remarks/>
        [XmlAttribute()]
        public string NumRegIdTrib {
            get {
                return this.numRegIdTribField;
            }
            set {
                this.numRegIdTribField = value;
            }
        }
        
        /// <remarks/>
        [XmlAttribute()]
        public string UsoCFDI {
            get {
                return this.usoCFDIField;
            }
            set {
                this.usoCFDIField = value;
            }
        }
    }
    
  
    /// <remarks/>
    
    [Serializable()]
    
    
    [XmlType(AnonymousType=true, Namespace="http://www.sat.gob.mx/cfd/3")]
    public partial class ComprobanteConcepto {
        
        private ComprobanteConceptoImpuestos impuestosField;
        
        private ComprobanteConceptoInformacionAduanera[] informacionAduaneraField;
        
        private ComprobanteConceptoCuentaPredial cuentaPredialField;
        
        private ComprobanteConceptoComplementoConcepto complementoConceptoField;
        
        private ComprobanteConceptoParte[] parteField;
        
        private string claveProdServField;
        
        private string noIdentificacionField;
        
        private decimal cantidadField;
        
        private string claveUnidadField;
        
        private string unidadField;
        
        private string descripcionField;
        
        private decimal valorUnitarioField;
        
        private decimal importeField;
        
        private decimal descuentoField;
        
        private bool descuentoFieldSpecified;
        
        /// <remarks/>
        public ComprobanteConceptoImpuestos Impuestos {
            get {
                return this.impuestosField;
            }
            set {
                this.impuestosField = value;
            }
        }
        
        /// <remarks/>
        [XmlElement("InformacionAduanera")]
        public ComprobanteConceptoInformacionAduanera[] InformacionAduanera {
            get {
                return this.informacionAduaneraField;
            }
            set {
                this.informacionAduaneraField = value;
            }
        }
        
        /// <remarks/>
        public ComprobanteConceptoCuentaPredial CuentaPredial {
            get {
                return this.cuentaPredialField;
            }
            set {
                this.cuentaPredialField = value;
            }
        }
        
        /// <remarks/>
        public ComprobanteConceptoComplementoConcepto ComplementoConcepto {
            get {
                return this.complementoConceptoField;
            }
            set {
                this.complementoConceptoField = value;
            }
        }
        
        /// <remarks/>
        [XmlElement("Parte")]
        public ComprobanteConceptoParte[] Parte {
            get {
                return this.parteField;
            }
            set {
                this.parteField = value;
            }
        }
        
        /// <remarks/>
        [XmlAttribute()]
        public string ClaveProdServ {
            get {
                return this.claveProdServField;
            }
            set {
                this.claveProdServField = value;
            }
        }
        
        /// <remarks/>
        [XmlAttribute()]
        public string NoIdentificacion {
            get {
                return this.noIdentificacionField;
            }
            set {
                this.noIdentificacionField = value;
            }
        }
        
        /// <remarks/>
        [XmlAttribute()]
        public decimal Cantidad {
            get {
                return this.cantidadField;
            }
            set {
                this.cantidadField = value;
            }
        }
        
        /// <remarks/>
        [XmlAttribute()]
        public string ClaveUnidad {
            get {
                return this.claveUnidadField;
            }
            set {
                this.claveUnidadField = value;
            }
        }
        
        /// <remarks/>
        [XmlAttribute()]
        public string Unidad {
            get {
                return this.unidadField;
            }
            set {
                this.unidadField = value;
            }
        }
        
        /// <remarks/>
        [XmlAttribute()]
        public string Descripcion {
            get {
                return this.descripcionField;
            }
            set {
                this.descripcionField = value;
            }
        }
        
        /// <remarks/>
        [XmlAttribute()]
        public decimal ValorUnitario {
            get {
                return this.valorUnitarioField;
            }
            set {
                this.valorUnitarioField = value;
            }
        }
        
        /// <remarks/>
        [XmlAttribute()]
        public decimal Importe {
            get {
                return this.importeField;
            }
            set {
                this.importeField = value;
            }
        }
        
        /// <remarks/>
        [XmlAttribute()]
        public decimal Descuento {
            get {
                return this.descuentoField;
            }
            set {
                this.descuentoField = value;
            }
        }
        
        /// <remarks/>
        [XmlIgnore()]
        public bool DescuentoSpecified {
            get {
                return this.descuentoFieldSpecified;
            }
            set {
                this.descuentoFieldSpecified = value;
            }
        }
    }
    
    /// <remarks/>
    
    [Serializable()]
    
    
    [XmlType(AnonymousType=true, Namespace="http://www.sat.gob.mx/cfd/3")]
    public partial class ComprobanteConceptoImpuestos {
        
        private ComprobanteConceptoImpuestosTraslado[] trasladosField;
        
        private ComprobanteConceptoImpuestosRetencion[] retencionesField;
        
        /// <remarks/>
        [XmlArrayItem("Traslado", IsNullable=false)]
        public ComprobanteConceptoImpuestosTraslado[] Traslados {
            get {
                return this.trasladosField;
            }
            set {
                this.trasladosField = value;
            }
        }
        
        /// <remarks/>
        [XmlArrayItem("Retencion", IsNullable=false)]
        public ComprobanteConceptoImpuestosRetencion[] Retenciones {
            get {
                return this.retencionesField;
            }
            set {
                this.retencionesField = value;
            }
        }
    }
    
    /// <remarks/>
    
    [Serializable()]
    
    
    [XmlType(AnonymousType=true, Namespace="http://www.sat.gob.mx/cfd/3")]
    public partial class ComprobanteConceptoImpuestosTraslado {
        
        private decimal baseField;
        
        private string impuestoField;
        
        private string tipoFactorField;
        
        private decimal tasaOCuotaField;
        
        private bool tasaOCuotaFieldSpecified;
        
        private decimal importeField;
        
        private bool importeFieldSpecified;
        
        /// <remarks/>
        [XmlAttribute()]
        public decimal Base {
            get {
                return this.baseField;
            }
            set {
                this.baseField = value;
            }
        }
        
        /// <remarks/>
        [XmlAttribute()]
        public string Impuesto {
            get {
                return this.impuestoField;
            }
            set {
                this.impuestoField = value;
            }
        }
        
        /// <remarks/>
        [XmlAttribute()]
        public string TipoFactor {
            get {
                return this.tipoFactorField;
            }
            set {
                this.tipoFactorField = value;
            }
        }
        
        /// <remarks/>
        [XmlAttribute()]
        public decimal TasaOCuota {
            get {
                return this.tasaOCuotaField;
            }
            set {
                this.tasaOCuotaField = value;
            }
        }
        
        /// <remarks/>
        [XmlIgnore()]
        public bool TasaOCuotaSpecified {
            get {
                return this.tasaOCuotaFieldSpecified;
            }
            set {
                this.tasaOCuotaFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [XmlAttribute()]
        public decimal Importe {
            get {
                return this.importeField;
            }
            set {
                this.importeField = value;
            }
        }
        
        /// <remarks/>
        [XmlIgnore()]
        public bool ImporteSpecified {
            get {
                return this.importeFieldSpecified;
            }
            set {
                this.importeFieldSpecified = value;
            }
        }
    }
    
  
    
    
    /// <remarks/>
    
    [Serializable()]
    
    
    [XmlType(AnonymousType=true, Namespace="http://www.sat.gob.mx/cfd/3")]
    public partial class ComprobanteConceptoImpuestosRetencion {
        
        private decimal baseField;
        
        private string impuestoField;
        
        private string tipoFactorField;
        
        private decimal tasaOCuotaField;
        
        private decimal importeField;
        
        /// <remarks/>
        [XmlAttribute()]
        public decimal Base {
            get {
                return this.baseField;
            }
            set {
                this.baseField = value;
            }
        }
        
        /// <remarks/>
        [XmlAttribute()]
        public string Impuesto {
            get {
                return this.impuestoField;
            }
            set {
                this.impuestoField = value;
            }
        }
        
        /// <remarks/>
        [XmlAttribute()]
        public string TipoFactor {
            get {
                return this.tipoFactorField;
            }
            set {
                this.tipoFactorField = value;
            }
        }
        
        /// <remarks/>
        [XmlAttribute()]
        public decimal TasaOCuota {
            get {
                return this.tasaOCuotaField;
            }
            set {
                this.tasaOCuotaField = value;
            }
        }
        
        /// <remarks/>
        [XmlAttribute()]
        public decimal Importe {
            get {
                return this.importeField;
            }
            set {
                this.importeField = value;
            }
        }
    }
    
    /// <remarks/>
    
    [Serializable()]
    
    
    [XmlType(AnonymousType=true, Namespace="http://www.sat.gob.mx/cfd/3")]
    public partial class ComprobanteConceptoInformacionAduanera {
        
        private string numeroPedimentoField;
        
        /// <remarks/>
        [XmlAttribute()]
        public string NumeroPedimento {
            get {
                return this.numeroPedimentoField;
            }
            set {
                this.numeroPedimentoField = value;
            }
        }
    }
    
    /// <remarks/>
    
    [Serializable()]
    
    
    [XmlType(AnonymousType=true, Namespace="http://www.sat.gob.mx/cfd/3")]
    public partial class ComprobanteConceptoCuentaPredial {
        
        private string numeroField;
        
        /// <remarks/>
        [XmlAttribute()]
        public string Numero {
            get {
                return this.numeroField;
            }
            set {
                this.numeroField = value;
            }
        }
    }
    
    /// <remarks/>
    
    [Serializable()]
    
    
    [XmlType(AnonymousType=true, Namespace="http://www.sat.gob.mx/cfd/3")]
    public partial class ComprobanteConceptoComplementoConcepto {
        
        private System.Xml.XmlElement[] anyField;
        
        /// <remarks/>
        [XmlAnyElement()]
        public System.Xml.XmlElement[] Any {
            get {
                return this.anyField;
            }
            set {
                this.anyField = value;
            }
        }
    }
    
    /// <remarks/>
    
    [Serializable()]
    
    
    [XmlType(AnonymousType=true, Namespace="http://www.sat.gob.mx/cfd/3")]
    public partial class ComprobanteConceptoParte {
        
        private ComprobanteConceptoParteInformacionAduanera[] informacionAduaneraField;
        
        private string claveProdServField;
        
        private string noIdentificacionField;
        
        private decimal cantidadField;
        
        private string unidadField;
        
        private string descripcionField;
        
        private decimal valorUnitarioField;
        
        private bool valorUnitarioFieldSpecified;
        
        private decimal importeField;
        
        private bool importeFieldSpecified;
        
        /// <remarks/>
        [XmlElement("InformacionAduanera")]
        public ComprobanteConceptoParteInformacionAduanera[] InformacionAduanera {
            get {
                return this.informacionAduaneraField;
            }
            set {
                this.informacionAduaneraField = value;
            }
        }
        
        /// <remarks/>
        [XmlAttribute()]
        public string ClaveProdServ {
            get {
                return this.claveProdServField;
            }
            set {
                this.claveProdServField = value;
            }
        }
        
        /// <remarks/>
        [XmlAttribute()]
        public string NoIdentificacion {
            get {
                return this.noIdentificacionField;
            }
            set {
                this.noIdentificacionField = value;
            }
        }
        
        /// <remarks/>
        [XmlAttribute()]
        public decimal Cantidad {
            get {
                return this.cantidadField;
            }
            set {
                this.cantidadField = value;
            }
        }
        
        /// <remarks/>
        [XmlAttribute()]
        public string Unidad {
            get {
                return this.unidadField;
            }
            set {
                this.unidadField = value;
            }
        }
        
        /// <remarks/>
        [XmlAttribute()]
        public string Descripcion {
            get {
                return this.descripcionField;
            }
            set {
                this.descripcionField = value;
            }
        }
        
        /// <remarks/>
        [XmlAttribute()]
        public decimal ValorUnitario {
            get {
                return this.valorUnitarioField;
            }
            set {
                this.valorUnitarioField = value;
            }
        }
        
        /// <remarks/>
        [XmlIgnore()]
        public bool ValorUnitarioSpecified {
            get {
                return this.valorUnitarioFieldSpecified;
            }
            set {
                this.valorUnitarioFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [XmlAttribute()]
        public decimal Importe {
            get {
                return this.importeField;
            }
            set {
                this.importeField = value;
            }
        }
        
        /// <remarks/>
        [XmlIgnore()]
        public bool ImporteSpecified {
            get {
                return this.importeFieldSpecified;
            }
            set {
                this.importeFieldSpecified = value;
            }
        }
    }
    
    /// <remarks/>
    
    [Serializable()]
    
    
    [XmlType(AnonymousType=true, Namespace="http://www.sat.gob.mx/cfd/3")]
    public partial class ComprobanteConceptoParteInformacionAduanera {
        
        private string numeroPedimentoField;
        
        /// <remarks/>
        [XmlAttribute()]
        public string NumeroPedimento {
            get {
                return this.numeroPedimentoField;
            }
            set {
                this.numeroPedimentoField = value;
            }
        }
    }
  
    /// <remarks/>
    
    [Serializable()]
    
    
    [XmlType(AnonymousType=true, Namespace="http://www.sat.gob.mx/cfd/3")]
    public partial class ComprobanteImpuestos {
        
        private ComprobanteImpuestosRetencion[] retencionesField;
        
        private ComprobanteImpuestosTraslado[] trasladosField;
        
        private decimal totalImpuestosRetenidosField;
        
        private bool totalImpuestosRetenidosFieldSpecified;
        
        private decimal totalImpuestosTrasladadosField;
        
        private bool totalImpuestosTrasladadosFieldSpecified;
        
        /// <remarks/>
        [XmlArrayItem("Retencion", IsNullable=false)]
        public ComprobanteImpuestosRetencion[] Retenciones {
            get {
                return this.retencionesField;
            }
            set {
                this.retencionesField = value;
            }
        }
        
        /// <remarks/>
        [XmlArrayItem("Traslado", IsNullable=false)]
        public ComprobanteImpuestosTraslado[] Traslados {
            get {
                return this.trasladosField;
            }
            set {
                this.trasladosField = value;
            }
        }
        
        /// <remarks/>
        [XmlAttribute()]
        public decimal TotalImpuestosRetenidos {
            get {
                return this.totalImpuestosRetenidosField;
            }
            set {
                this.totalImpuestosRetenidosField = value;
            }
        }
        
        /// <remarks/>
        [XmlIgnore()]
        public bool TotalImpuestosRetenidosSpecified {
            get {
                return this.totalImpuestosRetenidosFieldSpecified;
            }
            set {
                this.totalImpuestosRetenidosFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [XmlAttribute()]
        public decimal TotalImpuestosTrasladados {
            get {
                return this.totalImpuestosTrasladadosField;
            }
            set {
                this.totalImpuestosTrasladadosField = value;
            }
        }
        
        /// <remarks/>
        [XmlIgnore()]
        public bool TotalImpuestosTrasladadosSpecified {
            get {
                return this.totalImpuestosTrasladadosFieldSpecified;
            }
            set {
                this.totalImpuestosTrasladadosFieldSpecified = value;
            }
        }
    }
    
    /// <remarks/>
    
    [Serializable()]
    
    
    [XmlType(AnonymousType=true, Namespace="http://www.sat.gob.mx/cfd/3")]
    public partial class ComprobanteImpuestosRetencion {
        
        private string impuestoField;
        
        private decimal importeField;
        
        /// <remarks/>
        [XmlAttribute()]
        public string Impuesto {
            get {
                return this.impuestoField;
            }
            set {
                this.impuestoField = value;
            }
        }
        
        /// <remarks/>
        [XmlAttribute()]
        public decimal Importe {
            get {
                return this.importeField;
            }
            set {
                this.importeField = value;
            }
        }
    }
    
    /// <remarks/>
    
    [Serializable()]
    
    
    [XmlType(AnonymousType=true, Namespace="http://www.sat.gob.mx/cfd/3")]
    public partial class ComprobanteImpuestosTraslado {
        
        private string impuestoField;
        
        private string tipoFactorField;
        
        private decimal tasaOCuotaField;
        
        private decimal importeField;
        
        /// <remarks/>
        [XmlAttribute()]
        public string Impuesto {
            get {
                return this.impuestoField;
            }
            set {
                this.impuestoField = value;
            }
        }
        
        /// <remarks/>
        [XmlAttribute()]
        public string TipoFactor {
            get {
                return this.tipoFactorField;
            }
            set {
                this.tipoFactorField = value;
            }
        }
        
        /// <remarks/>
        [XmlAttribute()]
        public decimal TasaOCuota {
            get {
                return this.tasaOCuotaField;
            }
            set {
                this.tasaOCuotaField = value;
            }
        }
        
        /// <remarks/>
        [XmlAttribute()]
        public decimal Importe {
            get {
                return this.importeField;
            }
            set {
                this.importeField = value;
            }
        }
    }
    
    /// <remarks/>
    
    [Serializable()]
    
    
    [XmlType(AnonymousType=true, Namespace="http://www.sat.gob.mx/cfd/3")]
    public partial class ComprobanteComplemento {
        
        private System.Xml.XmlElement[] anyField;
        
        /// <remarks/>
        [XmlAnyElement()]
        public System.Xml.XmlElement[] Any {
            get {
                return this.anyField;
            }
            set {
                this.anyField = value;
            }
        }
    }
    
    /// <remarks/>
    
    [Serializable()]
    
    
    [XmlType(AnonymousType=true, Namespace="http://www.sat.gob.mx/cfd/3")]
    public partial class ComprobanteAddenda {
        
        private System.Xml.XmlElement[] anyField;
        
        /// <remarks/>
        [XmlAnyElement()]
        public System.Xml.XmlElement[] Any {
            get {
                return this.anyField;
            }
            set {
                this.anyField = value;
            }
        }
    }
    
   

}
