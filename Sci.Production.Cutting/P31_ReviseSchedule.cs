using Ict;
using Ict.Win;
using Microsoft.Office.Tools.Excel;
using Sci.Data;
using Sci.Production.Automation;
using Sci.Production.Class.Commons;
using Sci.Production.Prg;
using System;
using System.Data;
using System.Linq;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Cutting
{
    /// <inheritdoc/>
    public partial class P31_ReviseSchedule : Win.Tems.QueryForm
    {
        private DataRow CurrentMaintain;
        private DataTable dtDetail;

        /// <inheritdoc/>
        public P31_ReviseSchedule(DataRow currentMaintain, DataTable dtDetail)
        {
            this.InitializeComponent();
            this.CurrentMaintain = currentMaintain;
            this.dtDetail = dtDetail.Select("Completed = 'N'").TryCopyToDataTable(dtDetail); // 只有未完成的才能轉移
            this.displayFactory.Text = MyUtility.Convert.GetString(currentMaintain["FactoryID"]);
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.GridSetup();
            this.dtDetail.Columns.Add("Selected", typeof(bool));
            foreach (DataRow dr in this.dtDetail.Rows)
            {
                dr["Selected"] = false;
            }

            this.listControlBindingSource1.DataSource = this.dtDetail;
        }

        private void GridSetup()
        {
            this.grid1.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.grid1)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(2), iseditable: true, trueValue: 1, falseValue: 0)
                .Numeric("SpreadingSchdlSeq", header: "SCHDL\r\nSeq", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Text("Completed", header: "Completed", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Text("Suspend", header: "Suspend", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Text("MaterialStatus", header: "Material\r\nStatus", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("CutRef", header: "CutRef#", width: Widths.AnsiChars(6), iseditingreadonly: true)
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

        private void BtnRevise_Click(object sender, EventArgs e)
        {
            this.grid1.ValidateControl();
            if (MyUtility.Check.Empty(this.dateEstCut.Value) || MyUtility.Check.Empty(this.txtCell1.Text))
            {
                MyUtility.Msg.WarningBox("<Est. Cut Date>, <Cut Cell> cannot empty.");
                return;
            }

            DataRow[] drs = this.dtDetail.Select("Selected = 1");
            if (drs.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select Datas");
                return;
            }

            if (MyUtility.Convert.GetDate(this.CurrentMaintain["EstCutDate"]) == this.dateEstCut.Value && MyUtility.Convert.GetString(this.CurrentMaintain["CutCellID"]).EqualString(this.txtCell1.Text))
            {
                MyUtility.Msg.WarningBox(" [Est Cut Date] and [Cut Cell] are consistent with the current status and cannot be revised.");
                return;
            }

            /*
               例子: 原本P31操作,兩張單都(大於 操作Today) 12/05, 12/06, 將 12/06(原單) 部分轉移到 12/05(新單)
               1.在 12/05(新單) 編輯新增一筆輸入原本在 12/06 的 Cutref
               2.按 Default 按鈕重新編碼(新單) SpreadingSchdlSeq
               3.按存檔在 savepost 階段 <兩張單都存在將轉移的 Cutref>
               3-1 先傳送原單的表頭資訊給廠商 (使用DeleteSpreadingSchedule)
               3-2 刪除 DB 中原單表身轉移的 Cutref
               3-3 把原單的表頭資訊再傳一次給廠商 (使用 SendSpreadingSchedule)
               4.傳送轉移的 Cutref 新單表頭資訊 (使用SendSpreadingSchedule)
            -------------------------------------------------------------------
               參照以上,將勾選的單轉移
               1.確認目標單是否存在
               2-1.存在則             複製過去, 然後走上面流程
               2-2.不存在則新建一筆單,複製過去, 然後走上面流程
            */

            DataTable dt = drs.TryCopyToDataTable(this.dtDetail);

            string factroyNew = this.displayFactory.Text;
            string estCutDateNew = this.dateEstCut.Value.Value.ToString("yyyy/MM/dd");
            string cellNew = this.txtCell1.Text;

            // 找目標的單
            string sqlcmd = $@"
SELECT Ukey
FROM SpreadingSchedule WITH (NOLOCK)
WHERE FactoryID = '{factroyNew}'
AND EstCutDate = '{estCutDateNew}'
AND CutCellID = '{cellNew}'
";
            DualResult result;
            long ukeyNew = 0;
            Exception errMsg = null;
            using (TransactionScope transactionscope = new TransactionScope(TransactionScopeOption.Required, new TimeSpan(0, 15, 0)))
            {
                try
                {
                    // 1.準備新單的 Ukey
                    string sqlUpdateSpreadingSchedule = string.Empty;
                    if (MyUtility.Check.Seek(sqlcmd, out DataRow dr))
                    {
                        ukeyNew = MyUtility.Convert.GetLong(dr["Ukey"]);
                        sqlUpdateSpreadingSchedule = $@"
UPDATE SpreadingSchedule
SET EditName = '{Sci.Env.User.UserID}'
   ,EditDate = GETDATE()
WHERE Ukey = {ukeyNew}";
                    }
                    else
                    {
                        // 新建一張單, 並吐出 Ukey
                        sqlcmd = $@"
INSERT INTO SpreadingSchedule (FactoryID, EstCutDate, CutCellID, AddDate, AddName)
OUTPUT INSERTED.Ukey
VALUES ('{this.displayFactory.Text}', '{this.dateEstCut.Value.Value:yyyy/MM/dd}', '{this.txtCell1.Text}', GETDATE(), '{Sci.Env.User.UserID}')
";
                        if (!(result = DBProxy.Current.Select(null, sqlcmd, out DataTable dtNewUkey)))
                        {
                            throw result.GetException();
                        }

                        ukeyNew = MyUtility.Convert.GetLong(dtNewUkey.Rows[0]["Ukey"]);
                    }

                    // 2.把勾選的表身複製過去, 並取出重編碼
                    sqlcmd = $@"
INSERT INTO SpreadingSchedule_Detail (SpreadingScheduleUkey, CutRef, SpreadingSchdlSeq, IsAGVArrived, IsSuspend)
SELECT {ukeyNew}, CutRef, 0, IsAGVArrived, IsSuspend
FROM #tmp

{sqlUpdateSpreadingSchedule}

SELECT * FROM dbo.GetSpreadingSchedule('{factroyNew}','{estCutDateNew}','{cellNew}',{ukeyNew},'')
ORDER BY OrderID ,Cutno ,SpreadingSchdlSeq
";
                    if (!(result = MyUtility.Tool.ProcessWithDatatable(dt, "CutRef,IsAGVArrived,IsSuspend", sqlcmd, out DataTable dtNew)))
                    {
                        throw result.GetException();
                    }

                    // 3.重編新單的 SpreadingSchdlSeq
                    Cutting.SpreadingSchdlSeq(dtNew.AsEnumerable());

                    // 4.更新新單的 SpreadingSchdlSeq
                    sqlcmd = $@"
UPDATE ssd
SET SpreadingSchdlSeq = #tmp.SpreadingSchdlSeq
FROM SpreadingSchedule_Detail ssd WITH (NOLOCK)
INNER JOIN #tmp ON #tmp.SpreadingScheduleUkey = ssd.SpreadingScheduleUkey AND #tmp.CutRef = ssd.CutRef
";
                    if (!(result = MyUtility.Tool.ProcessWithDatatable(dtNew, "SpreadingScheduleUkey,CutRef,SpreadingSchdlSeq", sqlcmd, out DataTable _)))
                    {
                        throw result.GetException();
                    }

                    if (!(result = Cutting.P31SavePost(ukeyNew, factroyNew, this.dateEstCut.Value.Value, cellNew)))
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

            if (errMsg != null)
            {
                this.ShowErr(errMsg);
                return;
            }

            MyUtility.Msg.InfoBox("Success!");
            this.Close();
        }

        private void DateEstCut_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MyUtility.Check.Empty(this.dateEstCut.Value))
            {
                return;
            }

            if (this.dateEstCut.Value < DateTime.Today.AddDays(1))
            {
                e.Cancel = true;
                MyUtility.Msg.WarningBox("<Est. Cut Date> must be selected after tomorrow");
            }
        }
    }
}
