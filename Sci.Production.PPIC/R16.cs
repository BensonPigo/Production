using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using System.Runtime.InteropServices;
using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;

namespace Sci.Production.PPIC
{
    /// <inheritdoc/>
    public partial class R16 : Win.Tems.PrintForm
    {
        private DataTable printData;
        private string BrandID;
        private string MDivisionID;
        private string FactoryID;
        private bool IsOutstanding;
        private bool IsExcludeSister;
        private bool IsExcludeCancelShortage;
        private bool IsBookingOrder;
        private DateTime? BuyerDev_S;
        private DateTime? BuyerDev_E;

        /// <summary>
        /// Initializes a new instance of the <see cref="R16"/> class.
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public R16(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.txtMdivision.Enabled = false;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.txtMdivision.Text = Env.User.Keyword;
            this.txtfactory.Text = Env.User.Factory;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.BuyerDev_S = null;
            this.BuyerDev_E = null;

            if (!this.dateRangeByerDev.Value1.HasValue || !this.dateRangeByerDev.Value2.HasValue)
            {
                MyUtility.Msg.InfoBox("Buyer Delivery Date can not be empty.");
                return false;
            }

            if (this.dateRangeByerDev.Value1.HasValue)
            {
                this.BuyerDev_S = this.dateRangeByerDev.Value1.Value;
            }

            if (this.dateRangeByerDev.Value2.HasValue)
            {
                this.BuyerDev_E = this.dateRangeByerDev.Value2.Value;
            }

            this.MDivisionID = this.txtMdivision.Text;
            this.BrandID = this.txtbrand.Text;
            this.FactoryID = this.txtfactory.Text;
            this.IsOutstanding = this.chkOutstanding.Checked;
            this.IsExcludeSister = this.chkExcludeSis.Checked;
            this.IsExcludeCancelShortage = this.chkExcludeCancelShortage.Checked;
            this.IsBookingOrder = this.chkBookingOrder.Checked;

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            PPIC_R16_ViewModel model = new PPIC_R16_ViewModel()
            {
                BuyerDeliveryFrom = this.BuyerDev_S,
                BuyerDeliveryTo = this.BuyerDev_E,
                BrandID = this.BrandID,
                MDivisionID = this.MDivisionID,
                FactoryID = this.FactoryID,
                IsOutstanding = this.chkOutstanding.Checked,
                IsExcludeSister = this.IsExcludeSister,
                IsExcludeCancelShortage = this.IsExcludeCancelShortage,
                IsBookingOrder = this.IsBookingOrder,
                IsJunk = this.IsExcludeCancelShortage,
            };
            PPIC_R16 biModel = new PPIC_R16();
            Base_ViewModel resultReport = biModel.GetOustandingPO(model);

            if (!resultReport.Result)
            {
                return resultReport.Result;
            }

            resultReport.Dt.Columns.Remove("LastCartonReceivedDate");
            this.printData = resultReport.Dt;
            return resultReport.Result;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            this.SetCount(this.printData.Rows.Count);

            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\PPIC_R16.xltx"); // 預先開啟excel app
            MyUtility.Excel.CopyToXls(this.printData, string.Empty, "PPIC_R16.xltx", 1, false, null, objApp); // 將datatable copy to excel

            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
            objSheets.get_Range("J:J").ColumnWidth = 8;
            objSheets.get_Range("N:N").ColumnWidth = 8;
            objSheets.get_Range("O:O").ColumnWidth = 9;
            objSheets.get_Range("P:T").ColumnWidth = 8;
            objSheets.get_Range("V:AC").ColumnWidth = 8;
            objSheets.get_Range("G:G").ColumnWidth = 10;
            objSheets.get_Range("W:W").ColumnWidth = 10;
            objSheets.get_Range("U:U").ColumnWidth = 10;

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("PPIC_R16");
            objApp.ActiveWorkbook.SaveAs(strExcelName);
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);
            Marshal.ReleaseComObject(objSheets);

            strExcelName.OpenFile();
            #endregion

            return true;
        }
    }
}
