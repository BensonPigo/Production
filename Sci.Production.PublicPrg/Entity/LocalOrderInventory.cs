namespace Sci.Production.Prg.Entity
{
#pragma warning disable SA1600
#pragma warning disable SA1516
    public class LocalOrderInventory
    {
        public string Ukey { get; set; }
        public string ToUkey { get; set; }
        public string Poid { get; set; }
        public string Seq1 { get; set; }
        public string Seq2 { get; set; }
        public string Roll { get; set; }
        public string Dyelot { get; set; }
        public string Stocktype { get; set; }
        public string Barcode { get; set; }
        public string BarcodeSeq { get; set; }
        public decimal InQty { get; set; }
        public decimal OutQty { get; set; }
        public decimal AdjustQty { get; set; }
        public decimal BalanceQty { get; set; }
        public decimal ToBalanceQty { get; set; }
        public long Rn { get; set; }
        public string ContainerCode { get; set; }
        public string Tone { get; set; }
    }
#pragma warning restore SA1600
#pragma warning restore SA1516
}
