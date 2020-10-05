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
;
DECLARE @TPEtmp TABLE
(
	ID varchar(13)
)
;

insert into @TPEtmp
select ps.ID
from PO_Supp ps
inner join PO_Supp_Detail psd on ps.ID=psd.id and ps.SEQ1=psd.Seq1
inner join Fabric fb on psd.SCIRefno = fb.SCIRefno 
inner join MtlType ml on ml.id = fb.MtlTypeID
where 1=1 and ml.Junk =0 and psd.Junk=0 and fb.Junk =0
and ml.isThread=1 
and ps.SuppID <> 'FTY' and ps.Seq1 not Like '5%'
;

insert into @CMPDetail
select   [OutputDate] = s.OutputDate
		,[SPNo] = sd.OrderId
		,[Factory] = s.FactoryID
		,[FactoryType] = iif(f.Type='B','Bulk',iif(f.Type='S','Sample',f.Type))
		,[IsDevSample] = CASE WHEN otype.IsDevSample =1 THEN 'Y' ELSE 'N' END
		,[MDivision] = s.MDivisionID
		,[SCISeason] = sea.SeasonSCIID
		,[ArtworkType] = iif(s.Category = 'M','SEWING',att.ID)
		,[SubconType] = case	when s.Shift = 'O' then 'O'
								when o.SubconInSisterFty = 1 then 'I'
								else 'N' end
		,[SubconInOut] = case	when s.Shift = 'O' then isnull(s.SubconOutFty,'')
							when o.SubconInSisterFty = 1 then isnull(o.ProgramID,'')
							else '' end
		,[ContractNumber] = s.SubConOutContractNumber
		,[UnitPrice] = sod.UnitPrice
		,[Vat] = sod.Vat
		,[CPUPrice] = case when s.Category = 'M'  then Sum(sd.QAQty * mo.CPUFactor * mo.CPU)
							when att.ID = 'SEWING' then Sum(sd.QAQty * o.CPUFactor * O.CPU  * a.Rate)
							when att.ProductionUnit = 'QTY'  then ROUND(Sum(sd.QAQty * ot.Price * a.Rate) , 4)
							when att.ProductionUnit = 'TMS'  then ROUND(Sum(sd.QAQty * ot.Price * a.Rate) , 3)
							else 0 end
		,[Qty] = Sum(sd.QAQty)
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
left join ArtworkType att WITH (NOLOCK) on	att.ID =	ot.ArtworkTypeID and 
											att.Classify in ('I','A','P') and 
											-- Sewing need include data
											(att.IsTtlTMS = 0 or att.Seq = 1010 ) and 
											att.Junk = 0 and 
											att.IsPrintToCMP= 1
left join MockupOrder mo WITH (NOLOCK) on mo.ID = sd.OrderId
outer apply (select [Rate] = isnull([dbo].[GetOrderLocation_Rate](o.id ,sd.ComboType),100)/100) a
outer apply (select [Value] = IIF(s.Shift <> 'O' and o.Category <> 'M' and o.LocalOrder = 1, 'I',s.Shift)) as LastShift
where	s.MDivisionID = isnull(@M, s.MDivisionID) and 
		(exists(select 1 from SplitString(@Factory,',') where s.FactoryID = Data )  or @Factory is null) and
		s.OutputDate between @StartDate and @EndDate and (o.CateGory != 'G' or s.Category='M') and
		((LastShift.Value = 'O' and o.LocalOrder <> 1) or (LastShift.Value <> 'O') ) 
          --排除 subcon in non sister的數值
        and ((LastShift.Value <> 'I') or ( LastShift.Value = 'I' and o.SubconInSisterFty <> 0 ))   
		--將ArtworkType為'SP_THREAD'部分，排除掉是台北買線的部分。
		AND (  (NOT EXISTS (SELECT 1 FROm @TPEtmp WHERE ID = o.POID )AND att.ID = 'SP_THREAD')   OR   isnull(att.ID,'') <> 'SP_THREAD' ) 
		AND o.NonRevenue = 0

group by  s.OutputDate
		, sd.OrderId
		, s.FactoryID
		, iif(f.Type='B','Bulk',iif(f.Type='S','Sample',f.Type))
		, CASE WHEN otype.IsDevSample =1 THEN 'Y' ELSE 'N' END
		, s.MDivisionID
		, sea.SeasonSCIID
		, iif(s.Category = 'M','SEWING',att.ID)
		, case	when s.Shift = 'O' then 'O'
								when o.SubconInSisterFty = 1 then 'I'
								else 'N' end
		,case	when s.Shift = 'O' then isnull(s.SubconOutFty,'')
							when o.SubconInSisterFty = 1 then isnull(o.ProgramID,'')
							else '' end
		, s.SubConOutContractNumber
		, sod.UnitPrice
		, sod.Vat
		, att.ProductionUnit
		, s.Category
		, att.ID
order by 
		s.OutputDate
		, sd.OrderId
		, s.FactoryID
		, s.MDivisionID
		, sea.SeasonSCIID
		, iif(s.Category = 'M','SEWING',att.ID)

	delete @CMPDetail where ArtworkType is null

	return
end

go






