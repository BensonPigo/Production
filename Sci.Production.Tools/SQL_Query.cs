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
    public partial class SQL_Query : Sci.Win.Tems.QueryForm
    {
        public SQL_Query(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.GridSetup();
        }

        string sqlcmd;

        private void GridSetup()
        {
            this.gridSQLQuery.AutoGenerateColumns = true;
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            this.listControlBindingSource1.DataSource = null;

            this.sqlcmd = this.editSQL.Text;
            DualResult result;
            DataTable dt;
            result = DBProxy.Current.Select(string.Empty, this.sqlcmd, null, out dt);
            if (!result)
            {
                this.ShowErr(this.sqlcmd, result);
                return;
            }

            this.listControlBindingSource1.DataSource = dt;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
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

        private void btnSqlUpdate_Click(object sender, EventArgs e)
        {
            string[] dirs = Directory.GetFiles(Sci.Env.Cfg.ReportTempDir, "*.sql");
            string subject = string.Format("DataBase={0}, Account={1}, Factory={2} SQL Update Success !!", DBProxy.Current.DefaultModuleName, Sci.Env.User.UserName, Sci.Env.User.Factory);
            string desc = subject;

            if (dirs.Length == 0)
            {
                MyUtility.Msg.WarningBox("No update on this time !!", "Warning");
                return;
            }

            foreach (string dir in dirs)
            {
                string script = File.ReadAllText(dir);

                // DualResult upResult;
                TransactionScope _transactionscope = new TransactionScope();
                using (_transactionscope)
                {
                    try
                    {
                        SqlConnection connection;
                        DBProxy.Current.OpenConnection("Production", out connection);

                        using (connection)
                       {
                            Server db = new Server(new ServerConnection(connection));
                            db.ConnectionContext.ExecuteNonQuery(script);
                        }

                        _transactionscope.Complete();
                        _transactionscope.Dispose();
                    }
                    catch (Exception ex)
                    {
                        _transactionscope.Dispose();
                        this.ShowErr("Commit transaction error.", ex);
                        subject = string.Format("DataBase={0}, Account={1}, Factory={2} SQL Update Fail !!", DBProxy.Current.DefaultModuleName, Sci.Env.User.UserName, Sci.Env.User.Factory);
                        desc = subject + string.Format(
                            @"
------------------------------------------------------------
{0}
-----------------------------------------------------------", ex.ToString());
                        this.sendmail(subject, desc);
                        return;
                    }
                }

                _transactionscope.Dispose();
                _transactionscope = null;
            }

            MyUtility.Msg.InfoBox("Update completed !!");
            this.sendmail(subject, desc);
        }

        private void sendmail(string subject, string desc)
        {
            string sql_update_receiver = ConfigurationManager.AppSettings["sql_update_receiver"];
            Sci.Win.Tools.MailTo mail = new Sci.Win.Tools.MailTo(Sci.Env.Cfg.MailFrom, sql_update_receiver, string.Empty, subject, string.Empty, desc, true, true);
            mail.ShowDialog();
        }
    }
}
