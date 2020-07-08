using Ict;
using Sci.Data;
using Sci.Win;
using System.Data;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Shipping
{
    public partial class R18 : Win.Tems.PrintForm
    {
        private DataTable PrintTable;
        private string ShippingExpenseID_s_Where;
        private string ShippingExpenseID_e_Where;
        private string localSuppID_Where;
        private string Status_Where;

        public R18(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        private void TxtShippingExpenseID_s_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sqlCmd = "select ID from ShipExpense";
            Win.Tools.SelectItem item = new Win.Tools.SelectItem(sqlCmd, "20", string.Empty, "ID");
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            this.txtShippingExpenseID_s.Text = item.GetSelectedString();
        }

        private void TxtShippingExpenseID_e_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sqlCmd = "select ID from ShipExpense";
            Win.Tools.SelectItem item = new Win.Tools.SelectItem(sqlCmd, "20", string.Empty, "ID");
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            this.txtShippingExpenseID_e.Text = item.GetSelectedString();
        }

        protected override bool ValidateInput()
        {
            this.ShippingExpenseID_s_Where = string.Empty;
            this.ShippingExpenseID_e_Where = string.Empty;
            this.localSuppID_Where = string.Empty;
            this.Status_Where = string.Empty;
            if (!MyUtility.Check.Empty(this.txtShippingExpenseID_s.Text))
            {
                this.ShippingExpenseID_s_Where = $"AND s.ID >= '{this.txtShippingExpenseID_s.Text}'";
            }

            if (!MyUtility.Check.Empty(this.txtShippingExpenseID_e.Text))
            {
                this.ShippingExpenseID_e_Where = $"AND s.ID <= '{this.txtShippingExpenseID_e.Text}'";
            }

            if (!MyUtility.Check.Empty(this.txtsubcon.TextBox1.Text))
            {
                this.localSuppID_Where = $"AND s.LocalSuppID = '{this.txtsubcon.TextBox1.Text}'";
            }

            if (this.chkIncludeJunk.Checked)
            {
                this.Status_Where = string.Empty;
            }
            else
            {
                this.Status_Where = "AND s.junk = 0";
            }

            return base.ValidateInput();
        }

        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            string sqlCmd = string.Empty;

            sqlCmd = $@"
SELECT 
        s.ID
        ,s.Description
        ,s.UnitID
        ,s.BrandID
        ,s.LocalSuppID
        ,[Abbr]=(SELECT Abb FROM LocalSupp WHERE ID = s.LocalSuppID)
        ,s.CurrencyID
        ,s.Price
        ,s.CanvassDate
        ,s.AccountID
        ,[Account Name]=(select Name from dbo.SciFMS_AccountNo where id = s.AccountID)
        ,s.Status
        ,[LastPaymentDate]=LastPaymentDate.Date
FROM ShipExpense s
OUTER APPLY(
	select Date=max(sap.CDate) 
	from shippingAP sap
	left join ShippingAP_Detail sd  on sap.ID = sd.ID 
	WHERE sd.ShipExpenseID = s.ID
)LastPaymentDate

WHERE 1=1
    {this.ShippingExpenseID_s_Where}
    {this.ShippingExpenseID_e_Where}
    {this.localSuppID_Where}
    {this.Status_Where}
";

            return DBProxy.Current.Select(null, sqlCmd, out this.PrintTable);
        }

        protected override bool OnToExcel(ReportDefinition report)
        {
            this.SetCount(this.PrintTable.Rows.Count);
            if (this.PrintTable.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found");
                return false;
            }

            this.ShowWaitMessage("Excel Processing...");
            string excelName = "Shipping_R18";
            Excel.Application excelApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + $"\\{excelName}.xltx");
            MyUtility.Excel.CopyToXls(this.PrintTable, string.Empty, $"{excelName}.xltx", 1, false, null, excelApp, wSheet: excelApp.Sheets[1]); // 將datatable copy to excel
            excelApp.DisplayAlerts = false;
            Excel.Worksheet worksheet = excelApp.ActiveWorkbook.Worksheets[1]; // 取得工作表

            worksheet.Columns.AutoFit();
            #region 釋放上面開啟過excel物件
            string strExcelName = Class.MicrosoftFile.GetName(excelName);
            Excel.Workbook workbook = excelApp.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            excelApp.Quit();

            if (worksheet != null)
            {
                Marshal.FinalReleaseComObject(worksheet);
            }

            if (excelApp != null)
            {
                Marshal.FinalReleaseComObject(excelApp);
            }
            #endregion

            this.HideWaitMessage();
            strExcelName.OpenFile();
            return true;
        }
    }
}
