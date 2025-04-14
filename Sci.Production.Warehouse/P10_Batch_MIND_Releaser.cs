using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Class.Command;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P10_Batch_MIND_Releaser : Win.Tems.QueryForm
    {
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk = new Ict.Win.UI.DataGridViewCheckBoxColumn();

        /// <inheritdoc/>
        public P10_Batch_MIND_Releaser()
        {
            this.InitializeComponent();
            this.EditMode = true;
            this.dateIssueDate.DateBox1.Value = global::System.DateTime.Now;
            this.dateIssueDate.DateBox2.Value = global::System.DateTime.Now;
            this.Query();
            this.GridSetup();
        }

        private void GridSetup()
        {
            DataGridViewGeneratorTextColumnSettings releaser = new DataGridViewGeneratorTextColumnSettings();
            releaser.EditingMouseDown += (s, e) =>
            {
                if (!this.EditMode || e.RowIndex == -1)
                {
                    return;
                }

                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    DataRow dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);

                    string strID = MyUtility.Convert.GetString(dr["ID"]);

                    DataTable dt = this.listControlBindingSource1.DataSource as DataTable;

                    P10_AssignReleaser win = new P10_AssignReleaser(strID, false, dt);
                    win.ShowDialog(this);
                }
            };
            this.grid1.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.grid1)
            .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: true, falseValue: false).Get(out this.col_chk)
            .Text("ID", header: "ID", width: Widths.AnsiChars(20), iseditingreadonly: true)
            .Date("IssueDate", header: "Issue Date", width: Widths.AnsiChars(20), iseditingreadonly: true)
            .Text("Request#", header: "Request#", width: Widths.AnsiChars(20), iseditingreadonly: true)
            .Text("OrderID", header: "SP#", width: Widths.AnsiChars(20), iseditingreadonly: true)
            .EditText("Remark", header: "Remark", width: Widths.AnsiChars(25), iseditingreadonly: true)
            .Text("Releaser", header: "Releaser", width: Widths.AnsiChars(30), iseditingreadonly: true, settings: releaser)
            ;

            this.grid1.Columns["Releaser"].DefaultCellStyle.BackColor = Color.Pink;
        }

        private void Query()
        {
            string sqlcmd = $@"
            SELECT 
             [Selected] = 0
            ,[Id]
            ,[IssueDate]
            ,[Status]
            ,[Request#] = [CutplanID]
            ,[OrderID] = o.POID
            ,[Remark]
            ,[Releaser] = Releaser.val
            FROM Issue I WITH(NOLOCK)
            OUTER APPLY
            (
	            select val = stuff(
	            (
		            SELECT distinct concat(',',tmp.Releaser)
		            FROM
		            (
			            select Releaser from Issue_MIND
			            where id =I.Id
		            )tmp for xml path('')   
	            ),1,1,'')
            )Releaser
            OUTER APPLY
            (
                select top(1) POID from Issue_Detail where id = I.Id
            ) o
            WHERE 1=1 and Type='A' and I.ISSUEDATE >= '{this.dateIssueDate.DateBox1.Text}' AND I.ISSUEDATE <= '{this.dateIssueDate.DateBox2.Text}' AND I.MDivisionID = '{Env.User.Keyword}'
            ORDER BY I.ISSUEDATE, I.ID";
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out DataTable dt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.listControlBindingSource1.DataSource = dt;
        }

        private void BtnMIND_Click(object sender, EventArgs e)
        {
            DataTable dtSource = (DataTable)this.listControlBindingSource1.DataSource;

            // 使用 LINQ 過濾和選擇
            List<string> idList = dtSource.AsEnumerable()
            .Where(row => MyUtility.Convert.GetString(row["Selected"]) == "1")
            .Select(row => MyUtility.Convert.GetString(row["ID"]))
            .ToList();

            if (idList.Count == 0)
            {
                MyUtility.Msg.WarningBox("Please check the Issue first.");
                return;
            }

            DataTable dt = this.listControlBindingSource1.DataSource as DataTable;
            P10_AssignReleaser win = new P10_AssignReleaser(idList.JoinToString("','"), true, dt);
            win.ShowDialog(this);
            this.Query();
        }

        private void DateIssueDate_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.listControlBindingSource1.DataSource == null)
            {
                return;
            }

            this.Query();
        }
    }
}
