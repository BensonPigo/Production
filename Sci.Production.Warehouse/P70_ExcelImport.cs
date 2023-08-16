﻿using Ict.Win;
using Sci.Production.Class.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P70_ExcelImport : Win.Subs.Base
    {
        private DataTable grid2Data = new DataTable();
        private DataTable dtDetail;

        /// <inheritdoc/>
        public P70_ExcelImport(DataTable detailData)
        {
            this.InitializeComponent();
            this.dtDetail = detailData;
        }

        /// <inheritdoc/>
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
                .Text("Filename", header: "File Name", width: Widths.AnsiChars(20))
                .Text("Status", header: "Status", width: Widths.AnsiChars(45));

            // 取Grid結構
            this.grid2Data.Columns.Add("Poid", typeof(string));
            this.grid2Data.Columns.Add("Seq", typeof(string));
            this.grid2Data.Columns.Add("Seq1", typeof(string));
            this.grid2Data.Columns.Add("Seq2", typeof(string));
            this.grid2Data.Columns.Add("MaterialType", typeof(string));
            this.grid2Data.Columns.Add("Roll", typeof(string));
            this.grid2Data.Columns.Add("Dyelot", typeof(string));
            this.grid2Data.Columns.Add("Weight", typeof(string));
            this.grid2Data.Columns.Add("Tone", typeof(string));
            this.grid2Data.Columns.Add("Qty", typeof(int));
            this.grid2Data.Columns.Add("Unit", typeof(string));
            this.grid2Data.Columns.Add("Location", typeof(string));
            this.grid2Data.Columns.Add("Refno", typeof(string));
            this.grid2Data.Columns.Add("Color", typeof(string));
            this.grid2Data.Columns.Add("Desc", typeof(string));
            this.grid2Data.Columns.Add("StockType", typeof(string));
            this.grid2Data.Columns.Add("ErrMsg", typeof(string));

            this.listControlBindingSource2.DataSource = this.grid2Data;
            this.gridDetail.DataSource = this.listControlBindingSource2;
            this.gridDetail.IsEditingReadOnly = true;
            this.Helper.Controls.Grid.Generator(this.gridDetail)
                .Text("Poid", header: "SP#", width: Widths.AnsiChars(13))
                .Text("Seq", header: "Seq", width: Widths.AnsiChars(6))
                .Text("Desc", header: "Description", width: Widths.AnsiChars(20))
                .Text("Color", header: "Color", width: Widths.AnsiChars(8))
                .Text("Roll", header: "Roll", width: Widths.AnsiChars(8))
                .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(8))
                .Text("Tone", header: "Tone/Grp", width: Widths.AnsiChars(8))
                .Numeric("Qty", header: "Qty")
                .Text("Unit", header: "Unit", width: Widths.AnsiChars(6))
                .Text("Location", header: "Location", width: Widths.AnsiChars(10))
                .Text("ErrMsg", header: "Error Message", width: Widths.AnsiChars(100));

            for (int i = 0; i < this.gridDetail.ColumnCount; i++)
            {
                this.gridDetail.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

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
                        string seq = (string)MyUtility.Excel.GetExcelCellValue(objCellArray[1, 2], "C");
                        string roll = (string)MyUtility.Excel.GetExcelCellValue(objCellArray[1, 3], "C");
                        string dyelot = (string)MyUtility.Excel.GetExcelCellValue(objCellArray[1, 4], "C");
                        string tone = (string)MyUtility.Excel.GetExcelCellValue(objCellArray[1, 5], "C");
                        string qty = (string)MyUtility.Excel.GetExcelCellValue(objCellArray[1, 6], "C");
                        string location = (string)MyUtility.Excel.GetExcelCellValue(objCellArray[1, 7], "C");

                        if (sp.ToUpper() != "SP#" || seq.ToUpper() != "SEQ" || roll.ToUpper() != "ROLL" || dyelot.ToUpper() != "DYELOT" || tone.ToUpper() != "TONE/GRP" || qty.ToUpper() != "QTY" || location.ToUpper() != "LOCATION")
                        {
                            #region 將不存在欄位顯示於status
                            StringBuilder columnName = new StringBuilder();
                            if (sp.ToUpper() != "SP#")
                            {
                                columnName.Append("< SP# >, ");
                            }

                            if (seq.ToUpper() != "SEQ")
                            {
                                columnName.Append("< Seq >, ");
                            }

                            if (roll.ToUpper() != "ROLL")
                            {
                                columnName.Append("< Roll >, ");
                            }

                            if (dyelot.ToUpper() != "DYELOT")
                            {
                                columnName.Append("< Dyelot >, ");
                            }

                            if (tone.ToUpper() != "TONE/GRP")
                            {
                                columnName.Append("< Tone/Grp >, ");
                            }

                            if (qty.ToUpper() != "QTY")
                            {
                                columnName.Append("< Qty >, ");
                            }

                            if (location.ToUpper() != "LOCATION")
                            {
                                columnName.Append("< Location >, ");
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
                            bool allPass = true;
                            List<string> errStr = new List<string>();

                            while (intRowsRead < intRowsCount)
                            {
                                intRowsRead++;

                                range = worksheet.Range[string.Format("A{0}:G{0}", intRowsRead)];
                                objCellArray = range.Value;

                                DataRow newRow = this.grid2Data.NewRow();
                                newRow["POID"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 1], "C");

                                #region Check SP is exists
                                if (!this.CheckSP(newRow))
                                {
                                    errStr.Add($@"Cannot found SP# <{newRow["POID"]}>. ");
                                    allPass = false;
                                }
                                #endregion

                                #region Check Seq is exists

                                var excelSeq = (string)MyUtility.Excel.GetExcelCellValue(objCellArray[1, 2], "C");
                                int seqIndex = excelSeq.IndexOf(' ');
                                string seq1 = excelSeq.Substring(0, seqIndex);
                                string seq2 = excelSeq.Substring(seqIndex + 1);

                                newRow["Seq"] = excelSeq;
                                newRow["Seq1"] = seq1;
                                newRow["Seq2"] = seq2;

                                if (!this.CheckSeq(newRow))
                                {
                                    errStr.Add($@"SP#: {newRow["POID"]} Cannot found Seq <{newRow["Seq"]}>. ");
                                    allPass = false;
                                }
                                #endregion

                                #region Qty cannot be empty or zero
                                newRow["Qty"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 6], "N");

                                if (MyUtility.Check.Empty(MyUtility.Excel.GetExcelCellValue(objCellArray[1, 6], "N")) || MyUtility.Excel.GetExcelCellValue(objCellArray[1, 6], "N").EqualString("0"))
                                {
                                    errStr.Add($@"Qty cannot be empty or zero. ");
                                    allPass = false;
                                }
                                #endregion

                                #region Location mapping Table MtlLocation and split","
                                newRow["Location"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 7], "C");

                                if (!this.CheckLocation(newRow))
                                {
                                    errStr.Add($@"Cannot found Location <{newRow["Location"]}> ");
                                    allPass = false;
                                }
                                #endregion

                                newRow["Roll"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 3], "C");
                                newRow["Dyelot"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 4], "C");
                                newRow["Tone"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 5], "C");

                                newRow["Desc"] = string.Empty;
                                newRow["Color"] = string.Empty;
                                newRow["Weight"] = 0;
                                newRow["Unit"] = string.Empty;
                                newRow["Refno"] = string.Empty;
                                newRow["MaterialType"] = string.Empty;

                                if (!MyUtility.Check.Empty(MyUtility.Excel.GetExcelCellValue(objCellArray[1, 1], "C")) &&
                                    !MyUtility.Check.Empty(MyUtility.Excel.GetExcelCellValue(objCellArray[1, 2], "C")))
                                {
                                    string seekCmd = $@"
                                    select 
                                    [Desc],
                                    Color,
                                    Unit,
                                    Refno,
                                    [MaterialType] = IIF(FabricType = 'F',Concat ('Fabric-', MtlType),Concat ('Accessory-', MtlType))
                                    from LocalOrderMaterial
                                    where POID='{newRow["Poid"]}' and Seq1 = '{newRow["Seq1"]}' and Seq2 = '{newRow["Seq2"]}'";
                                    if (MyUtility.Check.Seek(seekCmd, out DataRow drs))
                                    {
                                        newRow["Desc"] = drs["Desc"];
                                        newRow["Color"] = drs["Color"];
                                        newRow["Unit"] = drs["Unit"];
                                        newRow["Refno"] = drs["Refno"];
                                        newRow["MaterialType"] = drs["MaterialType"];
                                    }
                                }

                                newRow["ErrMsg"] = errStr.JoinToString("\r\n");
                                this.grid2Data.Rows.Add(newRow);
                                errStr.Clear();
                            }

                            if (allPass)
                            {
                                dr["Status"] = "Check & Import Completed.";
                            }
                            else
                            {
                                dr["Status"] = "Some data import failed!!";
                            }
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

        private bool CheckLocation(DataRow dr)
        {
            bool pass = true;
            string sqlcmd = $@"
            select * 
            from dbo.SplitString('{dr["Location"]}',',') t
            left join MtlLocation m on t.Data = m.ID
            where m.ID is null";
            if (MyUtility.Check.Seek(sqlcmd))
            {
                pass = false;
            }

            return pass;
        }

        private bool CheckSP(DataRow dr)
        {
            bool pass = true;
            string sqlcmd = $@"select 1 from Orders where id='{dr["POID"]}'";
            if (!MyUtility.Check.Seek(sqlcmd))
            {
                pass = false;
            }

            return pass;
        }

        private bool CheckSeq(DataRow dr)
        {
            bool pass = true;
            string sqlcmd = $@"select 1 from LocalOrderMaterial where Poid = '{dr["POID"]}' and Seq1='{dr["Seq1"]}' and Seq2='{dr["Seq2"]}' ";
            if (!MyUtility.Check.Seek(sqlcmd))
            {
                pass = false;
            }

            return pass;
        }

        private void BtnWriteIn_Click(object sender, EventArgs e)
        {
            foreach (DataRow dr in ((DataTable)this.listControlBindingSource2.DataSource).Rows)
            {
                if (MyUtility.Check.Empty(dr["ErrMsg"]))
                {
                    DataRow insertRow = this.dtDetail.NewRow();
                    int dtCount = 0;
                    int index = 0;
                    if (this.dtDetail.Rows.Count > 1)
                    {
                        var rows = this.dtDetail.Select($"POID = '{dr["Poid"]}' and Seq1 = '{dr["Seq1"]}' and Seq2 = '{dr["Seq2"]}' and Roll = '{dr["Roll"]}' and Dyelot = '{dr["Dyelot"]}' and StockType = 'B'");

                        if (rows.Length != 0)
                        {
                            index = this.dtDetail.Rows.IndexOf(rows[0]);
                            dtCount = rows.Length;
                        }
                    }

                    if (dtCount == 0)
                    {
                        insertRow["Poid"] = dr["Poid"];
                        insertRow["Seq"] = dr["Seq"];
                        insertRow["Seq1"] = dr["Seq1"];
                        insertRow["Seq2"] = dr["Seq2"];
                        insertRow["StockType"] = "B";
                        insertRow["Roll"] = dr["Roll"];
                        insertRow["Dyelot"] = dr["Dyelot"];
                        insertRow["Refno"] = dr["Refno"];
                        insertRow["MaterialType"] = dr["MaterialType"];
                        insertRow["Weight"] = dr["Weight"];
                        insertRow["Color"] = dr["Color"];
                        insertRow["Tone"] = dr["Tone"];
                        insertRow["Qty"] = dr["Qty"];
                        insertRow["Unit"] = dr["Unit"];
                        insertRow["Location"] = dr["Location"];
                        this.dtDetail.Rows.Add(insertRow);
                    }
                    else
                    {
                        this.dtDetail.Rows[index]["Roll"] = dr["Roll"];
                        this.dtDetail.Rows[index]["Dyelot"] = dr["Dyelot"];
                        this.dtDetail.Rows[index]["Tone"] = dr["Tone"];
                        this.dtDetail.Rows[index]["Qty"] = dr["Qty"];
                        this.dtDetail.Rows[index]["Unit"] = dr["Unit"];
                        this.dtDetail.Rows[index]["Location"] = dr["Location"];
                    }

                }
            }

            MyUtility.Msg.InfoBox("Import complete.	");
            this.DialogResult = DialogResult.OK;
        }

        private void Btn_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
