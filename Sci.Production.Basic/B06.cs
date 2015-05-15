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
            this.DefaultFilter = "FactoryID = '" + Sci.Env.User.Factory + "'";
        }

        protected override void OnEditAfter()
        {
            base.OnEditAfter();
            this.textBox1.ReadOnly = true;
            this.textBox2.ReadOnly = true;
        }

        protected override void OnNewAfter()
        {
            base.OnNewAfter();
            CurrentMaintain["FactoryID"] = Sci.Env.User.Factory;
        }

        protected override bool OnSaveBefore()
        {
            if (String.IsNullOrWhiteSpace(CurrentMaintain["Year"].ToString()))
            {
                MessageBox.Show("< Year > can not be empty!");
                this.textBox1.Focus();
                return false;
            }

            if (String.IsNullOrWhiteSpace(CurrentMaintain["Month"].ToString()))
            {
                MessageBox.Show("< Monthly > can not be empty!");
                this.textBox2.Focus();
                return false;
            }

            if ((String.IsNullOrWhiteSpace(CurrentMaintain["ActiveManpower"].ToString())) || (double.Parse(CurrentMaintain["ActiveManpower"].ToString()) == 0))
            {
                MessageBox.Show("< Active Manpower > can not be empty!");
                this.numericBox3.Focus();
                return false;
            }
            return base.OnSaveBefore();
        }

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            base.OnValidated(e);
            string textValue = this.textBox1.Text;
            if (!string.IsNullOrWhiteSpace(textValue) && textValue != this.textBox1.OldValue)
            {
                if (!(2015 <= int.Parse(textValue) && int.Parse(textValue) <= 2100))
                {
                    MessageBox.Show("< Year > must be between 2015 ~ 2100");
                    this.textBox1.Text = "";
                    e.Cancel = true;
                    return;
                }
            }
        }

        private void textBox2_Validating(object sender, CancelEventArgs e)
        {
            base.OnValidated(e);
            string textValue = this.textBox2.Text;
            if (!string.IsNullOrWhiteSpace(textValue) && textValue != this.textBox2.OldValue)
            {
                if (!(1 <= int.Parse(textValue) && int.Parse(textValue) <= 12))
                {
                    MessageBox.Show("< Monthly > must be between 1 ~ 12");
                    this.textBox2.Text = "";
                    e.Cancel = true;
                    return;
                }
            }
        }

        private void numericBox2_Validated(object sender, EventArgs e)
        {
            if ((!string.IsNullOrWhiteSpace(this.numericBox2.Text)) && (!string.IsNullOrWhiteSpace(this.numericBox3.Text)))
            {
                if (double.Parse(this.numericBox2.Text) != 0)
                {
                    CurrentMaintain["ManpowerRatio"] = Math.Round(double.Parse(this.numericBox3.Text) / double.Parse(this.numericBox2.Text), 2);
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
    }
}
