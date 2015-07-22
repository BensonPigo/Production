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
        private DataRow[] dra;
        private string sqlCmd = "", filter = "", masterID;

        public P04(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.DefaultFilter = "FactoryID = '" + Sci.Env.User.Factory + "' AND Type = 'S'";
            detailgrid.AllowUserToOrderColumns = true;
        }

        protected override Ict.DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            masterID = (e.Master == null) ? "" : e.Master["ID"].ToString();
            this.DetailSelectCommand = string.Format(@"select a.ID,a.OrderID,a.OrderShipmodeSeq,o.StyleID,o.SeasonID,o.CustPONo,a.CTNStartNo,a.CTNEndNo,a.CTNQty, a.RefNo,b.Description,a.Article, a.Color, a.SizeCode, a.QtyPerCTN, a.ShipQty, a.NW, a.GW, a.NNW, a.NWPerPcs,isnull(oqd.Qty,0) as BalanceQty, a.Seq
from PackingList_Detail a
left join LocalItem b on b.RefNo = a.RefNo
left join Orders o on o.ID = a.OrderID
left join Order_QtyShip_Detail oqd on oqd.Id = a.OrderID and oqd.Seq = a.OrderShipmodeSeq and oqd.Article = a.Article and oqd.SizeCode = a.SizeCode
where a.id = '{0}'
order by a.Seq", masterID);

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
                        DataRow orderData;
                        if (!MyUtility.Check.Seek(string.Format("Select ID,StyleID,CustPONo from Orders where ID = '{0}' and Category = 'S' and BrandID = '{1}'", e.FormattedValue.ToString(), CurrentMaintain["BrandID"].ToString()), out orderData))
                        {
                            MessageBox.Show(string.Format("< SP No.: {0} > not found!!!", e.FormattedValue.ToString()));
                            dr["OrderID"] = "";
                            dr["OrderShipmodeSeq"] = "";
                            dr["Article"] = "";
                            dr["Color"] = "";
                            dr["SizeCode"] = "";
                            dr["StyleID"] = "";
                            dr["CustPONo"] = "";
                            dr.EndEdit();
                            e.Cancel = true;
                            return;
                        }
                        else
                        {
                            dr["OrderID"] = e.FormattedValue.ToString().ToUpper();
                            dr["StyleID"] = orderData["StyleID"].ToString();
                            dr["CustPONo"] = orderData["CustPONo"].ToString();
                            dr["Article"] = "";
                            dr["Color"] = "";
                            dr["SizeCode"] = "";
                            #region 若Order_QtyShip有多筆資料話就跳出視窗讓使者選擇Seq
                            sqlCmd = string.Format("select count(ID) as CountID from Order_QtyShip where ID = '{0}'", dr["OrderID"].ToString());
                            if (MyUtility.Check.Seek(sqlCmd, out orderData))
                            {
                                if (orderData["CountID"].ToString() == "1")
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
                                e.EditingControl.Text = "";
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
                                                                        from V_OrderFAColor 
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
        }

        protected override DualResult OnRenewDataDetailPost(RenewDataPostEventArgs e)
        {
            DataTable packData;
            #region 先撈出要加工的所有data
            masterID = e.Master["ID"] == null ? "" : e.Master["ID"].ToString();
            sqlCmd = string.Format(@"with PackData
as
(select distinct OrderID,OrderShipmodeSeq 
 from PackingList_Detail 
 where id = '{0}'
),
PackedData
as
(
select pld.OrderID, pld.OrderShipmodeSeq, pld.Article, pld.SizeCode, isnull(sum(pld.ShipQty),0) as ShipQty
from PackingList_Detail pld, PackData pd
where pld.OrderID = pd.OrderID
and pld.OrderShipmodeSeq = pd.OrderShipmodeSeq
group by pld.OrderID, pld.OrderShipmodeSeq, pld.Article, pld.SizeCode
),
InvAdjData
as
(
select ia.OrderID,ia.OrderShipmodeSeq,iaq.Article, iaq.SizeCode, isnull(sum(iaq.DiffQty),0) as TtlDiffQty
from InvAdjust ia, InvAdjust_Qty iaq, PackData pd
where ia.OrderID = pd.OrderID
and ia.OrderShipmodeSeq = pd.OrderShipmodeSeq
and ia.ID = iaq.ID
group by ia.OrderID,ia.OrderShipmodeSeq,iaq.Article, iaq.SizeCode
),
OrderQty
as
(
select pd.OrderID,pd.OrderShipmodeSeq,oqd.Article,oqd.SizeCode,oqd.Qty
from Order_QtyShip_Detail oqd, PackData pd
where oqd.Id = pd.OrderID
and oqd.Seq = pd.OrderShipmodeSeq
)
select oq.OrderID,oq.OrderShipmodeSeq,oq.Article,oq.SizeCode,oq.Qty-isnull(pd.ShipQty,0)+isnull(iad.TtlDiffQty,0) as BalanceQty
from OrderQty oq
left join PackedData pd on pd.OrderID = oq.OrderID and pd.OrderShipmodeSeq = oq.OrderShipmodeSeq and pd.Article = oq.Article and pd.SizeCode = oq.SizeCode
left join InvAdjData iad on iad.OrderID = oq.OrderID and iad.OrderShipmodeSeq = oq.OrderShipmodeSeq and iad.Article = oq.Article and iad.SizeCode = oq.SizeCode", masterID);
            result = DBProxy.Current.Select(null, sqlCmd, out packData);
            #endregion
            foreach (DataRow drw in e.Details.Rows)
            {
                dra = packData.Select(string.Format("OrderID = '{0}' and OrderShipmodeSeq = '{1}' and Article = '{2}' and SizeCode = '{3}'", drw["OrderID"].ToString(), drw["OrderShipmodeSeq"].ToString(), drw["Article"].ToString(), drw["SizeCode"].ToString()));
                if (dra.Length > 0)
                {
                    drw["BalanceQty"] = Convert.ToInt32(dra[0]["BalanceQty"]);
                }
            }

            return base.OnRenewDataDetailPost(e);
        }

        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            txtshipmode1.ReadOnly = false;
            CurrentMaintain["FactoryID"] = Sci.Env.User.Factory;
            CurrentMaintain["Type"] = "S";
            CurrentMaintain["Status"] = "New";
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
                drCtnQty = (MyUtility.Check.Empty(dr["CTNQty"]) ? 0 : Convert.ToInt32(dr["CTNQty"].ToString()));
                ctnQty = ctnQty + drCtnQty;
                shipQty = shipQty + Convert.ToInt32(dr["ShipQty"].ToString());
                nw = MyUtility.Math.Round(nw + (MyUtility.Check.Empty(dr["NW"]) ? 0 : Convert.ToDouble(dr["NW"].ToString())), 3);
                gw = MyUtility.Math.Round(gw + (MyUtility.Check.Empty(dr["GW"]) ? 0 : Convert.ToDouble(dr["GW"].ToString())), 3);
                nnw = MyUtility.Math.Round(nnw + (MyUtility.Check.Empty(dr["NNW"]) ? 0 : Convert.ToDouble(dr["NNW"].ToString())), 3);
                if (drCtnQty > 0)
                {
                    ctnCBM = MyUtility.GetValue.Lookup("CBM", dr["RefNo"].ToString(), "LocalItem", "RefNo");
                    cbm = MyUtility.Math.Round(cbm + (MyUtility.Math.Round(Convert.ToDouble(MyUtility.Check.Empty(ctnCBM) ? "0" : ctnCBM), 3) * drCtnQty), 4);
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
                string id = MyUtility.GetValue.GetID(ProductionEnv.Keyword + "PS", "PackingList", DateTime.Today, 2, "Id", null);
                if (MyUtility.Check.Empty(id))
                {
                    MyUtility.Msg.WarningBox("GetID fail, please try again!");
                    return false;
                }
                CurrentMaintain["ID"] = id;
            }

            return base.ClickSaveBefore();
        }

        protected override bool ClickSavePre()
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
                    sp1.Value = Convert.ToInt32(summaryData.Rows[0]["ShipQty"]) + Convert.ToInt32(CurrentMaintain["ShipQty"].ToString());

                    System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter();
                    sp2.ParameterName = "@ttlCTNQty";
                    sp2.Value = Convert.ToInt32(summaryData.Rows[0]["CTNQty"]) + Convert.ToInt32(CurrentMaintain["CTNQty"].ToString());

                    System.Data.SqlClient.SqlParameter sp3 = new System.Data.SqlClient.SqlParameter();
                    sp3.ParameterName = "@ttlNW";
                    sp3.Value = MyUtility.Math.Round(Convert.ToDouble(summaryData.Rows[0]["NW"]) + Convert.ToDouble(CurrentMaintain["NW"].ToString()), 2);

                    System.Data.SqlClient.SqlParameter sp4 = new System.Data.SqlClient.SqlParameter();
                    sp4.ParameterName = "@ttlNNW";
                    sp4.Value = MyUtility.Math.Round(Convert.ToDouble(summaryData.Rows[0]["NNW"]) + Convert.ToDouble(CurrentMaintain["NNW"].ToString()), 2);

                    System.Data.SqlClient.SqlParameter sp5 = new System.Data.SqlClient.SqlParameter();
                    sp5.ParameterName = "@ttlGW";
                    sp5.Value = MyUtility.Math.Round(Convert.ToDouble(summaryData.Rows[0]["GW"]) + Convert.ToDouble(CurrentMaintain["GW"].ToString()), 2);

                    System.Data.SqlClient.SqlParameter sp6 = new System.Data.SqlClient.SqlParameter();
                    sp6.ParameterName = "@ttlCBM";
                    sp6.Value = MyUtility.Math.Round(Convert.ToDouble(summaryData.Rows[0]["CBM"]) + Convert.ToDouble(CurrentMaintain["CBM"].ToString()), 2);

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

                    Sci.Data.DBProxy.Current.Execute(null, updateCmd, cmds);
                }
                else
                {
                    MyUtility.Msg.WarningBox("Update Garment Booking fail!");
                    return false;
                }
            }
            return base.ClickSavePre();
        }

        protected override void ClickSaveAfter()
        {
            base.ClickSaveAfter();
            //存檔成功後，要再呼叫CreateOrderCTNData
            bool prgResult = Prgs.CreateOrderCTNData(CurrentMaintain["ID"].ToString());
            txtshipmode1.ReadOnly = true;
        }

        protected override void ClickUndo()
        {
            base.ClickUndo();
            txtshipmode1.ReadOnly = true;
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
            DataTable weightData, tmpWeightData;
            DataRow[] weight;
            string message = "";

            if (!(result = DBProxy.Current.Select(null, "select '' as BrandID, '' as StyleID, '' as SeasonID, Article, SizeCode, NW, NNW from Style_WeightData where StyleUkey = ''", out weightData)))
            {
                MyUtility.Msg.WarningBox("Query 'weightData' schema fail!");
                return;
            }

            //檢查是否所有的SizeCode都有存在Style_WeightData中
            foreach (DataRow dr in ((DataTable)detailgridbs.DataSource).Rows)
            {
                filter = string.Format("BrandID = '{0}' and StyleID = '{1}' and SeasonID = '{2}'", CurrentMaintain["BrandID"].ToString(), dr["StyleID"].ToString(), dr["SeasonID"].ToString());
                weight = weightData.Select(filter);
                if (weight.Length == 0)
                {
                    //先將屬於此訂單的Style_WeightData給撈出來
                    sqlCmd = string.Format(@"select a.ID as StyleID,a.BrandID,a.SeasonID,isnull(b.Article,'') as Article,isnull(b.SizeCode,'') as SizeCode,isnull(b.NW,0) as NW,isnull(b.NNW,0) as NNW
from Style a
left join Style_WeightData b on b.StyleUkey = a.Ukey
where a.ID = '{0}' and a.BrandID = '{1}' and a.SeasonID = '{2}'", dr["StyleID"].ToString(), CurrentMaintain["BrandID"].ToString(), dr["SeasonID"].ToString());
                    if (!(result = DBProxy.Current.Select(null, sqlCmd, out tmpWeightData)))
                    {
                        MyUtility.Msg.WarningBox("Query weight data fail!");
                        return;
                    }
                    else
                    {
                        foreach (DataRow tpd in tmpWeightData.Rows)
                        {
                            tpd.AcceptChanges();
                            tpd.SetAdded();
                            weightData.ImportRow(tpd);
                        }
                    }
                }

                filter = string.Format("BrandID = '{0}' and StyleID = '{1}' and SeasonID = '{2}' and SizeCode = '{3}'", CurrentMaintain["BrandID"].ToString(), dr["StyleID"].ToString(), dr["SeasonID"].ToString(), dr["SizeCode"].ToString());
                weight = weightData.Select(filter);
                if (weight.Length == 0)
                {
                    if (message.IndexOf("SP#:" + dr["OrderID"].ToString() + ", Size:" + dr["SizeCode"].ToString()) <= 0)
                    {
                        message = message + "SP#:" + dr["OrderID"].ToString() + ", Size:" + dr["SizeCode"].ToString() + "\r\n";
                    }
                }
            }

            if (!MyUtility.Check.Empty(message))
            {
                buttonResult = MyUtility.Msg.WarningBox(message + " not in Style basic data, are you sure you want to  recalculate weight?", "Warning", MessageBoxButtons.YesNo);
                if (buttonResult == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }
            }

            double nw = 0, nnw = 0, ctnWeight = 0;
            string localItemWeight;
            DataTable tmpPacklistWeight;
            result = DBProxy.Current.Select(null, "select CTNStartNo, NW, NNW, GW from PackingList_Detail where 1=0", out tmpPacklistWeight);
            DataRow tmpPacklistRow;
            string ctnNo = ((DataTable)detailgridbs.DataSource).Rows[0]["CTNStartNo"].ToString();

            foreach (DataRow dr in ((DataTable)detailgridbs.DataSource).Rows)
            {
                if (!MyUtility.Check.Empty(dr["CTNQty"]))
                {
                    if (ctnNo != dr["CTNStartNo"].ToString())
                    {
                        tmpPacklistRow = tmpPacklistWeight.NewRow();
                        tmpPacklistRow["CTNStartNo"] = ctnNo;
                        tmpPacklistRow["NW"] = nw;
                        tmpPacklistRow["NNW"] = nnw;
                        tmpPacklistRow["GW"] = nw + ctnWeight;
                        tmpPacklistWeight.Rows.Add(tmpPacklistRow);
                    }
                    ctnNo = dr["CTNStartNo"].ToString();
                    nw = 0;
                    nnw = 0;
                    localItemWeight = MyUtility.GetValue.Lookup("Weight", MyUtility.Check.Empty(dr["RefNo"]) ? "" : dr["RefNo"].ToString(), "LocalItem", "RefNo");
                    ctnWeight = MyUtility.Math.Round(Convert.ToDouble(MyUtility.Check.Empty(localItemWeight) ? "0" : localItemWeight), 6);
                }

                filter = string.Format("BrandID = '{0}' and StyleID = '{1}' and SeasonID = '{2}' and Article = '{3}' and SizeCode = '{4}'", CurrentMaintain["BrandID"].ToString(), dr["StyleID"].ToString(), dr["SeasonID"].ToString(), dr["Article"].ToString(), dr["SizeCode"].ToString());
                weight = weightData.Select(filter);
                if (weight.Length > 0)
                {
                    nw = nw + Convert.ToDouble(weight[0]["NW"]) * Convert.ToInt32(dr["ShipQty"]);
                    nnw = nnw + Convert.ToDouble(weight[0]["NNW"]) * Convert.ToInt32(dr["ShipQty"]);
                    dr["NWPerPcs"] = Convert.ToDouble(weight[0]["NW"]);
                }
                else
                {
                    filter = string.Format("BrandID = '{0}' and StyleID = '{1}' and SeasonID = '{2}' and SizeCode = '{3}'", CurrentMaintain["BrandID"].ToString(), dr["StyleID"].ToString(), dr["SeasonID"].ToString(), dr["SizeCode"].ToString());
                    weight = weightData.Select(filter);
                    if (weight.Length > 0)
                    {
                        nw = nw + Convert.ToDouble(weight[0]["NW"]) * Convert.ToInt32(dr["ShipQty"]);
                        nnw = nnw + Convert.ToDouble(weight[0]["NNW"]) * Convert.ToInt32(dr["ShipQty"]);
                        dr["NWPerPcs"] = Convert.ToDouble(weight[0]["NW"]);
                    }
                    else
                    {
                        dr["NWPerPcs"] = 0;
                    }
                }
            }
            //最後一筆資料也要寫入
            tmpPacklistRow = tmpPacklistWeight.NewRow();
            tmpPacklistRow["CTNStartNo"] = ctnNo;
            tmpPacklistRow["NW"] = nw;
            tmpPacklistRow["NNW"] = nnw;
            tmpPacklistRow["GW"] = nw + ctnWeight;
            tmpPacklistWeight.Rows.Add(tmpPacklistRow);

            //將整箱重量回寫回表身Grid中CTNQty> 0的資料中
            foreach (DataRow dr in tmpPacklistWeight.Rows)
            {
                foreach (DataRow dr1 in ((DataTable)detailgridbs.DataSource).Rows)
                {
                    if (dr["CTNStartNo"].ToString() == dr1["CTNStartNo"].ToString())
                    {
                        if (!MyUtility.Check.Empty(dr1["CTNQty"]))
                        {
                            dr1["NW"] = dr["NW"];
                            dr1["NNW"] = dr["NNW"];
                            dr1["GW"] = dr["GW"];
                        }
                        else
                        {
                            dr1["NW"] = 0;
                            dr1["NNW"] = 0;
                            dr1["GW"] = 0;
                        }
                    }
                }
            }
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

            string errMesg = "";
            DualResult queryResult;
            DataTable queryData;
            //檢查累計Pullout數不可超過訂單數量
            sqlCmd = string.Format(@"select OrderID,OrderShipmodeSeq,Article,SizeCode,sum(ShipQty) as ShipQty 
from PackingList_Detail
where ID = '{0}'
group by OrderID,OrderShipmodeSeq,Article,SizeCode", CurrentMaintain["ID"].ToString());

            queryResult = DBProxy.Current.Select(null, sqlCmd, out queryData);
            if (queryResult)
            {
                foreach (DataRow dr in queryData.Rows)
                {
                    if (!Prgs.CheckPulloutComplete(dr["OrderID"].ToString(), dr["OrderShipmodeSeq"].ToString(), dr["Article"].ToString(), dr["SizeCode"].ToString(), Convert.ToInt32(dr["ShipQty"])))
                    {
                        errMesg = errMesg + "SP No.:" + dr["OrderID"].ToString() + "Color Way: " + dr["Article"].ToString() + ", Size: " + dr["SizeCode"].ToString() + "\r\n";
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
            sqlCmd = string.Format(@"with PackOrderID
as
(select distinct pld.OrderID
 from PackingList pl, PackingList_Detail pld
 where pl.ID = '{0}'
 and pld.ID = pl.ID
),
PackedDate
as
(select pld.OrderID,pld.Article,pld.SizeCode,sum(pld.ShipQty) as PackedShipQty
 from PackingList pl, PackingList_Detail pld, PackOrderID poid
 where pld.OrderID = poid.OrderID
 and pl.ID = pld.ID
 and pl.Status = 'Confirmed'
 group by pld.OrderID,pld.Article,pld.SizeCode
),
PackingData
as
(select pld.OrderID,pld.Article,pld.SizeCode,sum(pld.ShipQty) as ShipQty
 from PackingList_Detail pld
 where pld.ID = '{0}'
 group by pld.OrderID,pld.Article,pld.SizeCode
),
InvadjQty
as
(select ia.OrderID,iaq.Article, iaq.SizeCode,sum(iaq.DiffQty) as DiffQty
 from InvAdjust ia, InvAdjust_Qty iaq, PackOrderID poid
 where ia.OrderID = poid.OrderID
 and ia.ID = iaq.ID
 group by ia.OrderID,iaq.Article, iaq.SizeCode
),
SewingData
as
(select a.OrderID,a.Article,a.SizeCode,MIN(a.QAQty) as QAQty
 from (select poid.OrderID,oq.Article,oq.SizeCode, sl.Location, isnull(sum(sodd.QAQty),0) as QAQty
	   from PackOrderID poid
	   left join Orders o on o.ID = poid.OrderID
	   left join Order_Qty oq on oq.ID = o.ID
	   left join Style_Location sl on sl.StyleUkey = o.StyleUkey
	   left join SewingOutput_Detail_Detail sodd on sodd.OrderId = o.ID and sodd.Article = oq.Article  and sodd.SizeCode = oq.SizeCode and sodd.ComboType = sl.Location
	   group by poid.OrderID,oq.Article,oq.SizeCode, sl.Location) a
 group by a.OrderID,a.Article,a.SizeCode
)
select poid.OrderID,isnull(oq.Article,'') as Article,isnull(oq.SizeCode,'') as SizeCode, isnull(oq.Qty,0) as Qty, isnull(pedd.PackedShipQty,0)+isnull(pingd.ShipQty,0) as PackQty,isnull(iq.DiffQty,0) as DiffQty, isnull(sd.QAQty,0) as QAQty
from PackOrderID poid
left join Order_Qty oq on oq.ID = poid.OrderID
left join PackedDate pedd on pedd.Article = oq.Article and pedd.SizeCode = oq.SizeCode
left join PackingData pingd on pingd.Article = oq.Article and pingd.SizeCode = oq.SizeCode
left join InvadjQty iq on iq.Article = oq.Article and iq.SizeCode = oq.SizeCode
left join SewingData sd on sd.Article = oq.Article and sd.SizeCode = oq.SizeCode
order by poid.OrderID,oq.Article,oq.SizeCode", CurrentMaintain["ID"].ToString());

            queryResult = DBProxy.Current.Select(null, sqlCmd, out queryData);
            if (queryResult)
            {
                foreach (DataRow dr in queryData.Rows)
                {
                    if (!MyUtility.Check.Empty(dr["PackQty"]))
                    {
                        if (Convert.ToInt32(dr["PackQty"]) + Convert.ToInt32(dr["DiffQty"]) > Convert.ToInt32(dr["QAQty"]))
                        {
                            errMesg = errMesg + "SP No.:" + dr["OrderID"].ToString() + "Color Way: " + dr["Article"].ToString() + ", Size: " + dr["SizeCode"].ToString() + ", Qty: " + dr["Qty"].ToString() + ", Ship Qty: " + dr["PackQty"].ToString() + ", Sewing Qty:" + dr["QAQty"].ToString() + ((MyUtility.Check.Empty(dr["DiffQty"]) ? "" : ", Adj Qty:" + dr["DiffQty"].ToString())) + "." + (Convert.ToInt32(dr["PackQty"]) + Convert.ToInt32(dr["DiffQty"]) > Convert.ToInt32(dr["Qty"]) ? "   Pullout qty can't exceed order qty," : "") + " Pullout qty can't exceed sewing qty.\r\n";
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
