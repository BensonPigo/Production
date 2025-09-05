using Sci.Data;
using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Sci.Production.Prg.PowerBI.DataAccess
{
    /// <inheritdoc/>
    public class P_Import_SDP
    {
        /// <inheritdoc/>
        public P_Import_SDP()
        {
            DBProxy.Current.DefaultTimeout = 2700;
        }

        /// <inheritdoc/>
        public Base_ViewModel P_SDP(ExecutedList item)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            if (!item.SDate.HasValue)
            {
                item.SDate = DateTime.Parse(DateTime.Now.AddYears(-1).ToString("yyyy/01/01"));
            }

            if (!item.EDate.HasValue)
            {
                item.EDate = DateTime.Now;
            }

            try
            {
                Base_ViewModel resultReport = this.GetSDPData(item);
                if (!resultReport.Result)
                {
                    throw resultReport.Result.GetException();
                }

                // insert into PowerBI
                finalResult = this.UpdateBIData(resultReport.Dt, item);
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

        private Base_ViewModel GetSDPData(ExecutedList item)
        {
            List<SqlParameter> listPar = new List<SqlParameter>
            {
                new SqlParameter("@sDate", item.SDate),
                new SqlParameter("@eDate", item.EDate),
                new SqlParameter("@BIFactoryID", item.RgCode),
            };

            string sql = @"
	select
		CountryID = F.CountryID
		,KPICode = F.KPICode
		,FactoryID = o.FactoryID
		,OrderID = o.ID
		,o.StyleID
		,Seq = Order_QS.seq
		,BrandID = o.BrandID
		,Order_QS.BuyerDelivery
		,Order_QS.FtyKPI 
		,Order_QS.ShipmodeID
		,b.OTDExtension 
		,DeliveryByShipmode = Order_QS.ShipmodeID
		,OrderQty = Cast(Order_QS.QTY as int)										
		,Shipmode = Order_QS.ShipmodeID
		,GMTComplete = CASE o.GMTComplete WHEN 'C' THEN 'Y' 
										  WHEN 'S' THEN 'S' 
										  ELSE '' END
		,Order_QS.ReasonID
		,ReasonName = case o.Category when 'B' then r.Name
									  when 'S' then rs.Name
									  else '' end
		,o.MRHandle
		,o.SMR
		,PO.POHandle
		,PO.POSMR
		,o.OrderTypeID
		,ot.isDevSample				
		,c.alias
		,o.MDivisionID 
		,o.localorder
		, OutsdReason = rd.Name
		, ReasonRemark = o.OutstandingRemark
		,o.OnsiteSample
		,[CFAFinalInspectDate]=format(Order_QS.CFAFinalInspectDate, 'yyyy/MM/dd')
		,Order_QS.CFAFinalInspectResult
		,[CFA3rdInspectDate]=format(Order_QS.CFA3rdInspectDate, 'yyyy/MM/dd')
		,Order_QS.CFA3rdInspectResult
		,[IDDReason] = cr.Description
		,o.Dest
		,o.CustPONo
	into #tmp_main
	from Orders o with(nolock)	
	inner join Factory f with(nolock) on o.FactoryID = f.ID	
	inner join Order_QtyShip Order_QS with(nolock) on Order_QS.id = o.id
	left join Country c with(nolock) on f.CountryID = c.ID 
	left join OrderType ot with(nolock) on o.OrderTypeID = ot.ID and o.BrandID = ot.BrandID and o.BrandID = ot.BrandID
	left join PO with(nolock) on o.POID = PO.ID
	left join Reason r with(nolock) on r.id = Order_QS.ReasonID and r.ReasonTypeID = 'Order_BuyerDelivery'          
	left join Reason rs with(nolock) on rs.id = Order_QS.ReasonID and rs.ReasonTypeID = 'Order_BuyerDelivery_sample'
	left join Reason rd with(nolock) on rd.id = o.OutstandingReason and rd.ReasonTypeID = 'Delivery_OutStand'
	left join Brand b with(nolock) on o.BrandID = b.ID
	left join ClogReason cr with(nolock) on cr.ID = Order_QS.ClogReasonID  AND cr.Type='ID'
	where o.Junk = 0  
	and (isnull(ot.IsGMTMaster,0) = 0 or o.OrderTypeID = '') 
	and o.LocalOrder <> 1
	and o.IsForecast <> 1
	and o.Category in ('B', 'G')
	and f.[Type] = 'B'
	and f.IsProduceFty = 1
	and 
	(
		(o.EditDate is not null and o.EditDate between @sDate and @eDate) 
		OR 
		(o.EditDate is null and o.AddDate between @sDate and @eDate)
		OR 
		(Order_QS.EditDate is not null and Order_QS.EditDate between @sDate and @eDate) 
		OR
		(Order_QS.EditDate is null and Order_QS.AddDate between @sDate and @eDate)
	)

	select 
		countPulloutDate = pd.pulloutdate,
		OrderID = pd.OrderID,
		OrderShipmodeSeq = pd.OrderShipmodeSeq,
		maxPulloutDate =p.PulloutDate,
		pd.UKey,
		t.OTDExtension,
		t.BuyerDelivery,
		pd.pulloutdate,
		pdd.ShipQty
	into #tmp_Pullout_Detail_main
	from Pullout_Detail pd with(nolock)
	inner join #tmp_main t on pd.OrderID = t.OrderID and pd.OrderShipmodeSeq = t.Seq 
	inner join Pullout p with(nolock) on p.Id=pd.id AND p.PulloutDate=pd.PulloutDate  
	inner join Pullout_Detail_Detail pdd with(nolock) on pd.Ukey = pdd.Pullout_DetailUKey 

	select 
		countPulloutDate = count(main.countPulloutDate),
		OrderID = main.OrderID,
		OrderShipmodeSeq = main.OrderShipmodeSeq,
		maxPulloutDate =  max(main.maxPulloutDate),
		Qty = sum(IIF(main.pulloutdate <= DATEADD(day, isnull(main.OTDExtension,0), main.BuyerDelivery), main.ShipQty, 0)),
		FailQty = sum(IIF(main.pulloutdate >  DATEADD(day, isnull(main.OTDExtension,0), main.BuyerDelivery), main.shipqty, 0))
	into #tmp_Pullout_Detail_pd
	from #tmp_Pullout_Detail_main main
	Group by main.OrderID,main.OrderShipmodeSeq

	select 
		SewouptQty = sum(x.QaQty)
		,SewLastDate = CONVERT(date,max(SewLastDate))
		,OrderID
	into #tmp_SewingOutput
	from(
		select OrderID, Article, SizeCode, 
			Min(QaQty) as QaQty,
			Min(SewLastDate) as SewLastDate
		from (
			select ComboType, t.OrderID, Article, SizeCode, QaQty = Sum(sdd.QaQty), Max(OutputDate) as SewLastDate
			from SewingOutput_Detail_Detail sdd with(nolock)
			inner join (select distinct OrderID from #tmp_main) t on sdd.OrderID= t.OrderID
			inner join SewingOutput s with(nolock) on sdd.ID = s.ID 
			group by ComboType, t.OrderID, Article, SizeCode
		) as sdd
		group by OrderID, Article, SizeCode
	)x
	group by OrderID

	select 
		oqsD.ID
		,oqsD.Seq
		,SUM(CASE WHEN poPrice > 0 THEN oqsD.Qty ELSE 0 END) AS FOB
		,SUM(CASE WHEN poPrice = 0 THEN oqsD.Qty ELSE 0 END) AS FOC
	into #tmp_getQtyBySeq
	from Order_QtyShip_Detail oqsD with(nolock)
	inner join #tmp_main t ON t.OrderID = oqsD.ID AND t.Seq = oqsD.Seq
	inner join (
		select ID, Seq,Article, SizeCode, dbo.GetPoPriceByArticleSize(id, Article, SizeCode) AS poPrice
		from Order_QtyShip_Detail oqsD with(nolock)
	) po on t.OrderID = po.ID AND t.Seq = po.Seq and oqsD.Article = po.Article and oqsD.SizeCode = po.SizeCode
	group by oqsD.ID, oqsD.Seq
	order by oqsD.ID desc,oqsD.Seq asc 

	select distinct
		pld.OrderID,
		[Seq] = OrderShipmodeSeq,
		c.AddDate
	into #tmp_CReceive_UNIONALL
	from PackingList_Detail pld with(nolock)
	inner join #tmp_main t on t.OrderID = pld.OrderID
	inner join ClogReceive c with(nolock) on pld.ID = c.PackingListID and pld.OrderID = c.OrderID and pld.CTNStartNo = c.CTNStartNo
	where CTNQty > 0
	and c.PackingListID != ''
	and c.OrderID != ''
	and c.CTNStartNo != ''
	union all -- 找拆箱
	select OrderID = pd.OrigOrderID, OrderShipmodeSeq, c.AddDate
	from PackingList_Detail pd with(nolock)
	inner join ClogReceive c with(nolock) on pd.OrigID = c.PackingListID and pd.OrigOrderID = c.OrderID and pd.OrigCTNStartNo = c.CTNStartNo
	inner join #tmp_main t on t.OrderID = pd.OrderID
	where CTNQty > 0
	and c.PackingListID != ''
	and c.OrderID != ''
	and c.CTNStartNo != ''

	select 
		t.OrderID, 
		t.Seq, 
		AddDate = MAX(AddDate)
	into #tmp_CReceive
	from #tmp_CReceive_UNIONALL t
	inner join #tmp_main c on c.OrderID = t.OrderID and c.Seq = t.Seq
	where not exists 
	(
		-- 每個紙箱必須放在 Clog（ReceiveDate 有日期）
		select 1 
		from PackingList_Detail pdCheck with(nolock)
		where t.OrderID = pdCheck.OrderID 
		and t.Seq = pdCheck.OrderShipmodeSeq
		and pdCheck.ReceiveDate is null
	)
	group by t.OrderID, t.Seq

	select  
		[OrderID] = t.OrderID
		,[ordershipmodeseq] = t.Seq
		,[CTNLastReceiveDate] = c.AddDate
	into #tmp_ClogReceive
	from #tmp_main t 
	inner join #tmp_CReceive c on c.OrderID = t.OrderID and c.Seq = t.Seq
	UNION
	select  
		[OrderID] = t.OrderID
		,[ordershipmodeseq] = t.Seq
		,[CTNLastReceiveDate] = NULL
	from #tmp_main t  
	where NOT exists (select 1 from #tmp_CReceive c where c.OrderID = t.OrderID and c.Seq = t.Seq)

	select *
	, [BIFactoryID] = @BIFactoryID
	, [BIInsertDate] = GETDATE()
	from 
	(
		select
			[Country] = t.CountryID
			,[KPIGroup] = t.KPICode
			,[FactoryID] = t.FactoryID
			,[SPNO] = t.OrderID
			,[Style] = t.StyleID
			,[Seq] = t.Seq
			,[Brand] = t.BrandID
			,[BuyerDelivery] = t.BuyerDelivery
			,[FactoryKPI] = isnull(CONVERT(date, t.FtyKPI),'')
			,[Extension] = CONVERT(date,DATEADD(day, isnull(t.OTDExtension,0), t.FtyKPI))
			,[DeliveryByShipmode] = t.ShipmodeID
			,[OrderQty] = t.OrderQty
			,[OnTimeQty] = CASE WHEN t.OnsiteSample = 1 THEN IIF(GetOnsiteSampleFail.isFail = 1 or sew.SewLastDate is null, 0, Cast(t.OrderQty as int))
							  WHEN t.GMTComplete = 'S' and isnull(tpd.maxPulloutDate, packPulloutDate.val) is null THEN Cast(0 as int) --[IST20190675] 若為短交且PullOutDate是空的,不算OnTime也不算Fail,直接給0
							  WHEN isnull(t.isDevSample,0) = 1 then iif(pd2.isFail = 1 or isnull(pd2.PulloutDate, packPulloutDate.val) is null, 0, Cast(t.OrderQty as int))
							  Else Cast(isnull(tpd.Qty, isnull(packOnTimeQty.val, 0)) as int)
							  End
			,[FailQty] = CASE WHEN t.OnsiteSample = 1 THEN IIF(GetOnsiteSampleFail.isFail = 1 or sew.SewLastDate is null, Cast(t.OrderQty as int), 0)
							 WHEN t.GMTComplete = 'S' and isnull(tpd.maxPulloutDate, packPulloutDate.val) is null THEN Cast(0 as int)
							 WHEN isnull(t.isDevSample,0) = 1 then iif(pd2.isFail = 1 or isnull(pd2.PulloutDate, packPulloutDate.val) is null, Cast(t.OrderQty as int), 0)
							 --當pullout與packing都沒又抓到fail qty時，就當作全部fail
							 WHEN tpd.FailQty is null and packFailQty.val is null and packOnTimeQty.val is null then Cast(t.OrderQty as int)
							 Else Cast(isnull(tpd.FailQty, isnull(packFailQty.val, 0)) as int)
							End
			,[ClogRec_OnTimeQty] =	Case When t.GMTComplete = 'S' and ctnr.CTNLastReceiveDate is null then Cast(0 as int)
									When GetCTNFail.isFail = 0 Then Cast(t.OrderQty as int)
									Else Cast(0 as int)
									End

			,[ClogRec_FailQty] =	Case When t.GMTComplete = 'S' and ctnr.CTNLastReceiveDate is null then Cast(0 as int)
									When GetCTNFail.isFail = 1 Then Cast(t.OrderQty as int)
									Else Cast(0 as int)
									End
			,[PullOutDate] = isnull(iif(isnull(t.isDevSample,0) = 1, CONVERT(date, pd2.PulloutDate), CONVERT(date, tpd.maxPulloutDate)), packPulloutDate.val)
			,[Shipmode] = t.ShipmodeID
			,Pullouttimes = (select count(1) from (select distinct ID,OrderID,OrderShipmodeSeq from Pullout_Detail p2 with(nolock) where p2.OrderID = t.OrderID and p2.ShipQty > 0)x)
			,[GarmentComplete] = t.GMTComplete 
			,[ReasonID] = t.ReasonID
			,[OrderReason] =t.ReasonName   
			,[Handle] = dbo.getTPEPass1_ExtNo(t.MRHandle)
			,[SMR] = dbo.getTPEPass1_ExtNo(t.SMR)
			,[POHandle] = isnull(dbo.getTPEPass1_ExtNo(t.POHandle),'')
			,[POSMR] = isnull(dbo.getTPEPass1_ExtNo(t.POSMR),'')
			,[OrderType] = t.OrderTypeID
			,[DevSample] = iif(t.isDevSample = 1, 'Y', '')
			,[SewingQty] = sew.SewouptQty
			,[FOCQty] = getQtyBySeq.FOC
			,[LastSewingOutputDate] = convert(date,sew.SewLastDate)
			,[LastCartonReceivedDate] = ctnr.CTNLastReceiveDate
			,[IDDReason] = t.IDDReason
			,[PartialShipment] = iif(ps.ct>1,'Y','')
			,[Alias] = t.Alias
			,[CFAInspectionDate] = t.CFAFinalInspectDate
			,[CFAFinalInspectionResult] = t.CFAFinalInspectResult
			,[CFA3rdInspectDate] = t.CFA3rdInspectDate
			,[CFA3rdInspectResult] = t.CFA3rdInspectResult
			,[Destination] = t.Dest + '-' + t.Alias
			,[PONO] = t.CustPONo
			,[OutstandingReason]  = t.OutsdReason
			,[ReasonRemark] = t.ReasonRemark
		from #tmp_main t 
		left join #tmp_Pullout_Detail_pd tpd on t.OrderID = tpd.OrderID  and t.Seq = tpd.OrderShipmodeSeq
		left join #tmp_SewingOutput sew on sew.OrderId = t.OrderID
		left join #tmp_getQtyBySeq getQtyBySeq on getQtyBySeq.ID = t.OrderID and getQtyBySeq.Seq = t.Seq
		left join #tmp_ClogReceive ctnr on ctnr.orderid = t.OrderID and ctnr.ordershipmodeseq = t.Seq
		outer apply(
			select ct = count(distinct seq)
			from Order_QtyShip oq with(nolock)
			where oq.id = t.OrderID
		)ps
		outer apply (
			Select top 1 iif(pd.PulloutDate > DATEADD(day, isnull(t.OTDExtension,0), t.BuyerDelivery), 1, 0) isFail, pd.PulloutDate
			From Pullout p with(nolock)
			inner join Pullout_Detail pd with(nolock) on p.ID  = pd.id
			where pd.OrderID = t.OrderID
			and pd.OrderShipmodeSeq = t.Seq
			order by pd.PulloutDate ASC
		) pd2
		outer apply (
			select  [val] = sum(pld.shipqty)
			from PackingList pl with(nolock)
			inner join PackingList_Detail pld with(nolock) on pl.id = pld.id
			where   tpd.OrderID is null
					and pld.OrderID = t.OrderID
					and pld.OrderShipmodeSeq = t.Seq
					and pl.PulloutID is not null
					and pl.pulloutdate <= iif(t.ShipmodeID in ('E/C', 'E/P'), t.BuyerDelivery, DATEADD(day, isnull(t.OTDExtension,0), t.BuyerDelivery))
		) packOnTimeQty
		outer apply (
			select  [val] = sum(pld.shipqty)
			from PackingList pl with (nolock)
			inner join PackingList_Detail pld with (nolock) on pl.id = pld.id
			where   tpd.OrderID is null
					and pld.OrderID = t.OrderID
					and pld.OrderShipmodeSeq = t.Seq
					and pl.PulloutID is not null
					and pl.pulloutdate > iif(t.ShipmodeID in ('E/C', 'E/P'), t.BuyerDelivery, DATEADD(day, isnull(t.OTDExtension,0), t.BuyerDelivery))
		) packFailQty
		outer apply (
			select  [val] = min(pl.PulloutDate)
			from PackingList pl with (nolock)
			inner join PackingList_Detail pld with (nolock) on pl.id = pld.id
			where   tpd.OrderID is null
					and pld.OrderID = t.OrderID
					and pld.OrderShipmodeSeq = t.Seq
					and pl.PulloutID is not null
		) packPulloutDate
		outer apply (
			Select isFail = iif(sew.SewLastDate > t.BuyerDelivery, 1, 0)
		) GetOnsiteSampleFail
		outer apply(
			Select isFail = iif(isnull(ctnr.CTNLastReceiveDate,GetDate()) > t.BuyerDelivery, 1, 0)
		) as GetCTNFail
		where t.OrderQty > 0
		union all
		select
			[Country] = t.CountryID
			,[KPIGroup] = t.KPICode
			,[FactoryID] = t.FactoryID
			,[SPNO] = t.OrderID
			,[Style] = t.StyleID
			,[Seq] = t.Seq
			,[Brand] = t.BrandID
			,[BuyerDelivery] = t.BuyerDelivery
			,[FactoryKPI] = isnull(CONVERT(date, t.FtyKPI),'')
			,[Extension] = CONVERT(date,DATEADD(day, isnull(t.OTDExtension,0), t.FtyKPI))
			,[DeliveryByShipmode] = t.ShipmodeID
			,[OrderQty] = 0
			,[OnTimeQty] = 0
			,[FailQty] =Cast(isnull(t.OrderQty - (CONVERT(int,tpd.Qty) + CONVERT(int,tpd.FailQty)),0) as int)
			,[ClogRec_OnTimeQty] = 0
			,[ClogRec_FailQty] = 0
			,[PullOutDate] = null
			,[Shipmode] = t.ShipmodeID
			,Pullouttimes = 0
			,[GarmentComplete] = t.GMTComplete 
			,[ReasonID] = t.ReasonID
			,[OrderReason] =t.ReasonName   
			,[Handle] = dbo.getTPEPass1_ExtNo(t.MRHandle)
			,[SMR] = dbo.getTPEPass1_ExtNo(t.SMR)
			,[POHandle] = isnull(dbo.getTPEPass1_ExtNo(t.POHandle),'')
			,[POSMR] = isnull(dbo.getTPEPass1_ExtNo(t.POSMR),'')
			,[OrderType] = t.OrderTypeID
			,[DevSample] = iif(t.isDevSample = 1, 'Y', '')
			,[SewingQty] = sew.SewouptQty
			,[FOCQty] = getQtyBySeq.FOC
			,[LastSewingOutputDate] = convert(date,sew.SewLastDate)
			,[LastCartonReceivedDate] = ctnr.CTNLastReceiveDate
			,[IDDReason] = t.IDDReason
			,[PartialShipment] = iif(ps.ct>1,'Y','')
			,[Alias] = t.Alias
			,[CFAInspectionDate] = t.CFAFinalInspectDate
			,[CFAFinalInspectionResult] = t.CFAFinalInspectResult
			,[CFA3rdInspectDate] = t.CFA3rdInspectDate
			,[CFA3rdInspectResult] = t.CFA3rdInspectResult
			,[Destination] = t.Dest + '-' + t.Alias
			,[PONO] = t.CustPONo
			,[OutstandingReason]  = t.OutsdReason
			,[ReasonRemark] = t.ReasonRemark
		from #tmp_main t
		outer apply (
			select isPartial = iif (count(distinct oqs.Seq) > 1 ,'Y','')
			from Order_QtyShip oqs with(nolock)
			where oqs.Id = t.OrderID
		) getPartial
		left join #tmp_Pullout_Detail_pd tpd on t.OrderID = tpd.OrderID  and t.Seq = tpd.OrderShipmodeSeq
		left join #tmp_SewingOutput sew on sew.OrderId = t.OrderID
		left join #tmp_getQtyBySeq getQtyBySeq on getQtyBySeq.ID = t.OrderID and getQtyBySeq.Seq = t.Seq
		left join #tmp_ClogReceive ctnr on ctnr.orderid = t.OrderID and ctnr.ordershipmodeseq = t.Seq
		outer apply(
			select ct=count(distinct seq)
			from Order_QtyShip oq with(nolock)
			where oq.id = t.OrderID
		)ps
		where t.GMTComplete != 'S' 
		and t.OnsiteSample = 0 
		and Cast(isnull(t.OrderQty - (CONVERT(int,tpd.Qty) + CONVERT(int,tpd.FailQty)),0) as int) > 0
		and isnull(t.isDevSample,0) = 0 
	)tb
	order by tb.SPNO, tb.Seq, tb.KPIGroup
";

            Base_ViewModel resultReport = new Base_ViewModel
            {
                Result = DBProxy.Current.Select("Production", sql, listPar, out DataTable dataTable),
            };

            if (!resultReport.Result)
            {
                return resultReport;
            }

            resultReport.Dt = dataTable;
            return resultReport;
        }

        private Base_ViewModel UpdateBIData(DataTable dt, ExecutedList item)
        {
            Base_ViewModel finalResult;
            DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);
            using (sqlConn)
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>()
                {
                    new SqlParameter("@SDate", item.SDate),
                    new SqlParameter("@EDate", item.EDate),
                    new SqlParameter("@IsTrans", item.IsTrans),
                };
                string sql = $@"
	/************* 更新P_SDP的資料*************/
	update t set
	t.[Country]						=	isnull(s.[Country],'')
	,t.[KPIGroup]					=	isnull(s.[KPIGroup],'')
	,t.[Brand]						=	isnull(s.[Brand],'')
	,t.[BuyerDelivery]				=	s.[BuyerDelivery]
	,t.[FactoryKPI]					=	s.[FactoryKPI]
	,t.[Extension]					=	s.[Extension]
	,t.[DeliveryByShipmode]			=	isnull(s.[DeliveryByShipmode],'')
	,t.[OrderQty]					=	isnull(s.[OrderQty],0)
	,t.[OnTimeQty]					=	isnull(s.[OnTimeQty],0)
	,t.[FailQty]					=	isnull(s.[FailQty],0)
	,t.[ClogRec_OnTimeQty]			=	isnull(s.[ClogRec_OnTimeQty],0)
	,t.[ClogRec_FailQty]			=	isnull(s.[ClogRec_FailQty],0)
	,t.[PullOutDate]				=	s.[PullOutDate]
	,t.[Shipmode]					=	isnull(s.[Shipmode],'')
	,t.[Pullouttimes]				=	isnull(s.[Pullouttimes],0)
	,t.[GarmentComplete]			=	isnull(s.[GarmentComplete],'')
	,t.[ReasonID]					=	isnull(s.[ReasonID],'')
	,t.[OrderReason]				=	isnull(s.[OrderReason],'')
	,t.[Handle]						=	isnull(s.[Handle],'')
	,t.[SMR]						=	isnull(s.[SMR],'')
	,t.[POHandle]					=	isnull(s.[POHandle],'')
	,t.[POSMR]						=	isnull(s.[POSMR],'')
	,t.[OrderType]					=	isnull(s.[OrderType],'')
	,t.[DevSample]					=	isnull(s.[DevSample],'')
	,t.[SewingQty]					=	isnull(s.[SewingQty],0)
	,t.[FOCQty]						=	isnull(s.[FOCQty],0)
	,t.[LastSewingOutputDate]		=	s.[LastSewingOutputDate]
	,t.[LastCartonReceivedDate]		=	s.[LastCartonReceivedDate]
	,t.[IDDReason]					=	isnull(s.[IDDReason],'')
	,t.[PartialShipment]			=	isnull(s.[PartialShipment],'')
	,t.[Alias]						=	isnull(s.[Alias],'')
	,t.[CFAInspectionDate]			=	s.[CFAInspectionDate]
	,t.[CFAFinalInspectionResult]	=	isnull(s.[CFAFinalInspectionResult],'')
	,t.[CFA3rdInspectDate]			=	s.[CFA3rdInspectDate]
	,t.[CFA3rdInspectResult]		=	isnull(s.[CFA3rdInspectResult],'')
	,t.[Destination]				=	isnull(s.[Destination],'')
	,t.[PONO]						=	isnull(s.[PONO],'')
	,t.[OutstandingReason]			=	isnull(s.[OutstandingReason],'')
	,t.[ReasonRemark]				=	isnull(s.[ReasonRemark],'')
	,t.[FactoryID]					=	isnull(s.[FactoryID],'')
    ,t.[BIFactoryID]                =   isnull(s.[BIFactoryID],'')
    ,t.[BIInsertDate]               =   s.[BIInsertDate]
	,t.[BIStatus]					=   'New'
	from P_SDP t
	inner join #tmp s on t.[FactoryID] = s.[FactoryID] and
					     t.[SPNO]  = s.[SPNO]	and
						 t.[Style] = s.[Style]	and
						 t.[Seq]   = s.[Seq] and
						 t.[Pullouttimes] = s.[Pullouttimes]

	/************* 新增P_SDP的資料*************/
	insert into P_SDP (
		Country
		,KPIGroup
		,FactoryID
		,SPNo
		,Style
		,Seq
		,Brand
		,BuyerDelivery
		,FactoryKPI
		,Extension
		,DeliveryByShipmode
		,OrderQty
		,OnTimeQty
		,FailQty
		,ClogRec_OnTimeQty
		,ClogRec_FailQty
		,PullOutDate
		,ShipMode
		,Pullouttimes
		,GarmentComplete
		,ReasonID
		,OrderReason
		,Handle
		,SMR
		,POHandle
		,POSMR
		,OrderType
		,DevSample
		,SewingQty
		,FOCQty
		,LastSewingOutputDate
		,LastCartonReceivedDate
		,IDDReason
		,PartialShipment
		,Alias
		,CFAInspectionDate
		,CFAFinalInspectionResult
		,CFA3rdInspectDate
		,CFA3rdInspectResult
		,Destination
		,PONO
		,OutstandingReason
		,ReasonRemark
		,[BIFactoryID]
		,[BIInsertDate]
		,[BIStatus]
	)
	select 
		t.Country
		,t.KPIGroup
		,t.FactoryID
		,t.SPNo
		,t.Style
		,t.Seq
		,t.Brand
		,t.BuyerDelivery
		,t.FactoryKPI
		,t.Extension
		,t.DeliveryByShipmode
		,t.OrderQty
		,t.OnTimeQty
		,t.FailQty
		,t.ClogRec_OnTimeQty
		,t.ClogRec_FailQty
		,t.PullOutDate
		,t.ShipMode
		,t.Pullouttimes
		,t.GarmentComplete
		,t.ReasonID
		,isnull(t.OrderReason,'')
		,t.Handle
		,t.SMR
		,t.POHandle
		,t.POSMR
		,t.OrderType
		,t.DevSample
		,isnull(t.SewingQty,'')
		,t.FOCQty
		,t.LastSewingOutputDate
		,t.LastCartonReceivedDate
		,isnull(t.IDDReason,'')
		,t.PartialShipment
		,t.Alias
		,t.CFAInspectionDate
		,isnull(t.CFAFinalInspectionResult,'')
		,t.CFA3rdInspectDate
		,isnull(t.CFA3rdInspectResult,'')
		,t.Destination
		,t.PONO
		,isnull(t.OutstandingReason,'')
		,t.ReasonRemark
		,isnull(t.[BIFactoryID],'')
		,t.[BIInsertDate]
		,'New'
	from #tmp t
	where not exists 
	(
		select 1 from P_SDP s 
		where  t.[FactoryID] = s.[FactoryID] and
			   t.[SPNO]  = s.[SPNO]	and
			   t.[Style] = s.[Style]	and
			   t.[Seq]   = s.[Seq] and
			   t.[Pullouttimes] = s.[Pullouttimes]
	)

	if @IsTrans = 1
	begin
		insert into [P_SDP_History]([FactoryID], [SPNo], [Style], [Seq], [Pullouttimes], [BIFactoryID], [BIInsertDate])
		Select a.[FactoryID],
			a.[SPNo], 
			a.[Style],
			a.[Seq],
			a.[Pullouttimes],
			a.[BIFactoryID],
			[BIInsertDate] = getDate()
		from P_SDP a
		where not exists
		(
			select 1 from #tmp b 
			where  a.[FactoryID] = b.[FactoryID] and
				   a.[SPNO]  = b.[SPNO]	and
				   a.[Style] = b.[Style] and
				   a.[Seq]   = b.[Seq] and
				   a.[Pullouttimes] = b.[Pullouttimes]
		)
		and exists(select 1 from #tmp b where a.SPNo = b.SPNo)
	end

	-- 刪除掉#tmp不同Key且SPNo相同的資料
	Delete P_SDP
	from P_SDP as a 
	where not exists
	(
		select 1 from #tmp b 
		where  a.[FactoryID] = b.[FactoryID] and
			   a.[SPNO]  = b.[SPNO]	and
			   a.[Style] = b.[Style] and
			   a.[Seq]   = b.[Seq] and
			   a.[Pullouttimes] = b.[Pullouttimes]
	)
	and exists(select 1 from #tmp b where a.SPNo = b.SPNo)

	if @IsTrans = 1
	begin
		insert into [P_SDP_History]([FactoryID], [SPNo], [Style], [Seq], [Pullouttimes], [BIFactoryID], [BIInsertDate])
		Select p.[FactoryID],
			p.[SPNo], 
			p.[Style],
			p.[Seq],
			p.[Pullouttimes],
			p.[BIFactoryID],
			[BIInsertDate] = getDate()
		from P_SDP p
		inner join Production.dbo.Orders o with(nolock) on p.SPNo = o.ID
		inner join MainServer.Production.dbo.OrderType ot with(nolock) on o.OrderTypeID = ot.ID and o.BrandID = ot.BrandID and o.BrandID = ot.BrandID
		where ot.IsGMTMaster = 1
	end

	Delete p
	from P_SDP p
	inner join Production.dbo.Orders o with(nolock) on p.SPNo = o.ID
	inner join MainServer.Production.dbo.OrderType ot with(nolock) on o.OrderTypeID = ot.ID and o.BrandID = ot.BrandID and o.BrandID = ot.BrandID
	where ot.IsGMTMaster = 1

	if @IsTrans = 1
	begin
		insert into [P_SDP_History]([FactoryID], [SPNo], [Style], [Seq], [Pullouttimes], [BIFactoryID], [BIInsertDate])
		Select p.[FactoryID],
			p.[SPNo], 
			p.[Style],
			p.[Seq],
			p.[Pullouttimes],
			p.[BIFactoryID],
			[BIInsertDate] = getDate()
		from P_SDP p
		inner join Production.dbo.Orders o with(nolock) on p.SPNo = o.ID
		inner join Production.dbo.Factory f with(nolock) on o.FactoryID = f.ID
		where f.IsProduceFty = 0
	end

	Delete p
	from P_SDP p
	inner join Production.dbo.Orders o with(nolock) on p.SPNo = o.ID
	inner join Production.dbo.Factory f with(nolock) on o.FactoryID = f.ID
	where f.IsProduceFty = 0

	if @IsTrans = 1
	begin
		insert into [P_SDP_History]([FactoryID], [SPNo], [Style], [Seq], [Pullouttimes], [BIFactoryID], [BIInsertDate])
		Select p.[FactoryID],
			p.[SPNo], 
			p.[Style],
			p.[Seq],
			p.[Pullouttimes],
			p.[BIFactoryID],
			[BIInsertDate] = getDate()
		from P_SDP p
		inner join Production.dbo.Orders o with(nolock) on p.SPNo = o.ID
		where o.Junk = 1
	end

	Delete p
	from P_SDP p
	inner join Production.dbo.Orders o with(nolock) on p.SPNo = o.ID
	where o.Junk = 1

	if @IsTrans = 1
	begin
		insert into [P_SDP_History]([FactoryID], [SPNo], [Style], [Seq], [Pullouttimes], [BIFactoryID], [BIInsertDate])
		Select p.[FactoryID],
			p.[SPNo], 
			p.[Style],
			p.[Seq],
			p.[Pullouttimes],
			p.[BIFactoryID],
			[BIInsertDate] = getDate()
		from P_SDP p
		left join Production.dbo.Orders o with(nolock) on p.SPNo = o.ID
		where o.id is null
	end

	Delete p
	from P_SDP p
	left join Production.dbo.Orders o with(nolock) on p.SPNo = o.ID
	where o.id is null
";

                finalResult = new Base_ViewModel()
                {
                    Result = TransactionClass.ProcessWithDatatableWithTransactionScope(dt, null, sqlcmd: sql, result: out DataTable dataTable, conn: sqlConn, paramters: sqlParameters),
                };
            }

            return finalResult;
        }
    }
}
