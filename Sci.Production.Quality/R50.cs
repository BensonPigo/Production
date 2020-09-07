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
    public partial class R50 : Win.Tems.PrintForm
    {
        private readonly List<SqlParameter> Parameters = new List<SqlParameter>();
        private readonly StringBuilder Sqlcmd = new StringBuilder();
        private DataTable PrintData;

        /// <inheritdoc/>
        public R50(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.comboMDivision1.SetDefalutIndex();
            this.comboFactory1.SetDataSource();
            MyUtility.Tool.SetupCombox(this.comboShift, 1, 1, ",Day,Night");
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
            StringBuilder sqlwhere = new StringBuilder();
            if (!this.dateInspectionDate.Value1.Empty())
            {
                sqlwhere.Append("\r\nand Cast(CR.AddDate as Date) between @InspectionDate1 and @InspectionDate2");
                this.Parameters.Add(new SqlParameter("@InspectionDate1", this.dateInspectionDate.Value1));
                this.Parameters.Add(new SqlParameter("@InspectionDate2", this.dateInspectionDate.Value2));
            }

            if (!this.txtSP.Text.Empty())
            {
                sqlwhere.Append("\r\nand W.ID = @SP");
                this.Parameters.Add(new SqlParameter("@SP", this.txtSP.Text));
            }

            if (!this.txtstyle1.Text.Empty())
            {
                sqlwhere.Append("\r\nand O.StyleID= @Style");
                this.Parameters.Add(new SqlParameter("@Style", this.txtstyle1.Text));
            }

            if (!this.comboMDivision1.Text.Empty())
            {
                sqlwhere.Append("\r\nand O.MDivisionID= @M");
                this.Parameters.Add(new SqlParameter("@M", this.comboMDivision1.Text));
            }

            if (!this.comboFactory1.Text.Empty())
            {
                sqlwhere.Append("\r\nand O.FtyGroup= @F");
                this.Parameters.Add(new SqlParameter("@F", this.comboFactory1.Text));
            }

            if (!this.comboShift.Text.Empty())
            {
                sqlwhere.Append("\r\nand CAST(CR.AddDate as time) between @ShiftTime1 and @ShiftTime2");
                this.Parameters.Add(new SqlParameter("@ShiftTime1", this.txtShiftTime1.Text));
                this.Parameters.Add(new SqlParameter("@ShiftTime2", this.txtShiftTime2.Text));
            }
            #endregion

            this.Sqlcmd.Append(@"
select
	CR.AddDate,
	[RFT] = iif(isnull(CR.InspectQty, 0) = 0, 0, round((isnull(CR.InspectQty, 0)- isnull(CR.RejectQty, 0)) / Cast(CR.InspectQty as float),2)),
	CR.CutRef,
	W.ID,
	O.StyleID,
	O.SeasonID,
	Article.Article,
	ColorID.Colorid,
	Refno.Refno,
	SizeCode.SizeCode,
	W.FabricCombo,
	WS.Ratio,
	Layer.Layer,
	Roll.Roll,
	Dyelot.Dyelot,
	TicketYDS.TicketYDS,
	CR.Machine,
	[CuttingTable] = CR.CutCellId,
	CR.MarkerLength,
	OE.Width,
	CR.CuttableWidth,
	CR.ActualWidth,
	CR.Description,
	CR.[Top],
	CR.Middle,
	CR.Bottom,
	CR.InspRatio,
	DefectCode.DefectCode,
	DefectQty.DefectQty,
	CR.InspectQty,
	CR.RejectQty,
	Inspector = dbo.getPass1(CR.AddName),
	CR.Remark,
    CR.AddDate,
    CR.RepairedDatetime,
	RepairedTime = iif(RepairedDatetime is null,null,
		concat(IIF(ttlMINUTE > 1440, ttlMINUTE / 1440, 0), ' ',
			IIF(ttlMINUTE_D > 60, ttlMINUTE_D / 60, 0), ':',
			isnull(ttlMINUTE_D_HR, 0)))
from CutInsRecord CR WITH(NOLOCK)
outer apply(
	select Roll = STUFF((
		select CONCAT(CHAR(10), Roll)
		from CutInsRecord_RollDyelot CRR WITH(NOLOCK)
		where CR.Ukey=CRR.CutInsRecordUkey
		order by CRR.Ukey
		for XML path('')
	),1,1,'')
)Roll
outer apply(
	select Dyelot = STUFF((
		select CONCAT(CHAR(10), Dyelot)
		from CutInsRecord_RollDyelot CRR WITH(NOLOCK)
		where CR.Ukey=CRR.CutInsRecordUkey
		order by CRR.Ukey
		for XML path('')
	),1,1,'')
)Dyelot
outer apply(
	select TicketYDS = STUFF((
		select CONCAT(CHAR(10), TicketYDS)
		from CutInsRecord_RollDyelot CRR WITH(NOLOCK)
		where CR.Ukey=CRR.CutInsRecordUkey
		order by CRR.Ukey
		for XML path('')
	),1,1,'')
)TicketYDS
outer apply(
	select DefectCode = STUFF((
		select CONCAT(CHAR(10), DefectCode)
		from CutInsRecord_Defect CRD WITH(NOLOCK)
		where CR.Ukey=CRD.CutInsRecordUkey
		order by CRD.Ukey
		for XML path('')
	),1,1,'')
)DefectCode
outer apply(
	select DefectQty = STUFF((
		select CONCAT(CHAR(10), DefectQty)
		from CutInsRecord_Defect CRD WITH(NOLOCK)
		where CR.Ukey=CRD.CutInsRecordUkey
		order by CRD.Ukey
		for XML path('')
	),1,1,'')
)DefectQty
outer apply(select Top 1 * from WorkOrder W WITH(NOLOCK) where CR.CutRef=W.CutRef and CR.MDivisionID=W.MDivisionID)W
Left join Orders O on W.ID=O.ID
Left join Order_EachCons OE on W.ID=OE.ID and W.MarkerName=OE.MarkerName
outer apply(
	select ColorID = STUFF((
		select distinct CONCAT(',', ColorID)
		from WorkOrder W WITH(NOLOCK)
		where CR.CutRef=W.CutRef and CR.MDivisionID=W.MDivisionID
		for XML path('')
	),1,1,'')
)ColorID
outer apply(
	select Refno = STUFF((
		select distinct CONCAT(',', Refno)
		from WorkOrder W WITH(NOLOCK)
		where CR.CutRef=W.CutRef and CR.MDivisionID=W.MDivisionID
		for XML path('')
	),1,1,'')
)Refno
outer apply(
	select Article = STUFF((
		select distinct CONCAT(',', Article)
		from WorkOrder_Distribute WD WITH(NOLOCK)
		where W.Ukey=WD.WorkOrderUkey and WD.OrderID != 'EXCESS'
		for XML path('')
	),1,1,'')
)Article
outer apply(
	select SizeCode = STUFF((
		select distinct CONCAT(',', SizeCode)
		from WorkOrder_Distribute WD WITH(NOLOCK)
		where W.Ukey=WD.WorkOrderUkey and WD.OrderID != 'EXCESS'
		for XML path('')
	),1,1,'')
)SizeCode
outer apply(select Ratio = SUM(Qty) from WorkOrder_SizeRatio WS WITH(NOLOCK) where WS.WorkOrderUkey = W.Ukey)WS
outer apply(select Layer = SUM(Layer) from WorkOrder W WITH(NOLOCK) where CR.CutRef=W.CutRef and CR.MDivisionID=W.MDivisionID)Layer
outer apply(select ttlMINUTE = DATEDIFF(MINUTE, CR.AddDate, CR.RepairedDatetime))ttlMINUTE
outer apply(select ttlMINUTE_D = IIF(ttlMINUTE > 1440, ttlMINUTE - (ttlMINUTE / 1440) * 1440, ttlMINUTE))ttlMINUTE_D
outer apply(select ttlMINUTE_D_HR = IIF(ttlMINUTE_D > 60, ttlMINUTE_D - (ttlMINUTE_D / 60) * 60, ttlMINUTE_D))ttlMINUTE_D_HR
where 1=1
");
            this.Sqlcmd.Append(sqlwhere);
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

            string filename = "Quality_R50.xltx";
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
