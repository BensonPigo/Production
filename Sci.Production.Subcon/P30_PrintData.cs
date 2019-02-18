﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sci.Production.Subcon
{
    class P30_PrintData
    {
        public string Sort { get; set; }
        public string SP { get; set; }
        public string Delivery { get; set; }
        public string Refno { get; set; }
        public string Refno2 { get; set; }
        public string Color_Shade { get; set; }
        public string Description { get; set; }
        public string UPrice { get; set; }
        public decimal Order_Qty { get; set; }
        public string Unit { get; set; }
        public decimal Amount { get; set; }
        public byte[] ApvName { get; set; }
        public byte[] Lockname { get; set; }
    }
}
