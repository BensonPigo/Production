using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Quality
{
    /// <inheritdoc/>
    public partial class R52 : Win.Tems.PrintForm
    {
        private StringBuilder sqlCmd;
        private string CutRef1;
        private string CutRef2;
        private string SP;
        private string SubProcess;
        private string M;
        private string Factory;
        private string LastResult;
        private DateTime? dateEstCutDate1;
        private DateTime? dateEstCutDate2;
        private DateTime? dateBundle1;
        private DateTime? dateBundle2;
        private DateTime? dateBundleInspectDate1;
        private DateTime? dateBundleInspectDate2;
        private DataTable PrintData;
        private List<SqlParameter> sqlParameter = new List<SqlParameter>();

        /// <inheritdoc/>
        public R52(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.Comboload();
            this.comboFactory.SetDataSource();
        }

        private void Comboload()
        {
            DualResult result;
            if (result = DBProxy.Current.Select(null, "select '' as id union select MDivisionID from factory WITH (NOLOCK)", out DataTable dtfactory))
            {
                this.comboM.DataSource = dtfactory;
                this.comboM.DisplayMember = "ID";
            }
            else
            {
                this.ShowErr(result);
            }

            DataTable dtResultType = new DataTable();
            dtResultType.Columns.Add("type", typeof(string));
            dtResultType.Columns.Add("typeName", typeof(string));
            dtResultType.Rows.Add(string.Empty, string.Empty);
            dtResultType.Rows.Add("0", "Fail");
            dtResultType.Rows.Add("1", "Pass");
            this.comboLastResult.DataSource = dtResultType;
            this.comboLastResult.DisplayMember = "typeName";
            this.comboLastResult.ValueMember = "type";
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(this.dateBundleCDate.Value1) && MyUtility.Check.Empty(this.dateBundleCDate.Value2) &&
                MyUtility.Check.Empty(this.dateEstCutDate.Value1) && MyUtility.Check.Empty(this.dateEstCutDate.Value2) &&
                MyUtility.Check.Empty(this.dateBundleInspectDate.Value1) && MyUtility.Check.Empty(this.dateBundleInspectDate.Value2))
            {
                MyUtility.Msg.WarningBox("[Est Cut Date] or [Bundle CDate] or [Bundle leInspect Date] can't empty!!");
                return false;
            }

            this.SubProcess = this.txtsubprocess.Text;
            this.SP = this.txtSP.Text;
            this.M = this.comboM.Text;
            this.Factory = this.comboFactory.Text;
            this.CutRef1 = this.txtCutRefStart.Text;
            this.CutRef2 = this.txtCutRefEnd.Text;
            this.LastResult = this.comboLastResult.SelectedValue.ToString();
            this.dateBundle1 = this.dateBundleCDate.Value1;
            this.dateBundle2 = this.dateBundleCDate.Value2;
            this.dateBundleInspectDate1 = this.dateBundleInspectDate.Value1;
            this.dateBundleInspectDate2 = this.dateBundleInspectDate.Value2;
            this.dateEstCutDate1 = this.dateEstCutDate.Value1;
            this.dateEstCutDate2 = this.dateEstCutDate.Value2;
            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            this.sqlParameter.Clear();
            this.sqlCmd = new StringBuilder();

            this.sqlCmd.Append(@"
select 
    [Bundleno] = bmd.BundleNo,
    [Spno] = o.ID,
    [MasterSP] = b.POID,
    [M] = b.MDivisionid,
    [Factory] = o.FtyGroup,
    [Style] = o.StyleID,
    [Season] = o.SeasonID,
    [Brand] = o.BrandID,
    [Cut Ref#] = b.CutRef,
    [Comb] = b.PatternPanel,
    [Cut#] = b.Cutno,
    [Article] = b.Article,
    [Color] = b.ColorId,
    [Group] = bd.BundleGroup,
    [Size] = bd.SizeCode,
    [Qty] = bd.Qty,
    [Sub-process] = bmd.SubprocessID,
    [TransferDate] = bmd.AddDate,
    [Result] = CASE bmd.Result
                  WHEN 1 THEN 'Passed'
                  WHEN 0 THEN 'Failed'
               END,
    [MD Operator ID#] = bmd.OperatorID,
    [MD Operator Name] = pass1.Name
into #tmp 
from BundleMDScan bmd WITH (NOLOCK)
inner join Bundle_Detail bd WITH (NOLOCK) on bmd.BundleNo = bd.BundleNo
inner join Bundle b WITH (NOLOCK) on bd.ID = b.ID
inner join Bundle_Detail_Order bdo WITH (NOLOCK) on bdo.BundleNo = bmd.BundleNo
inner join Orders o WITH (NOLOCK) on bdo.OrderId = o.ID
left join [ExtendServer].ManufacturingExecution.dbo.Pass1 pass1 WITH (NOLOCK) on bmd.OperatorId = pass1.ID
where 1 = 1
");

            if (!MyUtility.Check.Empty(this.dateBundle1))
            {
                this.sqlParameter.Add(new SqlParameter("@dateBundle1", ((DateTime)this.dateBundle1.Value).ToString("yyyy-MM-dd")));
                this.sqlCmd.Append(" and b.Cdate >= @dateBundle1");
            }

            if (!MyUtility.Check.Empty(this.dateBundle2))
            {
                this.sqlParameter.Add(new SqlParameter("@dateBundle2", ((DateTime)this.dateBundle2.Value).ToString("yyyy-MM-dd")));
                this.sqlCmd.Append(" and b.Cdate <= @dateBundle2");
            }

            if (!MyUtility.Check.Empty(this.CutRef1) && (!MyUtility.Check.Empty(this.CutRef2)))
            {
                this.sqlParameter.Add(new SqlParameter("@CutRef1", this.CutRef1));
                this.sqlParameter.Add(new SqlParameter("@CutRef2", this.CutRef2));
                this.sqlCmd.Append(" and b.CutRef between @CutRef1 and @CutRef2");
            }

            if (!MyUtility.Check.Empty(this.SP))
            {
                this.sqlParameter.Add(new SqlParameter("@SP", this.SP));
                this.sqlCmd.Append(" and exists(select 1 from Bundle_Detail_Order with(nolock) where bundleNo = bd.bundleNo and Orderid = @SP)");
            }

            if (!MyUtility.Check.Empty(this.dateEstCutDate1) || !MyUtility.Check.Empty(this.dateEstCutDate2))
            {
                this.sqlCmd.Append(@"
                and exists (
                    select 1 from WorkOrder w WITH (NOLOCK) 
                    where w.id = b.POID 
                    and w.CutRef = b.CutRef 
                ");
                if (!MyUtility.Check.Empty(this.dateEstCutDate1))
                {
                    this.sqlParameter.Add(new SqlParameter("@dateEstCutDate1", ((DateTime)this.dateEstCutDate1.Value).ToString("yyyy-MM-dd")));
                    this.sqlCmd.Append(" and w.EstCutDate >= @dateEstCutDate1 ");
                }

                if (!MyUtility.Check.Empty(this.dateEstCutDate2))
                {
                    this.sqlParameter.Add(new SqlParameter("@dateEstCutDate2", ((DateTime)this.dateEstCutDate2.Value).ToString("yyyy-MM-dd")));
                    this.sqlCmd.Append(" and w.EstCutDate <= @dateEstCutDate2 ");
                }
                this.sqlCmd.Append(" ) ");
            }

            if (!MyUtility.Check.Empty(this.dateBundleInspectDate1))
            {
                this.sqlParameter.Add(new SqlParameter("@dateBundleInspectDate1", ((DateTime)this.dateBundleInspectDate1.Value).ToString("yyyy-MM-dd")));
                this.sqlCmd.Append(" and CONVERT(date, bmd.AddDate) >= @dateBundleInspectDate1");
            }

            if (!MyUtility.Check.Empty(this.dateBundleInspectDate2))
            {
                this.sqlParameter.Add(new SqlParameter("@dateBundleInspectDate2", ((DateTime)this.dateBundleInspectDate2.Value).ToString("yyyy-MM-dd")));
                this.sqlCmd.Append(" and CONVERT(date, bmd.AddDate) <= @dateBundleInspectDate2");
            }

            if (!MyUtility.Check.Empty(this.SubProcess))
            {
                this.sqlCmd.Append($@" and bmd.SubprocessID in ('{this.SubProcess.Replace(",", "','")}')" + Environment.NewLine);
            }

            if (!MyUtility.Check.Empty(this.M))
            {
                this.sqlParameter.Add(new SqlParameter("@M", this.M));
                this.sqlCmd.Append(" and b.MDivisionid = @M");
            }

            if (!MyUtility.Check.Empty(this.Factory))
            {
                this.sqlParameter.Add(new SqlParameter("@Factory", this.Factory));
                this.sqlCmd.Append(" and o.FtyGroup = @Factory");
            }

            if (!MyUtility.Check.Empty(this.LastResult))
            {
                this.sqlParameter.Add(new SqlParameter("@LastResult", this.LastResult));
                this.sqlCmd.Append(
                    @" and bmd.BundleNo IN (
                        SELECT BundleNo
                        FROM BundleMDScan WITH (NOLOCK)
                        WHERE AddDate = (
                            SELECT MAX(AddDate)
                            FROM BundleMDScan sub WITH (NOLOCK)
                            WHERE sub.BundleNo = BundleMDScan.BundleNo and sub.SubProcessID = BundleMDScan.SubProcessID
                        ) AND Result = @LastResult)");
            }

            this.sqlCmd.Append(@"
select distinct [Bundleno],
	[Spno] = Spno.val,
	[MasterSP],[M],[Factory],[Style],[Season],[Brand],[Cut Ref#],[Comb],[Cut#],[Article],
	[Color],[Group],[Size],[Qty],[Sub-process],[TransferDate],[Result],[MD Operator ID#],[MD Operator Name]
from #tmp tmp
outer apply (
	select val = case when (select count(1) from Bundle_Detail_Order WITH (NOLOCK) where BundleNo = tmp.[Bundleno]) = 1 
				then (select OrderID from Bundle_Detail_Order WITH (NOLOCK) where BundleNo = tmp.[Bundleno])
				else dbo.GetSinglelineSP((select OrderID from Bundle_Detail_Order WITH (NOLOCK) where BundleNo = tmp.[Bundleno] order by OrderID for XML RAW))
				end
)Spno

drop table #tmp
");

            return DBProxy.Current.Select(null, this.sqlCmd.ToString(), this.sqlParameter, out this.PrintData);
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.PrintData.Rows.Count);

            if (this.PrintData.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            string filename = "Quality_R52_Subprocess MD Inspection Report.xltx";
            Excel.Application excelApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\" + filename); // 預先開啟excel app
            MyUtility.Excel.CopyToXls(this.PrintData, string.Empty, filename, 1, false, null, excelApp, wSheet: excelApp.Sheets[1]);

            excelApp.Visible = true;
            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName(filename);
            Excel.Workbook workbook = excelApp.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            Marshal.ReleaseComObject(workbook);
            Marshal.ReleaseComObject(excelApp);
            #endregion
            return true;
        }
    }
}
