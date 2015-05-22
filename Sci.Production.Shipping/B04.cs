using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Sci.Data;
using Ict;

namespace Sci.Production.Shipping
{
    public partial class B04 : Sci.Win.Tems.Input1
    {
        public B04(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            Dictionary<String, String> comboBox1_RowSource = new Dictionary<string, string>();
            comboBox1_RowSource.Add("C", "CBM");
            comboBox1_RowSource.Add("G", "G.W.");
            comboBox1_RowSource.Add(" ", "Number of Deliver Sheets");
            comboBox1.DataSource = new BindingSource(comboBox1_RowSource, null);
            comboBox1.ValueMember = "Key";
            comboBox1.DisplayMember = "Value";
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            //撈Account Name資料
            string selectCommand = string.Format("select Name from AccountNo where ID = '{0}'", CurrentMaintain["AccountNo"].ToString());
            DataTable AccountNoTable;
            DualResult selectResult = DBProxy.Current.Select("Finance", selectCommand, out AccountNoTable);
            if (AccountNoTable != null && AccountNoTable.Rows.Count > 0)
            {
                this.displayBox2.Text = AccountNoTable.Rows[0]["Name"].ToString();
            }
            else
            {
                this.displayBox2.Text = "";
            }
        }
    }
}
