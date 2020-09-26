using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sci.Production.Subcon
{
    internal class P05_PrintData
    {
        public string OrderID { get; set; }
        public string ID { get; set; }
        public string StyleID { get; set; }
        public string Article { get; set; }
        public string Size { get; set; }
        public string ReqQTY { get; set; }
        public string ArtworkID { get; set; }
        public string PCS { get; set; }
        public string QtyGMT { get; set; }
        public string CutParts { get; set; }
        public byte[] MgApvName { get; set; }
        public byte[] DeptApvName { get; set; }
    }
}
