using Ict;
using Ict.Win;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
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
            DataTable dtSubDetail = (DataTable)this.gridbs.DataSource;

            if (!dtSubDetail.Columns.Contains("balanceqty"))
            {
                if (!dtSubDetail.Columns.Contains("SeqKey"))
                {
                    dtSubDetail.Columns.Add("SeqKey", typeof(int));
                }

                int seqKey = 0;
                foreach (DataRow dr in dtSubDetail.Rows)
                {
                    if (dr.RowState == DataRowState.Deleted)
                    {
                        continue;
                    }

                    dr["SeqKey"] = seqKey;
                    seqKey++;
                }

                DataTable dtFtyinventory;
                DualResult result;
                string cmdd = @"
SELECT r.ID, fi.POID, fi.SEQ1, fi.SEQ2, fi.Roll, fi.Dyelot, detail.Result, detail.Type, FL.nonCrocking, FL.nonHeat, FL.nonWash
  into #tmpDetial
  FROM #tmp t
  join dbo.FtyInventory fi WITH (NOLOCK) on t.FtyInventoryUkey=fi.Ukey
  left join Receiving_Detail rd WITH (NOLOCK) on rd.PoId = fi.POID and rd.Seq1 = fi.seq1 and rd.seq2 = fi.seq2 and rd.Roll = fi.Roll and rd.Dyelot = fi.Dyelot
  left join Receiving r WITH (NOLOCK) on r.id = rd.id
  join fir f on f.ReceivingID = r.id and f.POID= fi.POID	and f.SEQ1 =fi.Seq1 and f.SEQ2 = fi.Seq2 
  join FIR_Laboratory fl 
    on fl.ID = f.ID
OUTER APPLY (
Select Roll, Dyelot, Result = iif(DM.nonCrocking = 1, 'Pass', D.Result), Type = 'Crocking'
  from FIR_Laboratory DM
  join FIR_Laboratory_Crocking D
    on DM.ID = D.ID and fi.Roll = D.Roll and fi.Dyelot = D.Dyelot and DM.CrockingEncode = 1
 where DM.ID = fl.ID
union all
Select Roll, Dyelot, Result = iif(fl.nonheat = 1, 'Pass', D.Result), Type = 'Heat'
  from FIR_Laboratory DM
  join FIR_Laboratory_Heat D
    on DM.ID = D.ID and fi.Roll = D.Roll and fi.Dyelot = D.Dyelot and DM.HeatEncode = 1
 where DM.ID = fl.ID
union all
Select Roll, Dyelot, Result = iif(fl.nonwash = 1, 'Pass', D.Result), Type = 'Wash'
  from FIR_Laboratory DM
  join FIR_Laboratory_Wash D
    on DM.ID = D.ID and fi.Roll = D.Roll and fi.Dyelot = D.Dyelot and DM.WashEncode = 1
 where DM.ID = fl.ID
union all
Select top 1 Roll, Dyelot, D.Result, Type = 'Oven'
  from Oven_Detail D
  join Oven DM
    on DM.ID = d.ID
 where fl.POID = DM.POID and D.SEQ1 = fi.SEQ1 and D.SEQ2 = fi.SEQ2 and fi.Roll = D.Roll and fi.Dyelot = D.Dyelot and dm.Status = 'Confirmed'
order by isnull(DM.EditDate,DM.AddDate) desc,isnull(D.EditDate,D.AddDate) desc
union all
Select top 1 Roll, Dyelot, D.Result, Type = 'Color'
  from ColorFastness_Detail D
  join ColorFastness DM
    on DM.ID = d.ID
 where fl.POID = DM.POID and D.SEQ1 = fi.SEQ1 and D.SEQ2 = fi.SEQ2 and fi.Roll = D.Roll and fi.Dyelot = D.Dyelot and dm.Status = 'Confirmed'
order by isnull(DM.EditDate,DM.AddDate) desc,isnull(D.EditDate,D.AddDate) desc
) detail


select *
  into #tmp_WashLab 
  from (select distinct ID, POID, SEQ1, SEQ2, Roll, Dyelot, Type, Result, nonCrocking, nonHeat, nonWash
          from #tmpDetial
       ) Data
       PIVOT 
       (
        Count(Type) For Type in ([Crocking],[Heat],[Wash],[Oven],[Color])
       ) Data


select t.poid
       , t.Seq1
       , t.Seq2
       , fu.UnrollStatus
       , RelaxationStatus = dbo.GETRelaxationStatus(To_NewBarcode)
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
	   ,[WashLabReport] = case when FTY.POID is null Then 'Blank'
                               when WashLab_Blank.POID is not null Then 'Blank'
                               when WashLab_Sum.Cnt < 5 Then 'Blank'
                               when WashLab_Pass.Crocking > 0 and WashLab_Pass.Heat > 0 and WashLab_Pass.Wash > 0 and WashLab_Pass.Oven > 0 and WashLab_Pass.Color > 0 Then 'Pass'
                               else 'Fail' End
       , [Tone] = FTY.Tone
       , [GMTWash] = FTY.GMTWashStatus
       , t.SeqKey
from #tmp t
Left join dbo.FtyInventory FTY WITH (NOLOCK) on t.FtyInventoryUkey=FTY.Ukey
left join dbo.Issue_Summary isum with (nolock) on t.Issue_SummaryUkey = isum.Ukey
left join WHBarcodeTransaction w with (nolock) on w.TransactionID = t.ID
                                              and w.TransactionUkey = t.Ukey
                                              and w.Action = 'Confirm'
left join Fabric_UnrollandRelax fu with (nolock) on fu.Barcode = w.To_NewBarcode
left join Receiving_Detail rd WITH (NOLOCK) on rd.PoId = FTY.POID and rd.Seq1 = FTY.seq1 and rd.seq2 = FTY.seq2 and rd.Roll = FTY.Roll and rd.Dyelot = FTY.Dyelot
left join Receiving r WITH (NOLOCK) on r.id = rd.id
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
left join #tmp_WashLab WashLab_Blank on r.ID = WashLab_Blank.Id and WashLab_Blank.POID= FTY.POID and WashLab_Blank.SEQ1 = FTY.Seq1 and WashLab_Blank.SEQ2 = FTY.Seq2 and WashLab_Blank.Roll = FTY.Roll and WashLab_Blank.Dyelot = FTY.Dyelot and WashLab_Blank.Result is null
left join #tmp_WashLab WashLab_Pass on r.ID = WashLab_Pass.Id and WashLab_Pass.POID= FTY.POID and WashLab_Pass.SEQ1 = FTY.Seq1 and WashLab_Pass.SEQ2 = FTY.Seq2 and WashLab_Pass.Roll = FTY.Roll and WashLab_Pass.Dyelot = FTY.Dyelot and WashLab_Pass.Result = 'Pass'
outer apply (select Cnt = sum(Crocking + Heat + Wash + Oven + Color) from #tmp_WashLab where ID = r.ID and POID = FTY.POID and SEQ1 = FTY.SEQ1 and SEQ2 = FTY.SEQ2 and Roll = FTY.Roll and Dyelot = FTY.Dyelot) WashLab_Sum
order by GroupQty desc, t.dyelot, balanceqty desc
";
                if (!(result = MyUtility.Tool.ProcessWithDatatable(dtSubDetail, string.Empty, cmdd, out dtFtyinventory, "#tmp")))
                {
                    MyUtility.Msg.WarningBox(result.ToString());
                    return;
                }

                foreach (DataColumn colFtyinventory in dtFtyinventory.Columns)
                {
                    if (!dtSubDetail.Columns.Contains(colFtyinventory.ColumnName))
                    {
                        dtSubDetail.Columns.Add(colFtyinventory.ColumnName, colFtyinventory.DataType);
                    }
                }

                foreach (DataRow dr in dtFtyinventory.Rows)
                {
                    DataRow[] drOri = dtSubDetail.Select($"SeqKey = {dr["SeqKey"]}");
                    if (drOri.Length > 0)
                    {
                        drOri[0]["UnrollStatus"] = dr["UnrollStatus"];
                        drOri[0]["RelaxationStatus"] = dr["RelaxationStatus"];
                        drOri[0]["InQty"] = dr["InQty"];
                        drOri[0]["OutQty"] = dr["OutQty"];
                        drOri[0]["AdjustQty"] = dr["AdjustQty"];
                        drOri[0]["ReturnQty"] = dr["ReturnQty"];
                        drOri[0]["balanceqty"] = dr["balanceqty"];
                        drOri[0]["location"] = dr["location"];
                        drOri[0]["ContainerCode"] = dr["ContainerCode"];
                        drOri[0]["GroupQty"] = dr["GroupQty"];
                        drOri[0]["DetailFIR"] = dr["DetailFIR"];
                        drOri[0]["WashLabReport"] = dr["WashLabReport"];
                        drOri[0]["Tone"] = dr["Tone"];
                        drOri[0]["GMTWash"] = dr["GMTWash"];
                    }
                }
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
            .Text("WashLabReport", header: "WashLab Report", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("Tone", header: "Shade Band" + Environment.NewLine + "Tone/Grp", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("UnrollStatus", header: "Unroll Status", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("RelaxationStatus", header: "Relaxation Status", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("GMTWash", header: "GMT Wash", width: Widths.AnsiChars(10), iseditingreadonly: true)
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
