using System;
using System.Data;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;
using Ict;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// P05_Print
    /// </summary>
    public partial class P05_Print : Win.Tems.PrintForm
    {
        private DataTable gridData;
        private string readyDate1;
        private string readyDate2;
        private string strUp2SCIDelivery;
        private DataRow[] printData;

        /// <summary>
        /// P05_Print
        /// </summary>
        /// <param name="gridData">DataTable gridData</param>
        /// <param name="strUp2SCIDelivery">string strUp2SCIDelivery</param>
        public P05_Print(DataTable gridData, string strUp2SCIDelivery)
        {
            this.InitializeComponent();
            this.gridData = gridData;
            this.strUp2SCIDelivery = strUp2SCIDelivery;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.readyDate1 = MyUtility.Check.Empty(this.dateReadydate.Value1) ? string.Empty : Convert.ToDateTime(this.dateReadydate.Value1).ToString("d");
            this.readyDate2 = MyUtility.Check.Empty(this.dateReadydate.Value2) ? string.Empty : Convert.ToDateTime(this.dateReadydate.Value2).ToString("d");

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            this.printData = this.gridData.Select(string.Format("{0}{1}", MyUtility.Check.Empty(this.readyDate1) ? "1 = 1" : "ReadyDate >= '" + this.readyDate1 + "'", MyUtility.Check.Empty(this.readyDate2) ? string.Empty : " and ReadyDate <= '" + this.readyDate2 + "'"));
            return Ict.Result.True;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            if (this.printData.Length <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            string strXltName = Env.Cfg.XltPathDir + "\\PPIC_P05_Print.xltx";
            Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return false;
            }

            Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];
            int intRowsStart = 2;
            #region Header
            string factoryName = MyUtility.GetValue.Lookup(string.Format(
                @"
select  NameEN
from Factory
where ID = '{0}'", Env.User.Factory));
            worksheet.PageSetup.LeftHeader = string.Format("\n\nUp to SCI Delivery : {0}      Ready Date : {1} ~ {2}", this.strUp2SCIDelivery, this.readyDate1, this.readyDate2);
            worksheet.PageSetup.CenterHeader = string.Format("{0}\nProduction Schedule", factoryName);
            #endregion
            int rownum = 2, counter = 0;
            object[,] objArray = new object[1, 23];
            foreach (DataRow dr in this.printData)
            {
                rownum = intRowsStart + counter;
                worksheet.Cells[rownum, 1] = dr["ID"];
                worksheet.Cells[rownum, 2] = dr["StyleID"];
                worksheet.Cells[rownum, 3] = dr["SDPDate"];
                worksheet.Cells[rownum, 4] = dr["OrderQty"];
                worksheet.Cells[rownum, 5] = dr["Qty"];
                worksheet.Cells[rownum, 6] = dr["Inconsistent"];
                worksheet.Cells[rownum, 7] = dr["AlloQty"];
                worksheet.Cells[rownum, 8] = dr["KPILETA"];
                worksheet.Cells[rownum, 9] = dr["MTLETA"];
                worksheet.Cells[rownum, 10] = dr["MTLExport"];
                worksheet.Cells[rownum, 11] = dr["SewETA"];
                worksheet.Cells[rownum, 12] = dr["PackETA"];
                worksheet.Cells[rownum, 13] = dr["SewInLine"];
                worksheet.Cells[rownum, 14] = dr["SewOffLine"];
                worksheet.Cells[rownum, 15] = dr["ReadyDate"];
                worksheet.Cells[rownum, 16] = dr["EstPulloutDate"];
                worksheet.Cells[rownum, 17] = dr["BuyerDelivery"];
                worksheet.Cells[rownum, 18] = dr["Diff"];
                worksheet.Cells[rownum, 19] = dr["SewLine"];
                worksheet.Cells[rownum, 20] = dr["SCIDelivery"];
                worksheet.Cells[rownum, 21] = dr["OutReason"];
                worksheet.Cells[rownum, 22] = dr["OutReasonDesc"];
                worksheet.Cells[rownum, 23] = dr["OutRemark"];
                worksheet.Cells[rownum, 24] = dr["ProdRemark"];
                ((Excel.Range)worksheet.Range[worksheet.Cells[rownum, 1], worksheet.Cells[rownum, 24]]).Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlDash;
                counter++;
            }

            worksheet.Columns.AutoFit();

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("PPIC_P05_Print");
            Excel.Workbook workbook = excel.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            Marshal.ReleaseComObject(worksheet);
            Marshal.ReleaseComObject(workbook);

            strExcelName.OpenFile();
            #endregion
            return true;
        }
    }
}
