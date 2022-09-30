using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Data;

namespace Sci.Production.Class
{
    /// <inheritdoc/>
    public partial class WH_BarcodeEmpty : Sci.Win.Tems.QueryForm
    {
        private DataTable dtFty;

        /// <inheritdoc/>
        public WH_BarcodeEmpty(DataTable dt, string Msg)
        {
            this.InitializeComponent();
            this.dtFty = dt;
            this.labMsg.Text = Msg;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.gridInfo.IsEditingReadOnly = true;
            this.gridInfo.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridInfo)
                .Text("POID", header: "SP#", width: Widths.Auto(), iseditingreadonly: true)
                .Text("Seq", header: "Seq", width: Widths.Auto(), iseditingreadonly: true)
                .Text("Roll", header: "Roll", width: Widths.Auto(), iseditingreadonly: true)
                .Text("Dyelot", header: "Dyelot", width: Widths.Auto(), iseditingreadonly: true)
                .Text("StockType", header: "Stock Type", width: Widths.Auto(), iseditingreadonly: true)
                .Numeric("BalanceQty", header: "Balance Qty", width: Widths.Auto(), iseditingreadonly: true)
                ;
            this.GetData();
        }

        private void GetData()
        {
            string sqlcmd = $@"
select POID,Seq = Seq1 + ' ' + Seq2,Roll,Dyelot
,StockType = case StockType 
			when 'I' then 'Inventory'
			when 'B' then 'Bulk'
			when 'O' then 'Scrap'
			else StockType end 
, BalanceQty = InQty-OutQty+AdjustQty-ReturnQty
from FtyInventory f
where exists(
	select 1 from #tmp t
	where t.ukey = f.Ukey
)
";
            DualResult result = MyUtility.Tool.ProcessWithDatatable(this.dtFty, "", sqlcmd, out DataTable dtS);
            if (!result)
            {
                this.ShowErr(result);
            }

            this.listControlBindingSource1.DataSource = dtS;
            this.gridInfo.AutoResizeColumns();
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
