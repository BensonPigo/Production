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
    /// R39
    /// </summary>
    public partial class R39 : Win.Tems.PrintForm
    {
        private DataTable dtResult;

        /// <summary>
        /// R39
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R39(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {

            string sqlQuery = string.Empty;
            string sqlWhere = string.Empty;
            string sqlHaving = string.Empty;
            List<SqlParameter> listPar = new List<SqlParameter>();

            if (!MyUtility.Check.Empty(this.txtSP_From.Text))
            {
                sqlWhere += $@" and sfi.POID >= @FromPOID";
                listPar.Add(new SqlParameter("@FromPOID", this.txtSP_From.Text));
            }

            if (!MyUtility.Check.Empty(this.txtSP_To.Text))
            {
                sqlWhere += $@" and sfi.POID <= @ToPOID";
                listPar.Add(new SqlParameter("@ToPOID", this.txtSP_To.Text));
            }

            if (!MyUtility.Check.Empty(this.txtMdivision.Text))
            {
                sqlWhere += $@" and o.Mdivision = @Mdivision";
                listPar.Add(new SqlParameter("@Mdivision", this.txtMdivision.Text));
            }

            if (!MyUtility.Check.Empty(this.txtFactory.Text))
            {
                sqlWhere += $@" and o.FtyGroup = @FtyGroup";
                listPar.Add(new SqlParameter("@FtyGroup", this.txtFactory.Text));
            }

            if (!MyUtility.Check.Empty(this.txtRefnoFrom.Text))
            {
                sqlWhere += $@" and sfi.Refno >= @RefnoFrom";
                listPar.Add(new SqlParameter("@RefnoFrom", this.txtSP_From.Text));
            }

            if (!MyUtility.Check.Empty(this.txtRefnoTo.Text))
            {
                sqlWhere += $@" and sfi.Refno <= @RefnoTo";
                listPar.Add(new SqlParameter("@RefnoTo", this.txtRefnoTo.Text));
            }

            if (this.checkQty.Checked)
            {
                if (this.radioDetail.Checked)
                {
                    sqlWhere += $@" and (isnull(sfi.InQty, 0) - isnull(sfi.OutQty, 0) + isnull(sfi.AdjustQty, 0)) > 0";
                }
                else
                {
                    sqlHaving += $@" having (sum(isnull(sfi.InQty, 0)) - sum(isnull(sfi.OutQty, 0)) + sum(isnull(sfi.AdjustQty, 0))) > 0";
                }
            }

            if (this.radioSummary.Checked)
            {
                sqlQuery = $@"
select  sfi.POID,
        sfi.Refno,
        sf.Description,
        sf.Unit,
        [InQty] = sum(isnull(sfi.InQty, 0)),
        [OutQty] = sum(isnull(sfi.OutQty, 0)),
        [AdjustQty] = sum(isnull(sfi.AdjustQty, 0)),
        [Balance] = sum(isnull(sfi.InQty, 0)) - sum(isnull(sfi.OutQty, 0)) + sum(isnull(sfi.AdjustQty, 0)),
        [BulkLocation] = BulkLocation.val
from    SemiFinishedInventory sfi with (nolock) 
inner join orders o  with (nolock) on o.ID = sfi.POID
inner join  SemiFinished sf with (nolock) on sf.Refno = sfi.Refno
outer apply(SELECT val =  Stuff((select concat( ',',sfl.MtlLocationID)   
                                    from SemiFinishedInventory_Location sfl with (nolock)
                                    where   sfl.POID        = sfi.POID          and
                                            sfl.Refno       = sfi.Refno         and
                                            sfl.StockType   = sfi.StockType
                                FOR XML PATH('')),1,1,'') 
                ) BulkLocation
where   sfi.StockType  = 'B' {sqlWhere}
group by    sfi.POID,
            sfi.Refno,
            sf.Description,
            sf.Unit,
            sfi.StockType,
            BulkLocation.val
{sqlHaving}
";
            }
            else
            {
                sqlQuery = $@"
select  sfi.POID,
        sfi.Refno,
        sf.Description,
        sf.Unit,
        sfi.Roll,
        sfi.Dyelot,
        sfi.InQty,
        sfi.OutQty,
        sfi.AdjustQty,
        [Balance] = isnull(sfi.InQty, 0) - isnull(sfi.OutQty, 0) + isnull(sfi.AdjustQty, 0),
        [BulkLocation] = BulkLocation.val
from    SemiFinishedInventory sfi with (nolock) 
inner join orders o  with (nolock) on o.ID = sfi.POID
inner join  SemiFinished sf with (nolock) on sf.Refno = sfi.Refno
outer apply(SELECT val =  Stuff((select concat( ',',sfl.MtlLocationID)   
                                    from SemiFinishedInventory_Location sfl with (nolock)
                                    where   sfl.POID        = sfi.POID          and
                                            sfl.Refno       = sfi.Refno         and
                                            sfl.StockType   = sfi.StockType
                                FOR XML PATH('')),1,1,'') 
                ) BulkLocation
where   StockType = 'B' {sqlWhere}
";
            }

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

                string reportName = this.radioSummary.Checked ? "Warehouse_R39_Summary" : "Warehouse_R39_Detail";

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
