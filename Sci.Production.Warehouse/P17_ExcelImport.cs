﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using System.Linq;
using System.Runtime.InteropServices;
using System.Data.SqlClient;
using Excel = Microsoft.Office.Interop.Excel;
using Sci.Production.PublicPrg;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P17_ExcelImport : Win.Subs.Base
    {
        private DataTable grid2Data = new DataTable();
        private DataTable detailData;
        private DataRow master;

        /// <inheritdoc/>
        public P17_ExcelImport(DataRow master, DataTable detailData)
        {
            this.InitializeComponent();
            this.detailData = detailData;
            this.master = master;
        }

        /// <inheritdoc/>
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
            this.grid2Data.Columns.Add("ID", typeof(string));
            this.grid2Data.Columns.Add("poid", typeof(string));
            this.grid2Data.Columns.Add("seq", typeof(string));
            this.grid2Data.Columns.Add("seq1", typeof(string));
            this.grid2Data.Columns.Add("seq2", typeof(string));
            this.grid2Data.Columns.Add("Roll", typeof(string));
            this.grid2Data.Columns.Add("Dyelot", typeof(string));
            this.grid2Data.Columns.Add("Description", typeof(string));
            this.grid2Data.Columns.Add("stockunit", typeof(string));
            this.grid2Data.Columns.Add("qty", typeof(decimal));
            this.grid2Data.Columns.Add("Location", typeof(string));
            this.grid2Data.Columns.Add("ErrMsg", typeof(string));
            this.grid2Data.Columns.Add("CanWriteIn", typeof(bool));
            this.grid2Data.Columns.Add("MDivisionID", typeof(string));
            this.grid2Data.Columns.Add("Stocktype", typeof(string));
            this.grid2Data.Columns.Add("ftyinventoryukey", typeof(string));

            this.listControlBindingSource2.DataSource = this.grid2Data;
            this.gridPoid.DataSource = this.listControlBindingSource2;
            this.gridPoid.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.gridPoid)
                .Text("poid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("seq1", header: "Seq1", width: Widths.AnsiChars(4))
                .Text("seq2", header: "Seq2", width: Widths.AnsiChars(3))
                .Text("Roll", header: "Roll#", width: Widths.AnsiChars(9), iseditingreadonly: false)
                .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: false)
                .Numeric("qty", header: "Return Qty", width: Widths.AnsiChars(10), decimal_places: 2, integer_places: 10, iseditingreadonly: true)
                .Text("Location", header: "Bulk Location", iseditingreadonly: false)
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
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // 開窗且有選擇檔案
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
                if (MyUtility.Check.Empty(dr["Filename"]))
                {
                    continue;
                }

                if (!System.IO.File.Exists(MyUtility.Convert.GetString(dr["FullFileName"])))
                {
                    dr["Status"] = string.Format("Excel file not found < {0} >.", MyUtility.Convert.GetString(dr["Filename"]));
                    continue;
                }

                Excel.Application excel;
                try
                {
                    excel = new Excel.Application();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    dr["Status"] = string.Format("Not able to open excel file < {0} >.", MyUtility.Convert.GetString(dr["Filename"]));
                    continue;
                }

                excel.Workbooks.Open(MyUtility.Convert.GetString(dr["FullFileName"]));
                excel.Visible = false;
                Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];
                int intColumnsCount = worksheet.UsedRange.Columns.Count;

                if (intColumnsCount >= 30)
                {
                    Marshal.ReleaseComObject(worksheet);
                    excel.ActiveWorkbook.Close(false, Type.Missing, Type.Missing);

                    // excel.Workbooks.Close();
                    excel.Quit();
                    Marshal.ReleaseComObject(excel);
                    excel = null;
                    dr["Status"] = "Column count can not more than 30!!";
                    continue;
                }

                // 檢查Excel格式
                Excel.Range range = worksheet.Range[string.Format("A{0}:AE{0}", 2)];
                object[,] objCellArray = range.Value;
                string[] itemCheck = { "SP#", "SEQ1", "SEQ2", "Roll", "Dyelot", "Return Qty", "Bulk Location" };
                int[] itemPosition = new int[itemCheck.Length];
                string[] excelItem = new string[intColumnsCount + 1];

                for (int y = 1; y <= intColumnsCount; y++)
                {
                    excelItem[y] = (string)MyUtility.Excel.GetExcelCellValue(objCellArray[1, y], "C").ToString();
                }

                StringBuilder columnName = new StringBuilder();

                // 確認Excel各Item是否存在，並儲存所在位置
                for (int x = 0; x < itemCheck.Length; x++)
                {
                    for (int y = 1; y <= intColumnsCount; y++)
                    {
                        if (excelItem[y] == itemCheck[x])
                        {
                            itemPosition[x] = y;
                            break;
                        }
                    }

                    if (MyUtility.Check.Empty(itemPosition[x]))
                    {
                        columnName.Append("< " + itemCheck[x].ToString() + " >, ");
                    }
                }

                if (!MyUtility.Check.Empty(columnName.Length))
                {
                    dr["Status"] = columnName.ToString().Substring(0, columnName.ToString().Length - 2) + "column not found in the excel.";
                    Marshal.ReleaseComObject(worksheet);
                    continue;
                }

                int intRowsCount = worksheet.UsedRange.Rows.Count;
                int intRowsStart = 3;
                int intRowsRead = intRowsStart - 1;

                bool isLocationExists = false;

                while (intRowsRead < intRowsCount)
                {
                    intRowsRead++;
                    range = worksheet.Range[string.Format("A{0}:Z{0}", intRowsRead)];
                    objCellArray = range.Value;
                    Dictionary<string, bool> listNewRowErrMsg = new Dictionary<string, bool>();

                    DataRow newRow = this.grid2Data.NewRow();
                    string seq1 = (objCellArray[1, itemPosition[1]] == null) ? string.Empty : MyUtility.Excel.GetExcelCellValue(objCellArray[1, itemPosition[1]].ToString().Trim(), "C").ToString();
                    string seq2 = (objCellArray[1, itemPosition[2]] == null) ? string.Empty : MyUtility.Excel.GetExcelCellValue(objCellArray[1, itemPosition[2]].ToString().Trim(), "C").ToString();

                    // Location處理
                    string oriLocation = (objCellArray[1, itemPosition[6]] == null) ? string.Empty : MyUtility.Excel.GetExcelCellValue(objCellArray[1, itemPosition[6]].ToString().Trim(), "C").ToString();
                    string locations = oriLocation.Split(',').ToList().Where(o => !MyUtility.Check.Empty(o)).Distinct().JoinToString(",");
                    string notLocationExistsList = locations.Split(',').ToList().Where(o => !Prgs.CheckLocationExists("B", o)).JoinToString(",");

                    if (MyUtility.Check.Empty(notLocationExistsList))
                    {
                        isLocationExists = true;
                    }
                    else
                    {
                        isLocationExists = false;
                    }

                    newRow["poid"] = (objCellArray[1, itemPosition[0]] == null) ? string.Empty : MyUtility.Excel.GetExcelCellValue(objCellArray[1, itemPosition[0]].ToString().Trim(), "C");
                    newRow["seq"] = seq1 + " " + seq2;
                    newRow["seq1"] = seq1;
                    newRow["seq2"] = seq2;
                    newRow["Roll"] = (objCellArray[1, itemPosition[3]] == null) ? string.Empty : MyUtility.Excel.GetExcelCellValue(objCellArray[1, itemPosition[3]].ToString().Trim(), "C");
                    newRow["Dyelot"] = (objCellArray[1, itemPosition[4]] == null) ? string.Empty : MyUtility.Excel.GetExcelCellValue(objCellArray[1, itemPosition[4]].ToString().Trim(), "C").ToString();
                    newRow["qty"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, itemPosition[5]], "N");
                    newRow["CanWriteIn"] = true;
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
                        listColumnLengthErrMsg.Add("<Roll> length can't be more than 8 Characters.");
                    }

                    // Dyelot varchar(8)
                    if (Encoding.Default.GetBytes(newRow["Dyelot"].ToString()).Length > 8)
                    {
                        listColumnLengthErrMsg.Add("<Dyelot> length can't be more than 8 Characters.");
                    }

                    // Bulk Location varchar(8)
                    if (Encoding.Default.GetBytes(newRow["Location"].ToString()).Length > 200)
                    {
                        listColumnLengthErrMsg.Add("<Bulk Location> length can't be more than 200 Characters.");
                    }

                    // Qty  numeric (11, 2)
                    if (MyUtility.Convert.GetInt(newRow["qty"].ToString()) <= 0)
                    {
                        listColumnLengthErrMsg.Add("<Qty> value must be more than 0");
                    }

                    if (MyUtility.Convert.GetInt(newRow["qty"].ToString()) > 999999999)
                    {
                        listColumnLengthErrMsg.Add("<Qty> value can't be more than 999,999,999");
                    }

                    if (listColumnLengthErrMsg.Count > 0)
                    {
                        listNewRowErrMsg.Add(listColumnLengthErrMsg.JoinToString(Environment.NewLine), false);
                    }
                    #endregion

                    #region P17表身檢查
                    if (MyUtility.Check.Empty(newRow["seq1"]) || MyUtility.Check.Empty(newRow["seq2"]))
                    {
                        listNewRowErrMsg.Add(string.Format(@"SP#: {0} Seq#: {1}-{2} can't be empty", newRow["poid"], newRow["seq1"], newRow["seq2"]), false);
                    }

                    List<SqlParameter> sqlpar = new List<SqlParameter>();
                    sqlpar.Add(new SqlParameter("@POID", newRow["poid"].ToString().Trim()));
                    sqlpar.Add(new SqlParameter("@Seq1", newRow["seq1"].ToString().Trim()));
                    sqlpar.Add(new SqlParameter("@Seq2", newRow["seq2"].ToString().Trim()));
                    sqlpar.Add(new SqlParameter("@Roll", newRow["roll"].ToString().Trim()));
                    sqlpar.Add(new SqlParameter("@Dyelot", newRow["dyelot"].ToString().Trim()));
                    sqlpar.Add(new SqlParameter("@Location", newRow["Location"].ToString().Trim()));
                    sqlpar.Add(new SqlParameter("@MDivisionID", Env.User.Keyword));
                    DataRow dr2;
                    string sql = @"
select fi.*
	,[Description] = dbo.getmtldesc(fi.POID,fi.seq1, fi.seq2, 2, 0)
	,[StockUnit] = dbo.GetStockUnitBySPSeq(fi.POID, fi.seq1, fi.seq2)
	,[Location] = @Location
from ftyinventory fi
inner join Orders o on fi.POID = o.id
inner join Factory f on o.FtyGroup = f.id
where fi.StockType = 'B'
and fi.POID = @POID
and fi.Seq1 = @Seq1
and fi.Seq2 = @Seq2
and fi.Roll = @Roll
and fi.Dyelot = @Dyelot
and f.MDivisionID = @MDivisionID ";
                    bool result = MyUtility.Check.Seek(sql, sqlpar, out dr2);
                    if (result && isLocationExists)
                    {
                        newRow["Description"] = dr2["Description"];
                        newRow["StockUnit"] = dr2["StockUnit"];
                        newRow["Location"] = dr2["Location"];
                        newRow["ftyinventoryukey"] = dr2["Ukey"];
                        newRow["Location"] = dr2["Location"];
                    }
                    else
                    {
                        if (!result && isLocationExists)
                        {
                            listNewRowErrMsg.Add(
                                string.Format(
                                "SP#:{0}-Seq1:{1}-Seq2:{2}-Roll:{3}-Dyelot:{4}-Location:{5} is not found!!",
                                newRow["poid"],
                                newRow["seq1"],
                                newRow["seq2"],
                                newRow["roll"],
                                newRow["dyelot"],
                                newRow["Location"]), false);
                        }
                        else if (isLocationExists)
                        {
                            listNewRowErrMsg.Add(
                                string.Format(
                                "SP#:{0}-Seq1:{1}-Seq2:{2}-Roll:{3}-Dyelot:{4} is not found!!",
                                newRow["poid"],
                                newRow["seq1"],
                                newRow["seq2"],
                                newRow["roll"],
                                newRow["dyelot"]), false);
                        }
                        else
                        {
                            listNewRowErrMsg.Add(
                                string.Format(
                                "Location:{0} is not found!!",
                                newRow["Location"]), false);
                        }
                    }

                    #endregion

                    if (listNewRowErrMsg.Count == 0)
                    {
                        count++;
                    }
                    else
                    {
                        newRow["ErrMsg"] = listNewRowErrMsg.Select(s => s.Key).JoinToString(Environment.NewLine);
                    }

                    bool canNotWriteIn = listNewRowErrMsg.Any(s => s.Value == false);
                    if (canNotWriteIn)
                    {
                        newRow["CanWriteIn"] = false;
                    }

                    newRow["MDivisionID"] = Env.User.Keyword;
                    newRow["Stocktype"] = "B";

                    this.grid2Data.Rows.Add(newRow);
                }

                // -2 是要扣掉標題的Row數
                dr["Status"] = (intRowsCount - 2 == count) ? "Check & Import Completed." : "Some Data Faild. Please check Error Message.";
                dr["Count"] = count;

                Marshal.ReleaseComObject(worksheet);
                excel.ActiveWorkbook.Close(false, Type.Missing, Type.Missing);

                // excel.Workbooks.Close();
                excel.Quit();
                Marshal.ReleaseComObject(excel);
                excel = null;
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
            var tmpPacking = ((DataTable)this.listControlBindingSource2.DataSource).AsEnumerable();

            // 如果資料中有錯誤不能WriteIn
            if (tmpPacking.Any(s => (bool)s["CanWriteIn"] == false))
            {
                MyUtility.Msg.WarningBox("Import data error, please check column [Error Message] information to fix Excel.");
                return;
            }

            try
            {
                var q = from p in tmpPacking
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
                            Seq1 = m.First().Field<string>("seq1"),
                            Seq2 = m.First().Field<string>("seq2"),
                            Roll = m.First().Field<string>("Roll"),
                            Dyelot = m.First().Field<string>("Dyelot"),
                            count = m.Count(),
                        };
                if (q.ToList().Count > 0)
                {
                    string warning = string.Empty;

                    foreach (var dr in q)
                    {
                        warning += string.Format("{0}-{1}-{2}-{3}-{4}" + Environment.NewLine, dr.poid, dr.Seq1, dr.Seq2, dr.Roll, dr.Dyelot);
                    }

                    MyUtility.Msg.WarningBox(warning, "Roll# are duplicated!!");
                    return;
                }

                foreach (DataRow dr2 in tmpPacking)
                {
                    // 刪除 Import 重複的資料 by SP# Seq Carton#
                    DataRow[] checkRow = this.detailData.AsEnumerable().Where(row => row.RowState != DataRowState.Deleted && row["poid"].EqualString(dr2["poid"])
                                                                                && row["seq1"].EqualString(dr2["seq1"]) && row["seq2"].EqualString(dr2["seq2"])
                                                                                && row["roll"].EqualString(dr2["roll"]) && row["Dyelot"].EqualString(dr2["Dyelot"])).ToArray();

                    // 若沒有相同則寫入，有相同則更新
                    if (checkRow.Length == 0)
                    {
                        dr2["id"] = this.master["id"];
                        this.detailData.ImportRow(dr2);
                    }
                    else
                    {
                        foreach (var item in checkRow)
                        {
                            item["Qty"] = dr2["Qty"];
                            item["Location"] = dr2["Location"];
                        }
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

        private void BtnDownloadTempExcel_Click(object sender, EventArgs e)
        {
            Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Warehouse_P17_ExcelImport.xltx"); // 預先開啟excel app
            string strExcelName = Class.MicrosoftFile.GetName("Warehouse_P17_ExcelImport");
            objApp.ActiveWorkbook.SaveAs(strExcelName);
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);

            strExcelName.OpenFile();
        }
    }
}
