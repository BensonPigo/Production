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
            /// LabelsPackPlanCartonAddResponse
            /// </summary>
            [XmlElement(ElementName = "LabelsPackPlanCartonAddResponse", Namespace = "http://tempuri.org/")]
            public LabelsPackPlanCartonAddResponse LabelsPackPlanCartonAddResponse { get; set; }
        }

        /// <summary>
        /// LabelsPackPlanCartonAddResponse
        /// </summary>
        [XmlRoot(ElementName = "LabelsPackPlanCartonAddResponse", Namespace = "http://tempuri.org/")]
        public class LabelsPackPlanCartonAddResponse
        {
            /// <summary>
            /// LabelsPackPlanCartonAddResult
            /// </summary>
            [XmlElement(ElementName = "LabelsPackPlanCartonAddResult", Namespace = "http://tempuri.org/")]
            public LabelsPackPlanCartonAddResult LabelsPackPlanCartonAddResult { get; set; }
        }

        /// <summary>
        /// LabelsPackPlanCartonAddResult
        /// </summary>
        [XmlRoot(ElementName = "LabelsPackPlanCartonAddResult", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
        public class LabelsPackPlanCartonAddResult
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
        public class OutputMessage
        {
            /// <summary>
            /// Cover
            /// </summary>
            [XmlElement(ElementName = "Cover", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeFramework")]
            public Cover Cover { get; set; }

            /// <summary>
            /// ReturnCode
            /// </summary>
            [XmlElement(ElementName = "ReturnCode", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeFramework")]
            public int ReturnCode { get; set; }

            /// <summary>
            /// ReturnDescription
            /// </summary>
            [XmlElement(ElementName = "ReturnDescription", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeFramework")]
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
            [XmlElement(ElementName = "UuidReference", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeFramework")]
            public int UuidReference { get; set; }

            /// <summary>
            /// BusinessTransactionID
            /// </summary>
            [XmlElement(ElementName = "BusinessTransactionID", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeFramework")]
            public int BusinessTransactionID { get; set; }

            /// <summary>
            /// SenderID
            /// </summary>
            [XmlElement(ElementName = "SenderID", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeFramework")]
            public string SenderID { get; set; }

            /// <summary>
            /// ReceiverID
            /// </summary>
            [XmlElement(ElementName = "ReceiverID", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeFramework")]
            public string ReceiverID { get; set; }

            /// <summary>
            /// MessageType
            /// </summary>
            [XmlElement(ElementName = "MessageType", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeFramework")]
            public string MessageType { get; set; }

            /// <summary>
            /// ObjectType
            /// </summary>
            [XmlElement(ElementName = "ObjectType", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeFramework")]
            public string ObjectType { get; set; }
        }

        /// <summary>
        /// OutputData
        /// </summary>
        public class OutputData
        {
            /// <summary>
            /// Carton
            /// </summary>
            [XmlElement(ElementName = "Carton", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
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
            [XmlElement(ElementName = "FactoryCode", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public string FactoryCode { get; set; }

            /// <summary>
            /// CartonNumber
            /// </summary>
            [XmlElement(ElementName = "CartonNumber", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public string CartonNumber { get; set; }

            /// <summary>
            /// CartonBarcodeNumber
            /// </summary>
            [XmlElement(ElementName = "CartonBarcodeNumber", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public string CartonBarcodeNumber { get; set; }

            /// <summary>
            /// CartonType
            /// </summary>
            [XmlElement(ElementName = "CartonType", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public string CartonType { get; set; }

            /// <summary>
            /// DimensionUOM
            /// </summary>
            [XmlElement(ElementName = "DimensionUOM", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public string DimensionUOM { get; set; }

            /// <summary>
            /// Length
            /// </summary>
            [XmlElement(ElementName = "Length", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public double Length { get; set; }

            /// <summary>
            /// Width
            /// </summary>
            [XmlElement(ElementName = "Width", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public double Width { get; set; }

            /// <summary>
            /// Height
            /// </summary>
            [XmlElement(ElementName = "Height", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public double Height { get; set; }

            /// <summary>
            /// WeightUOM
            /// </summary>
            [XmlElement(ElementName = "WeightUOM", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public string WeightUOM { get; set; }

            /// <summary>
            /// GrossWeight
            /// </summary>
            [XmlElement(ElementName = "GrossWeight", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public double GrossWeight { get; set; }

            /// <summary>
            /// NetWeight
            /// </summary>
            [XmlElement(ElementName = "NetWeight", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public double NetWeight { get; set; }

            /// <summary>
            /// Volume
            /// </summary>
            [XmlElement(ElementName = "Volume", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public double Volume { get; set; }

            /// <summary>
            /// Content
            /// </summary>
            [XmlElement(ElementName = "Content", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public Content Content { get; set; }
        }

        /// <summary>
        /// Content
        /// </summary>
        public class Content
        {
            /// <summary>
            /// CartonContents
            /// </summary>
            [XmlElement(ElementName = "CartonContent", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public List<CartonContent> CartonContents { get; set; }
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

            /// <summary>
            /// MaterialNumber
            /// </summary>
            [XmlElement(ElementName = "MaterialNumber", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public string MaterialNumber { get; set; }

            /// <summary>
            /// UPC
            /// </summary>
            [XmlElement(ElementName = "UPC", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public string UPC { get; set; }
        }
    }
}
