using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using System.IO;
using System.Configuration;
using System.Transactions;
using System.Data.SqlClient;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;
using System.Linq;
using System.Xml.Linq;

namespace Sci.Production.Win
{
    /// <summary>
    /// Login
    /// </summary>
    public partial class Login : Sci.Win.Tools.Base
    {
        private Main app;
        private DualResult result;
        private string flagPathFile = Path.Combine(Application.StartupPath, "sql_update.txt");

        /// <summary>
        /// Initializes a new instance of the <see cref="Login"/> class.
        /// </summary>
        /// <param name="app">app</param>
        public Login(Main app)
        {
            this.app = app;
            this.InitializeComponent();

            this.ok.Click += this.Ok_Click;
            this.exit.Click += this.Exit_Click;

            // Sci.Production.SCHEMAS.PASS1Row data;
            if (ConfigurationManager.AppSettings["TaipeiServer"] != string.Empty)
            {
                // Assembly a = typeof(Module1).Assembly;
                this.label4.Visible = true;
                this.comboBox2.Visible = true;
                this.checkBoxTestEnvironment.Visible = true;
                this.ChangeTaipeiServer();
            }
        }

        private void Ok_Click(object sender, EventArgs e)
        {
            string act = this.act.Text;
            string loginFactory = (string)this.comboBox1.SelectedValue;
            string pwd = this.pwd.Text;

            if (act.Length == 0)
            {
                this.ShowErr("Please enter account.");
                this.act.Focus();
                return;
            }

            if (pwd.Length == 0)
            {
                this.ShowErr("Please enter password.");
                this.pwd.Focus();
                return;
            }

            if (MyUtility.Check.Empty(loginFactory))
            {
                this.ShowErr("Please select factory.");
                this.comboBox1.Focus();
                return;
            }

            IUserInfo user = null;
            UserInfo u = new UserInfo();
            this.result = UserLogin(act, pwd, loginFactory, u);

            if (!this.result)
            {
                this.ShowErr(this.result);
                return;
            }

            user = u;
            if (!(this.result = this.app.DoLogin(user)))
            {
                this.ShowErr(this.result);
                return;
            }

            // Sci.Env.App.Text = string.Format("Production Management System-({2})-{0}-({1})", Sci.Env.User.Factory, Sci.Env.User.UserID, Environment.MachineName);
            var appVerText = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(3);
            var appDirName = new DirectoryInfo(Application.StartupPath).Name;
            ConfigurationManager.AppSettings["formTextSufix"] = string.Format(
                "Production Management System - ({0}-ver {1}) - ({2})",
                appDirName,
                appVerText,
                Environment.MachineName);

            string userData = string.Format(
                "-{0}-({1}))",
                Env.User.Factory,
                Env.User.UserID);

            Env.App.Text = ConfigurationManager.AppSettings["formTextSufix"] + userData;

            this.DialogResult = DialogResult.OK;

            // 若sql_update.txt不存在，則執行SQL UPDATE
            // if (!File.Exists(flagPathFile)) this.checkUpdateSQL();
            this.Close();
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void Act_Validated(object sender, CancelEventArgs e)
        {
            this.comboBox1.DataSource = null;
            if (MyUtility.Check.Empty(this.act.Text.Trim()))
            {
                return;
            }

            DataTable dtPass1;
            string sqlCmd = "SELECT ID, Factory FROM Pass1 WHERE ID = @ID";
            SqlParameter sp1 = new SqlParameter
            {
                ParameterName = "@ID",
                Value = this.act.Text,
            };
            IList<SqlParameter> cmds = new List<SqlParameter>
            {
                sp1,
            };

            if (!(this.result = DBProxy.Current.Select(null, sqlCmd, cmds, out dtPass1)))
            {
                MyUtility.Msg.ErrorBox(this.result.ToString());
                e.Cancel = true;
                return;
            }

            if (dtPass1.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Account does not exist!");
                e.Cancel = true;
                return;
            }

            Dictionary<string, string> factoryOption = new Dictionary<string, string>();
            string[] factories = dtPass1.Rows[0]["Factory"].ToString().Split(new char[] { ',' });
            if (factories.Length > 0)
            {
                for (int i = 0; i < factories.Length; i++)
                {
                    factoryOption.Add(factories[i].Trim().ToUpper(), factories[i].Trim().ToUpper());
                }

                this.comboBox1.DataSource = new BindingSource(factoryOption, null);
                this.comboBox1.ValueMember = "Key";
                this.comboBox1.DisplayMember = "Value";
            }
        }

        /// <summary>
        /// User Login
        /// </summary>
        /// <param name="userid">User ID</param>
        /// <param name="pwd">PWD</param>
        /// <param name="factoryID">Factory ID</param>
        /// <param name="u">User Info</param>
        /// <returns>DualResult</returns>
        public static DualResult UserLogin(string userid, string pwd, string factoryID, UserInfo u)
        {
            DualResult result;
            DataTable dtFactory;
            string keyword = string.Empty;
            factoryID = factoryID.TrimEnd();
            if (!(result = DBProxy.Current.Select(null, string.Format("SELECT MDivisionID FROM Factory WHERE ID = '{0}'", factoryID), out dtFactory)))
            {
                return result;
            }

            if (dtFactory.Rows.Count > 0 && !MyUtility.Check.Empty(dtFactory.Rows[0]["MDivisionID"].ToString()))
            {
                keyword = dtFactory.Rows[0]["MDivisionID"].ToString().TrimEnd();
            }
            else
            {
                return new DualResult(false, "Mdivision does not exist!");
            }

            SCHEMAS.PASS1Row data;
            if (!(result = ProjUtils.GetPass1(userid, pwd, out data)))
            {
                return result;
            }

            if (data == null)
            {
                return new DualResult(false, "Account or password invalid.");
            }

            u.UserID = userid;
            u.UserPassword = pwd;
            if (!data.IsNAMENull())
            {
                u.UserName = data.NAME;
            }

            if (!data.IsISADMINNull())
            {
                u.IsAdmin = data.ISADMIN;
            }

            // if (!data.IsFACTORYNull()) Sci.Production.ProductionEnv.UserFactories = data.FACTORY;
            if (!data.IsFACTORYNull())
            {
                u.FactoryList = data.FACTORY;
            }

            if (!data.IsISMISNull())
            {
                u.IsMIS = data.ISMIS;
            }

            if (!data.IsEMAILNull())
            {
                u.MailAddress = data.EMAIL;
            }

            u.Factory = factoryID;
            u.Keyword = keyword;

            #region 登入時將SYSTEM資料表相關設定載入Sci.Env.Cfg中
            DataRow drSystem;
            if (!MyUtility.Check.Seek("select * from system", out drSystem, null))
            {
                return new DualResult(false, "Get system data fail !!");
            }
            else
            {
                Env.Cfg.MailServerIP = drSystem["Mailserver"].ToString().Trim();
                Env.Cfg.MailFrom = drSystem["Sendfrom"].ToString().Trim();
                Env.Cfg.MailServerAccount = drSystem["EmailID"].ToString().Trim();
                Env.Cfg.MailServerPassword = drSystem["EmailPwd"].ToString().Trim();
                Env.Cfg.FtpServerIP = drSystem["FtpIP"].ToString().Trim();
                Env.Cfg.FtpServerAccount = drSystem["FtpID"].ToString().Trim();
                Env.Cfg.FtpServerPassword = drSystem["FtpPwd"].ToString().Trim();
                Env.Cfg.ClipDir = drSystem["ClipPath"].ToString().Trim();
            }
            #endregion
            #region 寫入登入時間
            if (!DBProxy.Current.DefaultModuleName.Contains("PMSDB"))
            {
                if (!(result = DBProxy.Current.Execute(null, string.Format("update pass1 set LastLoginTime = GETDATE() where id = '{0}'", userid))))
                {
                    return result;
                }
            }
            #endregion
            #region 在台北端(PMSDB 或 testing)登入時, 關閉紀錄UserLog功能
            if (DBProxy.Current.DefaultModuleName.Contains("PMSDB") || DBProxy.Current.DefaultModuleName.Contains("testing"))
            {
                Env.Cfg.EnableUserLog = false;
            }
            #endregion
            return result;
        }

        private void CheckUpdateSQL()
        {
            string sql_update_receiver = ConfigurationManager.AppSettings["sql_update_receiver"];
            string[] dirs = Directory.GetFiles(Env.Cfg.ReportTempDir, "*.sql");
            if (dirs.Length == 0)
            {
                try
                {
                    File.WriteAllText(this.flagPathFile, string.Empty);  // 新增sql_update.txt以註記SQL更新成功
                }
                catch (Exception e)
                {
                    this.Sendmail(e.ToString(), sql_update_receiver);
                }

                return;
            }

            foreach (string dir in dirs)
            {
                string script = File.ReadAllText(dir);
                string strConnection = string.Empty;
                string strServer = string.Empty;

                TransactionScope transactionscope = new TransactionScope();
                using (transactionscope)
                {
                    try
                    {
                        SqlConnection connection;
                        DBProxy.Current.OpenConnection("Production", out connection);
                        strConnection = connection.ConnectionString.ToString();
                        using (connection)
                        {
                            Server db = new Server(new ServerConnection(connection));
                            strServer = db.Urn.ToString();
                            db.ConnectionContext.ExecuteNonQuery(script);
                        }

                        transactionscope.Complete();
                        transactionscope.Dispose();
                    }
                    catch (Exception ex)
                    {
                        transactionscope.Dispose();

                        string subject = string.Format("Auto Update SQL ERROR");
                        string desc = string.Format(
                            @"
Hi all,
    Factory = {0}, Account = {1},
    SQL UPDATE FAIL, Please check it.
-------------------------------------------------------------------
{2}
-------------------------------------------------------------------
Script
{3}
", Env.User.Factory,
                            Env.User.UserName,
                            ex.ToString(),
                            script);
                        Sci.Win.Tools.MailTo mail = new Sci.Win.Tools.MailTo(Env.Cfg.MailFrom, sql_update_receiver, string.Empty, subject, string.Empty, desc, true, true);
                        mail.ShowDialog();
                        return;
                    }
                }

                transactionscope.Dispose();
                transactionscope = null;
            }

            try
            {
                File.WriteAllText(this.flagPathFile, string.Empty);  // 新增sql_update.txt以註記SQL更新成功
            }
            catch (Exception e)
            {
                this.Sendmail(e.ToString(), sql_update_receiver);
            }
        }

        private void Sendmail(string desc, string receiver)
        {
            string subject = "Auto Update SQL ERROR";
            Sci.Win.Tools.MailTo mail = new Sci.Win.Tools.MailTo(Env.Cfg.MailFrom, receiver, string.Empty, subject, string.Empty, desc, true, true);
            mail.ShowDialog();
        }

        private void ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBox2.SelectedValue == null)
            {
                return;
            }

            DBProxy.Current.DefaultModuleName = this.comboBox2.SelectedValue2.ToString();
            this.act.Text = string.Empty;
            this.pwd.Text = string.Empty;
            this.comboBox1.DataSource = null;
        }

        private void ChangeTaipeiServer()
        {
            XDocument docx = XDocument.Load(Application.ExecutablePath + ".config");
            var hasConnectionNamedQuery = docx.Descendants("modules").Elements().Select(e => e.FirstAttribute.Value).ToList();
            Dictionary<string, string> systemOption = new Dictionary<string, string>();
            string[] strSevers = ConfigurationManager.AppSettings["TaipeiServer"].Split(new char[] { ',' });
            if (strSevers.Length > 0 && hasConnectionNamedQuery.Count > 0)
            {
                foreach (string strSever in strSevers)
                {
                    for (int i = 0; i < hasConnectionNamedQuery.Count; i++)
                    {
                        if (strSever == hasConnectionNamedQuery[i])
                        {
                            systemOption.Add(hasConnectionNamedQuery[i].Trim(), hasConnectionNamedQuery[i].Replace("PMSDB_", string.Empty).Replace("testing_", string.Empty).Trim().ToUpper());
                            break;
                        }
                    }
                }

                this.comboBox2.ValueMember = "Key";
                this.comboBox2.DisplayMember = "Value";
                this.comboBox2.DataSource = new BindingSource(systemOption, null);
            }
        }

        private void CheckBoxTestEnvironment_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBoxTestEnvironment.Checked)
            {
                ConfigurationManager.AppSettings["TaipeiServer"] = ConfigurationManager.AppSettings["TestingServer"];
                ConfigurationManager.AppSettings["ServerMatchFactory"] = ConfigurationManager.AppSettings["TestingServerMatchFactory"];
            }
            else
            {
                ConfigurationManager.AppSettings["TaipeiServer"] = ConfigurationManager.AppSettings["PMSDBServer"];
                ConfigurationManager.AppSettings["ServerMatchFactory"] = ConfigurationManager.AppSettings["PMSDBServerMatchFactory"];
            }

            this.ChangeTaipeiServer();
        }
    }
}
