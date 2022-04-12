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
    public partial class R12 : Win.Tems.PrintForm
    {
        private string sqlCmd = string.Empty;
        private List<SqlParameter> listSqlPar = new List<SqlParameter>();
        private DataTable dtResult;

        public R12(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.EditMode = true;
            this.cbFabricType.SelectedIndex = 0;
            this.cbRequestType.SelectedIndex = 0;
        }

        /// <inheritdoc/>
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
	 IssueID = il.Id
	,o.MDivisionID
	,o.FactoryID
	,il.RequestID
	,il.IssueDate
	,RequestType= IIF(il.Type = 'L', 'Lacking', 'Replacement')
	,Shift = case	when l.Shift = 'D' then 'Day'
					when l.Shift = 'N' then 'Night'
					when l.Shift = 'O' then 'Subcon-Out'
					else '' end
	,l.SubconName
	,il.Remark
	,il.ApvDate
	,il.Status
	,Createby = dbo.getPass1_ExtNo(il.AddName)
	,SP = ild.POID
	,ild.Seq1
	,ild.Seq2
	,ild.Roll
	,ild.Dyelot
	,Description = dbo.getMtlDesc(ild.poid, ild.seq1, ild.seq2, 2, 0)
	,Unit = psd.StockUnit
	, psd.RefNo
	, psd.ColorID
	, psd.SizeSpec
	,MaterialType = IIF(il.FabricType = 'F', 'Fabric', 'Accessory')
	,IssueQty = ild.Qty
	,BulkLocation = dbo.Getlocation(f.Ukey)
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

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            DualResult result = DBProxy.Current.Select(null, this.sqlCmd, this.listSqlPar, out this.dtResult);
            return result;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.dtResult.Rows.Count);

            if (this.dtResult.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            bool s = MyUtility.Excel.CopyToXls(this.dtResult, string.Empty, "Warehouse_R12.xltx", 1, true, null, null);
            return true;
        }
    }
}
