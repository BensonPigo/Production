using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Prg;
using Sci.Win.Tools;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.IE
{
    /// <summary>
    /// IE_B01
    /// </summary>
    public partial class B01 : Win.Tems.Input1
    {
        /// <summary>
        /// B01
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public B01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = "FactoryID = '" + Env.User.Factory + "'";
        }

        /// <summary>
        /// OnFormLoaded()
        /// </summary>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            MyUtility.Tool.SetupCombox(this.cboStyleType, 2, 1, "N,New,R,Repeat");

            this.gridBase.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.gridBase)
                .Numeric("No", header: "No", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Text("CheckList", header: "Check List Base", width: Widths.AnsiChars(28), iseditingreadonly: true)
                .CheckBox("Sel", header: "Sel", width: Widths.AnsiChars(3), trueValue: 1, falseValue: 0, iseditable: true)
                ;

            DataGridViewGeneratorTextColumnSettings col_ResponseDep = new DataGridViewGeneratorTextColumnSettings();
            col_ResponseDep.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    DataRow dr = this.gridDetail.GetDataRow(e.RowIndex);
                    string sqlitem = "select DISTINCT Dept from Employee where dept <> '' order by Dept";
                    SelectItem2 item = new SelectItem2(sqlitem, "Dept", "10", dr["ResponseDep"].ToString());
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    dr["ResponseDep"] = item.GetSelectedString();
                    dr.EndEdit();
                }
            };

            col_ResponseDep.CellValidating += (s, e) =>
            {
                string da = MyUtility.Convert.GetString(e.FormattedValue);
                if (da.Length >= 200)
                {
                    MyUtility.Msg.WarningBox("Input exceeds the 200 character limit. Please shorten your input.");
                    return;
                }
            };

            DataGridViewGeneratorNumericColumnSettings col_LeadTime = new DataGridViewGeneratorNumericColumnSettings();

            col_LeadTime.CellValidating += (s, e) =>
            {
                int da = MyUtility.Convert.GetInt(e.FormattedValue);
                if (da >= 32767)
                {
                    MyUtility.Msg.WarningBox("Input exceeds the 32767 character limit. Please shorten your input.");
                    return;
                }
            };

            this.gridDetail.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.gridDetail)
                .CheckBox("Sel", header: "Sel", width: Widths.AnsiChars(3), trueValue: 1, falseValue: 0, iseditable: true)
                .Numeric("No", header: "No", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Text("CheckList", header: "Check List Base", width: Widths.AnsiChars(28), iseditingreadonly: true)
                .EditText("ResponseDep", header: "Response Dep.", width: Widths.AnsiChars(13), iseditingreadonly: true, settings: col_ResponseDep)
                .Numeric("LeadTime", header: "Lead Time", width: Widths.AnsiChars(8), iseditingreadonly: false, settings: col_LeadTime)
                ;
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();

            string sqlcmd = $@"
Select Sel = Cast(0 as bit)
, cb.ID
, cb.No
, cb.CheckList
From ChgOverCheckListBase cb with (nolock)
where cb.ID not in (select ChgOverCheckListBaseID from ChgOverCheckList_Detail with (nolock) where ID = '{this.CurrentMaintain["ID"]}') and cb.junk = 0
order by No
";
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out DataTable dtBase);
            if (!result)
            {
                this.ShowErr(result);
                this.gridBaseBs.DataSource = null;
                return;
            }

            this.gridBaseBs.DataSource = dtBase;

            sqlcmd = $@"
Select Sel = Cast(0 as bit)
, cd.*
, cb.CheckList
, cb.No
From ChgOverCheckList_Detail cd with (nolock)
inner join ChgOverCheckListBase cb with (nolock) on cd.ChgOverCheckListBaseID = cb.ID
where cd.ID = '{this.CurrentMaintain["ID"]}'
order by No
";
            result = DBProxy.Current.Select(null, sqlcmd, out DataTable dtDetail);
            if (!result)
            {
                this.ShowErr(result);
                this.gridDetailBs.DataSource = null;
                return;
            }

            this.gridDetailBs.DataSource = dtDetail;

            // 預設No欄位Asc排序
            this.gridBase.Sort(this.gridBase.Columns["No"], ListSortDirection.Ascending);
            this.gridDetail.Sort(this.gridDetail.Columns["No"], ListSortDirection.Ascending);
            this.GetDetailCount();
        }

        /// <summary>
        /// ClickEditAfter()
        /// </summary>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtCategory.ReadOnly = true;
            this.cboStyleType.ReadOnly = true;
        }

        /// <summary>
        /// ClickSaveBefore()
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ClickSaveBefore()
        {
            foreach (DataRow dr in ((DataTable)this.gridDetailBs.DataSource).Rows)
            {
                if (dr["ResponseDep"].ToString().Length >= 200)
                {
                    MyUtility.Msg.WarningBox("Input exceeds the 200 character limit. Please shorten your input.");
                    return false;
                }
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["Category"]))
            {
                MyUtility.Msg.WarningBox("< Category > can not be empty!");
                this.txtCategory.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["StyleType"]))
            {
                MyUtility.Msg.WarningBox("< Style Type > can not be empty!");
                this.cboStyleType.Focus();
                return false;
            }

            // 檢查<Category> + <Style Type> 是否已存在於DB
            if (this.IsDetailInserting)
            {
                List<SqlParameter> paras = new List<SqlParameter>()
                {
                    new SqlParameter("@Category", this.CurrentMaintain["Category"].ToString()),
                    new SqlParameter("@StyleType", this.CurrentMaintain["StyleType"].ToString()),
                };

                string sql = "select 1 from ChgOverCheckList WITH (NOLOCK) WHERE Category = @Category AND StyleType = @StyleType";

                if (MyUtility.Check.Seek(sql, paras))
                {
                    MyUtility.Msg.WarningBox("This <Category> + <Style Type> already exists!");
                    return false;
                }
            }

            // 檢查Detail<Response Dep.>和<Lead Time>不可空白
            var dtDetail = (DataTable)this.gridDetailBs.DataSource;
            if (dtDetail.Rows.Count > 0
                && dtDetail.AsEnumerable().Where(r => VFP.Empty(r["ResponseDep"])).Any())
            {
                MyUtility.Msg.WarningBox("Please enter <Response Dep.> on the right side of the table.");
                return false;
            }

            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override DualResult ClickSavePost()
        {
            var dtDetail = (DataTable)this.gridDetailBs.DataSource;

            if (this.IsDetailInserting)
            {
                dtDetail.AsEnumerable().ToList().ForEach(r => r["ID"] = this.CurrentMaintain["ID"]);
            }

            // 更新ChgOverCheckList_Detail
            using (TransactionScope transactionscope = new TransactionScope())
            {
                string updSql = $@"
DELETE FROM dbo.ChgOverCheckList_Detail
WHERE ID = @ID AND NOT EXISTS (
    SELECT 1
    FROM #tmp s
    WHERE dbo.ChgOverCheckList_Detail.ID = s.ID 
    AND dbo.ChgOverCheckList_Detail.ChgOverCheckListBaseID = s.ChgOverCheckListBaseID
);

UPDATE dbo.ChgOverCheckList_Detail
SET ResponseDep = IIF(dbo.ChgOverCheckList_Detail.ResponseDep != s.ResponseDep, s.ResponseDep, dbo.ChgOverCheckList_Detail.ResponseDep),
    LeadTime = IIF(dbo.ChgOverCheckList_Detail.LeadTime != s.LeadTime, s.LeadTime, dbo.ChgOverCheckList_Detail.LeadTime),
    EditName = IIF(dbo.ChgOverCheckList_Detail.ResponseDep != s.ResponseDep OR dbo.ChgOverCheckList_Detail.LeadTime != s.LeadTime, @UserID, dbo.ChgOverCheckList_Detail.EditName),
    EditDate = IIF(dbo.ChgOverCheckList_Detail.ResponseDep != s.ResponseDep OR dbo.ChgOverCheckList_Detail.LeadTime != s.LeadTime, GETDATE(), dbo.ChgOverCheckList_Detail.EditDate)
FROM dbo.ChgOverCheckList_Detail
JOIN #tmp s ON dbo.ChgOverCheckList_Detail.ID = s.ID 
    AND dbo.ChgOverCheckList_Detail.ChgOverCheckListBaseID = s.ChgOverCheckListBaseID;

INSERT INTO dbo.ChgOverCheckList_Detail (ID, ChgOverCheckListBaseID, ResponseDep, LeadTime, AddName, AddDate)
SELECT s.ID, s.ChgOverCheckListBaseID, s.ResponseDep, s.LeadTime, @UserID, GETDATE()
FROM #tmp s
WHERE NOT EXISTS (
    SELECT 1
    FROM dbo.ChgOverCheckList_Detail t
    WHERE t.ID = s.ID 
    AND t.ChgOverCheckListBaseID = s.ChgOverCheckListBaseID
);
";

                List<SqlParameter> paras = new List<SqlParameter>();
                paras.Add(new SqlParameter("@UserID", Env.User.UserID));
                paras.Add(new SqlParameter("@ID", this.CurrentMaintain["ID"]));

                DualResult upResult;
                if (!(upResult = MyUtility.Tool.ProcessWithDatatable(dtDetail, string.Empty, updSql, out DataTable dt, paramters: paras)))
                {
                    transactionscope.Dispose();
                    return upResult;
                }

                transactionscope.Complete();
            }

            return base.ClickSavePost();
        }

        /// <inheritdoc/>
        protected override void ClickSaveAfter()
        {
            base.ClickSaveAfter();

            // 更新ChgOver_Check
            var sqlUpdate = @"
declare @ChgOver table (ID bigint, FactoryID varchar(8), Inline datetime)

-- 找出符合以下條件的ChgOver
-- 1. Inline >= 今天，並日期大於2025-01-01
insert into @ChgOver (ID, FactoryID, Inline)
select co.ID, co.FactoryID, co.Inline
from ChgOver co with (nolock)
where co.Category = @Category
and co.Type = @StyleType
and co.FactoryID = @FactoryID
and co.Inline >= Convert(date, GETDATE())
and co.Inline >= '2025-01-01'

-- 刪除並重新寫入ChgOver_Check資料
if @@RowCount > 0
begin
	delete ChgOver_Check where ID in (select ID from @ChgOver)

	insert into ChgOver_Check (ID, ChgOverCheckListID, Deadline, No, LeadTime, ResponseDep)
	select co.ID, ck.ID, dbo.CalculateWorkDate(co.Inline, -ckd.LeadTime, co.FactoryID), cb.ID, ckd.LeadTime, ckd.ResponseDep
	from @ChgOver co
	inner join ChgOverCheckList ck with (nolock) on ck.Category = @Category and ck.StyleType = @StyleType and ck.FactoryID = co.FactoryID
	inner join ChgOverCheckList_Detail ckd with (nolock) on ck.ID = ckd.ID
	inner join ChgOverCheckListBase cb with (nolock) on ckd.ChgOverCheckListBaseID = cb.ID
end
";
            List<SqlParameter> listPar = new List<SqlParameter>()
            {
                new SqlParameter("@Category", this.CurrentMaintain["Category"]),
                new SqlParameter("@StyleType", this.CurrentMaintain["StyleType"]),
                new SqlParameter("@FactoryID", Env.User.Factory),
            };

            DualResult result = DBProxy.Current.Execute(null, sqlUpdate, listPar);
            if (!result)
            {
                this.ShowErr(result);
            }
        }

        private void BtnAdd_Click(object sender, System.EventArgs e)
        {
            var dtBase = (DataTable)this.gridBaseBs.DataSource;
            var selBase = dtBase.AsEnumerable().Where(r => !VFP.Empty(r["Sel"]));

            if (selBase.Count() > 0)
            {
                this.SuspendLayout(); // 暫停更新畫面，以避免閃爍

                var dtDetail = (DataTable)this.gridDetailBs.DataSource;
                foreach (DataRow dr in selBase)
                {
                    var newRow = dtDetail.NewRow();
                    newRow["Sel"] = false;
                    newRow["No"] = dr["No"];
                    newRow["CheckList"] = dr["CheckList"];
                    newRow["ResponseDep"] = string.Empty;
                    newRow["LeadTime"] = 0;
                    newRow["ID"] = this.CurrentMaintain["ID"];
                    newRow["ChgOverCheckListBaseID"] = dr["ID"];
                    dtDetail.Rows.Add(newRow);
                }

                selBase.ToList().ForEach(r => dtBase.Rows.Remove(r));

                this.ResumeLayout(false);   // 恢復更新畫面
                this.GetDetailCount();
            }
        }

        private void BtnDelete_Click(object sender, System.EventArgs e)
        {
            var dtDetail = (DataTable)this.gridDetailBs.DataSource;
            var selDetail = dtDetail.AsEnumerable().Where(r => !VFP.Empty(r["Sel"]));

            if (selDetail.Count() > 0)
            {
                this.SuspendLayout(); // 暫停更新畫面，以避免閃爍

                var dtBase = (DataTable)this.gridBaseBs.DataSource;
                foreach (DataRow dr in selDetail)
                {
                    var newRow = dtBase.NewRow();
                    newRow["Sel"] = false;
                    newRow["No"] = dr["No"];
                    newRow["CheckList"] = dr["CheckList"];
                    newRow["ID"] = dr["ChgOverCheckListBaseID"];
                    dtBase.Rows.Add(newRow);
                }

                selDetail.ToList().ForEach(r => dtDetail.Rows.Remove(r));

                this.ResumeLayout(false);   // 恢復更新畫面
                this.GetDetailCount();
            }
        }

        private void GetDetailCount()
        {
            this.labDetailCount1.Text = ((DataTable)this.gridBaseBs.DataSource).Rows.Count.ToString();
            this.labDetailCount2.Text = ((DataTable)this.gridDetailBs.DataSource).Rows.Count.ToString();
        }
    }
}
