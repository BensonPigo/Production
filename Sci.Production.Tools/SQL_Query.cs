using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Ict.Win;
using System.Transactions;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Data.Common;
using System.Configuration;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Server;
using Microsoft.SqlServer.Management.Smo;

namespace Sci.Production.Tools
{
    public partial class SQL_Query : Sci.Win.Tems.QueryForm
    {
        public SQL_Query(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            GridSetup();
        }

        string sqlcmd;

        private void GridSetup()
        {
            this.gridSQLQuery.AutoGenerateColumns = true;
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            listControlBindingSource1.DataSource = null;
            
            sqlcmd = editSQL.Text;
            DualResult result;
            DataTable dt;
            result = DBProxy.Current.Select("", sqlcmd, null, out dt);
            if (!result)
            {
                ShowErr(sqlcmd, result);
                return;
            }

            listControlBindingSource1.DataSource = dt;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                button1.Visible = true;
            else
                button1.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string[] dirs = Directory.GetFiles(Sci.Env.Cfg.ReportTempDir,"*.sql");
            if (dirs.Length == 0)
            {
                MyUtility.Msg.WarningBox("No update on this time !!", "Warning");
                return;
            }

            foreach (string dir in dirs)
            {
                string script = File.ReadAllText(dir);
             
                //DualResult upResult;
                TransactionScope _transactionscope = new TransactionScope();
                using (_transactionscope)
                {
                    try
                    {
                        //script = script.Replace("GO\r\n", "");
                        //if (!(upResult = DBProxy.Current.Execute(null, script)))
                        //{
                        //    _transactionscope.Dispose();
                        //    ShowErr(script, upResult);
                        //    return;
                        //}

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
                        ShowErr("Commit transaction error.", ex);
                        return;
                    }
                }               
                _transactionscope.Dispose();
                _transactionscope = null;              
            }
            MyUtility.Msg.InfoBox("Update completed !!");
            
        }
    }
}
