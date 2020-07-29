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
using System.Transactions;
using Sci.Production.PublicPrg;
using System.Data.SqlClient;
using Sci.Win;
using System.Reflection;
using Sci.Production.Automation;
using System.Threading.Tasks;

namespace Sci.Production.Warehouse
{
    public partial class P62 : Win.Tems.Input8
    {
        public P62(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = string.Format("Type='I' and MDivisionID = '{0}'", Env.User.Keyword);

            // DoSubForm = new P10_Detail();
        }

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
                string STRrequestqty = this.detailgrid.Rows[e.RowIndex].Cells["requestqty"].Value.ToString();
                string STRaccu_issue = this.detailgrid.Rows[e.RowIndex].Cells["accu_issue"].Value.ToString();
                string STRqty = this.detailgrid.Rows[e.RowIndex].Cells["qty"].Value.ToString();
                decimal DECrequestqty;
                decimal DECaccu_issue;
                decimal DECqty;
                if (!decimal.TryParse(STRrequestqty, out DECrequestqty))
                {
                    DECrequestqty = 0;
                }

                if (!decimal.TryParse(STRaccu_issue, out DECaccu_issue))
                {
                    DECaccu_issue = 0;
                }

                if (!decimal.TryParse(STRqty, out DECqty))
                {
                    DECqty = 0;
                }

                if (e.ColumnIndex == this.detailgrid.Columns["bal_qty"].Index && !MyUtility.Check.Empty(this.detailgrid.Rows[e.RowIndex].Cells["requestqty"].Value))
                {
                    e.Value = DECrequestqty - DECaccu_issue;
                }

                if (e.ColumnIndex == this.detailgrid.Columns["var_qty"].Index && !MyUtility.Check.Empty(this.detailgrid.Rows[e.RowIndex].Cells["requestqty"].Value))
                {
                    e.Value = DECrequestqty - DECaccu_issue - DECqty;
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

        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["ID"].ToString();
            string cutplanID = (e.Master == null) ? string.Empty : e.Master["cutplanID"].ToString();
            this.DetailSelectCommand = string.Format(
                @"
select  s.*
    , f.Refno
	, [description] = f.DescDetail
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
	where c.id = '{1}'
	and c.Seq1 = s.Seq1
	and c.Seq2 = s.Seq2
) ec
outer apply(
	select accu_issue = isNull(sum(qty) , 0.00)
	from Issue a WITH (NOLOCK) 
	inner join Issue_Summary b WITH (NOLOCK) on a.Id=b.Id 
	where b.poid = s.poid 
			and a.CutplanID = '{1}' 
			and b.SCIRefno = s.SCIRefno 
			and b.Colorid = s.Colorid 
			and a.status = 'Confirmed'
			and a.id != s.id
			and a.Type = 'I'
) accu
outer apply(
	select top 1 p.NETQty
	from PO_Supp_Detail p
	where p.ID = s.POID and p.SCIRefno = s.SCIRefno and p.ColorID = s.ColorID
			and p.SEQ1 like 'A%' and p.NETQty <> 0
)Net
outer apply(select CuttingID from CutTapePlan where Id = '{1}' )c
outer apply(
	select ReqQty = isNull(sum(t2.ReleaseQty), 0)
	from CutTapePlan t1
	inner join CutTapePlan_Detail t2 on t1.id = t2.id
	inner join Issue i on i.CutplanID = t1.ID
	where t1.CuttingID = c.CuttingID
	and t1.id <> '{1}'
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

Where s.id = '{0}'", masterID, cutplanID);

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
                              where scirefno = s.scirefno)
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
	select top 1 p.NETQty
	from PO_Supp_Detail p
	where p.ID = s.POID and p.SCIRefno = s.SCIRefno and p.ColorID = s.ColorID
		 and p.SEQ1 like 'A%' and p.NETQty <> 0
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

        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["MDivisionID"] = Env.User.Keyword;
            this.CurrentMaintain["FactoryID"] = Env.User.Factory;
            this.CurrentMaintain["Status"] = "New";
            this.CurrentMaintain["Type"] = "I";
            this.CurrentMaintain["issuedate"] = DateTime.Now;
        }

        protected override bool ClickDeleteBefore()
        {
            if (this.CurrentMaintain["Status"].EqualString("CONFIRMED"))
            {
                MyUtility.Msg.WarningBox("Data is confirmed, can't delete.");
                return false;
            }

            return base.ClickDeleteBefore();
        }

        protected override bool ClickEditBefore()
        {
            if (this.CurrentMaintain["Status"].EqualString("CONFIRMED"))
            {
                MyUtility.Msg.WarningBox("Data is confirmed, can't modify.");
                return false;
            }

            return base.ClickEditBefore();
        }

        protected override bool ClickSaveBefore()
        {
            #region 表頭必輸檢查

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

            ////assign 給detail table ID
            // DataTable tmp = (DataTable)detailgridbs.DataSource;
            // foreach (DataRow row in tmp.Rows)
            // {
            //    row.SetField("ID", MyUtility.Convert.GetString(CurrentMaintain["id"]));
            //    DataTable subDT;
            //    if (GetSubDetailDatas(row, out subDT))
            //    {
            //        foreach (DataRow ddrow in subDT.Rows)
            //        {
            //            ddrow.SetField("ID", MyUtility.Convert.GetString(CurrentMaintain["id"]));
            //        }
            //    }
            // }

            // 取BarcodeNo
            IList<DataRow> listSubDetail = new List<DataRow>();
            DataTable dtTmp;
            foreach (DataRow dr in this.DetailDatas)
            {
                this.GetSubDetailDatas(dr, out dtTmp);

                foreach (DataRow subDr in dtTmp.Rows)
                {
                    listSubDetail.Add(subDr);
                }
            }

            DualResult resultBarcodeNo = Prgs.FillIssueDetailBarcodeNo(listSubDetail);

            if (!resultBarcodeNo)
            {
                return resultBarcodeNo;
            }

            // 將Issue_Detail的數量更新Issue_Summary
            DataTable subDetail;
            foreach (DataRow detailRow in this.DetailDatas)
            {
                this.GetSubDetailDatas(detailRow, out subDetail);
                if (subDetail.Rows.Count == 0)
                {
                    detailRow["Qty"] = 0;
                }
                else
                {
                    decimal detailQty = subDetail.AsEnumerable().Sum(s => s.RowState != DataRowState.Deleted ? (decimal)s["Qty"] : 0);
                }
            }

            return base.ClickSaveBefore();
        }

        protected override DualResult ConvertSubDetailDatasFromDoSubForm(SubDetailConvertFromEventArgs e)
        {
            sum_subDetail(e.Detail, e.SubDetails);

            return base.ConvertSubDetailDatasFromDoSubForm(e);
        }

        static void sum_subDetail(DataRow target, DataTable source)
        {
            target["aiqqty"] = (decimal)target["aiqqty"] - (decimal)target["qty"];
            target["qty"] = (source.Rows.Count == 0) ? 0m : source.AsEnumerable().Where(r => r.RowState != DataRowState.Deleted)
                .Sum(r => r.Field<decimal>("qty"));
            target["aiqqty"] = (decimal)target["aiqqty"] + (decimal)target["qty"];
            target["avqty"] = (decimal)target["arqty"] - (decimal)target["aiqqty"];
        }

        private void btnAutoPick_Click(object sender, EventArgs e)
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

                    sum_subDetail(dr, subDT);
                }
            }

            // 強制觸發CellValueNeeded，否則游標在Issue QTY上可能會無法觸發。
            this.detailgrid.SelectRowToNext();
            this.detailgrid.SelectRowToPrev();
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
        }

        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            var dr = this.CurrentMaintain;
            if (dr == null)
            {
                return;
            }

            StringBuilder sqlupd2 = new StringBuilder();
            string sqlcmd = string.Empty, sqlupd3 = string.Empty, ids = string.Empty;
            DualResult result, result2;
            DataTable datacheck;
            string sqlupd2_FIO = string.Empty;
            StringBuilder sqlupd2_B = new StringBuilder();

            #region Check Issue_Detail isn't Empty
            string checkSQL = string.Format(
                @"
select  isnull(sum(Qty), 0)
from issue_detail WITH (NOLOCK) 
where id = '{0}'", this.CurrentMaintain["ID"]);

            string checkQty = MyUtility.GetValue.Lookup(checkSQL);
            if (Convert.ToDecimal(checkQty) == 0)
            {
                MyUtility.Msg.WarningBox("All Issue_Qty are zero", "Warning");
                return;
            }
            #endregion

            #region 檢查庫存項lock
            sqlcmd = string.Format(
                @"Select d.poid,d.seq1,d.seq2,d.Roll,d.Qty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty,d.Dyelot
from dbo.Issue_Detail d WITH (NOLOCK) inner join FtyInventory f WITH (NOLOCK) 
on d.POID = f.POID  AND D.StockType = F.StockType
and d.Roll = f.Roll and d.Seq1 =f.Seq1 and d.Seq2 = f.Seq2 and d.Dyelot = f.Dyelot 
where f.lock=1 and d.Id = '{0}'", this.CurrentMaintain["id"]);
            if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                this.ShowErr(sqlcmd, result2);
                return;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    foreach (DataRow tmp in datacheck.Rows)
                    {
                        ids += string.Format(
                            "SP#: {0} Seq#: {1}-{2} Roll#: {3} Dyelot: {4} is locked!!" + Environment.NewLine,
                            tmp["poid"], tmp["seq1"], tmp["seq2"], tmp["roll"], tmp["Dyelot"]);
                    }

                    MyUtility.Msg.WarningBox("Material Locked!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }
            #endregion

            #region 檢查負數庫存

            sqlcmd = string.Format(
                @"Select d.poid,d.seq1,d.seq2,d.Roll,d.Qty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty,d.Dyelot
from dbo.Issue_Detail d WITH (NOLOCK) left join FtyInventory f WITH (NOLOCK) 
on d.POID = f.POID  AND D.StockType = F.StockType
and d.Roll = f.Roll and d.Seq1 =f.Seq1 and d.Seq2 = f.Seq2 and d.Dyelot = f.Dyelot 
where (isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) - d.Qty < 0) and d.Id = '{0}'", this.CurrentMaintain["id"]);
            if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                this.ShowErr(sqlcmd, result2);
                return;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    foreach (DataRow tmp in datacheck.Rows)
                    {
                        ids += string.Format(
                            "SP#: {0} Seq#: {1}-{2} Roll#: {3} Dyelot: {6}'s balance: {4} is less than issue qty: {5}" + Environment.NewLine,
                            tmp["poid"], tmp["seq1"], tmp["seq2"], tmp["roll"], tmp["balanceqty"], tmp["qty"], tmp["Dyelot"]);
                    }

                    MyUtility.Msg.WarningBox("Balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }

            #endregion 檢查負數庫存

            #region 更新表頭狀態資料

            sqlupd3 = string.Format(
                @"update Issue set status='Confirmed', editname = '{0}' , editdate = GETDATE()
                                where id = '{1}'", Env.User.UserID, this.CurrentMaintain["id"]);

            #endregion 更新表頭狀態資料

            #region 更新庫存數量  ftyinventory
            sqlcmd = string.Format(@"select * from issue_detail WITH (NOLOCK) where id='{0}'", this.CurrentMaintain["id"]);
            if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                this.ShowErr(sqlcmd, result2);
                return;
            }

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
                           poid = m.First().Field<string>("poid"),
                           seq1 = m.First().Field<string>("seq1"),
                           seq2 = m.First().Field<string>("seq2"),
                           stocktype = m.First().Field<string>("stocktype"),
                           qty = m.Sum(w => w.Field<decimal>("qty")),
                       }).ToList();
            sqlupd2_B.Append(Prgs.UpdateMPoDetail(4, null, true));

            sqlupd2_FIO = Prgs.UpdateFtyInventory_IO(4, null, true);
            #endregion

            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    DataTable resulttb;
                    if (!(result = MyUtility.Tool.ProcessWithObject(bs1, string.Empty, sqlupd2_B.ToString(), out resulttb, "#TmpSource")))
                    {
                        _transactionscope.Dispose();
                        this.ShowErr(result);
                        return;
                    }

                    if (!(result = MyUtility.Tool.ProcessWithDatatable(datacheck, string.Empty, sqlupd2_FIO, out resulttb, "#TmpSource")))
                    {
                        _transactionscope.Dispose();
                        this.ShowErr(result);
                        return;
                    }

                    if (!(result = DBProxy.Current.Execute(null, sqlupd3)))
                    {
                        _transactionscope.Dispose();
                        this.ShowErr(sqlupd3, result);
                        return;
                    }

                    _transactionscope.Complete();
                    _transactionscope.Dispose();
                    SentToGensong_AutoWHFabric();
                    MyUtility.Msg.InfoBox("Confirmed successful");
                }
                catch (Exception ex)
                {
                    _transactionscope.Dispose();
                    this.ShowErr("Commit transaction error.", ex);
                    return;
                }
            }

            _transactionscope.Dispose();
            _transactionscope = null;
        }

        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            DataTable datacheck;
            DataTable dt = (DataTable)this.detailgridbs.DataSource;

            DialogResult dResult = MyUtility.Msg.QuestionBox("Do you want to unconfirme it?");
            if (dResult == DialogResult.No)
            {
                return;
            }

            var dr = this.CurrentMaintain;
            if (dr == null)
            {
                return;
            }

            StringBuilder sqlupd2 = new StringBuilder();
            string sqlcmd = string.Empty, sqlupd3 = string.Empty, ids = string.Empty;
            DualResult result, result2;
            string sqlupd2_FIO = string.Empty;
            StringBuilder sqlupd2_B = new StringBuilder();

            #region 檢查庫存項lock
            sqlcmd = string.Format(
                @"Select d.poid,d.seq1,d.seq2,d.Roll,d.Qty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty,d.Dyelot
from dbo.Issue_Detail d WITH (NOLOCK) inner join FtyInventory f WITH (NOLOCK) 
on d.POID = f.POID  AND D.StockType = F.StockType
and d.Roll = f.Roll and d.Seq1 =f.Seq1 and d.Seq2 = f.Seq2 and d.Dyelot = f.Dyelot 
where f.lock=1 and d.Id = '{0}'", this.CurrentMaintain["id"]);
            if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                this.ShowErr(sqlcmd, result2);
                return;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    foreach (DataRow tmp in datacheck.Rows)
                    {
                        ids += string.Format(
                            "SP#: {0} Seq#: {1}-{2} Roll#: {3} Dyelot: {4} is locked!!" + Environment.NewLine,
                            tmp["poid"], tmp["seq1"], tmp["seq2"], tmp["roll"], tmp["Dyelot"]);
                    }

                    MyUtility.Msg.WarningBox("Material Locked!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }
            #endregion

            #region 檢查負數庫存

            sqlcmd = string.Format(
                @"Select d.poid,d.seq1,d.seq2,d.Roll,d.Qty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty,d.Dyelot
from dbo.Issue_Detail d WITH (NOLOCK) left join FtyInventory f WITH (NOLOCK) 
on d.POID = f.POID  AND D.StockType = F.StockType
and d.Roll = f.Roll and d.Seq1 =f.Seq1 and d.Seq2 = f.Seq2 and d.Dyelot = f.Dyelot 
where (isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) + d.Qty < 0) and d.Id = '{0}'", this.CurrentMaintain["id"]);
            if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                this.ShowErr(sqlcmd, result2);
                return;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    foreach (DataRow tmp in datacheck.Rows)
                    {
                        ids += string.Format(
                            "SP#: {0} Seq#: {1}-{2} Roll#: {3} Dyelot: {6}'s balance: {4} is less than issue qty: {5}" + Environment.NewLine,
                            tmp["poid"], tmp["seq1"], tmp["seq2"], tmp["roll"], tmp["balanceqty"], tmp["qty"], tmp["Dyelot"]);
                    }

                    MyUtility.Msg.WarningBox("Balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }

            #endregion 檢查負數庫存

            #region 更新表頭狀態資料

            sqlupd3 = string.Format(
                @"update Issue set status='New', editname = '{0}' , editdate = GETDATE()
                                where id = '{1}'", Env.User.UserID, this.CurrentMaintain["id"]);

            #endregion 更新表頭狀態資料

            #region 更新庫存數量  ftyinventory
            sqlcmd = string.Format(@"select * from issue_detail WITH (NOLOCK) where id='{0}'", this.CurrentMaintain["id"]);
            if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                this.ShowErr(sqlcmd, result2);
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
                           poid = m.First().Field<string>("poid"),
                           seq1 = m.First().Field<string>("seq1"),
                           seq2 = m.First().Field<string>("seq2"),
                           stocktype = m.First().Field<string>("stocktype"),
                           qty = -m.Sum(w => w.Field<decimal>("qty")),
                       }).ToList();

            sqlupd2_B.Append(Prgs.UpdateMPoDetail(4, null, false));
            sqlupd2_FIO = Prgs.UpdateFtyInventory_IO(4, null, false);
            #endregion 更新庫存數量  ftyinventory

            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    DataTable resulttb;
                    if (!(result = MyUtility.Tool.ProcessWithObject(bs1, string.Empty, sqlupd2_B.ToString(), out resulttb, "#TmpSource")))
                    {
                        _transactionscope.Dispose();
                        this.ShowErr(result);
                        return;
                    }

                    if (!(result = MyUtility.Tool.ProcessWithObject(bsfio, string.Empty, sqlupd2_FIO, out resulttb, "#TmpSource")))
                    {
                        _transactionscope.Dispose();
                        this.ShowErr(result);
                        return;
                    }

                    if (!(result = DBProxy.Current.Execute(null, sqlupd3)))
                    {
                        _transactionscope.Dispose();
                        this.ShowErr(sqlupd3, result);
                        return;
                    }

                    _transactionscope.Complete();
                    _transactionscope.Dispose();
                    SentToGensong_AutoWHFabric();
                    MyUtility.Msg.InfoBox("UnConfirmed successful");
                }
                catch (Exception ex)
                {
                    _transactionscope.Dispose();
                    this.ShowErr("Commit transaction error.", ex);
                    return;
                }
            }

            _transactionscope.Dispose();
            _transactionscope = null;
        }

        private void SentToGensong_AutoWHFabric()
        {
            if (true) return;// 暫未開放
            // AutoWHFabric WebAPI for Gensong
            if (Gensong_AutoWHFabric.IsGensong_AutoWHFabricEnable)
            {
                DataTable dtDetail = new DataTable();
                string sqlGetData = string.Empty;
                sqlGetData = $@"
select distinct 
 [Id] = i2.Id 
,[Type] = 'I'
,[CutPlanID] = isnull(i.CutplanID,'')
,[EstCutdate] = c.EstCutdate
,[SpreadingNoID] = isnull(c.SpreadingNoID,'')
,[PoId] = i2.POID
,[Seq1] = i2.Seq1
,[Seq2] = i2.Seq2
,[Roll] = i2.Roll
,[Dyelot] = i2.Dyelot
,[Barcode] = fty.Barcode
,[Qty] = i2.Qty
,[Ukey] = i2.ukey
,[Junk] = case when i.Status = 'Confirmed' then convert(bit, 0) else convert(bit, 1) end
,CmdTime = GetDate()
from Production.dbo.Issue_Detail i2
inner join Production.dbo.Issue i on i2.Id=i.Id
left join Production.dbo.Cutplan c on c.ID = i.CutplanID
outer apply(
	select Barcode
	from Production.dbo.FtyInventory
	where POID = i2.POID and Seq1=i2.Seq1
	and Seq2=i2.Seq2 and Roll=i2.Roll and Dyelot=i2.Dyelot
)fty
where i.Type = 'I'
and exists(
		select 1 from Production.dbo.PO_Supp_Detail 
		where id = i2.Poid and seq1=i2.seq1 and seq2=i2.seq2 
		and FabricType='F'
	)
and i.id = '{CurrentMaintain["ID"]}'

";

                DualResult drResult = DBProxy.Current.Select(string.Empty, sqlGetData, out dtDetail);
                if (!drResult)
                {
                    ShowErr(drResult);
                }
                Task.Run(() => new Gensong_AutoWHFabric().SentIssue_DetailToGensongAutoWHFabric(dtDetail))
               .ContinueWith(UtilityAutomation.AutomationExceptionHandler, TaskContinuationOptions.OnlyOnFaulted);
            }
        }
    }
}
