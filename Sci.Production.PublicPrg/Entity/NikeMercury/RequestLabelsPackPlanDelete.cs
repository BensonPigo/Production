using System;
using System.Xml.Serialization;

namespace Sci.Production.Prg.Entity.NikeMercury
{
    /// <summary>
    /// RequestLabelsPackPlanCreate
    /// </summary>
    public class RequestLabelsPackPlanDelete
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
            /// LabelsPackPlanDelete
            /// </summary>
            [XmlElement(ElementName = "LabelsPackPlanDelete", Namespace = "http://tempuri.org/")]
            public LabelsPackPlanDelete LabelsPackPlanDelete { get; set; }
        }

        /// <summary>
        /// LabelsPackPlanDelete
        /// </summary>
        [XmlRoot(ElementName = "LabelsPackPlanDelete", Namespace = "http://tempuri.org/")]
        public class LabelsPackPlanDelete
        {
            /// <summary>
            /// Input
            /// </summary>
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
            /// OrderNumber
            /// </summary>
            [XmlElement(ElementName = "OrderNumber", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public string OrderNumber { get; set; }

            /// <summary>
            /// OrderItem
            /// </summary>
            [XmlElement(ElementName = "OrderItem", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public string OrderItem { get; set; }
        }
    }
}
