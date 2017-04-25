using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.Packing
{
    public partial class P03_Print : Sci.Win.Tems.PrintForm
    {
        DataRow masterData;
        int orderQty;
        string reportType, ctn1, ctn2, specialInstruction;
        DataTable printData, ctnDim, qtyCtn, articleSizeTtlShipQty, printGroupData, clipData, qtyBDown;
        public P03_Print(DataRow MasterData, int OrderQty)
        {
            InitializeComponent();
            masterData = MasterData;
            orderQty = OrderQty;
            radioPackingListReportFormA.Checked = true;
            ControlPrintFunction(false);
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            ControlPrintFunction(radioBarcodePrint.Checked);
        }

        //控制元件是否可使用
        private void ControlPrintFunction(bool isSupport)
        {
            this.IsSupportToPrint = isSupport;
            this.IsSupportToExcel = !isSupport;
            txtCTNStart.Enabled = isSupport;
            txtCTNEnd.Enabled = isSupport;
            if (!isSupport)
            {
                txtCTNStart.Text = "";
                txtCTNEnd.Text = "";
            }
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            reportType = radioPackingListReportFormA.Checked ? "1" : radioPackingListReportFormB.Checked ? "2" : radioPackingGuideReport.Checked ? "3" : "4";
            ctn1 = txtCTNStart.Text;
            ctn2 = txtCTNEnd.Text;
            ReportResourceName = "BarcodePrint.rdlc";

            return base.ValidateInput();
        }

        // 非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            if (reportType == "1" || reportType == "2")
            {
                DualResult result = PublicPrg.Prgs.QueryPackingListReportData(MyUtility.Convert.GetString(masterData["ID"]), reportType, out printData, out ctnDim, out qtyBDown);
                return result;
            }
            else if (reportType == "3")
            {
                DualResult result = PublicPrg.Prgs.QueryPackingGuideReportData(MyUtility.Convert.GetString(masterData["ID"]), out printData, out ctnDim, out qtyCtn, out articleSizeTtlShipQty, out printGroupData, out clipData, out specialInstruction);
                return result;
            }
            else
            {
                DualResult result = PublicPrg.Prgs.PackingBarcodePrint(MyUtility.Convert.GetString(masterData["ID"]), ctn1, ctn2, out printData);
                if (!result)
                {
                    return result;
                }
                
                e.Report.ReportDataSource = printData;

            }
            return Result.True;
        }

        // 產生Excel
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            this.ShowWaitMessage("Data Loading....");
            if (reportType == "1" || reportType == "2")
            {
                PublicPrg.Prgs.PackingListToExcel_PackingListReport("\\Packing_P03_PackingListReport.xltx", masterData, reportType, printData, ctnDim, qtyBDown);
            }
            else if (reportType == "3")
            {
                PublicPrg.Prgs.PackingListToExcel_PackingGuideReport("\\Packing_P03_PackingGuideReport.xltx", printData, ctnDim, qtyCtn, articleSizeTtlShipQty, printGroupData, clipData, masterData, orderQty, specialInstruction);

            }
            this.HideWaitMessage();
            return true;
        }
    }
}
