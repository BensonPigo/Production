using System;

namespace Sci.Production.Prg.PowerBI.Model
{
    /// <inheritdoc/>
    public class Sewing_R02_MonthlyProductionOutputReport
    {
        /// <inheritdoc/>
        public DateTime? StartOutputDate { get; set; }

        /// <inheritdoc/>
        public DateTime? EndOutputDate { get; set; }

        /// <inheritdoc/>
        public string Factory { get; set; }

        /// <inheritdoc/>
        public string M { get; set; }

        /// <inheritdoc/>
        public int ReportType { get; set; }

        /// <inheritdoc/>
        public string StartSewingLine { get; set; }

        /// <inheritdoc/>
        public string EndSewingLine { get; set; }

        /// <inheritdoc/>
        public int OrderBy { get; set; }

        /// <inheritdoc/>
        public bool ExcludeNonRevenue { get; set; }

        /// <inheritdoc/>
        public bool ExcludeSampleFactory { get; set; }

        /// <inheritdoc/>
        public bool ExcludeOfMockUp { get; set; }

        /// <inheritdoc/>
        public bool IsCN { get; set; }

        /// <inheritdoc/>
        public DateTime StartDate { get; set; }

        /// <inheritdoc/>
        public DateTime EndDate { get; set; }
    }
}
