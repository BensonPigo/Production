using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Centralized
{
    public partial class Sewing_P11 : Sci.Win.Tems.QueryForm
    {
        private string Type;

        public Sewing_P11(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.txtCentralizedmulitM1.Text = Sci.Env.User.Keyword;
            this.txtCentralizedmulitFactory1.Text = Sci.Env.User.Factory;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Btnlock_Click(object sender, EventArgs e)
        {
            string m = this.txtCentralizedmulitM1.Text;
            string fty = this.txtCentralizedmulitFactory1.Text;
            if (MyUtility.Check.Empty(m) && MyUtility.Check.Empty(fty))
            {
                MyUtility.Msg.WarningBox("Please choose <M> or <Factory>");
                return;
            }

            if (MyUtility.Check.Empty(this.dateLock.Value))
            {
                MyUtility.Msg.WarningBox($"Please set <{this.Type} Date >");
                return;
            }

            string date = ((DateTime)this.dateLock.Value).ToString("d");

            if (((DateTime)this.dateLock.Value).MonthGreaterThan(DateTime.Now))
            {
                MyUtility.Msg.WarningBox($"Set <{this.Type} Date> before {DateTime.Now.ToString("yyyy/MM")}");
                return;
            }

            if (this.rdbtnLock.Checked)
            {
                // call [httppost]: Website:16888/api/Sewing/LockSewingMonthly
            }
            else if (this.rdbtnUnlock.Checked)
            {
                // call [httppost]: Website:16888/api/Sewing/UnlockSewingMonthly
            }
        }

        private void RdbtnLock_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rdbtnLock.Checked)
            {
                this.btnlock.Text = "Lock";
                this.lbDate.Text = "Lock Date";
                this.Type = "Lock";
            }
            else if (this.rdbtnUnlock.Checked)
            {
                this.btnlock.Text = "Unlock";
                this.lbDate.Text = "Unlock Date";
                this.Type = "Unlock";
            }
        }
    }
}
