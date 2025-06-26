using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sci.Production.Prg.PowerBI.Model
{
    /// <inheritdoc/>
    public class Cutting_R13_ViewModel
    {
        /// <inheritdoc/>
        public string MDivisionID { get; set; }

        /// <inheritdoc/>
        public string FactoryID { get; set; }

        /// <inheritdoc/>
        public string StyleID { get; set; }

        /// <inheritdoc/>
        public string CuttingSP1 { get; set; }

        /// <inheritdoc/>
        public string CuttingSP2 { get; set; }

        /// <inheritdoc/>
        public DateTime? Est_CutDate1 { get; set; }

        /// <inheritdoc/>
        public DateTime? Est_CutDate2 { get; set; }

        /// <inheritdoc/>
        public DateTime? ActCuttingDate1 { get; set; }

        /// <inheritdoc/>
        public DateTime? ActCuttingDate2 { get; set; }

        /// <inheritdoc/>
        public bool? IsPowerBI { get; set; } = false;
    }
}
