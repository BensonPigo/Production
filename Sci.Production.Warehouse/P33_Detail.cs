using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using Ict.Win;
using Ict;
using System.Linq;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P33_Detail : Win.Subs.Input8A
    {
        /// <inheritdoc/>
        public DataTable DtIssueBreakDown { get; set; }

        private string oriSuppColor = string.Empty;

        /// <inheritdoc/>
        public P33_Detail()
        {
            this.InitializeComponent();
            this.KeyField1 = "id";
            this.KeyField2 = "Issue_SummaryUkey";
        }

        /// <inheritdoc/>
        protected override void OnSubDetailInsert(int index = -1)
        {
            var frm = new P33_Detail_Detail(this.CurrentDetailData, (DataTable)this.gridbs.DataSource, this.CurrentDetailData["AccuIssued"].ToString(), this.CurrentDetailData["Use Qty By Stock Unit"].ToString());
            frm.P33_Detail = this;
            frm.ShowDialog(this);
            this.Sum_checkedqty();

            // base.OnSubDetailInsert(index);
            // CurrentSubDetailData["Issue_SummaryUkey"] = 0;
        }

        /// <inheritdoc/>
        protected override void OnAttached()
        {
            base.OnAttached();

            DataTable temp = (DataTable)this.gridbs.DataSource;
            if (!temp.Columns.Contains("BulkQty"))
            {
                DataTable dtFtyinventory;
                DualResult result;
                string c = @"  

select t.poid
       , t.Seq1
       , t.Seq2
	   , [BulkQty]= ISNULL(FTY.InQty - FTY.OutQty + FTY.AdjustQty - FTY.ReturnQty, 0)
	   , [BulkLocation]= ISNULL( Location.MtlLocationID ,'')
	   , t.Qty
       , t.ID
       , t.Issue_SummaryUkey
       , t.FtyInventoryUkey
       , t.StockType
       , t.ukey
from #tmp t    ---- #tmp = Issue_Detail
Left join dbo.FtyInventory FTY WITH (NOLOCK) on  t.FtyInventoryUkey=FTY.Ukey  ----t.POID = FTY.POID AND t.Seq1 = FTY.Seq1 AND t.Seq2 = FTY.Seq2 
OUTER APPLY(
	SELECT   [MtlLocationID] = STUFF(
	(
		SELECT DISTINCT ',' +fid.MtlLocationID 
		FROM FtyInventory_Detail FID 
		WHERE FID.Ukey= FTY.Ukey AND  fid.MtlLocationID  <> ''
		FOR XML PATH('')
	), 1, 1, '') 
)Location
WHERE (FTY.stocktype = 'B' OR FTY.stocktype IS NULL)
";
                if (!(result = MyUtility.Tool.ProcessWithDatatable(
                        temp, string.Empty,  c, out dtFtyinventory, "#tmp")))
                {
                    MyUtility.Msg.WarningBox(result.ToString());
                    return;
                }

                this.gridbs.DataSource = dtFtyinventory;

                // dtFtyinventory.DefaultView.Sort = "dyelot,balanceqty desc";
            }

            this.displaySCIRefno.Text = this.CurrentDetailData["SCIRefno"].ToString();
            this.displayRefno.Text = this.CurrentDetailData["Refno"].ToString();
            this.displaySCIRefno.Text = this.CurrentDetailData["SCIRefno"].ToString();
            this.displaySPNo.Text = this.CurrentDetailData["POID"].ToString();
            this.displayColorID.Text = this.CurrentDetailData["ColorID"].ToString();
            this.displaySuppColor.Text = this.CurrentDetailData["SuppColor"].ToString();
            this.oriSuppColor = this.CurrentDetailData["SuppColor"].ToString();
            this.editDesc.Text = this.CurrentDetailData["DescDetail"].ToString();

            this.numAccuIssue.Text = this.CurrentDetailData["AccuIssued"].ToString();
            this.numRequestQty.Text = this.CurrentDetailData["Use Qty By Stock Unit"].ToString();

            if (MyUtility.Check.Empty(this.CurrentDetailData["Use Qty By Stock Unit"]))
            {
                this.btnAutoPick.EditMode = Win.UI.AdvEditModes.DisableOnEdit;
            }
            else
            {
                this.btnAutoPick.EditMode = Win.UI.AdvEditModes.EnableOnEdit;
            }
        }

        /// <inheritdoc/>
        protected override bool OnGridSetup()
        {
            DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings();
            ns.CellValidating += (s, e) =>
            {
                // 先排除被刪掉的Row
                DataTable subDT = ((DataTable)this.gridbs.DataSource).AsEnumerable().Where(o => o.RowState != DataRowState.Deleted).CopyToDataTable();

                DataRow row = subDT.Rows[e.RowIndex];

                if (row.RowState == DataRowState.Deleted)
                {
                    return;
                }

                if (MyUtility.Check.Empty(row["BulkQty"]))
                {
                    return;
                }

                if (Convert.ToDecimal(row["BulkQty"]) < Convert.ToDecimal(e.FormattedValue))
                {
                    MyUtility.Msg.InfoBox("[Issue Qty] Can't over [Bulk Qty]!!");
                    row["Qty"] = row["Qty"];
                    return;
                }

                row["Qty"] = Convert.ToInt32(e.FormattedValue);
                subDT.EndInit();
                this.Sum_checkedqty();
            };
            this.Helper.Controls.Grid.Generator(this.grid)
            .Text("seq1", header: "Seq1", width: Widths.AnsiChars(5), iseditingreadonly: true)
            .Text("seq2", header: "Seq2", width: Widths.AnsiChars(5), iseditingreadonly: true)
            .Numeric("BulkQty", header: "Bulk Qty", width: Widths.AnsiChars(8), decimal_places: 0, integer_places: 8, iseditingreadonly: true)
            .Numeric("Qty", header: "Issue Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 8, settings: ns)
            .Text("BulkLocation", header: "Bulk Location", width: Widths.AnsiChars(15), iseditingreadonly: true)
            ;

            this.grid.Columns["Qty"].DefaultCellStyle.BackColor = Color.Pink;

            return true;
        }

        /// <inheritdoc/>
        protected override bool OnSaveBefore()
        {
            this.grid.ValidateControl();
            var dr2 = ((DataTable)this.gridbs.DataSource).Select("qty = 0");
            if (dr2.Length > 0)
            {
                MyUtility.Msg.WarningBox("Issue Qty of selected row can't be zero!", "Warning");
                return false;
            }

            var dr2_ba = ((DataTable)this.gridbs.DataSource).Select("qty > BulkQty");
            if (dr2_ba.Length > 0)
            {
                MyUtility.Msg.WarningBox("Issue Qty of selected row can't be more then Bulk qty!", "Warning");
                return false;
            }

            return base.OnSaveBefore();
        }

        /// <inheritdoc/>
        protected override bool OnUndo()
        {
            this.CurrentDetailData["SuppColor"] = this.oriSuppColor;
            return base.OnUndo();
        }

        private void BtnAutoPick_Click(object sender, EventArgs e)
        {
            decimal accuIssued = MyUtility.Check.Empty(this.CurrentDetailData["AccuIssued"]) ? 0 : Convert.ToDecimal(this.CurrentDetailData["AccuIssued"]);
            List<DataRow> issuedList = PublicPrg.Prgs.Thread_AutoPick(this.CurrentDetailData, accuIssued);

            if (issuedList == null)
            {
                return;
            }

            DataTable subDT = (DataTable)this.gridbs.DataSource;

            foreach (DataRow temp in subDT.ToList())
            {
                temp.Delete();
            }

            // subDT.Clear();
            foreach (DataRow dr2 in issuedList)
            {
                dr2.AcceptChanges();
                dr2.SetAdded();
                subDT.ImportRow(dr2);
            }

            this.Sum_checkedqty();
        }

        private void Sum_checkedqty()
        {
            this.grid.EndEdit();
            DataTable subDT = (DataTable)this.gridbs.DataSource;
            object sumIssueQTY = subDT.Compute("Sum(Qty)", string.Empty);
            this.numIssueQty.Text = sumIssueQTY.ToString();

            // this.numVariance.Value = this.numBalanceQty.Value - this.numIssueQty.Value;
            this.CurrentDetailData["IssueQty"] = MyUtility.Check.Empty(subDT.Compute("Sum(Qty)", string.Empty)) ? 0 : (decimal)subDT.Compute("Sum(Qty)", string.Empty);

            #region 根據新的Seq，動態串出SuppColor，並寫入Issue_Summary
            List<string> allSuppColor = new List<string>();

            foreach (DataRow item in subDT.AsEnumerable().Where(o => o.RowState != DataRowState.Deleted).ToList())
            {
                string seq1 = item["Seq1"].ToString();
                string seq2 = item["Seq2"].ToString();
                string suppColor = MyUtility.GetValue.Lookup($@"
SELECT SuppColor 
FROM PO_Supp_Detail WITH(NOLOCK) 
WHERE ID = '{this.CurrentDetailData["POID"]}' 
AND SCIRefno = '{this.CurrentDetailData["SCIRefno"]}' 
AND ColorID='{this.CurrentDetailData["ColorID"]}'
AND Seq1 = '{seq1}' AND Seq2 = '{seq2}'
");

                // 重複就不加進去了
                if (!allSuppColor.Contains(suppColor))
                {
                    allSuppColor.Add(suppColor);
                }

                // 若還沒有Issue.ID，表示是全新的第三層，因此除了Deleted的都是Add
                if (item.RowState != DataRowState.Added && MyUtility.Check.Empty(this.CurrentDetailData["ID"]))
                {
                    item.SetAdded();
                }
            }

            this.CurrentDetailData["SuppColor"] = allSuppColor.JoinToString(",");
            this.displaySuppColor.Text = this.CurrentDetailData["SuppColor"].ToString();
            #endregion
        }

        private void Save_Click(object sender, EventArgs e)
        {
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            this.Sum_checkedqty();
        }
    }
}
