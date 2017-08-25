using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using Ict;

namespace Sci.Production.PPIC
{
    public partial class P05_Print : Sci.Win.Tems.PrintForm
    {
        private DataTable gridData;
        private string readyDate1, readyDate2, strUp2SCIDelivery;
        private DataRow[] printData;
        
        public P05_Print(DataTable GridData, string strUp2SCIDelivery)
        {
            InitializeComponent();
            gridData = GridData;
            this.strUp2SCIDelivery = strUp2SCIDelivery;
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            readyDate1 = MyUtility.Check.Empty(dateReadydate.Value1) ? "" : Convert.ToDateTime(dateReadydate.Value1).ToString("d");
            readyDate2 = MyUtility.Check.Empty(dateReadydate.Value2) ? "" : Convert.ToDateTime(dateReadydate.Value2).ToString("d");

            return base.ValidateInput();
        }

        // 非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            printData = gridData.Select(string.Format("{0}{1}", (MyUtility.Check.Empty(readyDate1) ? "1 = 1" : "ReadyDate >= '" + readyDate1 + "'"), (MyUtility.Check.Empty(readyDate2) ? "" : " and ReadyDate <= '" + readyDate2 + "'")));
            return Result.True;
        }

        // 產生Excel
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            if (printData.Length <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            string strXltName = Sci.Env.Cfg.XltPathDir + "\\PPIC_P05_Print.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null) return false;
            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];
            int intRowsStart = 2;
            #region Header
            string factoryName = MyUtility.GetValue.Lookup(string.Format(@"
select  NameEN
from Factory
where ID = '{0}'", Sci.Env.User.Factory));
            worksheet.PageSetup.LeftHeader = string.Format("\n\nUp to SCI Delivery : {0}      Ready Date : {1} ~ {2}", strUp2SCIDelivery, readyDate1, readyDate2);
            worksheet.PageSetup.CenterHeader = string.Format("{0}\nProduction Schedule", factoryName);
            #endregion             
            int rownum = 2, counter = 0;            
            object[,] objArray = new object[1, 23];
            foreach (DataRow dr in printData)
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
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("PPIC_P05_Print");
            Microsoft.Office.Interop.Excel.Workbook workbook = excel.ActiveWorkbook;
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
