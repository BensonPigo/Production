using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using Sci.Production.PublicPrg;
using System.Linq;
using System.Data.SqlClient;

namespace Sci.Production.Packing
{
    /// <summary>
    /// Packing_P05
    /// </summary>
    public partial class P05 : Win.Tems.Input6
    {
        private string masterID;
        private Ict.Win.DataGridViewGeneratorTextColumnSettings orderid = new Ict.Win.DataGridViewGeneratorTextColumnSettings();
        private Ict.Win.DataGridViewGeneratorTextColumnSettings seq = new Ict.Win.DataGridViewGeneratorTextColumnSettings();
        private Ict.Win.DataGridViewGeneratorTextColumnSettings article = new Ict.Win.DataGridViewGeneratorTextColumnSettings();
        private Ict.Win.DataGridViewGeneratorTextColumnSettings size = new Ict.Win.DataGridViewGeneratorTextColumnSettings();
        private Ict.Win.DataGridViewGeneratorNumericColumnSettings balance = new Ict.Win.DataGridViewGeneratorNumericColumnSettings();
        private Ict.Win.UI.DataGridViewTextBoxColumn col_SP;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_Art;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_Size;
        private Ict.Win.UI.DataGridViewNumericBoxColumn col_qty;
        private DualResult result;
        private DataRow dr;
        private string sqlCmd = string.Empty;
        private string filter = string.Empty;
        private DataTable queryData;
        private DialogResult buttonResult;

        /// <summary>
        /// P05
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public P05(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = "MDivisionID = '" + Sci.Env.User.Keyword + "' AND Type = 'F'";
            this.detailgrid.AllowUserToOrderColumns = true;
            this.InsertDetailGridOnDoubleClick = false;
        }

        /// <summary>
        /// OnEditModeChanged
        /// </summary>
        protected override void OnEditModeChanged()
        {
            base.OnEditModeChanged();
            if (MyUtility.Check.Empty(this.btnBatchImport))
            {
                return;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain))
            {
                return;
            }

            this.btnBatchImport.Enabled = this.EditMode;
            this.CanEdit();
        }

        /// <summary>
        /// OnFormLoaded
        /// </summary>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            DataTable queryDT;
            string querySql = string.Format(
                @"
select '' FTYGroup

union 
select distinct FTYGroup 
from Factory 
where MDivisionID = '{0}'", Sci.Env.User.Keyword);
            DBProxy.Current.Select(null, querySql, out queryDT);
            MyUtility.Tool.SetupCombox(this.queryfors, 1, queryDT);
            this.queryfors.SelectedIndex = 0;
            this.queryfors.SelectedIndexChanged += (s, e) =>
            {
                switch (this.queryfors.SelectedIndex)
                {
                    case 0:
                        this.DefaultWhere = string.Empty;
                        break;
                    default:
                        this.DefaultWhere = string.Format(
                            @"
'{0}' in (select distinct FtyGroup 
          from orders o 
          where o.id in (select distinct PackingList_Detail.OrderID 
                         from PackingList_Detail 
                         where PackingList_Detail.id = PackingList.id)
         )", this.queryfors.SelectedValue);
                        break;
                }

                this.ReloadDatas();
            };
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
                    if (MyUtility.Check.Empty(e.FormattedValue))
                    {
                        this.ClearGridRowData(this.dr);
                        return;
                    }

                    if (e.FormattedValue.ToString() != this.dr["OrderID"].ToString())
                    {
                        // sql參數
                        SqlParameter sp1 = new SqlParameter("@orderid", e.FormattedValue.ToString());
                        SqlParameter sp2 = new SqlParameter("@brandid", MyUtility.Convert.GetString(this.CurrentMaintain["BrandID"]));
                        SqlParameter sp3 = new SqlParameter("@mdivisionid", Sci.Env.User.Keyword);

                        IList<SqlParameter> cmds = new List<SqlParameter>();
                        cmds.Add(sp1);
                        cmds.Add(sp2);
                        cmds.Add(sp3);

                        string sqlCmd = @"
Select  o.ID
        , o.SeasonID
        , o.StyleID
        , o.CustPONo 
        , o.FtyGroup
from Orders o WITH (NOLOCK) 
inner join Factory f on o.FactoryID = f.ID
where   o.ID = @orderid 
        and o.MDivisionID = @mdivisionid  
        and o.BrandID = @brandid 
        and o.IsForecast = 0 
        and o.LocalOrder = 0 
        and o.Junk = 0
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
                                List<SqlParameter> listSqlPar = new List<SqlParameter>();
                                listSqlPar.Add(new SqlParameter("@orderid", e.FormattedValue.ToString()));
                                string sqlChkCategory = $@"
select 1 from Orders o WITH (NOLOCK) where o.ID = @orderid and o.category in ('B','G')  ";
                                if (MyUtility.Check.Seek(sqlChkCategory, listSqlPar, null))
                                {
                                    MyUtility.Msg.WarningBox($"SP# {e.FormattedValue.ToString()} is Bulk/Garment and Unit Price is 0, please create chargeable packing list by Packing-P02, P03, P04.");
                                    this.ClearGridRowData(this.dr);
                                    e.Cancel = true;
                                    return;
                                }

                                this.dr["OrderID"] = e.FormattedValue.ToString().ToUpper();
                                this.dr["Factory"] = orderData.Rows[0]["FtyGroup"].ToString();
                                this.dr["StyleID"] = orderData.Rows[0]["StyleID"].ToString();
                                this.dr["CustPONo"] = orderData.Rows[0]["CustPONo"].ToString();
                                this.dr["SeasonID"] = orderData.Rows[0]["SeasonID"].ToString();
                                this.dr["Article"] = string.Empty;
                                this.dr["Color"] = string.Empty;
                                this.dr["SizeCode"] = string.Empty;
                                this.dr["qty"] = 0;
                                this.dr["ShipQty"] = 0;
                                this.dr["OtherConfirmQty"] = 0;
                                this.dr["InvAdjustQty"] = 0;
                                this.dr["BalanceQty"] = 0;
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
                                            this.CurrentMaintain["OrderShipmodeSeq"] = string.Empty;
                                        }
                                        else
                                        {
                                            this.CurrentMaintain["OrderShipmodeSeq"] = item.GetSelectedString();
                                        }
                                    }
                                }
                                #endregion
                                this.dr.EndEdit();
                            }
                        }
                    }
                }
            };

            // Seq
            this.seq.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && MyUtility.Check.Empty(this.CurrentMaintain["ExpressID"]))
                {
                    if (e.Button == System.Windows.Forms.MouseButtons.Right)
                    {
                        if (e.RowIndex != -1)
                        {
                            DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                            this.sqlCmd = string.Format("select Seq, BuyerDelivery,ShipmodeID,Qty from Order_QtyShip WITH (NOLOCK) where ID = '{0}'", dr["OrderID"].ToString());
                            Win.Tools.SelectItem item = new Win.Tools.SelectItem(this.sqlCmd, "4,20,20,10", string.Empty, "Seq,Buyer Delivery,ShipMode,Qty");
                            DialogResult returnResult = item.ShowDialog();
                            if (returnResult == DialogResult.Cancel)
                            {
                                e.EditingControl.Text = string.Empty;
                            }
                            else
                            {
                                dr["OrderShipmodeSeq"] = item.GetSelectedString();
                            }

                            dr["Article"] = string.Empty;
                            dr["Color"] = string.Empty;
                            dr["SizeCode"] = string.Empty;
                            dr["qty"] = 0;
                            dr["ShipQty"] = 0;
                            dr["OtherConfirmQty"] = 0;
                            dr["InvAdjustQty"] = 0;
                            dr["BalanceQty"] = 0;
                            dr.EndEdit();
                        }
                    }
                }
            };

            // Article
            this.article.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && MyUtility.Check.Empty(this.CurrentMaintain["ExpressID"]))
                {
                    if (e.Button == System.Windows.Forms.MouseButtons.Right)
                    {
                        if (e.RowIndex != -1)
                        {
                            DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                            this.sqlCmd = string.Format(
                                @"select distinct a.Article " + this.FOCQueryCmd() +
                                @"where a.Price = 0 ", dr["OrderID"].ToString(),
                                dr["OrderShipmodeSeq"].ToString());
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
                    if (MyUtility.Check.Empty(e.FormattedValue))
                    {
                        dr["Article"] = string.Empty;
                        dr["Color"] = string.Empty;
                        dr["SizeCode"] = string.Empty;
                        dr["qty"] = 0;
                        dr["ShipQty"] = 0;
                        dr["OtherConfirmQty"] = 0;
                        dr["InvAdjustQty"] = 0;
                        dr["BalanceQty"] = 0;
                        dr.EndEdit();
                        return;
                    }

                    if (e.FormattedValue.ToString() != dr["Article"].ToString())
                    {
                        this.sqlCmd = string.Format(
                            @"select a.Article " + this.FOCQueryCmd() +
                            @" where a.Price = 0 and a.Article = '{2}'",
                            dr["OrderID"].ToString(),
                            dr["OrderShipmodeSeq"].ToString(),
                            e.FormattedValue.ToString());

                        if (!MyUtility.Check.Empty(dr["SizeCode"]))
                        {
                            this.sqlCmd = this.sqlCmd + string.Format(" and a.SizeCode = '{0}'", dr["SizeCode"].ToString());
                        }

                        if (!MyUtility.Check.Seek(this.sqlCmd))
                        {
                            MyUtility.Msg.WarningBox(string.Format("< Article: {0} > not found!!!", e.FormattedValue.ToString()));
                            dr["Article"] = string.Empty;
                            dr["Color"] = string.Empty;
                            dr["SizeCode"] = string.Empty;
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
                            else
                            {
                                dr["Color"] = string.Empty;
                            }
                        }

                        dr["SizeCode"] = string.Empty;
                        dr["qty"] = 0;
                        dr["ShipQty"] = 0;
                        dr["OtherConfirmQty"] = 0;
                        dr["InvAdjustQty"] = 0;
                        dr["BalanceQty"] = 0;
                        dr.EndEdit();
                    }
                }
            };

            // SizeCode
            this.size.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && MyUtility.Check.Empty(this.CurrentMaintain["ExpressID"]))
                {
                    if (e.Button == System.Windows.Forms.MouseButtons.Right)
                    {
                        if (e.RowIndex != -1)
                        {
                            DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                            this.sqlCmd = string.Format(
                                @"select distinct a.SizeCode as Size,os.Seq " + this.FOCQueryCmd() + @"
left join Orders o WITH (NOLOCK) on o.ID = '{0}'
left join Order_SizeCode os WITH (NOLOCK) on os.Id = o.POID and os.SizeCode = a.SizeCode
where a.Price = 0 and a.Article = '{2}'
order by os.Seq", dr["OrderID"].ToString(),
                                dr["OrderShipmodeSeq"].ToString(),
                                dr["Article"].ToString());
                            this.result = DBProxy.Current.Select(null, this.sqlCmd, out this.queryData);
                            Win.Tools.SelectItem item = new Win.Tools.SelectItem(this.queryData, "Size", "8", dr["SizeCode"].ToString());
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
                    if (MyUtility.Check.Empty(e.FormattedValue))
                    {
                        dr["SizeCode"] = string.Empty;
                        dr["qty"] = 0;
                        dr["ShipQty"] = 0;
                        dr["OtherConfirmQty"] = 0;
                        dr["InvAdjustQty"] = 0;
                        dr["BalanceQty"] = 0;
                        dr.EndEdit();
                        return;
                    }

                    if (e.FormattedValue.ToString() != dr["SizeCode"].ToString())
                    {
                        this.sqlCmd = string.Format(
                            @"select a.SizeCode " + this.FOCQueryCmd() +
                            @" where a.Price = 0 and a.Article = '{2}' and a.SizeCode = '{3}'",
                            dr["OrderID"].ToString(),
                            dr["OrderShipmodeSeq"].ToString(),
                            dr["Article"].ToString(),
                            e.FormattedValue.ToString());
                        if (!MyUtility.Check.Seek(this.sqlCmd))
                        {
                            dr["SizeCode"] = string.Empty;
                            dr["qty"] = 0;
                            dr["ShipQty"] = 0;
                            dr["OtherConfirmQty"] = 0;
                            dr["InvAdjustQty"] = 0;
                            dr["BalanceQty"] = 0;
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox(string.Format("< SizeCode: {0} > not found!!!", e.FormattedValue.ToString()));
                            return;
                        }

                        dr["SizeCode"] = e.FormattedValue.ToString();

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
                        dr["BalanceQty"] = Convert.ToDecimal(dr["qty"].ToString()) - Convert.ToInt32(dr["OtherConfirmQty"]) - Convert.ToInt32(dr["InvAdjustQty"]) - Convert.ToDecimal(dr["ShipQty"].ToString());
                        dr.EndEdit();
                        this.ComputeOrderQty();
                    }
                }
            };

            this.balance.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    dr["shipQty"] = e.FormattedValue;
                    dr["BalanceQty"] = Convert.ToDecimal(dr["qty"].ToString()) - Convert.ToInt32(dr["OtherConfirmQty"]) - Convert.ToInt32(dr["InvAdjustQty"]) - Convert.ToDecimal(dr["ShipQty"].ToString());
                    dr.EndEdit();
                }
            };
            #endregion

            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("Factory", header: "Factory", width: Ict.Win.Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("OrderID", header: "SP No.", width: Ict.Win.Widths.AnsiChars(13), settings: this.orderid).Get(out this.col_SP)
                .Text("SeasonID", header: "Season", width: Ict.Win.Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("OrderShipmodeSeq", header: "Seq", width: Ict.Win.Widths.AnsiChars(2), iseditingreadonly: true, settings: this.seq)
                .Text("StyleID", header: "Style No.", width: Ict.Win.Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("CustPONo", header: "P.O. No.", width: Ict.Win.Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Article", header: "ColorWay", width: Ict.Win.Widths.AnsiChars(8), settings: this.article).Get(out this.col_Art)
                .Text("Color", header: "Color", width: Ict.Win.Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("SizeCode", header: "Size", width: Ict.Win.Widths.AnsiChars(8), settings: this.size).Get(out this.col_Size)
                .Numeric("Qty", header: "Order Qty", iseditingreadonly: true)
                .Numeric("ShipQty", header: "Qty", settings: this.balance).Get(out this.col_qty)
                .Numeric("BalanceQty", header: "Bal. Qty", iseditingreadonly: true)
                .Text("CustCDID", header: "Cust CD", width: Ict.Win.Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("Dest", header: "Dest", width: Ict.Win.Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("OrderTypeID", header: "Order Type", width: Ict.Win.Widths.AnsiChars(10), iseditingreadonly: true)
                ;
        }

        /// <summary>
        /// OnDetailEntered
        /// </summary>
        protected override void OnDetailEntered()
        {
            DataRow dr;
            this.labelConfirmed.Visible = MyUtility.Check.Empty(this.CurrentMaintain["ID"].ToString()) ? false : true;

            string sqlStatus = string.Format(@"select status from PackingList WITH (NOLOCK) where id='{0}'", this.CurrentMaintain["ID"].ToString());
            if (MyUtility.Check.Seek(sqlStatus, out dr))
            {
                this.labelConfirmed.Text = dr["Status"].ToString();
            }

            base.OnDetailEntered();

            DataTable dt = (DataTable)this.detailgridbs.DataSource;
            if (!dt.Columns.Contains("Qty"))
            {
                dt.ColumnsIntAdd("Qty");
            }

            if (!dt.Columns.Contains("OtherConfirmQty"))
            {
                dt.ColumnsIntAdd("OtherConfirmQty");
            }

            if (!dt.Columns.Contains("InvAdjustQty"))
            {
                dt.ColumnsIntAdd("InvAdjustQty");
            }

            #region ComputeOrderQty
            this.ComputeOrderQty();
            #endregion

            this.Color_Change();
        }

        /// <summary>
        /// ClickSaveAfter
        /// </summary>
        protected override void ClickSaveAfter()
        {
            base.ClickSaveAfter();
            DataTable dt = (DataTable)this.detailgridbs.DataSource;
            if (!dt.Columns.Contains("Qty"))
            {
                dt.ColumnsIntAdd("Qty");
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
                dr["Qty"] = needPackQty;
                dr["shipQty"] = ttlShipQty;
                dr.EndEdit();
                #endregion
            }
        }

        // 清空Order相關欄位值
        private void ClearGridRowData(DataRow dr)
        {
            dr["OrderID"] = string.Empty;
            dr["OrderShipmodeSeq"] = string.Empty;
            dr["Article"] = string.Empty;
            dr["Color"] = string.Empty;
            dr["SizeCode"] = string.Empty;
            dr["StyleID"] = string.Empty;
            dr["CustPONo"] = string.Empty;
            dr["qty"] = 0;
            dr["ShipQty"] = 0;
            dr["OtherConfirmQty"] = 0;
            dr["InvAdjustQty"] = 0;
            dr["BalanceQty"] = 0;
            dr.EndEdit();
        }

        /// <summary>
        /// ClickNewAfter
        /// </summary>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["MDivisionID"] = Sci.Env.User.Keyword;
            this.CurrentMaintain["FactoryID"] = Sci.Env.User.Factory;
            this.CurrentMaintain["Type"] = "F";
            this.CurrentMaintain["Dest"] = "ZZ";
            this.CurrentMaintain["Status"] = "New";
            this.CanEdit();
            this.btnBatchImport.Enabled = true;
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

            this.CanEdit();

            return base.ClickEditBefore();
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
                // string pullLock = MyUtility.GetValue.Lookup("select PullLock from System");
                if (MyUtility.Convert.GetDate(this.CurrentMaintain["PulloutDate"]) < MyUtility.Convert.GetDate(MyUtility.GetValue.Lookup("select PullLock from System WITH (NOLOCK) ")))
                {
                    this.datePullOutDate.Focus();
                    MyUtility.Msg.WarningBox("Pullout date less then pullout lock date!!");
                    return false;
                }

                // 如果Pullout report已存在且狀態為Confirmed時，需出訊息告知
                if (MyUtility.Check.Seek(string.Format("select ID,status from Pullout WITH (NOLOCK) where PulloutDate = '{0}' and MDivisionID = '{1}'", Convert.ToDateTime(this.CurrentMaintain["PulloutDate"].ToString()).ToString("d"), Sci.Env.User.Keyword), out this.dr))
                {
                    if (this.dr["Status"].ToString() != "New")
                    {
                        this.datePullOutDate.Focus();
                        MyUtility.Msg.WarningBox("Pullout date already exist pullout report and have been confirmed!");
                        return false;
                    }
                }
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["BrandID"]))
            {
                this.txtbrand.Focus();
                MyUtility.Msg.WarningBox("Brand can't empty!!");
                return false;
            }

            // 刪除表身SP No.或Qty為空白的資料，檢查表身的Color Way與Size不可以為空值，計算ShipQty，重算表身Grid的Bal. Qty
            int shipQty = 0, needPackQty = 0, ttlShipQty = 0, count = 0;

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
            StringBuilder msg = new StringBuilder();

            foreach (DataRow dr in this.DetailDatas)
            {
                #region 刪除表身SP No.為空白的資料
                if (MyUtility.Check.Empty(dr["OrderID"]))
                {
                    dr.Delete();
                    continue;
                }
                #endregion

                #region Qty 為0的資料
                if (MyUtility.Check.Empty(dr["ShipQty"]))
                {
                    msg.Append($@"SP: {dr["OrderID"]}, Colorway: {dr["Article"]}, Size: {dr["SizeCode"]}" + Environment.NewLine);
                }

                #endregion

                #region 訂單的BrandID要跟表頭的Brand相同
                if (MyUtility.GetValue.Lookup("BrandID", dr["OrderID"].ToString(), "Orders", "ID") != this.CurrentMaintain["BrandID"].ToString())
                {
                    MyUtility.Msg.WarningBox("SP No:" + dr["OrderID"].ToString() + "'s brand is not equal to Brand:" + this.CurrentMaintain["BrandID"].ToString() + ", so can't be save!");
                    return false;
                }
                #endregion

                #region 確認訂單不可含有Bulk和Garment單
                List<SqlParameter> listSqlPar = new List<SqlParameter>();
                listSqlPar.Add(new SqlParameter("@orderid", dr["OrderID"].ToString()));
                string sqlChkCategory = $@"
select 1 from Orders o WITH (NOLOCK) where o.ID = @orderid and o.category in ('B','G')  ";
                if (MyUtility.Check.Seek(sqlChkCategory, listSqlPar, null))
                {
                    MyUtility.Msg.WarningBox($"SP# {dr["OrderID"].ToString()} is Bulk/Garment and Unit Price is 0, please create chargeable packing list by Packing-P02, P03, P04.");
                    return false;
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
                    MyUtility.Msg.WarningBox("< Size >  can't empty!");
                    this.detailgrid.Focus();
                    return false;
                }
                #endregion

                #region 確認每筆資料都是FOC

                this.sqlCmd = string.Format(
                    @"select a.SizeCode " + this.FOCQueryCmd() +
                    @"where a.Price = 0 and a.Article = '{2}' and a.SizeCode = '{3}'",
                    dr["OrderID"].ToString(),
                    dr["OrderShipmodeSeq"].ToString(),
                    dr["Article"].ToString(),
                    dr["SizeCode"].ToString());
                if (!MyUtility.Check.Seek(this.sqlCmd))
                {
                    MyUtility.Msg.WarningBox("SP No:" + dr["OrderID"].ToString() + ", Color Code:" + dr["Article"].ToString() + ", Size:" + dr["SizeCode"].ToString() + " is not F.O.C data!!");
                    return false;
                }
                #endregion

                #region 計算ShipQty
                shipQty = shipQty + MyUtility.Convert.GetInt(dr["ShipQty"].ToString());
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
	   , oqd.Qty as Qty
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

                dr["BalanceQty"] = needPackQty - Convert.ToInt32(dr["OtherConfirmQty"]) - Convert.ToInt32(dr["InvAdjustQty"]) - ttlShipQty;
                if (Convert.ToInt32(dr["BalanceQty"]) < 0)
                {
                    isNegativeBalQty = true;
                    listErrorMsg.Add(string.Format("Order Qty: {0}, Shipped Qty: {1}, Qty: {2}", dr["Qty"], Convert.ToInt32(dr["OtherConfirmQty"]) - Convert.ToInt32(dr["InvAdjustQty"]), dr["ShipQty"]));
                    this.detailgrid.Rows[count].DefaultCellStyle.BackColor = Color.Pink;
                }
                #endregion
                count = count + 1;
            }

            // ShipQty
            this.CurrentMaintain["ShipQty"] = shipQty;

            if (!MyUtility.Check.Empty(msg.ToString()))
            {
                MyUtility.Msg.WarningBox("Below issue SP# [Qty] must be morre than 0!" + Environment.NewLine + msg.ToString());
                return false;
            }

            if (isNegativeBalQty)
            {
                MyUtility.Msg.WarningBox(listErrorMsg.JoinToString(Environment.NewLine) + Environment.NewLine + "Balance Quantity cannot < 0");
                return false;
            }

            // 表身Grid不可為空
            if (count == 0)
            {
                this.detailgrid.Focus();
                MyUtility.Msg.WarningBox("< Detail > can't be empty!");
                return false;
            }

            // GetID
            if (this.IsDetailInserting)
            {
                string id = MyUtility.GetValue.GetID(Sci.Env.User.Keyword + "FS", "PackingList", DateTime.Today, 2, "Id", null);
                if (MyUtility.Check.Empty(id))
                {
                    MyUtility.Msg.WarningBox("GetID fail, please try again!");
                    return false;
                }

                this.CurrentMaintain["ID"] = id;
                this.CurrentMaintain["INVNo"] = id;
            }

            // Get表身 SCICtnNo
            if (this.IsDetailInserting)
            {
                if (!PublicPrg.Prgs.GetSCICtnNo((DataTable)this.detailgridbs.DataSource, this.CurrentMaintain["ID"].ToString(), "IsDetailInserting"))
                {
                    return false;
                }
            }
            else
            {
                if (!PublicPrg.Prgs.GetSCICtnNo((DataTable)this.detailgridbs.DataSource, this.CurrentMaintain["ID"].ToString(), string.Empty))
                {
                    return false;
                }
            }

            return base.ClickSaveBefore();
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

            if (!MyUtility.Check.Empty(this.CurrentMaintain["ExpressID"]))
            {
                MyUtility.Msg.WarningBox("This record had HC No. Can't be deleted!");
                return false;
            }

            return base.ClickDeleteBefore();
        }

        // Pull-out Date Validating()
        private void DatePullOutDate_Validating(object sender, CancelEventArgs e)
        {
            if (!MyUtility.Check.Empty(this.datePullOutDate.Value) && this.datePullOutDate.Value !=
                this.datePullOutDate.OldValue)
            {
                if (MyUtility.Check.Seek(string.Format("select ID,status from Pullout WITH (NOLOCK) where PulloutDate = '{0}' and MDivisionID = '{1}'", Convert.ToDateTime(this.datePullOutDate.Value.ToString()).ToString("d"), Sci.Env.User.Keyword), out this.dr))
                {
                    if (this.dr["Status"].ToString() != "New")
                    {
                        this.datePullOutDate.Value = null;
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox("Pullout date already exist pullout report and have been confirmed!");
                        return;
                    }
                }
            }
        }

        // Batch Import
        private void BtnBatchImport_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["BrandID"]))
            {
                MyUtility.Msg.WarningBox("Brand can't be empty!");
                return;
            }

            P05_BatchImport callNextForm = new P05_BatchImport(this.CurrentMaintain, (DataTable)this.detailgridbs.DataSource);
            callNextForm.ShowDialog(this);
            this.ComputeOrderQty();
        }

        /// <summary>
        /// ClickConfirm
        /// </summary>
        protected override void ClickConfirm()
        {
            base.ClickConfirm();

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

            this.sqlCmd = string.Format("update PackingList set Status = 'Confirmed', EditName = '{0}', EditDate = GETDATE() where ID = '{1}'", Sci.Env.User.UserID, this.CurrentMaintain["ID"].ToString());
            this.result = DBProxy.Current.Execute(null, this.sqlCmd);
            if (!this.result)
            {
                MyUtility.Msg.WarningBox("Confirm fail !\r\n" + this.result.ToString());
                return;
            }
        }

        /// <summary>
        /// UnConfirm
        /// </summary>
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();

            // 如果Pullout Report已被Confirmed就不可以做UnConfirm
            this.sqlCmd = string.Format(
                @"select p.Status
from PackingList pl WITH (NOLOCK) , Pullout p WITH (NOLOCK) 
where pl.ID = '{0}'
and p.ID = pl.PulloutID", this.CurrentMaintain["ID"].ToString());
            if (MyUtility.Check.Seek(this.sqlCmd, out this.dr))
            {
                if (this.dr["Status"].ToString() != "New")
                {
                    MyUtility.Msg.WarningBox("Pullout report already confirmed, so can't unconfirm! ");
                    return;
                }
            }

            // 問是否要做Unconfirm，確定才繼續往下做
            this.buttonResult = MyUtility.Msg.WarningBox("Are you sure you want to < Unconfirm > this data?", "Warning", MessageBoxButtons.YesNo);
            if (this.buttonResult == System.Windows.Forms.DialogResult.No)
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

        // Check是否為FOC訂單的共用邏輯層
        private string FOCQueryCmd()
        {
            string sqlCmd = @"
from (
    select oqd.Article
        ,oqd.SizeCode
        ,isnull(ou2.POPrice,isnull(ou1.POPrice,-1)) as Price
    from Order_QtyShip_Detail oqd WITH (NOLOCK) 
    left join Order_UnitPrice ou1 WITH (NOLOCK) on ou1.Id = oqd.Id and ou1.Article = '----' and ou1.SizeCode = '----' 
    left join Order_UnitPrice ou2 WITH (NOLOCK) on ou2.Id = oqd.Id and ou2.Article = oqd.Article and ou2.SizeCode = oqd.SizeCode 
    where oqd.Id = '{0}'
    and oqd.Seq = '{1}'
) a ";
            return sqlCmd;
        }

        private void CanEdit()
        {
            #region 若已經有HC NO.,則不能edit只能Edit "Pullout Date"欄位
            if (this.EditMode && !MyUtility.Check.Empty(this.CurrentMaintain["ExpressID"]))
            {
                this.txtbrand.ReadOnly = true;
                this.txtbrand.IsSupportEditMode = false;

                this.txtshipmode.ReadOnly = true;
                this.txtshipmode.Enabled = false;

                this.numTtlCBM.ReadOnly = true;
                this.numTtlCBM.IsSupportEditMode = false;

                this.numTtlGW.ReadOnly = true;
                this.numTtlGW.IsSupportEditMode = false;

                this.editRemark.ReadOnly = true;
                this.editRemark.IsSupportEditMode = false;

                this.DetailGridEditing(false);
                this.btnBatchImport.Enabled = false;
                this.gridicon.Append.Enabled = false;
                this.gridicon.Insert.Enabled = false;
                this.gridicon.Remove.Enabled = false;
            }
            else if (this.EditMode)
            {
                this.txtbrand.ReadOnly = false;
                this.txtbrand.IsSupportEditMode = true;

                this.txtshipmode.ReadOnly = false;
                this.txtshipmode.Enabled = true;

                this.numTtlCBM.ReadOnly = false;
                this.numTtlCBM.IsSupportEditMode = true;

                this.numTtlGW.ReadOnly = false;
                this.numTtlGW.IsSupportEditMode = true;

                this.editRemark.ReadOnly = false;
                this.editRemark.IsSupportEditMode = true;

                this.DetailGridEditing(true);
                this.btnBatchImport.Enabled = this.EditMode;
                this.gridicon.Append.Enabled = true;
                this.gridicon.Insert.Enabled = true;
                this.gridicon.Remove.Enabled = true;
            }
            else
            {
                this.txtbrand.ReadOnly = true;
                this.txtbrand.IsSupportEditMode = true;

                this.txtshipmode.ReadOnly = true;
                this.txtshipmode.Enabled = true;

                this.numTtlCBM.ReadOnly = true;
                this.numTtlCBM.IsSupportEditMode = true;

                this.numTtlGW.ReadOnly = true;
                this.numTtlGW.IsSupportEditMode = true;

                this.editRemark.ReadOnly = true;
                this.editRemark.IsSupportEditMode = true;

                this.DetailGridEditing(false);
                this.btnBatchImport.Enabled = this.EditMode;
                this.gridicon.Append.Enabled = true;
                this.gridicon.Insert.Enabled = true;
                this.gridicon.Remove.Enabled = true;
            }
            #endregion
        }

        // 控制表身Grid欄位是否可被編輯
        private void DetailGridEditing(bool isEditing)
        {
            if (isEditing)
            {
                this.col_SP.IsEditingReadOnly = false;
                this.col_qty.IsEditingReadOnly = false;
                this.col_Art.IsEditingReadOnly = false;
                this.col_Size.IsEditingReadOnly = false;

                for (int i = 0; i < this.detailgrid.ColumnCount; i++)
                {
                    if (i == 0 || i == 2 || i == 5 || i == 7)
                    {
                        this.detailgrid.Columns[i].DefaultCellStyle.ForeColor = Color.Red;
                    }
                }
            }
            else
            {
                this.col_SP.IsEditingReadOnly = true;
                this.col_qty.IsEditingReadOnly = true;
                this.col_Art.IsEditingReadOnly = true;
                this.col_Size.IsEditingReadOnly = true;
                for (int i = 0; i < this.detailgrid.ColumnCount; i++)
                {
                    this.detailgrid.Columns[i].DefaultCellStyle.ForeColor = Color.Black;
                }
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

                    if (MyUtility.Convert.GetDecimal(dr["BalanceQty"]) < 0)
                    {
                        this.detailgrid.Rows[index].Cells[11].Style.BackColor = Color.Red;
                    }
                }
            }
        }
    }
}
