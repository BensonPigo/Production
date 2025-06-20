using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;

namespace Sci.Production.Prg.PowerBI.Model
{
    /// <inheritdoc/>
    public class PPIC_R21_ViewModel
    {
        /// <inheritdoc/>
        public DateTime? BuyerDeliveryFrom { get; set; }

        /// <inheritdoc/>
        public DateTime? BuyerDeliveryTo { get; set; }

        /// <inheritdoc/>
        public DateTime? DateTimeProcessFrom { get; set; }

        /// <inheritdoc/>
        public DateTime? DateTimeProcessTo { get; set; }

        /// <inheritdoc/>
        public string ComboProcess { get; set; }

        /// <inheritdoc/>
        public string MDivisionID { get; set; }

        /// <inheritdoc/>
        public string FactoryID { get; set; }

        /// <inheritdoc/>
        public bool ExcludeSisterTransferOut { get; set; }

        /// <inheritdoc/>
        public bool IncludeCancelOrder { get; set; }

        /// <inheritdoc/>
        public bool IsBI { get; set; }

        /// <inheritdoc/>
        public bool IsPPICP26 { get; set; }

        /// <inheritdoc/>
        public string OrderID { get; set; }

        /// <inheritdoc/>
        public string PackID { get; set; }

        /// <inheritdoc/>
        public string PO { get; set; }

        /// <inheritdoc/>
        public DateTime? SCIDeliveryFrom { get; set; }

        /// <inheritdoc/>
        public DateTime? SCIDeliveryTo { get; set; }
    }
}
