using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.PPIC
{
    public partial class P05_Print : Sci.Win.Tems.PrintForm
    {
        private DataTable gridData;
        private string readyDate1, readyDate2;
        private DataRow[] printData;
        
        public P05_Print(DataTable GridData)
        {
            InitializeComponent();
            gridData = GridData;
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
                counter++;
            }
            excel.Visible = true;
            return true;
        }
    }
}
