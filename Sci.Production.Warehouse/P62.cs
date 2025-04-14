using Ict;
using Ict.Win;
using Sci;
using Sci.Data;
using Sci.Production.Automation;
using Sci.Production.Automation.LogicLayer;
using Sci.Production.Prg.Entity;
using Sci.Production.PublicPrg;
using Sci.Win;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P62 : Win.Tems.Input8
    {
        /// <inheritdoc/>
        public P62(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = string.Format("Type='I' and MDivisionID = '{0}'", Env.User.Keyword);
        }

        /// <inheritdoc/>
        public P62(ToolStripMenuItem menuitem, string transID)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = $"Type='I' and id='{transID}'and MDivisionID = '{Env.User.Keyword}'";
            this.IsSupportNew = false;
            this.IsSupportEdit = false;
            this.IsSupportDelete = false;
            this.IsSupportConfirm = false;
            this.IsSupportUnconfirm = false;
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            Color backDefaultColor = this.detailgrid.DefaultCellStyle.BackColor;
            #region qty 開窗
            DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings();
            ns.CellMouseDoubleClick += (s, e) =>
            {
                this.DoSubForm = new P10_Detail(1, MyUtility.Convert.GetString(this.CurrentMaintain["CutplanID"]));
                this.DoSubForm.IsSupportUpdate = false;
                this.OpenSubDetailPage();
            };
            #endregion

            // 使用虛擬欄位顯示 "bal_qty"及"var_qty"
            this.detailgrid.VirtualMode = true;
            this.detailgrid.CellValueNeeded += (s, e) =>
            {
                if (this.detailgrid.Rows[e.RowIndex] == null)
                {
                    return;
                }

                string sTRrequestqty = this.detailgrid.Rows[e.RowIndex].Cells["requestqty"].Value.ToString();
                string sTRaccu_issue = this.detailgrid.Rows[e.RowIndex].Cells["accu_issue"].Value.ToString();
                string sTRqty = this.detailgrid.Rows[e.RowIndex].Cells["qty"].Value.ToString();
                decimal dECrequestqty;
                decimal dECaccu_issue;
                decimal dECqty;
                if (!decimal.TryParse(sTRrequestqty, out dECrequestqty))
                {
                    dECrequestqty = 0;
                }

                if (!decimal.TryParse(sTRaccu_issue, out dECaccu_issue))
                {
                    dECaccu_issue = 0;
                }

                if (!decimal.TryParse(sTRqty, out dECqty))
                {
                    dECqty = 0;
                }

                if (e.ColumnIndex == this.detailgrid.Columns["bal_qty"].Index && !MyUtility.Check.Empty(this.detailgrid.Rows[e.RowIndex].Cells["requestqty"].Value))
                {
                    e.Value = dECrequestqty - dECaccu_issue;
                }

                if (e.ColumnIndex == this.detailgrid.Columns["var_qty"].Index && !MyUtility.Check.Empty(this.detailgrid.Rows[e.RowIndex].Cells["requestqty"].Value))
                {
                    e.Value = dECrequestqty - dECaccu_issue - dECqty;
                }
            };

            #region 欄位設定
            this.Helper.Controls.Grid.Generator(this.detailgrid)
            .CellPOIDWithSeqRollDyelot("POID", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("Seq1", header: "Seq1", width: Widths.AnsiChars(3), iseditingreadonly: true)
            .Text("Seq2", header: "Seq2", width: Widths.AnsiChars(3), iseditingreadonly: true)
            .EditText("Description", header: "Description", width: Widths.AnsiChars(40), iseditingreadonly: true)
            .Numeric("requestqty", header: "Request", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true)
            .Numeric("accu_issue", header: "Accu. Issued", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true)
            .Numeric(string.Empty, name: "bal_qty", header: "Bal. Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true)
            .Numeric("qty", header: "Issue Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, settings: ns, iseditingreadonly: true)
            .Text("FinalFIR", header: "Final FIR", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Numeric(string.Empty, name: "var_qty", header: "Var Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true)
            .Numeric("arqty", header: "Accu Req. Qty by Material", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true)
            .Numeric("aiqqty", header: "Accu Issue Qty by Material", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true)
            .Numeric("avqty", header: "Accu Var by Material", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true)
            .Text("unit", header: "unit", width: Widths.AnsiChars(4), iseditingreadonly: true)
            .Numeric("netqty", header: "Net Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true)
            ;
            #endregion 欄位設定

            #region Grid 變色規則
            this.detailgrid.RowPrePaint += (s, e) =>
            {
                if (e.RowIndex < 0)
                {
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);

                #region 變色規則，若該 Row 已經變色則跳過
                decimal qty = MyUtility.Convert.GetDecimal(dr["qty"]);
                decimal accu_issue = MyUtility.Convert.GetDecimal(dr["accu_issue"]);
                decimal netqty = MyUtility.Convert.GetDecimal(dr["netqty"]);

                if (qty - accu_issue > netqty)
                {
                    if (this.detailgrid.Rows[e.RowIndex].DefaultCellStyle.BackColor != Color.FromArgb(255, 128, 192))
                    {
                        this.detailgrid.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(255, 128, 192);
                    }
                }
                else
                {
                    if (this.detailgrid.Rows[e.RowIndex].DefaultCellStyle.BackColor != backDefaultColor)
                    {
                        this.detailgrid.Rows[e.RowIndex].DefaultCellStyle.BackColor = backDefaultColor;
                    }
                }
                #endregion
            };
            #endregion
        }

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["ID"].ToString();
            string cutplanID = (e.Master == null) ? string.Empty : e.Master["cutplanID"].ToString();
            this.DetailSelectCommand = $@"
select  s.*
    , f.Refno
	, [description] = f.DescDetail + iif(isnull(Net.DescDetail,'')='','', CHAR(13) + CHAR(10) + CHAR(13) + CHAR(10) + Net.DescDetail)
	, [requestqty] = isnull(ec.RequestQty, 0.00)
	, [accu_issue] =isnull (accu.accu_issue, 0.00)
    , unit = (select top 1 StockUnit from Po_Supp_Detail psd where psd.Id = s.Poid and psd.SciRefno = s.SciRefno)
	, NetQty = isnull( Net.NETQty, 0)
    , [FinalFIR] = (
		SELECT Stuff((
			select concat( '/',isnull(Result,' '))   
			from dbo.FIR f with (nolock) 
			where f.poid = s.poid and f.SCIRefno = s.SCIRefno
			and exists(select 1 from Issue_Detail with (nolock) where Issue_SummaryUkey = s.Ukey and f.seq1 = seq1 and f.seq2 = seq2)
			FOR XML PATH('')
		),1,1,'') )
	, arqty = ec.RequestQty + AccuReq.ReqQty
	, aiqqty = AccuIssue.aiqqty
	, avqty = (ec.RequestQty + AccuReq.ReqQty) - AccuIssue.aiqqty
from dbo.Issue_Summary s WITH (NOLOCK) 
left join Fabric f on s.SciRefno = f.SciRefno
outer apply(
	select RequestQty = sum(c.ReleaseQty) 
	from dbo.CutTapePlan_Detail c WITH (NOLOCK) 
	where c.id = '{cutplanID}'
	and c.Seq1 = s.Seq1
	and c.Seq2 = s.Seq2
) ec
outer apply(
	select accu_issue = isNull(sum(qty) , 0.00)
	from Issue a WITH (NOLOCK) 
	inner join Issue_Summary b WITH (NOLOCK) on a.Id=b.Id 
	where b.poid = s.poid 
			and a.CutplanID = '{cutplanID}' 
			and b.SCIRefno = s.SCIRefno 
			and b.Colorid = s.Colorid 
			and a.status = 'Confirmed'
			and a.id != s.id
			and a.Type = 'I'
) accu
outer apply(
	select top 1 psd.NETQty,DescDetail = psdsS.SpecValue + psd.Special + '=' + convert(varchar,psd.Qty)
	from PO_Supp_Detail psd
    inner join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
    inner join PO_Supp_Detail_Spec psdsS WITH (NOLOCK) on psdsS.ID = psd.id and psdsS.seq1 = psd.seq1 and psdsS.seq2 = psd.seq2 and psdsS.SpecColumnID = 'Size'
	where psd.ID = s.POID and psd.SCIRefno = s.SCIRefno and psdsC.SpecValue = s.ColorID
			and psd.SEQ1 like 'A%' and psd.SEQ1 = s.Seq1 and psd.Seq2 = s.Seq2  and psd.NETQty <> 0
)Net
outer apply(select CuttingID from CutTapePlan where Id = '{cutplanID}' )c
outer apply(
	select ReqQty = isNull(sum(t2.ReleaseQty), 0)
	from CutTapePlan t1
	inner join CutTapePlan_Detail t2 on t1.id = t2.id
	inner join Issue i on i.CutplanID = t1.ID
	where t1.CuttingID = c.CuttingID
	and t1.id <> '{cutplanID}'
	and i.Status = 'Confirmed'
	and i.Type = 'I'
)AccuReq
outer apply(
	select aiqqty = isnull(sum(sm.Qty), 0)
    from Issue_Summary sm WITH (NOLOCK) 
    inner join Issue i WITH (NOLOCK) on sm.Id = i.Id 
    where sm.Poid = s.poid 
            and sm.SCIRefno = s.SCIRefno 
            and sm.Colorid = s.ColorID 
            and i.status = 'Confirmed'
			and i.Type = 'I'
)AccuIssue

Where s.id = '{masterID}'
";

            return base.OnDetailSelectCommandPrepare(e);
        }

        private void TxtRequest_Validating(object sender, CancelEventArgs e)
        {
            if (this.txtRequest.Text == this.txtRequest.OldValue)
            {
                return;
            }

            DataTable dt = (DataTable)this.detailgridbs.DataSource;
            for (int i = dt.Rows.Count - 1; i >= 0; i--)
            {
                dt.Rows[i].Delete();
            }

            if (MyUtility.Check.Empty(this.txtRequest.Text))
            {
                return;
            }

            string sqlcmd = $@"select id from dbo.CutTapePlan WITH (NOLOCK) where id= @Request and mdivisionid = '{Env.User.Keyword}'";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@Request", this.txtRequest.Text));
            parameters.Add(new SqlParameter("@ID", MyUtility.Convert.GetString(this.CurrentMaintain["ID"])));
            if (!MyUtility.Check.Seek(sqlcmd, parameters, null))
            {
                e.Cancel = true;
                MyUtility.Msg.WarningBox("Request not existe");
                return;
            }

            sqlcmd = $@"
declare @IssueID varchar(13) = @ID
, @CutTapePlanID varchar(13) = @Request

select s.*
	, ec.RequestQty
	, Qty = 0.00
	, accu.accu_issue
    , id = @IssueID
	, NetQty = isnull( Net.NETQty, 0)
	, description = (select DescDetail 
                              from fabric WITH (NOLOCK) 
                              where scirefno = s.scirefno) + iif(isnull(Net.DescDetail,'')='','', CHAR(13) + CHAR(10) + CHAR(13) + CHAR(10) + Net.DescDetail)
	, arqty = ec.RequestQty + AccuReq.ReqQty
	, aiqqty = AccuIssue.aiqqty
	, avqty = (ec.RequestQty + AccuReq.ReqQty) - AccuIssue.aiqqty
from (
	select distinct o.poid, bof.SCIRefno, t.ColorID, t.Refno , c.CuttingID, t.Seq1, t.Seq2
	from dbo.CutTapePlan_Detail t WITH (NOLOCK) 
	left join dbo.CutTapePlan c WITH (NOLOCK) on c.id = t.id
	left join Orders o  WITH (NOLOCK) on o.id = c.CuttingID
	left join Order_EachCons e  WITH (NOLOCK) on e.Ukey = t.Order_EachConsUkey
	left join Order_BOF bof WITH (NOLOCK) on bof.Id = e.Id and bof.FabricCode = e.FabricCode
	where t.id = @CutTapePlanID
) s
outer apply(
	select RequestQty = sum(c.ReleaseQty) 
	from dbo.CutTapePlan_Detail c WITH (NOLOCK) 
	where  c.id = @CutTapePlanID
	and c.Seq1 = s.Seq1
	and c.Seq2 = s.Seq2
) ec
outer apply(
	select ReqQty = isNull(sum(t2.ReleaseQty), 0)
	from CutTapePlan t1
	inner join CutTapePlan_Detail t2 on t1.id = t2.id
	inner join Issue i on i.CutplanID = t1.ID
	where t1.CuttingID = s.CuttingID
	and t1.id <> @CutTapePlanID
	and i.Status = 'Confirmed'
	and i.Type = 'I'
)AccuReq
outer apply(
	select accu_issue = isNull(sum(qty) , 0.00)
	from Issue a WITH (NOLOCK) 
    inner join Issue_Summary b WITH (NOLOCK) on a.Id=b.Id 
    where b.poid = s.poid 
            and a.CutplanID = @CutTapePlanID 
            and b.SCIRefno = s.SCIRefno 
            and b.Colorid = s.Colorid 
            and a.status = 'Confirmed'
            and a.id != @IssueID 
			and a.Type = 'I'
) accu
outer apply(
	select top 1 psd.NETQty,DescDetail = psdsS.SpecValue + psd.Special + '=' + convert(varchar,psd.Qty)
	from PO_Supp_Detail psd
    inner join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
    inner join PO_Supp_Detail_Spec psdsS WITH (NOLOCK) on psdsS.ID = psd.id and psdsS.seq1 = psd.seq1 and psdsS.seq2 = psd.seq2 and psdsS.SpecColumnID = 'Size'
	where psd.ID = s.POID and psd.SCIRefno = s.SCIRefno and psdsC.SpecValue = s.ColorID
			and psd.SEQ1 like 'A%' and psd.SEQ1 = s.Seq1 and psd.Seq2 = s.Seq2  and psd.NETQty <> 0
)Net
outer apply(
	select aiqqty = isnull(sum(sm.Qty), 0)
    from Issue_Summary sm WITH (NOLOCK) 
    inner join Issue i WITH (NOLOCK) on sm.Id = i.Id 
    where sm.Poid = s.poid 
            and sm.SCIRefno = s.SCIRefno 
            and sm.Colorid = s.ColorID 
            and i.status = 'Confirmed'
			and i.Type = 'I'
)AccuIssue
";
            DualResult result = DBProxy.Current.Select(null, sqlcmd, parameters, out dt);
            if (!result)
            {
                this.ShowErr(result);
                e.Cancel = true;
                return;
            }

            if (dt.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("CutTapePlan data not found!!");
                e.Cancel = true;
                return;
            }

            this.CurrentMaintain["cutplanid"] = this.txtRequest.Text;
            this.CurrentMaintain.EndEdit();

            foreach (DataRow item in dt.Rows)
            {
                item.AcceptChanges();
                item.SetAdded();
                ((DataTable)this.detailgridbs.DataSource).ImportRow(item);
            }
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["MDivisionID"] = Env.User.Keyword;
            this.CurrentMaintain["FactoryID"] = Env.User.Factory;
            this.CurrentMaintain["Status"] = "New";
            this.CurrentMaintain["Type"] = "I";
            this.CurrentMaintain["issuedate"] = DateTime.Now;
        }

        /// <inheritdoc/>
        protected override bool ClickDeleteBefore()
        {
            if (this.CurrentMaintain["Status"].EqualString("CONFIRMED"))
            {
                MyUtility.Msg.WarningBox("Data is confirmed, can't delete.");
                return false;
            }

            return base.ClickDeleteBefore();
        }

        /// <inheritdoc/>
        protected override bool ClickEditBefore()
        {
            if (this.CurrentMaintain["Status"].EqualString("CONFIRMED"))
            {
                MyUtility.Msg.WarningBox("Data is confirmed, can't modify.");
                return false;
            }

            return base.ClickEditBefore();
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            #region 表頭必輸檢查
            this.GetSubDetailDatas(out DataTable dtaa);
            if (MyUtility.Check.Empty(this.CurrentMaintain["cutplanId"]))
            {
                MyUtility.Msg.WarningBox("< Request# >  can't be empty!", "Warning");
                this.txtRequest.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["IssueDate"]))
            {
                MyUtility.Msg.WarningBox("< Issue Date >  can't be empty!", "Warning");
                this.dateIssueDate.Focus();
                return false;
            }

            #endregion

            // 將Issue_Detail的數量更新Issue_Summary
            DataTable subDetail;
            foreach (DataRow detailRow in this.DetailDatas)
            {
                this.GetSubDetailDatas(detailRow, out subDetail);
                decimal detailQty = 0;
                if (subDetail.Rows.Count > 0)
                {
                    detailQty = subDetail.AsEnumerable().Sum(s => s.RowState != DataRowState.Deleted ? (decimal)s["Qty"] : 0);
                }

                if (MyUtility.Convert.GetDecimal(detailRow["qty"]) != detailQty)
                {
                    MyUtility.Msg.WarningBox($"<SP#>{detailRow["POID"]}, <Seq>{detailRow["Seq1"]} {detailRow["Seq2"]} Issue Qty({detailRow["qty"]}) does not match the details' total({detailQty}). Please double click <Issue Qty> check detail again. ");
                    return false;
                }
            }

            #region 表身檢查
            if (this.DetailDatas.Count == 0)
            {
                MyUtility.Msg.WarningBox("Detail can't be empty", "Warning");
                return false;
            }

            double sum = 0.00;
            foreach (DataRow dr in this.DetailDatas)
            {
                sum += Convert.ToDouble(dr["qty"]);
            }

            if (sum == 0)
            {
                MyUtility.Msg.WarningBox("All Issue_Qty are zero", "Warning");
                return false;
            }
            #endregion

            // 取單號
            if (this.IsDetailInserting)
            {
                string tmpId = MyUtility.GetValue.GetID(Env.User.Keyword + "IT", "Issue", (DateTime)this.CurrentMaintain["IssueDate"]);
                if (MyUtility.Check.Empty(tmpId))
                {
                    MyUtility.Msg.WarningBox("Get document ID fail!!");
                    return false;
                }

                this.CurrentMaintain["id"] = tmpId;
            }

            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override DualResult ConvertSubDetailDatasFromDoSubForm(SubDetailConvertFromEventArgs e)
        {
            Sum_subDetail(e.Detail, e.SubDetails);

            return base.ConvertSubDetailDatasFromDoSubForm(e);
        }

        private static void Sum_subDetail(DataRow target, DataTable source)
        {
            target["aiqqty"] = (decimal)target["aiqqty"] - (decimal)target["qty"];
            target["qty"] = (source.Rows.Count == 0) ? 0m : source.AsEnumerable().Where(r => r.RowState != DataRowState.Deleted)
                .Sum(r => r.Field<decimal>("qty"));
            target["aiqqty"] = (decimal)target["aiqqty"] + (decimal)target["qty"];
            target["avqty"] = (decimal)target["arqty"] - (decimal)target["aiqqty"];
        }

        private void BtnAutoPick_Click(object sender, EventArgs e)
        {
            DataTable subDT;
            foreach (DataRow dr in this.DetailDatas)
            {
                var issued = Prgs.AutoPickTape(dr, MyUtility.Convert.GetString(this.CurrentMaintain["cutplanID"]));
                if (issued == null)
                {
                    break;
                }

                if (this.GetSubDetailDatas(dr, out subDT))
                {
                    foreach (DataRow temp in subDT.ToList())
                    {
                        temp.Delete();
                    }

                    foreach (DataRow dr2 in issued)
                    {
                        dr2.AcceptChanges();
                        dr2.SetAdded();
                        subDT.ImportRow(dr2);
                    }

                    Sum_subDetail(dr, subDT);
                }
            }

            // 強制觸發CellValueNeeded，否則游標在Issue QTY上可能會無法觸發。
            this.detailgrid.SelectRowToNext();
            this.detailgrid.SelectRowToPrev();
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();

            // System.Automation=1 和confirmed 且 有P99 Use 權限的人才可以看到此按紐
            if (UtilityAutomation.IsAutomationEnable && (this.CurrentMaintain["Status"].ToString().ToUpper() == "CONFIRMED") &&
                MyUtility.Check.Seek($@"
select * from Pass1
where (FKPass0 in (select distinct FKPass0 from Pass2 where BarPrompt = 'P99. Send to WMS command Status' and Used = 'Y') or IsMIS = 1 or IsAdmin = 1)
and ID = '{Sci.Env.User.UserID}'"))
            {
                this.btnCallP99.Visible = true;
            }
            else
            {
                this.btnCallP99.Visible = false;
            }
        }

        /// <inheritdoc/>
        protected override void ClickConfirm()
        {
            this.RenewData(); // 先重載資料, 避免雙開程式狀況
            base.ClickConfirm();
            if (this.CurrentMaintain == null)
            {
                return;
            }

            // 第3層才是 issue_detail
            DualResult result = DBProxy.Current.Select(null, $"select * from issue_detail WITH (NOLOCK) where id = '{this.CurrentMaintain["ID"]}'", out DataTable dtIssue_Detail);
            if (!result)
            {
                this.ShowErr(result);
            }

            #region Check Issue_Detail isn't Empty
            if (MyUtility.Convert.GetDecimal(dtIssue_Detail.Compute("sum(Qty)", string.Empty)) == 0)
            {
                MyUtility.Msg.WarningBox("All Issue_Qty are zero", "Warning");
                return;
            }
            #endregion

            // 取得 FtyInventory 資料 (包含PO_Supp_Detail.FabricType)
            result = Prgs.GetFtyInventoryData(dtIssue_Detail, this.Name, out DataTable dtOriFtyInventory);
            string ids = string.Empty;
            DataTable datacheck;

            // 檢查 Barcode不可為空
            if (!Prgs.CheckBarCode(dtOriFtyInventory, this.Name))
            {
                return;
            }

            #region 檢查物料Location 是否存在WMS
            if (!PublicPrg.Prgs.Chk_WMS_Location(this.CurrentMaintain["ID"].ToString(), this.Name))
            {
                return;
            }
            #endregion

            #region 檢查庫存項lock
            string sqlcmd = string.Format(
                @"
Select d.poid,d.seq1,d.seq2,d.Roll,d.Qty
    ,isnull(f.InQty,0) - isnull(f.OutQty,0) + isnull(f.AdjustQty,0) - isnull(f.ReturnQty,0) as balanceQty
    ,d.Dyelot
from dbo.Issue_Detail d WITH (NOLOCK) inner join FtyInventory f WITH (NOLOCK) 
on d.POID = f.POID  AND D.StockType = F.StockType
and d.Roll = f.Roll and d.Seq1 =f.Seq1 and d.Seq2 = f.Seq2 and d.Dyelot = f.Dyelot 
where f.lock=1 and d.Id = '{0}'", this.CurrentMaintain["id"]);
            if (!(result = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                this.ShowErr(sqlcmd, result);
                return;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    foreach (DataRow tmp in datacheck.Rows)
                    {
                        ids += $"SP#: {tmp["poid"]} Seq#: {tmp["seq1"]}-{tmp["seq2"]} Roll#: {tmp["roll"]} Dyelot: {tmp["Dyelot"]} is locked!!" + Environment.NewLine;
                    }

                    MyUtility.Msg.WarningBox("Material Locked!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }
            #endregion

            #region 檢查庫存項WMSLock
            if (!Prgs.ChkWMSLock(this.CurrentMaintain["id"].ToString(), "Issue_Detail"))
            {
                return;
            }
            #endregion

            #region 檢查Location是否為空值
            if (Prgs.ChkLocation(this.CurrentMaintain["ID"].ToString(), "Issue_Detail") == false)
            {
                return;
            }
            #endregion

            #region 檢查負數庫存

            sqlcmd = string.Format(
                @"
select	d.poid
		,d.seq1
		,d.seq2
		,d.Roll
		,d.Qty
		,[balanceQty] = isnull(f.InQty,0) - isnull(f.OutQty,0) + isnull(f.AdjustQty,0) - isnull(f.ReturnQty,0)
		,d.Dyelot
from (	SELECT POID, StockType, Roll, Seq1, Seq2, Dyelot, [Qty] = sum(Qty)
		from dbo.Issue_Detail WITH (NOLOCK)
		where id = '{0}' 
		group by POID, StockType, Roll, Seq1, Seq2, Dyelot) d
left join FtyInventory f WITH (NOLOCK) on   d.POID = f.POID  AND
                                            D.StockType = F.StockType and 
                                            d.Roll = f.Roll and
                                            d.Seq1 =f.Seq1 and
                                            d.Seq2 = f.Seq2 and
                                            d.Dyelot = f.Dyelot
where (isnull(f.InQty,0) - isnull(f.OutQty,0) + isnull(f.AdjustQty,0) - isnull(f.ReturnQty,0) - d.Qty < 0)", this.CurrentMaintain["id"]);
            if (!(result = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                this.ShowErr(sqlcmd, result);
                return;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    foreach (DataRow tmp in datacheck.Rows)
                    {
                        ids += string.Format(
                            "SP#: {0} Seq#: {1}-{2} Roll#: {3} Dyelot: {6}'s balance: {4} is less than issue qty: {5}" + Environment.NewLine, tmp["poid"], tmp["seq1"], tmp["seq2"], tmp["roll"], tmp["balanceqty"], tmp["qty"], tmp["Dyelot"]);
                    }

                    MyUtility.Msg.WarningBox("Balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }

            #endregion 檢查負數庫存

            #region 更新庫存數量  ftyinventory
            var bs1 = (from b in dtIssue_Detail.AsEnumerable()
                       group b by new
                       {
                           poid = b.Field<string>("poid"),
                           seq1 = b.Field<string>("seq1"),
                           seq2 = b.Field<string>("seq2"),
                           stocktype = b.Field<string>("stocktype"),
                       }
                        into m
                       select new Prgs_POSuppDetailData
                       {
                           Poid = m.First().Field<string>("poid"),
                           Seq1 = m.First().Field<string>("seq1"),
                           Seq2 = m.First().Field<string>("seq2"),
                           Stocktype = m.First().Field<string>("stocktype"),
                           Qty = m.Sum(w => w.Field<decimal>("qty")),
                       }).ToList();
            StringBuilder sqlupd2_B = new StringBuilder();
            sqlupd2_B.Append(Prgs.UpdateMPoDetail(4, null, true));
            string sqlupd2_FIO = Prgs.UpdateFtyInventory_IO(4, null, true);
            #endregion

            Exception errMsg = null;
            using (TransactionScope transactionscope = new TransactionScope())
            {
                try
                {
                    DataTable resulttb;
                    if (!(result = MyUtility.Tool.ProcessWithObject(bs1, string.Empty, sqlupd2_B.ToString(), out resulttb, "#TmpSource")))
                    {
                        throw result.GetException();
                    }

                    if (!(result = MyUtility.Tool.ProcessWithDatatable(dtIssue_Detail, string.Empty, sqlupd2_FIO, out resulttb, "#TmpSource")))
                    {
                        throw result.GetException();
                    }

                    if (!(result = DBProxy.Current.Execute(null, $"update Issue set status = 'Confirmed', editname = '{Env.User.UserID}', editdate = GETDATE() where id = '{this.CurrentMaintain["id"]}'")))
                    {
                        throw result.GetException();
                    }

                    if (!(result = Prgs.UpdateWH_Barcode(true, dtIssue_Detail, this.Name, out bool fromNewBarcode, dtOriFtyInventory)))
                    {
                        throw result.GetException();
                    }

                    transactionscope.Complete();
                }
                catch (Exception ex)
                {
                    errMsg = ex;
                }
            }

            if (!MyUtility.Check.Empty(errMsg))
            {
                this.ShowErr(errMsg);
                return;
            }

            // AutoWHFabric WebAPI
            Prgs_WMS.WMSprocess(false, dtIssue_Detail, this.Name, EnumStatus.New, EnumStatus.Confirm, dtOriFtyInventory);
            MyUtility.Msg.InfoBox("Confirmed successful");
        }

        /// <inheritdoc/>
        protected override void ClickUnconfirm()
        {
            this.RenewData(); // 先重載資料, 避免雙開程式狀況
            base.ClickUnconfirm();
            if (this.CurrentMaintain == null ||
                MyUtility.Msg.QuestionBox("Do you want to unconfirme it?") == DialogResult.No)
            {
                return;
            }

            // 第3層才是 issue_detail
            DualResult result = DBProxy.Current.Select(null, $"select * from issue_detail WITH (NOLOCK) where id = '{this.CurrentMaintain["ID"]}'", out DataTable dtIssue_Detail);
            if (!result)
            {
                this.ShowErr(result);
            }

            // 取得 FtyInventory 資料 (包含PO_Supp_Detail.FabricType)
            result = Prgs.GetFtyInventoryData(dtIssue_Detail, this.Name, out DataTable dtOriFtyInventory);
            DataTable datacheck;
            string ids = string.Empty;

            #region 檢查庫存項lock
            string sqlcmd = string.Format(
                @"
Select d.poid,d.seq1,d.seq2,d.Roll,d.Qty
    ,isnull(f.InQty,0) - isnull(f.OutQty,0) + isnull(f.AdjustQty,0) - isnull(f.ReturnQty,0) as balanceQty
    ,d.Dyelot
from dbo.Issue_Detail d WITH (NOLOCK) inner join FtyInventory f WITH (NOLOCK) 
on d.POID = f.POID  AND D.StockType = F.StockType
and d.Roll = f.Roll and d.Seq1 =f.Seq1 and d.Seq2 = f.Seq2 and d.Dyelot = f.Dyelot 
where f.lock=1 and d.Id = '{0}'", this.CurrentMaintain["id"]);
            if (!(result = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                this.ShowErr(sqlcmd, result);
                return;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    foreach (DataRow tmp in datacheck.Rows)
                    {
                        ids += $"SP#: {tmp["poid"]} Seq#: {tmp["seq1"]}-{tmp["seq2"]} Roll#: {tmp["roll"]} Dyelot: {tmp["Dyelot"]} is locked!!" + Environment.NewLine;
                    }

                    MyUtility.Msg.WarningBox("Material Locked!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }
            #endregion

            #region 檢查庫存項WMSLock
            if (!Prgs.ChkWMSLock(this.CurrentMaintain["id"].ToString(), "Issue_Detail"))
            {
                return;
            }
            #endregion

            #region 檢查資料有任一筆WMS已完成, 就不能unConfirmed
            if (!Prgs.ChkWMSCompleteTime(dtIssue_Detail, "Issue_Detail"))
            {
                return;
            }
            #endregion

            #region 檢查負數庫存

            sqlcmd = string.Format(
                @"
select	d.poid
		,d.seq1
		,d.seq2
		,d.Roll
		,d.Qty
		,[balanceQty] = isnull(f.InQty,0) - isnull(f.OutQty,0) + isnull(f.AdjustQty,0) - isnull(f.ReturnQty,0)
		,d.Dyelot
from (	SELECT POID, StockType, Roll, Seq1, Seq2, Dyelot, [Qty] = sum(Qty)
		from dbo.Issue_Detail WITH (NOLOCK)
		where id = '{0}' 
		group by POID, StockType, Roll, Seq1, Seq2, Dyelot) d
left join FtyInventory f WITH (NOLOCK) on   d.POID = f.POID  AND
                                            D.StockType = F.StockType and 
                                            d.Roll = f.Roll and
                                            d.Seq1 =f.Seq1 and
                                            d.Seq2 = f.Seq2 and
                                            d.Dyelot = f.Dyelot
where (isnull(f.InQty,0) - isnull(f.OutQty,0) + isnull(f.AdjustQty,0) - isnull(f.ReturnQty,0) + d.Qty < 0)
", this.CurrentMaintain["id"]);
            if (!(result = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                this.ShowErr(sqlcmd, result);
                return;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    foreach (DataRow tmp in datacheck.Rows)
                    {
                        ids += string.Format("SP#: {0} Seq#: {1}-{2} Roll#: {3} Dyelot: {6}'s balance: {4} is less than issue qty: {5}" + Environment.NewLine, tmp["poid"], tmp["seq1"], tmp["seq2"], tmp["roll"], tmp["balanceqty"], tmp["qty"], tmp["Dyelot"]);
                    }

                    MyUtility.Msg.WarningBox("Balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }

            #endregion 檢查負數庫存

            #region 更新庫存數量  ftyinventory
            sqlcmd = string.Format(@"select * from issue_detail WITH (NOLOCK) where id='{0}'", this.CurrentMaintain["id"]);
            if (!(result = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                this.ShowErr(sqlcmd, result);
                return;
            }

            var bsfio = (from m in datacheck.AsEnumerable()
                         select new
                         {
                             poid = m.Field<string>("poid"),
                             seq1 = m.Field<string>("seq1"),
                             seq2 = m.Field<string>("seq2"),
                             stocktype = m.Field<string>("stocktype"),
                             qty = -m.Field<decimal>("qty"),
                             roll = m.Field<string>("roll"),
                             dyelot = m.Field<string>("dyelot"),
                         }).ToList();

            var bs1 = (from b in datacheck.AsEnumerable()
                       group b by new
                       {
                           poid = b.Field<string>("poid"),
                           seq1 = b.Field<string>("seq1"),
                           seq2 = b.Field<string>("seq2"),
                           stocktype = b.Field<string>("stocktype"),
                       }
                        into m
                       select new Prgs_POSuppDetailData
                       {
                           Poid = m.First().Field<string>("poid"),
                           Seq1 = m.First().Field<string>("seq1"),
                           Seq2 = m.First().Field<string>("seq2"),
                           Stocktype = m.First().Field<string>("stocktype"),
                           Qty = -m.Sum(w => w.Field<decimal>("qty")),
                       }).ToList();

            StringBuilder sqlupd2_B = new StringBuilder();
            sqlupd2_B.Append(Prgs.UpdateMPoDetail(4, null, false));
            string sqlupd2_FIO = Prgs.UpdateFtyInventory_IO(4, null, false);
            #endregion 更新庫存數量  ftyinventory

            #region UnConfirmed 廠商能上鎖→PMS更新→廠商更新

            // 先確認 WMS 能否上鎖, 不能直接 return
            if (!Prgs_WMS.WMSLock(dtIssue_Detail, dtOriFtyInventory, this.Name, EnumStatus.Unconfirm))
            {
                return;
            }

            // PMS 的資料更新
            Exception errMsg = null;
            List<AutoRecord> autoRecordList = new List<AutoRecord>();
            using (TransactionScope transactionscope = new TransactionScope())
            {
                try
                {
                    DataTable resulttb;
                    if (!(result = MyUtility.Tool.ProcessWithObject(bs1, string.Empty, sqlupd2_B.ToString(), out resulttb, "#TmpSource")))
                    {
                        throw result.GetException();
                    }

                    if (!(result = MyUtility.Tool.ProcessWithObject(bsfio, string.Empty, sqlupd2_FIO, out resulttb, "#TmpSource")))
                    {
                        throw result.GetException();
                    }

                    // Barcode 需要判斷新的庫存, 在更新 FtyInventory 之後
                    if (!(result = Prgs.UpdateWH_Barcode(false, dtIssue_Detail, this.Name, out bool fromNewBarcode, dtOriFtyInventory)))
                    {
                        throw result.GetException();
                    }

                    if (!(result = DBProxy.Current.Execute(null, $"update Issue set status = 'New', editname = '{Env.User.UserID}', editdate = GETDATE() where id = '{this.CurrentMaintain["id"]}'")))
                    {
                        throw result.GetException();
                    }

                    // transactionscope 內, 準備 WMS 資料 & 將資料寫入 AutomationCreateRecord (Delete, Unconfirm)
                    Prgs_WMS.WMSprocess(false, dtIssue_Detail, this.Name, EnumStatus.Delete, EnumStatus.Unconfirm, dtOriFtyInventory, typeCreateRecord: 1, autoRecord: autoRecordList);
                    transactionscope.Complete();
                }
                catch (Exception ex)
                {
                    errMsg = ex;
                }
            }

            if (!MyUtility.Check.Empty(errMsg))
            {
                Prgs_WMS.WMSUnLock(false, dtIssue_Detail, this.Name, EnumStatus.UnLock, EnumStatus.Unconfirm, dtOriFtyInventory);
                this.ShowErr(errMsg);
                return;
            }

            // PMS 更新之後,才執行WMS
            Prgs_WMS.WMSprocess(false, dtIssue_Detail, this.Name, EnumStatus.Delete, EnumStatus.Unconfirm, dtOriFtyInventory, typeCreateRecord: 2, autoRecord: autoRecordList);
            MyUtility.Msg.InfoBox("UnConfirmed successful");
            #endregion
        }

        /// <inheritdoc/>
        protected override bool ClickPrint()
        {
            WH_Print p = new WH_Print(this.CurrentMaintain, "P62")
            {
                CurrentDataRow = this.CurrentMaintain,
            };

            p.ShowDialog();

            // 代表要列印 RDLC
            if (p.IsPrintRDLC)
            {
                string sqlcmd = $@"update Issue set  PrintName = '{Env.User.UserID}' , PrintDate = GETDATE()
                                where id = '{this.CurrentMaintain["id"]}'";

                DualResult result = DBProxy.Current.Execute(null, sqlcmd);
                if (!result)
                {
                    this.ShowErr(result);
                    return false;
                }

                string id = this.CurrentMaintain["ID"].ToString();
                string remark = this.CurrentMaintain["Remark"].ToString();
                string cutplanID = this.CurrentMaintain["cutplanID"].ToString();
                string issuedate = ((DateTime)MyUtility.Convert.GetDate(this.CurrentMaintain["issuedate"])).ToShortDateString();
                string factoryID = this.CurrentMaintain["FactoryID"].ToString();
                string confirmTime = this.CurrentMaintain["Status"].EqualString("CONFIRMED") ? MyUtility.Convert.GetDate(this.CurrentMaintain["EditDate"]).Value.ToString("yyyy/MM/dd HH:mm:ss") : string.Empty;

                #region  抓表頭資料
                List<SqlParameter> pars = new List<SqlParameter>
                {
                    new SqlParameter("@MDivision", Env.User.Keyword),
                };
                result = DBProxy.Current.Select(string.Empty, @"select NameEN from MDivision where id = @MDivision", pars, out DataTable dt);
                if (!result)
                {
                    this.ShowErr(result);
                }

                string rptTitle = dt.Rows[0]["NameEn"].ToString();
                ReportDefinition report = new ReportDefinition();
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("RptTitle", rptTitle));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("ID", id));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Remark", remark));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("cutplanID", cutplanID));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("issuetime", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("confirmTime", confirmTime));
                report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Factory", "Factory: " + factoryID));
                #endregion

                #region  抓表身資料
                sqlcmd = @"
select[Poid] = IIF((t.poid = lag(t.poid, 1, '') over(order by t.poid, t.seq1, t.seq2, t.Dyelot, t.Roll)

                        AND(t.seq1 = lag(t.seq1, 1, '') over(order by t.poid, t.seq1, t.seq2, t.Dyelot, t.Roll))

                        AND(t.seq2 = lag(t.seq2, 1, '') over(order by t.poid, t.seq1, t.seq2, t.Dyelot, t.Roll)))
			          , ''
                      , t.poid) 
        , [Seq] = IIF((t.poid = lag(t.poid, 1, '') over (order by t.poid, t.seq1, t.seq2, t.Dyelot, t.Roll)

                         AND(t.seq1 = lag(t.seq1, 1, '') over (order by t.poid, t.seq1, t.seq2, t.Dyelot, t.Roll))
			             AND(t.seq2 = lag(t.seq2, 1, '') over (order by t.poid, t.seq1, t.seq2, t.Dyelot, t.Roll))) 
			            , ''
                        , t.seq1+ '-' +t.seq2)
        , [GroupPoid] = t.poid 
        , [GroupSeq] = t.seq1+ '-' +t.seq2 
        , [desc] = IIF((t.poid = lag(t.poid, 1, '') over (order by t.poid, t.seq1, t.seq2, t.Dyelot, t.Roll)
                          AND(t.seq1 = lag(t.seq1, 1, '') over (order by t.poid, t.seq1, t.seq2, t.Dyelot, t.Roll))
			              AND(t.seq2 = lag(t.seq2, 1, '') over (order by t.poid, t.seq1, t.seq2, t.Dyelot, t.Roll))) 
				        , ''
                        , (SELECT Concat(stock7X.value
                                         , char(10)
                                            , rtrim(fbr.DescDetail)
                                            , char (10)
                                            , char (10)
                                            , (Select concat(ID, '-', Name) from Color WITH(NOLOCK) where id = iss.ColorId and BrandId = fbr.BrandID)
                                        )
									FROM fabric fbr WITH(NOLOCK) WHERE SCIRefno = p.SCIRefno))
		, Mdesc = IIF((t.poid = lag(t.poid, 1, '') over (order by t.poid, t.seq1, t.seq2, t.Dyelot, t.Roll)
                          AND(t.seq1 = lag(t.seq1, 1, '') over (order by t.poid, t.seq1, t.seq2, t.Dyelot, t.Roll))
			              AND(t.seq2 = lag(t.seq2, 1, '') over (order by t.poid, t.seq1, t.seq2, t.Dyelot, t.Roll))) 
                          ,Mdesc2.value,Mdesc.value)
        , t.Roll
        , t.Dyelot
        , t.Qty
        , p.StockUnit
        , [location]=dbo.Getlocation(b.ukey)  
        , b.ContainerCode
        , [Total]=sum(t.Qty) OVER(PARTITION BY t.POID , t.Seq1, t.Seq2 )
        , [ToneGrp] =b .Tone
from dbo.Issue_Detail t WITH (NOLOCK)
inner join Issue_Summary iss WITH (NOLOCK) on t.Issue_SummaryUkey = iss.Ukey
left join dbo.PO_Supp_Detail p  WITH (NOLOCK) on    p.id= t.poid
                                                    and p.SEQ1 = t.Seq1
                                                    and p.seq2 = t.Seq2
left join FtyInventory b WITH (NOLOCK) on   b.poid = t.poid
                                            and b.seq1 = t.seq1
                                            and b.seq2= t.seq2
                                            and b.Roll = t.Roll
                                            and b.Dyelot = t.Dyelot
                                            and b.StockType = t.StockType
outer apply(select value = Concat( 'Relaxation Type：'
                         ,(select FabricRelaxationID from [dbo].[SciMES_RefnoRelaxtime] where Refno = p.Refno)
                         ,CHAR(13) + CHAR(10)
                         ,CHAR(13) + CHAR(10)
                         ,(Select iss.Seq1 + '-' + iss.Seq2 + ':' + psdsS.SpecValue + pd.Special + '=' + convert(varchar, pd.Qty) 
                            from PO_Supp_Detail_Spec psdsS WITH (NOLOCK) 
                            join PO_Supp_Detail pd on pd.ID = psdsS.ID and pd.seq1 = psdsS.seq1 and pd.seq2 = psdsS.seq2
                           where psdsS.ID = iss.POID and psdsS.seq1 = iss.seq1 and psdsS.seq2 = iss.seq2 and psdsS.SpecColumnID = 'Size'))) Mdesc
outer apply(select value = Concat(CHAR(13) + CHAR(10),CHAR(13) + CHAR(10)
                         ,(Select iss.Seq1 + '-' + iss.Seq2 + ':' + psdsS.SpecValue + pd.Special + '=' + convert(varchar, pd.Qty) 
                            from PO_Supp_Detail_Spec psdsS WITH (NOLOCK) 
                            join PO_Supp_Detail pd on pd.ID = psdsS.ID and pd.seq1 = psdsS.seq1 and pd.seq2 = psdsS.seq2
                           where psdsS.ID = iss.POID and psdsS.seq1 = iss.seq1 and psdsS.seq2 = iss.seq2 and psdsS.SpecColumnID = 'Size'))) Mdesc2
outer apply (
    select value = iif(left(t.seq1, 1) != '7', ''
                                               , '**PLS USE STOCK FROM SP#:' + iif(isnull(concat(p.StockPOID, p.StockSeq1, p.StockSeq2), '') = '', '', concat(p.StockPOID, p.StockSeq1, p.StockSeq2)) + '**')
) as stock7X
where t.id= @ID";

                pars = new List<SqlParameter>
                {
                    new SqlParameter("@ID", this.CurrentMaintain["ID"].ToString()),
                };

                result = DBProxy.Current.Select(string.Empty, sqlcmd, pars, out DataTable dtExcel);
                if (!result)
                {
                    this.ShowErr(result);
                }

                if (dtExcel == null || dtExcel.Rows.Count == 0)
                {
                    MyUtility.Msg.InfoBox("Data not found !!!", string.Empty);
                    return false;
                }

                // 傳 list 資料 (直接利用P10_PrintData的結構)
                List<P10_PrintData> data = dtExcel.AsEnumerable()
                    .Select(row1 => new P10_PrintData()
                    {
                        GroupPoid = row1["GroupPoid"].ToString().Trim(),
                        GroupSeq = row1["GroupSeq"].ToString().Trim(),
                        Poid = row1["poid"].ToString().Trim(),
                        Seq = row1["SEQ"].ToString().Trim(),
                        Desc = row1["desc"].ToString().Trim(),
                        MDesc = row1["Mdesc"].ToString().Trim(),
                        Location = row1["Location"].ToString().Trim() + Environment.NewLine + row1["ContainerCode"].ToString().Trim(),
                        Unit = row1["StockUnit"].ToString().Trim(),
                        Roll = row1["Roll"].ToString().Trim(),
                        Dyelot = row1["Dyelot"].ToString().Trim(),
                        Qty = row1["Qty"].ToString().Trim(),
                        Total = row1["Total"].ToString().Trim(),
                        ToneGrp = row1["ToneGrp"].ToString().Trim(),
                    }).OrderBy(s => s.GroupPoid).ThenBy(s => s.GroupSeq).ThenBy(s => s.Dyelot).ThenBy(s => s.Roll).ToList();

                report.ReportDataSource = data;
                #endregion

                #region  指定是哪個 RDLC

                // DualResult result;
                Type reportResourceNamespace = typeof(P10_PrintData);
                Assembly reportResourceAssembly = reportResourceNamespace.Assembly;
                string reportResourceName = "P62_Print.rdlc";

                if (!(result = ReportResources.ByEmbeddedResource(reportResourceAssembly, reportResourceNamespace, reportResourceName, out IReportResource reportresource)))
                {
                    return false;
                }

                report.ReportResource = reportresource;

                // 開啟 report view
                var frm = new Win.Subs.ReportView(report)
                {
                    MdiParent = this.MdiParent,
                };
                frm.Show();

                #endregion
            }

            return true;
        }

        private void BtnCutRefNo_Click(object sender, EventArgs e)
        {
            var frm = new P62_CuttingRef(this.CurrentMaintain)
            {
                P62 = this,
            };
            frm.ShowDialog(this);
        }

        private void BtnCallP99_Click(object sender, EventArgs e)
        {
            P99_CallForm.CallForm(this.CurrentMaintain["ID"].ToString(), this.Name, this);
        }
    }
}
