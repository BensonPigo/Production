using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.IE
{
    public partial class B01 : Sci.Win.Tems.Input1
    {
        public B01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            Dictionary<String, String> comboBox1_RowSource = new Dictionary<string, string>();
            comboBox1_RowSource.Add("A", "All");
            comboBox1_RowSource.Add("N", "New");
            comboBox1_RowSource.Add("R", "Repeat");
            comboBox1.DataSource = new BindingSource(comboBox1_RowSource, null);
            comboBox1.ValueMember = "Key";
            comboBox1.DisplayMember = "Value";
        }

        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["UseFor"] = "A";
            CurrentMaintain["BaseOn"] = 1;
        }

        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.textBox1.ReadOnly = true;
        }

        protected override bool ClickSaveBefore()
        {
            if (String.IsNullOrWhiteSpace(CurrentMaintain["ID"].ToString()))
            {
                MessageBox.Show("< Code > can not be empty!");
                this.textBox1.Focus();
                return false;
            }

            if (String.IsNullOrWhiteSpace(CurrentMaintain["Description"].ToString()))
            {
                MessageBox.Show("< Activities > can not be empty!");
                this.textBox2.Focus();
                return false;
            }

            if (String.IsNullOrWhiteSpace(CurrentMaintain["UseFor"].ToString()))
            {
                MessageBox.Show("< New/Repeat > can not be empty!");
                this.comboBox1.Focus();
                return false;
            }
            return base.ClickSaveBefore();
        }
    }
}
