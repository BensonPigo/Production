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
using System.Runtime.InteropServices;
using System.IO;
using System.Drawing.Imaging;
using System.Linq;

namespace Sci.Production.Packing
{
    /// <summary>
    /// Pcking_P03_Print
    /// </summary>
    public partial class P03_Print : Sci.Win.Tems.PrintForm
    {
        private DataRow masterData;
        private int orderQty;
        private bool SP_Multiple = false;
        private string reportType;
        private string ctn1;
        private string ctn2;
        private string specialInstruction;
        private string country;
        private DataTable printData;
        private DataTable ctnDim;
        private DataTable qtyCtn;
        private DataTable articleSizeTtlShipQty;
        private DataTable printGroupData;
        private DataTable clipData;
        private DataTable qtyBDown;

        /// <summary>
        /// OrderQty
        /// </summary>
        public int OrderQty
        {
            get
            {
                return this.orderQty;
            }

            set
            {
                this.orderQty = value;
            }
        }

        /// <summary>
        /// P03_Print
        /// </summary>
        /// <param name="masterData">masterData</param>
        /// <param name="orderQty">OrderQty</param>
        public P03_Print(DataRow masterData, int orderQty)
        {
            this.InitializeComponent();

            // 如果是多訂單一起裝箱就不列印
            this.SP_Multiple = MyUtility.Convert.GetInt(MyUtility.GetValue.Lookup(string.Format("select COUNT(distinct OrderID+OrderShipmodeSeq) from PackingList_Detail WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(masterData["ID"])))) > 1;
            if (this.SP_Multiple)
            {
                this.radioPackingGuideReport.Visible = false;
            }

            this.masterData = masterData;
            this.OrderQty = orderQty;
            this.radioPackingListReportFormA.Checked = true;
            this.ControlPrintFunction(false);
        }

        private void RadioBarcodePrint_CheckedChanged(object sender, EventArgs e)
        {
            this.ControlPrintFunction(((Sci.Win.UI.RadioButton)sender).Checked);
            this.txtcountry1.Enabled = this.radioNewBarcodePrint.Checked;
            this.txtcountry1.TextBox1.Text = string.Empty;
        }

        // 控制元件是否可使用
        private void ControlPrintFunction(bool isSupport)
        {
            this.IsSupportToPrint = isSupport;
            this.IsSupportToExcel = !isSupport;
            this.txtCTNStart.Enabled = isSupport;
            this.txtCTNEnd.Enabled = isSupport;
            if (!isSupport)
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
            this.reportType = this.radioPackingListReportFormA.Checked ? "1" :
                this.radioPackingListReportFormB.Checked ? "2" : this.radioPackingGuideReport.Checked ? "3" : "4";
            this.ctn1 = this.txtCTNStart.Text;
            this.ctn2 = this.txtCTNEnd.Text;
            this.ReportResourceName = "P03_BarcodePrint.rdlc";
            this.country = this.txtcountry1.DisplayBox1.Text;
            return base.ValidateInput();
        }

        /// <summary>
        /// OnAsyncDataLoad非同步取資料
        /// </summary>
        /// <param name="e">e</param>
        /// <returns>DualResult</returns>
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            if (this.reportType == "1" || this.reportType == "2")
            {
                DualResult result = PublicPrg.Prgs.QueryPackingListReportData(MyUtility.Convert.GetString(this.masterData["ID"]), this.reportType, out this.printData, out this.ctnDim, out this.qtyBDown);
                return result;
            }
            else if (this.reportType == "3")
            {
                DualResult result = PublicPrg.Prgs.QueryPackingGuideReportData(MyUtility.Convert.GetString(this.masterData["ID"]), out this.printData, out this.ctnDim, out this.qtyCtn, out this.articleSizeTtlShipQty, out this.printGroupData, out this.clipData, out this.specialInstruction);
                return result;
            }

            return Result.True;
        }

        /// <summary>
        /// ToPrint
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ToPrint()
        {
            this.ValidateInput();

            this.ShowWaitMessage("Data Loading ...");
            DualResult result;
            if (this.radioNewBarcodePrint.Checked)
            {
                result = new PackingPrintBarcode().PrintBarcode(this.masterData["ID"].ToString(), this.ctn1, this.ctn2, "New", this.country);
            }
            else
            {
                result = new PackingPrintBarcode().PrintBarcode(this.masterData["ID"].ToString(), this.ctn1, this.ctn2);
            }

            if (result == false)
            {
                MyUtility.Msg.WarningBox(result.ToString());
                return false;
            }

            this.HideWaitMessage();
            return true;
        }

        /// <summary>
        /// OnToExcel產生Excel
        /// </summary>
        /// <param name="report">report</param>
        /// <returns>bool</returns>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            this.ShowWaitMessage("Data Loading....");
            if (this.reportType == "1" || this.reportType == "2")
            {
                if (this.SP_Multiple)
                {
                    PublicPrg.Prgs.PackingListToExcel_PackingListReport("\\Packing_P03_PackingListReport_Multiple.xltx", this.masterData, this.reportType, this.printData, this.ctnDim, this.qtyBDown);
                }
                else
                {
                    PublicPrg.Prgs.PackingListToExcel_PackingListReport("\\Packing_P03_PackingListReport.xltx", this.masterData, this.reportType, this.printData, this.ctnDim, this.qtyBDown);
                }
            }
            else if (this.reportType == "3")
            {
                PublicPrg.Prgs.PackingListToExcel_PackingGuideReport("\\Packing_P03_PackingGuideReport.xltx", this.printData, this.ctnDim, this.qtyCtn, this.articleSizeTtlShipQty, this.printGroupData, this.clipData, this.masterData, this.OrderQty, this.specialInstruction);
            }

            this.HideWaitMessage();
            return true;
        }
    }
}
