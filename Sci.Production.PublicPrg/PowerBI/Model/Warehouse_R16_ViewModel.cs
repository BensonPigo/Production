using System;

namespace Sci.Production.Prg.PowerBI.Model
{
    /// <inheritdoc/>
    public class Warehouse_R16_ViewModel
    {
        /// <inheritdoc/>
        public DateTime? IssueDateFrom { get; set; }

        /// <inheritdoc/>
        public DateTime? IssueDateTo { get; set; }

        /// <inheritdoc/>
        public string MDivisionID { get; set; }

        /// <inheritdoc/>
        public string FactoryID { get; set; }

        /// <inheritdoc/>
        public string CutplanIDFrom { get; set; }

        /// <inheritdoc/>
        public string CutplanIDTo { get; set; }

        /// <inheritdoc/>
        public string SPFrom { get; set; }

        /// <inheritdoc/>
        public string SPTo { get; set; }

        /// <inheritdoc/>
        public DateTime? EditDateFrom { get; set; }

        /// <inheritdoc/>
        public DateTime? EditDateTo { get; set; }
    }
}
