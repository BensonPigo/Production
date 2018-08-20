using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Sewing
{
    public partial class P08 : Sci.Win.Tems.QueryForm
    {
        public P08(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.Helper.Controls.Grid.Generator(this.grid1)
            .Date("ReceiveDate", header: "Receive Date", iseditingreadonly: true)
            .Text("PackingListID", header: "Pack ID", width: Widths.Auto(), iseditingreadonly: false)
            .Text("CTNStartNo", header: "CTN#", width: Widths.Auto(), iseditingreadonly: false)
            .Text("OrderID", header: "SP#", width: Widths.Auto(), iseditingreadonly: false)
            .Text("CustPONo", header: "PO#", width: Widths.Auto(), iseditingreadonly: false)
            .Text("StyleID", header: "Style#", width: Widths.Auto(), iseditingreadonly: false)
            .Text("BrandID", header: "Brand", width: Widths.Auto(), iseditingreadonly: false)
            .Text("Alias", header: "Destination", width: Widths.Auto(), iseditingreadonly: false)
            .Date("BuyerDelivery", header: "Buyer Delivery", width: Widths.Auto(), iseditingreadonly: false)
            .Date("SciDelivery", header: "SCI Delivery", width: Widths.Auto(), iseditingreadonly: false)
            .Text("ReceivedBy", header: "Received By", width: Widths.Auto(), iseditingreadonly: false);
        }

        private void BtnQuery_Click(object sender, EventArgs e)
        {
            this.listControlBindingSource1.DataSource = null;

            string datereceive1 = string.Empty, datereceive2 = string.Empty, packid = string.Empty, sp = string.Empty;
            string sqlwhere = string.Empty;
            if (!MyUtility.Check.Empty(this.dateReceive.TextBox1.Value))
            {
                datereceive1 = this.dateReceive.TextBox1.Text;
                datereceive2 = this.dateReceive.TextBox2.Text;
                sqlwhere += $@" and dr.ReceiveDate between @datereceive1 and @datereceive2 ";
            }

            if (!MyUtility.Check.Empty(this.txtPackID.Text))
            {
                packid = this.txtPackID.Text;
                sqlwhere += $@" and dr.PackingListID = @packid ";
            }

            if (!MyUtility.Check.Empty(this.txtsp.Text))
            {
                sp = this.txtsp.Text;
                sqlwhere += $@" and dr.OrderID  = @sp ";
            }

            string sqlcmd = $@"
declare @datereceive1 date = '{datereceive1}'
declare @datereceive2 date = '{datereceive2}'
declare @packid nvarchar(20) = '{packid}'
declare @sp nvarchar(20) = '{sp}'

select 
	dr.ReceiveDate
	,dr.PackingListID
	,dr.CTNStartNo
	,dr.OrderID
	,o.CustPONo
	,o.StyleID
	,o.BrandID
	,Country.Alias
	,o.BuyerDelivery
	,o.SciDelivery
	,ReceivedBy = dbo.getPass1(dr.AddName)
from DRYReceive dr with(nolock)
left join orders o with(nolock) on dr.OrderID = o.ID
left join Country with(nolock) on Country.id = o.Dest
where 1=1
{sqlwhere}
";
            DataTable dt;
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out dt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            if (dt.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Datas not found!");
            }

            this.listControlBindingSource1.DataSource = dt;
            this.grid1.AutoResizeColumns();
        }
    }
}
