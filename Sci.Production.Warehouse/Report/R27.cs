using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using Sci.Win;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Warehouse
{
    /// <summary>
    /// R27
    /// </summary>
    public partial class R27 : Win.Tems.PrintForm
    {
        private string sqlGetData;
        private List<SqlParameter> listPar;
        private DataTable dtResult;

        /// <summary>
        /// R27
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R27(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.comboIssueType.SelectedIndex = 0;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if (!this.dateRangeIssue.HasValue && MyUtility.Check.Empty(this.txtSPFrom.Text) && MyUtility.Check.Empty(this.txtSPTo.Text))
            {
                MyUtility.Msg.WarningBox("<Issue Date> or <SP#> must be entered");
                return false;
            }

            string sqlWhere = string.Empty;
            this.listPar = new List<SqlParameter>();
            this.sqlGetData = string.Empty;

            if (this.dateRangeIssue.HasValue1)
            {
                sqlWhere += "and I.IssueDate >= @IssueDateFrom ";
                this.listPar.Add(new SqlParameter("@IssueDateFrom", this.dateRangeIssue.DateBox1.Value));
            }

            if (this.dateRangeIssue.HasValue2)
            {
                sqlWhere += "and I.IssueDate <= @IssueDateTo ";
                this.listPar.Add(new SqlParameter("@IssueDateTo", this.dateRangeIssue.DateBox2.Value));
            }

            if (!MyUtility.Check.Empty(this.txtSPFrom.Text))
            {
                sqlWhere += "and ID.POID >= @SPFrom ";
                this.listPar.Add(new SqlParameter("@SPFrom", this.txtSPFrom.Text));
            }

            if (!MyUtility.Check.Empty(this.txtSPTo.Text))
            {
                sqlWhere += "and ID.POID <= @SPTo ";
                this.listPar.Add(new SqlParameter("@SPTo", this.txtSPTo.Text));
            }

            if (this.comboIssueType.Text == "Sewing")
            {
                sqlWhere += "and I.Type = 'B' ";
            }
            else if (this.comboIssueType.Text == "Packing")
            {
                sqlWhere += "and I.Type = 'C' ";
            }
            else
            {
                sqlWhere += "and I.Type in ('B','C') ";
            }

            if (!MyUtility.Check.Empty(this.txtMdivision.Text))
            {
                sqlWhere += "and I.MDivisionID = @MDivisionID ";
                this.listPar.Add(new SqlParameter("@MDivisionID", this.txtMdivision.Text));
            }

            if (!MyUtility.Check.Empty(this.txtfactory.Text))
            {
                sqlWhere += "and I.FactoryID = @FactoryID ";
                this.listPar.Add(new SqlParameter("@FactoryID", this.txtfactory.Text));
            }

            this.sqlGetData = $@"
select	I.ID,
		I.IssueDate,
		I.MDivisionID,
		I.FactoryID,
		I.CutplanID,
		I.OrderId,
		ID.POID,
		i.SewLine,
		[Seq] = concat(Ltrim(Rtrim( ID.seq1)), ' ',  ID.seq2),
		[Description] = dbo.getmtldesc(ID.poid, ID.seq1,ID.seq2,2,0),
		[Color] = dbo.GetColorMultipleID( PSD.BrandId,  PSD.ColorID),
		[Size] = PSD.SizeSpec,
		[@Qty] = PSD.UsedQty,
		PSD.SizeUnit,
		[Location] = dbo.Getlocation(f.ukey),
		[AccuIssued] =	isnull((select sum(Issue_Detail.qty) 
								from dbo.issue WITH (NOLOCK) 
								inner join dbo.Issue_Detail WITH (NOLOCK) on Issue_Detail.id = Issue.Id 
								where	Issue.type = I.Type and 
										Issue.Status = 'Confirmed' and 
										issue.id != I.Id and 
										Issue_Detail.poid = ID.poid and 
										Issue_Detail.seq1 = ID.seq1 and 
										Issue_Detail.seq2 = ID.seq2 and 
										Issue_Detail.roll = ID.roll and 
										Issue_Detail.stocktype = ID.stocktype),0.00),
		ID.Qty,
		PSD.StockUnit,
		[Output] =	case when I.Type = 'B' then isnull ((	select v.sizeqty+', ' 
															from (	select (rtrim(Issue_Size.SizeCode) +'*'+convert(varchar,Issue_Size.Qty)) as sizeqty 
																	from dbo.Issue_Size WITH (NOLOCK) 
																	where   Issue_Size.Issue_DetailUkey = ID.ukey and qty <>0) v 
															for xml path('')),'')
					else '' end,
		I.Remark,
		[AddName] = dbo.getPass1_ExtNo(I.AddName),
		i.AddDate,
		[EditName] = dbo.getPass1_ExtNo(I.EditName),
		I.EditDate
from Issue I with (nolock)
Inner join Issue_Detail ID with (nolock) on I.ID=ID.ID
Inner join po_supp_detail PSD on ID.POID=PSD.ID and ID.SEQ1=PSD.SEQ1 and ID.SEQ2=PSD.SEQ2
Inner join FtyInventory F on ID.Poid = F.Poid and ID.Seq1 = F.seq1 and ID.seq2 = F.seq2 and ID.roll = F.roll
where 1 = 1 {sqlWhere}

";

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            return DBProxy.Current.Select(null, this.sqlGetData, this.listPar, out this.dtResult);
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(ReportDefinition report)
        {
            this.ShowWaitMessage("Excel Processing...");
            this.SetCount(this.dtResult.Rows.Count); // 顯示筆數

            if (this.dtResult.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                this.HideWaitMessage();
                return false;
            }

            Excel.Application objApp = new Excel.Application();
            Utility.Report.ExcelCOM com = new Utility.Report.ExcelCOM(Env.Cfg.XltPathDir + "\\Warehouse_R27.xltx", objApp);
            com.UseInnerFormating = false;
            com.WriteTable(this.dtResult, 2);

            com.ExcelApp.ActiveWorkbook.Sheets[1].Select(Type.Missing);
            objApp.Visible = true;
            Marshal.ReleaseComObject(objApp);
            this.HideWaitMessage();
            return true;
        }
    }
}
