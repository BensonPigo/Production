using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;

namespace Sci.Production.Prg.PowerBI.Model
{
    /// <inheritdoc/>
    public class Sewing_R02_ViewModel
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

    /// <inheritdoc/>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Reviewed")]
    public class P_MonthlySewingOutputSummary_ViewModel
    {
        /// <inheritdoc/>
        public string Fty { get; set; }

        /// <inheritdoc/>
        public string Period { get; set; }

        /// <inheritdoc/>
        public string LastDatePerMonth { get; set; }

        /// <inheritdoc/>
        public int TtlQtyExclSubconOut { get; set; }

        /// <inheritdoc/>
        public decimal TtlCPUInclSubconIn { get; set; }

        /// <inheritdoc/>
        public decimal SubconInTtlCPU { get; set; }

        /// <inheritdoc/>
        public decimal SubconOutTtlCPU { get; set; }

        /// <inheritdoc/>
        public decimal PPH { get; set; }

        /// <inheritdoc/>
        public decimal AvgWorkHr { get; set; }

        /// <inheritdoc/>
        public int TtlManpower { get; set; }

        /// <inheritdoc/>
        public decimal TtlManhours { get; set; }

        /// <inheritdoc/>
        public decimal Eff { get; set; }

        /// <inheritdoc/>
        public decimal AvgWorkHrPAMS { get; set; }

        /// <inheritdoc/>
        public decimal TtlManpowerPAMS { get; set; }

        /// <inheritdoc/>
        public decimal TtlManhoursPAMS { get; set; }

        /// <inheritdoc/>
        public decimal EffPAMS { get; set; }

        /// <inheritdoc/>
        public int TransferManpowerPAMS { get; set; }

        /// <inheritdoc/>
        public decimal TransferManhoursPAMS { get; set; }

        /// <inheritdoc/>
        public decimal TtlRevenue { get; set; }

        /// <inheritdoc/>
        public int TtlWorkDay { get; set; }

        /// <inheritdoc/>
        public List<P_MonthlySewingOutputSummary_ViewModel> DataList;
    }
}
