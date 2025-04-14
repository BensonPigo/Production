using System;
using System.Windows.Forms;

namespace Sci.Production.Prg.PowerBI.Model
{
    /// <inheritdoc/>
    public class PPIC_R08_ViewModel
    {
        /// <inheritdoc/>
        public DateTime? CDate1 { get; set; }

        /// <inheritdoc/>
        public DateTime? CDate2 { get; set; }

        /// <inheritdoc/>
        public DateTime? ApvDate1 { get; set; }

        /// <inheritdoc/>
        public DateTime? ApvDate2 { get; set; }

        /// <inheritdoc/>
        public DateTime? Lockdate1 { get; set; }

        /// <inheritdoc/>
        public DateTime? Lockdate2 { get; set; }

        /// <inheritdoc/>
        public DateTime? Cfmdate1 { get; set; }

        /// <inheritdoc/>
        public DateTime? Cfmdate2 { get; set; }

        /// <inheritdoc/>
        public DateTime? Voucher1 { get; set; }

        /// <inheritdoc/>
        public DateTime? Voucher2 { get; set; }

        /// <inheritdoc/>
        public string MDivisionID { get; set; }

        /// <inheritdoc/>
        public string FactoryID { get; set; }

        /// <inheritdoc/>
        public string T { get; set; }

        /// <inheritdoc/>
        public string Status { get; set; }

        /// <inheritdoc/>
        public string Sharedept { get; set; }

        /// <inheritdoc/>
        public string ReportType { get; set; }

        /// <inheritdoc/>
        public bool IncludeJunk { get; set; }

        /// <inheritdoc/>
        public bool IsReplacementReport { get; set; }

        /// <inheritdoc/>
        public bool IsPowerBI { get; set; }
    }
}
