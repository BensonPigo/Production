using Sci.Data;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Data;

namespace Sci.Production.Prg.PowerBI.Logic
{
    /// <inheritdoc/>
    public class Cutting_R13
    {
        private DBProxy DBProxy;

        /// <inheritdoc/>
        public Cutting_R13()
        {
            this.DBProxy = new DBProxy()
            {
                DefaultTimeout = 900,
            };
        }

        /// <summary>
        /// Cutting R13
        /// </summary>
        /// <param name="model">Cutting_R13 ViewModel</param>
        /// <returns>Base_ViewModel</returns>
        public Base_ViewModel GetCuttingScheduleOutputData(Cutting_R13_ViewModel model)
        {
            string strWhere = string.Empty;
            if (!MyUtility.Check.Empty(model.MDivisionID))
            {
                strWhere += $@" and wo.MDivisionID = '{model.MDivisionID}' ";
            }

            if (!MyUtility.Check.Empty(model.FactoryID))
            {
                strWhere += $@" and wo.FactoryID = '{model.FactoryID}' ";
            }

            if (!MyUtility.Check.Empty(model.StyleID))
            {
                strWhere += $@" and o.StyleID = '{model.StyleID}' ";
            }

            if (!MyUtility.Check.Empty(model.CuttingSP1))
            {
                strWhere += $@" and wo.ID >= '{model.CuttingSP1}' ";
            }

            if (!MyUtility.Check.Empty(model.CuttingSP2))
            {
                strWhere += $@" and wo.ID <= '{model.CuttingSP2}' ";
            }

            if (!MyUtility.Check.Empty(model.Est_CutDate1))
            {
                strWhere += $@" and wo.EstCutDate >= cast('{Convert.ToDateTime(model.Est_CutDate1).ToString("yyyy/MM/dd")}' as date) ";
            }

            if (!MyUtility.Check.Empty(model.Est_CutDate2))
            {
                strWhere += $@" and wo.EstCutDate <= cast('{Convert.ToDateTime(model.Est_CutDate2).ToString("yyyy/MM/dd")}' as date) ";
            }

            if (!MyUtility.Check.Empty(model.ActCuttingDate1))
            {
                strWhere += $@" and MincDate.MincoDate >= cast('{Convert.ToDateTime(model.ActCuttingDate1).ToString("yyyy/MM/dd")}' as date) ";
            }

            if (!MyUtility.Check.Empty(model.ActCuttingDate2))
            {
                strWhere += $@" and MincDate.MincoDate <= cast('{Convert.ToDateTime(model.ActCuttingDate2).ToString("yyyy/MM/dd")}' as date) ";
            }

            string sqlCmd = $@"
            select [M] = wo.MDivisionID,
                [Factory] = wo.FactoryID,
                [Fabrication] = f.WeaveTypeID,
                [Est.Cutting Date]= wo.EstCutDate,
                [Act.Cutting Date] = MincDate.MincoDate,
                [Earliest Sewing Inline] = c.SewInLine,
                [Master SP#] = wo.ID, 
                [Brand]=o.BrandID,
                [Style#] = o.StyleID,
                [FabRef#] = wo.Refno,
                [Switch to Workorder] = iif(c.WorkType='1','Combination',Iif(c.WorkType='2','By SP#','')),
                [Ref#] = wo.CutRef,
                [Cut#] = wo.Cutno,
                [SpreadingNoID]=wo.SpreadingNoID,
                [Cut Cell] = wo.CutCellID,
                [Combination] = wo.FabricCombo,
                [Layers] = sum(wo.Layer),
                [LackingLayers] = isnull(acc.val,0),
                [Ratio] = stuff(SQty.val,1,1,''),
                [Consumption] = sum(wo.cons) ,
                [Marker Name] = wo.Markername,
                [Marker No.] = wo.MarkerNo,
                [Marker Length] = wo.MarkerLength
            into #tmp
            from WorkOrderForOutput wo
            left join Orders o WITH (NOLOCK) on o.id = wo.ID
            left join Cutting c WITH (NOLOCK) on c.ID = o.CuttingSP
            left join fabric f WITH (NOLOCK) on f.SCIRefno = wo.SCIRefno
            outer apply(
                select val = sum(aa.Layer) 
                from cuttingoutput_Detail aa WITH (NOLOCK)
                inner join CuttingOutput c WITH (NOLOCK) on aa.ID = c.ID
                where aa.CutRef = wo.CutRef and wo.CutRef <> ''
            )acc
            outer apply(
                Select MincoDate = MIN(co.cdate)
                From cuttingoutput co WITH (NOLOCK) 
                inner join cuttingoutput_detail cod WITH (NOLOCK) on co.id = cod.id
                Where cod.CutRef = wo.CutRef and co.Status != 'New' and co.FactoryID = wo.FactoryID and wo.CutRef <> ''
            )MincDate
            outer apply(
                select val = (
	                select distinct concat(',',SizeCode+'/'+Convert(varchar,Qty))
	                from WorkOrderForOutput_SizeRatio WITH (NOLOCK) 
	                where WorkOrderForOutputUkey = wo.UKey
	                for xml path('')
                )
            )as SQty
            where 1=1 
            {strWhere}
            group by wo.MDivisionId,wo.FactoryID,f.WeaveTypeID,wo.EstCutDate,MincDate.MincoDate,c.SewInLine,wo.ID,o.BrandID,o.StyleID,
		                wo.Refno,c.WorkType,wo.CutRef,wo.Cutno,wo.SpreadingNoID,wo.CutCellid,wo.FabricCombo,sqty.val,
		                wo.Markername,wo.MarkerNo,wo.MarkerLength,acc.val
            select [M],
                [Factory],
                [Fabrication],
                [Est.Cutting Date],
                [Act.Cutting Date] = IIF(sum([Layers]) = [LackingLayers],[Act.Cutting Date],Null),
                [Earliest Sewing Inline],
                [Master SP#], 
                [Brand],
                [Style#],
                [FabRef#],
                [Switch to Workorder],
                [Ref#],
                [Cut#],
                [SpreadingNoID],
                [Cut Cell],
                [Combination],
                [Layer] = sum([Layers]),
                [Layers Level]= case when sum([Layers]) between 1 and 5 then '1~5'
					                when sum([Layers]) between 6 and 10 then '6~10'
					                when sum([Layers]) between 11 and 15 then '11~15'
					                when sum([Layers]) between 16 and 30 then '16~30'
					                when sum([Layers]) between 31 and 50 then '31~50'
					                else '50 above'
					                end ,
                [LackingLayers] = sum([Layers])-[LackingLayers],
                [Ratio],
                [Consumption] = sum([Consumption]),
                [Act. Cons. Output] = iif(len([Marker Length]) > 0, isnull(iif(sum([Layers])-[LackingLayers] = 0, sum([Consumption]), [LackingLayers]* dbo.MarkerLengthToYDS([Marker Length])), 0.0), 0.0),
                [Balance Cons.] = iif(len([Marker Length]) > 0,sum([Consumption])- isnull(iif(sum([Layers])-[LackingLayers] = 0, sum([Consumption]), [LackingLayers]* dbo.MarkerLengthToYDS([Marker Length])), 0.0), 0.0), 
                [Marker Name],
                [Marker No.],
                [Marker Length],
                [Cutting Perimeter] = wk.ActCuttingPerimeter,
                [Straight Length] = wk.StraightLength,
                [Curved Length] = wk.CurvedLength,
                [Delay Reason] = dw.[Name],
                [Remark] = wk.Remark
            from #tmp t
            outer apply (select TOP 1 ActCuttingPerimeter,StraightLength,CurvedLength,[Remark] = wo.UnfinishedCuttingRemark ,UnfinishedCuttingReason from WorkOrderForOutput wo with (nolock) where wo.CutRef = t.[Ref#]) wk
            left join DropDownList dw with (nolock) on dw.Type = 'PMS_UnFinCutReason' and dw.ID = wk.UnfinishedCuttingReason
			
            group by [M],[Factory],[Fabrication],[Est.Cutting Date],[Act.Cutting Date],[Earliest Sewing Inline],
            [Master SP#],[Brand],[Style#],[FabRef#],[Switch to Workorder],[Ref#],
            [Cut#],[SpreadingNoID],[Cut Cell],[Combination],[LackingLayers],[Ratio],[Marker Name],
            [Marker No.], [Marker Length],wk.ActCuttingPerimeter,
            wk.StraightLength,wk.CurvedLength,
            dw.[Name],wk.Remark 

            drop table #tmp";

            Base_ViewModel resultReport = new Base_ViewModel
            {
                Result = this.DBProxy.Select("Production", sqlCmd, null, out DataTable dataTable),
            };

            if (!resultReport.Result)
            {
                return resultReport;
            }

            resultReport.Dt = dataTable;
            return resultReport;
        }
    }
}
