using System;
using System.Collections.Generic;

namespace Sci.Production.Prg.PowerBI.Model
{
    /// <inheritdoc/>
    public class Warehouse_R40_ViewModel
    {
        /// <inheritdoc/>
        public DateTime? ArriveDateStart { get; set; }

        /// <inheritdoc/>
        public DateTime? ArriveDateEnd { get; set; }

        /// <inheritdoc/>
        public string SP1 { get; set; }

        /// <inheritdoc/>
        public string SP2 { get; set; }

        /// <inheritdoc/>
        public string WKID1 { get; set; }

        /// <inheritdoc/>
        public string WKID2 { get; set; }

        /// <inheritdoc/>
        public string UpdateInfo { get; set; }

        /// <inheritdoc/>
        public string BrandID { get; set; }

        /// <inheritdoc/>
        public string Status { get; set; }

        /// <inheritdoc/>
        public bool IsPowerBI { get; set; }

        /// <inheritdoc/>
        public DateTime? AddEditDateStart { get; set; }

        /// <inheritdoc/>
        public DateTime? AddEditDateEnd { get; set; }
    }
}
