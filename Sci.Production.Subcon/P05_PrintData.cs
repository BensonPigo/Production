using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sci.Production.Subcon
{
    /// <inheritdoc/>
    internal class P05_PrintData
    {
        /// <inheritdoc/>
        public string OrderID { get; set; }

        /// <inheritdoc/>
        public string ID { get; set; }

        /// <inheritdoc/>
        public string StyleID { get; set; }

        /// <inheritdoc/>
        public string Article { get; set; }

        /// <inheritdoc/>
        public string Size { get; set; }

        /// <inheritdoc/>
        public string ReqQTY { get; set; }

        /// <inheritdoc/>
        public string ArtworkID { get; set; }

        /// <inheritdoc/>
        public string PCS { get; set; }

        /// <inheritdoc/>
        public string QtyGMT { get; set; }

        /// <inheritdoc/>
        public string CutParts { get; set; }

        /// <inheritdoc/>
        public byte[] MgApvName { get; set; }

        /// <inheritdoc/>
        public byte[] DeptApvName { get; set; }
    }
}
