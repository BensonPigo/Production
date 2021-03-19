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
    public partial class R51 : Win.Tems.PrintForm
    {
        private readonly List<SqlParameter> Parameters = new List<SqlParameter>();
        private readonly StringBuilder Sqlcmd = new StringBuilder();
        private string sqlCol;
        private DataTable PrintData;
        private DataTable CustomColumnDt;

        /// <inheritdoc/>
        public R51(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.comboMDivision1.SetDefalutIndex();
            this.comboFactory1.SetDataSource();
            MyUtility.Tool.SetupCombox(this.comboShift, 1, 1, ",Day,Night");
            DualResult result = DBProxy.Current.Select(null, "select distinct SubProcessID from SubProDefectCode", out DataTable dt);
            if (!result)
            {
                this.ShowErr(result);
            }

            MyUtility.Tool.SetupCombox(this.comboSubprocess, 1, dt);
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.Parameters.Clear();
            this.Sqlcmd.Clear();
            this.sqlCol = string.Empty;
            this.CustomColumnDt = null;
            if (this.dateInspectionDate.Value1.Empty() && this.txtSP.Text.Empty())
            {
                MyUtility.Msg.WarningBox("<Inspection Date>, <SP#> can not all empty!");
                return false;
            }

            string formatCol;
            string formatCol2 = string.Empty;
            string formatCol3 = string.Empty;
            string formatJoin;
            string s_d = $@"
outer apply(select ttlSecond_RD = sum(DATEDIFF(Second, StartResolveDate, EndResolveDate)) from SubProInsRecord_ResponseTeam where EndResolveDate is not null and SubProInsRecordUkey = SR.Ukey)ttlSecond_RD
outer apply(
	select SubProResponseTeamID = STUFF((
		select CONCAT(',', SubProResponseTeamID)
		from SubProInsRecord_ResponseTeam
		where SubProInsRecordUkey = SR.Ukey
		order by SubProResponseTeamID
		for xml path('')
	),1,1,'')
)SubProResponseTeamID
";

            this.sqlCol = $@"select AssignColumn,DisplayName from SubProCustomColumn where SubProcessID = '{this.comboSubprocess.Text}' order by AssignColumn";
            if (this.radioSummary.Checked)
            {
                formatJoin = @"outer apply (select [val] = sum(SRD.DefectQty)
                                            from SubProInsRecord_Defect SRD WITH(NOLOCK)
		                                    where SR.Ukey=SRD.SubProInsRecordUkey ) DefectQty" + s_d;
                formatCol = "DefectQty.val,";
            }
            else if (this.radioDetail_DefectType.Checked)
            {
                formatJoin = @"left join SubProInsRecord_Defect SRD on SR.Ukey = SRD.SubProInsRecordUkey" + s_d;
                formatCol = @"  SRD.DefectCode,
                                SRD.DefectQty,";
            }
            else
            {
                formatJoin = @"left join SubProInsRecord_Defect SRD on SR.Ukey = SRD.SubProInsRecordUkey
left join SubProInsRecord_ResponseTeam SRR on SRR.SubProInsRecordUkey = SR.Ukey
outer apply(select ttlSecond_RD = DATEDIFF(Second, StartResolveDate, EndResolveDate))ttlSecond_RD";
                formatCol = @"  SRD.DefectCode,
                                SRD.DefectQty,";
                formatCol2 = $@"SRR.StartResolveDate,
    SRR.EndResolveDate,
";
            }

            #region where
            StringBuilder sqlwhere1 = new StringBuilder();
            StringBuilder sqlwhere2 = new StringBuilder();
            StringBuilder declare = new StringBuilder();
            if (!this.dateInspectionDate.Value1.Empty())
            {
                sqlwhere1.Append("\r\nand Cast(SR.AddDate as Date) between @InspectionDate1 and @InspectionDate2");
                sqlwhere2.Append("\r\nand Cast(SR.AddDate as Date) between @InspectionDate1 and @InspectionDate2");
                this.Parameters.Add(new SqlParameter("@InspectionDate1p", SqlDbType.Date) { Value = this.dateInspectionDate.Value1 });
                this.Parameters.Add(new SqlParameter("@InspectionDate2p", SqlDbType.Date) { Value = this.dateInspectionDate.Value2 });
                declare.Append("\r\ndeclare @InspectionDate1 Date = @InspectionDate1p\r\ndeclare @InspectionDate2 Date = @InspectionDate2p");
            }

            if (!this.txtSP.Text.Empty())
            {
                sqlwhere1.Append("\r\nand B.OrderID = @SP");
                sqlwhere2.Append("\r\nand BR.OrderID = @SP");
                this.Parameters.Add(new SqlParameter("@SPp", SqlDbType.VarChar, 13) { Value = this.txtSP.Text });
                declare.Append("\r\ndeclare @SP varchar(13) = @SPp");
            }

            if (!this.txtstyle1.Text.Empty())
            {
                sqlwhere1.Append("\r\nand O.StyleID= @Style");
                sqlwhere2.Append("\r\nand O.StyleID= @Style");
                this.Parameters.Add(new SqlParameter("@Stylep", SqlDbType.VarChar, 15) { Value = this.txtstyle1.Text });
                declare.Append("\r\ndeclare @Style varchar(15) = @Stylep");
            }

            if (!this.comboSubprocess.Text.Empty())
            {
                sqlwhere1.Append("\r\nand SR.SubProcessID= @SubProcessID");
                sqlwhere2.Append("\r\nand SR.SubProcessID= @SubProcessID");
                this.Parameters.Add(new SqlParameter("@SubProcessIDp", SqlDbType.VarChar, 10) { Value = this.comboSubprocess.Text });
                declare.Append("\r\ndeclare @SubProcessID varchar(10) = @SubProcessIDp");
            }

            if (!this.comboMDivision1.Text.Empty())
            {
                sqlwhere1.Append("\r\nand B.MDivisionID= @M");
                sqlwhere2.Append("\r\nand BR.MDivisionID= @M");
                this.Parameters.Add(new SqlParameter("@Mp", SqlDbType.VarChar, 8) { Value = this.comboMDivision1.Text });
                declare.Append("\r\ndeclare @M varchar(8) = @Mp");
            }

            if (!this.comboFactory1.Text.Empty())
            {
                sqlwhere1.Append("\r\nand SR.FactoryID = @F");
                sqlwhere2.Append("\r\nand SR.FactoryID = @F");
                this.Parameters.Add(new SqlParameter("@Fp", SqlDbType.VarChar, 8) { Value = this.comboFactory1.Text });
                declare.Append("\r\ndeclare @F varchar(8) = @Fp");
            }

            if (!this.comboShift.Text.Empty())
            {
                sqlwhere1.Append("\r\nand SR.Shift = @Shift");
                sqlwhere2.Append("\r\nand SR.Shift = @Shift");
                this.Parameters.Add(new SqlParameter("@Shiftp", SqlDbType.VarChar, 5) { Value = this.comboShift.Text });
                declare.Append("\r\ndeclare @Shift varchar(5) = @Shiftp");
            }
            #endregion
            this.Sqlcmd.Append(declare);
            this.Sqlcmd.Append($@"
select
    SR.FactoryID,
    SR.SubProLocationID,
	Convert(date,SR.AddDate) as AddDate,
    SR.Shift,
	[RFT] = iif(isnull(BD.Qty, 0) = 0, 0, round((isnull(BD.Qty, 0)- isnull(SR.RejectQty, 0)) / Cast(BD.Qty as float),2)),
	SR.SubProcessID,
	SR.BundleNo,
	B.OrderID,
	BD.BundleGroup,
	O.styleID,
	B.Colorid,
	BD.SizeCode,
	BD.Qty,
	SR.RejectQty,
	SR.Machine,
	{formatCol}
	Inspector = (SELECT CONCAT(a.ID, ':', a.Name) from [ExtendServer].ManufacturingExecution.dbo.Pass1 a WITH (NOLOCK) where a.ID = SR.AddName),
	SR.Remark,
    AddDate2 = SR.AddDate,
    SR.RepairedDatetime,
	RepairedTime = iif(RepairedDatetime is null, null, ttlSecond),
    {formatCol2}
	ResolveTime = iif(isnull(ttlSecond_RD, 0) = 0, null, ttlSecond_RD),
	SubProResponseTeamID
    ,CustomColumn1
into #tmp
from SubProInsRecord SR WITH (NOLOCK)
Left join Bundle_Detail BD WITH (NOLOCK) on SR.BundleNo=BD.BundleNo
Left join Bundle B WITH (NOLOCK) on BD.ID=B.ID
Left join Orders O WITH (NOLOCK) on B.OrderID=O.ID
{formatJoin}
outer apply(select ttlSecond = DATEDIFF(Second, SR.AddDate, RepairedDatetime)) ttlSecond
Where 1=1
");
            this.Sqlcmd.Append(sqlwhere1);
            this.Sqlcmd.Append($@"
UNION

select
    SR.FactoryID,
    SR.SubProLocationID,
	Convert(date,SR.AddDate) as AddDate,
    SR.Shift,
	[RFT] = iif(isnull(BRD.Qty, 0) = 0, 0, round((isnull(BRD.Qty, 0)- isnull(SR.RejectQty, 0)) / Cast(BRD.Qty as float),2)),
	SR.SubProcessID,
	SR.BundleNo,
	BR.OrderID,
	BRD.BundleGroup,
	O.styleID,
	BR.Colorid,
	BRD.SizeCode,
	BRD.Qty,
	SR.RejectQty,
	SR.Machine,
	{formatCol}
	Inspector = (SELECT CONCAT(a.ID, ':', a.Name) from [ExtendServer].ManufacturingExecution.dbo.Pass1 a WITH (NOLOCK) where a.ID = SR.AddName),
	SR.Remark,
    AddDate2 = SR.AddDate,
    SR.RepairedDatetime,
	iif(RepairedDatetime is null, null, ttlSecond),
    {formatCol2}
	iif(isnull(ttlSecond_RD, 0) = 0, null, ttlSecond_RD),
	SubProResponseTeamID
    ,CustomColumn1
from SubProInsRecord SR WITH (NOLOCK)
Left join BundleReplacement_Detail BRD WITH (NOLOCK) on SR.BundleNo=BRD.BundleNo
Left join BundleReplacement BR WITH (NOLOCK) on BRD.ID=BR.ID
Left join Orders O WITH (NOLOCK) on BR.OrderID=O.ID
{formatJoin}
outer apply(select ttlSecond = DATEDIFF(Second, SR.AddDate, RepairedDatetime)) ttlSecond
Where 1=1
");
            this.Sqlcmd.Append(sqlwhere2);
            this.Sqlcmd.Append(@"
select *, BundleNoCT = COUNT(1) over(partition by t.BundleNo)
into #tmp2
from #tmp t

select *
from #tmp2 t
where BundleNoCT = 1--綁包/補料都沒有,在第一段union會合併成一筆
or (BundleNoCT > 1 and isnull(t.Orderid, '') <> '')--綁包/補料其中一個有

drop table #tmp,#tmp2
");

            return true;
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            if (!this.sqlCol.Empty())
            {
                DualResult result = DBProxy.Current.Select(null, this.sqlCol, out this.CustomColumnDt);
                if (!result)
                {
                    return result;
                }
            }

            return DBProxy.Current.Select(null, this.Sqlcmd.ToString(), this.Parameters, out this.PrintData);
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

            this.PrintData.Columns.Remove("BundleNoCT");

            string filename = this.radioSummary.Checked ? "Quality_R51_Summary.xltx" : this.radioDetail_DefectType.Checked ? "Quality_R51_Detail_DefectType.xltx" : "Quality_R51_Detail_Responseteam.xltx";
            Excel.Application excelApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\" + filename); // 預先開啟excel app
            MyUtility.Excel.CopyToXls(this.PrintData, string.Empty, filename, 2, false, null, excelApp, wSheet: excelApp.Sheets[1]);
            Excel.Worksheet worksheet = excelApp.ActiveWorkbook.Worksheets[1];
            if (this.CustomColumnDt != null)
            {
                int col = this.radioSummary.Checked ? 24 : this.radioDetail_DefectType.Checked ? 25 : 27;
                foreach (DataRow dr in this.CustomColumnDt.Rows)
                {
                    worksheet.Cells[1, col] = dr["DisplayName"];
                    col++;
                }
            }

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

        private void TxtShiftTime_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!TimeSpan.TryParse(((Sci.Win.UI.TextBox)sender).Text, out TimeSpan _))
            {
                e.Cancel = true;
                MyUtility.Msg.WarningBox("Incorrect time format");
            }
        }
    }
}
