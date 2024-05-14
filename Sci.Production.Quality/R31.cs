using Ict;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Sci.Production.Prg.PowerBI.Model;
using Sci.Production.Prg.PowerBI.Logic;

namespace Sci.Production.Quality
{
    /// <inheritdoc/>
    public partial class R31 : Sci.Win.Tems.PrintForm
    {
        private DataTable printData;

        private DateTime? Buyerdelivery1;
        private DateTime? Buyerdelivery2;
        private string sp1;
        private string sp2;
        private string MDivisionID;
        private string FactoryID;
        private string Brand;
        private string Stage;
        private bool exSis;
        private bool Outstanding;
        private List<string> categoryList = new List<string>();

        private DataTable dtQAR31_Outstanding;

        /// <inheritdoc/>
        public R31(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.comboM.SetDefalutIndex(true);
            this.comboFactory.SetDataSource();
            this.comboStage.Enabled = false;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.categoryList.Clear();
            this.sp1 = this.txtSP_s.Text;
            this.sp2 = this.txtSP_e.Text;
            this.MDivisionID = this.comboM.Text;
            this.FactoryID = this.comboFactory.Text;
            this.Brand = this.txtBrand.Text;
            this.Buyerdelivery1 = this.dateBuyerDev.Value1;
            this.Buyerdelivery2 = this.dateBuyerDev.Value2;
            this.exSis = this.chkExSis.Checked;

            if (this.chkBulk.Checked)
            {
                this.categoryList.Add("B");
            }

            if (this.chkSample.Checked)
            {
                this.categoryList.Add("S");
            }

            if (this.chkGarment.Checked)
            {
                this.categoryList.Add("G");
            }

            this.Outstanding = this.chkOutstanding.Checked;
            this.Stage = this.comboStage.Text;

            if (MyUtility.Check.Empty(this.Buyerdelivery1) &&
                    MyUtility.Check.Empty(this.Buyerdelivery2) &&
                    MyUtility.Check.Empty(this.sp1) &&
                    MyUtility.Check.Empty(this.sp2))
            {
                MyUtility.Msg.InfoBox("Buyer Delivery and SP# can't be all empty.");

                return false;
            }

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            List<SqlParameter> paramList = new List<SqlParameter>();

            if (this.Outstanding)
            {
                QA_R31 biModel = new QA_R31();

                #region 與BI共用Data Logic
                QA_R31_ViewModel qa_R31 = new QA_R31_ViewModel()
                {
                    BuyerDelivery1 = this.Buyerdelivery1,
                    BuyerDelivery2 = this.Buyerdelivery2,
                    SP1 = this.sp1,
                    SP2 = this.sp2,
                    MDivisionID = this.MDivisionID,
                    FactoryID = this.FactoryID,
                    Brand = this.Brand,
                    Category = this.categoryList.JoinToString("','"),
                    Exclude_Sister_Transfer_Out = this.exSis,
                    Outstanding = this.Outstanding,
                    InspStaged = this.Stage,
                };

                Base_ViewModel resultReport = biModel.GetQA_R31Data(qa_R31);
                if (!resultReport.Result)
                {
                    return resultReport.Result;
                }

                this.printData = resultReport.DtArr[0];

                #endregion
            }
            else
            {
                QA_R31_ViewModel par = new QA_R31_ViewModel()
                {
                    CategoryList = this.categoryList,
                    SP1 = this.sp1,
                    SP2 = this.sp2,
                    MDivisionID = this.MDivisionID,
                    FactoryID = this.FactoryID,
                    Brand = this.Brand,
                    BuyerDelivery1 = this.Buyerdelivery1,
                    BuyerDelivery2 = this.Buyerdelivery2,
                    Exclude_Sister_Transfer_Out = this.exSis,
                    Outstanding = this.Outstanding,
                    InspStaged = this.Stage,
                };

                Base_ViewModel base_ViewModel = new QA_R31().GetCFAMasterListReport(par);

                if (!base_ViewModel.Result)
                {
                    return base_ViewModel.Result;
                }

                this.printData = base_ViewModel.Dt;
            }

            return Result.True;
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

            // Outstanding需要使用不同範本
            string template = !this.Outstanding ? "Quality_R31.xltx" : "Quality_R31_Outstanding.xltx";

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + $"\\{template}"); // 預先開啟excel app
            MyUtility.Excel.CopyToXls(this.printData, string.Empty, template, 1, false, null, objApp); // 將datatable copy to excel
            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表

            objSheets.Columns[3].ColumnWidth = 12;
            objSheets.Columns[4].ColumnWidth = 12;
            objSheets.Columns[5].ColumnWidth = 12;
            objSheets.Columns[6].ColumnWidth = 12;

            // 客製化欄位，記得設定this.IsSupportCopy = true
            // this.CreateCustomizedExcel(ref objSheets);
            #region Save & Show Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Quality_R31");
            objApp.ActiveWorkbook.SaveAs(strExcelName);
            objApp.Quit();
            Marshal.ReleaseComObject(objSheets);
            Marshal.ReleaseComObject(objApp);

            strExcelName.OpenFile();
            #endregion
            return true;
        }

        private void ChkOutstanding_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkOutstanding.Checked)
            {
                this.comboStage.Enabled = true;
            }
            else
            {
                this.comboStage.Text = string.Empty;
                this.comboStage.Enabled = false;
            }
        }
    }
}
