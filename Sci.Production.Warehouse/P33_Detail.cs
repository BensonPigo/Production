using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Sci;
using Ict.Win;
using Ict;
using Sci.Data;
using System.Linq;

namespace Sci.Production.Warehouse
{
    public partial class P33_Detail : Sci.Win.Subs.Input8A
    {
        public DataTable dtIssueBreakDown { get; set; }
        public bool combo, isSave;

        public P33_Detail()
        {
            InitializeComponent();
            this.KeyField1 = "id";
            this.KeyField2 = "Issue_SummaryUkey";
        }
        ///Issue_Detail
        protected override void OnSubDetailInsert(int index = -1)
        {
            var frm = new Sci.Production.Warehouse.P33_Detail_Detail(CurrentDetailData, (DataTable)gridbs.DataSource , CurrentDetailData["AccuIssued"].ToString() , CurrentDetailData["Use Qty By Stock Unit"].ToString());
            frm.P33_Detail = this;
            frm.ShowDialog(this);
            sum_checkedqty();
            //base.OnSubDetailInsert(index);
            //CurrentSubDetailData["Issue_SummaryUkey"] = 0;
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            DataTable temp =(DataTable)gridbs.DataSource;
            if (!temp.Columns.Contains("BulkQty"))
            {
                DataTable dtFtyinventory;
                Ict.DualResult result;
                if (!(result = MyUtility.Tool.ProcessWithDatatable
                        (temp, "", @"  

select t.poid
       , t.Seq1
       , t.Seq2
	   , [BulkQty]= ISNULL(FTY.InQty-FTY.OutQty+ FTY.AdjustQty ,0)
	   , [BulkLocation]= ISNULL( Location.MtlLocationID ,'')
	   , t.Qty
       , t.ID
       , t.Issue_SummaryUkey
       , t.FtyInventoryUkey
       , t.StockType
       , t.ukey
       , t.BarcodeNo
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
", out dtFtyinventory, "#tmp")))
                {
                    MyUtility.Msg.WarningBox(result.ToString());
                    return;
                }
                gridbs.DataSource = dtFtyinventory;
                //dtFtyinventory.DefaultView.Sort = "dyelot,balanceqty desc";
            }

            this.displaySCIRefno.Text = CurrentDetailData["SCIRefno"].ToString();
            this.displayRefno.Text = CurrentDetailData["Refno"].ToString();
            this.displaySCIRefno.Text = CurrentDetailData["SCIRefno"].ToString();
            this.displaySPNo.Text = CurrentDetailData["POID"].ToString();
            this.displayColorID.Text = CurrentDetailData["SuppColor"].ToString();
            this.editDesc.Text = CurrentDetailData["DescDetail"].ToString();

            this.numAccuIssue.Text = CurrentDetailData["AccuIssued"].ToString();
            this.numRequestQty.Text = CurrentDetailData["Use Qty By Stock Unit"].ToString();

            if (MyUtility.Check.Empty(CurrentDetailData["Use Qty By Stock Unit"]))
            {
                this.btnAutoPick.EditMode = Win.UI.AdvEditModes.DisableOnEdit;
            }
            else
            {
                this.btnAutoPick.EditMode = Win.UI.AdvEditModes.EnableOnEdit;
            }

        }

        protected override bool OnGridSetup()
        {
            Ict.Win.DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings();
            ns.CellValidating += (s, e) =>
            {
                // 先排除被刪掉的Row
                DataTable subDT = ((DataTable)gridbs.DataSource).AsEnumerable().Where(o=>o.RowState != DataRowState.Deleted).CopyToDataTable();
                
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
                sum_checkedqty();
            };
            Helper.Controls.Grid.Generator(this.grid)
            .Text("seq1", header: "Seq1", width: Widths.AnsiChars(5), iseditingreadonly: true)  
            .Text("seq2", header: "Seq2", width: Widths.AnsiChars(5), iseditingreadonly: true)
            .Numeric("BulkQty", header: "Bulk Qty", width: Widths.AnsiChars(8), decimal_places: 0, integer_places: 8, iseditingreadonly: true)   
            .Numeric("Qty", header: "Issue Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 8, settings: ns)
            .Text("BulkLocation", header: "Bulk Location", width: Widths.AnsiChars(15), iseditingreadonly: true)
            ;    

            this.grid.Columns["Qty"].DefaultCellStyle.BackColor = Color.Pink;

            return true;
        }

        protected override bool OnSaveBefore()
        {
            grid.ValidateControl();
            var dr2 = ((DataTable)gridbs.DataSource).Select("qty = 0");
            if (dr2.Length > 0)
            {
                MyUtility.Msg.WarningBox("Issue Qty of selected row can't be zero!", "Warning");
                return false;
            }
            var dr2_ba = ((DataTable)gridbs.DataSource).Select("qty > BulkQty");
            if (dr2_ba.Length > 0)
            {
                MyUtility.Msg.WarningBox("Issue Qty of selected row can't be more then Bulk qty!", "Warning");
                return false;
            }
            return base.OnSaveBefore();
        }

        protected override bool OnUndo()
        {
            this.isSave = false;
            return base.OnUndo();
        }

        

        private void btnAutoPick_Click(object sender, EventArgs e)
        {
            decimal AccuIssued = MyUtility.Check.Empty(CurrentDetailData["AccuIssued"]) ? 0 : Convert.ToDecimal(CurrentDetailData["AccuIssued"]);
            List<DataRow> issuedList = PublicPrg.Prgs.Thread_AutoPick(CurrentDetailData, AccuIssued);

            if (issuedList == null)
            {
                return;
            }

            DataTable subDT = (DataTable)gridbs.DataSource;

            foreach (DataRow temp in subDT.ToList()) temp.Delete();
                
            //subDT.Clear();
            foreach (DataRow dr2 in issuedList)
            {
                dr2.AcceptChanges();
                dr2.SetAdded();
                subDT.ImportRow(dr2);
            }
            sum_checkedqty();
        }

        private void sum_checkedqty()
        {
            grid.EndEdit();
            DataTable subDT = (DataTable)gridbs.DataSource;
            Object SumIssueQTY = subDT.Compute("Sum(Qty)", "");
            this.numIssueQty.Text = SumIssueQTY.ToString();
            //this.numVariance.Value = this.numBalanceQty.Value - this.numIssueQty.Value;
            CurrentDetailData["IssueQty"] = MyUtility.Check.Empty(subDT.Compute("Sum(Qty)", "")) ? 0 : (decimal)subDT.Compute("Sum(Qty)", "");
        }

        private void save_Click(object sender, EventArgs e)
        {
            this.isSave = true;
        }

        private void delete_Click(object sender, EventArgs e)
        {
            sum_checkedqty();
        }

    }
}
