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
  
    public class ComprobanteConceptoImpuestos
    {
        private ComprobanteConceptoImpuestosTraslado[] trasladosField;

        private ComprobanteConceptoImpuestosRetencion[] retencionesField;


        [XmlArrayItem("Traslado", IsNullable = false)]
        public ComprobanteConceptoImpuestosTraslado[] Traslados
        {
            get { return trasladosField; }
            set { trasladosField = value; }
        }


        [XmlArrayItem("Retencion", IsNullable = false)]
        public ComprobanteConceptoImpuestosRetencion[] Retenciones
        {
            get { return retencionesField; }
            set { retencionesField = value; }
        }
    }
}