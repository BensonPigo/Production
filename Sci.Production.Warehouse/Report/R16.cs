using Ict;
using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Data;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class R16 : Win.Tems.PrintForm
    {
        private string mdivision;
        private string factory;
        private string request1;
        private string request2;
        private string sp1;
        private string sp2;
        private DateTime? issueDate1;
        private DateTime? issueDate2;
        private DataTable printData;

        /// <inheritdoc/>
        public R16(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.txtMdivision.Text = Env.User.Keyword;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(this.dateIssueDate.Value1) && MyUtility.Check.Empty(this.dateIssueDate.Value2))
            {
                MyUtility.Msg.WarningBox("Issue date can't be empty!!");
                return false;
            }

            this.issueDate1 = this.dateIssueDate.Value1;
            this.issueDate2 = this.dateIssueDate.Value2;
            this.mdivision = this.txtMdivision.Text;
            this.factory = this.txtfactory.Text;
            this.request1 = this.txtRquest1.Text;
            this.request2 = this.txtRquest2.Text;
            this.sp1 = this.txtSP1.Text;
            this.sp2 = this.txtSP2.Text;

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            Warehouse_R16_ViewModel model = new Warehouse_R16_ViewModel()
            {
                IssueDateFrom = this.issueDate1,
                IssueDateTo = this.issueDate2,
                MDivisionID = this.mdivision,
                FactoryID = this.factory,
                CutplanIDFrom = this.request1,
                CutplanIDTo = this.request2,
                SPFrom = this.sp1,
                SPTo = this.sp2,
                EditDateFrom = null,
                EditDateTo = null,
            };

            Warehouse_R16 biModel = new Warehouse_R16();
            Base_ViewModel resultReport = biModel.GetIssueFabricByCuttingTransactionList(model);
            if (!resultReport.Result)
            {
                return resultReport.Result;
            }

            this.printData = resultReport.Dt;
            this.printData.Columns.Remove("AddDate");
            this.printData.Columns.Remove("EditDate");
            this.printData.Columns.Remove("StockType");
            this.printData.Columns.Remove("Issue_DetailUkey");

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

            this.ShowWaitMessage("Starting EXCEL...");
            string reportName = "Warehouse_R16.xltx";
            Excel.Application excelApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + $"\\{reportName}");
            MyUtility.Excel.CopyToXls(this.printData, string.Empty, reportName, 1, false, null, excelApp, wSheet: excelApp.Sheets[1]);

            #region 釋放上面開啟過excel物件
            string strExcelName = Class.MicrosoftFile.GetName("Warehouse_R16");
            Excel.Workbook workbook = excelApp.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            excelApp.Quit();

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
