using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Shipping
{
    public partial class B50 : Sci.Win.Tems.Input1
    {
        public B50(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();

            MyUtility.Tool.SetupCombox(comboBox1, 1, 1, "FABRIC,ACCESSORY,CHEMICAL,MACHINERY,OTHETS");
        }

        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            comboBox1.SelectedIndex = -1;
        }

        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            textBox1.ReadOnly = true;
        }

        protected override bool ClickSaveBefore()
        {
            //檢查必輸欄位
            if (MyUtility.Check.Empty(textBox1.Text))
            {
                MyUtility.Msg.WarningBox("Description of Goods can't empty!!");
                textBox1.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(textBox2.Text))
            {
                MyUtility.Msg.WarningBox("HS Code can't empty!!");
                textBox2.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(comboBox1.Text))
            {
                MyUtility.Msg.WarningBox("Category can't empty!!");
                comboBox1.Focus();
                return false;
            }

            //填入NL Code
            if (IsDetailInserting)
            {
                string nlcode = MyUtility.GetValue.Lookup("select CONVERT(int,isnull(max(NLCode),0)) from KHGoodsHSCode");
                CurrentMaintain["NLCode"] = (MyUtility.Convert.GetInt(nlcode)+1).ToString("00000");
            }

            return base.ClickSaveBefore();
        }
    }
}
