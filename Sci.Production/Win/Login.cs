using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Class.Commons;
using System.IO;
using System.Configuration;
using System.Transactions;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;
using System.Linq;
using System.Xml.Linq;


namespace Sci.Production.Win
{
    public partial class Login : Sci.Win.Tools.Base
    {
        Sci.Production.Main app;
        DualResult result;
        string flagPathFile = Path.Combine(Application.StartupPath, "sql_update.txt");

        public Login(Sci.Production.Main app)
        {
            this.app = app;
            InitializeComponent();          
                          
            ok.Click += ok_Click;
            exit.Click += exit_Click;
            //Sci.Production.SCHEMAS.PASS1Row data;

            if (ConfigurationManager.AppSettings["TaipeiServer"] != "")
            {
                //Assembly a = typeof(Module1).Assembly;
                label4.Visible = true;
                comboBox2.Visible = true;
                this.checkBoxTestEnvironment.Visible = true;
                this.ChangeTaipeiServer();
            }
        }


        void ok_Click(object sender, EventArgs e)
        {
            string act = this.act.Text;
            string loginFactory = (string)this.comboBox1.SelectedValue;
            string pwd = this.pwd.Text;

            if (0 == act.Length)
            {
                ShowErr("Please enter account.");
                this.act.Focus();
                return;
            }
            if (0 == pwd.Length)
            {
                ShowErr("Please enter password.");
                this.pwd.Focus();
                return;
            }
            if (MyUtility.Check.Empty(loginFactory))
            {
                ShowErr("Please select factory.");
                this.comboBox1.Focus();
                return;
            }


            IUserInfo user = null;
            UserInfo u = new UserInfo();
            result = UserLogin(act, pwd, loginFactory, u);

            if (!result)
            {
                ShowErr(result);
                return;
            }
            user = u;
            if (!(result = app.DoLogin(user)))
            {
                ShowErr(result);
                return;
            }

            //Sci.Env.App.Text = string.Format("Production Management System-({2})-{0}-({1})", Sci.Env.User.Factory, Sci.Env.User.UserID, Environment.MachineName);

            var appVerText = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(3);
            var appDirName = new DirectoryInfo(Application.StartupPath).Name;
            ConfigurationManager.AppSettings["formTextSufix"] = string.Format("Production Management System - ({0}-ver {1}) - ({2})"
                , appDirName
                , appVerText
                , System.Environment.MachineName);

            string userData = string.Format("-{0}-({1}))"
                , Sci.Env.User.Factory
                , Sci.Env.User.UserID);

            Sci.Env.App.Text = ConfigurationManager.AppSettings["formTextSufix"] + userData;

            DialogResult = DialogResult.OK;

            //若sql_update.txt不存在，則執行SQL UPDATE
            //if (!File.Exists(flagPathFile)) this.checkUpdateSQL();

            Close();
        }

        void exit_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void act_Validated(object sender, CancelEventArgs e)
        {
            comboBox1.DataSource = null;
            if (MyUtility.Check.Empty(this.act.Text.Trim()))
            {
                return;
            }

            DataTable dtPass1;
            string SQLCmd = "SELECT ID, Factory FROM Pass1 WHERE ID = @ID";
            System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter();
            sp1.ParameterName = "@ID";
            sp1.Value = this.act.Text;
            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
            cmds.Add(sp1);

            if (!(result = DBProxy.Current.Select(null, SQLCmd, cmds, out dtPass1)))
            {
                MyUtility.Msg.ErrorBox(result.ToString());
                e.Cancel = true;
                return;
            }
            if (dtPass1.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Account does not exist!");
                e.Cancel = true;
                return;
            }

            Dictionary<String, String> factoryOption = new Dictionary<String, String>();
            string[] factories = dtPass1.Rows[0]["Factory"].ToString().Split(new char[] { ',' });
            if (factories.Length > 0)
            {
                for (int i = 0; i < factories.Length; i++)
                {
                    factoryOption.Add(factories[i].Trim().ToUpper(), factories[i].Trim().ToUpper());
                }
                comboBox1.DataSource = new BindingSource(factoryOption, null);
                comboBox1.ValueMember = "Key";
                comboBox1.DisplayMember = "Value";
            }
        }

        public static DualResult UserLogin(string userid, string pwd, string factoryID, UserInfo u)
        {
            DualResult result;
            DataTable dtFactory;
            string keyword = "";
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

            Sci.Production.SCHEMAS.PASS1Row data;
            if (!(result = Sci.Production.Win.ProjUtils.GetPass1(userid, pwd, out data))) return result;
            if (null == data) return new DualResult(false, "Account or password invalid.");

            u.UserID = userid;
            u.UserPassword = pwd;
            if (!data.IsNAMENull()) u.UserName = data.NAME;
            if (!data.IsISADMINNull()) u.IsAdmin = data.ISADMIN;
            //if (!data.IsFACTORYNull()) Sci.Production.ProductionEnv.UserFactories = data.FACTORY;
            if (!data.IsFACTORYNull()) u.FactoryList = data.FACTORY;
            if (!data.IsISMISNull()) u.IsMIS = data.ISMIS;
            if (!data.IsEMAILNull()) u.MailAddress = data.EMAIL;
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
                Sci.Env.Cfg.MailServerIP = drSystem["Mailserver"].ToString().Trim();
                Sci.Env.Cfg.MailFrom = drSystem["Sendfrom"].ToString().Trim();
                Sci.Env.Cfg.MailServerAccount = drSystem["EmailID"].ToString().Trim();
                Sci.Env.Cfg.MailServerPassword = drSystem["EmailPwd"].ToString().Trim();
                Sci.Env.Cfg.FtpServerIP = drSystem["FtpIP"].ToString().Trim();
                Sci.Env.Cfg.FtpServerAccount = drSystem["FtpID"].ToString().Trim();
                Sci.Env.Cfg.FtpServerPassword = drSystem["FtpPwd"].ToString().Trim();
                Sci.Env.Cfg.ClipDir = drSystem["ClipPath"].ToString().Trim();
            }
            #endregion
            #region 寫入登入時間
            if (!(result = DBProxy.Current.Execute(null, string.Format("update pass1 set LastLoginTime = GETDATE() where id = '{0}'", userid))))
            {
                return result;
            }
            #endregion
            return result;
        }

        private void checkUpdateSQL()
        {
            string sql_update_receiver = ConfigurationManager.AppSettings["sql_update_receiver"];
            string[] dirs = Directory.GetFiles(Sci.Env.Cfg.ReportTempDir, "*.sql");
            if (dirs.Length == 0)
            {
                try 
                {
                    System.IO.File.WriteAllText(flagPathFile, "");  //新增sql_update.txt以註記SQL更新成功
                }
                catch (Exception e)
                {
                    sendmail(e.ToString(), sql_update_receiver);
                }
                return;
            }

            foreach (string dir in dirs)
            {
                string script = File.ReadAllText(dir);
                string strConnection = "";
                string strServer = "";

                TransactionScope _transactionscope = new TransactionScope();
                using (_transactionscope)
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

                        _transactionscope.Complete();
                        _transactionscope.Dispose();
                    }
                    catch (Exception ex)
                    {
                        _transactionscope.Dispose();

                        string subject = string.Format("Auto Update SQL ERROR");
                        string desc = string.Format(@"
Hi all,
    Factory = {0}, Account = {1},
    SQL UPDATE FAIL, Please check it.
-------------------------------------------------------------------
{2}
-------------------------------------------------------------------
Script
{3}
", Sci.Env.User.Factory
 , Sci.Env.User.UserName
 , ex.ToString()
 , script);
                        Sci.Win.Tools.MailTo mail = new Sci.Win.Tools.MailTo(Sci.Env.Cfg.MailFrom, sql_update_receiver, "", subject, "", desc, true, true);
                        mail.ShowDialog();
                        return;
                    }
                }
                _transactionscope.Dispose();
                _transactionscope = null;
            }

            try
            {
                System.IO.File.WriteAllText(flagPathFile, "");  //新增sql_update.txt以註記SQL更新成功
            }
            catch (Exception e)
            {
                sendmail(e.ToString(), sql_update_receiver);
            }
        }

        private void sendmail(string desc, string receiver)
        {
            string subject = "Auto Update SQL ERROR";
            Sci.Win.Tools.MailTo mail = new Sci.Win.Tools.MailTo(Sci.Env.Cfg.MailFrom, receiver, "", subject, "", desc, true, true);
            mail.ShowDialog();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedValue == null) return;
            DBProxy.Current.DefaultModuleName = comboBox2.SelectedValue2.ToString();
            act.Text = "";
            pwd.Text = "";
            comboBox1.DataSource = null;
        }

        private void ChangeTaipeiServer()
        {
            XDocument docx = XDocument.Load(Application.ExecutablePath + ".config");
            var hasConnectionNamedQuery = docx.Descendants("modules").Elements().Select(e => e.FirstAttribute.Value).ToList();
            Dictionary<String, String> SystemOption = new Dictionary<String, String>();
            string[] strSevers = ConfigurationManager.AppSettings["TaipeiServer"].Split(new char[] { ',' });
            if (strSevers.Length > 0 && hasConnectionNamedQuery.Count > 0)
            {
                foreach (string strSever in strSevers)
                {
                    for (int i = 0; i < hasConnectionNamedQuery.Count; i++)
                    {
                        if (strSever == hasConnectionNamedQuery[i])
                        {
                            SystemOption.Add(hasConnectionNamedQuery[i].Trim(), hasConnectionNamedQuery[i].Replace("PMSDB_", string.Empty).Replace("testing_", string.Empty).Trim().ToUpper());
                            break;
                        }
                    }
                }

                comboBox2.ValueMember = "Key";
                comboBox2.DisplayMember = "Value";
                comboBox2.DataSource = new BindingSource(SystemOption, null);
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
