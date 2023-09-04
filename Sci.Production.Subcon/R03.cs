using Ict;
using Sci.Data;
using Sci.Utility.Report;
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

namespace Sci.Production.Subcon
{
    /// <inheritdoc/>
    public partial class R03 : Win.Tems.PrintForm
    {

        private DateTime? dateDate_1;
        private DateTime? dateDate_2;
        private DateTime? dateApproveDate_1;
        private DateTime? dateApproveDate_2;
        private string strMdivision;
        private string strFactory;
        private string strSupplier;
        private DataTable[] printData;

        /// <inheritdoc/>
        public R03(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();

            DataTable factory;
            DBProxy.Current.Select(null, "select '' as ID union all select ID from Factory WITH (NOLOCK) ", out factory);
            MyUtility.Tool.SetupCombox(this.comboFactory, 1, factory);
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if ( (MyUtility.Check.Empty(this.dateDate.Value1) ||
                MyUtility.Check.Empty(this.dateDate.Value2)) &&

                (MyUtility.Check.Empty(this.dateApproveDate.Value1) ||
                MyUtility.Check.Empty(this.dateApproveDate.Value2)))
            {
                MyUtility.Msg.WarningBox("Date and Approve Date can't be empty!!");
                return false;
            }

            this.dateDate_1 = this.dateDate.Value1;
            this.dateDate_2 = this.dateDate.Value2;
            this.dateApproveDate_1 = this.dateApproveDate.Value1;
            this.dateApproveDate_2 = this.dateApproveDate.Value2;
            this.strMdivision = this.txtMdivisionM.Text;
            this.strFactory = this.comboFactory.Text;
            this.strSupplier = this.txtSupplier.TextBox1.Text;
            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            string strWhere = string.Empty;

            if (!MyUtility.Check.Empty(this.dateDate_1) && !MyUtility.Check.Empty(this.dateDate_2))
            {
                sqlParameters.Add(new SqlParameter("@Date1", Convert.ToDateTime(this.dateDate_1).ToString("yyyy/MM/dd")));
                sqlParameters.Add(new SqlParameter("@Date2", Convert.ToDateTime(this.dateDate_2).ToString("yyyy/MM/dd")));
                strWhere += " and s.IssueDate BETWEEN  @Date1 and @Date2 ";
            }

            if (!MyUtility.Check.Empty(this.dateApproveDate_1) && !MyUtility.Check.Empty(this.dateApproveDate_2))
            {
                sqlParameters.Add(new SqlParameter("@ApproveDate_1", Convert.ToDateTime(this.dateApproveDate_1).ToString("yyyy/MM/dd")));
                sqlParameters.Add(new SqlParameter("@ApproveDate_2", Convert.ToDateTime(this.dateApproveDate_2).ToString("yyyy/MM/dd")));
                strWhere += " and s.ApvDate BETWEEN  @ApproveDate_1 and @ApproveDate_2";
            }

            if (!MyUtility.Check.Empty(this.strMdivision))
            {
                sqlParameters.Add(new SqlParameter("@MDivisionID", this.strMdivision));
                strWhere += " and s.MDivisionID = @MDivisionID";
            }

            if (!MyUtility.Check.Empty(this.strFactory))
            {
                sqlParameters.Add(new SqlParameter("@FactoryID", this.strFactory));
                strWhere += " and s.FactoryID = @FactoryID";
            }

            if (!MyUtility.Check.Empty(this.strSupplier))
            {
                sqlParameters.Add(new SqlParameter("@Supplier", this.strSupplier));
                strWhere += " and s.LocalSuppID = @Supplier";
            }

            string sqlcmd = $@"
            select 
              [ID] = s.ID
            , [Date] = s.IssueDate
            , [M] = s.MDivisionID
            , [Factory] = s.FactoryID
            , [Supplier] = s.LocalSuppID
            , [SupplierAbbr] = ls.Abb
            , [Terms] = pt.[ID] + '-' + pt.[Name]
            , [Invoice] = s.InvNo
            , [Corrency] = isnull(s.CurrencyID,'')
            , [Amount] = isnull(s.Amount,0)
            , [VatRate] = isnull(s.VatRate,0)
            , [Vat] = isnull(s.Vat,0)
            , [Total] = isnull(s.Amount,0) + isnull(s.Vat,0)
            , [Handle] = p1.[ID] + '-' + p1.[Name]
            , [Accountant] = p2.[ID] + '-' + p2.[Name]
            , [Status] = s.[Status]
            , [ApvDate] = s.ApvDate
            , [VoucherNo] = s.VoucherID
            , [VoucherDate] = s.VoucherDate
            , [ExVoucherNo] = s.ExVoucherID
            , [ExVoucherDate] = s.ExVoucherDate
            , [Remark] = s.Remark
            , [AddName] = s.AddName
            , [AddDate] = s.AddDate
            , [EditName] = s.EditName
            , [EditDate] = s.EditDate 
            FROM SubconOutContractAP s WITH(NOLOCK)
            LEFT JOIN LocalSupp ls WITH(NOLOCK) on ls.id = s.LocalSuppID
            LEFT JOIN PayTerm pt WITH(NOLOCK) on pt.id = s.PaytermID
            LEFT JOIN Pass1 p1 WITH(NOLOCK) on p1.id = s.Handle
            LEFT JOIN Pass1 p2 WITH(NOLOCK) on p2.id = s.ApvName
            WHERE 1 = 1
            {strWhere}

            SELECT 
              [ID] = sd.ID
            , [Supplier] = ls.ID + '-' + ls.[Name]
            , [ContractNo] = sd.ContractNumber
            , [SP] = sd.OrderId
            , [ComboType] = sd.ComboType
            , [Article] = sd.Article
            , [Qty] = sd.Qty
            , [Price] = isnull(sd.Price,0)
            , [Amount] = isnull(sd.Amount,0)
            , [Currency] = s.CurrencyID
            , [AccSewingQty] = isnull(SewingQty.val,0)
            , [AccPaidQty] = isnull(PaidQty.val,0)
            , [BalQty] = isnull(SewingQty.val,0) - isnull(PaidQty.val,0)
            FROM SubconOutContractAP_Detail sd WITH(NOLOCK)
            INNER JOIN SubconOutContractAP s WITH(NOLOCK) ON s.ID = sd.ID
            LEFT JOIN LocalSupp ls WITH(NOLOCK) ON ls.id = s.LocalSuppID
            OUTER APPLY
            (
	            select val = sum(sod.QAQty)  
	            from SewingOutput so
	            inner join SewingOutput_Detail sod on so.ID = sod.ID
	            where so.SubconOutFty = s.LocalSuppID
	            and so.SubConOutContractNumber = sd.ContractNumber
	            and sod.OrderId = sd.OrderId
	            and sod.ComboType = sd.ComboType
	            and sod.Article = sd.Article
            )SewingQty
            OUTER APPLY
            (
	            select val = sum(Qty)
		            from SubconOutContractAP_Detail sod
		            inner join SubconOutContractAP so on sod.ID = so.ID
		            where sod.ContractNumber = sd.ContractNumber
		            and sod.OrderId = sd.OrderId
		            and sod.ComboType = sd.ComboType
		            and sod.Article = sd.Article
		            and so.LocalSuppID = s.LocalSuppID
		            and so.ID <>sd.id
		            and so.Status <> 'New'
            )PaidQty
            WHERE 1 = 1
            {strWhere}
            ";

            DualResult result = DBProxy.Current.Select(null, sqlcmd,sqlParameters, out this.printData);
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
            this.SetCount(this.printData[0].Rows.Count);

            if (this.printData[0].Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            string excelName = "Subcon_R03";

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + $"\\{excelName}.xltx"); // 預先開啟excel app

            MyUtility.Excel.CopyToXls(this.printData[0], string.Empty, showExcel: false, xltfile: $"{excelName}.xltx", headerRow: 1, excelApp: objApp, wSheet: objApp.Sheets[1]);
            MyUtility.Excel.CopyToXls(this.printData[1], string.Empty, showExcel: false, xltfile: $"{excelName}.xltx", headerRow: 1, excelApp: objApp, wSheet: objApp.Sheets[2]);


            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName(excelName);
            Microsoft.Office.Interop.Excel.Workbook workbook = objApp.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);
            strExcelName.OpenFile();
            #endregion
            return true;
        }
    }
}
