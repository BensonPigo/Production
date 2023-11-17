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
            /// LabelsGS1128CartonPrintByCartonRangeResponse
            /// </summary>
            [XmlElement(ElementName = "LabelsGS1128CartonPrintByCartonRangeResponse", Namespace = "http://tempuri.org/")]
            public LabelsGS1128CartonPrintByCartonRangeResponse LabelsGS1128CartonPrintByCartonRangeResponse { get; set; }
        }

        /// <summary>
        /// LabelsGS1128CartonPrintByCartonRangeResponse
        /// </summary>
        [XmlRoot(ElementName = "LabelsGS1128CartonPrintByCartonRangeResponse", Namespace = "http://tempuri.org/")]
        public class LabelsGS1128CartonPrintByCartonRangeResponse
        {
            /// <summary>
            /// LabelsGS1128CartonPrintByCartonRangeResult
            /// </summary>
            [XmlElement(ElementName = "LabelsGS1128CartonPrintByCartonRangeResult", Namespace = "http://tempuri.org/")]
            public LabelsGS1128CartonPrintByCartonRangeResult LabelsGS1128CartonPrintByCartonRangeResult { get; set; }
        }

        /// <summary>
        /// LabelsGS1128CartonPrintByCartonRangeResult
        /// </summary>
        public class LabelsGS1128CartonPrintByCartonRangeResult
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
