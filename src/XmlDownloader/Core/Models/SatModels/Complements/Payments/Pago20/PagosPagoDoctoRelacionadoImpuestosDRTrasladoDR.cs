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

namespace XmlDownloader.Core.Models.SatModels.Complements.Payments.Pago20
{
    [Serializable]
    // [XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/Pagos20")]
    public class PagosPagoDoctoRelacionadoImpuestosDRTrasladoDR
    {

        private decimal baseDRField;

        private string impuestoDRField;

        private string tipoFactorDRField;

        private decimal tasaOCuotaDRField;

        private bool tasaOCuotaDRFieldSpecified;

        private decimal importeDRField;

        private bool importeDRFieldSpecified;


        [XmlAttribute]
        public decimal BaseDR
        {
            get
            {
                return baseDRField;
            }
            set
            {
                baseDRField = value;
            }
        }


        [XmlAttribute]
        public string ImpuestoDR
        {
            get
            {
                return impuestoDRField;
            }
            set
            {
                impuestoDRField = value;
            }
        }


        [XmlAttribute]
        public string TipoFactorDR
        {
            get
            {
                return tipoFactorDRField;
            }
            set
            {
                tipoFactorDRField = value;
                tasaOCuotaDRFieldSpecified = true;
            }
        }


        [XmlAttribute]
        public decimal TasaOCuotaDR
        {
            get
            {
                return tasaOCuotaDRField;
            }
            set
            {
                tasaOCuotaDRField = value;
                tasaOCuotaDRFieldSpecified = value >= 0;
            }
        }


        [XmlIgnore]
        public bool TasaOCuotaDRSpecified
        {
            get
            {
                return tasaOCuotaDRFieldSpecified;
            }
            set
            {
                tasaOCuotaDRFieldSpecified = value;
            }
        }


        [XmlAttribute]
        public decimal ImporteDR
        {
            get
            {
                return importeDRField;
            }
            set
            {
                importeDRField = value;
                importeDRFieldSpecified = value >= 0;
            }
        }


        [XmlIgnore]
        public bool ImporteDRSpecified
        {
            get
            {
                return importeDRFieldSpecified;
            }
            set
            {
                importeDRFieldSpecified = value;
            }
        }
    }
}