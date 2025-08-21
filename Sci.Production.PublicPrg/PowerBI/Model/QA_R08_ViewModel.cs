using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sci.Production.Prg.PowerBI.Model
{
    /// <inheritdoc/>
    public class QA_R08_ViewModel
    {
        /// <inheritdoc/>
        public DateTime? InspectionDateFrom { get; set; }

        /// <inheritdoc/>
        public DateTime? InspectionDateTo { get; set; }

        /// <inheritdoc/>
        public DateTime? EditDateFrom { get; set; }

        /// <inheritdoc/>
        public DateTime? EditDateTo { get; set; }

        /// <inheritdoc/>
        public string Inspectors { get; set; }

        /// <inheritdoc/>
        public string POIDFrom { get; set; }

        /// <inheritdoc/>
        public string POIDTo { get; set; }

        /// <inheritdoc/>
        public string RefNoFrom { get; set; }

        /// <inheritdoc/>
        public string RefNoTo { get; set; }

        /// <inheritdoc/>
        public string BrandID { get; set; }

        /// <inheritdoc/>
        public bool IsSummary { get; set; }

        /// <inheritdoc/>
        public bool IsBI { get; set; }
    }
}
