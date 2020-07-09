using System;
using System.Data;
using System.Windows.Forms;
using Ict;
using Sci.Win;
using Sci.Data;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;

namespace Sci.Production.Warehouse
{
    public partial class R07 : Win.Tems.PrintForm
    {
        DataTable dt;
        DateTime? strIssueDate1;
        DateTime? strIssueDate2;
        string strM;
        string strFactory;

        public R07(ToolStripMenuItem menuitem)
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

        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            DualResult result = Ict.Result.True;

            try
            {
                string sqlFilter = string.Empty;
                if (!MyUtility.Check.Empty(this.strIssueDate1))
                {
                    sqlFilter += string.IsNullOrEmpty(sqlFilter) ? string.Format(" s.IssueDate >= '{0}' ", Convert.ToDateTime(this.strIssueDate1).ToString("yyyyMMdd")) : string.Format(" and s.IssueDate >= '{0}' ", Convert.ToDateTime(this.strIssueDate1).ToString("yyyyMMdd"));
                }

                if (!MyUtility.Check.Empty(this.strIssueDate2))
                {
                    sqlFilter += string.IsNullOrEmpty(sqlFilter) ? string.Format(" s.IssueDate <= '{0}'", Convert.ToDateTime(this.strIssueDate2).ToString("yyyyMMdd")) : string.Format(" and s.IssueDate <= '{0}'", Convert.ToDateTime(this.strIssueDate2).ToString("yyyyMMdd"));
                }

                if (!MyUtility.Check.Empty(this.strM))
                {
                    sqlFilter += string.Format(" and o.MDivisionID = '{0}'", this.strM);
                }

                if (!MyUtility.Check.Empty(this.strFactory))
                {
                    sqlFilter += string.Format(" and o.FtyGroup = '{0}'", this.strFactory);
                }

                string sqlcmd = string.Format(
                    @"
select s.Id
	,o.MDivisionID
	,o.FactoryID
	,s.IssueDate
	,sd.FromPOID
	,[InventorySeq] = CONCAT(LTRIM(RTRIM(sd.FromSeq1)),' ',LTRIM(RTRIM(sd.FromSeq2)))
	,sd.FromRoll
	,sd.FromDyelot
	,[Description] = ltrim(rtrim(dbo.getMtlDesc(sd.FromPOID, sd.FromSeq1, sd.FromSeq2, 2, 0)))
	,po.StockUnit
	,c.Name
	,sd.Qty
	,sd.ToPOID
	,[BulkSeq] = CONCAT(LTRIM(RTRIM(sd.ToSeq1)),' ',LTRIM(RTRIM(sd.ToSeq2)))
	,[CreateBy] = (select dbo.getPass1_ExtNo(s.AddName))
	,[FromLocation] = dbo.Getlocation(fty.Ukey)
	,sd.ToLocation
from SubTransfer s with (nolock)
inner join SubTransfer_Detail sd with (nolock) on s.Id = sd.ID
inner join Orders o with (nolock) on sd.ToPOID = o.ID
left join PO_Supp_Detail po with (nolock) on sd.FromPOID = po.ID 
										and sd.FromSeq1 = po.SEQ1 
										and sd.FromSeq2 = po.SEQ2
left join Color c with (nolock) on po.ColorID = c.ID and po.BrandId = c.BrandId
left join FtyInventory fty with (nolock) on sd.FromPOID = fty.POID 
											and sd.FromSeq1 = fty.Seq1 
											and sd.FromSeq2 = fty.Seq2 
											and sd.FromStockType = fty.StockType
											and sd.FromRoll = fty.Roll
											and sd.FromDyelot = fty.Dyelot 
where {0}
and s.Status = 'Confirmed'
and s.Type = 'B'
order by s.IssueDate,s.Id,o.MDivisionID,o.FactoryID,sd.FromPOID,sd.FromSeq1,sd.FromSeq2,sd.FromRoll,sd.FromDyelot
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

        protected override bool OnToExcel(ReportDefinition report)
        {
            this.SetCount(this.dt.Rows.Count);
            if (this.dt.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found!!");
                return false;
            }

            try
            {
                Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Warehouse_R07.xltx"); // 預先開啟excel app
                MyUtility.Excel.CopyToXls(this.dt, string.Empty, "Warehouse_R07.xltx", 1, showExcel: false, showSaveMsg: false, excelApp: objApp);

                this.ShowWaitMessage("Excel Processing...");

                #region Save & Show Excel
                string strExcelName = Class.MicrosoftFile.GetName("Warehouse_R07");
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
    }
}
