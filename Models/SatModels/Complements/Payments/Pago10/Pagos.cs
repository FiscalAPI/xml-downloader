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

namespace Fiscalapi.XmlDownloader.Models.SatModels.Complements.Payments.Pago10
{


    [Serializable,
     XmlType(AnonymousType = true, Namespace = "http://www.sat.gob.mx/Pagos"),
     XmlRoot(Namespace = "http://www.sat.gob.mx/Pagos", IsNullable = false)]
    public class Pagos
    {
        private PagosPago[] pagoField;

        private string versionField;

        public Pagos()
        {
            versionField = "1.0";
        }

        
        [XmlElement("Pago")]
        public PagosPago[] Pago
        {
            get { return pagoField; }
            set { pagoField = value; }
        }

        
        [XmlAttribute]
        public string Version
        {
            get { return versionField; }
            set { versionField = value; }
        }
    }
}