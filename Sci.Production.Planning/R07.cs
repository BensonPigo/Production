using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using System.Runtime.InteropServices;

namespace Sci.Production.Planning
{
    /// <summary>
    /// R07
    /// </summary>
    public partial class R07 : Win.Tems.PrintForm
    {
        private string factory;
        private string mdivision;
        private string cdCode;
        private string shift;
        private DateTime? outputDate1;
        private DateTime? outputDate2;
        private DataTable printData;
        private StringBuilder condition = new StringBuilder();

        /// <summary>
        /// R07
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R07(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            MyUtility.Tool.SetupCombox(this.comboShift, 2, 1, ",,D,Day,N,Night");
        }

        /// <summary>
        /// ValidateInput
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(this.dateOutputDate.Value1))
            {
                MyUtility.Msg.WarningBox(" < Output Date > can't be empty!!");
                return false;
            }

            #region -- 擇一必輸的條件 --
            this.outputDate1 = this.dateOutputDate.Value1;
            this.outputDate2 = this.dateOutputDate.Value2;
            #endregion
            this.mdivision = this.txtMdivision.Text;
            this.factory = this.txtfactory.Text;
            this.cdCode = this.txtCDCode.Text;
            this.shift = this.comboShift.SelectedValue.ToString();
            return base.ValidateInput();
        }

        /// <summary>
        /// OnAsyncDataLoad
        /// </summary>
        /// <param name="e">e</param>
        /// <returns>DualResult</returns>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            string sqlCmd = string.Format(
                "exec dbo.GetAdidasEfficiencyReport '{0}', '{1}', '{2}', '{3}', '{4}', '{5}'",
                this.outputDate1.Value.ToString("yyyy/MM/dd"),
                this.outputDate2.Value.ToString("yyyy/MM/dd"),
                this.mdivision,
                this.factory,
                this.cdCode,
                this.shift);

            DBProxy.Current.DefaultTimeout = 1800;
            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), null, out this.printData);

            DBProxy.Current.DefaultTimeout = 0;
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return Ict.Result.True;
        }

        /// <summary>
        /// OnToExcel
        /// </summary>
        /// <param name="report">report</param>
        /// <returns>bool</returns>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.printData.Rows.Count);

            if (this.printData.Columns.Count > 16384)
            {
                MyUtility.Msg.WarningBox("Columns of Data is over 16,384 in excel file, please narrow down range of condition.");
                return false;
            }

            if (this.printData.Rows.Count + 6 > 1048576)
            {
                MyUtility.Msg.WarningBox("Lines of Data is over 1,048,576 in excel file, please narrow down range of condition.");
                return false;
            }

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Planning_R07.xltx"); // 預先開啟excel app
            MyUtility.Excel.CopyToXls(this.printData, string.Empty, "Planning_R07.xltx", 1, false, null, objApp);      // 將datatable copy to excel
            objApp.Visible = false;

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("Planning_R07");
            Microsoft.Office.Interop.Excel.Workbook workbook = objApp.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);
            Marshal.ReleaseComObject(workbook);

            strExcelName.OpenFile();
            #endregion
            return true;
        }
    }
}
