-- =============================================
-- Create date: 2020/08/06
-- Description:	Data Query Logic by PMS.Centralized R05 Report Sheet [Balance_Detail], Import Data to P_SDPOrderDetail
-- =============================================
CREATE PROCEDURE [dbo].[P_ImportLoadingProductionOutput_FTY]

AS
BEGIN
	SET NOCOUNT ON

	-- Get current server name
	declare @current_ServerName varchar(50) = (SELECT [Server Name] = @@SERVERNAME)	
	-- use current server name to take Production Server name
	declare @current_PMS_ServerName nvarchar(50) 
	= (
		select [value] = 
			CASE WHEN @current_ServerName= 'PHL-NEWPMS-02' THEN 'PHL-NEWPMS' -- PH1
				 WHEN @current_ServerName= 'VT1-PH2-PMS2b' THEN 'VT1-PH2-PMS2' -- PH2
				 WHEN @current_ServerName= 'system2017BK' THEN 'SYSTEM2017' -- SNP
				 WHEN @current_ServerName= 'SPS-SQL2' THEN 'SPS-SQL.spscd.com' -- SPS
				 WHEN @current_ServerName= 'SQLBK' THEN 'PMS-SXR' -- SPR
				 WHEN @current_ServerName= 'newerp-bak' THEN 'newerp' -- HZG		
				 WHEN @current_ServerName= 'SQL' THEN 'NDATA' -- HXG
				 when (select top 1 MDivisionID from Production.dbo.Factory) in ('VM2','VM1') then 'SYSTEM2016' -- ESP & SPT
			ELSE '' END
	)

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

	-- 非外代工
	select * from #tmp 

	union all

	-- 外代工
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
		T.ProductionFamilyID,
		T.FtyGroup,
		[PulloutComplete],
		T.SewInLine,
		T.SewOffLine,
		T.TransFtyZone 
	from #tmp T
	LEFT JOIN ['+@current_PMS_ServerName+'].Production.dbo.Factory f WITH(NOLOCK) ON f.ID= T.TransFtyZone
	where TransFtyZone != ''''

) a


drop table #tmp
'

SET @SqlCmd2 = '

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
		t.TransFtyZone =  s.TransFtyZone
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
	s.TransFtyZone
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
'

SET @SqlCmd_Combin = @SqlCmd1 + @SqlCmd2 + @SqlCmd3 
	EXEC sp_executesql @SqlCmd_Combin

End

GO


