using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;

namespace Sci.Production.Prg.PowerBI.Model
{
    /// <summary>
    /// PPIC R03 畫面上的篩選條件
    /// </summary>
#pragma warning disable SA1600 // Elements should be documented
#pragma warning disable SA1516 // Elements should be separated by blank line
    public class PPIC_R03_ViewModel
    {
        public bool IsPowerBI { get; set; }

        // PPIC R03 固定欄位的總數量
        public int ColumnsNum { get; set; }
        public string P_type { get; set; }
        public DateTime? BuyerDelivery1 { get; set; }
        public DateTime? BuyerDelivery2 { get; set; }
        public DateTime? SciDelivery1 { get; set; }
        public DateTime? SciDelivery2 { get; set; }

        // Cut-Off Date
        public DateTime? SDPDate1 { get; set; }

        // Cut-Off Date
        public DateTime? SDPDate2 { get; set; }

        // Cust RQS Date
        public DateTime? CRDDate1 { get; set; }

        // Cust RQS Date
        public DateTime? CRDDate2 { get; set; }
        public DateTime? PlanDate1 { get; set; }
        public DateTime? PlanDate2 { get; set; }
        public DateTime? CFMDate1 { get; set; }
        public DateTime? CFMDate2 { get; set; }
        public string SP1 { get; set; }
        public string SP2 { get; set; }
        public string StyleID { get; set; }
        public string Article { get; set; }
        public string SeasonID { get; set; }
        public string BrandID { get; set; }
        public string CustCDID { get; set; }
        public string Zone { get; set; }
        public string MDivisionID { get; set; }
        public string Factory { get; set; }
        public bool Bulk { get; set; }
        public bool Sample { get; set; }
        public bool Material { get; set; }
        public bool Forecast { get; set; }
        public bool Garment { get; set; }
        public bool SMTL { get; set; }

        // SubProcess
        public string ArtworkTypeID { get; set; }
        public bool IncludeHistoryOrder { get; set; }
        public bool IncludeArtworkData { get; set; }
        public bool PrintingDetail { get; set; }
        public bool ByCPU { get; set; }
        public bool IncludeArtworkDataKindIsPAP { get; set; }
        public bool SeparateByQtyBdownByShipmode { get; set; }
        public bool ListPOCombo { get; set; }
        public bool IncludeCancelOrder { get; set; }
    }
#pragma warning restore SA1516 // Elements should be separated by blank line
#pragma warning restore SA1600 // Elements should be documented
}
