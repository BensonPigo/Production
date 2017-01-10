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
    public partial class P06_Print : Sci.Win.Tems.PrintForm
    {
        string reportType, ctn1,ctn2,specialInstruction;
        DataTable printData, ctnDim, qtyCtn, articleSizeTtlShipQty, printGroupData,clipData;
        int orderQty;
        DataRow masterData;
        public P06_Print(DataRow MasterData, int OrderQty)
        {
            InitializeComponent();
            masterData = MasterData;
            orderQty = OrderQty;
            radioButton1.Checked = true;
            ControlPrintFunction(true);
        }

        //Packing Guide Report
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            ControlPrintFunction(radioButton1.Checked);
        }

        //控制元件是否可使用
        private void ControlPrintFunction(bool isSupport)
        {
            this.IsSupportToPrint = !isSupport;
            this.IsSupportToExcel = isSupport;
            textBox1.Enabled = !isSupport;
            textBox2.Enabled = !isSupport;
            if (isSupport)
            {
                textBox1.Text = "";
                textBox2.Text = "";
            }
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            reportType = radioButton1.Checked ? "1" : "2";
            ctn1 = textBox1.Text;
            ctn2 = textBox2.Text;
            ReportResourceName = "BarcodePrint.rdlc";

            return base.ValidateInput();
        }

        // 非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            if (reportType == "1")
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
            if (reportType == "1")
            {
                PublicPrg.Prgs.PackingListToExcel_PackingGuideReport("\\Packing_P03_PackingGuideReport.xltx", printData, ctnDim, qtyCtn, articleSizeTtlShipQty, printGroupData, clipData, masterData, orderQty, specialInstruction);
            }
            this.HideWaitMessage();
            return true;
        }
    }
}
