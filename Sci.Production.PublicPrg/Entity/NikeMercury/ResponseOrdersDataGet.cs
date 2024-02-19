using System;
using System.Xml.Serialization;

namespace Sci.Production.Prg.Entity.NikeMercury
{
    /// <summary>
    /// RequestLabelsPackPlanCreate
    /// </summary>
    public class ResponseOrdersDataGet
    {
        /// <summary>
        /// Envelope
        /// </summary>
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
            /// OrdersDataGetResponse
            /// </summary>
            [XmlElement(ElementName = "OrdersDataGetResponse", Namespace = "http://tempuri.org/")]
            public OrdersDataGetResponse OrdersDataGetResponse { get; set; }
        }

        /// <summary>
        /// OrdersDataGetResponse
        /// </summary>
        public class OrdersDataGetResponse
        {
            /// <summary>
            /// OrdersDataGetResult
            /// </summary>
            [XmlElement(ElementName = "OrdersDataGetResult", Namespace = "http://tempuri.org/")]
            public OrdersDataGetResult OrdersDataGetResult { get; set; }
        }

        /// <summary>
        /// OrdersDataGetResult
        /// </summary>
        public class OrdersDataGetResult
        {
            /// <summary>
            /// OutputData
            /// </summary>
            [XmlElement(ElementName = "OutputData", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeOrders")]
            public OutputData OutputData { get; set; }
        }

        /// <summary>
        /// OutputData
        /// </summary>
        public class OutputData
        {
            /// <summary>
            /// DataGetOrderItems
            /// </summary>
            [XmlArray(ElementName = "OutputDataSet2")]
            [XmlArrayItem(ElementName = "DataGetOrderItems")]
            public DataGetOrderItems[] DataGetOrderItems { get; set; }
        }

        /// <summary>
        /// DataGetOrderItems
        /// </summary>
        public class DataGetOrderItems
        {
            /// <summary>
            /// PO_Item
            /// </summary>
            public string PO_Item { get; set; }
        }
    }
}
