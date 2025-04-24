using Ict;
using Sci.Data;
using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Sci.Production.Prg.PowerBI.DataAccess
{
    /// <inheritdoc/>
    public class P_Import_ActualCutOutputReport
    {
        private DBProxy DBProxy;

        /// <inheritdoc/>
        public Base_ViewModel P_ActualCutOutputReport(DateTime? sDate, DateTime? eDate)
        {
            this.DBProxy = new DBProxy()
            {
                DefaultTimeout = 1800,
            };

            Base_ViewModel finalResult = new Base_ViewModel();

            if (!sDate.HasValue)
            {
                sDate = DateTime.Now.AddDays(-7);
            }

            if (!eDate.HasValue)
            {
                eDate = DateTime.Now;
            }

            try
            {
                Base_ViewModel resultReport = this.GetActualCutOutputReport_Data((DateTime)sDate, (DateTime)eDate);
                if (!resultReport.Result)
                {
                    throw resultReport.Result.GetException();
                }

                DataTable dataTable = resultReport.Dt;

                // insert into PowerBI
                finalResult = this.UpdateBIData(dataTable);
                if (!finalResult.Result)
                {
                    throw finalResult.Result.GetException();
                }

                finalResult.Result = new Ict.DualResult(true);
            }
            catch (Exception ex)
            {
                finalResult.Result = new Ict.DualResult(false, ex);
            }

            return finalResult;
        }

        private Base_ViewModel GetActualCutOutputReport_Data(DateTime sdate, DateTime edate)
        {
            string sqlcmd = $@" 
			declare @SDate varchar(20) = '{sdate.ToString("yyyy/MM/dd")}'
            declare @EDate varchar(20) = '{edate.ToString("yyyy/MM/dd")}'

			select DISTINCT w.ID
			into #tmpWorkOrderID
			from [MainServer].Production.dbo.WorkOrder w with(nolock) 
			left join [MainServer].Production.dbo.CuttingOutput_Detail cod with(nolock) on cod.WorkOrderUkey = w.Ukey
			left join [MainServer].Production.dbo.CuttingOutput co with(nolock) on co.id = cod.id
			where ((w.EstCutDate >= @SDate and w.EstCutDate <=  @EDate) OR (co.cDate >= @SDate and co.cDate <= @EDate))

			select w.ID,w.CutRef,w.MDivisionId,ActCutDate=max(co.cDate)
			into #tmp1
			from [MainServer].Production.dbo.WorkOrder w with(nolock) 
			left join [MainServer].Production.dbo.CuttingOutput_Detail cod with(nolock) on cod.WorkOrderUkey = w.Ukey
			left join [MainServer].Production.dbo.CuttingOutput co with(nolock) on co.id = cod.id
			where 1=1
			and isnull(w.CutRef,'') <> ''
			and w.id in (select id from #tmpWorkOrderID)
			group by w.CutRef,w.MDivisionID,w.ID

			select w.*,ActCutDate=co.cDate
			into #tmpCutRefNull
			from [MainServer].Production.dbo.WorkOrder w with(nolock) 
			left join [MainServer].Production.dbo.CuttingOutput_Detail cod with(nolock) on cod.WorkOrderUkey = w.Ukey
			left join [MainServer].Production.dbo.CuttingOutput co with(nolock) on co.id = cod.id
			where 1=1
			and isnull(w.CutRef,'') = ''
			and w.id in (select id from #tmpWorkOrderID)

			select t.CutRef,t.MDivisionId,t.ActCutDate ,t.ID,
				Layer=sum(w.Layer),
				Cons=sum(w.Cons)
			into #tmp2a
			from #tmp1 t
			inner join [MainServer].Production.dbo.WorkOrder w with(nolock) on w.CutRef = t.CutRef
			group by t.CutRef,t.MDivisionId,t.ActCutDate ,t.ID

			select t.CutRef,t.MDivisionId,t.ActCutDate,t.Layer,t.Cons,t.ID,
				noEXCESSqty=sum(iif(wd.OrderID <> 'EXCESS',wd.Qty,0)),
				EXCESSqty = sum(iif(wd.OrderID =  'EXCESS',wd.Qty,0))
			into #tmp2
			from #tmp2a t
			inner join [MainServer].Production.dbo.WorkOrder w with(nolock) on w.CutRef = t.CutRef
			inner join [MainServer].Production.dbo.WorkOrder_Distribute wd with(nolock) on wd.WorkOrderUkey = w.Ukey
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
				PerimeterM=isnull(iif(w.ActCuttingPerimeter not like '%yd%',w.ActCuttingPerimeter,cast(Production.dbo.GetActualPerimeter(w.ActCuttingPerimeter) as nvarchar)),''),
				PerimeterYd=isnull(iif(w.ActCuttingPerimeter not like '%yd%',w.ActCuttingPerimeter,cast(Production.dbo.GetActualPerimeterYd(w.ActCuttingPerimeter) as nvarchar)),''),
				t.Layer,
				SizeCode=isnull(SizeCode.SizeCode,''),
				t.Cons,
				t.EXCESSqty,
				NoofRoll=iif(isnull(NoofRoll.NoofRoll,0)<1,1,isnull(NoofRoll.NoofRoll,0)),
				DyeLot=iif(isnull(DyeLot.DyeLot,0)<1,1,isnull(DyeLot.DyeLot,0)),
				NoofWindow=isnull(iif(t.Layer=0,0,t.Cons/t.Layer/1.4),0),
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
			OUTER APPLY(SELECT TOP 1 * FROM [MainServer].Production.dbo.WorkOrder w with(nolock) WHERE w.CutRef = t.CutRef)w
			inner join [MainServer].Production.dbo.orders o with(nolock) on o.id = w.ID
			left join [MainServer].Production.dbo.Fabric f with(nolock) on f.SCIRefno = w.SCIRefno
			left join [MainServer].Production.dbo.SpreadingTime st with(nolock) on st.WeaveTypeID = f.WeaveTypeID
			left join ManufacturingExecution.dbo.RefnoRelaxtime rr WITH (NOLOCK) on rr.Refno = w.Refno
			left join ManufacturingExecution.dbo.FabricRelaxation fr WITH (NOLOCK) on rr.FabricRelaxationID = fr.ID
			left join [MainServer].Production.dbo.CuttingTime ct WITH (NOLOCK) on ct.WeaveTypeID = f.WeaveTypeID
			outer apply(
				select SubSP = stuff((
					select distinct concat(',',wd.OrderID)
					from [MainServer].Production.dbo.WorkOrder w2 with(nolock)
					inner join [MainServer].Production.dbo.WorkOrder_Distribute wd with(nolock) on wd.WorkOrderUkey = w2.Ukey
					where w2.CutRef = t.CutRef and t.ID=w2.ID
					For XML path('')
				),1,1,'')
			)subSp
			outer apply(
				select Size = stuff((
					select distinct concat(',',wd.SizeCode)
					from [MainServer].Production.dbo.WorkOrder w2 with(nolock)
					inner join [MainServer].Production.dbo.WorkOrder_Distribute wd with(nolock) on wd.WorkOrderUkey = w2.Ukey
					where w2.CutRef = t.CutRef
					For XML path('')
				),1,1,'')
			)size
			outer apply
			(
				select SizeCode = stuff(
				(
					Select concat(', ' , wd.sizecode, '/ ', wd.qty)
					From [MainServer].Production.dbo.WorkOrder w2 with(nolock)
					inner join [MainServer].Production.dbo.WorkOrder_SizeRatio wd WITH (NOLOCK) on wd.WorkOrderUkey = w2.Ukey
					Where w2.CutRef = t.CutRef
					For XML path('')
				),1,1,'')
			)SizeCode
			outer apply(
				select NoofRoll = count(1)
				from(
					select distinct cr.Seq1,cr.seq2,cr.Roll,cr.Dyelot
					from [MainServer].Production.dbo.CuttingOutputFabricRecord cr WITH (NOLOCK) 
					where cr.CutRef = w.CutRef and cr.MDivisionId = w.MDivisionId
				)disC
			)NoofRoll
			outer apply(
				select DyeLot = count(1)
				from(
					select distinct cr.Seq1,cr.seq2,cr.Dyelot
					from [MainServer].Production.dbo.CuttingOutputFabricRecord cr WITH (NOLOCK) 
					where cr.CutRef = w.CutRef and cr.MDivisionId = w.MDivisionId
				)disC
			)DyeLot
			outer apply(	
				select  ActualSpeed
				from [MainServer].Production.dbo.CuttingMachine_detail cmd WITH (NOLOCK) 
				inner join [MainServer].Production.dbo.CutCell cc WITH (NOLOCK) on cc.CuttingMachineID = cmd.id
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
				PerimeterM=isnull(iif(t.ActCuttingPerimeter not like '%yd%',t.ActCuttingPerimeter,cast(Production.dbo.GetActualPerimeter(t.ActCuttingPerimeter) as nvarchar)),''),
				PerimeterYd=isnull(iif(t.ActCuttingPerimeter not like '%yd%',t.ActCuttingPerimeter,cast(Production.dbo.GetActualPerimeterYd(t.ActCuttingPerimeter) as nvarchar)),''),
				t.Layer,
				SizeCode=isnull(SizeCode.SizeCode,''),
				t.Cons,
				EQ.EXCESSqty,
				NoofRoll=iif(isnull(NoofRoll.NoofRoll,0)<1,1,isnull(NoofRoll.NoofRoll,0)),
				DyeLot=iif(isnull(DyeLot.DyeLot,0)<1,1,isnull(DyeLot.DyeLot,0)),
				NoofWindow=isnull(iif(t.Layer=0,0,t.Cons/t.Layer/1.4),0),
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
			inner join [MainServer].Production.dbo.orders o with(nolock) on o.id = t.ID
			left join [MainServer].Production.dbo.Fabric f with(nolock) on f.SCIRefno = t.SCIRefno
			left join [MainServer].Production.dbo.SpreadingTime st with(nolock) on st.WeaveTypeID = f.WeaveTypeID
			left join ManufacturingExecution.dbo.RefnoRelaxtime rr WITH (NOLOCK) on rr.Refno = t.Refno
			left join ManufacturingExecution.dbo.FabricRelaxation fr WITH (NOLOCK) on rr.FabricRelaxationID = fr.ID
			left join [MainServer].Production.dbo.CuttingTime ct WITH (NOLOCK) on ct.WeaveTypeID = f.WeaveTypeID
			outer apply(
				select SubSP = stuff((
					select distinct concat(',',wd.OrderID)
					from [MainServer].Production.dbo.WorkOrder_Distribute wd with(nolock) 
					where wd.WorkOrderUkey=t.Ukey
					For XML path('')
				),1,1,'')
			)subSp
			outer apply(
				select 
					noEXCESSqty=sum(iif(wd.OrderID <> 'EXCESS',wd.Qty,0)),
					EXCESSqty = sum(iif(wd.OrderID =  'EXCESS',wd.Qty,0))
				from [MainServer].Production.dbo.WorkOrder_Distribute wd with(nolock)
				where wd.WorkOrderUkey = t.Ukey
			)EQ
			outer apply(
				select Size = stuff((
					select distinct concat(',',wd.SizeCode)
					from [MainServer].Production.dbo.WorkOrder w2 with(nolock)
					inner join [MainServer].Production.dbo.WorkOrder_Distribute wd with(nolock) on wd.WorkOrderUkey = w2.Ukey
					where wd.WorkOrderUkey=t.Ukey
					For XML path('')
				),1,1,'')
			)size
			outer apply
			(
				select SizeCode = stuff(
				(
					Select concat(', ' , wd.sizecode, '/ ', wd.qty)
					From [MainServer].Production.dbo.WorkOrder w2 with(nolock)
					inner join [MainServer].Production.dbo.WorkOrder_SizeRatio wd WITH (NOLOCK) on wd.WorkOrderUkey = w2.Ukey
					where wd.WorkOrderUkey=t.Ukey
					For XML path('')
				),1,1,'')
			)SizeCode
			outer apply(
				select NoofRoll = count(1)
				from(
					select distinct cr.Seq1,cr.seq2,cr.Roll,cr.Dyelot
					from [MainServer].Production.dbo.CuttingOutputFabricRecord cr WITH (NOLOCK) 
					where cr.CutRef = t.CutRef and cr.MDivisionId = t.MDivisionId
				)disC
			)NoofRoll
			outer apply(
				select DyeLot = count(1)
				from(
					select distinct cr.Seq1,cr.seq2,cr.Dyelot
					from [MainServer].Production.dbo.CuttingOutputFabricRecord cr WITH (NOLOCK) 
					where cr.CutRef = t.CutRef and cr.MDivisionId = t.MDivisionId
				)disC
			)DyeLot
			outer apply(	
				select  ActualSpeed
				from [MainServer].Production.dbo.CuttingMachine_detail cmd WITH (NOLOCK) 
				inner join [MainServer].Production.dbo.CutCell cc WITH (NOLOCK) on cc.CuttingMachineID = cmd.id
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
										  isnull([MachineSpreadingTime_min],0)+isnull([Separatortime_min],0)+isnull([ForwardTime_min],0)						 ,
				[TotalCuttingTime_min] = isnull([CuttingSetupTime_min],0)+isnull([MachCuttingTime_min],0)+isnull([WindowTime_min],0)	,
				[BIFactoryID] = (select top 1 IIF(RgCode = 'PHI', 'PH1', RgCode) from Production.dbo.[System]) ,
				[BIInsertDate] = GETDATE()   
			--into #detail
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
			outer apply(select [WindowTime_min]=Round(Windowtime * iif(isnull(Layer,0)=0 or isnull(WindowLength,0)=0,0,(Cons/Layer*0.9144)/WindowLength)/60,2))cal9";
            Base_ViewModel resultReport = new Base_ViewModel
            {
                Result = this.DBProxy.Select("Production", sqlcmd, out DataTable dataTables),
            };

            if (!resultReport.Result)
            {
                return resultReport;
            }

            resultReport.Dt = dataTables;
            return resultReport;
        }

        private Base_ViewModel UpdateBIData(DataTable dt)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            DualResult result;
            DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);

            using (sqlConn)
            {
                string sql = $@" 
				DELETE P_ActualCutOutputReport WHERE SP IN (SELECT ID FROM #detail)



				INSERT INTO [dbo].[P_ActualCutOutputReport]
				(
					 [FactoryID]
					,[EstCutDate]
					,[ActCutDate]
					,[CutCellid]
					,[SpreadingNoID]
					,[CutplanID]
					,[CutRef]
					,[SP]
					,[SubSP]
					,[StyleID]
					,[Size]
					,[noEXCESSqty]
					,[Description]
					,[WeaveTypeID]
					,[FabricCombo]
					,[MarkerLength]
					,[PerimeterYd]
					,[Layer]
					,[SizeCode]
					,[Cons]
					,[EXCESSqty]
					,[NoofRoll]
					,[DyeLot]
					,[NoofWindow]
					,[CuttingSpeed]
					,[PreparationTime]
					,[ChangeoverTime]
					,[SpreadingSetupTime]
					,[MachSpreadingTime]
					,[SeparatorTime]
					,[ForwardTime]
					,[CuttingSetupTime]
					,[MachCuttingTime]
					,[WindowTime]
					,[TotalSpreadingTime]
					,[TotalCuttingTime]
					,[BIFactoryID]	
					,[BIInsertDate]
				)
				select 
				ISNULL([FactoryID], '')
				,[EstCutDate]
				,[ActCutDate]
				,ISNULL([CutCellid], '')
				,ISNULL([SpreadingNoID], '')
				,ISNULL([CutplanID], '')
				,ISNULL([CutRef], '')
				,ISNULL(ID, '')
				,ISNULL([SubSP], '')
				,ISNULL([StyleID], '')
				,ISNULL([Size], '')
				,ISNULL([noEXCESSqty], 0)
				,ISNULL([Description], '')
				,ISNULL([WeaveTypeID], '')
				,ISNULL([FabricCombo], '')
				,ISNULL([MarkerLength], 0)
				,ISNULL([PerimeterYd], '')
				,ISNULL([Layer], 0)
				,ISNULL([SizeCode], '')
				,ISNULL([Cons], 0)
				,ISNULL([EXCESSqty], 0)
				,ISNULL([NoofRoll], 0)
				,ISNULL([DyeLot], 0)
				,ISNULL([NoofWindow], 0)
				,ISNULL(ActualSpeed, 0)
				,ISNULL([PreparationTime_min], 0)
				,ISNULL([ChangeoverTime_min], 0)
				,ISNULL([SpreadingSetupTime_min], 0)
				,ISNULL([MachineSpreadingTime_min], 0)
				,ISNULL([Separatortime_min], 0)
				,ISNULL([ForwardTime_min], 0)
				,ISNULL([CuttingSetupTime_min], 0)
				,ISNULL([MachCuttingTime_min], 0)
				,ISNULL([WindowTime_min], 0)
				,ISNULL([TotalSpreadingTime_min], 0)
				,ISNULL([TotalCuttingTime_min], 0)'
				,ISNULL([BIFactoryID], '')
				,ISNULL([BIInsertDate], GETDATE())
				from #detail d
				where not exists(select 1 from P_ActualCutOutputReport where cutref = d.cutref)
				order by FactoryID,EstCutDate,CutCellid

				if exists (select 1 from BITableInfo b where b.id = 'P_ActualCutOutputReport')
				begin
					update b
						set b.TransferDate = getdate()
					from BITableInfo b
					where b.id = 'P_ActualCutOutputReport'
				end
				else 
				begin
					insert into BITableInfo(Id, TransferDate)
					values('P_ActualCutOutputReport', getdate())
				end";

                result = TransactionClass.ProcessWithDatatableWithTransactionScope(dt, null, sql, out DataTable dataTable, conn: sqlConn, temptablename: "#detail");
            }

            finalResult.Result = result;

            return finalResult;
        }
    }
}
