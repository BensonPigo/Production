namespace Sci.Production.Prg.Entity
{
#pragma warning disable SA1600
#pragma warning disable SA1516
    public class IssueQtyBreakdown
    {
        /// <inheritdoc/>
        public string OrderID { get; set; }

        /// <inheritdoc/>
        public string Article { get; set; }

        /// <inheritdoc/>
        public string SizeCode { get; set; }

        /// <inheritdoc/>
        public int Qty { get; set; }
    }
#pragma warning restore SA1600
#pragma warning restore SA1516
}
