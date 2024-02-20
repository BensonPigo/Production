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
            /// <summary>
            /// Body
            /// </summary>
            [XmlElement(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
            public Body Body { get; set; }
        }

        /// <summary>
        /// Body
        /// </summary>
        public class Body
        {
            /// <summary>
            /// LabelsPackPlanCartonUpdateResponse
            /// </summary>
            [XmlElement(ElementName = "LabelsPackPlanCartonUpdateResponse", Namespace = "http://tempuri.org/")]
            public LabelsPackPlanCartonUpdateResponse LabelsPackPlanCartonUpdateResponse { get; set; }
        }

        /// <summary>
        /// LabelsPackPlanCartonUpdateResponse
        /// </summary>
        [XmlRoot(ElementName = "LabelsPackPlanCartonUpdateResponse", Namespace = "http://tempuri.org/")]
        public class LabelsPackPlanCartonUpdateResponse
        {
            /// <summary>
            /// LabelsPackPlanCartonUpdateResult
            /// </summary>
            [XmlElement(ElementName = "LabelsPackPlanCartonUpdateResult", Namespace = "http://tempuri.org/")]
            public LabelsPackPlanCartonUpdateResult LabelsPackPlanCartonUpdateResult { get; set; }
        }

        /// <summary>
        /// LabelsPackPlanCartonUpdateResult
        /// </summary>
        [XmlRoot(ElementName = "LabelsPackPlanCartonUpdateResult", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
        public class LabelsPackPlanCartonUpdateResult
        {
            /// <summary>
            /// OutputMessage
            /// </summary>
            [XmlElement(ElementName = "OutputMessage", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public OutputMessage OutputMessage { get; set; }

            /// <summary>
            /// OutputData
            /// </summary>
            [XmlElement(ElementName = "OutputData", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public OutputData OutputData { get; set; }
        }

        /// <summary>
        /// OutputMessage
        /// </summary>
        [XmlRoot(ElementName = "OutputMessage", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeFramework")]
        public class OutputMessage
        {
            /// <summary>
            /// Cover
            /// </summary>
            [XmlElement(ElementName = "Cover")]
            public Cover Cover { get; set; }

            /// <summary>
            /// ReturnCode
            /// </summary>
            [XmlElement(ElementName = "ReturnCode")]
            public int ReturnCode { get; set; }

            /// <summary>
            /// ReturnDescription
            /// </summary>
            [XmlElement(ElementName = "ReturnDescription")]
            public string ReturnDescription { get; set; }
        }

        /// <summary>
        /// Cover
        /// </summary>
        public class Cover
        {
            /// <summary>
            /// UuidReference
            /// </summary>
            [XmlElement(ElementName = "UuidReference")]
            public int UuidReference { get; set; }

            /// <summary>
            /// BusinessTransactionID
            /// </summary>
            [XmlElement(ElementName = "BusinessTransactionID")]
            public int BusinessTransactionID { get; set; }

            /// <summary>
            /// SenderID
            /// </summary>
            [XmlElement(ElementName = "SenderID")]
            public string SenderID { get; set; }

            /// <summary>
            /// ReceiverID
            /// </summary>
            [XmlElement(ElementName = "ReceiverID")]
            public string ReceiverID { get; set; }

            /// <summary>
            /// MessageType
            /// </summary>
            [XmlElement(ElementName = "MessageType")]
            public string MessageType { get; set; }

            /// <summary>
            /// ObjectType
            /// </summary>
            [XmlElement(ElementName = "ObjectType")]
            public string ObjectType { get; set; }
        }

        /// <summary>
        /// OutputData
        /// </summary>
        [XmlRoot(ElementName = "OutputData", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
        public class OutputData
        {
            /// <summary>
            /// Carton
            /// </summary>
            [XmlElement(ElementName = "Carton")]
            public Carton Carton { get; set; }
        }

        /// <summary>
        /// Carton
        /// </summary>
        public class Carton
        {
            /// <summary>
            /// FactoryCode
            /// </summary>
            [XmlElement(ElementName = "FactoryCode")]
            public string FactoryCode { get; set; }

            /// <summary>
            /// CartonNumber
            /// </summary>
            [XmlElement(ElementName = "CartonNumber")]
            public string CartonNumber { get; set; }

            /// <summary>
            /// CartonBarcodeNumber
            /// </summary>
            [XmlElement(ElementName = "CartonBarcodeNumber")]
            public string CartonBarcodeNumber { get; set; }

            /// <summary>
            /// CartonType
            /// </summary>
            [XmlElement(ElementName = "CartonType")]
            public string CartonType { get; set; }

            /// <summary>
            /// DimensionUOM
            /// </summary>
            [XmlElement(ElementName = "DimensionUOM")]
            public string DimensionUOM { get; set; }

            /// <summary>
            /// Length
            /// </summary>
            [XmlElement(ElementName = "Length")]
            public double Length { get; set; }

            /// <summary>
            /// Width
            /// </summary>
            [XmlElement(ElementName = "Width")]
            public double Width { get; set; }

            /// <summary>
            /// Height
            /// </summary>
            [XmlElement(ElementName = "Height")]
            public double Height { get; set; }

            /// <summary>
            /// WeightUOM
            /// </summary>
            [XmlElement(ElementName = "WeightUOM")]
            public string WeightUOM { get; set; }

            /// <summary>
            /// GrossWeight
            /// </summary>
            [XmlElement(ElementName = "GrossWeight")]
            public double GrossWeight { get; set; }

            /// <summary>
            /// NetWeight
            /// </summary>
            [XmlElement(ElementName = "NetWeight")]
            public double NetWeight { get; set; }

            /// <summary>
            /// Volume
            /// </summary>
            [XmlElement(ElementName = "Volume")]
            public double Volume { get; set; }

            /// <summary>
            /// Content
            /// </summary>
            [XmlElement(ElementName = "Content")]
            public Content Content { get; set; }
        }

        /// <summary>
        /// Content
        /// </summary>
        public class Content
        {
            /// <summary>
            /// CartonContent
            /// </summary>
            [XmlElement(ElementName = "CartonContent")]
            public CartonContent CartonContent { get; set; }
        }

        /// <summary>
        /// CartonContent
        /// </summary>
        public class CartonContent
        {
            /// <summary>
            /// OrderNumber
            /// </summary>
            [XmlElement(ElementName = "OrderNumber")]
            public string OrderNumber { get; set; }

            /// <summary>
            /// OrderItem
            /// </summary>
            [XmlElement(ElementName = "OrderItem")]
            public string OrderItem { get; set; }

            /// <summary>
            /// OrderSizeDescription
            /// </summary>
            [XmlElement(ElementName = "OrderSizeDescription")]
            public string OrderSizeDescription { get; set; }

            /// <summary>
            /// PackPlanQty
            /// </summary>
            [XmlElement(ElementName = "PackPlanQty")]
            public int PackPlanQty { get; set; }

            /// <summary>
            /// MaterialNumber
            /// </summary>
            [XmlElement(ElementName = "MaterialNumber")]
            public string MaterialNumber { get; set; }

            /// <summary>
            /// UPC
            /// </summary>
            [XmlElement(ElementName = "UPC")]
            public string UPC { get; set; }
        }
    }
}
