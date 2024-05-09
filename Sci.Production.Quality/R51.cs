using Ict;
using Sci.Data;
using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Quality
{
    /// <inheritdoc/>
    public partial class R51 : Win.Tems.PrintForm
    {
        private readonly List<SqlParameter> Parameters = new List<SqlParameter>();
        private readonly StringBuilder Sqlcmd = new StringBuilder();
        private string sqlCol;
        private DataTable PrintData;
        private DataTable CustomColumnDt;
        private string strM;
        private string strFactory;
        private string strShift;

        /// <inheritdoc/>
        public R51(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.comboMDivision1.SetDefalutIndex();
            this.comboFactory1.SetDataSource();
            MyUtility.Tool.SetupCombox(this.comboShift, 1, 1, ",Day,Night");
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.CustomColumnDt = null;

            if (this.dateInspectionDate.Value1.Empty() && this.txtSP.Text.Empty())
            {
                MyUtility.Msg.WarningBox("<Inspection Date>, <SP#> can not all empty!");
                return false;
            }

            if (MyUtility.Check.Empty(this.txtsubprocess.Text))
            {
                MyUtility.Msg.WarningBox(" <SubProcess> can not empty!");
                return false;
            }

            this.strM = this.comboMDivision1.Text;
            this.strFactory = this.comboFactory1.Text;
            this.strShift = this.comboShift.Text;
            return true;
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            this.sqlCol = string.Empty;
            string subProcessCondition = this.txtsubprocess.Text.Split(',').Select(s => $"'{s}'").JoinToString(",");
            this.sqlCol = $@"select AssignColumn,DisplayName,SubProcessID from SubProCustomColumn where SubProcessID in ({subProcessCondition}) order by AssignColumn";

            DBProxy.Current.DefaultTimeout = 600; // 加長時間成10分鐘, 避免time out
            QA_R51 biModel = new QA_R51();
            QA_R51_ViewModel qa_R51_Model = new QA_R51_ViewModel()
            {
                StartInspectionDate = this.dateInspectionDate.Value1,
                EndInspectionDate = this.dateInspectionDate.Value2,
                M = this.strM,
                Factory = this.strFactory,
                Shift = this.strShift,
                SubProcess = subProcessCondition,
                SP = this.txtSP.Text,
                Style = this.txtstyle1.Text,
                FormatType = this.radioSummary.Checked ? "Summary" : this.radioDetail_DefectType.Checked ? "DefectType" : this.radioDetail_Responseteam.Checked ? "ResponseTeam" : "Operator",
                IsBI = false,
            };

            Base_ViewModel resultReport = biModel.Get_QA_R51(qa_R51_Model);
            if (!this.sqlCol.Empty())
            {
                DualResult result = DBProxy.Current.Select(null, this.sqlCol, out this.CustomColumnDt);
                if (!result)
                {
                    return result;
                }
            }

            this.PrintData = resultReport.DtArr[0];

            DBProxy.Current.DefaultTimeout = 300;  // timeout時間改回5分鐘
            return Ict.Result.True;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.PrintData.Rows.Count);

            if (this.PrintData.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            this.PrintData.Columns.Remove("BundleNoCT");

            string filename = string.Empty;

            if (this.radioSummary.Checked)
            {
                filename = "Quality_R51_Summary.xltx";
            }
            else if (this.radioDetail_DefectType.Checked)
            {
                filename = "Quality_R51_Detail_DefectType.xltx";
            }
            else if (this.radioDetail_Operator.Checked)
            {
                filename = "Quality_R51_Detail_Operator.xltx";
            }
            else
            {
                filename = "Quality_R51_Detail_Responseteam.xltx";
            }

            Excel.Application excelApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\" + filename); // 預先開啟excel app

            Excel.Workbook xlWb = excelApp.ActiveWorkbook as Excel.Workbook;
            Excel.Worksheet xlSht = xlWb.Sheets[1];

            var listSubProcess = this.PrintData.AsEnumerable().Select(s => s["SubProcessID"].ToString()).Distinct();
            int sheetCnt = 2;
            int customColumn = this.PrintData.Columns.Count; // 自定義欄位最後一欄

            foreach (string subprocessID in listSubProcess)
            {
                xlSht.Copy(Type.Missing, xlWb.Sheets[1]);

                DataTable dtSubprocess = this.PrintData.AsEnumerable().Where(s => s["SubProcessID"].ToString() == subprocessID).CopyToDataTable();

                MyUtility.Excel.CopyToXls(dtSubprocess, string.Empty, filename, 2, false, null, excelApp, wSheet: excelApp.Sheets[sheetCnt]);
                xlWb.Sheets[sheetCnt].Name = subprocessID;

                if (this.CustomColumnDt != null)
                {
                    var custColumnNames = this.CustomColumnDt.AsEnumerable().Where(s => s["SubProcessID"].ToString() == subprocessID);
                    foreach (DataRow dr in custColumnNames)
                    {
                        xlWb.Sheets[sheetCnt].Cells[1, customColumn] = dr["DisplayName"];
                        customColumn++;
                    }

                    customColumn = this.PrintData.Columns.Count;
                }
            }

            excelApp.DisplayAlerts = false;
            xlSht.Delete();

            excelApp.Visible = true;
            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName(filename);

            xlWb.SaveAs(strExcelName);

            Marshal.ReleaseComObject(xlSht);
            Marshal.ReleaseComObject(xlWb);
            Marshal.ReleaseComObject(excelApp);
            #endregion
            return true;
        }

        private void TxtShiftTime_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!TimeSpan.TryParse(((Sci.Win.UI.TextBox)sender).Text, out TimeSpan _))
            {
                e.Cancel = true;
                MyUtility.Msg.WarningBox("Incorrect time format");
            }
        }
    }
}
