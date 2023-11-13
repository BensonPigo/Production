using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Sci.Production.Prg.Entity.NikeMercury
{
    /// <summary>
    /// LabelsPackPlanCartonAdd
    /// </summary>
    public class RequestLabelsPackPlanCartonAdd
    {
        /// <summary>
        /// Represents the SOAP Envelope.
        /// </summary>
        [XmlRoot(Namespace = "http://schemas.xmlsoap.org/soap/envelope/", ElementName = "Envelope")]
        public class Envelope
        {
            [XmlElement(Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
            public Header Header { get; set; }

            [XmlElement(Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
            public Body Body { get; set; }
        }

        /// <summary>
        /// Header
        /// </summary>
        public class Header
        {
            // You can add properties for header elements here if needed.
        }

        /// <summary>
        /// Body
        /// </summary>
        public class Body
        {
            /// <summary>
            /// LabelsPackPlanCartonAdd
            /// </summary>
            [XmlElement(Namespace = "http://tempuri.org/")]
            public LabelsPackPlanCartonAdd LabelsPackPlanCartonAdd { get; set; }
        }

        /// <summary>
        /// LabelsPackPlanCartonAdd
        /// </summary>
        [XmlRoot(Namespace = "http://tempuri.org/", ElementName = "LabelsPackPlanCartonAdd")]
        public class LabelsPackPlanCartonAdd
        {
            /// <summary>
            /// Input
            /// </summary>
            [XmlElement("input", Namespace = "http://tempuri.org/")]
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
            [XmlElement(Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public string FactoryCode { get; set; }

            /// <summary>
            /// CartonTypeCode
            /// </summary>
            [XmlElement(Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public string CartonTypeCode { get; set; }

            /// <summary>
            /// AddCartonContent
            /// </summary>
            [XmlElement(Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public AddCartonContent AddCartonContent { get; set; }
        }

        /// <summary>
        /// AddCartonContent
        /// </summary>
        public class AddCartonContent
        {
            /// <summary>
            /// AddCartonContentInput
            /// </summary>
            [XmlElement(Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public List<AddCartonContentInput> AddCartonContentInput { get; set; }
        }

        /// <summary>
        /// AddCartonContentInput
        /// </summary>
        public class AddCartonContentInput
        {
            /// <summary>
            /// OrderNumber
            /// </summary>
            [XmlElement(Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public string OrderNumber { get; set; }

            /// <summary>
            /// OrderItem
            /// </summary>
            [XmlElement(Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public string OrderItem { get; set; }

            /// <summary>
            /// OrderSizeDescription
            /// </summary>
            [XmlElement(Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public string OrderSizeDescription { get; set; }

            /// <summary>
            /// PackPlanQty
            /// </summary>
            [XmlElement(Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public int PackPlanQty { get; set; }
        }
    }
}
