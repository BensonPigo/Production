using Ict;
using Sci.Data;
using System;
using System.Data;
using System.Windows.Forms;
using System.IO;
using System.Transactions;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace Sci.Production.Tools
{
    /// <inheritdoc/>
    public partial class SQL_Query : Win.Tems.QueryForm
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SQL_Query"/> class.
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public SQL_Query(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.GridSetup();
        }

        private string sqlcmd;

        private void GridSetup()
        {
            this.gridSQLQuery.AutoGenerateColumns = true;
        }

        private void BtnQuery_Click(object sender, EventArgs e)
        {
            this.listControlBindingSource1.DataSource = null;

            this.sqlcmd = this.editSQL.Text;
            DualResult result;
            result = DBProxy.Current.Select(string.Empty, this.sqlcmd, null, out DataTable dt);
            if (!result)
            {
                this.ShowErr(this.sqlcmd, result);
                return;
            }

            this.listControlBindingSource1.DataSource = dt;
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox1.Checked)
            {
                this.btnSqlUpdate.Visible = true;
            }
            else
            {
                this.btnSqlUpdate.Visible = false;
            }
        }

        private void BtnSqlUpdate_Click(object sender, EventArgs e)
        {
            string[] dirs = Directory.GetFiles(Env.Cfg.ReportTempDir, "*.sql");
            string subject = string.Format("DataBase={0}, Account={1}, Factory={2} SQL Update Success !!", DBProxy.Current.DefaultModuleName, Env.User.UserName, Env.User.Factory);
            string desc = subject;

            if (dirs.Length == 0)
            {
                MyUtility.Msg.WarningBox("No update on this time !!", "Warning");
                return;
            }

            Exception exception = null;
            foreach (string dir in dirs)
            {
                string script = File.ReadAllText(dir);

                // DualResult upResult;
                TransactionScope transactionscope = new TransactionScope();
                using (transactionscope)
                {
                    try
                    {
                        DBProxy.Current.OpenConnection("Production", out SqlConnection connection);

                        using (connection)
                        {
                            Server db = new Server(new ServerConnection(connection));
                            db.ConnectionContext.ExecuteNonQuery(script);
                        }

                        transactionscope.Complete();
                        transactionscope.Dispose();
                    }
                    catch (Exception ex)
                    {
                        transactionscope.Dispose();
                        exception = ex;
                        subject = string.Format("DataBase={0}, Account={1}, Factory={2} SQL Update Fail !!", DBProxy.Current.DefaultModuleName, Env.User.UserName, Env.User.Factory);
                        desc = subject + string.Format(
                            @"
------------------------------------------------------------
{0}
-----------------------------------------------------------", ex.ToString());
                        this.Sendmail(subject, desc);
                        break;
                    }
                }

                transactionscope.Dispose();
                transactionscope = null;
            }

            if (exception != null)
            {
                this.ShowErr("Commit transaction error.", exception);
                return;
            }

            MyUtility.Msg.InfoBox("Update completed !!");
            this.Sendmail(subject, desc);
        }

        private void Sendmail(string subject, string desc)
        {
            string sql_update_receiver = ConfigurationManager.AppSettings["sql_update_receiver"];
            Win.Tools.MailTo mail = new Win.Tools.MailTo(Env.Cfg.MailFrom, sql_update_receiver, string.Empty, subject, string.Empty, desc, true, true);
            mail.ShowDialog();
        }
    }
}
