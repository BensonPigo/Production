namespace Sci.Production.Prg.Entity
{
#pragma warning disable SA1600
#pragma warning disable SA1516
    public class WHBarcodeTransaction
    {
        public string Function { get; set; }
        public string TransactionID { get; set; }
        public long TransactionUkey { get; set; }
        public long Rn { get; set; }
        public string Action { get; set; }
        public string FromFabric_FtyInventoryUkey { get; set; } // string 是因為 ProcessWithObject 用(long?)會掛掉, 但有寫 NULL 需求
        public string From_OldBarcode { get; set; }
        public string From_OldBarcodeSeq { get; set; }
        public string From_NewBarcode { get; set; }
        public string From_NewBarcodeSeq { get; set; }
        public string ToFabric_FtyInventoryUkey { get; set; } // string 是因為 ProcessWithObject 用(long?)會掛掉, 但有寫 NULL 需求
        public string To_OldBarcode { get; set; }
        public string To_OldBarcodeSeq { get; set; }
        public string To_NewBarcode { get; set; }
        public string To_NewBarcodeSeq { get; set; }
        public bool UpdatethisItem { get; set; }
    }
#pragma warning restore SA1600
#pragma warning restore SA1516
}
