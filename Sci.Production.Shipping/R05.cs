using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;

namespace Sci.Production.Shipping
{
    public partial class R05 : Sci.Win.Tems.PrintForm
    {
        DateTime? date1, date2;
        string mDivision,supplier;
        int orderby;
        DataTable printData;
        public R05(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            dateRange1.Value1 = new DateTime(DateTime.Now.Year, 1, 1); //預設帶入登入系統當年的第一天
            dateRange1.Value2 = DateTime.Today;
            DataTable mDivision;
            DBProxy.Current.Select(null, "select '' as ID union all select ID from MDivision", out mDivision);
            MyUtility.Tool.SetupCombox(comboBox1, 1, mDivision);
            comboBox1.Text = Sci.Env.User.Keyword;

            MyUtility.Tool.SetupCombox(comboBox2, 1, 1, "Supplier,Handle");
            comboBox2.SelectedIndex = 0;
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(dateRange1.Value1))
            {
                MyUtility.Msg.WarningBox("Date can't empty!!");
                return false;
            }

            mDivision = comboBox1.Text;
            date1 = dateRange1.Value1;
            date2 = dateRange1.Value2;
            supplier = txtsubcon1.TextBox1.Text;
            orderby = comboBox2.SelectedIndex;

            return base.ValidateInput();
        }

        // 非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(string.Format(@"select s.LocalSuppID + ' - ' + isnull(l.Abb,'') as Supplier,s.ID,s.Type,s.CDate,
s.Handle + ' - ' + isnull((select Name +' #'+ExtNo from Pass1 where ID = s.Handle),'') as Handle,
s.MDivisionID,s.CurrencyID,s.Amount+s.VAT as Amount,s.BLNo,s.InvNo,
isnull((select CONCAT(InvNo,'/') from (select distinct InvNo from ShareExpense where ShippingAPID = s.ID) a for xml path('')),'') as ExportInv
from ShippingAP s
left join LocalSupp l on s.LocalSuppID = l.ID
where s.ApvDate is null
and s.CDate between '{0}' and '{1}'", Convert.ToDateTime(date1).ToString("d"), Convert.ToDateTime(date2).ToString("d")));

            if (!MyUtility.Check.Empty(mDivision))
            {
                sqlCmd.Append(string.Format(" and s.MDivisionID = '{0}'", mDivision));
            }

            if (!MyUtility.Check.Empty(supplier))
            {
                sqlCmd.Append(string.Format(" and s.LocalSuppID = '{0}'", supplier));
            }

            if (orderby == 0)
            {
                sqlCmd.Append(" order by s.LocalSuppID");
            }
            else if (orderby == 1)
            {
                sqlCmd.Append(" order by s.Handle");
            }
            
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

            if (printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            MyUtility.Msg.WaitWindows("Starting EXCEL...");
            string strXltName = Sci.Env.Cfg.XltPathDir + "Shipping_R05_OutstandingPayment List.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null) return false;
            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

            //填內容值
            int intRowsStart = 3;
            object[,] objArray = new object[1, 11];
            foreach (DataRow dr in printData.Rows)
            {
                objArray[0, 0] = dr["Supplier"];
                objArray[0, 1] = dr["ID"];
                objArray[0, 2] = dr["Type"];
                objArray[0, 3] = dr["CDate"];
                objArray[0, 4] = dr["Handle"];
                objArray[0, 5] = dr["MDivisionID"];
                objArray[0, 6] = dr["CurrencyID"];
                objArray[0, 7] = dr["Amount"];
                objArray[0, 8] = dr["BLNo"];
                objArray[0, 9] = dr["InvNo"];
                objArray[0, 10] = MyUtility.Check.Empty(dr["ExportInv"]) ? "" : MyUtility.Convert.GetString(dr["ExportInv"]).Substring(0,MyUtility.Convert.GetString(dr["ExportInv"]).Length-1);
                worksheet.Range[String.Format("A{0}:K{0}", intRowsStart)].Value2 = objArray;
                intRowsStart++;
            }

            excel.Cells.EntireColumn.AutoFit();
            excel.Cells.EntireRow.AutoFit();
            MyUtility.Msg.WaitClear();
            excel.Visible = true;
            return true;
        }
    }
}
