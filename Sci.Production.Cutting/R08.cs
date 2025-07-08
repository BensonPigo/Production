using Ict;
using Sci.Data;
using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;
using Sci.Win;
using System;
using System.Data;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Cutting
{
    /// <inheritdoc/>
    public partial class R08 : Win.Tems.PrintForm
    {
        private DataTable[] printData;
        private string Mdivision;
        private string Factory;
        private DateTime? EstCutDate1;
        private DateTime? EstCutDate2;
        private DateTime? ActCutDate1;
        private DateTime? ActCutDate2;
        private string CuttingSP;
        private decimal? Speed;
        private decimal? WorkHoursDay;

        /// <inheritdoc/>
        public R08(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.Mdivision = this.txtMdivision1.Text;
            this.Factory = this.txtfactory1.Text;
            this.EstCutDate1 = this.dateEstCutDate.Value1;
            this.EstCutDate2 = this.dateEstCutDate.Value2;
            this.ActCutDate1 = this.dateActCutDate.Value1;
            this.ActCutDate2 = this.dateActCutDate.Value2;
            this.CuttingSP = this.txtCuttingSp.Text;
            this.Speed = this.numSpeed.Value;
            this.WorkHoursDay = this.numWorkHourDay.Value;
            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            Cutting_R08_ViewModel model = new Cutting_R08_ViewModel()
            {
                MDivisionID = this.Mdivision,
                FactoryID = this.Factory,
                EstCutDate1 = this.EstCutDate1,
                EstCutDate2 = this.EstCutDate2,
                ActCutDate1 = this.ActCutDate1,
                ActCutDate2 = this.ActCutDate2,
                CuttingSP = this.txtCuttingSp.Text,
                IsPowerBI = false,
            };

            Cutting_R08 biModel = new Cutting_R08();
            Base_ViewModel resultReport = biModel.GetActualCutOutput(model);
            if (!resultReport.Result)
            {
                return resultReport.Result;
            }

            this.printData = resultReport.DtArr;

            return resultReport.Result;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(ReportDefinition report)
        {
            this.ShowWaitMessage("Excel Processing...");
            this.SetCount(this.printData[0].Rows.Count);
            if (this.printData[0].Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                this.HideWaitMessage();
                return false;
            }

            string excelName = "Cutting_R08";
            Excel.Application excelApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + $"\\{excelName}.xltx");
            this.printData[0].Columns.Remove("MDivisionid");
            MyUtility.Excel.CopyToXls(this.printData[0], string.Empty, $"{excelName}.xltx", 1, false, null, excelApp, wSheet: excelApp.Sheets[1]); // 將datatable copy to excel
            excelApp.DisplayAlerts = false;
            Excel.Worksheet worksheet = excelApp.ActiveWorkbook.Worksheets[2]; // 取得工作表
            DataTable sodt = this.printData[4];
            DataTable codt = this.printData[5];
            int s = sodt.Rows.Count;

            worksheet.Cells[1, 2] = this.printData[1].Rows[0][0];
            worksheet.Cells[2, 2] = DateTime.Now;
            worksheet.Cells[1, 6] = this.printData[2].Rows[0][0];
            worksheet.Cells[2, 6] = this.printData[3].Rows[0][0];
            string sCol = MyUtility.Excel.ConvertNumericToExcelColumn(s + 1);
            string cCol = MyUtility.Excel.ConvertNumericToExcelColumn(codt.Rows.Count + 1);
            worksheet.Cells[2, 3] = $"=AVERAGE(B13:{sCol}13)";
            worksheet.Cells[2, 4] = $"=AVERAGE(B29:{cCol}29)";
            worksheet.Range[$"B4:{sCol}13"].Borders.Weight = 2; // 設定全框線
            worksheet.Range[$"B17:{cCol}29"].Borders.Weight = 2; // 設定全框線
            #region Spreading Output
            string col = string.Empty;
            for (int i = 0; i < s; i++)
            {
                worksheet.Cells[4, i + 2] = sodt.Rows[i]["SpreadingNoID"];
                col = MyUtility.Excel.ConvertNumericToExcelColumn(i + 2);
                worksheet.Cells[5, i + 2] = this.WorkHoursDay;
                worksheet.Cells[6, i + 2] = $"={col}5*F$2";
                worksheet.Cells[7, i + 2] = sodt.Rows[i]["TotalSpreadingYardage"];
                worksheet.Cells[8, i + 2] = sodt.Rows[i]["TotalSpreadingMarkerQty"];
                worksheet.Cells[9, i + 2] = sodt.Rows[i]["TotalSpreadingTime_hr"];
                worksheet.Cells[10, i + 2] = $"=({col}9/{col}6)";
                worksheet.Cells[11, i + 2] = $"={col}9-{col}6";
                worksheet.Cells[12, i + 2] = $"=CONCATENATE(IF(({this.Speed}*{col}5)=0,0,Round({col}7/({this.Speed}*{col}5),2)),\" | \",IF({col}6=0,0,Round({col}9/{col}6,2)),\" | \")";
                worksheet.Cells[13, i + 2] = $"=IF(({this.Speed}*{col}5)=0,0,Round({col}7/({this.Speed}*{col}5),2))*IF({col}6=0,0,Round({col}9/{col}6,2))/100";
            }

            col = col.EqualString(string.Empty) ? "A" : col;
            worksheet.get_Range("A3", col + "3").Merge(false);

            #endregion
            #region Cutting Output
            for (int i = 0; i < codt.Rows.Count; i++)
            {
                worksheet.Cells[17, i + 2] = codt.Rows[i]["CutCellid"];
                worksheet.Cells[18, i + 2] = codt.Rows[i]["CuttingMachDescription"];
                col = MyUtility.Excel.ConvertNumericToExcelColumn(i + 2);
                worksheet.Cells[19, i + 2] = this.WorkHoursDay;
                worksheet.Cells[20, i + 2] = $"={col}19*F$2";
                worksheet.Cells[21, i + 2] = codt.Rows[i]["AvgCutSpeedMperMin"];
                worksheet.Cells[22, i + 2] = codt.Rows[i]["TotalCuttingPerimeter"];
                worksheet.Cells[23, i + 2] = codt.Rows[i]["TotalCutMarkerQty"];
                worksheet.Cells[24, i + 2] = codt.Rows[i]["TotalCutFabricYardage"];
                worksheet.Cells[25, i + 2] = codt.Rows[i]["TotalCuttingTime_hrs"];
                worksheet.Cells[26, i + 2] = $"=({col}25/{col}20)";
                worksheet.Cells[27, i + 2] = $"={col}25-{col}20";
                worksheet.Cells[28, i + 2] = $"=CONCATENATE(IF(({this.Speed}*{col}19)=0,0,Round({col}22/({this.Speed}*{col}19),2)),\" | \",IF({col}20=0,0,Round({col}25/{col}20,2)),\" | \")";
                worksheet.Cells[29, i + 2] = $"=IF(({this.Speed}*{col}19)=0,0,Round({col}22/({this.Speed}*{col}19),2))*IF({col}20=0,0,Round({col}25/{col}20,2))/100";
            }

            col = col.EqualString(string.Empty) ? "A" : col;
            worksheet.get_Range("A16", col + "16").Merge(false);
            #endregion
            #region sheet Balancing Chart
            worksheet = excelApp.ActiveWorkbook.Worksheets[3]; // 取得工作表

            for (int i = 0; i < s + codt.Rows.Count - 2; i++)
            {
                worksheet.Rows[i + 3, Type.Missing].Insert(Excel.XlDirection.xlDown);
            }

            for (int i = 0; i < s; i++)
            {
                col = MyUtility.Excel.ConvertNumericToExcelColumn(i + 2);
                worksheet.Cells[i + 2, 1] = $"='Actual Output Summary'!{col}4";
                worksheet.Cells[i + 2, 2] = $"='Actual Output Summary'!{col}8";
                worksheet.Cells[i + 2, 4] = $"='Actual Output Summary'!{col}10";
            }

            for (int i = 0; i < codt.Rows.Count; i++)
            {
                col = MyUtility.Excel.ConvertNumericToExcelColumn(i + 2);
                worksheet.Cells[i + s + 2, 1] = $"='Actual Output Summary'!{col}17";
                worksheet.Cells[i + s + 2, 3] = $"='Actual Output Summary'!{col}23";
                worksheet.Cells[i + s + 2, 4] = $"='Actual Output Summary'!{col}26";
            }

            worksheet.Visible = Excel.XlSheetVisibility.xlSheetHidden; // 隱藏第3頁sheet
            #endregion
            worksheet = excelApp.ActiveWorkbook.Worksheets[1]; // 取得工作表

            // worksheet.Columns.AutoFit();
            #region 釋放上面開啟過excel物件
            string strExcelName = Class.MicrosoftFile.GetName(excelName);
            Excel.Workbook workbook = excelApp.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            excelApp.Quit();

            if (worksheet != null)
            {
                Marshal.FinalReleaseComObject(worksheet);
            }

            if (excelApp != null)
            {
                Marshal.FinalReleaseComObject(excelApp);
            }
            #endregion

            this.HideWaitMessage();
            strExcelName.OpenFile();
            return true;
        }
    }
}
