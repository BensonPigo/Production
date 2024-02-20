using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Sci.Production.Prg.Entity.NikeMercury
{
    /// <summary>
    /// RequestShipmentScannedCartonUploadLSP
    /// </summary>
    public class RequestShipmentScannedCartonUploadLSP
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
            /// ShipmentScannedCartonUploadLSP
            /// </summary>
            [XmlElement(Namespace = "http://tempuri.org/", ElementName = "ShipmentScannedCartonUploadLSP")]
            public ShipmentScannedCartonUploadLSP ShipmentScannedCartonUploadLSP { get; set; }
        }

        /// <summary>
        /// ShipmentScannedCartonUploadLSP
        /// </summary>
        public class ShipmentScannedCartonUploadLSP
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
            /// Trackingnumber
            /// </summary>
            [XmlElement(Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLogistics", ElementName = "Trackingnumber")]
            public string Trackingnumber { get; set; }

            /// <summary>
            /// LoadIndicator
            /// </summary>
            [XmlElement(Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLogistics", ElementName = "LoadIndicator")]
            public string LoadIndicator { get; set; }

            /// <summary>
            /// LSPCode
            /// </summary>
            [XmlElement(Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLogistics", ElementName = "LSPCode")]
            public string LSPCode { get; set; }

            /// <summary>
            /// ContainerType
            /// </summary>
            [XmlElement(Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLogistics", ElementName = "ContainerType")]
            public string ContainerType { get; set; }

            /// <summary>
            /// ContainerSealNumber
            /// </summary>
            [XmlElement(Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLogistics", ElementName = "ContainerSealNumber")]
            public string ContainerSealNumber { get; set; }

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
            /// FSPCode
            /// </summary>
            [XmlElement(Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLogistics", ElementName = "FSPCode")]
            public string FSPCode { get; set; }

            /// <summary>
            /// ShipmentDate
            /// </summary>
            [XmlElement(Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLogistics", ElementName = "ShipmentDate")]
            public string ShipmentDate { get; set; }

            /// <summary>
            /// LSPBookingNumber
            /// </summary>
            [XmlElement(Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLogistics", ElementName = "LSPBookingNumber")]
            public string LSPBookingNumber { get; set; }

            /// <summary>
            /// PortofOrigin
            /// </summary>
            [XmlElement(Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLogistics", ElementName = "PortofOrigin")]
            public string PortofOrigin { get; set; }

            /// <summary>
            /// ScannerID
            /// </summary>
            [XmlElement(Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLogistics", ElementName = "ScannerID")]
            public string ScannerID { get; set; }

            /// <summary>
            /// CartonNumberList
            /// </summary>
            [XmlElement(Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLogistics", ElementName = "CartonNumberList")]
            public CartonNumberList CartonNumberList { get; set; }
        }

        /// <summary>
        /// CartonNumberList
        /// </summary>
        public class CartonNumberList
        {
            /// <summary>
            /// CartonNumberDetails
            /// </summary>
            [XmlElement(Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLogistics", ElementName = "CartonNumberDetails")]
            public List<CartonNumberDetails> CartonNumberDetails { get; set; }
        }

        /// <summary>
        /// CartonNumberDetails
        /// </summary>
        public class CartonNumberDetails
        {
            /// <summary>
            /// CartonNumber
            /// </summary>
            [XmlElement(Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLogistics", ElementName = "CartonNumber")]
            public string CartonNumber { get; set; }

            /// <summary>
            /// UCC
            /// </summary>
            [XmlElement(Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLogistics", ElementName = "UCC")]
            public string UCC { get; set; }

            /// <summary>
            /// CartonTypeCode
            /// </summary>
            [XmlElement(Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLogistics", ElementName = "CartonTypeCode")]
            public string CartonTypeCode { get; set; }
        }
    }
}
