using System;
using System.Xml.Serialization;

namespace Sci.Production.Prg.Entity.NikeMercury
{
    /// <summary>
    /// RequestLabelsPackPlanCreate
    /// </summary>
    public class ResponseOrdersDataGet
    {
        [XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public class Envelope
        {
            [XmlElement(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
            public Body Body { get; set; }
        }

        public class Body
        {
            [XmlElement(ElementName = "OrdersDataGetResponse", Namespace = "http://tempuri.org/")]
            public OrdersDataGetResponse OrdersDataGetResponse { get; set; }
        }

        public class OrdersDataGetResponse
        {
            [XmlElement(ElementName = "OrdersDataGetResult", Namespace = "http://tempuri.org/")]
            public OrdersDataGetResult OrdersDataGetResult { get; set; }
        }

        public class OrdersDataGetResult
        {
            [XmlElement(ElementName = "OutputData", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeOrders")]
            public OutputData OutputData { get; set; }
        }

        public class OutputData
        {
            [XmlArray(ElementName = "OutputDataSet2")]
            [XmlArrayItem(ElementName = "DataGetOrderItems")]
            public DataGetOrderItems[] DataGetOrderItems { get; set; }
        }

        public class DataGetOrderItems
        {
            public string PO_Item { get; set; }
        }
    }
}
