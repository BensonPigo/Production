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
        }

        Sci.Production.Main app;


        void ok_Click(object sender, EventArgs e)
        {
            DualResult result;

            string act = this.act.Text;
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
                if (!data.IsFACTORYNull()) u.Factory = data.FACTORY;

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
    }
}
