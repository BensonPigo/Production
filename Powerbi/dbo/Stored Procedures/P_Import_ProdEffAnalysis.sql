-- =============================================
-- Description:	Import BI Table P_ProdEffAnalysis
-- =============================================
Create PROCEDURE [dbo].[P_Import_ProdEffAnalysis]
	@StartDate date,
	@EndDate date	
AS
BEGIN	
	SET NOCOUNT ON;

	declare @SDate date = @StartDate
	declare @EDate date = @EndDate

	-- 一週七天內資料
	if @StartDate is null
	begin
		set @SDate = '2018/01/01'
	end
	else
	begin
		set @SDate = @StartDate
	end

	if @EndDate is null
	begin 
		set @EDate = '2999/12/31'
	end
	else	
	begin
		set @EDate = @EndDate
	end

-- Main data
Select
[RS] = iif(ProductionUnit = 'TMS', 'CPU', iif(ProductionUnit = 'QTY', 'AMT','')),
otc.ArtworkTypeID,
at.IsTtlTMS,
o.ID, 
o.ProgramID,
o.StyleID,
o.SeasonID
, [BrandID] = case 
		when o.BrandID != 'SUBCON-I' then o.BrandID
		when Order2.BrandID is not null then Order2.BrandID
		when StyleBrand.BrandID is not null then StyleBrand.BrandID
		else o.BrandID end
, o.FactoryID
,o.POID 
,o.Category
,o.CdCodeID 
,o.CPU
,[ArtworkCPU] = ROUND(otc.TMS/1400,3)
,CPURate = o.CPUFactor * o.CPU  
,o.BuyerDelivery
,o.SCIDelivery
,so.SewingLineID 
,so.ManPower
,sod.ComboType
,sod.WorkHour
,sod.QAQty 
,QARate = sod.QAQty * isnull(Production.dbo.[GetOrderLocation_Rate](o.id ,sod.ComboType)/100,1)
,Round(sod.WorkHour * so.ManPower,2) as TotalManHour 
,CDDesc = s.Description 
,StyleDesc = s.Description
,s.ModularParent,
s.CPUAdjusted
,OutputDate
,Shift
, Team
,SCategory = so.Category
,o.CPUFactor
,[FtyZone]=f.FtyZone
,orderid
,Rate = isnull(Production.dbo.[GetOrderLocation_Rate]( o.id ,sod.ComboType)/100,1) 
,ActManPower= so.Manpower
, [MockupCPU] = isnull(mo.Cpu,0)
, [MockupCPUFactor] = isnull(mo.CPUFactor,0)
into #stmp
from Production.dbo.Orders o WITH (NOLOCK) 
inner join Production.dbo.SewingOutput_Detail sod WITH (NOLOCK) on sod.OrderId = o.ID
inner join Production.dbo.SewingOutput so WITH (NOLOCK) on so.ID = sod.ID and so.Shift <> 'O'  
inner join Production.dbo.Style s WITH (NOLOCK) on s.Ukey = o.StyleUkey
inner join Production.dbo.Factory f WITH (NOLOCK) on o.FactoryID=f.id
inner join Production.dbo.Brand b WITH (NOLOCK) on o.BrandID=b.ID
left join Production.dbo.MockupOrder mo WITH (NOLOCK) on mo.ID = sod.OrderId
outer apply( select BrandID from Production.dbo.orders o1 where o.CustPONo = o1.id) Order2
outer apply( select top 1 BrandID from Production.dbo.Style where id = o.StyleID 
    and SeasonID = o.SeasonID and BrandID != 'SUBCON-I') StyleBrand
inner join Production.dbo.Order_TmsCost otc on otc.id = o.ID
inner join Production.dbo.ArtworkType at on at.ID = otc.ArtworkTypeID
Where 1=1
and f.IsProduceFty = '1'
--排除non sister的資料o.LocalOrder = 1 and o.SubconInSisterFty = 0
and ((o.LocalOrder = 1 and o.SubconInType in ('1','2')) or (o.LocalOrder = 0 and o.SubconInType in ('0','3')))
and so.OutputDate between @SDate and @EDate
 AND o.Category in ('B','S') AND f.Type <>'S' 
 and Classify in ('I','A','P') 
 and IsPrintToCMP=1

 --by Program
select 
a.ArtworkTypeID
,a.IsTtlTMS
,a.RS
    , a.ProgramID
    , a.StyleID
    , a.SeasonID
    , a.BrandID
    , a.FtyZone
    , a.FactoryID
    , a.POID
    , a.Category
    , a.CdCodeID 
    , sty.CDCodeNew
    , sty.ProductType
    , sty.FabricType
    , sty.Lining
    , sty.Gender
    , sty.Construction
	,artworkcpu
    , CPU = sum(a.CPU)
    , CPURate = sum(a.CPURate)
    , a.BuyerDelivery, a.SCIDelivery, a.SewingLineID , a.ComboType
    , ManPower = sum(a.ManPower)
    , WorkHour = sum(Round(a.WorkHour,2)) 
    , QARate = convert(numeric(12,2)
    , sum(a.QARate))
    , TotalManHour = sum(ROUND( ActManPower * WorkHour, 2))
    , TotalCPUOut = Sum(ROUND(IIF(Category='M',MockupCPU*MockupCPUFactor, CPU*CPUFactor*Rate)*QAQty,3))
	, TotalArtwrokCPUOut = Sum(
		case when Category = 'M' then round(ArtworkCPU*MockupCPUFactor * QAQty,3)
		when IsTtlTMS = 1 and ArtworkTypeID = 'SEWING' then Round( Sewing.value * QAQty,3)
		else  round(ArtworkCPU*CPUFactor*Rate * QAQty, 3)
		end
	)
    , a.StyleDesc
    , a.ModularParent
    , CPUAdjusted = sum(a.CPUAdjusted)
    , QAQty = sum(a.QAQty) 
    ,a.OutputDate
into #tmpz
from #stmp a
Outer apply (
	SELECT s.CDCodeNew
        , ProductType = r2.Name
		, FabricType = r1.Name
		, s.Lining
		, s.Gender
		, Construction = d1.Name
	FROM Production.dbo.Style s WITH(NOLOCK)
	left join Production.dbo.DropDownList d1 WITH(NOLOCK) on d1.type= 'StyleConstruction' and d1.ID = s.Construction
	left join Production.dbo.Reason r1 WITH(NOLOCK) on r1.ReasonTypeID= 'Fabric_Kind' and r1.ID = s.FabricType
	left join Production.dbo.Reason r2 WITH(NOLOCK) on r2.ReasonTypeID= 'Style_Apparel_Type' and r2.ID = s.ApparelType
	where s.ID = a.StyleID 
	and s.SeasonID = a.SeasonID 
	and s.BrandID = a.BrandID
)sty
outer apply(
	select value = sum(ArtworkCPU*CPUFactor*Rate) from #stmp t
	where IsTtlTMS = 1
) Sewing
where (IsTtlTMS = 0 or ArtworkTypeID ='Sewing')
group by  a.ArtworkTypeID, a.RS, a.ProgramID, a.StyleID, a.SeasonID, a.BrandID , a.FactoryID, a.POID , a.Category, a.CdCodeID ,a.BuyerDelivery, a.SCIDelivery, a.SewingLineID 
, a.StyleDesc,a.ComboType,a.ModularParent, IsTtlTMS,a.OutputDate, Category, Shift, SewingLineID, Team, orderid, ComboType, SCategory, FactoryID, ProgramID,artworkcpu, CPU, CPUFactor, StyleID, Rate,FtyZone
, sty.CDCodeNew, sty.ProductType, sty.FabricType, sty.Lining, sty.Gender, sty.Construction
 

select StyleID,BrandID,StyleDesc,SeasonID,FactoryID,OutputDate = max(OutputDate)
into #tmp_MaxOutputDate
from #tmpz 
group by StyleID,BrandID,StyleDesc,SeasonID,FactoryID

select 
OutputDate = EOMONTH(OutputDate),
IsTtlTMS,
[ArtworkType] = 'TTL '+　ArtworkTypeID　+' ('+RS+')', 
ProgramID
    , StyleID
    , FtyZone
    , FactoryID
    , BrandID
    , CDCodeNew
    , ProductType
    , FabricType
    , Lining
    , Gender
    , Construction
    , StyleDesc
    , SeasonID
    , [Total Qty]=sum(QARate)
	, [Total Artwork CPU] = sum(TotalArtwrokCPUOut)
	, [Total CPU]=sum(TotalCPUOut)
    , [Total ManHours]=iif(ArtworkTypeID ='SEWING',CONVERT(Varchar , sum(TotalManHour)),'-')
    , [PPH]=iif(ArtworkTypeID ='SEWING',CONVERT(Varchar,Round((Sum(TotalCPUOut) / case when Sum(TotalManHour) is null or Sum(TotalManHour) = 0 then 1 else Sum(TotalManHour) end) ,2)),'-')
    , [EFF%]=iif(ArtworkTypeID ='SEWING',CONVERT(Varchar,Round((Sum(TotalCPUOut) / (case when Sum(TotalManHour) is null or Sum(TotalManHour) = 0 then 1 else Sum(TotalManHour) end * 3600 / 1400) * 100),2)),'-') 
    , [Remark]= iif(ArtworkTypeID ='SEWING',CONVERT(Varchar,case  when Max(OutputDate) is null then 'New Style'
            else concat((Stuff((
							select distinct concat(' ', t.SewingLineID)
							from #tmpz t
							where t.StyleID = #tmpz.StyleID
							and t.BrandID = #tmpz.BrandID
							and t.StyleDesc = #tmpz.StyleDesc
							and t.SeasonID = #tmpz.SeasonID
							and t.FactoryID = #tmpz.FactoryID
							and exists (select 1 from #tmp_MaxOutputDate t2
										where t2.StyleID = t.StyleID
										and t2.BrandID = t.BrandID 
										and t2.StyleDesc = t.StyleDesc
										and t2.SeasonID = t.SeasonID
										and t2.OutputDate = t.OutputDate
										and t2.FactoryID = t.FactoryID)
							FOR XML PATH('')) ,1,1,'')),'(',format(Max(OutputDate), 'yyyy/MM/dd'),')')
            end),'-')
into #tmpMain
from #tmpz 
Group BY OutputDate,ArtworkTypeID,IsTtlTMS, RS, ProgramID,StyleID,FtyZone,FactoryID,BrandID,CdCodeID,StyleDesc,SeasonID, CDCodeNew, ProductType, FabricType, Lining, Gender, Construction 
order by ProgramID,StyleID,FtyZone,FactoryID,BrandID,CdCodeID




delete t
from P_ProdEffAnalysis t
where  [Month] between @SDate and @EDate

insert into P_ProdEffAnalysis(
[Month]
      ,[ArtworkType]
      ,[Program]
      ,[Style]
      ,[FtyZone]
      ,[Factory]
      ,[Brand]
      ,[NewCDCode]
      ,[ProductType]
      ,[FabricType]
      ,[Lining]
      ,[Gender]
      ,[Construction]
      ,[StyleDescription]
      ,[Season]
      ,[TotalQty]
      ,[TotalCPU]
      ,[TotalManHours]
      ,[PPH]
      ,[EFF]
      ,[Remark]
)
select OutputDate
      ,[ArtworkType]
	  ,ProgramID
	  ,StyleID
	  ,FtyZone
	  ,FactoryID
	  ,BrandID
	  ,CDCodeNew
	  ,ProductType
	  ,FabricType
	  ,Lining
	  ,Gender
	  ,Construction
	  ,StyleDesc
	  ,SeasonID
	  ,[Total Qty]
	  ,[Total Artwork CPU]
	  ,[Total ManHours]
	  ,[PPH]
	  ,[EFF%]
	  ,[Remark]
from #tmpMain t

if exists(select 1 from BITableInfo where Id = 'P_ProdEffAnalysis')
begin
	update BITableInfo set TransferDate = getdate()
	where Id = 'P_ProdEffAnalysis'
end
else
begin
	insert into BITableInfo(Id, TransferDate, IS_Trans) values('P_ProdEffAnalysis', GETDATE(), 1)
end

drop table #tmpMain
drop table #tmp_MaxOutputDate,#stmp,#tmpz


end