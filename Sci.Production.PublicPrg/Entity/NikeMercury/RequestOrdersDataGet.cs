using System;
using System.Xml.Serialization;

namespace Sci.Production.Prg.Entity.NikeMercury
{
    /// <summary>
    /// RequestLabelsPackPlanCreate
    /// </summary>
    public class RequestOrdersDataGet
    {
        [XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public class Envelope
        {
            [XmlElement(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
            public Body Body { get; set; }
        }

        public class Body
        {
            [XmlElement(ElementName = "OrdersDataGet", Namespace = "http://tempuri.org/")]
            public OrdersDataGet OrdersDataGet { get; set; }
        }

        [XmlRoot(ElementName = "OrdersDataGet", Namespace = "http://tempuri.org/")]
        public class OrdersDataGet
        {
            [XmlElement(ElementName = "input", Namespace = "http://tempuri.org/")]
            public Input Input { get; set; }
        }

        public class Input
        {
            [XmlElement(ElementName = "FactoryCode", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeOrders")]
            public string FactoryCode { get; set; }

            [XmlElement(ElementName = "OrderNumber", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeOrders")]
            public string OrderNumber { get; set; }
        }
    }
}
