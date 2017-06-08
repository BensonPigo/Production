using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Warehouse
{
    public partial class R20 : Sci.Win.Tems.PrintForm
    {
        DataTable printData;

        public R20(ToolStripMenuItem menuitem)
            :base(menuitem)
        {
            InitializeComponent();
        }

        protected override bool ValidateInput()
        {
            #region Set Value

            #endregion
            #region 判斷必輸條件

            #endregion
            return true;
        }

        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            #region SQl Parameters
            List<SqlParameter> listSqlPar = new List<SqlParameter>();
            #endregion
            #region SQL Filte
            List<string> filte = new List<string>();
            #endregion
            #region SQL CMD
            string sqlCmd = string.Format(@"
");
            #endregion
            #region Get Data
            DualResult result;
            if (result = DBProxy.Current.Select(null, sqlCmd, listSqlPar, out printData))
            {
                return result;
            }
            #endregion
            return Result.True;
        }

        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            #region check printData
            if (printData == null || printData.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found.");
                return false;
            }
            #endregion
            this.SetCount(printData.Rows.Count);
            this.ShowWaitMessage("Excel Processing");
            #region To Excel
            Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Warehouse_R20.xltx");
            MyUtility.Excel.CopyToXls(printData, "", "Warehouse_R20.xltx", 2, showExcel: true, excelApp: objApp);
            Excel.Worksheet worksheet = objApp.Sheets[1];
            worksheet.Columns.AutoFit();

            if (objApp != null) Marshal.FinalReleaseComObject(objApp);
            if (worksheet != null) Marshal.FinalReleaseComObject(worksheet);
            #endregion
            this.HideWaitMessage();
            return true;
        }
    }
}
