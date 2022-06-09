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
    public partial class P05 : Sci.Win.Tems.Base
    {
        /// <inheritdoc/>
        public P05(ToolStripMenuItem menuitem)
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
            this.dateCreateTime.Value1 = DateTime.Now;
            this.dateCreateTime.Value2 = DateTime.Now;
            this.grid.IsEditingReadOnly = true;

            #region 表身欄位設定
            this.Helper.Controls.Grid.Generator(this.grid)
                .Text("Ukey", header: "ID", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("SuppID", header: "Supp", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("ModuleName", header: "Module Name", width: Widths.AnsiChars(18), iseditingreadonly: true)
                .Text("APIThread", header: "APIThread", width: Widths.AnsiChars(30), iseditingreadonly: true)
                .Text("SuppAPIThread", header: "Supp API Thread", width: Widths.AnsiChars(30), iseditingreadonly: true)
                .EditText("ErrorMsg", header: "Error Msg", width: Widths.AnsiChars(25), iseditingreadonly: true)
                .EditText("Json", header: "JSON", width: Widths.AnsiChars(30), iseditingreadonly: true)
                .DateTime("AddDate", header: "Create Time", width: Widths.AnsiChars(18), iseditingreadonly: true)
                .CheckBox("Success", header: "Success", width: Widths.AnsiChars(5), iseditable: false, trueValue: 1, falseValue: 0)
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
,Success
from AutomationReceivedMsg a with (nolock)
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
