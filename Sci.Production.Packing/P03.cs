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
    public partial class P03 : Sci.Win.Tems.Input6
    {
        Ict.Win.UI.DataGridViewTextBoxColumn col_ctnno;
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
        Ict.Win.DataGridViewGeneratorTextColumnSettings article = new Ict.Win.DataGridViewGeneratorTextColumnSettings();
        Ict.Win.DataGridViewGeneratorTextColumnSettings size = new Ict.Win.DataGridViewGeneratorTextColumnSettings();
        IList<string> comboBox1_RowSource = new List<string>();
        BindingSource comboxbs1;
        private MessageBoxButtons buttons = MessageBoxButtons.YesNo;
        private DialogResult buttonResult;
        private DualResult result;

        public P03(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.DefaultFilter = "FactoryID = '" + Sci.Env.User.Factory + "' AND Type = 'B'";
            detailgrid.AllowUserToOrderColumns = true;
            InsertDetailGridOnDoubleClick = false;
        }

        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["ID"].ToString();
            string OrderID = (e.Master == null) ? "" : e.Master["OrderID"].ToString();
            string OrderSeqID = (e.Master == null) ? "" : e.Master["OrderShipmodeSeq"].ToString();
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
select a.ID,a.OrderID,a.OrderShipmodeSeq,a.CTNStartNo,a.CTNQty, a.RefNo, a.Article, a.Color, a.SizeCode, a.QtyPerCTN, a.ShipQty, a.NW, a.GW, a.NNW, a.NWPerPcs, a.ScanQty,a.TransferToClogID, a.TransferDate, a.ReceiveDate, a.ClogLocationId, a.ReturnDate, b.Description, oqd.Qty-isnull(pd.TtlShipQty,0)+isnull(paq.TtlDiffQty,0)-pk.ShipQty as BalanceQty, a.Seq
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

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            comboBox1_RowSource.Add("");
            comboBox1_RowSource.Add("Transfer Clog");
            comboBox1_RowSource.Add("Clog Cfm");
            comboBox1_RowSource.Add("Location No");
            comboBox1_RowSource.Add("ColorWay");
            comboBox1_RowSource.Add("Color");
            comboBox1_RowSource.Add("Size");
            comboxbs1 = new BindingSource(comboBox1_RowSource, null);
            comboBox1.DataSource = comboxbs1;
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();

            //Shipping Lock是否可看見
            label23.Visible = MyUtility.Check.Empty(CurrentMaintain["GMTBookingLock"]) ? false : true;

            //Purchase Ctn
            displayBox7.Value = MyUtility.Check.Empty(CurrentMaintain["LocalPOID"]) ? "" : "Y";

            //Carton Dimension按鈕變色
            if (MyUtility.Check.Seek(CurrentMaintain["OrderID"].ToString(), "Order_CTNData", "ID"))
            {
                this.button1.ForeColor = Color.Blue;
            }
            else
            {
                this.button1.ForeColor = Color.Black;
            }

            //UnConfirm History按鈕變色
            if (MyUtility.Check.Seek(CurrentMaintain["ID"].ToString(), "PackingList_History", "ID"))
            {
                this.button3.ForeColor = Color.Blue;
            }
            else
            {
                this.button3.ForeColor = Color.Black;
            }

            //Start Ctn#
            string sqlCmd;
            DataRow orderData;
            sqlCmd = string.Format("select min(CTNStartNo) as CTNStartNo  from PackingList_Detail where ID = '{0}'", CurrentMaintain["ID"].ToString());
            if (MyUtility.Check.Seek(sqlCmd, out orderData))
            {
                displayBox6.Value = orderData["CTNStartNo"].ToString();
            }

            //帶出Orders相關欄位
            sqlCmd = string.Format("select StyleID,SeasonID,CustPONo,Customize1,ReadyDate from Orders where ID = '{0}'", CurrentMaintain["OrderID"].ToString());
            if (MyUtility.Check.Seek(sqlCmd, out orderData))
            {
                displayBox2.Value = orderData["StyleID"].ToString();
                displayBox3.Value = orderData["SeasonID"].ToString();
                displayBox4.Value = orderData["Customize1"].ToString();
                displayBox5.Value = orderData["CustPONo"].ToString();
                if (MyUtility.Check.Empty(orderData["ReadyDate"]))
                {
                    dateBox5.Value = null;
                }
                else
                {
                    dateBox5.Value = Convert.ToDateTime(orderData["ReadyDate"].ToString());
                }
                sqlCmd = string.Format("select BuyerDelivery from Order_QtyShip where ID = '{0}' and Seq = '{1}'", CurrentMaintain["OrderID"].ToString(), CurrentMaintain["OrderShipmodeSeq"].ToString());
                if (MyUtility.Check.Seek(sqlCmd, out orderData))
                {
                    dateBox1.Value = Convert.ToDateTime(orderData["BuyerDelivery"].ToString());
                }
            }

            //送交Clog時間是否有超過Ready Date
            if (MyUtility.Check.Empty(dateBox5.Value))
            {
                displayBox10.Value = "Fail";
            }
            else
            {
                sqlCmd = string.Format(@"select ReceiveDate
                                                            from PackingList_Detail
                                                          where ID = '{0}'
                                                            and CTNQty = 1
                                                            and (ReceiveDate is null or ReceiveDate > '{1}')", CurrentMaintain["ID"].ToString(), Convert.ToDateTime(dateBox5.Value.ToString()).ToString("d"));
                displayBox10.Value = MyUtility.Check.Seek(sqlCmd) ? "Fail" : "Pass";
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
                            dr["Color"] = "";
                            dr["SizeCode"] = "";
                        }
                        else
                        {
                            dr["Article"] = e.FormattedValue.ToString().ToUpper();
                            string sqlCmd = string.Format(@"select ColorID 
                                                                                  from V_OrderFAColor 
                                                                                  where ID = '{0}' and Article = '{1}'", CurrentMaintain["OrderID"].ToString(), dr["Article"]);
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
                .Numeric("NWPerPcs", header: "N.W./Pcs", integer_places: 2, decimal_places: 3, maximum: 99.999M, minimum: 0).Get(out col_nwpcs)
                .Numeric("ScanQty", header: "PC/Ctn Scanned", iseditingreadonly: true)
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

                    if (detailgrid.Columns[e.ColumnIndex].DataPropertyName == col_qtyperctn.DataPropertyName)
                    {
                        if (!MyUtility.Check.Empty(e.FormattedValue))
                        {
                            if (e.FormattedValue.ToString() != dr["QtyPerCTN"].ToString())
                            {
                                if (!CheckCanCahngeCol(dr["TransferToClogID"].ToString()))
                                {
                                    dr["QtyPerCTN"] = dr["QtyPerCTN"].ToString();
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

        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            txtshipmode1.ReadOnly = false;
            CurrentMaintain["FactoryID"] = Sci.Env.User.Factory;
            CurrentMaintain["Type"] = "B";
            CurrentMaintain["Status"] = "New";
        }

        protected override bool ClickEditBefore()
        {
            if (CurrentMaintain["Status"].ToString() == "Confirmed")
            {
                MyUtility.Msg.WarningBox("This record is < Confirmed >, can't be modified!");
                return false;
            }

            if (MyUtility.GetValue.Lookup("FtyGroup", CurrentMaintain["OrderID"].ToString(), "Orders", "ID") != Sci.Env.User.Factory)
            {
                MyUtility.Msg.WarningBox("Factory of this SP# is not equal to login factory, can't be modified!");
                return false;
            }
            return base.ClickEditBefore();
        }

        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            comboBox1.Text = "";

            //部分欄位會依某些條件來決定是否可以被修改
            if (!MyUtility.Check.Empty(CurrentMaintain["GMTBookingLock"]))
            {
                textBox1.ReadOnly = true;
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
                dateBox3.ReadOnly = true;
                dateBox4.ReadOnly = true;
            }

            //當此張PackingList是由PackingGuide轉過來的：SP#不可以被修改
            if (CurrentMaintain["ID"].ToString().IndexOf("PG") > 0)
            {
                textBox1.ReadOnly = true;
            }

            //當表身有任何一個箱子被送到Clog：SP#不可以被修改
            DataRow[] detailData = ((DataTable)detailgridbs.DataSource).Select("ClogReceiveID <> ''");
            if (detailData.Length != 0)
            {
                textBox1.ReadOnly = true;
            }
        }

        protected override bool ClickSaveBefore()
        {
            //檢查欄位值不可為空
            if (MyUtility.Check.Empty(CurrentMaintain["OrderID"]))
            {
                MyUtility.Msg.WarningBox("SP# can't empty!!");
                textBox1.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["ShipModeID"]))
            {
                MyUtility.Msg.WarningBox("Ship Mode can't empty!!");
                txtshipmode1.Focus();
                return false;
            }

            //檢查OrderID+Seq不可以重複建立
            if (MyUtility.Check.Seek(string.Format("select ID from PackingList where OrderID = '{0}' AND OrderShipmodeSeq = '{1}' AND ID != '{2}'", CurrentMaintain["OrderID"].ToString(), CurrentMaintain["OrderShipmodeSeq"].ToString(), IsDetailInserting ? "" : CurrentMaintain["ID"].ToString())))
            {
                MyUtility.Msg.WarningBox("SP No:" + CurrentMaintain["OrderID"].ToString() + ", Seq:" + CurrentMaintain["OrderShipmodeSeq"].ToString() + " already exist in packing list, can't be create again!");
                return false;
            }


            //表身的CTN#, Ref No., Color Way與Size不可以為空值，順便填入OrderID, OrderShipmodeSeq與Seq欄位值，計算CTNQty, ShipQty, NW, GW, NNW, CBM，重算表身Grid的Bal. Qty
            int i = 0, ctnQty = 0, shipQty = 0, ttlShipQty = 0, needPackQty = 0, count = 0;
            double nw = 0.0, gw = 0.0, nnw = 0.0, cbm = 0.0;
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

                if (MyUtility.Check.Empty(dr["RefNo"]))
                {
                    MyUtility.Msg.WarningBox("< Ref No. >  can't empty!");
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

                #region 填入OrderID, OrderShipmodeSeq與Seq欄位值
                i = i + 1;
                dr["OrderID"] = CurrentMaintain["OrderID"].ToString();
                dr["OrderShipmodeSeq"] = CurrentMaintain["OrderShipmodeSeq"].ToString();
                dr["Seq"] = Convert.ToString(i).PadLeft(6, '0');
                #endregion

                #region 計算CTNQty, ShipQty, NW, GW, NNW, CBM
                ctnQty = ctnQty + Convert.ToInt32(dr["CTNQty"].ToString());
                shipQty = shipQty + Convert.ToInt32(dr["ShipQty"].ToString());
                nw = MyUtility.Math.Round(nw +(MyUtility.Check.Empty(dr["NW"]) ? 0 : Convert.ToDouble(dr["NW"].ToString())), 3);
                gw = MyUtility.Math.Round(gw + (MyUtility.Check.Empty(dr["GW"]) ? 0 : Convert.ToDouble(dr["GW"].ToString())), 3);
                nnw = MyUtility.Math.Round(nnw + (MyUtility.Check.Empty(dr["NNW"]) ? 0 : Convert.ToDouble(dr["NNW"].ToString())), 3);
                if (MyUtility.Check.Empty(dr["CTNQty"]) || Convert.ToInt32(dr["CTNQty"].ToString()) > 0)
                {
                    cbm = MyUtility.Math.Round(cbm + (MyUtility.Math.Round(Convert.ToDouble(MyUtility.GetValue.Lookup("CBM", dr["RefNo"].ToString(), "LocalItem", "RefNo")), 3) * Convert.ToInt32(dr["CTNQty"].ToString())), 4);
                }
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

            //GetID
            if (IsDetailInserting)
            {
                string id = MyUtility.GetValue.GetID(Sci.Env.User.Keyword + "PL", "PackingList", DateTime.Today, 2, "Id", null);
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
                string sqlCmd = string.Format(@"select isnull(sum(ShipQty),0) as ShipQty,isnull(sum(CTNQty),0) as CTNQty,isnull(sum(NW),0) as NW,isnull(sum(GW),0) as GW,isnull(sum(NNW),0) as NNW,isnull(sum(CBM),0) as CBM
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
            //存檔成功後，要再呼叫UpdateOrdersCTN, CreateOrderCTNData
            bool prgResult = Prgs.UpdateOrdersCTN(CurrentMaintain["OrderID"].ToString());
            prgResult = Prgs.CreateOrderCTNData(CurrentMaintain["ID"].ToString());
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

        protected override void ClickDeleteAfter()
        {
            base.ClickDeleteAfter();
            bool prgResult = Prgs.UpdateOrdersCTN(CurrentMaintain["OrderID"].ToString());
        }

        //表身Grid的Delete
        protected override void OnDetailGridDelete()
        {
            //檢查此筆記錄是否已Transfer to Clog，若是則出訊息告知且無法刪除
            if (this.DetailDatas.Count > 0)
            {
                if (!MyUtility.Check.Empty(CurrentDetailData["TransferToClogID"]))
                {
                    MyUtility.Msg.WarningBox("This record had been send to CLOG, can't delete!!");
                    return;
                }
            }

            base.OnDetailGridDelete();
        }

        //控制表身Grid欄位是否可被編輯
        private void DetailGridEditing(bool isEditing)
        {
            if (isEditing)
            {
                col_ctnno.IsEditingReadOnly = false;
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
                    if (i == 0 || i == 1 || i == 2 || i == 4 || i == 6 || i == 7 || i == 8 || i == 10 || i == 11 || i == 12)
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
                col_ctnno.IsEditingReadOnly = true;
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
                    if (i != 13)
                    {
                        detailgrid.Columns[i].DefaultCellStyle.ForeColor = Color.Black;
                    }
                }
                col_refno.EditingMouseDown -= new EventHandler<Ict.Win.UI.DataGridViewEditingControlMouseEventArgs>(CartonRefnoCommon.EditingMouseDown);
            }
        }

        //檢查輸入的SP#是否正確
        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            if (EditMode)
            {
                if (textBox1.Text != textBox1.OldValue)
                {
                    bool returnData = false;
                    #region 檢查輸入的值是否符合條件
                    if (!MyUtility.Check.Empty(textBox1.Text))
                    {
                        DataRow orderData;
                        string sqlCmd = string.Format("select Category, LocalOrder, IsForecast from Orders where ID = '{0}' and FtyGroup = '{1}'", textBox1.Text, Sci.Env.User.Factory);
                        if (MyUtility.Check.Seek(sqlCmd, out orderData))
                        {
                            string msg = "";
                            //只能建立大貨單的資料
                            switch (orderData["Category"].ToString().Trim())
                            {
                                case "B":
                                    if (orderData["LocalOrder"].ToString() == "True")
                                    {
                                        msg = " is < Local order >, it can't be created!";
                                        returnData = true;
                                    }
                                    break;
                                case "M":
                                    msg = "category: < Material >, it can't be created!";
                                    returnData = true;
                                    break;
                                case "O":
                                    msg = "category: < Other >, it can't be created!";
                                    returnData = true;
                                    break;
                                case "S":
                                    msg = "category: < Sample>, it can't be created!";
                                    returnData = true;
                                    break;
                                default:
                                    if (orderData["IsForecast"].ToString() == "True")
                                    {
                                        msg = " is < Forecast >, it can't be created!";
                                        returnData = true;
                                    }
                                    break;
                            }
                            if (returnData)
                            {
                                MyUtility.Msg.WarningBox("SP#:" + textBox1.Text + msg);
                                //OrderID異動，其他相關欄位要跟著異動
                                ChangeOtherData("");
                                textBox1.Text = "";
                            }
                        }
                        else
                        {
                            returnData = true;
                            MyUtility.Msg.WarningBox("< SP# > does not exist!");
                            //OrderID異動，其他相關欄位要跟著異動
                            ChangeOtherData("");
                            textBox1.Text = "";
                        }
                    }
                    #endregion

                    if (returnData)
                    {
                        e.Cancel = true;
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
                CurrentMaintain["Dest"] = "";
                displayBox2.Value = "";
                displayBox3.Value = "";
                displayBox4.Value = "";
                displayBox5.Value = "";
                dateBox5.Value = null;
            }
            else
            {
                DataRow orderData;
                string sqlCmd;
                sqlCmd = string.Format("select StyleID,SeasonID,CustPONo,Customize1,ReadyDate,BrandID,CustCDID,Dest from Orders where ID = '{0}'", orderID);
                if (MyUtility.Check.Seek(sqlCmd, out orderData))
                {
                    //帶出相關欄位的資料
                    displayBox2.Value = orderData["StyleID"].ToString();
                    displayBox3.Value = orderData["SeasonID"].ToString();
                    displayBox4.Value = orderData["Customize1"].ToString();
                    displayBox5.Value = orderData["CustPONo"].ToString();
                    if (MyUtility.Check.Empty(orderData["ReadyDate"]))
                    {
                        dateBox5.Value = null;
                    }
                    else
                    {
                        dateBox5.Value = Convert.ToDateTime(orderData["ReadyDate"].ToString());
                    }
                    CurrentMaintain["BrandID"] = orderData["BrandID"].ToString();
                    CurrentMaintain["CustCDID"] = orderData["CustCDID"].ToString();
                    CurrentMaintain["Dest"] = orderData["Dest"].ToString();

                    #region 若Order_QtyShip有多筆資料話就跳出視窗讓使者選擇Seq
                    sqlCmd = string.Format("select count(ID) as CountID from Order_QtyShip where ID = '{0}'", orderID);
                    if (MyUtility.Check.Seek(sqlCmd, out orderData))
                    {
                        if (orderData["CountID"].ToString() == "1")
                        {
                            sqlCmd = string.Format("select ShipModeID,BuyerDelivery,Seq from Order_QtyShip where ID = '{0}'", orderID);
                            if (MyUtility.Check.Seek(sqlCmd, out orderData))
                            {
                                CurrentMaintain["OrderShipmodeSeq"] = orderData["Seq"].ToString();
                                CurrentMaintain["ShipModeID"] = orderData["ShipModeID"].ToString();
                                dateBox1.Value = Convert.ToDateTime(orderData["BuyerDelivery"].ToString());
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
                                dateBox1.Value = null;
                            }
                            else
                            {
                                orderQtyShipData = item.GetSelecteds();
                                CurrentMaintain["OrderShipmodeSeq"] = item.GetSelectedString();
                                CurrentMaintain["ShipModeID"] = orderQtyShipData[0]["ShipmodeID"].ToString();
                                dateBox1.Value = Convert.ToDateTime(orderQtyShipData[0]["BuyerDelivery"].ToString());
                            }
                        }
                    }
                    #endregion

                    //產生表身Grid的資料
                    GenDetailData(orderID, CurrentMaintain["OrderShipmodeSeq"].ToString());
                }
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
                string sqlCmd;
                sqlCmd = string.Format("select CtnType from Orders where ID = '{0}'", CurrentMaintain["OrderID"].ToString());
                if (MyUtility.GetValue.Lookup(sqlCmd) == "2")
                {
                    sqlCmd = string.Format("select * from Order_QtyCTN where Id = '{0}'", orderID);
                    if (!MyUtility.Check.Seek(sqlCmd))
                    {
                        MyUtility.Msg.WarningBox("No packing data, can't create!!");
                        return;
                    }
                    sqlCmd = string.Format(@"select o.ID as OrderID, oqd.Seq as OrderShipmodeSeq, oqd.Article, voc.ColorID as Color, oqd.SizeCode, oqd.Qty as ShipQty, oqc.Qty as QtyPerCTN
                                                                from Order_QtyShip_Detail oqd
                                                                left Join Orders o on o.ID = oqd.Id
                                                                left Join Order_QtyCTN oqc on oqc.id = oqd.Id and oqc.Article = oqd.Article and oqc.SizeCode = oqd.SizeCode
                                                                left join V_OrderFAColor voc on voc.id = oqd.Id and voc.Article = oqd.Article
                                                                left join Order_SizeCode os on os.id = o.POID and os.SizeCode = oqd.SizeCode
                                                                left join Order_Article oa on oa.id = oqd.Id and oa.Article = oqd.Article
                                                                where oqd.ID = '{0}' and oqd.Seq = '{1}'
                                                                order by oa.Seq,os.Seq", orderID, seq);
                }
                else
                {
                    sqlCmd = string.Format(@"select o.ID as OrderID, oqd.Seq as OrderShipmodeSeq, oqd.Article, voc.ColorID as Color, oqd.SizeCode, oqd.Qty as ShipQty, o.CTNQty as QtyPerCTN
                                                                from Order_QtyShip_Detail oqd
                                                                left Join Orders o on o.ID = oqd.Id
                                                                left join V_OrderFAColor voc on voc.id = oqd.Id and voc.Article = oqd.Article
                                                                left join Order_SizeCode os on os.id = o.POID and os.SizeCode = oqd.SizeCode
                                                                left join Order_Article oa on oa.id = oqd.Id and oa.Article = oqd.Article
                                                                where oqd.ID = '{0}' and oqd.Seq = '{1}'
                                                                order by oa.Seq,os.Seq", orderID, seq);
                }

                DataTable selectedData;
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

        //Seq按右鍵功能
        private void textBox2_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            if (MyUtility.Check.Empty(CurrentMaintain["GMTBookingLock"]))
            {
                IList<DataRow> orderQtyShipData;
                string sqlCmd = string.Format("select Seq, BuyerDelivery,ShipmodeID,Qty from Order_QtyShip where ID = '{0}'", CurrentMaintain["OrderID"].ToString());
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
                    dateBox1.Value = Convert.ToDateTime(orderQtyShipData[0]["BuyerDelivery"].ToString());
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

        //Sort by
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            label24.Visible = true;
            textBox3.Visible = false;
            dateBox6.Visible = false;
            button4.Visible = true;
            switch (comboBox1.SelectedValue.ToString())
            {
                case "Transfer Clog":
                    label24.Text = "Locate for Transfer Clog:";
                    label24.Width = 156;
                    dateBox6.Visible = true;
                    dateBox6.Location = new System.Drawing.Point(448, 222);
                    button4.Location = new System.Drawing.Point(591, 217);
                    ((DataTable)detailgridbs.DataSource).DefaultView.Sort = "TransferDate,Seq";
                    break;
                case "Clog Cfm":
                    label24.Text = "Locate for Clog Cfm:";
                    label24.Width = 129;
                    dateBox6.Visible = true;
                    dateBox6.Location = new System.Drawing.Point(420, 222);
                    button4.Location = new System.Drawing.Point(563, 217);
                    ((DataTable)detailgridbs.DataSource).DefaultView.Sort = "ReceiveDate,Seq";
                    break;
                case "Location No":
                    label24.Text = "Locate for Location No:";
                    label24.Width = 147;
                    textBox3.Visible = true;
                    textBox3.Location = new System.Drawing.Point(438, 222);
                    button4.Location = new System.Drawing.Point(537, 217);
                    ((DataTable)detailgridbs.DataSource).DefaultView.Sort = "ClogLocationId,Seq";
                    break;
                case "ColorWay":
                    label24.Text = "Locate for ColorWay:";
                    label24.Width = 135;
                    textBox3.Visible = true;
                    textBox3.Location = new System.Drawing.Point(426, 222);
                    button4.Location = new System.Drawing.Point(525, 217);
                    ((DataTable)detailgridbs.DataSource).DefaultView.Sort = "Article,Seq";
                    break;
                case "Color":
                    label24.Text = "Locate for Color:";
                    label24.Width = 106;
                    textBox3.Visible = true;
                    textBox3.Location = new System.Drawing.Point(397, 222);
                    button4.Location = new System.Drawing.Point(496, 217);
                    ((DataTable)detailgridbs.DataSource).DefaultView.Sort = "Color,Seq";
                    break;
                case "Size":
                    label24.Text = "Locate for Size:";
                    label24.Width = 100;
                    textBox3.Visible = true;
                    textBox3.Location = new System.Drawing.Point(391, 222);
                    button4.Location = new System.Drawing.Point(490, 217);
                    ((DataTable)detailgridbs.DataSource).DefaultView.Sort = "SizeCode,Seq";
                    break;
                default:
                    label24.Visible = false;
                    button4.Visible = false;
                    if (MyUtility.Check.Empty((DataTable)detailgridbs.DataSource)) break;
                    ((DataTable)detailgridbs.DataSource).DefaultView.Sort = "Seq";
                    break;
            }
        }

        //Carton Summary
        private void button1_Click(object sender, EventArgs e)
        {
            Sci.Production.Packing.P02_CartonSummary callNextForm = new Sci.Production.Packing.P02_CartonSummary(CurrentMaintain["OrderID"].ToString());
            callNextForm.ShowDialog(this);
        }

        //UnConfirm History
        private void button3_Click(object sender, EventArgs e)
        {
            Sci.Win.UI.ShowHistory callNextForm = new Sci.Win.UI.ShowHistory("PackingList_History", CurrentMaintain["ID"].ToString(), "Status", caption: "UnConfirm History", haveRemark: true, customerGridFormatTable: "HisType", moduleName: "PackingList");
            callNextForm.ShowDialog(this);
        }

        //Recalculate Weight
        private void button2_Click(object sender, EventArgs e)
        {
            //如果已經Shipping Lock的話就不可以再重算重量
            if (!MyUtility.Check.Empty(CurrentMaintain["GMTBookingLock"]))
            {
                MyUtility.Msg.WarningBox("This record already shipping lock, can't recalculate weight!");
                return;
            }

            //找出 StyleUkey
            string styleUKey = MyUtility.GetValue.Lookup("StyleUkey", CurrentMaintain["OrderID"].ToString(), "Orders", "ID");

            string message = "", sqlCmd = "", filter = "";
            DataTable weightData;
            DataRow[] weight;

            //先將屬於此訂單的Style_WeightData給撈出來
            sqlCmd = string.Format("select Article,SizeCode,NW,NNW from Style_WeightData where StyleUkey = '{0}'", styleUKey);
            if (!(result = DBProxy.Current.Select(null, sqlCmd, out weightData)))
            {
                MyUtility.Msg.WarningBox("Query weight data fail!");
                return;
            }

            //檢查是否所有的SizeCode都有存在Style_WeightData中
            foreach (DataRow dr in ((DataTable)detailgridbs.DataSource).Rows)
            {
                filter = string.Format("SizeCode = '{0}'", dr["SizeCode"].ToString());
                weight = weightData.Select(filter);
                if (weight.Length == 0)
                {
                    if (MyUtility.Check.Empty(message))
                    {
                        message = dr["SizeCode"].ToString();
                    }
                    else
                    {
                        if (message.IndexOf(dr["SizeCode"].ToString()) <= 0)
                        {
                            message = message + ',' + dr["SizeCode"].ToString();
                        }
                    }
                }
            }

            if (!MyUtility.Check.Empty(message))
            {
                buttonResult = MyUtility.Msg.WarningBox("Size: " + message + " not in Style basic data, are you sure you want to  recalculate weight?", "Warning", buttons);
                if (buttonResult == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }
            }

            double nw = 0, nnw = 0, ctnWeight = 0;
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
                    ctnWeight = MyUtility.Math.Round(Convert.ToDouble(MyUtility.GetValue.Lookup("Weight", dr["RefNo"].ToString(), "LocalItem", "RefNo")), 6);
                }

                filter = string.Format("Article = '{0}' and SizeCode = '{1}'", dr["Article"].ToString(), dr["SizeCode"].ToString());
                weight = weightData.Select(filter);
                if (weight.Length > 0)
                {
                    nw = nw + Convert.ToDouble(weight[0]["NW"]) * Convert.ToInt32(dr["ShipQty"]);
                    nnw = nnw + Convert.ToDouble(weight[0]["NNW"]) * Convert.ToInt32(dr["ShipQty"]);
                    dr["NWPerPcs"] = Convert.ToDouble(weight[0]["NW"]);
                }
                else
                {
                    filter = string.Format("SizeCode = '{0}'", dr["SizeCode"].ToString());
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

            //訂單工廠別與登入系統工廠別不一致時，不可以Confirm
            if (MyUtility.GetValue.Lookup("FtyGroup", CurrentMaintain["OrderID"].ToString(), "Orders", "ID") != Sci.Env.User.Factory)
            {
                MyUtility.Msg.WarningBox("This SP's factory not equal to login system factory so can't confirm!");
                return;
            }

            //還沒有Invoice No就不可以做Confirm
            if (MyUtility.Check.Empty(MyUtility.GetValue.Lookup("INVNo", CurrentMaintain["ID"].ToString(), "PackingList", "ID")))
            {
                MyUtility.Msg.WarningBox("Shipping is not yet booking so can't confirm!");
                return;
            }

            string sqlCmd = "", errMesg = "";
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
            if (!MyUtility.Check.Empty(CurrentMaintain["INVNo"]))
            {
                if (MyUtility.GetValue.Lookup("Status", CurrentMaintain["INVNo"].ToString(), "GMTBooking", "ID") == "Confirmed")
                {
                    MyUtility.Msg.WarningBox("Garment booking already confirmed, can't unconfirm! ");
                    return;
                }
            }

            //問是否要做Unconfirm，確定才繼續往下做
            buttonResult = MyUtility.Msg.WarningBox("Are you sure you want to < Unconfirm > this data?", "Warning", buttons);
            if (buttonResult == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            Sci.Win.UI.SelectReason callReason = new Sci.Win.UI.SelectReason();
            DialogResult dResult = callReason.ShowDialog(this);
            if (dResult == System.Windows.Forms.DialogResult.OK)
            {
                string reasonRemark = callReason.ReturnRemark;
                if (MyUtility.Check.Empty(reasonRemark))
                {
                    return;
                }
                else
                {
                    using (TransactionScope transactionScope = new TransactionScope())
                    {
                        try
                        {
                            string InsertCmd = @"insert into PackingList_History (ID, HisType, OldValue, NewValue, Remark, AddName, AddDate)
 values (@id,@hisType,@oldValue,@newValue,@remark,@addName,@addDate)";
                            string updateCmd = @"update PackingList set Status = 'New', EditName = @addName, EditDate = @addDate where ID = @id";

                            #region 準備sql參數資料
                            System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter();
                            sp1.ParameterName = "@id";
                            sp1.Value = CurrentMaintain["ID"].ToString();

                            System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter();
                            sp2.ParameterName = "@hisType";
                            sp2.Value = "Status";

                            System.Data.SqlClient.SqlParameter sp3 = new System.Data.SqlClient.SqlParameter();
                            sp3.ParameterName = "@oldValue";
                            sp3.Value = "Confirmed";

                            System.Data.SqlClient.SqlParameter sp4 = new System.Data.SqlClient.SqlParameter();
                            sp4.ParameterName = "@newValue";
                            sp4.Value = "New";

                            System.Data.SqlClient.SqlParameter sp5 = new System.Data.SqlClient.SqlParameter();
                            sp5.ParameterName = "@remark";
                            sp5.Value = reasonRemark;

                            System.Data.SqlClient.SqlParameter sp6 = new System.Data.SqlClient.SqlParameter();
                            sp6.ParameterName = "@addName";
                            sp6.Value = Sci.Env.User.UserID;

                            System.Data.SqlClient.SqlParameter sp7 = new System.Data.SqlClient.SqlParameter();
                            sp7.ParameterName = "@addDate";
                            sp7.Value = DateTime.Now;

                            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                            cmds.Add(sp1);
                            cmds.Add(sp2);
                            cmds.Add(sp3);
                            cmds.Add(sp4);
                            cmds.Add(sp5);
                            cmds.Add(sp6);
                            cmds.Add(sp7);
                            #endregion

                            DualResult result, result2;
                            result = Sci.Data.DBProxy.Current.Execute(null, InsertCmd, cmds);
                            result2 = Sci.Data.DBProxy.Current.Execute(null, updateCmd, cmds);

                            if (result && result2)
                            {
                                transactionScope.Complete();
                            }
                            else
                            {
                                MyUtility.Msg.WarningBox("UnConfirm failed, Pleaes re-try");
                            }
                        }
                        catch (Exception ex)
                        {
                            ShowErr("Commit transaction error.", ex);
                            return;
                        }
                    }
                }
            }
            else
            {
                return;
            }

            RenewData();
            OnDetailEntered();
            EnsureToolbarExt();
        }

        //Find Now
        private void button4_Click(object sender, EventArgs e)
        {

            int index;
            switch (comboBox1.SelectedValue.ToString())
            {
                case "Transfer Clog":
                    index = detailgridbs.Find("TransferDate", dateBox6.Value.ToString());
                    break;
                case "Clog Cfm":
                    index = detailgridbs.Find("ReceiveDate", dateBox6.Value.ToString());
                    break;
                case "Location No":
                    index = detailgridbs.Find("ClogLocationId", textBox3.Text.Trim());
                    break;
                case "ColorWay":
                    index = detailgridbs.Find("Article", textBox3.Text.Trim());
                    break;
                case "Color":
                    index = detailgridbs.Find("Color", textBox3.Text.Trim());
                    break;
                case "Size":
                    index = detailgridbs.Find("SizeCode", textBox3.Text.Trim());
                    break;
                default:
                    index = -1;
                    break;
            }

            if (index == -1)
            { MyUtility.Msg.WarningBox("Data was not found!!"); }
            else
            { detailgridbs.Position = index; }
        }

        //ShipMode
        private void txtshipmode1_SelectionChangeCommitted(object sender, EventArgs e)
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
}
