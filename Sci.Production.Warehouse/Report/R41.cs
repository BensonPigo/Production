using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using Sci.Win;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Warehouse
{
    /// <summary>
    /// R41
    /// </summary>
    public partial class R41 : Win.Tems.PrintForm
    {
        private DataTable dtResult;

        /// <summary>
        /// R41
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R41(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
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
                sqlWhere += $@" and i.IssueDate >= @IssueDate_S";
                listPar.Add(new SqlParameter("@IssueDate_S", this.dateIssue.Value1));
            }

            if (!MyUtility.Check.Empty(this.dateIssue.Value2))
            {
                sqlWhere += $@" and i.IssueDate <= @IssueDate_E";
                listPar.Add(new SqlParameter("@IssueDate_E", this.dateIssue.Value2));
            }

            if (!MyUtility.Check.Empty(this.txtRequest_From.Text))
            {
                sqlWhere += $@" and i.CutplanID >= @FromRequestNo";
                listPar.Add(new SqlParameter("@FromRequestNo", this.txtRequest_From.Text));
            }

            if (!MyUtility.Check.Empty(this.txtRequest_To.Text))
            {
                sqlWhere += $@" and i.CutplanID <= @ToRequestNo";
                listPar.Add(new SqlParameter("@ToRequestNo", this.txtRequest_To.Text));
            }

            if (!MyUtility.Check.Empty(this.txtMdivision.Text))
            {
                sqlWhere += $@" and i.MDivisionID = @Mdivision";
                listPar.Add(new SqlParameter("@Mdivision", this.txtMdivision.Text));
            }

            if (!MyUtility.Check.Empty(this.txtFactory.Text))
            {
                sqlWhere += $@" and i.FactoryID = @FtyGroup";
                listPar.Add(new SqlParameter("@FtyGroup", this.txtFactory.Text));
            }

            sqlQuery = $@"
select i.Id
,i.MDivisionID
,i.FactoryID
,i.CutplanID
,i.IssueDate
,i.AddDate
,[EditBy] = dbo.getPass1_ExtNo(i.EditName)
,i.EditDate
,i.Remark
,id.POID
,[Seq] = CONCAT(id.Seq1,' ',id.Seq2)
,po3.Refno
,po3.ColorID
,f.DescDetail
,id.Roll
,id.Dyelot
,po3.StockUnit
,id.Qty
,[BulkLocation] = dbo.Getlocation(fi.Ukey)
,[CreateBy] = dbo.getPass1_ExtNo(i.AddName)
from issue i
inner join Issue_Detail id on id.Id = i.Id
left join PO_Supp_Detail po3 on po3.ID = id.POID and po3.SEQ1 = id.Seq1 and po3.SEQ2 = id.Seq2
left join FtyInventory fi on fi.Ukey = id.FtyInventoryUkey
left join Fabric f on f.SCIRefno = po3.SCIRefno
where i.Type = 'I'
and i.Status = 'Confirmed'
{sqlWhere}
";

            DualResult result = DBProxy.Current.Select(null, sqlQuery, listPar, out this.dtResult);
            return result;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(ReportDefinition report)
        {
            if (this.dtResult.Rows.Count > 0)
            {
                this.SetCount(this.dtResult.Rows.Count);
                this.ShowWaitMessage("Excel Processing...");

                string reportName = "Warehouse_R41";

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
