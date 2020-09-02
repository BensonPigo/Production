namespace Sci.Production.Warehouse
{
    internal class P23_PrintData
    {
        public string StockSP { get; set; }

        public string StockSEQ { get; set; }

        public string IssueSP { get; set; }

        public string SEQ { get; set; }

        public string DESC { get; set; }

        public string Roll { get; set; }

        public string DYELOT { get; set; }

        public string Unit { get; set; }

        public string BULKLOCATION { get; set; }

        public string INVENTORYLOCATION { get; set; }

        public decimal QTY { get; set; }

        public string TotalQTY { get; set; }

        public string Groupby
        {
            get { return this.StockSP + this.StockSEQ; }
        }
    }
}
