using System;

namespace Sci.Production.Prg.PowerBI.Model
{
    /// <inheritdoc/>
    public class QA_R20_ViewModel
    {
        /// <inheritdoc/>
        public DateTime? CDate1 { get; set; }

        /// <inheritdoc/>
        public DateTime? CDate2 { get; set; }

        /// <inheritdoc/>
        public string Factory { get; set; }

        /// <inheritdoc/>
        public string Brand { get; set; }

        /// <inheritdoc/>
        public string Line { get; set; }

        /// <inheritdoc/>
        public string Cell { get; set; }

        /// <inheritdoc/>
        public bool IsPowerBI { get; set; }
    }
}
