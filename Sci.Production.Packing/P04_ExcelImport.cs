using System;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;

namespace Sci.Production.Packing
{
    /// <summary>
    /// Packing_P04_ExcelImport
    /// </summary>
    public partial class P04_ExcelImport : Win.Subs.Base
    {
        private DataTable grid2Data = new DataTable();
        private DataTable detailData;
        private string mBrandID;
        private string mShipModeID;

        /// <summary>
        /// P04_ExcelImport
        /// </summary>
        /// <param name="detailData">DetailData</param>
        /// <param name="brandID">BrandID</param>
        /// <param name="shipModeID">shipModeID</param>
        public P04_ExcelImport(DataTable detailData, string brandID, string shipModeID)
        {
            this.InitializeComponent();
            this.detailData = detailData;
            this.mBrandID = brandID;
            this.mShipModeID = shipModeID;
        }

        /// <summary>
        /// OnFormLoaded
        /// </summary>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            DataTable excelFile = new DataTable();
            excelFile.Columns.Add("Filename", typeof(string));
            excelFile.Columns.Add("Status", typeof(string));
            excelFile.Columns.Add("FullFileName", typeof(string));

            this.listControlBindingSource1.DataSource = excelFile;
            this.gridAttachFile.DataSource = this.listControlBindingSource1;
            this.gridAttachFile.IsEditingReadOnly = true;
            this.Helper.Controls.Grid.Generator(this.gridAttachFile)
                .Text("Filename", header: "File Name", width: Widths.AnsiChars(15))
                .Text("Status", header: "Status", width: Widths.AnsiChars(100));

            // 取Grid結構
            // string sqlCmd = "select SPACE(13) as OrderID, null as BuyerDelivery,SPACE(10) as ShipmodeID,SPACE(8) as Article,SPACE(6) as ColorID,SPACE(8) as SizeCode,0.0 as Qty,SPACE(100) as ErrMsg";
            // DualResult result = DBProxy.Current.Select(null, sqlCmd, out grid2Data);
            this.grid2Data.Columns.Add("OrderID", typeof(string));
            this.grid2Data.Columns.Add("BuyerDelivery", typeof(DateTime));
            this.grid2Data.Columns.Add("ShipmodeID", typeof(string));
            this.grid2Data.Columns.Add("Article", typeof(string));
            this.grid2Data.Columns.Add("ColorID", typeof(string));
            this.grid2Data.Columns.Add("SizeCode", typeof(string));
            this.grid2Data.Columns.Add("Qty", typeof(int));
            this.grid2Data.Columns.Add("ErrMsg", typeof(string));

            this.listControlBindingSource2.DataSource = this.grid2Data;
            this.gridDetail.DataSource = this.listControlBindingSource2;
            this.gridDetail.IsEditingReadOnly = true;
            this.Helper.Controls.Grid.Generator(this.gridDetail)
                .Text("OrderID", header: "SP No.", width: Widths.AnsiChars(13))
                .Date("BuyerDelivery", header: "Buyer Delivery")
                .Text("ShipmodeID", header: "Ship Mode", width: Widths.AnsiChars(10))
                .Text("Article", header: "Color Way", width: Widths.AnsiChars(8))
                .Text("ColorID", header: "Color", width: Widths.AnsiChars(6))
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(8))
                .Numeric("Qty", header: "Qty")
                .Text("ErrMsg", header: "Error Message", width: Widths.AnsiChars(100));

            for (int i = 0; i < this.gridDetail.ColumnCount; i++)
            {
                this.gridDetail.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        // Add Excel
        private void BtnAddExcel_Click(object sender, EventArgs e)
        {
            this.openFileDialog1.Filter = "Excel files (*.xlsx;*.xls;*.xlt)|*.xlsx;*.xls;*.xlt";

            // 開窗且有選擇檔案
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                DataRow dr = ((DataTable)this.listControlBindingSource1.DataSource).NewRow();
                dr["Filename"] = this.openFileDialog1.SafeFileName;
                dr["Status"] = string.Empty;
                dr["FullFileName"] = this.openFileDialog1.FileName;
                ((DataTable)this.listControlBindingSource1.DataSource).Rows.Add(dr);
                this.listControlBindingSource1.MoveLast();
            }
        }

        // Remove Excel
        private void BtnRemoveExcel_Click(object sender, EventArgs e)
        {
            if (this.listControlBindingSource1.Position != -1)
            {
                this.listControlBindingSource1.RemoveCurrent();
            }
        }

        // Check & Import
        private void BtnCheckImport_Click(object sender, EventArgs e)
        {
            #region 判斷第一個Grid是否有資料
            if (this.listControlBindingSource1.Count <= 0)
            {
                MyUtility.Msg.WarningBox("No excel data!!");
                return;
            }
            #endregion

            // 清空Grid2資料
            if (this.grid2Data != null)
            {
                this.grid2Data.Clear();
            }

            this.gridDetail.SuspendLayout();
            #region 檢查1. Grid中的檔案是否存在，不存在時顯示於status欄位；2. Grid中的檔案都可以正常開啟，無法開啟時顯示於status欄位；3.檢查開啟的excel檔存在必要的欄位，將不存在欄位顯示於status。當檢查都沒問題時，就將資料寫入第2個Grid
            foreach (DataRow dr in ((DataTable)this.listControlBindingSource1.DataSource).Rows)
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
                            dr["ErrMsg"] = ex.Message;
                            continue;
                        }

                        excel.Workbooks.Open(MyUtility.Convert.GetString(dr["FullFileName"]));
                        excel.Visible = false;
                        Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

                        // 檢查Excel格式
                        Microsoft.Office.Interop.Excel.Range range = worksheet.Range[string.Format("A{0}:G{0}", 1)];
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

                                range = worksheet.Range[string.Format("A{0}:G{0}", intRowsRead)];
                                objCellArray = range.Value;

                                DataRow newRow = this.grid2Data.NewRow();
                                newRow["OrderID"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 1], "C");
                                newRow["BuyerDelivery"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 2], "D");
                                newRow["ShipmodeID"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 3], "C");
                                newRow["Article"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 4], "C");
                                newRow["ColorID"] = MyUtility.GetValue.Lookup(string.Format("select ColorID from [dbo].[View_OrderFAColor] where ID = '{0}' and Article = '{1}'", MyUtility.Convert.GetString(newRow["OrderID"]), MyUtility.Convert.GetString(newRow["Article"])));
                                newRow["SizeCode"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 5], "C");
                                newRow["Qty"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 6], "N");

                                this.grid2Data.Rows.Add(newRow);
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

            this.gridDetail.ResumeLayout();
        }

        // Write in
        private void BtnWriteIn_Click(object sender, EventArgs e)
        {
            DataTable tmpPackData;

            string sqlcmd = $@"
select distinct a.*,o.ID,oq.Seq,oqd.Article as oArticle,oqd.SizeCode as oSizeCode,o.StyleID,o.CustPONo,o.Category,o.SeasonID,o.BrandID
        , CheckShipMode = isnull ((select 1 
                                   where  EXISTS(
                                                SELECT 1
                                                FROM ShipMode s
                                                WHERE s.ID = a.ShipModeID AND s.ShipGroup IN
                                                (
	                                                SELECT ShipGroup
	                                                FROM ShipMode ss
	                                                WHERE ss.ID = '{this.mShipModeID}'
                                                )
                                          )
                                    ), 0)
from #tmp a
left join Orders o WITH (NOLOCK) on o.ID = a.OrderID
left join Order_QtyShip oq WITH (NOLOCK) on oq.Id = o.ID
left join Order_QtyShip_Detail oqd WITH (NOLOCK) on oqd.Id = oq.Id and oqd.Seq = oq.Seq and oqd.Article = a.Article and oqd.SizeCode = a.SizeCode 
WHERE o.Category = 'S'
";

            DualResult result = MyUtility.Tool.ProcessWithDatatable((DataTable)this.listControlBindingSource2.DataSource, "OrderID,BuyerDelivery,ShipmodeID,Article,ColorID,SizeCode,Qty", sqlcmd, out tmpPackData);

            if (result == false)
            {
                MyUtility.Msg.ErrorBox("Process error.\r\n" + result.ToString());
                return;
            }

            bool allPass = true;
            int count = -1;
            foreach (DataRow dr in ((DataTable)this.listControlBindingSource2.DataSource).Rows)
            {
                count++;

                DataRow[] findDR = tmpPackData.Select($@"
OrderID = '{dr["OrderID"]}' 
AND BuyerDelivery = {(MyUtility.Check.Empty(dr["BuyerDelivery"]) ? "null" : "'" + Convert.ToDateTime(dr["BuyerDelivery"]).ToString("d") + "'")} 
AND ShipmodeID = '{dr["ShipmodeID"]}' 
AND Article = '{dr["Article"]}' 
AND SizeCode = '{dr["SizeCode"]}' 
AND Qty = {dr["Qty"]} 
AND BrandID = '{this.mBrandID}'
");

                if (findDR.Length > 0)
                {
                    if (MyUtility.Check.Empty(findDR[0]["ID"]))
                    {
                        dr["ErrMsg"] = "< SP No. > is not exist!";
                        this.gridDetail.Rows[count].DefaultCellStyle.BackColor = Color.FromArgb(255, 192, 203);
                        allPass = false;
                        continue;
                    }

                    if (MyUtility.Convert.GetString(findDR[0]["Category"]) != "S")
                    {
                        dr["ErrMsg"] = "< SP No. > is not Sample!";
                        this.gridDetail.Rows[count].DefaultCellStyle.BackColor = Color.FromArgb(255, 192, 203);
                        allPass = false;
                        continue;
                    }

                    if (MyUtility.Check.Empty(findDR[0]["oArticle"]) || MyUtility.Check.Empty(findDR[0]["oSizeCode"]))
                    {
                        dr["ErrMsg"] = "< Color Way> or < Size > is not exist to the < SP No. >!";
                        this.gridDetail.Rows[count].DefaultCellStyle.BackColor = Color.FromArgb(255, 192, 203);
                        allPass = false;
                        continue;
                    }

                    /*
                        判斷匯入的 Ship Mode 所屬的ShipGroup，是否與表頭一致（A/C、E/C和A/P 或 E/P屬於相同ShipGroup）
                    */
                    if (MyUtility.Convert.GetString(findDR[0]["CheckShipMode"]) != "1")
                    {
                        dr["ErrMsg"] = "< Ship Mode > does not match Packing List ship mode.";
                        this.gridDetail.Rows[count].DefaultCellStyle.BackColor = Color.FromArgb(255, 192, 203);
                        allPass = false;
                        continue;
                    }

                    DataRow insertRow = this.detailData.NewRow();
                    insertRow["OrderID"] = dr["OrderID"];
                    insertRow["OrderShipmodeSeq"] = findDR[0]["Seq"];
                    insertRow["StyleID"] = findDR[0]["StyleID"];
                    insertRow["CustPONo"] = findDR[0]["CustPONo"];
                    insertRow["SeasonID"] = findDR[0]["SeasonID"];
                    insertRow["Article"] = dr["Article"];
                    insertRow["Color"] = dr["ColorID"];
                    insertRow["SizeCode"] = dr["SizeCode"];
                    insertRow["ShipQty"] = dr["Qty"];
                    this.detailData.Rows.Add(insertRow);
                    this.gridDetail.Rows[count].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 255);
                }
                else
                {
                    dr["ErrMsg"] = "< SP No. > is not exist!";
                    this.gridDetail.Rows[count].DefaultCellStyle.BackColor = Color.FromArgb(255, 192, 203);
                    allPass = false;
                }
            }

            if (allPass)
            {
                MyUtility.Msg.InfoBox("Write in completed!!");
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            else
            {
                MyUtility.Msg.WarningBox("Write in have errors!!");
            }
        }
    }
}
