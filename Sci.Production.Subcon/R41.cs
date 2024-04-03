using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Sci.Production.Prg.PowerBI.Model;
using Sci.Production.Prg.PowerBI.Logic;
using Microsoft.Office.Interop.Excel;
using static Sci.MyUtility;
using static Ict.Win.WinAPI;
using System.Diagnostics;
using System.Linq;

namespace Sci.Production.Subcon
{
    /// <inheritdoc/>
    public partial class R41 : Win.Tems.PrintForm
    {
        private string SubProcess;
        private string SP;
        private string M;
        private string Factory;
        private string CutRef1;
        private string CutRef2;
        private string processLocation;
        private DateTime? dateBundle1;
        private DateTime? dateBundle2;
        private DateTime? dateBundleScanDate1;
        private DateTime? dateBundleScanDate2;
        private DateTime? dateEstCutDate1;
        private DateTime? dateEstCutDate2;
        private DateTime? dateBDelivery1;
        private DateTime? dateBDelivery2;
        private DateTime? dateSewInLine1;
        private DateTime? dateSewInLine2;
        private DateTime? dateLastSewDate1;
        private DateTime? dateLastSewDate2;
        private System.Data.DataTable printData;

        /// <inheritdoc/>
        public R41(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.Comboload();
            this.comboFactory.SetDataSource();
            this.comboRFIDProcessLocation.SetDataSource();
            this.comboRFIDProcessLocation.SelectedIndex = 0;
        }

        private void Comboload()
        {
            DualResult result;
            if (result = DBProxy.Current.Select(null, "select '' as id union select MDivisionID from factory WITH (NOLOCK) ", out System.Data.DataTable dtM))
            {
                this.comboM.DataSource = dtM;
                this.comboM.DisplayMember = "ID";
            }
            else
            {
                this.ShowErr(result);
            }
        }

        #region ToExcel3步驟

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.SubProcess = this.txtsubprocess.Text;
            this.SP = this.txtSPNo.Text;
            this.M = this.comboM.Text;
            this.Factory = this.comboFactory.Text;
            this.CutRef1 = this.txtCutRefStart.Text;
            this.CutRef2 = this.txtCutRefEnd.Text;
            this.dateBundle1 = this.dateBundleCDate.Value1;
            this.dateBundle2 = this.dateBundleCDate.Value2;
            this.dateBundleScanDate1 = this.dateBundleScanDate.Value1;
            this.dateBundleScanDate2 = this.dateBundleScanDate.Value2;
            this.dateEstCutDate1 = this.dateEstCutDate.Value1;
            this.dateEstCutDate2 = this.dateEstCutDate.Value2;
            this.dateBDelivery1 = this.dateBDelivery.Value1;
            this.dateBDelivery2 = this.dateBDelivery.Value2;
            this.dateSewInLine1 = this.dateSewInLine.Value1;
            this.dateSewInLine2 = this.dateSewInLine.Value2;
            this.dateLastSewDate1 = this.dateLastSewDate.Value1;
            this.dateLastSewDate2 = this.dateLastSewDate.Value2;

            this.processLocation = this.comboRFIDProcessLocation.Text;
            if (MyUtility.Check.Empty(this.CutRef1) && MyUtility.Check.Empty(this.CutRef2) &&
                MyUtility.Check.Empty(this.SP) &&
                MyUtility.Check.Empty(this.dateEstCutDate.Value1) && MyUtility.Check.Empty(this.dateEstCutDate.Value2) &&
                MyUtility.Check.Empty(this.dateBundleCDate.Value1) && MyUtility.Check.Empty(this.dateBundleCDate.Value2) &&
                MyUtility.Check.Empty(this.dateBundleScanDate.Value1) && MyUtility.Check.Empty(this.dateBundleScanDate.Value2) &&
                MyUtility.Check.Empty(this.dateSewInLine.Value1) && MyUtility.Check.Empty(this.dateSewInLine.Value2) &&
                !this.dateLastSewDate.HasValue)
            {
                MyUtility.Msg.WarningBox("[Cut Ref#][SP#][Est. Cutting Date][Bundle CDate][Bundle Scan Date],[Sewing Inline],[Last Sew. Date] cannot all empty !!");
                return false;
            }

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            List<SqlParameter> paramList = new List<SqlParameter>();
            SubCon_R41 biModel = new SubCon_R41();
            SubCon_R41_ViewModel parameter = new SubCon_R41_ViewModel()
            {
                CutRefNo1 = this.CutRef1,
                CutRefNo2 = this.CutRef2,
                SP1 = this.SP,
                EstCuttingDate1 = this.dateEstCutDate1,
                EstCuttingDate2 = this.dateEstCutDate2,
                BundleCDate1 = this.dateBundle1,
                BundleCDate2 = this.dateBundle2,
                BundleScanDate1 = this.dateBundleScanDate1,
                BundleScanDate2 = this.dateBundleScanDate2,
                SewingInlineDate1 = this.dateSewInLine1,
                SewingInlineDate2 = this.dateSewInLine2,
                LastSewDate1 = this.dateLastSewDate1,
                LastSewDate2 = this.dateLastSewDate2,
                BuyerDelDate1 = this.dateBDelivery1,
                BuyerDelDate2 = this.dateBDelivery2,
                SubProcessList = this.SubProcess,
                MDivisionID = this.M,
                FactoryID = this.Factory,
                ProcessLocation = this.processLocation,
                IsPowerBI = false,
            };

            Base_ViewModel resultReport = biModel.GetSubprocessWIPData(parameter);
            if (!resultReport.Result)
            {
                return resultReport.Result;
            }

            this.printData = resultReport.DtArr[0];

            return Ict.Result.True;
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

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Subcon_R41_Bundle tracking list (RFID).xltx"); // 預先開啟excel app

#if DEBUG
            //objApp.Visible = true;
#endif

            Worksheet worksheet1 = (Worksheet)objApp.ActiveWorkbook.Worksheets[1];
            Worksheet worksheetn = (Worksheet)objApp.ActiveWorkbook.Worksheets[2];
            worksheet1.Copy(worksheetn);

            int sheet = 1;

            // 因為一次載入太多筆資料到DataTable 會造成程式佔用大量記憶體，改為每1萬筆載入一次並貼在excel上
            #region 分段抓取資料填入excel
            this.ShowLoadingText($"Data Loading , please wait …");
            int batchSize = 100000; // 一次十萬筆資料
            int totalRows = this.printData.Rows.Count;
            int batches = (int)System.Math.Ceiling((double)totalRows / batchSize);

            // 複製資料到 Excel
            int startCopy = 1;
            int eachCopy = 100000;
            int loadCount1 = 0;

            // 10萬筆以內, 直接匯出Excel
            if (batches <= 1)
            {
                MyUtility.Excel.CopyToXls(this.printData, string.Empty, "Subcon_R41_Bundle tracking list (RFID).xltx", totalRows - (totalRows % batchSize) + 1, false, null, objApp, wSheet: objApp.Sheets[sheet]);
            }
            else
            {
                // 每10萬筆跑一次
                for (int i = 0; i < batches; i++)
                {
                    this.ShowLoadingText($"Data Loading – {startCopy * eachCopy} , please wait …");

                    System.Data.DataTable dtResult = this.printData.AsEnumerable().Skip(loadCount1).Take(eachCopy).CopyToDataTable();
                    MyUtility.Excel.CopyToXls(dtResult, string.Empty, "Subcon_R41_Bundle tracking list (RFID).xltx", 1 + loadCount1, false, null, objApp, wSheet: objApp.Sheets[sheet]); // 將datatable copy to excel
                    loadCount1 += eachCopy;
                    startCopy++;

                    // 勿動!! 超過100萬這個數字，DY的電腦會跑不動
                    // 每100萬資料超過sheet最大限制,就要換Sheet
                    if ((i + 1) % 10 == 0)
                    {
                        Worksheet worksheetA = (Worksheet)objApp.ActiveWorkbook.Worksheets[sheet + 1];
                        Worksheet worksheetB = (Worksheet)objApp.ActiveWorkbook.Worksheets[sheet + 2];
                        worksheetA.Copy(worksheetB);
                        sheet++;
                    }
                }
            }

            // this.SetCount((long)loadCounts);
            objApp.DisplayAlerts = false;
            ((Worksheet)objApp.Sheets[sheet + 1]).Delete();
            ((Worksheet)objApp.Sheets[1]).Select();
            objApp.DisplayAlerts = true;
            this.HideLoadingText();
            #endregion

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("Subcon_R41_Bundle tracking list (RFID)");
            Workbook workbook = objApp.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);          // 釋放objApp
            Marshal.ReleaseComObject(workbook);

            // printData.Clear();
            // printData.Dispose();
            strExcelName.OpenFile();
            #endregion
            return true;
        }

        #endregion
    }
}
