namespace Sci.Production.Warehouse
{
    internal class P25_PrintData
    {
        public string SP { get; set; }

        public string SEQ { get; set; }

        public string Roll { get; set; }

        public string DYELOT { get; set; }

        public string DESC { get; set; }

        public string Unit { get; set; }

        public string Type { get; set; }

        public string ActQty { get; set; }

        public string OLocation { get; set; }

        public string Groupby
        {
            get { return this.SP + this.SEQ; }
        }
    }
}
