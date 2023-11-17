using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Sci.Production.Prg.Entity.NikeMercury
{
    /// <summary>
    /// ResponseLabelsPackPlanCreate
    /// </summary>
    public class ResponseLabelsPackPlanCreate
    {
        /// <summary>
        /// Envelope
        /// </summary>
        // 指定 XML 命名空間
        [XmlRoot(Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public class Envelope
        {
            /// <summary>
            /// Body
            /// </summary>
            [XmlElement(Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
            public Body Body { get; set; }
        }

        /// <summary>
        /// Body
        /// </summary>
        public class Body
        {
            /// <summary>
            /// LabelsPackPlanCreateResponse
            /// </summary>
            [XmlElement(Namespace = "http://tempuri.org/")]
            public LabelsPackPlanCreateResponse LabelsPackPlanCreateResponse { get; set; }
        }

        /// <summary>
        /// LabelsPackPlanCreateResponse
        /// </summary>
        public class LabelsPackPlanCreateResponse
        {
            /// <summary>
            /// LabelsPackPlanCreateResult
            /// </summary>
            [XmlElement(Namespace = "http://tempuri.org/")]
            public LabelsPackPlanCreateResult LabelsPackPlanCreateResult { get; set; }
        }

        /// <summary>
        /// LabelsPackPlanCreateResult
        /// </summary>
        [XmlRoot(Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
        public class LabelsPackPlanCreateResult
        {
            /// <summary>
            /// OutputMessage
            /// </summary>
            [XmlElement(Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
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
            [XmlElement(Namespace = "http://schemas.datacontract.org/2004/07/OLLIeFramework")]
            public Cover Cover { get; set; }

            /// <summary>
            /// ReturnCode
            /// </summary>
            [XmlElement(Namespace = "http://schemas.datacontract.org/2004/07/OLLIeFramework")]
            public int ReturnCode { get; set; }

            /// <summary>
            /// ReturnDescription
            /// </summary>
            [XmlElement(Namespace = "http://schemas.datacontract.org/2004/07/OLLIeFramework")]
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
            public int UuidReference { get; set; }

            /// <summary>
            /// BusinessTransactionID
            /// </summary>
            public int BusinessTransactionID { get; set; }

            /// <summary>
            /// SenderID
            /// </summary>
            public string SenderID { get; set; }

            /// <summary>
            /// ReceiverID
            /// </summary>
            public string ReceiverID { get; set; }

            /// <summary>
            /// MessageType
            /// </summary>
            public string MessageType { get; set; }

            /// <summary>
            /// ObjectType
            /// </summary>
            public string ObjectType { get; set; }
        }

        /// <summary>
        /// OutputData
        /// </summary>
        public class OutputData
        {
            /// <summary>
            /// Cartons
            /// </summary>
            [XmlElement(ElementName = "Carton", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public List<Carton> Cartons { get; set; }
        }

        /// <summary>
        /// Carton
        /// </summary>
        public class Carton
        {
            /// <summary>
            /// FactoryCode
            /// </summary>
            public string FactoryCode { get; set; }

            /// <summary>
            /// CartonNumber
            /// </summary>
            public string CartonNumber { get; set; }

            /// <summary>
            /// CartonBarcodeNumber
            /// </summary>
            public string CartonBarcodeNumber { get; set; }

            /// <summary>
            /// CartonType
            /// </summary>
            public string CartonType { get; set; }

            /// <summary>
            /// DimensionUOM
            /// </summary>
            public string DimensionUOM { get; set; }

            /// <summary>
            /// Length
            /// </summary>
            public double Length { get; set; }

            /// <summary>
            /// Width
            /// </summary>
            public double Width { get; set; }

            /// <summary>
            /// Height
            /// </summary>
            public double Height { get; set; }

            /// <summary>
            /// WeightUOM
            /// </summary>
            public string WeightUOM { get; set; }

            /// <summary>
            /// GrossWeight
            /// </summary>
            public double GrossWeight { get; set; }

            /// <summary>
            /// NetWeight
            /// </summary>
            public double NetWeight { get; set; }

            /// <summary>
            /// Volume
            /// </summary>
            public double Volume { get; set; }

            /// <summary>
            /// Content
            /// </summary>
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
            [XmlElement(ElementName = "CartonContent", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public List<CartonContent> CartonContent { get; set; }
        }

        /// <summary>
        /// CartonContent
        /// </summary>
        public class CartonContent : IEquatable<CartonContent>
        {
            /// <summary>
            /// OrderSizeDescription
            /// </summary>
            public string OrderSizeDescription { get; set; }

            /// <summary>
            /// PackPlanQty
            /// </summary>
            public int PackPlanQty { get; set; }

            /// <summary>
            /// Equals
            /// </summary>
            /// <param name="other">other</param>
            /// <returns>bool</returns>
            public bool Equals(CartonContent other)
            {
                if (other is null)
                {
                    return false;
                }

                return this.OrderSizeDescription == other.OrderSizeDescription && this.PackPlanQty == other.PackPlanQty;
            }

            /// <summary>
            /// Equals
            /// </summary>
            /// <param name="obj">obj</param>
            /// <returns>bool</returns>
            public override bool Equals(object obj) => this.Equals(obj as CartonContent);
        }
    }
}
