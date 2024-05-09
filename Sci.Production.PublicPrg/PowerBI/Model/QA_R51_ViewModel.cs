using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;

namespace Sci.Production.Prg.PowerBI.Model
{
    /// <inheritdoc/>
    public class QA_R51_ViewModel
    {
        /// <inheritdoc/>
        public DateTime? StartInspectionDate { get; set; }

        /// <inheritdoc/>
        public DateTime? EndInspectionDate { get; set; }

        /// <inheritdoc/>
        public string SP { get; set; }

        /// <inheritdoc/>
        public string SubProcess { get; set; }

        /// <inheritdoc/>
        public string Style { get; set; }

        /// <inheritdoc/>
        public string M { get; set; }

        /// <inheritdoc />
        public string Factory { get; set; }

        /// <inheritdoc />
        public string Shift { get; set; }

        /// <inheritdoc />
        public string FormatType { get; set; }

        /// <inheritdoc />
        public bool IsBI { get; set; }

    }
}
