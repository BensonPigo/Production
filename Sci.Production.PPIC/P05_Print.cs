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
            readyDate1 = MyUtility.Check.Empty(dateRange1.Value1) ? "" : Convert.ToDateTime(dateRange1.Value1).ToString("d");
            readyDate2 = MyUtility.Check.Empty(dateRange1.Value2) ? "" : Convert.ToDateTime(dateRange1.Value2).ToString("d");

            return base.ValidateInput();
        }

        // 非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            printData = gridData.Select(string.Format("{0}{1}", (MyUtility.Check.Empty(readyDate1) ? "1 = 1" : "ReadyDate >= '" + readyDate1 + "'"), (MyUtility.Check.Empty(readyDate2) ? " and 1 = 1" : " and ReadyDate <= '" + readyDate2 + "'")));
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

            string strXltName = Sci.Env.Cfg.XltPathDir + "PPIC_P05_Print.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null) return false;
            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];
            int intRowsStart = 2;
            int rownum = 0, counter = 0;
            object[,] objArray = new object[1, 21];
            foreach (DataRow dr in printData)
            {
                rownum = intRowsStart + counter;
                objArray[0, 0] = dr["ID"];
                objArray[0, 1] = dr["StyleID"];
                objArray[0, 2] = dr["SDPDate"];
                objArray[0, 3] = dr["OrderQty"];
                objArray[0, 4] = dr["Qty"];
                objArray[0, 5] = dr["Inconsistent"];
                objArray[0, 6] = dr["AlloQty"];
                objArray[0, 7] = dr["KPILETA"];
                objArray[0, 8] = dr["MTLETA"];
                objArray[0, 9] = dr["MTLExport"];
                objArray[0, 10] = dr["SewETA"];
                objArray[0, 11] = dr["PackETA"];
                objArray[0, 12] = dr["SewInLine"];
                objArray[0, 13] = dr["SewOffLine"];
                objArray[0, 14] = dr["ReadyDate"];
                objArray[0, 15] = dr["EstPulloutDate"];
                objArray[0, 16] = dr["BuyerDelivery"];
                objArray[0, 17] = dr["Diff"];
                objArray[0, 18] = dr["SewLine"];
                objArray[0, 19] = dr["SCIDelivery"];
                objArray[0, 20] = dr["ProdRemark"];
                worksheet.Range[String.Format("A{0}:U{0}", rownum)].Value2 = objArray;
                counter++;
            }
            excel.Visible = true;
            return true;
        }
    }
}
