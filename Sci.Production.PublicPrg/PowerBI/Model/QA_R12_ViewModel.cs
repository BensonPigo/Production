using System;

namespace Sci.Production.Prg.PowerBI.Model
{
    /// <inheritdoc/>
    public class QA_R12_ViewModel
    {
        /// <summary>
        /// 1 : Receiving/Transfer in, 2 : Transfer Inventory to Bulk
        /// </summary>
        public int Transaction { get; set; }

        /// <inheritdoc/>
        public DateTime? ArriveWHDate1 { get; set; }

        /// <inheritdoc/>
        public DateTime? ArriveWHDate2 { get; set; }

        /// <inheritdoc/>
        public string SP1 { get; set; }

        /// <inheritdoc/>
        public string SP2 { get; set; }

        /// <inheritdoc/>
        public string WK1 { get; set; }

        /// <inheritdoc/>
        public string WK2 { get; set; }

        /// <inheritdoc/>
        public DateTime? InspectionDate1 { get; set; }

        /// <inheritdoc/>
        public DateTime? InspectionDate2 { get; set; }

        /// <inheritdoc/>
        public string Brand { get; set; }

        /// <summary>
        /// comboInspection.value (All, Physical, Weight, Shade Band, Continuity, Odor)
        /// </summary>
        public string Inspection { get; set; }

        /// <summary>
        /// comboInspectionResult.value (All, Pass, Fail, Pass/Fail, Not yet inspected)
        /// </summary>
        public string InspectionResult { get; set; }

        /// <summary>
        /// radioWKSeq
        /// </summary>
        public bool ByWKSeq { get; set; }

        /// <summary>
        /// radioRollDyelot
        /// </summary>
        public bool ByRollDyelot { get; set; }

        /// <inheritdoc/>
        public bool IsPowerBI { get; set; }
    }
}
