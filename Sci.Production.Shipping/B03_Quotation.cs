using System;
using System.Data;
using Ict;
using Sci.Data;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// B03_Quotation
    /// </summary>
    public partial class B03_Quotation : Sci.Win.Tems.Input1
    {
        private DataRow motherData;
        private bool CanEdit = false;
        private bool _canconfirm;

        /// <summary>
        /// MotherData
        /// </summary>
        protected DataRow MotherData
        {
            get
            {
                return this.motherData;
            }

            set
            {
                this.motherData = value;
            }
        }

        /// <summary>
        /// CanEdit1
        /// </summary>
        public bool CanEdit1
        {
            get
            {
                return this.CanEdit;
            }

            set
            {
                this.CanEdit = value;
            }
        }

        /// <summary>
        /// B03_CanvassRecord
        /// </summary>
        /// <param name="canedit">canedit</param>
        /// <param name="data">data</param>
        public B03_Quotation(bool canedit, DataRow data, bool cancomfirmed)
        {
            this.InitializeComponent();
            this.CanEdit1 = canedit;
            this.MotherData = data;
            this._canconfirm = cancomfirmed;
            this.DefaultFilter = "ID = '" + MyUtility.Convert.GetString(this.MotherData["ID"]).Trim() + "'";
            if (this.CurrentMaintain == null)
            {
                this.label1.Text = string.Empty;
            }

            // this.IsSupportEdit = canedit;
            // 選完Supp後要將回寫CurrencyID
            this.txtsubconSupplier1.TextBox1.Validating += (s, e) =>
                {
                    if (this.EditMode && this.txtsubconSupplier1.TextBox1.Text != this.txtsubconSupplier1.TextBox1.OldValue)
                    {
                        this.CurrentMaintain["CurrencyID1"] = MyUtility.GetValue.Lookup("CurrencyID", this.txtsubconSupplier1.TextBox1.Text, "LocalSupp", "ID");
                    }
                };
        }

        /// <inheritdoc/>
        protected override void EnsureToolbarExt()
        {
            base.EnsureToolbarExt();
            this.toolbar.cmdEdit.Enabled = this.CanEdit1 && !this.EditMode;
            this.toolbar.cmdNew.Enabled = this.CanEdit1 && !this.EditMode;
            this.toolbar.cmdConfirm.Visible = true;

            if (this.CurrentMaintain != null)
            {
                if (this.tabs.SelectedIndex == 0)
                {
                    this.toolbar.cmdConfirm.Enabled = false;
                    this.toolbar.cmdSave.Enabled = false;
                }
                else
                {
                    this.toolbar.cmdConfirm.Enabled = this._canconfirm && !this.EditMode && this.CurrentMaintain["Status"].ToString().EqualString("New");
                    this.toolbar.cmdSave.Enabled = this.CanEdit1 && this.EditMode;
                }
            }
            else
            {
                this.toolbar.cmdConfirm.Enabled = false;
                this.toolbar.cmdSave.Enabled = false;
            }
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.label1.Text = MyUtility.Convert.GetString(this.CurrentMaintain["Status"]);
        }

        /// <inheritdoc/>
        protected override bool ClickNewBefore()
        {
            // 檢查是否還有建立的紀錄尚未被confirm，若有則無法新增資料
            string sqlCmd = $"select ID from ShipExpense_CanVass WITH (NOLOCK) where ID = '{MyUtility.Convert.GetString(this.MotherData["ID"]).Trim()}' and Status = 'New'";
            if (MyUtility.Check.Seek(sqlCmd, null))
            {
                MyUtility.Msg.WarningBox("Still have data not yet confirm, so can't create new record!");
                return false;
            }

            return base.ClickNewBefore();
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["ID"] = MyUtility.Convert.GetString(this.MotherData["ID"]).Trim();
            this.CurrentMaintain["Status"] = "New";
            this.label1.Text = "New";
            this.CurrentMaintain["ChooseSupp"] = 1;
            this.CurrentMaintain["QuotDate1"] = DateTime.Now;
        }

        /// <inheritdoc/>
        protected override bool ClickEditBefore()
        {
            DataRow dr = this.grid.GetDataRow<DataRow>(this.grid.GetSelectedRowIndex());
            if (MyUtility.Convert.GetString(dr["Status"]) == "Confirmed")
            {
                MyUtility.Msg.WarningBox("Record is confirmed, can't modify!");
                return false;
            }

            return base.ClickEditBefore();
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            // 檢查是否還有建立的紀錄尚未被confirm，若有則無法新增資料
            string sqlCmd = $"select ID from ShipExpense_CanVass WITH (NOLOCK) where ID = '{MyUtility.Convert.GetString(this.MotherData["ID"]).Trim()}' and Status = 'New'  and ukey <> '{this.CurrentMaintain["ukey"]}'";
            if (MyUtility.Check.Seek(sqlCmd, null))
            {
                MyUtility.Msg.WarningBox("Still have data not yet confirm, so can't create new record!");
                return false;
            }

            var suppId = string.Empty;
            var price = 0.0;
            var currencyId = string.Empty;
            DateTime? QuotDate = null;

            #region 選取的報價資料
            switch (MyUtility.Convert.GetString(this.CurrentMaintain["ChooseSupp"]))
            {
                case "1":
                    suppId = MyUtility.Convert.GetString(this.CurrentMaintain["LocalSuppID1"]);
                    price = MyUtility.Convert.GetDouble(this.CurrentMaintain["Price1"]);
                    currencyId = MyUtility.Convert.GetString(this.CurrentMaintain["CurrencyID1"]);
                    QuotDate = MyUtility.Convert.GetDate(this.CurrentMaintain["QuotDate1"]);
                    break;
                case "2":
                    suppId = MyUtility.Convert.GetString(this.CurrentMaintain["LocalSuppID2"]);
                    price = MyUtility.Convert.GetDouble(this.CurrentMaintain["Price2"]);
                    currencyId = MyUtility.Convert.GetString(this.CurrentMaintain["CurrencyID2"]);
                    QuotDate = MyUtility.Convert.GetDate(this.CurrentMaintain["QuotDate2"]);
                    break;
                case "3":
                    suppId = MyUtility.Convert.GetString(this.CurrentMaintain["LocalSuppID3"]);
                    price = MyUtility.Convert.GetDouble(this.CurrentMaintain["Price3"]);
                    currencyId = MyUtility.Convert.GetString(this.CurrentMaintain["CurrencyID3"]);
                    QuotDate = MyUtility.Convert.GetDate(this.CurrentMaintain["QuotDate3"]);
                    break;
                case "4":
                    suppId = MyUtility.Convert.GetString(this.CurrentMaintain["LocalSuppID4"]);
                    price = MyUtility.Convert.GetDouble(this.CurrentMaintain["Price4"]);
                    currencyId = MyUtility.Convert.GetString(this.CurrentMaintain["CurrencyID4"]);
                    QuotDate = MyUtility.Convert.GetDate(this.CurrentMaintain["QuotDate4"]);
                    break;
            }
            #endregion

            if (MyUtility.Check.Empty(suppId) || MyUtility.Check.Empty(currencyId) || MyUtility.Check.Empty(price) || MyUtility.Check.Empty(QuotDate))
            {
                MyUtility.Msg.WarningBox("Data can't be empty!!");
                return false;
            }

            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override bool ClickDeleteBefore()
        {
            DataRow dr = this.grid.GetDataRow<DataRow>(this.grid.GetSelectedRowIndex());
            if (MyUtility.Convert.GetString(dr["Status"]) == "Confirmed")
            {
                MyUtility.Msg.WarningBox("Record is confirmed, can't delete!");
                return false;
            }

            return base.ClickDeleteBefore();
        }

        /// <inheritdoc/>
        protected override void ClickConfirm()
        {
            base.ClickConfirm();

            var currentMaintain = this.CurrentMaintain;
            if (currentMaintain == null)
            {
                return;
            }

            var suppId = string.Empty;
            var price = 0.0;
            var currencyId = string.Empty;

            #region 選取的報價資料
            switch (MyUtility.Convert.GetString(this.CurrentMaintain["ChooseSupp"]))
            {
                case "1":
                    suppId = MyUtility.Convert.GetString(this.CurrentMaintain["LocalSuppID1"]);
                    price = MyUtility.Convert.GetDouble(this.CurrentMaintain["Price1"]);
                    currencyId = MyUtility.Convert.GetString(this.CurrentMaintain["CurrencyID1"]);
                    break;
                case "2":
                    suppId = MyUtility.Convert.GetString(this.CurrentMaintain["LocalSuppID2"]);
                    price = MyUtility.Convert.GetDouble(this.CurrentMaintain["Price2"]);
                    currencyId = MyUtility.Convert.GetString(this.CurrentMaintain["CurrencyID2"]);
                    break;
                case "3":
                    suppId = MyUtility.Convert.GetString(this.CurrentMaintain["LocalSuppID3"]);
                    price = MyUtility.Convert.GetDouble(this.CurrentMaintain["Price3"]);
                    currencyId = MyUtility.Convert.GetString(this.CurrentMaintain["CurrencyID3"]);
                    break;
                case "4":
                    suppId = MyUtility.Convert.GetString(this.CurrentMaintain["LocalSuppID4"]);
                    price = MyUtility.Convert.GetDouble(this.CurrentMaintain["Price4"]);
                    currencyId = MyUtility.Convert.GetString(this.CurrentMaintain["CurrencyID4"]);
                    break;
            }
            #endregion

            if (MyUtility.Check.Empty(suppId) || MyUtility.Check.Empty(currencyId) || MyUtility.Check.Empty(price))
            {
                MyUtility.Msg.WarningBox("Choosed Set of data can't be empty!!");
                return;
            }

            DataTable dt;
            var s = new B03_BatchApprove(this.Reload);
            DualResult result = DBProxy.Current.Select(string.Empty, s.Sqlcmd(MyUtility.Convert.GetString(this.CurrentMaintain["id"]), MyUtility.Convert.GetString(this.CurrentMaintain["ukey"])), out dt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            if (!s.Confirm(dt))
            {
                return;
            }
        }

        /// <summary>
        /// Reload
        /// </summary>
        public void Reload()
        {
            this.ReloadDatas();
            this.RenewData();
        }
    }
}
