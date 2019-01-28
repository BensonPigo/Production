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

namespace Sci.Production.Packing
{
    /// <summary>
    /// P21
    /// </summary>
    public partial class P21 : Sci.Win.Tems.QueryForm
    {
        /// <summary>
        /// P21
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public P21(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.Helper.Controls.Grid.Generator(this.grid1)
            .Date("TransferDate", header: "Transfer Date", iseditingreadonly: true)
            .Text("PackingListID", header: "Pack ID", width: Widths.Auto(), iseditingreadonly: false)
            .Text("CTNStartNo", header: "CTN#", width: Widths.Auto(), iseditingreadonly: false)
            .Text("OrderID", header: "SP#", width: Widths.Auto(), iseditingreadonly: false)
            .Text("CustPONo", header: "PO#", width: Widths.Auto(), iseditingreadonly: false)
            .Text("StyleID", header: "Style#", width: Widths.Auto(), iseditingreadonly: false)
            .Text("BrandID", header: "Brand", width: Widths.Auto(), iseditingreadonly: false)
            .Text("Alias", header: "Destination", width: Widths.Auto(), iseditingreadonly: false)
            .Date("BuyerDelivery", header: "Buyer Delivery", width: Widths.Auto(), iseditingreadonly: false)
            .Date("SciDelivery", header: "SCI Delivery", width: Widths.Auto(), iseditingreadonly: false)
            .Text("TransferredBy", header: "Transferred By", width: Widths.Auto(), iseditingreadonly: false)
            ;
        }

        private void BtnQuery_Click(object sender, EventArgs e)
        {
            this.listControlBindingSource1.DataSource = null;

            string dateTransferDate1 = string.Empty;
            string dateTransferDate2 = string.Empty;
            string packid = string.Empty;
            string sp = string.Empty;
            string sqlwhere = string.Empty;
            if (!MyUtility.Check.Empty(this.dateTransfer.TextBox1.Value))
            {
                dateTransferDate1 = this.dateTransfer.TextBox1.Text;
                dateTransferDate2 = this.dateTransfer.TextBox2.Text;
                sqlwhere += $@" and pe.TransferDate between @dateTransferDate1 and @dateTransferDate2 ";
            }

            if (!MyUtility.Check.Empty(this.txtPackID.Text))
            {
                packid = this.txtPackID.Text;
                sqlwhere += $@" and pe.PackingListID = @packid ";
            }

            if (!MyUtility.Check.Empty(this.txtsp.Text))
            {
                sp = this.txtsp.Text;
                sqlwhere += $@" and pe.OrderID  = @sp ";
            }

            string sqlcmd = $@"
declare @dateTransferDate1 date = '{dateTransferDate1}'
declare @dateTransferDate2 date = '{dateTransferDate2}'
declare @packid nvarchar(20) = '{packid}'
declare @sp nvarchar(20) = '{sp}'

select 
	pe.TransferDate
	,pe.PackingListID
	,pe.CTNStartNo
	,pe.OrderID
	,o.CustPONo
	,o.StyleID
	,o.BrandID
	,Country.Alias
	,o.BuyerDelivery
	,o.SciDelivery
	,[TransferredBy] = dbo.getPass1(pe.AddName)
from PackErrTransfer pe with(nolock)
left join orders o with(nolock) on pe.OrderID = o.ID
left join Country with(nolock) on Country.id = o.Dest
where 1=1
{sqlwhere}
order by pe.PackingListID,pe.CTNStartNo,pe.TransferDate
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
