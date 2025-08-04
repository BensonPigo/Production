using System;

namespace Sci.Production.Prg.PowerBI.Model
{
    /// <inheritdoc/>
    public class QA_R32_ViewModel
    {
        /// <inheritdoc/>
        public DateTime? StartAuditDate { get; set; }

        /// <inheritdoc/>
        public DateTime? EndAuditDate { get; set; }

        /// <inheritdoc/>
        public DateTime? StartBuyerDelivery { get; set; }

        /// <inheritdoc/>
        public DateTime? EndBuyerDelivery { get; set; }

        /// <inheritdoc/>
        public string StartSP { get; set; }

        /// <inheritdoc/>
        public string EndSP { get; set; }

        /// <inheritdoc/>
        public string MDivisionID { get; set; }

        /// <inheritdoc/>
        public string FactoryID { get; set; }

        /// <inheritdoc/>
        public string BrandID { get; set; }

        /// <inheritdoc/>
        public string Stage { get; set; }

        /// <inheritdoc/>
        // 0 = Summary , 1 = Detail , 2 = 5X5 Report
        public int Format { get; set; }

        /// <inheritdoc/>
        public bool IsPowerBI { get; set; }

        /// <inheritdoc/>
        public string BIFactoryID { get; set; } = string.Empty;
    }
}
