using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.IE
{
    public partial class B02 : Sci.Win.Tems.Input1
    {
        public B02(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            Dictionary<String, String> comboBox1_RowSource = new Dictionary<string, string>();
            comboBox1_RowSource.Add("LBR", "Line Balancing (%)");
            comboBox1_RowSource.Add("LLER", "Lean Line Eff. (%)");
            comboBox1_RowSource.Add("EFF.", "Efficiency");
            comboBox1_RowSource.Add("COPT", "Changeover Process Time");
            comboBox1_RowSource.Add("COT", "Changeover Time");
            comboBox1.DataSource = new BindingSource(comboBox1_RowSource, null);
            comboBox1.ValueMember = "Key";
            comboBox1.DisplayMember = "Value";
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            if ((CurrentMaintain["Type"].ToString().Trim() == "COPT" || CurrentMaintain["Type"].ToString().Trim() == "COT"))
            {
                this.label6.Text = "Target (min)";
            }
            else
            {
                this.label6.Text = "Target (%)";
            }
        }

        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["EffectiveDate"] = DateTime.Today;
            CurrentMaintain["FactoryID"] = Sci.Env.User.Factory;
        }

        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.dateBox1.ReadOnly = true;
            this.comboBox1.ReadOnly = true;
            this.txtfactory1.ReadOnly = true;
        }

        protected override void ClickCopyAfter()
        {
            base.ClickCopyAfter();
            CurrentMaintain["ID"] = DBNull.Value;
        }

        protected override bool ClickSaveBefore()
        {
            if (String.IsNullOrWhiteSpace(CurrentMaintain["EffectiveDate"].ToString()))
            {
                MessageBox.Show("< Date > can not be empty!");
                this.dateBox1.Focus();
                return false;
            }

            if (String.IsNullOrWhiteSpace(CurrentMaintain["Type"].ToString()))
            {
                MessageBox.Show("< Type > can not be empty!");
                this.comboBox1.Focus();
                return false;
            }

            if (CurrentMaintain["Type"].ToString().Trim() != "COPT" && CurrentMaintain["Type"].ToString().Trim() != "COT")
            {
                if (String.IsNullOrWhiteSpace(CurrentMaintain["FactoryID"].ToString()))
                {
                    MessageBox.Show("< Factory > can not be empty!");
                    this.txtfactory1.Focus();
                    return false;
                }
            }

            if (String.IsNullOrWhiteSpace(CurrentMaintain["ID"].ToString()))
            {
                DateTime effectiveDate = (DateTime)CurrentMaintain["EffectiveDate"];
                string effectiveDateToString = effectiveDate.ToShortDateString();
                string selectCommand = string.Format("select ID from ChgOverTarget where EffectiveDate = '{0}' and FactoryID = '{1}' and Type = '{2}'", effectiveDateToString, CurrentMaintain["FactoryID"].ToString().Trim(), CurrentMaintain["Type"].ToString().Trim());
                if (MyUtility.Check.Seek(selectCommand, null))
                {
                    MessageBox.Show(string.Format("Data is Duplicate!!"));
                    return false;
                }
            }

            return base.ClickSaveBefore();
        }

        private void comboBox1_Validated(object sender, EventArgs e)
        {
            if (this.EditMode)
            {
                if ((CurrentMaintain["Type"].ToString().Trim() == "COPT" || CurrentMaintain["Type"].ToString().Trim() == "COT"))
                {
                    this.txtfactory1.ReadOnly = true;
                    CurrentMaintain["FactoryID"] = "";
                    this.label6.Text = "Target (min)";
                }
                else
                {
                    this.txtfactory1.ReadOnly = false;
                    this.label6.Text = "Target (%)";
                }
            }
        }
    }
}
