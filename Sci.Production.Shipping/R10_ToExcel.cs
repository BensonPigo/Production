using Ict;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// R10
    /// </summary>
    public partial class R10
    {
        private DualResult ShareExpenseExportFeeReportToExcel()
        {
            string strXltName = string.Empty;
            int allColumn;
            if (this.reportContent == 1)
            {
                strXltName = Env.Cfg.XltPathDir + "\\Shipping_R10_ShareExpenseExportFeeReport_Garment.xltx";
                allColumn = 26;
            }
            else
            {
                strXltName = Env.Cfg.XltPathDir + "\\Shipping_R10_ShareExpenseExportFeeReport_RawMaterial.xltx";
                allColumn = 24;
            }

            int dynamicAccCounts = this.accnoData.Rows.Count;
            int totalSumColumn = allColumn + dynamicAccCounts + (this.reportContent == 1 ? 1 : 2);

            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return new DualResult(false, $"{strXltName} not found");
            }

            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

            #region Setting AccountNo
            int i = 0;
            string accnoL1 = "5912"; // Z欄 5912-2222 Airfreight
            string accnoLnow = string.Empty;

            foreach (DataRow dr in this.accnoData.Rows)
            {
                i++;
                string sql = string.Format("select concat(SUBSTRING(id,1,4),iif(len(id)>4,'-'+SUBSTRING(id,5,4),''),char(10)+char(13) ,Name) from SciFMS_AccountNo  WITH (NOLOCK)  where ID = '{0}'", MyUtility.Convert.GetString(dr["Accno"]));
                string accnoColName = MyUtility.GetValue.Lookup(sql);
                accnoLnow = MyUtility.Convert.GetString(dr["Accno"]).Substring(0, 4);
                string accnoLnow2 = MyUtility.Convert.GetString(dr["Accno"]).Length > 8 ? MyUtility.Convert.GetString(dr["Accno"]).Substring(4) : MyUtility.Convert.GetString(dr["Accno"]).Length > 4 ? "-" + MyUtility.Convert.GetString(dr["Accno"]).Substring(4) : string.Empty;
                worksheet.Cells[1, allColumn + i] = MyUtility.Check.Empty(accnoColName) ? accnoLnow + accnoLnow2 : accnoColName;

                accnoL1 = MyUtility.Convert.GetString(dr["Accno"]).Substring(0, 4);
            }

            if (this.reportContent == 1)
            {
                worksheet.Cells[1, totalSumColumn] = "Total Export Fee";
            }
            else
            {
                worksheet.Cells[1, totalSumColumn - 1] = "Total Export Fee";
                worksheet.Cells[1, totalSumColumn] = "Shipping Memo";
            }

            // 匯率選擇 Fixed, KPI, 各費用欄位名稱加上 (USD)
            if (!MyUtility.Check.Empty(this.comboRateType.SelectedValue))
            {
                for (int k = allColumn - 5; k <= allColumn + i + 1; k++)
                {
                    worksheet.Cells[1, k] = worksheet.Cells[1, k].Value + "\r\n(USD)";
                }
            }

            string excelSumCol = PublicPrg.Prgs.GetExcelEnglishColumnName(allColumn + dynamicAccCounts);
            string excelColumn = PublicPrg.Prgs.GetExcelEnglishColumnName(totalSumColumn);

            var first6105 = this.accnoData.AsEnumerable().Where(w => MyUtility.Convert.GetString(w["Accno"]).Substring(0, 4).EqualString("6105")).GroupBy(t => 1).Select(s => new { rn = s.Min(m => MyUtility.Convert.GetInt(m["rn"])) }).ToList();
            string first6105Column = string.Empty;
            if (first6105.Count > 0)
            {
                if (this.accnoData.Select("Accno like '5912%'").Count() > 0)
                {
                    first6105Column = PublicPrg.Prgs.GetExcelEnglishColumnName(allColumn + first6105[0].rn + 1);
                }
                else
                {
                    first6105Column = PublicPrg.Prgs.GetExcelEnglishColumnName(allColumn + first6105[0].rn);
                }
            }

            string sumCol5912start = string.Empty;
            string sumCol5912 = string.Empty;
            string sumCol6105 = string.Empty;
            string sumCol5912TTL = string.Empty;
            string sumCol6105TTL = string.Empty;
            #endregion

            // 填內容值
            int intRowsStart = 2;
            object[,] objArray = new object[1, totalSumColumn];

            // 將ShippingMemo移到最後一個欄位
            if (this.printData.Columns.Contains("ShippingMemo"))
            {
                this.printData.Columns["ShippingMemo"].SetOrdinal(this.printData.Columns.Count - 1);
            }

            foreach (DataRow dr in this.printData.Rows)
            {

                if (this.reportContent == 1)
                {
                    objArray[0, 0] = dr[0];
                    objArray[0, 1] = dr[1];
                    objArray[0, 2] = dr[2];
                    objArray[0, 3] = dr[3];
                    objArray[0, 4] = dr[4];
                    objArray[0, 5] = dr[5];
                    objArray[0, 6] = dr[6];
                    objArray[0, 7] = dr[7];
                    objArray[0, 8] = dr[8];
                    objArray[0, 9] = dr[9];
                    objArray[0, 10] = dr[10];
                    objArray[0, 11] = dr[11];
                    objArray[0, 12] = dr[12];
                    objArray[0, 13] = dr[13];
                    objArray[0, 14] = dr[14];
                    objArray[0, 15] = dr[15];
                    objArray[0, 16] = dr[16];
                    objArray[0, 17] = MyUtility.Check.Empty(dr[17]) ? 0 : dr[17];
                    objArray[0, 18] = MyUtility.Check.Empty(dr[18]) ? 0 : dr[18];
                    objArray[0, 19] = MyUtility.Check.Empty(dr[19]) ? 0 : dr[19];
                    objArray[0, 20] = MyUtility.Check.Empty(dr[20]) ? 0 : dr[20];
                    objArray[0, 21] = MyUtility.Check.Empty(dr[21]) ? 0 : dr[21];
                    objArray[0, 22] = MyUtility.Check.Empty(dr[22]) ? 0 : dr[22];
                    objArray[0, 23] = MyUtility.Check.Empty(dr[23]) ? 0 : dr[23];
                    objArray[0, 24] = MyUtility.Check.Empty(dr[24]) ? 0 : dr[24];
                    objArray[0, 25] = MyUtility.Check.Empty(dr[25]) ? 0 : dr[25];
                }

                if (this.reportContent == 2)
                {
                    objArray[0, 0] = dr[0];
                    objArray[0, 1] = dr[1];
                    objArray[0, 2] = dr[2];
                    objArray[0, 3] = dr[3];
                    objArray[0, 4] = dr[4];
                    objArray[0, 5] = dr[5];
                    objArray[0, 6] = dr[6];
                    objArray[0, 7] = dr[7];
                    objArray[0, 8] = dr[8];
                    objArray[0, 9] = dr[9];
                    objArray[0, 10] = dr[10];
                    objArray[0, 11] = dr[11];
                    objArray[0, 12] = dr[12];
                    objArray[0, 13] = dr[13];
                    objArray[0, 14] = dr[14];
                    objArray[0, 15] = dr[15];
                    objArray[0, 16] = dr[16];
                    objArray[0, 17] = dr[17];
                    objArray[0, 18] = MyUtility.Check.Empty(dr[18]) ? 0 : dr[18];
                    objArray[0, 19] = MyUtility.Check.Empty(dr[19]) ? 0 : dr[19];
                    objArray[0, 20] = MyUtility.Check.Empty(dr[20]) ? 0 : dr[20];
                    objArray[0, totalSumColumn - 1] = dr["ShippingMemo"];
                }

                // 多增加的AccountID, 必須要動態的填入欄位值!
                if (dynamicAccCounts > 0)
                {
                    for (int t = 1; t <= dynamicAccCounts; t++)
                    {
                        if (MyUtility.Convert.GetString(dr.Table.Columns[allColumn - 1 + t].ColumnName).Contains("5912"))
                        {
                            if (MyUtility.Check.Empty(sumCol5912start))
                            {
                                sumCol5912start = PublicPrg.Prgs.GetExcelEnglishColumnName(allColumn + t);
                            }
                        }

                        if (MyUtility.Convert.GetString(dr.Table.Columns[allColumn - 1 + t].ColumnName).EqualString("5912-Total"))
                        {
                            if (MyUtility.Check.Empty(sumCol5912))
                            {
                                sumCol5912 = PublicPrg.Prgs.GetExcelEnglishColumnName(allColumn - 1 + t);
                                sumCol5912TTL = PublicPrg.Prgs.GetExcelEnglishColumnName(allColumn + t);
                            }

                            objArray[0, allColumn - 1 + t] = $"=W{intRowsStart}+SUM({sumCol5912start}{intRowsStart}:{sumCol5912}{intRowsStart})";
                        }
                        else if (MyUtility.Convert.GetString(dr.Table.Columns[allColumn - 1 + t].ColumnName).EqualString("6105-Total"))
                        {
                            if (MyUtility.Check.Empty(sumCol6105))
                            {
                                sumCol6105 = PublicPrg.Prgs.GetExcelEnglishColumnName(allColumn - 1 + t);
                                sumCol6105TTL = PublicPrg.Prgs.GetExcelEnglishColumnName(allColumn + t);
                            }

                            objArray[0, allColumn - 1 + t] = $"=SUM({first6105Column}{intRowsStart}:{sumCol6105}{intRowsStart})";
                        }
                        else
                        {
                            objArray[0, allColumn - 1 + t] = MyUtility.Check.Empty(dr[allColumn - 1 + t]) ? 0 : dr[allColumn - 1 + t];
                        }
                    }
                }

                string sc1 = string.Empty;
                string sc2 = string.Empty;
                if (!MyUtility.Check.Empty(sumCol5912TTL))
                {
                    sc1 = $"-{sumCol5912TTL}{intRowsStart}";
                }

                if (!MyUtility.Check.Empty(sumCol6105TTL))
                {
                    sc2 = $"-{sumCol6105TTL}{intRowsStart}";
                }

                string sumStartColEng = this.reportType == 1 ? "R" : this.reportContent == 2 ? "V" : "Y";

                if (this.reportContent == 1)
                {
                    objArray[0, totalSumColumn - 1] = string.Format("=SUM({2}{0}:{1}{0}) {3} {4}", intRowsStart, excelSumCol, sumStartColEng, sc1, sc2);
                }
                else
                {
                    objArray[0, totalSumColumn - 2] = string.Format("=SUM({2}{0}:{1}{0}) {3} {4}", intRowsStart, excelSumCol, sumStartColEng, sc1, sc2);
                }

                worksheet.Range[string.Format("A{0}:{1}{0}", intRowsStart, excelColumn)].Value2 = objArray;
                intRowsStart++;
            }

            //Microsoft.Office.Interop.Excel.Range range = worksheet.get_Range("C1", Missing.Value);
            //range.EntireColumn.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftToRight, Microsoft.Office.Interop.Excel.XlInsertFormatOrigin.xlFormatFromRightOrBelow);
            //worksheet.Cells[1, 3] = "On Board Date";
            //range = worksheet.get_Range("C2", "C" + (this.printData.Rows.Count + 1));
            //range.EntireColumn.NumberFormat = "yyyy/MM/dd";

            //// Sci.Utility.Report.ExcelCOM com = new Sci.Utility.Report.ExcelCOM();
            //object[,] arrayValues = tb_onBoardDate.ToArray2D();
            //range.Value2 = arrayValues;

            //range = worksheet.get_Range("E1", Missing.Value);
            //range.EntireColumn.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftToRight, Microsoft.Office.Interop.Excel.XlInsertFormatOrigin.xlFormatFromRightOrBelow);
            //worksheet.Cells[1, 5] = "Include Foundry";
            //range = worksheet.get_Range("E2", "E" + (this.printData.Rows.Count + 1));
            //arrayValues = tb_IncludeFoundry.ToArray2D();
            //range.Value2 = arrayValues;

            //range = worksheet.get_Range("F1", Missing.Value);
            //range.EntireColumn.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftToRight, Microsoft.Office.Interop.Excel.XlInsertFormatOrigin.xlFormatFromRightOrBelow);
            //worksheet.Cells[1, 6] = "Sis. Fty A/P#";
            //range = worksheet.get_Range("F2", "F" + (this.printData.Rows.Count + 1));
            //arrayValues = tb_SisFtyAP.ToArray2D();
            //range.Value2 = arrayValues;

            excel.Cells.EntireColumn.AutoFit();
            excel.Cells.EntireRow.AutoFit();
            this.HideWaitMessage();

            string strExcelName = Class.MicrosoftFile.GetName("Shipping_R10_ShareExpenseExportFeeReport");
            excel.ActiveWorkbook.SaveAs(strExcelName);
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            Marshal.ReleaseComObject(worksheet);

            strExcelName.OpenFile();

            return new DualResult(true);
        }

        private DualResult ShareExpenseExportBySPToExcel()
        {
            string strXltName = string.Empty;

            if (this.reportContent == 1)
            {
                strXltName = Env.Cfg.XltPathDir + "\\Shipping_R10_ShareExpenseExportBySP_Garment.xltx";
            }
            else
            {
                strXltName = Env.Cfg.XltPathDir + "\\Shipping_R10_ShareExpenseExportBySP_RawMaterial.xltx";
            }

            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return new DualResult(false, $"{strXltName} not found");
            }

            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

            int allColumn = 0;

            if (this.reportContent == 1)
            {
                allColumn = 32;
            }
            else
            {
                allColumn = 28;
            }

            int i = 0;
            int dynamicAccCounts = this.accnoData.Rows.Count;
            int totalSumColumn = allColumn + this.accnoData.Rows.Count + (this.reportContent == 1 ? 1 : 2);
            string accnoL1 = "5912"; // Z欄 5912-2222 Airfreight
            string accnoLnow = string.Empty;

            foreach (DataRow dr in this.accnoData.Rows)
            {
                i++;
                string sql = string.Format("select concat(SUBSTRING(id,1,4),iif(len(id)>4,'-'+SUBSTRING(id,5,4),''),char(10)+char(13) ,Name) from SciFMS_AccountNo  WITH (NOLOCK)  where ID = '{0}'", MyUtility.Convert.GetString(dr["Accno"]));
                string accnoColName = MyUtility.GetValue.Lookup(sql);
                accnoLnow = MyUtility.Convert.GetString(dr["Accno"]).Substring(0, 4);
                string accnoLnow2 = MyUtility.Convert.GetString(dr["Accno"]).Length > 8 ? MyUtility.Convert.GetString(dr["Accno"]).Substring(4) : MyUtility.Convert.GetString(dr["Accno"]).Length > 4 ? "-" + MyUtility.Convert.GetString(dr["Accno"]).Substring(4) : string.Empty;
                worksheet.Cells[1, allColumn + i] = MyUtility.Check.Empty(accnoColName) ? accnoLnow + accnoLnow2 : accnoColName;

                accnoL1 = MyUtility.Convert.GetString(dr["Accno"]).Substring(0, 4);
            }

            worksheet.Cells[1, allColumn + dynamicAccCounts + 1] = "Total Export Fee";

            if (this.reportContent == 2)
            {
                worksheet.Cells[1, allColumn + dynamicAccCounts + 2] = "Shipping Memo";
            }

            // 匯率選擇 Fixed, KPI, 各費用欄位名稱加上 (USD)
            if (!MyUtility.Check.Empty(this.comboRateType.SelectedValue))
            {
                for (int k = allColumn - 5; k <= allColumn + i + 1; k++)
                {
                    worksheet.Cells[1, k] = worksheet.Cells[1, k].Value + "\r\n(USD)";
                }
            }

            string excelSumCol = PublicPrg.Prgs.GetExcelEnglishColumnName(allColumn + dynamicAccCounts);
            string excelColumn = PublicPrg.Prgs.GetExcelEnglishColumnName(totalSumColumn);

            var first6105 = this.accnoData.AsEnumerable().Where(w => MyUtility.Convert.GetString(w["Accno"]).Substring(0, 4).EqualString("6105")).GroupBy(t => 1).Select(s => new { rn = s.Min(m => MyUtility.Convert.GetInt(m["rn"])) }).ToList();
            string first6105Column = string.Empty;
            if (first6105.Count > 0)
            {
                if (this.accnoData.Select("Accno like '5912%'").Count() > 0)
                {
                    first6105Column = PublicPrg.Prgs.GetExcelEnglishColumnName(allColumn + first6105[0].rn + 1);
                }
                else
                {
                    first6105Column = PublicPrg.Prgs.GetExcelEnglishColumnName(allColumn + first6105[0].rn);
                }
            }

            string sumCol5912start = string.Empty;
            string sumCol5912 = string.Empty;
            string sumCol6105 = string.Empty;
            string sumCol5912TTL = string.Empty;
            string sumCol6105TTL = string.Empty;

            // 填內容值
            int intRowsStart = 2;
            object[,] objArray = new object[1, totalSumColumn];

            // 將ShippingMemo移到最後一個欄位
            if (this.printData.Columns.Contains("ShippingMemo"))
            {
                this.printData.Columns["ShippingMemo"].SetOrdinal(this.printData.Columns.Count - 1);
            }

            foreach (DataRow dr in this.printData.Rows)
            {
                for (int f = 0; f < allColumn; f++)
                {
                    string typeName = dr[f].GetType().Name;

                    if ((typeName == typeof(int).Name ||
                        typeName == typeof(long).Name ||
                        typeName == typeof(decimal).Name ||
                        typeName == typeof(double).Name ||
                        typeName == typeof(DBNull).Name ||
                        typeName == typeof(float).Name) &&
                        (MyUtility.Check.Empty(dr[f]) || dr[f] == DBNull.Value))
                    {
                        objArray[0, f] = 0;
                    }
                    else
                    {
                        objArray[0, f] = dr[f];
                    }
                }

                if (this.reportContent == 2)
                {
                    objArray[0, totalSumColumn - 1] = dr["ShippingMemo"];
                }

                // 多增加的AccountID, 必須要動態的填入欄位值!
                if (dynamicAccCounts > 0)
                {
                    for (int c = 1; c <= dynamicAccCounts; c++)
                    {
                        if (MyUtility.Convert.GetString(dr.Table.Columns[allColumn - 1 + c].ColumnName).Contains("5912"))
                        {
                            if (MyUtility.Check.Empty(sumCol5912start))
                            {
                                sumCol5912start = PublicPrg.Prgs.GetExcelEnglishColumnName(allColumn + c);
                            }
                        }

                        if (MyUtility.Convert.GetString(dr.Table.Columns[allColumn - 1 + c].ColumnName).EqualString("5912-Total"))
                        {
                            if (MyUtility.Check.Empty(sumCol5912))
                            {
                                sumCol5912 = PublicPrg.Prgs.GetExcelEnglishColumnName(allColumn - 1 + c);
                                sumCol5912TTL = PublicPrg.Prgs.GetExcelEnglishColumnName(allColumn + c);
                            }

                            objArray[0, allColumn - 1 + c] = $"=AA{intRowsStart}+SUM({sumCol5912start}{intRowsStart}:{sumCol5912}{intRowsStart})";
                        }
                        else if (MyUtility.Convert.GetString(dr.Table.Columns[allColumn - 1 + c].ColumnName).EqualString("6105-Total"))
                        {
                            if (MyUtility.Check.Empty(sumCol6105))
                            {
                                sumCol6105 = PublicPrg.Prgs.GetExcelEnglishColumnName(allColumn - 1 + c);
                                sumCol6105TTL = PublicPrg.Prgs.GetExcelEnglishColumnName(allColumn + c);
                            }

                            objArray[0, allColumn - 1 + c] = $"=SUM({first6105Column}{intRowsStart}:{sumCol6105}{intRowsStart})";
                        }
                        else
                        {
                            objArray[0, allColumn - 1 + c] = MyUtility.Check.Empty(dr[allColumn - 1 + c]) ? 0 : dr[allColumn - 1 + c];
                        }
                    }
                }

                string sc1 = string.Empty;
                string sc2 = string.Empty;
                if (!MyUtility.Check.Empty(sumCol5912TTL))
                {
                    sc1 = $"-{sumCol5912TTL}{intRowsStart}";
                }

                if (!MyUtility.Check.Empty(sumCol6105TTL))
                {
                    sc2 = $"-{sumCol6105TTL}{intRowsStart}";
                }

                string sumStartColEng = this.reportType == 1 ? "R" : this.reportContent == 2 ? "V" : "Y";
                if (this.reportContent == 1)
                {
                    objArray[0, totalSumColumn - 1] = string.Format("=SUM({2}{0}:{1}{0}) {3} {4}", intRowsStart, excelSumCol, sumStartColEng, sc1, sc2);
                }
                else
                {
                    objArray[0, totalSumColumn - 2] = string.Format("=SUM({2}{0}:{1}{0}) {3} {4}", intRowsStart, excelSumCol, sumStartColEng, sc1, sc2);
                }

                worksheet.Range[string.Format("A{0}:{1}{0}", intRowsStart, excelColumn)].Value2 = objArray;
                intRowsStart++;
            }

            excel.Cells.EntireColumn.AutoFit();
            excel.Cells.EntireRow.AutoFit();
            this.HideWaitMessage();

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("Shipping_R10_ShareExpenseExportBySP");
            excel.ActiveWorkbook.SaveAs(strExcelName);
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            Marshal.ReleaseComObject(worksheet);

            strExcelName.OpenFile();
            #endregion

            return new DualResult(true);
        }

        private DualResult ShareExpenseExportBySPByFeeToExcel()
        {
            string strXltName = string.Empty;

            if (this.reportContent == 1)
            {
                strXltName = Env.Cfg.XltPathDir + "\\Shipping_R10_ShareExpenseExportBySPByFee_Garment.xltx";
            }
            else
            {
                strXltName = Env.Cfg.XltPathDir + "\\Shipping_R10_ShareExpenseExportBySPByFee_RawMaterial.xltx";
            }

            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return new DualResult(false, $"{strXltName} not found");
            }

            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

            // 匯率選擇 Fixed, KPI, 各費用欄位名稱加上 (USD)
            if (!MyUtility.Check.Empty(this.comboRateType.SelectedValue))
            {
                if (this.reportContent == 1)
                {
                    worksheet.Cells[1, 25] = worksheet.Cells[1, 25].Value + "\r\n(USD)";
                }
                else
                {
                    worksheet.Cells[1, 22] = worksheet.Cells[1, 22].Value + "\r\n(USD)";
                }
            }

            // 填內容值
            int intRowsStart = 2;
            object[,] objArray = new object[1, 36];
            foreach (DataRow dr in this.printData.Rows)
            {
                if (this.reportContent == 1)
                {
                    objArray[0, 0] = dr["Type"];
                    objArray[0, 1] = dr["ID"];
                    objArray[0, 2] = dr["OnBoardDate"];
                    objArray[0, 3] = dr["Shipper"];
                    objArray[0, 4] = dr["FactoryID"];
                    objArray[0, 5] = dr["MDivisionID"];
                    objArray[0, 6] = dr["KPICode"];
                    objArray[0, 7] = dr["Foundry"];
                    objArray[0, 8] = dr["SisFtyAPID"];
                    objArray[0, 9] = dr["BrandID"];
                    objArray[0, 10] = dr["Category"];
                    objArray[0, 11] = dr["OrderID"];
                    objArray[0, 12] = dr["BuyerDelivery"];
                    objArray[0, 13] = dr["OQty"];
                    objArray[0, 14] = dr["CustCDID"];
                    objArray[0, 15] = dr["Dest"];
                    objArray[0, 16] = dr["ShipModeID"];
                    objArray[0, 17] = dr["PackID"];
                    objArray[0, 18] = dr["PulloutID"];
                    objArray[0, 19] = dr["PulloutDate"];
                    objArray[0, 20] = dr["ShipQty"];
                    objArray[0, 21] = dr["CTNQty"];
                    objArray[0, 22] = dr["GW"];
                    objArray[0, 23] = dr["CBM"];
                    objArray[0, 24] = dr["Forwarder"];
                    objArray[0, 25] = dr["BLNo"];
                    objArray[0, 26] = dr["FeeType"];
                    objArray[0, 27] = dr["Amount"];
                    objArray[0, 28] = dr["freeSP"];
                    objArray[0, 29] = dr["CurrencyID"];
                    objArray[0, 30] = dr["APID"];
                    objArray[0, 31] = dr["CDate"];
                    objArray[0, 32] = dr["ApvDate"];
                    objArray[0, 33] = dr["VoucherID"];
                    objArray[0, 34] = dr["VoucherDate"];
                    objArray[0, 35] = dr["SubType"];

                    worksheet.Range[string.Format("A{0}:AJ{0}", intRowsStart)].Value2 = objArray;
                }
                else
                {
                    objArray[0, 0] = dr["Type"];
                    objArray[0, 1] = dr["ID"];
                    objArray[0, 2] = dr["Shipper"];
                    objArray[0, 3] = dr["BrandID"];
                    objArray[0, 4] = dr["Category"];
                    objArray[0, 5] = dr["OrderID"];
                    objArray[0, 6] = dr["BuyerDelivery"];
                    objArray[0, 7] = dr["OQty"];
                    objArray[0, 8] = dr["CustCDID"];
                    objArray[0, 9] = dr["Dest"];
                    objArray[0, 10] = dr["ShipModeID"];
                    objArray[0, 11] = dr["PackID"];
                    objArray[0, 12] = dr["PulloutID"];
                    objArray[0, 13] = dr["PulloutDate"];
                    objArray[0, 14] = dr["ShipQty"];
                    objArray[0, 15] = dr["CTNQty"];
                    objArray[0, 16] = dr["GW"];
                    objArray[0, 17] = dr["CBM"];
                    objArray[0, 18] = dr["Forwarder"];
                    objArray[0, 19] = dr["BLNo"];
                    objArray[0, 20] = dr["FeeType"];
                    objArray[0, 21] = dr["Amount"];
                    objArray[0, 22] = dr["CurrencyID"];
                    objArray[0, 23] = dr["APID"];
                    objArray[0, 24] = dr["CDate"];
                    objArray[0, 25] = dr["ApvDate"];
                    objArray[0, 26] = dr["VoucherID"];
                    objArray[0, 27] = dr["VoucherDate"];
                    objArray[0, 28] = dr["SubType"];

                    worksheet.Range[string.Format("A{0}:AC{0}", intRowsStart)].Value2 = objArray;
                }

                intRowsStart++;
            }

            excel.Cells.EntireColumn.AutoFit();
            excel.Cells.EntireRow.AutoFit();
            this.HideWaitMessage();

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("Shipping_R10_ShareExpenseExportBySPByFee");
            excel.ActiveWorkbook.SaveAs(strExcelName);
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            Marshal.ReleaseComObject(worksheet);

            strExcelName.OpenFile();
            #endregion
            return new DualResult(true);
        }

        private DualResult AirPrepaidExpenseToExcel()
        {
            string strXltName = string.Empty;

            strXltName = Env.Cfg.XltPathDir + "\\Shipping_R10_AirPrepaidExpense_Garment.xltx";

            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return new DualResult(false, $"{strXltName} not found");
            }

            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

            MyUtility.Excel.CopyToXls(this.printDataS[0], string.Empty, "Shipping_R10_AirPrepaidExpense_Garment.xltx", 1, false, null, excel, wSheet: excel.Sheets[1]);
            MyUtility.Excel.CopyToXls(this.printDataS[1], string.Empty, "Shipping_R10_AirPrepaidExpense_Garment.xltx", 1, false, null, excel, wSheet: excel.Sheets[2]);

            excel.Cells.EntireColumn.AutoFit();
            excel.Cells.EntireRow.AutoFit();
            this.HideWaitMessage();

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("Shipping_R10_ShareExpenseExportBySPByFee");
            excel.ActiveWorkbook.SaveAs(strExcelName);
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            Marshal.ReleaseComObject(worksheet);

            strExcelName.OpenFile();
            #endregion
            return new DualResult(true);
        }

        private void ReportType5ToExcel()
        {
            string strXltName = string.Empty;

            if (this.reportContent == 1)
            {
                strXltName = "Shipping_R10_ExportFeeReport(MergerdAcctCode)_Garment.xltx";
            }
            else
            {
                strXltName = "Shipping_R10_ExportFeeReport(MergerdAcctCode)_RawMaterial.xltx";
            }

            string strXltPath = Env.Cfg.XltPathDir + $"\\{strXltName}";

            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltPath);

            if (excel == null)
            {
                return;
            }

            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

            if (this.reportContent == 2)
            {
                worksheet.Cells[1, 4] = "FTY WK#";
            }

            MyUtility.Excel.CopyToXls(this.printData, string.Empty, strXltName, 1, showExcel: false, excelApp: excel);

            // 刪除不必要的欄位
            worksheet.get_Range("BA:BQ").EntireColumn.Delete();

            int x = this.printData.Rows.Count + 2;

            // 剩下的底色弄成白色，抓個兩百行不要被User看到就好
            worksheet.get_Range($"A{x}:AZ{x + 200}").Interior.Color = Color.White;

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("Shipping_R10_ExportFeeReport(MergerdAcctCode)");
            excel.ActiveWorkbook.SaveAs(strExcelName);
            excel.Quit();
            Marshal.ReleaseComObject(excel);

            strExcelName.OpenFile();
            #endregion
        }
    }
}
