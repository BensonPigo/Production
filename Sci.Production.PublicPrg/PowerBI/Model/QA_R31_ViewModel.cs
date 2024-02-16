using System;

namespace Sci.Production.Prg.PowerBI.Model
{
    /// <inheritdoc/>
    public class QA_R31_ViewModel
    {
        /// <inheritdoc/>
        public DateTime? BuyerDelivery1 { get; set; }

        /// <inheritdoc/>
        public DateTime? BuyerDelivery2 { get; set; }

        /// <inheritdoc/>
        public string SP1 { get; set; }

        /// <inheritdoc/>
        public string SP2 { get; set; }

        /// <inheritdoc/>
        public string MDivisionID { get; set; }

        /// <inheritdoc/>
        public string FactoryID { get; set; }

        /// <inheritdoc/>
        public string Brand { get; set; }

        /// <inheritdoc/>
        public string Category { get; set; }

        /// <inheritdoc/>
        public bool Exclude_Sister_Transfer_Out { get; set; }

        /// <inheritdoc/>
        public bool Outstanding { get; set; }

        /// <inheritdoc/>
        public string InspStaged { get; set; }

        /// <inheritdoc/>
        public bool IsPowerBI { get; set; }
    }
}
