using System;
using System.Xml.Serialization;

namespace Sci.Production.Prg.Entity.NikeMercury
{
    /// <summary>
    /// RequestLabelsPackPlanCreate
    /// </summary>
    public class RequestLabelsGS1128CartonPrintByCartonRange
    {
        /// <summary>
        /// Represents the SOAP Envelope.
        /// </summary>
        [XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public class Envelope
        {
            /// <summary>
            /// Header
            /// </summary>
            [XmlElement(ElementName = "Header", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
            public Header Header { get; set; }

            /// <summary>
            /// Body
            /// </summary>
            [XmlElement(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
            public Body Body { get; set; }
        }

        /// <summary>
        /// Header
        /// </summary>
        public class Header
        {
        }

        /// <summary>
        /// Body
        /// </summary>
        public class Body
        {
            /// <summary>
            /// LabelsGS1128CartonPrintByCartonRange
            /// </summary>
            [XmlElement(ElementName = "LabelsGS1128CartonPrintByCartonRange", Namespace = "http://tempuri.org/")]
            public LabelsGS1128CartonPrintByCartonRange LabelsGS1128CartonPrintByCartonRange { get; set; }
        }

        /// <summary>
        /// LabelsGS1128CartonPrintByCartonRange
        /// </summary>
        [XmlRoot(ElementName = "LabelsGS1128CartonPrintByCartonRange", Namespace = "http://tempuri.org/")]
        public class LabelsGS1128CartonPrintByCartonRange
        {
            [XmlElement(ElementName = "input", Namespace = "http://tempuri.org/")]
            public Input Input { get; set; }
        }

        /// <summary>
        /// Input
        /// </summary>
        public class Input
        {
            /// <summary>
            /// FactoryCode
            /// </summary>
            [XmlElement(ElementName = "FactoryCode", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public string FactoryCode { get; set; }

            /// <summary>
            /// PrintServerName
            /// </summary>
            [XmlElement(ElementName = "PrintServerName", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public string PrintServerName { get; set; }

            /// <summary>
            /// CartonNumberFrom
            /// </summary>
            [XmlElement(ElementName = "CartonNumberFrom", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public string CartonNumberFrom { get; set; }
        }
    }
}
