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

namespace Sci.Production.Tools
{
    public partial class SwitchFactory : Sci.Win.Tems.QueryForm
    {
        public SwitchFactory(ToolStripMenuItem menuitem) : base(menuitem)
        {
            //int openFormCount = Application.OpenForms.Cast<Form>().Count(openForm => openForm.IsMdiChild);
            //if (openFormCount > 0)
            //{
            //    MyUtility.Msg.WarningBox("Please close all processing forms first!");
            //    return;
            //}

            InitializeComponent();
            
            EditMode = true;
            act.Text = Sci.Env.User.UserID;
            act.Enabled = false;
            pwd.Text = Sci.Env.User.UserPassword;
            pwd.Enabled = false;

            DualResult result;
            DataTable dtPass1;
            string cmd = string.Format("SELECT ID, Factory FROM Pass1 WHERE ID = '{0}'", Sci.Env.User.UserID);
            if (!(result = DBProxy.Current.Select(null, cmd, out dtPass1)))
            {
                MyUtility.Msg.ErrorBox(result.ToString());
                Close();
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
                comboBox1.SelectedValue = Sci.Env.User.Factory;
            }

            ok.Click += (s, e) =>
                {
                    if (MyUtility.Check.Empty((string)this.comboBox1.SelectedValue))
                    {
                        MyUtility.Msg.WarningBox("Please select one factory");
                    }
                    else
                    {
                        UserInfo user = (UserInfo) Sci.Env.User;
                        user.Factory = (string)this.comboBox1.SelectedValue;
                        Env.User = user;
                        Close();
                    }
                };

            exit.Click += (s, e) =>
                {
                    Close();
                };
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            int openFormCount = Application.OpenForms.Cast<Form>().Count(openForm => openForm.IsMdiChild);
            if (openFormCount > 1)
            {
                MyUtility.Msg.WarningBox("Please close all processing forms first!");
                Close();
            }
        }
    }
}
