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
            this.DefaultFilter = "MDivisionID = '" + Sci.Env.User.Keyword + "'";
            gridicon.Append.Visible = false;
            gridicon.Insert.Visible = false;
            InsertDetailGridOnDoubleClick = false;
        }

        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["ID"].ToString();
            this.DetailSelectCommand = string.Format(@"select distinct crd.*, o.StyleID,o.SeasonID,o.BrandID,o.Customize1,o.CustPONo,c.Alias,oqs.BuyerDelivery
from ClogReceive_Detail crd
left join Orders o on o.ID = crd.OrderID
left join Country c on c.ID = o.Dest
left join PackingList_Detail pld on pld.ID = crd.PackingListId and pld.CTNQty = 1
left join Order_QtyShip oqs on oqs.Id = pld.OrderId and oqs.Seq = pld.OrderShipmodeSeq
where crd.ID = '{0}'", masterID);
            return base.OnDetailSelectCommandPrepare(e);
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
            ShowStatus();
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

        //修改前檢查，如果已經Confirm了，就不可以被修改
        protected override bool ClickEditBefore()
        {
            DataRow dr = grid.GetDataRow<DataRow>(grid.GetSelectedRowIndex());
            if (dr["Status"].ToString() == "Confirmed")
            {
                MyUtility.Msg.WarningBox("Record is confirmed, can't modify!");
                return false;
            }
            return base.ClickEditBefore();
        }

        //刪除前檢查，如果已經Confirm了，就不可以被刪除
        protected override bool ClickDeleteBefore()
        {
            DataRow dr = grid.GetDataRow<DataRow>(grid.GetSelectedRowIndex());
            if (dr["Status"].ToString() == "Confirmed")
            {
                MyUtility.Msg.WarningBox("Record is confirmed, can't delete!");
                return false;
            }
            return base.ClickDeleteBefore();
        }

        //新增時執行LOGISTIC->P02_InputDate
        protected override bool ClickNewBefore()
        {
            Sci.Production.Logistic.P02_InputDate callNextForm = new Sci.Production.Logistic.P02_InputDate("Input Receive Date", "Receive Date");
            DialogResult dr = callNextForm.ShowDialog(this);
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                receiveDate = callNextForm.returnDate;
                return base.ClickNewBefore();
            }
            return false;
        }

        //新增帶入預設值後再執行LOGISTIC->P02_BatchReceiving
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["ReceiveDate"] = receiveDate;
            CurrentMaintain["MDivisionID"] = Sci.Env.User.Keyword;
            CurrentMaintain["Status"] = "New";
            ShowStatus();
            CallBatchReceive();
        }

        //檢查表身不可以沒有資料
        protected override bool ClickSaveBefore()
        {
            DataRow[] detailData = ((DataTable)detailgridbs.DataSource).Select();
            if (detailData.Length == 0)
            {
                MyUtility.Msg.WarningBox("Detail can't empty!");
                return false;
            }
            if (IsDetailInserting)
            {
                string id = MyUtility.GetValue.GetID(Sci.Env.User.Keyword + "CR", "ClogReceive", Convert.ToDateTime(CurrentMaintain["ReceiveDate"].ToString()), 2, "Id", null);
                if (MyUtility.Check.Empty(id))
                {
                    MyUtility.Msg.WarningBox("GetID fail, please try again!");
                    return false;
                }
                CurrentMaintain["ID"] = id;
            }
            return base.ClickSaveBefore();
        }

        //Confirm
        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            string sqlCmd, wrongCtn = "", lostCtn = "";
            DualResult result;
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
                MyUtility.Msg.WarningBox("Connection fail!");
                return;
            }
            if (selectDate.Rows.Count > 0)
            {
                foreach (DataRow eachRow in selectDate.Rows)
                {
                    wrongCtn = wrongCtn + string.Format("Pack ID:{0}  SP#:{1}   CTN#:{2}\r\n", eachRow["PackingListId"].ToString(), eachRow["OrderId"].ToString(), eachRow["CTNStartNo"].ToString());
                }
            }
            if (!MyUtility.Check.Empty(wrongCtn))
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
                MyUtility.Msg.WarningBox("Connection fail!");
                return;
            }
            if (selectDate.Rows.Count > 0)
            {
                foreach (DataRow eachRow in selectDate.Rows)
                {
                    lostCtn = lostCtn + string.Format("Trans. Slip#:{0}   Pack ID:{1}  SP#:{2}   CTN#:{3}\r\n", eachRow["ID"].ToString(), eachRow["PackingListId"].ToString(), eachRow["OrderId"].ToString(), eachRow["CTNStartNo"].ToString());
                }
            }
            if (!MyUtility.Check.Empty(lostCtn))
            {
                lostCtn = "Lacking Rev#\r\n" + lostCtn;
            }
            #endregion
            if (!MyUtility.Check.Empty(wrongCtn) || !MyUtility.Check.Empty(lostCtn))
            {
                MyUtility.Msg.WarningBox(wrongCtn + lostCtn);
                return;
            }

            //update ClogReceive & PackingList_Detail data
            sqlCmd = string.Format(@"select PackingListId,CTNStartNo,ClogLocationId 
                                                        from ClogReceive_Detail 
                                                        where ID = '{0}'", CurrentMaintain["ID"].ToString());
            result = DBProxy.Current.Select(null, sqlCmd, out selectDate);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Connection fail!");
                return;
            }

            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    IList<string> updateCmds = new List<string>();
                    foreach (DataRow eachRow in selectDate.Rows)
                    {
                        updateCmds.Add(string.Format(@"update PackingList_Detail 
                                             set ClogReceiveId = '{0}', ReceiveDate = '{1}', ClogLocationId = '{2}' 
                                             where ID = '{3}' and CTNStartNo = '{4}';", CurrentMaintain["ID"].ToString(), Convert.ToDateTime(CurrentMaintain["ReceiveDate"].ToString()).ToString("d"),
                                                                                                          eachRow["ClogLocationId"].ToString(), eachRow["PackingListId"].ToString(), eachRow["CTNStartNo"].ToString()));
                    }
                    updateCmds.Add(string.Format("update ClogReceive set Status = 'Confirmed', EditName = '{0}', EditDate = GETDATE() where ID = '{1}';", Sci.Env.User.UserID, CurrentMaintain["ID"].ToString()));
                    result = DBProxy.Current.Executes(null, updateCmds);

                    //Update Orders的資料
                    if (!callGetCartonList())
                    {
                        return;
                    }

                    if (result)
                    {
                        transactionScope.Complete();
                    }
                    else
                    {
                        transactionScope.Dispose();
                        MyUtility.Msg.WarningBox("Confirm failed !\r\n" + result.ToString());
                        return;
                    }
                }
                catch (Exception ex)
                {
                    transactionScope.Dispose();
                    ShowErr("Confirm transaction error.", ex);
                    return;
                }
            }

            RefreshToolBar();
        }

        //UnConfirm
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            //檢查是否有箱子已被Clog Return，若有則出訊息告知且不做任何事
            DataTable selectData;
            string sqlCmd = string.Format("select count(b.id) as returnCTN from ClogReceive_Detail a, ClogReturn_Detail b where a.TransferToClogId = b.TransferToClogId and a.PackingListId = b.PackingListId and a.OrderId = b.OrderId and a.CTNStartNo = b.CTNStartNo and a.ID = '{0}'", CurrentMaintain["ID"].ToString());
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out selectData);
            if (!MyUtility.Check.Empty(selectData.Rows[0]["returnCTN"]))
            {
                MyUtility.Msg.WarningBox("This recode has return record, con not unconfirm!");
                return;
            }

            //先問使用者是否確定要Unconfirm，若是才做更新
            DialogResult unconfirmResult;
            unconfirmResult = MyUtility.Msg.WarningBox("Are you sure unconfirm this data?", buttons: MessageBoxButtons.YesNo);
            if (unconfirmResult == System.Windows.Forms.DialogResult.Yes)
            {
                using (TransactionScope transactionScope = new TransactionScope())
                {
                    try
                    {
                        string sqlCmd1 = string.Format("update ClogReceive set Status = 'New', EditName = '{0}', EditDate = GETDATE() where ID = '{1}'", Sci.Env.User.UserID, CurrentMaintain["ID"].ToString());
                        string sqlCmd2 = string.Format("update PackingList_Detail set ClogReceiveId = '', ReceiveDate = null, ClogLocationId = '' where ClogReceiveId = '{0}'", CurrentMaintain["ID"].ToString());
                        DualResult result1 = DBProxy.Current.Execute(null, sqlCmd1);
                        DualResult result2 = DBProxy.Current.Execute(null, sqlCmd2);
                        //Update Orders的資料
                        if (!callGetCartonList())
                        {
                            return;
                        }

                        if (result1 && result2)
                        {
                            transactionScope.Complete();
                        }
                        else
                        {
                            transactionScope.Dispose();
                            string failMsg = "";
                            if (!result1)
                            {
                                failMsg = result1.ToString() + "\r\n";
                            }

                            if (!result2)
                            {
                                failMsg = failMsg + result2.ToString();
                            }

                            MyUtility.Msg.WarningBox("Unconfirm fail !\r\n" + failMsg);
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        transactionScope.Dispose();
                        MyUtility.Msg.ErrorBox("Connection transaction error.\r\n" + ex.ToString());
                        return;
                    }
                }

                RefreshToolBar();
            }
        }

        //Update Orders的資料
        private bool callGetCartonList()
        {
            DataTable selectData;
            string sqlCmd = string.Format("select OrderID from ClogReceive_Detail where ID = '{0}' group by OrderID", CurrentMaintain["ID"].ToString());
            DualResult result1 = DBProxy.Current.Select(null, sqlCmd, out selectData);
            if (!result1)
            {
                MyUtility.Msg.WarningBox("Select update orders data fail!\r\n" + result1.ToString());
                return false;
            }
            DualResult prgResult = Prgs.UpdateOrdersCTN(selectData);
            if (!prgResult)
            {
                MyUtility.Msg.WarningBox("Update orders data fail!\r\n" + prgResult.ToString());
                return false;
            }
            return true;
        }

        //Batch Receive，執行LOGISTIC->P02_BatchReceiving
        private void button2_Click(object sender, EventArgs e)
        {
            CallBatchReceive();
        }

        //呼叫Batch Receive
        private void CallBatchReceive()
        {
            Sci.Production.Logistic.P02_BatchReceiving callNextForm = new Sci.Production.Logistic.P02_BatchReceiving(Convert.ToDateTime(CurrentMaintain["ReceiveDate"].ToString()), (DataTable)detailgridbs.DataSource);
            callNextForm.ShowDialog(this);
        }

        //Status顯示
        private void ShowStatus()
        {
            this.label1.Text = CurrentMaintain["Status"].ToString();
        }

        //(Un)Confirm後更新資料內容
        private void RefreshToolBar()
        {
            RenewData();
            OnDetailEntered();
            EnsureToolbarExt();
        }
    }
}

