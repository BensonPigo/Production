using Ict;
using Sci.Data;
using Sci.Production.Prg;
using Sci.Win;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static Sci.MyUtility;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Packing
{
    public partial class R05 : Sci.Win.Tems.PrintForm
    {
        private DataTable dtPrint;

        public R05(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }

        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(this.txtMachineNo.Text) && MyUtility.Check.Empty(this.dateCalibrationDate.Value1) && MyUtility.Check.Empty(this.dateCalibrationDate.Value2))
            {
                MyUtility.Msg.WarningBox("Machine# and Calibration Date cannot be empty!");
                return false;
            }

            return base.ValidateInput();
        }

        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            DualResult result = Result.True;
            string sqlcmd = string.Empty;
            List<SqlParameter> pars = new List<SqlParameter>();
            #region Where
            string strwhere = string.Empty;
            if (!MyUtility.Check.Empty(this.txtMachineNo.Text))
            {
                strwhere += $" and MachineID in (select Data From SplitString(@MachineNo,','))" + Environment.NewLine;
                pars.Add(new SqlParameter("@MachineNo", this.txtMachineNo.Text));
            }

            if (!MyUtility.Check.Empty(this.dateCalibrationDate.Value1))
            {
                strwhere += $" and CalibrationDate >= @CalibrationDate1" + Environment.NewLine;
                pars.Add(new SqlParameter("@CalibrationDate1", ((DateTime)this.dateCalibrationDate.Value1).ToString("yyyy-MM-dd")));
            }

            if (!MyUtility.Check.Empty(this.dateCalibrationDate.Value2))
            {
                strwhere += $" and CalibrationDate <= @CalibrationDate2" + Environment.NewLine;
                pars.Add(new SqlParameter("@CalibrationDate2", ((DateTime)this.dateCalibrationDate.Value2).ToString("yyyy-MM-dd")));
            }
            #endregion

            sqlcmd = $@"
select MachineID,
CalibrationDate, 
CalibrationTime = substring(convert(varchar,CalibrationTime),0,9), -- 轉字串不然ToExcle會error, 去除多餘的0
Result =
IIF(
	Point1 = 1 and 
	Point2 = 1 and
	point3 = 1 and
	point4 = 1 and
	point5 = 1 and
	point6 = 1 and
	point7 = 1 and
	point8 = 1 and
	point9 = 1 
	,'Pass','Fail')
from MDCalibrationList 
where SubmitDate is not null
{strwhere}

";
            result = DBProxy.Current.Select(null, sqlcmd, pars, out this.dtPrint);
            if (!result)
            {
                return result;
            }

            return Ict.Result.True;
        }

        protected override bool OnToExcel(ReportDefinition report)
        {
            #region check printData
            if (this.dtPrint.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found.");
                return false;
            }

            #endregion
            this.SetCount(this.dtPrint.Rows.Count);
            this.ShowWaitMessage("Excel Processing");

            #region To Excel
            string excelName = "Packing_R05";
            Excel.Application excelApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + $"\\{excelName}.xltx");
            ExcelPrg.CopyToXlsAutoSplitSheet(this.dtPrint, string.Empty, $"{excelName}.xltx", 1, false, null, excelApp, wSheet: excelApp.Sheets[1]);

            Excel.Worksheet worksheet = excelApp.ActiveWorkbook.Worksheets[1];
            worksheet.Columns.AutoFit();

            #endregion
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

        private void txtMachineNo_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sql = $@"
Select MachineID,Operator from MDMachineBasic
where Junk = 0";

            DualResult result = DBProxy.Current.Select(string.Empty, sql,out DataTable dt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            using (var dlg = new SelectItem2(dt, "MachineID,Operator", "MachineID,Operator", "13,13", this.txtMachineNo.Text))
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    this.txtMachineNo.Text = dlg.GetSelectedString();
                }
            }
        }

        private void txtMachineNo_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string strMachine = this.txtMachineNo.Text;
            if (!string.IsNullOrWhiteSpace(strMachine) && strMachine != this.txtMachineNo.OldValue)
            {
                List<SqlParameter> pars = new List<SqlParameter>();
                pars.Add(new SqlParameter("@MachineNo", this.txtMachineNo.Text));
                if (MyUtility.Check.Seek($@"Select MachineID from MDMachineBasic where Junk = 0 and MachineID in (select Data From SplitString(@MachineNo,','))", pars) == false)
                {
                    this.txtMachineNo.Text = string.Empty;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("< Machine# : {0} > not found!!!", strMachine));
                    return;
                }
            }
        }
    }
}
