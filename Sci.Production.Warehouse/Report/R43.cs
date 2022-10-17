using Ict;
using Sci.Data;
using Sci.Win;
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
using static Sci.MyUtility;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Warehouse
{
    public partial class R43 : Win.Tems.PrintForm
    {
        private DataTable dtResult;
        /// <summary>
        /// R43
        /// </summary>
        public R43(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }
        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if (!this.dateIssue.HasValue)
            {
                MyUtility.Msg.WarningBox("Issue Date cannot be empty.");
                return false;
            }
            return base.ValidateInput();
        }
        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            string sqlQuery = string.Empty;
            string sqlWhere = string.Empty;
            string sqlHaving = string.Empty;
            List<SqlParameter> listPar = new List<SqlParameter>();

            if (!MyUtility.Check.Empty(this.dateIssue.Value1))
            {
                sqlWhere += $@" ir.IssueDate >= @IssueDate_S";
                listPar.Add(new SqlParameter("@IssueDate_S", this.dateIssue.Value1));
            }

            if (!MyUtility.Check.Empty(this.dateIssue.Value2))
            {
                sqlWhere += $@" and ir.IssueDate <= @IssueDate_E";
                listPar.Add(new SqlParameter("@IssueDate_E", this.dateIssue.Value2));
            }

            if (!MyUtility.Check.Empty(this.txtTransfer.Text)) 
            {
                sqlWhere += $@" and ir.IssueID = @IssueID";
                listPar.Add(new SqlParameter("@IssueID",this.txtTransfer.Text));
            }

            if (!MyUtility.Check.Empty(this.txtMdivision.Text))
            {
                sqlWhere += $@" and ir.MDivisionID = @Mdivision";
                listPar.Add(new SqlParameter("@Mdivision", this.txtMdivision.Text));
            }

            if (!MyUtility.Check.Empty(this.txtFactory.Text))
            {
                sqlWhere += $@" and ir.FactoryID = @FtyGroup";
                listPar.Add(new SqlParameter("@FtyGroup", this.txtFactory.Text));
            }

            sqlQuery = $@"
                        select ir.Id
                        ,ir.IssueDate
                        ,ir.IssueId
                        ,ir.Status
                        ,ir.MDivisionID
                        ,ir.FactoryID
                        ,ir.Remark
                        ,ird.POID
                        ,[Seq] = Concat (ird.Seq1, ' ', ird.Seq2)
                        ,ird.Roll
                        ,ird.Dyelot
                        ,[Description] = dbo.getMtlDesc (ird.POID, ird.Seq1, ird.Seq2,2,0)
                        ,psd.StockUnit
                        ,ird.Qty
                        ,ird.Location
                        from IssueReturn ir
                        inner join IssueReturn_Detail ird with(nolock) on ir.Id = ird.id
                        left join PO_Supp_Detail psd with(nolock) on  psd.id = ird.POID and psd.SEQ1 = ird.Seq1 and psd.SEQ2 = ird.Seq2
                        Where {sqlWhere}
                        order by ir.ID, ird.POID, ird.Seq1, ird.Seq2, ird.Roll, ird.Dyelot";

            DualResult result = DBProxy.Current.Select(null, sqlQuery, listPar, out this.dtResult);
            return result;
        }

        protected override bool OnToExcel(ReportDefinition report)
        {
            if (this.dtResult.Rows.Count > 0)
            {
                this.SetCount(this.dtResult.Rows.Count);
                this.ShowWaitMessage("Excel Processing...");

                string reportName = "Warehouse_R43";

                Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + $"\\{reportName}.xltx"); // 預先開啟excel app
                MyUtility.Excel.CopyToXls(this.dtResult, null, $"{reportName}.xltx", 1, showExcel: false, showSaveMsg: false, excelApp: objApp);
                Excel.Worksheet worksheet = objApp.Sheets[1];
                worksheet.Rows.AutoFit();
                worksheet.Columns.AutoFit();

                #region Save & Show Excel
                string strExcelName = Class.MicrosoftFile.GetName(reportName);
                objApp.ActiveWorkbook.SaveAs(strExcelName);
                objApp.Quit();
                Marshal.ReleaseComObject(objApp);
                Marshal.ReleaseComObject(worksheet);

                strExcelName.OpenFile();
                #endregion
                this.HideWaitMessage();
            }
            else
            {
                this.SetCount(0);
                MyUtility.Msg.InfoBox("Data not found!!");
            }

            return true;
        }
    }
}
