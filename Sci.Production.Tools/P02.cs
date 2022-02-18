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
    public partial class P02 : Sci.Win.Tems.Base
    {
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_Select;

        /// <inheritdoc/>
        public P02(ToolStripMenuItem menuitem)
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
                .CheckBox("select", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_Select)
                .Text("Ukey", header: "ID", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("CallFrom", header: "Call From", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("SuppID", header: "Supp ID", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("ModuleName", header: "Module Name", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("SuppAPIThread", header: "Supp API Thread", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .DateTime("AddDate", header: "Create Time", width: Widths.AnsiChars(18), iseditingreadonly: true)
                .EditText("oriJson", header: "JSON", width: Widths.AnsiChars(30), iseditingreadonly: true)
                .EditText("oriTransJSON", header: "Trans JSON", width: Widths.AnsiChars(30), iseditingreadonly: true)
                .Text("TransferResult", header: "Transfer Result", width: Widths.AnsiChars(7), iseditingreadonly: true)
                .EditText("Msg", header: "Msg", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("ErrorType", header: "Error Type", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .CheckBox("Resent", header: "Resent", width: Widths.AnsiChars(5), iseditable: true, trueValue: 1, falseValue: 0)
                .DateTime("ResentTime", header: "Resent Time", width: Widths.AnsiChars(18), iseditingreadonly: true)
                ;
            #endregion

            // 設定自動換行
            this.grid.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            // 設定自動調整高度
            this.grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            this.Search();
            this.ChangeEditable();
        }

        private void ChangeEditable()
        {
            this.col_Select.CellEditable += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.grid.GetDataRow(e.RowIndex);
                if (dr["ErrorType_Chk"].ToString() == "Error Msg" && MyUtility.Check.Empty(dr["Resent"]))
                {
                    e.IsEditable = true;
                }
                else
                {
                    e.IsEditable = false;
                }
            };
        }

        private void Search()
        {
            #region where條件
            string strWhere = string.Empty;
            if (!MyUtility.Check.Empty(this.txtsupplier.TextBox1.Text))
            {
                strWhere += $" and em.SuppID = '{this.txtsupplier.TextBox1.Text}' " + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(this.txtJSONContains.Text))
            {
                strWhere += $" and em.JSON like '%{this.txtJSONContains.Text}%'" + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(this.txtCallFrom.Text))
            {
                strWhere += $" and t.CallFrom like '%{this.txtCallFrom.Text}%'" + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(this.dateCreateTime.Value1) && !MyUtility.Check.Empty(this.dateCreateTime.Value2))
            {
                strWhere += $@" and CONVERT(date,em.AddDate) between '{((DateTime)this.dateCreateTime.Value1).ToString("yyyy/MM/dd")}' and '{((DateTime)this.dateCreateTime.Value2).ToString("yyyy/MM/dd")}' 
    " + Environment.NewLine;
            }
            else if (!MyUtility.Check.Empty(this.dateCreateTime.Value1))
            {
                strWhere += $@"and CONVERT(date,em.AddDate) = '{((DateTime)this.dateCreateTime.Value1).ToString("yyyy/MM/dd")}' " + Environment.NewLine;
            }
            else if (!MyUtility.Check.Empty(this.dateCreateTime.Value2))
            {
                strWhere += $@"and CONVERT(date,em.AddDate) = '{((DateTime)this.dateCreateTime.Value2).ToString("yyyy/MM/dd")}' " + Environment.NewLine;
            }
            #endregion

            if (MyUtility.Check.Empty(strWhere))
            {
                strWhere = " and 1=0";
            }

            string sqlcmd = $@"

select * from (
-- Resent
select 
[select] = 0
,t.Ukey,t.CallFrom,t.SuppID,t.ModuleName,t.SuppAPIThread,t.AddDate
,[JSON] = LEFT(t.JSON,100) + '...'
,[oriJson] = t.JSON
,[TransJSON] = LEFT(t.TransJSON,100) + '...'
,[oriTransJSON] = t.TransJSON
,[TransferResult] = IIF(em.Ukey is null and cm.Ukey is null , 'Success','Fail')
,[Msg] = isnull(em.ErrorMsg,'') 
,[ErrorType] = IIF(em.Ukey != '', 'Error Msg' + CHAR(13) + CHAR(10) + convert(varchar(20), isnull(em.Ukey,'')) , ' Check Msg' + CHAR(13) + CHAR(10) + convert(varchar(20),isnull(cm.Ukey,'')))
,[ErrorType_Chk] = IIF(em.Ukey != '', 'Error Msg' , ' Check Msg')
,[Resent] = em.ReSented
,[ResentTime] = em.EditDate
from fps.dbo.AutomationTransRecord t
inner join Production.dbo.AutomationErrMsg em on convert(varchar(80), em.Ukey) = t.Activity
left join Production.dbo.AutomationCheckMsg cm on cm.AutomationTransRecordUkey = t.Ukey
where 1=1
and t.CallFrom = 'Resent'
 {strWhere}

 union all

-- !Resent 
select 
[select] = 0
,t.Ukey,t.CallFrom,t.SuppID,t.ModuleName,t.SuppAPIThread,t.AddDate
,[JSON] = LEFT(t.JSON,100) + '...'
,[oriJson] = t.JSON
,[TransJSON] = LEFT(t.TransJSON,100) + '...'
,[oriTransJSON] = t.TransJSON
,[TransferResult] = IIF(em.Ukey is null and cm.Ukey is null , 'Success','Fail')
,[Msg] = iif( isnull(em.ErrorMsg,'') = '', isnull(cm.ErrorMsg,''), isnull(em.ErrorMsg,'')) 
,[ErrorType] = IIF(em.Ukey != '', 'Error Msg' + CHAR(13) + CHAR(10) + convert(varchar(20), isnull(em.Ukey,'')) , ' Check Msg' + CHAR(13) + CHAR(10) + convert(varchar(20),isnull(cm.Ukey,'')))
,[ErrorType_Chk] = IIF(em.Ukey != '', 'Error Msg' , ' Check Msg')
,[Resent] =  em.ReSented
,[ResentTime] = em.EditDate
from fps.dbo.AutomationTransRecord t
inner join Production.dbo.AutomationErrMsg em on em.AutomationTransRecordUkey = t.Ukey
left join Production.dbo.AutomationCheckMsg cm on cm.AutomationTransRecordUkey = t.Ukey
where 1=1
and t.CallFrom != 'Resent'
{strWhere}
) a
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
