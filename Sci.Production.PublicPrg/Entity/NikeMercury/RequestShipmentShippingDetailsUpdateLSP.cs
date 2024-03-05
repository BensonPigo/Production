using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Sci.Production.Prg.Entity.NikeMercury
{
    /// <summary>
    /// RequestShipmentShippingDetailsUpdateLSP
    /// </summary>
    public class RequestShipmentShippingDetailsUpdateLSP
    {
        /// <summary>
        /// Envelope
        /// </summary>
        [XmlRoot(Namespace = "http://schemas.xmlsoap.org/soap/envelope/", ElementName = "Envelope")]
        public class Envelope
        {
            /// <summary>
            /// Header
            /// </summary>
            [XmlElement(Namespace = "http://schemas.xmlsoap.org/soap/envelope/", ElementName = "Header")]
            public Header Header { get; set; }

            /// <summary>
            /// Body
            /// </summary>
            [XmlElement(Namespace = "http://schemas.xmlsoap.org/soap/envelope/", ElementName = "Body")]
            public Body Body { get; set; }
        }

        /// <summary>
        /// Header
        /// </summary>
        public class Header
        {
            // Header properties go here if any
        }

        /// <summary>
        /// Body
        /// </summary>
        public class Body
        {
            /// <summary>
            /// ShipmentShippingDetailsUpdateLSP
            /// </summary>
            [XmlElement(Namespace = "http://tempuri.org/", ElementName = "ShipmentShippingDetailsUpdateLSP")]
            public ShipmentShippingDetailsUpdateLSP ShipmentShippingDetailsUpdateLSP { get; set; }
        }

        /// <summary>
        /// ShipmentShippingDetailsUpdateLSP
        /// </summary>
        public class ShipmentShippingDetailsUpdateLSP
        {
            /// <summary>
            /// Input
            /// </summary>
            [XmlElement(Namespace = "http://tempuri.org/", ElementName = "input")]
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
            [XmlElement(Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLogistics", ElementName = "FactoryCode")]
            public string FactoryCode { get; set; }

            /// <summary>
            /// LSPCode
            /// </summary>
            [XmlElement(Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLogistics", ElementName = "LSPCode")]
            public string LSPCode { get; set; }

            /// <summary>
            /// ShippingDetail
            /// </summary>
            [XmlElement(Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLogistics", ElementName = "ShippingDetail")]
            public ShippingDetail ShippingDetail { get; set; }

            /// <summary>
            /// FinancialList
            /// </summary>
            [XmlElement(Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLogistics", ElementName = "FinancialList")]
            public FinancialList FinancialList { get; set; }
        }

        /// <summary>
        /// ShippingDetail
        /// </summary>
        public class ShippingDetail
        {
            /// <summary>
            /// FactoryHubShipmentNumber
            /// </summary>
            [XmlElement(Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLogistics", ElementName = "FactoryHubShipmentNumber")]
            public string FactoryHubShipmentNumber { get; set; }

            /// <summary>
            /// FSPCode
            /// </summary>
            [XmlElement(Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLogistics", ElementName = "FSPCode")]
            public string FSPCode { get; set; } = string.Empty;

            /// <summary>
            /// Trackingnumber
            /// </summary>
            [XmlElement(Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLogistics", ElementName = "Trackingnumber")]
            public string Trackingnumber { get; set; }

            /// <summary>
            /// LoadIndicator
            /// </summary>
            [XmlElement(Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLogistics", ElementName = "LoadIndicator")]
            public string LoadIndicator { get; set; }
        }

        /// <summary>
        /// FinancialList
        /// </summary>
        public class FinancialList
        {
            /// <summary>
            /// FinancialDetails
            /// </summary>
            [XmlElement(Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLogistics", ElementName = "FinancialDetails")]
            public List<FinancialDetails> FinancialDetails { get; set; }
        }

        /// <summary>
        /// FinancialDetails
        /// </summary>
        public class FinancialDetails
        {
            /// <summary>
            /// OrderNumber
            /// </summary>
            [XmlElement(Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLogistics", ElementName = "OrderNumber")]
            public string OrderNumber { get; set; }

            /// <summary>
            /// LSPBookingNumber
            /// </summary>
            [XmlElement(Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLogistics", ElementName = "LSPBookingNumber")]
            public string LSPBookingNumber { get; set; }

            /// <summary>
            /// InvoiceNumber
            /// </summary>
            [XmlElement(Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLogistics", ElementName = "InvoiceNumber")]
            public string InvoiceNumber { get; set; }

            /// <summary>
            /// InvoiceDate
            /// </summary>
            [XmlElement(Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLogistics", ElementName = "InvoiceDate")]
            public string InvoiceDate { get; set; }

            /// <summary>
            /// OAReferenceNbr
            /// </summary>
            [XmlElement(Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLogistics", ElementName = "OAReferenceNbr")]
            public string OAReferenceNbr { get; set; }

            /// <summary>
            /// LCReferenceNbr
            /// </summary>
            [XmlElement(Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLogistics", ElementName = "LCReferenceNbr")]
            public string LCReferenceNbr { get; set; }
        }
    }
}
