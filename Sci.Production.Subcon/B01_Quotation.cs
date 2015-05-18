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
        protected DataRow dr;
        public B01_Quotation(bool canedit, DataRow data )
        {
            InitializeComponent();
            dr = data;
            string b01_refno = data["refno"].ToString();
            this.DefaultFilter = "refno = '"+ b01_refno+"'";
            
        }

        //新增預設值
        protected override void OnNewAfter()
        {
            base.OnNewAfter();
            CurrentMaintain["Refno"] = dr["refno"].ToString();
            CurrentMaintain["issuedate"] = DateTime.Today;
        }

        //修改前檢查
        protected override bool OnEditBefore()
        {
            if(CurrentMaintain["Encode"].ToString() == "True")
            {
                MessageBox.Show("Record is encoded, can't modify!");
                return false;
            }
            return base.OnEditBefore();
        }

        //刪除前檢查
        protected override bool OnDeleteBefore()
        {
            if (CurrentMaintain["Encode"].ToString() == "True")
            {
                MessageBox.Show("Record is encoded, can't delete!");
                return false;
            }
            return base.OnDeleteBefore();
        }

        //refresh
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.button1.Enabled=CurrentMaintain["Encode"].ToString() == "False";
        }

        //Encode button
        private void button1_Click(object sender, EventArgs e)
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
                MessageBox.Show("Choosed Set of data can't be empty!!");
                return;
            }

            TransactionScope _transactionscope = new TransactionScope();
            DualResult result;             
            try
            {
                String sqlcmd = "Update localitem_quot set encode = 1 where refno = '" + CurrentMaintain["refno"].ToString() + "'";
                result = Sci.Data.DBProxy.Current.Execute(null, sqlcmd);
               
                string s1 = "Update localitem set localsuppid = @suppid,price = @price,currencyid = @currencyid, quotdate = @quotdate where refno = @refno";
                
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

                result = Sci.Data.DBProxy.Current.Execute(null, s1, cmds);
                _transactionscope.Complete();
                MessageBox.Show("Encode sucessful");
                RenewData();
                OnDetailEntered();
            }
            catch (Exception ex)
            {
                ShowErr("Commit transaction error.", ex);
                return;
            }

            _transactionscope.Dispose();
            _transactionscope = null;
        }
    }
}
