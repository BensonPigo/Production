using Ict;
using Sci.Data;
using System;
using System.Data;
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
            this.sqlCmd = new StringBuilder();

            this.sqlCmd.Append(@"
select 
[Bundleno] = BundleMDScan.BundleNo,
[Spno] = Orders.ID,
[MasterSP] = Bundle.POID,
[M] = Bundle.MDivisionid,
[Factory] = Orders.FtyGroup,
[Style] = Orders.StyleID,
[Season] = Orders.SeasonID,
[Brand] = Orders.BrandID,
[Cut Ref#] = Bundle.CutRef,
[Comb] = Bundle.PatternPanel,
[Cut#] = Bundle.Cutno,
[Article] = Bundle.Article,
[Color] = Bundle.ColorId,
[Group] = Bundle_Detail.BundleGroup,
[Size] = Bundle_Detail.SizeCode,
[Qty] = Bundle_Detail.Qty,
[Sub-process] = BundleMDScan.SubprocessID,
[TransferDate] = BundleMDScan.AddDate,
[Result] = BundleMDScan.Result,
[Operator ID#] = BundleMDScan.OperatorID,
[Operator Name] = ManufacturingExecution.dbo.Pass1.Name
into #tmp 
from BundleMDScan
inner join Bundle_Detail on BundleMDScan.BundleNo = Bundle_Detail.BundleNo
inner join Bundle on Bundle_Detail.ID = Bundle.ID
inner join Bundle_Detail_Order on Bundle_Detail_Order.BundleNo = BundleMDScan.BundleNo
inner join Orders WITH (NOLOCK) on Bundle_Detail_Order.OrderId = Orders.ID
inner join WorkOrder on WorkOrder.id = Bundle.POID and WorkOrder.CutRef = Bundle.CutRef
left join ManufacturingExecution.dbo.Pass1 on BundleMDScan.OperatorId = ManufacturingExecution.dbo.Pass1.ID
where 1 = 1
");

            if (!MyUtility.Check.Empty(this.dateBundle1))
            {
                this.sqlCmd.Append(string.Format(@" and Bundle.Cdate >= '{0}'", Convert.ToDateTime(this.dateBundle1).ToString("yyyy/MM/dd")));
            }

            if (!MyUtility.Check.Empty(this.dateBundle2))
            {
                this.sqlCmd.Append(string.Format(@" and Bundle.Cdate <= '{0}'", Convert.ToDateTime(this.dateBundle2).ToString("yyyy/MM/dd")));
            }

            if (!MyUtility.Check.Empty(this.CutRef1) && (!MyUtility.Check.Empty(this.CutRef1)))
            {
                this.sqlCmd.Append(string.Format(@" and Bundle.CutRef between '{0}' and '{1}'", this.CutRef1, this.CutRef2));
            }

            if (!MyUtility.Check.Empty(this.SP))
            {
                this.sqlCmd.Append(string.Format(@" and exists(select 1 from Bundle_Detail_Order with(nolock) where bundleNo = Bundle_Detail.bundleNo and Orderid= '{0}')", this.SP));
            }

            if (!MyUtility.Check.Empty(this.dateEstCutDate1))
            {
                this.sqlCmd.Append(string.Format(@" and WorkOrder.EstCutDate >= '{0}'", Convert.ToDateTime(this.dateEstCutDate1).ToString("yyyy/MM/dd")));
            }

            if (!MyUtility.Check.Empty(this.dateEstCutDate2))
            {
                this.sqlCmd.Append(string.Format(@" and WorkOrder.EstCutDate <= '{0}'", Convert.ToDateTime(this.dateEstCutDate2).ToString("yyyy/MM/dd")));
            }

            if (!MyUtility.Check.Empty(this.dateBundleInspectDate1))
            {
                this.sqlCmd.Append(string.Format(@" and BundleMDScan.AddDate >= '{0}'", Convert.ToDateTime(this.dateBundleInspectDate1).ToString("yyyy/MM/dd")));
            }

            if (!MyUtility.Check.Empty(this.dateBundleInspectDate2))
            {
                this.sqlCmd.Append(string.Format(@" and BundleMDScan.AddDate <= '{0}'", Convert.ToDateTime(this.dateBundleInspectDate2).ToString("yyyy/MM/dd")));
            }

            if (!MyUtility.Check.Empty(this.SubProcess))
            {
                this.sqlCmd.Append($@" and (BundleMDScan.SubprocessID in ('{this.SubProcess.Replace(",", "','")}') or '{this.SubProcess}'='')" + Environment.NewLine);
            }

            if (!MyUtility.Check.Empty(this.M))
            {
                this.sqlCmd.Append(string.Format(@" and Bundle.MDivisionid = '{0}'", this.M));
            }

            if (!MyUtility.Check.Empty(this.Factory))
            {
                this.sqlCmd.Append(string.Format(@" and Orders.FtyGroup = '{0}'", this.Factory));
            }

            if (!MyUtility.Check.Empty(this.LastResult))
            {
                this.sqlCmd.Append(string.Format(
                    @" and BundleMDScan.BundleNo IN (
                        SELECT BundleNo
                        FROM BundleMDScan
                        WHERE AddDate = (
                            SELECT MAX(AddDate)
                            FROM BundleMDScan sub
                            WHERE sub.BundleNo = BundleMDScan.BundleNo and sub.SubProcessID = BundleMDScan.SubProcessID
                        ) AND Result = {0})", this.LastResult));
            }

            this.sqlCmd.Append(@"
select distinct [Bundleno],
	[Spno] = Spno.val,
	[MasterSP],[M],[Factory],[Style],[Season],[Brand],[Cut Ref#],[Comb],[Cut#],[Article],
	[Color],[Group],[Size],[Qty],[Sub-process],[TransferDate],[Result],[Operator ID#],[Operator Name]
from #tmp tmp
outer apply (
	select val = case when (select count(1) from Bundle_Detail_Order WITH (NOLOCK) where BundleNo = tmp.[Bundleno]) = 1 
				then (select OrderID from Bundle_Detail_Order WITH (NOLOCK) where BundleNo = tmp.[Bundleno])
				else dbo.GetSinglelineSP((select OrderID from Bundle_Detail_Order WITH (NOLOCK) where BundleNo = tmp.[Bundleno] order by OrderID for XML RAW))
				end
)Spno

drop table #tmp
");

            return DBProxy.Current.Select(null, this.sqlCmd.ToString(), out this.PrintData);
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
            MyUtility.Excel.CopyToXls(this.PrintData, string.Empty, filename, 2, false, null, excelApp, wSheet: excelApp.Sheets[1]);

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
