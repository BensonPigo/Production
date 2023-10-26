CREATE PROCEDURE [dbo].[P_Import_PPIC_R01_SewingLineScheduleBySP]
	@StartDate date = null,
	@EndDate date = null,
	@DateType varchar(30) = ''
AS
begin
Declare @SewingDateFromString varchar(8) = Format(@StartDate, 'yyyyMMdd')
Declare @SewingDateToString varchar(8) = Format(@EndDate, 'yyyyMMdd')

Declare @DateParameterFrom varchar(30) = '@SewingDateFrom'
Declare @DateParameterTo varchar(30) = '@SewingDateTo'

if(@DateType = 'SciDelivery')
begin
	set @DateParameterFrom = '@SciDeliveryFrom'
	set @DateParameterTo = '@SciDeliveryTo'
end

select * into #tmpP_SewingLineScheduleBySP
from P_SewingLineScheduleBySP
where 1 = 0

DECLARE @DynamicSQL NVARCHAR(MAX) = '
insert into #tmpP_SewingLineScheduleBySP(
ID
,SewingLineID
,MDivisionID
,FactoryID
,SPNo
,CustPONo
,Category
,ComboType
,SwitchToWorkorder
,Colorway
,SeasonID
,CDCodeNew
,ProductType
,MatchFabric
,FabricType
,Lining
,Gender
,Construction
,StyleID
,OrderQty
,AlloQty
,CutQty
,SewingQty
,ClogQty
,FirstCuttingOutputDate
,InspectionDate
,TotalStandardOutput
,WorkHour
,StandardOutputPerHour
,Efficiency
,KPILETA
,PFRemark
,ActMTLETA
,MTLExport
,CutInLine
,Inline
,Offline
,SCIDelivery
,BuyerDelivery
,CRDDate
,CPU
,SewingCPU
,VASSHAS
,ShipModeList
,Destination
,Artwork
,Remarks
,TTL_PRINTING_PCS
,TTL_PRINTING_PPU_PPU
,SubCon
)
select 
 ID								
,SewingLineID
,MDivisionID
,FactoryID
,SPNo
,CustPONo
,Category
,ComboType
,SwitchToWorkorder
,Colorway
,SeasonID
,CDCodeNew
,ProductType
,MatchFabric
,FabricType
,Lining
,Gender
,Construction
,StyleID
,OrderQty
,AlloQty
,CutQty
,SewingQty
,ClogQty
,FirstCuttingOutputDate
,InspectionDate
,TotalStandardOutput
,WorkHour
,StandardOutputPerHour
,Efficiency
,KPILETA
,PFRemark
,ActMTLETA
,MTLExport
,CutInLine
,Inline
,Offline
,SCIDelivery
,BuyerDelivery
,CRDDate
,CPU
,SewingCPU
,VASSHAS
,ShipModeList
,Destination
,Artwork
,Remarks
,TTL_PRINTING_PCS
,TTL_PRINTING_PPU_PPU
,SubCon
from OPENQUERY([MainServer], '' SET NOCOUNT ON; exec Production.dbo.PPIC_R01_SewingLineScheduleBySP '+@DateParameterFrom+' = '''''+ @SewingDateFromString +''''', '+@DateParameterTo+' = '''''+ @SewingDateToString +''''''')

'

EXEC sp_executesql @DynamicSQL

-- 更新 P_IssueFabricByCuttingTransactionList
if(@DateType = 'SciDelivery')
begin
	delete p
	from P_SewingLineScheduleBySP p
	where	p.SciDelivery >= @StartDate  and p.SciDelivery <= @EndDate and
			not exists(select 1 from #tmpP_SewingLineScheduleBySP t 
												where	p.ID = t.ID)
end
else
begin
	delete p
	from P_SewingLineScheduleBySP p
	where	(convert(date, p.Inline) >= @StartDate or (@StartDate between convert(date,p.Inline) and convert(date,p.Offline))) and
		    (convert(date, p.Offline) <= @EndDate or (@EndDate between convert(date,p.Inline) and convert(date,p.Offline))) and
			not exists(select 1 from #tmpP_SewingLineScheduleBySP t 
												where	p.ID = t.ID)	
end


update p set p.SewingLineID				= t.SewingLineID
			,p.MDivisionID				= t.MDivisionID
			,p.FactoryID				= t.FactoryID
			,p.SPNo						= t.SPNo
			,p.CustPONo					= t.CustPONo
			,p.Category					= t.Category
			,p.ComboType				= t.ComboType
			,p.SwitchToWorkorder		= t.SwitchToWorkorder
			,p.Colorway					= t.Colorway
			,p.SeasonID					= t.SeasonID
			,p.CDCodeNew				= t.CDCodeNew
			,p.ProductType				= t.ProductType
			,p.MatchFabric				= t.MatchFabric
			,p.FabricType				= t.FabricType
			,p.Lining					= t.Lining
			,p.Gender					= t.Gender
			,p.Construction				= t.Construction
			,p.StyleID					= t.StyleID
			,p.OrderQty					= t.OrderQty
			,p.AlloQty					= t.AlloQty
			,p.CutQty					= t.CutQty
			,p.SewingQty				= t.SewingQty
			,p.ClogQty					= t.ClogQty
			,p.FirstCuttingOutputDate	= t.FirstCuttingOutputDate
			,p.InspectionDate			= t.InspectionDate
			,p.TotalStandardOutput		= t.TotalStandardOutput
			,p.WorkHour					= t.WorkHour
			,p.StandardOutputPerHour	= t.StandardOutputPerHour
			,p.Efficiency				= t.Efficiency
			,p.KPILETA					= t.KPILETA
			,p.PFRemark					= t.PFRemark
			,p.ActMTLETA				= t.ActMTLETA
			,p.MTLExport				= t.MTLExport
			,p.CutInLine				= t.CutInLine
			,p.Inline					= t.Inline
			,p.Offline					= t.Offline
			,p.SCIDelivery				= t.SCIDelivery
			,p.BuyerDelivery			= t.BuyerDelivery
			,p.CRDDate					= t.CRDDate
			,p.CPU						= t.CPU
			,p.SewingCPU				= t.SewingCPU
			,p.VASSHAS					= t.VASSHAS
			,p.ShipModeList				= t.ShipModeList
			,p.Destination				= t.Destination
			,p.Artwork					= t.Artwork
			,p.Remarks					= t.Remarks
			,p.TTL_PRINTING_PCS			= t.TTL_PRINTING_PCS
			,p.TTL_PRINTING_PPU_PPU		= t.TTL_PRINTING_PPU_PPU
			,p.SubCon					= t.SubCon					
from P_SewingLineScheduleBySP p
inner join #tmpP_SewingLineScheduleBySP t on p.ID = t.ID

insert into P_SewingLineScheduleBySP(
		ID
		,SewingLineID
		,MDivisionID
		,FactoryID
		,SPNo
		,CustPONo
		,Category
		,ComboType
		,SwitchToWorkorder
		,Colorway
		,SeasonID
		,CDCodeNew
		,ProductType
		,MatchFabric
		,FabricType
		,Lining
		,Gender
		,Construction
		,StyleID
		,OrderQty
		,AlloQty
		,CutQty
		,SewingQty
		,ClogQty
		,FirstCuttingOutputDate
		,InspectionDate
		,TotalStandardOutput
		,WorkHour
		,StandardOutputPerHour
		,Efficiency
		,KPILETA
		,PFRemark
		,ActMTLETA
		,MTLExport
		,CutInLine
		,Inline
		,Offline
		,SCIDelivery
		,BuyerDelivery
		,CRDDate
		,CPU
		,SewingCPU
		,VASSHAS
		,ShipModeList
		,Destination
		,Artwork
		,Remarks
		,TTL_PRINTING_PCS
		,TTL_PRINTING_PPU_PPU
		,SubCon)
select	 t.ID
		,t.SewingLineID
		,t.MDivisionID
		,t.FactoryID
		,t.SPNo
		,t.CustPONo
		,t.Category
		,t.ComboType
		,t.SwitchToWorkorder
		,t.Colorway
		,t.SeasonID
		,t.CDCodeNew
		,t.ProductType
		,t.MatchFabric
		,t.FabricType
		,t.Lining
		,t.Gender
		,t.Construction
		,t.StyleID
		,t.OrderQty
		,t.AlloQty
		,t.CutQty
		,t.SewingQty
		,t.ClogQty
		,t.FirstCuttingOutputDate
		,t.InspectionDate
		,t.TotalStandardOutput
		,t.WorkHour
		,t.StandardOutputPerHour
		,t.Efficiency
		,t.KPILETA
		,t.PFRemark
		,t.ActMTLETA
		,t.MTLExport
		,t.CutInLine
		,t.Inline
		,t.Offline
		,t.SCIDelivery
		,t.BuyerDelivery
		,t.CRDDate
		,t.CPU
		,t.SewingCPU
		,t.VASSHAS
		,t.ShipModeList
		,t.Destination
		,t.Artwork
		,t.Remarks
		,t.TTL_PRINTING_PCS
		,t.TTL_PRINTING_PPU_PPU
		,t.SubCon
from #tmpP_SewingLineScheduleBySP t
where not exists(	select 1 
					from P_SewingLineScheduleBySP p
					where	p.ID = t.ID)

if exists(select 1 from BITableInfo where Id = 'P_SewingLineScheduleBySP')
begin
	update BITableInfo set TransferDate = getdate()
	where Id = 'P_SewingLineScheduleBySP'
end
else
begin
	insert into BITableInfo(Id, TransferDate, IS_Trans) values('P_SewingLineScheduleBySP', GETDATE(), 0)
end

end
go