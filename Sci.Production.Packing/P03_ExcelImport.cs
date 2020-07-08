using Ict.Win;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Packing
{
    /// <summary>
    /// Packing_P03_ExcelImport
    /// </summary>
    public partial class P03_ExcelImport : Win.Subs.Base
    {
        private DataRow P03_CurrentMaintain;
        private DataTable grid2Data = new DataTable();
        private DataTable detailData;

        /// <summary>
        /// P03_ExcelImport
        /// </summary>
        /// <param name="currentMaintain">CurrentMaintain</param>
        /// <param name="detailData">DetailData</param>
        public P03_ExcelImport(DataRow currentMaintain, DataTable detailData)
        {
            this.InitializeComponent();
            this.detailData = detailData;
            this.P03_CurrentMaintain = currentMaintain;
        }

        /// <summary>
        /// OnFormLoaded
        /// </summary>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            #region Grid1 Setting
            DataTable excelFile = new DataTable();
            excelFile.Columns.Add("Filename", typeof(string));
            excelFile.Columns.Add("Status", typeof(string));
            excelFile.Columns.Add("ErrLog", typeof(string));
            excelFile.Columns.Add("FullFileName", typeof(string));

            this.listControlBindingSource1.DataSource = excelFile;
            this.gridAttachFile.DataSource = this.listControlBindingSource1;
            this.gridAttachFile.IsEditingReadOnly = true;
            this.Helper.Controls.Grid.Generator(this.gridAttachFile)
                .Text("Filename", header: "File Name", width: Widths.AnsiChars(15))
                .EditText("Status", header: "Status", iseditingreadonly: true, width: Widths.AnsiChars(15))
                .EditText("Errlog", header: "ErrLog", iseditingreadonly: true, width: Widths.AnsiChars(30));
            #endregion

            #region Grid2 Setting
            this.grid2Data.Columns.Add("OrderID", typeof(string));
            this.grid2Data.Columns.Add("OrderShipmodeSeq", typeof(string));
            this.grid2Data.Columns.Add("StyleID", typeof(string));
            this.grid2Data.Columns.Add("CustPONo", typeof(string));
            this.grid2Data.Columns.Add("CTNStartNo", typeof(string));
            this.grid2Data.Columns.Add("CTNQty", typeof(int));
            this.grid2Data.Columns.Add("RefNo", typeof(string));
            this.grid2Data.Columns.Add("Description", typeof(string));
            this.grid2Data.Columns.Add("Article", typeof(string));
            this.grid2Data.Columns.Add("Color", typeof(string));
            this.grid2Data.Columns.Add("SizeCode", typeof(string));
            this.grid2Data.Columns.Add("QtyPerCTN", typeof(int));
            this.grid2Data.Columns.Add("ShipQty", typeof(int));
            this.grid2Data.Columns.Add("BalanceQty", typeof(int));
            this.grid2Data.Columns.Add("NW", typeof(int));
            this.grid2Data.Columns.Add("GW", typeof(int));
            this.grid2Data.Columns.Add("NNW", typeof(int));
            this.grid2Data.Columns.Add("NWPerPcs", typeof(int));

            this.listControlBindingSource2.DataSource = this.grid2Data;
            this.gridDetail.DataSource = this.listControlBindingSource2;
            this.gridDetail.IsEditingReadOnly = true;
            this.Helper.Controls.Grid.Generator(this.gridDetail)
                .Text("OrderID", header: "SP No.", width: Widths.AnsiChars(13))
                .Text("OrderShipmodeSeq", header: "Seq", width: Widths.AnsiChars(2), iseditingreadonly: true)
                .Text("StyleID", header: "Style No.", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("CustPONo", header: "P.O. No.", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("CTNStartNo", header: "CTN#", width: Widths.AnsiChars(6))
                .Numeric("CTNQty", header: "# of CTN")
                .CellCartonItem("RefNo", header: "Ref No.", width: Widths.AnsiChars(13))
                .Text("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("Article", header: "ColorWay", width: Widths.AnsiChars(8))
                .Text("Color", header: "Color", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(8))
                .Numeric("QtyPerCTN", header: "PC/Ctn")
                .Numeric("ShipQty", header: "Qty")
                .Numeric("NW", header: "N.W./Ctn", integer_places: 3, decimal_places: 3, maximum: 999.999M, minimum: 0)
                .Numeric("GW", header: "G.W./Ctn", integer_places: 3, decimal_places: 3, maximum: 999.999M, minimum: 0)
                .Numeric("NNW", header: "N.N.W./Ctn", integer_places: 3, decimal_places: 3, maximum: 999.999M, minimum: 0)
                .Numeric("NWPerPcs", header: "N.W./Pcs", integer_places: 2, decimal_places: 3, maximum: 99.999M, minimum: 0);
            #endregion

            for (int i = 0; i < this.gridDetail.ColumnCount; i++)
            {
                this.gridDetail.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private void BtnAddExcel_Click(object sender, EventArgs e)
        {
            this.openFileDialog1.Filter = "Excel files (*.xlsx)|*.xlsx";

            // 開窗且有選擇檔案
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                DataRow dr = ((DataTable)this.listControlBindingSource1.DataSource).NewRow();
                dr["Filename"] = this.openFileDialog1.SafeFileName;
                dr["Status"] = string.Empty;
                dr["Errlog"] = string.Empty;
                dr["FullFileName"] = this.openFileDialog1.FileName;
                ((DataTable)this.listControlBindingSource1.DataSource).Rows.Add(dr);
                this.listControlBindingSource1.MoveLast();
            }
        }

        private void BtnRemoveExcel_Click(object sender, EventArgs e)
        {
            if (this.listControlBindingSource1.Position != -1)
            {
                this.listControlBindingSource1.RemoveCurrent();
            }
        }

        private void BtnCheckImport_Click(object sender, EventArgs e)
        {
            #region 判斷第一個Grid是否有資料
            if (this.listControlBindingSource1.Count <= 0)
            {
                MyUtility.Msg.InfoBox("No excel data!!");
                return;
            }
            #endregion

            #region 清空Grid2資料
            if (this.grid2Data != null)
            {
                this.grid2Data.Clear();
            }

            this.gridDetail.SuspendLayout();
            #endregion

            /* 檢查
             * 1. Grid中的檔案是否存在，不存在時顯示於status欄位；
             * 2. Grid中的檔案都可以正常開啟，無法開啟時顯示於status欄位；
             * 3.檢查開啟的excel檔存在必要的欄位，將不存在欄位顯示於status。
             * 當檢查都沒問題時，就將資料寫入第2個Grid
             */
            #region 檢查
            foreach (DataRow dr in ((DataTable)this.listControlBindingSource1.DataSource).Rows)
            {
                dr["Status"] = "Failed";
                if (!MyUtility.Check.Empty(dr["Filename"]))
                {
                    if (!System.IO.File.Exists(MyUtility.Convert.GetString(dr["FullFileName"])))
                    {
                        dr["Errlog"] = string.Format("Excel file not found < {0} >.", MyUtility.Convert.GetString(dr["Filename"]));
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
                            dr["Errlog"] = string.Format("Not able to open excel file < {0} >. {1}", MyUtility.Convert.GetString(dr["Filename"]), ex.Message);
                            continue;
                        }

                        excel.Workbooks.Open(MyUtility.Convert.GetString(dr["FullFileName"]));
                        excel.Visible = false;
                        Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

                        #region 檢查Excel格式
                        Microsoft.Office.Interop.Excel.Range range = worksheet.Range[string.Format("A{0}:M{0}", 1)];
                        object[,] objCellArray = range.Value;
                        string sp = (string)MyUtility.Excel.GetExcelCellValue(objCellArray[1, 1], "C");
                        string seq = (string)MyUtility.Excel.GetExcelCellValue(objCellArray[1, 2], "C");
                        string ctnNo = (string)MyUtility.Excel.GetExcelCellValue(objCellArray[1, 3], "C");
                        string ctnQty = (string)MyUtility.Excel.GetExcelCellValue(objCellArray[1, 4], "C");
                        string refNo = (string)MyUtility.Excel.GetExcelCellValue(objCellArray[1, 5], "C");
                        string colorWay = (string)MyUtility.Excel.GetExcelCellValue(objCellArray[1, 6], "C");
                        string size = (string)MyUtility.Excel.GetExcelCellValue(objCellArray[1, 7], "C");
                        string pcCtn = (string)MyUtility.Excel.GetExcelCellValue(objCellArray[1, 8], "C");
                        string qty = (string)MyUtility.Excel.GetExcelCellValue(objCellArray[1, 9], "C");
                        string nwCtn = (string)MyUtility.Excel.GetExcelCellValue(objCellArray[1, 10], "C");
                        string gwCtn = (string)MyUtility.Excel.GetExcelCellValue(objCellArray[1, 11], "C");
                        string nnwCtn = (string)MyUtility.Excel.GetExcelCellValue(objCellArray[1, 12], "C");
                        string nwPcs = (string)MyUtility.Excel.GetExcelCellValue(objCellArray[1, 13], "C");

                        if (!sp.EqualString("SP No.") || !seq.EqualString("Seq") || !ctnNo.EqualString("CTN#") || !ctnQty.EqualString("# of CTN")
                            || !refNo.EqualString("Ref No.") || !colorWay.EqualString("ColorWay") || !size.EqualString("Size") || !pcCtn.EqualString("PC/Ctn")
                            || !qty.EqualString("Qty") || !nwCtn.EqualString("N.W./Ctn") || !gwCtn.EqualString("G.w./Ctn") || !nnwCtn.EqualString("N.N.W./Ctn")
                            || !nwPcs.EqualString("N.W./Pcs"))
                        {
                            #region 將不存在欄位顯示於status
                            StringBuilder columnName = new StringBuilder();
                            if (!sp.EqualString("SP No."))
                            {
                                columnName.Append("< SP No. >, ");
                            }

                            if (!seq.EqualString("Seq"))
                            {
                                columnName.Append("< Seq >, ");
                            }

                            if (!ctnNo.EqualString("CTN#"))
                            {
                                columnName.Append("< CTN# >, ");
                            }

                            if (!ctnQty.EqualString("# of CTN"))
                            {
                                columnName.Append("< # of CTN >, ");
                            }

                            if (!refNo.EqualString("Ref No."))
                            {
                                columnName.Append("< Ref No. >, ");
                            }

                            if (!colorWay.EqualString("ColorWay"))
                            {
                                columnName.Append("< ColorWay >, ");
                            }

                            if (!size.EqualString("Size"))
                            {
                                columnName.Append("< Size >, ");
                            }

                            if (!pcCtn.EqualString("PC/Ctn"))
                            {
                                columnName.Append("< PC/Ctn >, ");
                            }

                            if (!qty.EqualString("Qty"))
                            {
                                columnName.Append("< Qty >, ");
                            }

                            if (!nwCtn.EqualString("N.W./Ctn"))
                            {
                                columnName.Append("< N.W./Ctn >, ");
                            }

                            if (!gwCtn.EqualString("G.w./Ctn"))
                            {
                                columnName.Append("< G.w./Ctn >, ");
                            }

                            if (!nnwCtn.EqualString("N.N.W./Ctn"))
                            {
                                columnName.Append("< N.N.W./Ctn >, ");
                            }

                            if (!nwPcs.EqualString("N.W./Pcs"))
                            {
                                columnName.Append("< N.W./Pcs >, ");
                            }

                            dr["Errlog"] = columnName.ToString().Substring(0, columnName.ToString().Length - 2) + "column not found in the excel.";
                            #endregion
                        }
                        else
                        {
                            int intRowsCount = worksheet.UsedRange.Rows.Count;
                            int intColumnsCount = worksheet.UsedRange.Columns.Count;
                            int intRowsStart = 2;
                            int intRowsRead = intRowsStart - 1;
                            bool allPass = true, spSeq_Empty = false;
                            List<string> errStr = new List<string>();

                            #region Grid2 setData
                            while (intRowsRead < intRowsCount)
                            {
                                intRowsRead++;

                                range = worksheet.Range[string.Format("A{0}:M{0}", intRowsRead)];
                                objCellArray = range.Value;

                                // when  (SP#, Seq, Qty = empty or 0) then   此筆不匯入
                                if (MyUtility.Check.Empty(MyUtility.Excel.GetExcelCellValue(objCellArray[1, 1], "C")) || MyUtility.Check.Empty(MyUtility.Excel.GetExcelCellValue(objCellArray[1, 2], "C"))
                                    || MyUtility.Check.Empty(MyUtility.Excel.GetExcelCellValue(objCellArray[1, 9], "N")) || MyUtility.Excel.GetExcelCellValue(objCellArray[1, 9], "N").EqualString("0"))
                                {
                                    if (!spSeq_Empty)
                                    {
                                        spSeq_Empty = true;
                                    }

                                    allPass = false;
                                    continue;
                                }

                                DataRow newRow = this.grid2Data.NewRow();
                                newRow["OrderID"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 1], "C");
                                newRow["OrderShipmodeSeq"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 2], "C");

                                #region check SP+Seq
                                if (!this.CheckSP(newRow))
                                {
                                    errStr.Add(string.Format("< Row: {2} >, < SP No. >:{0}, < Seq >:{1} - Data not found.", newRow["OrderID"], newRow["OrderShipmodeSeq"], intRowsRead));
                                    allPass = false;
                                    continue;
                                }

                                newRow["StyleID"] = MyUtility.GetValue.Lookup(string.Format(
                                    "select StyleID from Orders where ID = '{0}' and Category = 'B' and BrandID = '{1}' and Dest = '{2}' and CustCDID = '{3}'",
                                    newRow["OrderID"].ToString(),
                                    this.P03_CurrentMaintain["BrandID"].ToString(),
                                    this.P03_CurrentMaintain["Dest"].ToString(),
                                    this.P03_CurrentMaintain["CustCDID"].ToString()));
                                newRow["CustPONo"] = MyUtility.GetValue.Lookup(string.Format(
                                    "select CustPoNo from Orders where ID = '{0}' and Category = 'B' and BrandID = '{1}' and Dest = '{2}' and CustCDID = '{3}'",
                                    newRow["OrderID"].ToString(),
                                    this.P03_CurrentMaintain["BrandID"].ToString(),
                                    this.P03_CurrentMaintain["Dest"].ToString(),
                                    this.P03_CurrentMaintain["CustCDID"].ToString()));
                                #endregion

                                newRow["CTNStartNo"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 3], "C");
                                newRow["CTNQty"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 4], "N");
                                newRow["RefNo"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 5], "C");

                                #region check RefNo
                                if (!this.CheckRefNo(newRow))
                                {
                                    errStr.Add(string.Format("< Row: {3} >, < SP No. >:{0}, < Seq >:{1}. < Ref No. >:{2} - Data not found.", newRow["OrderID"], newRow["OrderShipmodeSeq"], newRow["RefNo"], intRowsRead));
                                    allPass = false;
                                    continue;
                                }

                                newRow["Description"] = MyUtility.GetValue.Lookup(string.Format(
                                    "select Description,CtnWeight from LocalItem WITH (NOLOCK) where RefNo = '{0}'",
                                    newRow["RefNo"].ToString()));
                                #endregion

                                newRow["Article"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 6], "C");

                                #region check Article
                                if (!this.CheckArticle(newRow))
                                {
                                    errStr.Add(string.Format("< Row: {3} >, < SP No. >:{0}, < Seq >:{1}. < ColorWay >:{2} - Data not found.", newRow["OrderID"], newRow["OrderShipmodeSeq"], newRow["Article"], intRowsRead));
                                    allPass = false;
                                    continue;
                                }

                                newRow["Color"] = MyUtility.GetValue.Lookup(string.Format(
                                    @"select ColorID from View_OrderFAColor where ID = '{0}' and Article = '{1}'",
                                    newRow["OrderID"].ToString(),
                                    newRow["Article"]));
                                #endregion

                                newRow["SizeCode"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 7], "C");

                                #region check SizeCode
                                if (!this.CheckSize(newRow))
                                {
                                    errStr.Add(string.Format("< Row: {3} >, < SP No. >:{0}, < Seq >:{1}. < Size >:{2} - Data not found.", newRow["OrderID"], newRow["OrderShipmodeSeq"], newRow["SizeCode"], intRowsRead));
                                    allPass = false;
                                    continue;
                                }
                                #endregion

                                newRow["QtyPerCTN"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 8], "N");
                                newRow["ShipQty"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 9], "N");
                                newRow["NW"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 10], "N");
                                newRow["GW"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 11], "N");
                                newRow["NNW"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 12], "N");
                                newRow["NWPerPcs"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 13], "N");

                                this.grid2Data.Rows.Add(newRow);
                            }
                            #endregion

                            dr["Errlog"] = string.Empty;
                            if (allPass)
                            {
                                dr["Status"] = "Check & Import Completed.";
                            }
                            else
                            {
                                if (spSeq_Empty)
                                {
                                    dr["Errlog"] = "SP# and Seq can not be Empty, Qty can not be Empty or 0.\r\n";
                                }

                                dr["Errlog"] += errStr.JoinToString("\r\n");
                                MyUtility.Msg.InfoBox("Some data import failed !!");
                            }
                        }
                        #endregion

                        excel.Workbooks.Close();
                        excel.Quit();
                        excel = null;
                    }
                }
            }
            #endregion
        }

        private bool CheckSP(DataRow dr)
        {
            bool pass = true;
            if (!MyUtility.Check.Seek(string.Format("Select ID,StyleID,CustPONo from Orders WITH (NOLOCK) where ID = '{0}' and Category = 'B' and BrandID = '{1}' and Dest = '{2}' and CustCDID = '{3}'", dr["OrderID"], this.P03_CurrentMaintain["BrandID"].ToString(), this.P03_CurrentMaintain["Dest"].ToString(), this.P03_CurrentMaintain["CustCDID"].ToString())))
            {
                pass = false;
            }
            else if (!MyUtility.Check.Seek(string.Format("select Seq from Order_QtyShip where id = '{0}' and seq = '{1}'", dr["OrderID"], dr["OrderShipmodeSeq"])))
            {
                pass = false;
            }

            return pass;
        }

        private bool CheckRefNo(DataRow dr)
        {
            bool pass = true;
            if (!MyUtility.Check.Seek(string.Format("select RefNo from LocalItem WITH (NOLOCK) where RefNo = '{0}'", dr["RefNo"])))
            {
                pass = false;
            }

            return pass;
        }

        private bool CheckArticle(DataRow dr)
        {
            bool pass = true;
            if (!MyUtility.Check.Seek(string.Format("Select Distinct Article from Order_QtyShip_Detail WITH (NOLOCK) where ID = '{0}' and Seq = '{1}' and Article = '{2}'", dr["OrderID"], dr["OrderShipmodeSeq"], dr["Article"])))
            {
                pass = false;
            }

            return pass;
        }

        private bool CheckSize(DataRow dr)
        {
            bool pass = true;
            if (!MyUtility.Check.Seek(string.Format("Select SizeCode from Order_QtyShip_Detail WITH (NOLOCK) where ID = '{0}' and Seq = '{1}' and Article = '{2}' and SizeCode = '{3}'", dr["OrderID"], dr["OrderShipmodeSeq"], dr["Article"], dr["SizeCode"])))
            {
                pass = false;
            }

            return pass;
        }

        private void BtnWrite_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.detailData.Rows.Count; i++)
            {
                this.detailData.Rows[i].Delete();
            }

            foreach (DataRow dr in ((DataTable)this.listControlBindingSource2.DataSource).Rows)
            {
                DataRow insertRow = this.detailData.NewRow();
                insertRow["OrderID"] = dr["OrderID"];
                insertRow["OrderShipmodeSeq"] = dr["OrderShipmodeSeq"];
                insertRow["StyleID"] = dr["StyleID"];
                insertRow["CustPONo"] = dr["CustPONo"];
                insertRow["CTNStartNo"] = dr["CTNStartNo"];
                insertRow["CTNQty"] = dr["CTNQty"];
                insertRow["RefNo"] = dr["RefNo"];
                insertRow["Description"] = dr["Description"];
                insertRow["Article"] = dr["Article"];
                insertRow["Color"] = dr["Color"];
                insertRow["SizeCode"] = dr["SizeCode"];
                insertRow["QtyPerCTN"] = dr["QtyPerCTN"];
                insertRow["ShipQty"] = dr["ShipQty"];
                insertRow["NW"] = dr["NW"];
                insertRow["GW"] = dr["GW"];
                insertRow["NNW"] = dr["NNW"];
                insertRow["NWPerPcs"] = dr["NWPerPcs"];
                this.detailData.Rows.Add(insertRow);
            }

            MyUtility.Msg.InfoBox("Import complete.	");
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
