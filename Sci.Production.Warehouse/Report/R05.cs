using Ict;
using Sci.Data;
using System;
using System.Data;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    public partial class R05 : Win.Tems.PrintForm
    {
        public R05(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.txtMdivision1.Text = Sci.Env.User.Keyword;
        }

        DateTime? Issuedate1;
        DateTime? Issuedate2;
        string SP1;
        string SP2;
        string mdivisionid;
        string factory;
        int type;
        DataTable printData;

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            this.Issuedate1 = this.dateIssue.Value1;
            this.Issuedate2 = this.dateIssue.Value2;
            this.SP1 = this.txtSP1.Text;
            this.SP2 = this.txtSP2.Text;
            this.mdivisionid = this.txtMdivision1.Text;
            this.factory = this.txtfactory1.Text;
            this.type = this.radioTransferIn.Checked ? 0 : 1;
            return base.ValidateInput();
        }

        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            if (this.type == 0)
            {
                sqlCmd.Append(@"
select
	t.id,t.MDivisionID,t.FromFtyID,t.FactoryID,t.IssueDate,t.Status
	,td.POID,td.Seq1,td.Seq2,td.Roll,td.Dyelot
	,case when td.StockType = 'B' then 'Bulk'
	        when td.StockType = 'I' then 'Inventory'
	        end
	,td.Qty
	,Description = dbo.getMtlDesc(td.POID,td.seq1,td.seq2,2,0)
from TransferIn t with(nolock)
inner join TransferIn_Detail td with(nolock) on td.id = t.id
where 1=1
");
                if (!MyUtility.Check.Empty(this.Issuedate1))
                {
                    sqlCmd.Append(string.Format(@" and t.IssueDate between '{0}' and '{1}'", this.Issuedate1.Value.ToShortDateString(), this.Issuedate2.Value.ToShortDateString()));
                }

                if (!MyUtility.Check.Empty(this.SP1))
                {
                    sqlCmd.Append(string.Format(@" and td.POID >= '{0}'", this.SP1));
                }

                if (!MyUtility.Check.Empty(this.SP2))
                {
                    sqlCmd.Append(string.Format(@" and td.POID <='{0}'", this.SP2));
                }

                if (!MyUtility.Check.Empty(this.mdivisionid))
                {
                    sqlCmd.Append(string.Format(@" and t.MDivisionID = '{0}'", this.mdivisionid));
                }

                if (!MyUtility.Check.Empty(this.factory))
                {
                    sqlCmd.Append(string.Format(@" and t.FactoryID = '{0}'", this.factory));
                }
            }
            else
            {
                sqlCmd.Append(@"
select
	t.id,t.MDivisionID,t.FactoryID,t.ToMDivisionid,t.IssueDate,t.Status
	,td.POID,td.Seq1,td.Seq2,td.Roll,td.Dyelot
	,case when td.StockType = 'B' then 'Bulk'
	        when td.StockType = 'I' then 'Inventory'
	        end
	,td.Qty
	,Description = dbo.getMtlDesc(td.POID,td.seq1,td.seq2,2,0)
from TransferOut t with(nolock)
inner join TransferOut_Detail td with(nolock) on td.id = t.id
where 1=1
");
                if (!MyUtility.Check.Empty(this.Issuedate1))
                {
                    sqlCmd.Append(string.Format(@" and t.IssueDate between '{0}' and '{1}'", this.Issuedate1.Value.ToShortDateString(), this.Issuedate2.Value.ToShortDateString()));
                }

                if (!MyUtility.Check.Empty(this.SP1))
                {
                    sqlCmd.Append(string.Format(@" and td.POID >= '{0}'", this.SP1));
                }

                if (!MyUtility.Check.Empty(this.SP2))
                {
                    sqlCmd.Append(string.Format(@" and td.POID <='{1}'", this.SP2));
                }

                if (!MyUtility.Check.Empty(this.mdivisionid))
                {
                    sqlCmd.Append(string.Format(@" and t.MDivisionID = '{0}'", this.mdivisionid));
                }

                if (!MyUtility.Check.Empty(this.factory))
                {
                    sqlCmd.Append(string.Format(@" and t.FactoryID = '{0}'", this.factory));
                }
            }

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out this.printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return Result.True;
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

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Warehouse_R05.xltx"); // 預先開啟excel app
            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
            objSheets.Cells[1, 5] = this.type == 0 ? "Arrive W/H Date" : "Issue Date";
            MyUtility.Excel.CopyToXls(this.printData, string.Empty, "Warehouse_R05.xltx", 1, true, null, objApp);      // 將datatable copy to excel
            Marshal.ReleaseComObject(objSheets);
            return true;
        }
    }
}
