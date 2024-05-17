using System;
using System.Collections.Generic;

namespace Sci.Production.Prg.PowerBI.Model
{
    /// <inheritdoc/>
    public class SubCon_R41_ViewModel
    {
        /// <inheritdoc/>
        public string CutRefNo1 { get; set; }

        /// <inheritdoc/>
        public string CutRefNo2 { get; set; }

        /// <inheritdoc/>
        public string SP1 { get; set; }

        /// <inheritdoc/>
        public DateTime? EstCuttingDate1 { get; set; }

        /// <inheritdoc/>
        public DateTime? EstCuttingDate2 { get; set; }

        /// <inheritdoc/>
        public DateTime? BundleCDate1 { get; set; }

        /// <inheritdoc/>
        public DateTime? BundleCDate2 { get; set; }

        /// <inheritdoc/>
        public DateTime? BundleScanDate1 { get; set; }

        /// <inheritdoc/>
        public DateTime? BundleScanDate2 { get; set; }

        /// <inheritdoc/>
        public DateTime? SewingInlineDate1 { get; set; }

        /// <inheritdoc/>
        public DateTime? SewingInlineDate2 { get; set; }

        /// <inheritdoc/>
        public DateTime? LastSewDate1 { get; set; }

        /// <inheritdoc/>
        public DateTime? LastSewDate2 { get; set; }

        /// <inheritdoc/>
        public DateTime? BuyerDelDate1 { get; set; }

        /// <inheritdoc/>
        public DateTime? BuyerDelDate2 { get; set; }

        /// <inheritdoc/>
        public string SubProcessList { get; set; }

        /// <inheritdoc/>
        public string MDivisionID { get; set; }

        /// <inheritdoc/>
        public string FactoryID { get; set; }

        /// <inheritdoc/>
        public string ProcessLocation { get; set; }

        /// <inheritdoc/>
        public DateTime? BIEditDate { get; set; }

        /// <inheritdoc/>
        public bool IsPowerBI { get; set; }
    }
}
