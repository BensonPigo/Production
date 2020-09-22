using System;
using System.Collections.Generic;
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
using System.Threading.Tasks;
using Sci.Production.Automation;

namespace Sci.Production.Packing
{
    /// <summary>
    /// Packing_P04
    /// </summary>
    public partial class P04 : Win.Tems.Input6
    {
        private Ict.Win.UI.DataGridViewTextBoxColumn col_orderid;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_seq;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_1stctnno;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_lastctnno;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_refno;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_article;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_size;
        private Ict.Win.UI.DataGridViewNumericBoxColumn col_ctnqty;
        private Ict.Win.UI.DataGridViewNumericBoxColumn col_qtyperctn;
        private Ict.Win.UI.DataGridViewNumericBoxColumn col_shipqty;
        private Ict.Win.UI.DataGridViewNumericBoxColumn col_nw;
        private Ict.Win.UI.DataGridViewNumericBoxColumn col_gw;
        private Ict.Win.UI.DataGridViewNumericBoxColumn col_nnw;
        private Ict.Win.UI.DataGridViewNumericBoxColumn col_nwpcs;
        private DataGridViewGeneratorTextColumnSettings orderid = new DataGridViewGeneratorTextColumnSettings();
        private DataGridViewGeneratorTextColumnSettings seq = new DataGridViewGeneratorTextColumnSettings();
        private DataGridViewGeneratorTextColumnSettings article = new DataGridViewGeneratorTextColumnSettings();
        private DataGridViewGeneratorTextColumnSettings size = new DataGridViewGeneratorTextColumnSettings();
        private DataGridViewGeneratorNumericColumnSettings shipQtySetting = new DataGridViewGeneratorNumericColumnSettings();
        private DialogResult buttonResult;
        private DualResult result;
        private DataRow dr;
        private string sqlCmd = string.Empty;
        private string filter = string.Empty;
        private string masterID;

        /// <summary>
        /// P04
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public P04(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = "Type = 'S'";
            this.detailgrid.AllowUserToOrderColumns = true;
            this.InsertDetailGridOnDoubleClick = false;
        }

        /// <summary>
        /// OnDetailSelectCommandPrepare
        /// </summary>
        /// <param name="e">e</param>
        /// <returns>DualResult</returns>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            this.masterID = (e.Master == null) ? string.Empty : e.Master["ID"].ToString();
            this.DetailSelectCommand = Prgs.QueryPackingListSQLCmd(this.masterID);

            return base.OnDetailSelectCommandPrepare(e);
        }

        /// <summary>
        /// OnDetailEntered
        /// </summary>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();

            #region displayPurchaseCtn
            string sqlcmdC = $@"select 1 from LocalPO_Detail ld with(nolock) inner join LocalPO l with(nolock) on l.id = ld.Id
where RequestID='{this.CurrentMaintain["ID"]}' and l.status = 'Approved'
";
            if (MyUtility.Check.Seek(sqlcmdC))
            {
                this.displayPurchaseCTN.Text = "Y";
            }
            else
            {
                this.displayPurchaseCTN.Text = "N";
            }
            #endregion

            this.labelConfirmed.Visible = MyUtility.Check.Empty(this.CurrentMaintain["ID"]) ? false : true;
            DataRow dr;
            string sqlStatus = string.Format(@"select * from PackingList WITH (NOLOCK) where id='{0}'", this.CurrentMaintain["id"].ToString());
            if (MyUtility.Check.Seek(sqlStatus, out dr))
            {
                if (dr["Status"].ToString().ToUpper() == "NEW" && MyUtility.Check.Empty(this.CurrentMaintain["GMTBookingLock"]))
                {
                    this.labelConfirmed.Text = "New";
                }
                else if (dr["Status"].ToString().ToUpper() == "CONFIRMED" && !MyUtility.Check.Empty(this.CurrentMaintain["GMTBookingLock"]))
                {
                    this.labelConfirmed.Text = "Confirmed";
                }
                else if (dr["Status"].ToString().ToUpper() == "CONFIRMED" && MyUtility.Check.Empty(this.CurrentMaintain["GMTBookingLock"]))
                {
                    this.labelConfirmed.Text = "Confirmed";
                }
                else
                {
                    this.labelConfirmed.Text = "Shipping Lock";
                }
            }

            DataTable dt = (DataTable)this.detailgridbs.DataSource;
            if (!dt.Columns.Contains("OrderQty"))
            {
                dt.ColumnsIntAdd("OrderQty");
            }

            if (!dt.Columns.Contains("OtherConfirmQty"))
            {
                dt.ColumnsIntAdd("OtherConfirmQty");
            }

            if (!dt.Columns.Contains("InvAdjustQty"))
            {
                dt.ColumnsIntAdd("InvAdjustQty");
            }

            this.ComputeOrderQty();

            // 加總APPBookingVW & APPEstAmtVW
            this.numAppBookingVW.Value = MyUtility.Convert.GetDecimal(dt.Compute("sum(APPBookingVW)", string.Empty));
            this.numAppEstAmtVW.Value = MyUtility.Convert.GetDecimal(dt.Compute("sum(APPEstAmtVW)", string.Empty));

            this.Color_Change();
        }

        /// <summary>
        /// OnDetailGridSetup
        /// </summary>
        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();

            #region OrderID & Seq & Article & SizeCode按右鍵與Validating

            // OrderID
            this.orderid.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    this.dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);

                    if (!MyUtility.Check.Empty(e.FormattedValue) && e.FormattedValue.ToString() != this.dr["OrderID"].ToString())
                    {
                        // sql參數
                        System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter("@orderid", e.FormattedValue.ToString());
                        System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter("@brandid", MyUtility.Convert.GetString(this.CurrentMaintain["BrandID"]));

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
                            this.ClearGridRowData(this.dr);
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox("Sql connection fail!!\r\n" + result.ToString());
                            return;
                        }
                        else
                        {
                            if (orderData.Rows.Count <= 0)
                            {
                                MessageBox.Show(string.Format("< SP No.: {0} > not found!!!", e.FormattedValue.ToString()));
                                this.ClearGridRowData(this.dr);
                                e.Cancel = true;
                                return;
                            }
                            else
                            {
                                this.dr["OrderID"] = e.FormattedValue.ToString().ToUpper();
                                this.dr["StyleID"] = orderData.Rows[0]["StyleID"].ToString();
                                this.dr["CustPONo"] = orderData.Rows[0]["CustPONo"].ToString();
                                this.dr["SeasonID"] = orderData.Rows[0]["SeasonID"].ToString();
                                this.dr["Article"] = string.Empty;
                                this.dr["Color"] = string.Empty;
                                this.dr["SizeCode"] = string.Empty;
                                this.dr["OtherConfirmQty"] = 0;
                                this.dr["InvAdjustQty"] = 0;
                                this.dr["BalanceQty"] = 0;
                                this.dr["OrderQty"] = 0;
                                this.dr["shipQty"] = 0;
                                #region 若Order_QtyShip有多筆資料話就跳出視窗讓使者選擇Seq
                                DataRow orderQtyData;
                                sqlCmd = string.Format("select count(ID) as CountID from Order_QtyShip WITH (NOLOCK) where ID = '{0}'", this.dr["OrderID"].ToString());
                                if (MyUtility.Check.Seek(sqlCmd, out orderQtyData))
                                {
                                    if (orderQtyData["CountID"].ToString() == "1")
                                    {
                                        this.dr["OrderShipmodeSeq"] = MyUtility.GetValue.Lookup("Seq", this.dr["OrderID"].ToString(), "Order_QtyShip", "ID");
                                    }
                                    else
                                    {
                                        sqlCmd = string.Format("select Seq,BuyerDelivery,ShipmodeID,Qty from Order_QtyShip WITH (NOLOCK) where ID = '{0}'", this.dr["OrderID"].ToString());
                                        Win.Tools.SelectItem item = new Win.Tools.SelectItem(sqlCmd, "4,20,20,10", string.Empty, "Seq,Buyer Delivery,ShipMode,Qty");
                                        DialogResult returnResult = item.ShowDialog();
                                        if (returnResult == DialogResult.Cancel)
                                        {
                                            this.dr["OrderShipmodeSeq"] = string.Empty;
                                        }
                                        else
                                        {
                                            this.dr["OrderShipmodeSeq"] = item.GetSelectedString();
                                        }
                                    }
                                }
                                #endregion
                                this.dr.EndEdit();
                            }
                        }
                    }
                    else if (MyUtility.Check.Empty(e.FormattedValue))
                    {
                        this.ClearGridRowData(this.dr);
                    }
                }
            };

            // Seq
            this.seq.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode & MyUtility.Check.Empty(this.CurrentMaintain["GMTBookingLock"]))
                {
                    if (e.Button == MouseButtons.Right)
                    {
                        if (e.RowIndex != -1)
                        {
                            DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                            this.sqlCmd = string.Format("select Seq, BuyerDelivery,ShipmodeID,Qty from Order_QtyShip WITH (NOLOCK) where ID = '{0}'", dr["OrderID"].ToString());
                            Win.Tools.SelectItem item = new Win.Tools.SelectItem(this.sqlCmd, "4,20,20,10", string.Empty, "Seq,Buyer Delivery,ShipMode,Qty");
                            DialogResult returnResult = item.ShowDialog();
                            if (returnResult == DialogResult.Cancel)
                            {
                                return;
                            }
                            else
                            {
                                dr["OrderShipmodeSeq"] = item.GetSelectedString();
                            }

                            dr["Article"] = string.Empty;
                            dr["Color"] = string.Empty;
                            dr["SizeCode"] = string.Empty;
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

            // Article
            this.article.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode & MyUtility.Check.Empty(this.CurrentMaintain["GMTBookingLock"]))
                {
                    if (e.Button == MouseButtons.Right)
                    {
                        if (e.RowIndex != -1)
                        {
                            DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                            this.sqlCmd = string.Format("Select Distinct Article from Order_QtyShip_Detail WITH (NOLOCK) where ID = '{0}' and Seq = '{1}'", dr["OrderID"].ToString(), dr["OrderShipmodeSeq"].ToString());
                            Win.Tools.SelectItem item = new Win.Tools.SelectItem(this.sqlCmd, "8", dr["Article"].ToString());
                            DialogResult returnResult = item.ShowDialog();
                            if (returnResult == DialogResult.Cancel)
                            {
                                return;
                            }

                            e.EditingControl.Text = item.GetSelectedString();
                        }
                    }
                }
            };

            this.article.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    if (!MyUtility.Check.Empty(e.FormattedValue) && e.FormattedValue.ToString() != dr["Article"].ToString())
                    {
                        dr["Color"] = string.Empty;
                        dr["SizeCode"] = string.Empty;
                        dr["OtherConfirmQty"] = 0;
                        dr["InvAdjustQty"] = 0;
                        dr["BalanceQty"] = 0;
                        dr["OrderQty"] = 0;
                        dr["shipQty"] = 0;
                        if (!MyUtility.Check.Seek(string.Format("Select Article from Order_QtyShip_Detail WITH (NOLOCK) where ID = '{0}' and Seq = '{1}' and Article = '{2}'", dr["OrderID"].ToString(), dr["OrderShipmodeSeq"].ToString(), e.FormattedValue.ToString())))
                        {
                            MyUtility.Msg.WarningBox(string.Format("< Article: {0} > not found!!!", e.FormattedValue.ToString()));
                            dr["Article"] = string.Empty;
                        }
                        else
                        {
                            dr["Article"] = e.FormattedValue.ToString().ToUpper();
                            this.sqlCmd = string.Format(
                                @"select ColorID 
                                                                        from View_OrderFAColor 
                                                                        where ID = '{0}' and Article = '{1}'",
                                dr["OrderID"].ToString(),
                                dr["Article"]);
                            DataRow colorData;
                            if (MyUtility.Check.Seek(this.sqlCmd, out colorData))
                            {
                                dr["Color"] = colorData["ColorID"].ToString();
                            }
                        }

                        dr.EndEdit();
                    }
                    else if (MyUtility.Check.Empty(e.FormattedValue))
                    {
                        dr["Article"] = string.Empty;
                        dr["Color"] = string.Empty;
                        dr["SizeCode"] = string.Empty;
                        dr["OtherConfirmQty"] = 0;
                        dr["InvAdjustQty"] = 0;
                        dr["BalanceQty"] = 0;
                        dr["OrderQty"] = 0;
                        dr["shipQty"] = 0;
                        dr.EndEdit();
                    }
                }
            };

            // SizeCode
            this.size.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode & MyUtility.Check.Empty(this.CurrentMaintain["GMTBookingLock"]))
                {
                    if (e.Button == MouseButtons.Right)
                    {
                        if (e.RowIndex != -1)
                        {
                            DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                            this.sqlCmd = string.Format(
                                @"
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
order by os.Seq", dr["OrderID"].ToString(),
                                dr["OrderShipmodeSeq"].ToString(),
                                dr["Article"].ToString());
                            Win.Tools.SelectItem item = new Win.Tools.SelectItem(this.sqlCmd, "8", dr["SizeCode"].ToString());
                            DialogResult returnResult = item.ShowDialog();
                            if (returnResult == DialogResult.Cancel)
                            {
                                return;
                            }

                            e.EditingControl.Text = item.GetSelectedString();
                        }
                    }
                }
            };

            this.size.CellValidating += (s, e) =>
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
                            dr["SizeCode"] = string.Empty;
                            dr.EndEdit();
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox(string.Format("< SizeCode: {0} > not found!!!", e.FormattedValue.ToString()));
                            return;
                        }
                        else
                        {
                            dr["SizeCode"] = e.FormattedValue;
                            dr.EndEdit();
                            this.ComputeOrderQty();
                        }
                    }
                    else if (MyUtility.Check.Empty(e.FormattedValue))
                    {
                        dr["SizeCode"] = string.Empty;
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

            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("OrderID", header: "SP No.", width: Widths.AnsiChars(13), settings: this.orderid).Get(out this.col_orderid)
                .Text("SeasonID", header: "Season", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("OrderShipmodeSeq", header: "Seq", width: Widths.AnsiChars(2), iseditingreadonly: true, settings: this.seq).Get(out this.col_seq)
                .Text("StyleID", header: "Style No.", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("CustPONo", header: "P.O. No.", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("CTNStartNo", header: "1st CTN#", width: Widths.AnsiChars(6)).Get(out this.col_1stctnno)
                .Text("CTNEndNo", header: "Last CTN#", width: Widths.AnsiChars(6)).Get(out this.col_lastctnno)
                .Numeric("CTNQty", header: "# of CTN").Get(out this.col_ctnqty)
                .CellCartonItem("RefNo", header: "Ref No.", width: Widths.AnsiChars(13)).Get(out this.col_refno)
                .Text("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("Article", header: "ColorWay", width: Widths.AnsiChars(8), settings: this.article).Get(out this.col_article)
                .Text("Color", header: "Color", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(8), settings: this.size).Get(out this.col_size)
                .Numeric("QtyPerCTN", header: "PC/Ctn").Get(out this.col_qtyperctn)
                .Numeric("ShipQty", header: "Qty", settings: this.shipQtySetting).Get(out this.col_shipqty)
                .Numeric("BalanceQty", header: "Bal. Qty", iseditingreadonly: true)
                .Numeric("NW", header: "N.W./Ctn", integer_places: 3, decimal_places: 3, maximum: 999.999M, minimum: 0).Get(out this.col_nw)
                .Numeric("GW", header: "G.W./Ctn", integer_places: 3, decimal_places: 3, maximum: 999.999M, minimum: 0).Get(out this.col_gw)
                .Numeric("NNW", header: "N.N.W./Ctn", integer_places: 3, decimal_places: 3, maximum: 999.999M, minimum: 0).Get(out this.col_nnw)
                .Numeric("NWPerPcs", header: "N.W./Pcs", integer_places: 2, decimal_places: 3, maximum: 99.999M, minimum: 0).Get(out this.col_nwpcs);

            this.detailgrid.CellValueChanged += (s, e) =>
            {
                #region 選完RefNo後，要自動帶出Description
                if (this.detailgrid.Columns[e.ColumnIndex].DataPropertyName == this.col_refno.DataPropertyName)
                {
                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);

                    if (MyUtility.Check.Empty(dr["RefNo"]))
                    {
                        dr["Description"] = string.Empty;
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
                            dr["Description"] = string.Empty;
                        }
                    }

                    dr.EndEdit();
                }
                #endregion
            };

            this.Color_Change();
        }

        // 清空Order相關欄位值
        private void ClearGridRowData(DataRow dr)
        {
            dr["OrderID"] = string.Empty;
            dr["SeasonID"] = string.Empty;
            dr["OrderShipmodeSeq"] = string.Empty;
            dr["Article"] = string.Empty;
            dr["Color"] = string.Empty;
            dr["SizeCode"] = string.Empty;
            dr["StyleID"] = string.Empty;
            dr["CustPONo"] = string.Empty;
            dr["OtherConfirmQty"] = 0;
            dr["InvAdjustQty"] = 0;
            dr["BalanceQty"] = 0;
            dr["OrderQty"] = 0;
            dr["shipQty"] = 0;
            dr.EndEdit();
        }

        /// <summary>
        /// ClickNewAfter
        /// </summary>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.txtshipmode.ReadOnly = false;
            this.CurrentMaintain["FactoryID"] = Env.User.Factory;
            this.CurrentMaintain["Type"] = "S";
            this.CurrentMaintain["Status"] = "New";
            this.CurrentMaintain["MDivisionID"] = Env.User.Keyword;
            this.CurrentMaintain["QueryDate"] = DateTime.Now.ToShortDateString();

            this.gridicon.Append.Enabled = true;
            this.gridicon.Insert.Enabled = true;
            this.gridicon.Remove.Enabled = true;
            this.DetailGridEditing(true);
        }

        /// <summary>
        /// ClickEditBefore
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ClickEditBefore()
        {
            if (this.CurrentMaintain["Status"].ToString() == "Confirmed")
            {
                MyUtility.Msg.WarningBox("This record is < Confirmed >, can't be modified!");
                return false;
            }

            return base.ClickEditBefore();
        }

        /// <summary>
        /// ClickEditAfter
        /// </summary>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtbrand.ReadOnly = true;

            // 部分欄位會依某些條件來決定是否可以被修改
            if (!MyUtility.Check.Empty(this.CurrentMaintain["GMTBookingLock"]))
            {
                this.editRemark.ReadOnly = true;
                this.txtshipmode.ReadOnly = true;
                this.gridicon.Append.Enabled = false;
                this.gridicon.Insert.Enabled = false;
                this.gridicon.Remove.Enabled = false;
                this.DetailGridEditing(false);
            }
            else
            {
                this.DetailGridEditing(true);
                this.gridicon.Append.Enabled = true;
                this.gridicon.Insert.Enabled = true;
                this.gridicon.Remove.Enabled = true;
            }

            if (!MyUtility.Check.Empty(this.CurrentMaintain["LocalPOID"]))
            {
                this.dateCartonEstBooking.ReadOnly = true;
                this.dateCartonEstArrived.ReadOnly = true;
            }
        }

        /// <summary>
        /// ClickSaveBefore
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["ShipModeID"]))
            {
                this.txtshipmode.Focus();
                MyUtility.Msg.WarningBox("Ship Mode can't empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["BrandID"]))
            {
                this.txtbrand.Focus();
                MyUtility.Msg.WarningBox("Brand can't empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["Dest"]))
            {
                this.txtcountry.Focus();
                MyUtility.Msg.WarningBox("Destination can't empty!!");
                return false;
            }

            // 檢查 表頭的 ShipMode 是否屬於必須建立 APP（請至資料表 : ShipMode 確認是否需要建立 APP : NeedCreateAPP）
            // 若確認必須要建立 APP 則判斷表身的 N.W. / pcs 是否都有輸入
            bool isNeedCreateAPP = MyUtility.Check.Seek($"select 1 from ShipMode with (nolock) where ID = '{this.CurrentMaintain["ShipModeID"]}' and NeedCreateAPP = 1");

            if (isNeedCreateAPP)
            {
                bool isNWPcsEmpty = this.DetailDatas.Where(s => MyUtility.Check.Empty(s["NWPerPcs"])).Any();
                if (isNWPcsEmpty)
                {
                    string shipModeAirPP = Prgs.GetNeedCreateAppShipMode();
                    MyUtility.Msg.WarningBox($"Shipping mode is {shipModeAirPP} row data need input N.W./Pcs");
                    return false;
                }
            }

            // 刪除表身SP No.或Qty為空白的資料，檢查表身的Color Way與Size不可以為空值，順便填入Seq欄位值，計算CTNQty, ShipQty, NW, GW, NNW, CBM，重算表身Grid的Bal. Qty
            int i = 0, ctnQty = 0, shipQty = 0, needPackQty = 0, ttlShipQty = 0, count = 0, drCtnQty = 0;
            double nw = 0.0, gw = 0.0, nnw = 0.0, cbm = 0.0;
            string ctnCBM;
            bool isNegativeBalQty = false;
            DataTable needPackData, tmpPackData;
            DualResult selectResult;
            DataRow[] detailData;

            // 準備needPackData的Schema
            this.sqlCmd = "select OrderID, OrderShipmodeSeq, Article, SizeCode, ShipQty as Qty from PackingList_Detail WITH (NOLOCK) where ID = ''";
            if (!(selectResult = DBProxy.Current.Select(null, this.sqlCmd, out needPackData)))
            {
                MyUtility.Msg.WarningBox("Query  schema fail!");
                return false;
            }

            // Balance < 0 ErrorMsg
            List<string> listErrorMsg = new List<string>();

            foreach (DataRow dr in this.DetailDatas)
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
                    this.detailgrid.Focus();
                    MyUtility.Msg.WarningBox("< ColorWay >  can't empty!");
                    return false;
                }

                if (MyUtility.Check.Empty(dr["SizeCode"]))
                {
                    this.detailgrid.Focus();
                    MyUtility.Msg.WarningBox("< Size >  can't empty!");
                    return false;
                }
                #endregion

                #region 填入Seq欄位值
                i = i + 1;
                dr["Seq"] = Convert.ToString(i).PadLeft(6, '0');
                #endregion

                #region 計算CTNQty, ShipQty, NW, GW, NNW, CBM
                drCtnQty = MyUtility.Check.Empty(dr["CTNQty"]) ? 0 : MyUtility.Convert.GetInt(dr["CTNQty"]);
                ctnQty = ctnQty + drCtnQty;
                shipQty = shipQty + MyUtility.Convert.GetInt(dr["ShipQty"]);
                nw = MyUtility.Math.Round(nw + (MyUtility.Convert.GetDouble(dr["NW"]) * MyUtility.Convert.GetDouble(dr["CTNQty"])), 3);
                gw = MyUtility.Math.Round(gw + (MyUtility.Convert.GetDouble(dr["GW"]) * MyUtility.Convert.GetDouble(dr["CTNQty"])), 3);
                nnw = MyUtility.Math.Round(nnw + (MyUtility.Convert.GetDouble(dr["NNW"]) * MyUtility.Convert.GetDouble(dr["CTNQty"])), 3);
                if (drCtnQty > 0)
                {
                    ctnCBM = MyUtility.GetValue.Lookup("CBM", dr["RefNo"].ToString(), "LocalItem", "RefNo");

                    // ISP20181015 CBM抓到小數點後4位
                    cbm = MyUtility.Math.Round(cbm + (MyUtility.Convert.GetDouble(ctnCBM) * drCtnQty), 4);
                }
                #endregion

                #region 重新計算表身每一個紙箱的材積重
                DataRow drLocalitem;
                string sqlLocalItem = $@"
select 
[BookingVW] = isnull(round(
	(CtnLength * CtnWidth * CtnHeight * POWER(rate.value,3)) /6000
,2),0)
,[APPEstAmtVW] = isnull(round(
	(CtnLength * CtnWidth * CtnHeight * POWER(rate.value,3)) /5000
,2),0)
from LocalItem
outer apply(
	select value = ( case when CtnUnit='MM' then 0.1 else dbo.getUnitRate(CtnUnit,'CM') end)
) rate
where RefNo='{dr["Refno"]}'";
                if (MyUtility.Check.Empty(dr["CTNQty"]) || MyUtility.Convert.GetInt(dr["CTNQty"]) > 0)
                {
                    if (MyUtility.Check.Seek(sqlLocalItem, out drLocalitem))
                    {
                        dr["AppBookingVW"] = MyUtility.Convert.GetDecimal(drLocalitem["BookingVW"]) * MyUtility.Convert.GetInt(dr["CTNQty"]);
                        dr["APPEstAmtVW"] = MyUtility.Convert.GetDecimal(drLocalitem["APPEstAmtVW"]) * MyUtility.Convert.GetInt(dr["CTNQty"]);
                    }
                }
                else
                {
                    dr["AppBookingVW"] = 0;
                    dr["APPEstAmtVW"] = 0;
                }

                #endregion

                #region 重算表身Grid的Bal. Qty

                // 目前還有多少衣服尚未裝箱
                this.filter = string.Format("OrderID = '{0}' and OrderShipmodeSeq = '{1}'", dr["OrderID"].ToString(), dr["OrderShipmodeSeq"].ToString());
                detailData = needPackData.Select(this.filter);
                if (detailData.Length <= 0)
                {
                    // 撈取此SP+Seq尚未裝箱的數量
                    this.sqlCmd = string.Format(
                        @"
select oqd.Id as OrderID
       , oqd.Seq as OrderShipmodeSeq
       , oqd.Article
       , oqd.SizeCode
       , Qty = oqd.Qty 
from Order_QtyShip_Detail oqd WITH (NOLOCK) 
where oqd.Id = '{0}'
      and oqd.Seq = '{1}'", dr["OrderID"].ToString(),
                        dr["OrderShipmodeSeq"].ToString());
                    if (!(selectResult = DBProxy.Current.Select(null, this.sqlCmd, out tmpPackData)))
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
                this.filter = string.Format("OrderID = '{0}' and OrderShipmodeSeq = '{1}' and Article = '{2}' and SizeCode = '{3}'", dr["OrderID"].ToString(), dr["OrderShipmodeSeq"].ToString(), dr["Article"].ToString(), dr["SizeCode"].ToString());
                detailData = needPackData.Select(this.filter);
                if (detailData.Length > 0)
                {
                    needPackQty = MyUtility.Convert.GetInt(detailData[0]["Qty"]);
                }

                // 加總表身特定Article/SizeCode的Ship Qty數量
                detailData = ((DataTable)this.detailgridbs.DataSource).Select(this.filter);
                ttlShipQty = 0;
                if (detailData.Length != 0)
                {
                    foreach (DataRow dDr in detailData)
                    {
                        ttlShipQty = ttlShipQty + MyUtility.Convert.GetInt(dDr["ShipQty"]);
                    }
                }

                string strOtherConfirmSQL = string.Format(
                    @"
select Qty = isnull (sum (pld.ShipQty), 0)
from Packinglist pl
inner join Packinglist_detail pld on pl.ID = pld.ID
where pl.ID != '{0}'
      and pl.Status = 'Confirmed'
      and pld.OrderID = '{1}'
      and pld.OrderShipmodeSeq = '{2}'
      and pld.Article = '{3}'
      and pld.SizeCode = '{4}'", dr["ID"],
                    dr["OrderID"],
                    dr["OrderShipmodeSeq"],
                    dr["Article"],
                    dr["SizeCode"]);

                string strInvAdjustSQL = string.Format(
                    @"
select Qty = isnull (sum (InvAQ.DiffQty), 0)
from InvAdjust InvA
inner join  InvAdjust_Qty InvAQ WITH (NOLOCK) on InvA.ID = InvAQ.ID
where InvA.OrderID = '{0}'
      and InvA.OrderShipmodeSeq = '{1}'
      and InvAQ.Article = '{2}'
      and InvAQ.SizeCode = '{3}'", dr["OrderID"],
                    dr["OrderShipmodeSeq"],
                    dr["Article"],
                    dr["SizeCode"]);

                dr["OtherConfirmQty"] = MyUtility.GetValue.Lookup(strOtherConfirmSQL);
                dr["InvAdjustQty"] = MyUtility.GetValue.Lookup(strInvAdjustSQL);
                dr["BalanceQty"] = needPackQty - Convert.ToInt32(dr["OtherConfirmQty"]) - Convert.ToInt32(dr["InvAdjustQty"]) - ttlShipQty;
                if (needPackQty - ttlShipQty < 0)
                {
                    isNegativeBalQty = true;
                    listErrorMsg.Add(string.Format("Order Qty: {0}, Shipped Qty: {1}, Qty: {2}", dr["OrderQty"], Convert.ToInt32(dr["OtherConfirmQty"]) - Convert.ToInt32(dr["InvAdjustQty"]), dr["ShipQty"]));
                    this.detailgrid.Rows[count].DefaultCellStyle.BackColor = Color.Pink;
                }
                #endregion
                count = count + 1;
            }

            // CTNQty, ShipQty, NW, GW, NNW, CBM
            this.CurrentMaintain["CTNQty"] = ctnQty;
            this.CurrentMaintain["ShipQty"] = shipQty;
            this.CurrentMaintain["NW"] = nw;
            this.CurrentMaintain["GW"] = gw;
            this.CurrentMaintain["NNW"] = nnw;
            this.CurrentMaintain["CBM"] = cbm;

            if (isNegativeBalQty)
            {
                MyUtility.Msg.WarningBox(listErrorMsg.JoinToString(Environment.NewLine) + Environment.NewLine + "Balance Quantity cannot < 0");
                return false;
            }

            // 表身Grid不可為空
            if (i == 0)
            {
                this.detailgrid.Focus();
                MyUtility.Msg.WarningBox("< Detail > can't be empty!");
                return false;
            }

            // 檢查Refno是否有改變，若有改變提醒使用者通知採購團隊紙箱號碼改變。
            DataView dataView = this.DetailDatas.CopyToDataTable().DefaultView;
            DataTable dataTableDistinct = dataView.ToTable(true, "OrderID", "RefNo");
            StringBuilder warningmsg = new StringBuilder();
            warningmsg.Append("Please inform Purchase Team that the Carton Ref No. has been changed.");

            foreach (DataRow dt in dataTableDistinct.Rows)
            {
                if (!MyUtility.Check.Seek(string.Format("select * from LocalPO_Detail where OrderId='{0}'", dt["OrderID"].ToString())))
                {
                    continue;
                }

                if (!MyUtility.Check.Seek(string.Format("select * from LocalPO_Detail where OrderId='{0}' and Refno='{1}'", dt["OrderID"].ToString(), dt["RefNo"].ToString())))
                {
                    warningmsg.Append(Environment.NewLine + string.Format("SP#：<{0}>, RefNo：<{1}>.", dt["OrderID"].ToString(), dt["RefNo"].ToString()));
                }
            }

            if (warningmsg.ToString() != "Please inform Purchase Team that the Carton Ref No. has been changed.")
            {
                MyUtility.Msg.InfoBox(warningmsg.ToString());
            }

            // GetID
            if (this.IsDetailInserting)
            {
                string id = MyUtility.GetValue.GetID(Env.User.Keyword + "PS", "PackingList", DateTime.Today, 2, "Id", null);
                if (MyUtility.Check.Empty(id))
                {
                    MyUtility.Msg.WarningBox("GetID fail, please try again!");
                    return false;
                }

                this.CurrentMaintain["ID"] = id;
            }

            // Get表身 SCICtnNo
            if (this.IsDetailInserting)
            {
                if (!Prgs.GetSCICtnNo((DataTable)this.detailgridbs.DataSource, this.CurrentMaintain["ID"].ToString(), "IsDetailInserting"))
                {
                    return false;
                }
            }
            else
            {
                if (!Prgs.GetSCICtnNo((DataTable)this.detailgridbs.DataSource, this.CurrentMaintain["ID"].ToString(), string.Empty))
                {
                    return false;
                }
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["CBM"]) || MyUtility.Check.Empty(this.CurrentMaintain["GW"]))
            {
                this.numTtlCBM.Focus();
                MyUtility.Msg.WarningBox("Ttl CBM and Ttl GW can't be empty!!");
                return false;
            }

            return base.ClickSaveBefore();
        }

        /// <summary>
        /// ClickSaveAfter
        /// </summary>
        protected override void ClickSaveAfter()
        {
            base.ClickSaveAfter();
            DataTable dt = (DataTable)this.detailgridbs.DataSource;
            if (!dt.Columns.Contains("OrderQty"))
            {
                dt.ColumnsIntAdd("OrderQty");
            }

            if (!dt.Columns.Contains("OtherConfirmQty"))
            {
                dt.ColumnsIntAdd("OtherConfirmQty");
            }

            if (!dt.Columns.Contains("InvAdjustQty"))
            {
                dt.ColumnsIntAdd("InvAdjustQty");
            }

            this.ComputeOrderQty();
            #region ISP20200757 資料交換 - Sunrise
            Task.Run(() => new Sunrise_FinishingProcesses().SentPackingToFinishingProcesses(this.CurrentMaintain["ID"].ToString(), string.Empty))
                .ContinueWith(UtilityAutomation.AutomationExceptionHandler, TaskContinuationOptions.OnlyOnFaulted);
            #endregion
        }

        /// <inheritdoc/>
        protected override DualResult ClickDeletePost()
        {
            #region ISP20200757 資料交換 - Sunrise
            Task.Run(() => new Sunrise_FinishingProcesses().SentPackingToFinishingProcesses(this.CurrentMaintain["ID"].ToString(), "delete"))
                .ContinueWith(UtilityAutomation.AutomationExceptionHandler, TaskContinuationOptions.OnlyOnFaulted);
            #endregion
            return base.ClickDeletePost();
        }

        /// <summary>
        /// ClickSavePre
        /// </summary>
        /// <returns>DualResult</returns>
        protected override DualResult ClickSavePre()
        {
            return Prgs.P03_UpdateGMT(this.CurrentMaintain, (DataTable)this.detailgridbs.DataSource);
        }

        /// <summary>
        /// ClickSavePost
        /// </summary>
        /// <returns>DualResult</returns>
        protected override DualResult ClickSavePost()
        {
            // 存檔成功後，要再呼叫CreateOrderCTNData
            if (!Prgs.CreateOrderCTNData(this.CurrentMaintain["ID"].ToString()))
            {
                DualResult failResult = new DualResult(false, "Create Order_CTN fail!");
                return failResult;
            }

            return Ict.Result.True;
        }

        /// <summary>
        /// ClickDeleteBefore
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ClickDeleteBefore()
        {
            if (this.CurrentMaintain["Status"].ToString() == "Confirmed")
            {
                MyUtility.Msg.WarningBox("This record is < Confirmed >, can't be deleted!");
                return false;
            }

            if (!MyUtility.Check.Empty(this.CurrentMaintain["INVNo"]))
            {
                MyUtility.Msg.WarningBox("This record had booking no. Can't be deleted!");
                return false;
            }

            return base.ClickDeleteBefore();
        }

        /// <summary>
        /// ClickPrint
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ClickPrint()
        {
            P04_Print callNextForm = new P04_Print(this.CurrentMaintain);
            callNextForm.ShowDialog(this);
            return base.ClickPrint();
        }

        // 控制表身Grid欄位是否可被編輯
        private void DetailGridEditing(bool isEditing)
        {
            if (isEditing)
            {
                this.col_orderid.IsEditingReadOnly = false;
                this.col_1stctnno.IsEditingReadOnly = false;
                this.col_lastctnno.IsEditingReadOnly = false;
                this.col_ctnqty.IsEditingReadOnly = false;
                this.col_refno.IsEditingReadOnly = false;
                this.col_article.IsEditingReadOnly = false;
                this.col_size.IsEditingReadOnly = false;
                this.col_qtyperctn.IsEditingReadOnly = false;
                this.col_shipqty.IsEditingReadOnly = false;
                this.col_nw.IsEditingReadOnly = false;
                this.col_gw.IsEditingReadOnly = false;
                this.col_nnw.IsEditingReadOnly = false;

                for (int i = 0; i < this.detailgrid.ColumnCount; i++)
                {
                    if (i == 0 || i == 1 || i == 4 || i == 5 || i == 6 || i == 7 || i == 9 || i == 11 || i == 12 || i == 13 || i == 15 || i == 16 || i == 17 || i == 18)
                    {
                        this.detailgrid.Columns[i].DefaultCellStyle.ForeColor = Color.Red;
                    }
                }

                // 先-再+預防重複出現多次視窗
                this.col_refno.EditingMouseDown -= new EventHandler<Ict.Win.UI.DataGridViewEditingControlMouseEventArgs>(CartonRefnoCommon.EditingMouseDown);
                this.col_refno.EditingMouseDown += new EventHandler<Ict.Win.UI.DataGridViewEditingControlMouseEventArgs>(CartonRefnoCommon.EditingMouseDown);
            }
            else
            {
                this.col_orderid.IsEditingReadOnly = true;
                this.col_1stctnno.IsEditingReadOnly = true;
                this.col_lastctnno.IsEditingReadOnly = true;
                this.col_ctnqty.IsEditingReadOnly = true;
                this.col_refno.IsEditingReadOnly = true;
                this.col_article.IsEditingReadOnly = true;
                this.col_size.IsEditingReadOnly = true;
                this.col_qtyperctn.IsEditingReadOnly = true;
                this.col_shipqty.IsEditingReadOnly = true;
                this.col_nw.IsEditingReadOnly = true;
                this.col_gw.IsEditingReadOnly = true;
                this.col_nnw.IsEditingReadOnly = true;
                for (int i = 0; i < this.detailgrid.ColumnCount; i++)
                {
                    if (i != 18)
                    {
                        this.detailgrid.Columns[i].DefaultCellStyle.ForeColor = Color.Black;
                    }
                }

                this.col_refno.EditingMouseDown -= new EventHandler<Ict.Win.UI.DataGridViewEditingControlMouseEventArgs>(CartonRefnoCommon.EditingMouseDown);
            }
        }

        // Carton Summary
        private void BtnCartonSummary_Click(object sender, EventArgs e)
        {
            P04_CartonSummary callNextForm = new P04_CartonSummary(this.CurrentMaintain["ID"].ToString());
            callNextForm.ShowDialog(this);
        }

        // Batch Import
        private void BtnBatchImport_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["BrandID"]))
            {
                MyUtility.Msg.WarningBox("Brand can't be empty!");
                return;
            }

            P04_BatchImport callNextForm = new P04_BatchImport(this.CurrentMaintain, (DataTable)this.detailgridbs.DataSource);
            callNextForm.ShowDialog(this);
        }

        // Recalculate Weight
        private void BtnRecalculateWeight_Click(object sender, EventArgs e)
        {
            // 如果已經Shipping Lock的話就不可以再重算重量
            if (!MyUtility.Check.Empty(this.CurrentMaintain["GMTBookingLock"]))
            {
                MyUtility.Msg.WarningBox("This record already shipping lock, can't recalculate weight!");
                return;
            }

            Prgs.RecaluateCartonWeight((DataTable)this.detailgridbs.DataSource, this.CurrentMaintain);
        }

        /// <summary>
        /// ClickConfirm
        /// </summary>
        protected override void ClickConfirm()
        {
            base.ClickConfirm();

            // 還沒有Invoice No就不可以做Confirm
            if (MyUtility.Check.Empty(MyUtility.GetValue.Lookup("INVNo", this.CurrentMaintain["ID"].ToString(), "PackingList", "ID")))
            {
                MyUtility.Msg.WarningBox("Shipping is not yet booking so can't confirm!");
                return;
            }

            // 檢查累計Pullout數不可超過訂單數量
            if (!Prgs.CheckPulloutQtyWithOrderQty(this.CurrentMaintain["ID"].ToString()))
            {
                return;
            }

            // 檢查Sewing Output Qty是否有超過Packing Qty
            if (!Prgs.CheckPackingQtyWithSewingOutput(this.CurrentMaintain["ID"].ToString()))
            {
                return;
            }

            string sqlchk = $@"
SELECT  msg=concat(pd.OrderID,'(',pd.OrderShipmodeSeq,')')
FROm PackingList p
INNER JOIN PackingList_Detail pd On p.ID=pd.ID
INNER JOIN  Order_QtyShip oq ON pd.OrderID=oq.Id
LEFt JOIN ShipMode s ON oq.ShipModeID=s.ID
WHERE p.ID='{this.CurrentMaintain["ID"]}'
AND s.ShipGroup <> (
	SELECT TOP 1 sm.ShipGroup
	FROm PackingList p
	LEFt JOIN ShipMode sm ON p.ShipModeID=sm.ID
	WHERE p.ID='{this.CurrentMaintain["ID"]}'
)";

            DataTable dtchk;
            DualResult dualResult = DBProxy.Current.Select(null, sqlchk, out dtchk);
            if (!dualResult)
            {
                this.ShowErr(dualResult);
                return;
            }

            if (dtchk.Rows.Count > 0)
            {
                var os = dtchk.AsEnumerable().Select(s => MyUtility.Convert.GetString(s["msg"])).Distinct().ToList();
                string msg = @"Ship Mode are different, please check!
" + string.Join(",", os);
                MyUtility.Msg.WarningBox(msg);
                return;
            }

            #region 確認Order的訂單數量是否超過PackingList_Detail 的總和(不分Type、PackingListID)
            if (!Prgs.CheckOrderQty_ShipQty(MyUtility.Convert.GetString(this.CurrentMaintain["ID"])))
            {
                return;
            }
            #endregion

            if (!Prgs.CheckExistsOrder_QtyShip_Detail(MyUtility.Convert.GetString(this.CurrentMaintain["ID"])))
            {
                return;
            }

            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    // PackingList狀態改為Confirm
                    this.sqlCmd = string.Format("update PackingList set Status = 'Confirmed', EditName = '{0}', EditDate = GETDATE() where ID = '{1}'", Env.User.UserID, this.CurrentMaintain["ID"].ToString());
                    this.result = DBProxy.Current.Execute(null, this.sqlCmd);
                    if (!this.result)
                    {
                        transactionScope.Dispose();
                        MyUtility.Msg.WarningBox("Confirm fail !\r\n" + this.result.ToString());
                        return;
                    }

                    #region 修改GMTBooking

                    if (!MyUtility.Check.Empty(this.CurrentMaintain["INVNo"]))
                    {
                        // 取得其他PackingList的CBM，用以加總
                        this.sqlCmd = string.Format(
                                            @"select isnull(sum(CBM),0) as CBM from PackingList WITH (NOLOCK) where INVNo = '{0}' and ID != '{1}'",
                                            this.CurrentMaintain["INVNo"].ToString(),
                                            this.CurrentMaintain["ID"].ToString());

                        DataTable summaryData;

                        if (this.result = DBProxy.Current.Select(null, this.sqlCmd, out summaryData))
                        {
                            string updateCmd = @"update GMTBooking
                                        set  TotalCBM = @ttlCBM
                                        where ID = @INVNo";

                            System.Data.SqlClient.SqlParameter ttlCBM = new System.Data.SqlClient.SqlParameter();

                            ttlCBM.ParameterName = "@ttlCBM";
                            ttlCBM.Value = MyUtility.Convert.GetDouble(summaryData.Rows[0]["CBM"]) + MyUtility.Convert.GetDouble(this.CurrentMaintain["CBM"].ToString());

                            System.Data.SqlClient.SqlParameter iNVNo = new System.Data.SqlClient.SqlParameter();
                            iNVNo.ParameterName = "@INVNo";
                            iNVNo.Value = this.CurrentMaintain["INVNo"].ToString();

                            IList<System.Data.SqlClient.SqlParameter> parameters = new List<System.Data.SqlClient.SqlParameter>();
                            parameters.Add(ttlCBM);
                            parameters.Add(iNVNo);
                            this.result = DBProxy.Current.Execute(null, updateCmd, parameters);
                            if (!this.result)
                            {
                                transactionScope.Dispose();
                                DualResult failResult = new DualResult(false, "Update Garment Booking fail!\r\n" + this.result.ToString());
                                return;
                            }
                        }
                        else
                        {
                            transactionScope.Dispose();
                            DualResult failResult = new DualResult(false, "Select PackingList Summary fail!\r\n" + this.result.ToString());
                            return;
                        }
                    }
                    else
                    {
                        transactionScope.Dispose();
                        DualResult failResult = new DualResult(false, "Select PackingList fail!\r\n" + this.result.ToString());
                        return;
                    }

                    #endregion

                    transactionScope.Complete();
                    transactionScope.Dispose();
                }
                catch (Exception ex)
                {
                    transactionScope.Dispose();
                    this.ShowErr("Commit transaction error.", ex);
                    return;
                }
            }
        }

        /// <summary>
        /// ClickUnconfirm
        /// </summary>
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            if (!MyUtility.Check.Empty(this.CurrentMaintain["INVNo"]))
            {
                if (MyUtility.GetValue.Lookup("Status", this.CurrentMaintain["INVNo"].ToString(), "GMTBooking", "ID") == "Confirmed")
                {
                    MyUtility.Msg.WarningBox(string.Format("Garment booking already confirmed, so can't unconfirm! \n pullout report:{0}", this.CurrentMaintain["INVNo"].ToString()));
                    return;
                }
            }

            DataTable dtt;
            string chkpullout = string.Format(@"select p.Status, pl.PulloutId  from Pullout p,packinglist pl where pl.PulloutId = p.ID and pl.id='{0}'", this.CurrentMaintain["id"].ToString());
            DualResult dr = DBProxy.Current.Select(null, chkpullout, out dtt);
            if (!dr)
            {
                MyUtility.Msg.ErrorBox("Sql command Error!");
                return;
            }

            if (dtt.Rows.Count > 0)
            {
                if (dtt.Rows[0]["Status"].ToString().ToUpper() != "NEW")
                {
                    MyUtility.Msg.WarningBox(string.Format(
                        @"Pullout already confirmed, so can't unconfirm!
Pullout No. < {0} > ", dtt.Rows[0]["PulloutId"].ToString()));
                    return;
                }
            }

            // 問是否要做Unconfirm，確定才繼續往下做
            this.buttonResult = MyUtility.Msg.WarningBox("Are you sure you want to < Unconfirm > this data?", "Warning", MessageBoxButtons.YesNo);
            if (this.buttonResult == DialogResult.No)
            {
                return;
            }

            this.sqlCmd = string.Format("update PackingList set Status = 'New', EditName = '{0}', EditDate = GETDATE() where ID = '{1}'", Env.User.UserID, this.CurrentMaintain["ID"].ToString());

            this.result = DBProxy.Current.Execute(null, this.sqlCmd);
            if (!this.result)
            {
                MyUtility.Msg.WarningBox("UnConfirm failed, Pleaes re-try");
            }
        }

        // Download excel format
        private void BtnDownloadExcelFormat_Click(object sender, EventArgs e)
        {
            string strXltName = Env.Cfg.XltPathDir + "\\Packing_P04_ImportExcelFormat.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return;
            }

            excel.Visible = true;
        }

        // Import from excel
        private void BtnImportFromExcel_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["BrandID"]))
            {
                MyUtility.Msg.WarningBox("Please input < Brand >!!");
                return;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["ShipModeID"]))
            {
                MyUtility.Msg.WarningBox("Ship Mode can't empty.");
                return;
            }

            P04_ExcelImport callNextForm = new P04_ExcelImport((DataTable)this.detailgridbs.DataSource, this.CurrentMaintain["BrandID"].ToString(), this.CurrentMaintain["ShipModeID"].ToString());
            callNextForm.ShowDialog(this);
        }

        private void Txtcustcd_Validated(object sender, EventArgs e)
        {
            if (this.EditMode && !MyUtility.Check.Empty(this.txtcustcd.Text) && this.txtcustcd.OldValue != this.txtcustcd.Text)
            {
                this.CurrentMaintain["Dest"] = MyUtility.GetValue.Lookup(string.Format("SELECT CountryID FROM CustCD WITH (NOLOCK) WHERE BrandID = '{0}' AND ID = '{1}'", MyUtility.Convert.GetString(this.CurrentMaintain["BrandID"]), this.txtcustcd.Text));
            }
        }

        private void ComputeOrderQty()
        {
            int needPackQty = 0, ttlShipQty = 0;
            DataTable needPackData, tmpPackData;
            DualResult selectResult;
            DataRow[] detailData;

            // 準備needPackData的Schema
            this.sqlCmd = "select OrderID, OrderShipmodeSeq, Article, SizeCode, ShipQty as Qty from PackingList_Detail WITH (NOLOCK) where ID = ''";
            if (!(selectResult = DBProxy.Current.Select(null, this.sqlCmd, out needPackData)))
            {
                MyUtility.Msg.WarningBox("Query  schema fail!");
            }

            foreach (DataRow dr in this.DetailDatas)
            {
                #region 重算表身Grid的Bal. Qty

                // 目前還有多少衣服尚未裝箱
                this.filter = string.Format("OrderID = '{0}' and OrderShipmodeSeq = '{1}'", dr["OrderID"].ToString(), dr["OrderShipmodeSeq"].ToString());
                detailData = needPackData.Select(this.filter);

                if (detailData.Length <= 0)
                {
                    // 撈取此SP+Seq尚未裝箱的數量
                    this.sqlCmd = string.Format(
                        @"
select oqd.Id as OrderID
       , oqd.Seq as OrderShipmodeSeq
       , oqd.Article
       , oqd.SizeCode
       , Qty = oqd.Qty 
from Order_QtyShip_Detail oqd WITH (NOLOCK) 
where oqd.Id = '{0}'
      and oqd.Seq = '{1}'", dr["OrderID"].ToString(),
                        dr["OrderShipmodeSeq"].ToString());
                    if (!(selectResult = DBProxy.Current.Select(null, this.sqlCmd, out tmpPackData)))
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
                this.filter = string.Format("OrderID = '{0}' and OrderShipmodeSeq = '{1}' and Article = '{2}' and SizeCode = '{3}'", dr["OrderID"].ToString(), dr["OrderShipmodeSeq"].ToString(), dr["Article"].ToString(), dr["SizeCode"].ToString());
                detailData = needPackData.Select(this.filter);
                if (detailData.Length > 0)
                {
                    needPackQty = MyUtility.Convert.GetInt(detailData[0]["Qty"]);
                }

                // 加總表身特定Article/SizeCode的Ship Qty數量
                detailData = ((DataTable)this.detailgridbs.DataSource).Select(this.filter);
                ttlShipQty = 0;
                if (detailData.Length != 0)
                {
                    foreach (DataRow dDr in detailData)
                    {
                        ttlShipQty = ttlShipQty + MyUtility.Convert.GetInt(dDr["ShipQty"]);
                    }
                }

                string strOtherConfirmSQL = string.Format(
                    @"
select Qty = isnull (sum (pld.ShipQty), 0)
from Packinglist pl
inner join Packinglist_detail pld on pl.ID = pld.ID
where pl.ID != '{0}'
      and pl.Status = 'Confirmed'
      and pld.OrderID = '{1}'
      and pld.OrderShipmodeSeq = '{2}'
      and pld.Article = '{3}'
      and pld.SizeCode = '{4}'", dr["ID"],
                    dr["OrderID"],
                    dr["OrderShipmodeSeq"],
                    dr["Article"],
                    dr["SizeCode"]);

                string strInvAdjustSQL = string.Format(
                    @"
select Qty = isnull (sum (InvAQ.DiffQty), 0)
from InvAdjust InvA
inner join  InvAdjust_Qty InvAQ WITH (NOLOCK) on InvA.ID = InvAQ.ID
where InvA.OrderID = '{0}'
      and InvA.OrderShipmodeSeq = '{1}'
      and InvAQ.Article = '{2}'
      and InvAQ.SizeCode = '{3}'", dr["OrderID"],
                    dr["OrderShipmodeSeq"],
                    dr["Article"],
                    dr["SizeCode"]);

                dr["OtherConfirmQty"] = MyUtility.GetValue.Lookup(strOtherConfirmSQL);
                dr["InvAdjustQty"] = MyUtility.GetValue.Lookup(strInvAdjustSQL);
                dr["BalanceQty"] = needPackQty - Convert.ToInt32(dr["OtherConfirmQty"]) - Convert.ToInt32(dr["InvAdjustQty"]) - ttlShipQty;
                dr["OrderQty"] = needPackQty;
                dr.EndEdit();
                #endregion
            }
        }

        private void Color_Change()
        {
            if (this.detailgrid.Rows.Count > 0 || !MyUtility.Check.Empty(this.detailgrid))
            {
                for (int index = 0; index < this.detailgrid.Rows.Count; index++)
                {
                    DataRow dr = this.detailgrid.GetDataRow(index);
                    if (this.detailgrid.Rows.Count <= index || index < 0)
                    {
                        return;
                    }

                    if (!MyUtility.Check.Empty(dr["DisposeFromClog"]))
                    {
                        this.detailgrid.Rows[index].DefaultCellStyle.BackColor = Color.FromArgb(190, 190, 190);
                    }
                    else
                    {
                        this.detailgrid.Rows[index].DefaultCellStyle.BackColor = Color.White;
                    }

                    if (MyUtility.Convert.GetDecimal(dr["BalanceQty"]) < 0)
                    {
                        this.detailgrid.Rows[index].Cells[15].Style.BackColor = Color.Red;
                    }
                }
            }
        }
    }
}
