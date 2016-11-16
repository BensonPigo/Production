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
        Sci.Production.Main app;
        DualResult result;

        public Login(Sci.Production.Main app)
        {
            this.app = app;
            InitializeComponent();

            // 測試用登入
            this.act.Text = "MIS";
            this.pwd.Text = "0000";
            this.comboBox1.Text = "MAI";

            ok.Click += ok_Click;
            exit.Click += exit_Click;

            Sci.Production.SCHEMAS.PASS1Row data;
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
            if (!(result = AsyncHelper.Current.DataProcess(this, () =>
            {

                UserInfo u = new UserInfo();
                result = UserLogin(act, pwd, loginFactory, u);


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
                return result;
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
            Sci.Env.App.Text = "Mdivision = " + Sci.Env.User.Keyword;

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
            return result;
        }

    }
}
