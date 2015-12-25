using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;

namespace Sci.Production.Shipping
{
    public partial class P06_ReviseHistory : Sci.Win.Subs.Base
    {
        string pulloutID;
        Ict.Win.DataGridViewGeneratorNumericColumnSettings oldqty = new Ict.Win.DataGridViewGeneratorNumericColumnSettings();
        Ict.Win.DataGridViewGeneratorNumericColumnSettings newqty = new Ict.Win.DataGridViewGeneratorNumericColumnSettings();
        public P06_ReviseHistory(String pulloutID)
        {
            InitializeComponent();
            this.pulloutID = pulloutID;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            oldqty.CellMouseDoubleClick += (s, e) =>
                {
                    DataRow dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                    if (e.Button == System.Windows.Forms.MouseButtons.Left)
                    {
                        Sci.Production.Shipping.P06_ReviseHistory_Detail callNextForm = new Sci.Production.Shipping.P06_ReviseHistory_Detail(dr);
                        callNextForm.ShowDialog(this);
                    }
                };

            newqty.CellMouseDoubleClick += (s, e) =>
            {
                DataRow dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    Sci.Production.Shipping.P06_ReviseHistory_Detail callNextForm = new Sci.Production.Shipping.P06_ReviseHistory_Detail(dr);
                    callNextForm.ShowDialog(this);
                }
            };

            this.grid1.IsEditingReadOnly = true;
            grid1.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.grid1)
                .DateTime("AddDate", header: "Edit Date", width: Widths.AnsiChars(20))
                .Text("ReviseStatus", header: "Revise Status", width: Widths.AnsiChars(7))
                .Text("OrderID", header: "SP no.", width: Widths.AnsiChars(15))
                .Numeric("OldShipQty", header: "Ship Q'ty (Old)", width: Widths.AnsiChars(6), settings: oldqty)
                .Numeric("NewShipQty", header: "Ship Q'ty (Revised)", width: Widths.AnsiChars(6), settings: newqty)
                .Text("OldStatusExp", header: "Status (Old)", width: Widths.AnsiChars(8))
                .Text("NewStatusExp", header: "Status (New)", width: Widths.AnsiChars(8))
                .Text("StyleID", header: "Style#", width: Widths.AnsiChars(15))
                .Text("BrandID", header: "Brand", width: Widths.AnsiChars(8))
                .Text("Dest", header: "Destination", width: Widths.AnsiChars(2))
                .Text("OrderShipmodeSeq", header: "Order Shipmode Seq", width: Widths.AnsiChars(1))
                .Numeric("OrderQty", header: "Order Q'ty", width: Widths.AnsiChars(5))
                .Numeric("ShipModeSeqQty", header: "Order Q'ty by Seq", width: Widths.AnsiChars(5))
                .Text("ShipmodeID", header: "Shipping Mode", width: Widths.AnsiChars(10))
                .Text("PackingListID", header: "Packing#", width: Widths.AnsiChars(15))
                .Text("AddName", header: "Edit Name", width: Widths.AnsiChars(10));

            string sqlCmd = string.Format(@"select distinct pr.*,o.StyleID,o.BrandID,o.Dest,o.Qty as OrderQty,pd.OrderShipmodeSeq,oq.Qty as ShipModeSeqQty,
case pr.Type when 'R' then 'Revised' when 'M' then 'Missing' else 'Deleted' end as ReviseStatus,
case pr.OldStatus when 'P' then 'Partial' when 'C' then 'Complete' when 'E' then 'Exceed' when 'E' then 'Shortage' else '' end as OldStatusExp,
case pr.NewStatus when 'P' then 'Partial' when 'C' then 'Complete' when 'E' then 'Exceed' when 'E' then 'Shortage' else '' end as NewStatusExp
from Pullout_Revise pr
left join Orders o on o.ID = pr.OrderID
left join PackingList_Detail pd on pd.ID = pr.PackingListID and pd.OrderID = pr.OrderID
left join Order_QtyShip oq on oq.Id = pr.OrderID and oq.Seq = pd.OrderShipmodeSeq
where pr.ID = '{0}'", pulloutID);

            DataTable gridData;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out gridData);
            listControlBindingSource1.DataSource = gridData;
        }
    }
}
