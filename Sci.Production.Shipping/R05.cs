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
using System.Runtime.InteropServices;

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
            dateDate.Value1 = new DateTime(DateTime.Now.Year, 1, 1); //預設帶入登入系統當年的第一天
            dateDate.Value2 = DateTime.Today;
            DataTable mDivision;
            DBProxy.Current.Select(null, "select '' as ID union all select ID from MDivision WITH (NOLOCK) ", out mDivision);
            MyUtility.Tool.SetupCombox(comboM, 1, mDivision);
            comboM.Text = Sci.Env.User.Keyword;

            MyUtility.Tool.SetupCombox(comboOrderby, 1, 1, "Supplier,Handle");
            comboOrderby.SelectedIndex = 0;
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            //if (MyUtility.Check.Empty(dateRange1.Value1))
            //{
            //    MyUtility.Msg.WarningBox("Date can't empty!!");
            //    return false;
            //}

            mDivision = comboM.Text;
            date1 = dateDate.Value1;
            date2 = dateDate.Value2;
            supplier = txtSubconSupplier.TextBox1.Text;
            orderby = comboOrderby.SelectedIndex;

            return base.ValidateInput();
        }

        // 非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(string.Format(@"
select  Supplier = s.LocalSuppID + ' - ' + isnull(l.Abb,'')
        , s.ID
        , s.Type
        , s.CDate
        , Handle = s.Handle + ' - ' + isnull((select Name +' #'+ExtNo from Pass1 WITH (NOLOCK) where ID = s.Handle),'')
        , Brand = isnull (stuff ((select concat (',', BrandID)
                                  from (
                                      select distinct BrandID
                                      from GMTBooking gb
                                      inner join ShareExpense se on gb.id = se.InvNo
                                      where se.ShippingAPID = s.ID
                                  ) a for xml path(''))
                                 , 1, 1, '')
                          , '')
        , s.MDivisionID
        , s.CurrencyID
        , Amount = s.Amount + s.VAT
        , s.BLNo
        , s.InvNo
        , ExportInv = isnull (stuff ((select CONCAT ('/', InvNo) 
                                     from (
                                          select distinct InvNo 
                                          from ShareExpense WITH (NOLOCK) 
                                          where ShippingAPID = s.ID
                                     ) a for xml path(''))
                                    , 1, 1, '')
                             , '')
from ShippingAP s WITH (NOLOCK) 
left join LocalSupp l WITH (NOLOCK) on s.LocalSuppID = l.ID
where s.ApvDate is null
      and 1=1"));
            if (!MyUtility.Check.Empty(date1))
            {
                sqlCmd.Append(string.Format(" and s.CDate >= '{0}' ", Convert.ToDateTime(date1).ToString("d")));
            }
            if (!MyUtility.Check.Empty(date2))
            {
                sqlCmd.Append(string.Format(" and s.CDate <= '{0}' ", Convert.ToDateTime(date2).ToString("d")));
            }

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

            this.ShowWaitMessage("Starting EXCEL...");
            string strXltName = Sci.Env.Cfg.XltPathDir + "\\Shipping_R05_OutstandingPaymentList.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null) return false;
            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

            //填內容值
            int intRowsStart = 3;
            object[,] objArray = new object[1, printData.Rows.Count];
            foreach (DataRow dr in printData.Rows)
            {
                objArray[0, 0] = dr["Supplier"];
                objArray[0, 1] = dr["ID"];
                objArray[0, 2] = dr["Type"];
                objArray[0, 3] = dr["CDate"];
                objArray[0, 4] = dr["Handle"];
                objArray[0, 5] = dr["Brand"];
                objArray[0, 6] = dr["MDivisionID"];
                objArray[0, 7] = dr["CurrencyID"];
                objArray[0, 8] = dr["Amount"];
                objArray[0, 9] = dr["BLNo"];
                objArray[0, 10] = dr["InvNo"];
                objArray[0, 11] = dr["ExportInv"];
                worksheet.Range[String.Format("A{0}:L{0}", intRowsStart)].Value2 = objArray;
                intRowsStart++;
            }

            excel.Cells.EntireColumn.AutoFit();
            excel.Cells.EntireRow.AutoFit();
            this.HideWaitMessage();

            #region Save & Show Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Shipping_R05_OutstandingPaymentList");
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
