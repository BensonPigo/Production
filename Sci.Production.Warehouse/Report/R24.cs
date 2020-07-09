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
    public partial class R24 : Win.Tems.PrintForm
    {
        DataTable dt;
        DateTime? strIssueDate1;
        DateTime? strIssueDate2;
        string strM;
        string strFactory;

        public R24(ToolStripMenuItem menuitem)
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
                    sqlFilter += string.Format(" and '{0}' <= a.issuedate ", Convert.ToDateTime(this.strIssueDate1).ToString("d"));
                }

                if (!MyUtility.Check.Empty(this.strIssueDate2))
                {
                    sqlFilter += string.Format(" and a.issuedate <= '{0}'", Convert.ToDateTime(this.strIssueDate2).ToString("d"));
                }

                if (!MyUtility.Check.Empty(this.strM))
                {
                    sqlFilter += string.Format(" and Orders.MDivisionID = '{0}'", this.strM);
                }

                if (!MyUtility.Check.Empty(this.strFactory))
                {
                    sqlFilter += string.Format(" and Orders.FtyGroup = '{0}'", this.strFactory);
                }

                string sqlcmd = string.Format(
                    @"
select 	Orders.MDivisionID
        ,Orders.FtyGroup
        ,a.issuedate
		,poid = b.frompoid 
		,[Seq] = CONCAT(LTRIM(RTRIM(b.FromSeq1)),' ',LTRIM(RTRIM(b.FromSeq2)))
		,b.FromRoll
		,b.FromDyelot
		,[Description] = dbo.getMtlDesc(b.FromPOID,b.FromSeq1,b.FromSeq2,2,0)
		,po3.StockUnit
		,c.Name
		,qty = sum(b.qty) 	
        ,[Created by] = (select dbo.getPass1_ExtNo(a.AddName) )
		,[FromLocation] = dbo.Getlocation(ft.Ukey)
		,[ToLocation] = b.ToLocation
from dbo.SubTransfer a WITH (NOLOCK) 
inner join dbo.SubTransfer_Detail b WITH (NOLOCK) on a.id = b.id
inner join dbo.Orders WITH (NOLOCK)  on b.fromPoid = Orders.ID
left join PO_Supp_Detail po3 WITH (NOLOCK) on b.FromPOID=po3.ID
	and b.FromSeq1=po3.SEQ1 and b.FromSeq2=po3.SEQ2
left join FtyInventory ft WITH(NOLOCK) on b.FromPOID=ft.POID
	and b.FromSeq1=ft.Seq1 and b.FromSeq2 = ft.Seq2 and b.FromRoll=ft.Roll
	and b.FromStockType=ft.StockType and b.FromDyelot = ft.Dyelot
left join Color c  WITH(NOLOCK) on c.ID=po3.ColorID and c.BrandId=orders.BrandID
where a.Status = 'Confirmed' and a.type='A' 
{0}
group by Orders.MDivisionID, Orders.FtyGroup, a.issuedate, b.FromPOID, b.FromSeq1, b.FromSeq2,b.FromRoll,b.FromDyelot,po3.StockUnit,c.Name,ft.Ukey,b.ToLocation ,a.AddName
order by Orders.MDivisionID, Orders.FtyGroup, a.issuedate, b.FromPOID, b.FromSeq1, b.FromSeq2
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
                Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Warehouse_R24.xltx"); // 預先開啟excel app
                MyUtility.Excel.CopyToXls(this.dt, string.Empty, "Warehouse_R24.xltx", 1, showExcel: false, showSaveMsg: false, excelApp: objApp);

                this.ShowWaitMessage("Excel Processing...");
                Excel.Worksheet worksheet = objApp.Sheets[1];
                for (int i = 1; i <= this.dt.Rows.Count; i++)
                {
                    string str = worksheet.Cells[i + 1, 8].Value;
                    if (!MyUtility.Check.Empty(str))
                    {
                        worksheet.Cells[i + 1, 8] = str.Trim();
                    }

                    string str2 = worksheet.Cells[i + 1, 10].Value;
                    if (!MyUtility.Check.Empty(str2))
                    {
                        worksheet.Cells[i + 1, 10] = str2.Trim();
                    }
                }

                #region Save & Show Excel
                string strExcelName = Class.MicrosoftFile.GetName("Warehouse_R24");
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
    }
}
