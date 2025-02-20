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
    [Serializable]


    // [XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/Pagos20")]
    public class PagosPagoImpuestosPTrasladoP
    {

        private decimal basePField;

        private string impuestoPField;

        private string tipoFactorPField;

        private decimal tasaOCuotaPField;

        private bool tasaOCuotaPFieldSpecified;

        private decimal importePField;

        private bool importePFieldSpecified;


        [XmlAttribute]
        public decimal BaseP
        {
            get
            {
                return basePField;
            }
            set
            {
                basePField = value;
            }
        }


        [XmlAttribute]
        public string ImpuestoP
        {
            get
            {
                return impuestoPField;
            }
            set
            {
                impuestoPField = value;
            }
        }


        [XmlAttribute]
        public string TipoFactorP
        {
            get
            {
                return tipoFactorPField;
            }
            set
            {
                tipoFactorPField = value;
            }
        }


        [XmlAttribute]
        public decimal TasaOCuotaP
        {
            get
            {
                return tasaOCuotaPField;
            }
            set
            {
                tasaOCuotaPField = value;
                tasaOCuotaPFieldSpecified = value >= 0;
            }
        }


        [XmlIgnore]
        public bool TasaOCuotaPSpecified
        {
            get
            {
                return tasaOCuotaPFieldSpecified;
            }
            set
            {
                tasaOCuotaPFieldSpecified = value;
            }
        }


        [XmlAttribute]
        public decimal ImporteP
        {
            get
            {
                return importePField;
            }
            set
            {
                importePField = value;
                importePFieldSpecified = value >= 0;
            }
        }


        [XmlIgnore]
        public bool ImportePSpecified
        {
            get
            {
                return importePFieldSpecified;
            }
            set
            {
                importePFieldSpecified = value;
            }
        }
    }
}