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

namespace Sci.Production.Subcon
{
    public partial class R50 : Sci.Win.Tems.PrintForm
    {
        public R50(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }

        string Factory, BundleNO;
        DateTime? Date1, Date2;
        DataTable printTable;
        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            Date1 = Date.Value1;
            Date2 = Date.Value2;
            BundleNO = txtBundleNo.Text;
            Factory = txtfactory.Text;

            return base.ValidateInput();
        }

        //非同步讀取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"
Select 
	o.FactoryID,BD.BundleNo,bd.BundleGroup,o.StyleID,o.SeasonID,o.brandid,bd.Patterncode,bd.PatternDesc,bd.SizeCode,bd.Qty,
	b.poid,b.Colorid,b.Article,b.Cdate,b.Orderid,b.Item ,b.AddDate,B.AddName,B.EditDate,b.AddName
From Bundle_Detail BD 
Left join bundle b on (bd.Id = b.ID)
Left join Orders O on(b.Orderid = O.id)
Where 1=1
");
            if (!MyUtility.Check.Empty(Date1))
                sql.Append(string.Format("And (Convert (date, b.AddDate)  >= '{0}' Or Convert (date, b.EditDate) >= '{0}')", Convert.ToDateTime(Date1).ToString("d")));
            if (!MyUtility.Check.Empty(Date2))
                sql.Append(string.Format("And (Convert (date, b.AddDate)  <= '{0}' Or Convert (date, b.EditDate) <= '{0}')", Convert.ToDateTime(Date2).ToString("d")));
            if (!MyUtility.Check.Empty(BundleNO))
                sql.Append(string.Format("And BD.BundleNo = '{0}'", BundleNO));
            if (!MyUtility.Check.Empty(Factory))
                sql.Append(string.Format("And O.FtyGroup = '{0}'", Factory));
            DualResult result = DBProxy.Current.Select(null, sql.ToString(), out printTable);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }
            return Result.True;
        }

        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            SetCount(printTable.Rows.Count);
            if (printTable.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }
            //預先開啟excel app
            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Subcon_R50_ProductionBundleTransfer.xltx");
            // 將datatable copy to excel
            MyUtility.Excel.CopyToXls(printTable, "", "Subcon_R50_ProductionBundleTransfer.xltx", 1, true, null, objApp);
            // 取得工作表
            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];
            if (objSheets != null) Marshal.FinalReleaseComObject(objSheets);    //釋放sheet
            if (objApp != null) Marshal.FinalReleaseComObject(objApp);          //釋放objApp 
            return true;
        }
    }
}
