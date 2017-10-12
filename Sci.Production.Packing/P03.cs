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
using System.IO;


namespace Sci.Production.Packing
{
    public partial class P03 : Sci.Win.Tems.Input6
    {
        const int DescSort = 0, ASCSort = 1;
        Ict.Win.UI.DataGridViewTextBoxColumn col_orderid;
        Ict.Win.UI.DataGridViewTextBoxColumn col_seq;
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
        Ict.Win.DataGridViewGeneratorTextColumnSettings orderid = new Ict.Win.DataGridViewGeneratorTextColumnSettings();
        Ict.Win.DataGridViewGeneratorTextColumnSettings seq = new Ict.Win.DataGridViewGeneratorTextColumnSettings();
        Ict.Win.DataGridViewGeneratorTextColumnSettings article = new Ict.Win.DataGridViewGeneratorTextColumnSettings();
        Ict.Win.DataGridViewGeneratorTextColumnSettings size = new Ict.Win.DataGridViewGeneratorTextColumnSettings();
        IList<string> comboBox1_RowSource = new List<string>();
        BindingSource comboxbs1;
        private MessageBoxButtons buttons = MessageBoxButtons.YesNo;
        private DialogResult buttonResult;
        private DualResult result;
        private Boolean shipmode_Valid = false;
        private int RowIndex = 0, ColumnIndex = 0, detailgridSort = 0;
        public P03(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.DefaultFilter = "MDivisionID = '" + Sci.Env.User.Keyword + "' AND Type = 'B'";
            detailgrid.AllowUserToOrderColumns = true;
            InsertDetailGridOnDoubleClick = false;

            #region CTN# 排序
            this.detailgrid.CellClick += (s, e) =>
            {
                this.RowIndex = e.RowIndex;
                this.ColumnIndex = e.ColumnIndex;
            };
            this.detailgrid.Sorted += (s, e) =>
            {
                if (this.RowIndex == -1 && this.ColumnIndex == 6){
                    if(this.detailgridSort == DescSort)
                    {
                        ((DataTable)((BindingSource)this.detailgrid.DataSource).DataSource).DefaultView.Sort = "sortCTNNo DESC, CTNStartNo  DESC";
                        this.detailgridSort = ASCSort;
                    }
                    else{
                        ((DataTable)((BindingSource)this.detailgrid.DataSource).DataSource).DefaultView.Sort = "sortCTNNo ASC, CTNStartNo ASC";
                        this.detailgridSort = DescSort;
                    }
                }
            };
            #endregion
        }

        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["ID"].ToString();
            this.DetailSelectCommand = Prgs.QueryPackingListSQLCmd(masterID);
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
            comboSortby.DataSource = comboxbs1;

            DataTable queryDT;
            string querySql = string.Format(@"
select '' FTYGroup

union 
select distinct FTYGroup 
from Factory 
where MDivisionID = '{0}'", Sci.Env.User.Keyword);
            DBProxy.Current.Select(null, querySql, out queryDT);
            MyUtility.Tool.SetupCombox(queryfors, 1, queryDT);
            queryfors.SelectedIndex = 0;
            queryfors.SelectedIndexChanged += (s, e) =>
            {
                switch (queryfors.SelectedIndex)
                {
                    case 0:
                        this.DefaultWhere = "";
                        break;
                    default:
                        this.DefaultWhere = string.Format("FactoryID = '{0}'", queryfors.SelectedValue);
                        break;
                }
                this.ReloadDatas();
            };
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();

            labelCofirmed.Visible = MyUtility.Check.Empty(CurrentMaintain["ID"]) ? false : true;

            DataRow dr;
            string sqlStatus = string.Format(@"select * from PackingList WITH (NOLOCK) where id='{0}'", CurrentMaintain["id"].ToString());
            if (MyUtility.Check.Seek(sqlStatus, out dr))
            {
                if (dr["Status"].ToString().ToUpper() == "NEW" && MyUtility.Check.Empty(CurrentMaintain["GMTBookingLock"])) labelCofirmed.Text = "New";
                else if (dr["Status"].ToString().ToUpper() == "CONFIRMED" && !MyUtility.Check.Empty(CurrentMaintain["GMTBookingLock"])) labelCofirmed.Text = "Confirmed";
                else if (dr["Status"].ToString().ToUpper() == "CONFIRMED" && MyUtility.Check.Empty(CurrentMaintain["GMTBookingLock"])) labelCofirmed.Text = "Confirmed";
                else labelCofirmed.Text = "Shipping Lock";
            }


            //Purchase Ctn
            displayPurchaseCtn.Value = MyUtility.Check.Empty(CurrentMaintain["LocalPOID"]) ? "" : "Y";

            //UnConfirm History按鈕變色
            if (MyUtility.Check.Seek(CurrentMaintain["ID"].ToString(), "PackingList_History", "ID"))
            {
                this.btnUnConfirmHistory.ForeColor = Color.Blue;
            }
            else
            {
                this.btnUnConfirmHistory.ForeColor = Color.Black;
            }
            //Carton Summary按鈕變色
            if (MyUtility.Check.Seek(string.Format("select pd.ID from PackingList_Detail pd WITH (NOLOCK) , Order_CTNData oc WITH (NOLOCK) where pd.OrderID = oc.ID and pd.ID = '{0}'", CurrentMaintain["ID"].ToString())))
            {
                this.btnCartonSummary.ForeColor = Color.Blue;
            }
            else
            {
                this.btnCartonSummary.ForeColor = Color.Black;
            }

            //Start Ctn#
            string sqlCmd;
            DataRow orderData;
            sqlCmd = string.Format("select isnull(min(CTNStartNo),0) as CTNStartNo  from PackingList_Detail WITH (NOLOCK) where ID = '{0}'", CurrentMaintain["ID"].ToString());
            if (MyUtility.Check.Seek(sqlCmd, out orderData))
            {
                displayStartCtn.Value = orderData["CTNStartNo"].ToString();
            }
            if (!MyUtility.Check.Empty(CurrentMaintain["GMTBookingLock"]))
            {
                gridicon.Append.Enabled = false;
                gridicon.Insert.Enabled = false;
                gridicon.Remove.Enabled = false;
            }
            else
            {
                gridicon.Append.Enabled = true;
                gridicon.Insert.Enabled = true;
                gridicon.Remove.Enabled = true;
            }
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
                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    //檢查箱子如果有送到Clog則不可以被修改
                    if (e.FormattedValue.ToString() != dr["OrderID"].ToString())
                    {
                        if (!CheckCanCahngeCol(MyUtility.Convert.GetDate(dr["TransferDate"])))
                        {
                            dr["OrderID"] = dr["OrderID"].ToString();
                            e.Cancel = true;
                            return;
                        }
                    }

                    if (!MyUtility.Check.Empty(e.FormattedValue) && e.FormattedValue.ToString() != dr["OrderID"].ToString())
                    {
                        DataRow orderData;
                        if (!MyUtility.Check.Seek(string.Format(@"
Select  ID
        , SeasonID
        , StyleID
        , CustPONo 
        , FtyGroup
from Orders WITH (NOLOCK) 
where   ID = '{0}' 
        and ((Category = 'B' and LocalOrder = 0) or Category = 'S')
        and BrandID = '{1}' 
        and Dest = '{2}' 
        and CustCDID = '{3}'
        and MDivisionID = '{4}'"
                            , e.FormattedValue.ToString(), CurrentMaintain["BrandID"].ToString(), CurrentMaintain["Dest"].ToString(), CurrentMaintain["CustCDID"].ToString(), Sci.Env.User.Keyword), out orderData))
                        {
                            MessageBox.Show(string.Format("< SP No.: {0} > not found!!!", e.FormattedValue.ToString()));
                            dr["OrderID"] = "";
                            dr["OrderShipmodeSeq"] = "";
                            dr["Article"] = "";
                            dr["Color"] = "";
                            dr["SizeCode"] = "";
                            dr["StyleID"] = "";
                            dr["CustPONo"] = "";
                            dr["SeasonID"] = "";
                            dr["Factory"] = "";
                            dr.EndEdit();
                            e.Cancel = true;
                            return;
                        }
                        else
                        {
                            dr["OrderID"] = e.FormattedValue.ToString().ToUpper();
                            dr["Factory"] = orderData["FtyGroup"].ToString();
                            dr["StyleID"] = orderData["StyleID"].ToString();                            
                            dr["CustPONo"] = orderData["CustPONo"].ToString();
                            dr["SeasonID"] = orderData["SeasonID"].ToString();
                            dr["Article"] = "";
                            dr["Color"] = "";
                            dr["SizeCode"] = "";
                            #region 若Order_QtyShip有多筆資料話就跳出視窗讓使者選擇Seq
                            string sqlCmd = string.Format(@"
select count(oq.ID) as CountID 
from Order_QtyShip oq WITH (NOLOCK) inner join orders o WITH (NOLOCK) on oq.id = o.id 
where oq.ID = '{0}' and ShipmodeID = '{1}' and o.MDivisionID = '{2}'"
                                , dr["OrderID"].ToString(), CurrentMaintain["ShipModeID"].ToString(), Sci.Env.User.Keyword);
                            if (MyUtility.Check.Seek(sqlCmd, out orderData))
                            {
                                if (orderData["CountID"].ToString() == "1")
                                {
                                    string sqlCmd2 = string.Format(@"
select seq
from Order_QtyShip oq WITH (NOLOCK) inner join orders o WITH (NOLOCK) on oq.id = o.id 
where oq.ID = '{0}' and ShipmodeID = '{1}' and o.MDivisionID = '{2}'"
                                , dr["OrderID"].ToString(), CurrentMaintain["ShipModeID"].ToString(), Sci.Env.User.Keyword);
                                    if (MyUtility.Check.Seek(sqlCmd2, out orderData))
                                        dr["OrderShipmodeSeq"] = orderData["seq"].ToString();
                                }
                                else
                                {
                                    sqlCmd = string.Format(@"
select Seq,oq.BuyerDelivery,ShipmodeID,oq.Qty 
from Order_QtyShip oq WITH (NOLOCK) inner join orders o WITH (NOLOCK) on oq.id = o.id 
where oq.ID = '{0}' and ShipmodeID = '{1}' and o.MDivisionID = '{2}'"
                                        , dr["OrderID"].ToString(), CurrentMaintain["ShipModeID"].ToString(), Sci.Env.User.Keyword);
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
                }
            };

            //Seq
            seq.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && MyUtility.Check.Empty(CurrentMaintain["GMTBookingLock"]))
                {
                    if (e.Button == System.Windows.Forms.MouseButtons.Right)
                    {
                        if (e.RowIndex != -1)
                        {
                            DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                            string sqlCmd = string.Format(@"
select  oq.Seq
        , oq.BuyerDelivery
        , oq.ShipmodeID
        , oq.Qty 
from Order_QtyShip oq WITH (NOLOCK) 
inner join orders o WITH (NOLOCK) on oq.id = o.id  
where   oq.ID = '{0}' 
        and oq.ShipmodeID = '{1}' 
        and o.MDivisionID = '{2}'", dr["OrderID"].ToString(), CurrentMaintain["ShipModeID"].ToString(), Sci.Env.User.Keyword);
                            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlCmd, "4,20,20,10", "", "Seq,Buyer Delivery,ShipMode,Qty");
                            DialogResult returnResult = item.ShowDialog();
                            if (returnResult == DialogResult.Cancel) { return; }
                            e.EditingControl.Text = item.GetSelectedString();
                            dr["OrderShipmodeSeq"] = e.EditingControl.Text;
                            if (e.EditingControl.Text != dr["OrderShipmodeSeq"].ToString())
                            {
                                if (!CheckCanCahngeCol(MyUtility.Convert.GetDate(dr["TransferDate"])))
                                {
                                    dr["OrderShipmodeSeq"] = dr["OrderShipmodeSeq"].ToString();
                                    return;
                                }
                                else
                                {
                                    dr["Article"] = "";
                                    dr["Color"] = "";
                                    dr["SizeCode"] = "";
                                    dr.EndEdit();
                                }
                            }
                        }
                    }
                }
            };

            seq.CellValidating += (s, e) =>
            {
                if (EditMode) {
                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    DataRow chk_dr;
                    string sqlCmd = string.Format(@"
select  oq.Seq
        , oq.BuyerDelivery
        , oq.ShipmodeID
        , oq.Qty 
from Order_QtyShip oq WITH (NOLOCK) 
inner join orders o WITH (NOLOCK) on oq.id = o.id  
where   oq.ID = '{0}' 
        and oq.ShipmodeID = '{1}' 
        and o.MDivisionID = '{2}'
        and oq.Seq = '{3}'", dr["OrderID"].ToString(), CurrentMaintain["ShipModeID"].ToString(), Sci.Env.User.Keyword, e.FormattedValue);
                    if (!MyUtility.Check.Seek(sqlCmd, out chk_dr))
                    {
                        dr["Seq"] = "";
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox("< Seq:" + e.FormattedValue + " > not found!!!");
                        return;
                    }
                }
               

            };


            article.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && article.IsEditingReadOnly == false)
                {
                    if (e.Button == System.Windows.Forms.MouseButtons.Right)
                    {
                        if (e.RowIndex != -1)
                        {
                            DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                            string sqlCmd = string.Format("Select Distinct Article from Order_QtyShip_Detail WITH (NOLOCK) where ID = '{0}' and Seq = '{1}'", dr["OrderID"].ToString(), dr["OrderShipmodeSeq"].ToString());
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
                        if (!CheckCanCahngeCol(MyUtility.Convert.GetDate(dr["TransferDate"])))
                        {
                            dr["Article"] = dr["Article"].ToString();
                            e.Cancel = true;
                            return;
                        }
                    }

                    if (!MyUtility.Check.Empty(e.FormattedValue) && e.FormattedValue.ToString() != dr["Article"].ToString())
                    {
                        if (!MyUtility.Check.Seek(string.Format("Select Article from Order_QtyShip_Detail WITH (NOLOCK) where ID = '{0}' and Seq = '{1}' and Article = '{2}'", dr["OrderID"].ToString(), dr["OrderShipmodeSeq"].ToString(), e.FormattedValue.ToString())))
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

            size.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && size.IsEditingReadOnly == false)
                {
                    if (e.Button == System.Windows.Forms.MouseButtons.Right)
                    {
                        if (e.RowIndex != -1)
                        {
                            DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                            string sqlCmd = string.Format(@"Select oqd.SizeCode 
from Order_QtyShip_Detail oqd WITH (NOLOCK) 
left join Orders o WITH (NOLOCK) on o.ID = oqd.Id
left join Order_SizeCode os WITH (NOLOCK) on os.ID = o.POID and os.SizeCode = oqd.SizeCode
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
                    //檢查箱子如果有送到Clog則不可以被修改
                    if (e.FormattedValue.ToString() != dr["SizeCode"].ToString())
                    {
                        if (!CheckCanCahngeCol(MyUtility.Convert.GetDate(dr["TransferDate"])))
                        {
                            dr["SizeCode"] = dr["SizeCode"].ToString();
                            e.Cancel = true;
                            return;
                        }
                    }
                    if (!MyUtility.Check.Empty(e.FormattedValue) && e.FormattedValue.ToString() != dr["SizeCode"].ToString())
                    {
                        if (!MyUtility.Check.Seek(string.Format("Select SizeCode from Order_QtyShip_Detail WITH (NOLOCK) where ID = '{0}' and Seq = '{1}' and Article = '{2}' and SizeCode = '{3}'", dr["OrderID"].ToString(), dr["OrderShipmodeSeq"].ToString(), dr["Article"].ToString(), e.FormattedValue.ToString())))
                        {
                            dr["SizeCode"] = "";
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox(string.Format("< SizeCode: {0} > not found!!!", e.FormattedValue.ToString()));
                            return;
                        }
                    }
                }
            };
            #endregion

            Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("Factory", header: "Factory", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("OrderID", header: "SP No.", width: Widths.AnsiChars(13), settings: orderid).Get(out col_orderid)
                .Text("SeasonID", header: "Season", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("OrderShipmodeSeq", header: "Seq", width: Widths.AnsiChars(2), iseditingreadonly: false, settings: seq).Get(out col_seq)
                .Text("StyleID", header: "Style No.", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("CustPONo", header: "P.O. No.", width: Widths.AnsiChars(10), iseditingreadonly: true)
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
                                if (!CheckCanCahngeCol(MyUtility.Convert.GetDate(dr["TransferDate"])))
                                {
                                    dr["CTNStartNo"] = dr["CTNStartNo"].ToString();
                                    e.Cancel = true;
                                    return;
                                }
                            }
                        }
                    }
                    if (detailgrid.Columns[e.ColumnIndex].DataPropertyName == col_ctnqty.DataPropertyName)
                    {
                        if (e.FormattedValue.ToString() != dr["CTNQty"].ToString())
                        {
                            if (!CheckCanCahngeCol(MyUtility.Convert.GetDate(dr["TransferDate"])))
                            {
                                dr["CTNQty"] = dr["CTNQty"].ToString();
                                e.Cancel = true;
                                return;
                            }
                        }
                    }

                    if (detailgrid.Columns[e.ColumnIndex].DataPropertyName == col_qtyperctn.DataPropertyName)
                    {
                        if (!MyUtility.Check.Empty(e.FormattedValue))
                        {
                            if (e.FormattedValue.ToString() != dr["QtyPerCTN"].ToString())
                            {
                                if (!CheckCanCahngeCol(MyUtility.Convert.GetDate(dr["TransferDate"])))
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
                                if (!CheckCanCahngeCol(MyUtility.Convert.GetDate(dr["TransferDate"])))
                                {
                                    dr["ShipQty"] = dr["ShipQty"].ToString();
                                    e.Cancel = true;
                                    return;
                                }
                            }
                        }
                    }
                    #endregion

                    //# of CTN只能輸入0或1
                    if (detailgrid.Columns[e.ColumnIndex].DataPropertyName == col_ctnqty.DataPropertyName)
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
                        string seekSql = string.Format("select Description,CtnWeight from LocalItem WITH (NOLOCK) where RefNo = '{0}'", dr["RefNo"].ToString());
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

            //this.detailgrid.CellFormatting += (s, e) =>
            //{
            //    if (this.EditMode)
            //    {
            //        this.detailgrid.Rows[e.RowIndex].Cells["OrderShipmodeSeq"].Style.ForeColor = Color.Red;
            //    }else
            //    {
            //        this.detailgrid.Rows[e.RowIndex].Cells["OrderShipmodeSeq"].Style.ForeColor = Color.Black;
            //    }
            //};
        }

        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["MDivisionID"] = Sci.Env.User.Keyword;
            CurrentMaintain["Type"] = "B";
            CurrentMaintain["FactoryID"] = Sci.Env.User.Factory;
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
            comboSortby.Text = "";
            shipmode_Valid = false;
            if (CurrentMaintain["ID"].ToString().Substring(3, 2).ToUpper() == "PG")
            {
                txtbrand.ReadOnly = true;
                txtcustcd.ReadOnly = true;
                txtcountry.TextBox1.ReadOnly = true;
            }
            //部分欄位會依某些條件來決定是否可以被修改
            if (!MyUtility.Check.Empty(CurrentMaintain["GMTBookingLock"]))
            {
                txtbrand.ReadOnly = true;
                txtcustcd.ReadOnly = true;
                txtcountry.TextBox1.ReadOnly = true;
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
            }

            if (!MyUtility.Check.Empty(CurrentMaintain["LocalPOID"]))
            {
                dateCartonEstBooking.ReadOnly = true;
                dateCartonEstArrived.ReadOnly = true;
            }
        }

        private StringBuilder ctn_no_combine(string SP,string Seq) {
            StringBuilder ctn_no = new StringBuilder();
            var cnt_list = from r2 in DetailDatas.AsEnumerable()
                           where r2.Field<string>("OrderID") == SP &&
                                  r2.Field<string>("OrderShipmodeSeq") == Seq
                           select new { cnt_no = r2.Field<string>("CTNStartNo") };
            foreach (var cnt in cnt_list)
            {
                ctn_no.Append("," + cnt.cnt_no);
            }
            ctn_no.Remove(0, 1);
            return ctn_no;
        }

        protected override bool ClickSaveBefore()
        {
            //檢查欄位值不可為空
            if (MyUtility.Check.Empty(CurrentMaintain["BrandID"]))
            {
                txtbrand.Focus();
                MyUtility.Msg.WarningBox("Brand can't empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["CustCDID"]))
            {
                txtcustcd.Focus();
                MyUtility.Msg.WarningBox("CustCD can't empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["Dest"]))
            {
                txtcountry.Focus();
                MyUtility.Msg.WarningBox("Destination can't empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["ShipModeID"]))
            {
                txtshipmode.Focus();
                MyUtility.Msg.WarningBox("Ship Mode can't empty!!");
                return false;
            }


            //刪除表身SP No.或Qty為空白的資料，表身的CTN#, Ref No., Color Way與Size不可以為空值，計算CTNQty, ShipQty, NW, GW, NNW, CBM，重算表身Grid的Bal. Qty
            int i = 0, ctnQty = 0, shipQty = 0, ttlShipQty = 0, needPackQty = 0, count = 0;
            double nw = 0.0, gw = 0.0, nnw = 0.0, cbm = 0.0;
            string filter = "", sqlCmd;
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

            ////計算needPackQty
            //string OrderIDs = string.Empty;
            //foreach (DataRow dr in DetailDatas) OrderIDs += "'" + dr["OrderID"].ToString().Trim() + "',";
            //OrderIDs = OrderIDs.TrimEnd(',');
            //if (MyUtility.Check.Empty(OrderIDs))
            //{
            //    MyUtility.Msg.WarningBox("OrderID is empty,can't save! ");
            //    return false;
            //}
            //sqlCmd = string.Format("select SUM(ShipQty) as Qty from PackingList_Detail WITH (NOLOCK) where OrderID IN ({0})", OrderIDs);
            //needPackQty = MyUtility.Convert.GetInt(MyUtility.GetValue.Lookup(sqlCmd));

           


             foreach (DataRow dr in DetailDatas.OrderBy(u => u["ID"]).ThenBy(u => u["OrderShipmodeSeq"]))
            {
                #region 刪除表身SP No.或Qty為空白的資料
                if (MyUtility.Check.Empty(dr["OrderID"]) || MyUtility.Check.Empty(dr["ShipQty"]))
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

                #region 填入Seq欄位值
                i = i + 1;
                dr["Seq"] = Convert.ToString(i).PadLeft(6, '0');
                #endregion

                #region 計算CTNQty, ShipQty, NW, GW, NNW, CBM
                ctnQty = ctnQty + MyUtility.Convert.GetInt(dr["CTNQty"]);
                shipQty = shipQty + MyUtility.Convert.GetInt(dr["ShipQty"]);
                nw = MyUtility.Math.Round(nw + MyUtility.Convert.GetDouble(dr["NW"]), 3);
                gw = MyUtility.Math.Round(gw + MyUtility.Convert.GetDouble(dr["GW"]), 3);
                nnw = MyUtility.Math.Round(nnw + MyUtility.Convert.GetDouble(dr["NNW"]), 3);
                if (MyUtility.Check.Empty(dr["CTNQty"]) || MyUtility.Convert.GetInt(dr["CTNQty"]) > 0)
                {
                    cbm = MyUtility.Math.Round(cbm + (MyUtility.Math.Round(MyUtility.Convert.GetDouble(MyUtility.GetValue.Lookup("CBM", dr["RefNo"].ToString(), "LocalItem", "RefNo")), 3) * MyUtility.Convert.GetInt(dr["CTNQty"])), 4);
                }

                #endregion

                #region 重算表身Grid的Bal. Qty
                //目前還有多少衣服尚未裝箱
                //needPackQty = 0;
                filter = string.Format("OrderID = '{0}' and OrderShipmodeSeq = '{1}'", dr["OrderID"].ToString(), dr["OrderShipmodeSeq"].ToString());
                detailData = needPackData.Select(filter);
                if (detailData.Length <= 0)
                {
                    //撈取此SP+Seq尚未裝箱的數量
                    sqlCmd = string.Format(@"
select OrderID = oqd.Id
	   , OrderShipmodeSeq = oqd.Seq
	   , oqd.Article
	   , oqd.SizeCode
	   , Qty = (oqd.Qty - isnull(sum(pld.ShipQty),0) - isnull(sum(iaq.DiffQty), 0))
from Order_QtyShip_Detail oqd WITH (NOLOCK) 
left join PackingList_Detail pld WITH (NOLOCK) on pld.OrderID = oqd.Id 
												  and pld.OrderShipmodeSeq = oqd.Seq 
												  and pld.ID != '{0}'
left join InvAdjust ia WITH (NOLOCK) on ia.OrderID = oqd.ID 
										and ia.OrderShipmodeSeq = oqd.Seq
left join InvAdjust_Qty iaq WITH (NOLOCK) on iaq.ID = ia.ID 
											 and iaq.Article = oqd.Article 
											 and iaq.SizeCode = oqd.SizeCode
where oqd.Id = '{1}'
	  and oqd.Seq = '{2}'
group by oqd.Id, oqd.Seq, oqd.Article, oqd.SizeCode, oqd.Qty", CurrentMaintain["ID"].ToString(), dr["OrderID"].ToString(), dr["OrderShipmodeSeq"].ToString());
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
                    needPackQty = MyUtility.Convert.GetInt(detailData[0]["Qty"].ToString());
                }

                //加總表身特定Article/SizeCode的Ship Qty數量
                detailData = ((DataTable)detailgridbs.DataSource).Select(filter);
                ttlShipQty = 0;
                if (detailData.Length != 0)
                {
                    foreach (DataRow dDr in detailData)
                    {
                        ttlShipQty = ttlShipQty + MyUtility.Convert.GetInt(dDr["ShipQty"].ToString());
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


             #region ship mode 有變更時 check Order_QtyShip

             //if (shipmode_Valid)
             //{
                 string seekSql = "";
                 StringBuilder chk_ship_err = new StringBuilder();
                 StringBuilder chk_seq_null = new StringBuilder();
                 DataRow localItem;
                 


                 var check_chip_list = from r1 in DetailDatas.AsEnumerable()
                                       group r1 by new
                                       {
                                           SP = r1.Field<string>("OrderID"),
                                           Seq = r1.Field<string>("OrderShipmodeSeq")

                                       } into g
                                       select new
                                       {
                                           SP = g.Key.SP,
                                           Seq = g.Key.Seq
                                       };
                 foreach (var chk_item in check_chip_list)
                 {
                  

                     seekSql = string.Format("select ShipmodeID from Order_QtyShip WITH (NOLOCK) where ID = '{0}' and seq = '{1}' ", chk_item.SP, chk_item.Seq);
                     if ( MyUtility.Check.Empty(chk_item.Seq))
                     {
                         chk_seq_null.Append("<SP> " + chk_item.SP + " <CTN#> [" + ctn_no_combine(chk_item.SP, chk_item.Seq) + "]  \r\n");
                     }

                     if (shipmode_Valid && chk_seq_null.Length == 0)
                     {
                         if (!MyUtility.Check.Seek(seekSql, out localItem) )
                         {

                             chk_ship_err.Append("<SP> " + chk_item.SP + " <Seq> " + chk_item.Seq + " <CTN#> [" + ctn_no_combine(chk_item.SP, chk_item.Seq) + "] <ShipMode> [] \r\n");
                         }
                         else
                         {
                             if (CurrentMaintain["ShipModeID"].ToString() != localItem["ShipmodeID"].ToString())
                             {

                                 chk_ship_err.Append("<SP> " + chk_item.SP + " <Seq> " + chk_item.Seq + " <CTN#> [" + ctn_no_combine(chk_item.SP, chk_item.Seq) + "] <ShipMode> [" + localItem["ShipmodeID"].ToString() + "] \r\n");
                             }

                         }
                     }
                     

                 }

                 if (chk_seq_null.Length > 0)
                 {
                     chk_seq_null.Insert(0, " Seq can not empty , please check again:  \r\n");

                     MyUtility.Msg.WarningBox(chk_seq_null.ToString());
                     return false;
                 }

                if (chk_ship_err.Length > 0)
                {
                    chk_ship_err.Insert(0, " This shipmode is not equal, please check again:  \r\n");

                    MyUtility.Msg.WarningBox(chk_ship_err.ToString());
                    return false;
                }
             //}
             #endregion


             if (DetailDatas.Count == 0)
            {
                MyUtility.Msg.InfoBox("Detail cannot be empty");
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
            //表身重新計算後,再判斷CBM or GW 是不是0
            if (MyUtility.Check.Empty(CurrentMaintain["CBM"]) || MyUtility.Check.Empty(CurrentMaintain["GW"]))
            {
                numTtlCBM.Focus();
                MyUtility.Msg.WarningBox("Ttl CBM and Ttl GW can't be empty!!");
                return false;
            }
            return base.ClickSaveBefore();
        }

        protected override DualResult ClickSavePre()
        {



            if (!MyUtility.Check.Empty(CurrentMaintain["INVNo"]))
            {
                string sqlCmd = string.Format(@"select isnull(sum(ShipQty),0) as ShipQty,isnull(sum(CTNQty),0) as CTNQty,isnull(sum(NW),0) as NW,isnull(sum(GW),0) as GW,isnull(sum(NNW),0) as NNW,isnull(sum(CBM),0) as CBM
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
                    sp3.Value = MyUtility.Math.Round(MyUtility.Convert.GetDouble(summaryData.Rows[0]["NW"]) + MyUtility.Convert.GetDouble(CurrentMaintain["NW"]), 2);

                    System.Data.SqlClient.SqlParameter sp4 = new System.Data.SqlClient.SqlParameter();
                    sp4.ParameterName = "@ttlNNW";
                    sp4.Value = MyUtility.Math.Round(MyUtility.Convert.GetDouble(summaryData.Rows[0]["NNW"]) + MyUtility.Convert.GetDouble(CurrentMaintain["NNW"]), 2);

                    System.Data.SqlClient.SqlParameter sp5 = new System.Data.SqlClient.SqlParameter();
                    sp5.ParameterName = "@ttlGW";
                    sp5.Value = MyUtility.Math.Round(MyUtility.Convert.GetDouble(summaryData.Rows[0]["GW"]) + MyUtility.Convert.GetDouble(CurrentMaintain["GW"]), 2);

                    System.Data.SqlClient.SqlParameter sp6 = new System.Data.SqlClient.SqlParameter();
                    sp6.ParameterName = "@ttlCBM";
                    sp6.Value = MyUtility.Math.Round(MyUtility.Convert.GetDouble(summaryData.Rows[0]["CBM"]) + MyUtility.Convert.GetDouble(CurrentMaintain["CBM"]), 2);

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
                    DualResult failResult = new DualResult(false, "Select PackingList fail!!\r\n" + result.ToString());
                    return failResult;
                }
            }
            return Result.True;
        }

        protected override DualResult ClickSavePost()
        {
            //存檔成功後，要再呼叫UpdateOrdersCTN, CreateOrderCTNData
            DataTable OrderData;
            string sqlCmd = string.Format("select distinct OrderID from PackingList_Detail WITH (NOLOCK) where ID = '{0}'", CurrentMaintain["ID"].ToString());
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out OrderData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "select order list fail!\r\n" + result.ToString());
                return failResult;
            }
            result = Prgs.UpdateOrdersCTN(OrderData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Update Orders CTN fail!\r\n" + result.ToString());
                return failResult;
            }
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

        protected override DualResult ClickDeletePost()
        {
            DataTable OrderData;
            string sqlCmd = string.Format("select distinct OrderID from PackingList_Detail WITH (NOLOCK) where ID = '{0}'", CurrentMaintain["ID"].ToString());
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out OrderData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "select order list fail!\r\n" + result.ToString());
                return failResult;
            }
            result = Prgs.UpdateOrdersCTN(OrderData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Update Orders CTN fail!\r\n" + result.ToString());
                return failResult;
            }
            return Result.True;
        }

        protected override bool ClickPrint()
        {
            int orderQty = MyUtility.Convert.GetInt(MyUtility.GetValue.Lookup(string.Format(@"select isnull(oq.Qty ,0) as Qty
from (select distinct OrderID,OrderShipmodeSeq from PackingList_Detail WITH (NOLOCK) where ID = '{0}') a
left join Order_QtyShip oq WITH (NOLOCK) on oq.Id = a.OrderID and oq.Seq = a.OrderShipmodeSeq", MyUtility.Convert.GetString(CurrentMaintain["ID"]))));
            Sci.Production.Packing.P03_Print callNextForm = new Sci.Production.Packing.P03_Print(CurrentMaintain, orderQty);
            callNextForm.ShowDialog(this);
            return base.ClickPrint();
        }

        //表身Grid的Delete
        protected override void OnDetailGridDelete()
        {
            //檢查此筆記錄是否已Transfer to Clog，若是則出訊息告知且無法刪除
            if (this.DetailDatas.Count > 0)
            {
                if (!MyUtility.Check.Empty(CurrentDetailData["TransferDate"]))
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
                col_orderid.IsEditingReadOnly = false;
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
                    if (i == 0 || i == 1 || i == 4 || i == 5 || i == 6 || i == 8 || i == 10 || i == 11 || i == 12 || i == 14 || i == 15 || i == 16 || i == 17)
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
                    if (i != 17)
                    {
                        detailgrid.Columns[i].DefaultCellStyle.ForeColor = Color.Black;
                    }
                }
                col_refno.EditingMouseDown -= new EventHandler<Ict.Win.UI.DataGridViewEditingControlMouseEventArgs>(CartonRefnoCommon.EditingMouseDown);
            }
        }

        //Brand
        private void txtbrand_Validated(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(txtbrand.OldValue)) return;
            if (EditMode && txtbrand.OldValue != txtbrand.Text)
            {
                DeleteDetailData(txtbrand, txtbrand.OldValue);
            }
        }

        //CustCD
        private void txtcustcd_Validated(object sender, EventArgs e)
        {
            if (this.EditMode && !MyUtility.Check.Empty(txtcustcd.Text) && txtcustcd.OldValue != txtcustcd.Text)
            {
                CurrentMaintain["Dest"] = MyUtility.GetValue.Lookup(string.Format("SELECT CountryID FROM CustCD WITH (NOLOCK) WHERE BrandID = '{0}' AND ID = '{1}'", MyUtility.Convert.GetString(CurrentMaintain["BrandID"]), txtcustcd.Text));
            }
            if (MyUtility.Check.Empty(txtcustcd.OldValue)) return;
            if (EditMode && txtcustcd.OldValue != txtcustcd.Text)
            {
                DeleteDetailData(txtcustcd, txtcustcd.OldValue);
            }
        }

        //Destination
        private void txtcountry1_Validated(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(txtcountry.TextBox1.OldValue)) return;
            if (EditMode && txtcountry.TextBox1.OldValue != txtcountry.TextBox1.Text)
            {
                DeleteDetailData(txtcountry.TextBox1, txtcountry.TextBox1.OldValue);
                txtcountry.DataBindings.Cast<Binding>().ToList().ForEach(binding => binding.WriteValue());
            }
        }

        //ShipMode       
        
        private void txtshipmode_Validated(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(txtshipmode.OldValue)) return;
            if (MyUtility.Check.Empty(DetailDatas) || DetailDatas.Count == 0) return;
            if (EditMode && txtshipmode.OldValue != txtshipmode.SelectedValue)
            {
                ////if (MyUtility.Check.Empty(DetailDatas.Count)) return;
                //string tempOldValue = txtshipmode.OldValue.ToString();

                //DialogResult diresult = MyUtility.Msg.QuestionBox("The detail SEQ will be cleared, are you sure change type?");
                //if (diresult == DialogResult.No)
                //{
                //    txtshipmode.OldValue = tempOldValue;
                //    txtshipmode.SelectedValue = tempOldValue;
                //    txtshipmode.DataBindings.Cast<Binding>().ToList().ForEach(binding => binding.WriteValue());
                //    return;
                //}
                //// 清空表身Grid資料                
                //foreach (DataRow dr in DetailDatas)
                //{
                //    dr["OrderShipmodeSeq"] = "";
                //}
                //DeleteDetailData();
                shipmode_Valid = true;
            }
        }

        //刪除表身Grid的資料
        private void DeleteDetailData(TextBoxBase textbox, string oldvalue)
        {
            DialogResult diresult = MyUtility.Msg.QuestionBox("The detail grid will be cleared, are you sure change type?");
            if (diresult == DialogResult.No)
            {
                textbox.Text = oldvalue;
                textbox.DataBindings.Cast<Binding>().ToList().ForEach(binding => binding.WriteValue());
                return;
            }
            // 清空表身Grid資料
            foreach (DataRow dr in DetailDatas)
            {
                dr.Delete();
            }
        }

        //檢查箱子是否有送至Clog來決定欄位是否可被修改
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

        //Sort by
        private void comboSortby_SelectedIndexChanged(object sender, EventArgs e)
        {
            labelLocateforTransferClog.Visible = true;
            txtLocateforTransferClog.Visible = false;
            dateLocateforTransferClog.Visible = false;
            btnFindNow.Visible = true;
            switch (comboSortby.SelectedValue.ToString())
            {
                case "Transfer Clog":
                    labelLocateforTransferClog.Text = "Locate for Transfer Clog:";
                    labelLocateforTransferClog.Width = 156;
                    dateLocateforTransferClog.Visible = true;
                    dateLocateforTransferClog.Location = new System.Drawing.Point(448, 193);
                    btnFindNow.Location = new System.Drawing.Point(591, 188);
                    ((DataTable)detailgridbs.DataSource).DefaultView.Sort = "TransferDate,Seq";
                    break;
                case "Clog Cfm":
                    labelLocateforTransferClog.Text = "Locate for Clog Cfm:";
                    labelLocateforTransferClog.Width = 129;
                    dateLocateforTransferClog.Visible = true;
                    dateLocateforTransferClog.Location = new System.Drawing.Point(420, 193);
                    btnFindNow.Location = new System.Drawing.Point(563, 188);
                    ((DataTable)detailgridbs.DataSource).DefaultView.Sort = "ReceiveDate,Seq";
                    break;
                case "Location No":
                    labelLocateforTransferClog.Text = "Locate for Location No:";
                    labelLocateforTransferClog.Width = 147;
                    txtLocateforTransferClog.Visible = true;
                    txtLocateforTransferClog.Location = new System.Drawing.Point(438, 193);
                    btnFindNow.Location = new System.Drawing.Point(537, 188);
                    ((DataTable)detailgridbs.DataSource).DefaultView.Sort = "ClogLocationId,Seq";
                    break;
                case "ColorWay":
                    labelLocateforTransferClog.Text = "Locate for ColorWay:";
                    labelLocateforTransferClog.Width = 135;
                    txtLocateforTransferClog.Visible = true;
                    txtLocateforTransferClog.Location = new System.Drawing.Point(426, 193);
                    btnFindNow.Location = new System.Drawing.Point(525, 188);
                    ((DataTable)detailgridbs.DataSource).DefaultView.Sort = "Article,Seq";
                    break;
                case "Color":
                    labelLocateforTransferClog.Text = "Locate for Color:";
                    labelLocateforTransferClog.Width = 106;
                    txtLocateforTransferClog.Visible = true;
                    txtLocateforTransferClog.Location = new System.Drawing.Point(397, 193);
                    btnFindNow.Location = new System.Drawing.Point(496, 188);
                    ((DataTable)detailgridbs.DataSource).DefaultView.Sort = "Color,Seq";
                    break;
                case "Size":
                    labelLocateforTransferClog.Text = "Locate for Size:";
                    labelLocateforTransferClog.Width = 100;
                    txtLocateforTransferClog.Visible = true;
                    txtLocateforTransferClog.Location = new System.Drawing.Point(391, 193);
                    btnFindNow.Location = new System.Drawing.Point(490, 188);
                    ((DataTable)detailgridbs.DataSource).DefaultView.Sort = "SizeCode,Seq";
                    break;
                default:
                    labelLocateforTransferClog.Visible = false;
                    btnFindNow.Visible = false;
                    if (MyUtility.Check.Empty((DataTable)detailgridbs.DataSource)) break;
                    ((DataTable)detailgridbs.DataSource).DefaultView.Sort = "Seq";
                    break;
            }
        }

        //Carton Summary
        private void btnCartonSummary_Click(object sender, EventArgs e)
        {
            Sci.Production.Packing.P03_CartonSummary callNextForm = new Sci.Production.Packing.P03_CartonSummary(CurrentMaintain["ID"].ToString());
            callNextForm.ShowDialog(this);
        }

        //UnConfirm History
        private void btnUnConfirmHistory_Click(object sender, EventArgs e)
        {
            //Bug fix:0000276: PACKING_P03_Packing List Weight & Summary(Bulk)，1.點選[Unconfirm history]會出現錯誤訊息。
            //Sci.Win.UI.ShowHistory callNextForm = new Sci.Win.UI.ShowHistory("PackingList_History", CurrentMaintain["ID"].ToString(), "Status", caption: "UnConfirm History", haveRemark: true, customerGridFormatTable: "HisType", moduleName: "PackingList");
            Sci.Win.UI.ShowHistory callNextForm = new Sci.Win.UI.ShowHistory("PackingList_History", CurrentMaintain["ID"].ToString(), "Status", caption: "UnConfirm History", haveRemark: true, customerGridFormatTable: "HisType", moduleName: "Packing");
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

            Prgs.RecaluateCartonWeight(((DataTable)detailgridbs.DataSource), CurrentMaintain);
        }

        //Confirm
        protected override void ClickConfirm()
        {
            base.ClickConfirm();

            //訂單M別與登入系統M別不一致時，不可以Confirm
            DataTable DiffMOrder;
            string sqlCmd = string.Format("select distinct pd.OrderID from PackingList_Detail pd WITH (NOLOCK) , Orders o WITH (NOLOCK) where pd.ID = '{0}' and pd.OrderID = o.ID and o.MDivisionID <> '{1}'", CurrentMaintain["ID"].ToString(), Sci.Env.User.Keyword);
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out DiffMOrder);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Sql connection fail!\r\n" + result.ToString());
                return;
            }
            else
            {
                if (DiffMOrder.Rows.Count > 0)
                {
                    StringBuilder orderList = new StringBuilder();
                    foreach (DataRow dr in DiffMOrder.Rows)
                    {
                        orderList.Append(string.Format("\r\n{0}", dr["OrderID"].ToString()));
                    }
                    MyUtility.Msg.WarningBox("This SP's M not equal to login system M so can't confirm!" + orderList.ToString());
                    return;
                }
            }

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

            sqlCmd = string.Format("update PackingList set Status = 'Confirmed', EditName = '{0}', EditDate = '{1}' where ID = '{2}'", Sci.Env.User.UserID, DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), CurrentMaintain["ID"].ToString());
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
            SelectReason();


        }

        private void SelectReason()
        {

            Sci.Win.UI.SelectReason callReason = new Sci.Win.UI.SelectReason();
            DialogResult dResult = callReason.ShowDialog(this);
            if (dResult == System.Windows.Forms.DialogResult.OK)
            {
                string reasonRemark = callReason.ReturnRemark;

                if (MyUtility.Check.Empty(reasonRemark))
                {
                    MyUtility.Msg.InfoBox("Remark cannot be empty!");
                    SelectReason();
                    //return;
                }
                else
                {
                    using (TransactionScope transactionScope = new TransactionScope())
                    {
                        try
                        {
                            string InsertCmd = @"insert into PackingList_History (ID, HisType, OldValue, NewValue, Remark, AddName, AddDate)
 values (@id,@hisType,@oldValue,@newValue,@remark,@addName,GETDATE())";
                            string updateCmd = @"update PackingList set Status = 'New', EditName = @addName, EditDate = GETDATE() where ID = @id";

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

                            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                            cmds.Add(sp1);
                            cmds.Add(sp2);
                            cmds.Add(sp3);
                            cmds.Add(sp4);
                            cmds.Add(sp5);
                            cmds.Add(sp6);
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
                                transactionScope.Dispose();
                                MyUtility.Msg.WarningBox("UnConfirm failed, Pleaes re-try");
                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            transactionScope.Dispose();
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
        }

        //Find Now
        private void btnFindNow_Click(object sender, EventArgs e)
        {

            int index;
            switch (comboSortby.SelectedValue.ToString())
            {
                case "Transfer Clog":
                    index = detailgridbs.Find("TransferDate", dateLocateforTransferClog.Value.ToString());
                    break;
                case "Clog Cfm":
                    index = detailgridbs.Find("ReceiveDate", dateLocateforTransferClog.Value.ToString());
                    break;
                case "Location No":
                    index = detailgridbs.Find("ClogLocationId", txtLocateforTransferClog.Text.Trim());
                    break;
                case "ColorWay":
                    index = detailgridbs.Find("Article", txtLocateforTransferClog.Text.Trim());
                    break;
                case "Color":
                    index = detailgridbs.Find("Color", txtLocateforTransferClog.Text.Trim());
                    break;
                case "Size":
                    index = detailgridbs.Find("SizeCode", txtLocateforTransferClog.Text.Trim());
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

        private void btnDownloadSample_Click(object sender, EventArgs e)
        {
            string strXltName = Sci.Env.Cfg.XltPathDir + "\\Packing_P03_ImportExcelFormat.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null) return;
            excel.Visible = true;
        }

        private void btnImportFromExcel_Click(object sender, EventArgs e)
        {
            #region chech Brand, CustCD, Destination, ShipMode not empty
            List<string> errMsg = new List<string>();
            if (MyUtility.Check.Empty(txtbrand.Text.ToString()))
                errMsg.Add("< Brand >");
            if (MyUtility.Check.Empty(txtcustcd.Text.ToString()))
                errMsg.Add("< CustCD >");
            if (MyUtility.Check.Empty(txtcountry.TextBox1.Text.ToString()))
                errMsg.Add("< Destination >");
            if (MyUtility.Check.Empty(txtshipmode.Text.ToString()))
                errMsg.Add("< Ship Mode >");
            if (errMsg.Count > 0)
            {
                MyUtility.Msg.InfoBox(errMsg.JoinToString("\n") + " Can not be Empty!!");
                return;
            }
            #endregion

            Sci.Production.Packing.P03_ExcelImport nextForm = new Sci.Production.Packing.P03_ExcelImport(CurrentMaintain, (DataTable)detailgridbs.DataSource);
            nextForm.ShowDialog(this);
        }

    }
}
