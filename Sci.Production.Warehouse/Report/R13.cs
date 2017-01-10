using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Warehouse
{
    public partial class R13 : Sci.Win.Tems.PrintForm
    {
        string reason,  mdivision, stocktype ;
        DateTime? issueDate1, issueDate2;
        DataTable printData;

        public R13(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            txtMdivision1.Text = Sci.Env.User.Keyword;
            MyUtility.Tool.SetupCombox(cbbStockType,2,1, "A,Bulk,B,Inventory");
            cbbStockType.SelectedIndex = 0;
            txtReason1.SelectedIndex = 0;
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(dateRange1.Value1) ) 
            {
                MyUtility.Msg.WarningBox("< Adjust Date > can't be empty!!");
                return false;
            }

            issueDate1 = dateRange1.Value1;
            issueDate2 = dateRange1.Value2;
            mdivision = txtMdivision1.Text;
            stocktype = cbbStockType.SelectedValue.ToString();
            reason = txtReason1.SelectedValue.ToString();

            return base.ValidateInput();
        }

        // 非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            #region -- sql parameters declare --

            System.Data.SqlClient.SqlParameter sp_mdivision = new System.Data.SqlClient.SqlParameter();
            sp_mdivision.ParameterName = "@MDivision";

            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
            #endregion

            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(string.Format(@"SELECT a.MDivisionID,a.id, a.IssueDate, b.POID, b.seq1,b.seq2,b.Roll,b.Dyelot
, c.Refno, dbo.getMtlDesc(b.poid,b.seq1,b.seq2,2,0)[description]
,iif(b.StockType='B','Bulk',iif(b.stocktype ='I','Inventory',b.stocktype)) stock
,b.QtyBefore,b.QtyAfter
,b.ReasonId+'-'+(select Reason.Name from Reason where Reason.ReasonTypeID='Stock_Adjust' and Reason.id= b.ReasonId) reasonNm
,dbo.getPass1(a.EditName) editor
,a.editdate
FROM adjust a
inner join adjust_detail b on a.id = b.id
inner join po_supp_detail c on c.ID = b.poid and c.seq1 = b.Seq1 and c.SEQ2 = b.Seq2
Where a.Status = 'Confirmed' and a.issuedate between '{0}' and '{1}' and a.type = '{2}'
", Convert.ToDateTime(issueDate1).ToString("d")
 , Convert.ToDateTime(issueDate2).ToString("d")
 , stocktype));

            #region --- 條件組合  ---
            if (!MyUtility.Check.Empty(mdivision))
            {
                sqlCmd.Append(" and a.mdivisionid = @MDivision");
                sp_mdivision.Value = mdivision;
                cmds.Add(sp_mdivision);
            }

            if (!MyUtility.Check.Empty(reason))
            {
                sqlCmd.Append(string.Format(@" and b.reasonid = '{0}'",reason));
            }

            #endregion

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), cmds, out printData);
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

            //MyUtility.Excel.CopyToXls(printData, "", "Warehouse_R13.xltx", 1);
            Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Warehouse_R13.xltx"); //預先開啟excel app
            MyUtility.Excel.CopyToXls(printData, "", "Warehouse_R13.xltx", 1, showExcel: false, showSaveMsg: true, excelApp: objApp);      // 將datatable copy to excel
            Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表

            this.ShowWaitMessage("Excel Processing...");
            for (int i = 1; i <= printData.Rows.Count; i++) objSheets.Cells[i + 1, 10] = ((string)((Excel.Range)objSheets.Cells[i + 1, 10]).Value).Trim();
            objApp.Visible = true;

            if (objSheets != null) Marshal.FinalReleaseComObject(objSheets);    //釋放sheet
            if (objApp != null) Marshal.FinalReleaseComObject(objApp);          //釋放objApp
            this.HideWaitMessage();
            return true;
        }
    }
}
