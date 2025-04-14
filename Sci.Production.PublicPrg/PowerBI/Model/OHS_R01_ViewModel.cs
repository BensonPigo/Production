using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sci.Production.Prg.PowerBI.Model
{
    /// <inheritdoc/>
    public class OHS_R01
    {
        /// <inheritdoc/>
        public DateTime? StartDate { get; set; }

        /// <inheritdoc/>
        public DateTime? EndDate { get; set; }

        /// <inheritdoc/>
        public string DataType { get; set; }

        /// <inheritdoc/>
        public string FactoryID { get; set; }

        /// <inheritdoc/>
        public string Status { get; set; }

        /// <inheritdoc/>
        public DateTime? SBIDate { get; set; }

        /// <inheritdoc/>
        public DateTime? EBIDate { get; set; }

        /// <inheritdoc/>
        public bool IsBI { get; set; }
    }

    #pragma warning disable
    /// <inheritdoc/>
    public class OHS_R01_ViewModel
    {
        /// <inheritdoc/>
        public string ID { get; set; }

        /// <inheritdoc/>
        public string FactoryID { get; set; }

        /// <inheritdoc/>
        public string InjuryType { get; set; }

        /// <inheritdoc/>
        public DateTime? CDate { get; set; }

        /// <inheritdoc/>
        public string LossHours { get; set; }

        /// <inheritdoc/>
        public string IncidentType { get; set; }

        /// <inheritdoc/>
        public string IncidentRemark { get; set; }

        /// <inheritdoc/>
        public string SevereLevel { get; set; }

        /// <inheritdoc/>
        public string SevereRemark { get; set; }

        /// <inheritdoc/>
        public string CAP { get; set; }

        /// <inheritdoc/>
        public string Incharge { get; set; }

        /// <inheritdoc/>
        public DateTime? InchargeCheckDate { get; set; }

        /// <inheritdoc/>
        public string Approver { get; set; }

        /// <inheritdoc/>
        public DateTime? ApproveDate { get; set; }

        /// <inheritdoc/>
        public DateTime? ProcessDate { get; set; }

        /// <inheritdoc/>
        public DateTime? ProcessTime { get; set; }

        /// <inheritdoc/>
        public string ProcessUpdate { get; set; }

        /// <inheritdoc/>
        public string Status { get; set; }
    }
    #pragma warning restore
}
