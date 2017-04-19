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
            MyUtility.Tool.SetupCombox(comboCategory, 2, 1, "4,Material");
            txtsupplierID.TextBox1.ReadOnly = true;
            txtsupplierID.TextBox1.IsSupportEditMode = false;
        }

        //CTN No.
        private void textBox2_Validated(object sender, EventArgs e)
        {
            if (EditMode && txtCTNNo.OldValue != txtCTNNo.Text)
            {
                CurrentData["CTNNo"] = txtCTNNo.Text.Trim();
            }
        }

        protected override bool OnSaveBefore()
        {
            #region 檢查必輸欄位
            if (MyUtility.Check.Empty(CurrentData["CTNNo"]))
            {
                MyUtility.Msg.WarningBox("CTN No. can't empty!");
                txtCTNNo.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentData["Qty"]))
            {
                MyUtility.Msg.WarningBox("Q'ty can't empty!");
                numQty.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentData["NW"]))
            {
                MyUtility.Msg.WarningBox("N.W. (kg) can't empty!");
                numNW.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentData["Receiver"]))
            {
                MyUtility.Msg.WarningBox("Receiver can't empty!");
                txtReceiver.Focus();
                return false;
            }
            #endregion

            return true;
        }

        protected override DualResult OnSavePost()
        {
            DualResult result = DBProxy.Current.Execute(null, PublicPrg.Prgs.ReCalculateExpress(MyUtility.Convert.GetString(CurrentData["ID"])));
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Re-Calculate fail!! Pls try again.\r\n" + result.ToString());
                return failResult;
            }
            return Result.True;
        }

        protected override DualResult OnDeletePost()
        {
            DualResult result = DBProxy.Current.Execute(null, PublicPrg.Prgs.ReCalculateExpress(MyUtility.Convert.GetString(CurrentData["ID"])));
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Re-Calculate fail!! Pls try again.\r\n" + result.ToString());
                return failResult;
            }
            return Result.True;
        }
    }
}
