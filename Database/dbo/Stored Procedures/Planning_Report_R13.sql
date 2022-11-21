

CREATE PROCEDURE [dbo].[Planning_Report_R13]
	@Year int
	,@Month int
	,@ReportType int = 1 --AGC, Factory, MDP
	,@ByType int = 3 --By AGC, Factory, All 只有在MDP下才有作用
	,@SourceType int = 1 --Adidas, Reebok, All
	,@isDetail bit = 0
AS
BEGIN

	--準備額外的參數，非C#傳入的
	declare @date_S date = DATEFROMPARTS(@Year , @Month , 1)
	declare @date_E date = DATEADD(DAY , -1 , DATEADD(MONTH,3,@date_S))
	declare @dMonth1 varchar(6) = substring(CONVERT(char(10), @date_S, 112),1,6) 
	declare @dMonth2 varchar(6) = substring(CONVERT(char(10), DATEADD(MONTH, 1 , @date_S), 112),1,6) 
	declare @dMonth3 varchar(6) = substring(CONVERT(char(10), @date_E, 112),1,6) 
	declare @Brand varchar(8) = iif(@SourceType = 1, 'ADIDAS', iif(@SourceType = 2, 'Reebok', 'All'))

	select @dMonth1 as 'Month' into #dates union select @dMonth2 union select @dMonth3
	select * from #dates

	select *,dateadd(day,15,CRDDate) as CRDDate15 into #tmpOrders from Orders WITH (NOLOCK)
	where Orders.Category = 'B'
	and ((Orders.PlanDate between @date_S and @date_E and Orders.CRDDate is not null) 
		or (Orders.BuyerDelivery between @date_S and @date_E and Orders.CRDDate is not null) 
		or (Orders.CRDDate between @date_S and @date_E)
		or (Orders.SCIDelivery between @date_S and @date_E)
	)
	and (@Brand = 'All' or Orders.BrandID = @Brand)
	--and Junk = 0

	-- 資料來源Pullout更換成PackingList by ISP20220386
	select p.PulloutDate, pd.OrderID, ShipQty = sum(pd.ShipQty)
	into #tmpPullout 
	from PackingList p, PackingList_Detail pd
	where p.ID = pd.ID
	and p.PulloutID <> ''
	and exists(
		select 1 from #tmpOrders tmp
		where tmp.ID = pd.OrderID
	)
	group by p.PulloutDate, pd.OrderID


	declare @dNow date = getdate()

	--Detail into #tmpOrderDetail
	select 
	BrandID
	,o.BrandID + '-' + f.CountryID + '-' + cty.NameEN as Country
	,o.FactoryID
	,o.BrandAreaCode as AGC
	,OrderTypeID
	,o.ID
	,o.Category
	,iif(o.ProjectID = '', 'Inline', o.ProjectID) as ProjectID
	,o.CRDDate
	,o.BuyerDelivery
	,o.SciDelivery
	,o.PlanDate
	,pd.PulloutDate
	,o.OrigBuyerDelivery
	,o.FirstProduction
	,dLastProduction
	,o.Qty-o.FOCQty as Qty
	,isnull(BalQty_MDP , 0) as BalQty_MDP
	,isnull(BalQty_Dol_MDP , 0) as BalQty_Dol_MDP
	,isnull(SDP , 0) as SDP
	,isnull(OCPnew , 0) as OCPnew
	,isnull(PDPinLine , 0) as PDPinLine
	,isnull(Sdol , 0) as Sdol
	,isnull(SLTPDP , 0) as SLTPDP
	,isnull(FailQtyMDP , 0) as FailQtyMDP
	,isnull(FailQtyMDPDol , 0) as FailQtyMDPDol
	,isnull(M01 , 0) as M01
	,isnull(M03 , 0) as M03
	,isnull(M05 , 0) as M05
	,isnull(M08 , 0) as M08
	,substring(CONVERT(char(10), o.CRDDate, 112),1,6) as ddCRDDate
	,substring(CONVERT(char(10), o.BuyerDelivery, 112),1,6) as ddBuyerDelivery
	,substring(CONVERT(char(10), o.PlanDate, 112),1,6) as ddPlanDate
	into #tmpOrderDetail
	from #tmpOrders o 
	inner join Factory f WITH (NOLOCK) on f.ID = o.FactoryID
	left join Country cty WITH (NOLOCK) on f.CountryID = cty.ID
	outer apply ( select max(pd.PulloutDate) as PulloutDate from #tmpPullout pd where pd.OrderID = o.ID) pd
	outer apply ( select iif(o.Plandate > o.CRDDate ,o.BuyerDelivery, iif(o.SciDelivery > o.Plandate,o.SciDelivery,o.Plandate) ) as dLastProduction) dpro
	outer apply ( select iif(o.OrderTypeID not in ('CC','CM','CR'),
	  iif(@dNow > o.BuyerDelivery, (select sum(ShipQty) from #tmpPullout pd where pd.PulloutDate <= o.CRDDate and pd.OrderID = o.ID)
			,iif(o.BuyerDelivery <= o.CRDDate, o.Qty - o.FOCQty , 0) ) ,0)
	 as BalQty_MDP 
	) bqmdp
	outer apply ( select
	  iif(@dNow > o.BuyerDelivery, (select sum(ShipQty) from #tmpPullout pd where pd.PulloutDate <= o.CRDDate15 and pd.OrderID = o.ID)
	  , iif(o.BuyerDelivery <= o.CRDDate15, o.Qty - o.FOCQty , 0)) as BalQty_Dol_MDP ) bqdmdp
	outer apply ( select iif(o.OrderTypeID not in ('CC','CM','CR') , 
		(select sum(ShipQty) from #tmpPullout pd where pd.PulloutDate <= o.BuyerDelivery and pd.OrderID = o.ID) , 0 ) as SDP) sdp
	outer apply ( select iif(o.FirstProduction <= o.PlanDate and o.OrderTypeID not in ('CC','CM','CR') , o.Qty - o.FOCQty , 0) as OCPnew) ocpn --C07
	outer apply ( select iif(dLastProduction <= o.PlanDate and o.OrderTypeID not in ('CC','CM','CR') , o.Qty - o.FOCQty , 0) as PDPinLine) pdpline --C09
	outer apply ( select iif(o.OrderTypeID in ('CC','CM','CR') , 
		iif(@dNow > o.BuyerDelivery, (select sum(ShipQty) from #tmpPullout pd where pd.PulloutDate <= o.PlanDate and pd.OrderID = o.ID )
			, iif(o.BuyerDelivery <= o.PlanDate , o.Qty - o.FOCQty , 0 ) ) , 0 ) as Sdol
	) sdol --C12
	outer apply ( select sum(ShipQty) as FailQtyMDP from #tmpPullout pd where pd.PulloutDate > o.CRDDate and pd.OrderID = o.ID ) fqmdp
	outer apply ( select sum(ShipQty) as FailQtyMDPDol from #tmpPullout pd where pd.PulloutDate > o.CRDDate15 and pd.OrderID = o.ID ) fqmdpdol
	outer apply ( select iif(dLastProduction <= o.PlanDate and o.OrderTypeID in ('CC','CM','CR') , o.Qty - o.FOCQty , 0) as SLTPDP) sltpdp --C13
	outer apply ( select sum(tmp.Qty - tmp.FOCQty) as M01 from #tmpOrders tmp where tmp.ID = o.ID and tmp.OrderTypeID not in ('CC','CM','CR') and tmp.CRDDate between @date_S and @date_E ) m01
	outer apply ( select sum(tmp.Qty - tmp.FOCQty) as M03 from #tmpOrders tmp where tmp.ID = o.ID and tmp.OrderTypeID not in ('CC','CM','CR') and tmp.BuyerDelivery between @date_S and @date_E and CRDDate is not null) m03
	outer apply ( select sum(tmp.Qty - tmp.FOCQty) as M05 from #tmpOrders tmp where tmp.ID = o.ID and tmp.OrderTypeID not in ('CC','CM','CR') and tmp.PlanDate between @date_S and @date_E and CRDDate is not null) m05
	outer apply ( select sum(tmp.Qty - tmp.FOCQty) as M08 from #tmpOrders tmp where tmp.ID = o.ID and tmp.OrderTypeID in ('CC','CM','CR') and tmp.PlanDate between @date_S and @date_E and CRDDate is not null) m08

	order by BrandID, f.CountryID, o.FactoryID, o.BrandAreaCode, OrderTypeID, o.CRDDate, o.ID

	if(@ReportType = 1)
	begin
	--AGC Report Detail ***********************************************************************************************************************
	--第一段依照上面彙總的分子分母，By每個日期欄位丟進所屬的年月，目前三個CRDDate、BuyerDelivery、PlanDate
	select dd.dd, dd.ProjectID, dd.AGC, dd.Country, crd.MDP,crd.[Dol-MDP],buy.SDP,pl.[OCP-New],pl.[PDP in Line],pl.[SDol 0],pl.[SLT PDP] into #AGCRptDetail from (
		select distinct ddCRDDate as dd,ProjectID,AGC,Country from #tmpOrderDetail where ddCRDDate in (select * from #dates)
		union select distinct ddCRDDate,ProjectID,'','' from #tmpOrderDetail where ddCRDDate in (select * from #dates) --union 小計
		union select distinct ddCRDDate,'合計','','' from #tmpOrderDetail where ddCRDDate in (select * from #dates) --union 合計
	) dd
	left join (
		select o.ddCRDDate, o.ProjectID, o.AGC, o.Country
			,iif(SUM(M01) = 0 , 0 , CAST(sum(BalQty_MDP) AS numeric) / CAST(sum(M01) AS numeric)) as MDP
			,iif(SUM(M01) = 0 , 0 , CAST(sum(BalQty_Dol_MDP)  AS numeric) / CAST(sum(M01)AS numeric)) as [Dol-MDP]
		from #tmpOrderDetail o where ddCRDDate in (select * from #dates)
		group by o.ddCRDDate,o.ProjectID,o.AGC,o.Country
		union
		select o.ddCRDDate, o.ProjectID, '', ''
			,iif(SUM(M01) = 0 , 0 , CAST(sum(BalQty_MDP) AS numeric) / CAST(sum(M01) AS numeric)) as MDP
			,iif(SUM(M01) = 0 , 0 , CAST(sum(BalQty_Dol_MDP) AS numeric) / CAST(sum(M01) AS numeric)) as [Dol-MDP]
		from #tmpOrderDetail o  where ddCRDDate in (select * from #dates)
		group by o.ddCRDDate, o.ProjectID
		union
		select o.ddCRDDate, '合計', '', ''
			,iif(SUM(M01) = 0 , 0 , CAST(sum(BalQty_MDP) AS numeric) / CAST(sum(M01) AS numeric)) as MDP
			,iif(SUM(M01) = 0 , 0 , CAST(sum(BalQty_Dol_MDP) AS numeric) / CAST(sum(M01) AS numeric)) as [Dol-MDP]
		from #tmpOrderDetail o  where ddCRDDate in (select * from #dates)
		group by o.ddCRDDate
	) crd on dd.dd = crd.ddCRDDate and dd.ProjectID = crd.ProjectID and dd.AGC = crd.AGC and dd.Country = crd.Country
	left join (
		select o.ddBuyerDelivery, o.ProjectID, o.AGC, o.Country
			,iif(SUM(M03) = 0 , 0 , CAST(sum(SDP) AS numeric) / CAST(sum(M03) AS numeric)) as [SDP]
		from #tmpOrderDetail o where ddBuyerDelivery in (select * from #dates)
		group by o.ddBuyerDelivery,o.ProjectID,o.AGC,o.Country
		union
		select o.ddBuyerDelivery, o.ProjectID, '', ''	
			,iif(SUM(M03) = 0 , 0 , CAST(sum(SDP) AS numeric) / CAST(sum(M03) AS numeric)) as [SDP]
		from #tmpOrderDetail o  where ddBuyerDelivery in (select * from #dates)
		group by o.ddBuyerDelivery, o.ProjectID
		union 
		select o.ddBuyerDelivery, '合計', '', ''	
			,iif(SUM(M03) = 0 , 0 , CAST(sum(SDP) AS numeric) / CAST(sum(M03) AS numeric)) as [SDP]
		from #tmpOrderDetail o  where ddBuyerDelivery in (select * from #dates)
		group by o.ddBuyerDelivery
	) buy on dd.dd = buy.ddBuyerDelivery and dd.ProjectID = buy.ProjectID and dd.AGC = buy.AGC and dd.Country = buy.Country
	left join (
		select o.ddPlanDate, o.ProjectID, o.AGC, o.Country
			,iif(SUM(M05) = 0 , 0 , CAST(sum(OCPnew) AS numeric) / CAST(sum(M05) AS numeric)) as [OCP-New]
			,iif(SUM(M05) = 0 , 0 , CAST(sum(PDPinLine) AS numeric) / CAST(sum(M05) AS numeric)) as [PDP in Line]
			,iif(SUM(M08) = 0 , 0 , CAST(sum(Sdol) AS numeric) / CAST(sum(M08) AS numeric)) as [SDol 0]
			,iif(SUM(M08) = 0 , 0 , CAST(sum(SLTPDP) AS numeric) / CAST(sum(M08) AS numeric)) as [SLT PDP]
		from #tmpOrderDetail o where ddPlanDate in (select * from #dates)
		group by o.ddPlanDate,o.ProjectID,o.AGC,o.Country
		union
		select o.ddPlanDate, o.ProjectID, '', ''
			,iif(SUM(M05) = 0 , 0 , CAST(sum(OCPnew) AS numeric) / CAST(sum(M05) AS numeric)) as [OCP-New]
			,iif(SUM(M05) = 0 , 0 , CAST(sum(PDPinLine) AS numeric) / CAST(sum(M05) AS numeric)) as [PDP in Line]
			,iif(SUM(M08) = 0 , 0 , CAST(sum(Sdol) AS numeric) / CAST(sum(M08) AS numeric)) as [SDol 0]
			,iif(SUM(M08) = 0 , 0 , CAST(sum(SLTPDP) AS numeric) / CAST(sum(M08) AS numeric)) as [SLT PDP]
		from #tmpOrderDetail o where ddPlanDate in (select * from #dates)
		group by o.ddPlanDate, o.ProjectID
		union
		select o.ddPlanDate, '合計', '', ''
			,iif(SUM(M05) = 0 , 0 , CAST(sum(OCPnew) AS numeric) / CAST(sum(M05) AS numeric)) as [OCP-New]
			,iif(SUM(M05) = 0 , 0 , CAST(sum(PDPinLine) AS numeric) / CAST(sum(M05) AS numeric)) as [PDP in Line]
			,iif(SUM(M08) = 0 , 0 , CAST(sum(Sdol) AS numeric) / CAST(sum(M08) AS numeric)) as [SDol 0]
			,iif(SUM(M08) = 0 , 0 , CAST(sum(SLTPDP) AS numeric) / CAST(sum(M08) AS numeric)) as [SLT PDP]
		from #tmpOrderDetail o where ddPlanDate in (select * from #dates)
		group by o.ddPlanDate
	) pl on dd.dd = pl.ddPlanDate and dd.ProjectID = pl.ProjectID and dd.AGC = pl.AGC and dd.Country = pl.Country

	--Full Join By 三個月
	select * from ( select 
		iif(d1.ProjectID is null, iif(d2.ProjectID is null, d3.ProjectID, d2.ProjectID), d1.ProjectID) as ProjectID
		,iif(d1.AGC is null, iif(d2.AGC is null, d3.AGC, d2.AGC), d1.AGC) as AGC
		,iif(d1.Country is null, iif(d2.Country is null, d3.Country, d2.Country), d1.Country) as Country
		,isnull(d1.MDP,0) as [MDP_1], isnull(d1.[Dol-MDP],0) as [Dol-MDP_1], isnull(d1.SDP,0) as [SDP_1], isnull(d1.[OCP-New],0) as [OCP-New_1], isnull(d1.[PDP in Line],0) as [PDP in Line_1], isnull(d1.[SDol 0],0) as [SDol 0_1], isnull(d1.[SLT PDP],0) as [SLT PDP_1]
		,isnull(d2.MDP,0) as [MDP_2], isnull(d2.[Dol-MDP],0) as [Dol-MDP_2], isnull(d2.SDP,0) as [SDP_2], isnull(d2.[OCP-New],0) as [OCP-New_2], isnull(d2.[PDP in Line],0) as [PDP in Line_2], isnull(d2.[SDol 0],0) as [SDol 0_2], isnull(d2.[SLT PDP],0) as [SLT PDP_2]
		,isnull(d3.MDP,0) as [MDP_3], isnull(d3.[Dol-MDP],0) as [Dol-MDP_3], isnull(d3.SDP,0) as [SDP_3], isnull(d3.[OCP-New],0) as [OCP-New_3], isnull(d3.[PDP in Line],0) as [PDP in Line_3], isnull(d3.[SDol 0],0) as [SDol 0_3], isnull(d3.[SLT PDP],0) as [SLT PDP_3]
	from ( select * from #AGCRptDetail where dd = @dMonth1 ) d1 
	full join ( select * from #AGCRptDetail where dd = @dMonth2 )
	d2 on d1.ProjectID = d2.ProjectID and d1.AGC = d2.AGC and d1.Country = d2.Country
	full join ( select * from #AGCRptDetail where dd = @dMonth3 ) 
	d3 on d1.ProjectID = d3.ProjectID and d1.AGC = d3.AGC and d1.Country = d3.Country
	) c order by c.ProjectID,c.AGC

	end
	else if(@ReportType = 2)
	begin
	--Factory Report Detail 跟上面差不多的動作，但是AGC依照工廠Group by 其餘沒甚麼差別***********************************************************************************************************************
	select dd.dd, dd.ProjectID, dd.AGC, dd.Country, crd.MDP,crd.[Dol-MDP],buy.SDP,pl.[OCP-New],pl.[PDP in Line],pl.[SDol 0],pl.[SLT PDP] into #FtyRptDetail from (
		select distinct ddCRDDate as dd,ProjectID,AGC+'-'+FactoryID as AGC,Country from #tmpOrderDetail where ddCRDDate in (select * from #dates)
		union select distinct ddCRDDate,ProjectID,AGC,'' from #tmpOrderDetail where ddCRDDate in (select * from #dates) --union 小計
		union select distinct ddCRDDate,'合計','','' from #tmpOrderDetail where ddCRDDate in (select * from #dates) --union 合計
	) dd
	left join (
		select o.ddCRDDate, o.ProjectID, o.AGC+'-'+o.FactoryID as AGC, o.Country
			,iif(SUM(M01) = 0 , 0 , CAST(sum(BalQty_MDP) AS numeric) / CAST(sum(M01) AS numeric)) as MDP
			,iif(SUM(M01) = 0 , 0 , CAST(sum(BalQty_Dol_MDP) AS numeric) / CAST(sum(M01) AS numeric)) as [Dol-MDP]
		from #tmpOrderDetail o where ddCRDDate in (select * from #dates)
		group by o.ddCRDDate,o.ProjectID,o.AGC,o.Country,o.FactoryID
		union
		select o.ddCRDDate, o.ProjectID, o.AGC, ''
			,iif(SUM(M01) = 0 , 0 , CAST(sum(BalQty_MDP) AS numeric) / CAST(sum(M01) AS numeric)) as MDP
			,iif(SUM(M01) = 0 , 0 , CAST(sum(BalQty_Dol_MDP) AS numeric) / CAST(sum(M01) AS numeric)) as [Dol-MDP]
		from #tmpOrderDetail o where ddCRDDate in (select * from #dates)
		group by o.ddCRDDate, o.ProjectID,o.AGC
		union
		select o.ddCRDDate, '合計', '', ''
			,iif(SUM(M01) = 0 , 0 , CAST(sum(BalQty_MDP) AS numeric) / CAST(sum(M01) AS numeric)) as MDP
			,iif(SUM(M01) = 0 , 0 , CAST(sum(BalQty_Dol_MDP) AS numeric) / CAST(sum(M01) AS numeric)) as [Dol-MDP]
		from #tmpOrderDetail o where ddCRDDate in (select * from #dates)
		group by o.ddCRDDate
	) crd on dd.dd = crd.ddCRDDate and dd.ProjectID = crd.ProjectID and dd.AGC = crd.AGC and dd.Country = crd.Country
	left join (
		select o.ddBuyerDelivery, o.ProjectID, o.AGC+'-'+o.FactoryID as AGC, o.Country
			,iif(SUM(M03) = 0 , 0 , CAST(sum(SDP) AS numeric) / CAST(sum(M03) AS numeric)) as [SDP]
		from #tmpOrderDetail o where ddBuyerDelivery in (select * from #dates)
		group by o.ddBuyerDelivery,o.ProjectID,o.AGC,o.Country,o.FactoryID
		union
		select o.ddBuyerDelivery, o.ProjectID, o.AGC, ''
			,iif(SUM(M03) = 0 , 0 , CAST(sum(SDP) AS numeric) / CAST(sum(M03) AS numeric)) as [SDP]
		from #tmpOrderDetail o  where ddBuyerDelivery in (select * from #dates)
		group by o.ddBuyerDelivery, o.ProjectID,o.AGC
		union
		select o.ddBuyerDelivery, '合計', '', ''
			,iif(SUM(M03) = 0 , 0 , CAST(sum(SDP) AS numeric) / CAST(sum(M03) AS numeric)) as [SDP]
		from #tmpOrderDetail o  where ddBuyerDelivery in (select * from #dates)
		group by o.ddBuyerDelivery
	) buy on dd.dd = buy.ddBuyerDelivery and dd.ProjectID = buy.ProjectID and dd.AGC = buy.AGC and dd.Country = buy.Country
	left join (
		select o.ddPlanDate, o.ProjectID, o.AGC+'-'+o.FactoryID as AGC, o.Country
			,iif(SUM(M05) = 0 , 0 , CAST(sum(OCPnew) AS numeric) / CAST(sum(M05) AS numeric)) as [OCP-New]
			,iif(SUM(M05) = 0 , 0 , CAST(sum(PDPinLine) AS numeric) / CAST(sum(M05) AS numeric)) as [PDP in Line]
			,iif(SUM(M08) = 0 , 0 , CAST(sum(Sdol) AS numeric) / CAST(sum(M08) AS numeric)) as [SDol 0]
			,iif(SUM(M08) = 0 , 0 , CAST(sum(SLTPDP) AS numeric) / CAST(sum(M08) AS numeric)) as [SLT PDP]
		from #tmpOrderDetail o where ddPlanDate in (select * from #dates)
		group by o.ddPlanDate,o.ProjectID,o.AGC,o.Country,o.FactoryID
		union
		select o.ddPlanDate, o.ProjectID, o.AGC, ''
			,iif(SUM(M05) = 0 , 0 , CAST(sum(OCPnew) AS numeric) / CAST(sum(M05) AS numeric)) as [OCP-New]
			,iif(SUM(M05) = 0 , 0 , CAST(sum(PDPinLine) AS numeric) / CAST(sum(M05) AS numeric)) as [PDP in Line]
			,iif(SUM(M08) = 0 , 0 , CAST(sum(Sdol) AS numeric) / CAST(sum(M08) AS numeric)) as [SDol 0]
			,iif(SUM(M08) = 0 , 0 , CAST(sum(SLTPDP) AS numeric) / CAST(sum(M08) AS numeric)) as [SLT PDP]
		from #tmpOrderDetail o where ddPlanDate in (select * from #dates)
		group by o.ddPlanDate, o.ProjectID,o.AGC
		union
		select o.ddPlanDate, '合計', '', ''
			,iif(SUM(M05) = 0 , 0 , CAST(sum(OCPnew) AS numeric) / CAST(sum(M05) AS numeric)) as [OCP-New]
			,iif(SUM(M05) = 0 , 0 , CAST(sum(PDPinLine) AS numeric) / CAST(sum(M05) AS numeric)) as [PDP in Line]
			,iif(SUM(M08) = 0 , 0 , CAST(sum(Sdol) AS numeric) / CAST(sum(M08) AS numeric)) as [SDol 0]
			,iif(SUM(M08) = 0 , 0 , CAST(sum(SLTPDP) AS numeric) / CAST(sum(M08) AS numeric)) as [SLT PDP]
		from #tmpOrderDetail o where ddPlanDate in (select * from #dates)
		group by o.ddPlanDate
	) pl on dd.dd = pl.ddPlanDate and dd.ProjectID = pl.ProjectID and dd.AGC = pl.AGC and dd.Country = pl.Country
	order by dd.dd, crd.ProjectID,crd.Country

	select * from ( select 
		iif(d1.ProjectID is null, iif(d2.ProjectID is null, d3.ProjectID, d2.ProjectID), d1.ProjectID) as ProjectID
		,iif(d1.AGC is null, iif(d2.AGC is null, d3.AGC, d2.AGC), d1.AGC) as AGC
		,iif(d1.Country is null, iif(d2.Country is null, d3.Country, d2.Country), d1.Country) as Country
		,isnull(d1.MDP,0) as [MDP_1], isnull(d1.[Dol-MDP],0) as [Dol-MDP_1], isnull(d1.SDP,0) as [SDP_1], isnull(d1.[OCP-New],0) as [OCP-New_1], isnull(d1.[PDP in Line],0) as [PDP in Line_1], isnull(d1.[SDol 0],0) as [SDol 0_1], isnull(d1.[SLT PDP],0) as [SLT PDP_1]
		,isnull(d2.MDP,0) as [MDP_2], isnull(d2.[Dol-MDP],0) as [Dol-MDP_2], isnull(d2.SDP,0) as [SDP_2], isnull(d2.[OCP-New],0) as [OCP-New_2], isnull(d2.[PDP in Line],0) as [PDP in Line_2], isnull(d2.[SDol 0],0) as [SDol 0_2], isnull(d2.[SLT PDP],0) as [SLT PDP_2]
		,isnull(d3.MDP,0) as [MDP_3], isnull(d3.[Dol-MDP],0) as [Dol-MDP_3], isnull(d3.SDP,0) as [SDP_3], isnull(d3.[OCP-New],0) as [OCP-New_3], isnull(d3.[PDP in Line],0) as [PDP in Line_3], isnull(d3.[SDol 0],0) as [SDol 0_3], isnull(d3.[SLT PDP],0) as [SLT PDP_3]
	from ( select * from #FtyRptDetail where dd = @dMonth1 ) d1 
	full join ( select * from #FtyRptDetail  where dd = @dMonth2 )
	d2 on d1.ProjectID = d2.ProjectID and d1.AGC = d2.AGC and d1.Country = d2.Country
	full join ( select * from #FtyRptDetail  where dd = @dMonth3 ) 
	d3 on d1.ProjectID = d3.ProjectID and d1.AGC = d3.AGC and d1.Country = d3.Country
	) c order by c.ProjectID,c.AGC

	end
	else if(@ReportType = 3)
	begin
	--MDP Report – Detail
	declare @MDPCmds varchar(max) = '
	select dd.Month, a.Type, isnull(b.PO,0) as PO, isnull(b.QTY,0) as QTY, isnull(b.MDP,0) as MDP, isnull(b.[%],0) as [%], isnull(b.[Failed-PO],0) as [Failed-PO], isnull(b.[Failed-QTY],0) as [Failed-QTY] from (
		select ''CRD ORDERS'' as Type, 1 as idx union select ''CR/CM ORDERS'', 2 as idx union select ''NON-CR ORDERS'', 3 as idx union select ''MDP-Dol (Non-CR)'', 4 as idx 
	) a 
	left join (select * from #dates) dd on 1 = 1
	left join (
		select ddCRDDate, 1 as idx,COUNT(ID) as PO,SUM(Qty) as QTY,sum(BalQty_MDP) as MDP, CAST(sum(BalQty_MDP) AS numeric) / CAST(SUM(Qty) AS numeric) as ''%'', (select count(ID) as failedQty from #tmpOrderDetail where FailQtyMDP > 0 and @where@ ) as ''Failed-PO'', sum(FailQtyMDP) as [Failed-QTY]
		from #tmpOrderDetail
		where 1 = 1 and ddCRDDate in (select * from #dates) and @where@ group by ddCRDDate
		union select ddCRDDate, 2 as idx,COUNT(ID) as PO,SUM(Qty) as QTY,sum(BalQty_MDP) as MDP, CAST(sum(BalQty_MDP) AS numeric) / CAST(SUM(Qty) AS numeric) as ''%'', (select count(ID) as failedQty from #tmpOrderDetail where FailQtyMDP > 0 and @where@ ) as ''Failed-PO'', sum(FailQtyMDP) as [Failed-QTY]
		from #tmpOrderDetail
		where 1 = 1 and OrderTypeID in (''CC'',''CR'',''CM'') and ddCRDDate in (select * from #dates) and @where@ group by ddCRDDate
		union select ddCRDDate, 3 as idx,COUNT(ID) as PO,SUM(Qty) as QTY,sum(BalQty_MDP) as MDP,  CAST(sum(BalQty_MDP) AS numeric) /  CAST(SUM(Qty) AS numeric) as ''%'', (select count(ID) as failedQty from #tmpOrderDetail where FailQtyMDP > 0 and @where@ ) as ''Failed-PO'', sum(FailQtyMDP) as [Failed-QTY]
		from #tmpOrderDetail
		where 1 = 1 and OrderTypeID not in (''CC'',''CR'',''CM'') and ddCRDDate in (select * from #dates) and @where@ group by ddCRDDate
		union select ddCRDDate, 4 as idx,COUNT(ID) as PO,SUM(Qty) as QTY,sum(BalQty_MDP) as MDP, CAST(sum(BalQty_MDP) AS numeric) / CAST(SUM(Qty) AS numeric) as ''%'', (select count(ID) as failedQty from #tmpOrderDetail where FailQtyMDP > 0 and @where@ ) as ''Failed-PO'', sum(FailQtyMDP) as [Failed-QTY]
		from #tmpOrderDetail
		where 1 = 1 and OrderTypeID not in (''CC'',''CR'',''CM'') and BalQty_Dol_MDP > 0 and ddCRDDate in (select * from #dates) and @where@ group by ddCRDDate
	) b on a.idx = b.idx and dd.Month = b.ddCRDDate order by dd.Month, a.idx'

	declare @max int = 1 , @cnt int = 1
	declare @wheres varchar(max) = ''
	declare @ItemTbl table (idx int, item varchar(50))
	declare @Item varchar(50)
	if(@ByType = 1)
	begin
		insert into @ItemTbl select ROW_NUMBER()OVER(ORDER BY AGC) as idx, AGC from #tmpOrderDetail group by AGC order by AGC
		set @wheres = 'AGC = ''@item'''
	end
	else if(@ByType = 2)
	begin
		insert into @ItemTbl select ROW_NUMBER()OVER(ORDER BY FactoryID) as idx, FactoryID from #tmpOrderDetail group by FactoryID order by FactoryID
		set @wheres = 'FactoryID = ''@item'''
	end
	else if(@ByType = 3)
	begin
		--insert into @ItemTbl select ROW_NUMBER()OVER(ORDER BY ddCRDDate) as idx, ddCRDDate from #tmpOrderDetail where ddCRDDate in (select * from #dates) group by ddCRDDate order by ddCRDDate
		insert into @ItemTbl values(1,1)
		set @wheres = ' 1 = 1 '
	end

	select * from @ItemTbl

	select @cnt=min(idx),@max=max(idx) from @ItemTbl
		While @cnt <= @max
		begin
			Select @Item = item From @ItemTbl Where idx = @cnt;
			declare @where varchar(50) = REPLACE(@wheres,'@item',@Item)
			declare @MDPCmd varchar(max) = REPLACE(@MDPCmds,'@where@',@where)
			set @MDPCmd = REPLACE(@MDPCmd,'@dMonth1',@dMonth1)
			set @MDPCmd = REPLACE(@MDPCmd,'@dMonth2',@dMonth2)
			set @MDPCmd = REPLACE(@MDPCmd,'@dMonth3',@dMonth3)

			exec (@MDPCmd)

			set @cnt = @cnt + 1;
		end
	end

	if(@isDetail = 1) select * from #tmpOrderDetail

	--return
	IF (OBJECT_ID('tempdb..#FtyRptDetail') is not null) drop table #FtyRptDetail
	IF (OBJECT_ID('tempdb..#AGCRptDetail') is not null) drop table #AGCRptDetail
	IF (OBJECT_ID('tempdb..#item1') is not null) drop table #item1
	IF (OBJECT_ID('tempdb..#item2') is not null) drop table #item2
	IF (OBJECT_ID('tempdb..#item3') is not null) drop table #item3
	drop table #tmpOrderDetail
	drop table #tmpOrders
	drop table #tmpPullout
	drop table #dates
	
END