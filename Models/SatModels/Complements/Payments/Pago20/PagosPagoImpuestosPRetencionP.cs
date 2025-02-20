﻿//***************************************************************************************
// <Author>                                                                             *
//     Jesús Mendoza Jaurez.                                                            *
//     mendoza.git@gmail.com                                                            *
//     https://github.com/mendozagit                                                    *
//                                                                                      *
//     Los cambios en este archivo podrían causar un comportamiento incorrecto.         *
//     Este código no ofrece ningún tipo de garantía, se generó para ayudar a la        *
//     Comunidad open source, siéntanse libre de utilizarlo, sin ninguna garantía.      *
//     Nota: Mantenga este comentario para respetar al autor.                           *
// </Author>                                                                            *
//***************************************************************************************

using System.Xml.Serialization;

namespace Fiscalapi.XmlDownloader.Models.SatModels.Complements.Payments.Pago20
{
    [Serializable, XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/Pagos20")]
    public  class PagosPagoImpuestosPRetencionP {
    
        private string impuestoPField;
    
        private decimal importePField;
    
    
        [XmlAttribute]
        public string ImpuestoP {
            get {
                return impuestoPField;
            }
            set {
                impuestoPField = value;
            }
        }
    
    
        [XmlAttribute]
        public decimal ImporteP {
            get {
                return importePField;
            }
            set {
                importePField = value;
            }
        }
    }
}