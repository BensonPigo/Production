using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci;
using Sci.Data;
using System.Transactions;

namespace Sci.Production.Shipping
{
    public partial class B03_CanvassRecord : Sci.Win.Tems.Input1
    {
        protected DataRow motherData;
        public B03_CanvassRecord(bool canedit, DataRow data)
        {
            InitializeComponent();
            motherData = data;
            this.DefaultFilter = "ID = '" + MyUtility.Convert.GetString(motherData["ID"]).Trim() + "'";
            if (CurrentMaintain == null)
            {
                label1.Text = "";
            }

            //選完Supp後要將回寫CurrencyID
            this.txtsubcon1.TextBox1.Validating += (s, e) =>
                {
                    if (this.EditMode && this.txtsubcon1.TextBox1.Text != this.txtsubcon1.TextBox1.OldValue)
                    {
                        CurrentMaintain["CurrencyID1"] = MyUtility.GetValue.Lookup("CurrencyID", this.txtsubcon1.TextBox1.Text, "LocalSupp", "ID");
                    }
                };
            this.txtsubcon2.TextBox1.Validating += (s, e) =>
            {
                if (this.EditMode && this.txtsubcon2.TextBox1.Text != this.txtsubcon2.TextBox1.OldValue)
                {
                    CurrentMaintain["CurrencyID2"] = MyUtility.GetValue.Lookup("CurrencyID", this.txtsubcon2.TextBox1.Text, "LocalSupp", "ID");
                }
            };
            this.txtsubcon3.TextBox1.Validating += (s, e) =>
            {
                if (this.EditMode && this.txtsubcon3.TextBox1.Text != this.txtsubcon3.TextBox1.OldValue)
                {
                    CurrentMaintain["CurrencyID3"] = MyUtility.GetValue.Lookup("CurrencyID", this.txtsubcon3.TextBox1.Text, "LocalSupp", "ID");
                }
            };
            this.txtsubcon4.TextBox1.Validating += (s, e) =>
            {
                if (this.EditMode && this.txtsubcon4.TextBox1.Text != this.txtsubcon4.TextBox1.OldValue)
                {
                    CurrentMaintain["CurrencyID4"] = MyUtility.GetValue.Lookup("CurrencyID", this.txtsubcon4.TextBox1.Text, "LocalSupp", "ID");
                }
            };
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.label1.Text = MyUtility.Convert.GetString(CurrentMaintain["Status"]);
        }

        //檢查是否還有建立的紀錄尚未被confirm，若有則無法新增資料
        protected override bool ClickNewBefore()
        {
            string sqlCmd = string.Format("select ID from ShipExpense_CanVass where ID = '{0}' and Status = 'New'", MyUtility.Convert.GetString(motherData["ID"]));
            if (MyUtility.Check.Seek(sqlCmd, null))
            {
                MyUtility.Msg.WarningBox("Still have data not yet confirm, so can't create new record!");
                return false;
            }
            return base.ClickNewBefore();
        }

        //新增預設值
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["ID"] = MyUtility.Convert.GetString(motherData["ID"]).Trim();
            CurrentMaintain["ChooseSupp"] = 1;
            CurrentMaintain["Status"] = "New";
            this.label1.Text = "New";
        }

        //修改前檢查
        protected override bool ClickEditBefore()
        {
            DataRow dr = grid.GetDataRow<DataRow>(grid.GetSelectedRowIndex());
            if (MyUtility.Convert.GetString(dr["Status"]) == "Confirmed")
            {
                MyUtility.Msg.WarningBox("Record is confirmed, can't modify!");
                return false;
            }
            return base.ClickEditBefore();
        }

        //刪除前檢查
        protected override bool ClickDeleteBefore()
        {
            DataRow dr = grid.GetDataRow<DataRow>(grid.GetSelectedRowIndex());
            if (MyUtility.Convert.GetString(dr["Status"]) == "Confirmed")
            {
                MyUtility.Msg.WarningBox("Record is confirmed, can't delete!");
                return false;
            }
            return base.ClickDeleteBefore();
        }

        //Confirm
        protected override void ClickConfirm()
        {
            base.ClickConfirm();

            var currentMaintain = this.CurrentMaintain;
            if (currentMaintain == null)
            {
                return;
            }

            var suppId = "";
            var price = 0.0;
            var currencyId = "";

            #region 選取的報價資料
            switch (MyUtility.Convert.GetString(CurrentMaintain["ChooseSupp"]))
            {
                case "1":
                    suppId = MyUtility.Convert.GetString(CurrentMaintain["LocalSuppID1"]);
                    price = MyUtility.Convert.GetDouble(CurrentMaintain["Price1"]);
                    currencyId = MyUtility.Convert.GetString(CurrentMaintain["CurrencyID1"]);
                    break;
                case "2":
                    suppId = MyUtility.Convert.GetString(CurrentMaintain["LocalSuppID2"]);
                    price = MyUtility.Convert.GetDouble(CurrentMaintain["Price2"]);
                    currencyId = MyUtility.Convert.GetString(CurrentMaintain["CurrencyID2"]);
                    break;
                case "3":
                    suppId = MyUtility.Convert.GetString(CurrentMaintain["LocalSuppID3"]);
                    price = MyUtility.Convert.GetDouble(CurrentMaintain["Price3"]);
                    currencyId = MyUtility.Convert.GetString(CurrentMaintain["CurrencyID3"]);
                    break;
                case "4":
                    suppId = MyUtility.Convert.GetString(CurrentMaintain["LocalSuppID4"]);
                    price = MyUtility.Convert.GetDouble(CurrentMaintain["Price4"]);
                    currencyId = MyUtility.Convert.GetString(CurrentMaintain["CurrencyID4"]);
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
                    string updateCommand = string.Format("Update ShipExpense_CanVass set Status = 'Confirmed', EditName ='{0}', EditDate = GETDATE() where UKey = '{1}'", Sci.Env.User.UserID, MyUtility.Convert.GetString(CurrentMaintain["UKey"]));
                    string s1 = "Update ShipExpense set LocalSuppID = @suppId, Price = @price, CurrencyID = @currencyId, CanvassDate = @canvassDate, EditName = @editName, EditDate = GETDATE() where ID = @id";
                    #region 準備sql參數資料
                    System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter();
                    sp1.ParameterName = "@id";
                    sp1.Value = MyUtility.Convert.GetString(CurrentMaintain["ID"]);

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
                    sp5.Value = Convert.ToDateTime(CurrentMaintain["AddDate"]).ToString("d");

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
                    ShowErr("Commit transaction error.", ex);
                    return;
                }
            }

            result = RenewData();
            OnDetailEntered();
            EnsureToolbarExt();
        }
    }
}
