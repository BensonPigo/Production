using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Sci.Production.Prg.Entity.NikeMercury
{
    /// <summary>
    /// ResponseLabelsPackPlanCreate
    /// </summary>
    public class ResponseLabelsGS1128CartonPrintByCartonRange
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
            [XmlElement(ElementName = "LabelsGS1128CartonPrintByCartonRangeResponse", Namespace = "http://tempuri.org/")]
            public LabelsGS1128CartonPrintByCartonRangeResponse LabelsGS1128CartonPrintByCartonRangeResponse { get; set; }
        }

        [XmlRoot(ElementName = "LabelsGS1128CartonPrintByCartonRangeResponse", Namespace = "http://tempuri.org/")]
        public class LabelsGS1128CartonPrintByCartonRangeResponse
        {
            [XmlElement(ElementName = "LabelsGS1128CartonPrintByCartonRangeResult", Namespace = "http://tempuri.org/")]
            public LabelsGS1128CartonPrintByCartonRangeResult LabelsGS1128CartonPrintByCartonRangeResult { get; set; }
        }

        public class LabelsGS1128CartonPrintByCartonRangeResult
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
