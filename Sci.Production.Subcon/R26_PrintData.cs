namespace Sci.Production.Subcon
{
    class R26_PrintData
    {
        public string PO { get; set; }

        public string Code { get; set; }

        public string Color_Shade { get; set; }

        public string Description { get; set; }

        public decimal Quantity { get; set; }

        public string Unit { get; set; }

        public string Unit_Price { get; set; }

        public string Amount { get; set; }

        public string AccuAmount { get; set; }

        public string Total_Quantity { get; set; }

        public string Remark { get; set; }

        public string Title1 { get; set; }

        public string Issue_Date { get; set; }

        public string To { get; set; }

        public string Delivery_Date { get; set; }

        public string Title2 { get; set; }

        public string Title3 { get; set; }

        public string Tel { get; set; }

        public string Fax { get; set; }

        public string Total1 { get; set; }

        public string Total2 { get; set; }

        public string CurrencyId { get; set; }

        public string vat { get; set; }

        public string Grand_Total { get; set; }

        public string Group1
        {
            get { return this.To + this.Title1 + this.Issue_Date + this.Delivery_Date; }
        }

        public string Group2
        {
            get { return this.To + this.Title1 + this.Issue_Date + this.Delivery_Date + this.PO; }
        }

        public string ID { get; set; }
    }
}
