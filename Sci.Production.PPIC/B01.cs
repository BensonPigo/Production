using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.PPIC
{
    public partial class B01 : Sci.Win.Tems.Input1
    {
        public B01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            Dictionary<String, String> comboBox1_RowSource = new Dictionary<string, string>();
            comboBox1_RowSource.Add("L", "Lacking");
            comboBox1_RowSource.Add("R", "Replacement");
            comboBox1.DataSource = new BindingSource(comboBox1_RowSource, null);
            comboBox1.ValueMember = "Key";
            comboBox1.DisplayMember = "Value";
        }

        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["Type"] = "FL";
        }

        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.textBox1.ReadOnly = true;
        }

        protected override bool ClickSaveBefore()
        {
            if (string.IsNullOrWhiteSpace(CurrentMaintain["ID"].ToString()))
            {
                MessageBox.Show("< ID > can not be empty!");
                this.textBox1.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(CurrentMaintain["Description"].ToString()))
            {
                MessageBox.Show("< Description > can not be empty!");
                this.textBox2.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(CurrentMaintain["TypeForUse"].ToString()))
            {
                MessageBox.Show("< Type > can not be empty!");
                this.comboBox1.Focus();
                return false;
            }
            return base.ClickSaveBefore();
        }
    }
}
