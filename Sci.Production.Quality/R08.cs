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

namespace Sci.Production.Quality
{
    public partial class R08 : Sci.Win.Tems.PrintForm
    {

        public R08(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }

        DataTable printData;
        string sp1, sp2, uid;
        DateTime? InspectionDate1, InspectionDate2;
        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            InspectionDate1 = dateInspectionDate.Value1;
            InspectionDate2 = dateInspectionDate.Value2;
            uid = txtuser1.TextBox1.Text;
            sp1 = txtSPStart.Text;
            sp2 = txtSPEnd.Text;

            if (MyUtility.Check.Empty(InspectionDate1) || MyUtility.Check.Empty(InspectionDate2))
            {
                MyUtility.Msg.WarningBox("< Inspected Date > cannot be empty!");
                return false;
            }
            return base.ValidateInput();
        }

        //非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            #region 畫面上的條件
            StringBuilder sqlCmdW = new StringBuilder();
            sqlCmdW.Append(string.Format(" and FP.InspDate between '{0}' and '{1}'"
                , ((DateTime)InspectionDate1).ToString("d"), ((DateTime)InspectionDate2).ToString("d")));
            if (!MyUtility.Check.Empty(uid))
                sqlCmdW.Append(string.Format(" and FP.Inspector = '{0}'", uid));
            if (!MyUtility.Check.Empty(sp1))
                sqlCmdW.Append(string.Format(" and F.POID >= '{0}'", sp1));
            if (!MyUtility.Check.Empty(sp2))
                sqlCmdW.Append(string.Format(" and F.POID <= '{0}'", sp2));
            #endregion
            #region 主Sql
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(string.Format(@"
SELECT [Inspected Date] = FP.InspDate,
       [Inspector] = FP.Inspector,
       [SP#] = F.POID,
       [SEQ#] = concat(RTRIM(F.SEQ1) ,'-',F.SEQ2),
       [Roll#] = fp.Roll,
       [Dyelot#] = fp.Dyelot,
       [Arrived YDS] = RD.StockQty,
       [Actual YDS] = FP.ActualYds,
       [Speed] = '',
       [Grade] = FP.Grade
FROM FIR_Physical AS FP
LEFT JOIN FIR AS F ON FP.ID=F.ID
LEFT JOIN Receiving_Detail RD ON RD.PoId= F.POID AND RD.Seq1 = F.SEQ1 AND RD.Seq2 = F.SEQ2
								AND RD.Roll = FP.Roll AND RD.Dyelot = FP.Dyelot
WHERE 1=1
{0}
ORDER BY [Inspected Date],[Inspector],[SP#],[SEQ#],[Roll#],[Dyelot#]
"
                , sqlCmdW.ToString()));
            #endregion
            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }
            return Result.True;
        }

        // 產生Excel
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            SetCount(printData.Rows.Count);
            StringBuilder c = new StringBuilder();
            if (printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }
            string xltx = "Quality_R08.xltx";
            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\" + xltx); //預先開啟excel app
            MyUtility.Excel.CopyToXls(printData, "", xltx, 1, true, null, objApp);// 將datatable copy to excel
            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表

            if (objSheets != null) Marshal.FinalReleaseComObject(objSheets);    //釋放sheet
            if (objApp != null) Marshal.FinalReleaseComObject(objApp);          //釋放objApp
            return true;
        }
    }
}
