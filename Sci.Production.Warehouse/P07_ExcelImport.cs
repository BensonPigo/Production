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
using Sci.Data;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P07_ExcelImport : Win.Subs.Base
    {
        private DataTable grid2Data = new DataTable();
        private DataTable detailData;
        private DataRow master;
        private DataRow newRow;
        private List<string> listNewRowErrMsg;

        /// <inheritdoc/>
        public P07_ExcelImport(DataRow master, DataTable detailData)
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
            this.grid2Data.Columns.Add("FullRoll", typeof(string));
            this.grid2Data.Columns.Add("dyelot", typeof(string));
            this.grid2Data.Columns.Add("FullDyelot", typeof(string));
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
            this.grid2Data.Columns.Add("MINDQRCode", typeof(string));
            this.grid2Data.Columns.Add("Remark", typeof(string));
            this.grid2Data.Columns.Add("ErrMsg", typeof(string));
            this.grid2Data.Columns.Add("ErrMsgType", typeof(string));
            this.grid2Data.Columns.Add("fabrictype", typeof(string));
            this.grid2Data.Columns.Add("Refno", typeof(string));
            this.grid2Data.Columns.Add("ColorID", typeof(string));
            this.grid2Data.Columns.Add("MtlTypeID", typeof(string));

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
                .Text("FullRoll", header: "Full Carton#", width: Widths.AnsiChars(16))
                .Text("dyelot", header: "Lot No", width: Widths.AnsiChars(4))
                .Text("FullDyelot", header: "Full Lot No", width: Widths.AnsiChars(8))
                .Text("pounit", header: "Po Unit", width: Widths.AnsiChars(4))
                .Text("stockunit", header: "Stock Unit", width: Widths.AnsiChars(4))
                .Numeric("Qty", header: "Qty", decimal_places: 2, settings: ns)
                .Numeric("foc", header: "F.O.C", decimal_places: 2, settings: ns2)
                .Numeric("weight", header: "WeiKg", decimal_places: 2)
                .Numeric("actualWeight", header: "NetKg", decimal_places: 2)
                .Text("location", header: "Location", width: Widths.AnsiChars(8))
                .Text("MINDQRCode", header: "MIND QR Code", width: Widths.AnsiChars(30))
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
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
                        Microsoft.Office.Interop.Excel.Range range = worksheet.Range[string.Format("A{0}:AF{0}", 1)];
                        object[,] objCellArray = range.Value;
                        int[] itemPosition = new int[16];
                        string[] itemCheck = { string.Empty, "WK#", "SP#", "SEQ1", "SEQ2", "C/NO", "FULL C/NO", "LOT NO.", "FULL LOT NO.", "QTY", "F.O.C", "NETKG", "WEIKG", "LOCATION", "MIND QR CODE", "REMARK" };
                        string[] excelItem = new string[intColumnsCount + 1];

                        for (int y = 1; y <= intColumnsCount; y++)
                        {
                            excelItem[y] = (string)MyUtility.Excel.GetExcelCellValue(objCellArray[1, y], "C").ToString().ToUpper();
                        }

                        StringBuilder columnName = new StringBuilder();

                        // 確認Excel各Item是否存在，並儲存所在位置
                        for (int x = 1; x <= 15; x++)
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
                                this.listNewRowErrMsg = new List<string>();

                                this.newRow = this.grid2Data.NewRow();
                                this.newRow["wkno"] = (objCellArray[1, itemPosition[1]] == null) ? string.Empty : MyUtility.Excel.GetExcelCellValue(objCellArray[1, itemPosition[1]].ToString().Trim(), "C");
                                this.newRow["poid"] = (objCellArray[1, itemPosition[2]] == null) ? string.Empty : MyUtility.Excel.GetExcelCellValue(objCellArray[1, itemPosition[2]].ToString().ToUpper().Trim(), "C");
                                this.newRow["seq1"] = (objCellArray[1, itemPosition[3]] == null) ? string.Empty : MyUtility.Excel.GetExcelCellValue(objCellArray[1, itemPosition[3]].ToString().Trim(), "C");
                                this.newRow["seq2"] = (objCellArray[1, itemPosition[4]] == null) ? string.Empty : MyUtility.Excel.GetExcelCellValue(objCellArray[1, itemPosition[4]].ToString().Trim(), "C");
                                this.newRow["seq"] = ((objCellArray[1, itemPosition[3]] == null) ? string.Empty : MyUtility.Excel.GetExcelCellValue(objCellArray[1, itemPosition[3]].ToString().Trim(), "C").ToString()) + " " + ((objCellArray[1, itemPosition[4]] == null) ? string.Empty : MyUtility.Excel.GetExcelCellValue(objCellArray[1, itemPosition[4]].ToString().Trim(), "C").ToString());
                                this.newRow["PoIdSeq1"] = ((objCellArray[1, itemPosition[2]] == null) ? string.Empty : MyUtility.Excel.GetExcelCellValue(objCellArray[1, itemPosition[2]].ToString().Trim(), "C")) + ((objCellArray[1, itemPosition[3]] == null) ? string.Empty : MyUtility.Excel.GetExcelCellValue(objCellArray[1, itemPosition[3]].ToString().Trim(), "C").ToString());
                                this.newRow["PoIdSeq"] = ((objCellArray[1, itemPosition[2]] == null) ? string.Empty : MyUtility.Excel.GetExcelCellValue(objCellArray[1, itemPosition[2]].ToString().Trim(), "C")) + ((objCellArray[1, itemPosition[3]] == null) ? string.Empty : MyUtility.Excel.GetExcelCellValue(objCellArray[1, itemPosition[3]].ToString().Trim(), "C").ToString().PadRight(3)) + ((objCellArray[1, itemPosition[4]] == null) ? string.Empty : MyUtility.Excel.GetExcelCellValue(objCellArray[1, itemPosition[4]].ToString().Trim(), "C").ToString());
                                this.newRow["roll"] = (objCellArray[1, itemPosition[5]] == null) ? string.Empty : MyUtility.Excel.GetExcelCellValue(objCellArray[1, itemPosition[5]].ToString().Replace("'", string.Empty).Trim(), "C");
                                this.newRow["FullRoll"] = (objCellArray[1, itemPosition[6]] == null) ? string.Empty : MyUtility.Excel.GetExcelCellValue(objCellArray[1, itemPosition[6]].ToString().Replace("'", string.Empty).Trim(), "C");
                                this.newRow["dyelot"] = (objCellArray[1, itemPosition[7]] == null) ? string.Empty : MyUtility.Excel.GetExcelCellValue(objCellArray[1, itemPosition[7]].ToString().Replace("'", string.Empty).Trim(), "C");
                                this.newRow["FullDyelot"] = (objCellArray[1, itemPosition[8]] == null) ? string.Empty : MyUtility.Excel.GetExcelCellValue(objCellArray[1, itemPosition[8]].ToString().Replace("'", string.Empty).Trim(), "C");
                                this.newRow["qty"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, itemPosition[9]], "N");
                                this.newRow["foc"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, itemPosition[10]], "N");
                                this.newRow["shipqty"] = decimal.Parse(this.newRow["qty"].ToString()) + decimal.Parse(this.newRow["foc"].ToString().Trim());
                                this.newRow["actualqty"] = decimal.Parse(this.newRow["qty"].ToString()) + decimal.Parse(this.newRow["foc"].ToString().Trim());
                                this.newRow["actualWeight"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, itemPosition[11]], "N");
                                this.newRow["Weight"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, itemPosition[12]], "N");
                                this.newRow["location"] = (objCellArray[1, itemPosition[13]] == null) ? string.Empty : MyUtility.Excel.GetExcelCellValue(objCellArray[1, itemPosition[13]].ToString().Trim(), "C");
                                this.newRow["MINDQRCode"] = (objCellArray[1, itemPosition[14]] == null) ? string.Empty : MyUtility.Excel.GetExcelCellValue(objCellArray[1, itemPosition[14]].ToString().Trim(), "C");
                                this.newRow["Remark"] = (objCellArray[1, itemPosition[15]] == null) ? string.Empty : MyUtility.Excel.GetExcelCellValue(objCellArray[1, itemPosition[15]].ToString().Trim(), "C");

                                this.CheckData();

                                if (this.listNewRowErrMsg.Count == 0)
                                {
                                    count++;
                                }
                                else
                                {
                                    this.newRow["ErrMsg"] = this.listNewRowErrMsg.JoinToString(Environment.NewLine);
                                }

                                this.grid2Data.Rows.Add(this.newRow);
                            }

                            var dupMINDQRCode = this.grid2Data.AsEnumerable()
                                .Where(w => !MyUtility.Check.Empty(w["MINDQRCode"]))
                                .GroupBy(g => MyUtility.Convert.GetString(g["MINDQRCode"]))
                                .Select(s => new { MINDQRCode = s.Key, ct = s.Count() })
                                .Where(w => w.ct > 1).ToList();
                            foreach (var item in dupMINDQRCode)
                            {
                                foreach (var dupqr in this.grid2Data.Select($"MINDQRCode = '{item.MINDQRCode}'"))
                                {
                                    if (MyUtility.Check.Empty(dupqr["ErrMsg"]))
                                    {
                                        count--;
                                    }

                                    dupqr["ErrMsg"] += "\r\n" + $"This QR Code already exist WK#{this.master["InvNo"]}, cannot import.";
                                }
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
                DataRow curDataRow = this.gridPoid.GetDataRow(dr.Index);
                switch (curDataRow["ErrMsgType"].ToString())
                {
                    case "Hint":
                        dr.DefaultCellStyle.ForeColor = Color.Blue;
                        break;
                    case "Error":
                        dr.DefaultCellStyle.ForeColor = Color.Red;
                        break;
                    default:
                        dr.DefaultCellStyle.ForeColor = Color.Black;
                        break;
                }
            }
        }

        // Write in
        private void BtnWriteIn_Click(object sender, EventArgs e)
        {
            DataTable tmpPacking = (DataTable)this.listControlBindingSource2.DataSource;

            foreach (DataRow dr in tmpPacking.Rows)
            {
                this.newRow = dr;
                this.listNewRowErrMsg.Clear();
                this.CheckData();

                if (this.listNewRowErrMsg.Count == 0)
                {
                    dr["ErrMsg"] = string.Empty;
                }
                else
                {
                    dr["ErrMsg"] = this.listNewRowErrMsg.JoinToString(Environment.NewLine);
                }
            }

            // 如果資料中有錯誤不能WriteIn
            if (tmpPacking.AsEnumerable().Any(s => !MyUtility.Check.Empty(s["ErrMsg"].ToString())))
            {
                MyUtility.Msg.WarningBox("please check column [Error Message] information to fix Excel.");
                return;
            }

            try
            {
                var q = from p in tmpPacking.AsEnumerable()
                        group p by new
                        {
                            poid = p.Field<string>("poid").Trim(),
                            seq1 = p.Field<string>("seq1").Trim(),
                            seq2 = p.Field<string>("seq2").Trim(),
                            Roll = p.Field<string>("Roll").Trim(),
                            Dyelot = p.Field<string>("Dyelot").Trim(),
                        }
                        into m
                        where m.Count() > 1 // 只顯示超過一次以上的
                        select new
                        {
                            poid = m.First().Field<string>("poid").Trim(),
                            Seq1 = m.First().Field<string>("seq1").Trim(),
                            Seq2 = m.First().Field<string>("seq2").Trim(),
                            Roll = m.First().Field<string>("Roll").Trim(),
                            Dyelot = m.First().Field<string>("Dyelot").Trim(),
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

                if (tmpPacking.AsEnumerable().Where(w => w["ErrMsg"].ToString().Contains("This QR Code aleady exist")).Any() ||
                    tmpPacking.AsEnumerable().GroupBy(g => MyUtility.Convert.GetString(g["MINDQRCode"].ToString()))
                    .Any(a => !MyUtility.Check.Empty(a.Key) && a.Count() > 1))
                {
                    MyUtility.Msg.WarningBox("QR Code are duplicated!!");
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

        /// <summary>
        /// 共用檢查和調整匯入的資料
        /// </summary>
        private void CheckData()
        {
            List<string> listNewRowHintMsg = new List<string>();
            #region check Columns length
            List<string> listColumnLengthErrMsg = new List<string>();

            // Poid varchar(13)
            if (Encoding.Default.GetBytes(this.newRow["poid"].ToString()).Length > 13)
            {
                listColumnLengthErrMsg.Add("<SP#> length can't be more than 13 Characters.");
            }

            // Seq1 varchar(3)
            if (Encoding.Default.GetBytes(this.newRow["Seq1"].ToString()).Length > 3)
            {
                listColumnLengthErrMsg.Add("<SEQ1> length can't be more than 3 Characters.");
            }

            // Seq2 varchar(2)
            if (Encoding.Default.GetBytes(this.newRow["Seq2"].ToString()).Length > 2)
            {
                listColumnLengthErrMsg.Add("<SEQ2> length can't be more than 2 Characters.");
            }

            // Roll varchar(8)
            if (Encoding.Default.GetBytes(this.newRow["Roll"].ToString()).Length > 8)
            {
                listColumnLengthErrMsg.Add("<C/No> length can't be more than 8 Characters.");
            }

            // FullRoll varchar(50)
            if (Encoding.Default.GetBytes(this.newRow["FullRoll"].ToString()).Length > 50)
            {
                listColumnLengthErrMsg.Add("<Full C/No> length can't be more than 50 characters.");
            }

            // Dyelot varchar(8)
            if (Encoding.Default.GetBytes(this.newRow["Dyelot"].ToString()).Length > 8)
            {
                listColumnLengthErrMsg.Add("<LOT NO.> length can't be more than 8 Characters.");
            }

            // FullDyelot varchar(50)
            if (Encoding.Default.GetBytes(this.newRow["FullDyelot"].ToString()).Length > 50)
            {
                listColumnLengthErrMsg.Add("<Full LOT NO.> length can't be more than 50 characters.");
            }

            // qty + foc  numeric (11, 2)
            if (decimal.Parse(this.newRow["qty"].ToString()) + decimal.Parse(this.newRow["foc"].ToString()) > 999999999)
            {
                listColumnLengthErrMsg.Add("<Qty + F.O.C> value can't be more than 999,999,999");
            }

            // actualWeight numeric (7, 2)
            if (decimal.Parse(this.newRow["actualWeight"].ToString()) > 99999)
            {
                listColumnLengthErrMsg.Add("<NetKg> value can't be more than 99,999");
            }

            // Weight numeric (7, 2)
            if (decimal.Parse(this.newRow["Weight"].ToString()) > 99999)
            {
                listColumnLengthErrMsg.Add("<WeiKg> value can't be more than 99,999");
            }

            // Location varchar(60)
            if (Encoding.Default.GetBytes(this.newRow["Location"].ToString()).Length > 60)
            {
                listColumnLengthErrMsg.Add("<Location> length can't be more than 60 Characters.");
            }

            // MINDQRCode varchar(80)
            if (Encoding.Default.GetBytes(this.newRow["MINDQRCode"].ToString()).Length > 80)
            {
                listColumnLengthErrMsg.Add("<MINDQRCode> length can't be more than 80 Characters.");
            }

            // Remark nvarchar(100)
            if (Encoding.Default.GetBytes(this.newRow["Remark"].ToString()).Length > 100)
            {
                listColumnLengthErrMsg.Add("<Remark> length can't be more than 100 Characters.");
            }

            if (listColumnLengthErrMsg.Count > 0)
            {
                this.listNewRowErrMsg.Add(listColumnLengthErrMsg.JoinToString(Environment.NewLine));
            }
            #endregion

            // WK# not match InvNo
            if (!MyUtility.Check.Empty(this.newRow["wkno"]) && this.newRow["wkno"].ToString() != this.master["InvNo"].ToString())
            {
                this.listNewRowErrMsg.Add($"WK# is not match {this.master["InvNo"]}");
            }

            if (MyUtility.Check.Empty(this.newRow["poid"]) || MyUtility.Check.Empty(this.newRow["seq1"]) || MyUtility.Check.Empty(this.newRow["seq2"]))
            {
                this.listNewRowErrMsg.Add($"poid or seq1 or seq2 is empty!");
                return;
            }

            // Seq1 can't be 7X
            if (this.newRow["seq1"].ToString().Substring(0, 1) == "7")
            {
                this.listNewRowErrMsg.Add($"Can't not import 7X item (SP#:{this.newRow["poid"]}-Seq1:{this.newRow["seq1"]}-Seq2:{this.newRow["seq2"]})!!");
            }

            List<SqlParameter> sqlpar = new List<SqlParameter>();
            sqlpar.Add(new SqlParameter("@poid", this.newRow["poid"].ToString().Trim()));
            sqlpar.Add(new SqlParameter("@seq1", this.newRow["seq1"].ToString().Trim()));
            sqlpar.Add(new SqlParameter("@seq2", this.newRow["seq2"].ToString().Trim()));
            sqlpar.Add(new SqlParameter("@roll", this.newRow["roll"].ToString().Trim()));
            sqlpar.Add(new SqlParameter("@dyelot", this.newRow["dyelot"].ToString().Trim()));
            sqlpar.Add(new SqlParameter("@shipqty", this.newRow["shipqty"].ToString().Trim()));

            if (!MyUtility.Check.Empty(this.newRow["MINDQRCode"]))
            {
                string sqlCheckQRCode = @"
select PL.QRCode
from POShippingList P
INNER JOIN POShippingList_Line PL ON P.Ukey = PL.POShippingList_Ukey
WHERE P.POID= @poid AND P.Seq1 = @seq1  AND PL.Line = @seq2 and PL.PackageNo = @roll and PL.BatchNo = @dyelot
GROUP BY PL.QRCode
";
                DataTable dtCheckQRCode;

                DualResult result = DBProxy.Current.Select(null, sqlCheckQRCode, sqlpar, out dtCheckQRCode);

                if (!result)
                {
                    this.listNewRowErrMsg.Add(result.GetException().ToString());
                }
                else
                {
                    if (dtCheckQRCode.Rows.Count > 1)
                    {
                        this.listNewRowErrMsg.Add("There are more than two QR Code, please right-click on [MIND QR Code] to select one of the QR Code.");
                    }

                    if (dtCheckQRCode.Rows.Count == 1)
                    {
                        if (dtCheckQRCode.Rows[0]["QRCode"].ToString() != this.newRow["MINDQRCode"].ToString())
                        {
                            listNewRowHintMsg.Add("The keyin [MIND QR Code] is different from supplier, and has been updated to the QR Code of supplier.");
                        }

                        this.newRow["MINDQRCode"] = dtCheckQRCode.Rows[0]["QRCode"];
                    }
                }
            }

            DataRow dr2;
            string sql = @"
select  psd.fabrictype
        , psd.POUnit
        , StockUnit = psd.StockUnit
        , Round(dbo.GetUnitQty(psd.POUnit, psd.StockUnit, @shipqty), 2) as stockqty 
        , (select o.Category from View_WH_Orders o WITH (NOLOCK) where o.id= psd.id) as category
        , psd.Refno
        , [ColorID] = isnull(Color.Value,'')
        , ff.MtlTypeID
from dbo.PO_Supp_Detail psd WITH (NOLOCK) 
inner join [dbo].[Fabric] ff WITH (NOLOCK) on psd.SCIRefno= ff.SCIRefno
inner join [dbo].[MtlType] mm WITH (NOLOCK) on mm.ID = ff.MtlTypeID
inner join [dbo].[Unit] uu WITH (NOLOCK) on ff.UsageUnit = uu.ID
left join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
OUTER APPLY(
 SELECT [Value]=
	 CASE WHEN ff.MtlTypeID in ('EMB THREAD','SP THREAD','THREAD') THEN IIF(psd.SuppColor = '' or psd.SuppColor is null, dbo.GetColorMultipleID(psd.BrandID,isnull(psdsC.SpecValue, '')),psd.SuppColor)
		 ELSE dbo.GetColorMultipleID(psd.BrandID,isnull(psdsC.SpecValue, ''))
	 END
)Color
where psd.id=@poid and psd.seq1 =@seq1 and psd.seq2 = @seq2";

            if (MyUtility.Check.Seek(sql, sqlpar, out dr2))
            {
                // 非主料清空roll & dyelot
                if (dr2["fabrictype"].ToString() != "F")
                {
                    this.newRow["roll"] = string.Empty;
                    this.newRow["dyelot"] = string.Empty;
                }

                // po unit 空白不匯入
                if (MyUtility.Check.Empty(dr2["pounit"]))
                {
                    this.listNewRowErrMsg.Add($"PO Unit of SP#:{this.newRow["poid"]}-Seq1:{this.newRow["seq1"]}-Seq2:{this.newRow["seq2"]} is empty!!");
                }

                // stock unit空白不匯入
                if (MyUtility.Check.Empty(dr2["stockunit"]))
                {
                    this.listNewRowErrMsg.Add($"Stock Unit of SP#:{this.newRow["poid"]}-Seq1:{this.newRow["seq1"]}-Seq2:{this.newRow["seq2"]} is empty!!");
                }

                this.newRow["fabrictype"] = dr2["fabrictype"].ToString();
                this.newRow["Pounit"] = dr2["pounit"].ToString();
                this.newRow["StockUnit"] = dr2["StockUnit"].ToString();
                this.newRow["stockqty"] = decimal.Parse(dr2["stockqty"].ToString());
                this.newRow["Refno"] = dr2["Refno"].ToString();
                this.newRow["ColorID"] = dr2["ColorID"].ToString();
                this.newRow["MtlTypeID"] = dr2["MtlTypeID"];

                // 決定倉別
                if (dr2["category"].ToString() == "M")
                {
                    this.newRow["stocktype"] = "I";
                }
                else
                {
                    this.newRow["stocktype"] = "B";
                }

                // 檢查location是否正確
                if (!MyUtility.Check.Empty(this.newRow["location"]))
                {
                    string[] strA = Regex.Split(this.newRow["location"].ToString(), ",");
                    foreach (string i in strA.Distinct())
                    {
                        if (!MyUtility.Check.Seek($@"
select * 
from    dbo.mtllocation WITH (NOLOCK) 
where   stocktype='{this.newRow["stocktype"]}' 
        and junk != '1'
        and id='{i.Replace("'", "''")}'"))
                        {
                            this.listNewRowErrMsg.Add($"Location ({i}) of SP#:{this.newRow["poid"]}-Seq1:{this.newRow["seq1"]}-Seq2:{this.newRow["seq2"]} in stock ({this.newRow["stocktype"]}) is not found!!");
                        }
                    }
                }
            }
            else
            {
                this.listNewRowErrMsg.Add(string.Format("SP#:{0}-Seq1:{1}-Seq2:{2} is not found!!", this.newRow["poid"], this.newRow["seq1"], this.newRow["seq2"]));
            }

            sqlpar.Clear();
            if (!MyUtility.Check.Empty(this.newRow["MINDQRCode"].ToString().Trim()))
            {
                sqlpar.Add(new SqlParameter("@MINDQRCode", this.newRow["MINDQRCode"].ToString().Trim()));
                sql = $@"select r.InvNo from Receiving_Detail rd inner join Receiving r on r.id = rd.id where rd.MINDQRCode = @MINDQRCode and rd.id <> '{this.master["id"]}'";
                if (MyUtility.Check.Seek(sql, sqlpar, out dr2))
                {
                    this.listNewRowErrMsg.Add($"This QR Code already exist WK#{dr2["InvNo"]}, cannot import.");
                }

                if (this.detailData.AsEnumerable().Where(w => w.RowState != DataRowState.Deleted)
                    .Any(a => MyUtility.Convert.GetString(a["MINDQRCode"]) == MyUtility.Convert.GetString(this.newRow["MINDQRCode"])))
                {
                    this.listNewRowErrMsg.Add($"This QR Code already exist, cannot import.");
                }
            }

            if (listNewRowHintMsg.Count > 0 && this.listNewRowErrMsg.Count == 0)
            {
                this.newRow["ErrMsgType"] = "Hint";
            }
            else if (this.listNewRowErrMsg.Count > 0)
            {
                this.newRow["ErrMsgType"] = "Error";
            }
            else
            {
                this.newRow["ErrMsgType"] = string.Empty;
            }

            this.listNewRowErrMsg.AddRange(listNewRowHintMsg);
        }
    }
}
