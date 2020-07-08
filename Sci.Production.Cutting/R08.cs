using Ict;
using Sci.Data;
using Sci.Win;
using System;
using System.Data;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Cutting
{
    public partial class R08 : Win.Tems.PrintForm
    {
        private DataTable[] printData;
        private string Mdivision;
        private string Factory;
        private DateTime? EstCutDate1;
        private DateTime? EstCutDate2;
        private DateTime? ActCutDate1;
        private DateTime? ActCutDate2;
        private string SpreadingNo1;
        private string SpreadingNo2;
        private string CutCell1;
        private string CutCell2;
        private string CuttingSP;
        private decimal? Speed;
        private decimal? WorkHoursDay;

        public R08(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        protected override bool ValidateInput()
        {
            this.Mdivision = this.txtMdivision1.Text;
            this.Factory = this.txtfactory1.Text;
            this.EstCutDate1 = this.dateEstCutDate.Value1;
            this.EstCutDate2 = this.dateEstCutDate.Value2;
            this.ActCutDate1 = this.dateActCutDate.Value1;
            this.ActCutDate2 = this.dateActCutDate.Value2;
            this.SpreadingNo1 = this.txtSpreadingNo1.Text;
            this.SpreadingNo2 = this.txtSpreadingNo2.Text;
            this.CutCell1 = this.txtCell1.Text;
            this.CutCell2 = this.txtCell2.Text;
            this.CuttingSP = this.txtCuttingSp.Text;
            this.Speed = this.numSpeed.Value;
            this.WorkHoursDay = this.numWorkHourDay.Value;
            return base.ValidateInput();
        }

        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            string sqlcmd = string.Empty;
            #region where
            string where = string.Empty;
            if (!MyUtility.Check.Empty(this.Mdivision))
            {
                where += $" and w.MDivisionId = '{this.Mdivision}' ";
            }

            if (!MyUtility.Check.Empty(this.Factory))
            {
                where += $" and w.FactoryID = '{this.Factory}' ";
            }

            if (!MyUtility.Check.Empty(this.EstCutDate1))
            {
                where += $" and w.EstCutDate >= '{((DateTime)this.EstCutDate1).ToString("yyyy/MM/dd")}' ";
            }

            if (!MyUtility.Check.Empty(this.EstCutDate2))
            {
                where += $" and w.EstCutDate <=  '{((DateTime)this.EstCutDate2).ToString("yyyy/MM/dd")}' ";
            }

            if (!MyUtility.Check.Empty(this.ActCutDate1))
            {
                where += $" and co.cDate >= '{((DateTime)this.ActCutDate1).ToString("yyyy/MM/dd")}' ";
            }

            if (!MyUtility.Check.Empty(this.ActCutDate2))
            {
                where += $" and co.cDate <=  '{((DateTime)this.ActCutDate2).ToString("yyyy/MM/dd")}' ";
            }

            if (!MyUtility.Check.Empty(this.SpreadingNo1))
            {
                where += $" and w.SpreadingNoID >= '{this.SpreadingNo1}' ";
            }

            if (!MyUtility.Check.Empty(this.SpreadingNo2))
            {
                where += $" and w.SpreadingNoID <= '{this.SpreadingNo2}' ";
            }

            if (!MyUtility.Check.Empty(this.CutCell1))
            {
                where += $" and w.CutCellid >= '{this.CutCell1}' ";
            }

            if (!MyUtility.Check.Empty(this.CutCell2))
            {
                where += $" and w.CutCellid <= '{this.CutCell2}' ";
            }

            if (!MyUtility.Check.Empty(this.CuttingSP))
            {
                where += $" and w.ID = '{this.CuttingSP}' ";
            }
            #endregion
            sqlcmd = $@"
select w.ID,w.CutRef,w.MDivisionId,ActCutDate=max(co.cDate)
into #tmp1
from WorkOrder w with(nolock) 
left join CuttingOutput_Detail cod with(nolock) on cod.WorkOrderUkey = w.Ukey
left join CuttingOutput co with(nolock) on co.id = cod.id
where 1=1
and isnull(w.CutRef,'') <> ''
{where}
group by w.CutRef,w.MDivisionID,w.ID

select w.*,ActCutDate=co.cDate
into #tmpCutRefNull
from WorkOrder w with(nolock) 
left join CuttingOutput_Detail cod with(nolock) on cod.WorkOrderUkey = w.Ukey
left join CuttingOutput co with(nolock) on co.id = cod.id
where 1=1
and isnull(w.CutRef,'') = ''
{where}


select t.CutRef,t.MDivisionId,t.ActCutDate ,t.ID,
	Layer=sum(w.Layer),
	Cons=sum(w.Cons)
into #tmp2a
from #tmp1 t
inner join WorkOrder w with(nolock) on w.CutRef = t.CutRef and w.MDivisionId = t.MDivisionId
group by t.CutRef,t.MDivisionId,t.ActCutDate ,t.ID

select t.CutRef,t.MDivisionId,t.ActCutDate,t.Layer,t.Cons,t.ID,
	noEXCESSqty=sum(iif(wd.OrderID <> 'EXCESS',wd.Qty,0)),
	EXCESSqty = sum(iif(wd.OrderID =  'EXCESS',wd.Qty,0))
into #tmp2
from #tmp2a t
inner join WorkOrder w with(nolock) on w.CutRef = t.CutRef and w.MDivisionId = t.MDivisionId
inner join WorkOrder_Distribute wd with(nolock) on wd.WorkOrderUkey = w.Ukey
group by t.CutRef,t.MDivisionId,t.ActCutDate,t.Layer,t.Cons,t.ID

select distinct
    MDivisionid=isnull(t.MDivisionid,''),
	FactoryID=isnull(w.FactoryID,''),
    t.ActCutDate,
	w.EstCutDate,
	CutCellid=isnull(w.CutCellid,''),
	SpreadingNoID=isnull(w.SpreadingNoID,''),
	CutplanID=isnull(w.CutplanID,''),
	CutRef=isnull(w.CutRef,''),
	ID=isnull(w.ID,''),
	SubSP=isnull(subSp.SubSP,''),
	StyleID=isnull(o.StyleID,''),
	Size=isnull(size.Size,''),
	t.noEXCESSqty,
	Description=isnull(f.Description,''),
	WeaveTypeID=isnull(f.WeaveTypeID,''),
	FabricCombo=isnull(w.FabricCombo,''),
	MarkerLength=iif(w.Layer=0,0,w.cons/w.Layer),
	PerimeterM=isnull(iif(w.ActCuttingPerimeter not like '%yd%',w.ActCuttingPerimeter,cast(dbo.GetActualPerimeter(w.ActCuttingPerimeter) as nvarchar)),''),
	PerimeterYd=isnull(iif(w.ActCuttingPerimeter not like '%yd%',w.ActCuttingPerimeter,cast(dbo.GetActualPerimeterYd(w.ActCuttingPerimeter) as nvarchar)),''),
	t.Layer,
	SizeCode=isnull(SizeCode.SizeCode,''),
	t.Cons,
	t.EXCESSqty,
	NoofRoll=iif(isnull(NoofRoll.NoofRoll,0)<1,1,isnull(NoofRoll.NoofRoll,0)),
	DyeLot=iif(isnull(DyeLot.DyeLot,0)<1,1,isnull(DyeLot.DyeLot,0)),
	NoofWindow=isnull(t.Cons/t.Layer/1.4,0),
	ActualSpeed=isnull(ActSpd.ActualSpeed,0),
	PreparationTime=isnull(st.PreparationTime,0),
	[ChangeoverTime] = iif(isnull(fr.isRoll,0) = 0,st.ChangeOverUnRollTime,st.ChangeOverRollTime),
	SpreadingSetupTime=isnull(st.SetupTime,0),
	SpreadingTime=isnull(st.SpreadingTime,0),
	SeparatorTime=isnull(st.SeparatorTime,0),
	ForwardTime=isnull(st.ForwardTime,0),
	CuttingSetUpTime=isnull(ct.SetUpTime,0),
	WindowTime=isnull(ct.WindowTime,0),
	Refno=isnull(w.Refno,''),
	WindowLength=isnull(ct.WindowLength,0)
into #tmp3
from #tmp2 t
inner join WorkOrder w with(nolock) on w.CutRef = t.CutRef and w.MDivisionId = t.MDivisionId
inner join orders o with(nolock) on o.id = w.ID
left join Fabric f with(nolock) on f.SCIRefno = w.SCIRefno
left join SpreadingTime st with(nolock) on st.WeaveTypeID = f.WeaveTypeID
left join dbo.SciMES_RefnoRelaxtime rr WITH (NOLOCK) on rr.Refno = w.Refno
left join dbo.SciMES_FabricRelaxation fr WITH (NOLOCK) on rr.FabricRelaxationID = fr.ID
left join CuttingTime ct WITH (NOLOCK) on ct.WeaveTypeID = f.WeaveTypeID
outer apply(
	select SubSP = stuff((
		select distinct concat(',',wd.OrderID)
		from WorkOrder w2 with(nolock)
		inner join WorkOrder_Distribute wd with(nolock) on wd.WorkOrderUkey = w2.Ukey
		where w2.CutRef = t.CutRef and w2.MDivisionId = t.MDivisionId and t.ID=w2.ID
		For XML path('')
	),1,1,'')
)subSp
outer apply(
	select Size = stuff((
		select distinct concat(',',wd.SizeCode)
		from WorkOrder w2 with(nolock)
		inner join WorkOrder_Distribute wd with(nolock) on wd.WorkOrderUkey = w2.Ukey
		where w2.CutRef = t.CutRef and w2.MDivisionId = t.MDivisionId
		For XML path('')
	),1,1,'')
)size
outer apply
(
	select SizeCode = stuff(
	(
		Select concat(', ' , wd.sizecode, '/ ', wd.qty)
		From WorkOrder w2 with(nolock)
		inner join WorkOrder_SizeRatio wd WITH (NOLOCK) on wd.WorkOrderUkey = w2.Ukey
		Where w2.CutRef = t.CutRef and w2.MDivisionId = t.MDivisionId
		For XML path('')
	),1,1,'')
)SizeCode
outer apply(
	select NoofRoll = count(1)
	from(
		select distinct cr.Seq1,cr.seq2,cr.Roll,cr.Dyelot
		from CuttingOutputFabricRecord cr WITH (NOLOCK) 
		where cr.CutRef = w.CutRef and cr.MDivisionId = w.MDivisionId
	)disC
)NoofRoll
outer apply(
	select DyeLot = count(1)
	from(
		select distinct cr.Seq1,cr.seq2,cr.Dyelot
		from CuttingOutputFabricRecord cr WITH (NOLOCK) 
		where cr.CutRef = w.CutRef and cr.MDivisionId = w.MDivisionId
	)disC
)DyeLot
outer apply(	
	select  ActualSpeed
	from CuttingMachine_detail cmd WITH (NOLOCK) 
	inner join CutCell cc WITH (NOLOCK) on cc.CuttingMachineID = cmd.id
	where cc.id = w.CutCellid 
	and t.Layer between cmd.LayerLowerBound and cmd.LayerUpperBound
	and cmd.WeaveTypeID = f.WeaveTypeID 
)ActSpd

union all

select distinct
    MDivisionid=isnull(t.MDivisionid,''),
	FactoryID=isnull(t.FactoryID,''),
    t.ActCutDate,
	t.EstCutDate,
	CutCellid=isnull(t.CutCellid,''),
	SpreadingNoID=isnull(t.SpreadingNoID,''),
	CutplanID=isnull(t.CutplanID,''),
	CutRef=isnull(t.CutRef,''),
	ID=isnull(t.ID,''),
	SubSP=isnull(subSp.SubSP,''),
	StyleID=isnull(o.StyleID,''),
	Size=isnull(size.Size,''),
	EQ.noEXCESSqty,
	Description=isnull(f.Description,''),
	WeaveTypeID=isnull(f.WeaveTypeID,''),
	FabricCombo=isnull(t.FabricCombo,''),
	MarkerLength=iif(t.Layer=0,0,t.cons/t.Layer),
	PerimeterM=isnull(iif(t.ActCuttingPerimeter not like '%yd%',t.ActCuttingPerimeter,cast(dbo.GetActualPerimeter(t.ActCuttingPerimeter) as nvarchar)),''),
	PerimeterYd=isnull(iif(t.ActCuttingPerimeter not like '%yd%',t.ActCuttingPerimeter,cast(dbo.GetActualPerimeterYd(t.ActCuttingPerimeter) as nvarchar)),''),
	t.Layer,
	SizeCode=isnull(SizeCode.SizeCode,''),
	t.Cons,
	EQ.EXCESSqty,
	NoofRoll=iif(isnull(NoofRoll.NoofRoll,0)<1,1,isnull(NoofRoll.NoofRoll,0)),
	DyeLot=iif(isnull(DyeLot.DyeLot,0)<1,1,isnull(DyeLot.DyeLot,0)),
	NoofWindow=isnull(t.Cons/t.Layer/1.4,0),
	ActualSpeed=isnull(ActSpd.ActualSpeed,0),
	PreparationTime=isnull(st.PreparationTime,0),
	[ChangeoverTime] = iif(isnull(fr.isRoll,0) = 0,st.ChangeOverUnRollTime,st.ChangeOverRollTime),
	SpreadingSetupTime=isnull(st.SetupTime,0),
	SpreadingTime=isnull(st.SpreadingTime,0),
	SeparatorTime=isnull(st.SeparatorTime,0),
	ForwardTime=isnull(st.ForwardTime,0),
	CuttingSetUpTime=isnull(ct.SetUpTime,0),
	WindowTime=isnull(ct.WindowTime,0),
	Refno=isnull(t.Refno,''),
	WindowLength=isnull(ct.WindowLength,0)
from #tmpCutRefNull t
inner join orders o with(nolock) on o.id = t.ID
left join Fabric f with(nolock) on f.SCIRefno = t.SCIRefno
left join SpreadingTime st with(nolock) on st.WeaveTypeID = f.WeaveTypeID
left join dbo.SciMES_RefnoRelaxtime rr WITH (NOLOCK) on rr.Refno = t.Refno
left join dbo.SciMES_FabricRelaxation fr WITH (NOLOCK) on rr.FabricRelaxationID = fr.ID
left join CuttingTime ct WITH (NOLOCK) on ct.WeaveTypeID = f.WeaveTypeID
outer apply(
	select SubSP = stuff((
		select distinct concat(',',wd.OrderID)
		from WorkOrder_Distribute wd with(nolock) 
		where wd.WorkOrderUkey=t.Ukey
		For XML path('')
	),1,1,'')
)subSp
outer apply(
	select 
		noEXCESSqty=sum(iif(wd.OrderID <> 'EXCESS',wd.Qty,0)),
		EXCESSqty = sum(iif(wd.OrderID =  'EXCESS',wd.Qty,0))
	from WorkOrder_Distribute wd with(nolock)
	where wd.WorkOrderUkey = t.Ukey
)EQ
outer apply(
	select Size = stuff((
		select distinct concat(',',wd.SizeCode)
		from WorkOrder w2 with(nolock)
		inner join WorkOrder_Distribute wd with(nolock) on wd.WorkOrderUkey = w2.Ukey
		where wd.WorkOrderUkey=t.Ukey
		For XML path('')
	),1,1,'')
)size
outer apply
(
	select SizeCode = stuff(
	(
		Select concat(', ' , wd.sizecode, '/ ', wd.qty)
		From WorkOrder w2 with(nolock)
		inner join WorkOrder_SizeRatio wd WITH (NOLOCK) on wd.WorkOrderUkey = w2.Ukey
		where wd.WorkOrderUkey=t.Ukey
		For XML path('')
	),1,1,'')
)SizeCode
outer apply(
	select NoofRoll = count(1)
	from(
		select distinct cr.Seq1,cr.seq2,cr.Roll,cr.Dyelot
		from CuttingOutputFabricRecord cr WITH (NOLOCK) 
		where cr.CutRef = t.CutRef and cr.MDivisionId = t.MDivisionId
	)disC
)NoofRoll
outer apply(
	select DyeLot = count(1)
	from(
		select distinct cr.Seq1,cr.seq2,cr.Dyelot
		from CuttingOutputFabricRecord cr WITH (NOLOCK) 
		where cr.CutRef = t.CutRef and cr.MDivisionId = t.MDivisionId
	)disC
)DyeLot
outer apply(	
	select  ActualSpeed
	from CuttingMachine_detail cmd WITH (NOLOCK) 
	inner join CutCell cc WITH (NOLOCK) on cc.CuttingMachineID = cmd.id
	where cc.id = t.CutCellid 
	and t.Layer between cmd.LayerLowerBound and cmd.LayerUpperBound
	and cmd.WeaveTypeID = f.WeaveTypeID 
)ActSpd

select MDivisionID,FactoryID,EstCutDate,ActCutDate,CutCellid,SpreadingNoID,CutplanID,CutRef,ID,SubSP,StyleID,Size,noEXCESSqty,Description,WeaveTypeID,FabricCombo,
	MarkerLength,PerimeterYd,Layer,SizeCode,Cons,EXCESSqty,NoofRoll,DyeLot,NoofWindow,ActualSpeed,
	[PreparationTime_min],
	[ChangeoverTime_min],
	[SpreadingSetupTime_min],
	[MachineSpreadingTime_min],
	[Separatortime_min],
	[ForwardTime_min],
	[CuttingSetupTime_min],
	[MachCuttingTime_min],
	[WindowTime_min],
	[TotalSpreadingTime_min] =isnull([PreparationTime_min],0)+isnull([ChangeoverTime_min],0)+isnull([SpreadingSetupTime_min],0)+
							  isnull([MachineSpreadingTime_min],0)+isnull([Separatortime_min],0)+isnull([ForwardTime_min],0)						 ,
	[TotalCuttingTime_min] = isnull([CuttingSetupTime_min],0)+isnull([MachCuttingTime_min],0)+isnull([WindowTime_min],0)					   
into #detail
from #tmp3
outer apply(select PerimeterM_num=iif(isnumeric(PerimeterM)=1,cast(PerimeterM as numeric(20,4)),0))p
outer apply(select [PreparationTime_min]=Round(PreparationTime * iif(Layer=0,0,Cons/Layer)/60.0,2))cal1
outer apply(select [ChangeoverTime_min]=Round(Changeovertime * NoofRoll/60.0,2))cal2
outer apply(select [SpreadingSetupTime_min] = Round(SpreadingSetupTime/60.0,2))cal3
outer apply(select [MachineSpreadingTime_min]  =Round(SpreadingTime * Cons/60.0,2))cal4
outer apply(select [Separatortime_min] = Round(SeparatorTime * (DyeLot -1)/60.0,2))cal5
outer apply(select [ForwardTime_min] = Round(ForwardTime/60.0,2))cal6
outer apply(select [CuttingSetupTime_min]=Round(CuttingSetUpTime/60.0,2))cal7
outer apply(select [MachCuttingTime_min]=Round(iif(isnull(ActualSpeed,0)=0,0,isnull(p.PerimeterM_num,0)/ActualSpeed),2))cal8
outer apply(select [WindowTime_min]=Round(Windowtime * iif(isnull(Layer,0)=0 or isnull(WindowLength,0)=0,0,(Cons/Layer*0.9144)/WindowLength)/60,2))cal9

select * from #detail order by FactoryID,EstCutDate,CutCellid

select MDivisionId=stuff((select distinct concat(',',MDivisionId) from #tmp1 for xml path('')),1,1,'')
select ForecastPeriod=concat(format(min(ActCutDate),'MM/dd'),'-',format(max(ActCutDate),'MM/dd')) from #tmp1
select TotalWorkingDays=count(1) from(select distinct ActCutDate from #tmp1)a
--
select 
	d.SpreadingNoID,
	TotalSpreadingYardage = Sum(d.Cons),
	TotalSpreadingMarkerQty = count(1),
	TotalSpreadingTime_hr=sum(d.TotalSpreadingTime_min)/60.0
from #detail d
where isnull(d.SpreadingNoID,'') <>''
group by d.SpreadingNoID
--
select 
	d.CutCellid,
	CuttingMachDescription = cm.Description,
	AvgCutSpeedMperMin=avg(ActualSpeed),
	TotalCuttingPerimeter = sum(iif(isnumeric(d.PerimeterYd)=1,cast(d.PerimeterYd as numeric(20,4)),0)),
	TotalCutMarkerQty = count(1),
	TotalCutFabricYardage = Sum(d.Cons),
	TotalCuttingTime_hrs = sum(d.TotalCuttingTime_min)/60.0
from #detail d
inner join CutCell cc with(nolock)on cc.ID = d.CutCellid and cc.MDivisionid = d.MDivisionid
left join CuttingMachine cm with(nolock)on cm.ID = cc.CuttingMachineID
where isnull(d.CutCellid,'') <>''
group by d.CutCellid, cm.Description
order by d.CutCellid

drop table #tmp1,#tmp2a,#tmp2,#tmp3,#detail,#tmpCutRefNull
";
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out this.printData);
            if (!result)
            {
                return Result.F(result.ToString());
            }

            return Result.True;
        }

        protected override bool OnToExcel(ReportDefinition report)
        {
            this.ShowWaitMessage("Excel Processing...");
            this.SetCount(this.printData[0].Rows.Count);
            if (this.printData[0].Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                this.HideWaitMessage();
                return false;
            }

            string excelName = "Cutting_R08";
            Excel.Application excelApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + $"\\{excelName}.xltx");
            this.printData[0].Columns.Remove("MDivisionid");
            MyUtility.Excel.CopyToXls(this.printData[0], string.Empty, $"{excelName}.xltx", 1, false, null, excelApp, wSheet: excelApp.Sheets[1]); // 將datatable copy to excel
            excelApp.DisplayAlerts = false;
            Excel.Worksheet worksheet = excelApp.ActiveWorkbook.Worksheets[2]; // 取得工作表
            DataTable sodt = this.printData[4];
            DataTable codt = this.printData[5];
            int s = sodt.Rows.Count;

            worksheet.Cells[1, 2] = this.printData[1].Rows[0][0];
            worksheet.Cells[2, 2] = DateTime.Now;
            worksheet.Cells[1, 6] = this.printData[2].Rows[0][0];
            worksheet.Cells[2, 6] = this.printData[3].Rows[0][0];
            string sCol = MyUtility.Excel.ConvertNumericToExcelColumn(s + 1);
            string cCol = MyUtility.Excel.ConvertNumericToExcelColumn(codt.Rows.Count + 1);
            worksheet.Cells[2, 3] = $"=AVERAGE(B13:{sCol}13)";
            worksheet.Cells[2, 4] = $"=AVERAGE(B29:{cCol}29)";
            worksheet.Range[$"B4:{sCol}13"].Borders.Weight = 2; // 設定全框線
            worksheet.Range[$"B17:{cCol}29"].Borders.Weight = 2; // 設定全框線
            #region Spreading Output
            string col = string.Empty;
            for (int i = 0; i < s; i++)
            {
                worksheet.Cells[4, i + 2] = sodt.Rows[i]["SpreadingNoID"];
                col = MyUtility.Excel.ConvertNumericToExcelColumn(i + 2);
                worksheet.Cells[5, i + 2] = this.WorkHoursDay;
                worksheet.Cells[6, i + 2] = $"={col}5*F$2";
                worksheet.Cells[7, i + 2] = sodt.Rows[i]["TotalSpreadingYardage"];
                worksheet.Cells[8, i + 2] = sodt.Rows[i]["TotalSpreadingMarkerQty"];
                worksheet.Cells[9, i + 2] = sodt.Rows[i]["TotalSpreadingTime_hr"];
                worksheet.Cells[10, i + 2] = $"=({col}9/{col}6)";
                worksheet.Cells[11, i + 2] = $"={col}9-{col}6";
                worksheet.Cells[12, i + 2] = $"=CONCATENATE(IF(({this.Speed}*{col}5)=0,0,Round({col}7/({this.Speed}*{col}5),2)),\" | \",IF({col}6=0,0,Round({col}9/{col}6,2)),\" | \")";
                worksheet.Cells[13, i + 2] = $"=IF(({this.Speed}*{col}5)=0,0,Round({col}7/({this.Speed}*{col}5),2))*IF({col}6=0,0,Round({col}9/{col}6,2))/100";
            }

            col = col.EqualString(string.Empty) ? "A" : col;
            worksheet.get_Range("A3", col + "3").Merge(false);

            #endregion
            #region Cutting Output
            for (int i = 0; i < codt.Rows.Count; i++)
            {
                worksheet.Cells[17, i + 2] = codt.Rows[i]["CutCellid"];
                worksheet.Cells[18, i + 2] = codt.Rows[i]["CuttingMachDescription"];
                col = MyUtility.Excel.ConvertNumericToExcelColumn(i + 2);
                worksheet.Cells[19, i + 2] = this.WorkHoursDay;
                worksheet.Cells[20, i + 2] = $"={col}19*F$2";
                worksheet.Cells[21, i + 2] = codt.Rows[i]["AvgCutSpeedMperMin"];
                worksheet.Cells[22, i + 2] = codt.Rows[i]["TotalCuttingPerimeter"];
                worksheet.Cells[23, i + 2] = codt.Rows[i]["TotalCutMarkerQty"];
                worksheet.Cells[24, i + 2] = codt.Rows[i]["TotalCutFabricYardage"];
                worksheet.Cells[25, i + 2] = codt.Rows[i]["TotalCuttingTime_hrs"];
                worksheet.Cells[26, i + 2] = $"=({col}25/{col}20)";
                worksheet.Cells[27, i + 2] = $"={col}25-{col}20";
                worksheet.Cells[28, i + 2] = $"=CONCATENATE(IF(({this.Speed}*{col}19)=0,0,Round({col}22/({this.Speed}*{col}19),2)),\" | \",IF({col}20=0,0,Round({col}25/{col}20,2)),\" | \")";
                worksheet.Cells[29, i + 2] = $"=IF(({this.Speed}*{col}19)=0,0,Round({col}22/({this.Speed}*{col}19),2))*IF({col}20=0,0,Round({col}25/{col}20,2))/100";
            }

            col = col.EqualString(string.Empty) ? "A" : col;
            worksheet.get_Range("A16", col + "16").Merge(false);
            #endregion
            #region sheet Balancing Chart
            worksheet = excelApp.ActiveWorkbook.Worksheets[3]; // 取得工作表

            for (int i = 0; i < s + codt.Rows.Count - 2; i++)
            {
                worksheet.Rows[i + 3, Type.Missing].Insert(Excel.XlDirection.xlDown);
            }

            for (int i = 0; i < s; i++)
            {
                col = MyUtility.Excel.ConvertNumericToExcelColumn(i + 2);
                worksheet.Cells[i + 2, 1] = $"='Actual Output Summary'!{col}4";
                worksheet.Cells[i + 2, 2] = $"='Actual Output Summary'!{col}8";
                worksheet.Cells[i + 2, 4] = $"='Actual Output Summary'!{col}10";
            }

            for (int i = 0; i < codt.Rows.Count; i++)
            {
                col = MyUtility.Excel.ConvertNumericToExcelColumn(i + 2);
                worksheet.Cells[i + s + 2, 1] = $"='Actual Output Summary'!{col}17";
                worksheet.Cells[i + s + 2, 3] = $"='Actual Output Summary'!{col}23";
                worksheet.Cells[i + s + 2, 4] = $"='Actual Output Summary'!{col}26";
            }

            worksheet.Visible = Microsoft.Office.Interop.Excel.XlSheetVisibility.xlSheetHidden; // 隱藏第3頁sheet
            #endregion
            worksheet = excelApp.ActiveWorkbook.Worksheets[1]; // 取得工作表

            // worksheet.Columns.AutoFit();
            #region 釋放上面開啟過excel物件
            string strExcelName = Class.MicrosoftFile.GetName(excelName);
            Excel.Workbook workbook = excelApp.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            excelApp.Quit();

            if (worksheet != null)
            {
                Marshal.FinalReleaseComObject(worksheet);
            }

            if (excelApp != null)
            {
                Marshal.FinalReleaseComObject(excelApp);
            }
            #endregion

            this.HideWaitMessage();
            strExcelName.OpenFile();
            return true;
        }
    }
}
