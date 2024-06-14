using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sci.Production.Prg.PowerBI.Model
{
    public class RTLStatusByDay
    {
        public DateTime? TransferDate { get; set; }
        public string FactoryID { get; set; }
        public string BrandID { get; set; }
    }
    public class RTLStatus
    {        public string Line { get; set; }
        public int APSNo { get; set; }
        public string OrderID { get; set; }
        public string Article { get; set; }
        public string SizeCode { get; set; }
        public string StyleID { get; set; }
        public int StandardQty { get; set; }
        public int LoadingQty { get; set; }
        public int SewingLineQty { get; set; }
        public int WIPQty { get; set; }
        public decimal WipDays { get; set; }
        public decimal nWipDays { get; set; }
        public int nWipDaysQty { get; set; }
    }
}
