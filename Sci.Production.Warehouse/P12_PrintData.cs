using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Sci.Production.Warehouse
{
    public class P12_PrintData
    {
        public string POID { get; set; }
        public string SEQ { get; set; }
        public string DESC { get; set; }
        public string UNIT { get; set; }
        public string QTY { get; set; }
        public string BULKLocation { get; set; }
    }
}