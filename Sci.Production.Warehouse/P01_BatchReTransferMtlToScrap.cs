using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;
using System.Transactions;
using System.Linq;

namespace Sci.Production.Warehouse
{
    public partial class P01_BatchReTransferMtlToScrap : Win.Subs.Base
    {
        private DataRow dr_master;
        private DataTable dt_detail;
        private Dictionary<string, string> di_fabrictype = new Dictionary<string, string>();
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        private DataTable dtReTransferToScrapList;
        protected DataTable[] dtBatch;

        public P01_BatchReTransferMtlToScrap()
        {
            this.InitializeComponent();
            this.di_fabrictype.Add("F", "Fabric");
            this.di_fabrictype.Add("A", "Accessory");
            MyUtility.Tool.SetupCombox(this.comboCategory, 2, 1, ",All,B,Bulk,S,Sample,M,Material");
            this.comboCategory.SelectedIndex = 0;
        }

        public P01_BatchReTransferMtlToScrap(DataRow master, DataTable detail)
            : this()
        {
            this.dr_master = master;
            this.dt_detail = detail;
        }

        // Find Now Button
        private void BtnFindNow_Click(object sender, EventArgs e)
        {
            this.QueryData(false);
        }

        public void QueryData(bool autoQuery)
        {
            DateTime? pulloutdate1, pulloutdate2, buyerDelivery1, buyerDelivery2;
            string strSQLCmd = string.Empty;
            string sqlWhere = string.Empty;
            pulloutdate1 = this.datePullOutDate.Value1;
            pulloutdate2 = this.datePullOutDate.Value2;
            buyerDelivery1 = this.dateBuyerDelivery.Value1;
            buyerDelivery2 = this.dateBuyerDelivery.Value2;
            string sp1 = this.txtSPNoStart.Text.TrimEnd();
            string sp2 = this.txtSPNoEnd.Text.TrimEnd();
            string category = this.comboCategory.SelectedValue.ToString();
            string style = this.txtstyle.Text;
            string brand = this.txtbrand.Text;
            string factory = this.txtmfactory.Text;

            if (!autoQuery &&
                MyUtility.Check.Empty(this.datePullOutDate.Value1) &&
                MyUtility.Check.Empty(this.dateBuyerDelivery.Value1) &&
                (MyUtility.Check.Empty(this.txtSPNoStart.Text) || MyUtility.Check.Empty(this.txtSPNoEnd.Text)))
            {
                MyUtility.Msg.WarningBox("< Pullout Date > & < Buyer Delivery > & < SP# > can't be empty!!");
                return;
            }

            if (!MyUtility.Check.Empty(pulloutdate1))
            {
                sqlWhere += $@" and o.ActPulloutDate between '{Convert.ToDateTime(pulloutdate1).ToString("d")}' and '{Convert.ToDateTime(pulloutdate2).ToString("d")}'";
            }

            if (!MyUtility.Check.Empty(buyerDelivery1))
            {
                sqlWhere += $@" and o.BuyerDelivery between '{Convert.ToDateTime(buyerDelivery1).ToString("d")}' and '{Convert.ToDateTime(buyerDelivery2).ToString("d")}'";
            }

            if (!MyUtility.Check.Empty(sp1) || !MyUtility.Check.Empty(sp2))
            {
                sqlWhere += $@" and f.poid between '{sp1}' and '{sp2}'";
            }

            if (!MyUtility.Check.Empty(style))
            {
                sqlWhere += $@" and styleid = '{style}' ";
            }

            if (!MyUtility.Check.Empty(brand))
            {
                sqlWhere += $@" and o.brandid = '{brand}' ";
            }

            if (!MyUtility.Check.Empty(factory))
            {
                sqlWhere += $@" and o.factoryid = '{factory}' ";
            }

            if (!MyUtility.Check.Empty(category))
            {
                sqlWhere += $@" and o.category = '{category}' ";
            }

            strSQLCmd = $@"
select  f.POID,
        f.Seq1,
        f.Seq2,
		o.FactoryID,
		Category =case when o.Category='B'then 'Bulk'
			        when o.Category='M'then 'Material'
			        when o.Category='O'then 'Other'
			        when o.Category='S'then 'Sample'
			        end,
		o.StyleID,
		o.BrandID,
		o.BuyerDelivery,
        f.Lock,
        [MDivisionPoDetailUkey] = mdp.Ukey,
        f.Ukey
into #ReTransferToScrapList
from FtyInventory f with (nolock)
inner join orders o with (nolock) on o.id = f.POID
left join PO_Supp_Detail p1 WITH (NOLOCK) on p1.ID = f.PoId and p1.seq1 = f.Seq1 and p1.SEQ2 = f.Seq2
left join MDivisionPoDetail mdp on  mdp.POID=p1.ID and 
                                    mdp.Seq1=p1.SEQ1 and 
                                    mdp.Seq2=p1.SEQ2
where   f.InQty - f.OutQty+f.AdjustQty >0 and f.StockType='B' and o.WhseClose is not null 
        {sqlWhere}

select  [select] = 0,
        POID,
        FactoryID,
        Category,
        StyleID,
        BrandID,
        BuyerDelivery,
        [BulkMtlQty]  = count(1),
        [LockCnt] = sum(iif(Lock = 1,1,0))
into #ReTransferToScrapSummary
from #ReTransferToScrapList
group by POID,FactoryID,StyleID,BrandID,BuyerDelivery,Category

select POID,Seq1,Seq2,MDivisionPoDetailUkey,Ukey
from #ReTransferToScrapList

select	[Selected] = 0,
        POID,
        FactoryID,
        Category,
        StyleID,
        BrandID,
        BuyerDelivery,
        BulkMtlQty,
        LockCnt,
		[LastPulloutDate] =orderData.LastPulloutDate,
		[LastPPICCloseDate] = orderData.LastPPICCloseDate
from #ReTransferToScrapSummary s
outer apply (select [LastPulloutDate] = max(o.ActPulloutDate),[LastPPICCloseDate] = max(o.gmtclose)  
					from orders o with (nolock) where o.POID = s.POID) orderData

drop table #ReTransferToScrapList,#ReTransferToScrapSummary
";

            this.ShowWaitMessage("Data Loading....");

            DualResult result;
            if (result = DBProxy.Current.Select(null, strSQLCmd, out this.dtBatch))
            {
                if (this.dtBatch[1].Rows.Count == 0)
                {
                    this.HideWaitMessage();
                    MyUtility.Msg.WarningBox("Data not found!!");
                }

                this.dtReTransferToScrapList = this.dtBatch[0];
                this.listControlBindingSource1.DataSource = this.dtBatch[1];
                this.ChangeGridstyle();
            }
            else
            {
                this.ShowErr(strSQLCmd.ToString(), result);
            }

            this.HideWaitMessage();
        }

        private void ChangeGridstyle()
        {
            foreach (DataRow item in this.dtBatch[1].Rows)
            {
                if ((int)item["LockCnt"] > 0)
                {
                    int rowindex = this.gridBatchCloseRowMaterial.GetRowIndexByDataRow(item);
                    this.gridBatchCloseRowMaterial.Rows[rowindex].DefaultCellStyle.BackColor = Color.Silver;
                    this.gridBatchCloseRowMaterial.Rows[rowindex].Cells["Selected"].ReadOnly = true;
                }
            }
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.gridBatchCloseRowMaterial.IsEditingReadOnly = false; // 必設定, 否則CheckBox會顯示圖示
            this.gridBatchCloseRowMaterial.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridBatchCloseRowMaterial)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk) // 0
                .Text("poid", header: "PO#", iseditingreadonly: true, width: Widths.AnsiChars(13)) // 1
                .Text("factoryid", header: "Factory", iseditingreadonly: true, width: Widths.AnsiChars(8)) // 1
                .Text("category", header: "Category", iseditingreadonly: true, width: Widths.AnsiChars(8)) // 4
                .Text("styleid", header: "Style", iseditingreadonly: true, width: Widths.AnsiChars(20)) // 3
                .Text("brandid", header: "Brand", iseditingreadonly: true) // 5
                .Date("buyerdelivery", header: "Buyer Delivery", iseditingreadonly: true) // 5
                .Numeric("BulkMtlQty", header: "Bulk Mtl. Qty", iseditingreadonly: true) // 5
                .Date("LastPulloutDate", header: "Last Pullout Date", iseditingreadonly: true) // 5
                .Date("LastPPICCloseDate", header: "Last PPIC Close", iseditingreadonly: true) // 5
               ; // 8
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnBatchCloseRMTL_Click(object sender, EventArgs e)
        {
            this.gridBatchCloseRowMaterial.ValidateControl();
            DataTable dtGridBS1 = (DataTable)this.listControlBindingSource1.DataSource;
            if (MyUtility.Check.Empty(dtGridBS1) || dtGridBS1.Rows.Count == 0)
            {
                return;
            }

            DataRow[] drSelects = dtGridBS1.Select("Selected = 1");
            if (drSelects.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select rows first!", "Warnning");
                return;
            }

            DialogResult dResult = MyUtility.Msg.QuestionBox("Do you want to Re-Transfer Mtl. to Scrap?");
            if (dResult.ToString().ToUpper() == "NO")
            {
                return;
            }

            this.ShowWaitMessage("Processing...");
            using (TransactionScope transactionScope = new TransactionScope())
            {
                var listReTransferToScrap = this.dtReTransferToScrapList.AsEnumerable();
                foreach (DataRow transToScrapPO in drSelects)
                {
                    var listMtlItem = listReTransferToScrap.Where(s => s["POID"].ToString() == transToScrapPO["POID"].ToString()).ToList();

                    if (!listMtlItem.Any())
                    {
                        continue;
                    }

                    DualResult result = PublicPrg.Prgs.ReTransferMtlToScrapByPO(transToScrapPO["POID"].ToString(), listMtlItem);
                    if (!result)
                    {
                        transactionScope.Dispose();
                        this.ShowErr(result);
                        return;
                    }
                }

                transactionScope.Complete();
            }

            MyUtility.Msg.InfoBox("Finish Re-Transfer Mtl. to Scrap!!");
            this.HideWaitMessage();

            this.QueryData(true);
        }

        private void GridBatchCloseRowMaterial_Sorted(object sender, EventArgs e)
        {
            this.ChangeGridstyle();
        }
    }
}
