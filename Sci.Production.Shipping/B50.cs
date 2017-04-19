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

            MyUtility.Tool.SetupCombox(comboCategory, 1, 1, "FABRIC,ACCESSORY,CHEMICAL,MACHINERY,OTHETS");
        }

        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            comboCategory.SelectedIndex = -1;
        }

        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            txtDescriptionofGoods.ReadOnly = true;
        }

        protected override bool ClickSaveBefore()
        {
            //檢查必輸欄位
            if (MyUtility.Check.Empty(txtDescriptionofGoods.Text))
            {
                MyUtility.Msg.WarningBox("Description of Goods can't empty!!");
                txtDescriptionofGoods.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(txtHSCode.Text))
            {
                MyUtility.Msg.WarningBox("HS Code can't empty!!");
                txtHSCode.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(comboCategory.Text))
            {
                MyUtility.Msg.WarningBox("Category can't empty!!");
                comboCategory.Focus();
                return false;
            }

            //填入NL Code
            if (IsDetailInserting)
            {
                string nlcode = MyUtility.GetValue.Lookup("select CONVERT(int,isnull(max(NLCode),0)) from KHGoodsHSCode WITH (NOLOCK) ");
                CurrentMaintain["NLCode"] = (MyUtility.Convert.GetInt(nlcode)+1).ToString("00000");
            }

            return base.ClickSaveBefore();
        }
    }
}
