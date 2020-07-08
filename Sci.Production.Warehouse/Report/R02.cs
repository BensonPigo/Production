using System;
using System.Data;
using System.Windows.Forms;
using Sci.Data;
using Ict;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;

namespace Sci.Production.Warehouse
{
    public partial class R02 : Sci.Win.Tems.PrintForm
    {
        DataTable dt;
        DateTime? strIssueDate1;
        DateTime? strIssueDate2;
        string strM;
        string strFactory;

        public R02(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.EditMode = true;
        }

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
                // return MyUtility.Excel.CopyToXls(dt, "", "Warehouse_R02.xltx", 1,true);
                Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Warehouse_R02.xltx"); // 預先開啟excel app
                MyUtility.Excel.CopyToXls(this.dt, string.Empty, "Warehouse_R02.xltx", 1, showExcel: false, showSaveMsg: false, excelApp: objApp);

                this.ShowWaitMessage("Excel Processing...");
                Excel.Worksheet worksheet = objApp.Sheets[1];
                for (int i = 1; i <= this.dt.Rows.Count; i++)
                {
                    string str = worksheet.Cells[i + 1, 9].Value;
                    if (!MyUtility.Check.Empty(str))
                    {
                        worksheet.Cells[i + 1, 9] = str.Trim();
                    }
                }

                #region Save & Show Excel
                string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Warehouse_R02");
                objApp.ActiveWorkbook.SaveAs(strExcelName);
                objApp.Quit();
                Marshal.ReleaseComObject(objApp);
                Marshal.ReleaseComObject(worksheet);

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

        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            // return base.OnAsyncDataLoad(e);
            DualResult result = Result.True;

            try
            {
                string sqlFilter = string.Empty;
                if (!MyUtility.Check.Empty(this.strIssueDate1))
                {
                    sqlFilter += string.Format(" and '{0}' <= a.issuedate ", Convert.ToDateTime(this.strIssueDate1).ToString("d"));
                }

                if (!MyUtility.Check.Empty(this.strIssueDate2))
                {
                    sqlFilter += string.Format(" and a.issuedate <= '{0}'", Convert.ToDateTime(this.strIssueDate2).ToString("d"));
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
		,description = dbo.getmtldesc(b.FromPOID,b.FromSeq1,b.FromSeq2,2,0)
from dbo.SubTransfer a WITH (NOLOCK) 
inner join dbo.SubTransfer_Detail b WITH (NOLOCK) on a.id = b.id
inner join dbo.Orders on b.fromPoid = Orders.ID
where a.Status = 'Confirmed' and a.type='D' {0}
group by a.MDivisionID, Orders.FactoryID, a.issuedate, b.FromPOID, b.FromSeq1, b.FromSeq2
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
