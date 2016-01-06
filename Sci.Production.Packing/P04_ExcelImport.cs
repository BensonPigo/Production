using System;
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
    public partial class P04_ExcelImport : Sci.Win.Subs.Base
    {
        DataTable grid2Data= new DataTable();
        DataTable detailData;

        public P04_ExcelImport(DataTable DetailData)
        {
            InitializeComponent();
            detailData = DetailData;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            DataTable ExcelFile = new DataTable();
            ExcelFile.Columns.Add("Filename", typeof(String));
            ExcelFile.Columns.Add("Status", typeof(String));
            ExcelFile.Columns.Add("FullFileName", typeof(String));

            listControlBindingSource1.DataSource = ExcelFile;
            grid1.DataSource = listControlBindingSource1;
            grid1.IsEditingReadOnly = true;
            Helper.Controls.Grid.Generator(this.grid1)
                .Text("Filename", header: "File Name", width: Widths.AnsiChars(15))
                .Text("Status", header: "Status", width: Widths.AnsiChars(100));

            //取Grid結構
            //string sqlCmd = "select SPACE(13) as OrderID, null as BuyerDelivery,SPACE(10) as ShipmodeID,SPACE(8) as Article,SPACE(6) as ColorID,SPACE(8) as SizeCode,0.0 as Qty,SPACE(100) as ErrMsg";
            //DualResult result = DBProxy.Current.Select(null, sqlCmd, out grid2Data);
            grid2Data.Columns.Add("OrderID", typeof(String));
            grid2Data.Columns.Add("BuyerDelivery", typeof(DateTime));
            grid2Data.Columns.Add("ShipmodeID", typeof(String));
            grid2Data.Columns.Add("Article", typeof(String));
            grid2Data.Columns.Add("ColorID", typeof(String));
            grid2Data.Columns.Add("SizeCode", typeof(String));
            grid2Data.Columns.Add("Qty", typeof(Int32));
            grid2Data.Columns.Add("ErrMsg", typeof(String));

            listControlBindingSource2.DataSource = grid2Data;
            grid2.DataSource = listControlBindingSource2;
            grid2.IsEditingReadOnly = true;
            Helper.Controls.Grid.Generator(this.grid2)
                .Text("OrderID", header: "SP No.", width: Widths.AnsiChars(13))
                .Date("BuyerDelivery", header: "Buyer Delivery")
                .Text("ShipmodeID", header: "Ship Mode", width: Widths.AnsiChars(10))
                .Text("Article", header: "Color Way", width: Widths.AnsiChars(8))
                .Text("ColorID", header: "Color", width: Widths.AnsiChars(6))
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(8))
                .Numeric("Qty", header: "Qty")
                .Text("ErrMsg", header: "Error Message", width: Widths.AnsiChars(100));

            for (int i = 0; i < grid2.ColumnCount; i++)
            {
                grid2.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        //Add Excel
        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Excel files (*.xlsx)|*.xlsx";
            if (openFileDialog1.ShowDialog() == DialogResult.OK) //開窗且有選擇檔案
            {
                DataRow dr = ((DataTable)listControlBindingSource1.DataSource).NewRow();
                dr["Filename"] = openFileDialog1.SafeFileName;
                dr["Status"] = "";
                dr["FullFileName"] = openFileDialog1.FileName;
                ((DataTable)listControlBindingSource1.DataSource).Rows.Add(dr);
                listControlBindingSource1.MoveLast();
            }
        }

        //Remove Excel
        private void button2_Click(object sender, EventArgs e)
        {
            if (listControlBindingSource1.Position != -1)
            {
                listControlBindingSource1.RemoveCurrent();
            }
        }

        //Check & Import
        private void button3_Click(object sender, EventArgs e)
        {
            #region 判斷第一個Grid是否有資料
            if (listControlBindingSource1.Count <= 0)
            {
                MyUtility.Msg.WarningBox("No excel data!!");
                return;
            }
            #endregion 

            //清空Grid2資料
            if (grid2Data != null)
            {
                grid2Data.Clear();
            }
            grid2.SuspendLayout();
            #region 檢查1. Grid中的檔案是否存在，不存在時顯示於status欄位；2. Grid中的檔案都可以正常開啟，無法開啟時顯示於status欄位；3.檢查開啟的excel檔存在必要的欄位，將不存在欄位顯示於status。當檢查都沒問題時，就將資料寫入第2個Grid
            foreach (DataRow dr in ((DataTable)listControlBindingSource1.DataSource).Rows)
            {
                if (!MyUtility.Check.Empty(dr["Filename"]))
                {
                    if (!System.IO.File.Exists(MyUtility.Convert.GetString(dr["FullFileName"])))
                    {
                        dr["Status"] = string.Format("Excel file not found < {0} >.", MyUtility.Convert.GetString(dr["Filename"]));
                    }
                    else
                    {
                        Microsoft.Office.Interop.Excel.Application excel;
                        try
                        {
                            excel = new Microsoft.Office.Interop.Excel.Application();
                        }
                        catch (Exception ex)
                        {
                            dr["Status"] = string.Format("Not able to open excel file < {0} >.", MyUtility.Convert.GetString(dr["Filename"]));
                            continue;
                        }

                        excel.Workbooks.Open(MyUtility.Convert.GetString(dr["FullFileName"]));
                        excel.Visible = false;
                        Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

                        //檢查Excel格式
                        Microsoft.Office.Interop.Excel.Range range = worksheet.Range[String.Format("A{0}:G{0}", 1)];
                        object[,] objCellArray = range.Value;
                        string sp = (string)MyUtility.Excel.GetExcelCellValue(objCellArray[1, 1], "C");
                        string delivery = (string)MyUtility.Excel.GetExcelCellValue(objCellArray[1, 2], "C");
                        string shipmode = (string)MyUtility.Excel.GetExcelCellValue(objCellArray[1, 3], "C");
                        string article = (string)MyUtility.Excel.GetExcelCellValue(objCellArray[1, 4], "C");
                        string size = (string)MyUtility.Excel.GetExcelCellValue(objCellArray[1, 5], "C");
                        string qty = (string)MyUtility.Excel.GetExcelCellValue(objCellArray[1, 6], "C");

                        if (sp.ToUpper() != "SP NO." || delivery.ToUpper() != "BUYER DELIVERY" || shipmode.ToUpper() != "SHIP MODE" || article.ToUpper() != "COLOR WAY" || size.ToUpper() != "SIZE" || qty.ToUpper() != "QTY")
                        {
                            #region 將不存在欄位顯示於status
                            StringBuilder columnName = new StringBuilder();
                            if (sp.ToUpper() != "SP NO.")
                            {
                                columnName.Append("< SP No. >, ");
                            }
                            if (delivery.ToUpper() != "BUYER DELIVERY")
                            {
                                columnName.Append("< Buyer Delivery >, ");
                            }
                            if (shipmode.ToUpper() != "SHIP MODE")
                            {
                                columnName.Append("< Ship Mode >, ");
                            }
                            if (article.ToUpper() != "COLOR WAY")
                            {
                                columnName.Append("< Color Way >, ");
                            }
                            if (size.ToUpper() != "SIZE")
                            {
                                columnName.Append("< Size >, ");
                            }
                            if (qty.ToUpper() != "QTY")
                            {
                                columnName.Append("< Qty >, ");
                            }
                            dr["Status"] = columnName.ToString().Substring(0, columnName.ToString().Length - 2) + "column not found in the excel.";
                            #endregion
                        }
                        else
                        {
                            int intRowsCount = worksheet.UsedRange.Rows.Count;
                            int intColumnsCount = worksheet.UsedRange.Columns.Count;
                            int intRowsStart = 2;
                            int intRowsRead = intRowsStart - 1;

                            while (intRowsRead < intRowsCount)
                            {
                                intRowsRead++;

                                range = worksheet.Range[String.Format("A{0}:G{0}", intRowsRead)];
                                objCellArray = range.Value;

                                DataRow newRow = grid2Data.NewRow();
                                newRow["OrderID"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 1], "C");
                                newRow["BuyerDelivery"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 2], "D");
                                newRow["ShipmodeID"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 3], "C");
                                newRow["Article"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 4], "C");
                                newRow["ColorID"] = MyUtility.GetValue.Lookup(string.Format("select ColorID from [dbo].[View_OrderFAColor] where ID = '{0}' and Article = '{1}'", MyUtility.Convert.GetString(newRow["OrderID"]), MyUtility.Convert.GetString(newRow["Article"])));
                                newRow["SizeCode"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 5], "C");
                                newRow["Qty"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 6], "N");

                                grid2Data.Rows.Add(newRow);
                            }

                            dr["Status"] = "Check & Import Completed.";
                        }

                        excel.Workbooks.Close();
                        excel.Quit();
                        excel = null;
                    }
                }
            }
            #endregion

            grid2.ResumeLayout();
        }

        //Write in
        private void button4_Click(object sender, EventArgs e)
        {
            DataTable tmpPackData;
            try
            {
                MyUtility.Tool.ProcessWithDatatable((DataTable)listControlBindingSource2.DataSource, "OrderID,BuyerDelivery,ShipmodeID,Article,ColorID,SizeCode,Qty", @"select distinct a.*,o.ID,oq.Seq,oqd.Article as oArticle,oqd.SizeCode as oSizeCode,o.StyleID,o.CustPONo,o.Category
from #tmp a
left join Orders o on o.ID = a.OrderID
left join Order_QtyShip oq on oq.Id = o.ID and oq.BuyerDelivery = a.BuyerDelivery and oq.ShipmodeID = a.ShipmodeID
left join Order_QtyShip_Detail oqd on oqd.Id = oq.Id and oqd.Seq = oq.Seq and oqd.Article = a.Article and oqd.SizeCode = a.SizeCode", out tmpPackData);
            }
            catch (Exception ex)
            {
                MyUtility.Msg.ErrorBox("Process error.\r\n" + ex.ToString());
                return;
            }

            bool allPass = true;
            int count = -1;
            foreach (DataRow dr in ((DataTable)listControlBindingSource2.DataSource).Rows)
            {
                count++;
                DataRow[] findDR = tmpPackData.Select(string.Format("OrderID = '{0}' and BuyerDelivery = {1} and ShipmodeID = '{2}' and Article = '{3}' and SizeCode = '{4}' and Qty = {5}", MyUtility.Convert.GetString(dr["OrderID"]), MyUtility.Check.Empty(dr["BuyerDelivery"]) ? "null" : "'" + Convert.ToDateTime(dr["BuyerDelivery"]).ToString("d") + "'", MyUtility.Convert.GetString(dr["ShipmodeID"]), MyUtility.Convert.GetString(dr["Article"]), MyUtility.Convert.GetString(dr["SizeCode"]), MyUtility.Convert.GetString(dr["Qty"])));
                if (findDR.Length > 0)
                {
                    if (MyUtility.Check.Empty(findDR[0]["ID"]))
                    {
                        dr["ErrMsg"] = "< SP No. > is not exist!";
                        grid2.Rows[count].DefaultCellStyle.BackColor = Color.FromArgb(255, 192, 203);
                        allPass = false;
                        continue;
                    }
                    if (MyUtility.Convert.GetString(findDR[0]["Category"]) != "S")
                    {
                        dr["ErrMsg"] = "< SP No. > is not Sample!";
                        grid2.Rows[count].DefaultCellStyle.BackColor = Color.FromArgb(255, 192, 203);
                        allPass = false;
                        continue;
                    }

                    if (MyUtility.Check.Empty(findDR[0]["oArticle"]) || MyUtility.Check.Empty(findDR[0]["oSizeCode"]))
                    {
                        dr["ErrMsg"] = "< Color Way> or < Size > is not exist to the < SP No. >!";
                        grid2.Rows[count].DefaultCellStyle.BackColor = Color.FromArgb(255, 192, 203);
                        allPass = false;
                        continue;
                    }

                    DataRow insertRow = detailData.NewRow();
                    insertRow["OrderID"] = dr["OrderID"];
                    insertRow["OrderShipmodeSeq"] = findDR[0]["Seq"];
                    insertRow["StyleID"] = findDR[0]["StyleID"];
                    insertRow["CustPONo"] = findDR[0]["CustPONo"];
                    insertRow["Article"] = dr["Article"];
                    insertRow["Color"] = dr["ColorID"];
                    insertRow["SizeCode"] = dr["SizeCode"];
                    insertRow["ShipQty"] = dr["Qty"];
                    detailData.Rows.Add(insertRow);
                    grid2.Rows[count].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 255);
                }
                else
                {
                    dr["ErrMsg"] = "< SP No. > is not exist!";
                    grid2.Rows[count].DefaultCellStyle.BackColor = Color.FromArgb(255,192,203);
                    allPass = false;
                }
            }

            if (allPass)
            {
                MyUtility.Msg.InfoBox("Write in completed!!");
                DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            else
            {
                MyUtility.Msg.WarningBox("Write in have errors!!");
            }
        }


    }
}
