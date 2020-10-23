using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data.SqlClient;
using Sci.Win.Tools;
using System.Runtime.InteropServices;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// R18
    /// </summary>
    public partial class R18 : Win.Tems.PrintForm
    {
        private string sqlGetData;
        private DataTable dtResult;
        private List<SqlParameter> listPar = new List<SqlParameter>();

        /// <summary>
        /// Initializes a new instance of the <see cref="R18"/> class.
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public R18(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();

            DualResult result = DBProxy.Current.Select(null, "select '' as ID union all select ID from Factory WITH (NOLOCK) where IsProduceFty = 1", out DataTable dtFty);
            if (!result)
            {
                this.ShowErr(result);
            }

            this.comboFactory.DataSource = dtFty;
            this.comboFactory.ValueMember = "ID";
            this.comboFactory.DisplayMember = "ID";
            this.comboReportType.SelectedIndex = 0;
            this.comboStatus.SelectedIndex = 6;
            this.comboMDivision.SetDefalutIndex(true);
        }

        /// <summary>
        /// ValidateInput
        /// </summary>
        /// <returns>true</returns>
        protected override bool ValidateInput()
        {
            string sqlWhere = string.Empty;
            string sqlWhereResponsibilityDept = string.Empty;
            this.listPar = new List<SqlParameter>();
            this.sqlGetData = string.Empty;

            if (this.dateRangeConfirm.HasValue1)
            {
                sqlWhere += " and ICR.CFMDate >= @CFMDateFrom ";
                this.listPar.Add(new SqlParameter("@CFMDateFrom", this.dateRangeConfirm.DateBox1.Value));
            }

            if (this.dateRangeConfirm.HasValue2)
            {
                sqlWhere += " and ICR.CFMDate < DateAdd(Day,1,@CFMDateTo) ";
                this.listPar.Add(new SqlParameter("@CFMDateTo", this.dateRangeConfirm.DateBox2.Value));
            }

            if (this.dateRangeConfirmDept.HasValue1)
            {
                sqlWhere += " and ICR.RespDeptConfirmDate >= @RespDeptConfirmDateFrom ";
                this.listPar.Add(new SqlParameter("@RespDeptConfirmDateFrom", this.dateRangeConfirmDept.DateBox1.Value));
            }

            if (this.dateRangeConfirmDept.HasValue2)
            {
                sqlWhere += " and ICR.RespDeptConfirmDate < DateAdd(Day,1,@RespDeptConfirmDateTo) ";
                this.listPar.Add(new SqlParameter("@RespDeptConfirmDateTo", this.dateRangeConfirmDept.DateBox2.Value));
            }

            if (!MyUtility.Check.Empty(this.comboMDivision.Text))
            {
                sqlWhere += $" and f.MDivisionID = '{this.comboMDivision.Text}' ";
            }

            if (!MyUtility.Check.Empty(this.comboFactory.Text))
            {
                sqlWhere += $" and ICR.Department = '{this.comboFactory.Text}' ";
            }

            if (!MyUtility.Check.Empty(this.txtShareDept.Text))
            {
                sqlWhereResponsibilityDept += $" and icrrd.DepartmentID = '{this.txtShareDept.Text}' ";
            }

            if (!this.comboStatus.Text.EqualString("All"))
            {
                sqlWhere += $" and ICR.Status = '{this.comboStatus.Text}' ";
            }

            this.sqlGetData = $@"
select
    ICR.ID,
    f.MDivisionID,
    ICR.Department,
    f.KPICode,
    ICR.OrderID,
    o.StyleID,
    o.SeasonID,
    o.BrandID,
    [TotalQty] = iif(o.POID <> ICR.OrderID, o.Qty, (select sum(Qty) from orders with (nolock) where id = ICR.OrderID)),
    [PO_Handle] = [dbo].[getTPEPass1_ExtNo](PO.POHandle) ,
    [PO_SMR] = [dbo].[getTPEPass1_ExtNo](o.SMR),
    [MR] = [dbo].[getTPEPass1_ExtNo](o.MRHandle),
    [SMR] = [dbo].[getTPEPass1_ExtNo](o.SMR),
    [IssueSubject] = (select CONCAT(ID,' - ', Name) from Reason where ID = ICR.IrregularPOCostID And Reason.ReasonTypeID = 'PO_IrregularCost'),
    ICR.RMtlAmtUSD,
    ICR.OtherAmtUSD,
    ICR.ActFreightUSD,
    [TotalUSD] = ICR.RMtlAmtUSD + ICR.ActFreightUSD + ICR.OtherAmtUSD,
    ICR.VoucherID,
    ICR.VoucherDate,
    o.POID,
    ICR.IrregularPOCostID,
    ICR.Status,
    [AddDate] = format(ICR.AddDate, 'yyyy/MM/dd'), 
    ICR.CFMDate,
    FTY = iif(ICR.Responsible = 'S', ICR.BulkFTY, o.FactoryID)
into #tmpBaseICR
from ICR with (nolock)
left join Orders o with (nolock) on ICR.OrderID = o.ID
left join PO with (nolock) on o.POID = PO.ID
left join Factory f with (nolock) on ICR.Department = f.ID
where 1 = 1 {sqlWhere}

if ('{this.comboReportType.Text}' = 'Detail List')
begin
    select
        ICR.ID,
        ICR.Status,
        ICR.MDivisionID,
        ICR.Department,
        ICR.FTY,
        ICR.KPICode,
        ICR.OrderID,
        ICR.StyleID,
        ICR.SeasonID,
        ICR.BrandID,
        ICR.TotalQty,
        ICR.PO_Handle ,
        ICR.PO_SMR,
        ICR.MR,
        ICR.SMR,
        ICR.IssueSubject,
        ICR.RMtlAmtUSD,
        ICR.OtherAmtUSD,
        ICR.ActFreightUSD,
        ICR.TotalUSD,
        ICR.AddDate,
        ICR.CFMDate,
        ICR.VoucherID,
        ICR.VoucherDate,
        [Seq] = icrd.Seq1 + '-' + icrd.Seq2,
        [SourceType] = (select DropDownList.Name 
    					    from Fabric, DropDownList 
    					    where psd.SCIRefno = Fabric.SCIRefno 
    					    and Fabric.Type = DropDownList.ID 
    					    and DropDownList.type = 'FabricType' ),
        [WeaveType] = (SELECT WeaveTypeID FROM Fabric 
    								      WHERE SCIRefno = (SELECT SCIRefno FROM PO_Supp_Detail WHERE ID = ICR.POID AND Seq1 = icrd.Seq1 AND Seq2 = icrd.Seq2)),
        icrd.MtltypeID,
        icrd.ICRQty,
        icrd.ICRFoc,
        icrd.PriceUSD,
        [IrregularAmtUSD] = (Select Amount from dbo.GetAmountByUnit(icrd.PriceUSD, icrd.ICRQty, psd.POUnit,2))
    from #tmpBaseICR ICR
    left join ICR_Detail icrd with (nolock) on ICR.ID = icrd.ID
    left join PO_Supp_Detail psd with (nolock) on psd.ID = ICR.POID and psd.SEQ1= icrd.Seq1  and psd.SEQ2= icrd.Seq2
end
else
begin

    select 
        ICR.ID,
        ICR.Status,
        ICR.MDivisionID,
        ICR.Department,
        ICR.FTY,
        ICR.KPICode,
        ICR.OrderID,
        ICR.StyleID,
        ICR.SeasonID,
        ICR.BrandID,
        ICR.TotalQty,
        ICR.IssueSubject,
        ICR.RMtlAmtUSD,
        ICR.OtherAmtUSD,
        ICR.ActFreightUSD,
        ICR.TotalUSD,
        icrrd.FactoryID,
        icrrd.DepartmentID,
        icrrd.Amount,
        ICR.AddDate,
        ICR.CFMDate,
        ICR.VoucherID,
        ICR.VoucherDate,
        ICR.PO_Handle ,
        ICR.PO_SMR,
        ICR.MR,
        ICR.SMR
    from #tmpBaseICR ICR
    left join ICR_ResponsibilityDept icrrd with (nolock) on ICR.ID = icrrd.ID
    where 1 = 1 {sqlWhereResponsibilityDept}
end

drop table #tmpBaseICR
";
            return base.ValidateInput();
        }

        /// <summary>
        /// OnAsyncDataLoad
        /// </summary>
        /// <param name="e">e</param>
        /// <returns>true</returns>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            return DBProxy.Current.Select(null, this.sqlGetData, this.listPar, out this.dtResult);
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            if (this.dtResult == null || this.dtResult.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found.");
                this.SetCount(0);
                return false;
            }

            this.SetCount(this.dtResult.Rows.Count);
            this.ShowWaitMessage("Excel processing...");

            Excel.Application objApp = new Excel.Application();
            Utility.Report.ExcelCOM com;

            if (this.comboReportType.Text == "Detail List")
            {
                com = new Utility.Report.ExcelCOM(Env.Cfg.XltPathDir + "\\PPIC_R18_DetailList.xltx", objApp);
            }
            else
            {
                com = new Utility.Report.ExcelCOM(Env.Cfg.XltPathDir + "\\PPIC_R18_RespDeptList.xltx", objApp);
            }

            com.WriteTable(this.dtResult, 2);

            objApp.Visible = true;
            objApp.Rows.AutoFit();
            Marshal.ReleaseComObject(objApp);
            this.HideWaitMessage();

            return true;
        }

        private void TxtShareDept_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            SelectItem selectItem = new SelectItem("select ID,Name from FinanceEN.dbo.Department", string.Empty, string.Empty);
            DialogResult dialogResult = selectItem.ShowDialog();
            if (dialogResult != DialogResult.OK)
            {
                return;
            }

            this.txtShareDept.Text = selectItem.GetSelectedString();
        }
    }
}
