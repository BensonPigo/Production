using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Sci.Production.Prg.Entity.NikeMercury
{
    /// <summary>
    /// ResponseLabelsPackPlanCreate
    /// </summary>
    public class ResponseLabelsPackPlanCartonAdd
    {
        /// <summary>
        /// Envelope
        /// </summary>
        // 指定 XML 命名空間
        [XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public class Envelope
        {
            [XmlElement(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
            public Body Body { get; set; }
        }

        public class Body
        {
            [XmlElement(ElementName = "LabelsPackPlanCartonAddResponse", Namespace = "http://tempuri.org/")]
            public LabelsPackPlanCartonAddResponse LabelsPackPlanCartonAddResponse { get; set; }
        }

        [XmlRoot(ElementName = "LabelsPackPlanCartonAddResponse", Namespace = "http://tempuri.org/")]
        public class LabelsPackPlanCartonAddResponse
        {
            [XmlElement(ElementName = "LabelsPackPlanCartonAddResult", Namespace = "http://tempuri.org/")]
            public LabelsPackPlanCartonAddResult LabelsPackPlanCartonAddResult { get; set; }
        }

        [XmlRoot(ElementName = "LabelsPackPlanCartonAddResult", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
        public class LabelsPackPlanCartonAddResult
        {
            [XmlElement(ElementName = "OutputMessage", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public OutputMessage OutputMessage { get; set; }

            [XmlElement(ElementName = "OutputData", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public OutputData OutputData { get; set; }
        }

        public class OutputMessage
        {
            [XmlElement(ElementName = "Cover", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeFramework")]
            public Cover Cover { get; set; }

            [XmlElement(ElementName = "ReturnCode", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeFramework")]
            public int ReturnCode { get; set; }

            [XmlElement(ElementName = "ReturnDescription", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeFramework")]
            public string ReturnDescription { get; set; }
        }

        public class Cover
        {
            [XmlElement(ElementName = "UuidReference", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeFramework")]
            public int UuidReference { get; set; }

            [XmlElement(ElementName = "BusinessTransactionID", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeFramework")]
            public int BusinessTransactionID { get; set; }

            [XmlElement(ElementName = "SenderID", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeFramework")]
            public string SenderID { get; set; }

            [XmlElement(ElementName = "ReceiverID", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeFramework")]
            public string ReceiverID { get; set; }

            [XmlElement(ElementName = "MessageType", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeFramework")]
            public string MessageType { get; set; }

            [XmlElement(ElementName = "ObjectType", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeFramework")]
            public string ObjectType { get; set; }
        }

        public class OutputData
        {
            [XmlElement(ElementName = "Carton", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public Carton Carton { get; set; }
        }

        public class Carton
        {
            [XmlElement(ElementName = "FactoryCode", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public string FactoryCode { get; set; }

            [XmlElement(ElementName = "CartonNumber", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public string CartonNumber { get; set; }

            [XmlElement(ElementName = "CartonBarcodeNumber", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public string CartonBarcodeNumber { get; set; }

            [XmlElement(ElementName = "CartonType", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public string CartonType { get; set; }

            [XmlElement(ElementName = "DimensionUOM", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public string DimensionUOM { get; set; }

            [XmlElement(ElementName = "Length", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public double Length { get; set; }

            [XmlElement(ElementName = "Width", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public double Width { get; set; }

            [XmlElement(ElementName = "Height", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public double Height { get; set; }

            [XmlElement(ElementName = "WeightUOM", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public string WeightUOM { get; set; }

            [XmlElement(ElementName = "GrossWeight", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public double GrossWeight { get; set; }

            [XmlElement(ElementName = "NetWeight", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public double NetWeight { get; set; }

            [XmlElement(ElementName = "Volume", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public double Volume { get; set; }

            [XmlElement(ElementName = "Content", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public Content Content { get; set; }
        }

        public class Content
        {
            [XmlElement(ElementName = "CartonContent", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public List<CartonContent> CartonContents { get; set; }
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

            [XmlElement(ElementName = "MaterialNumber", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public string MaterialNumber { get; set; }

            [XmlElement(ElementName = "UPC", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public string UPC { get; set; }
        }
    }
}
