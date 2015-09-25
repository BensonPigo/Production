using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;
using Sci;

namespace Sci.Production.Shipping
{
    public partial class P02_EditFromPO : Sci.Win.Subs.Input2A
    {
        public P02_EditFromPO()
        {
            InitializeComponent();
            MyUtility.Tool.SetupCombox(comboBox1, 2, 1, "4,Material");
            txtsupplier1.TextBox1.ReadOnly = true;
            txtsupplier1.TextBox1.IsSupportEditMode = false;
        }

        //CTN No.
        private void textBox2_Validated(object sender, EventArgs e)
        {
            if (EditMode && textBox2.OldValue != textBox2.Text)
            {
                CurrentData["CTNNo"] = textBox2.Text.Trim();
            }
        }

        protected override bool OnSaveBefore()
        {
            #region 檢查必輸欄位
            if (MyUtility.Check.Empty(CurrentData["CTNNo"]))
            {
                MyUtility.Msg.WarningBox("CTN No. can't empty!");
                textBox2.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentData["Qty"]))
            {
                MyUtility.Msg.WarningBox("Q'ty can't empty!");
                numericBox2.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentData["NW"]))
            {
                MyUtility.Msg.WarningBox("N.W. (kg) can't empty!");
                numericBox3.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentData["Receiver"]))
            {
                MyUtility.Msg.WarningBox("Receiver can't empty!");
                textBox3.Focus();
                return false;
            }
            #endregion

            return true;
        }

        protected override bool OnSavePost()
        {
            DualResult result = DBProxy.Current.Execute(null, PublicPrg.Prgs.ReCalculateExpress(CurrentData["ID"].ToString()));
            if (!result)
            {
                MyUtility.Msg.WarningBox("Re-Calculate fail!! Pls try again.\r\n" + result.ToString());
                return false;
            }
            return true;
        }

        protected override bool OnDeletePost()
        {
            DualResult result = DBProxy.Current.Execute(null, PublicPrg.Prgs.ReCalculateExpress(CurrentData["ID"].ToString()));
            if (!result)
            {
                MyUtility.Msg.WarningBox("Re-Calculate fail!! Pls try again.\r\n" + result.ToString());
                return false;
            }
            return true;
        }
    }
}
