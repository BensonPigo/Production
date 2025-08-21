using Ict;
using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;
using Sci.Utility.Excel;
using System;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Quality
{
    /// <inheritdoc/>
    public partial class R08 : Win.Tems.PrintForm
    {
        private DataTable[] printData;
        private string Excelfile;
        private Color green = Color.FromArgb(153, 204, 0);
        private Color blue = Color.FromArgb(101, 215, 255);
        private Color zero = Color.FromArgb(250, 191, 143);
        private QA_R08_ViewModel model = new QA_R08_ViewModel();

        /// <summary>
        /// Initializes a new instance of the <see cref="R08"/> class.
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public R08(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.txtbrand.MultiSelect = true;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(this.dateInspectionDate.Value1) || MyUtility.Check.Empty(this.dateInspectionDate.Value2))
            {
                MyUtility.Msg.WarningBox("< Inspected Date > cannot be empty!");
                return false;
            }

            this.model = new QA_R08_ViewModel()
            {
                InspectionDateFrom = this.dateInspectionDate.Value1,
                InspectionDateTo = this.dateInspectionDate.Value2,
                EditDateFrom = null,
                EditDateTo = null,
                Inspectors = this.txtmulituser.TextBox1.Text,
                POIDFrom = this.txtSPStart.Text,
                POIDTo = this.txtSPEnd.Text,
                RefNoFrom = this.txtRefno1.Text,
                RefNoTo = this.txtRefno2.Text,
                BrandID = this.txtbrand.Text,
                IsSummary = !this.radioDetail.Checked,
            };

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            QA_R08 biModel = new QA_R08();
            Base_ViewModel resultReport = biModel.Get_QA_R08(this.model);

            if (!resultReport.Result)
            {
                return resultReport.Result;
            }

            if (this.radioDetail.Checked)
            {
                resultReport.DtArr[0].Columns.Remove("ReceivingID");
                resultReport.DtArr[0].Columns.Remove("InspSeq");
                resultReport.DtArr[0].Columns.Remove("AddDate");
            }

            this.printData = resultReport.DtArr;
            return resultReport.Result;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.printData[0].Rows.Count);
            StringBuilder c = new StringBuilder();
            if (this.printData[0].Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            string xltx = string.Empty;
            if (this.radioDetail.Checked)
            {
                xltx = "Quality_R08.xltx";
                Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\" + xltx); // 預先開啟excel app
                MyUtility.Excel.CopyToXls(this.printData[0], string.Empty, xltx, 1, true, null, objApp); // 將datatable copy to excel
            }
            else
            {
                xltx = "Quality_R08_Summery.xltx";
                Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\" + xltx); // 預先開啟excel app
                Excel.Worksheet wksheet = objApp.Sheets[1];
#if DEBUG
                objApp.Visible = true;
#endif

                int rowAdd = 0;
                int columnAdd = 2;
                string rgStr = string.Empty;

                //// 插入日期
                int daydiff = this.printData[2].Rows.Count;
                for (int i = 0; i < this.printData[2].Rows.Count; i++)
                {
                    // 寫入日期
                    wksheet.Cells[5 + i, 1] = ((DateTime)this.printData[2].Rows[i]["inspected date"]).ToString("MM/dd");
                }

                rgStr = string.Format($"A5:A{daydiff + 8}");
                Excel.Range rng = wksheet.get_Range(rgStr);
                rng.Font.Bold = true;

                // 插入QC資料
                wksheet.Cells[2, 1] = "QC";
                wksheet.Cells[2, 1].Interior.Color = this.green;
                wksheet.Cells[2, 1].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                wksheet.Cells[3, 1] = "Date";
                wksheet.Cells[3, 1].Interior.Color = this.green;
                wksheet.Cells[3, 1].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                if (this.printData[1].Rows.Count > 0)
                {
                    foreach (DataRow dr in this.printData[1].Rows)
                    {
                        // QC ID
                        wksheet.Cells[2, columnAdd] = dr["Inspector"].ToString();
                        rgStr = string.Format($"{MyExcelPrg.GetExcelColumnName(columnAdd)}2:{MyExcelPrg.GetExcelColumnName(columnAdd + 1)}2");
                        Excel.Range merge = wksheet.get_Range(rgStr);
                        merge.Interior.Color = this.green;
                        merge.Merge();
                        merge.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                        // QC Name
                        wksheet.Cells[3, columnAdd] = dr["QCName"].ToString();
                        rgStr = string.Format($"{MyExcelPrg.GetExcelColumnName(columnAdd)}3:{MyExcelPrg.GetExcelColumnName(columnAdd + 1)}3");
                        merge = wksheet.get_Range(rgStr);
                        merge.Interior.Color = this.blue;
                        merge.Merge();
                        merge.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                        // Header
                        wksheet.Cells[4, columnAdd] = "Roll";
                        wksheet.Cells[4, columnAdd + 1] = "Yard";

                        // Loop天數的次數
                        rowAdd = 5;
                        int cnt = 0;
                        for (int i = 0; i < this.printData[2].Rows.Count; i++)
                        {
                            DataRow[] selected = this.printData[0].Select($@"inspector='{dr["Inspector"]}' and [inspected date]='{((DateTime)this.printData[2].Rows[i]["inspected date"]).ToString("yyyy-MM-dd")}'");
                            if (selected.Length > 0)
                            {
                                wksheet.Cells[rowAdd, columnAdd] = selected[0].ItemArray[3];
                                wksheet.Cells[rowAdd, columnAdd + 1] = selected[0].ItemArray[4];
                                cnt++;
                            }
                            else
                            {
                                wksheet.Cells[rowAdd, columnAdd] = 0;
                                wksheet.Cells[rowAdd, columnAdd].Interior.Color = this.zero;
                                wksheet.Cells[rowAdd, columnAdd + 1] = 0;
                                wksheet.Cells[rowAdd, columnAdd + 1].Interior.Color = this.zero;
                            }

                            rowAdd++;
                        }

                        // Totoal
                        wksheet.Cells[rowAdd, columnAdd] = $"=SUM({MyExcelPrg.GetExcelColumnName(columnAdd)}5:{MyExcelPrg.GetExcelColumnName(columnAdd)}{rowAdd - 1})";
                        wksheet.Cells[rowAdd, columnAdd + 1] = $"=SUM({MyExcelPrg.GetExcelColumnName(columnAdd + 1)}5:{MyExcelPrg.GetExcelColumnName(columnAdd + 1)}{rowAdd - 1})";

                        // Avg
                        wksheet.Cells[rowAdd + 1, columnAdd] = $"=ROUND(SUM({MyExcelPrg.GetExcelColumnName(columnAdd)}5:{MyExcelPrg.GetExcelColumnName(columnAdd)}{rowAdd - 1})/{cnt},1)";
                        wksheet.Cells[rowAdd + 1, columnAdd + 1] = $"=ROUND(SUM({MyExcelPrg.GetExcelColumnName(columnAdd + 1)}5:{MyExcelPrg.GetExcelColumnName(columnAdd + 1)}{rowAdd - 1})/{cnt},1)";

                        columnAdd += 2;
                    }

                    // 加入Total Header
                    wksheet.Cells[2, columnAdd] = "Daily output";
                    rgStr = string.Format($"{MyExcelPrg.GetExcelColumnName(columnAdd)}2:{MyExcelPrg.GetExcelColumnName(columnAdd + 3)}2");
                    Excel.Range mergerge = wksheet.get_Range(rgStr);
                    mergerge.Interior.Color = this.green;
                    mergerge.Merge();
                    mergerge.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                    // Woven
                    wksheet.Cells[3, columnAdd] = "Woven";
                    wksheet.Cells[3, columnAdd].Interior.Color = this.green;
                    wksheet.Cells[4, columnAdd] = "Yard";

                    // Loop天數的次數
                    int cntWoven = 0;
                    for (int i = 0; i < daydiff; i++)
                    {
                        DataRow[] selected = this.printData[3].Select($@"[inspected date]='{((DateTime)this.printData[2].Rows[i]["inspected date"]).ToString("yyyy-MM-dd")}'");
                        if (selected.Length > 0)
                        {
                            wksheet.Cells[5 + i, columnAdd] = selected[0].ItemArray[1];
                            cntWoven++;
                        }
                        else
                        {
                            wksheet.Cells[5 + i, columnAdd] = 0;
                            wksheet.Cells[5 + i, columnAdd].Interior.Color = this.zero;
                        }
                    }

                    // Totoal
                    wksheet.Cells[daydiff + 5, columnAdd] = $"=SUM({MyExcelPrg.GetExcelColumnName(columnAdd)}5:{MyExcelPrg.GetExcelColumnName(columnAdd)}{rowAdd - 1})";

                    // Avg
                    wksheet.Cells[daydiff + 6, columnAdd] = $"=ROUND(SUM({MyExcelPrg.GetExcelColumnName(columnAdd)}5:{MyExcelPrg.GetExcelColumnName(columnAdd)}{rowAdd - 1})/{cntWoven},1)";

                    // Knit
                    columnAdd++;
                    wksheet.Cells[3, columnAdd] = "Knit";
                    wksheet.Cells[3, columnAdd].Interior.Color = this.green;
                    wksheet.Cells[4, columnAdd] = "Yard";

                    for (int i = 0; i < this.printData[4].Rows.Count; i++)
                    {
                        wksheet.Cells[5 + i, columnAdd] = this.printData[4].Rows[i]["Actual YDS"];
                    }

                    // Loop天數的次數
                    int cntKnit = 0;
                    for (int i = 0; i < daydiff; i++)
                    {
                        DataRow[] selected = this.printData[4].Select($@"[inspected date]='{((DateTime)this.printData[2].Rows[i]["inspected date"]).ToString("yyyy-MM-dd")}'");
                        if (selected.Length > 0)
                        {
                            wksheet.Cells[5 + i, columnAdd] = selected[0].ItemArray[1];
                            cntKnit++;
                        }
                        else
                        {
                            wksheet.Cells[5 + i, columnAdd] = 0;
                            wksheet.Cells[5 + i, columnAdd].Interior.Color = this.zero;
                        }
                    }

                    // Totoal
                    wksheet.Cells[daydiff + 5, columnAdd] = $"=SUM({MyExcelPrg.GetExcelColumnName(columnAdd)}5:{MyExcelPrg.GetExcelColumnName(columnAdd)}{rowAdd - 1})";

                    // Avg
                    wksheet.Cells[daydiff + 6, columnAdd] = $"=ROUND(SUM({MyExcelPrg.GetExcelColumnName(columnAdd)}5:{MyExcelPrg.GetExcelColumnName(columnAdd)}{rowAdd - 1})/{cntKnit},1)";

                    // Total
                    columnAdd++;
                    wksheet.Cells[3, columnAdd] = "Total";
                    rgStr = string.Format($"{MyExcelPrg.GetExcelColumnName(columnAdd)}3:{MyExcelPrg.GetExcelColumnName(columnAdd + 1)}3");
                    mergerge = wksheet.get_Range(rgStr);
                    mergerge.Interior.Color = this.green;
                    mergerge.Merge();
                    mergerge.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                    for (int i = 0; i < this.printData[2].Rows.Count; i++)
                    {
                        wksheet.Cells[5 + i, columnAdd] = this.printData[2].Rows[i]["Roll"];
                        wksheet.Cells[5 + i, columnAdd + 1] = this.printData[2].Rows[i]["Actual YDS"];
                    }

                    wksheet.Cells[4, columnAdd] = "Roll";

                    // Sum
                    wksheet.Cells[daydiff + 5, columnAdd] = $"=SUM({MyExcelPrg.GetExcelColumnName(columnAdd)}5:{MyExcelPrg.GetExcelColumnName(columnAdd)}{rowAdd - 1})";

                    // Avg
                    wksheet.Cells[daydiff + 6, columnAdd] = $"=ROUND(SUM({MyExcelPrg.GetExcelColumnName(columnAdd)}5:{MyExcelPrg.GetExcelColumnName(columnAdd)}{rowAdd - 1})/{daydiff},1)";

                    columnAdd++;
                    wksheet.Cells[4, columnAdd] = "Yard";

                    // Sum
                    wksheet.Cells[daydiff + 5, columnAdd] = $"=SUM({MyExcelPrg.GetExcelColumnName(columnAdd)}5:{MyExcelPrg.GetExcelColumnName(columnAdd)}{rowAdd - 1})";
                    wksheet.Cells[daydiff + 5, columnAdd].Font.Color = Color.Red;

                    // Avg
                    wksheet.Cells[daydiff + 6, columnAdd] = $"=ROUND(SUM({MyExcelPrg.GetExcelColumnName(columnAdd)}5:{MyExcelPrg.GetExcelColumnName(columnAdd)}{rowAdd - 1})/{daydiff},1)";
                    wksheet.Cells[daydiff + 6, columnAdd].Font.Color = Color.FromArgb(0, 112, 192);

                    // Remark
                    columnAdd++;
                    wksheet.Cells[2, columnAdd] = "Remark";
                    rgStr = string.Format($"{MyExcelPrg.GetExcelColumnName(columnAdd)}2:{MyExcelPrg.GetExcelColumnName(columnAdd)}4");
                    mergerge = wksheet.get_Range(rgStr);
                    mergerge.Interior.Color = this.green;
                    mergerge.Merge();
                    mergerge.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                    // Daily Output Format
                    rgStr = string.Format($"{MyExcelPrg.GetExcelColumnName(columnAdd - 4)}2:{MyExcelPrg.GetExcelColumnName(columnAdd)}{rowAdd + 1}");
                    mergerge = wksheet.get_Range(rgStr);
                    mergerge.Font.Bold = true;

                    // Total,Ave Format
                    rgStr = string.Format($"A{rowAdd}:{MyExcelPrg.GetExcelColumnName(columnAdd - 1)}{rowAdd + 1}");
                    mergerge = wksheet.get_Range(rgStr);
                    mergerge.Font.Bold = true;
                    mergerge.Interior.Color = Color.FromArgb(192, 192, 192);
                    wksheet.Cells[daydiff + 5, 1] = "Total";
                    wksheet.Cells[daydiff + 6, 1] = "Average";

                    // 畫線
                    wksheet.get_Range($"A2:{MyExcelPrg.GetExcelColumnName(columnAdd)}{rowAdd + 1}").Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                    rgStr = $"A1:{MyExcelPrg.GetExcelColumnName(columnAdd)}1";
                    mergerge = wksheet.get_Range(rgStr);
                    mergerge.Merge();
                    mergerge.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                }

                objApp.Columns.AutoFit();
                this.Excelfile = Class.MicrosoftFile.GetName("Quality_R08_Summery");
                objApp.ActiveWorkbook.SaveAs(this.Excelfile);
                objApp.Quit();
                Marshal.ReleaseComObject(wksheet);
                Marshal.ReleaseComObject(objApp);
                this.Excelfile.OpenFile();
            }

            return true;
        }
    }
}
