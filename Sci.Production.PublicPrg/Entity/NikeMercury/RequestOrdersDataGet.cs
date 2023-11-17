using System;
using System.Xml.Serialization;

namespace Sci.Production.Prg.Entity.NikeMercury
{
    /// <summary>
    /// RequestLabelsPackPlanCreate
    /// </summary>
    public class RequestOrdersDataGet
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
            /// OrdersDataGet
            /// </summary>
            [XmlElement(ElementName = "OrdersDataGet", Namespace = "http://tempuri.org/")]
            public OrdersDataGet OrdersDataGet { get; set; }
        }

        /// <summary>
        /// OrdersDataGet
        /// </summary>
        [XmlRoot(ElementName = "OrdersDataGet", Namespace = "http://tempuri.org/")]
        public class OrdersDataGet
        {
            /// <summary>
            /// Input
            /// </summary>
            [XmlElement(ElementName = "input", Namespace = "http://tempuri.org/")]
            public Input Input { get; set; }
        }

        /// <summary>
        /// Input
        /// </summary>
        public class Input
        {
            /// <summary>
            /// FactoryCode
            /// </summary>
            [XmlElement(ElementName = "FactoryCode", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeOrders")]
            public string FactoryCode { get; set; }

            /// <summary>
            /// OrderNumber
            /// </summary>
            [XmlElement(ElementName = "OrderNumber", Namespace = "http://schemas.datacontract.org/2004/07/OLLIeOrders")]
            public string OrderNumber { get; set; }
        }
    }
}
