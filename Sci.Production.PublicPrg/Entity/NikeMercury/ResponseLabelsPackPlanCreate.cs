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
            [XmlElement(Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
            public Body Body { get; set; }
        }

        public class Body
        {
            [XmlElement(Namespace = "http://tempuri.org/")]
            public LabelsPackPlanCreateResponse LabelsPackPlanCreateResponse { get; set; }
        }

        /// <summary>
        /// LabelsPackPlanCreateResponse
        /// </summary>
        public class LabelsPackPlanCreateResponse
        {
            [XmlElement(Namespace = "http://tempuri.org/")]
            public LabelsPackPlanCreateResult LabelsPackPlanCreateResult { get; set; }
        }

        /// <summary>
        /// LabelsPackPlanCreateResult
        /// </summary>
        [XmlRoot(Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
        public class LabelsPackPlanCreateResult
        {
            [XmlElement(Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public OutputMessage OutputMessage { get; set; }

            [XmlElement(ElementName = "OutputData", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public OutputData OutputData { get; set; }
        }

        /// <summary>
        /// OutputMessage
        /// </summary>
        public class OutputMessage
        {
            [XmlElement(Namespace = "http://schemas.datacontract.org/2004/07/OLLIeFramework")]
            public Cover Cover { get; set; }

            [XmlElement(Namespace = "http://schemas.datacontract.org/2004/07/OLLIeFramework")]
            public int ReturnCode { get; set; }

            [XmlElement(Namespace = "http://schemas.datacontract.org/2004/07/OLLIeFramework")]
            public string ReturnDescription { get; set; }
        }

        /// <summary>
        /// Cover
        /// </summary>
        public class Cover
        {
            public int UuidReference { get; set; }
            public int BusinessTransactionID { get; set; }
            public string SenderID { get; set; }
            public string ReceiverID { get; set; }
            public string MessageType { get; set; }
            public string ObjectType { get; set; }
        }

        /// <summary>
        /// OutputData
        /// </summary>
        public class OutputData
        {
            [XmlElement(ElementName = "Carton", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public List<Carton> Cartons { get; set; }
        }

        /// <summary>
        /// Carton
        /// </summary>
        public class Carton
        {
            public string FactoryCode { get; set; }
            public string CartonNumber { get; set; }
            public string CartonBarcodeNumber { get; set; }
            public string CartonType { get; set; }
            public string DimensionUOM { get; set; }
            public double Length { get; set; }
            public double Width { get; set; }
            public double Height { get; set; }
            public string WeightUOM { get; set; }
            public double GrossWeight { get; set; }
            public double NetWeight { get; set; }
            public double Volume { get; set; }
            public Content Content { get; set; }
        }

        public class Content
        {
            [XmlElement(ElementName = "CartonContent", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public List<CartonContent> CartonContent { get; set; }
        }

        public class CartonContent : IEquatable<CartonContent>
        {
            public string OrderSizeDescription { get; set; }
            public int PackPlanQty { get; set; }

            public bool Equals(CartonContent other)
            {
                if (other is null)
                    return false;

                return this.OrderSizeDescription == other.OrderSizeDescription && this.PackPlanQty == other.PackPlanQty;
            }

            public override bool Equals(object obj) => Equals(obj as CartonContent);
        }
    }
}
