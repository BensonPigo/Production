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
            [XmlElement(ElementName = "LabelsPackPlanCartonUpdate", Namespace = "http://tempuri.org/")]
            public LabelsPackPlanCartonUpdate LabelsPackPlanCartonUpdate { get; set; }
        }

        [XmlRoot(ElementName = "LabelsPackPlanCartonUpdate", Namespace = "http://tempuri.org/")]
        public class LabelsPackPlanCartonUpdate
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

            [XmlArray(ElementName = "CartonContent", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            [XmlArrayItem(ElementName = "CartonContent", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public List<CartonContent> CartonContent { get; set; }
        }

        public class CartonContent
        {
            [XmlElement(ElementName = "OrderNumber", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public string OrderNumber { get; set; }

            [XmlElement(ElementName = "OrderItem", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public string OrderItem { get; set; }

            [XmlElement(ElementName = "OrderSizeDescription", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public string OrderSizeDescription { get; set; }

            [XmlElement(ElementName = "PackPlanQty", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public int PackPlanQty { get; set; }
        }
    }
}
