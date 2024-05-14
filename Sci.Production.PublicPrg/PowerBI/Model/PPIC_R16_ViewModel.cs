using System;

namespace Sci.Production.Prg.PowerBI.Model
{
    /// <inheritdoc/>
    public class PPIC_R16_ViewModel
    {
        /// <inheritdoc/>
        public DateTime? BuyerDeliveryFrom { get; set; }

        /// <inheritdoc/>
        public DateTime? BuyerDeliveryTo { get; set; }

        /// <inheritdoc/>
        public string MDivisionID { get; set; }

        /// <inheritdoc/>
        public string BrandID { get; set; }

        /// <inheritdoc/>
        public string FactoryID { get; set; }

        /// <inheritdoc/>
        public bool IsOutstanding { get; set; }

        /// <inheritdoc/>
        public bool IsExcludeSister { get; set; }

        /// <inheritdoc/>
        public bool IsExcludeCancelShortage { get; set; }

        /// <inheritdoc/>
        public bool IsBookingOrder { get; set; }

        /// <inheritdoc/>
        public bool Is7DayEdit{ get; set; }

        /// <inheritdoc/>
        public bool IsJunk { get; set; }
    }
}
