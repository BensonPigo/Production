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
        private DataTable PrintData;

        /// <inheritdoc/>
        public R51(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.comboMDivision1.SetDefalutIndex();
            this.comboFactory1.SetDataSource();
            MyUtility.Tool.SetupCombox(this.comboShift, 1, 1, ",Day,Night");
            DualResult result = DBProxy.Current.Select(null, "select ArtworkTypeID = '' union all select distinct ArtworkTypeID from SubProDefectCode", out DataTable dt);
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
            if (this.dateInspectionDate.Value1.Empty() && this.txtSP.Text.Empty())
            {
                MyUtility.Msg.WarningBox("<Inspection Date>, <SP#> can not all empty!");
                return false;
            }

            #region where
            StringBuilder sqlwhere1 = new StringBuilder();
            StringBuilder sqlwhere2 = new StringBuilder();
            if (!this.dateInspectionDate.Value1.Empty())
            {
                sqlwhere1.Append("\r\nand Cast(SR.AddDate as Date) between @InspectionDate1 and @InspectionDate2");
                sqlwhere2.Append("\r\nand Cast(SR.AddDate as Date) between @InspectionDate1 and @InspectionDate2");
                this.Parameters.Add(new SqlParameter("@InspectionDate1", this.dateInspectionDate.Value1));
                this.Parameters.Add(new SqlParameter("@InspectionDate2", this.dateInspectionDate.Value2));
            }

            if (!this.txtSP.Text.Empty())
            {
                sqlwhere1.Append("\r\nand B.OrderID = @SP");
                sqlwhere2.Append("\r\nand BR.OrderID = @SP");
                this.Parameters.Add(new SqlParameter("@SP", this.txtSP.Text));
            }

            if (!this.txtstyle1.Text.Empty())
            {
                sqlwhere1.Append("\r\nand O.StyleID= @Style");
                sqlwhere2.Append("\r\nand O.StyleID= @Style");
                this.Parameters.Add(new SqlParameter("@Style", this.txtstyle1.Text));
            }

            if (!this.comboSubprocess.Text.Empty())
            {
                sqlwhere1.Append("\r\nand SR.ArtworkTypeID= @ArtworkTypeID");
                sqlwhere2.Append("\r\nand SR.ArtworkTypeID= @ArtworkTypeID");
                this.Parameters.Add(new SqlParameter("@ArtworkTypeID", this.comboSubprocess.Text));
            }

            if (!this.comboMDivision1.Text.Empty())
            {
                sqlwhere1.Append("\r\nand B.MDivisionID= @M");
                sqlwhere2.Append("\r\nand BR.MDivisionID= @M");
                this.Parameters.Add(new SqlParameter("@M", this.comboMDivision1.Text));
            }

            if (!this.comboFactory1.Text.Empty())
            {
                sqlwhere1.Append("\r\nand O.FtyGroup = @F");
                sqlwhere2.Append("\r\nand O.FtyGroup = @F");
                this.Parameters.Add(new SqlParameter("@F", this.comboFactory1.Text));
            }

            if (!this.comboShift.Text.Empty())
            {
                sqlwhere1.Append("\r\nand CAST(SR.AddDate as time) between @ShiftTime1 and @ShiftTime2");
                sqlwhere2.Append("\r\nand CAST(SR.AddDate as time) between @ShiftTime1 and @ShiftTime2");
                this.Parameters.Add(new SqlParameter("@ShiftTime1", this.txtShiftTime1.Text));
                this.Parameters.Add(new SqlParameter("@ShiftTime2", this.txtShiftTime2.Text));
            }
            #endregion

            this.Sqlcmd.Append(@"
select
	SR.AddDate,
	[RFT] = iif(isnull(BD.Qty, 0) = 0, 0, round((isnull(BD.Qty, 0)- isnull(SR.RejectQty, 0)) / Cast(BD.Qty as float),2)),
	SR.ArtworkTypeID,
	SR.BundleNo,
	B.OrderID,
	BD.BundleGroup,
	O.styleID,
	B.Colorid,
	BD.SizeCode,
	BD.Qty,
	SR.RejectQty,
	SR.Machine,
	DefectCode.DefectCode,
	DefectQty.DefectQty,
	Inspector = dbo.getPass1(SR.AddName),
	SR.Remark
into #tmp
from SubProInsRecord SR 
Left join Bundle_Detail BD on SR.BundleNo=BD.BundleNo
Left join Bundle B on BD.ID=B.ID
Left join Orders O on B.OrderID=O.ID
outer apply(
	select DefectCode = STUFF((
		select CONCAT(CHAR(10), DefectCode)
		from SubProInsRecord_Defect SRD WITH(NOLOCK)
		where SR.Ukey=SRD.SubProInsRecordUkey
		order by SRD.Ukey
		for XML path('')
	),1,1,'')
)DefectCode
outer apply(
	select DefectQty = STUFF((
		select CONCAT(CHAR(10), DefectQty)
		from SubProInsRecord_Defect SRD WITH(NOLOCK)
		where SR.Ukey=SRD.SubProInsRecordUkey
		order by SRD.Ukey
		for XML path('')
	),1,1,'')
)DefectQty
Where 1=1
");
            this.Sqlcmd.Append(sqlwhere1);
            this.Sqlcmd.Append(@"
UNION

select
	SR.AddDate,
	[RFT] = iif(isnull(BRD.Qty, 0) = 0, 0, round((isnull(BRD.Qty, 0)- isnull(SR.RejectQty, 0)) / Cast(BRD.Qty as float),2)),
	SR.ArtworkTypeID,
	SR.BundleNo,
	BR.OrderID,
	BRD.BundleGroup,
	O.styleID,
	BR.Colorid,
	BRD.SizeCode,
	BRD.Qty,
	SR.RejectQty,
	SR.Machine,
	DefectCode.DefectCode,
	DefectQty.DefectQty,
	Inspector = dbo.getPass1(SR.AddName),
	SR.Remark
from SubProInsRecord SR 
Left join BundleReplacement_Detail BRD on SR.BundleNo=BRD.BundleNo
Left join BundleReplacement BR on BRD.ID=BR.ID
Left join Orders O on BR.OrderID=O.ID
outer apply(
	select DefectCode = STUFF((
		select CONCAT(CHAR(10), DefectCode)
		from SubProInsRecord_Defect SRD WITH(NOLOCK)
		where SR.Ukey=SRD.SubProInsRecordUkey
		order by SRD.Ukey
		for XML path('')
	),1,1,'')
)DefectCode
outer apply(
	select DefectQty = STUFF((
		select CONCAT(CHAR(10), DefectQty)
		from SubProInsRecord_Defect SRD WITH(NOLOCK)
		where SR.Ukey=SRD.SubProInsRecordUkey
		order by SRD.Ukey
		for XML path('')
	),1,1,'')
)DefectQty
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

            string filename = "Quality_R51.xltx";
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

        private void ComboShift_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.txtShiftTime1.Enabled = this.txtShiftTime2.Enabled = !this.comboShift.Text.Empty();
            switch (this.comboShift.Text)
            {
                case "Day":
                    this.txtShiftTime1.Text = "07:00";
                    this.txtShiftTime2.Text = "16:00";
                    break;
                case "Night":
                    this.txtShiftTime1.Text = "17:00";
                    this.txtShiftTime2.Text = "23:59";
                    break;
                default:
                    this.txtShiftTime1.Text = this.txtShiftTime2.Text = string.Empty;
                    break;
            }
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
