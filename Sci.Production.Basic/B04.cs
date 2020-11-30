using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.PublicPrg;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Sci.Production.Basic
{
    /// <summary>
    /// B04
    /// </summary>
    public partial class B04 : Win.Tems.Input1
    {
        /// <summary>
        /// B04
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public B04(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();

            this.label2.ForeColor = Color.Black;

            // this.label2.BackColor = Color.White;
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

            string sqlcmd = $@"
SELECT * FROM LocalSupp_Bank_Detail WHERE Pkey=(
SELECT TOP 1 PKEY FROM LocalSupp_Bank WITH (NOLOCK) WHERE ID = '{this.CurrentMaintain["ID"]}' AND Status = 'Confirmed'ORDER BY ApproveDate DESC
)";
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out DataTable dt);

            this.chkPayByChk.Checked = MyUtility.GetValue.Lookup($"SELECT TOP 1 ByCheck FROM LocalSupp_Bank WITH (NOLOCK) WHERE ID = '{this.CurrentMaintain["ID"]}' AND Status = 'Confirmed'ORDER BY ApproveDate DESC") == "True" ? true : false;

            this.listControlBindingSource1.DataSource = dt;

            this.gridBankDetail.Columns.Clear();
            this.Helper.Controls.Grid.Generator(this.gridBankDetail)
                .CheckBox("IsDefault", header: "Default", width: Widths.AnsiChars(5), trueValue: 1, falseValue: 0, iseditable: false)
                .Text("AccountNo", header: "Account No.", width: Widths.AnsiChars(13))
                .Text("SWIFTCode", header: "Swift", width: Widths.AnsiChars(13))
                .Text("AccountName", header: "Account Name", width: Widths.AnsiChars(13))
                .Text("BankName", header: "Bank Name", width: Widths.AnsiChars(13))
                .Text("BranchCode", header: "Branch Code", width: Widths.AnsiChars(13))
                .Text("BranchName", header: "Branch Name", width: Widths.AnsiChars(13))
                .Text("CountryID", header: "Country", width: Widths.AnsiChars(13))
                .Text("Alias", header: "Country Name", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("City", header: "City", width: Widths.AnsiChars(13))
                .Text("MidSWIFTCode", header: "Intermediary Bank", width: Widths.AnsiChars(13))
                .Text("MidBankName", header: "Intermediary Bank-SWIFT Code", width: Widths.AnsiChars(13))
                .Text("Remark", header: "Remark", width: Widths.AnsiChars(13))
                ;
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

            string cmd = $@"SELECT 1 FROM LocalSupp WHERE ID != '{this.CurrentMaintain["ID"]}' AND TaxNo != '' AND TaxNo='{this.CurrentMaintain["TaxNo"]}' AND CurrencyID='{this.CurrentMaintain["CurrencyID"]}'";

            if (MyUtility.Check.Seek(cmd))
            {
                MyUtility.Msg.InfoBox("Tax No shouldn't has same Currency used on different Supplier code.");
                return false;
            }

            cmd = $"SELECT 1 FROM LocalSupp WHERE TaxNo='{this.CurrentMaintain["TaxNo"]}' AND TaxNo != '' AND Abb != '{this.CurrentMaintain["Abb"]}'";
            if (MyUtility.Check.Seek(cmd))
            {
                MyUtility.Msg.InfoBox("Tax No can only be used on same Supplier Abbreviation.");
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
            B04_AccountNo callNextForm = new B04_AccountNo(this.IsSupportEdit, this.CurrentMaintain["ID"].ToString(), null, null);
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
            B04_BankDetail callNextForm = new B04_BankDetail(Prgs.GetAuthority(Env.User.UserID, "B04. Supplier/Sub Con (Local)", "CanEdit"), this.CurrentMaintain["ID"].ToString(), null, null, this.Perm.Confirm, null);

            // Sci.Production.Basic.B04_BankDetail callNextForm = new Sci.Production.Basic.B04_BankDetail(new ToolStripMenuItem(), this.CurrentMaintain["ID"].ToString());
            callNextForm.ShowDialog(this);
            this.OnDetailEntered();
        }

        /// <summary>
        /// B04_FormLoaded
        /// </summary>
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
            DBProxy.Current.Execute(null, $"UPDATE LocalSupp SET Status = 'Junked',Junk=1,EditDate=GETDATE(),EditName='{Env.User.UserID}' WHERE ID='{this.CurrentMaintain["ID"]}'");
            MyUtility.Msg.InfoBox("Success!");
            this.RenewData();
        }

        /// <inheritdoc/>
        protected override void ClickUnJunk()
        {
            base.ClickUnJunk();
            DBProxy.Current.Execute(null, $"UPDATE LocalSupp SET Status = 'New' ,Junk=0,EditDate=GETDATE(),EditName='{Env.User.UserID}' WHERE ID='{this.CurrentMaintain["ID"]}'");
            MyUtility.Msg.InfoBox("Success!");
            this.RenewData();
        }

        private Form batchapprove;

        private void BtnBatchApprove_Click(object sender, EventArgs e)
        {
            if (!this.Perm.Confirm)
            {
                MyUtility.Msg.WarningBox("You don't have permission to confirm.");
                return;
            }

            if (this.batchapprove == null || this.batchapprove.IsDisposed)
            {
                this.batchapprove = new B04_BatchApprove(this.Reload);
                this.batchapprove.Show();
            }
            else
            {
                this.batchapprove.Activate();
            }
        }

        /// <inheritdoc/>
        public void Reload()
        {
            this.ReloadDatas();
            this.RenewData();
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            this.CurrentMaintain["Status"] = "New";
            base.ClickNewAfter();
        }
    }
}
