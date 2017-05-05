using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Basic
{
    public partial class B06 : Sci.Win.Tems.Input1
    {
        public B06(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.DefaultFilter = "MdivisionID = '" + Sci.Env.User.Keyword + "'";
        }

        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtYear.ReadOnly = true;
            this.txtMonthly.ReadOnly = true;
        }

        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["FactoryID"] = Sci.Env.User.Factory;
        }

        protected override bool ClickSaveBefore()
        {
            if (String.IsNullOrWhiteSpace(CurrentMaintain["Year"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Year > can not be empty!");
                this.txtYear.Focus();
                return false;
            }

            if (String.IsNullOrWhiteSpace(CurrentMaintain["Month"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Monthly > can not be empty!");
                this.txtMonthly.Focus();
                return false;
            }

            if ((String.IsNullOrWhiteSpace(CurrentMaintain["ActiveManpower"].ToString())) || (double.Parse(CurrentMaintain["ActiveManpower"].ToString()) == 0))
            {
                MyUtility.Msg.WarningBox("< Active Manpower > can not be empty!");
                this.numActiveManpower.Focus();
                return false;
            }
            CurrentMaintain["MDivisionID"] = MyUtility.GetValue.Lookup(string.Format("select distinct f.MDivisionID from Manpower m left join factory f on m.FactoryID=f.ID where m.FactoryID= '{0}'", CurrentMaintain["FactoryID"]), null);
            return base.ClickSaveBefore();
        }

        private void txtYear_Validating(object sender, CancelEventArgs e)
        {
            base.OnValidated(e);
            string textValue = this.txtYear.Text;
            if (!string.IsNullOrWhiteSpace(textValue) && textValue != this.txtYear.OldValue)
            {
                int n;
                if (!int.TryParse(txtYear.Text, out n))
                {
                    MyUtility.Msg.WarningBox("< Year > must be between 2015 ~ 2100");
                    this.txtYear.Text = "";
                    e.Cancel = true;
                    return;
                }
                if (!(2015 <= int.Parse(textValue) && int.Parse(textValue) <= 2100))
                    {
                        MyUtility.Msg.WarningBox("< Year > must be between 2015 ~ 2100");
                        this.txtYear.Text = "";
                        e.Cancel = true;
                        return;
                    }
            }
        }

        private void txtMonthly_Validating(object sender, CancelEventArgs e)
        {
            base.OnValidated(e);
            string textValue = this.txtMonthly.Text;
            if (!string.IsNullOrWhiteSpace(textValue) && textValue != this.txtMonthly.OldValue)
            {
                int n;
                if (!int.TryParse(txtMonthly.Text, out n))
                {
                    MyUtility.Msg.WarningBox("< Monthly > must be between 1 ~ 12");
                    this.txtMonthly.Text = "";
                    e.Cancel = true;
                    return;
                }
                if (!(1 <= int.Parse(textValue) && int.Parse(textValue) <= 12))
                {
                    MyUtility.Msg.WarningBox("< Monthly > must be between 1 ~ 12");
                    this.txtMonthly.Text = "";
                    e.Cancel = true;
                    return;
                }
            }
        }

        private void numActiveManpower_Validated(object sender, EventArgs e)
        {
            if ((!string.IsNullOrWhiteSpace(this.numericBox2.Text)) && (!string.IsNullOrWhiteSpace(this.numActiveManpower.Text)))
            {
                if (double.Parse(this.numericBox2.Text) != 0)
                {
                    CurrentMaintain["ManpowerRatio"] = Math.Round(double.Parse(this.numActiveManpower.Text) / double.Parse(this.numericBox2.Text), 2);
                }
                else
                {
                    CurrentMaintain["ManpowerRatio"] = 0;
                }
            }
            else
            {
                CurrentMaintain["ManpowerRatio"] = 0;
            }
        }

        private void numericBox7_Validated(object sender, EventArgs e)
        {
            if ((!string.IsNullOrWhiteSpace(this.numericBox7.Text)) && (!string.IsNullOrWhiteSpace(this.numericBox9.Text)))
            {
                if (double.Parse(this.numericBox7.Text) != 0)
                {
                    CurrentMaintain["PPH"] = Math.Round(double.Parse(this.numericBox9.Text) / double.Parse(this.numericBox7.Text), 2);
                }
                else
                {
                    CurrentMaintain["PPH"] = 0;
                }
            }
            else
            {
                CurrentMaintain["PPH"] = 0;
            }
        }

        private void txtFactory_Validating(object sender, CancelEventArgs e)
        {
            DataTable FactoryData; string fac = "";
            string sqlCmd = "select distinct FactoryID from Manpower WITH (NOLOCK) ";
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out FactoryData);
            foreach (DataRow dr in FactoryData.Rows)
            {
                fac = dr["FactoryID"].ToString();
                if (txtFactory.Text == fac) { return; }
            }
            if (txtFactory.Text == "")
            {
                txtFactory.Text = "";
                return;
            }
            if (txtFactory.Text != fac)
            {
                MyUtility.Msg.WarningBox("This Factory is wrong!");
                txtFactory.Text = "";
                return;
            }
        }

        private void txtFactory_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sqlCmd = "select distinct FactoryID from Manpower WITH (NOLOCK) ";

            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlCmd, "8", txtFactory.Text, "Factory");
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel) { return; }
            txtFactory.Text = item.GetSelectedString();
        }


    }
}
