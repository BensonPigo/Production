using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Packing
{
    /// <summary>
    /// Packing_Packing
    /// </summary>
    public partial class R02 : Sci.Win.Tems.PrintForm
    {
        /// <summary>
        /// R02
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R02(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();

            DataTable factory;
            DBProxy.Current.Select(null, "select '' union all select distinct FtyGroup from Factory WITH (NOLOCK) ", out factory);
            MyUtility.Tool.SetupCombox(this.comboFactory, 1, factory);
            this.comboFactory.Text = Sci.Env.User.Factory;
        }

        private string _sp1;
        private string _sp2;
        private string _po1;
        private string _po2;
        private string _bdate1;
        private string _bdate2;
        private string _scidate1;
        private string _scidate2;
        private string _offdate1;
        private string _offdate2;
        private string _brand;
        private string _mDivision;
        private string _factory;
        private string _POCompletion;
        private bool _bulk;
        private bool _sample;
        private bool _garment;
        private DataTable[] _printData;

        /// <summary>
        /// ValidateInput
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ValidateInput()
        {
            this._sp1 = this.txtSPNoStart.Text;
            this._sp2 = this.txtSPNoEnd.Text;
            this._po1 = this.txtPONoStart.Text;
            this._po2 = this.txtPONoEnd.Text;

            if (!MyUtility.Check.Empty(this.dateBuyerDelivery.Value1))
            {
                this._bdate1 = Convert.ToDateTime(this.dateBuyerDelivery.Value1).ToString("d");
            }
            else
            {
                this._bdate1 = null;
            }

            if (!MyUtility.Check.Empty(this.dateBuyerDelivery.Value2))
            {
                this._bdate2 = Convert.ToDateTime(this.dateBuyerDelivery.Value2).ToString("d");
            }
            else
            {
                this._bdate2 = null;
            }

            if (!MyUtility.Check.Empty(this.dateSCIDelivery.Value1))
            {
                this._scidate1 = Convert.ToDateTime(this.dateSCIDelivery.Value1).ToString("d");
            }
            else
            {
                this._scidate1 = null;
            }

            if (!MyUtility.Check.Empty(this.dateSCIDelivery.Value2))
            {
                this._scidate2 = Convert.ToDateTime(this.dateSCIDelivery.Value2).ToString("d");
            }
            else
            {
                this._scidate2 = null;
            }

            if (!MyUtility.Check.Empty(this.dateOffline.Value1))
            {
                this._offdate1 = Convert.ToDateTime(this.dateOffline.Value1).ToString("d");
            }
            else
            {
                this._offdate1 = null;
            }

            if (!MyUtility.Check.Empty(this.dateOffline.Value2))
            {
                this._offdate2 = Convert.ToDateTime(this.dateOffline.Value2).ToString("d");
            }
            else
            {
                this._offdate2 = null;
            }

            this._brand = this.txtbrand.Text;
            this._mDivision = this.txtMdivision1.Text;
            this._factory = this.comboFactory.Text;
            this._POCompletion = this.cmbPOcompletion.Text;
            this._bulk = this.chkBulk.Checked;
            this._sample = this.chkSample.Checked;
            this._garment = this.chkGarment.Checked;

            return base.ValidateInput();
        }

        /// <summary>
        /// OnAsyncDataLoad
        /// </summary>
        /// <param name="e">e</param>
        /// <returns>DualResult</returns>
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            string sqlcmd = $@"


";

            #region Get Data
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out this._printData);
            if (!result)
            {
                return result;
            }
            #endregion

            return Result.True;
        }

        /// <summary>
        /// OnToExcel
        /// </summary>
        /// <param name="report">report</param>
        /// <returns>bool</returns>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            #region check printData
            if (this._printData.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found.");
                return false;
            }
            #endregion

            this.SetCount(this._printData.Rows.Count);
            this.ShowWaitMessage("Excel Processing");

            #region To Excel
            string reportname = "Packing_R02.xltx";
            Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\" + reportname);
            Excel.Worksheet worksheet = objApp.Sheets[1];
            MyUtility.Excel.CopyToXls(this._printData, string.Empty, reportname, 3, showExcel: false, excelApp: objApp);
            worksheet.Columns.AutoFit();

            #region Save & Show Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Packing_R02");
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
