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
    public partial class P09 : Sci.Win.Tems.QueryForm
    {
        public P09(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();

            string selectCommand = string.Format("select ID = '',IDName = '' union all select ID, Name as IDName from DropDownList WITH (NOLOCK) where Type = '{0}' order by Seq", "Pms_DRYTransferTo");
            Ict.DualResult returnResult;
            DataTable dropDownListTable = new DataTable();
            if (returnResult = DBProxy.Current.Select(null, selectCommand, out dropDownListTable))
            {
                this.txtdropdownlist1.DataSource = dropDownListTable;
                this.txtdropdownlist1.DisplayMember = "IDName";
                this.txtdropdownlist1.ValueMember = "ID";
            }
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.Helper.Controls.Grid.Generator(this.grid1)
            .Text("TransferTo", header: "Transfer To", width: Widths.Auto(), iseditingreadonly: true)
            .Date("TransferDate", header: "Transfer Date", iseditingreadonly: true)
            .Text("PackingListID", header: "Pack ID", width: Widths.Auto(), iseditingreadonly: true)
            .Text("CTNStartNo", header: "CTN#", width: Widths.Auto(), iseditingreadonly: true)
            .Text("OrderID", header: "SP#", width: Widths.Auto(), iseditingreadonly: true)
            .Text("CustPONo", header: "PO#", width: Widths.Auto(), iseditingreadonly: true)
            .Text("StyleID", header: "Style#", width: Widths.Auto(), iseditingreadonly: true)
            .Text("BrandID", header: "Brand", width: Widths.Auto(), iseditingreadonly: true)
            .Text("Alias", header: "Destination", width: Widths.Auto(), iseditingreadonly: true)
            .Date("BuyerDelivery", header: "Buyer Delivery", width: Widths.Auto(), iseditingreadonly: true)
            .Date("SciDelivery", header: "SCI Delivery", width: Widths.Auto(), iseditingreadonly: true)
            .Text("TransferBy", header: "Transfer By", width: Widths.Auto(), iseditingreadonly: true);
        }

        private void BtnQuery_Click(object sender, EventArgs e)
        {
            this.listControlBindingSource1.DataSource = null;

            string dateTransfer1 = string.Empty, dateTransfer2 = string.Empty, packid = string.Empty, sp = string.Empty, transferTo = string.Empty;
            string sqlwhere = string.Empty;
            if (!MyUtility.Check.Empty(this.dateTransfer.TextBox1.Value))
            {
                dateTransfer1 = this.dateTransfer.TextBox1.Text;
                dateTransfer2 = this.dateTransfer.TextBox2.Text;
                sqlwhere += $@" and dr.TransferDate between @TransferDate1 and @TransferDate2 ";
            }

            if (!MyUtility.Check.Empty(this.txtdropdownlist1.Text))
            {
                transferTo = MyUtility.Convert.GetString(this.txtdropdownlist1.SelectedValue);
                sqlwhere += $@" and dr.TransferTo = @TransferTo";
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
declare @TransferDate1  date = '{dateTransfer1}'
declare @TransferDate2  date = '{dateTransfer2}'
declare @packid nvarchar(20) = '{packid}'
declare @sp nvarchar(20) = '{sp}'
declare @TransferTo nvarchar(20) = '{transferTo}'

select 
	dr.TransferTo
	,dr.TransferDate
	,dr.PackingListID
	,dr.CTNStartNo
	,dr.OrderID
	,o.CustPONo
	,o.StyleID
	,o.BrandID
	,Country.Alias
	,o.BuyerDelivery
	,o.SciDelivery
	,TransferBy = dbo.getPass1(dr.AddName)
from DRYTransfer dr with(nolock)
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
