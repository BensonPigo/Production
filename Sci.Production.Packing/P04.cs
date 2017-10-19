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
        Ict.Win.DataGridViewGeneratorNumericColumnSettings shipQtySetting = new DataGridViewGeneratorNumericColumnSettings();
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
            displayPurchaseCTN.Value = MyUtility.Check.Empty(CurrentMaintain["LocalPOID"]) ? "" : "Y";

            labelConfirmed.Visible = MyUtility.Check.Empty(CurrentMaintain["ID"]) ? false : true;
            DataRow dr;
            string sqlStatus = string.Format(@"select * from PackingList WITH (NOLOCK) where id='{0}'", CurrentMaintain["id"].ToString());
            if (MyUtility.Check.Seek(sqlStatus, out dr))
            {
                if (dr["Status"].ToString().ToUpper() == "NEW" && MyUtility.Check.Empty(CurrentMaintain["GMTBookingLock"])) labelConfirmed.Text = "New";
                else if (dr["Status"].ToString().ToUpper() == "CONFIRMED" && !MyUtility.Check.Empty(CurrentMaintain["GMTBookingLock"])) labelConfirmed.Text = "Confirmed";
                else if (dr["Status"].ToString().ToUpper() == "CONFIRMED" && MyUtility.Check.Empty(CurrentMaintain["GMTBookingLock"])) labelConfirmed.Text = "Confirmed";
                else labelConfirmed.Text = "Shipping Lock";
            }

            DataTable dt = ((DataTable)detailgridbs.DataSource);
            if (!dt.Columns.Contains("OrderQty"))
                dt.ColumnsIntAdd("OrderQty");
            if (!dt.Columns.Contains("OtherConfirmQty"))
                dt.ColumnsIntAdd("OtherConfirmQty");
            if (!dt.Columns.Contains("InvAdjustQty"))
                dt.ColumnsIntAdd("InvAdjustQty");
            ComputeOrderQty();
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

                        string sqlCmd = @"
Select o.ID
	   , o.SeasonID
	   , o.StyleID
	   , o.CustPONo 
from Orders o WITH (NOLOCK) 
inner join Factory f on o.FactoryID = f.ID
where o.ID = @orderid 
	  and o.Category = 'S' 
	  and o.BrandID = @brandid
	  and f.IsProduceFty = 1";

                        DataTable orderData;
                        DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out orderData);
                        if (!result)
                        {
                            ClearGridRowData(dr);
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox("Sql connection fail!!\r\n" + result.ToString());
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
                                dr["SeasonID"] = orderData.Rows[0]["SeasonID"].ToString();
                                dr["Article"] = "";
                                dr["Color"] = "";
                                dr["SizeCode"] = "";
                                dr["OtherConfirmQty"] = 0;
                                dr["InvAdjustQty"] = 0;
                                dr["BalanceQty"] = 0;
                                dr["OrderQty"] = 0;
                                dr["shipQty"] = 0;
                                #region 若Order_QtyShip有多筆資料話就跳出視窗讓使者選擇Seq
                                DataRow orderQtyData;
                                sqlCmd = string.Format("select count(ID) as CountID from Order_QtyShip WITH (NOLOCK) where ID = '{0}'", dr["OrderID"].ToString());
                                if (MyUtility.Check.Seek(sqlCmd, out orderQtyData))
                                {
                                    if (orderQtyData["CountID"].ToString() == "1")
                                    {
                                        dr["OrderShipmodeSeq"] = MyUtility.GetValue.Lookup("Seq", dr["OrderID"].ToString(), "Order_QtyShip", "ID");
                                    }
                                    else
                                    {
                                        sqlCmd = string.Format("select Seq,BuyerDelivery,ShipmodeID,Qty from Order_QtyShip WITH (NOLOCK) where ID = '{0}'", dr["OrderID"].ToString());
                                        Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlCmd, "4,20,20,10", "", "Seq,Buyer Delivery,ShipMode,Qty");
                                        DialogResult returnResult = item.ShowDialog();
                                        if (returnResult == DialogResult.Cancel)
                                        {                                            
                                            dr["OrderShipmodeSeq"] = "";
                                        }
                                        else
                                        {                                           
                                            dr["OrderShipmodeSeq"] = item.GetSelectedString();
                                        }
                                    }
                                }
                                #endregion
                                dr.EndEdit();
                            }
                        }
                    }else if (MyUtility.Check.Empty(e.FormattedValue))
                    {
                        ClearGridRowData(dr);
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
                            sqlCmd = string.Format("select Seq, BuyerDelivery,ShipmodeID,Qty from Order_QtyShip WITH (NOLOCK) where ID = '{0}'", dr["OrderID"].ToString());
                            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlCmd, "4,20,20,10", "", "Seq,Buyer Delivery,ShipMode,Qty");
                            DialogResult returnResult = item.ShowDialog();
                            if (returnResult == DialogResult.Cancel)
                            {
                                return;
                            }
                            else
                            {
                                dr["OrderShipmodeSeq"] = item.GetSelectedString();
                            }
                            dr["Article"] = "";
                            dr["Color"] = "";
                            dr["SizeCode"] = "";
                            dr["OtherConfirmQty"] = 0;
                            dr["InvAdjustQty"] = 0;
                            dr["BalanceQty"] = 0;
                            dr["OrderQty"] = 0;
                            dr["shipQty"] = 0;
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
                            sqlCmd = string.Format("Select Distinct Article from Order_QtyShip_Detail WITH (NOLOCK) where ID = '{0}' and Seq = '{1}'", dr["OrderID"].ToString(), dr["OrderShipmodeSeq"].ToString());
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
                        dr["Color"] = "";
                        dr["SizeCode"] = "";
                        dr["OtherConfirmQty"] = 0;
                        dr["InvAdjustQty"] = 0;
                        dr["BalanceQty"] = 0;
                        dr["OrderQty"] = 0;
                        dr["shipQty"] = 0;
                        if (!MyUtility.Check.Seek(string.Format("Select Article from Order_QtyShip_Detail WITH (NOLOCK) where ID = '{0}' and Seq = '{1}' and Article = '{2}'", dr["OrderID"].ToString(), dr["OrderShipmodeSeq"].ToString(), e.FormattedValue.ToString())))
                        {
                            MyUtility.Msg.WarningBox(string.Format("< Article: {0} > not found!!!", e.FormattedValue.ToString()));
                            dr["Article"] = "";
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
                        }
                        dr.EndEdit();
                    }
                    else if (MyUtility.Check.Empty(e.FormattedValue))
                    {
                        dr["Article"] = "";
                        dr["Color"] = "";
                        dr["SizeCode"] = "";
                        dr["OtherConfirmQty"] = 0;
                        dr["InvAdjustQty"] = 0;
                        dr["BalanceQty"] = 0;
                        dr["OrderQty"] = 0;
                        dr["shipQty"] = 0;
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
                            sqlCmd = string.Format(@"
select a.SizeCode,a.qty
from
(
	Select oqd.SizeCode, qty = isnull(oq.Qty,0) - isnull(sum(pdd.ShipQty),0),oqd.id
	from Order_QtyShip_Detail oqd WITH (NOLOCK) 
	left join Order_Qty oq WITH (NOLOCK) on oq.id = oqd.id and oq.Article = oqd.Article and oq.SizeCode = oqd.SizeCode
	left join Pullout_Detail_Detail pdd WITH (NOLOCK) on pdd.OrderID = oqd.id and pdd.Article = oqd.Article and pdd.SizeCode = oqd.SizeCode
	where oqd.ID = '{0}' and oqd.Seq = '{1}' and oqd.Article = '{2}' 
	group by oqd.SizeCode,oq.Qty,oqd.id,oqd.Article
)a
left join Orders o WITH (NOLOCK) on o.ID = a.Id
left join Order_SizeCode os WITH (NOLOCK) on os.ID = o.POID and os.SizeCode = a.SizeCode
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
                        dr["OtherConfirmQty"] = 0;
                        dr["InvAdjustQty"] = 0;
                        dr["BalanceQty"] = 0;
                        dr["OrderQty"] = 0;
                        dr["shipQty"] = 0;
                        if (!MyUtility.Check.Seek(string.Format("Select SizeCode from Order_QtyShip_Detail WITH (NOLOCK) where ID = '{0}' and Seq = '{1}' and Article = '{2}' and SizeCode = '{3}'", dr["OrderID"].ToString(), dr["OrderShipmodeSeq"].ToString(), dr["Article"].ToString(), e.FormattedValue.ToString())))
                        {
                            dr["SizeCode"] = "";
                            dr.EndEdit();
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox(string.Format("< SizeCode: {0} > not found!!!", e.FormattedValue.ToString()));
                            return;
                        }
                        else
                        {
                            dr["SizeCode"] = e.FormattedValue;
                            dr.EndEdit();
                            ComputeOrderQty();
                        }
                    }
                    else if (MyUtility.Check.Empty(e.FormattedValue))
                    {
                        dr["SizeCode"] = "";
                        dr["OtherConfirmQty"] = 0;
                        dr["InvAdjustQty"] = 0;
                        dr["BalanceQty"] = 0;
                        dr["OrderQty"] = 0;
                        dr["shipQty"] = 0;
                        dr.EndEdit();
                    }
                }
            };

            this.shipQtySetting.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    dr["ShipQty"] = e.FormattedValue;
                    dr["BalanceQty"] = Convert.ToDecimal(dr["OrderQty"].ToString()) - Convert.ToInt32(dr["OtherConfirmQty"]) - Convert.ToInt32(dr["InvAdjustQty"]) - Convert.ToDecimal(dr["ShipQty"].ToString());
                    dr.EndEdit();
                }
            };
            #endregion

            Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("OrderID", header: "SP No.", width: Widths.AnsiChars(13), settings: orderid).Get(out col_orderid)
                .Text("SeasonID", header: "Season", width: Widths.AnsiChars(6), iseditingreadonly: true)
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
                .Numeric("ShipQty", header: "Qty", settings: shipQtySetting).Get(out col_shipqty)
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
                        string seekSql = string.Format("select Description,Weight from LocalItem WITH (NOLOCK) where RefNo = '{0}'", dr["RefNo"].ToString());
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

            //for (int i = 0; i < this.detailgrid.ColumnCount; i++)
            //{
            //    this.detailgrid.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            //}
        }

        //清空Order相關欄位值
        private void ClearGridRowData(DataRow dr)
        {
            dr["OrderID"] = "";
            dr["SeasonID"] = "";
            dr["OrderShipmodeSeq"] = "";
            dr["Article"] = "";
            dr["Color"] = "";
            dr["SizeCode"] = "";
            dr["StyleID"] = "";
            dr["CustPONo"] = "";
            dr["OtherConfirmQty"] = 0;
            dr["InvAdjustQty"] = 0;
            dr["BalanceQty"] = 0;
            dr["OrderQty"] = 0;
            dr["shipQty"] = 0;
            dr.EndEdit();
        }

        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            txtshipmode.ReadOnly = false;
            CurrentMaintain["FactoryID"] = Sci.Env.User.Factory;
            CurrentMaintain["Type"] = "S";
            CurrentMaintain["Status"] = "New";
            CurrentMaintain["MDivisionID"] = Sci.Env.User.Keyword;

            gridicon.Append.Enabled = true;
            gridicon.Insert.Enabled = true;
            gridicon.Remove.Enabled = true;
            DetailGridEditing(true);
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
            txtbrand.ReadOnly = true;
            //部分欄位會依某些條件來決定是否可以被修改
            if (!MyUtility.Check.Empty(CurrentMaintain["GMTBookingLock"]))
            {
                editRemark.ReadOnly = true;
                txtshipmode.ReadOnly = true;
                gridicon.Append.Enabled = false;
                gridicon.Insert.Enabled = false;
                gridicon.Remove.Enabled = false;
                DetailGridEditing(false);
            }
            else
            {
                DetailGridEditing(true);
                gridicon.Append.Enabled = true;
                gridicon.Insert.Enabled = true;
                gridicon.Remove.Enabled = true;
            }

            if (!MyUtility.Check.Empty(CurrentMaintain["LocalPOID"]))
            {
                dateCartonEstBooking.ReadOnly = true;
                dateCartonEstArrived.ReadOnly = true;
            }
        }

        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(CurrentMaintain["ShipModeID"]))
            {
                txtshipmode.Focus();
                MyUtility.Msg.WarningBox("Ship Mode can't empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["BrandID"]))
            {
                txtbrand.Focus();
                MyUtility.Msg.WarningBox("Brand can't empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["Dest"]))
            {
                txtcountry.Focus();
                MyUtility.Msg.WarningBox("Destination can't empty!!");
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
            sqlCmd = "select OrderID, OrderShipmodeSeq, Article, SizeCode, ShipQty as Qty from PackingList_Detail WITH (NOLOCK) where ID = ''";
            if (!(selectResult = DBProxy.Current.Select(null, sqlCmd, out needPackData)))
            {
                MyUtility.Msg.WarningBox("Query  schema fail!");
                return false;
            }

            //Balance < 0 ErrorMsg
            List<string> listErrorMsg = new List<string>();

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
                    detailgrid.Focus();
                    MyUtility.Msg.WarningBox("< ColorWay >  can't empty!");
                    return false;
                }

                if (MyUtility.Check.Empty(dr["SizeCode"]))
                {
                    detailgrid.Focus();
                    MyUtility.Msg.WarningBox("< Size >  can't empty!");
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
                    sqlCmd = string.Format(@"
select oqd.Id as OrderID
       , oqd.Seq as OrderShipmodeSeq
       , oqd.Article
       , oqd.SizeCode
       , Qty = oqd.Qty 
from Order_QtyShip_Detail oqd WITH (NOLOCK) 
where oqd.Id = '{0}'
      and oqd.Seq = '{1}'", dr["OrderID"].ToString(), dr["OrderShipmodeSeq"].ToString());
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

                string strOtherConfirmSQL = string.Format(@"
select Qty = isnull (sum (pld.ShipQty), 0)
from Packinglist pl
inner join Packinglist_detail pld on pl.ID = pld.ID
where pl.ID != '{0}'
      and pl.Status = 'Confirmed'
      and pld.OrderID = '{1}'
      and pld.OrderShipmodeSeq = '{2}'
      and pld.Article = '{3}'
      and pld.SizeCode = '{4}'", dr["ID"]
                               , dr["OrderID"]
                               , dr["OrderShipmodeSeq"]
                               , dr["Article"]
                               , dr["SizeCode"]);

                string strInvAdjustSQL = string.Format(@"
select Qty = isnull (sum (InvAQ.DiffQty), 0)
from InvAdjust InvA
inner join  InvAdjust_Qty InvAQ WITH (NOLOCK) on InvA.ID = InvAQ.ID
where InvA.OrderID = '{0}'
      and InvA.OrderShipmodeSeq = '{1}'
      and InvAQ.Article = '{2}'
      and InvAQ.SizeCode = '{3}'", dr["OrderID"]
                                 , dr["OrderShipmodeSeq"]
                                 , dr["Article"]
                                 , dr["SizeCode"]);

                dr["OtherConfirmQty"] = MyUtility.GetValue.Lookup(strOtherConfirmSQL);
                dr["InvAdjustQty"] = MyUtility.GetValue.Lookup(strInvAdjustSQL);
                dr["BalanceQty"] = needPackQty - Convert.ToInt32(dr["OtherConfirmQty"]) - Convert.ToInt32(dr["InvAdjustQty"]) - ttlShipQty;
                if (needPackQty - ttlShipQty < 0)
                {
                    isNegativeBalQty = true;
                    listErrorMsg.Add(string.Format("Order Qty: {0}, Shipped Qty: {1}, Qty: {2}", dr["OrderQty"], Convert.ToInt32(dr["OtherConfirmQty"]) - Convert.ToInt32(dr["InvAdjustQty"]), dr["ShipQty"]));
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
                MyUtility.Msg.WarningBox(listErrorMsg.JoinToString(Environment.NewLine) + Environment.NewLine + "Balance Quantity cannot < 0");
                return false;
            }

            //表身Grid不可為空
            if (i == 0)
            {
                detailgrid.Focus();
                MyUtility.Msg.WarningBox("< Detail > can't be empty!");
                return false;
            }

            //檢查Refno是否有改變，若有改變提醒使用者通知採購團隊紙箱號碼改變。
            DataView dataView = DetailDatas.CopyToDataTable().DefaultView;
            DataTable dataTableDistinct = dataView.ToTable(true, "OrderID", "RefNo");
            StringBuilder warningmsg = new StringBuilder();
            warningmsg.Append("Please inform Purchase Team that the Carton Ref No. has been changed.");

            foreach (DataRow dt in dataTableDistinct.Rows)
            {
                if (!MyUtility.Check.Seek(string.Format("select * from LocalPO_Detail where OrderId='{0}'", dt["OrderID"].ToString()))) continue;

                if (!MyUtility.Check.Seek(string.Format("select * from LocalPO_Detail where OrderId='{0}' and Refno='{1}'", dt["OrderID"].ToString(), dt["RefNo"].ToString())))
                {
                    warningmsg.Append(Environment.NewLine + string.Format("SP#：<{0}>, RefNo：<{1}>.", dt["OrderID"].ToString(), dt["RefNo"].ToString()));
                }
            }
            if (warningmsg.ToString() != "Please inform Purchase Team that the Carton Ref No. has been changed.")
                MyUtility.Msg.InfoBox(warningmsg.ToString());

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
            if (MyUtility.Check.Empty(CurrentMaintain["CBM"]) || MyUtility.Check.Empty(CurrentMaintain["GW"]))
            {
                numTtlCBM.Focus();
                MyUtility.Msg.WarningBox("Ttl CBM and Ttl GW can't be empty!!");
                return false;
            }

            return base.ClickSaveBefore();
        }

        protected override void ClickSaveAfter()
        {
            base.ClickSaveAfter();
            DataTable dt = ((DataTable)detailgridbs.DataSource);
            if (!dt.Columns.Contains("OrderQty"))
                dt.ColumnsIntAdd("OrderQty");
            if (!dt.Columns.Contains("OtherConfirmQty"))
                dt.ColumnsIntAdd("OtherConfirmQty");
            if (!dt.Columns.Contains("InvAdjustQty"))
                dt.ColumnsIntAdd("InvAdjustQty");
            ComputeOrderQty();
        }

        protected override DualResult ClickSavePre()
        {
            if (!MyUtility.Check.Empty(CurrentMaintain["INVNo"]))
            {
                sqlCmd = string.Format(@"select isnull(sum(ShipQty),0) as ShipQty,isnull(sum(CTNQty),0) as CTNQty,isnull(sum(NW),0) as NW,isnull(sum(GW),0) as GW,isnull(sum(NNW),0) as NNW,isnull(sum(CBM),0) as CBM
from PackingList WITH (NOLOCK) 
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
        private void btnCartonSummary_Click(object sender, EventArgs e)
        {
            Sci.Production.Packing.P04_CartonSummary callNextForm = new Sci.Production.Packing.P04_CartonSummary(CurrentMaintain["ID"].ToString());
            callNextForm.ShowDialog(this);
        }

        //Batch Import
        private void btnBatchImport_Click(object sender, EventArgs e)
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
        private void btnRecalculateWeight_Click(object sender, EventArgs e)
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

           
        }

        //UnConfirm
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            if (!MyUtility.Check.Empty(CurrentMaintain["INVNo"]))
            {
                if (MyUtility.GetValue.Lookup("Status", CurrentMaintain["INVNo"].ToString(), "GMTBooking", "ID") == "Confirmed")
                {
                    MyUtility.Msg.WarningBox(string.Format("Garment booking already confirmed, so can't unconfirm! \n pullout report:{0}", CurrentMaintain["INVNo"].ToString()));
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

           
        }

        //Download excel format
        private void btnDownloadExcelFormat_Click(object sender, EventArgs e)
        {
            string strXltName = Sci.Env.Cfg.XltPathDir + "\\Packing_P04_ImportExcelFormat.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null) return;
            excel.Visible = true;
        }

        //Import from excel
        private void btnImportFromExcel_Click(object sender, EventArgs e)
        {
            
            if (MyUtility.Check.Empty(CurrentMaintain["BrandID"])) {
                MyUtility.Msg.WarningBox("Please input < Brand >!!");
                return;
            }
            Sci.Production.Packing.P04_ExcelImport callNextForm = new Sci.Production.Packing.P04_ExcelImport((DataTable)detailgridbs.DataSource, CurrentMaintain["BrandID"].ToString());
            callNextForm.ShowDialog(this);
        }

        private void txtcustcd_Validated(object sender, EventArgs e)
        {
            if (this.EditMode && !MyUtility.Check.Empty(txtcustcd.Text) && txtcustcd.OldValue != txtcustcd.Text)
            {
                CurrentMaintain["Dest"] = MyUtility.GetValue.Lookup(string.Format("SELECT CountryID FROM CustCD WITH (NOLOCK) WHERE BrandID = '{0}' AND ID = '{1}'", MyUtility.Convert.GetString(CurrentMaintain["BrandID"]), txtcustcd.Text));
            }      
        }

        private void ComputeOrderQty()
        {
            int needPackQty = 0, ttlShipQty = 0;
            DataTable needPackData, tmpPackData;
            DualResult selectResult;
            DataRow[] detailData;
            //準備needPackData的Schema
            sqlCmd = "select OrderID, OrderShipmodeSeq, Article, SizeCode, ShipQty as Qty from PackingList_Detail WITH (NOLOCK) where ID = ''";
            if (!(selectResult = DBProxy.Current.Select(null, sqlCmd, out needPackData)))
            {
                MyUtility.Msg.WarningBox("Query  schema fail!");
            }

            foreach (DataRow dr in DetailDatas)
            {
                #region 重算表身Grid的Bal. Qty
                //目前還有多少衣服尚未裝箱
                filter = string.Format("OrderID = '{0}' and OrderShipmodeSeq = '{1}'", dr["OrderID"].ToString(), dr["OrderShipmodeSeq"].ToString());
                detailData = needPackData.Select(filter);

                if (detailData.Length <= 0)
                {
                    //撈取此SP+Seq尚未裝箱的數量
                    sqlCmd = string.Format(@"
select oqd.Id as OrderID
       , oqd.Seq as OrderShipmodeSeq
       , oqd.Article
       , oqd.SizeCode
       , Qty = oqd.Qty 
from Order_QtyShip_Detail oqd WITH (NOLOCK) 
where oqd.Id = '{0}'
      and oqd.Seq = '{1}'", dr["OrderID"].ToString()
                          , dr["OrderShipmodeSeq"].ToString());
                    if (!(selectResult = DBProxy.Current.Select(null, sqlCmd, out tmpPackData)))
                    {
                        MyUtility.Msg.WarningBox("Query pack qty fail!");
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

                string strOtherConfirmSQL = string.Format(@"
select Qty = isnull (sum (pld.ShipQty), 0)
from Packinglist pl
inner join Packinglist_detail pld on pl.ID = pld.ID
where pl.ID != '{0}'
      and pl.Status = 'Confirmed'
      and pld.OrderID = '{1}'
      and pld.OrderShipmodeSeq = '{2}'
      and pld.Article = '{3}'
      and pld.SizeCode = '{4}'", dr["ID"]
                               , dr["OrderID"]
                               , dr["OrderShipmodeSeq"]
                               , dr["Article"]
                               , dr["SizeCode"]);

                string strInvAdjustSQL = string.Format(@"
select Qty = isnull (sum (InvAQ.DiffQty), 0)
from InvAdjust InvA
inner join  InvAdjust_Qty InvAQ WITH (NOLOCK) on InvA.ID = InvAQ.ID
where InvA.OrderID = '{0}'
      and InvA.OrderShipmodeSeq = '{1}'
      and InvAQ.Article = '{2}'
      and InvAQ.SizeCode = '{3}'", dr["OrderID"]
                                 , dr["OrderShipmodeSeq"]
                                 , dr["Article"]
                                 , dr["SizeCode"]);

                dr["OtherConfirmQty"] = MyUtility.GetValue.Lookup(strOtherConfirmSQL);
                dr["InvAdjustQty"] = MyUtility.GetValue.Lookup(strInvAdjustSQL);
                dr["BalanceQty"] = needPackQty - Convert.ToInt32(dr["OtherConfirmQty"]) - Convert.ToInt32(dr["InvAdjustQty"]) - ttlShipQty;
                dr["OrderQty"] = needPackQty;
                dr["shipQty"] = ttlShipQty;
                dr.EndEdit();
                #endregion
            }
        }
    }
}
