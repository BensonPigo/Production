using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Sci.Production.Prg.Entity.NikeMercury
{
    /// <summary>
    /// ResponseLabelsPackPlanCreate
    /// </summary>
    public class ResponseLabelsPackPlanCartonDelete
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
            /// LabelsPackPlanCartonDeleteResponse
            /// </summary>
            [XmlElement(ElementName = "LabelsPackPlanCartonDeleteResponse", Namespace = "http://tempuri.org/")]
            public LabelsPackPlanCartonDeleteResponse LabelsPackPlanCartonDeleteResponse { get; set; }
        }

        /// <summary>
        /// LabelsPackPlanCartonDeleteResponse
        /// </summary>
        [XmlRoot(ElementName = "LabelsPackPlanCartonDeleteResponse", Namespace = "http://tempuri.org/")]
        public class LabelsPackPlanCartonDeleteResponse
        {
            /// <summary>
            /// LabelsPackPlanCartonDeleteResult
            /// </summary>
            [XmlElement(ElementName = "LabelsPackPlanCartonDeleteResult", Namespace = "http://tempuri.org/")]
            public LabelsPackPlanCartonDeleteResult LabelsPackPlanCartonDeleteResult { get; set; }
        }

        /// <summary>
        /// LabelsPackPlanCartonDeleteResult
        /// </summary>
        public class LabelsPackPlanCartonDeleteResult
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
    }
}
