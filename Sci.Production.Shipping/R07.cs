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
    public partial class R07 : Sci.Win.Tems.PrintForm
    {
        DateTime? date1, date2, apvDate1, apvDate2;
        string supplier,mDivision;
        DataTable printData;
        public R07(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            dateRange1.Value1 = new DateTime(DateTime.Now.Year, 1, 1); //預設帶入登入系統當年的第一天
            dateRange1.Value2 = DateTime.Today;
            dateRange2.Value2 = DateTime.Today;
            DataTable mDivision;
            DBProxy.Current.Select(null, "select '' as ID union all select ID from MDivision", out mDivision);
            MyUtility.Tool.SetupCombox(comboBox1, 1, mDivision);
            comboBox1.Text = Sci.Env.User.Keyword;
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(dateRange1.Value1) && MyUtility.Check.Empty(dateRange1.Value2))
            {
                MyUtility.Msg.WarningBox("Date can't empty!!");
                return false;
            }

            mDivision = comboBox1.Text;
            date1 = dateRange1.Value1;
            date2 = dateRange1.Value2;
            apvDate1 = dateRange2.Value1;
            apvDate2 = dateRange2.Value2;
            supplier = txtsubcon1.TextBox1.Text;
            return base.ValidateInput();
        }

        // 非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(string.Format(@"select s.LocalSuppID+'-'+ISNULL(l.Abb,'') as Supplier,
s.MDivisionID,s.CurrencyID,SUM(s.Amount+s.VAT) as Amt,s.PayTermID+'-'+ISNULL(p.Name,'') as Terms
from ShippingAP s
left join LocalSupp l on s.LocalSuppID = l.ID
left join PayTerm p on s.PayTermID = p.ID
where s.CDate between '{0}' and '{1}'", Convert.ToDateTime(date1).ToString("d"), Convert.ToDateTime(date2).ToString("d")));

            if (!MyUtility.Check.Empty(apvDate1))
            {
                sqlCmd.Append(string.Format(" and s.ApvDate >= '{0}'", Convert.ToDateTime(apvDate1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(apvDate2))
            {
                sqlCmd.Append(string.Format(" and s.ApvDate <= '{0}'", Convert.ToDateTime(apvDate2).ToString("d")));
            }
            
            if (!MyUtility.Check.Empty(mDivision))
            {
                sqlCmd.Append(string.Format(" and s.MDivisionID = '{0}'", mDivision));
            }

            if (!MyUtility.Check.Empty(supplier))
            {
                sqlCmd.Append(string.Format(" and s.LocalSuppID = '{0}'", supplier));
            }

            sqlCmd.Append(@" group by s.LocalSuppID+'-'+ISNULL(l.Abb,''),s.MDivisionID,s.CurrencyID,s.PayTermID+'-'+ISNULL(p.Name,'')
            order by s.LocalSuppID+'-'+ISNULL(l.Abb,''),s.MDivisionID,s.PayTermID+'-'+ISNULL(p.Name,'')");
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

            this.ShowWaitMessage("Starting EXCEL...");
            string strXltName = Sci.Env.Cfg.XltPathDir + "\\Shipping_R07_PaymentSummary.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null) return false;
            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

            //填內容值
            int intRowsStart = 3;
            object[,] objArray = new object[1, 5];
            foreach (DataRow dr in printData.Rows)
            {
                objArray[0, 0] = dr["Supplier"];
                objArray[0, 1] = dr["MDivisionID"];
                objArray[0, 2] = dr["CurrencyID"];
                objArray[0, 3] = dr["Amt"];
                objArray[0, 4] = dr["Terms"];
                worksheet.Range[String.Format("A{0}:E{0}", intRowsStart)].Value2 = objArray;
                intRowsStart++;
            }

            excel.Cells.EntireColumn.AutoFit();
            excel.Cells.EntireRow.AutoFit();
            this.HideWaitMessage();
            excel.Visible = true;
            return true;
        }
    }
}
