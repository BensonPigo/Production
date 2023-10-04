using System;
using System.Xml.Serialization;

namespace Sci.Production.Prg.Entity
{
    /// <summary>
    /// RequestLabelsPackPlanCreate
    /// </summary>
    public class RequestLabelsPackPlanCreate
    {
        /// <summary>
        /// Represents the SOAP Envelope.
        /// </summary>
        [XmlRoot("Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public class Envelope
        {
            /// <summary>
            /// Header
            /// </summary>
            [XmlElement("Header", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
            public Header Header { get; set; }

            /// <summary>
            /// Body
            /// </summary>
            [XmlElement("Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
            public Body Body { get; set; }
        }

        /// <summary>
        /// Represents the SOAP Header.
        /// </summary>
        public class Header { }

        /// <summary>
        /// Represents the SOAP Body.
        /// </summary>
        public class Body
        {
            /// <summary>
            /// LabelsPackPlanCreate
            /// </summary>
            [XmlElement("LabelsPackPlanCreate", Namespace = "http://tempuri.org/")]
            public LabelsPackPlanCreate LabelsPackPlanCreate { get; set; }
        }

        /// <summary>
        /// Represents the request for LabelsPackPlanCreate operation.
        /// </summary>
        public class LabelsPackPlanCreate
        {
            /// <summary>
            /// input
            /// </summary>
            [XmlElement("input", Namespace = "http://tempuri.org/")]
            public Input input { get; set; }
        }

        /// <summary>
        /// Represents the input data for the request.
        /// </summary>
        public class Input
        {
            /// <summary>
            /// Gets or sets the factory code.
            /// </summary>
            [XmlElement("FactoryCode", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public string FactoryCode { get; set; }

            /// <summary>
            /// Gets or sets the order number.
            /// </summary>
            [XmlElement("OrderNumber", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public string OrderNumber { get; set; }

            /// <summary>
            /// Gets or sets the order item.
            /// </summary>
            [XmlElement("OrderItem", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public string OrderItem { get; set; }

            /// <summary>
            /// Gets or sets the single size per carton flag.
            /// </summary>
            [XmlElement("SingleSizePerCartonFlag", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public int SingleSizePerCartonFlag { get; set; }

            /// <summary>
            /// Gets or sets the size data.
            /// </summary>
            [XmlElement("SizeData", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public SizeData SizeData { get; set; }
        }

        /// <summary>
        /// Represents the size data for the request.
        /// </summary>
        public class SizeData
        {
            /// <summary>
            /// Gets or sets the pack plan order size array.
            /// </summary>
            [XmlElement("PackPlanOrderSize", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public PackPlanOrderSize[] PackPlanOrderSize { get; set; }
        }

        /// <summary>
        /// Represents the order size data.
        /// </summary>
        public class PackPlanOrderSize
        {
            /// <summary>
            /// Gets or sets the description.
            /// </summary>
            [XmlElement("Description", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public string Description { get; set; }

            /// <summary>
            /// Gets or sets the build quantity.
            /// </summary>
            [XmlElement("BuildQty", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public int BuildQty { get; set; }

            /// <summary>
            /// Gets or sets the carton pack factor.
            /// </summary>
            [XmlElement("CartonPackFactor", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public int CartonPackFactor { get; set; }

            /// <summary>
            /// Gets or sets the carton type code.
            /// </summary>
            [XmlElement("CartonTypeCode", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public string CartonTypeCode { get; set; }
        }
    }
}
