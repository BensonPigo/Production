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

namespace Sci.Production.Subcon
{
    public partial class B01_Quotation : Sci.Win.Tems.Input1
    {
        protected DataRow dr,dr_detail;
        bool _canedit;

        public B01_Quotation(bool canedit, DataRow data )
        {
            InitializeComponent();
            dr = data;
            _canedit = canedit;
            string b01_refno = data["refno"].ToString();
            this.DefaultFilter = "refno = '"+ b01_refno+"'";

            //選完Supp後要將回寫CurrencyID
            this.txtsubconSupplier1.TextBox1.Validated += (s, e) =>
            {
                if (this.EditMode && this.txtsubconSupplier1.TextBox1.Text != this.txtsubconSupplier1.TextBox1.OldValue)
                {
                    CurrentMaintain["CurrencyID1"] = MyUtility.GetValue.Lookup("CurrencyID", this.txtsubconSupplier1.TextBox1.Text, "LocalSupp", "ID");
                }
            };
            this.txtsubconSupplier2.TextBox1.Validated += (s, e) =>
            {
                if (this.EditMode && this.txtsubconSupplier2.TextBox1.Text != this.txtsubconSupplier2.TextBox1.OldValue)
                {
                    CurrentMaintain["CurrencyID2"] = MyUtility.GetValue.Lookup("CurrencyID", this.txtsubconSupplier2.TextBox1.Text, "LocalSupp", "ID");
                }
            };
            this.txtsubconSupplier3.TextBox1.Validated += (s, e) =>
            {
                if (this.EditMode && this.txtsubconSupplier3.TextBox1.Text != this.txtsubconSupplier3.TextBox1.OldValue)
                {
                    CurrentMaintain["CurrencyID3"] = MyUtility.GetValue.Lookup("CurrencyID", this.txtsubconSupplier3.TextBox1.Text, "LocalSupp", "ID");
                }
            };
            this.txtsubconSupplier4.TextBox1.Validated += (s, e) =>
            {
                if (this.EditMode && this.txtsubconSupplier4.TextBox1.Text != this.txtsubconSupplier4.TextBox1.OldValue)
                {
                    CurrentMaintain["CurrencyID4"] = MyUtility.GetValue.Lookup("CurrencyID", this.txtsubconSupplier4.TextBox1.Text, "LocalSupp", "ID");
                }
            };
            
        }

        protected override void EnsureToolbarExt()
        {
            base.EnsureToolbarExt();

            //讓有[SUBCON][B01]EDIT權限的使用者可以[新增][修改][刪除]
            this.toolbar.cmdNew.Enabled = _canedit;
            this.toolbar.cmdEdit.Enabled = _canedit;
            this.toolbar.cmdDelete.Enabled = _canedit;

        }

        protected override bool ClickNewBefore()
        {
            bool flag = false;
            System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter();
            sp1.ParameterName = "@refno";
            sp1.Value = dr["refno"].ToString();

            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
            cmds.Add(sp1);

            string sqlcmd = "select refno from localitem_quot WITH (NOLOCK) where refno = @refno and (Status ='New')";
            DBProxy.Current.Exists("", sqlcmd, cmds, out flag);
            if (flag)
            {
                MyUtility.Msg.WarningBox("Can't add data when data have not been approved.", "Warning");
                return false;
            }
            return base.ClickNewBefore();
        }

        //新增預設值
        protected override void ClickNewAfter()
        {
            
            base.ClickNewAfter();
            CurrentMaintain["Refno"] = dr["refno"].ToString();
            CurrentMaintain["issuedate"] = DateTime.Today;
            CurrentMaintain["Status"] = "New";
        }

        //修改前檢查
        protected override bool ClickEditBefore()
        {
            if (CurrentMaintain["Status"].ToString() == "Approved")
            {
                MyUtility.Msg.WarningBox("Record is Approved, can't modify!");
                return false;
            }
            return base.ClickEditBefore();
        }

        //刪除前檢查
        protected override bool ClickDeleteBefore()
        {
            if (CurrentMaintain["Status"].ToString() == "Approved")
            {
                MyUtility.Msg.WarningBox("Record is Approved, can't delete!");
                return false;
            }
            return base.ClickDeleteBefore();
        }

        //refresh
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.btnApprove.Enabled = (CurrentMaintain["Status"].ToString().ToUpper() == "NEW") && !this.EditMode;
        }

        //Encode button
        private void btnApprove_Click(object sender, EventArgs e)
        {
            
            var suppid = "";
            var price = 0.0;
            var currencyid = "";

            #region 選取的報價資料
            switch (CurrentMaintain["ChooseSupp"].ToString())
            {
                case "1":
                    suppid = CurrentMaintain["localsuppid1"].ToString();
                    price = double.Parse(CurrentMaintain["price1"].ToString());
                    currencyid = CurrentMaintain["currencyid1"].ToString();
                    break;
                case "2":
                    suppid = CurrentMaintain["localsuppid2"].ToString();
                    price = double.Parse(CurrentMaintain["price2"].ToString());
                    currencyid = CurrentMaintain["currencyid2"].ToString();
                    break;
                case "3":
                    suppid = CurrentMaintain["localsuppid3"].ToString();
                    price = double.Parse(CurrentMaintain["price3"].ToString());
                    currencyid = CurrentMaintain["currencyid3"].ToString();
                    break;
                case "4":
                    suppid = CurrentMaintain["localsuppid4"].ToString();
                    price = double.Parse(CurrentMaintain["price4"].ToString());
                    currencyid = CurrentMaintain["currencyid4"].ToString();
                    break;
            }
            #endregion

            if (string.IsNullOrWhiteSpace(suppid) || string.IsNullOrWhiteSpace(currencyid) || price == 0.0)
            {
                MyUtility.Msg.WarningBox("Choosed Set of data can't be empty!!");
                return;
            }

            DualResult result,result2;
            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    String sqlcmd = string.Format("Update localitem_quot set Status = 'Approved' ,editname = '{0}', editdate = GETDATE() where ukey = '{1}'", Env.User.UserID.ToString(), CurrentMaintain["ukey"].ToString());
                    result = Sci.Data.DBProxy.Current.Execute(null, sqlcmd);

                    string s1 = string.Format("Update localitem set localsuppid = @suppid,price = @price,currencyid = @currencyid, quotdate = @quotdate,editname = '{0}', editdate = GETDATE() where refno = @refno",Env.User.UserID.ToString());

                    #region 準備sql參數資料
                    System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter();
                    sp1.ParameterName = "@refno";
                    sp1.Value = CurrentMaintain["refno"].ToString();

                    System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter();
                    sp2.ParameterName = "@suppid";
                    sp2.Value = suppid;

                    System.Data.SqlClient.SqlParameter sp3 = new System.Data.SqlClient.SqlParameter();
                    sp3.ParameterName = "@price";
                    sp3.Value = price;

                    System.Data.SqlClient.SqlParameter sp4 = new System.Data.SqlClient.SqlParameter();
                    sp4.ParameterName = "@currencyid";
                    sp4.Value = currencyid;

                    System.Data.SqlClient.SqlParameter sp5 = new System.Data.SqlClient.SqlParameter();
                    sp5.ParameterName = "@quotdate";
                    sp5.Value = DateTime.Today;

                    IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                    cmds.Add(sp1);
                    cmds.Add(sp2);
                    cmds.Add(sp3);
                    cmds.Add(sp4);
                    cmds.Add(sp5);
                    #endregion

                    result2 = Sci.Data.DBProxy.Current.Execute(null, s1, cmds);

                    if (result && result2)
                    {
                        _transactionscope.Complete();
                        _transactionscope.Dispose();
                        MyUtility.Msg.WarningBox("Approved successful");
                    }
                    else
                    {
                        _transactionscope.Dispose();
                        MyUtility.Msg.WarningBox("Approved failed, Pleaes re-try");
                    }
                    
                }
                catch (Exception ex)
                {
                    _transactionscope.Dispose();
                    ShowErr("Commit transaction error.", ex);
                    return;
                }
            }

            _transactionscope.Dispose();
            _transactionscope = null;

          
        }
    }
}
