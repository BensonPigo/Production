using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sci.Production.Prg.PowerBI.Model
{
    /// <inheritdoc/>
    public class Dashboard_RTLStatus
    {
        public string Line { get; set; }

        public string APSNo { get; set; }

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

        public string StyleIDSimple { get; set; }

        public string StyleIDTooltip { get; set; }
        public string ColorType { get; set; }
    }
}
