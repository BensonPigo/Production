using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sci.Production.Prg.PowerBI.Model
{
    /// <inheritdoc/>
    public class Miscellaneous_R02_ViewModel
    {
        /// <inheritdoc/>
        public DateTime? StartCreateDate { get; set; }

        /// <inheritdoc/>
        public DateTime? EndCreateDate { get; set; }

        /// <inheritdoc/>
        public string PurchaseFrom { get; set; }

        /// <inheritdoc/>
        public DateTime? StartDeliveryDate { get; set; }

        /// <inheritdoc/>
        public DateTime? EndDeliveryDate { get; set; }

        /// <inheritdoc/>
        public string Type { get; set; }

        /// <inheritdoc/>
        public string MDivisionID { get; set; }

        /// <inheritdoc/>
        public string FactoryID { get; set; }

        /// <inheritdoc/>
        public string Supplier { get; set; }

        /// <inheritdoc/>
        public string SubSupplier { get; set; }

        /// <inheritdoc/>
        public string OrderBy { get; set; }

        /// <inheritdoc/>
        public string Status { get; set; }

        /// <inheritdoc/>
        public bool IsOutstanding { get; set; }

        /// <inheritdoc/>
        public string OutstandingType { get; set; }

        /// <inheritdoc/>
        public DateTime? SDate { get; set; }

        /// <inheritdoc/>
        public DateTime? EDate { get; set; }

        /// <inheritdoc/>
        public bool IsBI { get; set; }
    }

    #pragma warning disable
    /// <inheritdoc/>
    public class Miscellaneous_R02_Report
    {
        public string PurchaseFrom { get; set; }
        public string MDivisionID { get; set; }
        public string FactoryID { get; set; }
        public string PONo { get; set; }
        public string PRConfirmedDate { get; set; }
        public string CreateDate { get; set; }
        public string DeliveryDate { get; set; }
        public string Type { get; set; }
        public string Supplier { get; set; }
        public string Status { get; set; }
        public string ReqNo { get; set; }
        public string PRDate { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string POQty { get; set; }
        public string UnitID { get; set; }
        public string CurrencyID { get; set; }
        public string UnitPrice { get; set; }
        public string UnitPrice_USD { get; set; }
        public string POAmount { get; set; }
        public string POAmount_USD { get; set; }
        public string AccInQty { get; set; }
        public string TPEPO { get; set; }
        public string TPEQty { get; set; }
        public string TPECurrencyID { get; set; }
        public string TPEPrice { get; set; }
        public string TPEAmount { get; set; }
        public string ApQty { get; set; }
        public string APAmount { get; set; }
        public string RentalDay { get; set; }
        public string IncomingDate { get; set; }
        public string APApprovedDate { get; set; }
        public string Invoice { get; set; }
        public string RequestReason { get; set; }
        public string ProjectItem { get; set; }
        public string Project { get; set; }
        public string DepartmentID { get; set; }
        public string AccountID { get; set; }
        public string AccountName { get; set; }
        public string AccountCategory { get; set; }
        public string Budget { get; set; }
        public string InternalRemarks { get; set; }
        public string APID { get; set; }

    }
    #pragma warning restore
}
