using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Tools
{
    /// <inheritdoc/>
    public partial class P03 : Sci.Win.Tems.Base
    {
        /// <inheritdoc/>
        public P03(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnQuery_Click(object sender, EventArgs e)
        {
            this.Search();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            this.EditMode = true;
            base.OnFormLoaded();
            this.grid.IsEditingReadOnly = false;

            #region 表身欄位設定
            this.Helper.Controls.Grid.Generator(this.grid)
                .CheckBox("select", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
                .Text("Ukey", header: "ID", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("SuppID", header: "Supp", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("ModuleName", header: "Module Name", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("APIThread", header: "API Thread", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("SuppAPIThread", header: "Supp API Thread", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .EditText("ErrorMsg", header: "Error Msg", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .EditText("Json", header: "JSON", width: Widths.AnsiChars(30), iseditingreadonly: true)
                .DateTime("AddDate", header: "Create Time", width: Widths.AnsiChars(18), iseditingreadonly: true)
                .CheckBox("ReSented", header: "ReSent", width: Widths.AnsiChars(5), iseditable: false, trueValue: 1, falseValue: 0)
                .DateTime("EditDate", header: "ReSent Time", width: Widths.AnsiChars(18), iseditingreadonly: true)
                ;
            #endregion

            // 取消大寫 (整個Grid)
            Ict.WinEnv.DefaultDataGridViewTextBoxCharacterCasing = System.Windows.Forms.CharacterCasing.Normal;

            this.Search();
        }

        private void Search()
        {
            #region where條件
            string strWhere = string.Empty;
            if (!MyUtility.Check.Empty(this.txtsupplier.TextBox1.Text))
            {
                strWhere += $" and IIF( isnull(b.SuppID,'') = '', a.SuppID, b.SuppID) = '{this.txtsupplier.TextBox1.Text}' " + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(this.txtJSONContains.Text))
            {
                strWhere += $" and a.JSON like '%{this.txtJSONContains.Text}%'" + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(this.dateCreateTime.Value1) && !MyUtility.Check.Empty(this.dateCreateTime.Value2))
            {
                strWhere += $@" and CONVERT(date,a.AddDate) between '{((DateTime)this.dateCreateTime.Value1).ToString("yyyy/MM/dd")}' and '{((DateTime)this.dateCreateTime.Value2).ToString("yyyy/MM/dd")}' 
    " + Environment.NewLine;
            }
            else if (!MyUtility.Check.Empty(this.dateCreateTime.Value1))
            {
                strWhere += $@"and CONVERT(date,a.AddDate) = '{((DateTime)this.dateCreateTime.Value1).ToString("yyyy/MM/dd")}' " + Environment.NewLine;
            }
            else if (!MyUtility.Check.Empty(this.dateCreateTime.Value2))
            {
                strWhere += $@"and CONVERT(date,a.AddDate) = '{((DateTime)this.dateCreateTime.Value2).ToString("yyyy/MM/dd")}' " + Environment.NewLine;
            }

            if (this.checkNotResentYet.Checked)
            {
                strWhere += $" and a.ReSented = 0";
            }
            #endregion

            if (MyUtility.Check.Empty(strWhere))
            {
                strWhere = " and 1=0";
            }

            string sqlcmd = $@"
select 
[select] = 0
,a.Ukey
,SuppID = IIF( isnull(b.SuppID,'') = '', a.SuppID, b.SuppID)
,ModuleName = IIF( isnull(b.ModuleName,'') = '', a.ModuleName, b.ModuleName)
,APIThread
,a.SuppAPIThread
,ErrorMsg
,JSON
,AddDate
,ReSented
,EditDate
from AutomationErrMsg a
left join AutomationDisplay b on a.SuppAPIThread = b.SuppAPIThread
where 1=1
 {strWhere}

order by Ukey
";

            DualResult result = DBProxy.Current.Select(null, sqlcmd, out DataTable dt);
            if (result == false)
            {
                this.ShowErr(result.ToString());
            }

            this.listControlBindingSource1.DataSource = dt;
        }

        private void BtnResentByManual_Click(object sender, EventArgs e)
        {
            DataTable gridData = (DataTable)this.listControlBindingSource1.DataSource;
            DataRow[] selecteDatas = gridData.Select("select=1");
            if (selecteDatas.Length == 0)
            {
                MyUtility.Msg.InfoBox("No datas selected!!");
                return;
            }

            this.ShowWaitMessage("Processing...");

            DualResult result;

            // 正常執行但是Fail
            DataTable failDt = gridData.Clone();

            // 成功狀況
            DataTable successDt = gridData.Clone();

            foreach (DataRow dr in selecteDatas)
            {
                string cmdText = $@"
declare @result nvarchar(max)
exec dbo.SentJsonFromAutomationErrMsg {(long)dr["Ukey"]},'{Sci.Env.User.UserID}', @result OUTPUT, 1200
SELECT [Result]= @result

insert into FPS.dbo.AutomationTransRecord(
    [CallFrom]     
    ,Activity       
    ,SuppID         
    ,ModuleName     
    ,SuppAPIThread  
    ,JSON           
    ,TransJson      
    ,AddName        
    ,AddDate
)
select
	'Resent'
	,a.Ukey  
	,SuppID = IIF( isnull(b.SuppID,'') = '', a.SuppID, b.SuppID)
	,ModuleName = IIF( isnull(b.ModuleName,'') = '', a.ModuleName, b.ModuleName)
	,a.SuppAPIThread
	,JSON
	,JSON
	,AddName
	,AddDate
from dbo.AutomationErrMsg a with (nolock) 
left join AutomationDisplay b with (nolock) on a.SuppAPIThread = b.SuppAPIThread
where ukey = '{(long)dr["Ukey"]}'
";
                result = DBProxy.Current.Select(null, cmdText, out DataTable resultDt);
                if (!result)
                {
                    this.ShowErr(result);
                    return;
                }

                DataRow nRow = dr;
                if (!MyUtility.Check.Empty(resultDt.Rows[0]["Result"]))
                {
                    failDt.ImportRow(nRow);
                }
                else
                {
                    successDt.ImportRow(nRow);
                }
            }

            #region 統整最後結果

            if (failDt.Rows.Count > 0)
            {
                MyUtility.Msg.ShowMsgGrid(failDt, msg: "Resent fail!!");
            }

            // 無失敗才跳成功訊息
            if (failDt.Rows.Count == 0 && successDt.Rows.Count > 0)
            {
                MyUtility.Msg.InfoBox("Success!!");
            }
            #endregion

            this.HideWaitMessage();

            this.Search();
        }
    }
}
