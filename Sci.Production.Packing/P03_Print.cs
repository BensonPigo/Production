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
            radioButton1.Checked = true;
            ControlPrintFunction(false);
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            ControlPrintFunction(radioButton4.Checked);
        }

        //控制元件是否可使用
        private void ControlPrintFunction(bool isSupport)
        {
            this.IsSupportToPrint = isSupport;
            this.IsSupportToExcel = !isSupport;
            textBox1.Enabled = isSupport;
            textBox2.Enabled = isSupport;
            if (!isSupport)
            {
                textBox1.Text = "";
                textBox2.Text = "";
            }
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            reportType = radioButton1.Checked ? "1" : radioButton2.Checked ? "2" : radioButton3.Checked ? "3" : "4";
            ctn1 = textBox1.Text;
            ctn2 = textBox2.Text;
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
            if (reportType == "1" || reportType == "2")
            {
                PublicPrg.Prgs.PackingListToExcel_PackingListReport("\\Packing_P03_PackingListReport.xltx", masterData, reportType, printData, ctnDim, qtyBDown);
            }
            else if (reportType == "3")
            {
                PublicPrg.Prgs.PackingListToExcel_PackingGuideReport("\\Packing_P03_PackingGuideReport.xltx", printData, ctnDim, qtyCtn, articleSizeTtlShipQty, printGroupData, clipData, masterData, orderQty, specialInstruction);

            }

            return true;
        }
    }
}
