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
            this.DefaultFilter = "ID = '" + motherData["ID"].ToString().Trim() + "'";

            //選完Supp後要將回寫CurrencyID
            this.txtsubcon1.TextBox1.Validating += (s, e) =>
                {
                    if (this.EditMode && this.txtsubcon1.TextBox1.Text != this.txtsubcon1.TextBox1.OldValue)
                    {
                        CurrentMaintain["CurrencyID1"] = myUtility.Lookup("CurrencyID", this.txtsubcon1.TextBox1.Text, "LocalSupp", "ID");
                    }
                };
            this.txtsubcon2.TextBox1.Validating += (s, e) =>
            {
                if (this.EditMode && this.txtsubcon2.TextBox1.Text != this.txtsubcon2.TextBox1.OldValue)
                {
                    CurrentMaintain["CurrencyID2"] = myUtility.Lookup("CurrencyID", this.txtsubcon2.TextBox1.Text, "LocalSupp", "ID");
                }
            };
            this.txtsubcon3.TextBox1.Validating += (s, e) =>
            {
                if (this.EditMode && this.txtsubcon3.TextBox1.Text != this.txtsubcon3.TextBox1.OldValue)
                {
                    CurrentMaintain["CurrencyID3"] = myUtility.Lookup("CurrencyID", this.txtsubcon3.TextBox1.Text, "LocalSupp", "ID");
                }
            };
            this.txtsubcon4.TextBox1.Validating += (s, e) =>
            {
                if (this.EditMode && this.txtsubcon4.TextBox1.Text != this.txtsubcon4.TextBox1.OldValue)
                {
                    CurrentMaintain["CurrencyID4"] = myUtility.Lookup("CurrencyID", this.txtsubcon4.TextBox1.Text, "LocalSupp", "ID");
                }
            };
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.button1.Enabled = CurrentMaintain["Encode"].ToString() == "False";
        }

        //檢查是否還有建立的紀錄尚未被encode，若有則無法新增資料
        protected override bool OnNewBefore()
        {
            string sqlCmd = string.Format("select ID from ShipExpense_CanVass where ID = '{0}' and Encode = 0", this.motherData["ID"].ToString());
            if (myUtility.Seek(sqlCmd, null))
            {
                MessageBox.Show("Still have data not yet encode, so can't create new record!");
                return false;
            }
            return base.OnNewBefore();
        }

        //新增預設值
        protected override void OnNewAfter()
        {
            base.OnNewAfter();
            CurrentMaintain["ID"] = motherData["ID"].ToString().Trim();
            CurrentMaintain["ChooseSupp"] = 1;
            this.button1.Enabled = false;
        }

        //修改前檢查
        protected override bool OnEditBefore()
        {
            DataRow dr = grid.GetDataRow<DataRow>(grid.GetSelectedRowIndex());
            if (dr["Encode"].ToString() == "True")
            {
                MessageBox.Show("Record is encoded, can't modify!");
                return false;
            }
            return base.OnEditBefore();
        }

        //編輯狀態下按鈕不可以使用
        protected override void OnEditAfter()
        {
            base.OnEditAfter();
            this.button1.Enabled = false;
        }

        //刪除前檢查
        protected override bool OnDeleteBefore()
        {
            DataRow dr = grid.GetDataRow<DataRow>(grid.GetSelectedRowIndex());
            if (dr["Encode"].ToString() == "True")
            {
                MessageBox.Show("Record is encoded, can't delete!");
                return false;
            }
            return base.OnDeleteBefore();
        }

        //Encode button
        private void button1_Click(object sender, EventArgs e)
        {
            var suppId = "";
            var price = 0.0;
            var currencyId = "";

            #region 選取的報價資料
            switch (CurrentMaintain["ChooseSupp"].ToString())
            {
                case "1":
                    suppId = CurrentMaintain["LocalSuppID1"].ToString();
                    price = double.Parse(CurrentMaintain["Price1"].ToString());
                    currencyId = CurrentMaintain["CurrencyID1"].ToString();
                    break;
                case "2":
                    suppId = CurrentMaintain["LocalSuppID2"].ToString();
                    price = double.Parse(CurrentMaintain["Price2"].ToString());
                    currencyId = CurrentMaintain["CurrencyID2"].ToString();
                    break;
                case "3":
                    suppId = CurrentMaintain["LocalSuppID3"].ToString();
                    price = double.Parse(CurrentMaintain["Price3"].ToString());
                    currencyId = CurrentMaintain["CurrencyID3"].ToString();
                    break;
                case "4":
                    suppId = CurrentMaintain["LocalSuppID4"].ToString();
                    price = double.Parse(CurrentMaintain["Price4"].ToString());
                    currencyId = CurrentMaintain["CurrencyID4"].ToString();
                    break;
            }
            #endregion

            if (string.IsNullOrWhiteSpace(suppId) || string.IsNullOrWhiteSpace(currencyId) || price == 0.0)
            {
                MessageBox.Show("Choosed Set of data can't be empty!!");
                return;
            }

            DualResult result, result2;
            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    string updateCommand = string.Format("Update ShipExpense_CanVass set encode = 1, EditName ='{0}', EditDate = '{1}' where UKey = '{2}'", Sci.Env.User.UserID, DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), CurrentMaintain["UKey"].ToString());
                    result = Sci.Data.DBProxy.Current.Execute(null, updateCommand);

                    string s1 = "Update ShipExpense set LocalSuppID = @suppId, Price = @price, CurrencyID = @currencyId, CanvassDate = @canvassDate, EditName = @editName, EditDate = @editDate where ID = @id";

                    #region 準備sql參數資料
                    System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter();
                    sp1.ParameterName = "@id";
                    sp1.Value = CurrentMaintain["ID"].ToString();

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

                    System.Data.SqlClient.SqlParameter sp7 = new System.Data.SqlClient.SqlParameter();
                    sp7.ParameterName = "@editDate";
                    sp7.Value = DateTime.Now;

                    IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                    cmds.Add(sp1);
                    cmds.Add(sp2);
                    cmds.Add(sp3);
                    cmds.Add(sp4);
                    cmds.Add(sp5);
                    cmds.Add(sp6);
                    cmds.Add(sp7);
                    #endregion

                    result2 = Sci.Data.DBProxy.Current.Execute(null, s1, cmds);

                    if (result && result2)
                    {
                        transactionScope.Complete();
                        MessageBox.Show("Encode sucessful");
                    }
                    else
                    {
                        MessageBox.Show("Encode failed, Pleaes re-try");
                    }

                }
                catch (Exception ex)
                {
                    ShowErr("Commit transaction error.", ex);
                    return;
                }
            }

            result = RenewData();
            OnDetailEntered();
        }

       
    }
}
