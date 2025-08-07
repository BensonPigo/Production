using Ict;
using Sci.Data;
using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Quality
{
    /// <inheritdoc/>
    public partial class R12 : Win.Tems.PrintForm
    {
        private DataTable[] PrintData;
        private QA_R12_ViewModel model;

        /// <inheritdoc/>
        public R12(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.comboInspection.SelectedIndex = 0;
            this.comboInspectionResult.SelectedIndex = 0;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if (this.radioPanelTransaction.Value == "1")
            {
                if (this.dateArriveWHDate.Value1.Empty() &&
                    this.txtSP1.Text.Empty() && this.txtSP2.Text.Empty() &&
                    this.txtWK1.Text.Empty() && this.txtWK2.Text.Empty() &&
                    !this.dateInspectionDate.HasValue && this.txtbrand.Text.Empty())
                {
                    MyUtility.Msg.WarningBox("Arrive W/H Date, SP#, WK# , Brand and Inspection Date can't all empty!");
                    return false;
                }
            }
            else
            {
                if (this.dateArriveWHDate.Value1.Empty() &&
                    this.txtSP1.Text.Empty() && this.txtSP2.Text.Empty() &&
                    this.txtWK1.Text.Empty() && this.txtWK2.Text.Empty() && this.txtbrand.Text.Empty())
                {
                    MyUtility.Msg.WarningBox("Arrive W/H Date, SP#, Transfer ID , Brand can't all empty!");
                    return false;
                }
            }

            this.model = new QA_R12_ViewModel()
            {
                Transaction = this.radioPanelTransaction.Value == "1" ? 1 : 2,
                ArriveWHDate1 = this.dateArriveWHDate.Value1,
                ArriveWHDate2 = this.dateArriveWHDate.Value2,
                SP1 = this.txtSP1.Text.Trim(),
                SP2 = this.txtSP2.Text.Trim(),
                WK1 = this.txtWK1.Text.Trim(),
                WK2 = this.txtWK2.Text.Trim(),
                InspectionDate1 = this.dateInspectionDate.Value1,
                InspectionDate2 = this.dateInspectionDate.Value2,
                Brand = this.txtbrand.Text.Trim(),
                Inspection = this.comboInspection.Text.Trim(),
                InspectionResult = this.comboInspectionResult.Text.Trim(),
                ByWKSeq = this.radioWKSeq.Checked,
                ByRollDyelot = this.radioRollDyelot.Checked,
                IsPowerBI = false,
            };

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            // 資料邏輯跟BI共用
            QA_R12 biModel = new QA_R12();
            Base_ViewModel resultReport = biModel.GetQA_R12Data(this.model);
            if (!resultReport.Result)
            {
                return resultReport.Result;
            }

            this.PrintData = resultReport.DtArr;

            return resultReport.Result;
        }

        private void CreateExcel(string filename, string excelTitle, DataTable dtSrc)
        {
            int excelMaxRow = 1000000;
            int maxLoadRow = 250000;
            int rowCnt = dtSrc.Rows.Count;
            int loadTimes = Convert.ToInt32(Math.Ceiling((decimal)excelMaxRow / (decimal)maxLoadRow));
            DataTable dt;

            if (rowCnt > excelMaxRow)
            {
                Excel.Application excel = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\" + filename);

                excel.DisplayAlerts = false;
                Utility.Report.ExcelCOM comDetail = new Utility.Report.ExcelCOM(Env.Cfg.XltPathDir + "\\" + filename, excel);

                int sheetCnt = Convert.ToInt32(Math.Ceiling((decimal)rowCnt / (decimal)excelMaxRow));

                for (int i = 0; i < sheetCnt; i++)
                {
                    Excel.Worksheet worksheetA = (Excel.Worksheet)excel.ActiveWorkbook.Worksheets[i + 1];
                    Excel.Worksheet worksheetB = (Excel.Worksheet)excel.ActiveWorkbook.Worksheets[i + 2];
                    worksheetA.Copy(worksheetB);
                    ((Excel.Worksheet)excel.Sheets[i + 1]).Select();

                    for (int j = 0; j < loadTimes; j++)
                    {
                        int startRow = (i * excelMaxRow) + (j * maxLoadRow);
                        dt = dtSrc.AsEnumerable().Skip(startRow).Take(maxLoadRow).CopyToDataTable();

                        if (startRow > rowCnt)
                        {
                            break;
                        }

                        comDetail.WriteTable(dt, (j * maxLoadRow) + 2);
                    }
                }

                ((Excel.Worksheet)excel.Sheets[sheetCnt + 1]).Delete();
                ((Excel.Worksheet)excel.Sheets[1]).Select();
                this.SaveExcelwithName(excel, excelTitle);
            }
            else
            {
                dt = dtSrc;
                Excel.Application excelDetail = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\" + filename);
                Utility.Report.ExcelCOM comDetail = new Utility.Report.ExcelCOM(Env.Cfg.XltPathDir + "\\" + filename, excelDetail)
                {
                    ColumnsAutoFit = true,
                };

                comDetail.WriteTable(dt, 2);
                this.SaveExcelwithName(excelDetail, excelTitle);
            }
        }

        private void SaveExcelwithName(Excel.Application excelapp, string filename)
        {
            string strExcelName = Class.MicrosoftFile.GetName(filename);
            Excel.Workbook workbook = excelapp.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            excelapp.Quit();
            Marshal.ReleaseComObject(excelapp);          // 釋放objApp
            Marshal.ReleaseComObject(workbook);
            strExcelName.OpenFile();
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.PrintData.Sum(s => s.Rows.Count));

            if (!this.PrintData.Where(s => s.Rows.Count > 0).Any())
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            this.ShowWaitMessage("Starting EXCEL...");
            string transaction = this.radioPanelTransaction.Value == "1" ? string.Empty : "B2A_";
            string reportType = this.radioWKSeq.Checked ? "WKSeq" : "RollDyelot";
            string reportTitle = this.radioWKSeq.Checked ? " By WK, Seq" : " By Roll, Dyelot";

            if (this.comboInspection.Text == "All")
            {
                this.CreateExcel($"Quality_R12_{transaction}Physical{reportType}.xltx", $"R12 Fabric Physical Inspection List{reportTitle}", this.PrintData[0]);
                this.CreateExcel($"Quality_R12_{transaction}Weight{reportType}.xltx", $"R12 Fabric Weight Test List{reportTitle}", this.PrintData[1]);
                this.CreateExcel($"Quality_R12_{transaction}ShadeBond{reportType}.xltx", $"R12 Fabric Shade Band Test List{reportTitle}", this.PrintData[2]);
                this.CreateExcel($"Quality_R12_{transaction}Continuity{reportType}.xltx", $"R12 Fabric Continuilty Test List{reportTitle}", this.PrintData[3]);
                this.CreateExcel($"Quality_R12_{transaction}Odor{reportType}.xltx", $"R12 Fabric Odor Test List{reportTitle}", this.PrintData[4]);
            }
            else
            {
                switch (this.comboInspection.Text)
                {
                    case "Physical":
                        this.CreateExcel($"Quality_R12_{transaction}Physical{reportType}.xltx", $"R12 Fabric Physical Inspection List{reportTitle}", this.PrintData[0]);
                        break;
                    case "Weight":
                        this.CreateExcel($"Quality_R12_{transaction}Weight{reportType}.xltx", $"R12 Fabric Weight Test List{reportTitle}", this.PrintData[0]);
                        break;
                    case "Shade Band":
                        this.CreateExcel($"Quality_R12_{transaction}ShadeBond{reportType}.xltx", $"R12 Fabric Shade Band Test List{reportTitle}", this.PrintData[0]);
                        break;
                    case "Continuity":
                        this.CreateExcel($"Quality_R12_{transaction}Continuity{reportType}.xltx", $"R12 Fabric Continuilty Test List{reportTitle}", this.PrintData[0]);
                        break;
                    case "Odor":
                        this.CreateExcel($"Quality_R12_{transaction}Odor{reportType}.xltx", $"R12 Fabric Odor Test List{reportTitle}", this.PrintData[0]);
                        break;
                    default:
                        break;
                }
            }

            this.HideWaitMessage();
            return true;
        }

        private void RadioPanelTransaction_ValueChanged(object sender, EventArgs e)
        {
            switch (this.radioPanelTransaction.Value)
            {
                case "1":
                    this.lbDate.Text = "Arrive W/H Date";
                    this.lbSP.Text = "SP#";
                    this.label3.Text = "WK#";
                    this.label6.RectStyle.Color = Color.SkyBlue;
                    this.label6.TextStyle.Color = Color.Black;
                    this.radioWKSeq.Text = "By WK#, Seq";
                    break;
                case "2":
                    this.lbDate.Text = "Issue Date";
                    this.lbSP.Text = "Bulk SP#";
                    this.label3.Text = "Transfer ID";
                    this.label6.RectStyle.Color = Color.Gray;
                    this.label6.TextStyle.Color = Color.White;
                    this.radioWKSeq.Text = "By Transfer ID, Seq";
                    break;
            }
        }
    }
}
