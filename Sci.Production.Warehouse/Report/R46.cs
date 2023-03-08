using Ict;
using Sci.Data;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class R46 : Win.Tems.PrintForm
    {
        private List<string> sqlWherelist;
        private List<SqlParameter> lisSqlParameter;
        private string strSQLWhere = string.Empty;
        private DataTable printTable;

        /// <inheritdoc/>
        public R46(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.strSQLWhere = string.Empty;
            this.sqlWherelist = new List<string>();
            this.lisSqlParameter = new List<SqlParameter>();

            if (MyUtility.Check.Empty(this.txtSP1.Text) &&
                MyUtility.Check.Empty(this.txtSP2.Text) &&
                MyUtility.Check.Empty(this.dateIssue.Value1) &&
                MyUtility.Check.Empty(this.dateIssue.Value2))
            {
                MyUtility.Msg.WarningBox("Issue Date and SP# can’t all be empty.");
                return false;
            }

            if (!MyUtility.Check.Empty(this.txtSP1.Text) &&
                !MyUtility.Check.Empty(this.txtSP2.Text))
            {
                this.sqlWherelist.Add("ad.POID >= @SP1 and ad.POID <= @SP2");
                this.lisSqlParameter.Add(new SqlParameter("@SP1", this.txtSP1.Text));
                this.lisSqlParameter.Add(new SqlParameter("@SP2", this.txtSP2.Text));
            }

            if (!MyUtility.Check.Empty(this.dateIssue.Value1) &&
                !MyUtility.Check.Empty(this.dateIssue.Value2))
            {
                this.sqlWherelist.Add("A.IssueDate between @dateTransfer1 and @dateTransfer2");
                this.lisSqlParameter.Add(new SqlParameter("@dateTransfer1", this.dateIssue.Value1));
                this.lisSqlParameter.Add(new SqlParameter("@dateTransfer2", this.dateIssue.Value2));
            }

            if (!MyUtility.Check.Empty(this.txtMdivision.Text))
            {
                this.sqlWherelist.Add("vo.MDivisionID = @MDivisionID");
                this.lisSqlParameter.Add(new SqlParameter("@MDivisionID", this.txtMdivision.Text));
            }

            if (!MyUtility.Check.Empty(this.txtfactory.Text))
            {
                this.sqlWherelist.Add("vo.FtyGroup = @FtyGroup");
                this.lisSqlParameter.Add(new SqlParameter("@FtyGroup", this.txtfactory.Text));
            }

            this.strSQLWhere = string.Join(" and ", this.sqlWherelist);

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            string sqlcmd = $@"
select [Dispose ID] = a.ID
,[Dispose Dae] = a.IssueDate
,[M] = vo.MDivisionID
,[Factory] = vo.FactoryID
,[SP#] = ad.POID
,[Seq] = CONCAT(ad.Seq1,' ',ad.Seq2)
,[Roll] = ad.Roll
,[Dyelot] = ad.Dyelot
,[Ref#] = psd.Refno
,[Color] = IIF(f.MtlTypeID = 'EMB THREAD' or f.MtlTypeID = 'SP THREAD' or f.MtlTypeID = 'THREAD', IIF(psd.SuppColor = '' or psd.SuppColor is null, dbo.GetColorMultipleID(vo.BrandID, psd.ColorID), psd.SuppColor), dbo.GetColorMultipleID(vo.BrandID, psd.ColorID))
,[MaterialType] = concat(IIF(psd.FabricType = 'F', 'Fabric', IIF(psd.FabricType = 'A', 'Accessory', IIF(psd.FabricType = 'O', 'Other', psd.FabricType))), '-', f.MtlTypeID)
,[WeaveType] = f.WeaveTypeID
,[DisposeQty] = ad.QtyBefore-ad.QtyAfter
,[Unit] = psd.StockUnit
,[Location] = dbo.Getlocation(fty.Ukey)
,[Reason] = ad.ReasonId + '-' + (select name from Reason where ReasonTypeID='Stock_Remove' and id = ad.ReasonId)
from Adjust a
Inner join Adjust_Detail ad on ad.ID = a.ID
left join View_WH_Orders vo on vo.ID = ad.POID
Left join PO_Supp_Detail psd on psd.ID = ad.POID and psd.Seq1 = ad.Seq1 and psd.Seq2 = ad.Seq2
Left join Fabric f on f.SCIRefno = psd.SCIRefno
Left join FtyInventory fty on fty.POID = ad.POID and fty.Seq1 = ad.Seq1 and fty.Seq2 = ad.Seq2 and fty.Roll = ad.Roll and fty.Dyelot = ad.Dyelot and fty.StockType = ad.StockType
where a.Type = 'R' and
{this.strSQLWhere}
";

            DualResult result = DBProxy.Current.Select(null, sqlcmd, this.lisSqlParameter, out this.printTable);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return Ict.Result.True;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            string excelName = "Warehouse_R46";

            this.SetCount(this.printTable.Rows.Count);
            if (this.printTable == null || this.printTable.Rows.Count == 0)
            {
                MyUtility.Msg.ErrorBox("Data not found");
                return false;
            }

            Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\" + $"\\{excelName}.xltx"); // 預先開啟excel app
            MyUtility.Excel.CopyToXls(this.printTable, string.Empty, showExcel: false, xltfile: $"{excelName}.xltx", headerRow: 1, excelApp: objApp);

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName(excelName);

            Microsoft.Office.Interop.Excel.Workbook workbook = objApp.ActiveWorkbook;

            workbook.SaveAs(strExcelName);
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);
            strExcelName.OpenFile();
            #endregion

            return true;
        }
    }
}
