using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Class;
using Sci.Production.Class.Command;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Drawing;
using System.IO;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Shipping
{
    /// <inheritdoc/>
    public partial class P44 : Win.Tems.QueryForm
    {
        private DataTable QuertData;

        /// <summary>
        /// Initializes a new instance of the <see cref="P44"/> class.
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public P44(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.EditMode = true;
            this.gridIcon1.Insert.Visible = false;
            this.gridIcon1.Append.Visible = false;
            this.radio_WHR28.Checked = true;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.grid.IsEditingReadOnly = false;
            this.grid.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.grid)
            .CheckBox("selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
            .Date("CreateDate", header: "Create Date", iseditingreadonly: true)
            .Text("FileName", header: "File Name", width: Widths.AnsiChars(40), iseditingreadonly: true)
            .DateTime("EditDate", header: "Edit Date", iseditingreadonly: true)
            .Text("EditName", header: "Edit Name", width: Widths.AnsiChars(15), iseditingreadonly: true);
        }

        private void Query()
        {
            string strCreateDateStart = this.dateQueryCreateDate.Value1.Empty() ? string.Empty : ((DateTime)this.dateQueryCreateDate.Value1).ToString("yyyy/MM/dd");
            string strCreateDateEnd = this.dateQueryCreateDate.Value2.Empty() ? string.Empty : ((DateTime)this.dateQueryCreateDate.Value2).ToString("yyyy/MM/dd");
            string fileType = this.radioPanel.Value;

            #region SqlParameter
            List<SqlParameter> listSQLParameter = new List<SqlParameter>
            {
                new SqlParameter("@CreateDateStart", strCreateDateStart),
                new SqlParameter("@CreateDateEnd", strCreateDateEnd),
                new SqlParameter("@FileType", fileType),
            };
            #endregion

            #region SQL Filter
            List<string> listSQLFilter = new List<string>();
            if (!MyUtility.Check.Empty(strCreateDateStart)
                && !MyUtility.Check.Empty(strCreateDateEnd))
            {
                listSQLFilter.Add("and CreateDate between @CreateDateStart and @CreateDateEnd");
            }
            #endregion

            this.ShowWaitMessage("Data Loading....");

            #region Sql Command

            string strCmd = $@"

select [selected] = 0
,* 
from StatementReport
where 1=1
and FileType = '{fileType}'
{listSQLFilter.JoinToString($"{Environment.NewLine} ")}
Order by CreateDate Desc, EditDate Desc, FileName
";
            #endregion
            DualResult result = DBProxy.Current.Select(string.Empty, strCmd, listSQLParameter, out this.QuertData);

            if (!result)
            {
                MyUtility.Msg.WarningBox(result.ToString());
            }
            else if (this.QuertData.Rows.Count < 1)
            {
                this.listControlBindingSource1.DataSource = null;
                MyUtility.Msg.InfoBox("Data not found !");
            }
            else
            {
                this.listControlBindingSource1.DataSource = this.QuertData;
            }

            this.HideWaitMessage();
        }

        private void BtnQuery_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.dateQueryCreateDate.Value1) ||
                MyUtility.Check.Empty(this.dateQueryCreateDate.Value2))
            {
                MyUtility.Msg.WarningBox("Please fill in both Dates to Query!");
                return;
            }

            this.Query();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.dateCreateDate.Value))
            {
                MyUtility.Msg.WarningBox("<Create Date> cannot be empty!");
                this.dateCreateDate.Select();
                return;
            }

            // 呼叫File 選擇視窗
            OpenFileDialog ofdFileName = ProductionEnv.GetOpenFileDialog();
            ofdFileName.Multiselect = false;
            ofdFileName.Filter = "Excel files (*.xlsx;*.xls)|*.xlsx;*.xls";
            if (ofdFileName.ShowDialog() == DialogResult.OK)
            {
                if (!File.Exists(ofdFileName.FileName))
                {
                    this.ShowErr("Import File is not exist");
                    return;
                }
            }
            else
            {
                return;
            }

            this.ShowWaitMessage("Uploading...Please wait..");

            string fileName = ofdFileName.FileName;
            string[] clipFiles = new string[]
            {
                fileName,
            };
            string fileN = ofdFileName.SafeFileName;
            string uniqueKey = this.radioPanel.Value + ((DateTime)this.dateCreateDate.Value).ToString("yyyyMMdd") + Path.GetFileNameWithoutExtension(fileN);

            #region Save Data & upload excel
            DualResult result;
            List<SqlParameter> listSQLParameter = new List<SqlParameter>
            {
                new SqlParameter("@FileType", this.radioPanel.Value),
                new SqlParameter("@CreateDate", ((DateTime)this.dateCreateDate.Value).ToString("yyyy/MM/dd")),
                new SqlParameter("@FileName", fileN),
                new SqlParameter("@UserID", Env.User.UserID),
            };
            string sqlcmd = $@"
if exists(select 1 from StatementReport where FileType = @FileType and CreateDate = @CreateDate and FileName = @FileName)
begin
	update StatementReport
	set EditDate = GETDATE()
	,EditName = @UserID
	where FileType = @FileType and CreateDate = @CreateDate
    and FileName = @FileName
end
else
begin
	insert into StatementReport(FileType,CreateDate,FileName,EditName,EditDate)
	values(@FileType,@CreateDate,@FileName,@UserID,GETDATE())
end
";
            if (MyUtility.Check.Seek(@"select * from StatementReport where FileType = @FileType and CreateDate = @CreateDate and FileName = @FileName", listSQLParameter))
            {
                DialogResult dResult = MyUtility.Msg.QuestionBox(
                    $@"This FileName already exists with <{this.radioPanel.Value}> and <{((DateTime)this.dateCreateDate.Value).ToString("yyyy/MM/dd")}>.
Do you want to replace it?", "Question",
                    MessageBoxButtons.YesNo,
                    MessageBoxDefaultButton.Button2);
                if (dResult == DialogResult.No)
                {
                    return;
                }
            }

            string clipUKey = this.radioPanel.Value.ToString() + ((DateTime)this.dateCreateDate.Value).ToString("yyyyMMdd") + Path.GetFileNameWithoutExtension(fileN);

            TransactionScope transactionscope = new TransactionScope();
            SqlConnection sqlConn = null;
            DBProxy.Current.DefaultTimeout = 600;  // timeout時間改為10分鐘
            DBProxy.Current.OpenConnection(null, out sqlConn);
            using (transactionscope)
            using (sqlConn)
            {
                try
                {
                    if (!(result = DBProxy.Current.Execute(string.Empty, sqlcmd, listSQLParameter)))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(result);
                        this.HideWaitMessage();
                        return;
                    }

                    // 刪除重複的Create + FileType的Clip檔案和資料
                    if (MyUtility.Check.Seek($@"
select * 
from Clip 
where TableName = 'StatementReport' 
and UniqueKey = '{clipUKey}'"))
                    {
                        if (!(result = Sci.Win.Tools.Clip.DeleteClip("StatementReport", clipUKey)))
                        {
                            transactionscope.Dispose();
                            this.ShowErr(result);
                            return;
                        }
                    }

                    // 使用底層Clip來上傳檔案
                    // 新增clip 檔案和資料
                    result = Sci.Win.Tools.Clip.AddClip(tablename: "StatementReport", uniqueID: uniqueKey, userID: Env.User.UserID, description: "Import " + this.radioPanel.Value + " on " + ((DateTime)this.dateCreateDate.Value).ToString("yyyyMMdd"), clipFiles: clipFiles);
                    if (!result)
                    {
                        transactionscope.Dispose();
                        this.ShowErr(" Import file fail" + result);
                        this.HideWaitMessage();
                        return;
                    }

                    transactionscope.Complete();
                }
                catch (Exception ex)
                {
                    transactionscope.Dispose();
                    result = new DualResult(false, "Commit Save error.", ex);
                    this.HideWaitMessage();
                    return;
                }
            }

            MyUtility.Msg.InfoBox("Save successful");
            this.HideWaitMessage();
            this.Query();
            #endregion
        }

        private void GridIcon1_RemoveClick(object sender, EventArgs e)
        {
            this.grid.ValidateControl();
            this.listControlBindingSource1.EndEdit();
            DualResult result;
            DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
            if (MyUtility.Check.Empty(dt))
            {
                MyUtility.Msg.WarningBox("Please choose Report Type and Query first!");
                return;
            }

            DataRow[] selectedRow = dt.Select("selected = 1");
            if (selectedRow.Length == 0 || selectedRow.Length != 1)
            {
                MyUtility.Msg.WarningBox("Please select one and only one File to delete!");
                return;
            }

            DialogResult dResult = MyUtility.Msg.QuestionBox($@"Are you sure about deleting this report <{selectedRow[0]["FileName"].ToString()}>?", "Question", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2);
            if (dResult == DialogResult.No)
            {
                return;
            }

            string sqldel = @"delete from StatementReport where FileType = @FileType and CreateDate = @CreateDate and FileName = @FileName";
            List<SqlParameter> listSQLParameter = new List<SqlParameter>
            {
                new SqlParameter("@FileType", selectedRow[0]["FileType"]),
                new SqlParameter("@CreateDate", ((DateTime)selectedRow[0]["CreateDate"]).ToString("yyyy/MM/dd")),
                new SqlParameter("@FileName", selectedRow[0]["FileName"]),
            };

            string clipUKey = selectedRow[0]["FileType"].ToString() + ((DateTime)selectedRow[0]["CreateDate"]).ToString("yyyyMMdd") + Path.GetFileNameWithoutExtension(selectedRow[0]["FileName"].ToString());
            TransactionScope transactionscope = new TransactionScope();
            SqlConnection sqlConn = null;
            DBProxy.Current.OpenConnection(null, out sqlConn);
            using (transactionscope)
            using (sqlConn)
            {
                try
                {
                    if (!(result = DBProxy.Current.Execute(string.Empty, sqldel, listSQLParameter)))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(result);
                        return;
                    }

                    if (!(result = Sci.Win.Tools.Clip.DeleteClip("StatementReport", clipUKey)))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(result);
                        return;
                    }

                    transactionscope.Complete();
                }
                catch (Exception ex)
                {
                    transactionscope.Dispose();
                    result = new DualResult(false, "Commit delete error.", ex);
                    return;
                }
            }

            this.Query();
        }
    }
}
