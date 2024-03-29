using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;

namespace Sci.Production.Prg.PowerBI.Model
{
    /// <inheritdoc/>
    public class Planning_R15_ViewModel
    {
        /// <inheritdoc/>
        public DateTime? StartSciDelivery { get; set; }

        /// <inheritdoc/>
        public DateTime? EndSciDelivery { get; set; }

        /// <inheritdoc/>
        public DateTime? StartBuyerDelivery { get; set; }

        /// <inheritdoc/>
        public DateTime? EndBuyerDelivery { get; set; }

        /// <inheritdoc/>
        public DateTime? StartSewingInline { get; set; }

        /// <inheritdoc/>
        public DateTime? EndSewingInline { get; set; }

        /// <inheritdoc/>
        public DateTime? StartCutOffDate { get; set; }

        /// <inheritdoc/>
        public DateTime? EndCutOffDate { get; set; }

        /// <inheritdoc/>
        public DateTime? StartCustRQSDate { get; set; }

        /// <inheritdoc/>
        public DateTime? EndCustRQSDate { get; set; }

        /// <inheritdoc/>
        public DateTime? StartPlanDate { get; set; }

        /// <inheritdoc/>
        public DateTime? EndPlanDate { get; set; }

        /// <inheritdoc/>
        public string StartSP { get; set; }

        /// <inheritdoc/>
        public string EndSP { get; set; }

        /// <inheritdoc/>
        public DateTime? StartLastSewDate { get; set; }

        /// <inheritdoc/>
        public DateTime? EndLastSewDate { get; set; }

        /// <inheritdoc/>
        public string StyleID { get; set; }

        /// <inheritdoc/>
        public string BrandID { get; set; }

        /// <inheritdoc/>
        public string CustCD { get; set; }

        /// <inheritdoc/>
        public string MDivisionID { get; set; }

        /// <inheritdoc/>
        public string FactoryID { get; set; }

        /// <inheritdoc/>
        public string Category { get; set; }

        /// <inheritdoc/>
        public string OrderBy { get; set; }

        /// <inheritdoc/>
        public string SummaryBy { get; set; }

        /// <inheritdoc/>
        public string SubprocessID { get; set; }

        /// <inheritdoc/>
        public string ArtworkTypes { get; set; }

        /// <inheritdoc/>
        public bool OnlyShowCheckedSubprocessOrder { get; set; }

        /// <inheritdoc/>
        public bool IncludeAtworkData { get; set; }

        /// <inheritdoc/>
        public bool IncludeCancelOrder { get; set; }
    }
}
