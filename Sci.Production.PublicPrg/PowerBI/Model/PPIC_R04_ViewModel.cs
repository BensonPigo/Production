using System;

namespace Sci.Production.Prg.PowerBI.Model
{
    /// <inheritdoc/>
    public class PPIC_R04_ViewModel
    {
        /// <inheritdoc/>
        public string ReportType { get; set; }

        /// <inheritdoc/>
        public DateTime? ApvDate1 { get; set; }

        /// <inheritdoc/>
        public DateTime? ApvDate2 { get; set; }

        /// <inheritdoc/>
        public string MDivisionID { get; set; }

        /// <inheritdoc/>
        public string FactoryID { get; set; }

        /// <inheritdoc/>
        public int LeadTime { get; set; }

        /// <inheritdoc/>
        public string BIEditDate { get; set; }

        /// <inheritdoc/>
        public bool IsPowerBI { get; set; }
    }
}
