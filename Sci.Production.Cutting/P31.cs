﻿using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Automation;
using Sci.Production.Prg;
using Sci.Production.PublicPrg;
using Sci.Win.Tems;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Sci.Production.Automation.Gensong_SpreadingSchedule;

namespace Sci.Production.Cutting
{
    /// <inheritdoc/>
    public partial class P31 : Win.Tems.Input6
    {
        /// <inheritdoc/>
        public P31(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.txtCell1.MDivisionID = Sci.Env.User.Keyword;
            this.detailgrid.RowPrePaint += this.Detailgrid_RowPrePaint;
            this.gridicon.Insert.Visible = false;
        }

        private void Detailgrid_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            Color backDefaultColor = this.detailgrid.DefaultCellStyle.BackColor;
            if (e.RowIndex < 0)
            {
                return;
            }

            DataRow dr = ((DataRowView)this.detailgrid.Rows[e.RowIndex].DataBoundItem).Row;
            var x = this.DetailDatas
                .Where(w => MyUtility.Convert.GetString(w["Completed"]) == "N"
                        && !MyUtility.Convert.GetBool(w["IsSuspend"]));
            int minseq = 0;
            if (x.Any())
            {
                minseq = x.Min(m => MyUtility.Convert.GetInt(m["SpreadingSchdlSeq"]));
            }

            if (dr["Completed"].ToString() == "Y")
            {
                this.detailgrid.Rows[e.RowIndex].Cells["Completed"].Style.ForeColor = Color.Red;
            }

            // Row變色規則，若該 Row 已經變色則跳過
            if (dr["IsOutStanding"].ToString() == "Y")
            {
                if (this.detailgrid.Rows[e.RowIndex].DefaultCellStyle.BackColor != ColorTranslator.FromHtml("#9D9D9D"))
                {
                    this.detailgrid.Rows[e.RowIndex].DefaultCellStyle.BackColor = ColorTranslator.FromHtml("#9D9D9D");
                }
            }
            else if (MyUtility.Convert.GetInt(dr["SpreadingSchdlSeq"]) == minseq && minseq > 0)
            {
                if (this.detailgrid.Rows[e.RowIndex].DefaultCellStyle.BackColor != ColorTranslator.FromHtml("#FFB5B5"))
                {
                    this.detailgrid.Rows[e.RowIndex].DefaultCellStyle.BackColor = ColorTranslator.FromHtml("#FFB5B5");
                }
            }
            else
            {
                if (this.detailgrid.Rows[e.RowIndex].DefaultCellStyle.BackColor != backDefaultColor)
                {
                    this.detailgrid.Rows[e.RowIndex].DefaultCellStyle.BackColor = backDefaultColor;
                }
            }

            if (dr["MtlStatusValue"].ToString() == "Y")
            {
                this.detailgrid.Rows[e.RowIndex].Cells["MaterialStatus"].Style.BackColor = Color.LightPink;
            }
            else if (dr["MtlStatusValue"].ToString() == "N")
            {
                this.detailgrid.Rows[e.RowIndex].Cells["MaterialStatus"].Style.BackColor = Color.LightGreen;
            }

        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["FactoryID"] = Sci.Env.User.Factory;
            ((DataTable)this.detailgridbs.DataSource).Clear();
            this.GridIconEnable();
        }

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.dateEstCut.ReadOnly = true;
            this.txtCell1.ReadOnly = true;
            this.GridIconEnable();
        }

        private void GridIconEnable()
        {
            DateTime? toDay = MyUtility.Convert.GetDate(MyUtility.GetValue.Lookup("select format(getdate(), 'yyyy/MM/dd')"));
            this.gridicon.Append.Enabled = !(MyUtility.Convert.GetDate(this.CurrentMaintain["EstCutDate"]) < toDay);
        }

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string ukey = (e.Master == null) ? "0" : e.Master["Ukey"].ToString();
            string factoryID = (e.Master == null) ? string.Empty : e.Master["FactoryID"].ToString();
            string estCutDate = (e.Master == null) ? string.Empty : ((DateTime)e.Master["EstCutDate"]).ToString("yyyy/MM/dd");
            string cutCellID = (e.Master == null) ? string.Empty : e.Master["CutCellID"].ToString();
            this.DetailSelectCommand = $@"
select *, [MtlStatusValue] = '' from dbo.GetSpreadingSchedule('{factoryID}','{estCutDate}','{cutCellID}',{ukey},'')";
            return base.OnDetailSelectCommandPrepare(e);
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();

            DataGridViewGeneratorNumericColumnSettings seq = new DataGridViewGeneratorNumericColumnSettings();
            seq.CellEditable += (s, e) =>
            {
                // Act. CutDate 有值不可編輯 SpreadingSchdlSeq
                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                e.IsEditable = dr["Completed"].ToString() == "N";
            };
            seq.CellValidating += (s, e) =>
            {
                if (this.EditMode == false)
                {
                    return;
                }

                // 相同 CutRef 的 SpreadingSchdlSeq 一樣
                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                foreach (DataRow cdr in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
                    .Where(w => w.RowState != DataRowState.Deleted
                        && MyUtility.Convert.GetString(w["CutRef"]) == MyUtility.Convert.GetString(dr["CutRef"])))
                {
                    cdr["SpreadingSchdlSeq"] = MyUtility.Check.Empty(e.FormattedValue) ? DBNull.Value : e.FormattedValue;
                    cdr.EndEdit();
                }

                this.SeqtoNull();
            };

            DataGridViewGeneratorTextColumnSettings suspend = new DataGridViewGeneratorTextColumnSettings();
            suspend.EditingMouseDoubleClick += (s, e) =>
            {
                if (this.EditMode == false || e.RowIndex == -1)
                {
                    return;
                }

                // 若[Completed]=Y，則不能再調整
                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                if (MyUtility.Convert.GetString(dr["Completed"]) == "Y")
                {
                    return;
                }

                if (e.Button == MouseButtons.Left)
                {
                    bool isSuspend = MyUtility.Convert.GetBool(dr["IsSuspend"]);
                    foreach (DataRow cdr in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
                        .Where(w => w.RowState != DataRowState.Deleted
                            && MyUtility.Convert.GetString(w["CutRef"]) == MyUtility.Convert.GetString(dr["CutRef"])))
                    {
                        cdr["IsSuspend"] = !isSuspend;
                        cdr["Suspend"] = !isSuspend ? "Y" : "N";
                        cdr.EndEdit();
                    }
                }
            };

            DataGridViewGeneratorTextColumnSettings cutRef = new DataGridViewGeneratorTextColumnSettings();
            cutRef.CellEditable += (s, e) =>
            {
                // Act. CutDate 有值不可編輯 SpreadingSchdlSeq
                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                e.IsEditable = MyUtility.Check.Empty(dr["Cutref"]);
            };
            cutRef.CellValidating += (s, e) =>
            {
                if (this.EditMode == false || MyUtility.Check.Empty(e.FormattedValue))
                {
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                string oldvalue = MyUtility.Convert.GetString(dr["CutRef"]);
                string newvalue = MyUtility.Convert.GetString(e.FormattedValue);
                if (oldvalue == newvalue)
                {
                    return;
                }

                if (this.DetailDatas.Where(w => MyUtility.Convert.GetString(w["CutRef"]) == newvalue).Any())
                {
                    MyUtility.Msg.WarningBox($"CutRef:{newvalue} already exists!");
                    dr["CutRef"] = string.Empty;
                    return;
                }

                if (this.CheckCutrefToDay(newvalue))
                {
                    MyUtility.Msg.WarningBox($"Cannot add in curtref# {newvalue}, the cutref# has exists today.");
                    dr["CutRef"] = string.Empty;
                    return;
                }

                string sqlcmd = $@"
select * from dbo.GetSpreadingSchedule('{this.displayFactory.Text}','','',0,'{e.FormattedValue}')";
                DualResult result = DBProxy.Current.Select(null, sqlcmd, out DataTable dt);
                if (!result)
                {
                    this.ShowErr(result);
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    MyUtility.Msg.WarningBox($"Data not found!");
                    dr["CutRef"] = string.Empty;
                    return;
                }

                dr.Delete();
                foreach (DataRow drc in dt.Rows)
                {
                    ((DataTable)this.detailgridbs.DataSource).ImportRowAdded(drc);
                }

                DataTable detDtb = (DataTable)this.detailgridbs.DataSource; // 要被搜尋的grid
                this.detailgridbs.Position = detDtb.Rows.IndexOf(detDtb.Select($"CutRef = '{newvalue}'")[0]);
            };
            this.Helper.Controls.Grid.Generator(this.detailgrid)
               .Numeric("SpreadingSchdlSeq", header: "SCHDL\r\nSeq", width: Widths.AnsiChars(3), settings: seq)
               .Text("Completed", header: "Completed", width: Widths.AnsiChars(3), iseditingreadonly: true)
               .Text("Suspend", header: "Suspend", width: Widths.AnsiChars(3), iseditingreadonly: true, settings: suspend)
               .Text("MaterialStatus", header: "Material\r\nStatus", width: Widths.AnsiChars(5), iseditingreadonly: true)
               .Text("CutRef", header: "CutRef#", width: Widths.AnsiChars(6), settings: cutRef)
               .Numeric("Cutno", header: "SEQ", width: Widths.AnsiChars(3), iseditingreadonly: true)
               .Text("Markername", header: "Maker\r\nName", width: Widths.AnsiChars(10), iseditingreadonly: true)
               .Text("FabricCombo", header: "Fabric\r\nCombo", width: Widths.AnsiChars(3), iseditingreadonly: true)
               .Text("FabricPanelCode", header: "Fab_Panel\r\nCode", width: Widths.AnsiChars(3), iseditingreadonly: true)
               .Text("Article", header: "Article", width: Widths.AnsiChars(8), iseditingreadonly: true)
               .Text("ColorID", header: "Color", width: Widths.AnsiChars(3), iseditingreadonly: true)
               .Text("multisize", header: "Size", width: Widths.AnsiChars(3), iseditingreadonly: true)
               .Numeric("Layer", header: "Layers", width: Widths.AnsiChars(3), iseditingreadonly: true)
               .Text("TotalCutQty", header: "Total\r\nCutQty", width: Widths.AnsiChars(12), iseditingreadonly: true)
               .Text("orderid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
               .Text("Seq1", header: "Seq1", width: Widths.AnsiChars(3), iseditingreadonly: true)
               .Text("Seq2", header: "Seq2", width: Widths.AnsiChars(3), iseditingreadonly: true)
               .Text("Refno", header: "Ref#", width: Widths.AnsiChars(12), iseditingreadonly: true)
               .Text("WeaveTypeID", header: "WeaveType", width: Widths.AnsiChars(15), iseditingreadonly: true)
               .Numeric("Cons", header: "Cons", width: Widths.AnsiChars(10), decimal_places: 4, iseditingreadonly: true)
               .Date("EstCutDate", header: "Est. Cut Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
               .Text("CutplanID", header: "CutPlan#", width: Widths.AnsiChars(14), iseditingreadonly: true)
               .Text("IssueID", header: "Issue ID", width: Widths.AnsiChars(14), iseditingreadonly: true)
               .Text("IsOutStanding", header: "Is OutStanding", width: Widths.AnsiChars(3), iseditingreadonly: true)
               ;
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.RefreshMaterialStatus();
        }

        /// <inheritdoc/>
        protected override void OnDetailGridDelete()
        {
            var cutRef = this.detailgrid.SelectedRows.Cast<DataGridViewRow>()
                          .Select(s => MyUtility.Convert.GetString(s.Cells["CutRef"].Value))
                          .Distinct()
                          .ToList();
            ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
                .Where(w => w.RowState != DataRowState.Deleted && cutRef.Contains(MyUtility.Convert.GetString(w["CutRef"])))
                .Delete();
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["CutCellID"]) ||
                MyUtility.Check.Empty(this.CurrentMaintain["EstCutDate"]))
            {
                MyUtility.Msg.WarningBox("<Cut Cell>and <Est Cut Date> cannot be empty.");
                return false;
            }

            ((DataTable)this.detailgridbs.DataSource).AsEnumerable().Where(w => w.RowState != DataRowState.Deleted && MyUtility.Check.Empty(w["Cutref"])).ToList().ForEach(f => f.Delete());

            if (this.DetailDatas.Count == 0)
            {
                MyUtility.Msg.WarningBox("The detail cannot be empty.");
                return false;
            }

            foreach (DataRow dr in this.DetailDatas)
            {
                if (this.CheckCutrefToDay(MyUtility.Convert.GetString(dr["CutRef"])))
                {
                    MyUtility.Msg.WarningBox($"Cannot add in curtref# {dr["CutRef"]}, the cutref# has exists today.");
                    return false;
                }
            }

            if (!this.DetailDatas.Any(s => !MyUtility.Check.Empty(s["SpreadingSchdlSeq"])))
            {
                MyUtility.Msg.WarningBox("Please enter at least one SCHDL Seq");
                return false;
            }

            this.SeqtoNull();
            var x = this.DetailDatas.Where(w => !MyUtility.Check.Empty(w["SpreadingSchdlSeq"]))
                .Select(s => new
                {
                    CutRef = MyUtility.Convert.GetString(s["CutRef"]),
                    SpreadingSchdlSeq = MyUtility.Convert.GetDecimal(s["SpreadingSchdlSeq"]),
                }).Distinct().GroupBy(g => g.SpreadingSchdlSeq).Select(s => new { s.Key, ct = s.Count() })
                .Where(w => w.ct > 1).ToList();
            if (x.Any())
            {
                MyUtility.Msg.WarningBox("<SCHDL Seq> cannot be same in different <CutRef#>.");
                return false;
            }

            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override DualResult ClickSave()
        {
            // 防止SpreadingSchdlSeq null寫進資料庫
            IList<DataRow> listDetail = this.DetailDatas;
            for (int i = 0; i < listDetail.Count; i++)
            {
                if (MyUtility.Check.Empty(listDetail[i]["SpreadingSchdlSeq"]))
                {
                    listDetail[i].Delete();
                }
            }

            return base.ClickSave();
        }

        /// <inheritdoc/>
        protected override DualResult ClickSavePost()
        {
            List<SqlParameter> listPar = new List<SqlParameter>();
            listPar.Add(new SqlParameter("@EstCutDate", this.CurrentMaintain["EstCutDate"]));
            listPar.Add(new SqlParameter("@FactoryID", this.CurrentMaintain["FactoryID"]));
            listPar.Add(new SqlParameter("@CutCellID", this.CurrentMaintain["CutCellID"]));

            // 1.檢查此次有存檔CutRef是否有存在未來的日期，這些將會刪除的單資訊先撈出，Call廠商API做整單刪除
            string sqlDeleteList = $@"
declare @today date = getdate()

select distinct ss.FactoryID, ss.EstCutDate, ss.CutCellID
from SpreadingSchedule ss
inner join SpreadingSchedule_Detail ssd on ss.Ukey = ssd.SpreadingScheduleUkey
where   ss.EstCutDate <> @EstCutDate and
		ss.EstCutDate > @today and
		ss.FactoryID = @FactoryID and
		ss.CutCellID = @CutCellID  and
        ssd.IsAGVArrived = 0 and
        ssd.CutRef in (select  sd.CutRef from	SpreadingSchedule s with(nolock)
						 inner join SpreadingSchedule_Detail sd with(nolock) on s.Ukey = sd.SpreadingScheduleUkey
						 where	s.EstCutDate = @EstCutDate and
                                s.FactoryID = @FactoryID and
                                s.CutCellID = @CutCellID
						)
";
            DualResult result = DBProxy.Current.Select(null, sqlDeleteList, listPar, out DataTable dt);
            if (!result)
            {
                return result;
            }

            foreach (DataRow dr in dt.Rows)
            {
                result = new Gensong_SpreadingSchedule().DeleteSpreadingSchedule(dr["FactoryID"].ToString(), (DateTime)dr["EstCutDate"], dr["CutCellID"].ToString());
                if (!result)
                {
                    return result;
                }
            }

            // 2.檢查此次有存檔CutRef是否有存在未來的日期，若有就刪除，已完成與這次維護的資料不刪除 (ISP20210219)
            string sqlDeleteSameFutureCutRef = $@"
declare @today date = getdate()

delete  ssd
from SpreadingSchedule ss
inner join SpreadingSchedule_Detail ssd on ss.Ukey = ssd.SpreadingScheduleUkey
where   ss.EstCutDate <> @EstCutDate and
		ss.EstCutDate > @today and
		ss.FactoryID = @FactoryID and
		ss.CutCellID = @CutCellID  and
        ssd.IsAGVArrived = 0 and
        ssd.CutRef in (select  sd.CutRef from	SpreadingSchedule s with(nolock)
						 inner join SpreadingSchedule_Detail sd with(nolock) on s.Ukey = sd.SpreadingScheduleUkey
						 where	s.EstCutDate = @EstCutDate and
                                s.FactoryID = @FactoryID and
                                s.CutCellID = @CutCellID
						)
";
            result = DBProxy.Current.Execute(null, sqlDeleteSameFutureCutRef, listPar);
            if (!result)
            {
                return result;
            }

            // 3.呼叫中間API再把這些單重新傳給廠商新增 (呼叫中間API會依據Key重撈資料)
            foreach (DataRow dr in dt.Rows)
            {
                result = new Gensong_SpreadingSchedule().SendSpreadingSchedule(dr["FactoryID"].ToString(), (DateTime)dr["EstCutDate"], dr["CutCellID"].ToString());
                if (!result)
                {
                    return result;
                }
            }

            // 4.呼叫中間API 傳送當前這張單
            result = new Gensong_SpreadingSchedule().SendSpreadingSchedule(this.CurrentMaintain["FactoryID"].ToString(), (DateTime)this.CurrentMaintain["EstCutDate"], this.CurrentMaintain["CutCellID"].ToString());
            if (!result)
            {
                return result;
            }

            return base.ClickSavePost();
        }

        /// <inheritdoc/>
        protected override DualResult ClickDeletePost()
        {
            DualResult result = new Gensong_SpreadingSchedule().DeleteSpreadingSchedule(this.CurrentMaintain["FactoryID"].ToString(), (DateTime)this.CurrentMaintain["EstCutDate"], this.CurrentMaintain["CutCellID"].ToString());
            if (!result)
            {
                return result;
            }

            return base.ClickDeletePost();
        }

        /// <inheritdoc/>
        protected override DualResult OnSaveDetail(IList<DataRow> details, ITableSchema detailtableschema)
        {
            DataView dv = this.DetailDatas.CopyToDataTable().DefaultView;
            DataTable dis = dv.ToTable(true, "SpreadingScheduleUkey", "CutRef", "SpreadingSchdlSeq", "IsAGVArrived", "IsSuspend");
            string sqlcmd = $@"
delete [SpreadingSchedule_Detail] where SpreadingScheduleUkey = {this.CurrentMaintain["Ukey"]}

INSERT INTO [dbo].[SpreadingSchedule_Detail]
    ([SpreadingScheduleUkey]
    ,[CutRef]
    ,[SpreadingSchdlSeq]
    ,[IsAGVArrived]
    ,[IsSuspend])
select
    [SpreadingScheduleUkey]
    ,[CutRef]
    ,[SpreadingSchdlSeq]
    ,isnull([IsAGVArrived], 0)
    ,isnull([IsSuspend], 0)
from #tmp
";
            return MyUtility.Tool.ProcessWithDatatable(dis, string.Empty, sqlcmd, out DataTable dt);
        }

        private bool CheckCutrefToDay(string cutref)
        {
            string sqlcmd = $@"
            declare @today date = getdate()
select 1
from SpreadingSchedule ss
inner join SpreadingSchedule_Detail ssd on ss.Ukey = ssd.SpreadingScheduleUkey
where ssd.CutRef = '{cutref}'
and ss.EstCutDate = @today
and ss.Ukey <> '{this.CurrentMaintain["Ukey"]}'
";
            return MyUtility.Check.Seek(sqlcmd);
        }

        private void SeqtoNull()
        {
            foreach (var dr in this.DetailDatas)
            {
                if (MyUtility.Convert.GetDecimal(dr["SpreadingSchdlSeq"]) == 0)
                {
                    dr["SpreadingSchdlSeq"] = DBNull.Value;
                }
            }
        }

        private void DateEstCut_Validating(object sender, CancelEventArgs e)
        {
            if (MyUtility.Check.Empty(this.dateEstCut.Value))
            {
                return;
            }

            if (this.dateEstCut.Value < DateTime.Today)
            {
                MyUtility.Msg.WarningBox("<Est. Cut Date> should not before today.");
                this.CurrentMaintain["EstCutDate"] = DBNull.Value;
                return;
            }
        }

        private void DateEstCut_Validated(object sender, EventArgs e)
        {
            this.GetWorkOrderData();
        }

        private void TxtCell1_Validated(object sender, EventArgs e)
        {
            this.GetWorkOrderData();
        }

        private void GetWorkOrderData()
        {
            ((DataTable)this.detailgridbs.DataSource).Clear();
            if (MyUtility.Check.Empty(this.dateEstCut.Value) || MyUtility.Check.Empty(this.txtCell1.Text))
            {
                return;
            }

            string sqlcmd = $@"
select 1
from SpreadingSchedule
where FactoryID = '{this.displayFactory.Text}' and EstCutDate = '{this.dateEstCut.Text}' and CutCellid = '{this.txtCell1.Text}'";
            if (MyUtility.Check.Seek(sqlcmd))
            {
                MyUtility.Msg.WarningBox($@"Already exists SpreadingSchedule.
FactoryID:{this.displayFactory.Text} CutCellid:{this.txtCell1.Text} EstCutDate:{this.dateEstCut.Text}");
                return;
            }

            sqlcmd = $@"
select * from dbo.GetSpreadingSchedule('{this.displayFactory.Text}','{this.dateEstCut.Text}','{this.txtCell1.Text}',0,'')";
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out DataTable dt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            foreach (DataRow dr in dt.Rows)
            {
                ((DataTable)this.detailgridbs.DataSource).ImportRowAdded(dr);
            }

            this.RefreshMaterialStatus();
        }

        private void BtnDefault_Click(object sender, EventArgs e)
        {
            if (this.DetailDatas.Count == 0)
            {
                MyUtility.Msg.WarningBox("No data found.");
                return;
            }

            var x = this.DetailDatas
                .Where(w => MyUtility.Convert.GetString(w["Completed"]) == "N")
                .OrderBy(o => MyUtility.Convert.GetDate(o["EstCutDate"]))
                .ThenBy(o => MyUtility.Convert.GetString(o["OrderID"]))
                .ThenBy(o => MyUtility.Convert.GetDate(o["BuyerDelivery"]))
                .ThenBy(o => MyUtility.Convert.GetString(o["FabricCombo"]))
                .ThenBy(o => MyUtility.Convert.GetDecimal(o["Cutno"]));

            int i = this.DetailDatas
                .Where(w => MyUtility.Convert.GetString(w["Completed"]) == "Y")
                .Select(s => MyUtility.Convert.GetInt(s["SpreadingSchdlSeq"]))
                .OrderByDescending(o => o)
                .FirstOrDefault() + 1;

            x.ToList().ForEach(f => f["SpreadingSchdlSeq"] = DBNull.Value);

            var distinctCutRef = x.Select(s => s["CutRef"].ToString()).Distinct();
            foreach (string cutRef in distinctCutRef)
            {
                if (i > 999)
                {
                    break;
                }

                var sameCutref = x.Where(w => MyUtility.Check.Empty(w["SpreadingSchdlSeq"])
                    && MyUtility.Convert.GetString(w["CutRef"]) == cutRef);
                foreach (DataRow cdr in sameCutref)
                {
                    cdr["SpreadingSchdlSeq"] = i;
                }

                i++;
            }
        }

        private void RefreshMaterialStatus()
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["EstCutDate"]))
            {
                return;
            }

            string factoryID = this.CurrentMaintain["FactoryID"].ToString();
            DateTime estCutDate = (DateTime)this.CurrentMaintain["EstCutDate"];
            string cutCellID = this.CurrentMaintain["CutCellID"].ToString();
            InventorySpreadingSchedule inventorySpreadingSchedule;
            DualResult result = new Gensong_SpreadingSchedule().GetInventory(factoryID, estCutDate, cutCellID, out inventorySpreadingSchedule);

            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            if (inventorySpreadingSchedule.Inventory.Count == 0)
            {
                return;
            }

            foreach (DataRow dr in this.DetailDatas)
            {
                var matchRow = inventorySpreadingSchedule.Inventory
                        .Where(s => s.Cutref == dr["CutRef"].ToString() &&
                                    s.SCIRefNo == dr["SCIRefNo"].ToString() &&
                                    s.ColorID == dr["Colorid"].ToString());

                if (!matchRow.Any())
                {
                    continue;
                }

                if (matchRow.First().Ttl_Qty < (decimal)dr["ReqQty"])
                {
                    dr["MtlStatusValue"] = "Y";
                }
                else
                {
                    dr["MtlStatusValue"] = "N";
                }
            }
        }

        private void BtnFind_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.txtSP.Text) &&
                MyUtility.Check.Empty(this.txtCutRef.Text))
            {
                return;
            }

            foreach (DataGridViewRow item in this.detailgrid.Rows)
            {
                string sp = MyUtility.Check.Empty(this.txtSP.Text) ? item.Cells["orderid"].Value.ToString() : this.txtSP.Text;
                string cutRef = MyUtility.Check.Empty(this.txtCutRef.Text) ? item.Cells["CutRef"].Value.ToString() : this.txtCutRef.Text;

                if (item.Cells["orderid"].Value.ToString() == sp && item.Cells["CutRef"].Value.ToString() == cutRef)
                {
                    this.detailgrid.SelectRowTo(item.Index);
                    return;
                }
            }
        }

        private void BtnReviseSchedule_Click(object sender, EventArgs e)
        {

        }
    }
}
