using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Basic
{
    public partial class B13 : Sci.Win.Tems.Input1
    {
        public B13(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();

            Dictionary<String, String> comboBox1_RowSource = new Dictionary<string, string>();
            comboBox1_RowSource.Add("F", "Fabric");
            comboBox1_RowSource.Add("A", "Accessories");
            comboBox1_RowSource.Add("", "");
            comboBox1.DataSource = new BindingSource(comboBox1_RowSource, null);
            comboBox1.ValueMember = "Key";
            comboBox1.DisplayMember = "Value";
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            //在編輯模式下，下列這些欄位都不可以被修改
            if (this.EditMode)
            {
                this.comboBox1.ReadOnly = true;
                this.checkBox1.ReadOnly = true;
                this.checkBox2.ReadOnly = true;
                this.checkBox3.ReadOnly = true;
                this.checkBox4.ReadOnly = true;
            }
        }
    }
}
