namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public class P08_PrintFabircSticker_PrintData
    {
        /// <summary>
        /// new index
        /// </summary>
        public int Idx { get; set; }

        /// <summary>
        /// SP
        /// </summary>
        public string SP { get; set; }

        /// <summary>
        /// Ref
        /// </summary>
        public string Ref { get; set; }

        /// <summary>
        /// Color
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        /// Qty
        /// </summary>
        public int Qty { get; set; }

        /// <summary>
        /// Roll
        /// </summary>
        public string Roll { get; set; }

        /// <summary>
        /// Fabric Width
        /// </summary>
        public decimal FabWidth { get; set; }

        /// <summary>
        /// Dyelot
        /// </summary>
        public string Dyelot { get; set; }

        /// <summary>
        /// Po_Supp_Detail SizeSpec
        /// </summary>
        public string CutWidth { get; set; }

        /// <summary>
        /// Po_Supp_Detail Special
        /// </summary>
        public string CutType { get; set; }

        /// <summary>
        /// ExpSlice
        /// </summary>
        public string ExpSlice { get; set; }

        /// <summary>
        /// ActSlice
        /// </summary>
        public string ActSlice { get; set; }
    }
}
