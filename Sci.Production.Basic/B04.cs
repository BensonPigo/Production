using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Basic
{
    /// <summary>
    /// B04
    /// </summary>
    public partial class B04 : Sci.Win.Tems.Input1
    {
        /// <summary>
        /// B04
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public B04(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// OnDetailEntered
        /// </summary>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();

            // 按鈕Accounting chart no變色
            if (MyUtility.Check.Seek(this.CurrentMaintain["ID"].ToString(), "LocalSupp_AccountNo", "ID"))
            {
                this.btnAccountingChartNo.ForeColor = Color.Blue;
            }
            else
            {
                this.btnAccountingChartNo.ForeColor = Color.Black;
            }

            // 按鈕Bank detail變色
            if (MyUtility.Check.Seek(this.CurrentMaintain["ID"].ToString(), "LocalSupp_Bank", "ID"))
            {
                this.btnBankDetail.ForeColor = Color.Blue;
            }
            else
            {
                this.btnBankDetail.ForeColor = Color.Black;
            }
        }

        /// <summary>
        /// ClickEditAfter
        /// </summary>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtCode.ReadOnly = true;
            this.txtAbbreviation.ReadOnly = true;
        }

        /// <summary>
        /// ClickSaveBefore
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["ID"]))
            {
                MyUtility.Msg.WarningBox("< Code > can not be empty!");
                this.txtCode.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["Abb"]))
            {
                MyUtility.Msg.WarningBox("< Abbreviation > can not be empty!");
                this.txtAbbreviation.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["CountryID"]))
            {
                MyUtility.Msg.WarningBox("< Nationality > can not be empty!");
                this.txtCountryNationality.TextBox1.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["Name"]))
            {
                MyUtility.Msg.WarningBox("< Company > can not be empty!");
                this.txtCompany.Focus();
                return false;
            }

            return base.ClickSaveBefore();
        }

        /// <summary>
        /// btnAccountingChartNo_Click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void BtnAccountingChartNo_Click(object sender, EventArgs e)
        {
            Sci.Production.Basic.B04_AccountNo callNextForm = new Sci.Production.Basic.B04_AccountNo(this.IsSupportEdit, this.CurrentMaintain["ID"].ToString(), null, null);
            callNextForm.ShowDialog(this);
            this.OnDetailEntered();
        }

        /// <summary>
        /// btnBankDetail_Click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void BtnBankDetail_Click(object sender, EventArgs e)
        {
            Sci.Production.Basic.B04_BankData callNextForm = new Sci.Production.Basic.B04_BankData(this.IsSupportEdit, this.CurrentMaintain["ID"].ToString(), null, null);
            callNextForm.ShowDialog(this);
            this.OnDetailEntered();
        }
    }
}
