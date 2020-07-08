using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Sci.Production.Basic
{
    /// <summary>
    /// B06
    /// </summary>
    public partial class B06 : Win.Tems.Input1
    {
        /// <summary>
        /// B06
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public B06(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = "MdivisionID = '" + Sci.Env.User.Keyword + "'";
        }

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtYear.ReadOnly = true;
            this.txtMonthly.ReadOnly = true;
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["FactoryID"] = Sci.Env.User.Factory;
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            if (string.IsNullOrWhiteSpace(this.CurrentMaintain["Year"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Year > can not be empty!");
                this.txtYear.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(this.CurrentMaintain["Month"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Monthly > can not be empty!");
                this.txtMonthly.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(this.CurrentMaintain["ActiveManpower"].ToString()) || (double.Parse(this.CurrentMaintain["ActiveManpower"].ToString()) == 0))
            {
                MyUtility.Msg.WarningBox("< Active Manpower > can not be empty!");
                this.numActiveManpower.Focus();
                return false;
            }

            this.CurrentMaintain["MDivisionID"] = MyUtility.GetValue.Lookup(string.Format("select distinct f.MDivisionID from Manpower m left join factory f on m.FactoryID=f.ID where m.FactoryID= '{0}'", this.CurrentMaintain["FactoryID"]), null);
            return base.ClickSaveBefore();
        }

        private void TxtYear_Validating(object sender, CancelEventArgs e)
        {
            this.OnValidated(e);
            string textValue = this.txtYear.Text;
            if (!string.IsNullOrWhiteSpace(textValue) && textValue != this.txtYear.OldValue)
            {
                int n;
                if (!int.TryParse(this.txtYear.Text, out n))
                {
                    this.txtYear.Text = string.Empty;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox("< Year > must be between 2015 ~ 2100");
                    return;
                }

                if (!(int.Parse(textValue) >= 2015 && int.Parse(textValue) <= 2100))
                    {
                        this.txtYear.Text = string.Empty;
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox("< Year > must be between 2015 ~ 2100");
                        return;
                    }
            }
        }

        private void TxtMonthly_Validating(object sender, CancelEventArgs e)
        {
            this.OnValidated(e);
            string textValue = this.txtMonthly.Text;
            if (!string.IsNullOrWhiteSpace(textValue) && textValue != this.txtMonthly.OldValue)
            {
                int n;
                if (!int.TryParse(this.txtMonthly.Text, out n))
                {
                    this.txtMonthly.Text = string.Empty;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox("< Monthly > must be between 1 ~ 12");
                    return;
                }

                if (!(int.Parse(textValue) >= 1 && int.Parse(textValue) <= 12))
                {
                    this.txtMonthly.Text = string.Empty;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox("< Monthly > must be between 1 ~ 12");
                    return;
                }
            }
        }

        private void NumActiveManpower_Validated(object sender, EventArgs e)
        {
            if ((!string.IsNullOrWhiteSpace(this.numericBox2.Text)) && (!string.IsNullOrWhiteSpace(this.numActiveManpower.Text)))
            {
                if (double.Parse(this.numericBox2.Text) != 0)
                {
                    this.CurrentMaintain["ManpowerRatio"] = Math.Round(double.Parse(this.numActiveManpower.Text) / double.Parse(this.numericBox2.Text), 2);
                }
                else
                {
                    this.CurrentMaintain["ManpowerRatio"] = 0;
                }
            }
            else
            {
                this.CurrentMaintain["ManpowerRatio"] = 0;
            }
        }

        private void NumericBox7_Validated(object sender, EventArgs e)
        {
            if ((!string.IsNullOrWhiteSpace(this.numericBox7.Text)) && (!string.IsNullOrWhiteSpace(this.numericBox9.Text)))
            {
                if (double.Parse(this.numericBox7.Text) != 0)
                {
                    this.CurrentMaintain["PPH"] = Math.Round(double.Parse(this.numericBox9.Text) / double.Parse(this.numericBox7.Text), 2);
                }
                else
                {
                    this.CurrentMaintain["PPH"] = 0;
                }
            }
            else
            {
                this.CurrentMaintain["PPH"] = 0;
            }
        }
    }
}
