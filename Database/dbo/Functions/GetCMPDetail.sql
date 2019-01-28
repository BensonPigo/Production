-- =============================================
-- Author:		Aaron
-- Create date: 2018/12/21
-- Description:	拋轉FMS team所需要的sewing產出 ISP20181241
-- =============================================
CREATE function [dbo].[GetCMPDetail]
(
	  @M				VarChar(8)				
	 ,@Factory		varchar(80) 
	 ,@StartDate    date
	 ,@EndDate      date	
)
RETURNS @CMPDetail TABLE
(
	OutputDate date,
	SPNo varchar(13),
	Factory varchar(8),
	FactoryType varchar(6),
	IsDevSample varchar(1),
	MDivision varchar(8),
	SCISeason varchar(10),
	ArtworkType varchar(20),
	SubconType varchar(1),
	SubconInOut varchar(12),
	ContractNumber varchar(50),
	UnitPrice	numeric(16,4),
	Vat	numeric(11,2),
	CPUPrice numeric(17,4),
	Qty int
)
AS
Begin

if(@M = '')
Begin
	set @M = null
end

if(@Factory = '')
Begin
	set @Factory = null
end

if(	@M is null and @Factory is null or
	@StartDate is null or
	@EndDate is null )
Begin
	return
end


insert into @CMPDetail
select  s.OutputDate
		, sd.OrderId
		, s.FactoryID
		, iif(f.Type='B','Bulk',iif(f.Type='S','Sample',f.Type))
		, CASE WHEN otype.IsDevSample =1 THEN 'Y' ELSE 'N' END
		, s.MDivisionID
		, sea.SeasonSCIID
		, att.ID
		, [SubconType] = case	when s.Shift = 'O' then 'O'
								when o.SubconInSisterFty = 1 then 'I'
								else 'N' end
		, [SubconInOut] = case	when s.Shift = 'O' then isnull(s.SubconOutFty,'')
							when o.SubconInSisterFty = 1 then isnull(o.ProgramID,'')
							else '' end
		, s.SubConOutContractNumber
		, sod.UnitPrice
		, sod.Vat
		, [CPUPrice] = case when att.ProductionUnit = 'QTY' then ROUND(Sum(sd.QAQty * ot.Price * a.Rate) * iif(s.Category = 'M', mo.CPUFactor, o.CPUFactor), 4)
							when att.ProductionUnit = 'TMS' then ROUND(Sum(sd.QAQty * ot.Price * a.Rate) * iif(s.Category = 'M', mo.CPUFactor, o.CPUFactor), 3)
							else 0 end
		, [Qty] = Sum(sd.QAQty)
from SewingOutput s WITH (NOLOCK) 
inner join SewingOutput_Detail sd WITH (NOLOCK) on sd.ID = s.ID
left join Orders o WITH (NOLOCK) on o.ID = sd.OrderId 
left join OrderType otype WITH (NOLOCK) on o.OrderTypeID = otype.ID and o.BrandID = otype.BrandID
left join Factory f on s.FactoryID = f.ID
left join Season sea WITH (NOLOCK) on sea.ID = o.SeasonID and sea.BrandID = o.BrandID
left join SubconOutContract_Detail sod WITH (NOLOCK) on sod.SubConOutFty = s.SubconOutFty and 
														sod.ContractNumber = s.SubConOutContractNumber and
														sod.OrderId = sd.OrderId and
														sod.ComboType = sd.ComboType and 
														sod.Article = sd.Article
left join Order_TmsCost ot WITH (NOLOCK) on ot.ID = o.ID
inner join ArtworkType att WITH (NOLOCK) on	att.ID =	ot.ArtworkTypeID and 
											att.Classify in ('I','A','P') and 
											-- Sewing need include data
											(att.IsTtlTMS = 0 or att.Seq = 1010 ) and 
											att.Junk = 0 and 
											att.IsPrintToCMP= 1
left join MockupOrder mo WITH (NOLOCK) on mo.ID = sd.OrderId
outer apply (select [Rate] = isnull([dbo].[GetOrderLocation_Rate](o.id ,sd.ComboType),100)/100) a
where	s.MDivisionID = isnull(@M, s.MDivisionID) and 
		(exists(select 1 from SplitString(@Factory,',') where s.FactoryID = Data )  or @Factory is null) and
		s.OutputDate between @StartDate and @EndDate and (o.CateGory != 'G' or s.Category='M')
group by  s.OutputDate
		, sd.OrderId
		, s.FactoryID
		, iif(f.Type='B','Bulk',iif(f.Type='S','Sample',f.Type))
		, CASE WHEN otype.IsDevSample =1 THEN 'Y' ELSE 'N' END
		, s.MDivisionID
		, sea.SeasonSCIID
		, att.ID
		, case	when s.Shift = 'O' then 'O'
								when o.SubconInSisterFty = 1 then 'I'
								else 'N' end
		,case	when s.Shift = 'O' then isnull(s.SubconOutFty,'')
							when o.SubconInSisterFty = 1 then isnull(o.ProgramID,'')
							else '' end
		, s.SubConOutContractNumber
		, sod.UnitPrice
		, sod.Vat
		,att.ProductionUnit
		,mo.CPUFactor
		,o.CPUFactor
		,s.Category
order by 
		s.OutputDate
		, sd.OrderId
		, s.FactoryID
		, s.MDivisionID
		, sea.SeasonSCIID
		, att.ID
	return
end

go






