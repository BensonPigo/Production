using Ict;
using Sci.Data;
using System;
using System.Data;
using System.Windows.Forms;

namespace Sci.Production.Cutting
{
    /// <inheritdoc/>
    public partial class R16 : Win.Tems.PrintForm
    {
        private string mMDivision;
        private string factory;
        private string CuttingSP1;
        private string CuttingSP2;
        private string Style;
        private DateTime? Est_CutDate1;
        private DateTime? Est_CutDate2;
        private DateTime? ActCuttingDate1;
        private DateTime? ActCuttingDate2;
        private string strWhere;
        private DataTable[] printDatas;

        /// <inheritdoc/>
        public R16(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.comboMDivision.SetDefalutIndex(true);
            this.comboFactory.SetDataSource(this.comboMDivision.Text);
            this.comboMDivision.Text = Env.User.Keyword;
            this.dateEstCutDate.Value1 = DateTime.Now;
            this.dateEstCutDate.Value2 = DateTime.Now;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.strWhere = string.Empty;
            this.mMDivision = this.comboMDivision.Text;
            this.factory = this.comboFactory.Text;
            this.CuttingSP1 = this.txtCuttingSPStart.Text;
            this.CuttingSP2 = this.txtCuttingSPEnd.Text;
            this.Est_CutDate1 = this.dateEstCutDate.Value1;
            this.Est_CutDate2 = this.dateEstCutDate.Value2;

            if (MyUtility.Check.Empty(this.Est_CutDate1) &&
                MyUtility.Check.Empty(this.Est_CutDate1) &&
                MyUtility.Check.Empty(this.CuttingSP1) &&
                MyUtility.Check.Empty(this.CuttingSP2))
            {
                MyUtility.Msg.WarningBox("Please input one of [Est. Cut Date], [Cutting SP#] first.");
                return false;
            }

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            string sqlWhere = string.Empty;
            string sqlMain = string.Empty;
            if (!MyUtility.Check.Empty(this.factory))
            {
                sqlWhere += Environment.NewLine + $" and wo.FactoryID = '{this.factory}'";
            }

            if (!MyUtility.Check.Empty(this.mMDivision))
            {
                sqlWhere += Environment.NewLine + $" and  wo.MDivisionID >=  '{this.mMDivision}'";
            }

            if (!MyUtility.Check.Empty(this.Est_CutDate1))
            {
                sqlWhere += $@" and wo.EstCutDate >= '{System.Convert.ToDateTime(this.Est_CutDate1).ToString("yyyy/MM/dd")}' ";
            }

            if (!MyUtility.Check.Empty(this.Est_CutDate2))
            {
                sqlWhere += $@" and wo.EstCutDate <= '{System.Convert.ToDateTime(this.Est_CutDate2).ToString("yyyy/MM/dd")}' ";
            }

            if (!MyUtility.Check.Empty(this.CuttingSP1))
            {
                sqlWhere += $@" and wo.id >= '{this.CuttingSP1}' ";
            }

            if (!MyUtility.Check.Empty(this.CuttingSP2))
            {
                sqlWhere += $@" and wo.id <= '{this.CuttingSP2}' ";
            }

            sqlMain = $@"
select wo.FactoryID
,wo.CutCellID
,wo.CutRef
,[Sub Cut Refer#] = case 
	when wo.GroupID = '' then wo.CutRef
	when wo.GroupID !='' then subCutRef.val
	end
,wo.CutNo
,[Sub Cut#] = case 
	when wo.GroupID = '' then convert(varchar, wo.CutNo)
	when wo.GroupID !='' then subCutNo.val
	end
,o.StyleID
,wo.OrderID
,[Sub SP] = wod.val
,wo.MarkerName
,wo.MarkerNo
,[Size] = woSR.val
,[Ratio] = woRatio.val
,[OrderQty] = OrderQty.val
,[Combination] = Combination.val
,[Fabric Desc] = isnull(f.Description,'')
,[MarkerLength] = dbo.MarkerLengthToYDS(wo.MarkerLength)
,wo.ColorID
,wo.Tone
,wo.Layer
,Cons = wo.Layer * dbo.MarkerLengthToYDS(wo.MarkerLength)
,[Roll] = wosf.Cnt
,[Layer Spread] = isnull(wosf.Sum_SpreadingLayers,0)
,[Bal Layer] = isnull(wo.Layer,0) - isnull(wosf.Sum_SpreadingLayers,0)
,[Total Yards] = isnull(wosf.Sum_SpreadingLayers,0) * isnull(dbo.MarkerLengthToYDS(wo.MarkerLength),0)
,[Bal Yard] = isnull(wo.Layer * dbo.MarkerLengthToYDS(wo.MarkerLength),0) - isnull(wosf.Sum_SpreadingLayers,0) * isnull(dbo.MarkerLengthToYDS(wo.MarkerLength),0)
,[Merge Fabric] = isnull(wosf.Sum_MergeFabYards,0)
,[Use Cutends] = isnull(wosf.Sum_UseCutendsYards,0)
,[Damage] = isnull(wosf.Sum_DamageYards,0)
,[Act Cutends] = isnull(wosf.Sum_ActCutends,0)
,[Ori Cutends] = isnull(wosf.Sum_TicketYards,0) - (
(isnull(wosf.Sum_SpreadingLayers,0) * isnull(dbo.MarkerLengthToYDS(wo.MarkerLength),0)) - isnull(wosf.Sum_MergeFabYards,0) - isnull(wosf.Sum_UseCutendsYards,0) + isnull(wosf.Sum_DamageYards,0))
,[Variance] = isnull(wosf.Sum_ActCutends,0) - (isnull(wosf.Sum_TicketYards,0) - ((isnull(wosf.Sum_SpreadingLayers,0) * isnull(dbo.MarkerLengthToYDS(wo.MarkerLength),0)) - isnull(wosf.Sum_MergeFabYards,0) - isnull(wosf.Sum_UseCutendsYards,0) + isnull(wosf.Sum_DamageYards,0))),[Remark] = wo.SpreadingRemark
,wo.SpreadingNoID
,wo.Spreader
,[StartSpreadTime] = wosfTime.AddDate
,[EndSpreadTime] = wosfTime.UpdDate
from WorkOrderForOutput wo with(nolock)
left join Orders o with(nolock) on o.ID = wo.OrderID
left join Fabric f with(nolock) on f.SCIRefno = wo.SCIRefNo
outer apply(
	select val = Stuff((
		select concat(',',CutRef)
		from (
				select distinct CutRef
				from WorkOrderForOutputHistory woh with(nolock)
				where woh.GroupID = wo.GroupID
			) s
		for xml path ('')
	) , 1, 1, '')
) subCutRef
outer apply(
	select val = Stuff((
		select concat(',',CutNo)
		from (
				select distinct CutNo
				from WorkOrderForOutputHistory woh with(nolock)
				where woh.GroupID = wo.GroupID
			) s
		for xml path ('')
	) , 1, 1, '')
) subCutNo
outer apply(
	select val = Stuff((
		select concat(',',OrderID)
		from (
				select distinct wod.OrderID
				from WorkOrderForOutput_Distribute wod with(nolock) 
				where wo.Ukey = wod.WorkOrderForOutputUkey
			) s
		for xml path ('')
	) , 1, 1, '')
) wod
outer apply(
	select val = Stuff((
		select concat(',',SizeCode)
		from (
				select distinct wosr.SizeCode
				from WorkOrderForOutput_SizeRatio wosr with(nolock) 
				where wo.Ukey = wosr.WorkOrderForOutputUkey
			) s
		for xml path ('')
	) , 1, 1, '')
) woSR
outer apply(
	select val = Stuff((
		select Concat (',',Concat (SizeCode, '/', Qty))
		from (
				select SizeCode,Qty
				from WorkOrderForOutput_SizeRatio wosr with(nolock) 
				where wo.Ukey = wosr.WorkOrderForOutputUkey
			) s
		for xml path ('')
	) , 1, 1, '')
) woRatio
outer apply(
	select val = sum(Qty)
	from WorkOrderForOutput_Distribute wod with(nolock) 
	where wo.Ukey = wod.WorkOrderForOutputUkey
) OrderQty
outer apply(
	select val = Stuff((
		select concat(',',PatternPanel)
		from (
				select wopp.PatternPanel
				from WorkOrderForOutput_PatternPanel wopp with(nolock) 
				where wo.Ukey = wopp.WorkOrderForOutputUkey
			) s
		for xml path ('')
	) , 1, 1, '')
) Combination
outer apply(
	select Sum_SpreadingLayers = sum(SpreadingLayers)
	,Sum_MergeFabYards = sum(MergeFabYards)
	,Sum_UseCutendsYards = sum(UseCutendsYards)
	,Sum_DamageYards = sum(DamageYards)
	,Sum_ActCutends = sum(ActCutends)
	,Sum_TicketYards = sum(TicketYards)
	,Cnt = count(1)
	from WorkOrderForOutput_SpreadingFabric wosf with(nolock)
	where wosf.CutRef = wo.CutRef
	and wosf.SCIRefno = wo.SCIRefno
	and wosf.ColorID = wo.ColorID
	and wosf.Tone = wo.Tone
)wosf
outer apply(
	select AddDate = min(AddDate), UpdDate = MAX(UpdDate)
	from WorkOrderForOutput_SpreadingFabric wosf with(nolock)
	where wosf.CutRef = wo.CutRef
)wosfTime
where 1=1
{sqlWhere}

-- Detail
select wo.FactoryID
,wo.CutCellID
,wo.CutRef
,[Sub Cut Refer#] = case 
	when wo.GroupID = '' then wo.CutRef
	when wo.GroupID !='' then subCutRef.val
	end
,wo.CutNo
,[Sub Cut#] = case 
	when wo.GroupID = '' then convert(varchar, wo.CutNo)
	when wo.GroupID !='' then subCutNo.val
	end
,o.StyleID
,wo.OrderID
,[Sub SP] = wod.val
,wo.MarkerName
,wo.MarkerNo
,[Size] = woSR.val
,[Ratio] = woRatio.val
,[OrderQty] = OrderQty.val
,[Combination] = Combination.val
,[Fabric Desc] = isnull(f.Description,'')
,[MarkerLength] = dbo.MarkerLengthToYDS(wo.MarkerLength)
,wo.ColorID
,wo.Tone
,[Origin Tone] = fi.[Tone]
,wo.Layer
,cons = wo.Layer * dbo.MarkerLengthToYDS(wo.MarkerLength)
,[Seq] = concat(wosf.Seq1,' ',wosf.seq2)
,[Dyelot] = wosf.Dyelot
,[Roll] = wosf.Roll
,[Layer Spread] = wosf.SpreadingLayers
,[Total Yards] = isnull(wosf.SpreadingLayers,0) * isnull(dbo.MarkerLengthToYDS(wo.MarkerLength),0)
,[Merge Fabric] = isnull(wosf.MergeFabYards,0)
,[Use Cutends] = isnull(wosf.UseCutendsYards,0)
,[Damage] = isnull(wosf.DamageYards,0)
,[Act Cutends] = isnull(wosf.ActCutends,0)
,[Ori Cutends] = isnull(wosf.TicketYards,0) - ((isnull(wosf.SpreadingLayers,0) * isnull(dbo.MarkerLengthToYDS(wo.MarkerLength),0)) - isnull(wosf.MergeFabYards,0) - isnull(wosf
.UseCutendsYards,0) + isnull(wosf.DamageYards,0))
,[Variance] =  isnull(wosf.ActCutends,0) - (isnull(wosf.TicketYards,0) - ((isnull(wosf.SpreadingLayers,0) * isnull(dbo.MarkerLengthToYDS(wo.MarkerLength),0)) - isnull(wosf.MergeFabYards,0) - isnull(wosf
.UseCutendsYards,0) + isnull(wosf.DamageYards,0)))
,[Remark] = wosf.Remark
,wo.SpreadingNoID
,wosf.updname
,[StartSpreadTime] = wosf.AddDate
,[EndSpreadTime] = wosf.UpdDate
from WorkOrderForOutput wo with(nolock)
left join Orders o with(nolock) on o.ID = wo.OrderID
left join Fabric f with(nolock) on f.SCIRefno = wo.SCIRefNo
inner join WorkOrderForOutput_SpreadingFabric wosf with(nolock) on wosf.CutRef = wo.CutRef
	and wosf.SCIRefno = wo.SCIRefno
	and wosf.ColorID = wo.ColorID
	and wosf.Tone = wo.Tone
left join FtyInventory fi with(nolock) on fi.POID = wosf.POID and fi.Seq1 = wosf.Seq1
	and fi.Seq2 = wosf.Seq2 and fi.Roll = wosf.Roll and fi.Dyelot = wosf.Dyelot and fi.StockType = 'B'	
outer apply(
	select val = Stuff((
		select concat(',',CutRef)
		from (
				select distinct CutRef
				from WorkOrderForOutputHistory woh with(nolock)
				where woh.GroupID = wo.GroupID
			) s
		for xml path ('')
	) , 1, 1, '')
) subCutRef
outer apply(
	select val = Stuff((
		select concat(',',CutNo)
		from (
				select distinct CutNo
				from WorkOrderForOutputHistory woh with(nolock)
				where woh.GroupID = wo.GroupID
			) s
		for xml path ('')
	) , 1, 1, '')
) subCutNo
outer apply(
	select val = Stuff((
		select concat(',',OrderID)
		from (
				select distinct wod.OrderID
				from WorkOrderForOutput_Distribute wod with(nolock) 
				where wo.Ukey = wod.WorkOrderForOutputUkey
			) s
		for xml path ('')
	) , 1, 1, '')
) wod
outer apply(
	select val = Stuff((
		select concat(',',SizeCode)
		from (
				select distinct wosr.SizeCode
				from WorkOrderForOutput_SizeRatio wosr with(nolock) 
				where wo.Ukey = wosr.WorkOrderForOutputUkey
			) s
		for xml path ('')
	) , 1, 1, '')
) woSR
outer apply(
	select val = Stuff((
		select Concat (',',Concat (SizeCode, '/', Qty))
		from (
				select SizeCode,Qty
				from WorkOrderForOutput_SizeRatio wosr with(nolock) 
				where wo.Ukey = wosr.WorkOrderForOutputUkey
			) s
		for xml path ('')
	) , 1, 1, '')
) woRatio
outer apply(
	select val = sum(Qty)
	from WorkOrderForOutput_Distribute wod with(nolock) 
	where wo.Ukey = wod.WorkOrderForOutputUkey
) OrderQty
outer apply(
	select val = Stuff((
		select concat(',',PatternPanel)
		from (
				select wopp.PatternPanel
				from WorkOrderForOutput_PatternPanel wopp with(nolock) 
				where wo.Ukey = wopp.WorkOrderForOutputUkey
			) s
		for xml path ('')
	) , 1, 1, '')
) Combination
where 1=1
{sqlWhere}
";
            DualResult result = DBProxy.Current.Select(null, sqlMain, out this.printDatas);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return Ict.Result.True;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.printDatas[0].Rows.Count);

            if (this.printDatas[0].Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            this.ShowWaitMessage("Excel Processing...");
            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Cutting_R16.xltx"); // 預先開啟excel app
            MyUtility.Excel.CopyToXls(this.printDatas[0], string.Empty, "Cutting_R16.xltx", 1, false, null, objApp, wSheet: objApp.Sheets[1]);

            if (this.printDatas[1].Rows.Count > 0)
            {
                MyUtility.Excel.CopyToXls(this.printDatas[1], string.Empty, "Cutting_R16.xltx", 1, false, null, objApp, wSheet: objApp.Sheets[2]);      // 將datatable copy to excel
            }

            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
            objSheets.Columns[9].columnwidth = 20;
            objSheets.Columns[2].columnwidth = 10;
            Microsoft.Office.Interop.Excel.Worksheet objSheets2 = objApp.ActiveWorkbook.Worksheets[2];   // 取得工作表
            objSheets2.Columns[9].columnwidth = 20;

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("Cutting_R16");
            Microsoft.Office.Interop.Excel.Workbook workbook = objApp.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            objApp.Quit();
            strExcelName.OpenFile();
            #endregion
            this.HideWaitMessage();
            return true;
        }
    }
}
