using System.Windows.Forms;
using Ict.Win;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// P06_ShipQtyDetail
    /// </summary>
    public partial class P06_ShipQtyDetail : Sci.Win.Subs.Input8A
    {
        /// <summary>
        /// P06_ShipQtyDetail
        /// </summary>
        public P06_ShipQtyDetail()
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnAttached()
        {
            base.OnAttached();
            this.displaySPNo.Value = MyUtility.Convert.GetString(this.CurrentDetailData["OrderID"]);
            this.displaySEQ.Value = MyUtility.Convert.GetString(this.CurrentDetailData["OrderShipmodeSeq"]);
            this.displayPackingNo.Value = MyUtility.Convert.GetString(this.CurrentDetailData["PackingListID"]);
            this.displayShipMode.Value = MyUtility.Convert.GetString(this.CurrentDetailData["ShipmodeID"]);
            this.displayStatus.Value = MyUtility.Convert.GetString(this.CurrentDetailData["StatusExp"]);
            this.numOrderQty.Value = MyUtility.Convert.GetInt(this.CurrentDetailData["OrderQty"]);
            this.numOrderQtybySeq.Value = MyUtility.Convert.GetInt(this.CurrentDetailData["ShipModeSeqQty"]);
            this.numShipQty.Value = MyUtility.Convert.GetInt(this.CurrentDetailData["ShipQty"]);
        }

        /// <inheritdoc/>
        protected override bool OnGridSetup()
        {
            this.Helper.Controls.Grid.Generator(this.grid)
                .Text("Article", header: "Color way", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Numeric("Qty", header: "Order Q'ty by Seq", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("ShipQty", header: "Ship Q'ty", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("Variance", header: "Variance", width: Widths.AnsiChars(10), iseditingreadonly: true);

            for (int i = 0; i < this.grid.ColumnCount; i++)
            {
                this.grid.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            return true;
        }
    }
}
