using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using System.Runtime.InteropServices;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// R07
    /// </summary>
    public partial class R07 : Win.Tems.PrintForm
    {
        private DateTime? date1;
        private DateTime? date2;
        private DateTime? apvDate1;
        private DateTime? apvDate2;
        private string supplier;
        private string mDivision;
        private DataTable printData;

        /// <summary>
        /// R07
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R07(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.dateDate.Value1 = new DateTime(DateTime.Now.Year, 1, 1); // 預設帶入登入系統當年的第一天
            this.dateDate.Value2 = DateTime.Today;
            this.dateApvDate.Value2 = DateTime.Today;
            DataTable mDivision;
            DBProxy.Current.Select(null, "select '' as ID union all select ID from MDivision WITH (NOLOCK) ", out mDivision);
            MyUtility.Tool.SetupCombox(this.comboM, 1, mDivision);
            this.comboM.Text = Env.User.Keyword;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(this.dateDate.Value1) && MyUtility.Check.Empty(this.dateDate.Value2))
            {
                MyUtility.Msg.WarningBox("Date can't empty!!");
                return false;
            }

            this.mDivision = this.comboM.Text;
            this.date1 = this.dateDate.Value1;
            this.date2 = this.dateDate.Value2;
            this.apvDate1 = this.dateApvDate.Value1;
            this.apvDate2 = this.dateApvDate.Value2;
            this.supplier = this.txtSubconSupplier.TextBox1.Text;
            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(string.Format(@"select s.LocalSuppID+'-'+ISNULL(l.Abb,'') as Supplier,
s.MDivisionID,s.CurrencyID,SUM(s.Amount+s.VAT) as Amt,s.PayTermID+'-'+ISNULL(p.Name,'') as Terms
from ShippingAP s WITH (NOLOCK) 
left join LocalSupp l WITH (NOLOCK) on s.LocalSuppID = l.ID
left join PayTerm p WITH (NOLOCK) on s.PayTermID = p.ID
where 1=1"));
            if (!MyUtility.Check.Empty(this.date1))
            {
                sqlCmd.Append(string.Format(" and s.CDate >= '{0}'", Convert.ToDateTime(this.date1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.date2))
            {
                sqlCmd.Append(string.Format(" and s.CDate <= '{0}'", Convert.ToDateTime(this.date2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.apvDate1))
            {
                sqlCmd.Append(string.Format(" and CONVERT(DATE,s.ApvDate) >= '{0}'", Convert.ToDateTime(this.apvDate1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.apvDate2))
            {
                sqlCmd.Append(string.Format(" and CONVERT(DATE,s.ApvDate) <= '{0}'", Convert.ToDateTime(this.apvDate2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.mDivision))
            {
                sqlCmd.Append(string.Format(" and s.MDivisionID = '{0}'", this.mDivision));
            }

            if (!MyUtility.Check.Empty(this.supplier))
            {
                sqlCmd.Append(string.Format(" and s.LocalSuppID = '{0}'", this.supplier));
            }

            sqlCmd.Append(@" group by s.LocalSuppID+'-'+ISNULL(l.Abb,''),s.MDivisionID,s.CurrencyID,s.PayTermID+'-'+ISNULL(p.Name,'')
            order by s.LocalSuppID+'-'+ISNULL(l.Abb,''),s.MDivisionID,s.PayTermID+'-'+ISNULL(p.Name,'')");
            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out this.printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return Ict.Result.True;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.printData.Rows.Count);

            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            this.ShowWaitMessage("Starting EXCEL...");
            string strXltName = Env.Cfg.XltPathDir + "\\Shipping_R07_PaymentSummary.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return false;
            }

            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

            // 填內容值
            int intRowsStart = 3;
            object[,] objArray = new object[1, 5];
            foreach (DataRow dr in this.printData.Rows)
            {
                objArray[0, 0] = dr["Supplier"];
                objArray[0, 1] = dr["MDivisionID"];
                objArray[0, 2] = dr["CurrencyID"];
                objArray[0, 3] = dr["Amt"];
                objArray[0, 4] = dr["Terms"];
                worksheet.Range[string.Format("A{0}:E{0}", intRowsStart)].Value2 = objArray;
                intRowsStart++;
            }

            excel.Cells.EntireColumn.AutoFit();
            excel.Cells.EntireRow.AutoFit();
            this.HideWaitMessage();

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("Shipping_R07_PaymentSummary");
            excel.ActiveWorkbook.SaveAs(strExcelName);
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            Marshal.ReleaseComObject(worksheet);

            strExcelName.OpenFile();
            #endregion
            return true;
        }
    }
}
