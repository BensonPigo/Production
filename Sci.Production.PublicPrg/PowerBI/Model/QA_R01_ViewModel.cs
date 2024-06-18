using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sci.Production.Prg.PowerBI.Model
{
    /// <inheritdoc/>
    public class QA_R01_ViewModel
    {
        /// <inheritdoc/>
        public DateTime? StartInstPhysicalInspDate { get; set; }

        /// <inheritdoc/>
        public DateTime? EndInstPhysicalInspDate { get; set; }

        /// <inheritdoc/>
        public DateTime? StartArriveWHDate { get; set; }

        /// <inheritdoc/>
        public DateTime? EndArriveWHDate { get; set; }

        /// <inheritdoc/>
        public DateTime? StartSciDelivery { get; set; }

        /// <inheritdoc/>
        public DateTime? EndSciDelivery { get; set; }

        /// <inheritdoc/>
        public DateTime? StartSewingInLineDate { get; set; }

        /// <inheritdoc/>
        public DateTime? EndSewingInLineDate { get; set; }

        /// <inheritdoc/>
        public DateTime? StartEstCuttingDate { get; set; }

        /// <inheritdoc/>
        public DateTime? EndEstCuttingDate { get; set; }

        /// <inheritdoc/>
        public string StartWK { get; set; }

        /// <inheritdoc/>
        public string EndWK { get; set; }

        /// <inheritdoc/>
        public string StartSP { get; set; }

        /// <inheritdoc/>
        public string EndSP { get; set; }

        /// <inheritdoc/>
        public string Season { get; set; }

        /// <inheritdoc/>
        public string Brand { get; set; }

        /// <inheritdoc/>
        public string Refno { get; set; }

        /// <inheritdoc/>
        public string Category { get; set; }

        /// <inheritdoc/>
        public string Supplier { get; set; }

        /// <inheritdoc/>
        public string OverallResultStatus { get; set; }

        /// <inheritdoc/>
        public bool IsBI { get; set; }

        /// <inheritdoc/>
        public DateTime? StartBIDate { get; set; }

        /// <inheritdoc/>
        public DateTime? EndBIDate { get; set; }
    }
}
