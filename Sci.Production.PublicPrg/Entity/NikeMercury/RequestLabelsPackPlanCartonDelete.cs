using System;
using System.Xml.Serialization;

namespace Sci.Production.Prg.Entity.NikeMercury
{
    /// <summary>
    /// RequestLabelsPackPlanCreate
    /// </summary>
    public class RequestLabelsPackPlanCartonDelete
    {
        /// <summary>
        /// Represents the SOAP Envelope.
        /// </summary>
        [XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public class Envelope
        {
            [XmlElement(ElementName = "Header", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
            public Header Header { get; set; }

            [XmlElement(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
            public Body Body { get; set; }
        }

        public class Header
        {
        }

        public class Body
        {
            [XmlElement(ElementName = "LabelsPackPlanCartonDelete", Namespace = "http://tempuri.org/")]
            public LabelsPackPlanCartonDelete LabelsPackPlanCartonDelete { get; set; }
        }

        [XmlRoot(ElementName = "LabelsPackPlanCartonDelete", Namespace = "http://tempuri.org/")]
        public class LabelsPackPlanCartonDelete
        {
            [XmlElement(ElementName = "input", Namespace = "http://tempuri.org/")]
            public Input Input { get; set; }
        }

        public class Input
        {
            [XmlElement(ElementName = "FactoryCode", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public string FactoryCode { get; set; }

            [XmlElement(ElementName = "CartonNumber", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public string CartonNumber { get; set; }
        }
    }
}
