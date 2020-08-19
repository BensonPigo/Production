using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Sci.Production.Tools
{
    /// <inheritdoc/>
    public partial class P01 : Sci.Win.Tems.Base
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="P01"/> class.
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public P01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        private static List<string> outputFromStoredProcedure_List = new List<string>();

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            this.EditMode = true;
            base.OnFormLoaded();
            this.grid.IsEditingReadOnly = false;

            #region -- 欄位設定 --
            this.Helper.Controls.Grid.Generator(this.grid)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
                .Text("SuppID", header: "Supp#", width: Widths.AnsiChars(15))
                .Text("AbbEN", header: "Supp Name", width: Widths.AnsiChars(5))
                .Text("APIThread", header: "API Thread", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("ErrorCode", header: "Error Code", width: Widths.AnsiChars(5),  iseditingreadonly: true)
                .Text("ErrorMsg", header: "Error Msg.", width: Widths.AnsiChars(40), iseditingreadonly: true)
                .Text("JSON", header: "Content", width: Widths.AnsiChars(40), iseditingreadonly: true)
                .DateTime("AddDate", header: "Error Time", width: Widths.AnsiChars(20),  iseditingreadonly: true)
            ;
            #endregion 欄位設定

            this.Search();
        }

        private void BtnFilter_Click(object sender, EventArgs e)
        {
            if (!MyUtility.Check.Empty(this.txtsupplier.TextBox1.Text))
            {
                this.listControlBindingSource1.Filter = $"SuppID='{this.txtsupplier.TextBox1.Text}' ";
            }
            else
            {
                this.listControlBindingSource1.Filter = string.Empty;
            }
        }

        private void BtnEditSave_Click(object sender, EventArgs e)
        {
            if (this.btnEditSave.Text == "Edit")
            {
                this.btnEditSave.Text = "Save";
                this.RunTime.ReadOnly = false;
                this.RunTime.IsSupportEditMode = true;
            }
            else
            {
                this.btnEditSave.Text = "Edit";
                this.RunTime.ReadOnly = true;
                this.RunTime.IsSupportEditMode = false;
                int automationAutoRunTime = (int)this.RunTime.Value.Value;

                string cmd = $@"
UPDATE System
SET AutomationAutoRunTime = {automationAutoRunTime}
";
                DualResult result = DBProxy.Current.Execute(null, cmd);

                if (!result)
                {
                    this.ShowErr(result);
                }
            }
        }

        private void BtnRun_Click(object sender, EventArgs e)
        {
            DataTable gridData = (DataTable)this.listControlBindingSource1.DataSource;
            outputFromStoredProcedure_List.Clear();

            DataRow[] selecteDatas = gridData.Select("Selected=1");

            if (selecteDatas.Length == 0)
            {
                MyUtility.Msg.InfoBox("No datas selected!!");
                return;
            }

            this.ShowWaitMessage("Processing...");

            DualResult result;

            // 正常執行但是Fail
            DataTable failDt = gridData.Clone();

            // 其他例外狀況(例如找不到這筆資料的Ukey)
            DataTable otherFailDt = gridData.Clone();

            // 成功狀況
            DataTable successDt = gridData.Clone();

            foreach (DataRow selecteData in selecteDatas)
            {
                string cmdText = $@"
declare @dd bit
exec dbo.SentJsonToAGV {(long)selecteData["Ukey"]},'{Sci.Env.User.UserID}', @dd OUTPUT
SELECT [Result]= @dd
";
                result = DBProxy.Current.Select(null, cmdText, out DataTable resultDt);

                if (!result)
                {
                    this.ShowErr(result);
                    return;
                }

                if (result)
                {
                    DataRow nRow = selecteData;
                    otherFailDt.ImportRow(nRow);
                }
                else if (!(bool)resultDt.Rows[0]["Result"])
                {
                    DataRow nRow = selecteData;
                    failDt.ImportRow(nRow);
                }
                else
                {
                    DataRow nRow = selecteData;
                    successDt.ImportRow(nRow);
                }
            }

            #region 統整最後結果

            if (otherFailDt.Rows.Count > 0)
            {
                MyUtility.Msg.ShowMsgGrid(otherFailDt, msg: "Can not find this resent data!!");
            }

            if (failDt.Rows.Count > 0)
            {
                MyUtility.Msg.ShowMsgGrid(failDt, msg: "Resent fail!!");
            }

            // 無失敗才跳成功訊息
            if (otherFailDt.Rows.Count == 0 && failDt.Rows.Count == 0 && successDt.Rows.Count > 0)
            {
                MyUtility.Msg.InfoBox("Success!!");
            }
            #endregion

            this.HideWaitMessage();

            this.Search();
        }

        private void Search()
        {
            string cmd = $@"
SELECT [Selected]=0
    , SuppID
	,s.AbbEN
	,a.APIThread
	,a.ErrorCode
	,a.ErrorMsg
	,a.JSON
	,a.AddDate
    ,a.Ukey
FROM AutomationErrMsg a WITH(NOLOCK)
LEFT JOIN Supp s WITH(NOLOCK) ON a.SuppID=s.ID
";

            DBProxy.Current.Select(null, cmd, out DataTable dt);
            this.listControlBindingSource1.DataSource = dt;
        }
    }
}
