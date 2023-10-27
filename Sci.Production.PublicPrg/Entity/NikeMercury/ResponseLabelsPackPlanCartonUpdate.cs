using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Sci.Production.Prg.Entity.NikeMercury
{
    /// <summary>
    /// ResponseLabelsPackPlanCreate
    /// </summary>
    public class ResponseLabelsPackPlanCartonUpdate
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
            [XmlElement(ElementName = "LabelsPackPlanCartonUpdateResponse", Namespace = "http://tempuri.org/")]
            public LabelsPackPlanCartonUpdateResponse LabelsPackPlanCartonUpdateResponse { get; set; }
        }

        [XmlRoot(ElementName = "LabelsPackPlanCartonUpdateResponse", Namespace = "http://tempuri.org/")]
        public class LabelsPackPlanCartonUpdateResponse
        {
            [XmlElement(ElementName = "LabelsPackPlanCartonUpdateResult", Namespace = "http://tempuri.org/")]
            public LabelsPackPlanCartonUpdateResult LabelsPackPlanCartonUpdateResult { get; set; }
        }

        [XmlRoot(ElementName = "LabelsPackPlanCartonUpdateResult", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
        public class LabelsPackPlanCartonUpdateResult
        {
            [XmlElement(ElementName = "OutputMessage", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public OutputMessage OutputMessage { get; set; }

            [XmlElement(ElementName = "OutputData", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public OutputData OutputData { get; set; }
        }

        [XmlRoot(ElementName = "OutputMessage", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeFramework")]
        public class OutputMessage
        {
            [XmlElement(ElementName = "Cover")]
            public Cover Cover { get; set; }

            [XmlElement(ElementName = "ReturnCode")]
            public int ReturnCode { get; set; }

            [XmlElement(ElementName = "ReturnDescription")]
            public string ReturnDescription { get; set; }
        }

        public class Cover
        {
            [XmlElement(ElementName = "UuidReference")]
            public int UuidReference { get; set; }

            [XmlElement(ElementName = "BusinessTransactionID")]
            public int BusinessTransactionID { get; set; }

            [XmlElement(ElementName = "SenderID")]
            public string SenderID { get; set; }

            [XmlElement(ElementName = "ReceiverID")]
            public string ReceiverID { get; set; }

            [XmlElement(ElementName = "MessageType")]
            public string MessageType { get; set; }

            [XmlElement(ElementName = "ObjectType")]
            public string ObjectType { get; set; }
        }

        [XmlRoot(ElementName = "OutputData", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
        public class OutputData
        {
            [XmlElement(ElementName = "Carton")]
            public Carton Carton { get; set; }
        }

        public class Carton
        {
            [XmlElement(ElementName = "FactoryCode")]
            public string FactoryCode { get; set; }

            [XmlElement(ElementName = "CartonNumber")]
            public string CartonNumber { get; set; }

            [XmlElement(ElementName = "CartonBarcodeNumber")]
            public string CartonBarcodeNumber { get; set; }

            [XmlElement(ElementName = "CartonType")]
            public string CartonType { get; set; }

            [XmlElement(ElementName = "DimensionUOM")]
            public string DimensionUOM { get; set; }

            [XmlElement(ElementName = "Length")]
            public double Length { get; set; }

            [XmlElement(ElementName = "Width")]
            public double Width { get; set; }

            [XmlElement(ElementName = "Height")]
            public double Height { get; set; }

            [XmlElement(ElementName = "WeightUOM")]
            public string WeightUOM { get; set; }

            [XmlElement(ElementName = "GrossWeight")]
            public double GrossWeight { get; set; }

            [XmlElement(ElementName = "NetWeight")]
            public double NetWeight { get; set; }

            [XmlElement(ElementName = "Volume")]
            public double Volume { get; set; }

            [XmlElement(ElementName = "Content")]
            public Content Content { get; set; }
        }

        public class Content
        {
            [XmlElement(ElementName = "CartonContent")]
            public CartonContent CartonContent { get; set; }
        }

        public class CartonContent
        {
            [XmlElement(ElementName = "OrderNumber")]
            public string OrderNumber { get; set; }

            [XmlElement(ElementName = "OrderItem")]
            public string OrderItem { get; set; }

            [XmlElement(ElementName = "OrderSizeDescription")]
            public string OrderSizeDescription { get; set; }

            [XmlElement(ElementName = "PackPlanQty")]
            public int PackPlanQty { get; set; }

            [XmlElement(ElementName = "MaterialNumber")]
            public string MaterialNumber { get; set; }

            [XmlElement(ElementName = "UPC")]
            public string UPC { get; set; }
        }
    }
}
