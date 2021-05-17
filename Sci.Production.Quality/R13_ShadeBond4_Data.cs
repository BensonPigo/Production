namespace Sci.Production.Quality
{
    internal class R13_ShadeBond4_Data
    {
        public string FactoryID { get; set; }

        public string POID { get; set; }

        public string ReceivingID { get; set; }

        public string Style { get; set; }

        public string Color { get; set; }

        public string BrandID { get; set; }

        public string Supp { get; set; }

        public string Invo { get; set; }

        public string ETA { get; set; }

        public string Refno { get; set; }

        public string Packages { get; set; }

        public string Seq { get; set; }

        public string Group
        {
            get { return this.ReceivingID + this.POID + this.Seq; }
        }
    }
}
