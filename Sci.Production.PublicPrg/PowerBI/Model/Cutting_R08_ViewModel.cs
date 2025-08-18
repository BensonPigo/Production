using System;

namespace Sci.Production.Prg.PowerBI.Model
{
    /// <inheritdoc/>
    public class Cutting_R08_ViewModel
    {
        /// <inheritdoc/>
        public string MDivisionID { get; set; }

        /// <inheritdoc/>
        public string FactoryID { get; set; }

        /// <inheritdoc/>
        public DateTime? EstCutDate1 { get; set; }

        /// <inheritdoc/>
        public DateTime? EstCutDate2 { get; set; }

        /// <inheritdoc/>
        public DateTime? ActCutDate1 { get; set; }

        /// <inheritdoc/>
        public DateTime? ActCutDate2 { get; set; }

        /// <inheritdoc/>
        public string CuttingSP { get; set; }

        /// <inheritdoc/>
        public bool IsPowerBI { get; set; }
    }
}
