using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using System.Linq;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.Data.SqlClient;

namespace Sci.Production.Warehouse
{
    public partial class P07_ExcelImport : Win.Subs.Base
    {
        private DataTable grid2Data = new DataTable();
        private DataTable detailData;
        private DataRow master;

        public P07_ExcelImport(DataRow _master, DataTable detailData)
        {
            this.InitializeComponent();
            this.detailData = detailData;
            this.master = _master;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            DataTable excelFile = new DataTable();
            excelFile.Columns.Add("Filename", typeof(string));
            excelFile.Columns.Add("Status", typeof(string));
            excelFile.Columns.Add("Count", typeof(string));
            excelFile.Columns.Add("FullFileName", typeof(string));

            this.listControlBindingSource1.DataSource = excelFile;
            this.gridAttachFile.DataSource = this.listControlBindingSource1;
            this.gridAttachFile.IsEditingReadOnly = true;
            this.Helper.Controls.Grid.Generator(this.gridAttachFile)
                .Text("Filename", header: "File Name", width: Widths.AnsiChars(15))
                .Text("Status", header: "Status", width: Widths.AnsiChars(30))
                .Text("Count", header: "Count", width: Widths.AnsiChars(10))
                ;

            // 取Grid結構
            // string sqlCmd = "select SPACE(13) as OrderID, null as BuyerDelivery,SPACE(10) as ShipmodeID,SPACE(8) as Article,SPACE(6) as ColorID,SPACE(8) as SizeCode,0.0 as Qty,SPACE(100) as ErrMsg";
            // DualResult result = DBProxy.Current.Select(null, sqlCmd, out grid2Data);
            this.grid2Data.Columns.Add("wkno", typeof(string));
            this.grid2Data.Columns.Add("poid", typeof(string));
            this.grid2Data.Columns.Add("seq1", typeof(string));
            this.grid2Data.Columns.Add("seq2", typeof(string));
            this.grid2Data.Columns.Add("seq", typeof(string));
            this.grid2Data.Columns.Add("PoIdSeq1", typeof(string));
            this.grid2Data.Columns.Add("PoIdSeq", typeof(string));
            this.grid2Data.Columns.Add("roll", typeof(string));
            this.grid2Data.Columns.Add("dyelot", typeof(string));
            this.grid2Data.Columns.Add("qty", typeof(decimal));
            this.grid2Data.Columns.Add("foc", typeof(decimal));
            this.grid2Data.Columns.Add("shipqty", typeof(decimal));
            this.grid2Data.Columns.Add("actualqty", typeof(decimal));
            this.grid2Data.Columns.Add("stockqty", typeof(decimal));
            this.grid2Data.Columns.Add("weight", typeof(decimal));
            this.grid2Data.Columns.Add("actualWeight", typeof(decimal));
            this.grid2Data.Columns.Add("pounit", typeof(string));
            this.grid2Data.Columns.Add("stockunit", typeof(string));
            this.grid2Data.Columns.Add("stocktype", typeof(string));
            this.grid2Data.Columns.Add("id", typeof(string));
            this.grid2Data.Columns.Add("location", typeof(string));
            this.grid2Data.Columns.Add("Remark", typeof(string));
            this.grid2Data.Columns.Add("ErrMsg", typeof(string));
            this.grid2Data.Columns.Add("fabrictype", typeof(string));
            this.grid2Data.Columns.Add("Refno", typeof(string));
            this.grid2Data.Columns.Add("ColorID", typeof(string));

            this.listControlBindingSource2.DataSource = this.grid2Data;
            this.gridPoid.DataSource = this.listControlBindingSource2;
            #region Ship Qty Valid

            DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings();
            ns.CellValidating += (s, e) =>
            {
                if (this.EditMode && e.FormattedValue != null)
                {
                    this.grid2Data.Rows[e.RowIndex]["shipqty"] = Convert.ToDecimal(e.FormattedValue) + Convert.ToDecimal(this.grid2Data.Rows[e.RowIndex]["foc"].ToString());
                    this.grid2Data.Rows[e.RowIndex]["actualqty"] = Convert.ToDecimal(e.FormattedValue) + Convert.ToDecimal(this.grid2Data.Rows[e.RowIndex]["foc"].ToString());
                    this.grid2Data.Rows[e.RowIndex]["stockqty"] = Convert.ToDecimal(e.FormattedValue) + Convert.ToDecimal(this.grid2Data.Rows[e.RowIndex]["foc"].ToString());
                    this.grid2Data.Rows[e.RowIndex]["qty"] = e.FormattedValue;

                    // grid2Data.Rows[e.RowIndex]["foc"] = e.FormattedValue;
                }
            };

            DataGridViewGeneratorNumericColumnSettings ns2 = new DataGridViewGeneratorNumericColumnSettings();
            ns2.CellValidating += (s, e) =>
            {
                if (this.EditMode && e.FormattedValue != null)
                {
                    this.grid2Data.Rows[e.RowIndex]["shipqty"] = Convert.ToDecimal(e.FormattedValue) + Convert.ToDecimal(this.grid2Data.Rows[e.RowIndex]["qty"].ToString());
                    this.grid2Data.Rows[e.RowIndex]["actualqty"] = Convert.ToDecimal(e.FormattedValue) + Convert.ToDecimal(this.grid2Data.Rows[e.RowIndex]["qty"].ToString());
                    this.grid2Data.Rows[e.RowIndex]["stockqty"] = Convert.ToDecimal(e.FormattedValue) + Convert.ToDecimal(this.grid2Data.Rows[e.RowIndex]["qty"].ToString());

                    // grid2Data.Rows[e.RowIndex]["qty"] = e.FormattedValue;
                    this.grid2Data.Rows[e.RowIndex]["foc"] = e.FormattedValue;
                }
            };

            #endregion Ship Qty Valid

            this.gridPoid.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.gridPoid)
                .Text("poid", header: "SP#", width: Widths.AnsiChars(13))
                .Text("seq1", header: "Seq1", width: Widths.AnsiChars(4))
                .Text("seq2", header: "Seq2", width: Widths.AnsiChars(3))
                .Text("roll", header: "Carton#", width: Widths.AnsiChars(8))
                .Text("dyelot", header: "Lot No", width: Widths.AnsiChars(4))
                .Text("pounit", header: "Po Unit", width: Widths.AnsiChars(4))
                .Text("stockunit", header: "Stock Unit", width: Widths.AnsiChars(4))
                .Numeric("Qty", header: "Qty", decimal_places: 2, settings: ns)
                .Numeric("foc", header: "F.O.C", decimal_places: 2, settings: ns2)
                .Numeric("weight", header: "WeiKg", decimal_places: 2)
                .Numeric("actualWeight", header: "NetKg", decimal_places: 2)
                .Text("location", header: "Location", width: Widths.AnsiChars(8))
                .Text("Remark", header: "Remark", width: Widths.AnsiChars(8))
                .EditText("ErrMsg", header: "Error Message", width: Widths.AnsiChars(100), iseditingreadonly: true);

            for (int i = 0; i < this.gridPoid.ColumnCount; i++)
            {
                this.gridPoid.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                this.gridPoid.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
        }

        // Add Excel
        private void BtnAddExcel_Click(object sender, EventArgs e)
        {
            this.openFileDialog1.Filter = "Excel files (*.xlsx;*.xls)|*.xlsx;*.xls";
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK) // 開窗且有選擇檔案
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
            #region -- 判斷第一個Grid是否有資料 --
            if (this.listControlBindingSource1.Count <= 0)
            {
                MyUtility.Msg.WarningBox("No excel data!!");
                return;
            }
            #endregion

            #region -- 清空Grid2資料
            if (this.grid2Data != null)
            {
                this.grid2Data.Clear();
            }

            this.gridPoid.SuspendLayout();
            #endregion

            /* 檢查1. Grid中的檔案是否存在，不存在時顯示於status欄位
                 --   2. Grid中的檔案都可以正常開啟，無法開啟時顯示於status欄位
                 --   3.檢查開啟的excel檔存在必要的欄位，將不存在欄位顯示於status。當檢查都沒問題時，就將資料寫入第2個Grid*/
            #region
            int count = 0;
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
                            MessageBox.Show(ex.ToString());
                            dr["Status"] = string.Format("Not able to open excel file < {0} >.", MyUtility.Convert.GetString(dr["Filename"]));
                            continue;
                        }

                        excel.Workbooks.Open(MyUtility.Convert.GetString(dr["FullFileName"]));
                        excel.Visible = false;
                        Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];
                        int intColumnsCount = worksheet.UsedRange.Columns.Count;

                        if (intColumnsCount >= 30)
                        {
                            Marshal.ReleaseComObject(worksheet);
                            excel.ActiveWorkbook.Close(false, Type.Missing, Type.Missing);

                            // excel.Workbooks.Close();
                            excel.Quit();
                            Marshal.ReleaseComObject(excel);
                            excel = null;
                            MyUtility.Msg.WarningBox("Column count can not more than 30!!");
                            return;
                        }

                        // 檢查Excel格式
                        Microsoft.Office.Interop.Excel.Range range = worksheet.Range[string.Format("A{0}:AE{0}", 1)];
                        object[,] objCellArray = range.Value;
                        int[] itemPosition = new int[13];
                        string[] itemCheck = { string.Empty, "WK#", "SP#", "SEQ1", "SEQ2", "C/NO", "LOT NO.", "QTY", "F.O.C", "NETKG", "WEIKG", "LOCATION", "REMARK" };
                        string[] excelItem = new string[intColumnsCount + 1];

                        for (int y = 1; y <= intColumnsCount; y++)
                        {
                            excelItem[y] = (string)MyUtility.Excel.GetExcelCellValue(objCellArray[1, y], "C").ToString().ToUpper();
                        }

                        StringBuilder columnName = new StringBuilder();

                        // 確認Excel各Item是否存在，並儲存所在位置
                        for (int x = 1; x <= 12; x++)
                        {
                            for (int y = 1; y <= intColumnsCount; y++)
                            {
                                if (excelItem[y] == itemCheck[x])
                                {
                                    itemPosition[x] = y;
                                    break;
                                }
                            }

                            if (itemPosition[x] == 0)
                            {
                                columnName.Append("< " + itemCheck[x].ToString() + " >, ");
                            }
                        }

                        if (!MyUtility.Check.Empty(columnName.Length))
                        {
                            dr["Status"] = columnName.ToString().Substring(0, columnName.ToString().Length - 2) + "column not found in the excel.";
                        }
                        else
                        {
                            int intRowsCount = worksheet.UsedRange.Rows.Count;
                            int intRowsStart = 2;
                            int intRowsRead = intRowsStart - 1;

                            while (intRowsRead < intRowsCount)
                            {
                                intRowsRead++;
                                range = worksheet.Range[string.Format("A{0}:AE{0}", intRowsRead)];
                                objCellArray = range.Value;
                                List<string> listNewRowErrMsg = new List<string>();

                                DataRow newRow = this.grid2Data.NewRow();
                                newRow["wkno"] = (objCellArray[1, itemPosition[1]] == null) ? string.Empty : MyUtility.Excel.GetExcelCellValue(objCellArray[1, itemPosition[1]].ToString().Trim(), "C");
                                newRow["poid"] = (objCellArray[1, itemPosition[2]] == null) ? string.Empty : MyUtility.Excel.GetExcelCellValue(objCellArray[1, itemPosition[2]].ToString().Trim(), "C");
                                newRow["seq1"] = (objCellArray[1, itemPosition[3]] == null) ? string.Empty : MyUtility.Excel.GetExcelCellValue(objCellArray[1, itemPosition[3]].ToString().Trim(), "C");
                                newRow["seq2"] = (objCellArray[1, itemPosition[4]] == null) ? string.Empty : MyUtility.Excel.GetExcelCellValue(objCellArray[1, itemPosition[4]].ToString().Trim(), "C");
                                newRow["seq"] = ((objCellArray[1, itemPosition[3]] == null) ? string.Empty : MyUtility.Excel.GetExcelCellValue(objCellArray[1, itemPosition[3]].ToString().Trim(), "C").ToString().PadRight(3)) + ((objCellArray[1, itemPosition[4]] == null) ? string.Empty : MyUtility.Excel.GetExcelCellValue(objCellArray[1, itemPosition[4]].ToString().Trim(), "C").ToString());
                                newRow["PoIdSeq1"] = ((objCellArray[1, itemPosition[2]] == null) ? string.Empty : MyUtility.Excel.GetExcelCellValue(objCellArray[1, itemPosition[2]].ToString().Trim(), "C")) + ((objCellArray[1, itemPosition[3]] == null) ? string.Empty : MyUtility.Excel.GetExcelCellValue(objCellArray[1, itemPosition[3]].ToString().Trim(), "C").ToString());
                                newRow["PoIdSeq"] = ((objCellArray[1, itemPosition[2]] == null) ? string.Empty : MyUtility.Excel.GetExcelCellValue(objCellArray[1, itemPosition[2]].ToString().Trim(), "C")) + ((objCellArray[1, itemPosition[3]] == null) ? string.Empty : MyUtility.Excel.GetExcelCellValue(objCellArray[1, itemPosition[3]].ToString().Trim(), "C").ToString().PadRight(3)) + ((objCellArray[1, itemPosition[4]] == null) ? string.Empty : MyUtility.Excel.GetExcelCellValue(objCellArray[1, itemPosition[4]].ToString().Trim(), "C").ToString());
                                newRow["roll"] = (objCellArray[1, itemPosition[5]] == null) ? string.Empty : MyUtility.Excel.GetExcelCellValue(objCellArray[1, itemPosition[5]].ToString().Replace("'", string.Empty).Trim(), "C");
                                newRow["dyelot"] = (objCellArray[1, itemPosition[6]] == null) ? string.Empty : MyUtility.Excel.GetExcelCellValue(objCellArray[1, itemPosition[6]].ToString().Replace("'", string.Empty).Trim(), "C");
                                newRow["qty"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, itemPosition[7]], "N");
                                newRow["foc"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, itemPosition[8]], "N");
                                newRow["shipqty"] = decimal.Parse(newRow["qty"].ToString()) + decimal.Parse(newRow["foc"].ToString().Trim());
                                newRow["actualqty"] = decimal.Parse(newRow["qty"].ToString()) + decimal.Parse(newRow["foc"].ToString().Trim());
                                newRow["actualWeight"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, itemPosition[9]], "N");
                                newRow["Weight"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, itemPosition[10]], "N");
                                newRow["location"] = (objCellArray[1, itemPosition[11]] == null) ? string.Empty : MyUtility.Excel.GetExcelCellValue(objCellArray[1, itemPosition[11]].ToString().Trim(), "C");
                                newRow["Remark"] = (objCellArray[1, itemPosition[12]] == null) ? string.Empty : MyUtility.Excel.GetExcelCellValue(objCellArray[1, itemPosition[12]].ToString().Trim(), "C");

                                #region check Columns length
                                List<string> listColumnLengthErrMsg = new List<string>();

                                // Poid varchar(13)
                                if (Encoding.Default.GetBytes(newRow["poid"].ToString()).Length > 13)
                                {
                                    listColumnLengthErrMsg.Add("<SP#> length can't be more than 13 Characters.");
                                }

                                // Seq1 varchar(3)
                                if (Encoding.Default.GetBytes(newRow["Seq1"].ToString()).Length > 3)
                                {
                                    listColumnLengthErrMsg.Add("<SEQ1> length can't be more than 3 Characters.");
                                }

                                // Seq2 varchar(2)
                                if (Encoding.Default.GetBytes(newRow["Seq2"].ToString()).Length > 2)
                                {
                                    listColumnLengthErrMsg.Add("<SEQ2> length can't be more than 2 Characters.");
                                }

                                // Roll varchar(8)
                                if (Encoding.Default.GetBytes(newRow["Roll"].ToString()).Length > 8)
                                {
                                    listColumnLengthErrMsg.Add("<C/No> length can't be more than 8 Characters.");
                                }

                                // Dyelot varchar(8)
                                if (Encoding.Default.GetBytes(newRow["Dyelot"].ToString()).Length > 8)
                                {
                                    listColumnLengthErrMsg.Add("<LOT NO.> length can't be more than 8 Characters.");
                                }

                                // qty + foc  numeric (11, 2)
                                if (decimal.Parse(newRow["qty"].ToString()) + decimal.Parse(newRow["foc"].ToString()) > 999999999)
                                {
                                    listColumnLengthErrMsg.Add("<Qty + F.O.C> value can't be more than 999,999,999");
                                }

                                // actualWeight numeric (7, 2)
                                if (decimal.Parse(newRow["actualWeight"].ToString()) > 99999)
                                {
                                    listColumnLengthErrMsg.Add("<NetKg> value can't be more than 99,999");
                                }

                                // Weight numeric (7, 2)
                                if (decimal.Parse(newRow["Weight"].ToString()) > 99999)
                                {
                                    listColumnLengthErrMsg.Add("<WeiKg> value can't be more than 99,999");
                                }

                                // Location varchar(60)
                                if (Encoding.Default.GetBytes(newRow["Location"].ToString()).Length > 60)
                                {
                                    listColumnLengthErrMsg.Add("<Location> length can't be more than 60 Characters.");
                                }

                                // Remark nvarchar(100)
                                if (Encoding.Default.GetBytes(newRow["Remark"].ToString()).Length > 100)
                                {
                                    listColumnLengthErrMsg.Add("<Remark> length can't be more than 100 Characters.");
                                }

                                if (listColumnLengthErrMsg.Count > 0)
                                {
                                    listNewRowErrMsg.Add(listColumnLengthErrMsg.JoinToString(Environment.NewLine));
                                }
                                #endregion

                                if (!MyUtility.Check.Empty(newRow["wkno"]) && newRow["wkno"].ToString() != this.master["InvNo"].ToString())
                                {
                                    listNewRowErrMsg.Add(string.Format("WK# is not match {0}", this.master["InvNo"]));
                                }

                                if (MyUtility.Check.Empty(newRow["poid"]) || MyUtility.Check.Empty(newRow["seq1"]) || MyUtility.Check.Empty(newRow["seq2"]))
                                {
                                    continue;
                                }

                                if (newRow["seq1"].ToString().Substring(0, 1) == "7")
                                {
                                    listNewRowErrMsg.Add(string.Format("Can't not import 7X item (SP#:{0}-Seq1:{1}-Seq2:{2})!!", newRow["poid"],
                                                                                                                             newRow["seq1"],
                                                                                                                             newRow["seq2"]));
                                }

                                List<SqlParameter> sqlpar = new List<SqlParameter>();
                                sqlpar.Add(new SqlParameter("@poid", newRow["poid"].ToString().Trim()));
                                sqlpar.Add(new SqlParameter("@seq1", newRow["seq1"].ToString().Trim()));
                                sqlpar.Add(new SqlParameter("@seq2", newRow["seq2"].ToString().Trim()));
                                sqlpar.Add(new SqlParameter("@shipqty", newRow["shipqty"].ToString().Trim()));
                                DataRow dr2;
                                string sql = @"
select  pd.fabrictype
        , pd.POUnit
        , StockUnit = dbo.GetStockUnitBySPSeq (pd.id, pd.seq1, pd.seq2)
        , Round(dbo.GetUnitQty(pd.POUnit, dbo.GetStockUnitBySPSeq (pd.id, pd.seq1, pd.seq2), @shipqty), 2) as stockqty 
        , (select o.Category from Orders o WITH (NOLOCK) where o.id= pd.id) as category
        , pd.Refno
        , [ColorID] = isnull(Color.Value,'')
from dbo.PO_Supp_Detail pd WITH (NOLOCK) 
inner join [dbo].[Fabric] ff WITH (NOLOCK) on pd.SCIRefno= ff.SCIRefno
inner join [dbo].[MtlType] mm WITH (NOLOCK) on mm.ID = ff.MtlTypeID
inner join [dbo].[Unit] uu WITH (NOLOCK) on ff.UsageUnit = uu.ID
OUTER APPLY(
 SELECT [Value]=
	 CASE WHEN ff.MtlTypeID in ('EMB THREAD','SP THREAD','THREAD') THEN pd.SuppColor
		 ELSE dbo.GetColorMultipleID(pd.BrandID,pd.ColorID)
	 END
)Color
where pd.id=@poid and pd.seq1 =@seq1 and pd.seq2 = @seq2";

                                if (MyUtility.Check.Seek(sql, sqlpar, out dr2))
                                {
                                    // 非主料清空roll & dyelot
                                    if (dr2["fabrictype"].ToString() != "F")
                                    {
                                        newRow["roll"] = string.Empty;
                                        newRow["dyelot"] = string.Empty;
                                    }

                                    // po unit 空白不匯入
                                    if (MyUtility.Check.Empty(dr2["pounit"]))
                                    {
                                        listNewRowErrMsg.Add(string.Format("PO Unit of SP#:{0}-Seq1:{1}-Seq2:{2} is empty!!", newRow["poid"],
                                                                                                                          newRow["seq1"],
                                                                                                                          newRow["seq2"]));
                                    }

                                    // stock unit空白不匯入
                                    if (MyUtility.Check.Empty(dr2["stockunit"]))
                                    {
                                        listNewRowErrMsg.Add(string.Format("Stock Unit of SP#:{0}-Seq1:{1}-Seq2:{2} is empty!!", newRow["poid"],
                                                                                                                             newRow["seq1"],
                                                                                                                             newRow["seq2"]));
                                    }

                                    newRow["fabrictype"] = dr2["fabrictype"].ToString();
                                    newRow["Pounit"] = dr2["pounit"].ToString();
                                    newRow["StockUnit"] = dr2["StockUnit"].ToString();
                                    newRow["stockqty"] = decimal.Parse(dr2["stockqty"].ToString());
                                    newRow["Refno"] = dr2["Refno"].ToString();
                                    newRow["ColorID"] = dr2["ColorID"].ToString();

                                    // 決定倉別
                                    if (dr2["category"].ToString() == "M")
                                    {
                                        newRow["stocktype"] = "I";
                                    }
                                    else
                                    {
                                        newRow["stocktype"] = "B";
                                    }

                                    // 檢查location是否正確
                                    if (!MyUtility.Check.Empty(newRow["location"]))
                                    {
                                        string[] strA = Regex.Split(newRow["location"].ToString(), ",");
                                        foreach (string i in strA.Distinct())
                                        {
                                            if (!MyUtility.Check.Seek(string.Format(
                                                @"
select * 
from    dbo.mtllocation WITH (NOLOCK) 
where   stocktype='{0}' 
        and junk != '1'
        and id='{1}'", newRow["stocktype"], i)))
                                            {
                                                listNewRowErrMsg.Add(string.Format("Location ({3}) of SP#:{0}-Seq1:{1}-Seq2:{2} in stock ({4}) is not found!!", newRow["poid"],
                                                                                                                                                            newRow["seq1"],
                                                                                                                                                            newRow["seq2"],
                                                                                                                                                            i,
                                                                                                                                                            newRow["stocktype"]));
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    listNewRowErrMsg.Add(string.Format("SP#:{0}-Seq1:{1}-Seq2:{2} is not found!!", newRow["poid"], newRow["seq1"], newRow["seq2"]));
                                }

                                if (listNewRowErrMsg.Count == 0)
                                {
                                    count++;
                                }
                                else
                                {
                                    newRow["ErrMsg"] = listNewRowErrMsg.JoinToString(Environment.NewLine);
                                }

                                this.grid2Data.Rows.Add(newRow);
                            }

                            dr["Status"] = (intRowsCount - 1 == count) ? "Check & Import Completed." : "Some Data Faild. Please check Error Message.";
                            dr["Count"] = count;
                        }

                        Marshal.ReleaseComObject(worksheet);
                        excel.ActiveWorkbook.Close(false, Type.Missing, Type.Missing);

                        // excel.Workbooks.Close();
                        excel.Quit();
                        Marshal.ReleaseComObject(excel);
                        excel = null;
                    }
                }
            }
            #endregion

            this.gridPoid.ResumeLayout();
            foreach (DataGridViewRow dr in this.gridPoid.Rows)
            {
                if (!dr.Cells["ErrMsg"].Value.Empty())
                {
                    dr.DefaultCellStyle.ForeColor = Color.Red;
                }
            }
        }

        // Write in
        private void BtnWriteIn_Click(object sender, EventArgs e)
        {
            DataTable tmpPacking = (DataTable)this.listControlBindingSource2.DataSource;

            // if (((DataTable)listControlBindingSource2.DataSource).AsEnumerable().Any(row => row["ErrMsg"].Empty()))
            // {
            //    tmpPacking = ((DataTable)listControlBindingSource2.DataSource).AsEnumerable().Where(row => row["ErrMsg"].Empty()).CopyToDataTable();
            // } else
            // {
            //    MyUtility.Msg.InfoBox("Write in completed!!");
            //    DialogResult = System.Windows.Forms.DialogResult.OK;
            //    return;
            // }

            // 如果資料中有錯誤不能WriteIn
            if (tmpPacking.AsEnumerable().Any(s => s["ErrMsg"].ToString().Contains("length can't be more than")))
            {
                MyUtility.Msg.WarningBox("Excel column value over length limit,please check column [Error Message] information to fix Excel.");
                return;
            }

            try
            {
                var q = from p in tmpPacking.AsEnumerable()
                        group p by new
                        {
                            poid = p.Field<string>("poid"),
                            seq1 = p.Field<string>("seq1"),
                            seq2 = p.Field<string>("seq2"),
                            Roll = p.Field<string>("Roll"),
                            Dyelot = p.Field<string>("Dyelot"),
                        }
                        into m
                        where m.Count() > 1 // 只顯示超過一次以上的
                        select new
                        {
                            poid = m.First().Field<string>("poid"),
                            seq1 = m.First().Field<string>("seq1"),
                            seq2 = m.First().Field<string>("seq2"),
                            Roll = m.First().Field<string>("Roll"),
                            Dyelot = m.First().Field<string>("Dyelot"),
                            count = m.Count(),
                        };
                if (q.ToList().Count > 0)
                {
                    string warning = string.Empty;

                    foreach (var dr in q)
                    {
                        warning += string.Format("{0}-{1}-{2}-{3}-{4}" + Environment.NewLine, dr.poid, dr.seq1, dr.seq2, dr.Roll, dr.Dyelot);
                    }

                    MyUtility.Msg.WarningBox(warning, "Roll# are duplicated!!");
                    return;
                }

                ////刪除表身重新匯入
                // foreach (DataRow del in detailData.ToList())
                // {
                //    detailData.Rows.Remove(del);
                // }
                foreach (DataRow dr2 in tmpPacking.Rows)
                {
                    // 刪除 Import 重複的資料 by SP# Seq Carton#
                    DataRow[] checkRow = this.detailData.AsEnumerable().Where(row => row.RowState != DataRowState.Deleted && row["poid"].EqualString(dr2["poid"])
                                                                                && row["seq1"].EqualString(dr2["seq1"]) && row["seq2"].EqualString(dr2["seq2"])
                                                                                && row["roll"].EqualString(dr2["roll"]) && row["Dyelot"].EqualString(dr2["Dyelot"])).ToArray();

                    // 開始檢查FtyInventory
                    string poid = MyUtility.Convert.GetString(dr2["poid"]);
                    string seq1 = MyUtility.Convert.GetString(dr2["seq1"]);
                    string seq2 = MyUtility.Convert.GetString(dr2["seq2"]);
                    string roll = MyUtility.Convert.GetString(dr2["roll"]);
                    string dyelot = MyUtility.Convert.GetString(dr2["dyelot"]);
                    string fabricType = MyUtility.Convert.GetString(dr2["fabrictype"]);
                    string stockType = MyUtility.Convert.GetString(dr2["stockType"]);


                    // 布料，且都有值了才檢查該Row
                    if (fabricType.ToUpper() == "F" && !MyUtility.Check.Empty(poid) && !MyUtility.Check.Empty(seq1) && !MyUtility.Check.Empty(seq2) && !MyUtility.Check.Empty(roll) && !MyUtility.Check.Empty(dyelot))
                    {
                        bool chkFtyInventory = PublicPrg.Prgs.ChkFtyInventory(poid, seq1, seq2, roll, dyelot, stockType);

                        if (!chkFtyInventory)
                        {
                            MyUtility.Msg.WarningBox($"The Roll & Deylot of <SP#>:{poid}, <Seq>:{seq1} {seq2}, <Roll>:{roll}, <Dyelot>:{dyelot} already exists.");

                            return;
                        }
                    }

                    if (checkRow.Length == 0)
                    {
                        dr2["id"] = this.master["id"];
                        this.detailData.ImportRow(dr2);
                    }
                }
            }
            catch (Exception ex)
            {
                MyUtility.Msg.ErrorBox("Process error.\r\n" + ex.ToString());
                return;
            }

            MyUtility.Msg.InfoBox("Write in completed!!");
            this.DialogResult = DialogResult.OK;
        }
    }
}
