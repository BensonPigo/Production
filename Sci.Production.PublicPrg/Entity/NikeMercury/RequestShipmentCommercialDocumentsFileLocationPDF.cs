using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Sci.Production.Prg.Entity.NikeMercury
{
    /// <summary>
    /// RequestShipmentCommercialDocumentsFileLocationPDF
    /// </summary>
    public class RequestShipmentCommercialDocumentsFileLocationPDF
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
            /// ShipmentCommercialDocumentsFileLocationPDF
            /// </summary>
            [XmlElement(Namespace = "http://tempuri.org/", ElementName = "ShipmentCommercialDocumentsFileLocationPDF")]
            public ShipmentCommercialDocumentsFileLocationPDF ShipmentCommercialDocumentsFileLocationPDF { get; set; }
        }

        /// <summary>
        /// ShipmentCommercialDocumentsFileLocationPDF
        /// </summary>
        public class ShipmentCommercialDocumentsFileLocationPDF
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
            /// ShipmentNo
            /// </summary>
            [XmlElement(Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLogistics", ElementName = "ShipmentNo")]
            public string ShipmentNo { get; set; }

            /// <summary>
            /// CIReq
            /// </summary>
            [XmlElement(Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLogistics", ElementName = "CIReq")]
            public bool CIReq { get; set; }

            /// <summary>
            /// PLReq
            /// </summary>
            [XmlElement(Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLogistics", ElementName = "PLReq")]
            public string PLReq { get; set; }

            /// <summary>
            /// COOReq
            /// </summary>
            [XmlElement(Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLogistics", ElementName = "COOReq")]
            public bool COOReq { get; set; }

            /// <summary>
            /// COIReq
            /// </summary>
            [XmlElement(Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLogistics", ElementName = "COIReq")]
            public bool COIReq { get; set; }

            /// <summary>
            /// TSReq
            /// </summary>
            [XmlElement(Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLogistics", ElementName = "TSReq")]
            public bool TSReq { get; set; }

            /// <summary>
            /// SSReq
            /// </summary>
            [XmlElement(Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLogistics", ElementName = "SSReq")]
            public bool SSReq { get; set; }

            /// <summary>
            /// TCPLReq
            /// </summary>
            [XmlElement(Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLogistics", ElementName = "TCPLReq")]
            public bool TCPLReq { get; set; }

            /// <summary>
            /// TCCIReq
            /// </summary>
            [XmlElement(Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLogistics", ElementName = "TCCIReq")]
            public bool TCCIReq { get; set; }

            /// <summary>
            /// FctyAdr
            /// </summary>
            [XmlElement(Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLogistics", ElementName = "FctyAdr")]
            public string FctyAdr { get; set; }

            /// <summary>
            /// SellerAdr
            /// </summary>
            [XmlElement(Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLogistics", ElementName = "SellerAdr")]
            public string SellerAdr { get; set; }

            /// <summary>
            /// ConsolidateInv
            /// </summary>
            [XmlElement(Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLogistics", ElementName = "ConsolidateInv")]
            public bool ConsolidateInv { get; set; }

            /// <summary>
            /// PrintGWNWCI
            /// </summary>
            [XmlElement(Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLogistics", ElementName = "PrintGWNWCI")]
            public bool PrintGWNWCI { get; set; }

            /// <summary>
            /// PrintGWNWPL
            /// </summary>
            [XmlElement(Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLogistics", ElementName = "PrintGWNWPL")]
            public bool PrintGWNWPL { get; set; }

            /// <summary>
            /// PrintPrelimWMark
            /// </summary>
            [XmlElement(Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLogistics", ElementName = "PrintPrelimWMark")]
            public bool PrintPrelimWMark { get; set; }

            /// <summary>
            /// PrintFctyAdrPL
            /// </summary>
            [XmlElement(Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLogistics", ElementName = "PrintFctyAdrPL")]
            public bool PrintFctyAdrPL { get; set; }

            /// <summary>
            /// PrintMMPL
            /// </summary>
            [XmlElement(Namespace = "http://schemas.datacontract.org/2004/07/OLLIeLogistics", ElementName = "PrintMMPL")]
            public bool PrintMMPL { get; set; }
        }
    }
}
