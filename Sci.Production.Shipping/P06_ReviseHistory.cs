using System.Data;
using Ict.Win;
using Ict;
using Sci.Data;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// P06_ReviseHistory
    /// </summary>
    public partial class P06_ReviseHistory : Win.Subs.Base
    {
        private string pulloutID;
        private DataGridViewGeneratorNumericColumnSettings oldqty = new DataGridViewGeneratorNumericColumnSettings();
        private DataGridViewGeneratorNumericColumnSettings newqty = new DataGridViewGeneratorNumericColumnSettings();

        /// <summary>
        /// P06_ReviseHistory
        /// </summary>
        /// <param name="pulloutID">pulloutID</param>
        public P06_ReviseHistory(string pulloutID)
        {
            this.InitializeComponent();
            this.pulloutID = pulloutID;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.oldqty.CellMouseDoubleClick += (s, e) =>
                {
                    DataRow dr = this.gridReviseHistory.GetDataRow<DataRow>(e.RowIndex);
                    if (e.Button == System.Windows.Forms.MouseButtons.Left)
                    {
                        P06_ReviseHistory_Detail callNextForm = new P06_ReviseHistory_Detail(dr);
                        callNextForm.ShowDialog(this);
                    }
                };

            this.newqty.CellMouseDoubleClick += (s, e) =>
            {
                DataRow dr = this.gridReviseHistory.GetDataRow<DataRow>(e.RowIndex);
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    P06_ReviseHistory_Detail callNextForm = new P06_ReviseHistory_Detail(dr);
                    callNextForm.ShowDialog(this);
                }
            };

            this.gridReviseHistory.IsEditingReadOnly = true;
            this.gridReviseHistory.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridReviseHistory)
                .DateTime("AddDate", header: "Edit Date", width: Widths.AnsiChars(20))
                .Text("ReviseStatus", header: "Revise Status", width: Widths.AnsiChars(7))
                .Text("OrderID", header: "SP no.", width: Widths.AnsiChars(15))
                .Text("OldShipModeID", header: "Shipping Mode(Original)", width: Widths.AnsiChars(10))
                .Text("ShipmodeID", header: "Shipping Mode(Revised)", width: Widths.AnsiChars(10))
                .Numeric("OldShipQty", header: "Ship Q'ty (Old)", width: Widths.AnsiChars(6), settings: this.oldqty)
                .Numeric("NewShipQty", header: "Ship Q'ty (Revised)", width: Widths.AnsiChars(6), settings: this.newqty)
                .Text("OldStatusExp", header: "Status (Old)", width: Widths.AnsiChars(8))
                .Text("NewStatusExp", header: "Status (New)", width: Widths.AnsiChars(8))
                .Text("StyleID", header: "Style#", width: Widths.AnsiChars(15))
                .Text("BrandID", header: "Brand", width: Widths.AnsiChars(8))
                .Text("Dest", header: "Destination", width: Widths.AnsiChars(2))
                .Text("OrderShipmodeSeq", header: "Order Shipmode Seq", width: Widths.AnsiChars(1))
                .Numeric("OrderQty", header: "Order Q'ty", width: Widths.AnsiChars(5))
                .Numeric("ShipModeSeqQty", header: "Order Q'ty by Seq", width: Widths.AnsiChars(5))
                .Text("PackingListID", header: "Packing#", width: Widths.AnsiChars(15))
                .Text("AddName", header: "Edit Name", width: Widths.AnsiChars(10));

            string sqlCmd = string.Format(
                @"select distinct pr.*,o.StyleID,o.BrandID,o.Dest,o.Qty as OrderQty,pd.OrderShipmodeSeq,oq.Qty as ShipModeSeqQty,
case pr.Type when 'R' then 'Revised' when 'M' then 'Missing' else 'Deleted' end as ReviseStatus,
case pr.OldStatus when 'P' then 'Partial' when 'C' then 'Complete' when 'E' then 'Exceed' when 'E' then 'Shortage' else '' end as OldStatusExp,
case pr.NewStatus when 'P' then 'Partial' when 'C' then 'Complete' when 'E' then 'Exceed' when 'E' then 'Shortage' else '' end as NewStatusExp
from Pullout_Revise pr WITH (NOLOCK) 
left join Orders o WITH (NOLOCK) on o.ID = pr.OrderID
left join PackingList_Detail pd WITH (NOLOCK) on pd.ID = pr.PackingListID and pd.OrderID = pr.OrderID
left join Order_QtyShip oq WITH (NOLOCK) on oq.Id = pr.OrderID and oq.Seq = pd.OrderShipmodeSeq
where pr.ID = '{0}'", this.pulloutID);

            DataTable gridData;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out gridData);
            this.listControlBindingSource1.DataSource = gridData;
        }
    }
}
