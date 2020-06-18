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
using System.Runtime.InteropServices;
using System.Data.SqlClient;
using System.Transactions;
using System.Linq;

namespace Sci.Production.Packing
{
    /// <summary>
    /// Packing_P02
    /// </summary>
    public partial class P02 : Sci.Win.Tems.Input6
    {
        private Ict.Win.UI.DataGridViewTextBoxColumn col_refno;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_refno_Balance;
        private Ict.Win.UI.DataGridViewNumericBoxColumn col_qtyperctn;
        private Ict.Win.UI.DataGridViewNumericBoxColumn col_shipqty;
        private string printPackMethod = string.Empty;
        private int orderQty = 0;
        private int ttlShipQty = 0;

        /// <summary>
        /// P02
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public P02(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = "MDivisionID = '" + Sci.Env.User.Keyword + "'";
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
            this.DetailSelectCommand = string.Format(
                @"
select a.ID
	   , a.Refno
	   , a.Article
	   , a.Color
	   , a.SizeCode
	   , a.QtyPerCTN
	   , a.ShipQty
	   , a.NW
	   , a.GW
	   , a.NNW
	   , c.Description
       , selected = cast(0 as bit)
       , [Balance] = iif(isnull(a.QtyPerCTN,0) = 0, 0, a.ShipQty % a.QtyPerCTN)
	   , a.RefNoForBalance
       , [DescriptionforBalance] = c2.Description
from PackingGuide_Detail a WITH (NOLOCK) 
left join PackingGuide b WITH (NOLOCK) on a.Id = b.Id
left join LocalItem c WITH (NOLOCK) on a.RefNo = c.RefNo
left join LocalItem c2 WITH (NOLOCK) on a.RefNoForBalance = c2.RefNo
left join Orders d WITH (NOLOCK) on b.OrderID = d.ID
left join Order_Article e WITH (NOLOCK) on b.OrderID = e.Id 
										   and a.Article = e.Article
left join Order_SizeCode f WITH (NOLOCK) on d.POID = f.Id 
											and a.SizeCode = f.SizeCode
where a.Id = '{0}'
order by e.Seq, f.Seq", masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        /// <summary>
        /// OnFormLoaded
        /// </summary>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            Dictionary<string, string> comboBox1_RowSource = new Dictionary<string, string>();
            comboBox1_RowSource.Add("1", "SOLID COLOR/SIZE");
            comboBox1_RowSource.Add("2", "SOLID COLOR/ASSORTED SIZE");
            comboBox1_RowSource.Add("3", "ASSORTED COLOR/SOLID SIZE");
            comboBox1_RowSource.Add("4", "ASSORTED COLOR/SIZE");
            comboBox1_RowSource.Add("5", "OTHER");
            this.comboPackingMethod.DataSource = new BindingSource(comboBox1_RowSource, null);
            this.comboPackingMethod.ValueMember = "Key";
            this.comboPackingMethod.DisplayMember = "Value";
        }

        /// <summary>
        /// OnDetailEntered
        /// </summary>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.txtCartonRef.Text = string.Empty;
            DataRow orderData;
            string sqlCmd;
            sqlCmd = string.Format(
                @"select o.StyleID, o.SeasonID, o.CustPONo, os.Qty, o.CtnType, o.Packing,o.FtyGroup 
                    from Orders o WITH (NOLOCK) 
                    outer apply(
	                    select Qty
	                    from Order_QtyShip  WITH (NOLOCK)
	                    where Id = o.ID
	                    and Seq = '{1}'
                    )os 
                    where o.ID = '{0}'",
                MyUtility.Convert.GetString(this.CurrentMaintain["OrderID"]),
                MyUtility.Convert.GetString(this.CurrentMaintain["OrderShipmodeSeq"]));
            if (MyUtility.Check.Seek(sqlCmd, out orderData))
            {
                this.displayStyle.Value = orderData["StyleID"].ToString();
                this.displaySeason.Value = orderData["SeasonID"].ToString();
                this.displayPONo.Value = orderData["CustPONo"].ToString();
                this.numOrderQty.Value = MyUtility.Convert.GetInt(orderData["Qty"]);
                this.orderQty = MyUtility.Convert.GetInt(orderData["Qty"]);
                this.comboPackingMethod.SelectedValue = orderData["CtnType"].ToString();
                this.printPackMethod = orderData["CtnType"].ToString();
            }
            else
            {
                this.displayStyle.Value = string.Empty;
                this.displaySeason.Value = string.Empty;
                this.displayPONo.Value = string.Empty;
                this.numOrderQty.Value = 0;
                this.comboPackingMethod.SelectedValue = string.Empty;
                this.numTotalShipQty.Value = 0;
                this.orderQty = 0;
                this.ttlShipQty = 0;
                this.printPackMethod = string.Empty;
            }

            sqlCmd = string.Format("select isnull(SUM(ShipQty),0) from PackingGuide_Detail WITH (NOLOCK) where Id = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));
            this.numTotalShipQty.Value = MyUtility.Convert.GetInt(MyUtility.GetValue.Lookup(sqlCmd));
            this.ttlShipQty = MyUtility.Convert.GetInt(this.numTotalShipQty.Value);

            // Special Instruction按鈕變色
            if (MyUtility.Check.Empty(this.CurrentMaintain["SpecialInstruction"].ToString()))
            {
                this.btnSpecialInstruction.ForeColor = Color.Black;
            }
            else
            {
                this.btnSpecialInstruction.ForeColor = Color.Blue;
            }

            // Carton Dimension按鈕變色
            if (MyUtility.Check.Seek(this.CurrentMaintain["OrderID"].ToString(), "Order_CTNData", "ID"))
            {
                this.btnCartonDimension.ForeColor = Color.Blue;
            }
            else
            {
                this.btnCartonDimension.ForeColor = Color.Black;
            }

            // Switch to Packing list是否有權限使用
            this.btnSwitchToPackingList.Enabled = !this.EditMode && Prgs.GetAuthority(Sci.Env.User.UserID, "P02. Packing Guide", "CanEdit");
            this.btnSwitchToPLByArticle.Enabled = !this.EditMode && Prgs.GetAuthority(Sci.Env.User.UserID, "P02. Packing Guide", "CanEdit");
        }

        /// <summary>
        /// OnDetailGridSetup
        /// </summary>
        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();
            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .CheckBox("selected", header: string.Empty, width: Widths.AnsiChars(5), trueValue: 1, falseValue: 0)
                .CellCartonItem("RefNo", header: "Ref No.", width: Widths.AnsiChars(13)).Get(out this.col_refno)
                .Text("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("Article", header: "ColorWay", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("Color", header: "Color", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Numeric("QtyPerCTN", header: "Qty/Ctn").Get(out this.col_qtyperctn)
                .Numeric("ShipQty", header: "ShipQty").Get(out this.col_shipqty)
                .Numeric("NW", header: "N.W./Ctn", integer_places: 3, decimal_places: 3, maximum: 999.999M, minimum: 0)
                .Numeric("GW", header: "G.W./Ctn", integer_places: 3, decimal_places: 3, maximum: 999.999M, minimum: 0)
                .Numeric("NNW", header: "N.N.W./Ctn", integer_places: 3, decimal_places: 3, maximum: 999.999M, minimum: 0)
                .Numeric("Balance", header: "Balance", integer_places: 3, decimal_places: 3, maximum: 999.999M, minimum: 0, iseditingreadonly: true)
                .CellCartonItem("RefNoForBalance", header: "Ref No. for Balance", width: Widths.AnsiChars(15)).Get(out this.col_refno_Balance)
                .Text("DescriptionforBalance", header: "Description for Balance", width: Widths.AnsiChars(20), iseditingreadonly: true);

            this.detailgrid.CellValueChanged += (s, e) =>
            {
                DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                #region 選完RefNo後，要自動帶出Description與G.W
                if (this.detailgrid.Columns[e.ColumnIndex].DataPropertyName == this.col_refno.DataPropertyName)
                {
                    if (MyUtility.Check.Empty(dr["RefNo"]))
                    {
                        dr["Description"] = string.Empty;
                        dr["GW"] = dr["NW"];
                    }
                    else
                    {
                        // sql參數
                        System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter();
                        sp1.ParameterName = "@refno";
                        sp1.Value = dr["RefNo"].ToString();

                        IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                        cmds.Add(sp1);

                        string sqlCmd = "select Description,CtnWeight from LocalItem WITH (NOLOCK) where RefNo = @refno";
                        DataTable localItemData;
                        DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out localItemData);
                        if (result)
                        {
                            if (localItemData.Rows.Count > 0)
                            {
                                dr["Description"] = localItemData.Rows[0]["Description"].ToString();
                                dr["GW"] = MyUtility.Convert.GetDouble(dr["NW"]) + MyUtility.Convert.GetDouble(localItemData.Rows[0]["CtnWeight"]);
                            }
                            else
                            {
                                dr["Description"] = string.Empty;
                                dr["GW"] = dr["NW"];
                            }
                        }
                        else
                        {
                            MyUtility.Msg.WarningBox("Sql fail!!\r\n" + result.ToString());
                            return;
                        }
                    }

                    dr.EndEdit();
                }
                #endregion

                #region 輸入Qty/Ctn後要重算N.W.,G.W.,N.N.W.,Balance
                if (this.detailgrid.Columns[e.ColumnIndex].DataPropertyName == this.col_qtyperctn.DataPropertyName)
                {
                    // sql參數
                    System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter();
                    System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter();
                    System.Data.SqlClient.SqlParameter sp3 = new System.Data.SqlClient.SqlParameter();
                    System.Data.SqlClient.SqlParameter sp4 = new System.Data.SqlClient.SqlParameter();
                    sp1.ParameterName = "@article";
                    sp1.Value = dr["Article"].ToString();
                    sp2.ParameterName = "@sizecode";
                    sp2.Value = dr["SizeCode"].ToString();
                    sp3.ParameterName = "@refno";
                    sp3.Value = dr["RefNo"].ToString();
                    sp4.ParameterName = "@orderid";
                    sp4.Value = this.CurrentMaintain["OrderID"].ToString();

                    IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                    cmds.Add(sp1);
                    cmds.Add(sp2);
                    cmds.Add(sp3);
                    cmds.Add(sp4);

                    string sqlCmd = @"
select  isnull(li.CtnWeight, 0) as CTNWeight
        , isnull(sw.NW, isnull(sw2.NW, 0)) as NW
        , isnull(sw.NNW, isnull(sw2.NNW, 0)) as NNW
from Orders o WITH (NOLOCK) 
left join Style_WeightData sw WITH (NOLOCK) on sw.StyleUkey = o.StyleUkey 
                                               and sw.Article = @article 
                                               and sw.SizeCode = @sizecode
left join Style_WeightData sw2 WITH (NOLOCK) on sw2.StyleUkey = o.StyleUkey 
                                                and sw2.Article = '----' 
                                                and sw2.SizeCode = @sizecode
left join LocalItem li WITH (NOLOCK) on li.RefNo = @refno 
                                        and li.Category = 'CARTON'
where o.ID = @orderid";
                    DataTable selectedData;
                    DualResult result;
                    if (result = DBProxy.Current.Select(null, sqlCmd, cmds, out selectedData))
                    {
                        dr["NW"] = selectedData.Rows.Count == 0 ? 0 : MyUtility.Convert.GetDouble(selectedData.Rows[0]["NW"]) * MyUtility.Convert.GetDouble(dr["QtyPerCTN"]);
                        dr["GW"] = selectedData.Rows.Count == 0 ? 0 : MyUtility.Convert.GetDouble(dr["NW"]) + MyUtility.Convert.GetDouble(selectedData.Rows[0]["CTNWeight"]);
                        dr["NNW"] = selectedData.Rows.Count == 0 ? 0 : MyUtility.Convert.GetDouble(selectedData.Rows[0]["NNW"]) * MyUtility.Convert.GetDouble(dr["QtyPerCTN"]);
                    }
                    else
                    {
                        MyUtility.Msg.WarningBox("Sql connection fail!!\r\n" + result.ToString());
                        dr["NW"] = 0;
                        dr["GW"] = 0;
                        dr["NNW"] = 0;
                    }

                    dr["Balance"] = MyUtility.Convert.GetInt(dr["QtyPerCTN"]) == 0 ? 0 : MyUtility.Convert.GetInt(dr["ShipQty"]) % MyUtility.Convert.GetInt(dr["QtyPerCTN"]);
                    dr.EndEdit();
                }
                #endregion

                #region 輸入ShipQty後要重算Balance
                if (this.detailgrid.Columns[e.ColumnIndex].DataPropertyName == this.col_shipqty.DataPropertyName)
                {
                    dr["Balance"] = MyUtility.Convert.GetInt(dr["QtyPerCTN"]) == 0 ? 0 : MyUtility.Convert.GetInt(dr["ShipQty"]) % MyUtility.Convert.GetInt(dr["QtyPerCTN"]);
                    dr.EndEdit();
                }
                #endregion

                #region 選完RefNoForBalance後，要自動帶出DescriptionforBalance
                if (this.detailgrid.Columns[e.ColumnIndex].DataPropertyName == this.col_refno_Balance.DataPropertyName)
                {
                    dr["DescriptionforBalance"] = string.Empty;
                    if (!MyUtility.Check.Empty(dr["RefNoForBalance"]))
                    {
                        // sql參數
                        SqlParameter sp1 = new SqlParameter();
                        sp1.ParameterName = "@RefNoForBalance";
                        sp1.Value = dr["RefNoForBalance"].ToString();

                        IList<SqlParameter> cmds = new List<SqlParameter>();
                        cmds.Add(sp1);

                        string sqlCmd = "select Description from LocalItem WITH (NOLOCK) where RefNo = @RefNoForBalance";
                        DataTable localItemData;
                        DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out localItemData);
                        if (result)
                        {
                            if (localItemData.Rows.Count > 0)
                            {
                                dr["DescriptionforBalance"] = localItemData.Rows[0]["Description"].ToString();
                            }
                        }
                        else
                        {
                            MyUtility.Msg.WarningBox("Sql fail!!\r\n" + result.ToString());
                            return;
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
            this.CurrentMaintain["CTNStartNo"] = 1;
            this.displayStyle.Value = string.Empty;
            this.displaySeason.Value = string.Empty;
            this.displayPONo.Value = string.Empty;
            this.numOrderQty.Value = 0;
            this.numTotalShipQty.Value = 0;
            this.comboPackingMethod.SelectedValue = string.Empty;
        }

        /// <summary>
        /// ClickEditAfter
        /// </summary>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.ControlGridColumn();
        }

        /// <summary>
        /// ClickSaveBefore
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["OrderID"]))
            {
                this.txtSPNo.Focus();
                MyUtility.Msg.WarningBox("< SP No. > can not be empty!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["OrderShipmodeSeq"]))
            {
                this.txtSeq.Focus();
                MyUtility.Msg.WarningBox("< Seq > can not be empty!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["ShipModeID"]))
            {
                this.txtshipmode.Focus();
                MyUtility.Msg.WarningBox("< Shipping Mode > can not be empty!");
                return false;
            }

            // 檢查OrderID+Seq不可以重複建立
            DataRow existid;
            if (MyUtility.Check.Seek(string.Format("select ID from PackingGuide WITH (NOLOCK) where OrderID = '{0}' AND OrderShipmodeSeq = '{1}' AND ID != '{2}'", this.CurrentMaintain["OrderID"].ToString(), this.CurrentMaintain["OrderShipmodeSeq"].ToString(), this.IsDetailInserting ? string.Empty : this.CurrentMaintain["ID"].ToString()), out existid))
            {
                MyUtility.Msg.WarningBox($"SP No: {this.CurrentMaintain["OrderID"]}, Seq: {this.CurrentMaintain["OrderShipmodeSeq"]} already exists in packing guide {existid["ID"]}, can't be created again!");
                return false;
            }

            // 檢查表身不可以沒有資料
            DataRow[] detailData = ((DataTable)this.detailgridbs.DataSource).Select();
            if (detailData.Length == 0)
            {
                MyUtility.Msg.WarningBox("Detail can't empty!");
                return false;
            }

            // 表身的Ref No.與Qty/CTN不可以為空值
            foreach (DataRow dr in detailData)
            {
                bool isEmptyRefNo = MyUtility.Check.Empty(dr["RefNo"]);
                bool isEmptyQtyPerCTN = MyUtility.Check.Empty(dr["QtyPerCTN"]);
                bool isEmptyShipQty = MyUtility.Check.Empty(dr["ShipQty"]);
                if (isEmptyQtyPerCTN && !isEmptyShipQty)
                {
                    MyUtility.Msg.WarningBox("< Color Way > " + dr["Article"].ToString().Trim() + " < Qty/Ctn > can't empty!");
                    return false;
                }

                if (isEmptyRefNo && !isEmptyShipQty)
                {
                    MyUtility.Msg.WarningBox("< Color Way > " + dr["Article"].ToString().Trim() + " < Ref No. > can't empty!");
                    return false;
                }
            }

            #region 單色混碼 RefNo 需相同
            if (this.comboPackingMethod.SelectedIndex != -1 &&
                   this.comboPackingMethod.SelectedValue.ToString().EqualString("2"))
            {
                List<string> listRefNo = ((DataTable)this.detailgridbs.DataSource)
                         .AsEnumerable()
                         .GroupBy(x => x.Field<string>("RefNoForBalance"))
                         .Select(x => x.Key).ToList();
                if (listRefNo.Count() > 1)
                {
                    MyUtility.Msg.WarningBox("Packing Method : SOLID COLOR/ASSORTED SIZE\r\nCTN Ref No. must be the same for balance.");
                    return false;
                }
            }
            #endregion

            #region 計算Total Cartons & CBM

            // Total Cartons: 單色混碼裝：min(無條件捨去(同一顏色不同Size的訂單件數/每箱件數)) + (1(若其中一個Size有餘數) or 0(完全整除沒有餘數))
            int ttlCTN = 0, ctns = 0;
            double ctn, ttlCBM = 0.0;
            string cbm, cbmBalance;
            if (this.comboPackingMethod.SelectedIndex != -1)
            {
                if (this.comboPackingMethod.SelectedValue.ToString().EqualString("2"))
                {
                    DataTable groupData;
                    DualResult result;
                    if (result = DBProxy.Current.Select(null, "select '' as Article, 10 as ctn, 0.0 as CBM, 0 as Remainder, '' as RefNoForBalance where 1=0", out groupData))
                    {
                        string article = string.Empty;
                        int recordNo = -1;
                        foreach (DataRow dr in detailData)
                        {
                            if (article != dr["Article"].ToString())
                            {
                                article = dr["Article"].ToString();
                                DataRow dr1 = groupData.NewRow();
                                dr1["Article"] = article;
                                dr1["CBM"] = MyUtility.Convert.GetDouble(MyUtility.GetValue.Lookup("CBM", dr["RefNo"].ToString(), "LocalItem", "RefNo"));
                                groupData.Rows.Add(dr1);
                                recordNo += 1;
                            }

                            if (MyUtility.Check.Empty(MyUtility.Convert.GetDouble(dr["QtyPerCTN"])))
                            {
                                ctn = 0;
                            }
                            else
                            {
                                ctn = MyUtility.Convert.GetDouble(dr["ShipQty"]) / MyUtility.Convert.GetDouble(dr["QtyPerCTN"]);
                                if ((MyUtility.Convert.GetInt(dr["ShipQty"].ToString()) % MyUtility.Convert.GetInt(dr["QtyPerCTN"].ToString())) != 0)
                                {
                                    groupData.Rows[recordNo]["Remainder"] = 1;
                                    groupData.Rows[recordNo]["RefNoForBalance"] = dr["RefNoForBalance"];
                                }
                            }

                            ctns = (int)Math.Floor(ctn);
                            if (MyUtility.Check.Empty(groupData.Rows[recordNo]["ctn"]) || (MyUtility.Convert.GetInt(groupData.Rows[recordNo]["ctn"]) > ctns))
                            {
                                groupData.Rows[recordNo]["ctn"] = ctns;
                            }
                        }

                        foreach (DataRow dr in groupData.Rows)
                        {
                            int remainder = 0;
                            if (dr["Remainder"].ToString() == "1")
                            {
                                remainder = 1;
                            }

                            ttlCTN = ttlCTN + MyUtility.Convert.GetInt(dr["ctn"].ToString()) + remainder;

                            // 有設定尾箱的料號
                            if (!MyUtility.Check.Empty(dr["RefNoForBalance"]) && remainder == 1)
                            {
                                // 最後一箱CBM需另外抓取
                                cbmBalance = MyUtility.GetValue.Lookup("CBM", dr["RefNoForBalance"].ToString(), "LocalItem", "RefNo");
                                ttlCBM = ttlCBM + (MyUtility.Convert.GetDouble(dr["CBM"]) * MyUtility.Convert.GetInt(dr["ctn"])) + MyUtility.Convert.GetDouble(cbmBalance);
                            }
                            else
                            {
                                ttlCBM = ttlCBM + (MyUtility.Convert.GetDouble(dr["CBM"]) * (MyUtility.Convert.GetInt(dr["ctn"]) + remainder));
                            }
                        }
                    }
                    else
                    {
                        MyUtility.Msg.WarningBox("Sql fail!!\r\n" + result.ToString());
                        return false;
                    }
                }
                else
                {
                    // Total Cartons: 表身每一列資料的訂單件數/每箱件數無條件進位後加總
                    foreach (DataRow dr in detailData)
                    {
                        if (MyUtility.Check.Empty(MyUtility.Convert.GetDouble(dr["QtyPerCTN"])))
                        {
                            ctn = 0;
                        }
                        else
                        {
                            ctn = MyUtility.Convert.GetDouble(dr["ShipQty"]) / MyUtility.Convert.GetDouble(dr["QtyPerCTN"]);
                        }

                        ctns = (int)Math.Ceiling(ctn);
                        ttlCTN = ttlCTN + ctns;
                        cbm = MyUtility.GetValue.Lookup("CBM", dr["RefNo"].ToString(), "LocalItem", "RefNo");

                        // 有設定尾箱的料號
                        if (!MyUtility.Check.Empty(dr["RefNoForBalance"]) &&
                             MyUtility.Convert.GetDouble(dr["ShipQty"]) % MyUtility.Convert.GetDouble(dr["QtyPerCTN"]) > 0)
                        {
                            // 最後一箱CBM需另外抓取
                            cbmBalance = MyUtility.GetValue.Lookup("CBM", dr["RefNoForBalance"].ToString(), "LocalItem", "RefNo");
                            ttlCBM = ttlCBM + (MyUtility.Convert.GetDouble(cbm) * (ctns - 1)) + MyUtility.Convert.GetDouble(cbmBalance);
                        }
                        else
                        {
                            ttlCBM = ttlCBM + (MyUtility.Convert.GetDouble(cbm) * ctns);
                        }
                    }
                }
            }
            #endregion
            this.CurrentMaintain["CTNQty"] = ttlCTN;
            this.CurrentMaintain["CBM"] = ttlCBM;

            // GetID
            if (this.IsDetailInserting)
            {
                string id = MyUtility.GetValue.GetID(MyUtility.GetValue.Lookup("FtyGroup", this.CurrentMaintain["OrderID"].ToString(), "Orders", "ID") + "PG", "PackingGuide", DateTime.Today, 2, "Id", null);
                if (MyUtility.Check.Empty(id))
                {
                    MyUtility.Msg.WarningBox("GetID fail, please try again!");
                    return false;
                }

                this.CurrentMaintain["ID"] = id;
            }

            return base.ClickSaveBefore();
        }

        /// <summary>
        /// ClickPrint
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ClickPrint()
        {
            string minCtnQty = "0";

            // 如果是單色混碼包裝，就先算出最少箱數
            if (this.printPackMethod == "2")
            {
                minCtnQty = MyUtility.GetValue.Lookup(string.Format("select isnull(min(ShipQty/QtyPerCTN),0) from PackingGuide_Detail WITH (NOLOCK) where Id = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["ID"])));
            }

            string sqlCmd = string.Format(
                @"
select pd.Article,pd.Color,pd.SizeCode,pd.QtyPerCTN,pd.ShipQty,
    IIF(pd.ShipQty=0 or pd.QtyPerCTN=0,0,pd.ShipQty/pd.QtyPerCTN)as CtnQty,
    o.CustCDID,o.StyleID,o.CustPONo,o.Customize1,c.Alias,oq.BuyerDelivery
from PackingGuide p WITH (NOLOCK) 
left join PackingGuide_Detail pd WITH (NOLOCK) on p.Id = pd.Id
left join Orders o WITH (NOLOCK) on o.ID = p.OrderID
left join Order_Article oa WITH (NOLOCK) on oa.id = o.ID and oa.Article = pd.Article
left join Order_SizeCode os WITH (NOLOCK) on os.Id = o.POID and os.SizeCode = pd.SizeCode
left join Country c WITH (NOLOCK) on c.ID = o.Dest
left join Order_QtyShip oq WITH (NOLOCK) on oq.Id = o.ID and oq.Seq = p.OrderShipmodeSeq
where p.Id = '{0}'
order by oa.Seq,os.Seq", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));
            DataTable printData;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out printData);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Query data fail! \r\n" + result.ToString());
                return false;
            }

            if (printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("No data!");
                return false;
            }

            DataTable ctnDim, qtyCtn;
            sqlCmd = string.Format(
                @"
Declare @packinglistid VARCHAR(13),
		@refno VARCHAR(21), 
		@ctnstartno VARCHAR(6),
		@firstctnno VARCHAR(6),
		@lastctnno VARCHAR(6),
		@orirefnno VARCHAR(21),
		@insertrefno VARCHAR(13)

set @packinglistid = '{0}'

--建立暫存PackingList_Detail資料
DECLARE @tempPackingListDetail TABLE (
   RefNo VARCHAR(21),
   CTNNo VARCHAR(13)
)

--撈出PackingList_Detail
DECLARE cursor_PackingListDetail CURSOR FOR
	SELECT RefNo,CTNStartNo FROM PackingList_Detail WITH (NOLOCK) WHERE ID = @packinglistid and CTNQty > 0 ORDER BY Seq

--開始run cursor
OPEN cursor_PackingListDetail
--將第一筆資料填入變數
FETCH NEXT FROM cursor_PackingListDetail INTO @refno, @ctnstartno
SET @firstctnno = @ctnstartno
SET @lastctnno = @ctnstartno
SET @orirefnno = @refno
WHILE @@FETCH_STATUS = 0
BEGIN
	IF(@orirefnno <> @refno)
		BEGIN
			IF(@firstctnno = @lastctnno)
				BEGIN
					SET @insertrefno = @firstctnno
				END
			ELSE
				BEGIN
					SET @insertrefno = @firstctnno + '-' + @lastctnno
				END
			INSERT INTO @tempPackingListDetail (RefNo,CTNNo) VALUES (@orirefnno,@insertrefno)

			--數值重新記錄
			SET @orirefnno = @refno
			SET @firstctnno = @ctnstartno
			SET @lastctnno = @ctnstartno
		END
	ELSE
		BEGIN
			--紀錄箱號
			SET @lastctnno = @ctnstartno
		END

	FETCH NEXT FROM cursor_PackingListDetail INTO @refno, @ctnstartno
END
--最後一筆資料
--最後一筆資料
IF(@orirefnno <> '')
	BEGIN
		IF(@firstctnno = @lastctnno)
			BEGIN
				SET @insertrefno = @firstctnno
			END
		ELSE
			BEGIN
				SET @insertrefno = @firstctnno + '-' + @lastctnno
			END
		INSERT INTO @tempPackingListDetail (RefNo,CTNNo) VALUES (@orirefnno,@insertrefno)
	END
--關閉cursor與參數的關聯
CLOSE cursor_PackingListDetail
--將cursor物件從記憶體移除
DEALLOCATE cursor_PackingListDetail

select distinct t.RefNo,
Ctn = concat('(CTN#:',stuff((select concat(',',CTNNo) from @tempPackingListDetail where RefNo = t.RefNo for xml path('')),1,1,''),')')
into #tmp
from @tempPackingListDetail t
left join LocalItem l on l.RefNo = t.RefNo
order by RefNo

select distinct pd.RefNo, li.Description, STR(li.CtnLength,8,4)+'*'+STR(li.CtnWidth,8,4)+'*'+STR(li.CtnHeight,8,4) as Dimension, li.CtnUnit,a.Ctn
from PackingGuide_Detail pd WITH (NOLOCK) 
left join LocalItem li WITH (NOLOCK) on li.RefNo = pd.RefNo
left join LocalSupp ls WITH (NOLOCK) on ls.ID = li.LocalSuppid
outer apply(select Ctn from #tmp where Refno = pd.RefNo)a
where pd.ID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));
            result = DBProxy.Current.Select(null, sqlCmd, out ctnDim);

            sqlCmd = string.Format(
                @"
select isnull(oq.Article,'') as Article,isnull(oq.SizeCode,'') as SizeCode,isnull(oq.Qty,0) as Qty
from Orders o WITH (NOLOCK) 
left join Order_QtyCTN oq WITH (NOLOCK) on o.ID = oq.Id
left join Order_Article oa WITH (NOLOCK) on o.ID = oa.id and oq.Article = oa.Article
left join Order_SizeCode os WITH (NOLOCK) on o.POID = os.Id and oq.SizeCode = os.SizeCode
where o.ID = '{0}'
order by oa.Seq,os.Seq", MyUtility.Convert.GetString(this.CurrentMaintain["OrderID"]));
            result = DBProxy.Current.Select(null, sqlCmd, out qtyCtn);

            string strXltName = Sci.Env.Cfg.XltPathDir + "\\Packing_P02.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return false;
            }

            this.ShowWaitMessage("Starting to excel...");
            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];
            string nameEN = MyUtility.GetValue.Lookup("NameEN", Sci.Env.User.Factory, "Factory ", "id");
            worksheet.Cells[1, 1] = nameEN;
            worksheet.Cells[3, 2] = MyUtility.Check.Empty(printData.Rows[0]["BuyerDelivery"]) ? string.Empty : Convert.ToDateTime(printData.Rows[0]["BuyerDelivery"]).ToShortDateString();
            worksheet.Cells[3, 19] = Convert.ToDateTime(DateTime.Today).ToString("d");
            worksheet.Cells[4, 2] = MyUtility.Convert.GetString(printData.Rows[0]["CustCDID"]);
            worksheet.Cells[6, 1] = MyUtility.Convert.GetString(this.CurrentMaintain["OrderID"]);
            worksheet.Cells[6, 2] = MyUtility.Convert.GetString(printData.Rows[0]["StyleID"]);
            worksheet.Cells[6, 5] = MyUtility.Convert.GetString(printData.Rows[0]["Customize1"]);
            worksheet.Cells[6, 8] = MyUtility.Convert.GetString(printData.Rows[0]["CustPONo"]);
            worksheet.Cells[6, 11] = MyUtility.Convert.GetInt(this.CurrentMaintain["CTNQty"]);
            worksheet.Cells[6, 13] = MyUtility.Convert.GetString(printData.Rows[0]["Alias"]);
            worksheet.Cells[6, 17] = this.orderQty;
            worksheet.Cells[6, 19] = this.ttlShipQty;
            worksheet.Cells[6, 20] = "=Q6-S6";
            int row = 8, ctnNum = MyUtility.Convert.GetInt(this.CurrentMaintain["CTNStartNo"]), ttlCtn = 0;

            #region 先算出總共會有幾筆record
            int tmpCtnQty = 0;
            foreach (DataRow dr in printData.Rows)
            {
                int ctnQty = this.printPackMethod == "2" ? MyUtility.Convert.GetInt(minCtnQty) : MyUtility.Convert.GetInt(dr["CtnQty"]);
                int ctn = ctnQty == 0 ? 0 : (int)Math.Ceiling(MyUtility.Convert.GetDecimal(ctnQty) / 15);
                int ship = MyUtility.Convert.GetInt(dr["QtyPerCTN"]) * ctnQty;
                tmpCtnQty = tmpCtnQty + ctn + (ship >= MyUtility.Convert.GetInt(dr["ShipQty"]) ? 0 : 1);
            }

            // 範本已先有258 row，不夠的話再新增
            if (tmpCtnQty > 258)
            {
                // Insert row
                for (int i = 1; i <= tmpCtnQty - 258; i++)
                {
                    Microsoft.Office.Interop.Excel.Range rngToCopy = worksheet.get_Range("A8:A8").EntireRow;
                    Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range("A8:A8", Type.Missing).EntireRow;
                    rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown, rngToCopy.Copy(Type.Missing));
                }
            }
            else
            {
                // 刪除多餘的Row
                if (tmpCtnQty < 258)
                {
                    // Insert row
                    for (int i = 1; i <= 258 - tmpCtnQty; i++)
                    {
                        Microsoft.Office.Interop.Excel.Range rng = (Microsoft.Office.Interop.Excel.Range)excel.Rows[8, Type.Missing];
                        rng.Select();
                        rng.Delete(Microsoft.Office.Interop.Excel.XlDirection.xlUp);
                    }
                }
            }
            #endregion

            #region 寫入完整箱的資料
            foreach (DataRow dr in printData.Rows)
            {
                int ctnQty = this.printPackMethod == "2" ? MyUtility.Convert.GetInt(minCtnQty) : MyUtility.Convert.GetInt(dr["CtnQty"]);
                if (!MyUtility.Check.Empty(ctnQty))
                {
                    worksheet.Cells[row, 1] = MyUtility.Convert.GetString(dr["Article"]) + " " + MyUtility.Convert.GetString(dr["Color"]);
                    worksheet.Cells[row, 2] = MyUtility.Convert.GetString(dr["SizeCode"]);
                    worksheet.Cells[row, 3] = MyUtility.Convert.GetInt(dr["QtyPerCTN"]);
                    worksheet.Cells[row, 19] = MyUtility.Convert.GetInt(dr["QtyPerCTN"]) * ctnQty;
                    ttlCtn = 0;
                    if (this.printPackMethod == "2")
                    {
                        ctnNum = MyUtility.Convert.GetInt(this.CurrentMaintain["CTNStartNo"]);
                    }

                    for (int i = 1; i <= Math.Floor(MyUtility.Convert.GetDecimal(ctnQty - 1) / 15) + 1; i++)
                    {
                        for (int j = 1; j <= 15; j++)
                        {
                            ttlCtn++;
                            if (ttlCtn > MyUtility.Convert.GetInt(dr["CtnQty"]))
                            {
                                break;
                            }

                            worksheet.Cells[row, j + 3] = ctnNum;
                            ctnNum++;
                        }

                        row++;
                    }
                }
            }
            #endregion

            #region 處理餘箱部分
            int insertCTN = 1;
            foreach (DataRow dr in printData.Rows)
            {
                int ctnQty = this.printPackMethod == "2" ? MyUtility.Convert.GetInt(minCtnQty) : MyUtility.Convert.GetInt(dr["CtnQty"]);
                int remain = MyUtility.Convert.GetInt(dr["ShipQty"]) - (MyUtility.Convert.GetInt(dr["QtyPerCTN"]) * ctnQty);
                if (remain > 0)
                {
                    worksheet.Cells[row, 1] = MyUtility.Convert.GetString(dr["Article"]) + " " + MyUtility.Convert.GetString(dr["Color"]);
                    worksheet.Cells[row, 2] = MyUtility.Convert.GetString(dr["SizeCode"]);
                    worksheet.Cells[row, 3] = remain;
                    if ((this.printPackMethod == "2" && insertCTN == 1) || this.printPackMethod != "2")
                    {
                        worksheet.Cells[row, 4] = ctnNum;
                        insertCTN = 2;
                    }

                    worksheet.Cells[row, 19] = remain;
                    if (this.printPackMethod != "2")
                    {
                        ctnNum++;
                    }

                    row++;
                }
            }
            #endregion

            int startIndex = 0;
            int endIndex = 0;
            int dataRow = 0;

            // Carton Dimension:
            StringBuilder ctnDimension = new StringBuilder();
            foreach (DataRow dr in ctnDim.Rows)
            {
                ctnDimension.Append(string.Format("{0} / {1} / {2} {3}, {4}  \r\n", MyUtility.Convert.GetString(dr["RefNo"]), MyUtility.Convert.GetString(dr["Description"]), MyUtility.Convert.GetString(dr["Dimension"]), MyUtility.Convert.GetString(dr["CtnUnit"]), MyUtility.Convert.GetString(dr["Ctn"])));
            }

            foreach (DataRow dr in qtyCtn.Rows)
            {
                if (!MyUtility.Check.Empty(dr["Article"]))
                {
                    ctnDimension.Append(string.Format("{0} -> {1} / {2}, ", MyUtility.Convert.GetString(dr["Article"]), MyUtility.Convert.GetString(dr["SizeCode"]), MyUtility.Convert.GetString(dr["Qty"])));
                }
            }

            string cds = ctnDimension.Length > 0 ? ctnDimension.ToString().Substring(0, ctnDimension.ToString().Length - 2) : string.Empty;
            string[] cdsab = cds.Split('\r');
            int cdsi = 0;
            int cdsl = 113;
            foreach (string cdsc in cdsab)
            {
                if (cdsc.Length > cdsl)
                {
                    int h = cdsc.Length / cdsl;
                    for (int i = 0; i < h; i++)
                    {
                        cdsi += 1;
                    }
                }
            }

            int cdinst = 0;
            cdsi += cdsab.Length - 2;
            if (cdsi > 0)
            {
                for (int i = 0; i < cdsi; i++)
                {
                    Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(row + 1)), Type.Missing).EntireRow;
                    rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
                    Marshal.ReleaseComObject(rngToInsert);
                    cdinst++;
                }
            }

            worksheet.Cells[row, 2] = ctnDimension.Length > 0 ? cds : string.Empty;
            row = row + cdinst + 2;
            worksheet.Cells[row, 1] = "Remark: " + MyUtility.Convert.GetString(this.CurrentMaintain["Remark"]);

            // 填Special Instruction
            // 先取得Special Instruction總共有幾行
            string tmp = MyUtility.Convert.GetString(this.CurrentMaintain["SpecialInstruction"]);

            string[] tmpab = tmp.Split('\r');
            int ctmpc = 0;
            int l = 113;
            foreach (string tmpc in tmpab)
            {
                if (tmpc.Length > l)
                {
                    int h = tmpc.Length / l;
                    ctmpc += h;
                }
            }

            for (int i = 1; ; i++)
            {
                if (i > 1)
                {
                    startIndex = endIndex + 2;
                }

                if (tmp.IndexOf("\r\n", startIndex) > 0)
                {
                    endIndex = tmp.IndexOf("\r\n", startIndex);
                }
                else
                {
                    dataRow = i + 2 + ctmpc;
                    break;
                }
            }

            row++;
            if (dataRow > 2)
            {
                for (int i = 3; i < dataRow; i++)
                {
                    Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(row + 1)), Type.Missing).EntireRow;
                    rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
                    rngToInsert.RowHeight = 19.5;
                    Marshal.ReleaseComObject(rngToInsert);
                }
            }

            // 判斷第一碼為"=" 就塞space ,避免excel 誤認=是計算函數
            if (MyUtility.Check.Empty(this.CurrentMaintain["SpecialInstruction"]))
            {
                worksheet.Cells[row, 2] = MyUtility.Convert.GetString(this.CurrentMaintain["SpecialInstruction"]);
            }
            else if (this.CurrentMaintain["SpecialInstruction"].ToString().Substring(0, 1) == "=")
            {
                worksheet.Cells[row, 2] = "'" + MyUtility.Convert.GetString(this.CurrentMaintain["SpecialInstruction"]);
            }
            else
            {
                worksheet.Cells[row, 2] = MyUtility.Convert.GetString(this.CurrentMaintain["SpecialInstruction"]);
            }

            #region Save & Show Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Packing_P02");
            Microsoft.Office.Interop.Excel.Workbook workbook = excel.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            Marshal.ReleaseComObject(worksheet);
            Marshal.ReleaseComObject(workbook);

            strExcelName.OpenFile();
            #endregion
            this.HideWaitMessage();
            return base.ClickPrint();
        }

        // 控制Grid欄位的可修改性
        private void ControlGridColumn()
        {
            // 當Packing Method為SOLID COLOR/ASSORTED SIZE (Order.CTNType = ‘2’)時，欄位Qty/Ctn不可被修改
            if (this.comboPackingMethod.SelectedIndex != -1 && this.comboPackingMethod.SelectedValue.ToString() == "2")
            {
                this.col_qtyperctn.IsEditingReadOnly = true;
                this.detailgrid.Columns[5].DefaultCellStyle.ForeColor = Color.Black;
            }
            else
            {
                this.col_qtyperctn.IsEditingReadOnly = false;
                this.detailgrid.Columns[5].DefaultCellStyle.ForeColor = Color.Red;
            }
        }

        // 檢查輸入的SP#是否正確
        private void TxtSPNo_Validating(object sender, CancelEventArgs e)
        {
            if (this.EditMode)
            {
                if (this.txtSPNo.Text != this.txtSPNo.OldValue)
                {
                    bool returnData = false;
                    #region 檢查輸入的值是否符合條件
                    if (!MyUtility.Check.Empty(this.txtSPNo.Text))
                    {
                        DataRow orderData;
                        string sqlCmd = string.Format(
                            @"
select o.Category
       , o.LocalOrder
       , o.IsForecast 
from Orders o WITH (NOLOCK) 
inner join Factory f on o.FactoryID = f.ID
where o.ID = '{0}' 
      and o.MDivisionID = '{1}'
      and f.IsProduceFty = 1",
                            this.txtSPNo.Text,
                            Env.User.Keyword);
                        if (MyUtility.Check.Seek(sqlCmd, out orderData))
                        {
                            string msg = string.Empty;

                            // 只能建立大貨單的資料
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
                                case "T":
                                    msg = "category: < Sample MTL Booking >, it can't be created!";
                                    returnData = true;
                                    break;
                                case "O":
                                    msg = "category: < Other >, it can't be created!";
                                    returnData = true;
                                    break;
                                case "S":
                                    if (orderData["LocalOrder"].ToString() == "True")
                                    {
                                        msg = " is < Local order >, it can't be created!";
                                        returnData = true;
                                    }

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
                                MyUtility.Msg.WarningBox("SP#:" + this.txtSPNo.Text + msg);

                                // OrderID異動，其他相關欄位要跟著異動
                                this.ChangeOtherData(string.Empty);
                                this.txtSPNo.Text = string.Empty;
                            }
                        }
                        else
                        {
                            returnData = true;
                            MyUtility.Msg.WarningBox("< SP# > does not exist!");

                            // OrderID異動，其他相關欄位要跟著異動
                            this.ChangeOtherData(string.Empty);
                            this.txtSPNo.Text = string.Empty;
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
        private void TxtSPNo_Validated(object sender, EventArgs e)
        {
            if (this.txtSPNo.OldValue == this.txtSPNo.Text)
            {
                return;
            }

            // OrderID異動，其他相關欄位要跟著異動
            this.ChangeOtherData(this.txtSPNo.Text);
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
                this.CurrentMaintain["FactoryID"] = string.Empty;
                this.displayStyle.Value = string.Empty;
                this.displaySeason.Value = string.Empty;
                this.displayPONo.Value = string.Empty;
                this.numOrderQty.Value = 0;
                this.numTotalShipQty.Value = 0;
                this.comboPackingMethod.SelectedValue = string.Empty;
            }
            else
            {
                DataRow orderData;
                string sqlCmd;
                sqlCmd = string.Format("select StyleID,SeasonID,CustPONo,Qty,CtnType,Packing,FtyGroup from Orders WITH (NOLOCK) where ID = '{0}'", orderID);
                if (MyUtility.Check.Seek(sqlCmd, out orderData))
                {
                    // 帶出相關欄位的資料
                    this.displayStyle.Value = orderData["StyleID"].ToString();
                    this.displaySeason.Value = orderData["SeasonID"].ToString();
                    this.displayPONo.Value = orderData["CustPONo"].ToString();
                    this.comboPackingMethod.SelectedValue = orderData["CtnType"].ToString();
                    this.CurrentMaintain["SpecialInstruction"] = orderData["Packing"].ToString();
                    this.CurrentMaintain["FactoryID"] = orderData["FtyGroup"].ToString();

                    #region 若Order_QtyShip有多筆資料話就跳出視窗讓使者選擇Seq
                    sqlCmd = string.Format("select count(ID) as CountID from Order_QtyShip WITH (NOLOCK) where ID = '{0}'", orderID);
                    if (MyUtility.Check.Seek(sqlCmd, out orderData))
                    {
                        if (orderData["CountID"].ToString() == "1")
                        {
                            sqlCmd = string.Format("select ShipModeID,Seq,Qty from Order_QtyShip WITH (NOLOCK) where ID = '{0}'", orderID);
                            if (MyUtility.Check.Seek(sqlCmd, out orderData))
                            {
                                this.CurrentMaintain["OrderShipmodeSeq"] = orderData["Seq"].ToString();
                                this.CurrentMaintain["ShipModeID"] = orderData["ShipModeID"].ToString();
                                this.numTotalShipQty.Value = MyUtility.Convert.GetInt(orderData["Qty"]);
                                this.numOrderQty.Value = MyUtility.Convert.GetInt(orderData["Qty"]);
                            }
                        }
                        else
                        {
                            IList<DataRow> orderQtyShipData;
                            sqlCmd = string.Format("select Seq,BuyerDelivery,ShipmodeID,Qty from Order_QtyShip WITH (NOLOCK) where ID = '{0}'", orderID);
                            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlCmd, "4,20,20,10", string.Empty, "Seq,Buyer Delivery,ShipMode,Qty");
                            DialogResult returnResult = item.ShowDialog();
                            if (returnResult == DialogResult.Cancel)
                            {
                                this.CurrentMaintain["OrderShipmodeSeq"] = string.Empty;
                                this.CurrentMaintain["ShipModeID"] = string.Empty;
                                this.numTotalShipQty.Value = 0;
                                this.numOrderQty.Value = 0;
                            }
                            else
                            {
                                orderQtyShipData = item.GetSelecteds();
                                this.CurrentMaintain["OrderShipmodeSeq"] = item.GetSelectedString();
                                this.CurrentMaintain["ShipModeID"] = orderQtyShipData[0]["ShipmodeID"].ToString();
                                this.numTotalShipQty.Value = MyUtility.Convert.GetInt(orderQtyShipData[0]["Qty"].ToString());
                                this.numOrderQty.Value = MyUtility.Convert.GetInt(orderQtyShipData[0]["Qty"].ToString());
                            }
                        }
                    }
                    #endregion

                    // 產生表身Grid的資料
                    this.GenDetailData(orderID, this.CurrentMaintain["OrderShipmodeSeq"].ToString());
                }
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

            if (!MyUtility.Check.Empty(orderID) && !MyUtility.Check.Empty(seq))
            {
                string sqlCmd;
                if (this.comboPackingMethod.SelectedValue == null || string.IsNullOrEmpty(this.comboPackingMethod.SelectedValue.ToString()))
                {
                    sqlCmd = string.Format(
                        @"select '' as ID, '' as RefNo, '' as Description, oqd.Article, voc.ColorID as Color, oqd.SizeCode, oqd.Qty as ShipQty, o.CTNQty as QtyPerCTN, os.Seq,
	   sw.NW as NW1, sw.NNW as NNW1, sw2.NW as NW2, sw2.NNW as NNW2,
	   isnull(sw.NW, isnull(sw2.NW, 0))*o.CTNQty as NW,
	   isnull(sw.NW, isnull(sw2.NW, 0))*o.CTNQty as GW,
	   isnull(sw.NNW, isnull(sw2.NNW, 0))*o.CTNQty as NNW 
from Order_QtyShip_Detail oqd WITH (NOLOCK) 
left Join Orders o WITH (NOLOCK) on o.ID = oqd.Id
left join View_OrderFAColor voc on voc.id = oqd.Id and voc.Article = oqd.Article
left join Style_WeightData sw WITH (NOLOCK) on sw.StyleUkey = o.StyleUkey and sw.Article = oqd.Article and sw.SizeCode = oqd.SizeCode
left join Style_WeightData sw2 WITH (NOLOCK) on sw2.StyleUkey = o.StyleUkey and sw2.Article = '----' and sw2.SizeCode = oqd.SizeCode
left join Order_SizeCode os WITH (NOLOCK) on os.id = o.POID and os.SizeCode = oqd.SizeCode
left join Order_Article oa WITH (NOLOCK) on oa.id = oqd.Id and oa.Article = oqd.Article
where oqd.ID = '{0}' and oqd.Seq = '{1}'
order by oa.Seq,os.Seq",
                        orderID,
                        seq);
                }
                else if (this.comboPackingMethod.SelectedValue.ToString() == "2")
                {
                    sqlCmd = string.Format("select * from Order_QtyCTN WITH (NOLOCK) where Id = '{0}'", orderID);
                    if (!MyUtility.Check.Seek(sqlCmd))
                    {
                        MyUtility.Msg.WarningBox("No packing data, can't create!!");
                        return;
                    }

                    sqlCmd = string.Format(
                        @"select '' as ID, '' as RefNo, '' as Description, oqd.Article, voc.ColorID as Color, oqd.SizeCode, oqd.Qty as ShipQty, oqc.Qty as QtyPerCTN, os.Seq,
	   sw.NW as NW1, sw.NNW as NNW1, sw2.NW as NW2, sw2.NNW as NNW2,
	   isnull(sw.NW, isnull(sw2.NW, 0))*oqc.Qty as NW,
	   isnull(sw.NW, isnull(sw2.NW, 0))*oqc.Qty as GW,
	   isnull(sw.NNW, isnull(sw2.NNW, 0))*oqc.Qty as NNW 
from Order_QtyShip_Detail oqd WITH (NOLOCK) 
left Join Orders o WITH (NOLOCK) on o.ID = oqd.Id
left Join Order_QtyCTN oqc WITH (NOLOCK) on oqc.id = oqd.Id and oqc.Article = oqd.Article and oqc.SizeCode = oqd.SizeCode
left join View_OrderFAColor voc on voc.id = oqd.Id and voc.Article = oqd.Article
left join Style_WeightData sw WITH (NOLOCK) on sw.StyleUkey = o.StyleUkey and sw.Article = oqd.Article and sw.SizeCode = oqd.SizeCode
left join Style_WeightData sw2 WITH (NOLOCK) on sw2.StyleUkey = o.StyleUkey and sw2.Article = '----' and sw2.SizeCode = oqd.SizeCode
left join Order_SizeCode os WITH (NOLOCK) on os.id = o.POID and os.SizeCode = oqd.SizeCode
left join Order_Article oa WITH (NOLOCK) on oa.id = oqd.Id and oa.Article = oqd.Article
where oqd.ID = '{0}' and oqd.Seq = '{1}'
order by oa.Seq,os.Seq",
                        orderID,
                        seq);
                }
                else
                {
                    sqlCmd = string.Format(
                        @"select '' as ID, '' as RefNo, '' as Description, oqd.Article, voc.ColorID as Color, oqd.SizeCode, oqd.Qty as ShipQty, o.CTNQty as QtyPerCTN, os.Seq,
	   sw.NW as NW1, sw.NNW as NNW1, sw2.NW as NW2, sw2.NNW as NNW2,
	   isnull(sw.NW, isnull(sw2.NW, 0))*o.CTNQty as NW,
	   isnull(sw.NW, isnull(sw2.NW, 0))*o.CTNQty as GW,
	   isnull(sw.NNW, isnull(sw2.NNW, 0))*o.CTNQty as NNW 
from Order_QtyShip_Detail oqd WITH (NOLOCK) 
left Join Orders o WITH (NOLOCK) on o.ID = oqd.Id
left join View_OrderFAColor voc on voc.id = oqd.Id and voc.Article = oqd.Article
left join Style_WeightData sw WITH (NOLOCK) on sw.StyleUkey = o.StyleUkey and sw.Article = oqd.Article and sw.SizeCode = oqd.SizeCode
left join Style_WeightData sw2 WITH (NOLOCK) on sw2.StyleUkey = o.StyleUkey and sw2.Article = '----' and sw2.SizeCode = oqd.SizeCode
left join Order_SizeCode os WITH (NOLOCK) on os.id = o.POID and os.SizeCode = oqd.SizeCode
left join Order_Article oa WITH (NOLOCK) on oa.id = oqd.Id and oa.Article = oqd.Article
where oqd.ID = '{0}' and oqd.Seq = '{1}'
order by oa.Seq,os.Seq",
                        orderID,
                        seq);
                }

                DataTable selectedData;
                DualResult result;
                if (result = DBProxy.Current.Select(null, sqlCmd, out selectedData))
                {
                    foreach (DataRow dr in selectedData.Rows)
                    {
                        dr.AcceptChanges();
                        dr.SetAdded();
                        ((DataTable)this.detailgridbs.DataSource).ImportRow(dr);
                    }
                }
                else
                {
                    MyUtility.Msg.WarningBox("Sql connection fail!!\r\n" + result.ToString());
                    return;
                }
            }
        }

        // Seq按右鍵功能
        private void TxtSeq_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            IList<DataRow> orderQtyShipData;
            string sqlCmd = string.Format("select Seq, BuyerDelivery,ShipmodeID,Qty from Order_QtyShip WITH (NOLOCK) where ID = '{0}'", this.CurrentMaintain["OrderID"].ToString());
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlCmd, "4,20,20,10", string.Empty, "Seq,Buyer Delivery,ShipMode,Qty");
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                this.CurrentMaintain["OrderShipmodeSeq"] = string.Empty;
                this.CurrentMaintain["ShipModeID"] = string.Empty;
                this.numTotalShipQty.Value = 0;
            }
            else
            {
                orderQtyShipData = item.GetSelecteds();
                this.CurrentMaintain["OrderShipmodeSeq"] = item.GetSelectedString();
                this.CurrentMaintain["ShipModeID"] = orderQtyShipData[0]["ShipmodeID"].ToString();
                this.numTotalShipQty.Value = MyUtility.Convert.GetInt(orderQtyShipData[0]["Qty"]);
            }

            // 產生表身Grid的資料
            this.GenDetailData(this.CurrentMaintain["OrderID"].ToString(), this.CurrentMaintain["OrderShipmodeSeq"].ToString());
        }

        // Special Instruction
        private void BtnSpecialInstruction_Click(object sender, EventArgs e)
        {
            Sci.Win.Tools.EditMemo callNextForm = new Sci.Win.Tools.EditMemo(this.CurrentMaintain["SpecialInstruction"].ToString(), "Special Instruction", false, null);
            callNextForm.ShowDialog(this);
        }

        // Carton Dimension
        private void BtnCartonDimension_Click(object sender, EventArgs e)
        {
            Sci.Production.Packing.P02_CartonSummary callNextForm = new Sci.Production.Packing.P02_CartonSummary(this.CurrentMaintain["OrderID"].ToString());
            callNextForm.ShowDialog(this);
        }

        private string GetSwitchToPackingListSQL(string fromBtn)
        {
            string insertCmd = string.Empty;
            if (fromBtn.Equals("btnSwitchToPackingList"))
            {
                #region 組SwitchToPackingList Insert SQL
                if (this.comboPackingMethod.SelectedIndex != -1 && this.comboPackingMethod.SelectedValue.ToString() == "2")
                {
                    #region 單色混碼
                    insertCmd = string.Format(
                        @"--宣告變數: PackingGuide帶入的參數
DECLARE @id VARCHAR(13),
		@mdivisionid VARCHAR(8),
		@factoryid VARCHAR(8),
		@orderid VARCHAR(13),
		@ordershipmodeseq VARCHAR(2),
		@shipmodeid VARCHAR(10),
		@ctnstartno INT,
		@cbm NUMERIC(8,4),
		@remark NVARCHAR(125)
--設定變數值
SET @id = '{0}'
SELECT @mdivisionid = MDivisionID, @factoryid = FactoryID, @orderid = OrderID, @ordershipmodeseq = OrderShipmodeSeq, @shipmodeid = ShipModeID, @ctnstartno = CTNStartNo, @cbm = CBM, @remark = Remark  FROM PackingGuide WHERE Id = @id

--宣告變數: Orders相關的參數
DECLARE @brandid VARCHAR(8),
		@dest VARCHAR(2),
		@custcdid VARCHAR(16)
--設定變數值
SELECT @brandid = BrandID, @dest = Dest, @custcdid = CustCDID FROM Orders WITH (NOLOCK) WHERE ID = @orderid

--建立tmpe table存放展開後結果
DECLARE @tempPackingList TABLE (
   RefNo VARCHAR(21),
   CTNStartNo VARCHAR(6),
   CTNQty INT,
   Seq VARCHAR(6),
   Article VARCHAR(8),
   Color VARCHAR(6),
   SizeCode VARCHAR(8),
   QtyPerCTN SMALLINT,
   ShipQty INT,
   NW NUMERIC(7,3),
   GW NUMERIC(7,3),
   NNW NUMERIC(7,3),
   NWPerPcs NUMERIC(5,3),
   CtnNo INT,
   SizeSeq INT,
   Barcode varchar(30)
)

--建立tmpe table存放餘箱資料
DECLARE @tempRemainder TABLE (
   RefNo VARCHAR(21),
   CTNQty INT,
   Seq INT,
   Article VARCHAR(8),
   Color VARCHAR(6),
   SizeCode VARCHAR(8),
   QtyPerCTN SMALLINT,
   ShipQty INT,
   NW NUMERIC(7,3),
   GW NUMERIC(7,3),
   NNW NUMERIC(7,3),
   NWPerPcs NUMERIC(5,3),
   SizeSeq INT,
   Barcode varchar(30)
)

--將PackingGuide_Detail中的Article撈出來
DECLARE cursor_groupbyarticle CURSOR FOR
	SELECT Distinct a.Article, b.Seq FROM PackingGuide_Detail a WITH (NOLOCK) , Order_Article b WITH (NOLOCK) WHERE a.Id = @id AND b.id = @orderid AND a.Article = b.Article ORDER BY b.Seq

--宣告變數: PackingGuide_Detail相關的參數
DECLARE @refno VARCHAR(21),
		@article VARCHAR(8),
		@color VARCHAR(6),
		@sizecode VARCHAR(8),
		@qtyperctn SMALLINT,
		@shipqty INT,
		@nw NUMERIC(7,3),
		@gw NUMERIC(7,3),
		@nnw NUMERIC(7,3),
        @BarCode varchar(30),
		@RefNoForBalance VARCHAR(21)

--宣告變數: 記錄程式中的資料
DECLARE @currentqty INT, --目前數量
		@ctnno INT, --箱號
        @recordctnno INT, --紀錄起始箱號
		@seqcount INT, --排序用編號
		@seq VARCHAR(6), --排序用編號
		@remaindercount INT, --餘箱資料個數
		@ttlshipqty INT, --總件數，寫入PackingList時使用
		@ttlnw NUMERIC(9,3), --總淨重，寫入PackingList時使用
		@ttlgw NUMERIC(9,3), --總毛重，寫入PackingList時使用
		@ttlnnw NUMERIC(9,3), --總淨淨重，寫入PackingList時使用
		@firstsize VARCHAR(8), --第一筆的SizeCode
		@minctn INT, --最少箱數
		@_i INT --計算迴圈用

Declare @GwBalance NUMERIC(7,3) -- 尾箱重新撈取GW

SET @recordctnno = @ctnstartno
SET @remaindercount = 0
SET @minctn = 0

--開始run cursor
OPEN cursor_groupbyarticle
--將第一筆資料填入變數
FETCH NEXT FROM cursor_groupbyarticle INTO @article, @seq
WHILE @@FETCH_STATUS = 0
BEGIN
	SET @firstsize = ''
    SET @recordctnno = @recordctnno + @minctn
	--先算出最少箱數
	SELECT @minctn = MIN(ShipQty/QtyPerCTN) FROM PackingGuide_Detail WITH (NOLOCK) WHERE ID = @id AND Article = @article

	--撈出PackingGuide_Detail資料
	DECLARE cursor_packingguide CURSOR FOR
		SELECT a.RefNo,a.Color,a.SizeCode,a.QtyPerCTN,a.ShipQty,a.NW,a.GW,a.NNW,c.Seq,isnull(cb.BarCode,''), a.RefNoForBalance
		FROM PackingGuide_Detail a WITH (NOLOCK) 
		LEFT JOIN Orders b WITH (NOLOCK) ON b.ID = @orderid
		LEFT JOIN Order_SizeCode c WITH (NOLOCK) ON c.Id = b.POID AND a.SizeCode = c.SizeCode
        LEFT JOIN CustBarCode cb WITH (NOLOCK) ON b.CustPoNo = cb.CustPoNo and b.StyleID = cb.StyleID and a.Article = cb.Article and a.SizeCode = cb.SizeCode
		WHERE a.Id = @id AND a.Article = @article
		ORDER BY c.Seq

	OPEN cursor_packingguide
	FETCH NEXT FROM cursor_packingguide INTO @refno, @color, @sizecode, @qtyperctn, @shipqty, @nw, @gw, @nnw, @seq, @BarCode, @RefNoForBalance
	SET @firstsize = @sizecode
	WHILE @@FETCH_STATUS = 0
	BEGIN
		IF @firstsize = @sizecode
			BEGIN
				SET @ctnno = @recordctnno
				SET @_i = 0
				WHILE (@_i < @minctn)
				BEGIN
					INSERT INTO @tempPackingList (RefNo,CTNQty,Article,Color,SizeCode,QtyPerCTN,ShipQty,NW,GW,NNW,NWPerPcs,CtnNo,SizeSeq,BarCode)
						VALUES (@refno, 1, @article, @color, @sizecode, @qtyperctn, @qtyperctn, @nw, @gw-@nw, @nnw, @nw/@qtyperctn, @ctnno, @seq,@BarCode)
					SET @_i = @_i + 1
					SET @ctnno = @ctnno + 1
				END
			END
		ELSE
			BEGIN
				SET @ctnno = @recordctnno
				SET @_i = 0
				WHILE (@_i < @minctn)
				BEGIN
					INSERT INTO @tempPackingList (RefNo,CTNQty,Article,Color,SizeCode,QtyPerCTN,ShipQty,NW,GW,NNW,NWPerPcs,CtnNo,SizeSeq,BarCode)
						VALUES (@refno, 0, @article, @color, @sizecode, @qtyperctn, @qtyperctn, @nw, 0, @nnw, @nw/@qtyperctn, @ctnno, @seq,@BarCode)
					SET @_i = @_i + 1
					SET @ctnno = @ctnno + 1
				END
			END

		FETCH NEXT FROM cursor_packingguide INTO @refno, @color, @sizecode, @qtyperctn, @shipqty, @nw, @gw, @nnw, @seq, @BarCode, @RefNoForBalance
	END
	CLOSE cursor_packingguide
	
	--整理餘箱資料
	SET @firstsize = ''
	OPEN cursor_packingguide
	FETCH NEXT FROM cursor_packingguide INTO @refno, @color, @sizecode, @qtyperctn, @shipqty, @nw, @gw, @nnw, @seq, @BarCode, @RefNoForBalance
	WHILE @@FETCH_STATUS = 0
	BEGIN
		SET @currentqty = @shipqty - (@qtyperctn * @minctn)
		IF @currentqty > 0
			BEGIN
				SET @remaindercount = @remaindercount + 1

				IF @firstsize = ''
					BEGIN
						SET @firstsize = @sizecode
					END

				IF ISNULL(@RefNoForBalance,'') <> ''
					BEGIN
						SELECT @GwBalance = CtnWeight FROM LocalItem WHERE RefNo = @RefNoForBalance
					END
				ELSE
					BEGIN
						SET @RefNoForBalance = null
						SET @GwBalance = null
					END


				IF @firstsize = @sizecode
					BEGIN
						INSERT INTO @tempRemainder (RefNo,CTNQty,Article,Color,SizeCode,QtyPerCTN,ShipQty,NW,GW,NNW,NWPerPcs,Seq,SizeSeq,BarCode)
							VALUES (ISNULL(@RefNoForBalance,@refno), 1, @article, @color, @sizecode, @currentqty, @currentqty, (@nw/@qtyperctn)*@currentqty, ISNULL(@GwBalance,@gw-@nw), (@nnw/@qtyperctn)*@currentqty, @nw/@qtyperctn, @remaindercount, @seq,@BarCode)
					END
				ELSE
					BEGIN
						INSERT INTO @tempRemainder (RefNo,CTNQty,Article,Color,SizeCode,QtyPerCTN,ShipQty,NW,GW,NNW,NWPerPcs,Seq,SizeSeq,BarCode)
							VALUES (ISNULL(@RefNoForBalance,@refno), 0, @article, @color, @sizecode, @currentqty, @currentqty, (@nw/@qtyperctn)*@currentqty, 0, (@nnw/@qtyperctn)*@currentqty, @nw/@qtyperctn, @remaindercount, @seq,@BarCode)
					END
			END
		FETCH NEXT FROM cursor_packingguide INTO @refno, @color, @sizecode, @qtyperctn, @shipqty, @nw, @gw, @nnw, @seq, @BarCode, @RefNoForBalance
	END
	CLOSE cursor_packingguide
	DEALLOCATE cursor_packingguide

	FETCH NEXT FROM cursor_groupbyarticle INTO @article, @seq
END
--關閉cursor與參數的關聯
CLOSE cursor_groupbyarticle
--將cursor物件從記憶體移除
DEALLOCATE cursor_groupbyarticle

--找出目前最大箱號
SELECT @ctnno = isnull(MAX(CtnNo),0) FROM @tempPackingList
--宣告變數
DECLARE @ctnqty INT, --Carton數
		@nwperpcs NUMERIC(5,3) --每件淨重

--將Remainder資料整理進@tempPackingList
DECLARE cursor_tempremainder CURSOR FOR
	SELECT RefNo,CTNQty,Article,Color,SizeCode,QtyPerCTN,ShipQty,NW,GW,NNW,NWPerPcs,SizeSeq,BarCode FROM @tempRemainder ORDER BY Seq

OPEN cursor_tempremainder
FETCH NEXT FROM cursor_tempremainder INTO @refno, @ctnqty, @article, @color, @sizecode, @qtyperctn, @shipqty, @nw, @gw, @nnw, @nwperpcs, @seq,@BarCode
WHILE @@FETCH_STATUS = 0
BEGIN
	IF @ctnqty = 1
		BEGIN
			SET @ctnno = @ctnno + 1
		END

	INSERT INTO @tempPackingList (RefNo,CTNQty,Article,Color,SizeCode,QtyPerCTN,ShipQty,NW,GW,NNW,NWPerPcs,CtnNo,SizeSeq,BarCode)
		VALUES (@refno, @ctnqty, @article, @color, @sizecode, @qtyperctn, @shipqty, @nw, @gw, @nnw, @nwperpcs, @ctnno, @seq,@BarCode)
	
	FETCH NEXT FROM cursor_tempremainder INTO @refno, @ctnqty, @article, @color, @sizecode, @qtyperctn, @shipqty, @nw, @gw, @nnw, @nwperpcs, @seq,@BarCode
END
CLOSE cursor_tempremainder
DEALLOCATE cursor_tempremainder

--更新箱號
UPDATE @tempPackingList SET CTNStartNo = CONVERT(VARCHAR,CtnNo)

--更新重量
DECLARE cursor_@temppacklistgroup CURSOR FOR
	SELECT DISTINCT Article,CtnNo FROM @tempPackingList
OPEN cursor_@temppacklistgroup
FETCH NEXT FROM cursor_@temppacklistgroup INTO @article, @ctnno
WHILE @@FETCH_STATUS = 0
BEGIN
	SELECT @nw = SUM(NW), @gw = SUM(GW), @nnw = SUM(NNW), @seqcount = MAX(CtnNo) FROM @tempPackingList WHERE Article = @article and CtnNo = @ctnno
	UPDATE @tempPackingList 
	SET NW = @nw,
		GW = @nw+@gw,
		NNW = @nnw
	WHERE Article = @article and CtnNo = @ctnno AND CTNQty = 1

	UPDATE @tempPackingList 
	SET NW = 0, GW = 0, NNW = 0 WHERE Article = @article and CtnNo = @ctnno AND CTNQty = 0

	FETCH NEXT FROM cursor_@temppacklistgroup INTO @article, @ctnno
END
CLOSE cursor_@temppacklistgroup
DEALLOCATE cursor_@temppacklistgroup

--全部總重量
SELECT @ttlnw = SUM(NW), @ttlgw = SUM(GW), @ttlnnw = SUM(NNW), @ttlshipqty = SUM(ShipQty), @seqcount = SUM(CtnQty) FROM @tempPackingList

--刪除PackingList_Detail資料
DELETE PackingList_Detail WHERE ID = @id

--資料存入PackingList & PackingList_Detail
--宣告變數
DECLARE @havepl INT, --檢查PackingList是否存在
		@addname VARCHAR(10), --系統登入人員
		@adddate DATETIME --新增時間
SET @addname = '{1}'
SET @adddate = GETDATE()

SET XACT_ABORT ON;
BEGIN TRANSACTION
--PackingList
SELECT @havepl = count(ID) FROM PackingList WITH (NOLOCK) WHERE ID = @id
IF @havepl = 0
	BEGIN --新增PackingList
		INSERT INTO PackingList (ID,Type,MDivisionID,FactoryID,ShipModeID,BrandID,Dest,CustCDID,CTNQty,ShipQty,NW,GW,NNW,CBM,Remark,Status,AddName,AddDate,QueryDate)
			VALUES (@id, 'B', @mdivisionid, @factoryid, @shipmodeid, @brandid, @dest, @custcdid, @seqcount, @ttlshipqty, @ttlnw, @ttlgw, @ttlnnw, @cbm, @remark, 'New', @addname, @adddate, @adddate)
	END
ELSE
	BEGIN --更新PackingList
		UPDATE PackingList 
		SET MDivisionID = @mdivisionid,
			FactoryID = @factoryid,
			ShipModeID = @shipmodeid,
			BrandID = @brandid,
			Dest = @dest,
			CustCDID = @custcdid,
			CTNQty = @seqcount,
			ShipQty = @ttlshipqty,
			NW = @ttlnw,
			GW = @ttlgw,
			NNW = @ttlnnw,
			CBM = @cbm,
			Remark = @remark,
			Status = 'New',
			AddName = @addname,
			AddDate = @adddate,
			EditName = '',
			EditDate = null,
            QueryDate = @adddate
		WHERE ID = @id
	END

--PackingList_Detail
DECLARE @cartonsatrtno VARCHAR(6)

SET @seqcount = 0

DECLARE cursor_temppackinglist CURSOR FOR
	SELECT RefNo,CTNStartNo,CTNQty,Article,Color,SizeCode,QtyPerCTN,ShipQty,NW,GW,NNW,NWPerPcs,BarCode FROM @tempPackingList ORDER BY CtnNo,SizeSeq
OPEN cursor_temppackinglist
FETCH NEXT FROM cursor_temppackinglist INTO @refno, @cartonsatrtno, @ctnqty, @article, @color, @sizecode, @qtyperctn, @shipqty, @nw, @gw, @nnw, @nwperpcs,@BarCode
WHILE @@FETCH_STATUS = 0
BEGIN
	SET @seqcount = @seqcount + 1
	SELECT @seq = REPLICATE('0', 6 - LEN(CONVERT(VARCHAR,@seqcount))) + CONVERT(VARCHAR,@seqcount)
	INSERT INTO PackingList_Detail(ID,OrderID,OrderShipmodeSeq,RefNo,CTNStartNo,CTNEndNo,CTNQty,Article,Color,SizeCode,QtyPerCTN,ShipQty,NW,GW,NNW,NWPerPcs,Seq,BarCode)
		VALUES (@id, @orderid, @ordershipmodeseq, @refno, @cartonsatrtno, @cartonsatrtno, @ctnqty, @article, @color, @sizecode, @qtyperctn, @shipqty, @nw, @gw, @nnw, @nwperpcs, @seq,@BarCode)
	--將下一筆資料填入變數
	FETCH NEXT FROM cursor_temppackinglist INTO @refno, @cartonsatrtno, @ctnqty, @article, @color, @sizecode, @qtyperctn, @shipqty, @nw, @gw, @nnw, @nwperpcs,@BarCode
END
CLOSE cursor_temppackinglist
DEALLOCATE cursor_temppackinglist

IF @@ERROR <> 0
	ROLLBACK TRANSACTION
ELSE
	COMMIT TRANSACTION",
        this.CurrentMaintain["ID"].ToString(),
        Sci.Env.User.UserID);
                    #endregion
                }
                else
                {
                    #region 單色單碼
                    insertCmd = string.Format(
                        @"--宣告變數: PackingGuide帶入的參數
DECLARE @id VARCHAR(13),
		@mdivisionid VARCHAR(8),
		@factoryid VARCHAR(8),
		@orderid VARCHAR(13),
		@ordershipmodeseq VARCHAR(2),
		@shipmodeid VARCHAR(10),
		@ctnstartno INT,
		@cbm NUMERIC(8,4),
		@remark NVARCHAR(125)
--設定變數值
SET @id = '{0}'
SELECT @mdivisionid = MDivisionID, @factoryid = FactoryID, @orderid = OrderID, @ordershipmodeseq = OrderShipmodeSeq, @shipmodeid = ShipModeID, @ctnstartno = CTNStartNo, @cbm = CBM, @remark = Remark  FROM PackingGuide WITH (NOLOCK) WHERE Id = @id

--宣告變數: Orders相關的參數
DECLARE @brandid VARCHAR(8),
		@dest VARCHAR(2),
		@custcdid VARCHAR(16)
--設定變數值
SELECT @brandid = BrandID, @dest = Dest, @custcdid = CustCDID FROM Orders WITH (NOLOCK) WHERE ID = @orderid

--建立tmpe table存放展開後結果
DECLARE @tempPackingList TABLE (
   RefNo VARCHAR(21),
   CTNStartNo VARCHAR(6),
   CTNQty INT,
   Seq VARCHAR(6),
   Article VARCHAR(8),
   Color VARCHAR(6),
   SizeCode VARCHAR(8),
   QtyPerCTN SMALLINT,
   ShipQty INT,
   NW NUMERIC(7,3),
   GW NUMERIC(7,3),
   NNW NUMERIC(7,3),
   NWPerPcs NUMERIC(5,3),
   BarCode  varchar(30)
)

--建立tmpe table存放餘箱資料
DECLARE @tempRemainder TABLE (
   RefNo VARCHAR(21),
   CTNQty INT,
   Seq INT,
   Article VARCHAR(8),
   Color VARCHAR(6),
   SizeCode VARCHAR(8),
   QtyPerCTN SMALLINT,
   ShipQty INT,
   NW NUMERIC(7,3),
   GW NUMERIC(7,3),
   NNW NUMERIC(7,3),
   NWPerPcs NUMERIC(5,3),
   BarCode varchar(30)
)

--將PackingGuide_Detail資料存放至Cursor
DECLARE cursor_packguide CURSOR FOR
	SELECT a.RefNo,a.Article,a.Color,a.SizeCode,a.QtyPerCTN,a.ShipQty,a.NW,a.GW,a.NNW ,isnull(cb.BarCode,''), a.RefNoForBalance
	FROM PackingGuide_Detail a WITH (NOLOCK) 
	LEFT JOIN Orders b WITH (NOLOCK) ON b.ID = @orderid
	LEFT JOIN Order_Article c WITH (NOLOCK) ON c.Id = @orderid AND a.Article = c.Article
	LEFT JOIN Order_SizeCode d WITH (NOLOCK) ON d.Id = b.POID AND a.SizeCode = d.SizeCode
    LEFT JOIN CustBarCode cb WITH (NOLOCK) ON b.CustPoNo = cb.CustPoNo and b.StyleID = cb.StyleID and a.Article = cb.Article and a.SizeCode = cb.SizeCode
	WHERE a.ID = @id ORDER BY c.Seq,d.Seq

--宣告變數: PackingGuide_Detail相關的參數
DECLARE @refno VARCHAR(21),
		@article VARCHAR(8),
		@color VARCHAR(6),
		@sizecode VARCHAR(8),
		@qtyperctn SMALLINT,
		@shipqty INT,
		@nw NUMERIC(7,3),
		@gw NUMERIC(7,3),
		@nnw NUMERIC(7,3),
        @BarCode varchar(30),
		@RefNoForBalance VARCHAR(21)

--宣告變數: 記錄程式中的資料
DECLARE @currentqty INT, --目前數量
		@ctnno INT, --箱號
		@realctnno VARCHAR(6), --寫入Table中的箱號
		@seqcount INT, --排序用編號
		@seq VARCHAR(6), --排序用編號
		@remaindercount INT, --餘箱資料個數
		@ttlshipqty INT, --總件數，寫入PackingList時使用
		@ttlnw NUMERIC(9,3), --總淨重，寫入PackingList時使用
		@ttlgw NUMERIC(9,3), --總毛重，寫入PackingList時使用
		@ttlnnw NUMERIC(9,3) --總淨淨重，寫入PackingList時使用

Declare @GwBalance NUMERIC(7,3) -- 尾箱重新撈取GW

SET @ctnno = @ctnstartno
SET @seqcount = 0
SET @remaindercount = 0
SET @ttlshipqty = 0
SET @ttlnw = 0
SET @ttlgw = 0
SET @ttlnnw = 0

--開始run cursor
OPEN cursor_packguide
--將第一筆資料填入變數
FETCH NEXT FROM cursor_packguide INTO @refno, @article, @color, @sizecode, @qtyperctn, @shipqty, @nw, @gw, @nnw,@BarCode, @RefNoForBalance
WHILE @@FETCH_STATUS = 0
BEGIN
	IF @qtyperctn > 0
		BEGIN
			SET @currentqty = @shipqty
			WHILE @currentqty > 0
			BEGIN
				IF @currentqty >= @qtyperctn
					BEGIN
						SET @seqcount = @seqcount + 1
						SELECT @seq = REPLICATE('0', 6 - LEN(CONVERT(VARCHAR,@seqcount))) + CONVERT(VARCHAR,@seqcount)
						SELECT @realctnno = CONVERT(VARCHAR,@ctnno)
						INSERT INTO @tempPackingList (RefNo,CTNStartNo,CTNQty,Article,Color,SizeCode,QtyPerCTN,ShipQty,NW,GW,NNW,NWPerPcs,Seq,BarCode)
							VALUES (@refno, @realctnno, 1, @article, @color, @sizecode, @qtyperctn, @qtyperctn, @nw, @gw, @nnw, @nw/@qtyperctn, @seq,@BarCode)
						SET @ctnno = @ctnno + 1
						SET @ttlnw = @ttlnw + @nw
						SET @ttlgw = @ttlgw + @gw
						SET @ttlnnw = @ttlnnw + @nnw
						SET @ttlshipqty = @ttlshipqty + @qtyperctn
					END
				ELSE
					BEGIN
						SET @remaindercount = @remaindercount + 1
						IF ISNULL(@RefNoForBalance,'') = ''
						BEGIN
							INSERT INTO @tempRemainder (RefNo,CTNQty,Article,Color,SizeCode,QtyPerCTN,ShipQty,NW,GW,NNW,NWPerPcs,Seq,BarCode)
								VALUES (@refno, 1, @article, @color, @sizecode, @currentqty, @currentqty, (@nw/@qtyperctn)*@currentqty, ((@nw/@qtyperctn)*@currentqty)+(@gw-@nw), (@nnw/@qtyperctn)*@currentqty, (@nw/@qtyperctn), @remaindercount,@BarCode)
						END
						ELSE
						BEGIN
							SELECT @GwBalance = CtnWeight FROM LocalItem WHERE RefNo = @RefNoForBalance
							-- 有設定尾箱的料號
							INSERT INTO @tempRemainder (RefNo,CTNQty,Article,Color,SizeCode,QtyPerCTN,ShipQty,NW,GW,NNW,NWPerPcs,Seq,BarCode)
								VALUES (@RefNoForBalance, 1, @article, @color, @sizecode, @currentqty, @currentqty, (@nw/@qtyperctn)*@currentqty, ((@nw/@qtyperctn)*@currentqty)+@GwBalance, (@nnw/@qtyperctn)*@currentqty, (@nw/@qtyperctn), @remaindercount,@BarCode)
						END
					END

				SET @currentqty = @currentqty - @qtyperctn
			END
		END

	--將下一筆資料填入變數
	FETCH NEXT FROM cursor_packguide INTO @refno, @article, @color, @sizecode, @qtyperctn, @shipqty, @nw, @gw, @nnw,@BarCode, @RefNoForBalance
END

--關閉cursor與參數的關聯
CLOSE cursor_packguide
--將cursor物件從記憶體移除
DEALLOCATE cursor_packguide

--將餘箱資料寫入@tempPackingList
--將@tempRemainder資料存放至Cursor
DECLARE cursor_temRemainder CURSOR FOR
	SELECT RefNo,CTNQty,Article,Color,SizeCode,QtyPerCTN,ShipQty,NW,GW,NNW,NWPerPcs,BarCode FROM @tempRemainder ORDER BY Seq
--宣告變數: 記錄程式中的資料
DECLARE @ctnqty INT, --Carton數
		@nwperpcs NUMERIC(5,3) --每件淨重

OPEN cursor_temRemainder
FETCH NEXT FROM cursor_temRemainder INTO @refno, @ctnqty, @article, @color, @sizecode, @qtyperctn, @shipqty, @nw, @gw, @nnw, @nwperpcs,@BarCode
WHILE @@FETCH_STATUS = 0
BEGIN
	SET @seqcount = @seqcount + 1
	SELECT @seq = REPLICATE('0', 6 - LEN(CONVERT(VARCHAR,@seqcount))) + CONVERT(VARCHAR,@seqcount)
	SELECT @realctnno = CONVERT(VARCHAR,@ctnno)
	INSERT INTO @tempPackingList (RefNo,CTNStartNo,CTNQty,Article,Color,SizeCode,QtyPerCTN,ShipQty,NW,GW,NNW,NWPerPcs,Seq,BarCode)
		VALUES (@refno, @realctnno, @ctnqty, @article, @color, @sizecode, @qtyperctn, @qtyperctn, @nw, @gw, @nnw, @nwperpcs, @seq,@BarCode)
	SET @ctnno = @ctnno + 1
	SET @ttlnw = @ttlnw + @nw
	SET @ttlgw = @ttlgw + @gw
	SET @ttlnnw = @ttlnnw + @nnw
	SET @ttlshipqty = @ttlshipqty + @qtyperctn

	--將下一筆資料填入變數
	FETCH NEXT FROM cursor_temRemainder INTO @refno, @ctnqty, @article, @color, @sizecode, @qtyperctn, @shipqty, @nw, @gw, @nnw, @nwperpcs,@BarCode
END

CLOSE cursor_temRemainder
DEALLOCATE cursor_temRemainder

--刪除PackingList_Detail資料
DELETE PackingList_Detail WHERE ID = @id

--資料存入PackingList & PackingList_Detail
--宣告變數
DECLARE @havepl INT, --檢查PackingList是否存在
		@addname VARCHAR(10), --系統登入人員
		@adddate DATETIME --新增時間
SET @addname = '{1}'
SET @adddate = GETDATE()

SET XACT_ABORT ON;
BEGIN TRANSACTION
--PackingList
SELECT @havepl = count(ID) FROM PackingList WITH (NOLOCK) WHERE ID = @id
IF @havepl = 0
	BEGIN --新增PackingList
		INSERT INTO PackingList (ID,Type,MDivisionID,FactoryID,ShipModeID,BrandID,Dest,CustCDID,CTNQty,ShipQty,NW,GW,NNW,CBM,Remark,Status,AddName,AddDate,QueryDate)
			VALUES (@id, 'B', @mdivisionid, @factoryid, @shipmodeid, @brandid, @dest, @custcdid, @seqcount, @ttlshipqty, @ttlnw, @ttlgw, @ttlnnw, @cbm, @remark, 'New', @addname, @adddate, @adddate)
	END
ELSE
	BEGIN --更新PackingList
		UPDATE PackingList 
		SET MDivisionID = @mdivisionid,
			FactoryID = @factoryid,
			ShipModeID = @shipmodeid,
			BrandID = @brandid,
			Dest = @dest,
			CustCDID = @custcdid,
			CTNQty = @seqcount,
			ShipQty = @ttlshipqty,
			NW = @ttlnw,
			GW = @ttlgw,
			NNW = @ttlnnw,
			CBM = @cbm,
			Remark = @remark,
			Status = 'New',
			AddName = @addname,
			AddDate = @adddate,
			EditName = '',
			EditDate = null,
            QueryDate = @adddate
		WHERE ID = @id
	END

--PackingList_Detail
DECLARE @cartonsatrtno VARCHAR(6)

DECLARE cursor_temPackingList CURSOR FOR
	SELECT RefNo,CTNStartNo,CTNQty,Article,Color,SizeCode,QtyPerCTN,ShipQty,NW,GW,NNW,NWPerPcs,Seq,BarCode FROM @tempPackingList ORDER BY Seq
OPEN cursor_temPackingList
FETCH NEXT FROM cursor_temPackingList INTO @refno, @cartonsatrtno, @ctnqty, @article, @color, @sizecode, @qtyperctn, @shipqty, @nw, @gw, @nnw, @nwperpcs, @seq,@BarCode
WHILE @@FETCH_STATUS = 0
BEGIN
	INSERT INTO PackingList_Detail(ID,OrderID,OrderShipmodeSeq,RefNo,CTNStartNo,CTNEndNo,CTNQty,Article,Color,SizeCode,QtyPerCTN,ShipQty,NW,GW,NNW,NWPerPcs,Seq,BarCode)
		VALUES (@id, @orderid, @ordershipmodeseq, @refno, @cartonsatrtno, @cartonsatrtno, @ctnqty, @article, @color, @sizecode, @qtyperctn, @shipqty, @nw, @gw, @nnw, @nwperpcs, @seq,@BarCode)
	--將下一筆資料填入變數
	FETCH NEXT FROM cursor_temPackingList INTO @refno, @cartonsatrtno, @ctnqty, @article, @color, @sizecode, @qtyperctn, @shipqty, @nw, @gw, @nnw, @nwperpcs, @seq,@BarCode
END
CLOSE cursor_temPackingList
DEALLOCATE cursor_temPackingList

IF @@ERROR <> 0
	ROLLBACK TRANSACTION
ELSE
	COMMIT TRANSACTION",
                        this.CurrentMaintain["ID"].ToString(),
                        Env.User.UserID);
                    #endregion
                }
                #endregion

            }
            else
            {
                #region 組SwitchToPackingList by Article Insert SQL
                if (this.comboPackingMethod.SelectedIndex != -1 && this.comboPackingMethod.SelectedValue.ToString() == "2")
                {
                    #region 單色混碼
                    insertCmd = string.Format(
                        @"--宣告變數: PackingGuide帶入的參數
DECLARE @id VARCHAR(13),
		@mdivisionid VARCHAR(8),
		@factoryid VARCHAR(8),
		@orderid VARCHAR(13),
		@ordershipmodeseq VARCHAR(2),
		@shipmodeid VARCHAR(10),
		@ctnstartno INT,
		@cbm NUMERIC(8,4),
		@remark NVARCHAR(125)
--設定變數值
SET @id = '{0}'
SELECT @mdivisionid = MDivisionID, @factoryid = FactoryID, @orderid = OrderID, @ordershipmodeseq = OrderShipmodeSeq, @shipmodeid = ShipModeID, @ctnstartno = CTNStartNo, @cbm = CBM, @remark = Remark  FROM PackingGuide WHERE Id = @id

--宣告變數: Orders相關的參數
DECLARE @brandid VARCHAR(8),
		@dest VARCHAR(2),
		@custcdid VARCHAR(16)
--設定變數值
SELECT @brandid = BrandID, @dest = Dest, @custcdid = CustCDID FROM Orders WITH (NOLOCK) WHERE ID = @orderid

--建立tmpe table存放展開後結果
DECLARE @tempPackingList TABLE (
   RefNo VARCHAR(21),
   CTNStartNo VARCHAR(6),
   CTNQty INT,
   Seq VARCHAR(6),
   Article VARCHAR(8),
   Color VARCHAR(6),
   SizeCode VARCHAR(8),
   QtyPerCTN SMALLINT,
   ShipQty INT,
   NW NUMERIC(7,3),
   GW NUMERIC(7,3),
   NNW NUMERIC(7,3),
   NWPerPcs NUMERIC(5,3),
   CtnNo INT,
   SizeSeq INT,
   LastCntNo VARCHAR(2),
   ArticleSeq INT,
   Barcode varchar(30)
)

--建立tmpe table存放餘箱資料
DECLARE @tempRemainder TABLE (
   RefNo VARCHAR(21),
   CTNQty INT,
   Seq INT,
   Article VARCHAR(8),
   Color VARCHAR(6),
   SizeCode VARCHAR(8),
   QtyPerCTN SMALLINT,
   ShipQty INT,
   NW NUMERIC(7,3),
   GW NUMERIC(7,3),
   NNW NUMERIC(7,3),
   NWPerPcs NUMERIC(5,3),
   SizeSeq INT,
   Barcode varchar(30)
)

--將PackingGuide_Detail中的Article撈出來
DECLARE cursor_groupbyarticle CURSOR FOR
	SELECT Distinct a.Article, b.Seq FROM PackingGuide_Detail a WITH (NOLOCK) , Order_Article b WITH (NOLOCK) WHERE a.Id = @id AND b.id = @orderid AND a.Article = b.Article ORDER BY b.Seq

--宣告變數: PackingGuide_Detail相關的參數
DECLARE @refno VARCHAR(21),
		@article VARCHAR(8),
		@color VARCHAR(6),
		@sizecode VARCHAR(8),
		@qtyperctn SMALLINT,
		@shipqty INT,
		@nw NUMERIC(7,3),
		@gw NUMERIC(7,3),
		@nnw NUMERIC(5,3),
        @BarCode varchar(30),
		@RefNoForBalance VARCHAR(21)

--宣告變數: 記錄程式中的資料
DECLARE @currentqty INT, --目前數量
		@ctnno INT, --箱號
        @recordctnno INT, --紀錄起始箱號
		@seqcount INT, --排序用編號
		@seq VARCHAR(6), --排序用編號
		@articleSeq VARCHAR(6), --排序用編號
		@remaindercount INT, --餘箱資料個數
		@ttlshipqty INT, --總件數，寫入PackingList時使用
		@ttlnw NUMERIC(9,3), --總淨重，寫入PackingList時使用
		@ttlgw NUMERIC(9,3), --總毛重，寫入PackingList時使用
		@ttlnnw NUMERIC(9,3), --總淨淨重，寫入PackingList時使用
		@firstsize VARCHAR(8), --第一筆的SizeCode
		@minctn INT, --最少箱數
		@_i INT --計算迴圈用

Declare @GwBalance NUMERIC(7,3) -- 尾箱重新撈取GW

DECLARE @articlecnt INT
DECLARE @lastctn varchar(2)
SET @recordctnno = @ctnstartno
SET @remaindercount = 0
SET @minctn = 0
SET @articlecnt = 0

--開始run cursor
OPEN cursor_groupbyarticle
--將第一筆資料填入變數
FETCH NEXT FROM cursor_groupbyarticle INTO @article, @articleSeq
WHILE @@FETCH_STATUS = 0
BEGIN
	SET @firstsize = ''
	SET @articlecnt = @articlecnt + 1
	SET @lastctn = CHAR(@articlecnt + 64) --ASCII 65開始是A
    --SET @recordctnno = @recordctnno + @minctn
	--先算出最少箱數
	SELECT @minctn = MIN(ShipQty/QtyPerCTN) FROM PackingGuide_Detail WITH (NOLOCK) WHERE ID = @id AND Article = @article

	--撈出PackingGuide_Detail資料
	DECLARE cursor_packingguide CURSOR FOR
		SELECT a.RefNo,a.Color,a.SizeCode,a.QtyPerCTN,a.ShipQty,a.NW,a.GW,a.NNW,c.Seq,isnull(cb.BarCode,''), a.RefNoForBalance
		FROM PackingGuide_Detail a WITH (NOLOCK) 
		LEFT JOIN Orders b WITH (NOLOCK) ON b.ID = @orderid
		LEFT JOIN Order_SizeCode c WITH (NOLOCK) ON c.Id = b.POID AND a.SizeCode = c.SizeCode
        LEFT JOIN CustBarCode cb WITH (NOLOCK) ON b.CustPoNo = cb.CustPoNo and b.StyleID = cb.StyleID and a.Article = cb.Article and a.SizeCode = cb.SizeCode
		WHERE a.Id = @id AND a.Article = @article
		ORDER BY c.Seq

	OPEN cursor_packingguide
	FETCH NEXT FROM cursor_packingguide INTO @refno, @color, @sizecode, @qtyperctn, @shipqty, @nw, @gw, @nnw, @seq, @Barcode, @RefNoForBalance
	SET @firstsize = @sizecode
	WHILE @@FETCH_STATUS = 0
	BEGIN
		IF @firstsize = @sizecode
			BEGIN
				SET @ctnno = @recordctnno
				SET @_i = 0
				WHILE (@_i < @minctn)
				BEGIN
					INSERT INTO @tempPackingList (RefNo,CTNQty,Article,Color,SizeCode,QtyPerCTN,ShipQty,NW,GW,NNW,NWPerPcs,CtnNo,SizeSeq,LastCntNo,ArticleSeq,Barcode)
						VALUES (@refno, 1, @article, @color, @sizecode, @qtyperctn, @qtyperctn, @nw, @gw-@nw, @nnw, @nw/@qtyperctn, @ctnno, @seq,@lastctn,@articleSeq,@Barcode)
					SET @_i = @_i + 1
					SET @ctnno = @ctnno + 1
				END
			END
		ELSE
			BEGIN
				SET @ctnno = @recordctnno
				SET @_i = 0
				WHILE (@_i < @minctn)
				BEGIN
					INSERT INTO @tempPackingList (RefNo,CTNQty,Article,Color,SizeCode,QtyPerCTN,ShipQty,NW,GW,NNW,NWPerPcs,CtnNo,SizeSeq,LastCntNo,ArticleSeq,Barcode)
						VALUES (@refno, 0, @article, @color, @sizecode, @qtyperctn, @qtyperctn, @nw, 0, @nnw, @nw/@qtyperctn, @ctnno, @seq,@lastctn,@articleSeq,@Barcode)
					SET @_i = @_i + 1
					SET @ctnno = @ctnno + 1
				END
			END

		FETCH NEXT FROM cursor_packingguide INTO @refno, @color, @sizecode, @qtyperctn, @shipqty, @nw, @gw, @nnw, @seq, @Barcode, @RefNoForBalance
	END
	CLOSE cursor_packingguide
	
	--整理餘箱資料
	SELECT @ctnno = isnull(MAX(CtnNo),0) FROM @tempPackingList where Article = @article
	SET @firstsize = ''
	OPEN cursor_packingguide
	FETCH NEXT FROM cursor_packingguide INTO @refno, @color, @sizecode, @qtyperctn, @shipqty, @nw, @gw, @nnw, @seq, @Barcode, @RefNoForBalance
	WHILE @@FETCH_STATUS = 0
	BEGIN
		SET @currentqty = @shipqty - (@qtyperctn * @minctn)
		IF @currentqty > 0
			BEGIN
				SET @remaindercount = @remaindercount + 1

				IF @firstsize = ''
					BEGIN
						SET @firstsize = @sizecode
					END

				IF ISNULL(@RefNoForBalance,'') <> ''
					BEGIN
						SELECT @GwBalance = CtnWeight FROM LocalItem WHERE RefNo = @RefNoForBalance
					END
				ELSE
					BEGIN
						SET @RefNoForBalance = null
						SET @GwBalance = null
					END

				IF @firstsize = @sizecode
					BEGIN
						SET @ctnno = @ctnno + 1

						INSERT INTO @tempPackingList (RefNo,CTNQty,Article,Color,SizeCode,QtyPerCTN,ShipQty,NW,GW,NNW,NWPerPcs,CtnNo,SizeSeq,LastCntNo,ArticleSeq,Barcode)
							VALUES (ISNULL(@RefNoForBalance,@refno), 1, @article, @color, @sizecode, @currentqty, @currentqty, (@nw/@qtyperctn)*@currentqty, ISNULL(@GwBalance,@gw-@nw), (@nnw/@qtyperctn)*@currentqty, @nw/@qtyperctn, @ctnno, @seq, @lastctn, @articleSeq, @Barcode)
					END
				ELSE
					BEGIN
						INSERT INTO @tempPackingList (RefNo,CTNQty,Article,Color,SizeCode,QtyPerCTN,ShipQty,NW,GW,NNW,NWPerPcs,CtnNo,SizeSeq,LastCntNo,ArticleSeq,Barcode)
							VALUES (ISNULL(@RefNoForBalance,@refno), 0, @article, @color, @sizecode, @currentqty, @currentqty, (@nw/@qtyperctn)*@currentqty, 0, (@nnw/@qtyperctn)*@currentqty, @nw/@qtyperctn, @ctnno, @seq, @lastctn, @articleSeq, @Barcode)
					END
			END
		FETCH NEXT FROM cursor_packingguide INTO @refno, @color, @sizecode, @qtyperctn, @shipqty, @nw, @gw, @nnw, @seq, @Barcode, @RefNoForBalance
	END
	CLOSE cursor_packingguide
	DEALLOCATE cursor_packingguide

	FETCH NEXT FROM cursor_groupbyarticle INTO @article, @articleSeq
END
--關閉cursor與參數的關聯
CLOSE cursor_groupbyarticle
--將cursor物件從記憶體移除
DEALLOCATE cursor_groupbyarticle

--宣告變數
DECLARE @ctnqty INT, --Carton數
		@nwperpcs NUMERIC(5,3) --每件淨重

--更新箱號
UPDATE @tempPackingList SET CTNStartNo = CONVERT(VARCHAR,CtnNo) + LastCntNo

--更新重量
DECLARE cursor_@temppacklistgroup CURSOR FOR
	SELECT DISTINCT Article,CtnNo FROM @tempPackingList
OPEN cursor_@temppacklistgroup
FETCH NEXT FROM cursor_@temppacklistgroup INTO @article, @ctnno
WHILE @@FETCH_STATUS = 0
BEGIN
	SELECT @nw = SUM(NW), @gw = SUM(GW), @nnw = SUM(NNW), @seqcount = MAX(CtnNo) FROM @tempPackingList WHERE Article = @article and CtnNo = @ctnno
	UPDATE @tempPackingList 
	SET NW = @nw,
		GW = @nw+@gw,
		NNW = @nnw
	WHERE Article = @article and CtnNo = @ctnno AND CTNQty = 1

	UPDATE @tempPackingList 
	SET NW = 0, GW = 0, NNW = 0 WHERE Article = @article and CtnNo = @ctnno AND CTNQty = 0

	FETCH NEXT FROM cursor_@temppacklistgroup INTO @article, @ctnno
END
CLOSE cursor_@temppacklistgroup
DEALLOCATE cursor_@temppacklistgroup

--全部總重量
SELECT @ttlnw = SUM(NW), @ttlgw = SUM(GW), @ttlnnw = SUM(NNW), @ttlshipqty = SUM(ShipQty), @seqcount = SUM(CtnQty) FROM @tempPackingList

--刪除PackingList_Detail資料
DELETE PackingList_Detail WHERE ID = @id

--資料存入PackingList & PackingList_Detail
--宣告變數
DECLARE @havepl INT, --檢查PackingList是否存在
		@addname VARCHAR(10), --系統登入人員
		@adddate DATETIME --新增時間
SET @addname = '{1}'
SET @adddate = GETDATE()

SET XACT_ABORT ON;
BEGIN TRANSACTION
--PackingList
SELECT @havepl = count(ID) FROM PackingList WITH (NOLOCK) WHERE ID = @id
IF @havepl = 0
	BEGIN --新增PackingList
		INSERT INTO PackingList (ID,Type,MDivisionID,FactoryID,ShipModeID,BrandID,Dest,CustCDID,CTNQty,ShipQty,NW,GW,NNW,CBM,Remark,Status,AddName,AddDate,QueryDate)
			VALUES (@id, 'B', @mdivisionid, @factoryid, @shipmodeid, @brandid, @dest, @custcdid, @seqcount, @ttlshipqty, @ttlnw, @ttlgw, @ttlnnw, @cbm, @remark, 'New', @addname, @adddate, @adddate)
	END
ELSE
	BEGIN --更新PackingList
		UPDATE PackingList 
		SET MDivisionID = @mdivisionid,
			FactoryID = @factoryid,
			ShipModeID = @shipmodeid,
			BrandID = @brandid,
			Dest = @dest,
			CustCDID = @custcdid,
			CTNQty = @seqcount,
			ShipQty = @ttlshipqty,
			NW = @ttlnw,
			GW = @ttlgw,
			NNW = @ttlnnw,
			CBM = @cbm,
			Remark = @remark,
			Status = 'New',
			AddName = @addname,
			AddDate = @adddate,
			EditName = '',
			EditDate = null,
            QueryDate = @adddate
		WHERE ID = @id
	END

--PackingList_Detail
DECLARE @cartonsatrtno VARCHAR(6)

SET @seqcount = 0

DECLARE cursor_temppackinglist CURSOR FOR
	SELECT RefNo,CTNStartNo,CTNQty,Article,Color,SizeCode,QtyPerCTN,ShipQty,NW,GW,NNW,NWPerPcs,Barcode FROM @tempPackingList ORDER BY ArticleSeq,CtnNo,SizeSeq

OPEN cursor_temppackinglist
FETCH NEXT FROM cursor_temppackinglist INTO @refno, @cartonsatrtno, @ctnqty, @article, @color, @sizecode, @qtyperctn, @shipqty, @nw, @gw, @nnw, @nwperpcs,@Barcode
WHILE @@FETCH_STATUS = 0
BEGIN
	SET @seqcount = @seqcount + 1
	SELECT @seq = REPLICATE('0', 6 - LEN(CONVERT(VARCHAR,@seqcount))) + CONVERT(VARCHAR,@seqcount)
	INSERT INTO PackingList_Detail(ID,OrderID,OrderShipmodeSeq,RefNo,CTNStartNo,CTNEndNo,CTNQty,Article,Color,SizeCode,QtyPerCTN,ShipQty,NW,GW,NNW,NWPerPcs,Seq,Barcode)
		VALUES (@id, @orderid, @ordershipmodeseq, @refno, @cartonsatrtno, @cartonsatrtno, @ctnqty, @article, @color, @sizecode, @qtyperctn, @shipqty, @nw, @gw, @nnw, @nwperpcs, @seq, @Barcode)
	--將下一筆資料填入變數
	FETCH NEXT FROM cursor_temppackinglist INTO @refno, @cartonsatrtno, @ctnqty, @article, @color, @sizecode, @qtyperctn, @shipqty, @nw, @gw, @nnw, @nwperpcs, @Barcode
END
CLOSE cursor_temppackinglist
DEALLOCATE cursor_temppackinglist

IF @@ERROR <> 0
	ROLLBACK TRANSACTION
ELSE
	COMMIT TRANSACTION",
        this.CurrentMaintain["ID"].ToString(),
        Sci.Env.User.UserID);
                    #endregion
                }
                else
                {
                    #region 單色單碼
                    insertCmd = string.Format(
                        @"--宣告變數: PackingGuide帶入的參數
DECLARE @id VARCHAR(13),
		@mdivisionid VARCHAR(8),
		@factoryid VARCHAR(8),
		@orderid VARCHAR(13),
		@ordershipmodeseq VARCHAR(2),
		@shipmodeid VARCHAR(10),
		@ctnstartno INT,
		@cbm NUMERIC(8,4),
		@remark NVARCHAR(125),
		@article VARCHAR(8)

--設定變數值
SET @id = '{0}'
SELECT @mdivisionid = MDivisionID, @factoryid = FactoryID, @orderid = OrderID, @ordershipmodeseq = OrderShipmodeSeq, @shipmodeid = ShipModeID, @ctnstartno = CTNStartNo, @cbm = CBM, @remark = Remark  FROM PackingGuide WITH (NOLOCK) WHERE Id = @id

--宣告變數: Orders相關的參數
DECLARE @brandid VARCHAR(8),
		@dest VARCHAR(2),
		@custcdid VARCHAR(16)
--設定變數值
SELECT @brandid = BrandID, @dest = Dest, @custcdid = CustCDID FROM Orders WITH (NOLOCK) WHERE ID = @orderid

--建立tmpe table存放展開後結果
DECLARE @tempPackingList TABLE (
   RefNo VARCHAR(21),
   CTNStartNo VARCHAR(6),
   CTNQty INT,
   Seq VARCHAR(6),
   Article VARCHAR(8),
   Color VARCHAR(6),
   SizeCode VARCHAR(8),
   QtyPerCTN SMALLINT,
   ShipQty INT,
   NW NUMERIC(7,3),
   GW NUMERIC(7,3),
   NNW NUMERIC(7,3),
   NWPerPcs NUMERIC(5,3),
   Barcode varchar(30)
)

--建立tmpe table存放餘箱資料
DECLARE @tempRemainder TABLE (
   RefNo VARCHAR(21),
   CTNQty INT,
   Seq INT,
   Article VARCHAR(8),
   Color VARCHAR(6),
   SizeCode VARCHAR(8),
   QtyPerCTN SMALLINT,
   ShipQty INT,
   NW NUMERIC(7,3),
   GW NUMERIC(7,3),
   NNW NUMERIC(7,3),
   NWPerPcs NUMERIC(5,3),
   Barcode varchar(30)
)

--先開article的cursor
DECLARE cursor_packing_article CURSOR FOR
	select a.Article
		FROM PackingGuide_Detail a WITH (NOLOCK)
		LEFT JOIN Order_Article c WITH (NOLOCK) ON c.Id = @orderid AND a.Article = c.Article
		WHERE a.ID = @id group by a.Article ORDER BY max(c.Seq)



--宣告變數: PackingGuide_Detail相關的參數
DECLARE @refno VARCHAR(21),
		@color VARCHAR(6),
		@sizecode VARCHAR(8),
		@qtyperctn SMALLINT,
		@shipqty INT,
		@nw NUMERIC(7,3),
		@gw NUMERIC(7,3),
		@nnw NUMERIC(5,3),
        @BarCode varchar(30),
		@RefNoForBalance VARCHAR(21)

--宣告變數: 記錄程式中的資料
DECLARE @currentqty INT, --目前數量
		@ctnno INT, --箱號
		@realctnno VARCHAR(6), --寫入Table中的箱號
		@seqcount INT, --排序用編號
		@seq VARCHAR(6), --排序用編號
		@remaindercount INT, --餘箱資料個數
		@ttlshipqty INT, --總件數，寫入PackingList時使用
		@ttlnw NUMERIC(9,3), --總淨重，寫入PackingList時使用
		@ttlgw NUMERIC(9,3), --總毛重，寫入PackingList時使用
		@ttlnnw NUMERIC(9,3) --總淨淨重，寫入PackingList時使用

Declare @GwBalance NUMERIC(7,3) -- 尾箱重新撈取GW

DECLARE @lastctn varchar(2)
DECLARE @articlecnt INT
SET @seqcount = 0
SET @remaindercount = 0
SET @ttlshipqty = 0
SET @ttlnw = 0
SET @ttlgw = 0
SET @ttlnnw = 0
SET @articlecnt = 0
OPEN cursor_packing_article
FETCH NEXT FROM cursor_packing_article INTO @article
WHILE @@FETCH_STATUS = 0
BEGIN
	SET @articlecnt = @articlecnt + 1
	SET @ctnno = @ctnstartno
	SET @lastctn = CHAR(@articlecnt + 64) --ASCII 65開始是A
	--開始run cursor
	--將PackingGuide_Detail資料存放至Cursor
	DECLARE cursor_packguide CURSOR FOR
	    SELECT a.RefNo,a.Color,a.SizeCode,a.QtyPerCTN,a.ShipQty,a.NW,a.GW,a.NNW , isnull(cb.BarCode,''), a.RefNoForBalance
	    FROM PackingGuide_Detail a WITH (NOLOCK) 
	    LEFT JOIN Orders b WITH (NOLOCK) ON b.ID = @orderid
	    LEFT JOIN Order_SizeCode d WITH (NOLOCK) ON d.Id = b.POID AND a.SizeCode = d.SizeCode
        LEFT JOIN CustBarCode cb WITH (NOLOCK) ON b.CustPoNo = cb.CustPoNo and b.StyleID = cb.StyleID and a.Article = cb.Article and a.SizeCode = cb.SizeCode
	    WHERE a.ID = @id and a.Article = @article ORDER BY d.Seq
	OPEN cursor_packguide
	--將第一筆資料填入變數
	FETCH NEXT FROM cursor_packguide INTO @refno, @color, @sizecode, @qtyperctn, @shipqty, @nw, @gw, @nnw, @Barcode, @RefNoForBalance
	WHILE @@FETCH_STATUS = 0
	BEGIN
		IF @qtyperctn > 0
		BEGIN
			SET @currentqty = @shipqty
			WHILE @currentqty > 0
			BEGIN
				IF @currentqty >= @qtyperctn
					BEGIN
						SET @seqcount = @seqcount + 1
						SELECT @seq = REPLICATE('0', 6 - LEN(CONVERT(VARCHAR,@seqcount))) + CONVERT(VARCHAR,@seqcount)
						SELECT @realctnno = CONVERT(VARCHAR,@ctnno) + @lastctn
						INSERT INTO @tempPackingList (RefNo,CTNStartNo,CTNQty,Article,Color,SizeCode,QtyPerCTN,ShipQty,NW,GW,NNW,NWPerPcs,Seq,Barcode)
							VALUES (@refno, @realctnno, 1, @article, @color, @sizecode, @qtyperctn, @qtyperctn, @nw, @gw, @nnw, @nw/@qtyperctn, @seq,@Barcode)
						SET @ctnno = @ctnno + 1
						SET @ttlnw = @ttlnw + @nw
						SET @ttlgw = @ttlgw + @gw
						SET @ttlnnw = @ttlnnw + @nnw
						SET @ttlshipqty = @ttlshipqty + @qtyperctn
					END
				ELSE
					BEGIN
						SET @remaindercount = @remaindercount + 1
						IF ISNULL(@RefNoForBalance,'') = ''
						BEGIN
							INSERT INTO @tempRemainder (RefNo,CTNQty,Article,Color,SizeCode,QtyPerCTN,ShipQty,NW,GW,NNW,NWPerPcs,Seq,Barcode)
								VALUES (@refno, 1, @article, @color, @sizecode, @currentqty, @currentqty, (@nw/@qtyperctn)*@currentqty, ((@nw/@qtyperctn)*@currentqty)+(@gw-@nw), (@nnw/@qtyperctn)*@currentqty, (@nw/@qtyperctn), @remaindercount,@Barcode)
						END
						ELSE
						BEGIN
							SELECT @GwBalance = CtnWeight FROM LocalItem WHERE RefNo = @RefNoForBalance
							-- 有設定尾箱的料號
							INSERT INTO @tempRemainder (RefNo,CTNQty,Article,Color,SizeCode,QtyPerCTN,ShipQty,NW,GW,NNW,NWPerPcs,Seq,BarCode)
								VALUES (@RefNoForBalance, 1, @article, @color, @sizecode, @currentqty, @currentqty, (@nw/@qtyperctn)*@currentqty, ((@nw/@qtyperctn)*@currentqty)+@GwBalance, (@nnw/@qtyperctn)*@currentqty, (@nw/@qtyperctn), @remaindercount,@Barcode)
						END
					END
	
				SET @currentqty = @currentqty - @qtyperctn
			END
		END
		--將下一筆資料填入變數
		FETCH NEXT FROM cursor_packguide INTO @refno, @color, @sizecode, @qtyperctn, @shipqty, @nw, @gw, @nnw,@Barcode, @RefNoForBalance
	END
	--關閉cursor與參數的關聯
	CLOSE cursor_packguide
	--將cursor物件從記憶體移除
	DEALLOCATE cursor_packguide

	--將餘箱資料寫入@tempPackingList
	--將@tempRemainder資料存放至Cursor
	DECLARE cursor_temRemainder CURSOR FOR
		SELECT RefNo,CTNQty,Article,Color,SizeCode,QtyPerCTN,ShipQty,NW,GW,NNW,NWPerPcs,Barcode FROM @tempRemainder ORDER BY Seq
	--宣告變數: 記錄程式中的資料
	DECLARE @ctnqty INT, --Carton數
			@nwperpcs NUMERIC(5,3) --每件淨重
	
	OPEN cursor_temRemainder
	FETCH NEXT FROM cursor_temRemainder INTO @refno, @ctnqty, @article, @color, @sizecode, @qtyperctn, @shipqty, @nw, @gw, @nnw, @nwperpcs,@Barcode
	WHILE @@FETCH_STATUS = 0
	BEGIN
		SET @seqcount = @seqcount + 1
		SELECT @seq = REPLICATE('0', 6 - LEN(CONVERT(VARCHAR,@seqcount))) + CONVERT(VARCHAR,@seqcount)
		SELECT @realctnno = CONVERT(VARCHAR,@ctnno) + @lastctn
		INSERT INTO @tempPackingList (RefNo,CTNStartNo,CTNQty,Article,Color,SizeCode,QtyPerCTN,ShipQty,NW,GW,NNW,NWPerPcs,Seq,Barcode)
			VALUES (@refno, @realctnno, @ctnqty, @article, @color, @sizecode, @qtyperctn, @qtyperctn, @nw, @gw, @nnw, @nwperpcs, @seq,@Barcode)
		SET @ctnno = @ctnno + 1
		SET @ttlnw = @ttlnw + @nw
		SET @ttlgw = @ttlgw + @gw
		SET @ttlnnw = @ttlnnw + @nnw
		SET @ttlshipqty = @ttlshipqty + @qtyperctn
	
		--將下一筆資料填入變數
		FETCH NEXT FROM cursor_temRemainder INTO @refno, @ctnqty, @article, @color, @sizecode, @qtyperctn, @shipqty, @nw, @gw, @nnw, @nwperpcs, @Barcode
	END
	CLOSE cursor_temRemainder
	DEALLOCATE cursor_temRemainder
	DELETE @tempRemainder


FETCH NEXT FROM cursor_packing_article INTO @article
END
CLOSE cursor_packing_article
DEALLOCATE cursor_packing_article


--刪除PackingList_Detail資料
DELETE PackingList_Detail WHERE ID = @id

--資料存入PackingList & PackingList_Detail
--宣告變數
DECLARE @havepl INT, --檢查PackingList是否存在
		@addname VARCHAR(10), --系統登入人員
		@adddate DATETIME --新增時間
SET @addname = '{1}'
SET @adddate = GETDATE()

SET XACT_ABORT ON;
BEGIN TRANSACTION
--PackingList
SELECT @havepl = count(ID) FROM PackingList WITH (NOLOCK) WHERE ID = @id
IF @havepl = 0
	BEGIN --新增PackingList
		INSERT INTO PackingList (ID,Type,MDivisionID,FactoryID,ShipModeID,BrandID,Dest,CustCDID,CTNQty,ShipQty,NW,GW,NNW,CBM,Remark,Status,AddName,AddDate,QueryDate)
			VALUES (@id, 'B', @mdivisionid, @factoryid, @shipmodeid, @brandid, @dest, @custcdid, @seqcount, @ttlshipqty, @ttlnw, @ttlgw, @ttlnnw, @cbm, @remark, 'New', @addname, @adddate, @adddate)
	END
ELSE
	BEGIN --更新PackingList
		UPDATE PackingList 
		SET MDivisionID = @mdivisionid,
			FactoryID = @factoryid,
			ShipModeID = @shipmodeid,
			BrandID = @brandid,
			Dest = @dest,
			CustCDID = @custcdid,
			CTNQty = @seqcount,
			ShipQty = @ttlshipqty,
			NW = @ttlnw,
			GW = @ttlgw,
			NNW = @ttlnnw,
			CBM = @cbm,
			Remark = @remark,
			Status = 'New',
			AddName = @addname,
			AddDate = @adddate,
			EditName = '',
			EditDate = null,
            QueryDate = @adddate
		WHERE ID = @id
	END

--PackingList_Detail
DECLARE @cartonsatrtno VARCHAR(6)

DECLARE cursor_temPackingList CURSOR FOR
	SELECT RefNo,CTNStartNo,CTNQty,Article,Color,SizeCode,QtyPerCTN,ShipQty,NW,GW,NNW,NWPerPcs,Seq,Barcode FROM @tempPackingList ORDER BY Seq
OPEN cursor_temPackingList
FETCH NEXT FROM cursor_temPackingList INTO @refno, @cartonsatrtno, @ctnqty, @article, @color, @sizecode, @qtyperctn, @shipqty, @nw, @gw, @nnw, @nwperpcs, @seq, @Barcode
WHILE @@FETCH_STATUS = 0
BEGIN
	INSERT INTO PackingList_Detail(ID,OrderID,OrderShipmodeSeq,RefNo,CTNStartNo,CTNEndNo,CTNQty,Article,Color,SizeCode,QtyPerCTN,ShipQty,NW,GW,NNW,NWPerPcs,Seq,Barcode)
		VALUES (@id, @orderid, @ordershipmodeseq, @refno, @cartonsatrtno, @cartonsatrtno, @ctnqty, @article, @color, @sizecode, @qtyperctn, @shipqty, @nw, @gw, @nnw, @nwperpcs, @seq,@Barcode)
	--將下一筆資料填入變數
	FETCH NEXT FROM cursor_temPackingList INTO @refno, @cartonsatrtno, @ctnqty, @article, @color, @sizecode, @qtyperctn, @shipqty, @nw, @gw, @nnw, @nwperpcs, @seq,@Barcode
END
CLOSE cursor_temPackingList
DEALLOCATE cursor_temPackingList

IF @@ERROR <> 0
	ROLLBACK TRANSACTION
ELSE
	COMMIT TRANSACTION
",
                        this.CurrentMaintain["ID"].ToString(),
                        Env.User.UserID);
                    #endregion
                }
                #endregion
            }

            return insertCmd;
        }

        // Switch to Packing list
        private void BtnSwitchToPackingList_Click(object sender, EventArgs e)
        {
            // 檢查OrderID+Seq不可以重複建立
            if (MyUtility.Check.Seek(string.Format("select ID from PackingList WITH (NOLOCK) where OrderID = '{0}' AND OrderShipmodeSeq = '{1}' AND ID != '{2}'", this.CurrentMaintain["OrderID"].ToString(), this.CurrentMaintain["OrderShipmodeSeq"].ToString(), this.CurrentMaintain["ID"].ToString())))
            {
                MyUtility.Msg.WarningBox("SP No:" + this.CurrentMaintain["OrderID"].ToString() + ", Seq:" + this.CurrentMaintain["OrderShipmodeSeq"].ToString() + " already exist in packing list, can't be create again!");
                return;
            }

            // 檢查訂單狀態：如果已經Pullout Complete出訊息告知使用者且不做任何事
            string lookupReturn = MyUtility.GetValue.Lookup("select PulloutComplete from Orders WITH (NOLOCK) where ID = '" + this.CurrentMaintain["OrderID"].ToString() + "'");
            if (lookupReturn == "True")
            {
                MyUtility.Msg.WarningBox("SP# was ship complete!! You can't switch to packing list.");
                return;
            }

            // 檢查PackingList狀態：(1)PackingList如果已經Confirm就出訊息告知使用者且不做任事 (2)如果已經有Invoice No就出訊息告知使用者且不做任事
            DataRow seekData;
            string seekCmd = "select Status, InvNo from PackingList WITH (NOLOCK) where ID = '" + this.CurrentMaintain["ID"].ToString().Trim() + "'";
            if (MyUtility.Check.Seek(seekCmd, out seekData))
            {
                if (seekData["Status"].ToString() == "Confirmed")
                {
                    MyUtility.Msg.WarningBox("SP# has been confirmed!! You can't switch to packing list.");
                    return;
                }

                if (seekData["InvNo"].ToString() == "Y")
                {
                    MyUtility.Msg.WarningBox("SP# was booking!! You can't switch to packing list.");
                    return;
                }
            }

            // 檢查PackingList是否已經有箱子送到Clog，若有，就出訊息告知使用者且不做任事
            seekCmd = "select ID from PackingList_Detail WITH (NOLOCK) where ID = '" + this.CurrentMaintain["ID"].ToString() + "' and TransferDate is not null";
            if (MyUtility.Check.Seek(seekCmd))
            {
                MyUtility.Msg.WarningBox("SP# has been transfer!! You can't switch to packing list.");
                return;
            }

            string sqlchk = $@"
select ShipQty =
isnull((select sum(QtyPerCTN)
from PackingList_Detail with (nolock) where OrderID = '{this.CurrentMaintain["OrderID"].ToString()}' and ID <> '{this.CurrentMaintain["ID"].ToString()}' ),0)
+
isnull((select sum(iq.DiffQty)
FROm InvAdjust i
INNER JOIN InvAdjust_Qty iq ON i.ID = iq.ID
where OrderID = '{this.CurrentMaintain["OrderID"].ToString()}'),0)
";
            int shipQty = MyUtility.Convert.GetInt(MyUtility.GetValue.Lookup(sqlchk));

            sqlchk = $@"
select Qty=isnull(sum(Qty),0)
from Order_QtyShip_Detail with (nolock) where ID = '{this.CurrentMaintain["OrderID"].ToString()}'
";
            int orderQty = MyUtility.Convert.GetInt(MyUtility.GetValue.Lookup(sqlchk));

            if (shipQty >= orderQty)
            {
                string pdids = MyUtility.GetValue.Lookup($"select id = stuff((select concat(',',id)from (select distinct id from PackingList_Detail where OrderID = '{this.CurrentMaintain["OrderID"]}') a for xml path('')),1,1,'')");
                MyUtility.Msg.WarningBox($"SP# {this.CurrentMaintain["OrderID"]} ship QTY can not more than total Qrder QTY! PackingList ID {pdids}");
                return;
            }

            #region 檢查Packinglist_Detail與此次轉換數量加總是否超過Order_QtyShip數量
            string sqlCheckShipQty = $@"
declare @PKQty int
declare @shipQty int

select @PKQty = isnull(sum(QtyPerCTN),0)
from PackingList_Detail with (nolock) where OrderID = '{this.CurrentMaintain["OrderID"].ToString()}' and
                                            OrderShipmodeSeq = '{this.CurrentMaintain["OrderShipmodeSeq"].ToString()}' and
                                            ID <> '{this.CurrentMaintain["ID"].ToString()}' 

select @shipQty = sum (Qty)
from Order_QtyShip_Detail with (nolock) where ID = '{this.CurrentMaintain["OrderID"].ToString()}' and Seq = '{this.CurrentMaintain["OrderShipmodeSeq"].ToString()}'

select [PKQty] = @PKQty,[shipQty] = @shipQty

";
            DataRow drCheckShipQty;
            MyUtility.Check.Seek(sqlCheckShipQty, out drCheckShipQty);
            bool isOverShipQty = ((int)drCheckShipQty["PKQty"] + this.numTotalShipQty.Value) > (int)drCheckShipQty["shipQty"] ? true : false;
            if (isOverShipQty)
            {
                MyUtility.Msg.WarningBox($"<SP#>{this.CurrentMaintain["OrderID"]},<Seq>{this.CurrentMaintain["OrderShipmodeSeq"].ToString()} already switch Packing Qty({drCheckShipQty["PKQty"].ToString()}) and this time switch Packing Qty({this.numTotalShipQty.Value}),can not more than this Seq Ship Qty({drCheckShipQty["shipQty"].ToString()})");
                return;
            }
            #endregion
            string insertCmd = this.GetSwitchToPackingListSQL(((Button)sender).Name);

            using (TransactionScope transaction = new TransactionScope())
            {
                DualResult result = DBProxy.Current.Execute(null, insertCmd);
                if (result)
                {
                    // 存檔成功後，要再呼叫UpdateOrdersCTN, CreateOrderCTNData
                    bool prgResult = Prgs.UpdateOrdersCTN(this.CurrentMaintain["OrderID"].ToString());
                    if (!prgResult)
                    {
                        return;
                    }

                    prgResult = Prgs.CreateOrderCTNData(this.CurrentMaintain["ID"].ToString());
                    if (!prgResult)
                    {
                        return;
                    }

                    prgResult = Prgs.PackingP02CreateSCICtnNo(this.CurrentMaintain["ID"].ToString());
                    if (!prgResult)
                    {
                        return;
                    }

                    transaction.Complete();
                    transaction.Dispose();
                    MyUtility.Msg.InfoBox("Switch completed!");
                }
                else
                {
                    transaction.Dispose();
                    MyUtility.Msg.WarningBox("Switch fail!\r\n" + result.ToString());
                    return;
                }
            }
        }

        // Packing Method
        private void ComboPackingMethod_SelectionChangeCommitted(object sender, EventArgs e)
        {
            this.ControlGridColumn();
        }

        // ShipMode
        private void Txtshipmode_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (!MyUtility.Check.Empty(this.txtshipmode.SelectedValue))
            {
                if (MyUtility.Check.Empty(this.CurrentMaintain["OrderShipmodeSeq"]))
                {
                    MyUtility.Msg.WarningBox("ShipMode is incorrect!");
                    this.txtshipmode.SelectedValue = string.Empty;
                }
                else
                {
                    string sqlCmd = string.Format("select ShipModeID from Order_QtyShip WITH (NOLOCK) where ID = '{0}' and Seq = '{1}'", this.CurrentMaintain["OrderID"].ToString(), this.CurrentMaintain["OrderShipmodeSeq"].ToString());
                    DataRow qtyShipData;
                    if (MyUtility.Check.Seek(sqlCmd, out qtyShipData))
                    {
                        if (qtyShipData["ShipModeID"].ToString() != this.txtshipmode.SelectedValue.ToString())
                        {
                            MyUtility.Msg.WarningBox("ShipMode is incorrect!");
                            this.txtshipmode.SelectedValue = string.Empty;
                        }
                    }
                    else
                    {
                        MyUtility.Msg.WarningBox("ShipMode is incorrect!");
                        this.txtshipmode.SelectedValue = string.Empty;
                    }
                }
            }
        }

        private void TxtCartonRef_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem("select RefNo,Description,STR(CtnLength,8,4)+'*'+STR(CtnWidth,8,4)+'*'+STR(CtnHeight,8,4) as Dim from LocalItem where Category = 'CARTON' and Junk = 0 order by RefNo", "10,25,25", this.txtCartonRef.Text.Trim());
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            this.txtCartonRef.Text = item.GetSelectedString();
        }

        private void TxtCartonRefBalance_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem("select RefNo,Description,STR(CtnLength,8,4)+'*'+STR(CtnWidth,8,4)+'*'+STR(CtnHeight,8,4) as Dim from LocalItem where Category = 'CARTON' and Junk = 0 order by RefNo", "10,25,25", this.txtCartonRefBalance.Text.Trim());
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            this.txtCartonRefBalance.Text = item.GetSelectedString();
        }

        private void TxtCartonRef_Validating(object sender, CancelEventArgs e)
        {
            if (MyUtility.Check.Empty(this.txtCartonRef.Text))
            {
                this.txtCartonRef.Text = string.Empty;
                return;
            }

            List<SqlParameter> sqlpar = new List<SqlParameter>();
            sqlpar.Add(new SqlParameter("@CartonRef", this.txtCartonRef.Text));

            string seekSql = "select RefNo,Description,CtnWeight from LocalItem where Category = 'CARTON' and Junk = 0 and RefNo = @CartonRef";
            DataRow dr;
            if (!MyUtility.Check.Seek(seekSql, sqlpar, out dr))
            {
                e.Cancel = true;
                MyUtility.Msg.WarningBox(string.Format("< Ref No. : {0} > not found!!!", this.txtCartonRef.Text));
                return;
            }
            else
            {
                this.txtCartonRef.Text = MyUtility.Convert.GetString(dr["RefNo"]);
            }
        }

        private void TxtCartonRefBalance_Validating(object sender, CancelEventArgs e)
        {
            if (MyUtility.Check.Empty(this.txtCartonRefBalance.Text))
            {
                this.txtCartonRefBalance.Text = string.Empty;
                return;
            }

            List<SqlParameter> sqlpar = new List<SqlParameter>();
            sqlpar.Add(new SqlParameter("@CartonRef", this.txtCartonRefBalance.Text));

            string seekSql = "select RefNo,Description,CtnWeight from LocalItem where Category = 'CARTON' and Junk = 0 and RefNo = @CartonRef";
            DataRow dr;
            if (!MyUtility.Check.Seek(seekSql, sqlpar, out dr))
            {
                e.Cancel = true;
                MyUtility.Msg.WarningBox(string.Format("< Ref No. : {0} > not found!!!", this.txtCartonRefBalance.Text));
                return;
            }
            else
            {
                this.txtCartonRefBalance.Text = MyUtility.Convert.GetString(dr["RefNo"]);
            }
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            this.detailgrid.ValidateControl();
            foreach (DataGridViewRow item in this.detailgrid.Rows)
            {
                // 判斷selected欄位
                if (MyUtility.Convert.GetBool(item.Cells[0].Value))
                {
                    item.Cells[1].Value = this.txtCartonRef.Text; // Refno欄位
                }
            }
        }

        private void BtnUpdateBalance_Click(object sender, EventArgs e)
        {
            this.detailgrid.ValidateControl();
            foreach (DataGridViewRow item in this.detailgrid.Rows)
            {
                // 判斷selected欄位
                if (MyUtility.Convert.GetBool(item.Cells[0].Value))
                {
                    item.Cells[12].Value = this.txtCartonRefBalance.Text; // RefNoForBalance欄位
                }
            }
        }
    }
}
