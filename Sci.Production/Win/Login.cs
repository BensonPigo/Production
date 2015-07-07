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

namespace Sci.Production.Win
{
    public partial class Login : Sci.Win.Tools.Base
    {
        public Login(Sci.Production.Main app)
        {
            this.app = app;
            InitializeComponent();

            ok.Click += ok_Click;
            exit.Click += exit_Click;

            Sci.Production.SCHEMAS.PASS1Row data;
        }

        Sci.Production.Main app;


        void ok_Click(object sender, EventArgs e)
        {
            DualResult result;

            string act = this.act.Text;
            string loginFactory = (string)this.comboBox1.SelectedValue;
            if (0 == act.Length)
            {
                ShowErr("Please enter account.");
                this.act.Focus();
                return;
            }
            string pwd = this.pwd.Text;
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
            if (!(result = AsyncHelper.Current.DataProcess(this, () =>
            {
                Sci.Production.SCHEMAS.PASS1Row data;
                if (!(result = Sci.Production.Win.ProjUtils.GetPass1(act, pwd, out data))) return result;
                if (null == data) return new DualResult(false, "Account or password invalid.");

                UserInfo u = new UserInfo();
                u.UserID = act;
                u.UserPassword = pwd;
                if (!data.IsNAMENull()) u.UserName = data.NAME;
                if (!data.IsISADMINNull()) u.IsAdmin = data.ISADMIN;
                //if (!data.IsFACTORYNull()) Sci.Production.ProductionEnv.UserFactories = data.FACTORY;
                u.Factory = loginFactory;
                u.IsMIS = data.ISMIS;
               
                
                // 載入登入人員相關資訊
                //u.AuthorityList = "";
                //u.IsTS = false;
                //u.FactoryList = "";
                //u.Department = "";
                //u.Director = "";
                //u.ProxyList = "";
                //u.MemberList = "";
                //u.SpecialAuthorityList = "";
                //u.BrandList = "";
                //u.MailAddress = "";
               

                // 載入根據登入資訊而異系統參數, 
                //Sci.Env.Cfg.ReportTitle = "XXX"

                user = u;
                return Result.True;
            })))
            {
                ShowErr(result);
                return;
            }

            if (!(result = app.DoLogin(user)))
            {
                ShowErr(result);
                return;
            }

            DialogResult = DialogResult.OK;
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

            DualResult result;
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
                MyUtility.Msg.ErrorBox("Account is not exist!");
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
    }
}
