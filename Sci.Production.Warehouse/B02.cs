using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci;
using Sci.Data;

namespace Sci.Production.Warehouse
{
    public partial class B02 : Sci.Win.Tems.Input1
    {
        public B02(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();

            Dictionary<String, String> comboBox1_RowSource = new Dictionary<string, string>();
            comboBox1_RowSource.Add("I", "Inventory");
            comboBox1_RowSource.Add("B", "Bulk");
            comboBox1.DataSource = new BindingSource(comboBox1_RowSource, null);
            comboBox1.ValueMember = "Key";
            comboBox1.DisplayMember = "Value";

        }

        //編輯狀態限制
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.textBox1.ReadOnly = true;
        }

        //存檔前檢查
        protected override bool ClickSaveBefore()
        {
            if (String.IsNullOrWhiteSpace(CurrentMaintain["ID"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Code > can not be empty!");
                this.textBox1.Focus();
                return false;
            }

            if (String.IsNullOrWhiteSpace(CurrentMaintain["Stocktype"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Stock Type > can not be empty!");
                this.comboBox1.Focus();
                return false;
            }

            if (String.IsNullOrWhiteSpace(CurrentMaintain["Description"].ToString()))
            {
                MyUtility.Msg.WarningBox("< Description > can not be empty!");
                this.textBox2.Focus();
                return false;
            }

            return base.ClickSaveBefore();
        }
    }
}
