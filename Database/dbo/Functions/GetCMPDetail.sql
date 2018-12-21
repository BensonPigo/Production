-- =============================================
-- Author:		Aaron
-- Create date: 2018/12/21
-- Description:	拋轉FMS team所需要的sewing產出 ISP20181241
-- =============================================
CREATE function [dbo].[GetCMPDetail]
(
	  @M				VarChar(8)				
	 ,@Factory		varchar(8) 
	 ,@StartDate    date
	 ,@EndDate      date	
)
RETURNS @CMPDetail TABLE
(
	OutputDate date,
	SPNo varchar(13),
	Factory varchar(8),
	MDivision varchar(8),
	SCISeason varchar(10),
	ArtworkType varchar(4),
	SubconType varchar(1),
	SubconInOut varchar(12),
	CPUPrice numeric(17,4)
)
AS
Begin

if(	@M is null or
	@Factory is null or
	@StartDate is null or
	@EndDate is null )
Begin
	return
end


insert into @CMPDetail
select  s.OutputDate
		, sd.OrderId
		, s.FactoryID
		, s.MDivisionID
		, o.SeasonID
		, ot.Seq
		, [SubconType] = case	when s.Shift = 'O' then 'O'
								when o.SubconInSisterFty = 1 then 'I'
								else 'N' end
		, [SubconInOut] = case	when s.Shift = 'O' then s.SubconOutFty
							when o.SubconInSisterFty = 1 then o.ProgramID
							else '' end
		, [CPUPrice] = case when att.ProductionUnit = 'QTY' then ROUND(Sum(sd.QAQty * ot.Price * a.Rate), 4)
							when att.ProductionUnit = 'TMS' then ROUND(Sum(sd.QAQty * ot.Price * a.Rate), 3)
							else 0 end
from SewingOutput s WITH (NOLOCK) 
inner join SewingOutput_Detail sd WITH (NOLOCK) on sd.ID = s.ID
left join Orders o WITH (NOLOCK) on o.ID = sd.OrderId 
left join Order_TmsCost ot WITH (NOLOCK) on ot.ID = o.ID
left join ArtworkType att WITH (NOLOCK) on	att.ID =	ot.ArtworkTypeID and 
											att.Classify in ('I','A','P') and 
											att.IsTtlTMS = 0 and 
											att.Junk = 0 and 
											att.IsPrintToCMP= 1
left join MockupOrder mo WITH (NOLOCK) on mo.ID = sd.OrderId
outer apply (select [Rate] = isnull([dbo].[GetOrderLocation_Rate](o.id ,sd.ComboType),100)/100) a
where s.MDivisionID = @M and s.FactoryID = @Factory and s.OutputDate between @StartDate and @EndDate and (o.CateGory != 'G' or s.Category='M')
group by  s.OutputDate
		, sd.OrderId
		, s.FactoryID
		, s.MDivisionID
		, o.SeasonID
		, ot.Seq
		, case	when s.Shift = 'O' then 'O'
								when o.SubconInSisterFty = 1 then 'I'
								else 'N' end
		,case	when s.Shift = 'O' then s.SubconOutFty
								when o.SubconInSisterFty = 1 then o.ProgramID
								else '' end
		,att.ProductionUnit
	return
end
go






