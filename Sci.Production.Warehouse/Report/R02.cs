﻿using System;
using System.Data;
using System.Windows.Forms;
using Sci.Data;
using Ict;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class R02 : Win.Tems.PrintForm
    {
        private DataTable dt;
        private DateTime? strIssueDate1;
        private DateTime? strIssueDate2;
        private string strM;
        private string strFactory;

        /// <inheritdoc/>
        public R02(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.EditMode = true;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.strIssueDate1 = this.dateIssueDate.Value1;
            this.strIssueDate2 = this.dateIssueDate.Value2;
            this.strM = this.txtMdivision.Text;
            this.strFactory = this.txtfactory.Text;

            if (MyUtility.Check.Empty(this.strIssueDate1) && MyUtility.Check.Empty(this.strIssueDate2))
            {
                MyUtility.Msg.WarningBox("Issue date can't be empty!!");
                return false;
            }

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            this.SetCount(this.dt.Rows.Count);
            if (this.dt.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found!!");
                return false;
            }

            try
            {
                Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Warehouse_R02.xltx"); // 預先開啟excel app
                MyUtility.Excel.CopyToXls(this.dt, string.Empty, "Warehouse_R02.xltx", 1, showExcel: false, showSaveMsg: false, excelApp: objApp);

                this.ShowWaitMessage("Excel Processing...");

                #region Save & Show Excel
                string strExcelName = Class.MicrosoftFile.GetName("Warehouse_R02");
                objApp.ActiveWorkbook.SaveAs(strExcelName);
                objApp.Quit();
                Marshal.ReleaseComObject(objApp);

                strExcelName.OpenFile();
                #endregion
                this.HideWaitMessage();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            DualResult result = Ict.Result.True;

            try
            {
                string sqlFilter = string.Empty;
                if (!MyUtility.Check.Empty(this.strIssueDate1))
                {
                    sqlFilter += string.Format(" and '{0}' <= a.issuedate ", Convert.ToDateTime(this.strIssueDate1).ToString("yyyy/MM/dd"));
                }

                if (!MyUtility.Check.Empty(this.strIssueDate2))
                {
                    sqlFilter += string.Format(" and a.issuedate <= '{0}'", Convert.ToDateTime(this.strIssueDate2).ToString("yyyy/MM/dd"));
                }

                if (!MyUtility.Check.Empty(this.strM))
                {
                    sqlFilter += string.Format(" and a.MDivisionID = '{0}'", this.strM);
                }

                if (!MyUtility.Check.Empty(this.strFactory))
                {
                    sqlFilter += string.Format(" and Orders.FactoryID = '{0}'", this.strFactory);
                }

                string sqlcmd = string.Format(
                    @"
select 	a.MDivisionID
        ,Orders.FactoryID
        ,a.issuedate
		,poid = b.frompoid 
		,seq1 = b.fromseq1 
		,seq2 = b.fromseq2 
		,qty = sum(b.qty) 
		,unit =  isnull((select stockunit 
						 from po_supp_detail WITH (NOLOCK) 
						 where id= b.FromPOID and seq1 = b.FromSeq1 and seq2 = b.FromSeq2)
						,'')
		,description = RTRIM(LTRIM( dbo.getmtldesc(b.FromPOID,b.FromSeq1,b.FromSeq2,2,0) )) 
		,[Junk]=IIF(Orders.Junk=1,'Y','N')
		,Orders.OrderTypeID
from dbo.SubTransfer a WITH (NOLOCK) 
inner join dbo.SubTransfer_Detail b WITH (NOLOCK) on a.id = b.id
inner join dbo.Orders on b.fromPoid = Orders.ID
where a.Status = 'Confirmed' and a.type='D' {0}
group by a.MDivisionID, Orders.FactoryID, a.issuedate, b.FromPOID, b.FromSeq1, b.FromSeq2, Orders.Junk, Orders.OrderTypeID
order by a.MDivisionID, Orders.FactoryID, a.issuedate, b.FromPOID, b.FromSeq1, b.FromSeq2
", sqlFilter);
                result = DBProxy.Current.Select(null, sqlcmd, out this.dt);
                if (!result)
                {
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }
    }
}
