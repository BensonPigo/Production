using Ict;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using Sci.Data;
using Sci.Production.Prg;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Transactions;
using System.Windows.Forms;
using System.Xml.Linq;
using static Sci.AuthenticationAPI.AuthenticationAD;

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
        private static bool isNeedOTP;

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
                this.label4.Visible = true;
                this.comboBox2.Visible = true;
                this.checkBoxTestEnvironment.Visible = true;

                // 抓電腦登入名稱
                string loginAccount = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                string[] strSevers = ConfigurationManager.AppSettings["PMSTeam_LoginAccount"].Split(new char[] { ',' });
                var result = strSevers.Where(c => c.ToUpper().Contains(loginAccount.ToUpper()));
                if (result.Any())
                {
                    this.act.Text = "SCIMIS";
                    this.pwd.Text = "SCIMIS888";
                    this.comboBox1.SelectedValue = "MAI";
                    this.checkBoxTestEnvironment.Checked = true;
                    this.ChangeTaipeiServer();
                    this.Account_Valid();
                    this.ok.Focus();
                    this.ok.Select();
                }

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
            UserInfo userInfo = new UserInfo();
            this.result = UserLogin(act, pwd, loginFactory, ref userInfo);

            if (!this.result)
            {
                this.ShowErr(this.result);
                return;
            }

            // mfa 驗證
            bool isNeedOTPFty = MyUtility.Convert.GetBool(MyUtility.GetValue.Lookup("Select isNeedOTPFty from system"));
            if (isNeedOTP && isNeedOTPFty)
            {
                var otpForm = new OTPVerification(userInfo);
                DialogResult otpDialog = otpForm.ShowDialog();
                if (otpDialog == DialogResult.Cancel)
                {
                    return;
                }
            }

            user = userInfo;
            if (!(this.result = this.app.DoLogin(user)))
            {
                this.ShowErr(this.result);
                return;
            }

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

        private bool Account_Valid()
        {
            this.comboBox1.DataSource = null;
            if (MyUtility.Check.Empty(this.act.Text.Trim()))
            {
                return true;
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
                return false;
            }

            if (dtPass1.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Account does not exist!");
                return false;
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

            return true;
        }

        private void Act_Validated(object sender, CancelEventArgs e)
        {
            if (this.Account_Valid() == false)
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// User Login
        /// </summary>
        /// <param name="userid">User ID</param>
        /// <param name="pwd">PWD</param>
        /// <param name="factoryID">Factory ID</param>
        /// <param name="userInfo">User Info</param>
        /// <returns>DualResult</returns>
        public static DualResult UserLogin(string userid, string pwd, string factoryID, ref UserInfo userInfo)
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

            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@ID", userid),
                new SqlParameter("@Password", pwd),
            };
            if (!MyUtility.Check.Seek("Select * From Pass1 Where ID = @ID And Password = @Password", parameters, out DataRow drPass1))
            {
                return new DualResult(false, "Account or password invalid");
            }

            userInfo = new UserInfo
            {
                UserID = drPass1.Field<string>("ID"),
                UserPassword = drPass1.Field<string>("Password"),
                UserName = drPass1.Field<string>("Name"),
                IsAdmin = drPass1.Field<bool?>("IsAdmin").GetValueOrDefault(false),
                IsMIS = drPass1.Field<bool?>("IsMIS").GetValueOrDefault(false),
                FactoryList = drPass1.Field<string>("Factory"),
                MailAddress = drPass1.Field<string>("EMAIL"),
                Factory = factoryID,
                Keyword = keyword,
                LoginName = drPass1.Field<string>("ADAccount"),
            };

            isNeedOTP = MyUtility.Convert.GetBool(drPass1["IsNeedOTP"]);

            #region 登入時將SYSTEM資料表相關設定載入Sci.Env.Cfg中
            bool isLoginCheckADAccount = true;
            DataRow drSystem;
            if (!MyUtility.Check.Seek("select * from system", out drSystem, "Production"))
            {
                return new DualResult(false, "Get system data fail !!");
            }
            else
            {
                Env.Cfg.MailServerIP = drSystem["Mailserver"].ToString().Trim();
                Env.Cfg.MailFrom = drSystem["Sendfrom"].ToString().Trim();
                Env.Cfg.MailServerAccount = drSystem["EmailID"].ToString().Trim();
                Env.Cfg.MailServerPassword = drSystem["EmailPwd"].ToString().Trim();
                Env.Cfg.SFTP_Server_IP = drSystem["SFtpIP"].ToString().Trim();
                Env.Cfg.SFTP_Server_Port = ushort.Parse(drSystem["SFtpPort"].ToString());
                Env.Cfg.SFTP_Server_Account = drSystem["SFtpID"].ToString().Trim();
                Env.Cfg.SFTP_Server_Password = drSystem["SFtpPwd"].ToString().Trim();

                // Env.Cfg.FtpServerIP = drSystem["FtpIP"].ToString().Trim();
                // Env.Cfg.FtpServerAccount = drSystem["FtpID"].ToString().Trim();
                // Env.Cfg.FtpServerPassword = drSystem["FtpPwd"].ToString().Trim();
                Env.Cfg.ClipDir = drSystem["ClipPath"].ToString().Trim();
                Env.Cfg.MailServerPort = MyUtility.Check.Empty(drSystem["MailServerPort"]) ? Convert.ToUInt16(25) : Convert.ToUInt16(drSystem["MailServerPort"]);
                isLoginCheckADAccount = drSystem.Field<bool>("IsLoginCheckADAccount");
            }
            #endregion
            #region AD驗證
            if (ConfigurationManager.AppSettings["TaipeiServer"] == string.Empty && isLoginCheckADAccount)
            {
                if (string.IsNullOrEmpty(drPass1.Field<string>("ADAccount")))
                {
                    return new DualResult(false, "AD Account is empty, please check with local IT.");
                }

                try
                {
                    string region = DBProxy.Current.DefaultModuleName
                        .Replace("_Formal", string.Empty)
                        .Replace("_Dummy", string.Empty)
                        .Replace("_Training", string.Empty)
                        .Replace("PMSDB_", string.Empty)
                        .Replace("testing_", string.Empty)
                        .Replace("PH1", "PHI");
                    Env.User = userInfo; // dll裡面需要。
                    ADAuthResult adResult = new AuthenticationAPI.AuthenticationAD().ADAuthByRegion(region, userInfo.LoginName);
                    if (!adResult.Pass)
                    {
                        return new DualResult(false, adResult.Message);
                    }
                }
                catch (Exception ex)
                {
                    MyUtility.Msg.ErrorBox(ex.Message);
                }
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

        private void ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBox2.SelectedValue == null)
            {
                return;
            }

            DBProxy.Current.DefaultModuleName = this.comboBox2.SelectedValue2.ToString();
            this.comboBox1.DataSource = null;
            this.Account_Valid();
        }

        private void ChangeTaipeiServer()
        {
            List<string> hasConnectionNamedQuery = new List<string>();

            var cfgsection = (CfgSection)ConfigurationManager.GetSection("sci");

            if (cfgsection != null)
            {
                foreach (CfgSection.Module it in cfgsection.Modules)
                {
                    hasConnectionNamedQuery.Add(it.Name);
                }
            }

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
                StaticEntity.LoginRegionList = systemOption;
            }
        }

        private void CheckBoxTestEnvironment_CheckedChanged(object sender, EventArgs e)
        {
            // 保存當前帳密
            string currentAct = this.act.Text;
            string currentPassword = this.pwd.Text;
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
            this.Account_Valid();

            // 將當前帳密給補回去
            this.act.Text = currentAct;
            this.pwd.Text = currentPassword;
        }
    }
}
