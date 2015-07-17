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

namespace Sci.Production.Packing
{
    public partial class P02 : Sci.Win.Tems.Input6
    {
        Ict.Win.UI.DataGridViewTextBoxColumn col_refno;
        Ict.Win.UI.DataGridViewNumericBoxColumn col_qtyperctn;

        public P02(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.DefaultFilter = "FactoryID = '" + Sci.Env.User.Factory + "'";
            
        }

        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["ID"].ToString();
            this.DetailSelectCommand = string.Format(@"select a.Refno,a.Article,a.Color,a.SizeCode,a.QtyPerCTN,a.ShipQty,a.NW,a.GW,a.NNW,c.Description
                                                                                       from PackingGuide_Detail a
                                                                                       left join PackingGuide b on a.Id = b.Id
                                                                                       left join LocalItem c on a.RefNo = c.RefNo
                                                                                       left join Orders d on b.OrderID = d.ID
                                                                                       left join Order_Article e on b.OrderID = e.Id and a.Article = e.Article
                                                                                       left join Order_SizeCode f on d.POID = f.Id and a.SizeCode = f.SizeCode
                                                                                       where a.Id ='{0}'
                                                                                       order by e.Seq,f.Seq",masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            Dictionary<String, String> comboBox1_RowSource = new Dictionary<string, string>();
            comboBox1_RowSource.Add("1", "SOLID COLOR/SIZE");
            comboBox1_RowSource.Add("2", "SOLID COLOR/ASSORTED SIZE");
            comboBox1_RowSource.Add("3", "ASSORTED COLOR/SOLID SIZE");
            comboBox1_RowSource.Add("4", "ASSORTED COLOR/SIZE");
            comboBox1_RowSource.Add("5", "OTHER");
            comboBox1.DataSource = new BindingSource(comboBox1_RowSource, null);
            comboBox1.ValueMember = "Key";
            comboBox1.DisplayMember = "Value";
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            DataRow orderData;
            string sqlCmd;
            sqlCmd = string.Format("select StyleID,SeasonID,CustPONo,Qty,CtnType from Orders where ID = '{0}'", CurrentMaintain["OrderID"].ToString());
            if (MyUtility.Check.Seek(sqlCmd, out orderData))
            {
                displayBox2.Value = orderData["StyleID"].ToString();
                displayBox3.Value = orderData["SeasonID"].ToString();
                displayBox4.Value = orderData["CustPONo"].ToString();
                numericBox1.Value = Convert.ToInt32(orderData["Qty"].ToString());
                comboBox1.SelectedValue = orderData["CtnType"].ToString();
                sqlCmd = string.Format("select Qty from Order_QtyShip where ID = '{0}' and Seq = '{1}'", CurrentMaintain["OrderID"].ToString(), CurrentMaintain["OrderShipmodeSeq"].ToString());
                if (MyUtility.Check.Seek(sqlCmd, out orderData))
                {
                    numericBox4.Value = Convert.ToInt32(orderData["Qty"].ToString());
                }
            }

            //Special Instruction按鈕變色
            if (MyUtility.Check.Empty(CurrentMaintain["SpecialInstruction"].ToString()))
            {
                this.button1.ForeColor = Color.Black;
            }
            else
            {
                this.button1.ForeColor = Color.Blue;
            }

            //Carton Dimension按鈕變色
            if (MyUtility.Check.Seek(CurrentMaintain["OrderID"].ToString(), "Order_CTNData", "ID"))
            {
                this.button2.ForeColor = Color.Blue;
            }
            else
            {
                this.button2.ForeColor = Color.Black;
            }

            //Switch to Packing list是否有權限使用
            this.button3.Enabled = !this.EditMode && this.IsSupportEdit;
        }

        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();
            Helper.Controls.Grid.Generator(this.detailgrid)
                .CellCartonItem("RefNo", header: "Ref No.", width: Widths.AnsiChars(13)).Get(out col_refno)
                .Text("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("Article", header: "ColorWay", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("Color", header: "Color", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Numeric("QtyPerCTN", header: "Qty/Ctn").Get(out col_qtyperctn)
                .Numeric("ShipQty", header: "ShipQty", iseditingreadonly: true)
                .Numeric("NW", header: "N.W./Ctn", integer_places: 3, decimal_places: 3, maximum: 999.999M, minimum: 0)
                .Numeric("GW", header: "G.W./Ctn", integer_places: 3, decimal_places: 3, maximum: 999.999M, minimum: 0)
                .Numeric("NNW", header: "N.N.W./Ctn", integer_places: 3, decimal_places: 3, maximum: 999.999M, minimum: 0);

            this.detailgrid.CellValueChanged += (s, e) =>
            {
                #region 選完RefNo後，要自動帶出Description與G.W
                if (detailgrid.Columns[e.ColumnIndex].DataPropertyName == col_refno.DataPropertyName)
                {
                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);

                    if (MyUtility.Check.Empty(dr["RefNo"]))
                    {
                        dr["Description"] = "";
                        dr["GW"] = dr["NW"].ToString();
                    }
                    else
                    {
                        string seekSql = string.Format("select Description,Weight from LocalItem where RefNo = '{0}'", dr["RefNo"].ToString());
                        DataRow localItem;
                        if (MyUtility.Check.Seek(seekSql, out localItem))
                        {
                            dr["Description"] = localItem["Description"].ToString();
                            dr["GW"] = Convert.ToDouble(dr["NW"].ToString()) + Convert.ToDouble(localItem["Weight"].ToString());
                        }
                        else
                        {
                            dr["Description"] = "";
                            dr["GW"] = dr["NW"].ToString();
                        }
                    }
                    dr.EndEdit();
                }
                #endregion

                #region 輸入Qty/Ctn後要重算N.W.,G.W.,N.N.W.
                if (detailgrid.Columns[e.ColumnIndex].DataPropertyName == col_qtyperctn.DataPropertyName)
                {
                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);

                    string sqlCmd = string.Format(@"select isnull(li.Weight, 0) as CTNWeight,
                                                                                     isnull(sw.NW, isnull(sw2.NW, 0)) as NW,
                                                                                     isnull(sw.NNW, isnull(sw2.NNW, 0)) as NNW
                                                                          from Orders o
                                                                          left join Style_WeightData sw on sw.StyleUkey = o.StyleUkey and sw.Article = '{0}' and sw.SizeCode = '{1}'
                                                                          left join Style_WeightData sw2 on sw2.StyleUkey = o.StyleUkey and sw2.Article = '----' and sw2.SizeCode = '{1}'
                                                                          left join LocalItem li on li.RefNo = '{2}' and li.Category = 'CARTON'
                                                                          where o.ID = '{3}'", dr["Article"].ToString(), dr["SizeCode"].ToString(), dr["RefNo"].ToString(), CurrentMaintain["OrderID"].ToString());
                    DataTable selectedData;
                    DualResult result;
                    if (result = DBProxy.Current.Select(null, sqlCmd, out selectedData))
                    {
                        dr["NW"] = Convert.ToDouble(selectedData.Rows[0]["NW"].ToString()) * Convert.ToDouble(dr["QtyPerCTN"].ToString());
                        dr["GW"] = Convert.ToDouble(dr["NW"].ToString()) + Convert.ToDouble(selectedData.Rows[0]["CTNWeight"].ToString());
                        dr["NNW"] = Convert.ToDouble(selectedData.Rows[0]["NNW"].ToString()) * Convert.ToDouble(dr["QtyPerCTN"].ToString());
                    }
                    dr.EndEdit();
                }
                #endregion
            };
        }

        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();

            CurrentMaintain["FactoryID"] = Sci.Env.User.Factory;
            CurrentMaintain["CTNStartNo"] = 1;
            displayBox2.Value = "";
            displayBox3.Value = "";
            displayBox4.Value = "";
            numericBox1.Value = 0;
            numericBox4.Value = 0;
            comboBox1.SelectedValue = "";
        }

        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            ControlGridColumn();
        }

        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(CurrentMaintain["OrderID"]))
            {
                MessageBox.Show("< SP No. > can not be empty!");
                this.textBox1.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["OrderShipmodeSeq"]))
            {
                MessageBox.Show("< Seq > can not be empty!");
                this.textBox2.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["ShipModeID"]))
            {
                MessageBox.Show("< Shipping Mode > can not be empty!");
                this.txtshipmode1.Focus();
                return false;
            }

            //檢查表身不可以沒有資料
            DataRow[] detailData = ((DataTable)detailgridbs.DataSource).Select();
            if (detailData.Length == 0)
            {
                MessageBox.Show("Detail can't empty!");
                return false;
            }

            //表身的Ref No.與Qty/CTN不可以為空值
            foreach (DataRow dr in detailData)
            {
                bool isEmptyRefNo = MyUtility.Check.Empty(dr["RefNo"]);
                bool isEmptyQtyPerCTN = MyUtility.Check.Empty(dr["QtyPerCTN"]);
                bool isEmptyShipQty = MyUtility.Check.Empty(dr["ShipQty"]);
                if (isEmptyQtyPerCTN && !isEmptyShipQty)
                {
                    MessageBox.Show("< Color Way > " + dr["Article"].ToString().Trim() + " < Qty/Ctn > can't empty!");
                    return false;
                }

                if (isEmptyRefNo && !isEmptyShipQty)
                {
                    MessageBox.Show("< Color Way > " + dr["Article"].ToString().Trim() + " < Ref No. > can't empty!");
                    return false;
                }
            }

            #region 計算Total Cartons & CBM
            //Total Cartons: 單色混碼裝：min(同一顏色不同Size的訂單件數/每箱件數無條件捨去) + 1(若其中一個Size有餘數) or 0(完全整除沒有餘數)
            int ttlCTN = 0, ctns = 0;
            double ctn, ttlCBM = 0.0;
            string cbm;

            if (comboBox1.SelectedValue == "2")
            {
                DataTable groupData;
                DualResult result;
                if (result = DBProxy.Current.Select(null, "select '' as Article, 10 as ctn, 0.0 as CBM, 0 as Remainder where 1=0", out groupData))
                {
                    string article = "";
                    int recordNo = -1;
                    foreach (DataRow dr in detailData)
                    {
                        if (article != dr["Article"].ToString())
                        {
                            article = dr["Article"].ToString();
                            cbm = MyUtility.GetValue.Lookup("CBM", dr["RefNo"].ToString(), "LocalItem", "RefNo");
                            DataRow dr1 = groupData.NewRow();
                            dr1["Article"] = article;
                            dr1["CBM"] = Convert.ToDouble(cbm);
                            groupData.Rows.Add(dr1);
                            recordNo += 1;
                        }
                        if (MyUtility.Check.Empty(Convert.ToDouble(dr["QtyPerCTN"])))
                        {
                            ctn = 0;
                        }
                        else
                        {
                            ctn = Convert.ToDouble(dr["ShipQty"].ToString()) / Convert.ToDouble(dr["QtyPerCTN"].ToString());
                            if ((Convert.ToInt32(dr["ShipQty"].ToString()) % Convert.ToInt32(dr["QtyPerCTN"].ToString())) != 0)
                            {
                                groupData.Rows[recordNo]["Remainder"] = 1;
                            }
                        }
                        ctns = (int)Math.Floor(ctn);
                        if (string.IsNullOrWhiteSpace(groupData.Rows[recordNo]["ctn"].ToString()) || (Convert.ToInt32(groupData.Rows[recordNo]["ctn"].ToString()) > ctns))
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
                        ttlCTN = ttlCTN + Convert.ToInt32(dr["ctn"].ToString()) + remainder;
                        ttlCBM = ttlCBM + Convert.ToDouble(dr["CBM"].ToString()) * (Convert.ToInt32(dr["ctn"].ToString()) + remainder);
                    }
                }
            }
            else
            {
                //Total Cartons: 表身每一列資料的訂單件數/每箱件數無條件進位後加總
                foreach (DataRow dr in detailData)
                {
                    if (MyUtility.Check.Empty(Convert.ToDouble(dr["QtyPerCTN"])))
                    {
                        ctn = 0;
                    }
                    else
                    {
                        ctn = Convert.ToDouble(dr["ShipQty"].ToString()) / Convert.ToDouble(dr["QtyPerCTN"].ToString());
                    }
                    ctns = (int)Math.Ceiling(ctn);
                    ttlCTN = ttlCTN + ctns;
                    cbm = MyUtility.GetValue.Lookup("CBM", dr["RefNo"].ToString(), "LocalItem", "RefNo");
                    ttlCBM = ttlCBM + Convert.ToDouble(cbm) * ctns;
                }
            }
            #endregion
            CurrentMaintain["CTNQty"] = ttlCTN;
            CurrentMaintain["CBM"] = ttlCBM;

            //GetID
            if (IsDetailInserting)
            {
                string id = MyUtility.GetValue.GetID(ProductionEnv.Keyword + "PG", "PackingGuide", DateTime.Today, 2, "Id", null);
                if (MyUtility.Check.Empty(id))
                {
                    MessageBox.Show("GetID fail, please try again!");
                    return false;
                }
                CurrentMaintain["ID"] = id;
            }
            return base.ClickSaveBefore();
        }

        //控制Grid欄位的可修改性
        private void ControlGridColumn()
        {
            //當Packing Method為SOLID COLOR/ASSORTED SIZE (Order.CTNType = ‘2’)時，欄位Qty/Ctn不可被修改
            if (comboBox1.SelectedValue.ToString() == "2")
            {
                detailgrid.Columns[5].ReadOnly = true;
            }
            else
            {
                detailgrid.Columns[5].ReadOnly = false;
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
                                MessageBox.Show("SP#:" + textBox1.Text + msg);
                                //OrderID異動，其他相關欄位要跟著異動
                                ChangeOtherData("");
                                textBox1.Text = "";
                            }
                        }
                        else
                        {
                            returnData = true;
                            MessageBox.Show("< SP# > does not exist!");
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
                displayBox2.Value = "";
                displayBox3.Value = "";
                displayBox4.Value = "";
                numericBox1.Value = 0;
                numericBox4.Value = 0;
                comboBox1.SelectedValue = "";
            }
            else
            {
                DataRow orderData;
                string sqlCmd;
                sqlCmd = string.Format("select StyleID,SeasonID,CustPONo,Qty,CtnType,Packing from Orders where ID = '{0}'", orderID);
                if (MyUtility.Check.Seek(sqlCmd, out orderData))
                {
                    //帶出相關欄位的資料
                    displayBox2.Value = orderData["StyleID"].ToString();
                    displayBox3.Value = orderData["SeasonID"].ToString();
                    displayBox4.Value = orderData["CustPONo"].ToString();
                    numericBox1.Value = Convert.ToInt32(orderData["Qty"].ToString());
                    comboBox1.SelectedValue = orderData["CtnType"].ToString();
                    CurrentMaintain["SpecialInstruction"] = orderData["Packing"].ToString();

                    #region 若Order_QtyShip有多筆資料話就跳出視窗讓使者選擇Seq
                    int orderQty = Convert.ToInt32(orderData["Qty"].ToString());
                    sqlCmd = string.Format("select count(ID) as CountID from Order_QtyShip where ID = '{0}'", orderID);
                    if (MyUtility.Check.Seek(sqlCmd, out orderData))
                    {
                        if (orderData["CountID"].ToString() == "1")
                        {
                            sqlCmd = string.Format("select ShipModeID,Seq from Order_QtyShip where ID = '{0}'", orderID);
                            if (MyUtility.Check.Seek(sqlCmd, out orderData))
                            {
                                CurrentMaintain["OrderShipmodeSeq"] = orderData["Seq"].ToString();
                                CurrentMaintain["ShipModeID"] = orderData["ShipModeID"].ToString();
                                numericBox4.Value = orderQty;
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
                                numericBox4.Value = 0;
                            }
                            else
                            {
                                orderQtyShipData = item.GetSelecteds();
                                CurrentMaintain["OrderShipmodeSeq"] = item.GetSelectedString();
                                CurrentMaintain["ShipModeID"] = orderQtyShipData[0]["ShipmodeID"].ToString();
                                numericBox4.Value = Convert.ToInt32(orderQtyShipData[0]["Qty"].ToString());
                            }
                        }
                    }
                    #endregion

                    //ControlGridColumn();
                    GenDetailData(orderID, CurrentMaintain["OrderShipmodeSeq"].ToString());
                }
            }
        }

        //產生表身Grid的資料
        private void GenDetailData(string orderID, string seq)
        {
            if (!MyUtility.Check.Empty(orderID) && !MyUtility.Check.Empty(orderID))
            {
                string sqlCmd;
                if (comboBox1.SelectedValue.ToString() == "2")
                {
                    sqlCmd = string.Format("select * from Order_QtyCTN where Id = '{0}'", orderID);
                    if (!MyUtility.Check.Seek(sqlCmd))
                    {
                        MessageBox.Show("No packing data, can't create!!");
                        return;
                    }
                    sqlCmd = string.Format(@"select '' as ID, '' as RefNo, '' as Description, oqd.Article, oc.Color, oqd.SizeCode, oqd.Qty as ShipQty, oqc.Qty as QtyPerCTN, os.Seq,
                                                                           sw.NW as NW1, sw.NNW as NNW1, sw2.NW as NW2, sw2.NNW as NNW2,
                                                                           isnull(sw.NW, isnull(sw2.NW, 0))*oqc.Qty as NW,
                                                                           isnull(sw.NW, isnull(sw2.NW, 0))*oqc.Qty as GW,
                                                                           isnull(sw.NNW, isnull(sw2.NNW, 0))*oqc.Qty as NNW 
                                                                from Order_QtyShip_Detail oqd
                                                                left Join Orders o on o.ID = oqd.Id
                                                                left Join Order_QtyCTN oqc on oqc.id = oqd.Id and oqc.Article = oqd.Article and oqc.SizeCode = oqd.SizeCode
                                                                left join (select distinct id, Article, PatternPanel, (select ColorID 
                                                                                                                                                from Order_ColorCombo 
                                                                                                                                                where LectraCode = (select min(LectraCode) 
                                                                                                                                                                                   from Order_ColorCombo 
                                                                                                                                                                                   where id = a.id and  Article = a.Article and PatternPanel = 'FA') 
                                                                                                                                                                                   and id = a.id and  Article = a.Article and PatternPanel = 'FA') as Color 
                                                                               from Order_ColorCombo a
                                                                               where a.PatternPanel = 'FA') oc on oc.id = o.POID and oc.Article = oqd.Article and oc.PatternPanel = 'FA'
                                                                left join Style_WeightData sw on sw.StyleUkey = o.StyleUkey and sw.Article = oqd.Article and sw.SizeCode = oqd.SizeCode
                                                                left join Style_WeightData sw2 on sw2.StyleUkey = o.StyleUkey and sw2.Article = '----' and sw2.SizeCode = oqd.SizeCode
                                                                left join Order_SizeCode os on os.id = o.POID and os.SizeCode = oqd.SizeCode
                                                                left join Order_Article oa on oa.id = oqd.Id and oa.Article = oqd.Article
                                                                where oqd.ID = '{0}' and oqd.Seq = '{1}'
                                                                order by oa.Seq,os.Seq", orderID, seq);
                }
                else
                {
                    sqlCmd = string.Format(@"select '' as ID, '' as RefNo, '' as Description, oqd.Article, oc.Color, oqd.SizeCode, oqd.Qty as ShipQty, o.CTNQty as QtyPerCTN, os.Seq,
                                                                           sw.NW as NW1, sw.NNW as NNW1, sw2.NW as NW2, sw2.NNW as NNW2,
                                                                           isnull(sw.NW, isnull(sw2.NW, 0))*o.CTNQty as NW,
                                                                           isnull(sw.NW, isnull(sw2.NW, 0))*o.CTNQty as GW,
                                                                           isnull(sw.NNW, isnull(sw2.NNW, 0))*o.CTNQty as NNW 
                                                                from Order_QtyShip_Detail oqd
                                                                left Join Orders o on o.ID = oqd.Id
                                                                left join (select distinct id, Article, PatternPanel, (select ColorID 
                                                                                                                                                from Order_ColorCombo 
                                                                                                                                                where LectraCode = (select min(LectraCode) 
                                                                                                                                                                                   from Order_ColorCombo 
                                                                                                                                                                                   where id = a.id and  Article = a.Article and PatternPanel = 'FA') 
                                                                                                                                                and id = a.id and  Article = a.Article and PatternPanel = 'FA') as Color 
                                                                               from Order_ColorCombo a
                                                                               where a.PatternPanel = 'FA') oc on oc.id = oqd.Id and oc.Article = oqd.Article and oc.PatternPanel = 'FA'
                                                                left join Style_WeightData sw on sw.StyleUkey = o.StyleUkey and sw.Article = oqd.Article and sw.SizeCode = oqd.SizeCode
                                                                left join Style_WeightData sw2 on sw2.StyleUkey = o.StyleUkey and sw2.Article = '----' and sw2.SizeCode = oqd.SizeCode
                                                                left join Order_SizeCode os on os.id = o.POID and os.SizeCode = oqd.SizeCode
                                                                left join Order_Article oa on oa.id = oqd.Id and oa.Article = oqd.Article
                                                                where oqd.ID = '{0}' and oqd.Seq = '{1}'
                                                                order by oa.Seq,os.Seq", orderID, seq);
                }

                DataTable selectedData;
                DualResult result;
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
            IList<DataRow> orderQtyShipData;
            string sqlCmd = string.Format("select Seq, BuyerDelivery,ShipmodeID,Qty from Order_QtyShip where ID = '{0}'", CurrentMaintain["OrderID"].ToString());
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlCmd, "4,20,20,10", "", "Seq,Buyer Delivery,ShipMode,Qty");
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                CurrentMaintain["OrderShipmodeSeq"] = "";
                CurrentMaintain["ShipModeID"] = "";
                numericBox4.Value = 0;
            }
            else
            {
                orderQtyShipData = item.GetSelecteds();
                CurrentMaintain["OrderShipmodeSeq"] = item.GetSelectedString();
                CurrentMaintain["ShipModeID"] = orderQtyShipData[0]["ShipmodeID"].ToString();
                numericBox4.Value = Convert.ToInt32(orderQtyShipData[0]["Qty"].ToString());
            }
            // 清空表身Grid資料
            ((DataTable)detailgridbs.DataSource).Clear();
            GenDetailData(CurrentMaintain["OrderID"].ToString(), CurrentMaintain["OrderShipmodeSeq"].ToString());
        }

        //ShipMode
        private void txtshipmode1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.EditMode)
            {
                if (!MyUtility.Check.Empty(txtshipmode1.SelectedValue))
                {
                    if (MyUtility.Check.Empty(CurrentMaintain["OrderShipmodeSeq"]))
                    {
                        MessageBox.Show("ShipMode is incorrect!");
                        txtshipmode1.SelectedValue = "";
                    }
                    else
                    {
                        string sqlCmd = string.Format("select ShipModeID from Order_QtyShip where ID = '{0}' and Seq = '{1}'", CurrentMaintain["OrderID"].ToString(), CurrentMaintain["OrderShipmodeSeq"].ToString());
                        DataRow qtyShipData;
                        if (MyUtility.Check.Seek(sqlCmd, out qtyShipData))
                        {
                            if (qtyShipData["ShipModeID"].ToString() != txtshipmode1.SelectedValue.ToString())
                            {
                                MessageBox.Show("ShipMode is incorrect!");
                                txtshipmode1.SelectedValue = "";
                            }
                        }
                        else
                        {
                            MessageBox.Show("ShipMode is incorrect!");
                            txtshipmode1.SelectedValue = "";
                        }
                    }
                }
            }
        }

        //Packing Method
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != -1)
            {
                ControlGridColumn();
            }
        }

        //Special Instruction
        private void button1_Click(object sender, EventArgs e)
        {
            Sci.Win.Tools.EditMemo callNextForm = new Sci.Win.Tools.EditMemo(CurrentMaintain["SpecialInstruction"].ToString(), "Special Instruction", false,null);
            callNextForm.ShowDialog(this);
        }

        //Carton Dimension
        private void button2_Click(object sender, EventArgs e)
        {
            Sci.Production.Packing.P02_CartonSummary callNextForm = new Sci.Production.Packing.P02_CartonSummary(CurrentMaintain["OrderID"].ToString());
            callNextForm.ShowDialog(this);
        }

        //Switch to Packing list
        private void button3_Click(object sender, EventArgs e)
        {
            ////檢查訂單狀態：如果已經Pullout Complete出訊息告知使用者且不做任何事
            string lookupReturn = MyUtility.GetValue.Lookup("select PulloutComplete from Orders where ID = '" + CurrentMaintain["OrderID"].ToString() + "'");
            if (lookupReturn == "True")
            {
                MessageBox.Show("SP# was ship complete!! You can't switch to packing list.");
                return;
            }

            //檢查PackingList狀態：(1)PackingList如果已經Confirm就出訊息告知使用者且不做任事 (2)如果已經有Invoice No就出訊息告知使用者且不做任事
            DataRow seekData;
            string seekCmd = "select Status, INVNo from PackingList where ID = '" + CurrentMaintain["ID"].ToString() + "'";
            if (MyUtility.Check.Seek(seekCmd, out seekData))
            {
                if (seekData["Status"].ToString() == "Confirmed")
                {
                    MessageBox.Show("SP# has been confirmed!! You can't switch to packing list.");
                    return;
                }

                if (!MyUtility.Check.Empty(seekData["INVNo"]))
                {
                    MessageBox.Show("SP# was booking!! You can't switch to packing list.");
                    return;
                }
            }

            //檢查PackingList是否已經有箱子送到Clog，若有，就出訊息告知使用者且不做任事
            seekCmd = "select ID from PackingList_Detail where ID = '" + CurrentMaintain["ID"].ToString() + "' and TransferToClogID != ''";
            if (MyUtility.Check.Seek(seekCmd))
            {
                MessageBox.Show("SP# has been transfer!! You can't switch to packing list.");
                return;
            }

            #region 組Insert SQL
            string insertCmd;
            if (comboBox1.SelectedValue.ToString() == "2")
            {
                #region 單色混碼
                insertCmd = string.Format(@"--宣告變數: PackingGuide帶入的參數
DECLARE @id VARCHAR(13),
		@factoryid VARCHAR(8),
		@orderid VARCHAR(13),
		@ordershipmodeseq VARCHAR(2),
		@shipmodeid VARCHAR(10),
		@ctnstartno INT,
		@cbm NUMERIC(8,3),
		@remark NVARCHAR(125)
--設定變數值
SET @id = '{0}'
SELECT @factoryid = FactoryID, @orderid = OrderID, @ordershipmodeseq = OrderShipmodeSeq, @shipmodeid = ShipModeID, @ctnstartno = CTNStartNo, @cbm = CBM, @remark = Remark  FROM PackingGuide WHERE Id = @id

--宣告變數: Orders相關的參數
DECLARE @brandid VARCHAR(8),
		@dest VARCHAR(2),
		@custcdid VARCHAR(16)
--設定變數值
SELECT @brandid = BrandID, @dest = Dest, @custcdid = CustCDID FROM Orders WHERE ID = @orderid

--建立tmpe table存放展開後結果
DECLARE @tempPackingList TABLE (
   RefNo VARCHAR(20),
   CTNStartNo VARCHAR(6),
   CTNQty INT,
   Seq VARCHAR(6),
   Article VARCHAR(8),
   Color VARCHAR(6),
   SizeCode VARCHAR(8),
   QtyPerCTN SMALLINT,
   ShipQty INT,
   NW NUMERIC(6,3),
   GW NUMERIC(6,3),
   NNW NUMERIC(6,3),
   NWPerPcs NUMERIC(5,3),
   CtnNo INT,
   SizeSeq INT
)

--建立tmpe table存放餘箱資料
DECLARE @tempRemainder TABLE (
   RefNo VARCHAR(20),
   CTNQty INT,
   Seq INT,
   Article VARCHAR(8),
   Color VARCHAR(6),
   SizeCode VARCHAR(8),
   QtyPerCTN SMALLINT,
   ShipQty INT,
   NW NUMERIC(6,3),
   GW NUMERIC(6,3),
   NNW NUMERIC(6,3),
   NWPerPcs NUMERIC(5,3),
   SizeSeq INT
)

--將PackingGuide_Detail中的Article撈出來
DECLARE cursor_groupbyarticle CURSOR FOR
	SELECT Distinct a.Article, b.Seq FROM PackingGuide_Detail a, Order_Article b WHERE a.Id = @id AND b.id = @orderid AND a.Article = b.Article ORDER BY b.Seq

--宣告變數: PackingGuide_Detail相關的參數
DECLARE @refno VARCHAR(20),
		@article VARCHAR(8),
		@color VARCHAR(6),
		@sizecode VARCHAR(8),
		@qtyperctn SMALLINT,
		@shipqty INT,
		@nw NUMERIC(6,3),
		@gw NUMERIC(6,3),
		@nnw NUMERIC(5,3)

--宣告變數: 記錄程式中的資料
DECLARE @currentqty INT, --目前數量
		@ctnno INT, --箱號
        @recordctnno INT, --紀錄起始箱號
		@seqcount INT, --排序用編號
		@seq VARCHAR(6), --排序用編號
		@remaindercount INT, --餘箱資料個數
		@ttlshipqty INT, --總件數，寫入PackingList時使用
		@ttlnw NUMERIC(8,3), --總淨重，寫入PackingList時使用
		@ttlgw NUMERIC(8,3), --總毛重，寫入PackingList時使用
		@ttlnnw NUMERIC(7,3), --總淨淨重，寫入PackingList時使用
		@firstsize VARCHAR(8), --第一筆的SizeCode
		@minctn INT, --最少箱數
		@_i INT --計算迴圈用

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
	SELECT @minctn = MIN(ShipQty/QtyPerCTN) FROM PackingGuide_Detail WHERE ID = @id AND Article = @article

	--撈出PackingGuide_Detail資料
	DECLARE cursor_packingguide CURSOR FOR
		SELECT a.RefNo,a.Color,a.SizeCode,a.QtyPerCTN,a.ShipQty,a.NW,a.GW,a.NNW,c.Seq
		FROM PackingGuide_Detail a
		LEFT JOIN Orders b ON b.ID = @orderid
		LEFT JOIN Order_SizeCode c ON c.Id = b.POID AND a.SizeCode = c.SizeCode
		WHERE a.Id = @id AND a.Article = @article
		ORDER BY c.Seq

	OPEN cursor_packingguide
	FETCH NEXT FROM cursor_packingguide INTO @refno, @color, @sizecode, @qtyperctn, @shipqty, @nw, @gw, @nnw, @seq
	SET @firstsize = @sizecode
	WHILE @@FETCH_STATUS = 0
	BEGIN
		IF @firstsize = @sizecode
			BEGIN
				SET @ctnno = @recordctnno
				SET @_i = 0
				WHILE (@_i < @minctn)
				BEGIN
					INSERT INTO @tempPackingList (RefNo,CTNQty,Article,Color,SizeCode,QtyPerCTN,ShipQty,NW,GW,NNW,NWPerPcs,CtnNo,SizeSeq)
						VALUES (@refno, 1, @article, @color, @sizecode, @qtyperctn, @qtyperctn, @nw, @gw-@nw, @nnw, @nw/@qtyperctn, @ctnno, @seq)
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
					INSERT INTO @tempPackingList (RefNo,CTNQty,Article,Color,SizeCode,QtyPerCTN,ShipQty,NW,GW,NNW,NWPerPcs,CtnNo,SizeSeq)
						VALUES (@refno, 0, @article, @color, @sizecode, @qtyperctn, @qtyperctn, @nw, 0, @nnw, @nw/@qtyperctn, @ctnno, @seq)
					SET @_i = @_i + 1
					SET @ctnno = @ctnno + 1
				END
			END

		FETCH NEXT FROM cursor_packingguide INTO @refno, @color, @sizecode, @qtyperctn, @shipqty, @nw, @gw, @nnw, @seq
	END
	CLOSE cursor_packingguide
	
	--整理餘箱資料
	SET @firstsize = ''
	OPEN cursor_packingguide
	FETCH NEXT FROM cursor_packingguide INTO @refno, @color, @sizecode, @qtyperctn, @shipqty, @nw, @gw, @nnw, @seq
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

				IF @firstsize = @sizecode
					BEGIN
						
						INSERT INTO @tempRemainder (RefNo,CTNQty,Article,Color,SizeCode,QtyPerCTN,ShipQty,NW,GW,NNW,NWPerPcs,Seq,SizeSeq)
							VALUES (@refno, 1, @article, @color, @sizecode, @currentqty, @currentqty, (@nw/@qtyperctn)*@currentqty, @gw-@nw, (@nnw/@qtyperctn)*@currentqty, @nw/@qtyperctn, @remaindercount, @seq)
					END
				ELSE
					BEGIN
						INSERT INTO @tempRemainder (RefNo,CTNQty,Article,Color,SizeCode,QtyPerCTN,ShipQty,NW,GW,NNW,NWPerPcs,Seq,SizeSeq)
							VALUES (@refno, 0, @article, @color, @sizecode, @currentqty, @currentqty, (@nw/@qtyperctn)*@currentqty, 0, (@nnw/@qtyperctn)*@currentqty, @nw/@qtyperctn, @remaindercount, @seq)
					END
			END
		FETCH NEXT FROM cursor_packingguide INTO @refno, @color, @sizecode, @qtyperctn, @shipqty, @nw, @gw, @nnw, @seq
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
SELECT @ctnno = MAX(CtnNo) FROM @tempPackingList
--宣告變數
DECLARE @ctnqty INT, --Carton數
		@nwperpcs NUMERIC(5,3) --每件淨重

--將Remainder資料整理進@tempPackingList
DECLARE cursor_tempremainder CURSOR FOR
	SELECT RefNo,CTNQty,Article,Color,SizeCode,QtyPerCTN,ShipQty,NW,GW,NNW,NWPerPcs,SizeSeq FROM @tempRemainder ORDER BY Seq

OPEN cursor_tempremainder
FETCH NEXT FROM cursor_tempremainder INTO @refno, @ctnqty, @article, @color, @sizecode, @qtyperctn, @shipqty, @nw, @gw, @nnw, @nwperpcs, @seq
WHILE @@FETCH_STATUS = 0
BEGIN
	IF @ctnqty = 1
		BEGIN
			SET @ctnno = @ctnno + 1
		END

	INSERT INTO @tempPackingList (RefNo,CTNQty,Article,Color,SizeCode,QtyPerCTN,ShipQty,NW,GW,NNW,NWPerPcs,CtnNo,SizeSeq)
		VALUES (@refno, @ctnqty, @article, @color, @sizecode, @qtyperctn, @shipqty, @nw, @gw, @nnw, @nwperpcs, @ctnno, @seq)
	
	FETCH NEXT FROM cursor_tempremainder INTO @refno, @ctnqty, @article, @color, @sizecode, @qtyperctn, @shipqty, @nw, @gw, @nnw, @nwperpcs, @seq
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
SELECT @ttlnw = SUM(NW), @ttlgw = SUM(GW), @ttlnnw = SUM(NNW), @ttlshipqty = SUM(ShipQty), @seqcount = MAX(CtnNo) FROM @tempPackingList

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
SELECT @havepl = count(ID) FROM PackingList WHERE ID = @id
IF @havepl = 0
	BEGIN --新增PackingList
		INSERT INTO PackingList (ID,Type,FactoryID,OrderID,OrderShipmodeSeq,ShipModeID,BrandID,Dest,CustCDID,CTNQty,ShipQty,NW,GW,NNW,CBM,Remark,Status,AddName,AddDate)
			VALUES (@id, 'B', @factoryid, @orderid,@ordershipmodeseq, @shipmodeid, @brandid, @dest, @custcdid, @seqcount, @ttlshipqty, @ttlnw, @ttlgw, @ttlnnw, @cbm, @remark, 'New', @addname, @adddate)
	END
ELSE
	BEGIN --更新PackingList
		UPDATE PackingList 
		SET FactoryID = @factoryid,
			OrderID = @orderid,
			OrderShipmodeSeq = @ordershipmodeseq,
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
			EditDate = null
		WHERE ID = @id
	END

--PackingList_Detail
DECLARE @cartonsatrtno VARCHAR(6)

SET @seqcount = 0

DECLARE cursor_temppackinglist CURSOR FOR
	SELECT RefNo,CTNStartNo,CTNQty,Article,Color,SizeCode,QtyPerCTN,ShipQty,NW,GW,NNW,NWPerPcs FROM @tempPackingList ORDER BY CtnNo,SizeSeq
OPEN cursor_temppackinglist
FETCH NEXT FROM cursor_temppackinglist INTO @refno, @cartonsatrtno, @ctnqty, @article, @color, @sizecode, @qtyperctn, @shipqty, @nw, @gw, @nnw, @nwperpcs
WHILE @@FETCH_STATUS = 0
BEGIN
	SET @seqcount = @seqcount + 1
	SELECT @seq = REPLICATE('0', 6 - LEN(CONVERT(VARCHAR,@seqcount))) + CONVERT(VARCHAR,@seqcount)
	INSERT INTO PackingList_Detail(ID,OrderID,OrderShipmodeSeq,RefNo,CTNStartNo,CTNEndNo,CTNQty,Article,Color,SizeCode,QtyPerCTN,ShipQty,NW,GW,NNW,NWPerPcs,Seq)
		VALUES (@id, @orderid, @ordershipmodeseq, @refno, @cartonsatrtno, @cartonsatrtno, @ctnqty, @article, @color, @sizecode, @qtyperctn, @shipqty, @nw, @gw, @nnw, @nwperpcs, @seq)
	--將下一筆資料填入變數
	FETCH NEXT FROM cursor_temppackinglist INTO @refno, @cartonsatrtno, @ctnqty, @article, @color, @sizecode, @qtyperctn, @shipqty, @nw, @gw, @nnw, @nwperpcs
END
CLOSE cursor_temppackinglist
DEALLOCATE cursor_temppackinglist

IF @@ERROR <> 0
	ROLLBACK TRANSACTION
ELSE
	COMMIT TRANSACTION", CurrentMaintain["ID"].ToString(), Sci.Env.User.UserID);
                #endregion
            }
            else
            {
                #region 單色單碼
                insertCmd = string.Format(@"--宣告變數: PackingGuide帶入的參數
DECLARE @id VARCHAR(13),
		@factoryid VARCHAR(8),
		@orderid VARCHAR(13),
		@ordershipmodeseq VARCHAR(2),
		@shipmodeid VARCHAR(10),
		@ctnstartno INT,
		@cbm NUMERIC(8,3),
		@remark NVARCHAR(125)
--設定變數值
SET @id = '{0}'
SELECT @factoryid = FactoryID, @orderid = OrderID, @ordershipmodeseq = OrderShipmodeSeq, @shipmodeid = ShipModeID, @ctnstartno = CTNStartNo, @cbm = CBM, @remark = Remark  FROM PackingGuide WHERE Id = @id

--宣告變數: Orders相關的參數
DECLARE @brandid VARCHAR(8),
		@dest VARCHAR(2),
		@custcdid VARCHAR(16)
--設定變數值
SELECT @brandid = BrandID, @dest = Dest, @custcdid = CustCDID FROM Orders WHERE ID = @orderid

--建立tmpe table存放展開後結果
DECLARE @tempPackingList TABLE (
   RefNo VARCHAR(20),
   CTNStartNo VARCHAR(6),
   CTNQty INT,
   Seq VARCHAR(6),
   Article VARCHAR(8),
   Color VARCHAR(6),
   SizeCode VARCHAR(8),
   QtyPerCTN SMALLINT,
   ShipQty INT,
   NW NUMERIC(6,3),
   GW NUMERIC(6,3),
   NNW NUMERIC(6,3),
   NWPerPcs NUMERIC(5,3)
)

--建立tmpe table存放餘箱資料
DECLARE @tempRemainder TABLE (
   RefNo VARCHAR(20),
   CTNQty INT,
   Seq INT,
   Article VARCHAR(8),
   Color VARCHAR(6),
   SizeCode VARCHAR(8),
   QtyPerCTN SMALLINT,
   ShipQty INT,
   NW NUMERIC(6,3),
   GW NUMERIC(6,3),
   NNW NUMERIC(6,3),
   NWPerPcs NUMERIC(5,3)
)

--將PackingGuide_Detail資料存放至Cursor
DECLARE cursor_packguide CURSOR FOR
	SELECT a.RefNo,a.Article,a.Color,a.SizeCode,a.QtyPerCTN,a.ShipQty,a.NW,a.GW,a.NNW 
	FROM PackingGuide_Detail a
	LEFT JOIN Orders b ON b.ID = @orderid
	LEFT JOIN Order_Article c  ON c.Id = @orderid AND a.Article = c.Article
	LEFT JOIN Order_SizeCode d ON d.Id = b.POID AND a.SizeCode = d.SizeCode
	WHERE a.ID = @id ORDER BY c.Seq,d.Seq

--宣告變數: PackingGuide_Detail相關的參數
DECLARE @refno VARCHAR(20),
		@article VARCHAR(8),
		@color VARCHAR(6),
		@sizecode VARCHAR(8),
		@qtyperctn SMALLINT,
		@shipqty INT,
		@nw NUMERIC(6,3),
		@gw NUMERIC(6,3),
		@nnw NUMERIC(5,3)

--宣告變數: 記錄程式中的資料
DECLARE @currentqty INT, --目前數量
		@ctnno INT, --箱號
		@realctnno VARCHAR(6), --寫入Table中的箱號
		@seqcount INT, --排序用編號
		@seq VARCHAR(6), --排序用編號
		@remaindercount INT, --餘箱資料個數
		@ttlshipqty INT, --總件數，寫入PackingList時使用
		@ttlnw NUMERIC(8,3), --總淨重，寫入PackingList時使用
		@ttlgw NUMERIC(8,3), --總毛重，寫入PackingList時使用
		@ttlnnw NUMERIC(7,3) --總淨淨重，寫入PackingList時使用
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
FETCH NEXT FROM cursor_packguide INTO @refno, @article, @color, @sizecode, @qtyperctn, @shipqty, @nw, @gw, @nnw
WHILE @@FETCH_STATUS = 0
BEGIN
	IF @qtyperctn > 0
		BEGIN
			SET @currentqty = @shipqty
			WHILE @currentqty > 0
			BEGIN
				IF @currentqty > @qtyperctn
					BEGIN
						SET @seqcount = @seqcount + 1
						SELECT @seq = REPLICATE('0', 6 - LEN(CONVERT(VARCHAR,@seqcount))) + CONVERT(VARCHAR,@seqcount)
						SELECT @realctnno = CONVERT(VARCHAR,@ctnno)
						INSERT INTO @tempPackingList (RefNo,CTNStartNo,CTNQty,Article,Color,SizeCode,QtyPerCTN,ShipQty,NW,GW,NNW,NWPerPcs,Seq)
							VALUES (@refno, @realctnno, 1, @article, @color, @sizecode, @qtyperctn, @qtyperctn, @nw, @gw, @nnw, @nw/@qtyperctn, @seq)
						SET @ctnno = @ctnno + 1
						SET @ttlnw = @ttlnw + @nw
						SET @ttlgw = @ttlgw + @gw
						SET @ttlnnw = @ttlnnw + @nnw
						SET @ttlshipqty = @ttlshipqty + @qtyperctn
					END
				ELSE
					BEGIN
						SET @remaindercount = @remaindercount + 1
						INSERT INTO @tempRemainder (RefNo,CTNQty,Article,Color,SizeCode,QtyPerCTN,ShipQty,NW,GW,NNW,NWPerPcs,Seq)
							VALUES (@refno, 1, @article, @color, @sizecode, @currentqty, @currentqty, (@nw/@qtyperctn)*@currentqty, ((@nw/@qtyperctn)*@currentqty)+(@gw-@nw), (@nnw/@qtyperctn)*@currentqty, (@nw/@qtyperctn), @remaindercount)
					END

				SET @currentqty = @currentqty - @qtyperctn
			END
		END

	--將下一筆資料填入變數
	FETCH NEXT FROM cursor_packguide INTO @refno, @article, @color, @sizecode, @qtyperctn, @shipqty, @nw, @gw, @nnw
END

--關閉cursor與參數的關聯
CLOSE cursor_packguide
--將cursor物件從記憶體移除
DEALLOCATE cursor_packguide

--將餘箱資料寫入@tempPackingList
--將@tempRemainder資料存放至Cursor
DECLARE cursor_temRemainder CURSOR FOR
	SELECT RefNo,CTNQty,Article,Color,SizeCode,QtyPerCTN,ShipQty,NW,GW,NNW,NWPerPcs FROM @tempRemainder ORDER BY Seq
--宣告變數: 記錄程式中的資料
DECLARE @ctnqty INT, --Carton數
		@nwperpcs NUMERIC(5,3) --每件淨重

OPEN cursor_temRemainder
FETCH NEXT FROM cursor_temRemainder INTO @refno, @ctnqty, @article, @color, @sizecode, @qtyperctn, @shipqty, @nw, @gw, @nnw, @nwperpcs
WHILE @@FETCH_STATUS = 0
BEGIN
	SET @seqcount = @seqcount + 1
	SELECT @seq = REPLICATE('0', 6 - LEN(CONVERT(VARCHAR,@seqcount))) + CONVERT(VARCHAR,@seqcount)
	SELECT @realctnno = CONVERT(VARCHAR,@ctnno)
	INSERT INTO @tempPackingList (RefNo,CTNStartNo,CTNQty,Article,Color,SizeCode,QtyPerCTN,ShipQty,NW,GW,NNW,NWPerPcs,Seq)
		VALUES (@refno, @realctnno, @ctnqty, @article, @color, @sizecode, @qtyperctn, @qtyperctn, @nw, @gw, @nnw, @nwperpcs, @seq)
	SET @ctnno = @ctnno + 1
	SET @ttlnw = @ttlnw + @nw
	SET @ttlgw = @ttlgw + @gw
	SET @ttlnnw = @ttlnnw + @nnw
	SET @ttlshipqty = @ttlshipqty + @qtyperctn

	--將下一筆資料填入變數
	FETCH NEXT FROM cursor_temRemainder INTO @refno, @ctnqty, @article, @color, @sizecode, @qtyperctn, @shipqty, @nw, @gw, @nnw, @nwperpcs
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
SELECT @havepl = count(ID) FROM PackingList WHERE ID = @id
IF @havepl = 0
	BEGIN --新增PackingList
		INSERT INTO PackingList (ID,Type,FactoryID,OrderID,OrderShipmodeSeq,ShipModeID,BrandID,Dest,CustCDID,CTNQty,ShipQty,NW,GW,NNW,CBM,Remark,Status,AddName,AddDate)
			VALUES (@id, 'B', @factoryid, @orderid,@ordershipmodeseq, @shipmodeid, @brandid, @dest, @custcdid, @seqcount, @ttlshipqty, @ttlnw, @ttlgw, @ttlnnw, @cbm, @remark, 'New', @addname, @adddate)
	END
ELSE
	BEGIN --更新PackingList
		UPDATE PackingList 
		SET FactoryID = @factoryid,
			OrderID = @orderid,
			OrderShipmodeSeq = @ordershipmodeseq,
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
			EditDate = null
		WHERE ID = @id
	END

--PackingList_Detail
DECLARE @cartonsatrtno VARCHAR(6)

DECLARE cursor_temPackingList CURSOR FOR
	SELECT RefNo,CTNStartNo,CTNQty,Article,Color,SizeCode,QtyPerCTN,ShipQty,NW,GW,NNW,NWPerPcs,Seq FROM @tempPackingList ORDER BY Seq
OPEN cursor_temPackingList
FETCH NEXT FROM cursor_temPackingList INTO @refno, @cartonsatrtno, @ctnqty, @article, @color, @sizecode, @qtyperctn, @shipqty, @nw, @gw, @nnw, @nwperpcs, @seq
WHILE @@FETCH_STATUS = 0
BEGIN
	INSERT INTO PackingList_Detail(ID,OrderID,OrderShipmodeSeq,RefNo,CTNStartNo,CTNEndNo,CTNQty,Article,Color,SizeCode,QtyPerCTN,ShipQty,NW,GW,NNW,NWPerPcs,Seq)
		VALUES (@id, @orderid, @ordershipmodeseq, @refno, @cartonsatrtno, @cartonsatrtno, @ctnqty, @article, @color, @sizecode, @qtyperctn, @shipqty, @nw, @gw, @nnw, @nwperpcs, @seq)
	--將下一筆資料填入變數
	FETCH NEXT FROM cursor_temPackingList INTO @refno, @cartonsatrtno, @ctnqty, @article, @color, @sizecode, @qtyperctn, @shipqty, @nw, @gw, @nnw, @nwperpcs, @seq
END
CLOSE cursor_temPackingList
DEALLOCATE cursor_temPackingList

IF @@ERROR <> 0
	ROLLBACK TRANSACTION
ELSE
	COMMIT TRANSACTION", CurrentMaintain["ID"].ToString(), Sci.Env.User.UserID);
                #endregion
            }
            #endregion

            DualResult result = DBProxy.Current.Execute(null, insertCmd);
            if (result)
            {
                //存檔成功後，要再呼叫UpdateOrdersCTN, CreateOrderCTNData
                bool prgResult = Prgs.UpdateOrdersCTN(CurrentMaintain["OrderID"].ToString());
                prgResult = Prgs.CreateOrderCTNData(CurrentMaintain["ID"].ToString());

                MessageBox.Show("Switch completed!");
            }
            else
            {
                MessageBox.Show("Switch fail!");
            }
        }
    }
}
