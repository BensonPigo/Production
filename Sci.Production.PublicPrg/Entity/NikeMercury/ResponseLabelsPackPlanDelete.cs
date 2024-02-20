using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Sci.Production.Prg.Entity.NikeMercury
{
    /// <summary>
    /// ResponseLabelsPackPlanCreate
    /// </summary>
    public class ResponseLabelsPackPlanDelete
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
            /// LabelsPackPlanDeleteResponse
            /// </summary>
            [XmlElement(ElementName = "LabelsPackPlanDeleteResponse", Namespace = "http://tempuri.org/")]
            public LabelsPackPlanDeleteResponse LabelsPackPlanDeleteResponse { get; set; }
        }

        /// <summary>
        /// LabelsPackPlanDeleteResponse
        /// </summary>
        [XmlRoot(ElementName = "LabelsPackPlanDeleteResponse", Namespace = "http://tempuri.org/")]
        public class LabelsPackPlanDeleteResponse
        {
            /// <summary>
            /// LabelsPackPlanDeleteResult
            /// </summary>
            [XmlElement(ElementName = "LabelsPackPlanDeleteResult", Namespace = "http://tempuri.org/")]
            public LabelsPackPlanDeleteResult LabelsPackPlanDeleteResult { get; set; }
        }

        /// <summary>
        /// LabelsPackPlanDeleteResult
        /// </summary>
        public class LabelsPackPlanDeleteResult
        {
            /// <summary>
            /// OutputMessage
            /// </summary>
            [XmlElement(ElementName = "OutputMessage", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public OutputMessage OutputMessage { get; set; }
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
