using System;
using Ict;
using Sci.Data;
using Sci.Production.PublicPrg;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// P02_EditFromPO
    /// </summary>
    public partial class P02_EditFromPO : Win.Subs.Input2A
    {
        /// <summary>
        /// P02_EditFromPO
        /// </summary>
        public P02_EditFromPO()
        {
            this.InitializeComponent();
            MyUtility.Tool.SetupCombox(this.comboCategory, 2, 1, "4,Material");
            this.txtsupplierID.TextBox1.ReadOnly = true;
            this.txtsupplierID.TextBox1.IsSupportEditMode = false;
        }

        // CTN No.
        private void TxtCTNNo_Validated(object sender, EventArgs e)
        {
            if (this.EditMode && this.txtCTNNo.OldValue != this.txtCTNNo.Text)
            {
                this.CurrentData["CTNNo"] = this.txtCTNNo.Text.Trim();
            }
        }

        /// <inheritdoc/>
        protected override bool OnSaveBefore()
        {
            #region 檢查必輸欄位
            if (MyUtility.Check.Empty(this.CurrentData["CTNNo"]))
            {
                this.txtCTNNo.Focus();
                MyUtility.Msg.WarningBox("CTN No. can't empty!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentData["Qty"]))
            {
                this.numQty.Focus();
                MyUtility.Msg.WarningBox("Q'ty can't empty!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentData["NW"]))
            {
                this.numNW.Focus();
                MyUtility.Msg.WarningBox("N.W. (kg) can't empty!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentData["Receiver"]))
            {
                this.txtReceiver.Focus();
                MyUtility.Msg.WarningBox("Receiver can't empty!");
                return false;
            }

            // 該單Approved / Junk都不允許調整資料
            if (!Prgs.checkP02Status(this.CurrentData["ID"].ToString()))
            {
                return false;
            }
            #endregion

            return true;
        }

        /// <inheritdoc/>
        protected override DualResult OnSavePost()
        {
            DualResult result = DBProxy.Current.Execute(null, Prgs.ReCalculateExpress(MyUtility.Convert.GetString(this.CurrentData["ID"])));
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Re-Calculate fail!! Pls try again.\r\n" + result.ToString());
                return failResult;
            }

            return Ict.Result.True;
        }

        /// <inheritdoc/>
        protected override bool OnDeleteBefore()
        {
            // 該單Approved / Junk都不允許調整資料
            if (!Prgs.checkP02Status(this.CurrentData["ID"].ToString()))
            {
                return false;
            }

            return base.OnDeleteBefore();
        }

        /// <inheritdoc/>
        protected override DualResult OnDeletePost()
        {
            DualResult result = DBProxy.Current.Execute(null, Prgs.ReCalculateExpress(MyUtility.Convert.GetString(this.CurrentData["ID"])));
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Re-Calculate fail!! Pls try again.\r\n" + result.ToString());
                return failResult;
            }

            return Ict.Result.True;
        }
    }
}
