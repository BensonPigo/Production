using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Warehouse
{
    public partial class R13 : Win.Tems.PrintForm
    {
        private string reason;
        private string mdivision;
        private string factory;
        private string stocktype;
        private DateTime? issueDate1;
        private DateTime? issueDate2;
        private DataTable printData;

        public R13(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.txtMdivision.Text = Env.User.Keyword;
            MyUtility.Tool.SetupCombox(this.comboStockType, 2, 1, ",ALL,A,Bulk,B,Inventory,O,Scrap");
            this.comboStockType.SelectedIndex = 0;
            this.txtReason.SelectedIndex = 0;
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(this.dateAdjustDate.Value1) && MyUtility.Check.Empty(this.dateAdjustDate.Value2))
            {
                MyUtility.Msg.WarningBox("< Adjust Date > can't be empty!!");
                return false;
            }

            this.issueDate1 = this.dateAdjustDate.Value1;
            this.issueDate2 = this.dateAdjustDate.Value2;
            this.mdivision = this.txtMdivision.Text;
            this.factory = this.txtfactory.Text;
            this.stocktype = this.comboStockType.SelectedValue.ToString();
            this.reason = this.txtReason.SelectedValue.ToString();

            return base.ValidateInput();
        }

        // 非同步取資料
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
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
            sqlCmd.Append(@"
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
                                                           , iif(b.stocktype ='O','Scrap'
                                                           , b.stocktype)))
        , b.QtyBefore
        , b.QtyAfter
        , reasonNm = 
			iif(a.type='O' , b.ReasonId+'-'+(select Reason.Name from Reason WITH (NOLOCK) where Reason.ReasonTypeID='Stock_Adjust' and Reason.id= b.ReasonId) ,
			iif(a.type='R' , b.ReasonId+'-'+(select Reason.Name from Reason WITH (NOLOCK) where Reason.ReasonTypeID='Stock_Remove' and Reason.id= b.ReasonId),
		 b.ReasonId+'-'+(select Reason.Name from Reason WITH (NOLOCK) where Reason.ReasonTypeID='Stock_Adjust' and Reason.id= b.ReasonId) ))
        , editor = dbo.getPass1(a.EditName) 
        , a.editdate
FROM adjust a WITH (NOLOCK) 
inner join adjust_detail b WITH (NOLOCK) on a.id = b.id
inner join Orders orders on b.POID = orders.ID
inner join po_supp_detail c WITH (NOLOCK) on c.ID = b.poid and c.seq1 = b.Seq1 and c.SEQ2 = b.Seq2
Where a.Status = 'Confirmed' ");
            if (this.stocktype == "O")
            {
                sqlCmd.Append(@" and a.type in ('O','R') ");
            }

            if (!MyUtility.Check.Empty(this.stocktype) && this.stocktype != "O")
            {
                sqlCmd.Append(string.Format(@" and a.type='{0}' ", this.stocktype));
            }
            else
            {
                sqlCmd.Append(@" and a.type in ('A','B','O','R') ");
            }

            if (!MyUtility.Check.Empty(this.issueDate1))
            {
                sqlCmd.Append(string.Format(" and '{0}' <= a.issuedate", Convert.ToDateTime(this.issueDate1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.issueDate2))
            {
                sqlCmd.Append(string.Format(" and a.issuedate <= '{0}'", Convert.ToDateTime(this.issueDate2).ToString("d")));
            }

            #region --- 條件組合  ---
            if (!MyUtility.Check.Empty(this.mdivision))
            {
                sqlCmd.Append(" and a.mdivisionid = @MDivision");
                sp_mdivision.Value = this.mdivision;
                cmds.Add(sp_mdivision);
            }

            if (!MyUtility.Check.Empty(this.factory))
            {
                sqlCmd.Append(" and orders.FactoryId = @factory");
                sp_factory.Value = this.factory;
                cmds.Add(sp_factory);
            }

            if (!MyUtility.Check.Empty(this.reason))
            {
                sqlCmd.Append(string.Format(@" and b.reasonid = '{0}'", this.reason));
            }

            #endregion

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), cmds, out this.printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
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

            // MyUtility.Excel.CopyToXls(printData, "", "Warehouse_R13.xltx", 1);
            Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Warehouse_R13.xltx"); // 預先開啟excel app
            MyUtility.Excel.CopyToXls(this.printData, string.Empty, "Warehouse_R13.xltx", 1, showExcel: false, showSaveMsg: false, excelApp: objApp);      // 將datatable copy to excel
            Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表

            this.ShowWaitMessage("Excel Processing...");
            for (int i = 1; i <= this.printData.Rows.Count; i++)
            {
                string str = objSheets.Cells[i + 1, 10].Value;
                if (!MyUtility.Check.Empty(str))
                {
                    objSheets.Cells[i + 1, 10] = str.Trim();
                }
            }

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("Warehouse_R13");
            objApp.ActiveWorkbook.SaveAs(strExcelName);
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);
            Marshal.ReleaseComObject(objSheets);

            strExcelName.OpenFile();
            #endregion
            this.HideWaitMessage();
            return true;
        }
    }
}
