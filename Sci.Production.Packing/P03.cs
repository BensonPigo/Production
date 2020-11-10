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
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Sci.Production.Automation;

namespace Sci.Production.Packing
{
    /// <summary>
    /// Packing_P03
    /// </summary>
    public partial class P03 : Win.Tems.Input6
    {
        private const int DescSort = 0;
        private const int ASCSort = 1;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_orderid;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_seq;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_ctnno;
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
        private IList<string> comboBox1_RowSource = new List<string>();
        private BindingSource comboxbs1;
        private MessageBoxButtons buttons = MessageBoxButtons.YesNo;
        private DialogResult buttonResult;
        private DualResult result;
        private int RowIndex = 0;
        private int ColumnIndex = 0;
        private int detailgridSort = 0;

        /// <summary>
        /// ComboBox1_RowSource
        /// </summary>
        public IList<string> ComboBox1_RowSource
        {
            get
            {
                return this.ComboBox1_RowSource1;
            }

            set
            {
                this.ComboBox1_RowSource1 = value;
            }
        }

        /// <summary>
        /// ComboBox1_RowSource1
        /// </summary>
        public IList<string> ComboBox1_RowSource1
        {
            get
            {
                return this.comboBox1_RowSource;
            }

            set
            {
                this.comboBox1_RowSource = value;
            }
        }

        /// <summary>
        /// ButtonResult
        /// </summary>
        public DialogResult ButtonResult
        {
            get
            {
                return this.buttonResult;
            }

            set
            {
                this.buttonResult = value;
            }
        }

        /// <summary>
        /// P03
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public P03(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = "QueryDate >= dateadd(year,-1,getdate()) AND MDivisionID = '" + Env.User.Keyword + "' AND Type = 'B'";
            this.detailgrid.AllowUserToOrderColumns = true;
            this.InsertDetailGridOnDoubleClick = false;
            this.ReloadTimeoutSeconds = 900;
            #region CTN# 排序
            this.detailgrid.CellClick += (s, e) =>
            {
                this.RowIndex = e.RowIndex;
                this.ColumnIndex = e.ColumnIndex;
            };
            this.detailgrid.Sorted += (s, e) =>
            {
                if (this.RowIndex == -1 && this.ColumnIndex == 6)
                {
                    if (this.detailgridSort == DescSort)
                    {
                        ((DataTable)((BindingSource)this.detailgrid.DataSource).DataSource).DefaultView.Sort = "sortCTNNo DESC, CTNStartNo  DESC";
                        this.detailgridSort = ASCSort;
                    }
                    else
                    {
                        ((DataTable)((BindingSource)this.detailgrid.DataSource).DataSource).DefaultView.Sort = "sortCTNNo ASC, CTNStartNo ASC";
                        this.detailgridSort = DescSort;
                    }
                }
            };
            #endregion
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
        /// OnFormLoaded
        /// </summary>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.btnEdit.Enabled = this.Perm.Edit;

            this.ComboBox1_RowSource.Add(string.Empty);
            this.ComboBox1_RowSource.Add("Transfer Clog");
            this.ComboBox1_RowSource.Add("Clog Cfm");
            this.ComboBox1_RowSource.Add("Location No");
            this.ComboBox1_RowSource.Add("ColorWay");
            this.ComboBox1_RowSource.Add("Color");
            this.ComboBox1_RowSource.Add("Size");
            this.comboxbs1 = new BindingSource(this.ComboBox1_RowSource, null);
            this.comboSortby.DataSource = this.comboxbs1;
            MyUtility.Tool.SetupCombox(this.cbDuring, 2, 1, "A,Default,B,All");

            DataTable queryDT;
            string querySql = string.Format(
                @"
select '' FTYGroup

union 
select distinct FTYGroup 
from Factory 
where MDivisionID = '{0}'", Env.User.Keyword);
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
                        this.DefaultWhere = string.Format("FactoryID = '{0}'", this.queryfors.SelectedValue);
                        break;
                }

                this.ReloadDatas();
            };
        }

        // BatchConfirm
        private void BtnBatchConf_Click(object sender, EventArgs e)
        {
            var frm = new P03_BatchConfirm();
            this.ShowWaitMessage("Data Loading....");
            this.HideWaitMessage();
            frm.ShowDialog(this);
            this.ReloadDatas();
            this.RenewData();
        }

        private void CbDuring_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cbDuring = (ComboBox)sender;
            bool bolInit = this.DefaultFilter.IndexOf("QueryDate >= dateadd(year,-1,getdate())") > 0;
            bool bolReload = true;
            switch (cbDuring.SelectedIndex)
            {
                case 0:
                    if (bolInit)
                    {
                        bolReload = false;
                    }
                    else
                    {
                        this.DefaultFilter = "QueryDate >= dateadd(year,-1,getdate()) AND MDivisionID = '" + Env.User.Keyword + "' AND Type = 'B'";
                    }

                    break;
                case 1:
                    this.DefaultFilter = "MDivisionID = '" + Env.User.Keyword + "' AND Type = 'B'";
                    break;
            }

            if (bolReload)
            {
                this.ReloadDatas();
            }
        }

        /// <summary>
        /// OnDetailEntered
        /// </summary>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();

            this.labelCofirmed.Visible = MyUtility.Check.Empty(this.CurrentMaintain["ID"]) ? false : true;

            DataRow dr;
            string sqlStatus = string.Format(@"select * from PackingList WITH (NOLOCK) where id='{0}'", this.CurrentMaintain["id"].ToString());
            if (MyUtility.Check.Seek(sqlStatus, out dr))
            {
                if (dr["Status"].ToString().ToUpper() == "NEW" && MyUtility.Check.Empty(this.CurrentMaintain["GMTBookingLock"]))
                {
                    this.labelCofirmed.Text = "New";
                }
                else if (dr["Status"].ToString().ToUpper() == "CONFIRMED" && !MyUtility.Check.Empty(this.CurrentMaintain["GMTBookingLock"]))
                {
                    this.labelCofirmed.Text = "Confirmed";
                }
                else if (dr["Status"].ToString().ToUpper() == "CONFIRMED" && MyUtility.Check.Empty(this.CurrentMaintain["GMTBookingLock"]))
                {
                    this.labelCofirmed.Text = "Confirmed";
                }
                else
                {
                    this.labelCofirmed.Text = "Shipping Lock";
                }
            }

            // UnConfirm History按鈕變色
            if (MyUtility.Check.Seek(this.CurrentMaintain["ID"].ToString(), "PackingList_History", "ID"))
            {
                this.btnUnConfirmHistory.ForeColor = Color.Blue;
            }
            else
            {
                this.btnUnConfirmHistory.ForeColor = Color.Black;
            }

            // Carton Summary按鈕變色
            if (MyUtility.Check.Seek(string.Format("select pd.ID from PackingList_Detail pd WITH (NOLOCK) , Order_CTNData oc WITH (NOLOCK) where pd.OrderID = oc.ID and pd.ID = '{0}'", this.CurrentMaintain["ID"].ToString())))
            {
                this.btnCartonSummary.ForeColor = Color.Blue;
            }
            else
            {
                this.btnCartonSummary.ForeColor = Color.Black;
            }

            // Scan and Pack Deleted History按鈕變色
            if (MyUtility.Check.Seek($@"
select 1 from PackingList_Detail pd WITH (NOLOCK)
INNER JOIN PackingScan_History ph  WITH (NOLOCK) ON pd.ID=ph.PackingListID AND pd.OrderID=ph.OrderID AND pd.CTNStartNo=ph.CTNStartNo AND pd.SCICtnNo=ph.SCICtnNo
WHERE pd.ID='{this.CurrentMaintain["ID"]}'
"))
            {
                this.btnPackScanHistory.ForeColor = Color.Blue;
            }
            else
            {
                this.btnPackScanHistory.ForeColor = Color.Black;
            }

            // Repack Cartons 控制
            bool isNotNew = !this.CurrentMaintain["Status"].Equals("New");
            bool isShippingLock = this.CurrentMaintain["GMTBookingLock"].Equals("Y");
            bool isCanEdit = Prgs.GetAuthority(Env.User.UserID, "P03. Packing List Weight && Summary(Bulk)", "CanEdit");
            if (this.EditMode || isNotNew || !isCanEdit || isShippingLock)
            {
                this.btnRepackCartons.Enabled = false;
            }
            else
            {
                this.btnRepackCartons.Enabled = true;
            }

            // Start Ctn#
            string sqlCmd;
            DataRow orderData;
            sqlCmd = string.Format("select top 1 CTNStartNo  from PackingList_Detail WITH (NOLOCK) where ID = '{0}' order by seq  ", this.CurrentMaintain["ID"].ToString());
            if (MyUtility.Check.Seek(sqlCmd, out orderData))
            {
                this.displayStartCtn.Value = orderData["CTNStartNo"].ToString();
            }

            if (!MyUtility.Check.Empty(this.CurrentMaintain["GMTBookingLock"]))
            {
                this.gridicon.Append.Enabled = false;
                this.gridicon.Insert.Enabled = false;
                this.gridicon.Remove.Enabled = false;
            }
            else
            {
                this.gridicon.Append.Enabled = true;
                this.gridicon.Insert.Enabled = true;
                this.gridicon.Remove.Enabled = true;
            }

            if (this.DetailDatas.Count > 0)
            {
                this.datesciDelivery.Value = MyUtility.Convert.GetDate(this.DetailDatas[0]["sciDelivery"]);
                this.datekpileta.Value = MyUtility.Convert.GetDate(this.DetailDatas[0]["kpileta"]);
            }

            #region Show Cancel Order
            string sqlcmdC;
            DataRow cancelOrder;
            sqlcmdC = $@"SELECT [Cancel] = CASE WHEN count(*) > 0 THEN 1 ELSE 0 END
                         FROM orders o 
                         INNER JOIN PackingList_Detail p2 ON o.ID = p2.OrderID
                         WHERE  o.Junk = 1 AND p2.ID = '{this.CurrentMaintain["ID"]}'";
            if (MyUtility.Check.Seek(sqlcmdC, out cancelOrder))
            {
                switch (cancelOrder["Cancel"].ToString())
                {
                    case "1":
                        this.checkCancelledOrder.Checked = true;
                        break;
                    case "0":
                        this.checkCancelledOrder.Checked = false;
                        break;
                }
            }
            #endregion
            #region displayPurchaseCtn
            sqlcmdC = $@"select 1 from LocalPO_Detail ld with(nolock) inner join LocalPO l with(nolock) on l.id = ld.Id
where RequestID='{this.CurrentMaintain["ID"]}' and l.status = 'Approved'
";
            if (MyUtility.Check.Seek(sqlcmdC))
            {
                this.displayPurchaseCtn.Text = "Y";
            }
            else
            {
                this.displayPurchaseCtn.Text = "N";
            }
            #endregion
            #region disClogCFMStatus
            sqlcmdC = $@"select iif(count(pd.ID) = count(pd.ReceiveDate), 'Y','N') [ClogCFMStatus]
                         from PackingList_Detail pd
                         where pd.id = '{this.CurrentMaintain["ID"]}'";
            if (MyUtility.Check.Seek(sqlcmdC, out cancelOrder))
            {
                this.disClogCFMStatus.Text = cancelOrder["ClogCFMStatus"].ToString();
            }
            #endregion

            // 加總APPBookingVW & APPEstAmtVW
            DataTable dt = (DataTable)this.detailgridbs.DataSource;
            this.numAppBookingVW.Value = MyUtility.Convert.GetDecimal(dt.Compute("sum(APPBookingVW)", string.Empty));
            this.numAppEstAmtVW.Value = MyUtility.Convert.GetDecimal(dt.Compute("sum(APPEstAmtVW)", string.Empty));

            this.btnClrearCustCtn.Enabled = this.EditMode;

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
                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);

                    // 檢查箱子如果有送到Clog則不可以被修改
                    if (e.FormattedValue.ToString() != dr["OrderID"].ToString())
                    {
                        if (!this.CheckCanCahngeCol(MyUtility.Convert.GetDate(dr["TransferDate"])))
                        {
                            dr["OrderID"] = dr["OrderID"].ToString();
                            e.Cancel = true;
                            return;
                        }
                    }

                    if (!MyUtility.Check.Empty(e.FormattedValue) && e.FormattedValue.ToString() != dr["OrderID"].ToString())
                    {
                        DataRow orderData;
                        if (!MyUtility.Check.Seek(
                            string.Format(
                                @"
Select  o.ID
        , o.SeasonID
        , o.StyleID
        , o.CustPONo 
        , o.FtyGroup
from Orders o WITH (NOLOCK) 
inner join Factory f on o.FactoryID = f.ID
where   o.ID = '{0}' 
        and ((o.Category = 'B' and o.LocalOrder = 0) or o.Category = 'S' or o.Category = 'G')
        and o.BrandID = '{1}' 
        and o.Dest = '{2}' 
        and o.CustCDID = '{3}'
        and o.MDivisionID = '{4}'
        and f.IsProduceFty = 1",
                                e.FormattedValue.ToString(),
                                this.CurrentMaintain["BrandID"].ToString(),
                                this.CurrentMaintain["Dest"].ToString(),
                                this.CurrentMaintain["CustCDID"].ToString(),
                                Env.User.Keyword), out orderData))
                        {
                            MessageBox.Show(string.Format("< SP No.: {0} > not found!!!", e.FormattedValue.ToString()));
                            dr["OrderID"] = string.Empty;
                            dr["OrderShipmodeSeq"] = string.Empty;
                            dr["Article"] = string.Empty;
                            dr["Color"] = string.Empty;
                            dr["SizeCode"] = string.Empty;
                            dr["StyleID"] = string.Empty;
                            dr["CustPONo"] = string.Empty;
                            dr["SeasonID"] = string.Empty;
                            dr["Factory"] = string.Empty;
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
                            dr["Article"] = string.Empty;
                            dr["Color"] = string.Empty;
                            dr["SizeCode"] = string.Empty;
                            #region 若Order_QtyShip有多筆資料話就跳出視窗讓使者選擇Seq
                            string sqlCmd = string.Format(
                                @"
select count(oq.ID) as CountID 
from Order_QtyShip oq WITH (NOLOCK) inner join orders o WITH (NOLOCK) on oq.id = o.id 
where oq.ID = '{0}' and ShipmodeID = '{1}' and o.MDivisionID = '{2}'",
                                dr["OrderID"].ToString(),
                                this.CurrentMaintain["ShipModeID"].ToString(),
                                Env.User.Keyword);
                            if (MyUtility.Check.Seek(sqlCmd, out orderData))
                            {
                                if (orderData["CountID"].ToString() == "1")
                                {
                                    string sqlCmd2 = string.Format(
                                        @"
select seq
from Order_QtyShip oq WITH (NOLOCK) inner join orders o WITH (NOLOCK) on oq.id = o.id 
where oq.ID = '{0}' and ShipmodeID = '{1}' and o.MDivisionID = '{2}'",
                                        dr["OrderID"].ToString(),
                                        this.CurrentMaintain["ShipModeID"].ToString(),
                                        Env.User.Keyword);
                                    if (MyUtility.Check.Seek(sqlCmd2, out orderData))
                                    {
                                        dr["OrderShipmodeSeq"] = orderData["seq"].ToString();
                                    }
                                }
                                else
                                {
                                    sqlCmd = string.Format(
                                        @"
select Seq,oq.BuyerDelivery,ShipmodeID,oq.Qty 
from Order_QtyShip oq WITH (NOLOCK) inner join orders o WITH (NOLOCK) on oq.id = o.id 
where oq.ID = '{0}' and ShipmodeID = '{1}' and o.MDivisionID = '{2}'",
                                        dr["OrderID"].ToString(),
                                        this.CurrentMaintain["ShipModeID"].ToString(),
                                        Env.User.Keyword);
                                    Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlCmd, "4,20,20,10", string.Empty, "Seq,Buyer Delivery,ShipMode,Qty");
                                    DialogResult returnResult = item.ShowDialog();
                                    if (returnResult == DialogResult.Cancel)
                                    {
                                        dr["OrderShipmodeSeq"] = string.Empty;
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

                    // 檢查OrderID+Seq不可以重複建立
                    if (!Prgs.P03CheckDouble_SpSeq(dr["OrderID"].ToString(), dr["OrderShipmodeSeq"].ToString(), this.CurrentMaintain["ID"].ToString()))
                    {
                        dr["OrderID"] = string.Empty;
                        dr["OrderShipmodeSeq"] = string.Empty;
                        dr["Article"] = string.Empty;
                        dr["Color"] = string.Empty;
                        dr["SizeCode"] = string.Empty;
                        dr["StyleID"] = string.Empty;
                        dr["CustPONo"] = string.Empty;
                        dr["SeasonID"] = string.Empty;
                        dr["Factory"] = string.Empty;
                        dr.EndEdit();
                        e.Cancel = true;
                        return;
                    }
                }
            };

            // Seq
            this.seq.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && MyUtility.Check.Empty(this.CurrentMaintain["GMTBookingLock"]))
                {
                    if (e.Button == MouseButtons.Right)
                    {
                        if (e.RowIndex != -1)
                        {
                            DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                            string sqlCmd = string.Format(
                                @"
select  oq.Seq
        , oq.BuyerDelivery
        , oq.ShipmodeID
        , oq.Qty 
from Order_QtyShip oq WITH (NOLOCK) 
inner join orders o WITH (NOLOCK) on oq.id = o.id  
where   oq.ID = '{0}' 
        and oq.ShipmodeID = '{1}' 
        and o.MDivisionID = '{2}'",
                                dr["OrderID"].ToString(),
                                this.CurrentMaintain["ShipModeID"].ToString(),
                                Env.User.Keyword);
                            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlCmd, "4,20,20,10", string.Empty, "Seq,Buyer Delivery,ShipMode,Qty");
                            DialogResult returnResult = item.ShowDialog();
                            if (returnResult == DialogResult.Cancel)
                            {
                                return;
                            }

                            e.EditingControl.Text = item.GetSelectedString();
                            dr["OrderShipmodeSeq"] = e.EditingControl.Text;
                            if (e.EditingControl.Text != dr["OrderShipmodeSeq"].ToString())
                            {
                                if (!this.CheckCanCahngeCol(MyUtility.Convert.GetDate(dr["TransferDate"])))
                                {
                                    dr["OrderShipmodeSeq"] = dr["OrderShipmodeSeq"].ToString();
                                    return;
                                }
                                else
                                {
                                    dr["Article"] = string.Empty;
                                    dr["Color"] = string.Empty;
                                    dr["SizeCode"] = string.Empty;
                                    dr.EndEdit();
                                }
                            }
                        }
                    }
                }
            };

            this.seq.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    DataRow chk_dr;
                    string sqlCmd = string.Format(
                        @"
select  oq.Seq
        , oq.BuyerDelivery
        , oq.ShipmodeID
        , oq.Qty 
from Order_QtyShip oq WITH (NOLOCK) 
inner join orders o WITH (NOLOCK) on oq.id = o.id  
where   oq.ID = '{0}' 
        and oq.ShipmodeID = '{1}' 
        and o.MDivisionID = '{2}'
        and oq.Seq = '{3}'",
                        dr["OrderID"].ToString(),
                        this.CurrentMaintain["ShipModeID"].ToString(),
                        Env.User.Keyword,
                        e.FormattedValue);
                    if (!MyUtility.Check.Seek(sqlCmd, out chk_dr))
                    {
                        dr["Seq"] = string.Empty;
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox("< Seq:" + e.FormattedValue + " > not found!!!");
                        return;
                    }

                    // 檢查OrderID+Seq不可以重複建立
                    if (!Prgs.P03CheckDouble_SpSeq(dr["OrderID"].ToString(), ((object)e.FormattedValue).ToString(), this.CurrentMaintain["ID"].ToString()))
                    {
                        dr["OrderID"] = string.Empty;
                        dr["OrderShipmodeSeq"] = string.Empty;
                        dr["Article"] = string.Empty;
                        dr["Color"] = string.Empty;
                        dr["SizeCode"] = string.Empty;
                        dr["StyleID"] = string.Empty;
                        dr["CustPONo"] = string.Empty;
                        dr["SeasonID"] = string.Empty;
                        dr["Factory"] = string.Empty;
                        dr.EndEdit();
                        e.Cancel = true;
                        return;
                    }
                }
            };

            this.article.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && this.article.IsEditingReadOnly == false)
                {
                    if (e.Button == MouseButtons.Right)
                    {
                        if (e.RowIndex != -1)
                        {
                            DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                            string sqlCmd = string.Format("Select Distinct Article from Order_QtyShip_Detail WITH (NOLOCK) where ID = '{0}' and Seq = '{1}'", dr["OrderID"].ToString(), dr["OrderShipmodeSeq"].ToString());
                            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlCmd, "8", dr["Article"].ToString());
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
                        if (!MyUtility.Check.Seek(string.Format("Select Article from Order_QtyShip_Detail WITH (NOLOCK) where ID = '{0}' and Seq = '{1}' and Article = '{2}'", dr["OrderID"].ToString(), dr["OrderShipmodeSeq"].ToString(), e.FormattedValue.ToString())))
                        {
                            MyUtility.Msg.WarningBox(string.Format("< Article: {0} > not found!!!", e.FormattedValue.ToString()));
                            dr["Article"] = string.Empty;
                            dr["Color"] = string.Empty;
                            dr["SizeCode"] = string.Empty;
                        }
                        else
                        {
                            dr["Article"] = e.FormattedValue.ToString().ToUpper();
                            string sqlCmd = string.Format(
                                @"select ColorID 
                                                                                  from View_OrderFAColor 
                                                                                  where ID = '{0}' and Article = '{1}'",
                                dr["OrderID"].ToString(),
                                dr["Article"]);
                            DataRow colorData;
                            if (MyUtility.Check.Seek(sqlCmd, out colorData))
                            {
                                dr["Color"] = colorData["ColorID"].ToString();
                            }
                            else
                            {
                                dr["Color"] = string.Empty;
                            }
                        }

                        dr.EndEdit();
                    }
                }
            };

            this.size.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && this.size.IsEditingReadOnly == false)
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
                                dr["OrderID"].ToString(),
                                dr["OrderShipmodeSeq"].ToString(),
                                dr["Article"].ToString());
                            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlCmd, "8", dr["SizeCode"].ToString());
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
                        if (!MyUtility.Check.Seek(string.Format("Select SizeCode from Order_QtyShip_Detail WITH (NOLOCK) where ID = '{0}' and Seq = '{1}' and Article = '{2}' and SizeCode = '{3}'", dr["OrderID"].ToString(), dr["OrderShipmodeSeq"].ToString(), dr["Article"].ToString(), e.FormattedValue.ToString())))
                        {
                            dr["SizeCode"] = string.Empty;
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox(string.Format("< SizeCode: {0} > not found!!!", e.FormattedValue.ToString()));
                            return;
                        }
                    }
                }
            };
            #endregion

            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("Factory", header: "Factory", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("OrderID", header: "SP No.", width: Widths.AnsiChars(13), settings: this.orderid).Get(out this.col_orderid)
                .Text("Cancel", header: "Cancel Order", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("SeasonID", header: "Season", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("OrderShipmodeSeq", header: "Seq", width: Widths.AnsiChars(2), iseditingreadonly: false, settings: this.seq).Get(out this.col_seq)
                .Date("IDD", header: "Intended Delivery", iseditingreadonly: true)
                .Text("StyleID", header: "Style No.", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("CustPONo", header: "P.O. No.", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("CTNStartNo", header: "CTN#", width: Widths.AnsiChars(6)).Get(out this.col_ctnno)
                .Numeric("CTNQty", header: "# of CTN").Get(out this.col_ctnqty)
                .CellCartonItem("RefNo", header: "Ref No.", width: Widths.AnsiChars(13)).Get(out this.col_refno)
                .Text("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("Article", header: "ColorWay", width: Widths.AnsiChars(8), settings: this.article).Get(out this.col_article)
                .Text("Color", header: "Color", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(8), settings: this.size).Get(out this.col_size)
                .Numeric("QtyPerCTN", header: "PC/Ctn").Get(out this.col_qtyperctn)
                .Numeric("ShipQty", header: "Qty").Get(out this.col_shipqty)
                .Numeric("BalanceQty", header: "Bal. Qty", iseditingreadonly: true)
                .Numeric("NW", header: "N.W./Ctn", integer_places: 3, decimal_places: 3, maximum: 999.999M, minimum: 0).Get(out this.col_nw)
                .Numeric("GW", header: "G.W./Ctn", integer_places: 3, decimal_places: 3, maximum: 999.999M, minimum: 0).Get(out this.col_gw)
                .Numeric("NNW", header: "N.N.W./Ctn", integer_places: 3, decimal_places: 3, maximum: 999.999M, minimum: 0).Get(out this.col_nnw)
                .Numeric("NWPerPcs", header: "N.W./Pcs", integer_places: 2, decimal_places: 3, maximum: 99.999M, minimum: 0).Get(out this.col_nwpcs)
                .Numeric("ScanQty", header: "PC/Ctn Scanned", iseditingreadonly: true)
                .Date("TransferDate", header: "Transfer CLOG", iseditingreadonly: true)
                .Date("ReceiveDate", header: "CLOG CFM", iseditingreadonly: true)
                .Text("ClogLocationId", header: "Clog Location", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("CFALocationID", header: "CFA Location No.", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Date("ReturnDate", header: "Return Date", iseditingreadonly: true)
                .Date("DisposeDate", header: "Dispose Date", iseditingreadonly: true)
                .Text("Barcode", header: "Barcode", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("CustCTN", header: "Cust CTN#", width: Widths.AnsiChars(20), iseditingreadonly: true);

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

                    if (this.detailgrid.Columns[e.ColumnIndex].DataPropertyName == this.col_qtyperctn.DataPropertyName)
                    {
                        if (!MyUtility.Check.Empty(e.FormattedValue))
                        {
                            if (e.FormattedValue.ToString() != dr["QtyPerCTN"].ToString())
                            {
                                if (!this.CheckCanCahngeCol(MyUtility.Convert.GetDate(dr["TransferDate"])))
                                {
                                    dr["QtyPerCTN"] = dr["QtyPerCTN"].ToString();
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

                    // # of CTN只能輸入0或1
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

            this.Color_Change();
        }

        /// <summary>
        /// ClickNewAfter
        /// </summary>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["MDivisionID"] = Env.User.Keyword;
            this.CurrentMaintain["Type"] = "B";
            this.CurrentMaintain["FactoryID"] = Env.User.Factory;
            this.CurrentMaintain["Status"] = "New";
            this.CurrentMaintain["QueryDate"] = DateTime.Now.ToShortDateString();
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
            this.comboSortby.Text = string.Empty;
            if (this.CurrentMaintain["ID"].ToString().Substring(3, 2).ToUpper() == "PG")
            {
                this.txtbrand.ReadOnly = true;
            }

            // 部分欄位會依某些條件來決定是否可以被修改
            if (!MyUtility.Check.Empty(this.CurrentMaintain["GMTBookingLock"]))
            {
                this.txtbrand.ReadOnly = true;
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
            }

            if (!MyUtility.Check.Empty(this.CurrentMaintain["LocalPOID"]))
            {
                this.dateCartonEstBooking.ReadOnly = true;
                this.dateCartonEstArrived.ReadOnly = true;
            }
        }

        private StringBuilder Ctn_no_combine(string sP, string seq)
        {
            StringBuilder ctn_no = new StringBuilder();
            var cnt_list = from r2 in this.DetailDatas.AsEnumerable()
                           where r2.Field<string>("OrderID") == sP &&
                                  r2.Field<string>("OrderShipmodeSeq") == seq
                           select new { cnt_no = r2.Field<string>("CTNStartNo") };
            foreach (var cnt in cnt_list)
            {
                ctn_no.Append("," + cnt.cnt_no);
            }

            ctn_no.Remove(0, 1);
            return ctn_no;
        }

        /// <summary>
        /// ClickSaveBefore
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ClickSaveBefore()
        {
            #region 檢查欄位值不可為空
            if (MyUtility.Check.Empty(this.CurrentMaintain["BrandID"]))
            {
                this.txtbrand.Focus();
                MyUtility.Msg.WarningBox("Brand can't empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["CustCDID"]))
            {
                this.txtcustcd.Focus();
                MyUtility.Msg.WarningBox("CustCD can't empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["Dest"]))
            {
                this.txtcountry.Focus();
                MyUtility.Msg.WarningBox("Destination can't empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["ShipModeID"]))
            {
                this.txtshipmode.Focus();
                MyUtility.Msg.WarningBox("Ship Mode can't empty!!");
                return false;
            }

            if (this.CurrentMaintain["Remark"].ToString().Length > 150)
            {
                this.editRemark.Focus();
                MyUtility.Msg.WarningBox("Remark can't be more than 150 Characters.");
                return false;
            }
            #endregion

            // 保存前檢查加入若PackingLIst 的[Ship Mode]不在Order_QtyShip中，不給存
            DataTable detailsData = (DataTable)this.detailgridbs.DataSource;
            DataTable dataTableDistinct = detailsData.DefaultView.ToTable(true, "OrderID");
            List<string> orderIdList = dataTableDistinct.AsEnumerable().Select(o => o["OrderID"].ToString()).ToList();

            foreach (var orderID in orderIdList)
            {
                bool exists = MyUtility.Check.Seek($"SELECT TOP 1 ShipmodeID FROM Order_QtyShip WHERE ID='{orderID}' AND ShipmodeID='{this.CurrentMaintain["ShipModeID"].ToString()}'");

                if (!exists)
                {
                    MyUtility.Msg.WarningBox($"There is no [Ship Mode] in this [SP No.]:{orderID} !");
                    return false;
                }
            }

            bool isSavecheckOK = Prgs.P03SaveCheck(this.CurrentMaintain, (DataTable)this.detailgridbs.DataSource, this.detailgrid);
            if (!isSavecheckOK)
            {
                return false;
            }

            // 檢查Packing數量是否超過總訂單數量
            var listOrder = this.DetailDatas.GroupBy(s => new { OrderID = s["OrderID"].ToString() })
                .Select(s => new
                {
                    ID = s.Key.OrderID,
                    ShipQty = s.Sum(dr => MyUtility.Convert.GetInt(dr["ShipQty"])),
                });
            foreach (var order in listOrder)
            {
                string id = MyUtility.Check.Empty(this.CurrentMaintain["ID"]) ? "New Packing List" : this.CurrentMaintain["ID"].ToString();
                DualResult resultCompare = Prgs.CompareOrderQtyPackingQty(order.ID, id, order.ShipQty);
                if (!resultCompare)
                {
                    MyUtility.Msg.WarningBox(resultCompare.Description);
                    return false;
                }
            }

            // GetID
            if (this.IsDetailInserting)
            {
                string id = MyUtility.GetValue.GetID(Env.User.Keyword + "PL", "PackingList", DateTime.Today, 2, "Id", null);
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

            return base.ClickSaveBefore();
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
            // 存檔成功後，要再呼叫UpdateOrdersCTN, CreateOrderCTNData
            Prgs.P03_UpdateOthers(this.CurrentMaintain);

            // 檢查表身的ShipMode與表頭的ShipMode如果不同就不可以SAVE，存檔後提醒
            this.CheckShipMode("save");
            DualResult result = Prgs.CheckExistsOrder_QtyShip_Detail(MyUtility.Convert.GetString(this.CurrentMaintain["ID"]), showmsg: false);
            if (!result)
            {
                return result;
            }

            string upd_sql = $@"
UPDATE PackingList_Detail
SET CTNEndNo = CTNStartNo
WHERE ID =  '{this.CurrentMaintain["ID"]}'";
            DualResult upd_result = DBProxy.Current.Execute(null, upd_sql);

            return Ict.Result.True;
        }

        /// <inheritdoc/>
        protected override void ClickSaveAfter()
        {
            base.ClickSaveAfter();
            #region ISP20200757 資料交換 - Sunrise
            Task.Run(() => new Sunrise_FinishingProcesses().SentPackingToFinishingProcesses(this.CurrentMaintain["ID"].ToString(), string.Empty))
                .ContinueWith(UtilityAutomation.AutomationExceptionHandler, TaskContinuationOptions.OnlyOnFaulted);
            #endregion

            #region ISP20201607 資料交換 - Gensong
            if (Gensong_FinishingProcesses.IsGensong_FinishingProcessesEnable)
            {
                // 不透過Call API的方式，自己組合，傳送API
                Task.Run(() => new Gensong_FinishingProcesses().SentPackingListToFinishingProcesses(this.CurrentMaintain["ID"].ToString(), string.Empty))
                    .ContinueWith(UtilityAutomation.AutomationExceptionHandler, TaskContinuationOptions.OnlyOnFaulted);
            }
            #endregion
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
        /// ClickDeletePost
        /// </summary>
        /// <returns>DualResult</returns>
        protected override DualResult ClickDeletePost()
        {
            DataTable orderData;
            string sqlCmd = string.Format("select distinct OrderID from PackingList_Detail WITH (NOLOCK) where ID = '{0}'", this.CurrentMaintain["ID"].ToString());
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out orderData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "select order list fail!\r\n" + result.ToString());
                return failResult;
            }

            result = Prgs.UpdateOrdersCTN(orderData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Update Orders CTN fail!\r\n" + result.ToString());
                return failResult;
            }

            #region 一併移除 PackingListID 相對應貼標 / 噴碼的資料
            sqlCmd = $@"

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
            result = DBProxy.Current.Execute(null, sqlCmd);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Delete <Shipping Mark Picture> 、<Shipping Mark Stamp> fail! \r\n" + result.ToString());
                return failResult;
            }
            #endregion

            #region ISP20200757 資料交換 - Sunrise
            Task.Run(() => new Sunrise_FinishingProcesses().SentPackingToFinishingProcesses(this.CurrentMaintain["ID"].ToString(), string.Empty))
                .ContinueWith(UtilityAutomation.AutomationExceptionHandler, TaskContinuationOptions.OnlyOnFaulted);
            #endregion

            #region ISP20201607 資料交換 - Gensong
            if (Gensong_FinishingProcesses.IsGensong_FinishingProcessesEnable)
            {
                // 不透過Call API的方式，自己組合，傳送API
                Task.Run(() => new Gensong_FinishingProcesses().SentPackingListToFinishingProcesses(this.CurrentMaintain["ID"].ToString(), string.Empty))
                    .ContinueWith(UtilityAutomation.AutomationExceptionHandler, TaskContinuationOptions.OnlyOnFaulted);
            }
            #endregion
            return Ict.Result.True;
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
            Sci.Production.Packing.P03_Print callNextForm = new Sci.Production.Packing.P03_Print(this.CurrentMaintain, orderQty);
            callNextForm.ShowDialog(this);
            return base.ClickPrint();
        }

        /// <summary>
        /// 表身Grid的Delete
        /// </summary>
        protected override void OnDetailGridDelete()
        {
            // 檢查此筆記錄是否已Transfer to Clog，若是則出訊息告知且無法刪除
            if (this.DetailDatas.Count > 0)
            {
                if (!MyUtility.Check.Empty(this.CurrentDetailData["TransferDate"]))
                {
                    MyUtility.Msg.WarningBox("This record had been send to CLOG, can't delete!!");
                    return;
                }
            }

            base.OnDetailGridDelete();
        }

        // 控制表身Grid欄位是否可被編輯
        private void DetailGridEditing(bool isEditing)
        {
            if (isEditing)
            {
                this.col_orderid.IsEditingReadOnly = false;
                this.col_ctnno.IsEditingReadOnly = false;
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
                    if (i == 0 || i == 1 || i == 4 || i == 5 || i == 6 || i == 8 || i == 10 || i == 11 || i == 12 || i == 14 || i == 15 || i == 16 || i == 17)
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
                this.col_ctnno.IsEditingReadOnly = true;
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
                    this.detailgrid.Columns[i].DefaultCellStyle.ForeColor = Color.Black;
                }

                this.col_refno.EditingMouseDown -= new EventHandler<Ict.Win.UI.DataGridViewEditingControlMouseEventArgs>(CartonRefnoCommon.EditingMouseDown);
            }
        }

        // Brand
        private void Txtbrand_Validated(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.txtbrand.OldValue))
            {
                return;
            }

            if (this.EditMode && this.txtbrand.OldValue != this.txtbrand.Text)
            {
                this.DeleteDetailData(this.txtbrand, this.txtbrand.OldValue);
            }
        }

        // CustCD
        private void Txtcustcd_Validated(object sender, EventArgs e)
        {
            if (this.EditMode && !MyUtility.Check.Empty(this.txtcustcd.Text) && this.txtcustcd.OldValue != this.txtcustcd.Text)
            {
                this.CurrentMaintain["Dest"] = MyUtility.GetValue.Lookup(string.Format("SELECT CountryID FROM CustCD WITH (NOLOCK) WHERE BrandID = '{0}' AND ID = '{1}'", MyUtility.Convert.GetString(this.CurrentMaintain["BrandID"]), this.txtcustcd.Text));
            }
        }

        // Destination
        private void Txtcountry1_Validated(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.txtcountry.TextBox1.OldValue))
            {
                return;
            }

            if (this.EditMode && this.txtcountry.TextBox1.OldValue != this.txtcountry.TextBox1.Text)
            {
                this.DeleteDetailData(this.txtcountry.TextBox1, this.txtcountry.TextBox1.OldValue);
                this.txtcountry.DataBindings.Cast<Binding>().ToList().ForEach(binding => binding.WriteValue());
            }
        }

        // ShipMode
        private void Txtshipmode_Validated(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.txtshipmode.OldValue))
            {
                return;
            }

            if (MyUtility.Check.Empty(this.DetailDatas) || this.DetailDatas.Count == 0)
            {
                return;
            }
        }

        // 刪除表身Grid的資料
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
            foreach (DataRow dr in this.DetailDatas)
            {
                dr.Delete();
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

        // Sort by
        private void ComboSortby_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.labelLocateforTransferClog.Visible = true;
            this.txtLocateforTransferClog.Visible = false;
            this.dateLocateforTransferClog.Visible = false;
            this.btnFindNow.Visible = true;
            switch (this.comboSortby.SelectedValue.ToString())
            {
                case "Transfer Clog":
                    this.labelLocateforTransferClog.Text = "Locate for Transfer Clog:";
                    this.labelLocateforTransferClog.Width = 156;
                    this.dateLocateforTransferClog.Visible = true;
                    this.dateLocateforTransferClog.Location = new System.Drawing.Point(538, 284);
                    this.btnFindNow.Location = new System.Drawing.Point(675, 279);
                    ((DataTable)this.detailgridbs.DataSource).DefaultView.Sort = "TransferDate,Seq";
                    break;
                case "Clog Cfm":
                    this.labelLocateforTransferClog.Text = "Locate for Clog Cfm:";
                    this.labelLocateforTransferClog.Width = 129;
                    this.dateLocateforTransferClog.Visible = true;
                    this.dateLocateforTransferClog.Location = new System.Drawing.Point(511, 284);
                    this.btnFindNow.Location = new System.Drawing.Point(650, 279);
                    ((DataTable)this.detailgridbs.DataSource).DefaultView.Sort = "ReceiveDate,Seq";
                    break;
                case "Location No":
                    this.labelLocateforTransferClog.Text = "Locate for Location No:";
                    this.labelLocateforTransferClog.Width = 147;
                    this.txtLocateforTransferClog.Visible = true;
                    this.txtLocateforTransferClog.Location = new System.Drawing.Point(525, 284);
                    this.btnFindNow.Location = new System.Drawing.Point(615, 279);
                    ((DataTable)this.detailgridbs.DataSource).DefaultView.Sort = "ClogLocationId,Seq";
                    break;
                case "ColorWay":
                    this.labelLocateforTransferClog.Text = "Locate for ColorWay:";
                    this.labelLocateforTransferClog.Width = 135;
                    this.txtLocateforTransferClog.Visible = true;
                    this.txtLocateforTransferClog.Location = new System.Drawing.Point(513, 284);
                    this.btnFindNow.Location = new System.Drawing.Point(603, 279);
                    ((DataTable)this.detailgridbs.DataSource).DefaultView.Sort = "Article,Seq";
                    break;
                case "Color":
                    this.labelLocateforTransferClog.Text = "Locate for Color:";
                    this.labelLocateforTransferClog.Width = 106;
                    this.txtLocateforTransferClog.Visible = true;
                    this.txtLocateforTransferClog.Location = new System.Drawing.Point(483, 284);
                    this.btnFindNow.Location = new System.Drawing.Point(573, 279);
                    ((DataTable)this.detailgridbs.DataSource).DefaultView.Sort = "Color,Seq";
                    break;
                case "Size":
                    this.labelLocateforTransferClog.Text = "Locate for Size:";
                    this.labelLocateforTransferClog.Width = 100;
                    this.txtLocateforTransferClog.Visible = true;
                    this.txtLocateforTransferClog.Location = new System.Drawing.Point(477, 284);
                    this.btnFindNow.Location = new System.Drawing.Point(567, 279);
                    ((DataTable)this.detailgridbs.DataSource).DefaultView.Sort = "SizeCode,Seq";
                    break;
                default:
                    this.labelLocateforTransferClog.Visible = false;
                    this.btnFindNow.Visible = false;
                    if (MyUtility.Check.Empty((DataTable)this.detailgridbs.DataSource))
                    {
                        break;
                    }

                    ((DataTable)this.detailgridbs.DataSource).DefaultView.Sort = "Seq";
                    break;
            }
        }

        // Carton Summary
        private void BtnCartonSummary_Click(object sender, EventArgs e)
        {
            Sci.Production.Packing.P03_CartonSummary callNextForm = new Sci.Production.Packing.P03_CartonSummary(this.CurrentMaintain["ID"].ToString());
            callNextForm.ShowDialog(this);
        }

        // UnConfirm History
        private void BtnUnConfirmHistory_Click(object sender, EventArgs e)
        {
            // Bug fix:0000276: PACKING_P03_Packing List Weight & Summary(Bulk)，1.點選[Unconfirm history]會出現錯誤訊息。
            // Sci.Win.UI.ShowHistory callNextForm = new Sci.Win.UI.ShowHistory("PackingList_History", CurrentMaintain["ID"].ToString(), "Status", caption: "UnConfirm History", haveRemark: true, customerGridFormatTable: "HisType", moduleName: "PackingList");
            Sci.Win.UI.ShowHistory callNextForm = new Sci.Win.UI.ShowHistory("PackingList_History", this.CurrentMaintain["ID"].ToString(), "Status", caption: "UnConfirm History", haveRemark: true, customerGridFormatTable: "HisType", moduleName: "Packing");
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

            // 檢查Packing數量是否超過總訂單數量
            var listOrder = this.DetailDatas.Select(s => s["OrderID"].ToString()).Distinct();
            foreach (string orderID in listOrder)
            {
                DualResult resultCompare = Prgs.CompareOrderQtyPackingQty(orderID, string.Empty, 0);
                if (!resultCompare)
                {
                    MyUtility.Msg.WarningBox(resultCompare.Description);
                    return;
                }
            }

            if (!Prgs.CheckExistsOrder_QtyShip_Detail(MyUtility.Convert.GetString(this.CurrentMaintain["ID"])))
            {
                return;
            }

            #region 檢查
            string cmd = $@"
SELECT [SP No.]=pd.OrderID
    , [Style No.]=o.StyleID
    , [ColorWay]=pd.Article
    , [Color]=pd.Color
    , [Size]=pd.SizeCode
    , [Seq]=pd.OrderShipmodeSeq
    , [CTN#]=pd.CTNStartNo
FROM PackingList p
INNER JOIN PackingList_Detail pd ON p.ID = pd.ID
INNER JOIN ShipMode s ON p.ShipModeID = s.ID
INNER JOIN Orders o ON o.ID = pd.OrderID
WHERE p.ID='{this.CurrentMaintain["ID"]}'
AND s.NeedCreateAPP = 1 
AND pd.NW = 0
AND pd.CTNQty = 1
";
            DataTable dt;
            DBProxy.Current.Select(null, cmd, out dt);

            if (dt.Rows.Count > 0)
            {
                var m = MyUtility.Msg.ShowMsgGrid(dt, "When shipmode need to create Air Pre-Paid, [N.W./Ctn] can't be 0, Please help to modify [N.W./Ctn] of the following data.");

                m.Width = 850;
                m.grid1.Columns[0].Width = 100;
                m.grid1.Columns[1].Width = 100;
                m.grid1.Columns[2].Width = 100;
                m.grid1.Columns[3].Width = 100;
                m.grid1.Columns[4].Width = 100;
                m.grid1.Columns[5].Width = 100;
                m.grid1.Columns[6].Width = 100;
                m.TopMost = true;
                return;
            }
            #endregion

            string sqlcmd = $@"exec dbo.usp_Packing_P03_Confirm '{this.CurrentMaintain["ID"]}','{Env.User.Factory}','{Env.User.UserID}','1'";
            DataTable dtSP = new DataTable();
            if (this.result = DBProxy.Current.Select(string.Empty, sqlcmd, out dtSP))
            {
                if (dtSP.Rows.Count > 0)
                {
                    if (!MyUtility.Check.Empty(dtSP.Rows[0]["msg"]))
                    {
                        MyUtility.Msg.WarningBox(dtSP.Rows[0]["msg"].ToString());
                    }
                }
            }
            else
            {
                this.ShowErr(this.result);
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
                    MyUtility.Msg.WarningBox("Garment booking already confirmed, can't unconfirm! ");
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
            this.ButtonResult = MyUtility.Msg.WarningBox("Are you sure you want to < Unconfirm > this data?", "Warning", this.buttons);
            if (this.ButtonResult == DialogResult.No)
            {
                return;
            }

            this.SelectReason();
        }

        private void SelectReason()
        {
            Sci.Win.UI.SelectReason callReason = new Sci.Win.UI.SelectReason();
            DialogResult dResult = callReason.ShowDialog(this);
            if (dResult == DialogResult.OK)
            {
                string reasonRemark = callReason.ReturnRemark;

                if (MyUtility.Check.Empty(reasonRemark))
                {
                    MyUtility.Msg.InfoBox("Remark cannot be empty!");
                    this.SelectReason();
                }
                else
                {
                    using (TransactionScope transactionScope = new TransactionScope())
                    {
                        try
                        {
                            string insertCmd = @"insert into PackingList_History (ID, HisType, OldValue, NewValue, Remark, AddName, AddDate)
 values (@id,@hisType,@oldValue,@newValue,@remark,@addName,GETDATE())";
                            string updateCmd = @"update PackingList set Status = 'New', EditName = @addName, EditDate = GETDATE() where ID = @id";

                            #region 準備sql參數資料

                            System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter
                            {
                                ParameterName = "@id",
                                Value = this.CurrentMaintain["ID"].ToString(),
                            };

                            System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter
                            {
                                ParameterName = "@hisType",
                                Value = "Status",
                            };

                            System.Data.SqlClient.SqlParameter sp3 = new System.Data.SqlClient.SqlParameter
                            {
                                ParameterName = "@oldValue",
                                Value = "Confirmed",
                            };

                            System.Data.SqlClient.SqlParameter sp4 = new System.Data.SqlClient.SqlParameter
                            {
                                ParameterName = "@newValue",
                                Value = "New",
                            };

                            System.Data.SqlClient.SqlParameter sp5 = new System.Data.SqlClient.SqlParameter
                            {
                                ParameterName = "@remark",
                                Value = reasonRemark,
                            };

                            System.Data.SqlClient.SqlParameter sp6 = new System.Data.SqlClient.SqlParameter
                            {
                                ParameterName = "@addName",
                                Value = Env.User.UserID,
                            };

                            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>
                            {
                                sp1,
                                sp2,
                                sp3,
                                sp4,
                                sp5,
                                sp6,
                            };
                            #endregion

                            DualResult result, result2;
                            result = DBProxy.Current.Execute(null, insertCmd, cmds);
                            result2 = DBProxy.Current.Execute(null, updateCmd, cmds);

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
                            this.ShowErr("Commit transaction error.", ex);
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

        // 檢查表身的ShipMode與表頭的ShipMode要相同
        private bool CheckShipMode(string p_type)
        {
            StringBuilder msg = new StringBuilder();
            DualResult result;
            DataTable dt = (DataTable)this.detailgridbs.DataSource;
            DataTable chktb;
            string chkshipmode = string.Format(@"select distinct t.OrderID,t.OrderShipmodeSeq,o.ShipModeID
from #tmp t inner join Order_QtyShip o with (nolock) on t.OrderID = o.id and t.OrderShipmodeSeq = o.Seq");
            result = MyUtility.Tool.ProcessWithDatatable(dt, string.Empty, chkshipmode, out chktb, "#tmp");
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Sql command error!");
                return false;
            }

            foreach (DataRow dr in chktb.Select(string.Format("ShipModeID <> '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["ShipModeID"]))))
            {
                msg.Append(string.Format("SP#:{0},  Seq:{1} Shipping Mode:{2}\r\n", MyUtility.Convert.GetString(dr["OrderID"]), MyUtility.Convert.GetString(dr["OrderShipmodeSeq"]), MyUtility.Convert.GetString(dr["ShipModeID"])));
            }

            if (msg.Length > 0)
            {
                if (p_type.Equals("save"))
                {
                    MyUtility.Msg.InfoBox("Packing List Ship Mode does not match Order Q'ty B'down.\r\n" + msg.ToString());
                }
                else
                {
                    MyUtility.Msg.WarningBox("Packing List Ship Mode does not match Order Q'ty B'down.\r\n" + msg.ToString());
                }

                return false;
            }

            return true;
        }

        // Find Now
        private void BtnFindNow_Click(object sender, EventArgs e)
        {
            int index;
            switch (this.comboSortby.SelectedValue.ToString())
            {
                case "Transfer Clog":
                    index = this.detailgridbs.Find("TransferDate", this.dateLocateforTransferClog.Value.ToString());
                    break;
                case "Clog Cfm":
                    index = this.detailgridbs.Find("ReceiveDate", this.dateLocateforTransferClog.Value.ToString());
                    break;
                case "Location No":
                    index = this.detailgridbs.Find("ClogLocationId", this.txtLocateforTransferClog.Text.Trim());
                    break;
                case "ColorWay":
                    index = this.detailgridbs.Find("Article", this.txtLocateforTransferClog.Text.Trim());
                    break;
                case "Color":
                    index = this.detailgridbs.Find("Color", this.txtLocateforTransferClog.Text.Trim());
                    break;
                case "Size":
                    index = this.detailgridbs.Find("SizeCode", this.txtLocateforTransferClog.Text.Trim());
                    break;
                default:
                    index = -1;
                    break;
            }

            if (index == -1)
            {
                MyUtility.Msg.WarningBox("Data was not found!!");
            }
            else
            {
                this.detailgridbs.Position = index;
            }
        }

        private void BtnUpdateBarcode_Click(object sender, EventArgs e)
        {
            // 檢查是否已經pullout
            if (MyUtility.Check.Seek($"select status from Pullout where  ID = '{this.CurrentMaintain["PulloutID"]}' and status = 'Confirmed' "))
            {
                MyUtility.Msg.WarningBox($"<#{this.CurrentMaintain["ID"]}> Already pullout!, Cannot update barcode!!");
                return;
            }

            DataTable custbarcode_result;
            string sqlcmd = $@"select distinct o.StyleID,pd.Article,pd.SizeCode,cb.Barcode from PackingList_Detail pd WITH (NOLOCK)
inner join orders o WITH (NOLOCK) on o.ID = pd.OrderID
inner join CustBarCode cb WITH (NOLOCK) on o.CustPONo = cb.CustPONo and o.BrandID = cb.BrandID and o.StyleID = cb.StyleID and pd.Article = cb.Article and pd.SizeCode = cb.SizeCode
where pd.id = '{this.CurrentMaintain["ID"]}'";

            DualResult result = DBProxy.Current.Select(null, sqlcmd, out custbarcode_result);
            if (result)
            {
                using (custbarcode_result)
                {
                    if (custbarcode_result.Rows.Count > 0)
                    {
                        string upd_sql = $@"update pd set pd.Barcode = cb.BarCode
from PackingList_Detail pd WITH (NOLOCK)
inner join orders o WITH (NOLOCK) on o.ID = pd.OrderID
inner join CustBarCode cb WITH (NOLOCK) on o.CustPONo = cb.CustPONo and o.BrandID = cb.BrandID and o.StyleID = cb.StyleID and pd.Article = cb.Article and pd.SizeCode = cb.SizeCode
where pd.id = '{this.CurrentMaintain["ID"]}'";
                        DualResult upd_result = DBProxy.Current.Execute(null, upd_sql);
                        if (!upd_result)
                        {
                            this.ShowErr(result);
                            return;
                        }
                        else
                        {
                            var m = new Win.UI.MsgGridForm(custbarcode_result, "Updated as follows barcode", "Update successful", null, MessageBoxButtons.OK)
                            {
                                Width = 600,
                            };
                            m.grid1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                            m.grid1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                            m.grid1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                            m.grid1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                            m.text_Find.Width = 140;
                            m.btn_Find.Location = new Point(150, 6);
                            m.btn_Find.Anchor = AnchorStyles.Left | AnchorStyles.Top;
                            m.ShowDialog();
                            this.RenewData();
                        }
                    }
                    else
                    {
                        MyUtility.Msg.WarningBox("Please go to Packing.P17 Import Scan & Pack Barcode file first");
                        return;
                    }
                }
            }
            else
            {
                this.ShowErr(result);
                return;
            }
        }

        private void BtnUPCSticker_Click(object sender, EventArgs e)
        {
            Sci.Production.Packing.P03_UPCSticker callNextForm = new Sci.Production.Packing.P03_UPCSticker(this.CurrentMaintain["ID"].ToString());
            callNextForm.ShowDialog(this);
            this.RenewData();
            this.OnDetailEntered();
        }

        private void BtnDownload_Click(object sender, EventArgs e)
        {
            if (this.DetailDatas.Count == 0)
            {
                return;
            }

            DataTable printData;
            string sqlcmd = $@"
SELECT 
DISTINCT
PD.OrderID AS 'SP No.',
O.CustPONo AS 'P.O. No.',
PD.ID AS 'Pack ID',
PD.CTNStartNo AS 'CTN#',
PD.Article AS 'ColorWay',
pd.Color as 'Color',
pd.SizeCode as 'SizeCode',
PD.CustCTN AS 'Cust CTN#',
pd.seq
 FROM PackingList_Detail PD 
LEFT JOIN Orders O ON PD.OrderID = O.ID
WHERE PD.ID ='{this.CurrentMaintain["ID"]}' --放入該表頭的Pack ID
order by PD.seq
";
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out printData);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            if (printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return;
            }

            printData.Columns.Remove("seq");
            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Packing_P03_CustCTN.xltx");
            MyUtility.Excel.CopyToXls(printData, string.Empty, "Packing_P03_CustCTN.xltx", 1, false, null, objApp); // 將datatable copy to excel
            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
            objSheets.Range["H2", $"H{printData.Rows.Count + 1}"].Interior.Color = Color.FromArgb(255, 199, 206);
            objApp.Visible = true;
        }

        private void BtnImportFromExcel_Click(object sender, EventArgs e)
        {
            DataTable dtexcel;
            if (this.EditMode)
            {
                return;
            }

            this.openFileDialog1.Filter = "Excel files (*.xlsx;*.xls;*.xlt)|*.xlsx;*.xls;*.xlt";

            // 開窗且有選擇檔案
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string msg;

                dtexcel = this.GetExcel(this.openFileDialog1.FileName, out msg);
                if (!MyUtility.Check.Empty(msg))
                {
                    MyUtility.Msg.ErrorBox(msg);
                    return;
                }

                if (MyUtility.Check.Empty(dtexcel.Columns["Pack ID"]) || MyUtility.Check.Empty(dtexcel.Columns["CTN#"]) || MyUtility.Check.Empty(dtexcel.Columns["Cust CTN#"]))
                {
                    MyUtility.Msg.WarningBox("excel file format error !!");
                    return;
                }

                foreach (DataRow item in dtexcel.Rows)
                {
                    if (!MyUtility.Convert.GetString(item["Pack ID"]).EqualString(MyUtility.Convert.GetString(this.CurrentMaintain["ID"])))
                    {
                        MyUtility.Msg.WarningBox("excel file format error !!");
                        return;
                    }

                    if (!MyUtility.Check.Empty(item["Cust CTN#"]))
                    {
                        if (item["Cust CTN#"].ToString().Length > 30)
                        {
                            MyUtility.Msg.WarningBox("Cust CTN# length can not be more than 30 !!");
                            return;
                        }
                    }
                }

                string updateSqlCmd = $@"
update b set b.CustCTN = a.[Cust CTN#]
from #tmp a
inner join PackingList_Detail b on a.[Pack ID] = b.ID and a.CTN# = b.CTNStartNo

update PackingList set  EditName = '{Env.User.UserID}', EditDate = GETDATE() where ID = '{this.CurrentMaintain["ID"].ToString()}'
";
                DataTable udt;
                DualResult result = MyUtility.Tool.ProcessWithDatatable(dtexcel, string.Empty, updateSqlCmd, out udt);
                if (!result)
                {
                    this.ShowErr(result);
                }
                else
                {
                    MyUtility.Msg.InfoBox("Import Success !!");
                    this.RenewData();
                    this.OnDetailEntered();
                }
            }
        }

        /// <summary>
        /// GetExcel
        /// </summary>
        /// <param name="strPath">strPath</param>
        /// <param name="strMsg">strMsg</param>
        /// <returns>DataTable</returns>
        public DataTable GetExcel(string strPath, out string strMsg)
        {
            try
            {
                Microsoft.Office.Interop.Excel.Application xlsApp = new Microsoft.Office.Interop.Excel.Application
                {
                    Visible = false,
                };
                Microsoft.Office.Interop.Excel.Workbook xlsBook = xlsApp.Workbooks.Open(strPath);
                Microsoft.Office.Interop.Excel.Worksheet xlsSheet = xlsBook.ActiveSheet;
                Microsoft.Office.Interop.Excel.Range xlsRangeFirstCell = xlsSheet.get_Range("A1");
                Microsoft.Office.Interop.Excel.Range xlsRangeLastCell = xlsSheet.Cells.SpecialCells(Microsoft.Office.Interop.Excel.XlCellType.xlCellTypeLastCell);
                Microsoft.Office.Interop.Excel.Range xlsRange = xlsSheet.get_Range(xlsRangeFirstCell, xlsRangeLastCell);
                object[,] objValue = xlsRange.Value2 as object[,];

                // Array[][] to DataTable
                long lngColumnCount = objValue.GetLongLength(1);
                long lngRowCount = objValue.GetLongLength(0);
                DataTable dtExcel = new DataTable();
                for (int j = 1; j <= lngColumnCount; j++)
                {
                    dtExcel.Columns.Add(objValue[1, j].ToString());
                }

                for (int i = 2; i <= lngRowCount; i++)
                {
                    DataRow drRow = dtExcel.NewRow();
                    bool isNUll = false;
                    for (int j = 1; j <= lngColumnCount; j++)
                    {
                        if (!MyUtility.Check.Empty(objValue[i, j]))
                        {
                            drRow[j - 1] = MyUtility.Check.Empty(objValue[i, j]) ? string.Empty : objValue[i, j].ToString();
                        }
                        else
                        {
                            isNUll = true;
                        }
                    }

                    if (!isNUll)
                    {
                        dtExcel.Rows.Add(drRow);
                    }
                }

                xlsBook.Close();
                xlsApp.Quit();
                strMsg = string.Empty;
                return dtExcel;
            }
            catch (Exception ex)
            {
                strMsg = ex.Message;
                return null;
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

                    if (!MyUtility.Check.Empty(dr["DisposeFromClog"]) || !MyUtility.Check.Empty(dr["DisposeDate"]))
                    {
                        this.detailgrid.Rows[index].DefaultCellStyle.BackColor = Color.FromArgb(190, 190, 190);
                    }
                    else
                    {
                        this.detailgrid.Rows[index].DefaultCellStyle.BackColor = Color.White;
                    }

                    if (MyUtility.Convert.GetDecimal(dr["BalanceQty"]) < 0)
                    {
                        this.detailgrid.Rows[index].Cells[16].Style.BackColor = Color.Red;
                    }
                }
            }
        }

        private void BtnRepackCartons_Click(object sender, EventArgs e)
        {
            DataTable dtDetail = this.DetailDatas.CopyToDataTable();
            P03_RepackCartons p03_RepackCartons = new P03_RepackCartons(this.CurrentMaintain, dtDetail);
            p03_RepackCartons.ShowDialog();
            if (p03_RepackCartons.DialogResult == DialogResult.Yes)
            {
                this.ReloadDatas();
            }
        }

        private void BtnPackScanHistory_Click(object sender, EventArgs e)
        {
            Sci.Production.Packing.P03_ScanAndPackDeletedHistory form = new P03_ScanAndPackDeletedHistory(this.CurrentMaintain["ID"].ToString());
            form.ShowDialog(this);
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (this.CurrentMaintain["Status"].ToString() == "Confirmed")
            {
                MyUtility.Msg.WarningBox("This record is < Confirmed >, can't be modified!");
                return;
            }

            if (this.CurrentMaintain == null || this.CurrentMaintain["ID"].ToString().Empty())
            {
                return;
            }

            P03_CartonBooking p03_CartonBooking = new P03_CartonBooking(this.CurrentMaintain);
            p03_CartonBooking.ShowDialog(this);
            this.RenewData();
        }

        private void BtnClrearCustCtn_Click(object sender, EventArgs e)
        {
            // 清空表身Grid資料
            foreach (DataRow dr in this.DetailDatas)
            {
                dr["CustCTN"] = string.Empty;
            }
        }
    }
}
