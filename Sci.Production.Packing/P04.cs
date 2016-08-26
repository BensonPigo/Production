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
using System.Transactions;
using System.Linq;

namespace Sci.Production.Packing
{
    public partial class P04 : Sci.Win.Tems.Input6
    {
        Ict.Win.UI.DataGridViewTextBoxColumn col_orderid;
        Ict.Win.UI.DataGridViewTextBoxColumn col_seq;
        Ict.Win.UI.DataGridViewTextBoxColumn col_1stctnno;
        Ict.Win.UI.DataGridViewTextBoxColumn col_lastctnno;
        Ict.Win.UI.DataGridViewTextBoxColumn col_refno;
        Ict.Win.UI.DataGridViewTextBoxColumn col_article;
        Ict.Win.UI.DataGridViewTextBoxColumn col_size;
        Ict.Win.UI.DataGridViewNumericBoxColumn col_ctnqty;
        Ict.Win.UI.DataGridViewNumericBoxColumn col_qtyperctn;
        Ict.Win.UI.DataGridViewNumericBoxColumn col_shipqty;
        Ict.Win.UI.DataGridViewNumericBoxColumn col_nw;
        Ict.Win.UI.DataGridViewNumericBoxColumn col_gw;
        Ict.Win.UI.DataGridViewNumericBoxColumn col_nnw;
        Ict.Win.UI.DataGridViewNumericBoxColumn col_nwpcs;
        Ict.Win.DataGridViewGeneratorTextColumnSettings orderid = new Ict.Win.DataGridViewGeneratorTextColumnSettings();
        Ict.Win.DataGridViewGeneratorTextColumnSettings seq = new Ict.Win.DataGridViewGeneratorTextColumnSettings();
        Ict.Win.DataGridViewGeneratorTextColumnSettings article = new Ict.Win.DataGridViewGeneratorTextColumnSettings();
        Ict.Win.DataGridViewGeneratorTextColumnSettings size = new Ict.Win.DataGridViewGeneratorTextColumnSettings();
        private DialogResult buttonResult;
        private DualResult result;
        private DataRow dr;
        private string sqlCmd = "", filter = "", masterID;

        public P04(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.DefaultFilter = "Type = 'S'";
            detailgrid.AllowUserToOrderColumns = true;
            InsertDetailGridOnDoubleClick = false;
        }

        protected override Ict.DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            masterID = (e.Master == null) ? "" : e.Master["ID"].ToString();
            this.DetailSelectCommand = Prgs.QueryPackingListSQLCmd(masterID);

            return base.OnDetailSelectCommandPrepare(e);
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();

            //Purchase Ctn
            displayBox5.Value = MyUtility.Check.Empty(CurrentMaintain["LocalPOID"]) ? "" : "Y";

            //Shipping Lock是否可看見
            label23.Visible = MyUtility.Check.Empty(CurrentMaintain["GMTBookingLock"]) ? false : true;
        }

        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();

            #region OrderID & Seq & Article & SizeCode按右鍵與Validating
            //OrderID
            orderid.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    
                    if (!MyUtility.Check.Empty(e.FormattedValue) && e.FormattedValue.ToString() != dr["OrderID"].ToString())
                    {
                        //sql參數
                        System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter("@orderid", e.FormattedValue.ToString());
                        System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter("@brandid", MyUtility.Convert.GetString(CurrentMaintain["BrandID"]));

                        IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                        cmds.Add(sp1);
                        cmds.Add(sp2);

                        string sqlCmd = "Select ID,StyleID,CustPONo from Orders where ID = @orderid and Category = 'S' and BrandID = @brandid";

                        DataTable orderData;
                        DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out orderData);
                        if (!result)
                        {
                            MyUtility.Msg.WarningBox("Sql connection fail!!\r\n" + result.ToString());
                            ClearGridRowData(dr);
                            e.Cancel = true;
                            return;
                        }
                        else
                        {
                            if (orderData.Rows.Count <= 0)
                            {
                                MessageBox.Show(string.Format("< SP No.: {0} > not found!!!", e.FormattedValue.ToString()));
                                ClearGridRowData(dr);
                                e.Cancel = true;
                                return;
                            }
                            else
                            {
                                dr["OrderID"] = e.FormattedValue.ToString().ToUpper();
                                dr["StyleID"] = orderData.Rows[0]["StyleID"].ToString();
                                dr["CustPONo"] = orderData.Rows[0]["CustPONo"].ToString();
                                dr["Article"] = "";
                                dr["Color"] = "";
                                dr["SizeCode"] = "";
                                #region 若Order_QtyShip有多筆資料話就跳出視窗讓使者選擇Seq
                                DataRow orderQtyData;
                                sqlCmd = string.Format("select count(ID) as CountID from Order_QtyShip where ID = '{0}'", dr["OrderID"].ToString());
                                if (MyUtility.Check.Seek(sqlCmd, out orderQtyData))
                                {
                                    if (orderQtyData["CountID"].ToString() == "1")
                                    {
                                        dr["OrderShipmodeSeq"] = MyUtility.GetValue.Lookup("Seq", dr["OrderID"].ToString(), "Order_QtyShip", "ID");
                                    }
                                    else
                                    {
                                        sqlCmd = string.Format("select Seq,BuyerDelivery,ShipmodeID,Qty from Order_QtyShip where ID = '{0}'", dr["OrderID"].ToString());
                                        Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlCmd, "4,20,20,10", "", "Seq,Buyer Delivery,ShipMode,Qty");
                                        DialogResult returnResult = item.ShowDialog();
                                        if (returnResult == DialogResult.Cancel)
                                        {
                                            CurrentMaintain["OrderShipmodeSeq"] = "";
                                        }
                                        else
                                        {
                                            CurrentMaintain["OrderShipmodeSeq"] = item.GetSelectedString();
                                        }
                                    }
                                }
                                #endregion
                                dr.EndEdit();
                            }
                        }
                    }
                }
            };

            //Seq
            seq.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode & MyUtility.Check.Empty(CurrentMaintain["GMTBookingLock"]))
                {
                    if (e.Button == System.Windows.Forms.MouseButtons.Right)
                    {
                        if (e.RowIndex != -1)
                        {
                            DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                            sqlCmd = string.Format("select Seq, BuyerDelivery,ShipmodeID,Qty from Order_QtyShip where ID = '{0}'", dr["OrderID"].ToString());
                            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlCmd, "4,20,20,10", "", "Seq,Buyer Delivery,ShipMode,Qty");
                            DialogResult returnResult = item.ShowDialog();
                            if (returnResult == DialogResult.Cancel)
                            {
                                return;
                            }
                            else
                            {
                                e.EditingControl.Text = item.GetSelectedString();
                            }
                            dr["Article"] = "";
                            dr["Color"] = "";
                            dr["SizeCode"] = "";
                            dr.EndEdit();
                        }
                    }
                }
            };

            //Article
            article.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode & MyUtility.Check.Empty(CurrentMaintain["GMTBookingLock"]))
                {
                    if (e.Button == System.Windows.Forms.MouseButtons.Right)
                    {
                        if (e.RowIndex != -1)
                        {
                            DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                            sqlCmd = string.Format("Select Distinct Article from Order_QtyShip_Detail where ID = '{0}' and Seq = '{1}'", dr["OrderID"].ToString(), dr["OrderShipmodeSeq"].ToString());
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
                    if (!MyUtility.Check.Empty(e.FormattedValue) && e.FormattedValue.ToString() != dr["Article"].ToString())
                    {
                        if (!MyUtility.Check.Seek(string.Format("Select Article from Order_QtyShip_Detail where ID = '{0}' and Seq = '{1}' and Article = '{2}'", dr["OrderID"].ToString(), dr["OrderShipmodeSeq"].ToString(), e.FormattedValue.ToString())))
                        {
                            MyUtility.Msg.WarningBox(string.Format("< Article: {0} > not found!!!", e.FormattedValue.ToString()));
                            dr["Article"] = "";
                            dr["Color"] = "";
                            dr["SizeCode"] = "";
                        }
                        else
                        {
                            dr["Article"] = e.FormattedValue.ToString().ToUpper();
                            sqlCmd = string.Format(@"select ColorID 
                                                                        from View_OrderFAColor 
                                                                        where ID = '{0}' and Article = '{1}'", dr["OrderID"].ToString(), dr["Article"]);
                            DataRow colorData;
                            if (MyUtility.Check.Seek(sqlCmd, out colorData))
                            {
                                dr["Color"] = colorData["ColorID"].ToString();
                            }
                            else
                            {
                                dr["Color"] = "";
                            }
                        }
                        dr.EndEdit();
                    }
                }
            };

            //SizeCode
            size.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode & MyUtility.Check.Empty(CurrentMaintain["GMTBookingLock"]))
                {
                    if (e.Button == System.Windows.Forms.MouseButtons.Right)
                    {
                        if (e.RowIndex != -1)
                        {
                            DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                            sqlCmd = string.Format(@"Select oqd.SizeCode 
from Order_QtyShip_Detail oqd
left join Orders o on o.ID = oqd.Id
left join Order_SizeCode os on os.ID = o.POID and os.SizeCode = oqd.SizeCode
where oqd.ID = '{0}' and oqd.Seq = '{1}' and oqd.Article = '{2}' 
order by os.Seq", dr["OrderID"].ToString(), dr["OrderShipmodeSeq"].ToString(), dr["Article"].ToString());
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
                    if (!MyUtility.Check.Empty(e.FormattedValue) && e.FormattedValue.ToString() != dr["SizeCode"].ToString())
                    {
                        if (!MyUtility.Check.Seek(string.Format("Select SizeCode from Order_QtyShip_Detail where ID = '{0}' and Seq = '{1}' and Article = '{2}' and SizeCode = '{3}'", dr["OrderID"].ToString(), dr["OrderShipmodeSeq"].ToString(), dr["Article"].ToString(), e.FormattedValue.ToString())))
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
                .Text("OrderID", header: "SP No.", width: Widths.AnsiChars(13), settings: orderid).Get(out col_orderid)
                .Text("OrderShipmodeSeq", header: "Seq", width: Widths.AnsiChars(2), iseditingreadonly: true, settings: seq).Get(out col_seq)
                .Text("StyleID", header: "Style No.", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("CustPONo", header: "P.O. No.", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("CTNStartNo", header: "1st CTN#", width: Widths.AnsiChars(6)).Get(out col_1stctnno)
                .Text("CTNEndNo", header: "Last CTN#", width: Widths.AnsiChars(6)).Get(out col_lastctnno)
                .Numeric("CTNQty", header: "# of CTN").Get(out col_ctnqty)
                .CellCartonItem("RefNo", header: "Ref No.", width: Widths.AnsiChars(13)).Get(out col_refno)
                .Text("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("Article", header: "ColorWay", width: Widths.AnsiChars(8), settings: article).Get(out col_article)
                .Text("Color", header: "Color", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(8), settings: size).Get(out col_size)
                .Numeric("QtyPerCTN", header: "PC/Ctn").Get(out col_qtyperctn)
                .Numeric("ShipQty", header: "Qty").Get(out col_shipqty)
                .Numeric("BalanceQty", header: "Bal. Qty", iseditingreadonly: true)
                .Numeric("NW", header: "N.W./Ctn", integer_places: 3, decimal_places: 3, maximum: 999.999M, minimum: 0).Get(out col_nw)
                .Numeric("GW", header: "G.W./Ctn", integer_places: 3, decimal_places: 3, maximum: 999.999M, minimum: 0).Get(out col_gw)
                .Numeric("NNW", header: "N.N.W./Ctn", integer_places: 3, decimal_places: 3, maximum: 999.999M, minimum: 0).Get(out col_nnw)
                .Numeric("NWPerPcs", header: "N.W./Pcs", integer_places: 2, decimal_places: 3, maximum: 99.999M, minimum: 0).Get(out col_nwpcs);

            this.detailgrid.CellValueChanged += (s, e) =>
            {
                #region 選完RefNo後，要自動帶出Description
                if (detailgrid.Columns[e.ColumnIndex].DataPropertyName == col_refno.DataPropertyName)
                {
                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);

                    if (MyUtility.Check.Empty(dr["RefNo"]))
                    {
                        dr["Description"] = "";
                    }
                    else
                    {
                        string seekSql = string.Format("select Description,Weight from LocalItem where RefNo = '{0}'", dr["RefNo"].ToString());
                        DataRow localItem;
                        if (MyUtility.Check.Seek(seekSql, out localItem))
                        {
                            dr["Description"] = localItem["Description"].ToString();
                        }
                        else
                        {
                            dr["Description"] = "";
                        }
                    }
                    dr.EndEdit();
                }
                #endregion
            };

            for (int i = 0; i < this.detailgrid.ColumnCount; i++)
            {
                this.detailgrid.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        //清空Order相關欄位值
        private void ClearGridRowData(DataRow dr)
        {
            dr["OrderID"] = "";
            dr["OrderShipmodeSeq"] = "";
            dr["Article"] = "";
            dr["Color"] = "";
            dr["SizeCode"] = "";
            dr["StyleID"] = "";
            dr["CustPONo"] = "";
            dr.EndEdit();
        }

        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            txtshipmode1.ReadOnly = false;
            CurrentMaintain["FactoryID"] = Sci.Env.User.Factory;
            CurrentMaintain["Type"] = "S";
            CurrentMaintain["Status"] = "New";
            CurrentMaintain["MDivisionID"] = Sci.Env.User.Keyword;
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
            txtbrand1.ReadOnly = true;
            //部分欄位會依某些條件來決定是否可以被修改
            if (!MyUtility.Check.Empty(CurrentMaintain["GMTBookingLock"]))
            {
                editBox1.ReadOnly = true;
                txtshipmode1.ReadOnly = true;
                gridicon.Append.Enabled = false;
                gridicon.Insert.Enabled = false;
                gridicon.Remove.Enabled = false;
                DetailGridEditing(false);
            }
            else
            {
                DetailGridEditing(true);
            }

            if (!MyUtility.Check.Empty(CurrentMaintain["LocalPOID"]))
            {
                dateBox1.ReadOnly = true;
                dateBox2.ReadOnly = true;
            }
        }

        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(CurrentMaintain["ShipModeID"]))
            {
                MyUtility.Msg.WarningBox("Ship Mode can't empty!!");
                txtshipmode1.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["BrandID"]))
            {
                MyUtility.Msg.WarningBox("Brand can't empty!!");
                txtbrand1.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["Dest"]))
            {
                MyUtility.Msg.WarningBox("Destination can't empty!!");
                txtcountry1.Focus();
                return false;
            }

            //刪除表身SP No.或Qty為空白的資料，檢查表身的Color Way與Size不可以為空值，順便填入Seq欄位值，計算CTNQty, ShipQty, NW, GW, NNW, CBM，重算表身Grid的Bal. Qty
            int i = 0, ctnQty = 0, shipQty = 0, needPackQty = 0, ttlShipQty = 0, count = 0, drCtnQty = 0;
            double nw = 0.0, gw = 0.0, nnw = 0.0, cbm = 0.0;
            string ctnCBM;
            bool isNegativeBalQty = false;
            DataTable needPackData, tmpPackData;
            DualResult selectResult;
            DataRow[] detailData;

            //準備needPackData的Schema
            sqlCmd = "select OrderID, OrderShipmodeSeq, Article, SizeCode, ShipQty as Qty from PackingList_Detail where ID = ''";
            if (!(selectResult = DBProxy.Current.Select(null, sqlCmd, out needPackData)))
            {
                MyUtility.Msg.WarningBox("Query  schema fail!");
                return false;
            }
            foreach (DataRow dr in DetailDatas)
            {
                #region 刪除表身SP No.或Qty為空白的資料
                if (MyUtility.Check.Empty(dr["OrderID"]) || MyUtility.Check.Empty(dr["ShipQty"]))
                {
                    dr.Delete();
                    continue;
                }
                #endregion

                #region 表身的Color Way與Size不可以為空值
                if (MyUtility.Check.Empty(dr["Article"]))
                {
                    MyUtility.Msg.WarningBox("< ColorWay >  can't empty!");
                    detailgrid.Focus();
                    return false;
                }

                if (MyUtility.Check.Empty(dr["SizeCode"]))
                {
                    MyUtility.Msg.WarningBox("< Size >  can't empty!");
                    detailgrid.Focus();
                    return false;
                }
                #endregion

                #region 填入Seq欄位值
                i = i + 1;
                dr["Seq"] = Convert.ToString(i).PadLeft(6, '0');
                #endregion

                #region 計算CTNQty, ShipQty, NW, GW, NNW, CBM
                drCtnQty = (MyUtility.Check.Empty(dr["CTNQty"]) ? 0 : MyUtility.Convert.GetInt(dr["CTNQty"]));
                ctnQty = ctnQty + drCtnQty;
                shipQty = shipQty + MyUtility.Convert.GetInt(dr["ShipQty"]);
                nw = MyUtility.Math.Round(nw + MyUtility.Convert.GetDouble(dr["NW"]), 3);
                gw = MyUtility.Math.Round(gw + MyUtility.Convert.GetDouble(dr["GW"]), 3);
                nnw = MyUtility.Math.Round(nnw + MyUtility.Convert.GetDouble(dr["NNW"]), 3);
                if (drCtnQty > 0)
                {
                    ctnCBM = MyUtility.GetValue.Lookup("CBM", dr["RefNo"].ToString(), "LocalItem", "RefNo");
                    cbm = MyUtility.Math.Round(cbm + (MyUtility.Math.Round(MyUtility.Convert.GetDouble(ctnCBM), 3) * drCtnQty), 4);
                }
                #endregion

                #region 重算表身Grid的Bal. Qty
                //目前還有多少衣服尚未裝箱
                filter = string.Format("OrderID = '{0}' and OrderShipmodeSeq = '{1}'", dr["OrderID"].ToString(), dr["OrderShipmodeSeq"].ToString());
                detailData = needPackData.Select(filter);
                if (detailData.Length <= 0)
                {
                    //撈取此SP+Seq尚未裝箱的數量
                    sqlCmd = string.Format(@"select oqd.Id as OrderID, oqd.Seq as OrderShipmodeSeq, oqd.Article, oqd.SizeCode, (oqd.Qty - isnull(sum(pld.ShipQty),0) - isnull(sum(iaq.DiffQty), 0)) as Qty
from Order_QtyShip_Detail oqd
left join PackingList_Detail pld on pld.OrderID = oqd.Id and pld.OrderShipmodeSeq = oqd.Seq and pld.ID != '{0}'
left join InvAdjust ia on ia.OrderID = oqd.ID and ia.OrderShipmodeSeq = oqd.Seq
left join InvAdjust_Qty iaq on iaq.ID = ia.ID and iaq.Article = oqd.Article and iaq.SizeCode = oqd.SizeCode
where oqd.Id = '{1}'
and oqd.Seq = '{2}'
group by oqd.Id,oqd.Seq,oqd.Article,oqd.SizeCode,oqd.Qty", CurrentMaintain["ID"].ToString(), dr["OrderID"].ToString(), dr["OrderShipmodeSeq"].ToString());
                    if (!(selectResult = DBProxy.Current.Select(null, sqlCmd, out tmpPackData)))
                    {
                        MyUtility.Msg.WarningBox("Query pack qty fail!");
                        return false;
                    }
                    else
                    {
                        foreach (DataRow tpd in tmpPackData.Rows)
                        {
                            tpd.AcceptChanges();
                            tpd.SetAdded();
                            needPackData.ImportRow(tpd);
                        }
                    }
                }

                needPackQty = 0;
                filter = string.Format("OrderID = '{0}' and OrderShipmodeSeq = '{1}' and Article = '{2}' and SizeCode = '{3}'", dr["OrderID"].ToString(), dr["OrderShipmodeSeq"].ToString(), dr["Article"].ToString(), dr["SizeCode"].ToString());
                detailData = needPackData.Select(filter);
                if (detailData.Length > 0)
                {
                    needPackQty = MyUtility.Convert.GetInt(detailData[0]["Qty"]);
                }

                //加總表身特定Article/SizeCode的Ship Qty數量
                detailData = ((DataTable)detailgridbs.DataSource).Select(filter);
                ttlShipQty = 0;
                if (detailData.Length != 0)
                {
                    foreach (DataRow dDr in detailData)
                    {
                        ttlShipQty = ttlShipQty + MyUtility.Convert.GetInt(dDr["ShipQty"]);
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
            //CTNQty, ShipQty, NW, GW, NNW, CBM
            CurrentMaintain["CTNQty"] = ctnQty;
            CurrentMaintain["ShipQty"] = shipQty;
            CurrentMaintain["NW"] = nw;
            CurrentMaintain["GW"] = gw;
            CurrentMaintain["NNW"] = nnw;
            CurrentMaintain["CBM"] = cbm;

            if (isNegativeBalQty)
            {
                MyUtility.Msg.WarningBox("Quantity entered is greater than order quantity!!");
                return false;
            }

            //表身Grid不可為空
            if (i == 0)
            {
                MyUtility.Msg.WarningBox("< Detail > can't be empty!");
                detailgrid.Focus();
                return false;
            }

            //GetID
            if (IsDetailInserting)
            {
                string id = MyUtility.GetValue.GetID(Sci.Env.User.Keyword + "PS", "PackingList", DateTime.Today, 2, "Id", null);
                if (MyUtility.Check.Empty(id))
                {
                    MyUtility.Msg.WarningBox("GetID fail, please try again!");
                    return false;
                }
                CurrentMaintain["ID"] = id;
            }

            return base.ClickSaveBefore();
        }

        protected override DualResult ClickSavePre()
        {
            if (!MyUtility.Check.Empty(CurrentMaintain["INVNo"]))
            {
                sqlCmd = string.Format(@"select isnull(sum(ShipQty),0) as ShipQty,isnull(sum(CTNQty),0) as CTNQty,isnull(sum(NW),0) as NW,isnull(sum(GW),0) as GW,isnull(sum(NNW),0) as NNW,isnull(sum(CBM),0) as CBM
from PackingList
where INVNo = '{0}'
and ID != '{1}'", CurrentMaintain["INVNo"].ToString(), CurrentMaintain["ID"].ToString());

                DataTable summaryData;
                if (result = DBProxy.Current.Select(null, sqlCmd, out summaryData))
                {
                    string updateCmd = @"update GMTBooking
set TotalShipQty = @ttlShipQty, TotalCTNQty = @ttlCTNQty, TotalNW = @ttlNW, TotalNNW = @ttlNNW, TotalGW = @ttlGW, TotalCBM = @ttlCBM
where ID = @INVNo";
                    #region 準備sql參數資料
                    System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter();
                    sp1.ParameterName = "@ttlShipQty";
                    sp1.Value = MyUtility.Convert.GetInt(summaryData.Rows[0]["ShipQty"]) + MyUtility.Convert.GetInt(CurrentMaintain["ShipQty"]);

                    System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter();
                    sp2.ParameterName = "@ttlCTNQty";
                    sp2.Value = MyUtility.Convert.GetInt(summaryData.Rows[0]["CTNQty"]) + MyUtility.Convert.GetInt(CurrentMaintain["CTNQty"]);

                    System.Data.SqlClient.SqlParameter sp3 = new System.Data.SqlClient.SqlParameter();
                    sp3.ParameterName = "@ttlNW";
                    sp3.Value = MyUtility.Math.Round(MyUtility.Convert.GetDouble(summaryData.Rows[0]["NW"]) + MyUtility.Convert.GetDouble(CurrentMaintain["NW"].ToString()), 2);

                    System.Data.SqlClient.SqlParameter sp4 = new System.Data.SqlClient.SqlParameter();
                    sp4.ParameterName = "@ttlNNW";
                    sp4.Value = MyUtility.Math.Round(MyUtility.Convert.GetDouble(summaryData.Rows[0]["NNW"]) + MyUtility.Convert.GetDouble(CurrentMaintain["NNW"].ToString()), 2);

                    System.Data.SqlClient.SqlParameter sp5 = new System.Data.SqlClient.SqlParameter();
                    sp5.ParameterName = "@ttlGW";
                    sp5.Value = MyUtility.Math.Round(MyUtility.Convert.GetDouble(summaryData.Rows[0]["GW"]) + MyUtility.Convert.GetDouble(CurrentMaintain["GW"].ToString()), 2);

                    System.Data.SqlClient.SqlParameter sp6 = new System.Data.SqlClient.SqlParameter();
                    sp6.ParameterName = "@ttlCBM";
                    sp6.Value = MyUtility.Math.Round(MyUtility.Convert.GetDouble(summaryData.Rows[0]["CBM"]) + MyUtility.Convert.GetDouble(CurrentMaintain["CBM"].ToString()), 2);

                    System.Data.SqlClient.SqlParameter sp7 = new System.Data.SqlClient.SqlParameter();
                    sp7.ParameterName = "@INVNo";
                    sp7.Value = CurrentMaintain["INVNo"].ToString();


                    IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                    cmds.Add(sp1);
                    cmds.Add(sp2);
                    cmds.Add(sp3);
                    cmds.Add(sp4);
                    cmds.Add(sp5);
                    cmds.Add(sp6);
                    cmds.Add(sp7);
                    #endregion

                    result = Sci.Data.DBProxy.Current.Execute(null, updateCmd, cmds);
                    if (!result)
                    {
                        DualResult failResult = new DualResult(false, "Update Garment Booking fail!\r\n" + result.ToString());
                        return failResult;
                    }
                }
                else
                {
                    DualResult failResult = new DualResult(false, "Select PackingList fail!\r\n" + result.ToString());
                    return failResult;
                }
            }
            return Result.True;
        }

        protected override DualResult ClickSavePost()
        {
            //存檔成功後，要再呼叫CreateOrderCTNData
            if (!Prgs.CreateOrderCTNData(CurrentMaintain["ID"].ToString()))
            {
                DualResult failResult = new DualResult(false, "Create Order_CTN fail!");
                return failResult;
            }
            return Result.True;
        }

        protected override bool ClickDeleteBefore()
        {
            if (CurrentMaintain["Status"].ToString() == "Confirmed")
            {
                MyUtility.Msg.WarningBox("This record is < Confirmed >, can't be deleted!");
                return false;
            }

            if (!MyUtility.Check.Empty(CurrentMaintain["INVNo"]))
            {
                MyUtility.Msg.WarningBox("This record had booking no. Can't be deleted!");
                return false;
            }
            return base.ClickDeleteBefore();
        }

        protected override bool ClickPrint()
        {
            Sci.Production.Packing.P04_Print callNextForm = new Sci.Production.Packing.P04_Print(CurrentMaintain);
            callNextForm.ShowDialog(this);
            return base.ClickPrint();
        }

        //控制表身Grid欄位是否可被編輯
        private void DetailGridEditing(bool isEditing)
        {
            if (isEditing)
            {
                col_orderid.IsEditingReadOnly = false;
                col_1stctnno.IsEditingReadOnly = false;
                col_lastctnno.IsEditingReadOnly = false;
                col_ctnqty.IsEditingReadOnly = false;
                col_refno.IsEditingReadOnly = false;
                col_article.IsEditingReadOnly = false;
                col_size.IsEditingReadOnly = false;
                col_qtyperctn.IsEditingReadOnly = false;
                col_shipqty.IsEditingReadOnly = false;
                col_nw.IsEditingReadOnly = false;
                col_gw.IsEditingReadOnly = false;
                col_nnw.IsEditingReadOnly = false;

                for (int i = 0; i < detailgrid.ColumnCount; i++)
                {
                    if (i == 0 || i == 1 || i == 4 || i == 5 || i == 6 || i == 7 || i == 9 || i == 11 || i == 12 || i == 13 || i == 15 || i == 16 || i == 17 || i == 18)
                    {
                        detailgrid.Columns[i].DefaultCellStyle.ForeColor = Color.Red;
                    }
                }
                //先-再+預防重複出現多次視窗
                col_refno.EditingMouseDown -= new EventHandler<Ict.Win.UI.DataGridViewEditingControlMouseEventArgs>(CartonRefnoCommon.EditingMouseDown);
                col_refno.EditingMouseDown += new EventHandler<Ict.Win.UI.DataGridViewEditingControlMouseEventArgs>(CartonRefnoCommon.EditingMouseDown);
            }
            else
            {
                col_orderid.IsEditingReadOnly = true;
                col_1stctnno.IsEditingReadOnly = true;
                col_lastctnno.IsEditingReadOnly = true;
                col_ctnqty.IsEditingReadOnly = true;
                col_refno.IsEditingReadOnly = true;
                col_article.IsEditingReadOnly = true;
                col_size.IsEditingReadOnly = true;
                col_qtyperctn.IsEditingReadOnly = true;
                col_shipqty.IsEditingReadOnly = true;
                col_nw.IsEditingReadOnly = true;
                col_gw.IsEditingReadOnly = true;
                col_nnw.IsEditingReadOnly = true;
                for (int i = 0; i < detailgrid.ColumnCount; i++)
                {
                    if (i != 18)
                    {
                        detailgrid.Columns[i].DefaultCellStyle.ForeColor = Color.Black;
                    }
                }
                col_refno.EditingMouseDown -= new EventHandler<Ict.Win.UI.DataGridViewEditingControlMouseEventArgs>(CartonRefnoCommon.EditingMouseDown);
            }
        }

        //Carton Summary
        private void button1_Click(object sender, EventArgs e)
        {
            Sci.Production.Packing.P04_CartonSummary callNextForm = new Sci.Production.Packing.P04_CartonSummary(CurrentMaintain["ID"].ToString());
            callNextForm.ShowDialog(this);
        }

        //Batch Import
        private void button2_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(CurrentMaintain["BrandID"]))
            {
                MyUtility.Msg.WarningBox("Brand can't be empty!");
                return;
            }
            Sci.Production.Packing.P04_BatchImport callNextForm = new Sci.Production.Packing.P04_BatchImport(CurrentMaintain, (DataTable)detailgridbs.DataSource);
            callNextForm.ShowDialog(this);
        }

        //Recalculate Weight
        private void button5_Click(object sender, EventArgs e)
        {
            //如果已經Shipping Lock的話就不可以再重算重量
            if (!MyUtility.Check.Empty(CurrentMaintain["GMTBookingLock"]))
            {
                MyUtility.Msg.WarningBox("This record already shipping lock, can't recalculate weight!");
                return;
            }

            Prgs.RecaluateCartonWeight((DataTable)detailgridbs.DataSource, CurrentMaintain);
        }

        //Confirm
        protected override void ClickConfirm()
        {
            base.ClickConfirm();

            //還沒有Invoice No就不可以做Confirm
            if (MyUtility.Check.Empty(MyUtility.GetValue.Lookup("INVNo", CurrentMaintain["ID"].ToString(), "PackingList", "ID")))
            {
                MyUtility.Msg.WarningBox("Shipping is not yet booking so can't confirm!");
                return;
            }

            //檢查累計Pullout數不可超過訂單數量
            if (!Prgs.CheckPulloutQtyWithOrderQty(CurrentMaintain["ID"].ToString()))
            {
                return;
            }

            //檢查Sewing Output Qty是否有超過Packing Qty
            if (!Prgs.CheckPackingQtyWithSewingOutput(CurrentMaintain["ID"].ToString()))
            {
                return;
            }

            sqlCmd = string.Format("update PackingList set Status = 'Confirmed', EditName = '{0}', EditDate = GETDATE() where ID = '{1}'", Sci.Env.User.UserID, CurrentMaintain["ID"].ToString());
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
            if (!MyUtility.Check.Empty(CurrentMaintain["INVNo"]))
            {
                if (MyUtility.GetValue.Lookup("Status", CurrentMaintain["INVNo"].ToString(), "GMTBooking", "ID") == "Confirmed")
                {
                    MyUtility.Msg.WarningBox("Garment booking already confirmed, so can't unconfirm! ");
                    return;
                }
            }

            //問是否要做Unconfirm，確定才繼續往下做
            buttonResult = MyUtility.Msg.WarningBox("Are you sure you want to < Unconfirm > this data?", "Warning", MessageBoxButtons.YesNo);
            if (buttonResult == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            sqlCmd = string.Format("update PackingList set Status = 'New', EditName = '{0}', EditDate = GETDATE() where ID = '{1}'", Sci.Env.User.UserID, CurrentMaintain["ID"].ToString());

            result = DBProxy.Current.Execute(null, sqlCmd);
            if (!result)
            {
                MyUtility.Msg.WarningBox("UnConfirm failed, Pleaes re-try");
            }

            RenewData();
            OnDetailEntered();
            EnsureToolbarExt();
        }

        //Download excel format
        private void button4_Click(object sender, EventArgs e)
        {
            string strXltName = Sci.Env.Cfg.XltPathDir + "\\Packing_P04_ImportExcelFormat.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null) return;
            excel.Visible = true;
        }

        //Import from excel
        private void button3_Click(object sender, EventArgs e)
        {
            Sci.Production.Packing.P04_ExcelImport callNextForm = new Sci.Production.Packing.P04_ExcelImport((DataTable)detailgridbs.DataSource);
            callNextForm.ShowDialog(this);
        }
    }
}
