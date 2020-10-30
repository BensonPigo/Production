using System;
using System.Data;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;
using Sci.Win;
using System.Collections.Generic;
using System.Reflection;
using Ict.Win;
using System.Linq;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P11_PrintKarbanCard : Win.Tems.PrintForm
    {
        private string mdivision;
        private string factory;
        private string id1;
        private string id2;
        private DateTime? issueDate1;
        private DateTime? issueDate2;
        private DataTable printData;

        /// <summary>
        /// Initializes a new instance of the <see cref="P11_PrintKarbanCard"/> class.
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public P11_PrintKarbanCard()
        {
            this.InitializeComponent();
            this.txtMdivision.Text = Env.User.Keyword;
            // this.print.Enabled = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="P11_PrintKarbanCard"/> class.
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public P11_PrintKarbanCard(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.txtMdivision.Text = Env.User.Keyword;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(this.dateIssueDate.Value1) && MyUtility.Check.Empty(this.dateIssueDate.Value2)
                 && MyUtility.Check.Empty(this.txtID1.Text) && MyUtility.Check.Empty(this.txtID2.Text))
            {
                MyUtility.Msg.WarningBox("[Issue Date], [ID] can't be empty!!");
                return false;
            }

            this.issueDate1 = this.dateIssueDate.Value1;
            this.issueDate2 = this.dateIssueDate.Value2;
            this.mdivision = this.txtMdivision.Text;
            this.factory = this.txtfactory.Text;
            this.id1 = this.txtID1.Text;
            this.id2 = this.txtID2.Text;

            return base.ValidateInput();
        }

        /// <inheritdoc/>
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

            if (!MyUtility.Check.Empty(this.id1))
            {
                where += $@" and i.Id >='{this.id1}' ";
            }

            if (!MyUtility.Check.Empty(this.id2))
            {
                where += $@" and i.Id <='{this.id2}' ";
            }

            if (!MyUtility.Check.Empty(this.mdivision))
            {
                where += $@" and o.MDivisionID ='{this.mdivision}' ";
            }

            if (!MyUtility.Check.Empty(this.factory))
            {
                where += $@" and o.FactoryID ='{this.factory}' ";
            }
            #endregion

            string sqlcmd = $@"
select [head] = i.FactoryID + ' Kanban Card'
	, [JITDate] = ''
	, [Line] = ''
	, o.ID
	, o.StyleID
	, ib.Article
	, ib_size.SizeCode
	, ib_Qty.Qty
    , [Idx] = ROW_NUMBER() over(order by i.id, o.ID, o.StyleID)
from Issue i 
left join Orders o on i.OrderId = o.ID
outer apply (
	select Article = stuff((
		select distinct concat(',', ib.Article)
		from Issue_Breakdown ib 
		where ib.Id = i.Id
		for xml path('')
	),1,1,'')
)ib
outer apply (
	select SizeCode = stuff((
		select concat('/', ib.SizeCode)
		from Issue_Breakdown ib 
		where ib.Id = i.Id
		order by ib.OrderID, ib.Article, ib.SizeCode
		for xml path('')
	),1,1,'')
)ib_size
outer apply (
	select Qty = stuff((
		select concat('/', ib.Qty)
		from Issue_Breakdown ib 
		where ib.Id = i.Id
		order by ib.OrderID, ib.Article, ib.SizeCode
		for xml path('')
	),1,1,'')
)ib_Qty
where i.Type = 'B'
{where}
";

            DualResult result = DBProxy.Current.Select(null, sqlcmd, null, out this.printData);
            if (!result)
            {
                return result;
            }

            return Ict.Result.True;
        }

        /// <inheritdoc/>
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
            string reportName = "Warehouse_P11_KanbanCard.xltx";
            Excel.Application excelApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + $"\\{reportName}");
            Excel.Worksheet worksheet1 = (Excel.Worksheet)excelApp.ActiveWorkbook.Worksheets[1];

            worksheet1.Cells[1, 1] = this.printData.Rows[0]["head"].ToString();
            worksheet1.Cells[4, 2] = this.printData.Rows[0]["ID"].ToString();
            worksheet1.Cells[5, 2] = this.printData.Rows[0]["StyleID"].ToString();
            worksheet1.Cells[6, 2] = this.printData.Rows[0]["Article"].ToString();
            worksheet1.Cells[7, 2] = this.printData.Rows[0]["SizeCode"].ToString();
            worksheet1.Cells[8, 2] = this.printData.Rows[0]["Qty"].ToString();
            if (this.printData.Rows.Count > 1)
            {
                int r1 = 1;
                int r2 = r1 + 7;
                int c1 = 5;
                int c2 = c1 + 3;
                string cEn1 = MyUtility.Excel.ConvertNumericToExcelColumn(c1);
                string cEn2 = MyUtility.Excel.ConvertNumericToExcelColumn(c2);
                for (int i = 1; i <= this.printData.Rows.Count - 1; i++)
                {
                    DataRow row = this.printData.Rows[i];
                    worksheet1.get_Range("A1:D8").Copy();
                    worksheet1.get_Range(string.Format("{0}{1}:{2}{3}", cEn1, r1, cEn2, r2)).PasteSpecial(Excel.XlPasteType.xlPasteAll, Excel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, false, false);
                    worksheet1.Cells[r1, c1] = row["head"].ToString();
                    worksheet1.Cells[r1 + 3, c1 + 1] = row["ID"].ToString();
                    worksheet1.Cells[r1 + 4, c1 + 1] = row["StyleID"].ToString();
                    worksheet1.Cells[r1 + 5, c1 + 1] = row["Article"].ToString();
                    worksheet1.Cells[r1 + 6, c1 + 1] = row["SizeCode"].ToString();
                    worksheet1.Cells[r2, c1 + 1] = row["Qty"].ToString();

                    c1 = c1 + 4;
                    c2 = c1 + 3;
                    cEn1 = MyUtility.Excel.ConvertNumericToExcelColumn(c1);
                    cEn2 = MyUtility.Excel.ConvertNumericToExcelColumn(c2);

                    if (c1 > 16)
                    {
                        r1 = r1 + 8;
                        r2 = r1 + 7;
                        c1 = 1;
                        c2 = c1 + 3;
                        cEn1 = MyUtility.Excel.ConvertNumericToExcelColumn(c1);
                        cEn2 = MyUtility.Excel.ConvertNumericToExcelColumn(c2);
                    }
                }
            }

            worksheet1.Columns.AutoFit();

            #region 釋放上面開啟過excel物件
            string strExcelName = Class.MicrosoftFile.GetName("Warehouse_P11_KanbanCard");
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

        /// <inheritdoc/>
        protected override bool OnToPrint(ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.printData.Rows.Count);

            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            List<P11_PrintKarbanCard_PrintData> printDataList = this.printData.AsEnumerable().Select((dr, index)
                => new P11_PrintKarbanCard_PrintData()
            {
                Idx = MyUtility.Convert.GetInt(dr["Idx"]),
                Head = dr["head"].ToString(),
                ID = dr["ID"].ToString(),
                StyleID = dr["StyleID"].ToString(),
                Article = dr["Article"].ToString(),
                SizeCode = dr["SizeCode"].ToString(),
                Qty = dr["Qty"].ToString(),
            }).ToList();

            report.ReportDataSource = printDataList;

            Type reportResourceNamespace = typeof(P11_PrintKarbanCard_PrintData);
            Assembly reportResourceAssembly = reportResourceNamespace.Assembly;
            string reportResourceName = "P11_PrintKarbanCard.rdlc";
            DualResult result;
            IReportResource reportresource;
            if (!(result = ReportResources.ByEmbeddedResource(reportResourceAssembly, reportResourceNamespace, reportResourceName, out reportresource)))
            {
                this.ShowErr(result);
                return false;
            }

            report.ReportResource = reportresource;

            // 開啟 report view
            var frm = new Win.Subs.ReportView(report)
            {
                MdiParent = this.MdiParent,
            };
            frm.Show();

            return true;
        }
    }
}
