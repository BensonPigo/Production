﻿using System;
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

namespace Sci.Production.Tools
{
    public partial class SwitchFactory : Sci.Win.Tems.QueryForm
    {
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
            Close();
        }
    }
}
