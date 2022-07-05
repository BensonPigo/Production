﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using Ict.Win;
using Ict;
using System.Linq;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P10_Detail : Win.Subs.Input8A
    {
        private int Type = 0;
        private string RequestID;

        /// <inheritdoc/>
        public P10_Detail(int type = 0, string requestID = "")
        {
            this.InitializeComponent();
            this.KeyField1 = "id";
            this.KeyField2 = "Issue_SummaryUkey";
            this.Type = type;
            this.RequestID = requestID;
            if (type == 1)
            {
                this.Text = "P62. Issue Fabric Detail";
            }
        }

        /// <inheritdoc/>
        protected override void OnSubDetailInsert(int index = -1)
        {
            var frm = new P10_Detail_Detail(this.CurrentDetailData, (DataTable)this.gridbs.DataSource, this.Type);
            frm.P10_Detail = this;
            frm.ShowDialog(this);
            this.Sum_checkedqty();
        }

        /// <inheritdoc/>
        protected override void OnAttached()
        {
            base.OnAttached();
            DataTable temp = (DataTable)this.gridbs.DataSource;
            if (!temp.Columns.Contains("balanceqty"))
            {
                DataTable dtFtyinventory;
                DualResult result;
                string cmdd = @"          
select t.poid
       , t.Seq1
       , t.Seq2
       , t.UnrollStatus
       , RelaxationStatus = dbo.GETRelaxationStatus(t.Ukey)
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
       , FTY.ReturnQty
	   , FTY.InQty - FTY.OutQty + FTY.AdjustQty - FTY.ReturnQty as balanceqty
	   , [location] = dbo.Getlocation(FTY.Ukey)
       , [ContainerCode] = FTY.ContainerCode
	   , GroupQty = Sum(FTY.InQty - FTY.OutQty + FTY.AdjustQty - FTY.ReturnQty) over (partition by t.dyelot)
       , [DetailFIR] = concat(isnull(Physical.Result,' '),'/',isnull(Weight.Result,' '),'/',isnull(Shadebone.Result,' '),'/',isnull(Continuity.Result,' '),'/',isnull(Odor.Result,' '))
       , [Tone] = ShadeboneTone.Tone
from #tmp t
Left join dbo.FtyInventory FTY WITH (NOLOCK) on t.FtyInventoryUkey=FTY.Ukey
left join dbo.Issue_Summary isum with (nolock) on t.Issue_SummaryUkey = isum.Ukey
outer apply (select  TOP 1 fp.Result
            from dbo.FIR f with (nolock) 
	        inner join dbo.FIR_Physical fp with (nolock) on f.ID = fp.ID and fp.Roll = t.Roll and fp.Dyelot = t.Dyelot
	        where poid = t.poid and seq1 = t.seq1 and seq2 = t.seq2 and SCIRefno = isum.SCIRefno
			order by ISNULL(fp.EditDate,fp.AddDate) DESC ) Physical
outer apply (select TOP 1 fw.Result
            from dbo.FIR f with (nolock) 
	        inner join dbo.FIR_Weight fw with (nolock) on f.ID = fw.ID and fw.Roll = t.Roll and fw.Dyelot = t.Dyelot
	        where poid = t.poid and seq1 = t.seq1 and seq2 = t.seq2 and SCIRefno = isum.SCIRefno
			order by ISNULL(fw.EditDate,fw.AddDate) DESC ) Weight
outer apply (select TOP 1 fs.Result
            from dbo.FIR f with (nolock) 
	        inner join dbo.FIR_Shadebone fs with (nolock) on f.ID = fs.ID and fs.Roll = t.Roll and fs.Dyelot = t.Dyelot
	        where poid = t.poid and seq1 = t.seq1 and seq2 = t.seq2 and SCIRefno = isum.SCIRefno
			order by ISNULL(fs.EditDate,fs.AddDate) DESC 
			) Shadebone
outer apply (select  TOP 1 fc.Result
            from dbo.FIR f with (nolock) 
	        inner join dbo.FIR_Continuity fc with (nolock) on f.ID = fc.ID and fc.Roll = t.Roll and fc.Dyelot = t.Dyelot
	        where poid = t.poid and seq1 = t.seq1 and seq2 = t.seq2 and SCIRefno = isum.SCIRefno
			order by ISNULL(fc.EditDate,fc.AddDate) DESC ) Continuity
outer apply (select  TOP 1 fc.Result
            from dbo.FIR f with (nolock) 
	        inner join dbo.FIR_Odor fc with (nolock) on f.ID = fc.ID and fc.Roll = t.Roll and fc.Dyelot = t.Dyelot
	        where poid = t.poid and seq1 = t.seq1 and seq2 = t.seq2 and SCIRefno = isum.SCIRefno
			order by ISNULL(fc.EditDate,fc.AddDate) DESC ) Odor
outer apply (select [Tone] = MAX(fs.Tone)
            from FtyInventory fi with (nolock) 
            Left join FIR f with (nolock) on f.poid = fi.poid and f.seq1 = fi.seq1 and f.seq2 = fi.seq2
	        Left join FIR_Shadebone fs with (nolock) on f.ID = fs.ID and fs.Roll = fi.Roll and fs.Dyelot = fi.Dyelot
	        where fi.Ukey = FTY.Ukey
			) ShadeboneTone
order by GroupQty desc, t.dyelot, balanceqty desc";
                if (!(result = MyUtility.Tool.ProcessWithDatatable(
                        temp, string.Empty, cmdd, out dtFtyinventory, "#tmp")))
                {
                    MyUtility.Msg.WarningBox(result.ToString());
                    return;
                }

                this.gridbs.DataSource = dtFtyinventory;
            }

            this.displayID.Text = this.CurrentDetailData["id"].ToString();
            this.displaySCIRefno.Text = this.CurrentDetailData["SCIRefno"].ToString();
            this.displayRefno.Text = this.CurrentDetailData["refno"].ToString();
            this.displaySPNo.Text = this.CurrentDetailData["poid"].ToString();
            this.displayColorID.Text = this.CurrentDetailData["colorid"].ToString();

            // this.dis_sizespec.Text = CurrentDetailData["sizespec"].ToString();
            this.displayDesc.Text = this.CurrentDetailData["description"].ToString();
            this.numRequestQty.Text = this.CurrentDetailData["requestqty"].ToString();
            this.numAccuIssue.Text = this.CurrentDetailData["accu_issue"].ToString();

            string sTRrequestqty = this.CurrentDetailData["requestqty"].ToString();
            string sTRaccu_issue = this.CurrentDetailData["accu_issue"].ToString();
            decimal dECrequestqty;
            decimal dECaccu_issue;
            if (!decimal.TryParse(sTRrequestqty, out dECrequestqty))
            {
                dECrequestqty = 0;
            }

            if (!decimal.TryParse(sTRaccu_issue, out dECaccu_issue))
            {
                dECaccu_issue = 0;
            }

            this.numBalanceQty.Value = dECrequestqty - dECaccu_issue;
            this.numIssueQty.Text = this.CurrentDetailData["qty"].ToString();
            string sTRqty = this.CurrentDetailData["qty"].ToString();
            decimal dECqty;
            if (!decimal.TryParse(sTRqty, out dECqty))
            {
                dECqty = 0;
            }

            this.numVariance.Value = this.numBalanceQty.Value - this.numIssueQty.Value;
        }

        /// <inheritdoc/>
        protected override bool OnGridSetup()
        {
            DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings();
            ns.CellValidating += (s, e) =>
                {
                    this.Sum_checkedqty();
                };

            Ict.Win.UI.DataGridViewTextBoxColumn cbb_ContainerCode;
            this.Helper.Controls.Grid.Generator(this.grid)
            .CellPOIDWithSeqRollDyelot("poid", header: "poid", width: Widths.AnsiChars(14), iseditingreadonly: true) // 1
            .Text("seq1", header: "seq1", width: Widths.AnsiChars(4), iseditingreadonly: true) // 2
            .Text("seq2", header: "seq2", width: Widths.AnsiChars(3), iseditingreadonly: true) // 3
            .Text("roll", header: "roll", width: Widths.AnsiChars(10), iseditingreadonly: true) // 4
            .Text("dyelot", header: "dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true) // 5
            .Numeric("qty", header: "Issue Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 8, settings: ns) // 6
            .Text("location", header: "Bulk" + Environment.NewLine + "Location", width: Widths.AnsiChars(10), iseditingreadonly: true) // 7
            .Text("ContainerCode", header: "Container Code", iseditingreadonly: true).Get(out cbb_ContainerCode)
            .Numeric("inqty", header: "In Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 8, iseditingreadonly: true) // 8
            .Numeric("outqty", header: "Out Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 8, iseditingreadonly: true) // 9
            .Numeric("adjustqty", header: "Adjust" + Environment.NewLine + "Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 8, iseditingreadonly: true) // 10
            .Numeric("ReturnQty", header: "Return" + Environment.NewLine + "Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 8, iseditingreadonly: true) // 10
            .Numeric("balanceqty", header: "Balance" + Environment.NewLine + "Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 8, iseditingreadonly: true) // 11
            .Text("DetailFIR", header: "Phy/Wei/Shade/Cont/Odor", width: Widths.AnsiChars(18), iseditingreadonly: true) // 13
            .Text("Tone", header: "Shade Band" + Environment.NewLine + "Tone/Grp", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("UnrollStatus", header: "Unroll Status", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("RelaxationStatus", header: "Relaxation Status", width: Widths.AnsiChars(8), iseditingreadonly: true)
            ;

            // 僅有自動化工廠 ( System.Automation = 1 )才需要顯示該欄位 by ISP20220035
            cbb_ContainerCode.Visible = Automation.UtilityAutomation.IsAutomationEnable;
            this.grid.Columns["qty"].DefaultCellStyle.BackColor = Color.Pink;

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

            var dr2_ba = ((DataTable)this.gridbs.DataSource).Select("qty > balanceqty");
            if (dr2_ba.Length > 0)
            {
                MyUtility.Msg.WarningBox("Issue Qty of selected row can't be more then balance qty!", "Warning");
                return false;
            }

            return base.OnSaveBefore();
        }

        private void BtnAutoPick_Click(object sender, EventArgs e)
        {
            IList<DataRow> issued;
            if (this.Type == 0)
            {
                issued = PublicPrg.Prgs.Autopick(this.CurrentDetailData);
            }
            else
            {
                issued = PublicPrg.Prgs.AutoPickTape(this.CurrentDetailData, this.RequestID);
            }

            if (issued == null)
            {
                return;
            }

            DataTable subDT = (DataTable)this.gridbs.DataSource;

            foreach (DataRow temp in subDT.ToList())
            {
                temp.Delete();
            }

            // subDT.Clear();
            foreach (DataRow dr2 in issued)
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
            object sumIssueQTY = subDT.Compute("Sum(qty)", string.Empty);
            this.numIssueQty.Text = sumIssueQTY.ToString();
            this.numVariance.Value = this.numBalanceQty.Value - this.numIssueQty.Value;
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            this.Sum_checkedqty();
        }
    }
}
