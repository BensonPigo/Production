namespace Sci.Production.Warehouse
{
    class P25_PrintData
    {
        public string SP { get; set; }

        public string SEQ { get; set; }

        public string Roll { get; set; }

        public string DYELOT { get; set; }

        public string DESC { get; set; }

        public string Unit { get; set; }

        public string Type { get; set; }

        public string ActQty { get; set; }

        public string oLocation { get; set; }

        public string groupby
        {
            get { return this.SP + this.SEQ; }
        }
    }
}
