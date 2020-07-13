using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Data;
using System.Windows.Forms;

namespace Sci.Production.Logistic
{
    /// <inheritdoc/>
    public partial class P13 : Win.Tems.QueryForm
    {
        /// <inheritdoc/>
        public P13(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.grid1.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.grid1)
            .Text("packinglistID", header: "Pack ID", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("CTNStartNo", header: "CTN#", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("CustCTN", header: "Cust CTN#", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("OrderID", header: "SP#", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("CustPONo", header: "PO No.", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("StyleID", header: "Style#", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("SeasonID", header: "Season", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Text("BrandID", header: "Brand", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("Alias", header: "Destination", width: Widths.AnsiChars(12), iseditingreadonly: true)
            .Date("BuyerDelivery", header: "Buyer Delivery", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Date("SCIDelivery", header: "SCI Delivery", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Numeric("ShipQty", header: "Ship Qty", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Numeric("ScanQty", header: "Scan Qty", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Text("ScanName", header: "Scan Name", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .DateTime("ScanEditDate", header: "Scan Date", width: Widths.AnsiChars(20), iseditingreadonly: true)
            .Text("ClogPulloutName", header: "Clog Pullout Name", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Date("ClogPulloutDate", header: "Clog Pullout Date", width: Widths.AnsiChars(20), iseditingreadonly: true)
            .Text("Name", header: "Transport", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("PulloutTransportNo", header: "Transport No", width: Widths.AnsiChars(10), iseditingreadonly: true)
            ;
        }

        private void BtnFind_Click(object sender, EventArgs e)
        {
            this.Find();
        }

        private void Find()
        {
            this.listControlBindingSource1.DataSource = null;

            if (MyUtility.Check.Empty(this.txtSP.Text) && MyUtility.Check.Empty(this.comboDropDownList1.SelectedValue)
                && MyUtility.Check.Empty(this.txtTransportNo.Text) &&
                MyUtility.Check.Empty(this.txtPackID.Text) && MyUtility.Check.Empty(this.datepullout.Value1))
            {
                MyUtility.Msg.WarningBox("SP#, PackID, Transport No, Clog Pullout Date cannot all be empty.");
                return;
            }

            string where = string.Empty;
            if (!MyUtility.Check.Empty(this.txtSP.Text))
            {
                where += $@" and pld.OrderID='{this.txtSP.Text}'";
            }

            if (!MyUtility.Check.Empty(this.txtPackID.Text))
            {
                where += $@" and pld.id='{this.txtPackID.Text}'";
            }

            if (!MyUtility.Check.Empty(this.comboDropDownList1.SelectedValue))
            {
                where += $@" and pld.PulloutTransport='{this.comboDropDownList1.SelectedValue}'";
            }

            if (!MyUtility.Check.Empty(this.txtTransportNo.Text))
            {
                where += $@" and pld.PulloutTransportNo='{this.txtTransportNo.Text}'";
            }

            if (!MyUtility.Check.Empty(this.datepullout.Value1))
            {
                where += $@" and pld.ClogPulloutDate between '{((DateTime)this.datepullout.Value1).ToString("d")}' and '{((DateTime)this.datepullout.Value2).ToString("d")}'";
            }

            string sqlcmd = $@"
select
	packinglistID=pld.ID,pld.CTNStartNo,pld.CustCTN,pld.OrderID,o.CustPONo,o.StyleID,o.SeasonID,o.BrandID,c.Alias,o.BuyerDelivery,
	o.SCIDelivery,pld.ShipQty,pld.ScanQty,pld.ScanName,pld.ScanEditDate,
	pld.ClogPulloutName,
	pld.ClogPulloutDate,
	pld.PulloutTransportNo,
	dl.Name
from PackingList pl with (nolock)
inner join PackingList_Detail pld  with(nolock) on pl.id = pld.id
inner join orders o WITH (NOLOCK) on o.id	= pld.orderid
left join Country c WITH (NOLOCK) on o.Dest = c.ID
left join DropDownList dl on dl.Type = 'Pms_PulloutTransport' and dl.ID = pld.PulloutTransport
where 1=1
        and pl.MDivisionID = '{Env.User.Keyword}'  
        and pld.ClogPulloutDate is not null
        {where}
order by pld.ID,pld.CTNStartNo
";
            DataTable dt;
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out dt);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Query data fail.\r\n" + result.ToString());
                return;
            }

            if (dt.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return;
            }

            this.listControlBindingSource1.DataSource = dt;
        }
    }
}
