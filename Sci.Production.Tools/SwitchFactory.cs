using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using System.IO;
using System.Configuration;
using System.Xml.Linq;

namespace Sci.Production.Tools
{
    public partial class SwitchFactory : Sci.Win.Tems.QueryForm
    {
        string OriginalDatasource;
        public SwitchFactory(ToolStripMenuItem menuitem) : base(menuitem)
        {
            InitializeComponent();
  
            EditMode = true;
            txtAccount.Text = Sci.Env.User.UserID;
            txtAccount.Enabled = false;
            txtPassword.Text = Sci.Env.User.UserPassword;
            txtPassword.Enabled = false;

            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            int openFormCount = Application.OpenForms.Cast<Form>().Count(openForm => openForm.IsMdiChild);
            if (openFormCount > 1)
            {
                MyUtility.Msg.WarningBox("Please close all processing forms first!");
                Close();
                return;
            }

            if (ConfigurationManager.AppSettings["TaipeiServer"] != "")
            {
                OriginalDatasource = DBProxy.Current.DefaultModuleName;
                //Assembly a = typeof(Module1).Assembly;
                label4.Visible = true;
                comboBox2.Visible = true;
                this.checkBoxTestEnvironment.Visible = true;

                if (OriginalDatasource.Contains("testing_"))
                {
                    this.checkBoxTestEnvironment.Checked = true;
                }

                ChangeTaipeiServer();
                comboBox2.SelectedValue = OriginalDatasource;
            }


            DualResult result;
            DataTable dtPass1;
            string cmd = string.Format("SELECT ID, Factory FROM Pass1 WHERE ID = '{0}'", Sci.Env.User.UserID);
            if (!(result = DBProxy.Current.Select(null, cmd, out dtPass1)))
            {
                MyUtility.Msg.ErrorBox(result.ToString());
                Close();
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
                comboFactory.DataSource = new BindingSource(factoryOption, null);
                comboFactory.ValueMember = "Key";
                comboFactory.DisplayMember = "Value";
                comboFactory.SelectedValue = Sci.Env.User.Factory;
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {

            DataTable dtFactory;
            DualResult result;
            if (MyUtility.Check.Empty((string)this.comboFactory.SelectedValue))
            {
                MyUtility.Msg.WarningBox("Please select one factory");
            }
            else
            {
                UserInfo user = (UserInfo)Sci.Env.User;
                string newFactory = (string)this.comboFactory.SelectedValue;
                bool isFactoryChanged = !newFactory.EqualString(Sci.Env.User.Factory);
                //if (!(result = DBProxy.Current.Select(null, string.Format("SELECT id FROM MDivision WHERE ID = '{0}'", (string)this.comboBox1.SelectedValue), out dtFactory)))
                if (!(result = DBProxy.Current.Select(null, string.Format("SELECT MDivisionid FROM Factory WHERE ID = '{0}'",newFactory), out dtFactory)))
                {
                    ShowErr(result.ToString());
                    return;
                }
                //if (dtFactory.Rows.Count > 0 && !MyUtility.Check.Empty(dtFactory.Rows[0]["id"].ToString()))

                if (dtFactory.Rows.Count > 0 && !MyUtility.Check.Empty(dtFactory.Rows[0]["MDivisionid"].ToString()))
                {
                    //user.Keyword = dtFactory.Rows[0]["id"].ToString();
                    user.Keyword = dtFactory.Rows[0]["MDivisionid"].ToString();
                }
                else
                {
                    ShowErr("MDivisionid is not exist!");
                    return;
                }
                user.Factory = (string)this.comboFactory.SelectedValue;
                this.DialogResult = isFactoryChanged 
                        ? System.Windows.Forms.DialogResult.OK
                        : System.Windows.Forms.DialogResult.Cancel ;
                Env.User = user;
                //Sci.Env.App.Text = string.Format("Production Management System-({2})-{0}-({1})", Sci.Env.User.Factory, Sci.Env.User.UserID, Environment.MachineName);
                var appVerText = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(3);
                var appDirName = new DirectoryInfo(Application.StartupPath).Name;
                string userData = string.Format("-{0}-({1}))"
                    , Sci.Env.User.Factory
                    , Sci.Env.User.UserID);

                Sci.Env.App.Text = ConfigurationManager.AppSettings["formTextSufix"] + userData;
                
                Close();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {

            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            if (ConfigurationManager.AppSettings["TaipeiServer"] != "")
            {
                DBProxy.Current.DefaultModuleName = OriginalDatasource;
                if (OriginalDatasource.Contains("PMSDB"))
                {
                    ConfigurationManager.AppSettings["TaipeiServer"] = ConfigurationManager.AppSettings["PMSDBServer"];
                    ConfigurationManager.AppSettings["ServerMatchFactory"] = ConfigurationManager.AppSettings["PMSDBServerMatchFactory"];
                }
                else
                {
                    ConfigurationManager.AppSettings["TaipeiServer"] = ConfigurationManager.AppSettings["TestingServer"];
                    ConfigurationManager.AppSettings["ServerMatchFactory"] = ConfigurationManager.AppSettings["TestingServerMatchFactory"];
                }
            }

            Close();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedValue == null) return;
            DBProxy.Current.DefaultModuleName = comboBox2.SelectedValue2.ToString();
            DualResult result;
            DataTable dtPass1;
            DataRow drpass1;
            string cmd = string.Format("SELECT ID, Factory FROM Pass1 WHERE ID = '{0}'", Sci.Env.User.UserID);
            if (!(result = DBProxy.Current.Select(null, cmd, out dtPass1)))
            {
                Close();
                MyUtility.Msg.ErrorBox(result.ToString());
                return;
            }
            if (!MyUtility.Check.Seek(cmd, out drpass1))
            {
                MyUtility.Msg.WarningBox("Account does not exist!");
                comboFactory.Text = "";
                comboFactory.DataSource = null;
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
                comboFactory.ValueMember = "Key";
                comboFactory.DisplayMember = "Value";
                comboFactory.DataSource = new BindingSource(factoryOption, null);
            }
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

        private void checkBoxTestEnvironment_CheckedChanged(object sender, EventArgs e)
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

            ChangeTaipeiServer();
        }
    }
}
