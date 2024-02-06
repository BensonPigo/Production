using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;

namespace Sci.Production.Prg.PowerBI.Model
{
    /// <inheritdoc/>
#pragma warning disable SA1600 // Elements should be documented
#pragma warning disable SA1516 // Elements should be separated by blank line
    public class PPIC_R03_ViewModel
    {
        public DateTime? BuyerDelivery1 { get; set; }
        public DateTime? BuyerDelivery2 { get; set; }
        public DateTime? SciDelivery1 { get; set; }
        public DateTime? SciDelivery2 { get; set; }
        public DateTime? CutOffDate1 { get; set; }
        public DateTime? CutOffDate2 { get; set; }
        public DateTime? CustRQSDate1 { get; set; }
        public DateTime? CustRQSDate2 { get; set; }
        public DateTime? PlanDate1 { get; set; }
        public DateTime? PlanDate2 { get; set; }
        public string SP1 { get; set; }
        public string SP2 { get; set; }
        public string Style { get; set; }
        public string Article { get; set; }
        public string Season { get; set; }
        public string Brand { get; set; }
        public string CustCD { get; set; }
        public string Zone { get; set; }
        public string M { get; set; }
        public string Factory { get; set; }
        public bool Bulk { get; set; }
        public bool Sample { get; set; }
        public bool Material { get; set; }
        public bool Forecast { get; set; }
        public bool Garment { get; set; }
        public bool SMTL { get; set; }
        public string SubProcess { get; set; }
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
