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
    public partial class P10_Detail : Sci.Win.Subs.Input8A
    {
        int Type = 0;
        string RequestID;
        public P10_Detail(int type = 0, string requestID = "")
        {
            InitializeComponent();
            this.KeyField1 = "id";
            this.KeyField2 = "Issue_SummaryUkey";
            Type = type;
            RequestID = requestID;
        }

        protected override void OnSubDetailInsert(int index = -1)
        {
            var frm = new Sci.Production.Warehouse.P10_Detail_Detail(CurrentDetailData, (DataTable)gridbs.DataSource, this.Type);
            frm.P10_Detail = this;
            frm.ShowDialog(this);
            sum_checkedqty();
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
       , roll = Rtrim(Ltrim(t.roll))
       , dyelot = Rtrim(Ltrim(t.dyelot))
       , t.Qty
       , t.ID
       , t.Issue_SummaryUkey
       , t.FtyInventoryUkey
       , t.MDivisionID
       , t.StockType
       , t.ukey
	   , FTY.InQty
	   , FTY.OutQty
	   , FTY.AdjustQty
	   , FTY.InQty - FTY.OutQty + FTY.AdjustQty as balanceqty
	   , [location] = dbo.Getlocation(FTY.Ukey)
	   , GroupQty = Sum(FTY.InQty-FTY.OutQty+FTY.AdjustQty) over (partition by t.dyelot)
       , [DetailFIR] = concat(isnull(Physical.Result,' '),'/',isnull(Weight.Result,' '),'/',isnull(Shadebone.Result,' '),'/',isnull(Continuity.Result,' '),'/',isnull(Odor.Result,' '))
       , t.BarcodeNo
from #tmp t
Left join dbo.FtyInventory FTY WITH (NOLOCK) on t.FtyInventoryUkey=FTY.Ukey
left join dbo.Issue_Summary isum with (nolock) on t.Issue_SummaryUkey = isum.Ukey
outer apply (select fp.Result
            from dbo.FIR f with (nolock) 
	        inner join dbo.FIR_Physical fp with (nolock) on f.ID = fp.ID and fp.Roll = t.Roll and fp.Dyelot = t.Dyelot
	        where poid = t.poid and seq1 = t.seq1 and seq2 = t.seq2 and SCIRefno = isum.SCIRefno) Physical
outer apply (select fw.Result
            from dbo.FIR f with (nolock) 
	        inner join dbo.FIR_Weight fw with (nolock) on f.ID = fw.ID and fw.Roll = t.Roll and fw.Dyelot = t.Dyelot
	        where poid = t.poid and seq1 = t.seq1 and seq2 = t.seq2 and SCIRefno = isum.SCIRefno) Weight
outer apply (select fs.Result
            from dbo.FIR f with (nolock) 
	        inner join dbo.FIR_Shadebone fs with (nolock) on f.ID = fs.ID and fs.Roll = t.Roll and fs.Dyelot = t.Dyelot
	        where poid = t.poid and seq1 = t.seq1 and seq2 = t.seq2 and SCIRefno = isum.SCIRefno) Shadebone
outer apply (select fc.Result
            from dbo.FIR f with (nolock) 
	        inner join dbo.FIR_Continuity fc with (nolock) on f.ID = fc.ID and fc.Roll = t.Roll and fc.Dyelot = t.Dyelot
	        where poid = t.poid and seq1 = t.seq1 and seq2 = t.seq2 and SCIRefno = isum.SCIRefno) Continuity
outer apply (select fc.Result
            from dbo.FIR f with (nolock) 
	        inner join dbo.FIR_Odor fc with (nolock) on f.ID = fc.ID and fc.Roll = t.Roll and fc.Dyelot = t.Dyelot
	        where poid = t.poid and seq1 = t.seq1 and seq2 = t.seq2 and SCIRefno = isum.SCIRefno) Odor
order by GroupQty desc, t.dyelot, balanceqty desc", out dtFtyinventory, "#tmp")))
                {
                    MyUtility.Msg.WarningBox(result.ToString());
                    return;
                }
                gridbs.DataSource = dtFtyinventory;
            }

            this.displayID.Text = CurrentDetailData["id"].ToString();
            this.displaySCIRefno.Text = CurrentDetailData["SCIRefno"].ToString();
            this.displayRefno.Text = CurrentDetailData["refno"].ToString();
            this.displaySPNo.Text = CurrentDetailData["poid"].ToString();
            this.displayColorID.Text = CurrentDetailData["colorid"].ToString();
            //this.dis_sizespec.Text = CurrentDetailData["sizespec"].ToString();
            this.displayDesc.Text = CurrentDetailData["description"].ToString();
            this.numRequestQty.Text = CurrentDetailData["requestqty"].ToString();
            this.numAccuIssue.Text = CurrentDetailData["accu_issue"].ToString();
       
            string STRrequestqty = CurrentDetailData["requestqty"].ToString();
            string STRaccu_issue = CurrentDetailData["accu_issue"].ToString();
            decimal DECrequestqty;
            decimal DECaccu_issue;
            if (!decimal.TryParse(STRrequestqty, out DECrequestqty))
            { DECrequestqty = 0; }
            if (!decimal.TryParse(STRaccu_issue, out DECaccu_issue))
            { DECaccu_issue = 0; }
            this.numBalanceQty.Value = DECrequestqty - DECaccu_issue;
            this.numIssueQty.Text = CurrentDetailData["qty"].ToString();
            string STRqty = CurrentDetailData["qty"].ToString();
            decimal DECqty;
            if (!decimal.TryParse(STRqty, out DECqty))
            { DECqty = 0; }

            this.numVariance.Value = this.numBalanceQty.Value - this.numIssueQty.Value;
        }

        protected override bool OnGridSetup()
        {
            Ict.Win.DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings();
            ns.CellValidating += (s, e) =>
                {                
                    sum_checkedqty();
                };
            Helper.Controls.Grid.Generator(this.grid)
                //.Text("id", header: "id", width: Widths.AnsiChars(13), iseditingreadonly: true)  //0
                //.Numeric("Issue_SummaryUkey", header: "Issue_SummaryUkey", width: Widths.AnsiChars(8), integer_places: 10)    //6
            //.Text("FtyInventoryUkey", header: "FtyInventory" + Environment.NewLine + "Ukey", width: Widths.AnsiChars(8), iseditingreadonly: true)    //0
            .CellPOIDWithSeqRollDyelot("poid", header: "poid", width: Widths.AnsiChars(14), iseditingreadonly: true)  //1
            .Text("seq1", header: "seq1", width: Widths.AnsiChars(4), iseditingreadonly: true)  //2
            .Text("seq2", header: "seq2", width: Widths.AnsiChars(3), iseditingreadonly: true)  //3
            .Text("roll", header: "roll", width: Widths.AnsiChars(10), iseditingreadonly: true)  //4
            .Text("dyelot", header: "dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true)  //5
            .Numeric("qty", header: "Issue Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 8, settings: ns)    //6
            .Text("location", header: "Bulk" + Environment.NewLine + "Location", width: Widths.AnsiChars(10), iseditingreadonly: true)  //7
            .Numeric("inqty", header: "In Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 8, iseditingreadonly: true)    //8
            .Numeric("outqty", header: "Out Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 8, iseditingreadonly: true)    //9
            .Numeric("adjustqty", header: "Adjust" + Environment.NewLine + "Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 8, iseditingreadonly: true)    //10
            .Numeric("balanceqty", header: "Balance" + Environment.NewLine + "Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 8, iseditingreadonly: true)    //11
            .Text("DetailFIR", header: "Phy/Wei/Shade/Cont/Odor", width: Widths.AnsiChars(18), iseditingreadonly: true)  //13
            ;     //

            this.grid.Columns["qty"].DefaultCellStyle.BackColor = Color.Pink;

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
            IList<DataRow> issued;
            if (Type == 0)
            {
                issued = PublicPrg.Prgs.autopick(CurrentDetailData);
            }
            else
            {
                issued = PublicPrg.Prgs.AutoPickTape(CurrentDetailData, RequestID);
            }

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
            Object SumIssueQTY = subDT.Compute("Sum(qty)","");
            this.numIssueQty.Text = SumIssueQTY.ToString();
            this.numVariance.Value = this.numBalanceQty.Value - this.numIssueQty.Value;

        }

        private void delete_Click(object sender, EventArgs e)
        {
            sum_checkedqty();
        }

    }
}
