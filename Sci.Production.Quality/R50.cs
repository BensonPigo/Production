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

            string sql;
            if (this.radioSummary.Checked)
            {
                sql = $@"
select 
     Convert(date,CR.AddDate),
     CR.Shift,
     [RFT] = Round(iif(CheckTimes.TotalCnt = 0, 0, (convert(decimal(6,3),RollDyelot.EmptyCnt)/CheckTimes.TotalCnt)*100),2),
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
     CR.Machine,
     [CuttingTable] = CR.CutCellId,
     CR.MarkerLength,
     OE.Width,
     CR.CuttableWidth,
     CR.ActualWidth,
     CheckTimes.[Top],
     CheckTimes.Middle,
     CheckTimes.Bottom,
     CheckTimes.[T/M/B],
     RollDyelot.InspRatio,
     InspectQty = CR.InspectQty,
     RejectQty = RollDyelot.NotEmptyCnt,
     DefectQty = CR.DefectQty,
     Inspector = (SELECT CONCAT(a.ID, ':', a.Name) from [ExtendServer].ManufacturingExecution.dbo.Pass1 a WITH (NOLOCK) where a.ID = CR.AddName),
     CR.Remark,
     CR.AddDate	
from CutInsRecord CR WITH(NOLOCK)
outer apply(select Top 1 * from WorkOrderForOutput W WITH(NOLOCK) where CR.CutRef=W.CutRef )W
Left join Orders O on W.ID=O.ID
Left join Order_EachCons OE on W.ID=OE.ID and W.MarkerName=OE.MarkerName and OE.MarkerNo = W.MarkerNo and OE.MarkerVersion = W.MarkerVersion
outer apply (select  isnull(sum(iif(DefectCode = '',1,0)),0) EmptyCnt ,
                     isnull(sum(iif(DefectCode != '',1,0)),0) NotEmptyCnt,
                     isnull(sum(InspRatio),0) InspRatio,
                     count(1) DataCnt
               from CutInsRecord_RollDyelot where CutInsRecordUkey = CR.Ukey) RollDyelot
outer apply (
select [Top]=sum(iif(TMB = '1',1,0)),
      Middle=sum(iif(TMB = '2',1,0)),
      Bottom=sum(iif(TMB = '3',1,0)),
      [T/M/B] = sum(iif(TMB = '4',1,0)),
      isnull(count(1),0) TotalCnt
 from CutInsRecord_RollDyelot
where CutInsRecordUkey = CR.Ukey ) CheckTimes
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
	select ColorID = STUFF((
		select distinct CONCAT(',', ColorID)
		from WorkOrderForOutput W WITH(NOLOCK)
		where CR.CutRef=W.CutRef
		for XML path('')
	),1,1,'')
)ColorID
outer apply(
	select Refno = STUFF((
		select distinct CONCAT(',', Refno)
		from WorkOrderForOutput W WITH(NOLOCK)
		where CR.CutRef=W.CutRef
		for XML path('')
	),1,1,'')
)Refno
outer apply(
	select Article = STUFF((
		select distinct CONCAT(',', Article)
		from WorkOrderForOutput_Distribute WD WITH(NOLOCK)
		where W.Ukey=WD.WorkOrderForOutputUkey and WD.OrderID != 'EXCESS'
		for XML path('')
	),1,1,'')
)Article
outer apply(
	select SizeCode = STUFF((
		select distinct CONCAT(',', SizeCode)
		from WorkOrderForOutput_Distribute WD WITH(NOLOCK)
		where W.Ukey=WD.WorkOrderForOutputUkey and WD.OrderID != 'EXCESS'
		for XML path('')
	),1,1,'')
)SizeCode
outer apply(select Ratio = SUM(Qty) from WorkOrderForOutput_SizeRatio WS WITH(NOLOCK) where WS.WorkOrderForOutputUkey = W.Ukey)WS
outer apply(select Layer = SUM(Layer) from WorkOrderForOutput W WITH(NOLOCK) where CR.CutRef=W.CutRef )Layer
where 1=1";
            }
            else
            {
                sql = $@"
select 
    Convert(date,CR.AddDate),
    CR.Shift,
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
    CRR.Roll,
    CRR.Dyelot,
    CRR.TicketYDS,
    CRR.CutPartName,
    CRR.InspRatio,
    TMBList.Name,
    DeviationList.Name,
    CRR.DefectCode,
    CR.Machine,
    [CuttingTable] = CR.CutCellId,
    CR.MarkerLength,
    OE.Width,
    CR.CuttableWidth,
    CR.ActualWidth,
    Inspector = (SELECT CONCAT(a.ID, ':', a.Name) from [ExtendServer].ManufacturingExecution.dbo.Pass1 a WITH (NOLOCK) where a.ID = CR.AddName),
    CR.Remark,
    CR.AddDate
from CutInsRecord CR WITH(NOLOCK)
outer apply(select Top 1 * from WorkOrderForOutput W WITH(NOLOCK) where CR.CutRef=W.CutRef )W
 left join Orders O WITH(NOLOCK) on W.ID=O.ID
 left join Order_EachCons OE WITH(NOLOCK) on W.ID=OE.ID and W.MarkerName=OE.MarkerName and OE.MarkerNo = W.MarkerNo and OE.MarkerVersion = W.MarkerVersion
 left join CutInsRecord_RollDyelot CRR WITH(NOLOCK) ON  CR.Ukey = CRR.CutInsRecordUkey
outer apply(select Ratio = SUM(Qty) from WorkOrderForOutput_SizeRatio WS WITH(NOLOCK) where WS.WorkOrderForOutputUkey = W.Ukey)WS
outer apply(select Layer = SUM(Layer) from WorkOrderForOutput W WITH(NOLOCK) where CR.CutRef=W.CutRef )Layer
outer apply(
	select ColorID = STUFF((
		select distinct CONCAT(',', ColorID)
		from WorkOrderForOutput W WITH(NOLOCK)
		where CR.CutRef=W.CutRef
		for XML path('')
	),1,1,'')
)ColorID
outer apply(
	select Refno = STUFF((
		select distinct CONCAT(',', Refno)
		from WorkOrderForOutput W WITH(NOLOCK)
		where CR.CutRef=W.CutRef
		for XML path('')
	),1,1,'')
)Refno
outer apply(
	select Article = STUFF((
		select distinct CONCAT(',', Article)
		from WorkOrderForOutput_Distribute WD WITH(NOLOCK)
		where W.Ukey=WD.WorkOrderForOutputUkey and WD.OrderID != 'EXCESS'
		for XML path('')
	),1,1,'')
)Article
outer apply(
	select SizeCode = STUFF((
		select distinct CONCAT(',', SizeCode)
		from WorkOrderForOutput_Distribute WD WITH(NOLOCK)
		where W.Ukey=WD.WorkOrderForOutputUkey and WD.OrderID != 'EXCESS'
		for XML path('')
	),1,1,'')
)SizeCode
 left join DropDownList TMBList WITH(NOLOCK) on TMBList.type = 'PMS_CutInspTMB' and TMBList.ID = CRR.TMB
 left join DropDownList DeviationList WITH(NOLOCK) on DeviationList.type = 'PMS_CutInspDeviation' and DeviationList.ID = CRR.DeviationValue
where 1=1";
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
                sqlwhere.Append("\r\nand CR.FactoryID= @F");
                this.Parameters.Add(new SqlParameter("@F", this.comboFactory1.Text));
            }

            if (!this.comboShift.Text.Empty())
            {
                sqlwhere.Append("\r\nand CR.Shift = @Shift");
                this.Parameters.Add(new SqlParameter("@Shift", this.comboShift.Text));
            }
            #endregion

            this.Sqlcmd.Append(sql);
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

            string filename = this.radioSummary.Checked ? "Quality_R50_Summary.xltx" : "Quality_R50_Detail.xltx";
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
