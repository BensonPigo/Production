using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using System.Transactions;
using Ict.Win;
using Ict;
using Sci.Data;
using Sci.Production.PublicPrg;

namespace Sci.Production.Logistic
{
    public partial class P03 : Sci.Win.Tems.Input6
    {
        protected DateTime returnDate;
        public P03(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.DefaultFilter = "FactoryID = '" + Sci.Env.User.Factory + "'";
            gridicon.Append.Visible = false;
            gridicon.Insert.Visible = false;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            //新增Import From Barcode按鈕
            Sci.Win.UI.Button btn = new Sci.Win.UI.Button();
            btn.Text = "Import From Barcode";
            btn.Click += new EventHandler(btn_Click);
            browsetop.Controls.Add(btn);
            btn.Size = new Size(165, 30);//預設是(80,30)
        }

        //Import From Barcode按鈕的Click事件
        private void btn_Click(object sender, EventArgs e)
        {
            Sci.Production.Logistic.P03_ImportFromBarCode callNextForm = new Sci.Production.Logistic.P03_ImportFromBarCode();
            DialogResult result = callNextForm.ShowDialog(this);
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                this.ReloadDatas();
            }
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.label1.Text = CurrentMaintain["Status"].ToString();
        }

        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();
            Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("TransferToClogId", header: "Trans. Slip#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("PackingListId", header: "Pack ID", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("OrderId", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("CTNStartNo", header: "CTN#", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("StyleID", header: "Style#", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("SeasonID", header: "Season", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("BrandID", header: "Brand", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("CustPONo", header: "P.O.#", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Customize1", header: "Order#", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Alias", header: "Destination#", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Date("BuyerDelivery", header: "Buyer Delivery", width: Widths.AnsiChars(10), iseditingreadonly: true);
        }

        //加工資料
        protected override DualResult OnRenewDataDetailPost(RenewDataPostEventArgs e)
        {
            e.Details.Columns.Add("StyleID");
            e.Details.Columns.Add("SeasonID");
            e.Details.Columns.Add("BrandID");
            e.Details.Columns.Add("Customize1");
            e.Details.Columns.Add("CustPONo");
            e.Details.Columns.Add("Alias");
            e.Details.Columns.Add("BuyerDelivery", type: typeof(DateTime));
            foreach (DataRow gridData in e.Details.Rows)
            {
                string selectCmd = string.Format(@"select a.StyleID,a.SeasonID,a.BrandID,a.Customize1,a.CustPONo,b.Alias,a.BuyerDelivery 
                                                                           from Orders a 
                                                                           left join Country b on b.ID = a.Dest 
                                                                           where a.ID = '{0}'", gridData["OrderID"].ToString());
                DataTable orderData;
                DualResult dr;
                if (dr = DBProxy.Current.Select(null, selectCmd, out orderData))
                {
                    if (orderData.Rows.Count > 0)
                    {
                        gridData["StyleID"] = orderData.Rows[0]["StyleID"].ToString().Trim();
                        gridData["SeasonID"] = orderData.Rows[0]["SeasonID"].ToString().Trim();
                        gridData["BrandID"] = orderData.Rows[0]["BrandID"].ToString().Trim();
                        gridData["Customize1"] = orderData.Rows[0]["Customize1"].ToString().Trim();
                        gridData["CustPONo"] = orderData.Rows[0]["CustPONo"].ToString().Trim();
                        gridData["Alias"] = orderData.Rows[0]["Alias"].ToString().Trim();
                        gridData["BuyerDelivery"] = orderData.Rows[0]["BuyerDelivery"];
                    }
                }
            }
            return base.OnRenewDataDetailPost(e);
        }

        //修改前檢查，如果已經Confirm了，就不可以被修改
        protected override bool OnEditBefore()
        {
            DataRow dr = grid.GetDataRow<DataRow>(grid.GetSelectedRowIndex());
            if (dr["Status"].ToString() == "Confirmed")
            {
                MessageBox.Show("Record is confirmed, can't modify!");
                return false;
            }
            return base.OnEditBefore();
        }

        //刪除前檢查，如果已經Confirm了，就不可以被刪除
        protected override bool OnDeleteBefore()
        {
            DataRow dr = grid.GetDataRow<DataRow>(grid.GetSelectedRowIndex());
            if (dr["Status"].ToString() == "Confirmed")
            {
                MessageBox.Show("Record is confirmed, can't delete!");
                return false;
            }
            return base.OnDeleteBefore();
        }

        //新增時執行LOGISTIC->P02_InputDate
        protected override bool OnNewBefore()
        {
            Sci.Production.Logistic.P02_InputDate callNextForm = new Sci.Production.Logistic.P02_InputDate("Input Return Date", "Return Date");
            DialogResult dr = callNextForm.ShowDialog(this);
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                returnDate = callNextForm.returnDate;
                return base.OnNewBefore();
            }
            return false;
        }

        //新增帶入預設值後再執行LOGISTIC->P03_BatchReturn
        protected override void OnNewAfter()
        {
            base.OnNewAfter();
            CurrentMaintain["ReturnDate"] = returnDate;
            CurrentMaintain["FactoryID"] = Sci.Env.User.Factory;
            CurrentMaintain["Status"] = "New";
            this.label1.Text = "New";
            Sci.Production.Logistic.P03_BatchReturn callNextForm = new Sci.Production.Logistic.P03_BatchReturn(Convert.ToDateTime(CurrentMaintain["ReturnDate"].ToString()), (DataTable)detailgridbs.DataSource);
            callNextForm.ShowDialog(this);
        }

        //檢查表身不可以沒有資料
        protected override bool OnSaveBefore()
        {
            DataRow[] detailData = ((DataTable)detailgridbs.DataSource).Select();
            if (detailData.Length == 0)
            {
                MessageBox.Show("Detail can't empty!");
                return false;
            }
            if (IsDetailInserting)
            {
                string id = myUtility.GetID(ProductionEnv.Keyword + "CN", "ClogReturn", Convert.ToDateTime(CurrentMaintain["ReturnDate"].ToString()), 2, "Id", null);
                if (myUtility.Empty(id))
                {
                    MessageBox.Show("GetID fail, please try again!");
                    return false;
                }
                CurrentMaintain["ID"] = id;
            }
            return base.OnSaveBefore();
        }

        //Confirm
        protected override void OnConfirm()
        {
            base.OnConfirm();
            string sqlCmd;
            DualResult result, result1;
            DataTable selectDate;

            #region update ClogReturn & PackingList_Detail data
            sqlCmd = string.Format(@"select a.ID,a.OrderID,a.CTNStartNo,a.ClogLocationId 
                                                        from PackingList_Detail a, ClogReturn_Detail b 
                                                        where b.ID = '{0}' and a.ID = b.PackingListId and a.OrderId = b.OrderId and a.CTNStartNo = b.CTNStartNo", CurrentMaintain["ID"].ToString());
            result = DBProxy.Current.Select(null, sqlCmd, out selectDate);
            if (!result)
            {
                MessageBox.Show("Connection fail!");
                return;
            }

            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    sqlCmd = string.Format("update ClogReturn set Status = 'Confirmed', EditName = '{0}', EditDate = '{1}' where ID = '{2}'", Sci.Env.User.UserID, DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), CurrentMaintain["ID"].ToString());
                    result = DBProxy.Current.Execute(null, sqlCmd);
                    #region 宣告Update PackingList_Detail sql參數
                    IList<System.Data.SqlClient.SqlParameter> pckinglistcmds = new List<System.Data.SqlClient.SqlParameter>();
                    System.Data.SqlClient.SqlParameter detail1 = new System.Data.SqlClient.SqlParameter();
                    System.Data.SqlClient.SqlParameter detail2 = new System.Data.SqlClient.SqlParameter();
                    System.Data.SqlClient.SqlParameter detail3 = new System.Data.SqlClient.SqlParameter();
                    System.Data.SqlClient.SqlParameter detail4 = new System.Data.SqlClient.SqlParameter();
                    System.Data.SqlClient.SqlParameter detail5 = new System.Data.SqlClient.SqlParameter();
                    detail1.ParameterName = "@clogReturnID";
                    detail2.ParameterName = "@returnDate";
                    detail3.ParameterName = "@packingListID";
                    detail4.ParameterName = "@orderID";
                    detail5.ParameterName = "@CTNStartNo";
                    pckinglistcmds.Add(detail1);
                    pckinglistcmds.Add(detail2);
                    pckinglistcmds.Add(detail3);
                    pckinglistcmds.Add(detail4);
                    pckinglistcmds.Add(detail5);
                    #endregion
                    foreach (DataRow eachRow in selectDate.Rows)
                    {
                        sqlCmd = @"update PackingList_Detail 
                                             set ClogReturnID = @clogReturnID, ReturnDate = @returnDate, TransferToClogID = '', TransferDate = null, ClogReceiveID = '', ReceiveDate = null, ClogLocationId = '' 
                                             where ID = @packingListID and OrderID = @orderID and CTNStartNo = @CTNStartNo";
                        #region Update PackingList_Detail sql參數資料
                        detail1.Value = CurrentMaintain["ID"].ToString();
                        detail2.Value = Convert.ToDateTime(CurrentMaintain["ReturnDate"].ToString()).ToString("d");
                        detail3.Value = eachRow["ID"].ToString();
                        detail4.Value = eachRow["OrderId"].ToString();
                        detail5.Value = eachRow["CTNStartNo"].ToString();
                        #endregion
                        result1 = Sci.Data.DBProxy.Current.Execute(null, sqlCmd, pckinglistcmds);
                        if (!result1)
                        {
                            MessageBox.Show("Confirm failed, Pleaes re-try\r\n" + result1.ToString());
                            break;
                        }
                    }

                    if (result)
                    {
                        transactionScope.Complete();
                    }
                    else
                    {
                        MessageBox.Show("Confirm fail !\r\n" + result.ToString());
                        return;
                    }
                }
                catch (Exception ex)
                {
                    ShowErr("Confirm transaction error.", ex);
                    return;
                }
            }
            #endregion

            #region Update Orders的資料
            DataTable selectData;
            sqlCmd = string.Format("select OrderID from ClogReturn_Detail where ID = '{0}' group by OrderID", CurrentMaintain["ID"].ToString());
            result = DBProxy.Current.Select(null, sqlCmd, out selectData);
            if (!result)
            {
                MessageBox.Show("Update orders data fail!");
            }

            bool prgResult, lastResult = false;
            foreach (DataRow currentRow in selectData.Rows)
            {
                prgResult = Prgs.UpdateOrdersCTN(currentRow["OrderID"].ToString());
                if (!prgResult)
                {
                    lastResult = true;
                }

            }
            if (lastResult)
            {
                MessageBox.Show("Update orders data fail!");
            }
            #endregion

            RenewData();
            OnDetailEntered();
            EnsureToolbarCUSR();
        }

        //Batch Return，執行LOGISTIC->P03_BatchReturn
        private void button2_Click(object sender, EventArgs e)
        {
            Sci.Production.Logistic.P03_BatchReturn callNextForm = new Sci.Production.Logistic.P03_BatchReturn(Convert.ToDateTime(CurrentMaintain["ReturnDate"].ToString()), (DataTable)detailgridbs.DataSource);
            callNextForm.ShowDialog(this);
        }
    }
}
