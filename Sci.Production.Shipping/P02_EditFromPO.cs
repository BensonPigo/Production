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
        private void txtCTNNo_Validated(object sender, EventArgs e)
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
                txtCTNNo.Focus();
                MyUtility.Msg.WarningBox("CTN No. can't empty!");
                return false;
            }
            if (MyUtility.Check.Empty(CurrentData["Qty"]))
            {
                numQty.Focus();
                MyUtility.Msg.WarningBox("Q'ty can't empty!");
                return false;
            }
            if (MyUtility.Check.Empty(CurrentData["NW"]))
            {
                numNW.Focus();
                MyUtility.Msg.WarningBox("N.W. (kg) can't empty!");
                return false;
            }
            if (MyUtility.Check.Empty(CurrentData["Receiver"]))
            {
                txtReceiver.Focus();
                MyUtility.Msg.WarningBox("Receiver can't empty!");
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
