using System;
using System.Data;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Warehouse
{
    public partial class R16 : Win.Tems.PrintForm
    {
        private string mdivision;
        private string factory;
        private string request1;
        private string request2;
        private DateTime? issueDate1;
        private DateTime? issueDate2;
        private DataTable printData;

        public R16(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.txtMdivision.Text = Env.User.Keyword;
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(this.dateIssueDate.Value1) && MyUtility.Check.Empty(this.dateIssueDate.Value2))
            {
                MyUtility.Msg.WarningBox("Issue date can't be empty!!");
                return false;
            }

            this.issueDate1 = this.dateIssueDate.Value1;
            this.issueDate2 = this.dateIssueDate.Value2;
            this.mdivision = this.txtMdivision.Text;
            this.factory = this.txtfactory.Text;
            this.request1 = this.txtRquest1.Text;
            this.request2 = this.txtRquest2.Text;

            return base.ValidateInput();
        }

        // 非同步取資料
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            #region where
            string where = string.Empty;
            if (!MyUtility.Check.Empty(this.issueDate1))
            {
                where += $@" and i.IssueDate >= '{((DateTime)this.issueDate1).ToString("yyyy/MM/dd")}'";
            }

            if (!MyUtility.Check.Empty(this.issueDate2))
            {
                where += $@" and i.IssueDate <= '{((DateTime)this.issueDate2).ToString("yyyy/MM/dd")}'";
            }

            if (!MyUtility.Check.Empty(this.mdivision))
            {
                where += $@" and o.MDivisionID ='{this.mdivision}' ";
            }

            if (!MyUtility.Check.Empty(this.factory))
            {
                where += $@" and o.FactoryID ='{this.factory}' ";
            }

            if (!MyUtility.Check.Empty(this.request1))
            {
                where += $@" and i.cutplanID >='{this.request1}' ";
            }

            if (!MyUtility.Check.Empty(this.request2))
            {
                where += $@" and i.cutplanID <='{this.request2}' ";
            }

            #endregion

            string sqlcmd = $@"
select
	i.Id
	,o.MDivisionID 
	,o.FactoryID
	,i.CutplanID
	,i.IssueDate
	,line=(
		select stuff((
			select concat(',',t.SewLine)
			from (
				select distinct o.SewLine 
				from orders o WITH (NOLOCK)
				where c.Poid = o.POID and o.sewline !=''
			) t
			for xml path('')
		),1,1,'')
	)
	,c.CutCellID
	,x2.cutref
	,i.Remark
	,SP=id.POID
	,Seq=CONCAT(LTRIM(RTRIM(id.Seq1)),' ',LTRIM(RTRIM(id.Seq2)))
	,psd.Refno
	,psd.ColorID
	,DescDetail=LTRIM(RTRIM(f.DescDetail))
	,id.roll
	,id.dyelot
	,psd.StockUnit
	,id.Qty
	,BulkLocation=dbo.Getlocation(fi.Ukey)
	,Createby=dbo.getPass1_ExtNo(i.AddName)
from issue i WITH (NOLOCK) 
inner join issue_detail id WITH (NOLOCK) on i.id = id.id
inner join Orders o WITH (NOLOCK) on id.POID = o.id
inner join Cutplan c WITH (NOLOCK) on c.ID = i.CutplanID
left join po_supp_detail psd WITH (NOLOCK) on psd.id = id.poid and psd.seq1 = id.seq1 and psd.seq2 =id.seq2
left join Fabric f WITH (NOLOCK) on f.SCIRefno  = psd.SCIRefno
left join FtyInventory fi WITH (NOLOCK) on fi.POID = id.POID and fi.Seq1 = id.Seq1 and fi.Seq2 = id.Seq2 and fi.Roll = id.Roll and fi.Dyelot = id.Dyelot and id.StockType = fi.StockType
outer apply(
	select cutref = stuff((
		Select concat(' / ', w.FabricCombo,'-',x1.CutNo)
		from Cutplan_Detail cd WITH (NOLOCK)
		inner join workorder w WITH (NOLOCK) on w.Ukey = cd.WorkorderUkey 
		outer apply(
			select CutNo=stuff((
				select concat(',',cd2.CutNo)
				from Cutplan_Detail cd2 WITH (NOLOCK)
				inner join workorder w2 WITH (NOLOCK) on w2.Ukey = cd2.WorkorderUkey 
				where cd2.ID=i.CutplanID and w2.FabricCombo=w.FabricCombo
				group by cd2.CutNo
                order by cd2.CutNo
				for xml path('')
			),1,1,'')
		)x1
		where cd.ID=i.CutplanID
		group by w.FabricCombo,x1.CutNo
        order by w.FabricCombo,x1.CutNo
		for xml path('')
	),1,3,'')
)x2
where 1=1
And i.type = 'A' 
AND i.Status = 'Confirmed' 
{where}

order by IssueDate, ID, SP, Seq, Roll, Dyelot
";

            DualResult result = DBProxy.Current.Select(null, sqlcmd, null, out this.printData);
            if (!result)
            {
                return result;
            }

            foreach (DataRow row in this.printData.Rows)
            {
                row["DescDetail"] = MyUtility.Convert.GetString(row["DescDetail"]).Trim();
            }

            return Ict.Result.True;
        }

        // 產生Excel
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.printData.Rows.Count);

            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            this.ShowWaitMessage("Starting EXCEL...");
            string reportName = "Warehouse_R16.xltx";
            Excel.Application excelApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + $"\\{reportName}");
            MyUtility.Excel.CopyToXls(this.printData, string.Empty, reportName, 1, false, null, excelApp, wSheet: excelApp.Sheets[1]);

            #region 釋放上面開啟過excel物件
            string strExcelName = Class.MicrosoftFile.GetName("Shipping_R16");
            Excel.Workbook workbook = excelApp.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            excelApp.Quit();

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
