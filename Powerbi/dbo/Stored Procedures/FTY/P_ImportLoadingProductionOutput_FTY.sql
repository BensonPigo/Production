-- =============================================
-- Create date: 2020/08/06
-- Description:	Data Query Logic by PMS.Centralized R05 Report Sheet [Balance_Detail], Import Data to P_SDPOrderDetail
-- =============================================
CREATE PROCEDURE [dbo].[P_ImportLoadingProductionOutput_FTY]

AS
BEGIN
	SET NOCOUNT ON

	declare @current_PMS_ServerName nvarchar(50) = 'MainServer'

declare @SqlCmd_Combin nvarchar(max) =''
declare @SqlCmd1 nvarchar(max) ='';
declare @SqlCmd2 nvarchar(max) ='';
declare @SqlCmd3 nvarchar(max) ='';
declare @strID nvarchar(15) = N'SubCON-Out_'


declare @useYear varchar(4) = (select YEAR(GETDATE()))
declare @curr_Month varchar(2) = (select MONTH(GETDATE()))

if( @curr_Month = '1' or @curr_Month = '01')
begin
	select @useYear = YEAR(DATEADD(YEAR,-1,GETDATE()))	
end

declare @S_Year as varchar(10) = CAST(@useYear AS varchar) 

SET @SqlCmd1 = '

SELECT * into #tmp FROM OPENQUERY(['+@current_PMS_ServerName+'], 
''exec Production.dbo.GetProductionOutputSummary @Year = '''''+@useYear+'''''
,@DateType=1, @ChkOrder=1, @ChkForecast=1, @ChkFtylocalOrder=1, @ExcludeSampleFactory=1, @ChkMonthly=1, @IncludeCancelOrder=1, @IsFtySide=0, @IsPowerBI=1
'')

select * into #Final 
from (

	-- �D�~�N�u
	select
		t.[MDivisionID],
		t.[FtyZone],
		t.[FactoryID],
		T.BuyerDelivery,
		T.SciDelivery,
		T.SCIKey,
		T.SCIKeyHalf,
		T.BuyerKey,
		T.BuyerKeyHalf,
		t.[ID],
		T.Category ,
		T.Cancelled,
		T.IsCancelNeedProduction,
		t.[Buyback],
		T.PartialShipment,
		T.LastBuyerDelivery,
		T.StyleID,
		T.SeasonID,
		T.CustPONO,
		T.BrandID,
		T.CPU,
		T.Qty,
		T.FOCQty,
		T.PulloutQty,
		T.OrderShortageCPU,
		t.[TotalCPU],
		t.[SewingOutput],
		t.[SewingOutputCPU],
		t.[BalanceQty],
		t.[BalanceCPU],
		t.[BalanceCPUIrregular],
		T.SewLine,
		T.Dest,
		T.OrderTypeID,
		T.ProgramID,
		T.CdCodeID,
		ProductionFamilyID = '''',
		T.FtyGroup,
		t.[PulloutComplete],
		T.SewInLine,
		T.SewOffLine,
		T.TransFtyZone 
		,[CDCodeNew] = sty.CDCodeNew
		,[ProductType] = sty.ProductType
		,[FabricType] = sty.FabricType
		,[Lining] = sty.Lining
		,[Gender] = sty.Gender
		,[Construction] = sty.Construction
		,t.FMSister
		,t.SampleGroup
		,t.OrderReason
	from #tmp t
	Outer apply (
		SELECT s.CDCodeNew
			, s.[ID]
			, ProductType = r2.Name
			, FabricType = r1.Name
			, Lining
			, Gender
			, Construction = d1.Name
		FROM ['+@current_PMS_ServerName+'].Production.dbo.Orders o WITH(NOLOCK)
		left join ['+@current_PMS_ServerName+'].Production.dbo.Style s WITH(NOLOCK) on s.Ukey = o.StyleUkey
		left join ['+@current_PMS_ServerName+'].Production.dbo.DropDownList d1 WITH(NOLOCK) on d1.type= ''StyleConstruction'' and d1.ID = s.Construction
		left join ['+@current_PMS_ServerName+'].Production.dbo.Reason r1 WITH(NOLOCK) on r1.ReasonTypeID= ''Fabric_Kind'' and r1.ID = s.FabricType
		left join ['+@current_PMS_ServerName+'].Production.dbo.Reason r2 WITH(NOLOCK) on r2.ReasonTypeID= ''Style_Apparel_Type'' and r2.ID = s.ApparelType
		where o.ID = t.ID
	)sty
	where exists(select 1 from ['+@current_PMS_ServerName+'].Production.dbo.Factory f WITH(NOLOCK) where f.ID = t.FactoryID and f.IsProduceFty = 1)
	'
SET @SqlCmd2 = '
	union all

	-- �~�N�u
	select [MDivisionID] = F.MDivisionID,
		[FtyZone] = F.FtyZone,
		[FactoryID] = F.ID,
		T.BuyerDelivery,
		T.SciDelivery,
		T.SCIKey,
		T.SCIKeyHalf,
		T.BuyerKey,
		T.BuyerKeyHalf,
		[ID] =  CONVERT(varchar(24), '''+@strID+''' + T.ID),
		T.Category ,
		T.Cancelled,
		T.IsCancelNeedProduction,
		[Buyback],
		T.PartialShipment,
		T.LastBuyerDelivery,
		T.StyleID,
		T.SeasonID,
		T.CustPONO,
		T.BrandID,
		T.CPU,
		T.Qty,
		T.FOCQty,
		T.PulloutQty,
		T.OrderShortageCPU,
		[TotalCPU] = -TotalCPU,
		[SewingOutput] = 0,
		[SewingOutputCPU] = 0,
		[BalanceQty] = 0,
		[BalanceCPU] = 0,
		[BalanceCPUIrregular] = 0,
		T.SewLine,
		T.Dest,
		T.OrderTypeID,
		T.ProgramID,
		T.CdCodeID,
		ProductionFamilyID = '''',
		T.FtyGroup,
		[PulloutComplete],
		T.SewInLine,
		T.SewOffLine,
		T.TransFtyZone 
		,[CDCodeNew] = sty.CDCodeNew
		,[ProductType] = sty.ProductType
		,[FabricType] = sty.FabricType
		,[Lining] = sty.Lining
		,[Gender] = sty.Gender
		,[Construction] = sty.Construction
		,t.FMSister
		,t.SampleGroup
		,t.OrderReason
	from #tmp T
	LEFT JOIN ['+@current_PMS_ServerName+'].Production.dbo.SCIFty f WITH(NOLOCK) ON f.ID= T.TransFtyZone
	Outer apply (
		SELECT s.CDCodeNew
			, s.[ID]
			, ProductType = r2.Name
			, FabricType = r1.Name
			, Lining
			, Gender
			, Construction = d1.Name
		FROM ['+@current_PMS_ServerName+'].Production.dbo.Orders o WITH(NOLOCK)
		left join ['+@current_PMS_ServerName+'].Production.dbo.Style s WITH(NOLOCK) on s.Ukey = o.StyleUkey
		left join ['+@current_PMS_ServerName+'].Production.dbo.DropDownList d1 WITH(NOLOCK) on d1.type= ''StyleConstruction'' and d1.ID = s.Construction
		left join ['+@current_PMS_ServerName+'].Production.dbo.Reason r1 WITH(NOLOCK) on r1.ReasonTypeID= ''Fabric_Kind'' and r1.ID = s.FabricType
		left join ['+@current_PMS_ServerName+'].Production.dbo.Reason r2 WITH(NOLOCK) on r2.ReasonTypeID= ''Style_Apparel_Type'' and r2.ID = s.ApparelType
		where o.ID = T.ID
	)sty
	where TransFtyZone != ''''

) a


drop table #tmp
'

SET @SqlCmd3 = '

update t
set	    t.MDivisionID =  s.MDivisionID,
		t.FtyZone =  s.FtyZone,
		t.FactoryID =  s.FactoryID,
		t.BuyerDelivery =  s.BuyerDelivery,
		t.SciDelivery =  s.SciDelivery,
		t.SCIKey =  s.SCIKey,
		t.SCIKeyHalf =  s.SCIKeyHalf,
		t.BuyerKey =  s.BuyerKey,
		t.BuyerKeyHalf =  s.BuyerKeyHalf,
		t.SPNO =  s.ID,
		t.Category =  s.Category,
		t.Cancelled =  s.Cancelled,
		t.IsCancelNeedProduction =  s.IsCancelNeedProduction,
		t.PartialShipment =  s.PartialShipment,
		t.LastBuyerDelivery =  s.LastBuyerDelivery,
		t.StyleID =  s.StyleID,
		t.SeasonID =  s.SeasonID,
		t.CustPONO =  s.CustPONO,
		t.BrandID =  s.BrandID,
		t.CPU =  s.CPU,
		t.Qty =  s.Qty,
		t.FOCQty =  s.FOCQty,
		t.PulloutQty =  s.PulloutQty,
		t.OrderShortageCPU =  s.OrderShortageCPU,
		t.TotalCPU =  s.TotalCPU,
		t.SewingOutput =  s.SewingOutput,
		t.SewingOutputCPU =  s.SewingOutputCPU,
		t.BalanceQty =  s.BalanceQty,
		t.BalanceCPU =  s.BalanceCPU,
		t.BalanceCPUIrregular =  s.BalanceCPUIrregular,
		t.SewLine =  s.SewLine,
		t.Dest =  s.Dest,
		t.OrderTypeID =  s.OrderTypeID,
		t.ProgramID =  s.ProgramID,
		t.CdCodeID =  s.CdCodeID,
		t.ProductionFamilyID =  s.ProductionFamilyID,
		t.FtyGroup =  s.FtyGroup,
		t.PulloutComplete =  s.PulloutComplete,
		t.SewInLine =  s.SewInLine,
		t.SewOffLine =  s.SewOffLine,
		t.TransFtyZone =  s.TransFtyZone,
		t.CDCodeNew =  s.CDCodeNew,
		t.ProductType =  s.ProductType,
		t.FabricType =  s.FabricType,
		t.Lining =  s.Lining,
		t.Gender =  s.Gender,
		t.Construction =  s.Construction,
		t.[FM Sister] = s.FMSister,
		t.[Sample Group] = s.SampleGroup,
		t.[Order Reason] = s.OrderReason
from P_LoadingProductionOutput as t
inner join #Final s 
on t.FactoryID=s.FactoryID  
   AND t.SPNO=s.ID 


insert into P_LoadingProductionOutput
	select  s.MDivisionID,
	s.FtyZone,
	s.FactoryID,
	s.BuyerDelivery,
	s.SciDelivery,
	s.SCIKey,
	s.SCIKeyHalf,
	s.BuyerKey,
	s.BuyerKeyHalf,
	s.ID,
	s.Category,
	s.Cancelled,
	s.IsCancelNeedProduction,
	s.PartialShipment,
	s.LastBuyerDelivery,
	s.StyleID,
	s.SeasonID,
	s.CustPONO,
	s.BrandID,
	s.CPU,
	s.Qty,
	s.FOCQty,
	s.PulloutQty,
	s.OrderShortageCPU,
	s.TotalCPU,
	s.SewingOutput,
	s.SewingOutputCPU,
	s.BalanceQty,
	s.BalanceCPU,
	s.BalanceCPUIrregular,
	s.SewLine,
	s.Dest,
	s.OrderTypeID,
	s.ProgramID,
	s.CdCodeID,
	s.ProductionFamilyID,
	s.FtyGroup,
	s.PulloutComplete,
	s.SewInLine,
	s.SewOffLine,
	s.TransFtyZone,
	s.CDCodeNew,
	s.ProductType,
	s.FabricType,
	s.Lining,
	s.Gender,
	s.Construction,
	s.FMSister,
	s.SampleGroup,
	s.OrderReason
from #Final s
where not exists(
	select 1 from P_LoadingProductionOutput t 
	where t.FactoryID=s.FactoryID  
	AND t.SPNO = s.ID
)

delete t
from P_LoadingProductionOutput t WITH (NOLOCK)
where 
(
	YEAR(BuyerDelivery) = '''+@S_Year+'''
	or
	Year(cast(dateadd(day,-7,SciDelivery) as date)) = '''+@S_Year+'''	
)
and exists	   (select 1 from #Final f where t.FactoryID=f.FactoryID AND t.MDivisionID=f.MDivisionID  ) 
and not exists (select 1 from #Final s where t.FactoryID=s.FactoryID AND t.SPNO=s.ID );

delete t
from P_LoadingProductionOutput t
left join [MainServer].Production.dbo.Orders o on t.SPNO = o.ID and t.FactoryID = o.FactoryID
where o.ID is null
and t.Category = ''Forecast''

update b
    set b.TransferDate = getdate()
from BITableInfo b
where b.id = ''P_LoadingProductionOutput''
'

SET @SqlCmd_Combin = @SqlCmd1 + @SqlCmd2 + @SqlCmd3 
	EXEC sp_executesql @SqlCmd_Combin

End

GO


