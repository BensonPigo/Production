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
        string reason,  mdivision, factory, stocktype ;
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
            if (MyUtility.Check.Empty(dateRange1.Value1) && MyUtility.Check.Empty(dateRange1.Value2))
            {
                MyUtility.Msg.WarningBox("< Adjust Date > can't be empty!!");
                return false;
            }

            issueDate1 = dateRange1.Value1;
            issueDate2 = dateRange1.Value2;
            mdivision = txtMdivision1.Text;
            factory = txtfactory1.Text;
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

            System.Data.SqlClient.SqlParameter sp_factory = new System.Data.SqlClient.SqlParameter();
            sp_factory.ParameterName = "@factory";

            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
            #endregion

            string[] x = new string[3];

            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(string.Format(@"
SELECT  a.MDivisionID
        , orders.FactoryID
        , a.id
        , a.IssueDate
        , b.POID
        , b.seq1
        , b.seq2
        , b.Roll
        , b.Dyelot
        , c.Refno
        , [description] = dbo.getMtlDesc(b.poid,b.seq1,b.seq2,2,0)
        , stock = iif(b.StockType='B', 'Bulk'
                                     , iif(b.stocktype ='I','Inventory'
                                                           ,b.stocktype)) 
        , b.QtyBefore
        , b.QtyAfter
        , reasonNm = b.ReasonId+'-'+(select Reason.Name from Reason WITH (NOLOCK) where Reason.ReasonTypeID='Stock_Adjust' and Reason.id= b.ReasonId) 
        , editor = dbo.getPass1(a.EditName) 
        , a.editdate
FROM adjust a WITH (NOLOCK) 
inner join adjust_detail b WITH (NOLOCK) on a.id = b.id
inner join Orders orders on b.POID = orders.ID
inner join po_supp_detail c WITH (NOLOCK) on c.ID = b.poid and c.seq1 = b.Seq1 and c.SEQ2 = b.Seq2
Where a.Status = 'Confirmed' and a.type = '{0}'
", stocktype));
            if (!MyUtility.Check.Empty(issueDate1))
                sqlCmd.Append(string.Format(" and '{0}' <= a.issuedate", Convert.ToDateTime(issueDate1).ToString("d")));
            if(!MyUtility.Check.Empty(issueDate2))
                sqlCmd.Append(string.Format(" and a.issuedate <= '{0}'", Convert.ToDateTime(issueDate2).ToString("d")));

            #region --- 條件組合  ---
            if (!MyUtility.Check.Empty(mdivision))
            {
                sqlCmd.Append(" and a.mdivisionid = @MDivision");
                sp_mdivision.Value = mdivision;
                cmds.Add(sp_mdivision);
            }

            if (!MyUtility.Check.Empty(factory))
            {
                sqlCmd.Append(" and orders.FactoryId = @factory");
                sp_factory.Value = factory;
                cmds.Add(sp_factory);
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
            for (int i = 1; i <= printData.Rows.Count; i++)
            {
                string str = objSheets.Cells[i + 1, 10].Value;
                if(!MyUtility.Check.Empty(str))
                    objSheets.Cells[i + 1, 10] = str.Trim();
            }
                objApp.Visible = true;

            if (objSheets != null) Marshal.FinalReleaseComObject(objSheets);    //釋放sheet
            if (objApp != null) Marshal.FinalReleaseComObject(objApp);          //釋放objApp
            this.HideWaitMessage();
            return true;
        }
    }
}
