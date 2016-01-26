﻿using System;
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
        private string printPackMethod = "";
        private int orderQty = 0, ttlShipQty = 0;
        public P02(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.DefaultFilter = "MDivisionID = '" + Sci.Env.User.Keyword + "'";
            InsertDetailGridOnDoubleClick = false;
        }

        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["ID"].ToString();
            this.DetailSelectCommand = string.Format(@"select a.ID,a.Refno,a.Article,a.Color,a.SizeCode,a.QtyPerCTN,a.ShipQty,a.NW,a.GW,a.NNW,c.Description
                                                                                       from PackingGuide_Detail a
                                                                                       left join PackingGuide b on a.Id = b.Id
                                                                                       left join LocalItem c on a.RefNo = c.RefNo
                                                                                       left join Orders d on b.OrderID = d.ID
                                                                                       left join Order_Article e on b.OrderID = e.Id and a.Article = e.Article
                                                                                       left join Order_SizeCode f on d.POID = f.Id and a.SizeCode = f.SizeCode
                                                                                       where a.Id ='{0}'
                                                                                       order by e.Seq,f.Seq", masterID);
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
            sqlCmd = string.Format("select StyleID,SeasonID,CustPONo,Qty,CtnType from Orders where ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["OrderID"]));
            if (MyUtility.Check.Seek(sqlCmd, out orderData))
            {
                displayBox2.Value = orderData["StyleID"].ToString();
                displayBox3.Value = orderData["SeasonID"].ToString();
                displayBox4.Value = orderData["CustPONo"].ToString();
                numericBox1.Value = MyUtility.Convert.GetInt(orderData["Qty"]);
                orderQty = MyUtility.Convert.GetInt(orderData["Qty"]);
                comboBox1.SelectedValue = orderData["CtnType"].ToString();
                printPackMethod = orderData["CtnType"].ToString();            
            }
            else
            {
                displayBox2.Value = "";
                displayBox3.Value = "";
                displayBox4.Value = "";
                numericBox1.Value = 0;
                comboBox1.SelectedValue = "";
                numericBox4.Value = 0;
                orderQty = 0;
                ttlShipQty = 0;
                printPackMethod = "";
            }
            sqlCmd = string.Format("select isnull(SUM(ShipQty),0) from PackingGuide_Detail where Id = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["ID"]));
            numericBox4.Value = MyUtility.Convert.GetInt(MyUtility.GetValue.Lookup(sqlCmd));
            ttlShipQty = MyUtility.Convert.GetInt(numericBox4.Value);    

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
            this.button3.Enabled = !this.EditMode && Prgs.GetAuthority(Sci.Env.User.UserID, "P02. Packing Guide", "CanEdit");
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
                        dr["GW"] = dr["NW"];
                    }
                    else
                    {
                        //sql參數
                        System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter();
                        sp1.ParameterName = "@refno";
                        sp1.Value = dr["RefNo"].ToString();

                        IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                        cmds.Add(sp1);

                        string sqlCmd = "select Description,Weight from LocalItem where RefNo = @refno";
                        DataTable LocalItemData;
                        DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out LocalItemData);
                        if (result)
                        {
                            dr["Description"] = LocalItemData.Rows[0]["Description"].ToString();
                            dr["GW"] = MyUtility.Convert.GetDouble(dr["NW"]) + MyUtility.Convert.GetDouble(LocalItemData.Rows[0]["Weight"]);
                        }
                        else
                        {
                            dr["Description"] = "";
                            dr["GW"] = dr["NW"];
                        }
                    }
                    dr.EndEdit();
                }
                #endregion

                #region 輸入Qty/Ctn後要重算N.W.,G.W.,N.N.W.
                if (detailgrid.Columns[e.ColumnIndex].DataPropertyName == col_qtyperctn.DataPropertyName)
                {
                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    //sql參數
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
                    sp4.Value = CurrentMaintain["OrderID"].ToString();

                    IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                    cmds.Add(sp1);
                    cmds.Add(sp2);
                    cmds.Add(sp3);
                    cmds.Add(sp4);

                    string sqlCmd = @"select isnull(li.Weight, 0) as CTNWeight,
                                                                                     isnull(sw.NW, isnull(sw2.NW, 0)) as NW,
                                                                                     isnull(sw.NNW, isnull(sw2.NNW, 0)) as NNW
                                                                          from Orders o
                                                                          left join Style_WeightData sw on sw.StyleUkey = o.StyleUkey and sw.Article = @article and sw.SizeCode = @sizecode
                                                                          left join Style_WeightData sw2 on sw2.StyleUkey = o.StyleUkey and sw2.Article = '----' and sw2.SizeCode = @sizecode
                                                                          left join LocalItem li on li.RefNo = @refno and li.Category = 'CARTON'
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
                        MyUtility.Msg.WarningBox("Sql connection fail!!\r\n"+result.ToString());
                        dr["NW"] = 0;
                        dr["GW"] = 0;
                        dr["NNW"] = 0;
                    }
                    dr.EndEdit();
                }
                #endregion
            };
        }

        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();

            CurrentMaintain["MDivisionID"] = Sci.Env.User.Keyword;
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
                MyUtility.Msg.WarningBox("< SP No. > can not be empty!");
                this.textBox1.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["OrderShipmodeSeq"]))
            {
                MyUtility.Msg.WarningBox("< Seq > can not be empty!");
                this.textBox2.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["ShipModeID"]))
            {
                MyUtility.Msg.WarningBox("< Shipping Mode > can not be empty!");
                this.txtshipmode1.Focus();
                return false;
            }

            //檢查OrderID+Seq不可以重複建立
            if (MyUtility.Check.Seek(string.Format("select ID from PackingGuide where OrderID = '{0}' AND OrderShipmodeSeq = '{1}' AND ID != '{2}'", CurrentMaintain["OrderID"].ToString(), CurrentMaintain["OrderShipmodeSeq"].ToString(), IsDetailInserting ? "" : CurrentMaintain["ID"].ToString())))
            {
                MyUtility.Msg.WarningBox("SP No:" + CurrentMaintain["OrderID"].ToString() + ", Seq:" + CurrentMaintain["OrderShipmodeSeq"].ToString() + " already exist in packing guide, can't be create again!");
                return false;
            }

            //檢查表身不可以沒有資料
            DataRow[] detailData = ((DataTable)detailgridbs.DataSource).Select();
            if (detailData.Length == 0)
            {
                MyUtility.Msg.WarningBox("Detail can't empty!");
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
                    MyUtility.Msg.WarningBox("< Color Way > " + dr["Article"].ToString().Trim() + " < Qty/Ctn > can't empty!");
                    return false;
                }

                if (isEmptyRefNo && !isEmptyShipQty)
                {
                    MyUtility.Msg.WarningBox("< Color Way > " + dr["Article"].ToString().Trim() + " < Ref No. > can't empty!");
                    return false;
                }
            }

            #region 計算Total Cartons & CBM
            //Total Cartons: 單色混碼裝：min(無條件捨去(同一顏色不同Size的訂單件數/每箱件數)) + (1(若其中一個Size有餘數) or 0(完全整除沒有餘數))
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
                        ttlCBM = ttlCBM + MyUtility.Convert.GetDouble(dr["CBM"]) * (MyUtility.Convert.GetInt(dr["ctn"]) + remainder);
                    }
                }
            }
            else
            {
                //Total Cartons: 表身每一列資料的訂單件數/每箱件數無條件進位後加總
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
                    ttlCBM = ttlCBM + MyUtility.Convert.GetDouble(cbm) * ctns;
                }
            }
            #endregion
            CurrentMaintain["CTNQty"] = ttlCTN;
            CurrentMaintain["CBM"] = ttlCBM;

            //GetID
            if (IsDetailInserting)
            {
                string id = MyUtility.GetValue.GetID(MyUtility.GetValue.Lookup("FtyGroup",CurrentMaintain["OrderID"].ToString(),"Orders","ID") + "PG", "PackingGuide", DateTime.Today, 2, "Id", null);
                if (MyUtility.Check.Empty(id))
                {
                    MyUtility.Msg.WarningBox("GetID fail, please try again!");
                    return false;
                }
                CurrentMaintain["ID"] = id;
            }
            return base.ClickSaveBefore();
        }

        protected override bool ClickPrint()
        {
            string minCtnQty = "0";
            //如果是單色混碼包裝，就先算出最少箱數
            if (printPackMethod == "2")
            {
                minCtnQty = MyUtility.GetValue.Lookup(string.Format("select isnull(min(ShipQty/QtyPerCTN),0) from PackingGuide_Detail where Id = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["ID"])));
            }
            string sqlCmd = string.Format(@"select pd.Article,pd.Color,pd.SizeCode,pd.QtyPerCTN,pd.ShipQty,isnull((pd.ShipQty/pd.QtyPerCTN),0) as CtnQty,o.CustCDID,o.StyleID,o.CustPONo,o.Customize1,c.Alias,oq.BuyerDelivery
from PackingGuide p
left join PackingGuide_Detail pd on p.Id = pd.Id
left join Orders o on o.ID = p.OrderID
left join Order_Article oa on oa.id = o.ID and oa.Article = pd.Article
left join Order_SizeCode os on os.Id = o.POID and os.SizeCode = pd.SizeCode
left join Country c on c.ID = o.Dest
left join Order_QtyShip oq on oq.Id = o.ID and oq.Seq = p.OrderShipmodeSeq
where p.Id = '{0}'
order by oa.Seq,os.Seq", MyUtility.Convert.GetString(CurrentMaintain["ID"]));
            DataTable PrintData;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out PrintData);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Query data fail! \r\n"+result.ToString());
                return false;
            }
            if (PrintData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("No data!");
                return false;
            }

            DataTable CtnDim, QtyCtn;
            sqlCmd = string.Format(@"select distinct pd.RefNo, li.Description, STR(li.CtnLength,8,4)+'*'+STR(li.CtnWidth,8,4)+'*'+STR(li.CtnHeight,8,4) as Dimension, li.CtnUnit
from PackingGuide_Detail pd
left join LocalItem li on li.RefNo = pd.RefNo
left join LocalSupp ls on ls.ID = li.LocalSuppid
where pd.ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["ID"]));
            result = DBProxy.Current.Select(null, sqlCmd, out CtnDim);

            sqlCmd = string.Format(@"select isnull(oq.Article,'') as Article,isnull(oq.SizeCode,'') as SizeCode,isnull(oq.Qty,0) as Qty
from Orders o
left join Order_QtyCTN oq on o.ID = oq.Id
left join Order_Article oa on o.ID = oa.id and oq.Article = oa.Article
left join Order_SizeCode os on o.POID = os.Id and oq.SizeCode = os.SizeCode
where o.ID = '{0}'
order by oa.Seq,os.Seq", MyUtility.Convert.GetString(CurrentMaintain["OrderID"]));
            result = DBProxy.Current.Select(null, sqlCmd, out QtyCtn);

            string strXltName = Sci.Env.Cfg.XltPathDir + "Packing_P02.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null) return false;
            MyUtility.Msg.WaitWindows("Starting to excel...");
            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];
            excel.Visible = false;
            
            worksheet.Cells[2, 2] = MyUtility.Check.Empty(PrintData.Rows[0]["BuyerDelivery"]) ? "" : Convert.ToDateTime(PrintData.Rows[0]["BuyerDelivery"]).ToString("d");
            worksheet.Cells[2, 19] = Convert.ToDateTime(DateTime.Today).ToString("d");
            worksheet.Cells[3, 2] = MyUtility.Convert.GetString(PrintData.Rows[0]["CustCDID"]);
            worksheet.Cells[5, 1] = MyUtility.Convert.GetString(CurrentMaintain["OrderID"]);
            worksheet.Cells[5, 2] = MyUtility.Convert.GetString(PrintData.Rows[0]["StyleID"]);
            worksheet.Cells[5, 5] = MyUtility.Convert.GetString(PrintData.Rows[0]["Customize1"]);
            worksheet.Cells[5, 8] = MyUtility.Convert.GetString(PrintData.Rows[0]["CustPONo"]);
            worksheet.Cells[5, 11] = MyUtility.Convert.GetInt(CurrentMaintain["CTNQty"]);
            worksheet.Cells[5, 13] = MyUtility.Convert.GetString(PrintData.Rows[0]["Alias"]);
            worksheet.Cells[5, 17] = orderQty;
            worksheet.Cells[5, 19] = ttlShipQty;
            worksheet.Cells[5, 20] = "=Q5-S5";
            int row = 7, ctnNum = MyUtility.Convert.GetInt(CurrentMaintain["CTNStartNo"]), ttlCtn = 0;

            #region 先算出總共會有幾筆record
            int tmpCtnQty = 0;
            foreach (DataRow dr in PrintData.Rows)
            {
                int ctnQty = (printPackMethod == "2" ? MyUtility.Convert.GetInt(minCtnQty) : MyUtility.Convert.GetInt(dr["CtnQty"]));
                int ctn = ctnQty == 0 ? 0 : (int)Math.Ceiling(MyUtility.Convert.GetDecimal(ctnQty) / 15);
                int ship = MyUtility.Convert.GetInt(dr["QtyPerCTN"]) * ctnQty;
                tmpCtnQty = tmpCtnQty + ctn + (ship >= MyUtility.Convert.GetInt(dr["ShipQty"]) ? 0 : 1);
            }
            if (tmpCtnQty > 258) //範本已先有258 row，不夠的話再新增
            {
                for (int i = 1; i <= tmpCtnQty - 258; i++) //Insert row
                {
                    Microsoft.Office.Interop.Excel.Range rngToCopy = worksheet.get_Range("A8:A8").EntireRow;
                    Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range("A8:A8", Type.Missing).EntireRow;
                    rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown, rngToCopy.Copy(Type.Missing));
                }
            }
            else
            {
                if (tmpCtnQty < 258) //刪除多餘的Row
                {
                    for (int i = 1; i <= 258-tmpCtnQty; i++) //Insert row
                    {
                        Microsoft.Office.Interop.Excel.Range rng = (Microsoft.Office.Interop.Excel.Range)excel.Rows[7, Type.Missing];
                        rng.Select();
                        rng.Delete(Microsoft.Office.Interop.Excel.XlDirection.xlUp);
                    }
                }
            }
            #endregion

            #region 寫入完整箱的資料
            foreach (DataRow dr in PrintData.Rows)
            {
                int ctnQty = (printPackMethod == "2" ? MyUtility.Convert.GetInt(minCtnQty) : MyUtility.Convert.GetInt(dr["CtnQty"]));
                if (!MyUtility.Check.Empty(ctnQty))
                {
                    worksheet.Cells[row, 1] = MyUtility.Convert.GetString(dr["Article"]) + " " + MyUtility.Convert.GetString(dr["Color"]);
                    worksheet.Cells[row, 2] = MyUtility.Convert.GetString(dr["SizeCode"]);
                    worksheet.Cells[row, 3] = MyUtility.Convert.GetInt(dr["QtyPerCTN"]);
                    worksheet.Cells[row, 19] = MyUtility.Convert.GetInt(dr["QtyPerCTN"]) * ctnQty;
                    ttlCtn = 0;

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
            foreach (DataRow dr in PrintData.Rows)
            {
                int ctnQty = (printPackMethod == "2" ? MyUtility.Convert.GetInt(minCtnQty) : MyUtility.Convert.GetInt(dr["CtnQty"]));
                int remain = MyUtility.Convert.GetInt(dr["ShipQty"]) - (MyUtility.Convert.GetInt(dr["QtyPerCTN"]) * ctnQty);
                if (remain > 0)
                {
                    worksheet.Cells[row, 1] = MyUtility.Convert.GetString(dr["Article"]) + " " + MyUtility.Convert.GetString(dr["Color"]);
                    worksheet.Cells[row, 2] = MyUtility.Convert.GetString(dr["SizeCode"]);
                    worksheet.Cells[row, 3] = remain;
                    worksheet.Cells[row, 4] = ctnNum;
                    worksheet.Cells[row, 19] = remain;
                    ctnNum++;
                    row++;
                }
            }
            #endregion

            worksheet.Cells[row, 1] = "Remark: " + MyUtility.Convert.GetString(CurrentMaintain["Remark"]);
            //填Special Instruction
            //先取得Special Instruction總共有幾行
            int startIndex = 0;
            int endIndex = 0;
            int dataRow = 0;
            for (int i = 1; ; i++)
            {
                if (i > 1)
                {
                    startIndex = endIndex + 2;
                }
                if (MyUtility.Convert.GetString(CurrentMaintain["SpecialInstruction"]).IndexOf("\r\n", startIndex) > 0)
                {
                    endIndex = MyUtility.Convert.GetString(CurrentMaintain["SpecialInstruction"]).IndexOf("\r\n", startIndex);
                }
                else
                {
                    dataRow = i + 1;
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
                }
            }
            worksheet.Cells[row, 2] = MyUtility.Convert.GetString(CurrentMaintain["SpecialInstruction"]);

            //Carton Dimension:
            StringBuilder ctnDimension = new StringBuilder();
            foreach (DataRow dr in CtnDim.Rows)
            {
                ctnDimension.Append(string.Format("{0} / {1} / {2} {3}  \r\n", MyUtility.Convert.GetString(dr["RefNo"]), MyUtility.Convert.GetString(dr["Description"]), MyUtility.Convert.GetString(dr["Dimension"]), MyUtility.Convert.GetString(dr["CtnUnit"])));
            }

            foreach (DataRow dr in QtyCtn.Rows)
            {
                if (!MyUtility.Check.Empty(dr["Article"]))
                {
                    ctnDimension.Append(string.Format("{0} -> {1} / {2}, ", MyUtility.Convert.GetString(dr["Article"]), MyUtility.Convert.GetString(dr["SizeCode"]), MyUtility.Convert.GetString(dr["Qty"])));
                }
            }
            row = row + (dataRow > 2 ? dataRow - 1 : 2);
            worksheet.Cells[row, 2] = ctnDimension.Length > 0 ? ctnDimension.ToString().Substring(0,ctnDimension.ToString().Length-2) : "";

            MyUtility.Msg.WaitClear();
            
            excel.Visible = true;
            return base.ClickPrint();
        }

        //控制Grid欄位的可修改性
        private void ControlGridColumn()
        {
            //當Packing Method為SOLID COLOR/ASSORTED SIZE (Order.CTNType = ‘2’)時，欄位Qty/Ctn不可被修改
            if (comboBox1.SelectedIndex != -1 && comboBox1.SelectedValue.ToString() == "2")
            {
                col_qtyperctn.IsEditingReadOnly = true;
                detailgrid.Columns[5].DefaultCellStyle.ForeColor = Color.Black;
            }
            else
            {
                col_qtyperctn.IsEditingReadOnly = false;
                detailgrid.Columns[5].DefaultCellStyle.ForeColor = Color.Red;
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
                        string sqlCmd = string.Format("select Category, LocalOrder, IsForecast from Orders where ID = '{0}' and MDivisionID = '{1}'", textBox1.Text, Sci.Env.User.Keyword);
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
                CurrentMaintain["FactoryID"] = "";
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
                sqlCmd = string.Format("select StyleID,SeasonID,CustPONo,Qty,CtnType,Packing,FtyGroup from Orders where ID = '{0}'", orderID);
                if (MyUtility.Check.Seek(sqlCmd, out orderData))
                {
                    //帶出相關欄位的資料
                    displayBox2.Value = orderData["StyleID"].ToString();
                    displayBox3.Value = orderData["SeasonID"].ToString();
                    displayBox4.Value = orderData["CustPONo"].ToString();
                    numericBox1.Value = MyUtility.Convert.GetInt(orderData["Qty"]);
                    comboBox1.SelectedValue = orderData["CtnType"].ToString();
                    CurrentMaintain["SpecialInstruction"] = orderData["Packing"].ToString();
                    CurrentMaintain["FactoryID"] = orderData["FtyGroup"].ToString();

                    #region 若Order_QtyShip有多筆資料話就跳出視窗讓使者選擇Seq
                    int orderQty = MyUtility.Convert.GetInt(orderData["Qty"]);
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
                                numericBox4.Value = MyUtility.Convert.GetInt(orderQtyShipData[0]["Qty"].ToString());
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
            if (!MyUtility.Check.Empty(orderID) && !MyUtility.Check.Empty(seq))
            {
                string sqlCmd;
                if (comboBox1.SelectedValue.ToString() == "2")
                {
                    sqlCmd = string.Format("select * from Order_QtyCTN where Id = '{0}'", orderID);
                    if (!MyUtility.Check.Seek(sqlCmd))
                    {
                        MyUtility.Msg.WarningBox("No packing data, can't create!!");
                        return;
                    }
                    sqlCmd = string.Format(@"select '' as ID, '' as RefNo, '' as Description, oqd.Article, voc.ColorID as Color, oqd.SizeCode, oqd.Qty as ShipQty, oqc.Qty as QtyPerCTN, os.Seq,
	   sw.NW as NW1, sw.NNW as NNW1, sw2.NW as NW2, sw2.NNW as NNW2,
	   isnull(sw.NW, isnull(sw2.NW, 0))*oqc.Qty as NW,
	   isnull(sw.NW, isnull(sw2.NW, 0))*oqc.Qty as GW,
	   isnull(sw.NNW, isnull(sw2.NNW, 0))*oqc.Qty as NNW 
from Order_QtyShip_Detail oqd
left Join Orders o on o.ID = oqd.Id
left Join Order_QtyCTN oqc on oqc.id = oqd.Id and oqc.Article = oqd.Article and oqc.SizeCode = oqd.SizeCode
left join View_OrderFAColor voc on voc.id = oqd.Id and voc.Article = oqd.Article
left join Style_WeightData sw on sw.StyleUkey = o.StyleUkey and sw.Article = oqd.Article and sw.SizeCode = oqd.SizeCode
left join Style_WeightData sw2 on sw2.StyleUkey = o.StyleUkey and sw2.Article = '----' and sw2.SizeCode = oqd.SizeCode
left join Order_SizeCode os on os.id = o.POID and os.SizeCode = oqd.SizeCode
left join Order_Article oa on oa.id = oqd.Id and oa.Article = oqd.Article
where oqd.ID = '{0}' and oqd.Seq = '{1}'
order by oa.Seq,os.Seq", orderID, seq);
                }
                else
                {
                    sqlCmd = string.Format(@"select '' as ID, '' as RefNo, '' as Description, oqd.Article, voc.ColorID as Color, oqd.SizeCode, oqd.Qty as ShipQty, o.CTNQty as QtyPerCTN, os.Seq,
	   sw.NW as NW1, sw.NNW as NNW1, sw2.NW as NW2, sw2.NNW as NNW2,
	   isnull(sw.NW, isnull(sw2.NW, 0))*o.CTNQty as NW,
	   isnull(sw.NW, isnull(sw2.NW, 0))*o.CTNQty as GW,
	   isnull(sw.NNW, isnull(sw2.NNW, 0))*o.CTNQty as NNW 
from Order_QtyShip_Detail oqd
left Join Orders o on o.ID = oqd.Id
left join View_OrderFAColor voc on voc.id = oqd.Id and voc.Article = oqd.Article
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
                numericBox4.Value = MyUtility.Convert.GetInt(orderQtyShipData[0]["Qty"]);
            }
            //產生表身Grid的資料
            GenDetailData(CurrentMaintain["OrderID"].ToString(), CurrentMaintain["OrderShipmodeSeq"].ToString());
        }

        //Special Instruction
        private void button1_Click(object sender, EventArgs e)
        {
            Sci.Win.Tools.EditMemo callNextForm = new Sci.Win.Tools.EditMemo(CurrentMaintain["SpecialInstruction"].ToString(), "Special Instruction", false, null);
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
            //檢查OrderID+Seq不可以重複建立
            if (MyUtility.Check.Seek(string.Format("select ID from PackingList where OrderID = '{0}' AND OrderShipmodeSeq = '{1}' AND ID != '{2}'", CurrentMaintain["OrderID"].ToString(), CurrentMaintain["OrderShipmodeSeq"].ToString(), CurrentMaintain["ID"].ToString())))
            {
                MyUtility.Msg.WarningBox("SP No:" + CurrentMaintain["OrderID"].ToString() + ", Seq:" + CurrentMaintain["OrderShipmodeSeq"].ToString() + " already exist in packing list, can't be create again!");
                return;
            }

            //檢查訂單狀態：如果已經Pullout Complete出訊息告知使用者且不做任何事
            string lookupReturn = MyUtility.GetValue.Lookup("select PulloutComplete from Orders where ID = '" + CurrentMaintain["OrderID"].ToString() + "'");
            if (lookupReturn == "True")
            {
                MyUtility.Msg.WarningBox("SP# was ship complete!! You can't switch to packing list.");
                return;
            }

            //檢查PackingList狀態：(1)PackingList如果已經Confirm就出訊息告知使用者且不做任事 (2)如果已經有Invoice No就出訊息告知使用者且不做任事
            DataRow seekData;
            string seekCmd = "select Status, INVNo from PackingList where ID = '" + CurrentMaintain["ID"].ToString().Trim() + "'";
            if (MyUtility.Check.Seek(seekCmd, out seekData))
            {
                if (seekData["Status"].ToString() == "Confirmed")
                {
                    MyUtility.Msg.WarningBox("SP# has been confirmed!! You can't switch to packing list.");
                    return;
                }

                if (!MyUtility.Check.Empty(seekData["INVNo"]))
                {
                    MyUtility.Msg.WarningBox("SP# was booking!! You can't switch to packing list.");
                    return;
                }
            }

            //檢查PackingList是否已經有箱子送到Clog，若有，就出訊息告知使用者且不做任事
            seekCmd = "select ID from PackingList_Detail where ID = '" + CurrentMaintain["ID"].ToString() + "' and TransferToClogID != ''";
            if (MyUtility.Check.Seek(seekCmd))
            {
                MyUtility.Msg.WarningBox("SP# has been transfer!! You can't switch to packing list.");
                return;
            }

            #region 組Insert SQL
            string insertCmd;
            if (comboBox1.SelectedIndex != -1 && comboBox1.SelectedValue.ToString() == "2")
            {
                #region 單色混碼
                insertCmd = string.Format(@"--宣告變數: PackingGuide帶入的參數
DECLARE @id VARCHAR(13),
		@mdivisionid VARCHAR(8),
		@factoryid VARCHAR(8),
		@orderid VARCHAR(13),
		@ordershipmodeseq VARCHAR(2),
		@shipmodeid VARCHAR(10),
		@ctnstartno INT,
		@cbm NUMERIC(8,3),
		@remark NVARCHAR(125)
--設定變數值
SET @id = '{0}'
SELECT @mdivisionid = MDivisionID, @factoryid = FactoryID, @orderid = OrderID, @ordershipmodeseq = OrderShipmodeSeq, @shipmodeid = ShipModeID, @ctnstartno = CTNStartNo, @cbm = CBM, @remark = Remark  FROM PackingGuide WHERE Id = @id

--宣告變數: Orders相關的參數
DECLARE @brandid VARCHAR(8),
		@dest VARCHAR(2),
		@custcdid VARCHAR(16)
--設定變數值
SELECT @brandid = BrandID, @dest = Dest, @custcdid = CustCDID FROM Orders WHERE ID = @orderid

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
   NW NUMERIC(6,3),
   GW NUMERIC(6,3),
   NNW NUMERIC(6,3),
   NWPerPcs NUMERIC(5,3),
   CtnNo INT,
   SizeSeq INT
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
DECLARE @refno VARCHAR(21),
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
SELECT @havepl = count(ID) FROM PackingList WHERE ID = @id
IF @havepl = 0
	BEGIN --新增PackingList
		INSERT INTO PackingList (ID,Type,MDivisionID,FactoryID,ShipModeID,BrandID,Dest,CustCDID,CTNQty,ShipQty,NW,GW,NNW,CBM,Remark,Status,AddName,AddDate)
			VALUES (@id, 'B', @mdivisionid, @factoryid, @shipmodeid, @brandid, @dest, @custcdid, @seqcount, @ttlshipqty, @ttlnw, @ttlgw, @ttlnnw, @cbm, @remark, 'New', @addname, @adddate)
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
		@mdivisionid VARCHAR(8),
		@factoryid VARCHAR(8),
		@orderid VARCHAR(13),
		@ordershipmodeseq VARCHAR(2),
		@shipmodeid VARCHAR(10),
		@ctnstartno INT,
		@cbm NUMERIC(8,3),
		@remark NVARCHAR(125)
--設定變數值
SET @id = '{0}'
SELECT @mdivisionid = MDivisionID, @factoryid = FactoryID, @orderid = OrderID, @ordershipmodeseq = OrderShipmodeSeq, @shipmodeid = ShipModeID, @ctnstartno = CTNStartNo, @cbm = CBM, @remark = Remark  FROM PackingGuide WHERE Id = @id

--宣告變數: Orders相關的參數
DECLARE @brandid VARCHAR(8),
		@dest VARCHAR(2),
		@custcdid VARCHAR(16)
--設定變數值
SELECT @brandid = BrandID, @dest = Dest, @custcdid = CustCDID FROM Orders WHERE ID = @orderid

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
   NW NUMERIC(6,3),
   GW NUMERIC(6,3),
   NNW NUMERIC(6,3),
   NWPerPcs NUMERIC(5,3)
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
DECLARE @refno VARCHAR(21),
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
		INSERT INTO PackingList (ID,Type,MDivisionID,FactoryID,ShipModeID,BrandID,Dest,CustCDID,CTNQty,ShipQty,NW,GW,NNW,CBM,Remark,Status,AddName,AddDate)
			VALUES (@id, 'B', @mdivisionid, @factoryid, @shipmodeid, @brandid, @dest, @custcdid, @seqcount, @ttlshipqty, @ttlnw, @ttlgw, @ttlnnw, @cbm, @remark, 'New', @addname, @adddate)
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

                MyUtility.Msg.InfoBox("Switch completed!");
            }
            else
            {
                MyUtility.Msg.WarningBox("Switch fail!");
            }
        }

        //Packing Method
        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ControlGridColumn();
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
                        }
                    }
                    else
                    {
                        MyUtility.Msg.WarningBox("ShipMode is incorrect!");
                        txtshipmode1.SelectedValue = "";
                    }
                }
            }
        }
    }
}
