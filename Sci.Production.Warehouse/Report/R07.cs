using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Win;
using Sci.Data;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;

namespace Sci.Production.Warehouse
{
    public partial class R07 : Sci.Win.Tems.PrintForm
    {
        DataTable dt;
        DateTime? strIssueDate1, strIssueDate2;
        string strM, strFactory;
        public R07(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.EditMode = true;
        }

        protected override bool ValidateInput()
        {
            strIssueDate1 = dateIssueDate.Value1;
            strIssueDate2 = dateIssueDate.Value2;
            strM = txtMdivision.Text;
            strFactory = txtfactory.Text;

            if (MyUtility.Check.Empty(strIssueDate1) && MyUtility.Check.Empty(strIssueDate2))
            {
                MyUtility.Msg.WarningBox("Issue date can't be empty!!");
                return false;
            }
            return base.ValidateInput();
        }

        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            DualResult result = Result.True;

            try
            {
                string sqlFilter = string.Empty;
                if (!MyUtility.Check.Empty(strIssueDate1))
                    sqlFilter += string.IsNullOrEmpty(sqlFilter) ? string.Format(" s.IssueDate >= '{0}' ", Convert.ToDateTime(strIssueDate1).ToString("yyyyMMdd")) : string.Format(" and s.IssueDate >= '{0}' ", Convert.ToDateTime(strIssueDate1).ToString("yyyyMMdd"));
                if (!MyUtility.Check.Empty(strIssueDate2))
                    sqlFilter += string.IsNullOrEmpty(sqlFilter) ? string.Format(" s.IssueDate <= '{0}'", Convert.ToDateTime(strIssueDate2).ToString("yyyyMMdd")) : string.Format(" and s.IssueDate <= '{0}'", Convert.ToDateTime(strIssueDate2).ToString("yyyyMMdd"));
                if (!MyUtility.Check.Empty(strM))
                    sqlFilter += string.Format(" and o.MDivisionID = '{0}'", strM);
                if (!MyUtility.Check.Empty(strFactory))
                    sqlFilter += string.Format(" and o.FtyGroup = '{0}'", strFactory);

                string sqlcmd = string.Format(@"
select s.Id
	,o.MDivisionID
	,o.FactoryID
	,s.IssueDate
	,sd.FromPOID
	,[InventorySeq] = sd.FromSeq1 + sd.FromSeq2
	,sd.FromRoll
	,sd.FromDyelot
	,[Description] = ltrim(rtrim(dbo.getMtlDesc(sd.FromPOID, sd.FromSeq1, sd.FromSeq2, 2, 0)))
	,po.StockUnit
	,c.Name
	,sd.Qty
	,sd.ToPOID
	,[BulkSeq] = sd.ToSeq1 + sd.ToSeq2
	,[CreateBy] = s.AddName
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
											and sd.FromRoll = fty.Roll
											and sd.FromDyelot = fty.Dyelot 
											and sd.FromStockType = fty.StockType
where {0}
and s.Status = 'Confirmed'
and s.Type = 'B'
", sqlFilter);
                result = DBProxy.Current.Select(null, sqlcmd, out dt);
                if (!result) return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        protected override bool OnToExcel(ReportDefinition report)
        {
            SetCount(dt.Rows.Count);
            if (dt.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found!!");
                return false;
            }

            try
            {
                Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Warehouse_R07.xltx"); //預先開啟excel app
                MyUtility.Excel.CopyToXls(dt, "", "Warehouse_R07.xltx", 1, showExcel: false, showSaveMsg: false, excelApp: objApp);

                this.ShowWaitMessage("Excel Processing...");

                #region Save & Show Excel
                string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Warehouse_R07");
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
