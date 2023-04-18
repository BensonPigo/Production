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
        private bool isSubTransferOrBorrowBack;

        /// <inheritdoc/>
        public WH_BarcodeEmpty(DataTable dt, string msg, bool isSubTransferOrBorrowBack = false)
        {
            this.InitializeComponent();
            this.dtFty = dt;
            this.labMsg.Text = msg;
            this.isSubTransferOrBorrowBack = isSubTransferOrBorrowBack;
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
                .Numeric("BalanceQty", header: "Balance Qty", width: Widths.Auto(), decimal_places: 2, iseditingreadonly: true)
                ;
            this.GetData();
        }

        private void GetData()
        {
            string balanceQty = this.isSubTransferOrBorrowBack ? "t.Qty" : "f.InQty-f.OutQty+f.AdjustQty-f.ReturnQty";
            string sqlcmd = $@"
select f.POID,Seq = f.Seq1 + ' ' + f.Seq2,f.Roll,f.Dyelot
,StockType = case f.StockType 
			when 'I' then 'Inventory'
			when 'B' then 'Bulk'
			when 'O' then 'Scrap'
			else f.StockType end 
, BalanceQty = {balanceQty}
from FtyInventory f
inner join #tmp t on t.ukey = f.Ukey
";
            DualResult result = MyUtility.Tool.ProcessWithDatatable(this.dtFty, string.Empty, sqlcmd, out DataTable dtS);
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
