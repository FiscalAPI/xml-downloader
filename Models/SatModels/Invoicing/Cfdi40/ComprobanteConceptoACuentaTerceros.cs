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
  
    public class ComprobanteConceptoACuentaTerceros
    {
        private string rfcACuentaTercerosField;

        private string nombreACuentaTercerosField;

        private string regimenFiscalACuentaTercerosField;

        private string domicilioFiscalACuentaTercerosField;


        [XmlAttribute]
        public string RfcACuentaTerceros
        {
            get { return rfcACuentaTercerosField; }
            set { rfcACuentaTercerosField = value; }
        }


        [XmlAttribute]
        public string NombreACuentaTerceros
        {
            get { return nombreACuentaTercerosField; }
            set { nombreACuentaTercerosField = value; }
        }


        [XmlAttribute]
        public string RegimenFiscalACuentaTerceros
        {
            get { return regimenFiscalACuentaTercerosField; }
            set { regimenFiscalACuentaTercerosField = value; }
        }


        [XmlAttribute]
        public string DomicilioFiscalACuentaTerceros
        {
            get { return domicilioFiscalACuentaTercerosField; }
            set { domicilioFiscalACuentaTercerosField = value; }
        }
    }
}