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
    /// P22
    /// </summary>
    public partial class P22 : Sci.Win.Tems.QueryForm
    {
        /// <summary>
        /// P22
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public P22(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.Helper.Controls.Grid.Generator(this.grid1)
            .Date("CFMDate", header: "Confirm Date", iseditingreadonly: true)
            .Text("PackingListID", header: "Pack ID", width: Widths.Auto(), iseditingreadonly: false)
            .Text("CTNStartNo", header: "CTN#", width: Widths.Auto(), iseditingreadonly: false)
            .Text("OrderID", header: "SP#", width: Widths.Auto(), iseditingreadonly: false)
            .Text("CustPONo", header: "PO#", width: Widths.Auto(), iseditingreadonly: false)
            .Text("StyleID", header: "Style#", width: Widths.Auto(), iseditingreadonly: false)
            .Text("BrandID", header: "Brand", width: Widths.Auto(), iseditingreadonly: false)
            .Text("Alias", header: "Destination", width: Widths.Auto(), iseditingreadonly: false)
            .Date("BuyerDelivery", header: "Buyer Delivery", width: Widths.Auto(), iseditingreadonly: false)
            .Date("SciDelivery", header: "SCI Delivery", width: Widths.Auto(), iseditingreadonly: false)
            .Text("ConfirmedBy", header: "Confirmed By", width: Widths.Auto(), iseditingreadonly: false)
            ;
        }

        private void BtnQuery_Click(object sender, EventArgs e)
        {
            this.listControlBindingSource1.DataSource = null;

            string dateCFMDate1 = string.Empty;
            string dateCFMDate2 = string.Empty;
            string packid = string.Empty;
            string sp = string.Empty;
            string sqlwhere = string.Empty;
            if (!MyUtility.Check.Empty(this.dateConfirm.TextBox1.Value))
            {
                dateCFMDate1 = this.dateConfirm.TextBox1.Text;
                dateCFMDate2 = this.dateConfirm.TextBox2.Text;
                sqlwhere += $@" and pe.CFMDate between @dateCFMDate1 and @dateCFMDate2 ";
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
declare @dateCFMDate1 date = '{dateCFMDate1}'
declare @dateCFMDate2 date = '{dateCFMDate2}'
declare @packid nvarchar(20) = '{packid}'
declare @sp nvarchar(20) = '{sp}'

select 
	pe.CFMDate
	,pe.PackingListID
	,pe.CTNStartNo
	,pe.OrderID
	,o.CustPONo
	,o.StyleID
	,o.BrandID
	,Country.Alias
	,o.BuyerDelivery
	,o.SciDelivery
	,[ConfirmedBy] = dbo.getPass1(pe.AddName)
from PackErrCFM pe with(nolock)
left join orders o with(nolock) on pe.OrderID = o.ID
left join Country with(nolock) on Country.id = o.Dest
where 1=1
{sqlwhere}
order by pe.PackingListID,pe.CTNStartNo,pe.CFMDate
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
