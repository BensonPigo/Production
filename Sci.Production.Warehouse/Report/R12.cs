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
    public partial class R12 : Sci.Win.Tems.PrintForm
    {
        string sqlCmd = string.Empty;
        List<SqlParameter> listSqlPar = new List<SqlParameter>();
        DataTable dtResult;

        public R12(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.EditMode = true;
            this.cbFabricType.SelectedIndex = 0;
            this.cbRequestType.SelectedIndex = 0;
        }

        protected override bool ValidateInput()
        {
            if (!this.dateRangeIssueDate.HasValue)
            {
                MyUtility.Msg.WarningBox("Issue date can't be empty!!");
                return false;
            }

            this.listSqlPar.Clear();
            string sqlWhere = string.Empty;

            if (this.dateRangeIssueDate.HasValue1)
            {
                sqlWhere += " and il.IssueDate >= @FromIssueDate ";
                this.listSqlPar.Add(new SqlParameter("@FromIssueDate", this.dateRangeIssueDate.DateBox1.Value));
            }

            if (this.dateRangeIssueDate.HasValue2)
            {
                sqlWhere += " and il.IssueDate <= @ToIssueDate ";
                this.listSqlPar.Add(new SqlParameter("@ToIssueDate", this.dateRangeIssueDate.DateBox2.Value));
            }

            if (this.cbFabricType.Text != "All")
            {
                string decodeFabricType = this.cbFabricType.Text == "Fabric" ? "F" : "A";
                sqlWhere += $" and il.FabricType = '{decodeFabricType}'";
            }

            if (this.cbRequestType.Text != "All")
            {
                string decodeRequestType = this.cbRequestType.Text == "Lacking" ? "L" : "R";
                sqlWhere += $" and il.Type = '{decodeRequestType}'";
            }

            if (!MyUtility.Check.Empty(this.txtMdivision.Text))
            {
                sqlWhere += $" and o.MDivisionID = '{this.txtMdivision.Text}'";
            }

            if (!MyUtility.Check.Empty(this.txtfactory.Text))
            {
                sqlWhere += $" and o.FactoryID = '{this.txtfactory.Text}'";
            }

            this.sqlCmd = $@"
select 
il.Id,
o.MDivisionID,
o.FactoryID,
il.RequestID,
il.IssueDate,
[FabricType] = IIF(il.FabricType = 'F', 'Fabric', 'Accessory'),
[RequestType] = IIF(il.Type = 'L', 'Lacking', 'Replacement'),
[Shift] = case	when l.Shift = 'D' then 'Day'
				when l.Shift = 'N' then 'Night'
				when l.Shift = 'O' then 'Subcon-Out'
				else '' end,
l.SubconName,
il.ApvDate,
il.Status,
il.Remark,
ild.POID,
[Seq] = CONCAT(LTRIM(RTRIM(ild.Seq1)),' ',LTRIM(RTRIM(ild.Seq2))),
ild.Roll,
ild.Dyelot,
[Description] = dbo.getMtlDesc(ild.poid, ild.seq1, ild.seq2, 2, 0),
psd.StockUnit,
ild.Qty,
[BulkLocation] = dbo.Getlocation(f.Ukey),
[CreateBy] = dbo.getPass1_ExtNo(il.AddName)
from IssueLack il with (nolock)
inner join IssueLack_Detail ild with (nolock) on il.ID = ild.ID
inner join Orders o with (nolock) on o.ID = ild.poid
left join PO_Supp_Detail psd with (nolock) on psd.ID = ild.POID and psd.SEQ1 = ild.Seq1 and psd.SEQ2 = ild.Seq2 
left join FtyInventory f with (nolock) on f.POID = ild.POID and f.SEQ1 = ild.Seq1 and f.SEQ2 = ild.Seq2  and f.Roll = ild.Roll and f.Dyelot = ild.Dyelot
left join Lack l with (nolock) on l.ID = il.RequestID
where il.Status <> 'New'
{sqlWhere}
order by il.IssueDate,il.Id,ild.POID,ild.Seq1,ild.Seq2,ild.Roll,ild.Dyelot
";
            return true;
        }

        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            DualResult result = DBProxy.Current.Select(null, this.sqlCmd, this.listSqlPar, out this.dtResult);
            return result;
        }

        protected override bool OnToExcel(ReportDefinition report)
        {
            this.SetCount(this.dtResult.Rows.Count);
            if (this.dtResult.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found!!");
                return true;
            }

            this.ShowWaitMessage("Excel Processing...");
            Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Warehouse_R12.xltx"); // 預先開啟excel app
            Sci.Utility.Report.ExcelCOM com = new Sci.Utility.Report.ExcelCOM(Sci.Env.Cfg.XltPathDir + "\\Warehouse_R12.xltx", objApp);
            com.WriteTable(this.dtResult, 2);

            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Warehouse_R12");
            objApp.ActiveWorkbook.SaveAs(strExcelName);
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);

            strExcelName.OpenFile();

            this.HideWaitMessage();
            return true;
        }
    }
}
