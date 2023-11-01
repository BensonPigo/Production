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

            [XmlElement(ElementName = "ReturnCode", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeFramework")]
            public int ReturnCode { get; set; }

            [XmlElement(ElementName = "ReturnDescription", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeFramework")]
            public string ReturnDescription { get; set; }
        }
    }
}
