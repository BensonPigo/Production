namespace Sci.Production.Warehouse
{
    /// <summary>
    /// P45 RDLC Print Data
    /// </summary>
    internal class P45_PrintData
    {
        /// <summary>
        /// Adjust_Detail.POID
        /// </summary>
        public string POID { get; set; }

        /// <summary>
        /// Adjust_Detail.SEQ
        /// </summary>
        public string SEQ { get; set; }

        /// <summary>
        /// Adjust_Detail.Roll
        /// </summary>
        public string Roll { get; set; }

        /// <summary>
        /// Adjust_Detail.Dyelot
        /// </summary>
        public string Dyelot { get; set; }

        /// <summary>
        /// GetColorMultipleID
        /// </summary>
        public string ColorID { get; set; }

        /// <summary>
        /// getmtldesc
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Adjust_Detail.QtyBefore - Adjust_Detail.QtyAfter
        /// </summary>
        public string AdjustQty { get; set; }

        /// <summary>
        /// PO_Supp_Detail.StockUnit
        /// </summary>
        public string StockUnit { get; set; }

        /// <summary>
        /// Getlocation
        /// </summary>
        public string Location { get; set; }
    }
}