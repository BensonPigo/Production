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

namespace Sci.Production.Packing
{
    /// <summary>
    /// Packing_P03
    /// </summary>
    public partial class P03 : Sci.Win.Tems.Input6
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
        private Ict.Win.DataGridViewGeneratorTextColumnSettings orderid = new Ict.Win.DataGridViewGeneratorTextColumnSettings();
        private Ict.Win.DataGridViewGeneratorTextColumnSettings seq = new Ict.Win.DataGridViewGeneratorTextColumnSettings();
        private Ict.Win.DataGridViewGeneratorTextColumnSettings article = new Ict.Win.DataGridViewGeneratorTextColumnSettings();
        private Ict.Win.DataGridViewGeneratorTextColumnSettings size = new Ict.Win.DataGridViewGeneratorTextColumnSettings();
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
            this.DefaultFilter = "MDivisionID = '" + Sci.Env.User.Keyword + "' AND Type = 'B'";
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
            this.ComboBox1_RowSource.Add(string.Empty);
            this.ComboBox1_RowSource.Add("Transfer Clog");
            this.ComboBox1_RowSource.Add("Clog Cfm");
            this.ComboBox1_RowSource.Add("Location No");
            this.ComboBox1_RowSource.Add("ColorWay");
            this.ComboBox1_RowSource.Add("Color");
            this.ComboBox1_RowSource.Add("Size");
            this.comboxbs1 = new BindingSource(this.ComboBox1_RowSource, null);
            this.comboSortby.DataSource = this.comboxbs1;

            #region 新增Bath Confirm 按鈕
            Sci.Win.UI.Button btnBatchConf = new Win.UI.Button();
            btnBatchConf.Text = "Batch Confirm";
            btnBatchConf.Click += new EventHandler(this.btnBatchConf_Click);
            this.browsetop.Controls.Add(btnBatchConf);
            btnBatchConf.Size = new Size(122, 30);
            btnBatchConf.Visible = true;
            #endregion

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
                        this.DefaultWhere = string.Format("FactoryID = '{0}'", this.queryfors.SelectedValue);
                        break;
                }

                this.ReloadDatas();
            };
        }

        private void btnBatchConf_Click(object sender, EventArgs e)
        {
            var frm = new Sci.Production.Packing.P03_BatchConfirm();
            this.ShowWaitMessage("Data Loading....");
            this.HideWaitMessage();
            frm.ShowDialog(this);
            this.ReloadDatas();
            this.RenewData();
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

            // Purchase Ctn
            this.displayPurchaseCtn.Value = MyUtility.Check.Empty(this.CurrentMaintain["LocalPOID"]) ? string.Empty : "Y";

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
        }

        // 檢查OrderID+Seq不可以重複建立
        private bool CheckDouble_SpSeq(string orderid, string seq)
        {
            if (MyUtility.Check.Seek(string.Format("select ID from PackingList_Detail WITH (NOLOCK) where OrderID = '{0}' AND OrderShipmodeSeq = '{1}' AND ID != '{2}'", orderid, seq, this.CurrentMaintain["ID"].ToString())))
            {
                MyUtility.Msg.WarningBox("SP No:" + orderid + ", Seq:" + seq + " already exist in packing list, can't be create again!");
                return false;
            }

            return true;
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
                                Sci.Env.User.Keyword), out orderData))
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
                                Sci.Env.User.Keyword);
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
                                        Sci.Env.User.Keyword);
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
                                        Sci.Env.User.Keyword);
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
                    if (!this.CheckDouble_SpSeq(dr["OrderID"].ToString(), dr["OrderShipmodeSeq"].ToString()))
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
                    if (e.Button == System.Windows.Forms.MouseButtons.Right)
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
                                Sci.Env.User.Keyword);
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
                    if (!this.CheckDouble_SpSeq(dr["OrderID"].ToString(), ((object)e.FormattedValue).ToString()))
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
                    if (e.Button == System.Windows.Forms.MouseButtons.Right)
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
                    if (e.Button == System.Windows.Forms.MouseButtons.Right)
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
                .Text("ClogLocationId", header: "Location No.", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Date("ReturnDate", header: "Return Date", iseditingreadonly: true)
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
        }

        /// <summary>
        /// ClickNewAfter
        /// </summary>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["MDivisionID"] = Sci.Env.User.Keyword;
            this.CurrentMaintain["Type"] = "B";
            this.CurrentMaintain["FactoryID"] = Sci.Env.User.Factory;
            this.CurrentMaintain["Status"] = "New";
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
            // 檢查欄位值不可為空
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

            #region 檢查表頭的CustCD與表身所有SP的 Orders.custcdid是否相同
            DataTable dtCheckCustCD;
            List<SqlParameter> listCheckCustCDSqlParameter = new List<SqlParameter>();
            listCheckCustCDSqlParameter.Add(new SqlParameter("@CustCD", this.txtcustcd.Text));
            string strCheckCustCD = @"
select OrderID
from #tmp
outer apply (
    select value = isnull (o.CustCDID, '')
    from Orders o
    where #tmp.OrderID = o.ID
) CustCD
where CustCD.value != @CustCD
      or CustCD.value is null";
            DualResult reslut = MyUtility.Tool.ProcessWithDatatable(this.DetailDatas.CopyToDataTable(), null, strCheckCustCD, out dtCheckCustCD, paramters: listCheckCustCDSqlParameter);
            if (reslut == false)
            {
                MyUtility.Msg.WarningBox(this.result.ToString());
                return false;
            }
            else if (dtCheckCustCD != null && dtCheckCustCD.Rows.Count > 0)
            {
                MyUtility.Msg.WarningBox("CustCD are different, please check!");
                return false;
            }
            #endregion

            // 刪除表身SP No.或Qty為空白的資料，表身的CTN#, Ref No., Color Way與Size不可以為空值，計算CTNQty, ShipQty, NW, GW, NNW, CBM，重算表身Grid的Bal. Qty
            int i = 0, ctnQty = 0, shipQty = 0, ttlShipQty = 0, needPackQty = 0, count = 0;
            double nw = 0.0, gw = 0.0, nnw = 0.0, cbm = 0.0;
            string filter = string.Empty, sqlCmd;
            bool isNegativeBalQty = false;
            DataTable needPackData, tmpPackData;
            DualResult selectResult;
            DataRow[] detailData;

            // 準備needPackData的Schema
            sqlCmd = "select OrderID, OrderShipmodeSeq, Article, SizeCode, ShipQty as Qty from PackingList_Detail WITH (NOLOCK) where ID = ''";
            if (!(selectResult = DBProxy.Current.Select(null, sqlCmd, out needPackData)))
            {
                MyUtility.Msg.WarningBox("Query  schema fail!");
                return false;
            }

            foreach (DataRow dr in this.DetailDatas.OrderBy(u => u["ID"]).ThenBy(u => u["OrderShipmodeSeq"]))
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
                    // ISP20181015 CBM抓到小數點後4位
                    cbm = cbm + (MyUtility.Convert.GetDouble(MyUtility.GetValue.Lookup("CBM", dr["RefNo"].ToString(), "LocalItem", "RefNo")) * MyUtility.Convert.GetInt(dr["CTNQty"]));
                }

                #endregion

                #region 重算表身Grid的Bal. Qty

                // 目前還有多少衣服尚未裝箱
                // needPackQty = 0;
                filter = string.Format("OrderID = '{0}' and OrderShipmodeSeq = '{1}'", dr["OrderID"].ToString(), dr["OrderShipmodeSeq"].ToString());
                detailData = needPackData.Select(filter);
                if (detailData.Length <= 0)
                {
                    // 撈取此SP+Seq尚未裝箱的數量
                    sqlCmd = string.Format(
                        @"

select OrderID = oqd.Id
	   , OrderShipmodeSeq = oqd.Seq
	   , oqd.Article
	   , oqd.SizeCode
	   , Qty = (oqd.Qty - isnull(AccuShipQty.value, 0) - isnull(InvAdjustDiffQty.value, 0))
from Order_QtyShip_Detail oqd WITH (NOLOCK) 
outer apply (
	select value = sum(pld.ShipQty)
	from PackingList pl With (NoLock)
	inner join PackingList_Detail pld WITH (NOLOCK) on pl.ID = pld.ID
	where pl.Status = 'Confirmed'
		  and pl.ID != '{0}'
		  and pld.OrderID = oqd.Id 
		  and pld.OrderShipmodeSeq = oqd.Seq
		  and pld.Article = oqd.Article
		  and pld.SizeCode = oqd.SizeCode		  
) AccuShipQty
outer apply (
	select value = sum (iaq.DiffQty)
	from InvAdjust ia WITH (NOLOCK)
	inner join InvAdjust_Qty iaq WITH (NOLOCK) on iaq.ID = ia.ID 
											 
	where ia.OrderID = oqd.ID 
		  and ia.OrderShipmodeSeq = oqd.Seq
		  and iaq.Article = oqd.Article 
		  and iaq.SizeCode = oqd.SizeCode
) InvAdjustDiffQty
where oqd.Id = '{1}'
	  and oqd.Seq = '{2}'",
                        this.CurrentMaintain["ID"].ToString(),
                        dr["OrderID"].ToString(),
                        dr["OrderShipmodeSeq"].ToString());
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

                // 加總表身特定Article/SizeCode的Ship Qty數量
                detailData = ((DataTable)this.detailgridbs.DataSource).Select(filter);
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
                    this.detailgrid.Rows[count].DefaultCellStyle.BackColor = Color.Pink;
                }
                #endregion

                count = count + 1;
            }

            #region ship mode 有變更時 check Order_QtyShip

            StringBuilder chk_ship_err = new StringBuilder();
            StringBuilder chk_seq_null = new StringBuilder();
            var check_chip_list = from r1 in this.DetailDatas.AsEnumerable()
                                  group r1 by new
                                  {
                                      SP = r1.Field<string>("OrderID"),
                                      Seq = r1.Field<string>("OrderShipmodeSeq")
                                  }

into g
                                  select new
                                  {
                                      SP = g.Key.SP,
                                      Seq = g.Key.Seq
                                  };
            foreach (var chk_item in check_chip_list)
            {
                if (MyUtility.Check.Empty(chk_item.Seq))
                {
                    chk_seq_null.Append("<SP> " + chk_item.SP + " <CTN#> [" + this.Ctn_no_combine(chk_item.SP, chk_item.Seq) + "]  \r\n");
                }
            }

            if (chk_seq_null.Length > 0)
            {
                chk_seq_null.Insert(0, " Seq can not empty , please check again:  \r\n");

                MyUtility.Msg.WarningBox(chk_seq_null.ToString());
                return false;
            }

            #endregion

            if (this.DetailDatas.Count == 0)
            {
                MyUtility.Msg.InfoBox("Detail cannot be empty");
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

            // CTNQty, ShipQty, NW, GW, NNW, CBM
            this.CurrentMaintain["CTNQty"] = ctnQty;
            this.CurrentMaintain["ShipQty"] = shipQty;
            this.CurrentMaintain["NW"] = nw;
            this.CurrentMaintain["GW"] = gw;
            this.CurrentMaintain["NNW"] = nnw;
            this.CurrentMaintain["CBM"] = cbm;

            if (isNegativeBalQty)
            {
                MyUtility.Msg.WarningBox("Quantity entered is greater than order quantity!!");
                return false;
            }

            // GetID
            if (this.IsDetailInserting)
            {
                string id = MyUtility.GetValue.GetID(Sci.Env.User.Keyword + "PL", "PackingList", DateTime.Today, 2, "Id", null);
                if (MyUtility.Check.Empty(id))
                {
                    MyUtility.Msg.WarningBox("GetID fail, please try again!");
                    return false;
                }

                this.CurrentMaintain["ID"] = id;
            }

            // 表身重新計算後,再判斷CBM or GW 是不是0
            if (MyUtility.Check.Empty(this.CurrentMaintain["CBM"]) || MyUtility.Check.Empty(this.CurrentMaintain["GW"]))
            {
                this.numTtlCBM.Focus();
                MyUtility.Msg.WarningBox("Ttl CBM and Ttl GW can't be empty!!");
                return false;
            }

            return base.ClickSaveBefore();
        }

        /// <summary>
        /// ClickSavePre
        /// </summary>
        /// <returns>DualResult</returns>
        protected override DualResult ClickSavePre()
        {
            if (!MyUtility.Check.Empty(this.CurrentMaintain["INVNo"]))
            {
                string sqlCmd = string.Format(
                    @"select isnull(sum(ShipQty),0) as ShipQty,isnull(sum(CTNQty),0) as CTNQty,isnull(sum(NW),0) as NW,isnull(sum(GW),0) as GW,isnull(sum(NNW),0) as NNW,isnull(sum(CBM),0) as CBM
from PackingList WITH (NOLOCK) 
where INVNo = '{0}'
and ID != '{1}'",
                    this.CurrentMaintain["INVNo"].ToString(),
                    this.CurrentMaintain["ID"].ToString());

                DataTable summaryData;
                if (this.result = DBProxy.Current.Select(null, sqlCmd, out summaryData))
                {
                    string updateCmd = @"update GMTBooking
set TotalShipQty = @ttlShipQty, TotalCTNQty = @ttlCTNQty, TotalNW = @ttlNW, TotalNNW = @ttlNNW, TotalGW = @ttlGW, TotalCBM = @ttlCBM
where ID = @INVNo";
                    #region 準備sql參數資料
                    System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter();
                    sp1.ParameterName = "@ttlShipQty";
                    sp1.Value = MyUtility.Convert.GetInt(summaryData.Rows[0]["ShipQty"]) + MyUtility.Convert.GetInt(this.CurrentMaintain["ShipQty"]);

                    System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter();
                    sp2.ParameterName = "@ttlCTNQty";
                    sp2.Value = MyUtility.Convert.GetInt(summaryData.Rows[0]["CTNQty"]) + MyUtility.Convert.GetInt(this.CurrentMaintain["CTNQty"]);

                    System.Data.SqlClient.SqlParameter sp3 = new System.Data.SqlClient.SqlParameter();
                    sp3.ParameterName = "@ttlNW";
                    sp3.Value = MyUtility.Math.Round(MyUtility.Convert.GetDouble(summaryData.Rows[0]["NW"]) + MyUtility.Convert.GetDouble(this.CurrentMaintain["NW"]), 2);

                    System.Data.SqlClient.SqlParameter sp4 = new System.Data.SqlClient.SqlParameter();
                    sp4.ParameterName = "@ttlNNW";
                    sp4.Value = MyUtility.Math.Round(MyUtility.Convert.GetDouble(summaryData.Rows[0]["NNW"]) + MyUtility.Convert.GetDouble(this.CurrentMaintain["NNW"]), 2);

                    System.Data.SqlClient.SqlParameter sp5 = new System.Data.SqlClient.SqlParameter();
                    sp5.ParameterName = "@ttlGW";
                    sp5.Value = MyUtility.Math.Round(MyUtility.Convert.GetDouble(summaryData.Rows[0]["GW"]) + MyUtility.Convert.GetDouble(this.CurrentMaintain["GW"]), 2);

                    System.Data.SqlClient.SqlParameter sp6 = new System.Data.SqlClient.SqlParameter();
                    sp6.ParameterName = "@ttlCBM";
                    // ISP20181015 CBM抓到小數點後4位
                    sp6.Value = MyUtility.Convert.GetDouble(summaryData.Rows[0]["CBM"]) + MyUtility.Convert.GetDouble(this.CurrentMaintain["CBM"]);

                    System.Data.SqlClient.SqlParameter sp7 = new System.Data.SqlClient.SqlParameter();
                    sp7.ParameterName = "@INVNo";
                    sp7.Value = this.CurrentMaintain["INVNo"].ToString();

                    IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                    cmds.Add(sp1);
                    cmds.Add(sp2);
                    cmds.Add(sp3);
                    cmds.Add(sp4);
                    cmds.Add(sp5);
                    cmds.Add(sp6);
                    cmds.Add(sp7);
                    #endregion

                    this.result = Sci.Data.DBProxy.Current.Execute(null, updateCmd, cmds);
                    if (!this.result)
                    {
                        DualResult failResult = new DualResult(false, "Update Garment Booking fail!\r\n" + this.result.ToString());
                        return failResult;
                    }
                }
                else
                {
                    DualResult failResult = new DualResult(false, "Select PackingList fail!!\r\n" + this.result.ToString());
                    return failResult;
                }
            }

            return Result.True;
        }

        /// <summary>
        /// ClickSavePost
        /// </summary>
        /// <returns>DualResult</returns>
        protected override DualResult ClickSavePost()
        {
            // 存檔成功後，要再呼叫UpdateOrdersCTN, CreateOrderCTNData
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

            if (!Prgs.CreateOrderCTNData(this.CurrentMaintain["ID"].ToString()))
            {
                DualResult failResult = new DualResult(false, "Create Order_CTN fail!");
                return failResult;
            }

            // 檢查表身的ShipMode與表頭的ShipMode如果不同就不可以SAVE，存檔後提醒
            this.CheckShipMode("save");

            return Result.True;
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

            return Result.True;
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
                    if (i != 17)
                    {
                        this.detailgrid.Columns[i].DefaultCellStyle.ForeColor = Color.Black;
                    }
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
                    this.dateLocateforTransferClog.Location = new System.Drawing.Point(538, 261);
                    this.btnFindNow.Location = new System.Drawing.Point(675, 256);
                    ((DataTable)this.detailgridbs.DataSource).DefaultView.Sort = "TransferDate,Seq";
                    break;
                case "Clog Cfm":
                    this.labelLocateforTransferClog.Text = "Locate for Clog Cfm:";
                    this.labelLocateforTransferClog.Width = 129;
                    this.dateLocateforTransferClog.Visible = true;
                    this.dateLocateforTransferClog.Location = new System.Drawing.Point(511, 261);
                    this.btnFindNow.Location = new System.Drawing.Point(650, 256);
                    ((DataTable)this.detailgridbs.DataSource).DefaultView.Sort = "ReceiveDate,Seq";
                    break;
                case "Location No":
                    this.labelLocateforTransferClog.Text = "Locate for Location No:";
                    this.labelLocateforTransferClog.Width = 147;
                    this.txtLocateforTransferClog.Visible = true;
                    this.txtLocateforTransferClog.Location = new System.Drawing.Point(525, 261);
                    this.btnFindNow.Location = new System.Drawing.Point(615, 256);
                    ((DataTable)this.detailgridbs.DataSource).DefaultView.Sort = "ClogLocationId,Seq";
                    break;
                case "ColorWay":
                    this.labelLocateforTransferClog.Text = "Locate for ColorWay:";
                    this.labelLocateforTransferClog.Width = 135;
                    this.txtLocateforTransferClog.Visible = true;
                    this.txtLocateforTransferClog.Location = new System.Drawing.Point(513, 261);
                    this.btnFindNow.Location = new System.Drawing.Point(603, 256);
                    ((DataTable)this.detailgridbs.DataSource).DefaultView.Sort = "Article,Seq";
                    break;
                case "Color":
                    this.labelLocateforTransferClog.Text = "Locate for Color:";
                    this.labelLocateforTransferClog.Width = 106;
                    this.txtLocateforTransferClog.Visible = true;
                    this.txtLocateforTransferClog.Location = new System.Drawing.Point(483, 261);
                    this.btnFindNow.Location = new System.Drawing.Point(573, 256);
                    ((DataTable)this.detailgridbs.DataSource).DefaultView.Sort = "Color,Seq";
                    break;
                case "Size":
                    this.labelLocateforTransferClog.Text = "Locate for Size:";
                    this.labelLocateforTransferClog.Width = 100;
                    this.txtLocateforTransferClog.Visible = true;
                    this.txtLocateforTransferClog.Location = new System.Drawing.Point(477, 261);
                    this.btnFindNow.Location = new System.Drawing.Point(567, 256);
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
            //DataTable dtMantain = new DataTable();
            //dtMantain.Columns.Add("ID", typeof(string));
            //DataRow dr = dtMantain.NewRow();
            //dr["ID"] = this.CurrentMaintain["ID"].ToString();
            //dtMantain.Rows.Add(dr);
            //this.ConfirmFunction(dtMantain);

           string sqlcmd = $@"exec dbo.usp_Packing_P03_Confirm '{this.CurrentMaintain["ID"]}','{Sci.Env.User.Factory}','{Sci.Env.User.UserID}','1'";
            DataTable dtSP = new DataTable();
            if (result = DBProxy.Current.Select(string.Empty, sqlcmd, out dtSP))
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
            if (this.ButtonResult == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            this.SelectReason();
        }

//        private void ConfirmFunction(DataTable dt)
//        {
//            if (MyUtility.Check.Empty(dt))
//            {
//                MyUtility.Msg.WarningBox("data not found!");
//                return;
//            }

//            foreach (DataRow drMain in dt.Rows)
//            {
//                int i = 0, ctnQty = 0, shipQty = 0;
//                double nw = 0.0, gw = 0.0, nnw = 0.0, cbm = 0.0;
//                DataTable dtDetail;
//                DualResult r;
//                string sqlcmd = $@"
//select * from PackingList_Detail
//where id='{drMain["id"]}' order by id,OrderShipmodeSeq";
//                if (!(r = DBProxy.Current.Select(null, sqlcmd, out dtDetail)))
//                {
//                    continue;
//                }

//                foreach (DataRow dr in dtDetail.Rows)
//                {
//                    #region 計算CTNQty, ShipQty, NW, GW, NNW, CBM
//                    ctnQty = ctnQty + MyUtility.Convert.GetInt(dr["CTNQty"]);
//                    shipQty = shipQty + MyUtility.Convert.GetInt(dr["ShipQty"]);
//                    nw = MyUtility.Math.Round(nw + MyUtility.Convert.GetDouble(dr["NW"]), 3);
//                    gw = MyUtility.Math.Round(gw + MyUtility.Convert.GetDouble(dr["GW"]), 3);
//                    nnw = MyUtility.Math.Round(nnw + MyUtility.Convert.GetDouble(dr["NNW"]), 3);
//                    if (MyUtility.Check.Empty(dr["CTNQty"]) || MyUtility.Convert.GetInt(dr["CTNQty"]) > 0)
//                    {
//                        cbm = MyUtility.Math.Round(cbm + (MyUtility.Math.Round(MyUtility.Convert.GetDouble(MyUtility.GetValue.Lookup("CBM", dr["RefNo"].ToString(), "LocalItem", "RefNo")), 3) * MyUtility.Convert.GetInt(dr["CTNQty"])), 4);
//                    }

//                    #endregion
//                }

//                string updaterc = $@"
//update packinglist set
//    CTNQty ={ctnQty},
//    ShipQty ={shipQty},
//    NW       ={nw},
//    GW       ={gw},
//    NNW     ={nnw},
//    CBM      ={cbm}
//where id = '{this.CurrentMaintain["ID"]}'
//";

//                DualResult resultrc = DBProxy.Current.Execute(null, updaterc);
//                if (!resultrc)
//                {
//                    this.ShowErr(resultrc);
//                    return;
//                }

//                // 表身重新計算後,再判斷CBM or GW 是不是0
//                if (MyUtility.Check.Empty(MyUtility.GetValue.Lookup($"select CBM from PackingList where id='{drMain["id"]}'"))
//                    || MyUtility.Check.Empty(MyUtility.GetValue.Lookup($"select gw from PackingList where id='{drMain["id"]}'")))
//                {
//                    MyUtility.Msg.WarningBox("Ttl CBM and Ttl GW can't be empty!!");
//                    return;
//                }

//                // 訂單M別與登入系統M別不一致時，不可以Confirm
//                DataTable diffMOrder;
//                string sqlCmd = string.Format("select distinct pd.OrderID from PackingList_Detail pd WITH (NOLOCK) , Orders o WITH (NOLOCK) where pd.ID = '{0}' and pd.OrderID = o.ID and o.MDivisionID <> '{1}'", drMain["id"].ToString(), Sci.Env.User.Keyword);
//                DualResult result = DBProxy.Current.Select(null, sqlCmd, out diffMOrder);
//                if (!result)
//                {
//                    MyUtility.Msg.WarningBox("Sql connection fail!\r\n" + result.ToString());
//                    return;
//                }
//                else
//                {
//                    if (diffMOrder.Rows.Count > 0)
//                    {
//                        StringBuilder orderList = new StringBuilder();
//                        foreach (DataRow dr in diffMOrder.Rows)
//                        {
//                            orderList.Append(string.Format("\r\n{0}", dr["OrderID"].ToString()));
//                        }

//                        MyUtility.Msg.WarningBox("This SP's M not equal to login system M so can't confirm!" + orderList.ToString());
//                        return;
//                    }
//                }

//                // 還沒有Invoice No就不可以做Confirm
//                if (MyUtility.Check.Empty(MyUtility.GetValue.Lookup("INVNo", drMain["id"].ToString(), "PackingList", "ID")))
//                {
//                    MyUtility.Msg.WarningBox("Shipping is not yet booking so can't confirm!");
//                    return;
//                }

//                // 檢查累計Pullout數不可超過訂單數量
//                if (!Prgs.CheckPulloutQtyWithOrderQty(drMain["id"].ToString()))
//                {
//                    return;
//                }

//                // 檢查Sewing Output Qty是否有超過Packing Qty
//                if (!Prgs.CheckPackingQtyWithSewingOutput(drMain["id"].ToString()))
//                {
//                    return;
//                }

//                // 檢查表身的ShipMode與表頭的ShipMode如果不同就不可以SAVE
//                if (!this.CheckShipMode("confirm"))
//                {
//                    return;
//                }

//                // 檢查表身SP是否為製造單，製造單不能confirm
//                var dis_detail = (from rr in dtDetail.AsEnumerable()
//                                  select rr["OrderID"]).Distinct().ToList();
//                StringBuilder alertmsg = new StringBuilder();
//                foreach (string rr in dis_detail)
//                {
//                    if (MyUtility.Check.Seek(
//                        string.Format(
//                            @"select ot.id from OrderType ot 
//                                                            where exists (select o.id from orders o where o.id = '{0}' and
//                                                                                     o.BrandID = ot.BrandID and o.OrderTypeID = ot.ID) 
//                                                                                     and ot.IsGMTMaster = 1 ", rr), string.Empty))
//                    {
//                        alertmsg.Append("< SP#> " + r + Environment.NewLine);
//                    }
//                }

//                if (alertmsg.Length > 0)
//                {
//                    alertmsg.Insert(0, "The GMT Master order cannot be confirmed!! " + Environment.NewLine);
//                    MyUtility.Msg.WarningBox(alertmsg.ToString());
//                    return;
//                }

//                sqlCmd = string.Format("update PackingList set Status = 'Confirmed', EditName = '{0}', EditDate = '{1}' where ID = '{2}'", Sci.Env.User.UserID, DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), drMain["id"].ToString());
//                result = DBProxy.Current.Execute(null, sqlCmd);
//                if (!result)
//                {
//                    MyUtility.Msg.WarningBox("Confirm fail !\r\n" + result.ToString());
//                    return;
//                }
//            }

//        }

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
                            System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter();
                            sp1.ParameterName = "@id";
                            sp1.Value = this.CurrentMaintain["ID"].ToString();

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
                            result = Sci.Data.DBProxy.Current.Execute(null, insertCmd, cmds);
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
                    MyUtility.Msg.InfoBox("Save successfully, please be reminded the Ship Modes are different!\r\n" + msg.ToString());
                }
                else
                {
                    MyUtility.Msg.WarningBox("Ship Mode are different, please check!\r\n" + msg.ToString());
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
                            var m = new Sci.Win.UI.MsgGridForm(custbarcode_result, "Updated as follows barcode", "Update successful", null, MessageBoxButtons.OK);

                            m.Width = 600;
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
            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Packing_P03_CustCTN.xltx");
            MyUtility.Excel.CopyToXls(printData, string.Empty, "Packing_P03_CustCTN.xltx", 1, false, null, objApp);// 將datatable copy to excel
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
                }

                string updateSqlCmd = $@"
update b set b.CustCTN = a.[Cust CTN#]
from #tmp a
inner join PackingList_Detail b on a.[Pack ID] = b.ID and a.CTN# = b.CTNStartNo
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
                Microsoft.Office.Interop.Excel.Application xlsApp = new Microsoft.Office.Interop.Excel.Application();
                xlsApp.Visible = false;
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
                    for (int j = 1; j <= lngColumnCount; j++)
                    {
                        drRow[j - 1] = MyUtility.Check.Empty(objValue[i, j]) ? "" : objValue[i, j].ToString();
                    }

                    dtExcel.Rows.Add(drRow);
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
    }
}
