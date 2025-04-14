using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;

namespace Sci.Production.Prg.PowerBI.Model
{
    /// <inheritdoc/>
    public class Planning_P08_ViewModel
    {
        /// <inheritdoc/>
        public string MDivisionID { get; set; }

        /// <inheritdoc/>
        public string FactoryID { get; set; }

        /// <inheritdoc/>
        public DateTime? SewingSDate { get; set; }

        /// <inheritdoc/>
        public DateTime? SewingEDate { get; set; }

        /// <inheritdoc/>
        public DateTime? SewingInlineSDate { get; set; }

        /// <inheritdoc/>
        public DateTime? SewingInlineEDate { get; set; }

        /// <inheritdoc/>
        public DateTime? SewingOfflineSDate { get; set; }

        /// <inheritdoc/>
        public DateTime? SewingOfflineEDate { get; set; }

        /// <inheritdoc/>
        public bool IsBI { get; set; }

        /// <inheritdoc/>
        public bool OnlySchema { get; set; }
    }
}
