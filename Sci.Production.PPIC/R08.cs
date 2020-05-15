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
using sxrc = Sci.Utility.Excel.SaveXltReportCls;
using System.Runtime.InteropServices;
using System.Data.SqlClient;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// R08
    /// </summary>
    public partial class R08 : Sci.Win.Tems.PrintForm
    {
        private DataTable[] _printData;
        private DateTime? Cdate1;
        private DateTime? Cdate2;
        private DateTime? Apvdate1;
        private DateTime? Apvdate2;
        private DateTime? Lockdate1;
        private DateTime? Lockdate2;
        private DateTime? Cfmdate1;
        private DateTime? Cfmdate2;
        private string MDivisionID;
        private string FtyGroup;
        private string T;
        private string Status;
        private string Sharedept;
        private string ReportType;

        /// <summary>
        /// R08
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public R08(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            DataTable mDivision, factory;
            DBProxy.Current.Select(null, "select '' as ID union all select ID from MDivision WITH (NOLOCK) ", out mDivision);
            MyUtility.Tool.SetupCombox(this.comboM, 1, mDivision);
            MyUtility.Tool.SetupCombox(this.comboType, 2, 1, ",,F,Fabric,A,Accessory,");
            MyUtility.Tool.SetupCombox(this.cmbStatus, 1, 1, "ALL,Approved,Auto.Lock,Checked,Confirmed,Junked,New");
            MyUtility.Tool.SetupCombox(this.cmbReportType, 2, 1, "0,Detail List,1,Resp. Dept. List");
            DBProxy.Current.Select(null, "select '' as ID union all select distinct FtyGroup from Factory WITH (NOLOCK) ", out factory);
            MyUtility.Tool.SetupCombox(this.comboFactory, 1, factory);
            this.comboM.Text = Sci.Env.User.Keyword;
            this.comboType.SelectedIndex = 0;
            this.cmbStatus.SelectedIndex = 0;
            this.cmbReportType.SelectedIndex = 0;
            this.comboFactory.Text = Sci.Env.User.Factory;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.Cdate1 = this.dateCreateDate.Value1;
            this.Cdate2 = this.dateCreateDate.Value2;
            this.Apvdate1 = this.dateApvDate.Value1;
            this.Apvdate2 = this.dateApvDate.Value2;
            this.Lockdate1 = this.dateLock.Value1;
            this.Lockdate2 = this.dateLock.Value2;
            this.Cfmdate1 = this.dateCfm.Value1;
            this.Cfmdate2 = this.dateCfm.Value2;
            this.MDivisionID = this.comboM.Text;
            this.FtyGroup = this.comboFactory.Text;
            this.T = MyUtility.Convert.GetString(this.comboType.SelectedValue);
            this.Status = this.cmbStatus.Text;
            this.Sharedept = this.txtSharedept.Text;
            this.ReportType = this.cmbReportType.Text;
            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            #region SqlParameter
            List<SqlParameter> sqlpar = new List<SqlParameter>();
            #endregion

            #region where
            string where = string.Empty;
            if (!MyUtility.Check.Empty(this.Cdate1))
            {
                where += $@"and rr.CDate >= @CDate1 " + "\r\n";
                sqlpar.Add(new SqlParameter("@CDate1", this.Cdate1));
            }

            if (!MyUtility.Check.Empty(this.Cdate2))
            {
                where += $@"and rr.CDate <= @CDate2 " + "\r\n";
                sqlpar.Add(new SqlParameter("@CDate2", this.Cdate2));
            }

            if (!MyUtility.Check.Empty(this.Apvdate1))
            {
                where += $@"and rr.ApvDate >= @ApvDate1 " + "\r\n";
                sqlpar.Add(new SqlParameter("@ApvDate1", this.Apvdate1));
            }

            if (!MyUtility.Check.Empty(this.Apvdate2))
            {
                where += $@"and rr.ApvDate <= @ApvDate2 " + "\r\n";
                sqlpar.Add(new SqlParameter("@ApvDate2", this.Apvdate2));
            }

            if (!MyUtility.Check.Empty(this.Lockdate1))
            {
                where += $@"and rr.LockDate >= @Lockdate1 " + "\r\n";
                sqlpar.Add(new SqlParameter("@Lockdate1", this.Lockdate1));
            }

            if (!MyUtility.Check.Empty(this.Lockdate2))
            {
                where += $@"and rr.LockDate <= @Lockdate2 " + "\r\n";
                sqlpar.Add(new SqlParameter("@Lockdate2", this.Lockdate2));
            }

            if (!MyUtility.Check.Empty(this.Cfmdate1))
            {
                where += $@"and cast(rr.RespDeptConfirmDate as date) >= @Cfmdate1 " + "\r\n";
                sqlpar.Add(new SqlParameter("@Cfmdate1", this.Cfmdate1));
            }

            if (!MyUtility.Check.Empty(this.Cfmdate2))
            {
                where += $@"and cast(rr.RespDeptConfirmDate as date) <= @Cfmdate2 " + "\r\n";
                sqlpar.Add(new SqlParameter("@Cfmdate2", this.Cfmdate2));
            }

            if (!MyUtility.Check.Empty(this.MDivisionID))
            {
                where += $@"and rr.MDivisionID = @MDivisionID " + "\r\n";
                sqlpar.Add(new SqlParameter("@MDivisionID", this.MDivisionID));
            }

            if (!MyUtility.Check.Empty(this.FtyGroup))
            {
                where += $@"and rr.FactoryID = @FactoryID " + "\r\n";
                sqlpar.Add(new SqlParameter("@FactoryID", this.FtyGroup));
            }

            if (!MyUtility.Check.Empty(this.T))
            {
                where += $@"and rr.Type = @Type " + "\r\n";
                sqlpar.Add(new SqlParameter("@Type", this.T));
            }

            if (this.Status != "ALL")
            {
                where += $@"and rr.Status = @Status " + "\r\n";
                sqlpar.Add(new SqlParameter("@Status", this.Status));
            }

            if (!MyUtility.Check.Empty(this.Sharedept))
            {
                where += $@"and exists(select 1 from ICR_ResponsibilityDept icr with(nolock) where icr.ID = rr.id and icr.DepartmentID =  @DepartmentID) " + "\r\n";
                sqlpar.Add(new SqlParameter("@DepartmentID", this.Sharedept));
            }
            #endregion

            #region sqlcmd 主table
            string sqlcmd = string.Empty;
            if (this.ReportType == "Detail List")
            {
                sqlcmd = $@"
select
	rr.id,
	Type=IIF(rr.Type = 'F', 'Fabric', 'Accessory'),
	rr.MDivisionID,
	rr.FactoryID,
	rr.POID,
	Style = (select ID from Style s where s.Ukey = o.StyleUkey),
	o.BrandID,
	o.SeasonID,
	rr.Status,
	rr.CDate,
	rr.ApvDate,
	rr.LockDate,
	rr.Responsibility,
	x.ttlestamt,
	rr.RMtlAmt,
	rr.ActFreight,
	rr.EstFreight,
	rr.SurchargeAmt,
    TTLUS = isnull(rr.RMtlAmt,0) + isnull(rr.ActFreight,0) +isnull(rr.EstFreight,0) + isnull(rr.SurchargeAmt,0),
    POHandle = [dbo].[getTPEPass1_ExtNo](PO.POHandle),
    PCSMR = [dbo].[getTPEPass1_ExtNo](PO.PCSMR),
	rr.TransferResponsible,
	rr.TransferNo
from ReplacementReport rr WITH (NOLOCK) 
left join Orders o WITH (NOLOCK)  on o.ID = rr.POID
left join PO with(nolock) on PO.ID = rr.POID
outer apply(
	select ttlestamt = SUM(EstReplacementAMT)
	from (
		select EstReplacementAMT = case when rrd.Junk =1 then 0
                        else
                        rrd.EstInQty * isnull((select RateValue from Unit_Rate where UnitFrom = rrd.ReplacementUnit and UnitTo = psd.POUnit),1) *
						psd.Price * isnull(dbo.getRate('KP',Supp.Currencyid,'USD',rr.CDate),1)
						end
		from ReplacementReport_Detail rrd with(nolock)
		left join PO_Supp_Detail psd WITH (NOLOCK) on psd.ID = rr.POID and psd.SEQ1 = rrd.Seq1 and psd.SEQ2 = rrd.Seq2
		left join PO_Supp ps WITH (NOLOCK) on ps.ID = psd.ID and ps.SEQ1 = psd.SEQ1
		left join Supp WITH (NOLOCK) on Supp.ID = ps.SuppID
		where  rrd.ID = rr.ID
	)x
)x
where 1=1
{where}

select
	rr.ID,
	Type = IIF(rr.Type = 'F', 'Fabric', 'Accessory'),
	M = (Select MDivisionID from Factory with(nolock) where ID = rr.FactoryID),
    rr.FactoryID,
	rr.POID,
	o.StyleID,
	o.SeasonID,
	o.BrandID,
	rr.Status,
	rr.CDate,
	rr.ApvDate,
	rr.CompleteDate,
	rr.LockDate,
	Responsibility = (select Name from DropDownList dd with(nolock) where dd.ID = rr.Responsibility and dd.Type = 'Replacement.R'),
	POSMR = [dbo].[getTPEPass1_ExtNo](PO.POSMR),
	POHandle = [dbo].[getTPEPass1_ExtNo](PO.POHandle),
	PCSMR = [dbo].[getTPEPass1_ExtNo](PO.PCSMR),
	PCHandle = [dbo].[getTPEPass1_ExtNo](PO.PCHandle),
	Prepared = [dbo].[getPass1_ExtNo](rr.ApplyName),
	PPICFactorymgr = [dbo].[getPass1_ExtNo](rr.ApvName),
	rr.VoucherID,
	rr.VoucherDate,
	Junk=iif(rrd.Junk=1,'Y',''),
	Seq = iif(isnull(rrd.Seq1,'') = '','',CONCAT(rrd.Seq1,'-',rrd.Seq2)),
	f.MtlTypeID,
	rrd.Refno,
	f.DescDetail,
	rrd.ColorID,
	rrd.EstInQty,
	rrd.ActInQty,
	FinalNeedQty =IIF(rr.Type = 'F', rrd.FinalNeedQty,  rrd.TotalRequest),
	rrd.TotalRequest,
	rrd.AfterCuttingRequest,
    EstReplacementAMT = case when rrd.Junk =1 then 0
						else 
                        rrd.EstInQty * isnull((select RateValue from Unit_Rate where UnitFrom = rrd.ReplacementUnit and UnitTo = psd.POUnit),1) *
						psd.Price * isnull(dbo.getRate('KP',Supp.Currencyid,'USD',rr.CDate),1)
                        end,
    psd.POAmt,
    psd.ShipAmt,
	rrd.ResponsibilityReason,
	rrd.Suggested,
	rrd.PurchaseID,
	NewSeq =iif(isnull(rrd.NewSeq1,'') = '','', CONCAT(rrd.NewSeq1,'-',rrd.NewSeq2))
from ReplacementReport rr with(nolock)
inner join Orders o with(nolock) on o.ID = rr.POID
left join ReplacementReport_Detail rrd with(nolock) on rrd.ID = rr.ID
left join PO_Supp_Detail psd WITH (NOLOCK) on psd.ID = rr.POID and psd.SEQ1 = rrd.Seq1 and psd.SEQ2 = rrd.Seq2
left join PO_Supp ps WITH (NOLOCK) on ps.ID = psd.ID and ps.SEQ1 = psd.SEQ1
left join Supp WITH (NOLOCK) on Supp.ID = ps.SuppID
left join PO with(nolock) on PO.ID = rr.POID
left join Fabric f with(nolock) on f.SCIRefno = rrd.SCIRefno
where 1=1
{where}
";
            }
            else
            {
                sqlcmd = $@"
select
    rr.ID,
    Type = IIF(rr.Type = 'F', 'Fabric', 'Accessory'),
    M = (Select MDivisionID from Factory with(nolock) where ID = rr.FactoryID),
    rr.FactoryID,
    rr.POID,
    o.StyleID,
    o.SeasonID,
    o.BrandID,
    rr.Status,
    rr.CDate,
    rr.ApvDate,
    rr.CompleteDate,
    rr.LockDate,
    Responsibility = (select Name from DropDownList dd with(nolock) where dd.ID = rr.Responsibility and dd.Type = 'Replacement.R'),
	x.ttlestamt,
    rr.RMtlAmt,
    rr.ActFreight,
    rr.EstFreight,
    rr.SurchargeAmt,
    TTLUS = isnull(rr.RMtlAmt,0) + isnull(rr.ActFreight,0) +isnull(rr.EstFreight,0) + isnull(rr.SurchargeAmt,0),
    ResponsibilityFty = icr.FactoryID,
    icr.DepartmentID,
    icr.Percentage,
    icr.Amount,
    rr.VoucherID,
    rr.VoucherDate,
    POSMR = [dbo].[getTPEPass1_ExtNo](PO.POSMR),
    POHandle = [dbo].[getTPEPass1_ExtNo](PO.POHandle),
    PCSMR = [dbo].[getTPEPass1_ExtNo](PO.PCSMR),
    PCHandle = [dbo].[getTPEPass1_ExtNo](PO.PCHandle),
    Prepared = [dbo].[getPass1_ExtNo](rr.ApplyName),
    PPICFactorymgr = [dbo].[getPass1_ExtNo](rr.ApvName)
from ReplacementReport rr with(nolock)
inner join Orders o with(nolock) on o.ID = rr.POID
left join PO with(nolock) on PO.ID = rr.POID
left join  ICR_ResponsibilityDept icr with(nolock) on icr.ID = rr.ID
outer apply(
	select ttlestamt = SUM(EstReplacementAMT)
	from (
		select EstReplacementAMT = case when rrd.Junk =1 then 0
                        else
                        rrd.EstInQty * isnull((select RateValue from Unit_Rate where UnitFrom = rrd.ReplacementUnit and UnitTo = psd.POUnit),1) *
						psd.Price * isnull(dbo.getRate('KP',Supp.Currencyid,'USD',rr.CDate),1)
						end
		from ReplacementReport_Detail rrd with(nolock)
		left join PO_Supp_Detail psd WITH (NOLOCK) on psd.ID = rr.POID and psd.SEQ1 = rrd.Seq1 and psd.SEQ2 = rrd.Seq2
		left join PO_Supp ps WITH (NOLOCK) on ps.ID = psd.ID and ps.SEQ1 = psd.SEQ1
		left join Supp WITH (NOLOCK) on Supp.ID = ps.SuppID
		where  rrd.ID = rr.ID
	)x
)x
where 1=1
{where}
";
            }
            #endregion

            DualResult result = DBProxy.Current.Select(null, sqlcmd, sqlpar, out this._printData);
            if (!result)
            {
                return result;
            }

            return Result.True;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this._printData[0].Rows.Count);

            if (this._printData[0].Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            string filename = string.Empty;
            if (this.ReportType == "Detail List")
            {
                filename = "PPIC_R08_DetailList";
            }
            else
            {
                filename = "PPIC_R08_RespDeptList";
            }

            this.ShowWaitMessage("Excel Processing...");
            Excel.Application excelApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + $"\\{filename}.xltx");
            MyUtility.Excel.CopyToXls(this._printData[0], string.Empty, $"{filename}.xltx", 1, false, null, excelApp, wSheet: excelApp.Sheets[1]); // 將datatable copy to excel
            if (this._printData.Length == 2)
            {
                MyUtility.Excel.CopyToXls(this._printData[1], string.Empty, $"{filename}.xltx", 1, false, null, excelApp, wSheet: excelApp.Sheets[2]); // 將datatable copy to excel
            }

            Excel.Worksheet worksheet = excelApp.ActiveWorkbook.Worksheets[1]; // 取得工作表
            worksheet.Columns.AutoFit();
            if (this._printData.Length == 2)
            {
                worksheet = excelApp.ActiveWorkbook.Worksheets[2]; // 取得工作表
                worksheet.Columns[27].ColumnWidth = 66;
            }

            worksheet.Rows.AutoFit();
            excelApp.Visible = true;
            #region 釋放上面開啟過excel物件
            Marshal.ReleaseComObject(worksheet);
            Marshal.ReleaseComObject(excelApp);
            #endregion

            this.HideWaitMessage();
            return true;
        }

        private void TxtSharedept_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sql = "select ID,Name from FinanceEN.dbo.Department";
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sql, "12,15", this.txtSharedept.Text);
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.txtSharedept.Text = item.GetSelectedString();
        }
    }
}
