using System;
using System.Collections.Generic;
using System.Data;
using Ict;
using Sci.Data;

namespace Sci.Production.Subcon
{
    public partial class B01_Quotation : Win.Tems.Input1
    {
        protected DataRow dr;
        protected DataRow dr_detail;
        private bool _canedit;
        private bool _canconfirm;

        public B01_Quotation(bool canedit, DataRow data, bool cancomfirmed)
        {
            this.InitializeComponent();
            this.dr = data;
            this._canedit = canedit;
            this._canconfirm = cancomfirmed;
            string b01_refno = data["refno"].ToString();
            this.DefaultFilter = "refno = '" + b01_refno + "'";

            // 選完Supp後要將回寫CurrencyID
            this.txtsubconSupplier1.TextBox1.Validated += (s, e) =>
            {
                if (this.EditMode && this.txtsubconSupplier1.TextBox1.Text != this.txtsubconSupplier1.TextBox1.OldValue)
                {
                    this.CurrentMaintain["CurrencyID1"] = MyUtility.GetValue.Lookup("CurrencyID", this.txtsubconSupplier1.TextBox1.Text, "LocalSupp", "ID");
                }
            };
            this.txtsubconSupplier2.TextBox1.Validated += (s, e) =>
            {
                if (this.EditMode && this.txtsubconSupplier2.TextBox1.Text != this.txtsubconSupplier2.TextBox1.OldValue)
                {
                    this.CurrentMaintain["CurrencyID2"] = MyUtility.GetValue.Lookup("CurrencyID", this.txtsubconSupplier2.TextBox1.Text, "LocalSupp", "ID");
                }
            };
            this.txtsubconSupplier3.TextBox1.Validated += (s, e) =>
            {
                if (this.EditMode && this.txtsubconSupplier3.TextBox1.Text != this.txtsubconSupplier3.TextBox1.OldValue)
                {
                    this.CurrentMaintain["CurrencyID3"] = MyUtility.GetValue.Lookup("CurrencyID", this.txtsubconSupplier3.TextBox1.Text, "LocalSupp", "ID");
                }
            };
            this.txtsubconSupplier4.TextBox1.Validated += (s, e) =>
            {
                if (this.EditMode && this.txtsubconSupplier4.TextBox1.Text != this.txtsubconSupplier4.TextBox1.OldValue)
                {
                    this.CurrentMaintain["CurrencyID4"] = MyUtility.GetValue.Lookup("CurrencyID", this.txtsubconSupplier4.TextBox1.Text, "LocalSupp", "ID");
                }
            };
        }

        /// <inheritdoc/>
        protected override void EnsureToolbarExt()
        {
            base.EnsureToolbarExt();

            // 讓有[SUBCON][B01]EDIT權限的使用者可以[新增][修改][刪除]
            this.toolbar.cmdNew.Enabled = this._canedit;
            this.toolbar.cmdEdit.Enabled = this._canedit;
            this.toolbar.cmdDelete.Enabled = this._canedit;
            this.toolbar.cmdConfirm.Visible = true;

            if (this.CurrentMaintain != null)
            {
                if (this.tabs.SelectedIndex == 0)
                {
                    this.toolbar.cmdConfirm.Enabled = false;
                }
                else
                {
                    this.toolbar.cmdConfirm.Enabled = this._canconfirm && !this.EditMode && this.CurrentMaintain["Status"].ToString().EqualString("New");
                }
            }
            else
            {
                this.toolbar.cmdConfirm.Enabled = false;
            }
        }

        /// <inheritdoc/>
        protected override bool ClickNewBefore()
        {
            bool flag = false;
            System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter();
            sp1.ParameterName = "@refno";
            sp1.Value = this.dr["refno"].ToString();

            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
            cmds.Add(sp1);

            string sqlcmd = "select refno from localitem_quot WITH (NOLOCK) where refno = @refno and Status ='New'";
            DBProxy.Current.Exists(string.Empty, sqlcmd, cmds, out flag);
            if (flag)
            {
                MyUtility.Msg.WarningBox("Can't add data when data have not been approved.", "Warning");
                return false;
            }

            return base.ClickNewBefore();
        }

        // 新增預設值

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["Refno"] = this.dr["refno"].ToString();
            this.CurrentMaintain["issuedate"] = DateTime.Today;
            this.CurrentMaintain["Status"] = "New";
            this.CurrentMaintain["ChooseSupp"] = 1;
            this.CurrentMaintain["QuotDate1"] = DateTime.Now;
        }

        // 修改前檢查

        /// <inheritdoc/>
        protected override bool ClickEditBefore()
        {
            if (this.CurrentMaintain["Status"].ToString() == "Approved")
            {
                MyUtility.Msg.WarningBox("Record is Approved, can't modify!");
                return false;
            }

            return base.ClickEditBefore();
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            bool flag = false;
            System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter();
            sp1.ParameterName = "@refno";
            sp1.Value = this.dr["refno"].ToString();

            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
            cmds.Add(sp1);

            string sqlcmd = $"select refno from localitem_quot WITH (NOLOCK) where refno = @refno and Status ='New' and ukey <> '{this.CurrentMaintain["ukey"]}'";
            DBProxy.Current.Exists(string.Empty, sqlcmd, cmds, out flag);
            if (flag)
            {
                MyUtility.Msg.WarningBox("Can't add data when data have not been approved.", "Warning");
                return false;
            }

            var suppid = string.Empty;
            var price = 0.0;
            var currencyid = string.Empty;
            DateTime? quotDate = null;

            #region 選取的報價資料
            switch (this.CurrentMaintain["ChooseSupp"].ToString())
            {
                case "1":
                    suppid = this.CurrentMaintain["localsuppid1"].ToString();
                    price = double.Parse(this.CurrentMaintain["price1"].ToString());
                    currencyid = this.CurrentMaintain["currencyid1"].ToString();
                    quotDate = MyUtility.Convert.GetDate(this.CurrentMaintain["QuotDate1"]);
                    break;
                case "2":
                    suppid = this.CurrentMaintain["localsuppid2"].ToString();
                    price = double.Parse(this.CurrentMaintain["price2"].ToString());
                    currencyid = this.CurrentMaintain["currencyid2"].ToString();
                    quotDate = MyUtility.Convert.GetDate(this.CurrentMaintain["QuotDate2"]);
                    break;
                case "3":
                    suppid = this.CurrentMaintain["localsuppid3"].ToString();
                    price = double.Parse(this.CurrentMaintain["price3"].ToString());
                    currencyid = this.CurrentMaintain["currencyid3"].ToString();
                    quotDate = MyUtility.Convert.GetDate(this.CurrentMaintain["QuotDate3"]);
                    break;
                case "4":
                    suppid = this.CurrentMaintain["localsuppid4"].ToString();
                    price = double.Parse(this.CurrentMaintain["price4"].ToString());
                    currencyid = this.CurrentMaintain["currencyid4"].ToString();
                    quotDate = MyUtility.Convert.GetDate(this.CurrentMaintain["QuotDate4"]);
                    break;
            }
            #endregion

            if (string.IsNullOrWhiteSpace(suppid) || string.IsNullOrWhiteSpace(currencyid) || price == 0.0 || MyUtility.Check.Empty(quotDate))
            {
                MyUtility.Msg.WarningBox("Choosed Set of data can't be empty!!");
                return false;
            }

            return base.ClickSaveBefore();
        }

        // 刪除前檢查

        /// <inheritdoc/>
        protected override bool ClickDeleteBefore()
        {
            if (this.CurrentMaintain["Status"].ToString() == "Approved")
            {
                MyUtility.Msg.WarningBox("Record is Approved, can't delete!");
                return false;
            }

            return base.ClickDeleteBefore();
        }

        // refresh

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
        }

        /// <inheritdoc/>
        protected override void ClickConfirm()
        {
            var suppid = string.Empty;
            var price = 0.0;
            var currencyid = string.Empty;

            #region 選取的報價資料
            switch (this.CurrentMaintain["ChooseSupp"].ToString())
            {
                case "1":
                    suppid = this.CurrentMaintain["localsuppid1"].ToString();
                    price = double.Parse(this.CurrentMaintain["price1"].ToString());
                    currencyid = this.CurrentMaintain["currencyid1"].ToString();
                    break;
                case "2":
                    suppid = this.CurrentMaintain["localsuppid2"].ToString();
                    price = double.Parse(this.CurrentMaintain["price2"].ToString());
                    currencyid = this.CurrentMaintain["currencyid2"].ToString();
                    break;
                case "3":
                    suppid = this.CurrentMaintain["localsuppid3"].ToString();
                    price = double.Parse(this.CurrentMaintain["price3"].ToString());
                    currencyid = this.CurrentMaintain["currencyid3"].ToString();
                    break;
                case "4":
                    suppid = this.CurrentMaintain["localsuppid4"].ToString();
                    price = double.Parse(this.CurrentMaintain["price4"].ToString());
                    currencyid = this.CurrentMaintain["currencyid4"].ToString();
                    break;
            }
            #endregion

            if (string.IsNullOrWhiteSpace(suppid) || string.IsNullOrWhiteSpace(currencyid) || price == 0.0)
            {
                MyUtility.Msg.WarningBox("Choosed Set of data can't be empty!!");
                return;
            }

            DataTable dt;
            var s = new B01_BatchApprove(this.Reload);
            DualResult result = DBProxy.Current.Select(string.Empty, s.Sqlcmd(MyUtility.Convert.GetString(this.CurrentMaintain["Refno"]), MyUtility.Convert.GetString(this.CurrentMaintain["ukey"])), out dt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            if (!s.Confirm(dt))
            {
                return;
            }

            base.ClickConfirm();
        }

        public void Reload()
        {
            this.ReloadDatas();
            this.RenewData();
        }
    }
}
