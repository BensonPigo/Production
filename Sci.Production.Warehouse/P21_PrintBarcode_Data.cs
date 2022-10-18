using System.Drawing;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
#pragma warning disable SA1516 // Elements should be separated by blank line
#pragma warning disable SA1600 // Elements should be documented
    internal class P21_PrintBarcode_Data
    {
        public string SP { get; set; }
        public string Seq { get; set; }
        public string GW { get; set; }
        public string AW { get; set; }
        public string Location { get; set; }
        public string Refno { get; set; }
        public string Roll { get; set; }
        public string Color { get; set; }
        public string Dyelot { get; set; }
        public string Qty { get; set; }
        public string FactoryID { get; set; }
        public byte[] Image { get; set; }
    }
#pragma warning restore SA1516 // Elements should be separated by blank line
#pragma warning restore SA1600 // Elements should be documented
}
