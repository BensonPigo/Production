using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    public partial class R05 : Sci.Win.Tems.PrintForm
    {
        public R05(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            txtMdivision1.Text = Sci.Env.User.Keyword;
        }

        DateTime? Issuedate1, Issuedate2;
        string SP1, SP2, mdivisionid, factory;
        int type;
        DataTable printData;

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            Issuedate1 = dateIssue.Value1;
            Issuedate2 = dateIssue.Value2;
            SP1 = txtSP1.Text;
            SP2 = txtSP2.Text;
            mdivisionid = txtMdivision1.Text;
            factory = txtfactory1.Text;
            type = radioTransferIn.Checked ? 0 : 1;
            return base.ValidateInput();
        }
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            if (type == 0)
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
                if (!MyUtility.Check.Empty(Issuedate1))
                {
                    sqlCmd.Append(string.Format(@" and t.IssueDate between '{0}' and '{1}'", Issuedate1.Value.ToShortDateString(), Issuedate2.Value.ToShortDateString()));
                }
                if (!MyUtility.Check.Empty(SP1))
                {
                    sqlCmd.Append(string.Format(@" and td.POID >= '{0}'", SP1));
                }
                if (!MyUtility.Check.Empty(SP2))
                {
                    sqlCmd.Append(string.Format(@" and td.POID <='{0}'", SP2));
                }
                if (!MyUtility.Check.Empty(mdivisionid))
                {
                    sqlCmd.Append(string.Format(@" and t.MDivisionID = '{0}'", mdivisionid));
                }
                if (!MyUtility.Check.Empty(factory))
                {
                    sqlCmd.Append(string.Format(@" and t.FactoryID = '{0}'", factory));
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
                if (!MyUtility.Check.Empty(Issuedate1))
                {
                    sqlCmd.Append(string.Format(@" and t.IssueDate between '{0}' and '{1}'", Issuedate1.Value.ToShortDateString(), Issuedate2.Value.ToShortDateString()));
                }
                if (!MyUtility.Check.Empty(SP1))
                {
                    sqlCmd.Append(string.Format(@" and td.POID >= '{0}'", SP1));
                }
                if (!MyUtility.Check.Empty(SP2))
                {
                    sqlCmd.Append(string.Format(@" and td.POID <='{1}'", SP2));
                }
                if (!MyUtility.Check.Empty(mdivisionid))
                {
                    sqlCmd.Append(string.Format(@" and t.MDivisionID = '{0}'", mdivisionid));
                }
                if (!MyUtility.Check.Empty(factory))
                {
                    sqlCmd.Append(string.Format(@" and t.FactoryID = '{0}'", factory));
                }
            }

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out printData);
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
            SetCount(printData.Rows.Count);

            if (printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }
            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Warehouse_R05.xltx"); //預先開啟excel app            
            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
            objSheets.Cells[1, 5] = type == 0 ? "Arrive W/H Date" : "Issue Date";
            MyUtility.Excel.CopyToXls(printData, "", "Warehouse_R05.xltx", 1, true, null, objApp);      // 將datatable copy to excel
            Marshal.ReleaseComObject(objSheets);
            return true;
        }
    }
}
