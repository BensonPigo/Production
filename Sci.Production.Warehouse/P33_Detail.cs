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
        public P33_Detail()
        {
            InitializeComponent();
            this.KeyField1 = "id";
            this.KeyField2 = "Issue_SummaryUkey";
        }
        ///Issue_Detail
        protected override void OnSubDetailInsert(int index = -1)
        {
            var frm = new Sci.Production.Warehouse.P33_Detail_Detail(CurrentDetailData, (DataTable)gridbs.DataSource);
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
            if (!temp.Columns.Contains("balanceqty"))
            {
                DataTable dtFtyinventory;
                Ict.DualResult result;
                if (!(result = MyUtility.Tool.ProcessWithDatatable
                        (temp, "", @"  

select t.poid
       , t.Seq1
       , t.Seq2
	   , [BulkQty]=FTY.InQty-FTY.OutQty+ FTY.AdjustQty
	   , t.Qty
	   , [BulkLocation]=FTYD.MtlLocationID
from #tmp t    ---- #tmp = Issue_Detail
Left join dbo.FtyInventory FTY WITH (NOLOCK) on t.FtyInventoryUkey=FTY.Ukey AND FTY.StockType = 'B'
Left JOIN FtyInventory_Detail FTYD WITH (NOLOCK)  ON FTYD.Ukey= FTY.Ukey
", out dtFtyinventory, "#tmp")))
                {
                    MyUtility.Msg.WarningBox(result.ToString());
                    return;
                }
                gridbs.DataSource = dtFtyinventory;
                //dtFtyinventory.DefaultView.Sort = "dyelot,balanceqty desc";
            }

            this.displayRefno.Text = CurrentDetailData["Refno"].ToString();
            this.displaySPNo.Text = CurrentDetailData["POID"].ToString();
            this.displayColorID.Text = CurrentDetailData["SuppColor"].ToString();
            this.editDesc.Text = CurrentDetailData["DescDetail"].ToString();

            this.numAccuIssue.Text = CurrentDetailData["AccuIssued"].ToString();
            this.numRequestQty.Text = CurrentDetailData["Use Qty By Stock Unit"].ToString();
       
            //this.numIssueQty.Text = //Sum([表身][Issue Qty])

        }

        protected override bool OnGridSetup()
        {
            Ict.Win.DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings();
            ns.CellValidating += (s, e) =>
            {
                DataTable subDT = (DataTable)gridbs.DataSource;

                DataRow row = subDT.Rows[e.RowIndex];

                if (MyUtility.Check.Empty(row["BulkQty"]))
                {
                    return;
                }

                if (Convert.ToDecimal(row["BulkQty"]) < Convert.ToDecimal(e.FormattedValue))
                {
                    MyUtility.Msg.InfoBox("[Issue Qty] Can't over [Bulk Qty]!!");
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
            var dr2_ba = ((DataTable)gridbs.DataSource).Select("qty > balanceqty");
            if (dr2_ba.Length > 0)
            {
                MyUtility.Msg.WarningBox("Issue Qty of selected row can't be more then balance qty!", "Warning");
                return false;
            }
            return base.OnSaveBefore();
        } 

        private void btnAutoPick_Click(object sender, EventArgs e)
        {
            var issued = PublicPrg.Prgs.autopick(CurrentDetailData);
            if (issued == null)
            {
                return;
            }

            DataTable subDT = (DataTable)gridbs.DataSource;

            foreach (DataRow temp in subDT.ToList()) temp.Delete();
                
            //subDT.Clear();
            foreach (DataRow dr2 in issued)
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

        }

        private void delete_Click(object sender, EventArgs e)
        {
            sum_checkedqty();
        }

    }
}
