using Ict;
using Sci.Data;
using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Data;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Cutting
{
    /// <inheritdoc/>
    public partial class R13 : Win.Tems.PrintForm
    {
        private string mMDivision;
        private string factory;
        private string CuttingSP1;
        private string CuttingSP2;
        private string Style;
        private DateTime? Est_CutDate1;
        private DateTime? Est_CutDate2;
        private DateTime? ActCuttingDate1;
        private DateTime? ActCuttingDate2;
        private string strWhere;
        private DataTable printData;

        /// <inheritdoc/>
        public R13(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            DBProxy.Current.Select(null, "select distinct MDivisionID from WorkOrderForOutput WITH (NOLOCK) ", out DataTable workOrder);
            MyUtility.Tool.SetupCombox(this.comboMDivision, 1, workOrder);
            DBProxy.Current.Select(null, "select '' as ID union all select distinct FtyGroup from Factory WITH (NOLOCK) ", out DataTable factory); // 要預設空白
            MyUtility.Tool.SetupCombox(this.comboFactory, 1, factory);
            this.comboMDivision.Text = Env.User.Keyword;
            this.comboFactory.SelectedIndex = 0;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.strWhere = string.Empty;
            this.mMDivision = this.comboMDivision.Text;
            this.factory = this.comboFactory.Text;
            this.Style = this.txtstyle.Text;
            this.CuttingSP1 = this.txtCuttingSPStart.Text;
            this.CuttingSP2 = this.txtCuttingSPEnd.Text;
            this.Est_CutDate1 = this.dateEstCutDate.Value1;
            this.Est_CutDate2 = this.dateEstCutDate.Value2;
            this.ActCuttingDate1 = this.dateActCuttingDate.Value1;
            this.ActCuttingDate2 = this.dateActCuttingDate.Value2;

            if (MyUtility.Check.Empty(this.Est_CutDate1) &&
                MyUtility.Check.Empty(this.Est_CutDate1) &&
                MyUtility.Check.Empty(this.CuttingSP1) &&
                MyUtility.Check.Empty(this.CuttingSP2) &&
                MyUtility.Check.Empty(this.ActCuttingDate1) &&
                MyUtility.Check.Empty(this.ActCuttingDate2))
            {
                MyUtility.Msg.WarningBox("Please input  one of [Est. Cut Date]、[Act. Cutting Date]、[Cutting SP#] first!");
                return false;
            }

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            Cutting_R13_ViewModel model = new Cutting_R13_ViewModel()
            {
                MDivisionID = this.mMDivision,
                FactoryID = this.factory,
                StyleID = this.Style,
                CuttingSP1 = this.CuttingSP1,
                CuttingSP2 = this.CuttingSP2,
                Est_CutDate1 = this.Est_CutDate1,
                Est_CutDate2 = this.Est_CutDate2,
                ActCuttingDate1 = this.ActCuttingDate1,
                ActCuttingDate2 = this.ActCuttingDate2,
            };

            Cutting_R13 biModel = new Cutting_R13();
            Base_ViewModel resultReport = biModel.GetCuttingScheduleOutputData(model);
            if (!resultReport.Result)
            {
                return resultReport.Result;
            }

            this.printData = resultReport.Dt;
            return Ict.Result.True;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.printData.Rows.Count);

            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Cutting_R13.xltx"); // 預先開啟excel app
            MyUtility.Excel.CopyToXls(this.printData, string.Empty, "Cutting_R13.xltx", 1, false, null, objApp);      // 將datatable copy to excel
            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("Cutting_R13");
            Microsoft.Office.Interop.Excel.Workbook workbook = objApp.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            objApp.Quit();
            strExcelName.OpenFile();
            #endregion
            return true;
        }
    }
}
