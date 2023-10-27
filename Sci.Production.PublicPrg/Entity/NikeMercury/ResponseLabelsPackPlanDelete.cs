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
            [XmlElement(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
            public Body Body { get; set; }
        }

        public class Body
        {
            [XmlElement(ElementName = "LabelsPackPlanDeleteResponse", Namespace = "http://tempuri.org/")]
            public LabelsPackPlanDeleteResponse LabelsPackPlanDeleteResponse { get; set; }
        }

        [XmlRoot(ElementName = "LabelsPackPlanDeleteResponse", Namespace = "http://tempuri.org/")]
        public class LabelsPackPlanDeleteResponse
        {
            [XmlElement(ElementName = "LabelsPackPlanDeleteResult", Namespace = "http://tempuri.org/")]
            public LabelsPackPlanDeleteResult LabelsPackPlanDeleteResult { get; set; }
        }

        public class LabelsPackPlanDeleteResult
        {
            [XmlElement(ElementName = "OutputMessage", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLabels")]
            public OutputMessage OutputMessage { get; set; }
        }

        [XmlRoot(ElementName = "OutputMessage", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeFramework")]
        public class OutputMessage
        {
            [XmlElement(ElementName = "Cover", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeFramework")]
            public Cover Cover { get; set; }

            [XmlElement(ElementName = "ReturnCode", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeFramework")]
            public int ReturnCode { get; set; }

            [XmlElement(ElementName = "ReturnDescription", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeFramework")]
            public string ReturnDescription { get; set; }
        }

        [XmlRoot(ElementName = "Cover", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeFramework")]
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
    }
}
