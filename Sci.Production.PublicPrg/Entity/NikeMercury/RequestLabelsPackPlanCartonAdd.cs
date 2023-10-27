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

        public class Header
        {
            // You can add properties for header elements here if needed.
        }

        public class Body
        {
            [XmlElement(Namespace = "http://tempuri.org/")]
            public LabelsPackPlanCartonAdd LabelsPackPlanCartonAdd { get; set; }
        }

        [XmlRoot(Namespace = "http://tempuri.org/", ElementName = "LabelsPackPlanCartonAdd")]
        public class LabelsPackPlanCartonAdd
        {
            [XmlElement("input", Namespace = "http://tempuri.org/")]
            public Input Input { get; set; }
        }

        public class Input
        {
            [XmlElement(Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public string FactoryCode { get; set; }

            [XmlElement(Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public string CartonTypeCode { get; set; }

            [XmlElement(Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public AddCartonContent AddCartonContent { get; set; }
        }

        public class AddCartonContent
        {
            [XmlElement(Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public List<AddCartonContentInput> AddCartonContentInput { get; set; }
        }

        public class AddCartonContentInput
        {
            [XmlElement(Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public string OrderNumber { get; set; }

            [XmlElement(Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public string OrderItem { get; set; }

            [XmlElement(Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public string OrderSizeDescription { get; set; }

            [XmlElement(Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public int PackPlanQty { get; set; }
        }
    }
}
