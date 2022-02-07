using System.Drawing;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    internal class P21_PrintBarcode_Data
    {
        /// <inheritdoc/>
        public string SP { get; set; }

        /// <inheritdoc/>
        public string Ref { get; set; }

        /// <inheritdoc/>
        public string SEQ { get; set; }

        /// <inheritdoc/>
        public string Lot { get; set; }

        /// <inheritdoc/>
        public string Color { get; set; }

        /// <inheritdoc/>
        public string Roll { get; set; }

        /// <inheritdoc/>
        public string Qty { get; set; }

        /// <inheritdoc/>
        public Bitmap Barcode { get; set; }
    }
}
