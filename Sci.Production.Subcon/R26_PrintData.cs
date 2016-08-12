using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sci.Production.Subcon
{
    class R26_PrintData
    {
        public string PO { get; set; }
        public string Code { get; set; }
        public string Color_Shade { get; set; }
        public string Description { get; set; }
        public string Quantity { get; set; }
        public string Unit { get; set; }
        public string Unit_Price { get; set; }
        public string Amount { get; set; }
        public string Total_Quantity { get; set; }
        public string Remark { get; set; }
        public string Title1 { get; set; }
        public string Issue_Date { get; set; }
        public string To { get; set; }
        public string Group_FIDS { get { return this.Title1 + this.Issue_Date + this.To; } }
        public string Group_PO { get { return this.PO + this.Code; } }
    }
}
