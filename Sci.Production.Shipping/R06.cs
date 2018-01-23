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
    /// <summary>
    /// R06
    /// </summary>
    public partial class R06 : Sci.Win.Tems.PrintForm
    {
        private DateTime? date1;
        private DateTime? date2;
        private DateTime? apvDate1;
        private DateTime? apvDate2;
        private string blno1;
        private string blno2;
        private string supplier;
        private string mDivision;
        private int orderby;
        private DataTable printData;

        /// <summary>
        /// R06
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R06(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.dateDate.Value1 = new DateTime(DateTime.Now.Year, 1, 1); // 預設帶入登入系統當年的第一天
            this.dateDate.Value2 = DateTime.Today;
            this.dateApvDate.Value2 = DateTime.Today;
            DataTable mDivision;
            DBProxy.Current.Select(null, "select '' as ID union all select ID from MDivision WITH (NOLOCK) ", out mDivision);
            MyUtility.Tool.SetupCombox(this.comboM, 1, mDivision);
            this.comboM.Text = Sci.Env.User.Keyword;

            MyUtility.Tool.SetupCombox(this.comboOrderby, 1, 1, "M,B/L No.");
            this.comboOrderby.SelectedIndex = 0;
            this.radioDetail.Checked = true;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(this.dateDate.Value1) && MyUtility.Check.Empty(this.dateDate.Value2) && MyUtility.Check.Empty(this.dateApvDate.Value1) && MyUtility.Check.Empty(this.dateApvDate.Value2))
            {
                MyUtility.Msg.WarningBox("Date can't empty!!");
                return false;
            }

            this.mDivision = this.comboM.Text;
            this.date1 = this.dateDate.Value1;
            this.date2 = this.dateDate.Value2;
            this.apvDate1 = this.dateApvDate.Value1;
            this.apvDate2 = this.dateApvDate.Value2;
            this.blno1 = this.txtBLNoStart.Text;
            this.blno2 = this.txtBLNoEnd.Text;
            this.supplier = this.txtSubconSupplier.TextBox1.Text;
            this.orderby = this.comboOrderby.SelectedIndex;

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            if (this.radioDetail.Checked == true)
            {
                sqlCmd.Append(@"select	s.Type,
		Supplier = s.LocalSuppID + ls.Abb,
		s.ID,
		s.VoucherID,
		s.CDate,
		s.[ApvDate],
		s.[MDivisionID],
		s.[CurrencyID],
		s.[Amount],
		s.[BLNo],
		s.[Remark],
		s.[InvNo],
		ExportINV =  Stuff((select iif(WKNo = '','',concat( '/',WKNo)) + iif(InvNo = '','',concat( '/',InvNo) )   from ShareExpense she where she.ShippingAPID = s.ID FOR XML PATH('')),1,1,'') ,
		sd.[ShipExpenseID],
		se.Description,
		sd.[Qty],
		se.UnitID,
		sd.[CurrencyID],
		sd.[Price],
		sd.[Rate],
		sd.[Amount],
		sd.[Remark],
		se.AccountID,
		an.Name
from ShippingAP s WITH (NOLOCK)
inner join ShippingAP_Detail sd WITH (NOLOCK) ON s.ID = sd.ID
left join ShipExpense se  WITH (NOLOCK) ON sd.ShipExpenseID = se.ID
left join [FinanceEN].dbo.AccountNo an on an.ID = se.AccountID 
left join LocalSupp ls WITH (NOLOCK) on s.LocalSuppID = ls.ID
where s.Status = 'Approved'");
            }
            else
            {
                sqlCmd.Append(@"select s.Type,s.LocalSuppID+'-'+ISNULL(l.Abb,'') as Supplier,s.ID,s.VoucherID,
s.CDate,CONVERT(DATE,s.ApvDate) as ApvDate,s.MDivisionID,s.CurrencyID,s.Amount+s.VAT as Amt,s.BLNo,s.Remark,s.InvNo,
isnull((select CONCAT(InvNo,'/') from (select distinct InvNo from ShareExpense WITH (NOLOCK) where ShippingAPID = s.ID) a for xml path('')),'') as ExportInv
from ShippingAP s WITH (NOLOCK) 
left join LocalSupp l WITH (NOLOCK) on s.LocalSuppID = l.ID
where s.Status = 'Approved'");
            }

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

            if (!MyUtility.Check.Empty(this.blno1))
            {
                sqlCmd.Append(string.Format(" and s.BLNo >= '{0}'", this.blno1));
            }

            if (!MyUtility.Check.Empty(this.blno2))
            {
                sqlCmd.Append(string.Format(" and s.BLNo <= '{0}'", this.blno2));
            }

            if (!MyUtility.Check.Empty(this.mDivision))
            {
                sqlCmd.Append(string.Format(" and s.MDivisionID = '{0}'", this.mDivision));
            }

            if (!MyUtility.Check.Empty(this.supplier))
            {
                sqlCmd.Append(string.Format(" and s.LocalSuppID = '{0}'", this.supplier));
            }

            if (this.orderby == 0)
            {
                sqlCmd.Append(" order by s.MDivisionID");
            }
            else if (this.orderby == 1)
            {
                sqlCmd.Append(" order by s.BLNo");
            }

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out this.printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return Result.True;
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
            string strXltName = this.radioDetail.Checked == true ? Sci.Env.Cfg.XltPathDir + "\\Shipping_R06_PaymentListDetail.xltx" : Sci.Env.Cfg.XltPathDir + "\\Shipping_R06_PaymentList.xltx";

            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return false;
            }

            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

            if (this.radioDetail.Checked == true)
            {
                Sci.Utility.Report.ExcelCOM com = new Sci.Utility.Report.ExcelCOM(Sci.Env.Cfg.XltPathDir + "\\Shipping_R06_PaymentListDetail.xltx", excel);
                com.WriteTable(this.printData, 2);
            }
            else
            {
                // 填內容值
                int intRowsStart = 3;
                object[,] objArray = new object[1, 13];
                foreach (DataRow dr in this.printData.Rows)
                {
                    objArray[0, 0] = dr["Type"];
                    objArray[0, 1] = dr["Supplier"];
                    objArray[0, 2] = dr["ID"];
                    objArray[0, 3] = dr["VoucherID"];
                    objArray[0, 4] = dr["CDate"];
                    objArray[0, 5] = dr["ApvDate"];
                    objArray[0, 6] = dr["MDivisionID"];
                    objArray[0, 7] = dr["CurrencyID"];
                    objArray[0, 8] = dr["Amt"];
                    objArray[0, 9] = dr["BLNo"];
                    objArray[0, 10] = dr["Remark"];
                    objArray[0, 11] = dr["InvNo"];
                    objArray[0, 12] = MyUtility.Check.Empty(dr["ExportInv"]) ? string.Empty : MyUtility.Convert.GetString(dr["ExportInv"]).Substring(0, MyUtility.Convert.GetString(dr["ExportInv"]).Length - 1);
                    worksheet.Range[string.Format("A{0}:M{0}", intRowsStart)].Value2 = objArray;
                    intRowsStart++;
                }
            }

            excel.Cells.EntireColumn.AutoFit();
            excel.Cells.EntireRow.AutoFit();
            this.HideWaitMessage();

            #region Save & Show Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName(this.radioDetail.Checked == true ? "Shipping_R06_PaymentListDetail" : "Shipping_R06_PaymentList");
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
