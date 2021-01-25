using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;
using Sci.Production.PublicPrg;
using System.Threading.Tasks;
using Sci.Production.Automation;

namespace Sci.Production.Packing
{
    /// <summary>
    /// Packing_P06
    /// </summary>
    public partial class P06 : Win.Tems.Input6
    {
        private Ict.Win.UI.DataGridViewTextBoxColumn col_ctnno;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_article;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_size;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_refno;
        private Ict.Win.UI.DataGridViewNumericBoxColumn col_ctnqty;
        private Ict.Win.UI.DataGridViewNumericBoxColumn col_shipqty;
        private DataGridViewGeneratorTextColumnSettings article = new DataGridViewGeneratorTextColumnSettings();
        private DataGridViewGeneratorTextColumnSettings size = new DataGridViewGeneratorTextColumnSettings();

        private DualResult result;
        private DialogResult buttonResult;
        private DataTable selectedData;

        /// <summary>
        /// P06
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public P06(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = "MDivisionID = '" + Env.User.Keyword + "' AND Type = 'L'";
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
            string masterID = (e.Master == null) ? string.Empty : e.Master["ID"].ToString();
            this.DetailSelectCommand = Prgs.QueryPackingListSQLCmd(masterID);

            return base.OnDetailSelectCommandPrepare(e);
        }

        /// <summary>
        /// OnDetailEntered
        /// </summary>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();

            this.labelConfirmed.Visible = MyUtility.Check.Empty(this.CurrentMaintain["ID"].ToString()) ? false : true;

            DataRow dr1;
            string sqlStatus = string.Format(@"select status from PackingList WITH (NOLOCK) where id='{0}'", this.CurrentMaintain["ID"].ToString());
            if (MyUtility.Check.Seek(sqlStatus, out dr1))
            {
                this.labelConfirmed.Text = dr1["Status"].ToString();
            }

            // 帶出Orders相關欄位
            DataRow dr;
            string sqlCmd = string.Format("select StyleID,SeasonID,CustPONo,Customize1,ReadyDate from Orders WITH (NOLOCK) where ID = '{0}'", this.CurrentMaintain["OrderID"].ToString());
            if (MyUtility.Check.Seek(sqlCmd, out dr))
            {
                this.displayStyle.Value = dr["StyleID"].ToString();
                this.displaySeason.Value = dr["SeasonID"].ToString();
                this.displayPONo.Value = dr["CustPONo"].ToString();
            }

            // Carton Summary按鈕變色
            string chksql = $@"
select 1
from PackingList pl WITH (NOLOCK) , PackingList_Detail pd WITH (NOLOCK)
left join LocalItem li WITH (NOLOCK) on li.RefNo = pd.RefNo
left join LocalSupp ls WITH (NOLOCK) on ls.ID = li.LocalSuppid
where pl.Type = 'L'
and pd.ID = pl.ID
and pd.ID = '{this.CurrentMaintain["ID"]}'
";
            if (MyUtility.Check.Seek(chksql))
            {
                this.btnCartonSummary.ForeColor = Color.Blue;
            }
            else
            {
                this.btnCartonSummary.ForeColor = Color.Black;
            }

            this.Color_Change();
        }

        /// <summary>
        /// OnDetailGridSetup
        /// </summary>
        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();

            #region Article & SizeCode按右鍵與Validating
            this.article.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode)
                {
                    if (e.Button == MouseButtons.Right)
                    {
                        if (e.RowIndex != -1)
                        {
                            DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                            string sqlCmd = string.Format("Select Distinct Article from Order_QtyShip_Detail WITH (NOLOCK) where ID = '{0}' and Seq = '{1}'", this.CurrentMaintain["OrderID"].ToString(), this.CurrentMaintain["OrderShipmodeSeq"].ToString());
                            Win.Tools.SelectItem item = new Win.Tools.SelectItem(sqlCmd, "8", dr["Article"].ToString());
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

                    // 檢查箱子如果有送到Clog則不可以被修改
                    if (e.FormattedValue.ToString() != dr["Article"].ToString())
                    {
                        if (!this.CheckCanCahngeCol(MyUtility.Convert.GetDate(dr["TransferDate"])))
                        {
                            dr["Article"] = dr["Article"].ToString();
                            e.Cancel = true;
                            return;
                        }
                    }

                    if (!MyUtility.Check.Empty(e.FormattedValue) && e.FormattedValue.ToString() != dr["Article"].ToString())
                    {
                        // sql參數
                        System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter("@orderid", MyUtility.Convert.GetString(this.CurrentMaintain["OrderID"]));
                        System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter("@seq", MyUtility.Convert.GetString(this.CurrentMaintain["OrderShipmodeSeq"]));
                        System.Data.SqlClient.SqlParameter sp3 = new System.Data.SqlClient.SqlParameter("@article", e.FormattedValue.ToString());

                        IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                        cmds.Add(sp1);
                        cmds.Add(sp2);
                        cmds.Add(sp3);

                        DataTable qrderQty;
                        string sqlCmd = "Select Article from Order_QtyShip_Detail WITH (NOLOCK) where ID = @orderid and Seq = @seq and Article = @article";
                        DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out qrderQty);
                        if (!result)
                        {
                            dr["Article"] = string.Empty;
                            dr["SizeCode"] = string.Empty;
                            dr.EndEdit();
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox("Sql connection fail!!\r\n" + result.ToString());
                            return;
                        }
                        else
                        {
                            if (qrderQty.Rows.Count <= 0)
                            {
                                dr["Article"] = string.Empty;
                                dr["SizeCode"] = string.Empty;
                                dr.EndEdit();
                                e.Cancel = true;
                                MyUtility.Msg.WarningBox(string.Format("< Article: {0} > not found!!!", e.FormattedValue.ToString()));
                                return;
                            }
                            else
                            {
                                dr["Article"] = e.FormattedValue.ToString();
                                dr["SizeCode"] = string.Empty;
                                dr.EndEdit();
                            }
                        }
                    }
                }
            };

            this.size.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode)
                {
                    if (e.Button == MouseButtons.Right)
                    {
                        if (e.RowIndex != -1)
                        {
                            DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                            string sqlCmd = string.Format(
                                @"Select oqd.SizeCode 
from Order_QtyShip_Detail oqd WITH (NOLOCK) 
left join Orders o WITH (NOLOCK) on o.ID = oqd.Id
left join Order_SizeCode os WITH (NOLOCK) on os.ID = o.POID and os.SizeCode = oqd.SizeCode
where oqd.ID = '{0}' and oqd.Seq = '{1}' and oqd.Article = '{2}' 
order by os.Seq",
                                this.CurrentMaintain["OrderID"].ToString(),
                                this.CurrentMaintain["OrderShipmodeSeq"].ToString(),
                                dr["Article"].ToString());
                            Win.Tools.SelectItem item = new Win.Tools.SelectItem(sqlCmd, "8", dr["SizeCode"].ToString());
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

                    // 檢查箱子如果有送到Clog則不可以被修改
                    if (e.FormattedValue.ToString() != dr["SizeCode"].ToString())
                    {
                        if (!this.CheckCanCahngeCol(MyUtility.Convert.GetDate(dr["TransferDate"])))
                        {
                            dr["SizeCode"] = dr["SizeCode"].ToString();
                            e.Cancel = true;
                            return;
                        }
                    }

                    if (!MyUtility.Check.Empty(e.FormattedValue) && e.FormattedValue.ToString() != dr["SizeCode"].ToString())
                    {
                        // sql參數
                        System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter("@orderid", MyUtility.Convert.GetString(this.CurrentMaintain["OrderID"]));
                        System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter("@seq", MyUtility.Convert.GetString(this.CurrentMaintain["OrderShipmodeSeq"]));
                        System.Data.SqlClient.SqlParameter sp3 = new System.Data.SqlClient.SqlParameter("@article", MyUtility.Convert.GetString(dr["Article"]));
                        System.Data.SqlClient.SqlParameter sp4 = new System.Data.SqlClient.SqlParameter("@sizecode", e.FormattedValue.ToString());

                        IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                        cmds.Add(sp1);
                        cmds.Add(sp2);
                        cmds.Add(sp3);
                        cmds.Add(sp4);

                        DataTable qrderQty1;
                        string sqlCmd = "Select SizeCode from Order_QtyShip_Detail WITH (NOLOCK) where ID = @orderid and Seq = @seq and Article = @article and SizeCode = @sizecode";
                        DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out qrderQty1);
                        if (!result)
                        {
                            dr["SizeCode"] = string.Empty;
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox("Sql connection fail!!\r\n" + result.ToString());
                            return;
                        }
                        else
                        {
                            if (qrderQty1.Rows.Count <= 0)
                            {
                                dr["SizeCode"] = string.Empty;
                                e.Cancel = true;
                                MyUtility.Msg.WarningBox(string.Format("< SizeCode: {0} > not found!!!", e.FormattedValue.ToString()));
                                return;
                            }
                        }
                    }
                }
            };
            #endregion

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
                        string seekSql = string.Format("select Description,CtnWeight from LocalItem WITH (NOLOCK) where RefNo = '{0}'", dr["RefNo"].ToString());
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
            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("CTNStartNo", header: "CTN#", width: Widths.AnsiChars(6)).Get(out this.col_ctnno)
                .Numeric("CTNQty", header: "# of CTN").Get(out this.col_ctnqty)
                .CellCartonItem("RefNo", header: "Ref No.", width: Widths.AnsiChars(13)).Get(out this.col_refno)
                .Text("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("Article", header: "ColorWay", width: Widths.AnsiChars(8), settings: this.article).Get(out this.col_article)
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(8), settings: this.size).Get(out this.col_size)
                .Numeric("ShipQty", header: "Qty").Get(out this.col_shipqty)
                .Numeric("BalanceQty", header: "Bal. Qty", iseditingreadonly: true)
                .Date("TransferDate", header: "Transfer CLOG", iseditingreadonly: true)
                .Date("ReceiveDate", header: "CLOG CFM", iseditingreadonly: true)
                .Text("ClogLocationId", header: "Clog Location", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Date("ReturnDate", header: "Return Date", iseditingreadonly: true)
                .Text("CFALocationID", header: "CFA Location No.", width: Widths.AnsiChars(10), iseditingreadonly: true)
                ;

            #region 欄位的Validating
            this.detailgrid.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    #region 檢查箱子如果有送到Clog則不可以被修改
                    if (this.detailgrid.Columns[e.ColumnIndex].DataPropertyName == this.col_ctnno.DataPropertyName)
                    {
                        if (!MyUtility.Check.Empty(e.FormattedValue))
                        {
                            if (e.FormattedValue.ToString() != dr["CTNStartNo"].ToString())
                            {
                                if (!this.CheckCanCahngeCol(MyUtility.Convert.GetDate(dr["TransferDate"])))
                                {
                                    dr["CTNStartNo"] = dr["CTNStartNo"].ToString();
                                    e.Cancel = true;
                                    return;
                                }
                            }
                        }
                    }

                    if (this.detailgrid.Columns[e.ColumnIndex].DataPropertyName == this.col_ctnqty.DataPropertyName)
                    {
                        if (!MyUtility.Check.Empty(e.FormattedValue))
                        {
                            if (e.FormattedValue.ToString() != dr["CTNQty"].ToString())
                            {
                                if (!this.CheckCanCahngeCol(MyUtility.Convert.GetDate(dr["TransferDate"])))
                                {
                                    dr["CTNQty"] = dr["CTNQty"].ToString();
                                    e.Cancel = true;
                                    return;
                                }
                            }
                        }
                    }

                    if (this.detailgrid.Columns[e.ColumnIndex].DataPropertyName == this.col_shipqty.DataPropertyName)
                    {
                        if (!MyUtility.Check.Empty(e.FormattedValue))
                        {
                            if (e.FormattedValue.ToString() != dr["ShipQty"].ToString())
                            {
                                if (!this.CheckCanCahngeCol(MyUtility.Convert.GetDate(dr["TransferDate"])))
                                {
                                    dr["ShipQty"] = dr["ShipQty"].ToString();
                                    e.Cancel = true;
                                    return;
                                }
                            }
                        }
                    }
                    #endregion

                    if (this.detailgrid.Columns[e.ColumnIndex].DataPropertyName == this.col_ctnqty.DataPropertyName)
                    {
                        if (!MyUtility.Check.Empty(e.FormattedValue))
                        {
                            if (e.FormattedValue.ToString() != dr["CTNQty"].ToString())
                            {
                                if (e.FormattedValue.ToString() != "0" && e.FormattedValue.ToString() != "1")
                                {
                                    dr["CTNQty"] = 0;
                                    e.Cancel = true;
                                    MyUtility.Msg.WarningBox("# of CTN only keyin 1 or 0");
                                    return;
                                }
                            }
                        }
                    }
                }
            };

            #endregion
        }

        /// <summary>
        /// ClickNewAfter
        /// </summary>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["MDivisionID"] = Env.User.Keyword;
            this.CurrentMaintain["Type"] = "L";
            this.CurrentMaintain["Status"] = "New";
            this.CurrentMaintain["Dest"] = "ZZ";
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

            // 當表身有任何一個箱子被送到Clog：SP#不可以被修改
            DataRow[] detailData = ((DataTable)this.detailgridbs.DataSource).Select("ReceiveDate is null");
            if (detailData.Length != 0)
            {
                this.txtSP.ReadOnly = true;
            }
        }

        /// <summary>
        /// ClickSaveBefore
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ClickSaveBefore()
        {
            if (!MyUtility.Check.Empty(this.CurrentMaintain["PulloutDate"]))
            {
                // Pullout date不可小於System的Pullout lock date
                string pullLock = MyUtility.GetValue.Lookup("select PullLock from System WITH (NOLOCK) ");
                if (MyUtility.Convert.GetDate(this.CurrentMaintain["PulloutDate"]) < MyUtility.Convert.GetDate(pullLock))
                {
                    this.datePullOutDate.Focus();
                    MyUtility.Msg.WarningBox("Pullout date less then pullout lock date!!");
                    return false;
                }

                // 如果Pullout report已存在且狀態為Confirmed時，需出訊息告知
                DataRow dr;
                if (MyUtility.Check.Seek(string.Format("select ID,status from Pullout WITH (NOLOCK) where PulloutDate = '{0}' and MDivisionID = '{1}'", Convert.ToDateTime(this.CurrentMaintain["PulloutDate"].ToString()).ToString("d"), Env.User.Keyword), out dr))
                {
                    if (dr["Status"].ToString() != "New")
                    {
                        this.datePullOutDate.Focus();
                        MyUtility.Msg.WarningBox("Pullout date already exist pullout report and have been confirmed!");
                        return false;
                    }
                }
            }

            // 檢查欄位值不可為空
            if (MyUtility.Check.Empty(this.CurrentMaintain["OrderID"]))
            {
                this.txtSP.Focus();
                MyUtility.Msg.WarningBox("SP# can't empty!!");
                return false;
            }

            // 檢查OrderID+Seq不可以重複建立
            if (MyUtility.Check.Seek(string.Format("select ID from PackingList WITH (NOLOCK) where OrderID = '{0}' AND OrderShipmodeSeq = '{1}' AND ID != '{2}'", this.CurrentMaintain["OrderID"].ToString(), this.CurrentMaintain["OrderShipmodeSeq"].ToString(), MyUtility.Convert.GetString(this.CurrentMaintain["ID"]))))
            {
                MyUtility.Msg.WarningBox("SP No:" + this.CurrentMaintain["OrderID"].ToString() + ", Seq:" + this.CurrentMaintain["OrderShipmodeSeq"].ToString() + " already exist in packing list, can't be create again!");
                return false;
            }

            // 表身的CTN#,Color Way與Size不可以為空值，順便填入OrderID, OrderShipmodeSeq與Seq欄位值，計算CTNQty, ShipQty，重算表身Grid的Bal. Qty
            int i = 0, ctnQty = 0, shipQty = 0, ttlShipQty = 0, needPackQty = 0, count = 0;
            string filter = string.Empty, sqlCmd;
            bool isNegativeBalQty = false;
            DataTable needPackData;
            DualResult selectResult;
            DataRow[] detailData;
            #region 先將此Packinglist的各Article & SizeCode尚未裝箱件數撈出來
            sqlCmd = string.Format(
                @"select oqd.Article,oqd.SizeCode,(oqd.Qty-isnull(sum(pd.ShipQty), 0) - isnull(sum(iaq.DiffQty), 0)) as Qty
from Order_QtyShip_Detail oqd WITH (NOLOCK) 
left join PackingList_Detail pd WITH (NOLOCK) on pd.ID != '{0}' and oqd.Id = pd.OrderID and oqd.Seq = pd.OrderShipmodeSeq and oqd.Article = pd.Article and oqd.SizeCode = pd.SizeCode
left join InvAdjust ia WITH (NOLOCK) on ia.OrderID = pd.OrderID and ia.OrderShipmodeSeq = pd.OrderShipmodeSeq
left join InvAdjust_Qty iaq WITH (NOLOCK) on iaq.ID = ia.ID and iaq.Article = pd.Article and iaq.SizeCode = pd.SizeCode
where oqd.ID = '{1}'
group by oqd.Article,oqd.SizeCode, oqd.Qty",
                this.CurrentMaintain["ID"].ToString(),
                this.CurrentMaintain["OrderID"].ToString());
            if (!(selectResult = DBProxy.Current.Select(null, sqlCmd, out needPackData)))
            {
                MyUtility.Msg.WarningBox("Query pack qty fail!");
                return false;
            }
            #endregion

            foreach (DataRow dr in this.DetailDatas)
            {
                #region 刪除表身Qty為空白的資料
                if (MyUtility.Check.Empty(dr["ShipQty"]))
                {
                    dr.Delete();
                    continue;
                }
                #endregion

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

                #region 填入OrderID, OrderShipmodeSeq,Seq,QtyPerCTN與CTNQty欄位值
                i = i + 1;
                dr["OrderID"] = this.CurrentMaintain["OrderID"].ToString();
                dr["OrderShipmodeSeq"] = this.CurrentMaintain["OrderShipmodeSeq"].ToString();
                dr["Seq"] = Convert.ToString(i).PadLeft(6, '0');
                dr["QtyPerCTN"] = dr["ShipQty"];
                if (MyUtility.Check.Empty(dr["CTNQty"]))
                {
                    dr["CTNQty"] = 0;
                }
                #endregion

                #region 計算CTNQty, ShipQty
                ctnQty = ctnQty + MyUtility.Convert.GetInt(dr["CTNQty"]);
                shipQty = shipQty + MyUtility.Convert.GetInt(dr["ShipQty"]);
                #endregion

                #region 重算表身Grid的Bal. Qty

                // 目前還有多少衣服尚未裝箱
                needPackQty = 0;
                filter = string.Format("Article = '{0}' and SizeCode = '{1}'", dr["Article"].ToString(), dr["SizeCode"].ToString());
                detailData = needPackData.Select(filter);
                if (detailData.Length > 0)
                {
                    needPackQty = MyUtility.Convert.GetInt(detailData[0]["Qty"]);
                }

                // 加總表身特定Article/SizeCode的Ship Qty數量
                detailData = ((DataTable)this.detailgridbs.DataSource).Select(filter);
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
                    this.detailgrid.Rows[count].DefaultCellStyle.BackColor = Color.Pink;
                }
                #endregion
                count = count + 1;
            }

            if (this.DetailDatas.Count == 0)
            {
                MyUtility.Msg.InfoBox("Detail cannot be empty");
                return false;
            }

            this.CurrentMaintain["CTNQty"] = ctnQty;
            this.CurrentMaintain["ShipQty"] = shipQty;

            if (isNegativeBalQty)
            {
                MyUtility.Msg.WarningBox("Quantity entered is greater than order quantity!!");
                return false;
            }

            // GetID
            if (this.IsDetailInserting)
            {
                string id = MyUtility.GetValue.GetID(MyUtility.GetValue.Lookup(string.Format("select (select KeyWord from Factory WITH (NOLOCK) where ID = Orders.FtyGroup) as KeyWord from Orders WITH (NOLOCK) where ID = '{0}'", this.CurrentMaintain["OrderID"].ToString())) + "LS", "PackingList", DateTime.Today, 2, "Id", null);
                if (MyUtility.Check.Empty(id))
                {
                    MyUtility.Msg.WarningBox("GetID fail, please try again!");
                    return false;
                }

                this.CurrentMaintain["ID"] = id;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["CBM"]) || MyUtility.Check.Empty(this.CurrentMaintain["GW"]))
            {
                this.numTtlCBM.Focus();
                MyUtility.Msg.WarningBox("Ttl CBM and Ttl GW can't be empty!!");
                return false;
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

            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override void ClickSaveAfter()
        {
            base.ClickSaveAfter();
            #region ISP20200757 資料交換 - Sunrise
            Task.Run(() => new Sunrise_FinishingProcesses().SentPackingToFinishingProcesses(this.CurrentMaintain["ID"].ToString(), string.Empty))
                .ContinueWith(UtilityAutomation.AutomationExceptionHandler, System.Threading.CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.FromCurrentSynchronizationContext());
            #endregion

            #region ISP20201607 資料交換 - Gensong
            if (Gensong_FinishingProcesses.IsGensong_FinishingProcessesEnable)
            {
                // 不透過Call API的方式，自己組合，傳送API
                Task.Run(() => new Gensong_FinishingProcesses().SentPackingListToFinishingProcesses(this.CurrentMaintain["ID"].ToString(), string.Empty))
                    .ContinueWith(UtilityAutomation.AutomationExceptionHandler, System.Threading.CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.FromCurrentSynchronizationContext());
            }
            #endregion
        }

        /// <inheritdoc/>
        protected override DualResult ClickDeletePost()
        {
            #region 一併移除 PackingListID 相對應貼標 / 噴碼的資料
            string sqlCmd = $@"

 DELETE picd
 FROM ShippingMarkPic pic
 INNER JOIN ShippingMarkPic_Detail picd ON pic.Ukey = picd.ShippingMarkPicUkey
 WHERE pic.PackingListID='{this.CurrentMaintain["ID"]}'

 DELETE ShippingMarkPic
 WHERE PackingListID='{this.CurrentMaintain["ID"]}'


 DELETE stampd
 FROM ShippingMarkStamp stamp
 INNER JOIN ShippingMarkStamp_Detail stampd ON stamp.PackingListID = stampD.PackingListID
 WHERE stamp.PackingListID='{this.CurrentMaintain["ID"]}'

 DELETE ShippingMarkStamp
 WHERE PackingListID='{this.CurrentMaintain["ID"]}'
";
            DualResult result = DBProxy.Current.Execute(null, sqlCmd);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Delete <Shipping Mark Picture> 、<Shipping Mark Stamp> fail! \r\n" + result.ToString());
                return failResult;
            }
            #endregion

            #region ISP20200757 資料交換 - Sunrise
            Task.Run(() => new Sunrise_FinishingProcesses().SentPackingToFinishingProcesses(this.CurrentMaintain["ID"].ToString(), "delete"))
                .ContinueWith(UtilityAutomation.AutomationExceptionHandler, System.Threading.CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.FromCurrentSynchronizationContext());
            #endregion

            #region ISP20201607 資料交換 - Gensong
            if (Gensong_FinishingProcesses.IsGensong_FinishingProcessesEnable)
            {
                // 不透過Call API的方式，自己組合，傳送API
                Task.Run(() => new Gensong_FinishingProcesses().SentPackingListToFinishingProcesses(this.CurrentMaintain["ID"].ToString(), string.Empty))
                    .ContinueWith(UtilityAutomation.AutomationExceptionHandler, System.Threading.CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.FromCurrentSynchronizationContext());
            }
            #endregion
            return base.ClickDeletePost();
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

            // 檢查表身不可以有箱子送至Clog
            foreach (DataRow dr in ((DataTable)this.detailgridbs.DataSource).Rows)
            {
                if (!MyUtility.Check.Empty(dr["TransferDate"]))
                {
                    MyUtility.Msg.WarningBox(dr["CTNStartNo"].ToString() + " had been send to Clog, can't be deleted!");
                    return false;
                }
            }

            return base.ClickDeleteBefore();
        }

        /// <summary>
        /// ClickPrint
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ClickPrint()
        {
            int orderQty = MyUtility.Convert.GetInt(MyUtility.GetValue.Lookup(string.Format(
                @"select isnull(oq.Qty ,0) as Qty
from (select distinct OrderID,OrderShipmodeSeq from PackingList_Detail WITH (NOLOCK) where ID = '{0}') a
left join Order_QtyShip oq WITH (NOLOCK) on oq.Id = a.OrderID and oq.Seq = a.OrderShipmodeSeq", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]))));
            P06_Print callNextForm = new P06_Print(this.CurrentMaintain, orderQty);
            callNextForm.ShowDialog(this);

            return base.ClickPrint();
        }

        // 檢查輸入的SP#是否正確
        private void TxtSP_Validating(object sender, CancelEventArgs e)
        {
            if (this.EditMode)
            {
                if (!MyUtility.Check.Empty(this.txtSP.Text) && this.txtSP.Text != this.txtSP.OldValue)
                {
                    // sql參數
                    System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter("@orderid", this.txtSP.Text);
                    System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter("@mdivisionid", Env.User.Keyword);

                    IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                    cmds.Add(sp1);
                    cmds.Add(sp2);
                    DataTable orderData;
                    string sqlCmd = "select ID, StyleID, SeasonID, CustPONo, LocalOrder from Orders WITH (NOLOCK) where ID = @orderid and MDivisionID = @mdivisionid";

                    DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out orderData);
                    if (!result)
                    {
                        // OrderID異動，其他相關欄位要跟著異動
                        this.ChangeOtherData(string.Empty);
                        this.txtSP.Text = string.Empty;
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox("Sql connectionfail!!\r\n" + result.ToString());
                        return;
                    }
                    else
                    {
                        if (orderData.Rows.Count <= 0)
                        {
                            // OrderID異動，其他相關欄位要跟著異動
                            this.ChangeOtherData(string.Empty);
                            this.txtSP.Text = string.Empty;
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox("< SP# > does not exist!");
                            return;
                        }
                        else
                        {
                            if (orderData.Rows[0]["LocalOrder"].ToString() == "False")
                            {
                                // OrderID異動，其他相關欄位要跟著異動
                                this.ChangeOtherData(string.Empty);
                                this.txtSP.Text = string.Empty;
                                e.Cancel = true;
                                MyUtility.Msg.WarningBox("This SP# is not local order!");
                                return;
                            }
                        }
                    }
                }
            }
        }

        // SP#輸入完成後要帶入其他欄位值
        private void TxtSP_Validated(object sender, EventArgs e)
        {
            if (this.txtSP.OldValue == this.txtSP.Text)
            {
                return;
            }

            // OrderID異動，其他相關欄位要跟著異動
            this.ChangeOtherData(this.txtSP.Text);
        }

        // OrderID異動，其他相關欄位要跟著異動
        private void ChangeOtherData(string orderID)
        {
            // 清空表身Grid資料
            foreach (DataRow dr in this.DetailDatas)
            {
                dr.Delete();
            }

            this.CurrentMaintain["CTNQty"] = 0;

            if (MyUtility.Check.Empty(orderID))
            {
                // OrderID為空值時，要把其他相關欄位值清空
                this.CurrentMaintain["OrderShipmodeSeq"] = string.Empty;
                this.CurrentMaintain["ShipModeID"] = string.Empty;
                this.CurrentMaintain["BrandID"] = string.Empty;
                this.CurrentMaintain["CustCDID"] = string.Empty;
                this.CurrentMaintain["FactoryID"] = string.Empty;
                this.displayStyle.Value = string.Empty;
                this.displaySeason.Value = string.Empty;
                this.displayPONo.Value = string.Empty;
            }
            else
            {
                DataRow dr;
                string sqlCmd = string.Format(
                    @"
select  StyleID
        , SeasonID
        , CustPONo
        , Customize1
        , ReadyDate
        , BrandID
        , CustCDID
        , Dest 
        , FtyGroup
from Orders WITH (NOLOCK) 
where ID = '{0}'", orderID);
                if (MyUtility.Check.Seek(sqlCmd, out dr))
                {
                    // 帶出相關欄位的資料
                    this.displayStyle.Value = dr["StyleID"].ToString();
                    this.displaySeason.Value = dr["SeasonID"].ToString();
                    this.displayPONo.Value = dr["CustPONo"].ToString();
                    this.CurrentMaintain["BrandID"] = dr["BrandID"].ToString();
                    this.CurrentMaintain["CustCDID"] = dr["CustCDID"].ToString();
                    this.CurrentMaintain["FactoryID"] = dr["FtyGroup"].ToString();

                    #region 若Order_QtyShip有多筆資料話就跳出視窗讓使者選擇Seq
                    sqlCmd = string.Format("select count(ID) as CountID from Order_QtyShip WITH (NOLOCK) where ID = '{0}'", orderID);
                    if (MyUtility.Check.Seek(sqlCmd, out dr))
                    {
                        if (dr["CountID"].ToString() == "1")
                        {
                            sqlCmd = string.Format("select ShipModeID,BuyerDelivery,Seq from Order_QtyShip WITH (NOLOCK) where ID = '{0}'", orderID);
                            if (MyUtility.Check.Seek(sqlCmd, out dr))
                            {
                                this.CurrentMaintain["OrderShipmodeSeq"] = dr["Seq"].ToString();
                                this.CurrentMaintain["ShipModeID"] = dr["ShipModeID"].ToString();
                            }
                        }
                        else
                        {
                            this.SeqRightClick();
                        }
                    }
                    #endregion

                    // 產生表身Grid的資料
                    this.GenDetailData(orderID, this.CurrentMaintain["OrderShipmodeSeq"].ToString());
                }
            }
        }

        // Seq按右鍵功能
        private void TxtSeq_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            if (this.EditMode)
            {
                this.SeqRightClick();

                // 產生表身Grid的資料
                this.GenDetailData(this.CurrentMaintain["OrderID"].ToString(), this.CurrentMaintain["OrderShipmodeSeq"].ToString());
            }
        }

        private void SeqRightClick()
        {
            IList<DataRow> orderQtyShipData;
            string sqlCmd = string.Format("select Seq,BuyerDelivery,ShipmodeID,Qty from Order_QtyShip WITH (NOLOCK) where ID = '{0}'", this.CurrentMaintain["OrderID"].ToString());
            Win.Tools.SelectItem item = new Win.Tools.SelectItem(sqlCmd, "4,20,20,10", string.Empty, "Seq,Buyer Delivery,ShipMode,Qty");
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                this.CurrentMaintain["OrderShipmodeSeq"] = string.Empty;
                this.CurrentMaintain["ShipModeID"] = string.Empty;
                this.datePullOutDate.Value = null;
            }
            else
            {
                orderQtyShipData = item.GetSelecteds();
                this.CurrentMaintain["OrderShipmodeSeq"] = item.GetSelectedString();
                this.CurrentMaintain["ShipModeID"] = orderQtyShipData[0]["ShipmodeID"].ToString();
            }
        }

        // 檢查箱子是否有送至Clog來決定欄位是否可被修改
        private bool CheckCanCahngeCol(DateTime? transferDate)
        {
            if (MyUtility.Check.Empty(transferDate))
            {
                return true;
            }
            else
            {
                MyUtility.Msg.WarningBox("This record had been send to CLOG, can't modified!!");
                return false;
            }
        }

        // 產生表身Grid的資料
        private void GenDetailData(string orderID, string seq)
        {
            // 清空表身Grid資料
            foreach (DataRow dr in this.DetailDatas)
            {
                dr.Delete();
            }

            if (!MyUtility.Check.Empty(orderID) && !MyUtility.Check.Empty(orderID))
            {
                string sqlCmd = string.Format(
                    @"select '' as ID,o.ID as OrderID, oqd.Seq as OrderShipmodeSeq, oqd.Article, oqd.SizeCode, oqd.Qty as ShipQty, o.CTNQty as QtyPerCTN
from Order_QtyShip_Detail oqd WITH (NOLOCK) 
left Join Orders o WITH (NOLOCK) on o.ID = oqd.Id 
left join Order_SizeCode os WITH (NOLOCK) on os.id = o.POID and os.SizeCode = oqd.SizeCode
left join Order_Article oa WITH (NOLOCK) on oa.id = oqd.Id and oa.Article = oqd.Article
where oqd.ID = '{0}' and oqd.Seq = '{1}'
order by oa.Seq,os.Seq",
                    orderID,
                    seq);

                if (this.result = DBProxy.Current.Select(null, sqlCmd, out this.selectedData))
                {
                    foreach (DataRow dr in this.selectedData.Rows)
                    {
                        dr.AcceptChanges();
                        dr.SetAdded();
                        ((DataTable)this.detailgridbs.DataSource).ImportRow(dr);
                    }
                }
            }
        }

        // Pull-out Date Validating()
        private void DatePullOutDate_Validating(object sender, CancelEventArgs e)
        {
            if (!MyUtility.Check.Empty(this.datePullOutDate.Value) && this.datePullOutDate.Value != this.datePullOutDate.OldValue)
            {
                DataRow dr;
                if (MyUtility.Check.Seek(string.Format("select ID,status from Pullout WITH (NOLOCK) where PulloutDate = '{0}' and MDivisionID = '{1}'", Convert.ToDateTime(this.datePullOutDate.Value.ToString()).ToString("d"), Env.User.Keyword), out dr))
                {
                    if (dr["Status"].ToString() != "New")
                    {
                        this.datePullOutDate.Value = null;
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox("Pullout date already exist pullout report and have been confirmed!");
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// ClickConfirm
        /// </summary>
        protected override void ClickConfirm()
        {
            base.ClickConfirm();

            // Pull-out date不可為空
            if (MyUtility.Check.Empty(this.CurrentMaintain["PulloutDate"]))
            {
                MyUtility.Msg.WarningBox("Pull-out date can't empty!!");
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

            string sqlCmd = string.Format("update PackingList set Status = 'Confirmed', EditName = '{0}', EditDate = GETDATE() where ID = '{1}'", Env.User.UserID, this.CurrentMaintain["ID"].ToString());
            this.result = DBProxy.Current.Execute(null, sqlCmd);
            if (!this.result)
            {
                MyUtility.Msg.WarningBox("Confirm fail !\r\n" + this.result.ToString());
                return;
            }
        }

        /// <summary>
        /// ClickUnconfirm
        /// </summary>
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            DataRow dr;

            // 如果Pullout Report已被Confirmed就不可以做UnConfirm
            string sqlCmd = string.Format(
                @"select p.Status
from PackingList pl WITH (NOLOCK) , Pullout p WITH (NOLOCK) 
where pl.ID = '{0}'
and p.ID = pl.PulloutID",
                this.CurrentMaintain["ID"].ToString());
            if (MyUtility.Check.Seek(sqlCmd, out dr))
            {
                if (dr["Status"].ToString() != "New")
                {
                    MyUtility.Msg.WarningBox("Pullout report already confirmed, so can't unconfirm! ");
                    return;
                }
            }

            // 問是否要做Unconfirm，確定才繼續往下做
            this.buttonResult = MyUtility.Msg.WarningBox("Are you sure you want to < Unconfirm > this data?", "Warning", MessageBoxButtons.YesNo);
            if (this.buttonResult == DialogResult.No)
            {
                return;
            }

            sqlCmd = string.Format("update PackingList set Status = 'New', EditName = '{0}', EditDate = GETDATE() where ID = '{1}'", Env.User.UserID, this.CurrentMaintain["ID"].ToString());

            this.result = DBProxy.Current.Execute(null, sqlCmd);
            if (!this.result)
            {
                MyUtility.Msg.WarningBox("UnConfirm failed, Pleaes re-try");
            }
        }

        private void BtnCartonSummary_Click(object sender, EventArgs e)
        {
            P03_CartonSummary callNextForm = new P03_CartonSummary(this.CurrentMaintain["ID"].ToString());
            callNextForm.ShowDialog(this);
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

                    if (MyUtility.Convert.GetDecimal(dr["BalanceQty"]) < 0)
                    {
                        this.detailgrid.Rows[index].Cells[7].Style.BackColor = Color.Red;
                    }
                }
            }
        }
    }
}
