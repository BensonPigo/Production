using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Sci.Production.Prg.Entity.NikeMercury
{
    /// <summary>
    /// ResponseLabelsPackPlanCreate
    /// </summary>
    public class ResponseShipmentScannedCartonUploadLSP
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
            /// ShipmentScannedCartonUploadLSPResponse
            /// </summary>
            [XmlElement(ElementName = "ShipmentScannedCartonUploadLSPResponse", Namespace = "http://tempuri.org/")]
            public ShipmentScannedCartonUploadLSPResponse ShipmentScannedCartonUploadLSPResponse { get; set; }
        }

        /// <summary>
        /// ShipmentScannedCartonUploadLSPResponse
        /// </summary>
        [XmlRoot(ElementName = "ShipmentScannedCartonUploadLSPResponse", Namespace = "http://tempuri.org/")]
        public class ShipmentScannedCartonUploadLSPResponse
        {
            /// <summary>
            /// ShipmentScannedCartonUploadLSPResult
            /// </summary>
            [XmlElement(ElementName = "ShipmentScannedCartonUploadLSPResult", Namespace = "http://tempuri.org/")]
            public ShipmentScannedCartonUploadLSPResult ShipmentScannedCartonUploadLSPResult { get; set; }
        }

        /// <summary>
        /// ShipmentScannedCartonUploadLSPResult
        /// </summary>
        public class ShipmentScannedCartonUploadLSPResult
        {
            /// <summary>
            /// OutputMessage
            /// </summary>
            [XmlElement(ElementName = "OutputMessage", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLogistics")]
            public OutputMessage OutputMessage { get; set; }

            /// <summary>
            /// FactoryHubShipmentNumber
            /// </summary>
            [XmlElement(ElementName = "FactoryHubShipmentNumber", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLogistics")]
            public string FactoryHubShipmentNumber { get; set; }
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
        [XmlRoot(ElementName = "Cover", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeFramework")]
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
    }
}
