using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;

namespace Sci.Production.Prg.PowerBI.Model
{
#pragma warning disable SA1600 // Elements should be documented
#pragma warning disable SA1516 // Elements should be separated by blank line
    public class PPIC_R02_ViewModel
    {
        public bool IsPowerBI { get; set; }
        public DateTime? Date1 { get; set; }
        public DateTime? Date2 { get; set; }
        public DateTime? SciDelivery1 { get; set; }
        public DateTime? SciDelivery2 { get; set; }
        public DateTime? ProvideDate1 { get; set; }
        public DateTime? ProvideDate2 { get; set; }
        public DateTime? ReceiveDate1 { get; set; }
        public DateTime? ReceiveDate2 { get; set; }
        public string BrandID { get; set; }
        public string StyleID { get; set; }
        public string SeasonID { get; set; }
        public string MDivisionID { get; set; }
        public string MRHandle { get; set; }
        public string SMR { get; set; }
        public string PoHandle { get; set; }
        public string POSMR { get; set; }
        public int PrintType { get; set; }
    }
#pragma warning restore SA1516 // Elements should be separated by blank line
#pragma warning restore SA1600 // Elements should be documented
}
