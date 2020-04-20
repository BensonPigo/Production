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
    public partial class P01 : Sci.Win.Tems.Base
    {
        public P01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }

        private static List<string> outputFromStoredProcedure_List = new List<string>();

        protected override void OnFormLoaded()
        {
            this.EditMode = true;
            base.OnFormLoaded();


            this.grid.IsEditingReadOnly = false;

            #region -- 欄位設定 --
            Helper.Controls.Grid.Generator(this.grid)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
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

        private void btnFilter_Click(object sender, EventArgs e)
        {
            if (!MyUtility.Check.Empty(this.txtsupplier.TextBox1.Text))
            {
                this.listControlBindingSource1.Filter = $"SuppID='{this.txtsupplier.TextBox1.Text}' ";
            }
            else
            {
                this.listControlBindingSource1.Filter = "";
            }
        }

        private void btnEditSave_Click(object sender, EventArgs e)
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

                int AutomationAutoRunTime = (int)this.RunTime.Value.Value;

                string cmd = $@"
UPDATE System
SET AutomationAutoRunTime = {AutomationAutoRunTime}
";
                DualResult Result = DBProxy.Current.Execute(null, cmd);

                if (!Result)
                {
                    this.ShowErr(Result);
                }
            }
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            DataTable gridData = (DataTable)this.listControlBindingSource1.DataSource;
            outputFromStoredProcedure_List.Clear();

            DataRow[] selecteDatas = gridData.Select("Selected=1");

            if (selecteDatas.Length == 0)
            {
                MyUtility.Msg.InfoBox("No datas selected!!");
                return;
            }

            //////////
            ///
            this.ShowWaitMessage("Processing...");

            DualResult result;

            // 正常執行但是Fail
            DataTable FailDt = gridData.Clone();

            // 其他例外狀況(例如找不到這筆資料的Ukey)
            DataTable otherFailDt = gridData.Clone();

            // 成功狀況
            DataTable successDt = gridData.Clone();

            foreach (DataRow selecteData in selecteDatas)
            {

                DataTable resultDt;
                string cmdText = $@"
declare @dd bit
exec dbo.SentJsonToAGV {(Int64)selecteData["Ukey"]},'{Sci.Env.User.UserID}', @dd OUTPUT
SELECT [Result]= @dd
";
                result = DBProxy.Current.Select(null, cmdText, out resultDt);

                if (!result)
                {
                    DataRow nRow = selecteData;
                    otherFailDt.ImportRow(nRow);
                }
                else if (!(bool)resultDt.Rows[0]["Result"])
                {
                    DataRow nRow = selecteData;
                    FailDt.ImportRow(nRow);
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


            if (FailDt.Rows.Count > 0)
            {
                MyUtility.Msg.ShowMsgGrid(FailDt, msg: "Resent fail!!");
            }

            // 無失敗才跳成功訊息
            if (otherFailDt.Rows.Count == 0 && FailDt.Rows.Count == 0 && successDt.Rows.Count > 0)
            {
                MyUtility.Msg.InfoBox("Success!!");
            }

            #endregion



            this.HideWaitMessage();

            this.Search();
        }

        

        private void Search()
        {

            DataTable dt;
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

            DBProxy.Current.Select(null, cmd, out dt);

            this.listControlBindingSource1.DataSource = dt;

        }
    }
}
