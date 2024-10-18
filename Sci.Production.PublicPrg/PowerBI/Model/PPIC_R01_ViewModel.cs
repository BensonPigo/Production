using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;

namespace Sci.Production.Prg.PowerBI.Model
{
    /// <inheritdoc/>
    public class PPIC_R01_ViewModel
    {
        /// <inheritdoc/>
        public DateTime? Inline { get; set; }

        /// <inheritdoc/>
        public DateTime? Offline { get; set; }

        /// <inheritdoc/>
        public string Line1 { get; set; }

        /// <inheritdoc/>
        public string Line2 { get; set; }

        /// <inheritdoc/>
        public string MDivisionID { get; set; }

        /// <inheritdoc/>
        public string FactoryID { get; set; }

        /// <inheritdoc/>
        public DateTime? BuyerDelivery1 { get; set; }

        /// <inheritdoc/>
        public DateTime? BuyerDelivery2 { get; set; }

        /// <inheritdoc/>
        public DateTime? SciDelivery1 { get; set; }

        /// <inheritdoc/>
        public DateTime? SciDelivery2 { get; set; }

        /// <inheritdoc/>
        public string Brand { get; set; }

        /// <inheritdoc/>
        public string Subprocess { get; set; }

        /// <inheritdoc/>
        public bool IsPowerBI { get; set; }
    }

    /// <inheritdoc/>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "<暫止>")]
    public class PPIC_R01bySP_ViewModel
    {
        /// <inheritdoc/>
        public string MDivisionID { get; set; }

        /// <inheritdoc/>
        public string FactoryID { get; set; }

        /// <inheritdoc/>
        public string SewingLineIDFrom { get; set; }

        /// <inheritdoc/>
        public string SewingLineIDTo { get; set; }

        /// <inheritdoc/>
        public DateTime? SewingDateFrom { get; set; }

        /// <inheritdoc/>
        public DateTime? SewingDateTo { get; set; }

        /// <inheritdoc/>
        public DateTime? BuyerDeliveryFrom { get; set; }

        /// <inheritdoc/>
        public DateTime? BuyerDeliveryTo { get; set; }

        /// <inheritdoc/>
        public DateTime? SciDeliveryFrom { get; set; }

        /// <inheritdoc/>
        public DateTime? SciDeliveryTo { get; set; }

        /// <inheritdoc/>
        public string BrandID { get; set; }

        /// <inheritdoc/>
        public string SubProcess { get; set; }
        /// <inheritdoc/>
        public bool IsPowerBI { get; set; }
    }
}
