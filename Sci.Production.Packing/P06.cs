using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;
using Sci.Production.PublicPrg;

namespace Sci.Production.Packing
{
    public partial class P06 : Sci.Win.Tems.Input6
    {
        Ict.Win.UI.DataGridViewTextBoxColumn col_ctnno;
        Ict.Win.UI.DataGridViewTextBoxColumn col_article;
        Ict.Win.UI.DataGridViewTextBoxColumn col_size;
        Ict.Win.UI.DataGridViewNumericBoxColumn col_ctnqty;
        Ict.Win.UI.DataGridViewNumericBoxColumn col_shipqty;
        Ict.Win.DataGridViewGeneratorTextColumnSettings article = new Ict.Win.DataGridViewGeneratorTextColumnSettings();
        Ict.Win.DataGridViewGeneratorTextColumnSettings size = new Ict.Win.DataGridViewGeneratorTextColumnSettings();

        private string sqlCmd;
        private DataRow dr;
        private DualResult result;
        private DialogResult buttonResult;
        private DataTable selectedData;
        private string masterID, OrderID, OrderSeqID;

        public P06(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.DefaultFilter = "FactoryID = '" + Sci.Env.User.Factory + "' AND Type = 'L'";
            detailgrid.AllowUserToOrderColumns = true;
            InsertDetailGridOnDoubleClick = false;
        }

        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            masterID = (e.Master == null) ? "" : e.Master["ID"].ToString();
            OrderID = (e.Master == null) ? "" : e.Master["OrderID"].ToString();
            OrderSeqID = (e.Master == null) ? "" : e.Master["OrderShipmodeSeq"].ToString();
            this.DetailSelectCommand = string.Format(@"with AccuPKQty
as
(select Article, SizeCode ,sum(ShipQty) as TtlShipQty
 from PackingList_Detail
 where ID != '{0}'
 and OrderID = '{1}'
and OrderShipmodeSeq = '{2}'
 group by Article, SizeCode
),
PulloutAdjQty
as
(select iaq.Article, iaq.SizeCode, sum(iaq.DiffQty) as TtlDiffQty
 from InvAdjust ia, InvAdjust_Qty iaq
 where ia.OrderID = '{1}'
 and ia.OrderShipmodeSeq = '{2}'
 and ia.ID = iaq.ID
 group by iaq.Article, iaq.SizeCode
),
PackQty
as
(select Article, SizeCode, sum(ShipQty) as ShipQty
 from PackingList_Detail
 where ID = '{0}'
 group by Article, SizeCode
)
select a.ID,a.OrderID,a.OrderShipmodeSeq,a.CTNStartNo,a.CTNQty, a.RefNo, a.Article, a.SizeCode, a.ShipQty,a.TransferToClogID, a.TransferDate, a.ReceiveDate, a.ClogLocationId, a.ReturnDate, b.Description, oqd.Qty-isnull(pd.TtlShipQty,0)+isnull(paq.TtlDiffQty,0)-pk.ShipQty as BalanceQty, a.Seq
from PackingList_Detail a
left join LocalItem b on b.RefNo = a.RefNo
left join AccuPKQty pd on pd.Article = a.Article and pd.SizeCode = a.SizeCode
left join PulloutAdjQty paq on paq.Article = a.Article and paq.SizeCode = a.SizeCode
left join PackQty pk on pk.Article = a.Article and pk.SizeCode = a.SizeCode
left join Order_QtyShip_Detail oqd on oqd.Id = a.OrderID and oqd.Seq = a.OrderShipmodeSeq and oqd.Article = a.Article and oqd.SizeCode = a.SizeCode
where a.id = '{0}'
order by a.Seq", masterID, OrderID, OrderSeqID);

            return base.OnDetailSelectCommandPrepare(e);
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();

            //帶出Orders相關欄位
            sqlCmd = string.Format("select StyleID,SeasonID,CustPONo,Customize1,ReadyDate from Orders where ID = '{0}'", CurrentMaintain["OrderID"].ToString());
            if (MyUtility.Check.Seek(sqlCmd, out dr))
            {
                displayBox2.Value = dr["StyleID"].ToString();
                displayBox4.Value = dr["SeasonID"].ToString();
                displayBox5.Value = dr["CustPONo"].ToString();
            }
        }

        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();

            #region Article & SizeCode按右鍵與Validating
            article.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode)
                {
                    if (e.Button == System.Windows.Forms.MouseButtons.Right)
                    {
                        if (e.RowIndex != -1)
                        {
                            DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                            string sqlCmd = string.Format("Select Distinct Article from Order_QtyShip_Detail where ID = '{0}' and Seq = '{1}'", CurrentMaintain["OrderID"].ToString(), CurrentMaintain["OrderShipmodeSeq"].ToString());
                            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlCmd, "8", dr["Article"].ToString());
                            DialogResult returnResult = item.ShowDialog();
                            if (returnResult == DialogResult.Cancel) { return; }
                            e.EditingControl.Text = item.GetSelectedString();
                        }
                    }
                }
            };

            article.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    //檢查箱子如果有送到Clog則不可以被修改
                    if (e.FormattedValue.ToString() != dr["Article"].ToString())
                    {
                        if (!CheckCanCahngeCol(dr["TransferToClogID"].ToString()))
                        {
                            dr["Article"] = dr["Article"].ToString();
                            e.Cancel = true;
                            return;
                        }
                    }

                    if (!MyUtility.Check.Empty(e.FormattedValue) && e.FormattedValue.ToString() != dr["Article"].ToString())
                    {
                        if (!MyUtility.Check.Seek(string.Format("Select Article from Order_QtyShip_Detail where ID = '{0}' and Seq = '{1}' and Article = '{2}'", CurrentMaintain["OrderID"].ToString(), CurrentMaintain["OrderShipmodeSeq"].ToString(), e.FormattedValue.ToString())))
                        {
                            MyUtility.Msg.WarningBox(string.Format("< Article: {0} > not found!!!", e.FormattedValue.ToString()));
                            dr["Article"] = "";
                            dr["SizeCode"] = "";
                            dr.EndEdit();
                        }
                    }
                }
            };

            size.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode)
                {
                    if (e.Button == System.Windows.Forms.MouseButtons.Right)
                    {
                        if (e.RowIndex != -1)
                        {
                            DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                            string sqlCmd = string.Format(@"Select oqd.SizeCode 
from Order_QtyShip_Detail oqd
left join Orders o on o.ID = oqd.Id
left join Order_SizeCode os on os.ID = o.POID and os.SizeCode = oqd.SizeCode
where oqd.ID = '{0}' and oqd.Seq = '{1}' and oqd.Article = '{2}' 
order by os.Seq", CurrentMaintain["OrderID"].ToString(), CurrentMaintain["OrderShipmodeSeq"].ToString(), dr["Article"].ToString());
                            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlCmd, "8", dr["SizeCode"].ToString());
                            DialogResult returnResult = item.ShowDialog();
                            if (returnResult == DialogResult.Cancel) { return; }
                            e.EditingControl.Text = item.GetSelectedString();
                        }
                    }
                }
            };

            size.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    //檢查箱子如果有送到Clog則不可以被修改
                    if (e.FormattedValue.ToString() != dr["SizeCode"].ToString())
                    {
                        if (!CheckCanCahngeCol(dr["TransferToClogID"].ToString()))
                        {
                            dr["SizeCode"] = dr["SizeCode"].ToString();
                            e.Cancel = true;
                            return;
                        }
                    }

                    if (!MyUtility.Check.Empty(e.FormattedValue) && e.FormattedValue.ToString() != dr["SizeCode"].ToString())
                    {
                        if (!MyUtility.Check.Seek(string.Format("Select SizeCode from Order_QtyShip_Detail where ID = '{0}' and Seq = '{1}' and Article = '{2}' and SizeCode = '{3}'", CurrentMaintain["OrderID"].ToString(), CurrentMaintain["OrderShipmodeSeq"].ToString(), dr["Article"].ToString(), e.FormattedValue.ToString())))
                        {
                            MyUtility.Msg.WarningBox(string.Format("< SizeCode: {0} > not found!!!", e.FormattedValue.ToString()));
                            dr["SizeCode"] = "";
                            e.Cancel = true;
                            return;
                        }
                    }
                }
            };
            #endregion

            Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("CTNStartNo", header: "CTN#", width: Widths.AnsiChars(6)).Get(out col_ctnno)
                .Numeric("CTNQty", header: "# of CTN").Get(out col_ctnqty)
                .Text("Article", header: "ColorWay", width: Widths.AnsiChars(8), settings: article).Get(out col_article)
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(8), settings: size).Get(out col_size)
                .Numeric("ShipQty", header: "Qty").Get(out col_shipqty)
                .Numeric("BalanceQty", header: "Bal. Qty", iseditingreadonly: true)
                .Date("TransferDate", header: "Transfer CLOG", iseditingreadonly: true)
                .Date("ReceiveDate", header: "CLOG CFM", iseditingreadonly: true)
                .Text("ClogLocationId", header: "Location No.", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Date("ReturnDate", header: "Return Date", iseditingreadonly: true);

            #region 欄位的Validating
            this.detailgrid.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    #region 檢查箱子如果有送到Clog則不可以被修改
                    if (detailgrid.Columns[e.ColumnIndex].DataPropertyName == col_ctnno.DataPropertyName)
                    {
                        if (!MyUtility.Check.Empty(e.FormattedValue))
                        {
                            if (e.FormattedValue.ToString() != dr["CTNStartNo"].ToString())
                            {
                                if (!CheckCanCahngeCol(dr["TransferToClogID"].ToString()))
                                {
                                    dr["CTNStartNo"] = dr["CTNStartNo"].ToString();
                                    e.Cancel = true;
                                    return;
                                }
                            }
                        }
                    }

                    if (detailgrid.Columns[e.ColumnIndex].DataPropertyName == col_shipqty.DataPropertyName)
                    {
                        if (!MyUtility.Check.Empty(e.FormattedValue))
                        {
                            if (e.FormattedValue.ToString() != dr["ShipQty"].ToString())
                            {
                                if (!CheckCanCahngeCol(dr["TransferToClogID"].ToString()))
                                {
                                    dr["ShipQty"] = dr["ShipQty"].ToString();
                                    e.Cancel = true;
                                    return;
                                }
                            }
                        }
                    }
                    #endregion

                    if (detailgrid.Columns[e.ColumnIndex].DataPropertyName == col_ctnqty.DataPropertyName)
                    {
                        if (!MyUtility.Check.Empty(e.FormattedValue))
                        {
                            if (e.FormattedValue.ToString() != dr["CTNQty"].ToString())
                            {
                                if (e.FormattedValue.ToString() != "0" && e.FormattedValue.ToString() != "1")
                                {
                                    MyUtility.Msg.WarningBox("# of CTN only keyin 1 or 0");
                                    dr["CTNQty"] = 0;
                                    e.Cancel = true;
                                    return;
                                }
                            }
                        }
                    }
                }
            };

            #endregion
        }

        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            txtshipmode1.ReadOnly = false;
            CurrentMaintain["FactoryID"] = Sci.Env.User.Factory;
            CurrentMaintain["Type"] = "L";
            CurrentMaintain["Status"] = "New";
            CurrentMaintain["Dest"] = "ZZ";
        }

        protected override bool ClickEditBefore()
        {
            if (CurrentMaintain["Status"].ToString() == "Confirmed")
            {
                MyUtility.Msg.WarningBox("This record is < Confirmed >, can't be modified!");
                return false;
            }
            return base.ClickEditBefore();
        }

        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();

            //當表身有任何一個箱子被送到Clog：SP#不可以被修改
            DataRow[] detailData = ((DataTable)detailgridbs.DataSource).Select("ClogReceiveID <> ''");
            if (detailData.Length != 0)
            {
                textBox1.ReadOnly = true;
            }
        }

        protected override bool ClickSaveBefore()
        {
            if (!MyUtility.Check.Empty(CurrentMaintain["PulloutDate"]))
            {
                //Pullout date不可小於System的Pullout lock date
                string pullLock = MyUtility.GetValue.Lookup("select PullLock from System");
                if (Convert.ToDateTime(CurrentMaintain["PulloutDate"].ToString()) < Convert.ToDateTime(pullLock))
                {
                    MyUtility.Msg.WarningBox("Pullou date less then pullout lock date!!");
                    dateBox1.Focus();
                    return false;
                }

                //如果Pullout report已存在且狀態為Confirmed時，需出訊息告知
                if (MyUtility.Check.Seek(string.Format("select ID,status from Pullout where PulloutDate = '{0}' and FactoryID = '{1}'", Convert.ToDateTime(CurrentMaintain["PulloutDate"].ToString()).ToString("d"), Sci.Env.User.Factory), out dr))
                {
                    if (dr["Status"].ToString() == "Confirmed")
                    {
                        MyUtility.Msg.WarningBox("Pullout date already exist pullout report and have been confirmed!");
                        dateBox1.Focus();
                        return false;
                    }
                }
            }

            //檢查欄位值不可為空
            if (MyUtility.Check.Empty(CurrentMaintain["OrderID"]))
            {
                MyUtility.Msg.WarningBox("SP# can't empty!!");
                textBox1.Focus();
                return false;
            }

            //檢查OrderID+Seq不可以重複建立
            if (MyUtility.Check.Seek(string.Format("select ID from PackingList where OrderID = '{0}' AND OrderShipmodeSeq = '{1}' AND ID != '{2}'", CurrentMaintain["OrderID"].ToString(), CurrentMaintain["OrderShipmodeSeq"].ToString(), IsDetailInserting ? "" : CurrentMaintain["ID"].ToString())))
            {
                MyUtility.Msg.WarningBox("SP No:" + CurrentMaintain["OrderID"].ToString() + ", Seq:" + CurrentMaintain["OrderShipmodeSeq"].ToString() + " already exist in packing list, can't be create again!");
                return false;
            }

            //表身的CTN#,Color Way與Size不可以為空值，順便填入OrderID, OrderShipmodeSeq與Seq欄位值，計算CTNQty, ShipQty，重算表身Grid的Bal. Qty
            int i = 0, ctnQty = 0, shipQty = 0, ttlShipQty = 0, needPackQty = 0, count = 0;
            string filter = "", sqlCmd;
            bool isNegativeBalQty = false;
            DataTable needPackData;
            DualResult selectResult;
            DataRow[] detailData;
            #region 先將此Packinglist的各Article & SizeCode尚未裝箱件數撈出來
            sqlCmd = string.Format(@"select oqd.Article,oqd.SizeCode,(oqd.Qty-isnull(sum(pd.ShipQty), 0) - isnull(sum(iaq.DiffQty), 0)) as Qty
from Order_QtyShip_Detail oqd
left join PackingList_Detail pd on pd.ID != '{0}' and oqd.Id = pd.OrderID and oqd.Seq = pd.OrderShipmodeSeq and oqd.Article = pd.Article and oqd.SizeCode = pd.SizeCode
left join InvAdjust ia on ia.OrderID = pd.OrderID and ia.OrderShipmodeSeq = pd.OrderShipmodeSeq
left join InvAdjust_Qty iaq on iaq.ID = ia.ID and iaq.Article = pd.Article and iaq.SizeCode = pd.SizeCode
where oqd.ID = '{1}'
group by oqd.Article,oqd.SizeCode, oqd.Qty", CurrentMaintain["ID"].ToString(), CurrentMaintain["OrderID"].ToString());
            if (!(selectResult = DBProxy.Current.Select(null, sqlCmd, out needPackData)))
            {
                MyUtility.Msg.WarningBox("Query pack qty fail!");
                return false;
            }
            #endregion

            foreach (DataRow dr in ((DataTable)detailgridbs.DataSource).Rows)
            {
                #region 表身的CTN#, Ref No., Color Way與Size不可以為空值
                if (MyUtility.Check.Empty(dr["CTNStartNo"]))
                {
                    MyUtility.Msg.WarningBox("< CTN# >  can't empty!");
                    return false;
                }

                if (MyUtility.Check.Empty(dr["Article"]))
                {
                    MyUtility.Msg.WarningBox("< ColorWay >  can't empty!");
                    return false;
                }

                if (MyUtility.Check.Empty(dr["SizeCode"]))
                {
                    MyUtility.Msg.WarningBox("< Size >  can't empty!");
                    return false;
                }
                #endregion

                #region 填入OrderID, OrderShipmodeSeq,Seq與CTNQty欄位值
                i = i + 1;
                dr["OrderID"] = CurrentMaintain["OrderID"].ToString();
                dr["OrderShipmodeSeq"] = CurrentMaintain["OrderShipmodeSeq"].ToString();
                dr["Seq"] = Convert.ToString(i).PadLeft(6, '0');
                if (MyUtility.Check.Empty(dr["CTNQty"]))
                {
                    dr["CTNQty"] = 0;
                }
                #endregion

                #region 計算CTNQty, ShipQty
                ctnQty = ctnQty + Convert.ToInt32(dr["CTNQty"].ToString());
                shipQty = shipQty + Convert.ToInt32(dr["ShipQty"].ToString());
                #endregion

                #region 重算表身Grid的Bal. Qty
                //目前還有多少衣服尚未裝箱
                needPackQty = 0;
                filter = string.Format("Article = '{0}' and SizeCode = '{1}'", dr["Article"].ToString(), dr["SizeCode"].ToString());
                detailData = needPackData.Select(filter);
                if (detailData.Length > 0)
                {
                    needPackQty = Convert.ToInt32(detailData[0]["Qty"].ToString());
                }

                //加總表身特定Article/SizeCode的Ship Qty數量
                detailData = ((DataTable)detailgridbs.DataSource).Select(filter);
                ttlShipQty = 0;
                if (detailData.Length != 0)
                {
                    foreach (DataRow dDr in detailData)
                    {
                        ttlShipQty = ttlShipQty + Convert.ToInt32(dDr["ShipQty"].ToString());
                    }
                }

                dr["BalanceQty"] = needPackQty - ttlShipQty;
                if (needPackQty - ttlShipQty < 0)
                {
                    isNegativeBalQty = true;
                    detailgrid.Rows[count].DefaultCellStyle.BackColor = Color.Pink;
                }
                #endregion
                count = count + 1;
            }
            //CTNQty, ShipQty
            CurrentMaintain["CTNQty"] = ctnQty;
            CurrentMaintain["ShipQty"] = shipQty;

            if (isNegativeBalQty)
            {
                MyUtility.Msg.WarningBox("Quantity entered is greater than order quantity!!");
                return false;
            }

            //GetID
            if (IsDetailInserting)
            {
                string id = MyUtility.GetValue.GetID(Sci.Env.User.Keyword + "LS", "PackingList", DateTime.Today, 2, "Id", null);
                if (MyUtility.Check.Empty(id))
                {
                    MyUtility.Msg.WarningBox("GetID fail, please try again!");
                    return false;
                }
                CurrentMaintain["ID"] = id;
            }
            return base.ClickSaveBefore();
        }

        protected override bool ClickDeleteBefore()
        {
            if (CurrentMaintain["Status"].ToString() == "Confirmed")
            {
                MyUtility.Msg.WarningBox("This record is < Confirmed >, can't be deleted!");
                return false;
            }

            //檢查表身不可以有箱子送至Clog
            foreach (DataRow dr in ((DataTable)detailgridbs.DataSource).Rows)
            {
                if (!MyUtility.Check.Empty(dr["TransferDate"]))
                {
                    MyUtility.Msg.WarningBox(dr["CTNStartNo"].ToString() + " had been send to Clog, can't be deleted!");
                    return false;
                }
            }

            return base.ClickDeleteBefore();
        }

        //檢查輸入的SP#是否正確
        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            if (EditMode)
            {
                if (textBox1.Text != textBox1.OldValue)
                {
                    if (!MyUtility.Check.Empty(textBox1.Text))
                    {
                        string sqlCmd = string.Format("select ID, StyleID, SeasonID, CustPONo, LocalOrder from Orders where ID = '{0}' and FtyGroup = '{1}'", textBox1.Text, Sci.Env.User.Factory);
                        if (MyUtility.Check.Seek(sqlCmd, out dr))
                        {
                            if (dr["LocalOrder"].ToString() == "False")
                            {
                                MyUtility.Msg.WarningBox("This SP# is not local order!");
                                //OrderID異動，其他相關欄位要跟著異動
                                ChangeOtherData("");
                                textBox1.Text = "";
                                e.Cancel = true;
                                return;
                            }
                        }
                        else
                        {
                            MyUtility.Msg.WarningBox("< SP# > does not exist!");
                            //OrderID異動，其他相關欄位要跟著異動
                            ChangeOtherData("");
                            textBox1.Text = "";
                            e.Cancel = true;
                            return;
                        }
                    }
                }
            }
        }

        // SP#輸入完成後要帶入其他欄位值
        private void textBox1_Validated(object sender, EventArgs e)
        {
            if (textBox1.OldValue == textBox1.Text)
            {
                return;
            }

            //OrderID異動，其他相關欄位要跟著異動
            ChangeOtherData(textBox1.Text);
        }

        //OrderID異動，其他相關欄位要跟著異動
        private void ChangeOtherData(string orderID)
        {
            // 清空表身Grid資料
            foreach (DataRow dr in DetailDatas)
            {
                dr.Delete();
            }

            CurrentMaintain["CTNQty"] = 0;

            if (MyUtility.Check.Empty(orderID))
            {
                //OrderID為空值時，要把其他相關欄位值清空
                CurrentMaintain["OrderShipmodeSeq"] = "";
                CurrentMaintain["ShipModeID"] = "";
                CurrentMaintain["BrandID"] = "";
                CurrentMaintain["CustCDID"] = "";
                displayBox2.Value = "";
                displayBox4.Value = "";
                displayBox5.Value = "";
            }
            else
            {
                sqlCmd = string.Format("select StyleID,SeasonID,CustPONo,Customize1,ReadyDate,BrandID,CustCDID,Dest from Orders where ID = '{0}'", orderID);
                if (MyUtility.Check.Seek(sqlCmd, out dr))
                {
                    //帶出相關欄位的資料
                    displayBox2.Value = dr["StyleID"].ToString();
                    displayBox4.Value = dr["SeasonID"].ToString();
                    displayBox5.Value = dr["CustPONo"].ToString();
                    CurrentMaintain["BrandID"] = dr["BrandID"].ToString();
                    CurrentMaintain["CustCDID"] = dr["CustCDID"].ToString();

                    #region 若Order_QtyShip有多筆資料話就跳出視窗讓使者選擇Seq
                    sqlCmd = string.Format("select count(ID) as CountID from Order_QtyShip where ID = '{0}'", orderID);
                    if (MyUtility.Check.Seek(sqlCmd, out dr))
                    {
                        if (dr["CountID"].ToString() == "1")
                        {
                            sqlCmd = string.Format("select ShipModeID,BuyerDelivery,Seq from Order_QtyShip where ID = '{0}'", orderID);
                            if (MyUtility.Check.Seek(sqlCmd, out dr))
                            {
                                CurrentMaintain["OrderShipmodeSeq"] = dr["Seq"].ToString();
                                CurrentMaintain["ShipModeID"] = dr["ShipModeID"].ToString();
                            }
                        }
                        else
                        {
                            IList<DataRow> orderQtyShipData;
                            sqlCmd = string.Format("select Seq,BuyerDelivery,ShipmodeID,Qty from Order_QtyShip where ID = '{0}'", orderID);
                            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlCmd, "4,20,20,10", "", "Seq,Buyer Delivery,ShipMode,Qty");
                            DialogResult returnResult = item.ShowDialog();
                            if (returnResult == DialogResult.Cancel)
                            {
                                CurrentMaintain["OrderShipmodeSeq"] = "";
                                CurrentMaintain["ShipModeID"] = "";
                            }
                            else
                            {
                                orderQtyShipData = item.GetSelecteds();
                                CurrentMaintain["OrderShipmodeSeq"] = item.GetSelectedString();
                                CurrentMaintain["ShipModeID"] = orderQtyShipData[0]["ShipmodeID"].ToString();
                            }
                        }
                    }
                    #endregion

                    //產生表身Grid的資料
                    GenDetailData(orderID, CurrentMaintain["OrderShipmodeSeq"].ToString());
                }
            }
        }

        //Seq按右鍵功能
        private void textBox2_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            if (textBox1.ReadOnly == false)
            {
                IList<DataRow> orderQtyShipData;
                string sqlCmd = string.Format("select Seq,BuyerDelivery,ShipmodeID,Qty from Order_QtyShip where ID = '{0}'", CurrentMaintain["OrderID"].ToString());
                Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlCmd, "4,20,20,10", "", "Seq,Buyer Delivery,ShipMode,Qty");
                DialogResult returnResult = item.ShowDialog();
                if (returnResult == DialogResult.Cancel)
                {
                    CurrentMaintain["OrderShipmodeSeq"] = "";
                    CurrentMaintain["ShipModeID"] = "";
                    dateBox1.Value = null;
                }
                else
                {
                    orderQtyShipData = item.GetSelecteds();
                    CurrentMaintain["OrderShipmodeSeq"] = item.GetSelectedString();
                    CurrentMaintain["ShipModeID"] = orderQtyShipData[0]["ShipmodeID"].ToString();
                }
                //產生表身Grid的資料
                GenDetailData(CurrentMaintain["OrderID"].ToString(), CurrentMaintain["OrderShipmodeSeq"].ToString());
            }
        }

        //檢查箱子是否有送至Clog來決定欄位是否可被修改
        private bool CheckCanCahngeCol(string transferToClogID)
        {
            if (MyUtility.Check.Empty(transferToClogID))
            {
                return true;
            }
            else
            {
                MyUtility.Msg.WarningBox("This record had been send to CLOG, can't modified!!");
                return false;
            }
        }

        //產生表身Grid的資料
        private void GenDetailData(string orderID, string seq)
        {
            // 清空表身Grid資料
            foreach (DataRow dr in DetailDatas)
            {
                dr.Delete();
            }
            if (!MyUtility.Check.Empty(orderID) && !MyUtility.Check.Empty(orderID))
            {
                sqlCmd = string.Format(@"select '' as ID,o.ID as OrderID, oqd.Seq as OrderShipmodeSeq, oqd.Article, oqd.SizeCode, oqd.Qty as ShipQty, o.CTNQty as QtyPerCTN
                                                            from Order_QtyShip_Detail oqd
                                                            left Join Orders o on o.ID = oqd.Id
                                                            left join Order_SizeCode os on os.id = o.POID and os.SizeCode = oqd.SizeCode
                                                            left join Order_Article oa on oa.id = oqd.Id and oa.Article = oqd.Article
                                                            where oqd.ID = '{0}' and oqd.Seq = '{1}'
                                                            order by oa.Seq,os.Seq", orderID, seq);

                if (result = DBProxy.Current.Select(null, sqlCmd, out selectedData))
                {
                    foreach (DataRow dr in selectedData.Rows)
                    {
                        dr.AcceptChanges();
                        dr.SetAdded();
                        ((DataTable)detailgridbs.DataSource).ImportRow(dr);
                    }
                }
            }
        }

        //Pull-out Date Validating()
        private void dateBox1_Validating(object sender, CancelEventArgs e)
        {
            if (!MyUtility.Check.Empty(dateBox1.Value) && dateBox1.Value != dateBox1.OldValue)
            {
                if (MyUtility.Check.Seek(string.Format("select ID,status from Pullout where PulloutDate = '{0}' and FactoryID = '{1}'", Convert.ToDateTime(dateBox1.Value.ToString()).ToString("d"), Sci.Env.User.Factory), out dr))
                {
                    if (dr["Status"].ToString() == "Confirmed")
                    {
                        MyUtility.Msg.WarningBox("Pullout date already exist pullout report and have been confirmed!");
                        dateBox1.Value = null;
                        e.Cancel = true;
                        return;
                    }
                }
            }
        }

        //ShipMode
        private void txtshipmode1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.EditMode)
            {
                if (!MyUtility.Check.Empty(txtshipmode1.SelectedValue))
                {
                    if (MyUtility.Check.Empty(CurrentMaintain["OrderShipmodeSeq"]))
                    {
                        MyUtility.Msg.WarningBox("ShipMode is incorrect!");
                        txtshipmode1.SelectedValue = "";
                        return;
                    }
                    else
                    {
                        string sqlCmd = string.Format("select ShipModeID from Order_QtyShip where ID = '{0}' and Seq = '{1}'", CurrentMaintain["OrderID"].ToString(), CurrentMaintain["OrderShipmodeSeq"].ToString());
                        DataRow qtyShipData;
                        if (MyUtility.Check.Seek(sqlCmd, out qtyShipData))
                        {
                            if (qtyShipData["ShipModeID"].ToString() != txtshipmode1.SelectedValue.ToString())
                            {
                                MyUtility.Msg.WarningBox("ShipMode is incorrect!");
                                txtshipmode1.SelectedValue = "";
                                return;
                            }
                        }
                        else
                        {
                            MyUtility.Msg.WarningBox("ShipMode is incorrect!");
                            txtshipmode1.SelectedValue = "";
                            return;
                        }
                    }
                }
            }
        }

        //Confirm
        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            //Pull-out date不可為空
            if (MyUtility.Check.Empty(CurrentMaintain["PulloutDate"]))
            {
                MyUtility.Msg.WarningBox("Pull-out date can't empty!!");
                return;
            }

            string errMesg = "";
            DualResult queryResult;
            DataTable queryData;
            //檢查累計Pullout數不可超過訂單數量
            sqlCmd = string.Format(@"select Article,SizeCode,sum(ShipQty) as ShipQty 
from PackingList_Detail
where ID = '{0}'
group by Article,SizeCode", CurrentMaintain["ID"].ToString());

            queryResult = DBProxy.Current.Select(null, sqlCmd, out queryData);
            if (queryResult)
            {
                foreach (DataRow dr in queryData.Rows)
                {
                    if (!Prgs.CheckPulloutComplete(CurrentMaintain["OrderID"].ToString(), CurrentMaintain["OrderShipmodeSeq"].ToString(), dr["Article"].ToString(), dr["SizeCode"].ToString(), Convert.ToInt32(dr["ShipQty"])))
                    {
                        errMesg = errMesg + "Color Way: " + dr["Article"].ToString() + ", Size: " + dr["SizeCode"].ToString() + "\r\n";
                    }
                }
                if (!MyUtility.Check.Empty(errMesg))
                {
                    MyUtility.Msg.WarningBox("Pullout qty is more than order qty!\n\r" + errMesg);
                    return;
                }
            }
            else
            {
                MyUtility.Msg.WarningBox("Query packinglist fail!");
                return;
            }

            //檢查Sewing Output Qty是否有超過Packing Qty
            errMesg = "";
            sqlCmd = string.Format(@"with PackedDate
as
(select pld.Article,pld.SizeCode,sum(pld.ShipQty) as PackedShipQty
 from PackingList pl, PackingList_Detail pld
 where pld.OrderID = '{0}'
 and pl.ID = pld.ID
 and pl.Status = 'Confirmed'
 group by pld.Article,pld.SizeCode
),
PackingData
as
(select pld.Article,pld.SizeCode,sum(pld.ShipQty) as ShipQty
 from PackingList_Detail pld
 where pld.ID = '{1}'
 group by pld.Article,pld.SizeCode
),
InvadjQty
as
(select iaq.Article, iaq.SizeCode,sum(iaq.DiffQty) as DiffQty
 from InvAdjust ia, InvAdjust_Qty iaq
 where ia.OrderID = '{0}'
 and ia.ID = iaq.ID
 group by iaq.Article, iaq.SizeCode
),
SewingData
as
(select a.Article,a.SizeCode,MIN(a.QAQty) as QAQty
 from (select oq.Article,oq.SizeCode, sl.Location, isnull(sum(sodd.QAQty),0) as QAQty
	   from Orders o
	   left join Order_Qty oq on oq.ID = o.ID
	   left join Style_Location sl on sl.StyleUkey = o.StyleUkey
	   left join SewingOutput_Detail_Detail sodd on sodd.OrderId = o.ID and sodd.Article = oq.Article  and sodd.SizeCode = oq.SizeCode and sodd.ComboType = sl.Location
	   where o.ID = '{0}'
	   group by oq.Article,oq.SizeCode, sl.Location) a
 group by a.Article,a.SizeCode
)
select oq.Article,oq.SizeCode, oq.Qty, isnull(pedd.PackedShipQty,0)+isnull(pingd.ShipQty,0) as PackQty,isnull(iq.DiffQty,0) as DiffQty, isnull(sd.QAQty,0) as QAQty
from Order_Qty oq
left join PackedDate pedd on pedd.Article = oq.Article and pedd.SizeCode = oq.SizeCode
left join PackingData pingd on pingd.Article = oq.Article and pingd.SizeCode = oq.SizeCode
left join InvadjQty iq on iq.Article = oq.Article and iq.SizeCode = oq.SizeCode
left join SewingData sd on sd.Article = oq.Article and sd.SizeCode = oq.SizeCode
where oq.ID = '{0}'
order by oq.Article,oq.SizeCode", CurrentMaintain["OrderID"].ToString(), CurrentMaintain["ID"].ToString());

            queryResult = DBProxy.Current.Select(null, sqlCmd, out queryData);
            if (queryResult)
            {
                foreach (DataRow dr in queryData.Rows)
                {
                    if (!MyUtility.Check.Empty(dr["PackQty"]))
                    {
                        if (Convert.ToInt32(dr["PackQty"]) + Convert.ToInt32(dr["DiffQty"]) > Convert.ToInt32(dr["QAQty"]))
                        {
                            errMesg = errMesg + "Color Way: " + dr["Article"].ToString() + ", Size: " + dr["SizeCode"].ToString() + ", Qty: " + dr["Qty"].ToString() + ", Ship Qty: " + dr["PackQty"].ToString() + ", Sewing Qty:" + dr["QAQty"].ToString() + ((MyUtility.Check.Empty(dr["DiffQty"]) ? "" : ", Adj Qty:" + dr["DiffQty"].ToString())) + "." + (Convert.ToInt32(dr["PackQty"]) + Convert.ToInt32(dr["DiffQty"]) > Convert.ToInt32(dr["Qty"]) ? "   Pullout qty can't exceed order qty," : "") + " Pullout qty can't exceed sewing qty.\r\n";
                        }
                    }
                }
                if (!MyUtility.Check.Empty(errMesg))
                {
                    MyUtility.Msg.WarningBox(errMesg);
                    return;
                }
            }
            else
            {
                MyUtility.Msg.WarningBox("Query sewing fail!");
                return;
            }

            sqlCmd = string.Format("update PackingList set Status = 'Confirmed', EditName = '{0}', EditDate = '{1}' where ID = '{2}'", Sci.Env.User.UserID, DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), CurrentMaintain["ID"].ToString());
            result = DBProxy.Current.Execute(null, sqlCmd);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Confirm fail !\r\n" + result.ToString());
                return;
            }

            RenewData();
            OnDetailEntered();
            EnsureToolbarExt();
        }

        //UnConfirm
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            //如果Pullout Report已被Confirmed就不可以做UnConfirm
            sqlCmd = string.Format(@"select p.Status
from PackingList pl, Pullout p
where pl.ID = '{0}'
and p.ID = pl.PulloutID", CurrentMaintain["ID"].ToString());
            if (MyUtility.Check.Seek(sqlCmd, out dr))
            {
                if (dr["Status"].ToString() == "Confirmed")
                {
                    MyUtility.Msg.WarningBox("Pullout report already confirmed, so can't unconfirm! ");
                    return;
                }
            }

            //問是否要做Unconfirm，確定才繼續往下做
            buttonResult = MyUtility.Msg.WarningBox("Are you sure you want to < Unconfirm > this data?", "Warning", MessageBoxButtons.YesNo);
            if (buttonResult == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            sqlCmd = string.Format("update PackingList set Status = 'New', EditName = '{0}', EditDate = '{1}' where ID = '{2}'", Sci.Env.User.UserID, DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), CurrentMaintain["ID"].ToString());

            result = DBProxy.Current.Execute(null, sqlCmd);
            if (!result)
            {
                MyUtility.Msg.WarningBox("UnConfirm failed, Pleaes re-try");
            }

            RenewData();
            OnDetailEntered();
            EnsureToolbarExt();
        }
    }
}
