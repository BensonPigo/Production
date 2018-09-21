using System;
using System.Collections.Generic;
using System.Data;
using Ict;
using Sci.Data;

namespace Sci.Production.Subcon
{
    public partial class B01_Quotation : Sci.Win.Tems.Input1
    {
        protected DataRow dr,dr_detail;
        bool _canedit;
        bool _canconfirm;
        public B01_Quotation(bool canedit, DataRow data ,bool cancomfirmed)
        {
            InitializeComponent();
            dr = data;
            _canedit = canedit;
            _canconfirm = cancomfirmed;
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

        protected override bool ClickNewBefore()
        {
            bool flag = false;
            System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter();
            sp1.ParameterName = "@refno";
            sp1.Value = dr["refno"].ToString();

            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
            cmds.Add(sp1);

            string sqlcmd = "select refno from localitem_quot WITH (NOLOCK) where refno = @refno and Status ='New'";
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
            CurrentMaintain["ChooseSupp"] = 1;
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

        protected override bool ClickSaveBefore()
        {
            bool flag = false;
            System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter();
            sp1.ParameterName = "@refno";
            sp1.Value = dr["refno"].ToString();

            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
            cmds.Add(sp1);

            string sqlcmd = "select refno from localitem_quot WITH (NOLOCK) where refno = @refno and Status ='New'";
            DBProxy.Current.Exists("", sqlcmd, cmds, out flag);
            if (flag)
            {
                MyUtility.Msg.WarningBox("Can't add data when data have not been approved.", "Warning");
                return false;
            }
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
                return false;
            }

            return base.ClickSaveBefore();
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
        }

        protected override void ClickConfirm()
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

            DataTable dt;
            var s = new B01_BatchApprove();
            DualResult result = DBProxy.Current.Select(string.Empty, s.sqlcmd(MyUtility.Convert.GetString(this.CurrentMaintain["Refno"])), out dt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            if(!s.confirm(dt))
            {
                return;
            }

            base.ClickConfirm();
        }
    }
}
