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
        private Ict.Win.UI.DataGridViewTextBoxColumn col_ErrType;
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_Resent;

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
            this.dateTimePicker1.CustomFormat = "yyyy/MM/dd HH:mm:ss";
            this.dateTimePicker2.CustomFormat = "yyyy/MM/dd HH:mm:ss";

            this.dateTimePicker1.Value = DateTime.Now.AddHours(-1);
            this.dateTimePicker2.Value = DateTime.Now;

            DataGridViewGeneratorTextColumnSettings col_Json = new DataGridViewGeneratorTextColumnSettings();
            col_Json.EditingMouseDoubleClick += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    DataRow dr = this.grid.GetDataRow(e.RowIndex);
                    Win.Tools.EditMemo callNextForm = new Win.Tools.EditMemo(dr["oriJson"].ToString(), "Full JSON", false, null);
                    callNextForm.ShowDialog(this);
                }
            };

            DataGridViewGeneratorTextColumnSettings col_TransJSON = new DataGridViewGeneratorTextColumnSettings();
            col_TransJSON.EditingMouseDoubleClick += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    DataRow dr = this.grid.GetDataRow(e.RowIndex);
                    Win.Tools.EditMemo callNextForm = new Win.Tools.EditMemo(dr["oriTransJSON"].ToString(), "Full Trans JSON", false, null);
                    callNextForm.ShowDialog(this);
                }
            };

            #region 表身欄位設定
            this.Helper.Controls.Grid.Generator(this.grid)
                .CheckBox("select", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_Select)
                .Text("Ukey", header: "ID", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("CallFrom", header: "Call From", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("Activity", header: "Activity", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("SuppID", header: "Supp ID", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("ModuleName", header: "Module Name", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("SuppAPIThread", header: "Supp API Thread", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .DateTime("AddDate", header: "Create Time", width: Widths.AnsiChars(18), iseditingreadonly: true)
                .Text("Json", header: "JSON", width: Widths.AnsiChars(30), iseditingreadonly: true, settings: col_Json)
                .Text("TransJSON", header: "Trans JSON", width: Widths.AnsiChars(30), iseditingreadonly: true, settings: col_TransJSON)
                .Text("TransferResult", header: "Transfer Result", width: Widths.AnsiChars(7), iseditingreadonly: true)
                .EditText("Msg", header: "Msg", width: Widths.AnsiChars(35), iseditingreadonly: true)
                .Text("ErrorType", header: "Error Type", width: Widths.AnsiChars(8), iseditingreadonly: true).Get(out this.col_ErrType)
                .CheckBox("Resent", header: "Resent", trueValue: 1, falseValue: 0, iseditable: true).Get(out this.col_Resent)
                .DateTime("ResentTime", header: "Resent Time", width: Widths.AnsiChars(18), iseditingreadonly: true)
                ;
            #endregion

            // 設定自動換行
            this.col_ErrType.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            // 設定自動調整高度
            this.col_ErrType.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            this.grid.RowTemplate.Height = 43;

            // 取消大寫 (整個Grid)
            Ict.WinEnv.DefaultDataGridViewTextBoxCharacterCasing = System.Windows.Forms.CharacterCasing.Normal;

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

            this.col_Resent.CellEditable += (s, e) =>
             {
                 if (e.RowIndex == -1)
                 {
                     return;
                 }

                 e.IsEditable = false;
             };
        }

        private void ChangeDateTimepickCheck(DateTimePicker dateTimePicker)
        {
            if (dateTimePicker.Checked)
            {
                dateTimePicker.CustomFormat = "yyyy/MM/dd HH:mm:ss";
            }
            else
            {
                dateTimePicker.CustomFormat = " ";
            }
        }

        private void Search()
        {
            DBProxy.Current.DefaultTimeout = 900;  // timeout時間改為15分鐘
            #region where條件
            string strWhere = string.Empty;
            if (!MyUtility.Check.Empty(this.txtsupplier.TextBox1.Text))
            {
                strWhere += $" and t.SuppID = '{this.txtsupplier.TextBox1.Text}' " + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(this.txtJSONContains.Text))
            {
                strWhere += $" and t.JSON like '%{this.txtJSONContains.Text}%'" + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(this.txtCallFrom.Text))
            {
                strWhere += $" and t.CallFrom like '%{this.txtCallFrom.Text}%'" + Environment.NewLine;
            }

            strWhere += $@"
and t.AddDate between '{this.dateTimePicker1.Text}' and '{this.dateTimePicker2.Text}'";

            #endregion

            if (MyUtility.Check.Empty(strWhere))
            {
                strWhere = " and 1=0";
            }

            string sqlcmd = $@"

select 
[select] = 0
,t.Ukey,t.CallFrom,t.Activity
,SuppID = IIF( isnull(b.SuppID,'') = '', t.SuppID, b.SuppID)
,ModuleName = IIF( isnull(b.ModuleName,'') = '', t.ModuleName, b.ModuleName)
,t.SuppAPIThread,t.AddDate
,[JSON] = LEFT(t.JSON,100) + '...'
,[oriJson] = t.JSON
,[TransJSON] = LEFT(t.TransJSON,100) + '...'
,[oriTransJSON] = t.TransJSON
,[TransferResult] = case when isnull(em.ErrorMsg,'') !='' then 'Fail'
						 when isnull(cm.ErrorMsg,'') !='' then 'Fail'
						 else 'Success' end
,[Msg] = case when t.CallFrom = 'Resent' then isnull(em.ErrorMsg,'') 
			  else iif( isnull(em.ErrorMsg,'') = '', isnull(cm.ErrorMsg,''), isnull(em.ErrorMsg,''))  end
,[ErrorType] = case when em.Ukey != '' then 'Error Msg' + CHAR(13) + CHAR(10) + convert(varchar(20), isnull(em.Ukey,''))
					when cm.Ukey != '' then 'Check Msg' + CHAR(13) + CHAR(10) + convert(varchar(20),isnull(cm.Ukey,''))
					else '' end,[ErrorType_Chk] = IIF(em.Ukey != '', 'Error Msg' , ' Check Msg')
,[Resent] = em.ReSented
,[ResentTime] = em.EditDate
,[ErrMsgUkey] = em.Ukey
from fps.dbo.AutomationTransRecord t with(nolock)
left join Production.dbo.AutomationErrMsg em with(nolock) on em.AutomationTransRecordUkey = t.Ukey
left join Production.dbo.AutomationCheckMsg cm with(nolock) on cm.AutomationTransRecordUkey = t.Ukey
left join Production.dbo.AutomationDisplay b with(nolock) on t.SuppAPIThread = b.SuppAPIThread
where 1=1
{strWhere}
order by t.Ukey
";

            DualResult result = DBProxy.Current.Select(null, sqlcmd, out DataTable dt);
            if (result == false)
            {
                this.ShowErr(result.ToString());
            }

            this.listControlBindingSource1.DataSource = dt;

            DBProxy.Current.DefaultTimeout = 300;  // timeout時間改回5分鐘
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
exec dbo.SentJsonFromAutomationErrMsg {(long)dr["ErrMsgUkey"]},'{Sci.Env.User.UserID}', @result OUTPUT, 1200
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
	,Ukey  
	,SuppID
	,ModuleName
	,SuppAPIThread
	,JSON
	,JSON
	,AddName
	,AddDate
from dbo.AutomationErrMsg with (nolock)
where ukey = '{(long)dr["ErrMsgUkey"]}'
and not exists(select 1 from FPS.dbo.AutomationTransRecord where Activity = '{(long)dr["ErrMsgUkey"]}' and CallFrom = 'Resent')
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
