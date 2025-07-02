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

namespace Sci.Production.Packing
{
    /// <summary>
    /// Packing_Packing
    /// </summary>
    public partial class R01 : Win.Tems.PrintForm
    {
        /// <summary>
        /// R01
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();

            DataTable factory;
            DBProxy.Current.Select(null, "select '' union all select distinct FtyGroup from Factory WITH (NOLOCK) ", out factory);
            MyUtility.Tool.SetupCombox(this.comboFactory, 1, factory);
            this.comboFactory.Text = Env.User.Factory;
            this.txtMdivision1.Text = Env.User.Keyword;

            this.dateScan1.CustomFormat = "yyyy/MM/dd HH:mm";
            this.dateScan2.CustomFormat = "yyyy/MM/dd HH:mm";
            this.dateScan1.Text = DateTime.Now.ToString("yyyy/MM/dd 08:00");
            this.dateScan2.Text = DateTime.Now.ToString("yyyy/MM/dd 12:00");
        }

        // 驗證輸入條件
        private string _sp1;

        // 驗證輸入條件
        private string _sp2;

        // 驗證輸入條件
        private string _packingno1;

        // 驗證輸入條件
        private string _packingno2;

        // 驗證輸入條件
        private string _po1;

        // 驗證輸入條件
        private string _po2;

        // 驗證輸入條件
        private string _brand;

        // 驗證輸入條件
        private string _mDivision;

        // 驗證輸入條件
        private string _factory;

        // 驗證輸入條件
        private string _bdate1;

        // 驗證輸入條件
        private string _bdate2;

        // 驗證輸入條件
        private string _scandate1;
        private string _scandate1e;

        // 驗證輸入條件
        private string _scandate2;
        private string _scandate2e;

        // 驗證輸入條件
        private string _ScanName;
        private string _Barcode;

        /// <summary>
        /// ValidateInput
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ValidateInput()
        {
            this._sp1 = this.txtSPNoStart.Text;
            this._sp2 = this.txtSPNoEnd.Text;
            this._packingno1 = this.txtPackingStart.Text;
            this._packingno2 = this.txtPackingEnd.Text;
            if (!MyUtility.Check.Empty(this.dateBuyerDelivery.Value1))
            {
                this._bdate1 = Convert.ToDateTime(this.dateBuyerDelivery.Value1).ToString("yyyy/MM/dd");
            }
            else
            {
                this._bdate1 = null;
            }

            if (!MyUtility.Check.Empty(this.dateBuyerDelivery.Value2))
            {
                this._bdate2 = Convert.ToDateTime(this.dateBuyerDelivery.Value2).ToString("yyyy/MM/dd");
            }
            else
            {
                this._bdate2 = null;
            }

            if (!MyUtility.Check.Empty(this.dateScan1.Value))
            {
                this._scandate1 = Convert.ToDateTime(this.dateScan1.Value).ToString("yyyy/MM/dd HH:mm:ss");
                this._scandate1e = Convert.ToDateTime(this.dateScan1.Value).ToString("yyyy/MM/dd HH:mm");
            }
            else
            {
                this._scandate1 = null;
                this._scandate1e = null;
            }

            if (!MyUtility.Check.Empty(this.dateScan2.Value))
            {
                this._scandate2 = Convert.ToDateTime(this.dateScan2.Value).AddMinutes(1).ToString("yyyy/MM/dd HH:mm:ss");
                this._scandate2e = Convert.ToDateTime(this.dateScan2.Value).ToString("yyyy/MM/dd HH:mm");
            }
            else
            {
                this._scandate2 = null;
                this._scandate2e = null;
            }

            this._po1 = this.txtPONoStart.Text;
            this.Po2 = this.txtPONoEnd.Text;
            this._brand = this.txtbrand.Text;
            this._mDivision = this.txtMdivision1.Text;
            this._factory = this.comboFactory.Text;
            this._ScanName = this.txtuser1.TextBox1.Text;
            this._Barcode = this.txtBarcode.Text;

            return base.ValidateInput();
        }

        // 非同步取資料
        private DataTable _printData;
        private string _columnname;

        /// <summary>
        /// Po2
        /// </summary>
        public string Po2
        {
            get
            {
                return this._po2;
            }

            set
            {
                this._po2 = value;
            }
        }

        /// <summary>
        /// OnAsyncDataLoad
        /// </summary>
        /// <param name="e">e</param>
        /// <returns>DualResult</returns>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            Packing_R01 biModel = new Packing_R01();
            Packing_R01_ViewModel model = new Packing_R01_ViewModel()
            {
                SP1 = this._sp1,
                SP2 = this._sp2,
                PackingID1 = this._packingno1,
                PackingID2 = this._packingno2,
                BuyerDelivery1 = this._bdate1,
                BuyerDelivery2 = this._bdate2,
                ScanEditDate1 = this._scandate1,
                ScanEditDate2 = this._scandate2,
                PO1 = this._po1,
                PO2 = this._po2,
                Brand = this._brand,
                MDivisionID = this._mDivision,
                FactoryID = this._factory,
                ScanName = this._ScanName,
                IsSummary = this.rdbtnSummary.Checked ? true : false,
                IsDetail = this.rdbtnDetail.Checked ? true : false,
                Barcode = this._Barcode,
                IsPowerBI = false,
            };

            Base_ViewModel resultReport = biModel.GetPacking_R01Data(model);
            if (!resultReport.Result)
            {
                return resultReport.Result;
            }

            this._printData = resultReport.Dt;

            resultReport = biModel.GetPacking_R01CustomizeData(model);
            if (!resultReport.Result)
            {
                return resultReport.Result;
            }

            this._columnname = resultReport.Dt.Rows[0]["Customize1"].ToString();
            return resultReport.Result;
        }

        /// <summary>
        /// OnToExcel
        /// </summary>
        /// <param name="report">report</param>
        /// <returns>bool</returns>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            #region check printData
            if (this._printData == null || this._printData.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found.");
                return false;
            }
            #endregion

            this.SetCount(this._printData.Rows.Count);
            this.ShowWaitMessage("Excel Processing");

            #region To Excel
            string reportname = "Packing_R01.xltx";
            Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\" + reportname);
            Excel.Worksheet worksheet = objApp.Sheets[1];
            worksheet.Cells[2, 2] = this._sp1 + "~" + this._sp2;
            worksheet.Cells[2, 5] = this._packingno1 + "~" + this._packingno2;
            worksheet.Cells[2, 8] = this._bdate1 + "~" + this._bdate2;
            worksheet.Cells[2, 11] = this._scandate1e + "~" + this._scandate2e;
            worksheet.Cells[2, 16] = this._po1 + "~" + this.Po2;
            worksheet.Cells[2, 18] = this._brand;
            worksheet.Cells[2, 21] = this._factory;
            worksheet.Cells[2, 24] = this.rdbtnDetail.Checked ? "Complete" : (this.rdbtnSummary.Checked ? "Not Complete" : "ALL");
            worksheet.Cells[3, 9] = this._columnname;
            MyUtility.Excel.CopyToXls(this._printData, string.Empty, reportname, 3, showExcel: false, excelApp: objApp);
            worksheet.Columns.AutoFit();

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("Packing_R01");
            Excel.Workbook workbook = objApp.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);
            Marshal.ReleaseComObject(worksheet);
            Marshal.ReleaseComObject(workbook);

            strExcelName.OpenFile();
            #endregion
            #endregion
            this.HideWaitMessage();
            return true;
        }
    }
}
