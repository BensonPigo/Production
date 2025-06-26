using Sci.Data;
using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sci.Production.Prg.PowerBI.DataAccess
{
    /// <inheritdoc/>
    public class P_Import_MtlStatusAnalisis
    {
        /// <inheritdoc/>
        public P_Import_MtlStatusAnalisis()
        {
            DBProxy.Current.DefaultTimeout = 7200;
        }

        /// <inheritdoc/>
        public Base_ViewModel P_MtlStatusAnalisis(ExecutedList item)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            try
            {
                if (!item.SDate.HasValue)
                {
                    item.SDate = DateTime.Now.AddDays(-60);
                }

                if (!item.EDate.HasValue)
                {
                    item.EDate = DateTime.Now.AddDays(30);
                }

                Base_ViewModel resultReport = this.GetMtlStatusAnalisisData(item);
                if (!resultReport.Result)
                {
                    throw resultReport.Result.GetException();
                }

                // insert into PowerBI
                finalResult = this.UpdateBIData(resultReport.Dt);
                if (!finalResult.Result)
                {
                    throw finalResult.Result.GetException();
                }

                finalResult = new Base().UpdateBIData(item);
            }
            catch (Exception ex)
            {
                finalResult.Result = new Ict.DualResult(false, ex);
            }

            return finalResult;
        }

        private Base_ViewModel GetMtlStatusAnalisisData(ExecutedList item)
        {
            Base_ViewModel finalResult;
            Data.DBProxy.Current.OpenConnection("Production", out SqlConnection sqlConn);
            using (sqlConn)
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>()
                {
                    new SqlParameter("@CloseDate_S", item.SDate),
                    new SqlParameter("@CloseDate_E", item.EDate),
                    new SqlParameter("@BIFactoryID", item.RgCode),
                };

                string sql = @"	
--declare @CloseDate_S Date = dateadd(DAY, -60, getdate()),
--	@CloseDate_E Date = dateadd(DAY, 30, getdate())

select type, a.ID, a.Seq1, a.Seq2, a.Handle, a.FactoryID, a.SuppID
	, [BuyerDelivery] = MIN(a.Delivery)
	, [Qty] = SUM(a.Qty)
	, [BalQty] = SUM(a.BalQty)
into #tmp_MmsPO_Detail
from (
	Select type = 'MachinePO_Detail', mpd.ID, mpd.Seq1, mpd.Seq2, mpd.Delivery, mp.Handle, mp.FactoryID, mpd.Qty, mpd.SuppID
		, [BalQty] = mpd.Qty - mpd.ShipQty
	From [ExtendServer].Machine.dbo.MachinePO_Detail mpd with(nolock)
	inner join [ExtendServer].Machine.dbo.MachinePO mp with(nolock) on mpd.ID = mp.ID
	where exists (select 1 from Production.dbo.Export_Detail ed with(nolock) where mpd.ID = ed.PoID and ed.POType = 'M')
	union all
	Select type = 'MiscPO_Detail',mid.TPEPOID, mid.SEQ1, mid.SEQ2, mid.SuppDelivery, mi.Handle, mi.FactoryID, mid.TPEQty, mid.SuppID
		, [BalQty] = mid.Qty - mid.ShipQty
	From [ExtendServer].Machine.dbo.MiscPO_Detail mid with(nolock)
	inner join [ExtendServer].Machine.dbo.MiscPO mi with(nolock) on mid.ID = mi.ID
	where exists (select 1 from Production.dbo.Export_Detail ed with(nolock) where mid.TPEPOID = ed.PoID and ed.POType = 'M')	
	union all
	Select type = 'PartPO_Detail',ppd.TPEPOID, ppd.SEQ1, ppd.SEQ2, ppd.SuppDelivery, pp.Handle, pp.FactoryID, ppd.TPEQty, ppd.SuppID
		, [BalQty] = ppd.Qty - ppd.ShipQty
	From [ExtendServer].Machine.dbo.PartPO_Detail ppd with(nolock)
	inner join [ExtendServer].Machine.dbo.PartPO pp with(nolock) on ppd.ID = pp.ID
	where exists (select 1 from Production.dbo.Export_Detail ed with(nolock) where ppd.TPEPOID = ed.PoID and ed.POType = 'M')
	union all
	Select type = 'MiscOtherPO_Detail',motd.ID, motd.SEQ1, motd.SEQ2, motd.Delivery, mo.Handle, mo.FactoryID, motd.Qty, motd.SuppID
		, [BalQty] = motd.Qty - motd.ShipQty
	From [ExtendServer].Machine.dbo.MiscOtherPO_Detail motd with(nolock)
	inner join [ExtendServer].Machine.dbo.MiscOtherPO mo with(nolock) on motd.ID = mo.ID
	where exists (select 1 from Production.dbo.Export_Detail ed with(nolock) where motd.ID = ed.PoID and ed.POType = 'M')
) a
group by  type,  a.ID, a.Seq1, a.Seq2, a.Handle, a.FactoryID, a.SuppID

Select Distinct
		e.ID
	, e.ExportCountry
	, e.ExportPort
	, e.ShipModeID
	, e.CloseDate
	, e.ETD
	, e.ETA
	, e.WhseArrival
	, LETA = Convert(Date, null)
	, PFday = 0
	, ExportFty = e.FactoryID
	, mp.factoryID
	, ed.POID 
	, Seq = (
		iif(s.ThirdCountry = 0
			, stuff((
				Select distinct concat(';', iif(a.Type = 'F' And sum(a.BalQty) > 0
							, isnull(a.Seq1, '') + '-' + isnull(a.AbbCH, '') + '(' + cast(sum(a.BalQty) as varchar(10)) + '-' + a.UnitID + ')'
							, isnull(a.Seq1, '') + '-' + isnull(a.AbbCH, '')))
					From (
						Select distinct ded.Seq1 
							, BalQty = isnull(dmpd.BalQty, 0)
							, df.Type
							, ds.AbbCH 
							, ded.UnitID
						From Production.dbo.Export_Detail ded with(nolock)
						Left join #tmp_MmsPO_Detail dmpd with(nolock) on ded.PoID = dmpd.ID And dmpd.Seq1 = ded.Seq1 And dmpd.Seq2 = ded.Seq2
						Left join Production.dbo.Supp ds with(nolock) on ds.ID = dmpd.SuppID
						Left join Production.dbo.Fabric df with(nolock) on ded.SCIRefno = df.SCIRefno
						Where ded.ID = e.ID And ded.PoID = ed.PoID
					) as a
					Group by a.AbbCH, a.Seq1, a.Type, a.UnitID
					For xml path('')
				), 1, 1, '')
			, '')
	)	
	, Category = ''
	, PFETA = Convert(Date, null)
	, SewDate = Convert(Date, null)
	, mp.BuyerDelivery 
	, SCIDelivery = Convert(Date, null)
	, POSMR = poSMR.ID + '-' + poSMR.name + ' #' + poSMR.ExtNo
	, POHandle = poHandle.ID + '-' + poHandle.name + ' #' + poHandle.ExtNo
	, SMR = ''
	, MRHandle = ''
	, PCHandle = ''
	, StyleID =''
	, POComboList = Convert(varchar(max), '')
	, SumQty.SumQty
	, ProjectID=''
	, Reason = Convert(varchar(max), '')
	, Handle =  handle.ID + '-' + handle.name + ' #' + handle.ExtNo
	, Confirm = IIF(e.Confirm = 1, 'Y', 'N')
	, Duty = iif(isnull(esa.Duty, '') != '', ed.Seq1 + '-' + duty.Name, '')
	, PFRemark = ''
	, POType = 'MMS'
into #tmpMMS
From Production.dbo.Export e with(nolock)
Inner Join Production.dbo.Export_Detail ed with(nolock) on e.ID = ed.ID
Left Join Production.dbo.Fabric f with(nolock) on ed.SCIRefno = f.SCIRefno
Left join Production.dbo.Supp s with(nolock) on s.ID = ed.SuppID
Left join Production.dbo.Export_ShareAmount esa with(nolock) on esa.Ukey = ed.Export_ShareAmount_Ukey
Left join Production.dbo.ExpressDuty duty with(nolock) on duty.ID = iif(isnull(esa.Duty, '') = '', ed.Duty, esa.Duty) 
Left Join Production.dbo.TPEPass1 handle with(nolock) on handle.ID = e.Handle
Outer apply(
	select mp.Handle, mp.FactoryID, [BuyerDelivery] = min(mp.BuyerDelivery)
	from #tmp_MmsPO_Detail mp
	where ed.PoID = mp.ID and ed.Seq1 = mp.Seq1	and ed.Seq2 = mp.Seq2
	group by mp.Handle, mp.FactoryID
) mp
Outer apply(
	Select SumQty = sum(mpd.Qty)
	from #tmp_MmsPO_Detail mpd
	Where ed.PoID = mpd.ID
) SumQty
Left join Production.dbo.Pass1 p with(nolock) on p.ID = mp.Handle
Left join Production.dbo.TPEPass1 poSMR with(nolock) on poSMR.ID = p.Supervisor
Left join Production.dbo.TPEPass1 poHandle with(nolock) on poHandle.ID = mp.Handle
Where ed.POType = 'M'
And e.CloseDate Between @CloseDate_S And @CloseDate_E
And exists (select 1 from Production.dbo.Factory f with(nolock) where e.FactoryID = f.ID and f.IsProduceFty = 1)
Order by e.ID, ed.POID

Select Distinct 
		e.ID
	, e.ExportCountry
	, e.ExportPort
	, e.ShipModeID
	, e.CloseDate
	, e.ETD
	, e.ETA
	, e.WhseArrival
	, LETA = DateData.LETA
	, PFday = Isnull(datediff(day, e.ETA, pocl.SCIDelivery), 0)
	, ExportFty = e.FactoryID
	, o.factoryID
	, ed.POID
	, Seq = (iif(s.ThirdCountry = 0
			, stuff((
					Select distinct concat(';',iif(a.Type = 'F' and sum(a.BalQty) > 0
						, a.Seq1+ '-' + a.AbbCH + '(' + cast(sum(a.BalQty) as varchar(10)) + '-' + a.UnitID + ')'
						, a.Seq1+ '-' + a.AbbCH ))
					From (
						Select distinct ed2.Seq1 
							, BalQty = po3.Qty - po3.ShipQty
							, f.Type
							, s.AbbCH
							, ed2.UnitID
							, s.ThirdCountry 
						From Production.dbo.Export_Detail ed2 with(nolock)
						Left join Production.dbo.PO_Supp_Detail po3 with(nolock) on ed2.PoID = po3.ID And ed2.Seq1 = po3.Seq1 aND ed2.Seq2 = po3.Seq2
						Left join Production.dbo.PO_Supp po2 with(nolock) on ed2.POID = po2.ID And ed2.Seq1 = po2.SEQ1
						Left join Production.dbo.Supp s with(nolock) on s.ID = po2.SuppID
						Left join Production.dbo.Fabric f with(nolock) on ed2.SCIRefno = f.SCIRefno 
						Where ed2.ID = e.ID And ed2.PoID = ed.PoID
					) a
					Group by a.AbbCH, a.Seq1, a.Type, a.UnitID
					For xml path('')
				), 1, 1, '')
			, '')
	)
	, Category = category.Name
	, PFETA = OrdersPfeta.PFETA
	, SewDate = DateData.SewDate
	, BuyerDelivery = DateData.BuyerDelivery
	, SCIDelivery = DateData.SCIDelivery
	, POSMR = poSMR.ID + '-' + poSMR.name + ' #' + poSMR.ExtNo
	, POHandle = poHandle.ID + '-' + poHandle.name + ' #' + poHandle.ExtNo
	, SMR = smr.ID + '-' + smr.name + ' #' + smr.ExtNo
	, MRHandle = mrHandle.ID + '-' + mrHandle.name + ' #' + mrHandle.ExtNo
	, PCHandle = pcHandle.ID + '-' + pcHandle.name + ' #' + pcHandle.ExtNo
	, o.StyleID
	, POComboList = pocl.POComboList
	, SumQty.SumQty
	, o.ProjectID
	, Reason = (
		stuff((
				select distinct Concat(',',ed2.earlyshipReason,'-',r2.Name)
				from Production.dbo.Export_Detail ed2 with(nolock)
				left join Production.dbo.Export e2 on e2.ID = ed2.ID
				left join Production.dbo.Reason r2 on r2.ReasonTypeID = 'EarlyShip' and r2.id = ed2.earlyshipReason
				where e2.ID = e.ID and ed2.PoID = ed.PoID and isnull(ed2.earlyshipReason,'') != ''
					For xml path('')
			), 1, 1, '')
	)
	, Handle = handle.ID + '-' + handle.name + ' #' + handle.ExtNo
	, Confirm = IIF(isnull(psdComplete.notCompleteCount, 0) > 0, 'N', 'Y')  --PO_Supp_Detail.Complete 有一筆不為1 顯示N
	, Duty = iif(isnull(esa.Duty, '') <> '', Concat(ed.Seq1, '-', duty.Name), '')
	, PFRemark = Iif(esa.Duty in ('2', 'W', 'C'), iif(o.Category = 'M', GetMPFRemark.Remark, GetPFRemark.Remark), '')
	, POType = 'Garment'
Into #tmpGarment
From Production.dbo.Export_Detail ed with(nolock)
Inner join Production.dbo.Export e with(nolock) on e.ID = ed.ID 
inner join Production.dbo.Orders o with(nolock) on ed.PoID = o.ID
Left join Production.dbo.Export_ShareAmount esa with(nolock) on esa.Ukey = ed.Export_ShareAmount_Ukey --要追加轉入 ed.[Export_ShareAmount_Ukey]
Left join Production.dbo.PO with(nolock) on ed.POID = PO.ID
Left join Production.dbo.Fabric f with(nolock) on ed.SCIRefno = f.SCIRefno --要追加轉入 ed.SCIRefno
Left join Production.dbo.Supp s with(nolock) on s.ID = ed.SuppID
Left join Production.dbo.DropDownList category with(nolock) on category.Type = 'Category' And category.ID = o.Category
Left join Production.dbo.Reason duty with(nolock) on duty.ReasonTypeID = 'ExpressDuty' And duty.ID = isnull(esa.Duty, ed.Duty)
Left join Production.dbo.TPEPass1 poSMR with(nolock) on poSMR.ID = PO.POSMR
Left join Production.dbo.TPEPass1 poHandle with(nolock) on poHandle.ID = PO.POHandle
Left join Production.dbo.TPEPass1 pcHandle with(nolock) on pcHandle.ID = PO.PCHandle
Left join Production.dbo.TPEPass1 smr with(nolock) on smr.ID = o.SMR
Left join Production.dbo.TPEPass1 mrHandle with(nolock) on mrHandle.ID =  o.MRHandle
Left join Production.dbo.Pass1 handle with(nolock) on handle.ID = e.Handle
Outer apply(
		Select do.POID, SumQty = sum(Qty)
		From Production.dbo.Orders do with(nolock)
		Where do.POID = o.ID
		Group by do.POID
) SumQty
Outer apply(
		Select Top 1 Remark 
		From Production.dbo.Order_PFHis with(nolock)
		Where Order_PFHis.Id = ed.PoID
		Order by AddDate Desc
) GetPFRemark
Outer apply(
		Select Top 1 Remark 
		From Production.dbo.Order_PFHis with(nolock)
		Where Order_PFHis.Id = ed.DutyID --要追加轉入 ed.DutyID
		Order by AddDate Desc
) GetMPFRemark
Outer apply(
	select LETA = min(Isnull(pfeta, iif(Category <> 'S', leta, null)))
		, SCIDelivery = min(SCIDelivery)
		, BuyerDelivery = min(BuyerDelivery)
		, SewDate = min(SewInLIne)
	From Production.dbo.Orders with(nolock)
	Where poid = ed.POID And Qty > 0 And Junk = 0
) as DateData
Outer apply(
	Select min(Orders.PFETA) PFETA
	From Production.dbo.Orders with(nolock)
	Where PoID = ed.POID And Qty > 0 And PFETA is not null
) as OrdersPfeta
Outer apply(
	Select pocl.POComboList, DateData.*, OrdersPfeta.PFETA
	From Production.dbo.Order_POComboList pocl with(nolock)
	where ed.POID = pocl.ID
) pocl
OUTER APPLY(
	select [notCompleteCount] = SUM(iif(po3.Complete = 1, 0, 1)) 
	from Production.dbo.Export_Detail ed2 with(nolock) 
	inner join Production.dbo.PO_Supp_Detail po3 with(nolock) on ed2.PoID = po3.ID And ed2.Seq1 = po3.Seq1 And ed2.Seq2 = po3.Seq2
	where ed2.ID = e.ID and ed2.PoID = ed.PoID and po3.Junk = 0		
) psdComplete
Where ed.POType ='G'
And e.CloseDate Between @CloseDate_S And @CloseDate_E
And exists (select 1 from Production.dbo.Factory f with(nolock) where o.FactoryID = f.ID and f.IsProduceFty = 1)
Order by e.ID, ed.POID


Select *, 
[BIFactoryID] = @BIFactoryID, 
[BIInsertDate] = GETDATE()
From #tmpGarment
UNION
Select *,
[BIFactoryID] = @BIFactoryID, 
[BIInsertDate] = GETDATE()
From #tmpMMS



drop table #tmpGarment, #tmpMMS
";
                finalResult = new Base_ViewModel
                {
                    Result = DBProxy.Current.SelectByConn(conn: sqlConn, cmdtext: sql, parameters: sqlParameters, datas: out DataTable dataTable),
                    Dt = dataTable,
                };
            }

            return finalResult;
        }

        private Base_ViewModel UpdateBIData(DataTable dt)
        {
            Base_ViewModel finalResult;
            Data.DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);
            using (sqlConn)
            {
                string sql = @"	
insert into P_MtlStatusAnalisis_History ([Ukey],BIFactoryID,BIInsertDate)
Select p.Ukey, p.BIFactoryID, GetDate()
from POWERBIReportData.dbo.P_MtlStatusAnalisis p
inner join [MainServer].[Production].[dbo].[Orders] o on p.SPNo = o.ID
inner join [MainServer].[Production].[dbo].[Factory] f on o.FactoryID = f.ID
where f.IsProduceFty = 0

Delete p
from POWERBIReportData.dbo.P_MtlStatusAnalisis p
inner join [MainServer].[Production].[dbo].[Orders] o on p.SPNo = o.ID
inner join [MainServer].[Production].[dbo].[Factory] f on o.FactoryID = f.ID
where f.IsProduceFty = 0


insert into P_MtlStatusAnalisis_History ([Ukey],BIFactoryID,BIInsertDate)
Select p.Ukey, p.BIFactoryID, GetDate()
from POWERBIReportData.dbo.P_MtlStatusAnalisis p
where exists (select 1 from #tmp t where t.PoID = p.[SPNo] and t.CloseDate = p.[Close_Date])

Delete p
from POWERBIReportData.dbo.P_MtlStatusAnalisis p
where exists (select 1 from #tmp t where t.PoID = p.[SPNo] and t.CloseDate = p.[Close_Date])

Insert Into POWERBIReportData.dbo.P_MtlStatusAnalisis ([WK]
		, [LoadingCountry]
		, [LoadingPort]
		, [Shipmode]
		, [Close_Date]
		, [ETD]
		, [ETA]
		, [Arrive_WH_Date]
		, [KPI_LETA]
		, [Prod_LT]
		, [WK_Factory]
		, [FactoryID]
		, [SPNo]
		, [SEQ]
		, [Category]
		, [PF_ETA]
		, [SewinLine]
		, [BuyerDelivery]
		, [SCIDelivery]
		, [PO_SMR]
		, [PO_Handle]
		, [SMR]
		, [MR]
		, [PC_Handle]
		, [Style]
		, [SP_List]
		, [PO_Qty]
		, [Project]
		, [Early_Ship_Reason]
		, [WK_Handle]
		, [MTL_Confirm]
		, [Duty]
		, [PF_Remark]
		, [Type]
		, [BIFactoryID]
		, [BIInsertDate]
)	
select ISNULL(t.ID, '')
	, ISNULL(t.ExportCountry, '')
	, ISNULL(t.ExportPort, '')
	, ISNULL(t.ShipModeID, '')
	, t.CloseDate
	, t.ETD
	, t.ETA
	, t.WhseArrival
	, t.LETA
	, ISNULL(t.PFday, 0)
	, ISNULL(t.ExportFty, '')
	, ISNULL(t.FactoryID, '')
	, ISNULL(t.POID, '')
	, ISNULL(t.Seq, '')
	, ISNULL(t.Category, '')
	, t.PFETA
	, t.SewDate
	, t.BuyerDelivery
	, t.SCIDelivery
	, ISNULL(t.POSMR, '')
	, ISNULL(t.POHandle, '')
	, ISNULL(t.SMR, '')
	, ISNULL(t.MRHandle, '')
	, ISNULL(t.PCHandle, '')
	, ISNULL(t.StyleID, '')
	, ISNULL(t.POComboList, '')
	, ISNULL(t.SumQty, 0)
	, ISNULL(t.ProjectID, '')
	, ISNULL(t.Reason, '')
	, ISNULL(t.Handle, '')
	, ISNULL(t.Confirm, '')
	, ISNULL(t.Duty, '')
	, ISNULL(t.PFRemark, '')
	, ISNULL(t.POType, '')
	, t.[BIFactoryID]
	, t.[BIInsertDate]
from #tmp t 
where not exists (select 1 from POWERBIReportData.dbo.P_MtlStatusAnalisis p with(nolock) where t.PoID = p.[SPNo] and t.CloseDate = p.[Close_Date])
";
                finalResult = new Base_ViewModel()
                {
                    Result = TransactionClass.ProcessWithDatatableWithTransactionScope(dt, null, sqlcmd: sql, result: out DataTable dataTable, conn: sqlConn),
                };
            }

            return finalResult;
        }
    }
}
