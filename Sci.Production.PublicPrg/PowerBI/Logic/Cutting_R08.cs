using Sci.Data;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace Sci.Production.Prg.PowerBI.Logic
{
    /// <inheritdoc/>
    public class Cutting_R08
    {
        private DBProxy DBProxy;

        /// <inheritdoc/>
        public Cutting_R08()
        {
            this.DBProxy = new DBProxy()
            {
                DefaultTimeout = 1800,
            };
        }

        /// <summary>
        /// 根據指定的篩選條件，取得實際裁剪產出資料。
        /// </summary>
        /// <remarks>
        /// 此方法會根據提供的篩選條件組合並執行 SQL 查詢，以取得詳細的裁剪產出資料。
        /// 查詢內容包含多個資料表的連接與彙總，以產生完整的產出資訊。
        /// <paramref name="model"/> 參數中的篩選條件會動態組成查詢條件。
        /// 若 <see cref="Cutting_R08_ViewModel.IsPowerBI"/> 設為 <see langword="true"/>，則會套用特定的篩選條件以符合 Power BI 的需求。
        /// 回傳的 <see cref="Base_ViewModel"/> 會包含查詢結果及一組 <see cref="DataTable"/> 陣列，代表詳細資料。
        /// </remarks>
        /// <param name="model">
        /// 一個 <see cref="Cutting_R08_ViewModel"/> 實例，包含篩選參數，例如事業部 ID、廠別 ID、預計與實際裁剪日期及其他相關條件。
        /// </param>
        /// <returns>
        /// 一個 <see cref="Base_ViewModel"/> 物件，包含查詢結果及篩選後的資料表陣列。
        /// 若操作失敗，<see cref="Base_ViewModel.Result"/> 屬性會為 <see langword="false"/>。
        /// </returns>
        public Base_ViewModel GetActualCutOutput(Cutting_R08_ViewModel model)
        {
            string strWhere = string.Empty;
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@MDivisionID", model.MDivisionID),
                new SqlParameter("@FactoryID", model.FactoryID),
                new SqlParameter("@EstCutDateStart", model.EstCutDate1),
                new SqlParameter("@EstCutDateEnd", model.EstCutDate2),
                new SqlParameter("@ActCutDateStart", model.ActCutDate1),
                new SqlParameter("@ActCutDateEnd", model.ActCutDate2),
                new SqlParameter("@CuttingSP", model.CuttingSP),
            }
            ;

            if (!MyUtility.Check.Empty(model.MDivisionID))
            {
                strWhere += " and co.MDivisionId = @MDivisionID ";
            }

            if (!MyUtility.Check.Empty(model.FactoryID))
            {
                strWhere += " and w.FactoryID = @FactoryID ";
            }

            if (!MyUtility.Check.Empty(model.EstCutDate1))
            {
                strWhere += " and w.EstCutDate >= @EstCutDateStart ";
            }

            if (!MyUtility.Check.Empty(model.EstCutDate2))
            {
                strWhere += " and w.EstCutDate <= @EstCutDateEnd ";
            }

            if (!MyUtility.Check.Empty(model.ActCutDate1))
            {
                strWhere += " and co.cDate >= @ActCutDateStart ";
            }

            if (!MyUtility.Check.Empty(model.ActCutDate2))
            {
                strWhere += " and co.cDate <= @ActCutDateEnd ";
            }

            if (!MyUtility.Check.Empty(model.CuttingSP))
            {
                strWhere += " and w.ID = @CuttingSP ";
            }

            if (model.IsPowerBI)
            {
                strWhere = " and ((w.EstCutDate >= @EstCutDateStart and w.EstCutDate <= @EstCutDateEnd) or (co.cDate >= @ActCutDateStart and co.cDate >= @ActCutDateEnd)) ";
            }

            string sqlCmd = $@"
select w.ID,w.CutRef,w.MDivisionId,ActCutDate=max(co.cDate),w.WorkOrderForPlanningUkey
into #tmp1
from WorkOrderForOutput w with(nolock) 
left join CuttingOutput_Detail cod with(nolock) on cod.WorkOrderForOutputUkey = w.Ukey
left join CuttingOutput co with(nolock) on co.id = cod.id
where isnull(w.CutRef,'') <> ''
{strWhere}
group by w.CutRef,w.MDivisionID,w.ID,w.WorkOrderForPlanningUkey

select w.*,ActCutDate=co.cDate
into #tmpCutRefNull
from WorkOrderForOutput w with(nolock) 
left join CuttingOutput_Detail cod with(nolock) on cod.WorkOrderForOutputUkey = w.Ukey
left join CuttingOutput co with(nolock) on co.id = cod.id
where isnull(w.CutRef,'') = ''
{strWhere}


select t.CutRef,t.MDivisionId,t.ActCutDate ,t.ID,t.WorkOrderForPlanningUkey,
	Layer=sum(w.Layer),
	Cons=sum(w.Cons)
into #tmp2a
from #tmp1 t
inner join WorkOrderForOutput w with(nolock) on w.CutRef = t.CutRef
group by t.CutRef,t.MDivisionId,t.ActCutDate ,t.ID,t.WorkOrderForPlanningUkey

select t.CutRef,t.MDivisionId,t.ActCutDate,t.Layer,t.Cons,t.ID,t.WorkOrderForPlanningUkey,
	noEXCESSqty=sum(iif(wd.OrderID <> 'EXCESS',wd.Qty,0)),
	EXCESSqty = sum(iif(wd.OrderID =  'EXCESS',wd.Qty,0))
into #tmp2
from #tmp2a t
inner join WorkOrderForOutput w with(nolock) on w.CutRef = t.CutRef
inner join WorkOrderForOutput_Distribute wd with(nolock) on wd.WorkOrderForOutputUkey = w.Ukey
group by t.CutRef,t.MDivisionId,t.ActCutDate,t.Layer,t.Cons,t.ID,t.WorkOrderForPlanningUkey

select distinct
    MDivisionid=isnull(t.MDivisionid,''),
	FactoryID=isnull(w.FactoryID,''),
    t.ActCutDate,
	w.EstCutDate,
	CutCellid=isnull(w.CutCellid,''),
	SpreadingNoID=isnull(w.SpreadingNoID,''),
	CutplanID=isnull(wofp.CutplanID,''),
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
	{(model.IsPowerBI ? ", [WorkOrderUkey] = w.Ukey" : string.Empty)}
into #tmp3
from #tmp2 t
left join WorkOrderForPlanning wofp WITH(NOLOCK) on t.WorkOrderForPlanningUkey =  wofp.Ukey
OUTER APPLY(SELECT TOP 1 * FROM WorkOrderForOutput w with(nolock) WHERE w.CutRef = t.CutRef)w
inner join orders o with(nolock) on o.id = w.ID
left join Fabric f with(nolock) on f.SCIRefno = w.SCIRefno
left join SpreadingTime st with(nolock) on st.WeaveTypeID = f.WeaveTypeID
left join dbo.SciMES_RefnoRelaxtime rr WITH (NOLOCK) on rr.Refno = w.Refno
left join dbo.SciMES_FabricRelaxation fr WITH (NOLOCK) on rr.FabricRelaxationID = fr.ID
left join CuttingTime ct WITH (NOLOCK) on ct.WeaveTypeID = f.WeaveTypeID
outer apply(
	select SubSP = stuff((
		select distinct concat(',',wd.OrderID)
		from WorkOrderForOutput w2 with(nolock)
		inner join WorkOrderForOutput_Distribute wd with(nolock) on wd.WorkOrderForOutputUkey = w2.Ukey
		where w2.CutRef = t.CutRef and t.ID=w2.ID
		For XML path('')
	),1,1,'')
)subSp
outer apply(
	select Size = stuff((
		select distinct concat(',',wd.SizeCode)
		from WorkOrderForOutput w2 with(nolock)
		inner join WorkOrderForOutput_Distribute wd with(nolock) on wd.WorkOrderForOutputUkey = w2.Ukey
		where w2.CutRef = t.CutRef
		For XML path('')
	),1,1,'')
)size
outer apply
(
	select SizeCode = stuff(
	(
		Select concat(', ' , wd.sizecode, '/ ', wd.qty)
		From WorkOrderForOutput w2 with(nolock)
		inner join WorkOrderForOutput_SizeRatio wd WITH (NOLOCK) on wd.WorkOrderForOutputUkey = w2.Ukey
		Where w2.CutRef = t.CutRef
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
    and cc.MDivisionid = t.mdivisionid
)ActSpd

union all

select distinct
    MDivisionid=isnull(t.MDivisionid,''),
	FactoryID=isnull(t.FactoryID,''),
    t.ActCutDate,
	t.EstCutDate,
	CutCellid=isnull(t.CutCellid,''),
	SpreadingNoID=isnull(t.SpreadingNoID,''),
	CutplanID=isnull(wofp.CutplanID,''),
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
	{(model.IsPowerBI ? ", [WorkOrderUkey] = t.Ukey" : string.Empty)}
from #tmpCutRefNull t
left join WorkOrderForPlanning wofp WITH(NOLOCK) on t.WorkOrderForPlanningUkey =  wofp.Ukey
inner join orders o with(nolock) on o.id = t.ID
left join Fabric f with(nolock) on f.SCIRefno = t.SCIRefno
left join SpreadingTime st with(nolock) on st.WeaveTypeID = f.WeaveTypeID
left join dbo.SciMES_RefnoRelaxtime rr WITH (NOLOCK) on rr.Refno = t.Refno
left join dbo.SciMES_FabricRelaxation fr WITH (NOLOCK) on rr.FabricRelaxationID = fr.ID
left join CuttingTime ct WITH (NOLOCK) on ct.WeaveTypeID = f.WeaveTypeID
outer apply(
	select SubSP = stuff((
		select distinct concat(',',wd.OrderID)
		from WorkOrderForOutput_Distribute wd with(nolock) 
		where wd.WorkOrderForOutputUkey=t.Ukey
		For XML path('')
	),1,1,'')
)subSp
outer apply(
	select 
		noEXCESSqty=sum(iif(wd.OrderID <> 'EXCESS',wd.Qty,0)),
		EXCESSqty = sum(iif(wd.OrderID =  'EXCESS',wd.Qty,0))
	from WorkOrderForOutput_Distribute wd with(nolock)
	where wd.WorkOrderForOutputUkey = t.Ukey
)EQ
outer apply(
	select Size = stuff((
		select distinct concat(',',wd.SizeCode)
		from WorkOrderForOutput w2 with(nolock)
		inner join WorkOrderForOutput_Distribute wd with(nolock) on wd.WorkOrderForOutputUkey = w2.Ukey
		where wd.WorkOrderForOutputUkey=t.Ukey
		For XML path('')
	),1,1,'')
)size
outer apply
(
	select SizeCode = stuff(
	(
		Select concat(', ' , wd.sizecode, '/ ', wd.qty)
		From WorkOrderForOutput w2 with(nolock)
		inner join WorkOrderForOutput_SizeRatio wd WITH (NOLOCK) on wd.WorkOrderForOutputUkey = w2.Ukey
		where wd.WorkOrderForOutputUkey=t.Ukey
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
    and cc.MDivisionid = t.mdivisionid
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
							  isnull([MachineSpreadingTime_min],0)+isnull([Separatortime_min],0)+isnull([ForwardTime_min],0),
	[TotalCuttingTime_min] = isnull([CuttingSetupTime_min],0)+isnull([MachCuttingTime_min],0)+isnull([WindowTime_min],0)					   
	{(model.IsPowerBI ? ", [WorkOrderUkey]" : string.Empty)}
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

            Base_ViewModel resultReport = new Base_ViewModel
            {
                Result = this.DBProxy.Select("Production", sqlCmd, parameters, out DataTable[] dataTables),
            };

            if (!resultReport.Result)
            {
                return resultReport;
            }

            resultReport.DtArr = dataTables;
            return resultReport;
        }
    }
}
