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
    public partial class P02 : Sci.Win.Tems.Input6
    {
        protected DateTime receiveDate;
        public P02(ToolStripMenuItem menuitem)
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
            Sci.Production.Logistic.P02_ImportFromBarCode callNextForm = new Sci.Production.Logistic.P02_ImportFromBarCode();
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
                .Text("PackingListId", header: "Pack Id", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("OrderId", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("CTNStartNo", header: "CTN#", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("StyleID", header: "Style#", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("SeasonID", header: "Season", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("BrandID", header: "Brand", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("CustPONo", header: "P.O.#", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Customize1", header: "Order#", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Alias", header: "Destination#", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Date("BuyerDelivery", header: "Buyer Delivery", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .CellClogLocation("ClogLocationId", header: "Location No", width: Widths.AnsiChars(10));
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
            Sci.Production.Logistic.P02_InputDate callNextForm = new Sci.Production.Logistic.P02_InputDate("Input Receive Date", "Receive Date");
            DialogResult dr = callNextForm.ShowDialog(this);
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                receiveDate = callNextForm.returnDate;
                return base.OnNewBefore();
            }
            return false;
        }

        //新增帶入預設值後再執行LOGISTIC->P02_BatchReceiving
        protected override void OnNewAfter()
        {
            base.OnNewAfter();
            CurrentMaintain["ReceiveDate"] = receiveDate;
            CurrentMaintain["FactoryID"] = Sci.Env.User.Factory;
            CurrentMaintain["Status"] = "New";
            this.label1.Text = "New";
            Sci.Production.Logistic.P02_BatchReceiving callNextForm = new Sci.Production.Logistic.P02_BatchReceiving(Convert.ToDateTime(CurrentMaintain["ReceiveDate"].ToString()), (DataTable)detailgridbs.DataSource);
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
                string id = myUtility.GetID(ProductionEnv.Keyword + "CR", "ClogReceive", Convert.ToDateTime(CurrentMaintain["ReceiveDate"].ToString()), 2, "Id", null);
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
            string sqlCmd, wrongCtn = "", lostCtn = "";
            DualResult result, result1;
            DataTable selectDate;
            //檢查收到的箱號與箱數要和Transfer to Clog一樣，若不一至則出訊息告知且不做任何事
            #region Wrong Receive
            sqlCmd = string.Format(@"select * from (
                                                            select a.PackingListId,a.OrderId,a.CTNStartNo,b.Id 
                                                            from ClogReceive_Detail a
                                                            left join TransferToClog_Detail b 
                                                                on a.TransferToClogId = b.Id 
                                                                and a.PackingListId = b.PackingListId 
                                                                and a.OrderId = b.OrderId  
                                                                and a.CTNStartNo = b.CTNStartNo
                                                            where a.ID = '{0}') c where c.Id is null", CurrentMaintain["ID"].ToString());
            result = DBProxy.Current.Select(null, sqlCmd, out selectDate);
            if (!result)
            {
                MessageBox.Show("Connection fail!");
                return;
            }
            if (selectDate.Rows.Count > 0)
            {
                foreach (DataRow eachRow in selectDate.Rows)
                {
                    wrongCtn = wrongCtn + string.Format("Pack ID:{0}  SP#:{1}   CTN#:{2}\r\n", eachRow["PackingListId"].ToString(), eachRow["OrderId"].ToString(), eachRow["CTNStartNo"].ToString());
                }
            }
            if (!myUtility.Empty(wrongCtn))
            {
                wrongCtn = "Wrong Rev#\r\n" + wrongCtn;
            }
            #endregion
            #region Lack Receive
            sqlCmd = string.Format(@"select * from (
                                                            select a.Id,a.PackingListId,a.OrderId,a.CTNStartNo,b.Id as ReceiveID 
                                                            from TransferToClog_Detail a
                                                            left join ClogReceive_Detail b
                                                                on a.Id = b.TransferToClogId
                                                                and a.PackingListId = b.PackingListId 
                                                                and a.OrderId = b.OrderId  
                                                                and a.CTNStartNo = b.CTNStartNo
                                                            where a.Id in (select distinct TransferToClogId  from ClogReceive_Detail where id = '{0}')) c 
                                                        where c.ReceiveID is null or c.ReceiveID != '{0}'", CurrentMaintain["ID"].ToString());
            result = DBProxy.Current.Select(null, sqlCmd, out selectDate);
            if (!result)
            {
                MessageBox.Show("Connection fail!");
                return;
            }
            if (selectDate.Rows.Count > 0)
            {
                foreach (DataRow eachRow in selectDate.Rows)
                {
                    lostCtn = lostCtn + string.Format("Trans. Slip#:{0}   Pack ID:{1}  SP#:{2}   CTN#:{3}\r\n", eachRow["ID"].ToString(), eachRow["PackingListId"].ToString(), eachRow["OrderId"].ToString(), eachRow["CTNStartNo"].ToString());
                }
            }
            if (!myUtility.Empty(lostCtn))
            {
                lostCtn = "Lacking Rev#\r\n" + lostCtn;
            }
            #endregion
            if (!myUtility.Empty(wrongCtn) || !myUtility.Empty(lostCtn))
            {
                MessageBox.Show(wrongCtn + lostCtn);
                return;
            }

            //update ClogReceive & PackingList_Detail data
            sqlCmd = string.Format(@"select a.ID,a.OrderID,a.CTNStartNo,b.ClogLocationId 
                                                        from PackingList_Detail a, ClogReceive_Detail b 
                                                        where b.ID = '{0}' and a.ID = b.PackingListId and a.OrderId = b.OrderId and a.CTNStartNo = b.CTNStartNo",CurrentMaintain["ID"].ToString());
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
                    sqlCmd = string.Format("update ClogReceive set Status = 'Confirmed', EditName = '{0}', EditDate = '{1}' where ID = '{2}'", Sci.Env.User.UserID, DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), CurrentMaintain["ID"].ToString());
                    result = DBProxy.Current.Execute(null, sqlCmd);
                    #region 宣告Update PackingList_Detail sql參數
                    IList<System.Data.SqlClient.SqlParameter> pckinglistcmds = new List<System.Data.SqlClient.SqlParameter>();
                    System.Data.SqlClient.SqlParameter detail1 = new System.Data.SqlClient.SqlParameter();
                    System.Data.SqlClient.SqlParameter detail2 = new System.Data.SqlClient.SqlParameter();
                    System.Data.SqlClient.SqlParameter detail3 = new System.Data.SqlClient.SqlParameter();
                    System.Data.SqlClient.SqlParameter detail4 = new System.Data.SqlClient.SqlParameter();
                    System.Data.SqlClient.SqlParameter detail5 = new System.Data.SqlClient.SqlParameter();
                    System.Data.SqlClient.SqlParameter detail6 = new System.Data.SqlClient.SqlParameter();
                    detail1.ParameterName = "@clogReceiveId";
                    detail2.ParameterName = "@receiveDate";
                    detail3.ParameterName = "@clogLocationId";
                    detail4.ParameterName = "@packingListID";
                    detail5.ParameterName = "@orderID";
                    detail6.ParameterName = "@CTNStartNo";
                    pckinglistcmds.Add(detail1);
                    pckinglistcmds.Add(detail2);
                    pckinglistcmds.Add(detail3);
                    pckinglistcmds.Add(detail4);
                    pckinglistcmds.Add(detail5);
                    pckinglistcmds.Add(detail6);
                    #endregion
                    foreach (DataRow eachRow in selectDate.Rows)
                    {
                        sqlCmd = @"update PackingList_Detail 
                                             set ClogReceiveId = @clogReceiveId, ReceiveDate = @receiveDate, ClogLocationId = @clogLocationId 
                                             where ID = @packingListID and OrderID = @orderID and CTNStartNo = @CTNStartNo";
                        #region Update PackingList_Detail sql參數資料
                        detail1.Value = CurrentMaintain["ID"].ToString();
                        detail2.Value = Convert.ToDateTime(CurrentMaintain["ReceiveDate"].ToString()).ToString("d");
                        detail3.Value = eachRow["ClogLocationId"].ToString();
                        detail4.Value = eachRow["ID"].ToString();
                        detail5.Value = eachRow["OrderId"].ToString();
                        detail6.Value = eachRow["CTNStartNo"].ToString();
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

            //Update Orders的資料
            callGetCartonList();

            RenewData();
            OnDetailEntered();
            EnsureToolbarCUSR();
        }

        //UnConfirm
        protected override void OnUnconfirm()
        {
            base.OnUnconfirm();
            //檢查是否有箱子已被Clog Return，若有則出訊息告知且不做任何事
            DataTable selectData;
            string sqlCmd = string.Format("select count(b.id) as returnCTN from ClogReceive_Detail a, ClogReturn_Detail b where a.TransferToClogId = b.TransferToClogId and a.PackingListId = b.PackingListId and a.OrderId = b.OrderId and a.CTNStartNo = b.CTNStartNo and a.ID = '{0}'", CurrentMaintain["ID"].ToString());
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out selectData);
            if (!myUtility.Empty(selectData.Rows[0]["returnCTN"]))
            {
                MessageBox.Show("This recode has return record, con not unconfirm!");
                return;
            }

            //先問使用者是否確定要Unconfirm，若是才做更新
            DialogResult unconfirmResult;
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            unconfirmResult = MessageBox.Show("Are you sure unconfirm this data?", "Warning", buttons);
            if (unconfirmResult == System.Windows.Forms.DialogResult.Yes)
            {
                 using (TransactionScope transactionScope = new TransactionScope())
                 {
                        string sqlCmd1 = string.Format("update ClogReceive set Status = 'New', EditName = '{0}', EditDate = '{1}' where ID = '{2}'", Sci.Env.User.UserID, DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), CurrentMaintain["ID"].ToString());
                        string sqlCmd2 = string.Format("update PackingList_Detail set ClogReceiveId = '', ReceiveDate = null, ClogLocationId = '' where ClogReceiveId = '{0}'", CurrentMaintain["ID"].ToString());
                        DualResult result1 = DBProxy.Current.Execute(null, sqlCmd1);
                        DualResult result2 = DBProxy.Current.Execute(null, sqlCmd2);
                        if (result1 && result2)
                        {
                            transactionScope.Complete();
                        }
                        else
                        {
                            string failMsg = "";
                            if (!result1)
                            {
                                failMsg = result1.ToString() + "\r\n";
                            }

                            if (!result2)
                            {
                                failMsg = failMsg + result2.ToString();
                            }

                            MessageBox.Show("Unconfirm fail !\r\n" + failMsg);
                            return;
                        }
                 }

                //Update Orders的資料
                callGetCartonList();

                RenewData();
                OnDetailEntered();
                EnsureToolbarCUSR();
            }
        }

        //Update Orders的資料
        private void callGetCartonList()
        {
            DataTable selectData;
            string sqlCmd = string.Format("select OrderID from ClogReceive_Detail where ID = '{0}' group by OrderID", CurrentMaintain["ID"].ToString());
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out selectData);
            if (!result)
            {
                MessageBox.Show("Update orders data fail!");
            }

            bool prgResult, lastResult = false;
            foreach (DataRow currentRow in selectData.Rows)
            {
                prgResult = Prgs.GetCartonList(currentRow["OrderID"].ToString());
                if (!prgResult)
                {
                    lastResult = true;
                }

            }
            if (lastResult)
            {
                MessageBox.Show("Update orders data fail!");
            }
        }

        //Batch Receive，執行LOGISTIC->P02_BatchReceiving
        private void button2_Click(object sender, EventArgs e)
        {
            Sci.Production.Logistic.P02_BatchReceiving callNextForm = new Sci.Production.Logistic.P02_BatchReceiving(Convert.ToDateTime(CurrentMaintain["ReceiveDate"].ToString()), (DataTable)detailgridbs.DataSource);
            callNextForm.ShowDialog(this);
        }
    }
}

