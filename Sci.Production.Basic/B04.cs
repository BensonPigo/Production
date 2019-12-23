﻿using Sci.Data;
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
        private bool Junk;
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

            this.JunkSwitch();
        }

        /// <summary>
        /// ClickEditAfter
        /// </summary>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtCode.ReadOnly = true;
            this.txtAbbreviation.ReadOnly = false;
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

            if (MyUtility.Check.Empty(this.CurrentMaintain["CurrencyID"]))
            {
                MyUtility.Msg.WarningBox("< Currency > can not be empty!");
                this.txtCurrency.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["PayTermID"]))
            {
                MyUtility.Msg.WarningBox("< Payment Term > can not be empty!");
                this.txtpayterm_ftyPaymentTerm.Focus();
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

        /// <summary>
        /// B04_FormLoaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void B04_FormLoaded(object sender, EventArgs e)
        {
            MyUtility.Tool.SetupCombox(this.queryfors, 2, 1, "0,Exclude Junk,1,Include Junk");

            // 預設查詢為 Exclude Junk
            this.queryfors.SelectedIndex = 0;
            this.DefaultWhere = "JUNK = 0";
            this.ReloadDatas();
        }

        /// <summary>
        /// OnFormLoaded
        /// </summary>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.queryfors.SelectedIndexChanged += (s, e) =>
            {
                string hasJunk = MyUtility.Check.Empty(this.queryfors.SelectedValue) ? string.Empty : this.queryfors.SelectedValue.ToString();
                switch (hasJunk)
                {
                    case "0":
                        this.DefaultWhere = "JUNK = 0";
                        break;
                    case "1":
                        this.DefaultWhere = "JUNK = 1";
                        break;
                    default:
                        this.DefaultWhere = string.Empty;
                        break;
                }
                this.ReloadDatas();
            };
        }

        /// /// <inheritdoc/>
        protected override void ClickJunk()
        {
            base.ClickJunk();
            DBProxy.Current.Execute(null, $"UPDATE LocalSupp SET Junk=1,EditDate=GETDATE(),EditName='{Sci.Env.User.UserID}' WHERE ID='{this.CurrentMaintain["ID"]}'");
            MyUtility.Msg.InfoBox("Success!");
            this.RenewData();
        }

        protected override void ClickUnJunk()
        {
            base.ClickUnJunk();
            DBProxy.Current.Execute(null, $"UPDATE LocalSupp SET Junk=0,EditDate=GETDATE(),EditName='{Sci.Env.User.UserID}' WHERE ID='{this.CurrentMaintain["ID"]}'");
            MyUtility.Msg.InfoBox("Success!");
            this.RenewData();
        }

        /// <summary>
        /// 判斷junk欄位、異動Toolbar
        /// </summary>
        private void JunkSwitch()
        {
            if (this.EditMode || this.CurrentMaintain == null)
            {
                return;
            }

            this.Junk = Convert.ToBoolean(this.CurrentMaintain["Junk"]);
            if (this.Junk)
            {
                this.toolbar.cmdUnJunk.Enabled = true;
                this.toolbar.cmdJunk.Enabled = false;
            }
            else
            {
                this.toolbar.cmdJunk.Enabled = true;
                this.toolbar.cmdUnJunk.Enabled = false;
            }
        }

        protected override void EnsureToolbarExt()
        {
            base.EnsureToolbarExt();
            this.JunkSwitch();
        }
    }
}
