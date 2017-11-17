using System;
using System.Collections.Generic;
using System.Data;
using Ict;
using System.Transactions;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// B03_CanvassRecord
    /// </summary>
    public partial class B03_CanvassRecord : Sci.Win.Tems.Input1
    {
        private DataRow motherData;
        private bool CanEdit = false;

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
        public B03_CanvassRecord(bool canedit, DataRow data)
        {
            this.InitializeComponent();
            this.CanEdit1 = canedit;
            this.MotherData = data;
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

            this.txtsubconSupplier2.TextBox1.Validating += (s, e) =>
            {
                if (this.EditMode && this.txtsubconSupplier2.TextBox1.Text != this.txtsubconSupplier2.TextBox1.OldValue)
                {
                    this.CurrentMaintain["CurrencyID2"] = MyUtility.GetValue.Lookup("CurrencyID", this.txtsubconSupplier2.TextBox1.Text, "LocalSupp", "ID");
                }
            };

            this.txtsubconSupplier3.TextBox1.Validating += (s, e) =>
            {
                if (this.EditMode && this.txtsubconSupplier3.TextBox1.Text != this.txtsubconSupplier3.TextBox1.OldValue)
                {
                    this.CurrentMaintain["CurrencyID3"] = MyUtility.GetValue.Lookup("CurrencyID", this.txtsubconSupplier3.TextBox1.Text, "LocalSupp", "ID");
                }
            };

            this.txtsubconSupplier4.TextBox1.Validating += (s, e) =>
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
            this.toolbar.cmdEdit.Enabled = this.CanEdit1 && !this.EditMode;
            this.toolbar.cmdNew.Enabled = this.CanEdit1 && !this.EditMode;
            this.toolbar.cmdConfirm.Visible = this.CanEdit1 && !this.EditMode;

            if (this.CurrentMaintain != null)
            {
                if (this.tabs.SelectedIndex == 0)
                {
                    this.toolbar.cmdConfirm.Enabled = false;
                    this.toolbar.cmdSave.Enabled = false;
                }
                else
                {
                    this.toolbar.cmdConfirm.Enabled = this.CanEdit1 && !this.EditMode && this.CurrentMaintain["Status"].ToString().ToUpper() == "NEW";
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
            string sqlCmd = string.Format("select ID from ShipExpense_CanVass WITH (NOLOCK) where ID = '{0}' and Status = 'New'", MyUtility.Convert.GetString(this.MotherData["ID"]));
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
            this.CurrentMaintain["ChooseSupp"] = 1;
            this.CurrentMaintain["Status"] = "New";
            this.label1.Text = "New";
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

            DualResult result, result2;
            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    string updateCommand = string.Format("Update ShipExpense_CanVass set Status = 'Confirmed', EditName ='{0}', EditDate = GETDATE() where UKey = '{1}'", Sci.Env.User.UserID, MyUtility.Convert.GetString(this.CurrentMaintain["UKey"]));
                    string s1 = "Update ShipExpense set LocalSuppID = @suppId, Price = @price, CurrencyID = @currencyId, CanvassDate = @canvassDate, EditName = @editName, EditDate = GETDATE() where ID = @id";
                    #region 準備sql參數資料
                    System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter();
                    sp1.ParameterName = "@id";
                    sp1.Value = MyUtility.Convert.GetString(this.CurrentMaintain["ID"]);

                    System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter();
                    sp2.ParameterName = "@suppId";
                    sp2.Value = suppId;

                    System.Data.SqlClient.SqlParameter sp3 = new System.Data.SqlClient.SqlParameter();
                    sp3.ParameterName = "@price";
                    sp3.Value = price;

                    System.Data.SqlClient.SqlParameter sp4 = new System.Data.SqlClient.SqlParameter();
                    sp4.ParameterName = "@currencyId";
                    sp4.Value = currencyId;

                    System.Data.SqlClient.SqlParameter sp5 = new System.Data.SqlClient.SqlParameter();
                    sp5.ParameterName = "@canvassDate";
                    sp5.Value = Convert.ToDateTime(this.CurrentMaintain["AddDate"]).ToString("d");

                    System.Data.SqlClient.SqlParameter sp6 = new System.Data.SqlClient.SqlParameter();
                    sp6.ParameterName = "@editName";
                    sp6.Value = Sci.Env.User.UserID;

                    IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                    cmds.Add(sp1);
                    cmds.Add(sp2);
                    cmds.Add(sp3);
                    cmds.Add(sp4);
                    cmds.Add(sp5);
                    cmds.Add(sp6);
                    #endregion

                    result = Sci.Data.DBProxy.Current.Execute(null, updateCommand);
                    result2 = Sci.Data.DBProxy.Current.Execute(null, s1, cmds);

                    if (result && result2)
                    {
                        transactionScope.Complete();
                    }
                    else
                    {
                        transactionScope.Dispose();
                        MyUtility.Msg.WarningBox("Confirm failed, Pleaes re-try");
                    }
                }
                catch (Exception ex)
                {
                    transactionScope.Dispose();
                    this.ShowErr("Commit transaction error.", ex);
                    return;
                }
            }
        }
    }
}
