﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.Packing
{
    public partial class P09_StartToScan : Sci.Win.Subs.Base
    {
        P09_IDX_CTRL IDX = new P09_IDX_CTRL();
        DataRow MasterDR;
        public P09_StartToScan(DataRow MasterDataRow)
        {
            InitializeComponent();
            MasterDR = MasterDataRow;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            //Grid設定
            this.gridDetail.IsEditingReadOnly = true;
            this.gridDetail.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.gridDetail)
                .Text("Article", header: "Colorway", width: Widths.AnsiChars(13))
                .Text("Color", header: "Color", width: Widths.AnsiChars(10))
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(10))
                .Numeric("QtyPerCTN", header: "PC/CTN", width: Widths.AnsiChars(15))
                .Numeric("ScanQty", header: "PC/Ctn Scanned", width: Widths.AnsiChars(20))
                .Text("Barcode", header: "Ref. Barcode", width: Widths.AnsiChars(30));

            //按Header沒有排序功能
            for (int i = 0; i < this.gridDetail.ColumnCount; i++)
            {
                this.gridDetail.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            //設定Grid的Back Color
            gridDetail.RowsAdded += (s, e) =>
            {
                DataTable dtData = (DataTable)listControlBindingSource1.DataSource;
                for (int i = 0; i < e.RowCount; i++)
                {
                    if (MyUtility.Convert.GetInt(dtData.Rows[i]["QtyPerCTN"]) > MyUtility.Convert.GetInt(dtData.Rows[i]["ScanQty"]))
                    {
                        gridDetail.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(255, 192, 203);
                    }
                    else
                    {
                        gridDetail.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 255);
                    }
                }
            };

            gridDetail.RowValidated += (s, e) =>
                {
                    DataTable dtData = (DataTable)listControlBindingSource1.DataSource;
                    if (MyUtility.Convert.GetInt(dtData.Rows[e.RowIndex]["QtyPerCTN"]) > MyUtility.Convert.GetInt(dtData.Rows[e.RowIndex]["ScanQty"]))
                    {
                        gridDetail.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(255, 192, 203);
                    }
                    else
                    {
                        gridDetail.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 255);
                    }
                };

            gridDetail.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            gridDetail.DefaultCellStyle.ForeColor = System.Drawing.Color.Blue;

            //撈Grid資料
            string sqlCmd = string.Format(@"select ID,OrderID,CTNStartNo,Article,Color,SizeCode,QtyPerCTN,ScanQty,Barcode,ShipQty
from PackingList_Detail WITH (NOLOCK) 
where ID = '{0}'
and CTNStartNo = '{1}'
order by Seq", MyUtility.Convert.GetString(MasterDR["ID"]), MyUtility.Convert.GetString(MasterDR["CTNStartNo"]));
            DataTable gridData;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out gridData);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Query data fail!!\r\n" + result.ToString());
            }
            listControlBindingSource1.DataSource = gridData;
            if (gridData != null)
            {
                object obj = gridData.Compute("SUM(QtyPerCTN)", "");
                labelTtlPCCTN1.Text = MyUtility.Convert.GetString(obj);
                labelTtlPCCTNScanned1.Text = "0";
            }
            else
            {
                labelTtlPCCTN1.Text = "0";
                labelTtlPCCTNScanned1.Text = "0";
            }
        }

        //改變Grid's Row時，表頭的資訊要跟著改變
        private void gridDetail_RowSelecting(object sender, Ict.Win.UI.DataGridViewRowSelectingEventArgs e)
        {
            DataRow dr = gridDetail.GetDataRow(e.RowIndex);
            displayRefBarcode.Value = MyUtility.Convert.GetString(dr["Barcode"]);
            labelID.Text = MyUtility.Convert.GetString(dr["ID"]);
            labelArticle.Text = MyUtility.Convert.GetString(dr["Article"]);
            labelColor1.Text = MyUtility.Convert.GetString(dr["Color"]);
            labelSizeCode.Text = MyUtility.Convert.GetString(dr["SizeCode"]);
            labelCTNStartNo.Text = MyUtility.Convert.GetString(dr["CTNStartNo"]);
            labelOrderID.Text = MyUtility.Convert.GetString(dr["OrderID"]);
            btnReset.Enabled = !MyUtility.Check.Empty(dr["Barcode"]);
        }

        //Complete
        private void btnComplete_Click(object sender, EventArgs e)
        {
            if (labelTtlPCCTN1.Text != labelTtlPCCTNScanned1.Text)
            {
                MyUtility.Msg.WarningBox("It's not complete!!");
                return;
            }

            IList<string> updateCmds = new List<string>();
            StringBuilder orderID = new StringBuilder();
            StringBuilder article = new StringBuilder();
            StringBuilder color = new StringBuilder();
            StringBuilder sizeCode = new StringBuilder();
            StringBuilder qtyPerCTN = new StringBuilder();
            StringBuilder scanQty = new StringBuilder();
            StringBuilder barcode = new StringBuilder();
            
            foreach (DataRowView dr in listControlBindingSource1)
            {
                updateCmds.Add(string.Format(@"update PackingList_Detail set Barcode = '{0}', ScanQty = {1}, ScanEditDate = GETDATE() where ID = '{2}' and OrderID = '{3}' and CTNStartNo = '{4}' and Article = '{5}' and SizeCode = '{6}';",
                    MyUtility.Convert.GetString(dr["Barcode"]), MyUtility.Convert.GetString(dr["ScanQty"]), MyUtility.Convert.GetString(dr["ID"]), MyUtility.Convert.GetString(dr["OrderID"]), MyUtility.Convert.GetString(dr["CTNStartNo"]), MyUtility.Convert.GetString(dr["Article"]), MyUtility.Convert.GetString(dr["SizeCode"])));
                orderID.Append(MyUtility.Convert.GetString(dr["OrderID"])+"/");
                article.Append(MyUtility.Convert.GetString(dr["Article"]) + "/");
                color.Append(MyUtility.Convert.GetString(dr["Color"]) + "/");
                sizeCode.Append(MyUtility.Convert.GetString(dr["SizeCode"]) + "/");
                qtyPerCTN.Append(MyUtility.Convert.GetString(dr["QtyPerCTN"]) + "/");
                scanQty.Append(MyUtility.Convert.GetString(dr["ScanQty"]) + "/");
                barcode.Append(MyUtility.Convert.GetString(dr["Barcode"]) + "/");
            }

            DualResult result = DBProxy.Current.Executes(null, updateCmds);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Update data fail!! Pls try again.\r\n" + result.ToString());
                return;
            }

            MasterDR["OrderID"] = orderID.ToString().Substring(0, orderID.ToString().Length - 1);
            MasterDR["Article"] = article.ToString().Substring(0, article.ToString().Length - 1);
            MasterDR["Color"] = color.ToString().Substring(0, color.ToString().Length - 1);
            MasterDR["SizeCode"] = sizeCode.ToString().Substring(0, sizeCode.ToString().Length - 1);
            MasterDR["QtyPerCTN"] = qtyPerCTN.ToString().Substring(0, qtyPerCTN.ToString().Length - 1);
            MasterDR["ScanQty"] = scanQty.ToString().Substring(0, scanQty.ToString().Length - 1);
            MasterDR["Barcode"] = barcode.ToString().Substring(0, barcode.ToString().Length - 1);
            MasterDR["NotYetScan"] = "0";
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        //Cancel
        private void btnCancel_Click(object sender, EventArgs e)
        {
            //問是否要Cancel，確定才繼續往下做
            DialogResult buttonResult = MyUtility.Msg.WarningBox("Are you sure to cancel this scanning?", "Warning", MessageBoxButtons.YesNo);
            if (buttonResult == System.Windows.Forms.DialogResult.No)
            {
                return;
            }
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        //Reset
        private void btnReset_Click(object sender, EventArgs e)
        {
            //問是否要清空Ref. Barcode值，確定才繼續往下做
            DialogResult buttonResult = MyUtility.Msg.WarningBox("Are you sure to reset ref. barcode?", "Warning", MessageBoxButtons.YesNo);
            if (buttonResult == System.Windows.Forms.DialogResult.No)
            {
                return;
            }
            DataRow dr = gridDetail.GetDataRow(gridDetail.GetSelectedRowIndex());
            dr["Barcode"] = "";
            dr["ScanQty"] = 0;
            displayRefBarcode.Value = "";
            object obj = ((DataTable)listControlBindingSource1.DataSource).Compute("SUM(ScanQty)", "");
            labelTtlPCCTNScanned1.Text = MyUtility.Convert.GetString(obj);
        }

        private void txtScanBarcode_Validating(object sender, CancelEventArgs e)
        {
            if (!MyUtility.Check.Empty(txtScanBarcode.Text))
            {
                int pos = listControlBindingSource1.Find("Barcode", txtScanBarcode.Text);
                if (pos >= 0)
                {
                    listControlBindingSource1.Position = pos;
                    DataRow dr = gridDetail.GetDataRow(gridDetail.GetSelectedRowIndex());
                    if (MyUtility.Convert.GetInt(dr["ScanQty"]) >= MyUtility.Convert.GetInt(dr["QtyPerCTN"]))
                    {
                        MyUtility.Msg.WarningBox("PC/Ctn Scanned exceed PC/CTN, can't scanned!!");
                        IDX.IdxCall(254, "a:" + txtScanBarcode.Text.Trim(), ("a:" + txtScanBarcode.Text.Trim()).Length);
                        txtScanBarcode.Text = "";
                        e.Cancel = true;
                        return;
                    }
                    else
                    {
                        Counter(dr);
                        e.Cancel = true;
                        return;
                    }
                }
                else
                {
                    //取結構
                    string sqlCmd = "select OrderID,Article,Color,SizeCode from PackingList_Detail WITH (NOLOCK) where 1=0";
                    DataTable SelectItemData;
                    DualResult result = DBProxy.Current.Select(null, sqlCmd, out SelectItemData);
                    if (!result)
                    {
                        MyUtility.Msg.WarningBox("Query structure fail!! Pls scan again.\r\n"+result.ToString());
                        IDX.IdxCall(254, "a:" + txtScanBarcode.Text.Trim(), ("a:" + txtScanBarcode.Text.Trim()).Length);
                        txtScanBarcode.Text = "";
                        e.Cancel = true;
                        return;
                    }

                    int noBarcodeRecCount = 0;
                    foreach (DataRowView dr in listControlBindingSource1)
                    {
                        if (MyUtility.Check.Empty(dr["Barcode"]))
                        {
                            noBarcodeRecCount++;
                            DataRow newRow = SelectItemData.NewRow();
                            newRow["OrderID"] = dr["OrderID"];
                            newRow["Article"] = dr["Article"];
                            newRow["Color"] = dr["Color"];
                            newRow["SizeCode"] = dr["SizeCode"];
                            SelectItemData.Rows.Add(newRow);
                        }
                    }

                    if (noBarcodeRecCount == 0)
                    {
                        MyUtility.Msg.WarningBox("Wrong barcode, please check barcode again!!");
                        IDX.IdxCall(254, "a:" + txtScanBarcode.Text.Trim(), ("a:" + txtScanBarcode.Text.Trim()).Length);
                        txtScanBarcode.Text = "";
                        e.Cancel = true;
                        return;
                    }
                    else if (noBarcodeRecCount == 1)
                    {
                        //如果只剩下一筆資料還沒有Barcode，就直接將指標移至這筆資料
                        int newPos = listControlBindingSource1.Find("Barcode", "");
                        listControlBindingSource1.Position = newPos;
                        DataRow dr = gridDetail.GetDataRow(newPos);
                        IDX.IdxCall(254, "A:" + txtScanBarcode.Text.Trim() + "=" + dr["QtyPerCtn"].ToString().Trim()
                                       , ("A:" + txtScanBarcode.Text.Trim() + "=" + dr["QtyPerCtn"].ToString().Trim()).Length);
                        dr["Barcode"] = txtScanBarcode.Text;
                        displayRefBarcode.Value = txtScanBarcode.Text;
                        Counter(dr);
                        e.Cancel = true;
                        return;
                    }
                    else
                    {
                        Sci.Win.Tools.SelectItem item = new Win.Tools.SelectItem(SelectItemData, "OrderID,Article,Color,SizeCode", "13,8,6,8", "", headercaptions: "SP#,Colorway,Color,Size");
                        DialogResult returnResult = item.ShowDialog();
                        if (returnResult == DialogResult.Cancel) {return;}
                        IList<DataRow> selectedData = item.GetSelecteds();
                        int newPos = -1;
                        bool hadFound = false;
                        foreach (DataRowView dr in listControlBindingSource1)
                        {
                            newPos++;
                            if (MyUtility.Convert.GetString(dr["OrderID"]) == MyUtility.Convert.GetString(selectedData[0]["OrderID"]) && MyUtility.Convert.GetString(dr["Article"]) == MyUtility.Convert.GetString(selectedData[0]["Article"]) && MyUtility.Convert.GetString(dr["SizeCode"]) == MyUtility.Convert.GetString(selectedData[0]["SizeCode"]))
                            {
                                hadFound = true;
                                break;
                            }
                        }
                        if (hadFound)
                        {
                            listControlBindingSource1.Position = newPos;
                            DataRow dr = gridDetail.GetDataRow(newPos);
                            IDX.IdxCall(254, "A:" + txtScanBarcode.Text.Trim() + "=" + dr["QtyPerCtn"].ToString().Trim()
                                           , ("A:" + txtScanBarcode.Text.Trim() + "=" + dr["QtyPerCtn"].ToString().Trim()).Length);
                            dr["Barcode"] = txtScanBarcode.Text;
                            displayRefBarcode.Value = txtScanBarcode.Text;
                            Counter(dr);
                            e.Cancel = true;
                            return;
                        }
                        else
                        {
                            MyUtility.Msg.WarningBox("Selected data not found! Please scan again.");
                            IDX.IdxCall(254, "a:" + txtScanBarcode.Text.Trim(), ("a:" + txtScanBarcode.Text.Trim()).Length);
                            txtScanBarcode.Text = "";
                            e.Cancel = true;
                            return;
                        }
                    }
                }
            }
        }

        private void Counter(DataRow dr)
        {
            dr["ScanQty"] = MyUtility.Convert.GetInt(dr["ScanQty"]) + 1;
            labelTtlPCCTNScanned1.Text = MyUtility.Convert.GetString(MyUtility.Convert.GetInt(labelTtlPCCTNScanned1.Text) + 1);
            txtScanBarcode.Text = "";
        }

        private void displayRefBarcode_TextChanged(object sender, EventArgs e)
        {
            btnReset.Enabled = !displayRefBarcode.Value.ToString().Empty();  
        }
    }
}
