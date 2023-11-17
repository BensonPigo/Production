using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Sci.Production.Prg.Entity.NikeMercury
{
    /// <summary>
    /// LabelsPackPlanCartonAdd
    /// </summary>
    public class RequestLabelsPackPlanCartonUpdate
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
            /// LabelsPackPlanCartonUpdate
            /// </summary>
            [XmlElement(ElementName = "LabelsPackPlanCartonUpdate", Namespace = "http://tempuri.org/")]
            public LabelsPackPlanCartonUpdate LabelsPackPlanCartonUpdate { get; set; }
        }

        /// <summary>
        /// LabelsPackPlanCartonUpdate
        /// </summary>
        [XmlRoot(ElementName = "LabelsPackPlanCartonUpdate", Namespace = "http://tempuri.org/")]
        public class LabelsPackPlanCartonUpdate
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
            /// CartonNumber
            /// </summary>
            [XmlElement(ElementName = "CartonNumber", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public string CartonNumber { get; set; }

            /// <summary>
            /// CartonContent
            /// </summary>
            [XmlArray(ElementName = "CartonContent", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            [XmlArrayItem(ElementName = "CartonContent", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public List<CartonContent> CartonContent { get; set; }
        }

        /// <summary>
        /// CartonContent
        /// </summary>
        public class CartonContent
        {
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

            /// <summary>
            /// OrderSizeDescription
            /// </summary>
            [XmlElement(ElementName = "OrderSizeDescription", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public string OrderSizeDescription { get; set; }

            /// <summary>
            /// PackPlanQty
            /// </summary>
            [XmlElement(ElementName = "PackPlanQty", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public int PackPlanQty { get; set; }
        }
    }
}
