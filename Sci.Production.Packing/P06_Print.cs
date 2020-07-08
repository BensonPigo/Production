using System;
using System.Data;
using Ict;

namespace Sci.Production.Packing
{
    /// <summary>
    /// Packing_P06_Print
    /// </summary>
    public partial class P06_Print : Win.Tems.PrintForm
    {
        private string reportType;
        private string ctn1;
        private string ctn2;
        private string specialInstruction;
        private DataTable printData;
        private DataTable ctnDim;
        private DataTable qtyCtn;
        private DataTable articleSizeTtlShipQty;
        private DataTable printGroupData;
        private DataTable clipData;
        private int orderQty;
        private DataRow masterData;

        /// <summary>
        /// P06_Print
        /// </summary>
        /// <param name="masterData">MasterData</param>
        /// <param name="orderQty">orderQty</param>
        public P06_Print(DataRow masterData, int orderQty)
        {
            this.InitializeComponent();
            this.masterData = masterData;
            this.orderQty = orderQty;
            this.radioPackingGuideReport.Checked = true;
            this.ControlPrintFunction(true);
        }

        // Packing Guide Report
        private void RadioPackingGuideReport_CheckedChanged(object sender, EventArgs e)
        {
            this.ControlPrintFunction(this.radioPackingGuideReport.Checked);
        }

        // 控制元件是否可使用
        private void ControlPrintFunction(bool isSupport)
        {
            this.IsSupportToPrint = !isSupport;
            this.IsSupportToExcel = isSupport;
            this.txtCTNStart.Enabled = !isSupport;
            this.txtCTNEnd.Enabled = !isSupport;
            if (isSupport)
            {
                this.txtCTNStart.Text = string.Empty;
                this.txtCTNEnd.Text = string.Empty;
            }
        }

        /// <summary>
        /// ValidateInput驗證輸入條件
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ValidateInput()
        {
            this.reportType = this.radioPackingGuideReport.Checked ? "1" : "2";
            this.ctn1 = this.txtCTNStart.Text;
            this.ctn2 = this.txtCTNEnd.Text;
            this.ReportResourceName = "BarcodePrint.rdlc";

            return base.ValidateInput();
        }

        /// <summary>
        /// ToPrint
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ToPrint()
        {
            this.ValidateInput();

            this.ShowWaitMessage("Data Loading ...");
            DualResult result = new PackingPrintBarcode().PrintBarcode(this.masterData["ID"].ToString(), this.ctn1, this.ctn2);

            if (result == false)
            {
                MyUtility.Msg.WarningBox(result.ToString());
                return false;
            }

            this.HideWaitMessage();
            return true;
        }

        /// <summary>
        /// OnAsyncDataLoad非同步取資料
        /// </summary>
        /// <param name="e">e</param>
        /// <returns>DualResult</returns>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            if (this.reportType == "1")
            {
                DualResult result = PublicPrg.Prgs.QueryPackingGuideReportData(MyUtility.Convert.GetString(this.masterData["ID"]), out this.printData, out this.ctnDim, out this.qtyCtn, out this.articleSizeTtlShipQty, out this.printGroupData, out this.clipData, out this.specialInstruction);
                return result;
            }
            else
            {
                DualResult result = PublicPrg.Prgs.PackingBarcodePrint(MyUtility.Convert.GetString(this.masterData["ID"]), this.ctn1, this.ctn2, out this.printData);
                if (!result)
                {
                    return result;
                }

                e.Report.ReportDataSource = this.printData;
            }

            return Result.True;
        }

        /// <summary>
        /// OnToExcel產生Excel
        /// </summary>
        /// <param name="report">report</param>
        /// <returns>bool</returns>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            this.ShowWaitMessage("Data Loading....");
            if (this.reportType == "1")
            {
                PublicPrg.Prgs.PackingListToExcel_PackingGuideReport("\\Packing_P03_PackingGuideReport.xltx", this.printData, this.ctnDim, this.qtyCtn, this.articleSizeTtlShipQty, this.printGroupData, this.clipData, this.masterData, this.orderQty, this.specialInstruction, false);
            }

            this.HideWaitMessage();
            return true;
        }
    }
}
