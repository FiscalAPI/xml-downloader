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

namespace Fiscalapi.XmlDownloader.Models.SatModels.Invoicing.Cfdi40
{
    [Serializable]
  
    public class ComprobanteImpuestosTraslado
    {
        private decimal baseField;

        private string impuestoField;

        private string tipoFactorField;

        private decimal tasaOCuotaField;

        private bool tasaOCuotaFieldSpecified;

        private decimal importeField;

        private bool importeFieldSpecified;


        [XmlAttribute]
        public decimal Base
        {
            get { return baseField; }
            set { baseField = value; }
        }


        [XmlAttribute]
        public string Impuesto
        {
            get { return impuestoField; }
            set { impuestoField = value; }
        }


        [XmlAttribute]
        public string TipoFactor
        {
            get { return tipoFactorField; }
            set { tipoFactorField = value; }
        }


        [XmlAttribute]
        public decimal TasaOCuota
        {
            get { return tasaOCuotaField; }
            set
            {
                tasaOCuotaField = value;
                tasaOCuotaFieldSpecified = value > 0;

            }
        }


        [XmlIgnore]
        public bool TasaOCuotaSpecified
        {
            get { return tasaOCuotaFieldSpecified; }
            set { tasaOCuotaFieldSpecified = value; }
        }


        [XmlAttribute]
        public decimal Importe
        {
            get { return importeField; }
            set
            {
                importeField = value;
                importeFieldSpecified = value > 0;
            }
        }


        [XmlIgnore]
        public bool ImporteSpecified
        {
            get { return importeFieldSpecified; }
            set { importeFieldSpecified = value; }
        }
    }
}