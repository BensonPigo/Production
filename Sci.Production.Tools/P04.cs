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
    public partial class P04 : Sci.Win.Tems.Base
    {
        /// <inheritdoc/>
        public P04(ToolStripMenuItem menuitem)
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
            this.dateTimePicker1.CustomFormat = "yyyy/MM/dd HH:mm:ss";
            this.dateTimePicker2.CustomFormat = "yyyy/MM/dd HH:mm:ss";

            this.dateTimePicker1.Value = DateTime.Now.AddHours(-1);
            this.dateTimePicker2.Value = DateTime.Now;

            this.grid.IsEditingReadOnly = true;
            DataGridViewGeneratorTextColumnSettings col_Json = new DataGridViewGeneratorTextColumnSettings();
            col_Json.EditingMouseDoubleClick += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    DataRow dr = this.grid.GetDataRow(e.RowIndex);
                    string fullJson = MyUtility.GetValue.Lookup($@"select Json from AutomationCheckMsg with(nolock) where ukey = '{dr["Ukey"]}'");
                    Win.Tools.EditMemo callNextForm = new Win.Tools.EditMemo(fullJson, "Full JSON", false, null);
                    callNextForm.ShowDialog(this);
                }
            };

            #region 表身欄位設定
            this.Helper.Controls.Grid.Generator(this.grid)
                .Text("Ukey", header: "ID", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("SuppID", header: "Supp", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("ModuleName", header: "Module Name", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("APIThread", header: "APIThread", width: Widths.AnsiChars(25), iseditingreadonly: true)
                .Text("SuppAPIThread", header: "Supp API Thread", width: Widths.AnsiChars(25), iseditingreadonly: true)
                .EditText("ErrorMsg", header: "Error Msg", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("Json", header: "JSON", width: Widths.AnsiChars(30), iseditingreadonly: true, settings: col_Json)
                .DateTime("AddDate", header: "Create Time", width: Widths.AnsiChars(18), iseditingreadonly: true)
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

            strWhere += $@"
and a.AddDate between '{this.dateTimePicker1.Text}' and '{this.dateTimePicker2.Text}'";

            #endregion

            if (MyUtility.Check.Empty(strWhere))
            {
                strWhere = " and 1=0";
            }

            string sqlcmd = $@"
select 
a.Ukey  
,SuppID = IIF( isnull(b.SuppID,'') = '', a.SuppID, b.SuppID)
,ModuleName = IIF( isnull(b.ModuleName,'') = '', a.ModuleName, b.ModuleName)
,APIThread
,a.SuppAPIThread
,ErrorMsg
,[JSON] = LEFT(a.JSON,100) + '...'
,AddDate
from AutomationCheckMsg a with (nolock)
left join AutomationDisplay b with (nolock) on a.SuppAPIThread = b.SuppAPIThread
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
    }
}
