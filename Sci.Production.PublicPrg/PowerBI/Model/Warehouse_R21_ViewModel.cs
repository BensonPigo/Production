using System;

namespace Sci.Production.Prg.PowerBI.Model
{
    /// <inheritdoc/>
    public class Warehouse_R21_ViewModel
    {
        /// <inheritdoc/>
        public int ReportType { get; set; }

        /// <inheritdoc/>
        public string StartSPNo { get; set; }

        /// <inheritdoc/>
        public string EndSPNo { get; set; }

        /// <inheritdoc/>
        public string MDivision { get; set; }

        /// <inheritdoc/>
        public string Factory { get; set; }

        /// <inheritdoc/>
        public string StartRefno { get; set; }

        /// <inheritdoc/>
        public string EndRefno { get; set; }

        /// <inheritdoc/>
        public string Color { get; set; }

        /// <inheritdoc/>
        public string MT { get; set; }

        /// <inheritdoc/>
        public string MtlTypeID { get; set; }

        /// <inheritdoc/>
        public string ST { get; set; }

        /// <inheritdoc/>
        public bool BoolCheckQty { get; set; }

        /// <inheritdoc/>
        public DateTime? BuyerDeliveryFrom { get; set; }

        /// <inheritdoc/>
        public DateTime? BuyerDeliveryTo { get; set; }

        /// <inheritdoc/>
        public DateTime? ETAFrom { get; set; }

        /// <inheritdoc/>
        public DateTime? ETATo { get; set; }

        /// <inheritdoc/>
        public DateTime? OrigBuyerDeliveryFrom { get; set; }

        /// <inheritdoc/>
        public DateTime? OrigBuyerDeliveryTo { get; set; }

        /// <inheritdoc/>
        public bool Bulk { get; set; }

        /// <inheritdoc/>
        public bool Sample { get; set; }

        /// <inheritdoc/>
        public bool Material { get; set; }

        /// <inheritdoc/>
        public bool Smtl { get; set; }

        /// <inheritdoc/>
        public bool Complete { get; set; }

        /// <inheritdoc/>
        public bool NoLocation { get; set; }

        /// <inheritdoc/>
        public DateTime? ArriveWHFrom { get; set; }

        /// <inheritdoc/>
        public DateTime? ArriveWHTo { get; set; }

        /// <inheritdoc/>
        public string WorkNo { get; set; }

        /// <inheritdoc/>
        public string StartLocation { get; set; }

        /// <inheritdoc/>
        public string EndLocation { get; set; }

        /// <inheritdoc/>
        public DateTime? AddEditDateStart { get; set; }

        /// <inheritdoc/>
        public DateTime? AddEditDateEnd { get; set; }

        /// <inheritdoc/>
        public bool IsPowerBI { get; set; }
    }
}
