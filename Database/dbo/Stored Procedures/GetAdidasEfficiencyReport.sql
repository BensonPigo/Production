CREATE PROCEDURE [dbo].[GetAdidasEfficiencyReport]
	@OutPutDateS as date,
	@OutPutDateE as date,
	@M as varchar(8) = '',
	@FactoryID as varchar(8) = '',
	@CDCode as varchar(6) = '',
	@Shift as varchar(1) = '',
	@IsSintexEffReportCompare as bit = 0
AS
BEGIN
	declare @sql as varchar(max) = ''

	if @OutPutDateS = null or @OutPutDateE = null
	begin
		return
	end


if @IsSintexEffReportCompare = 0
Begin
	set @sql = N'
	select s.OutputDate
		, s.FactoryID
		, s.SewingLineID
		, s.Shift
		, [Category] = case o.Category when ''B'' then ''Bulk''
								   when ''S'' then ''Sample''
								   when ''M'' then ''Material''
								   when ''O'' then ''Other'' 
								   when ''G'' then ''Garment''
								   when ''T'' then ''SMLT''
					else o.Category
					end	
		, o.StyleID
		, s.Manpower
		, [ManHour] = sum(sd.WorkHour)
		, [TotalOutput] = sum(sd.QAQty)
		, [CD] = concat(o.CdCodeID, ''-'', sd.ComboType)
		, o.SeasonID
		, o.BrandID
		, [Fabrication] = ''''
		, [ProductGroup] = ''''
		, [ProductFabrication] = ''''
		, [GSD] = iif(SUM(isnull(sd.QAQty, 0)) = 0, 0, SUM(o.CPU * o.CPUFactor * isnull([dbo].[GetOrderLocation_Rate](o.id, sd.ComboType), 100) / 100 * sd.QAQty * 1400) / SUM(sd.QAQty) / 60) * 1.1
		, [Earnedhours] = cast(0 as decimal(10, 2))
		, [TotalWorkingHours] = ''''
		, [CumulateDaysofDaysinProduction] = (select cumulate from dbo.getSewingOutputCumulateOfDays(o.StyleID, s.SewingLineID, s.OutputDate, s.FactoryID))	
		, [EfficiencyLine] = ''''
		, [GSDProsmv] = iif(SUM(isnull(sd.QAQty, 0)) = 0, 0, SUM(o.CPU * o.CPUFactor * isnull([dbo].[GetOrderLocation_Rate](o.id, sd.ComboType), 100) / 100 * sd.QAQty * 1400) / SUM(sd.QAQty) / 60)
		, [Earnedhours2] = cast(0 as decimal(10, 2))
		, [EfficiencyLine2] = ''''
		, [NoofInlineDefects] = sum(isnull(InlineInspection.RejectWIP, 0))
		, [NoofEndlineDefectiveGarments] = sum(isnull(Inspection.cnt, 0))
		, [WFT] = ''''
		-- , [rid] = ROW_NUMBER() over(order by s.OutputDate, s.FactoryID, s.SewingLineID, s.Shift, o.StyleID, o.SeasonID, o.BrandID) + 1
		, [Country] = case f.CountryID when ''PH'' then ''Philippines''
					   when ''VN'' then ''Vietnam''
					   when ''KH'' then ''Cambodia''
					   when ''CN'' then ''China''
				else f.CountryID end
		, [Month] = Left(DATENAME(m, s.OutputDate),3)
		, [IsGSDPro] = ''V''
		, [Orderseq] = case f.CountryID when ''PH'' then 1
					   when ''VN'' then 3
					   when ''KH'' then 2
					   when ''CN'' then 4
					else 5 end
	from [Production].[dbo].SewingOutput s with (nolock)
	Inner join [Production].[dbo].SewingOutput_Detail sd with (nolock) on s.ID = sd.ID
	Inner Join [Production].[dbo].Orders o with (nolock) on sd.OrderId = o.ID
	Inner join [Production].[dbo].Factory f with (nolock) on o.FactoryID=f.ID
	Outer apply (
		select [RejectWIP] = Sum(i.RejectWIP)
		from SciMES_InlineInspection i with (nolock)
		where  s.OutputDate = i.InspectionDate 
		and sd.OrderId = i.OrderID 
		and s.SewingLineID = i.Line 
		and s.Shift = iif(i.Shift = ''Day'', ''D'', ''N'')
		and s.FactoryID = i.FactoryID 
		and sd.ComboType = i.Location 
		and s.Team = i.Team
	)InlineInspection
	Outer apply (
		select cnt = Count(*)
		from SciMES_Inspection i with (nolock) 
		where s.OutputDate = i.InspectionDate 
		and sd.OrderId = i.OrderID 
		and sd.Article = i.Article
		and s.SewingLineID = i.Line 
		and s.FactoryID = i.FactoryID 
		and i.Status in (''Fixed'',''Dispose'',''Reject'')
		and sd.ComboType = i.Location 
		and s.Shift = iif(i.Shift = ''Day'', ''D'', ''N'')
		and s.Team = i.Team 
	)Inspection
	Where  s.Outputdate between ''' + FORMAT(@OutPutDateS, 'yyyyMMdd') + ''' and  ''' + FORMAT(@OutPutDateE, 'yyyyMMdd') + '''
	and o.BrandID in (''Adidas'', ''Reebok'') 
	and o.category in (''B'', ''S'') 
	and f.IsSampleRoom = 0
	and not (sd.WorkHour = 0 and sd.QAQty = 0)
	and s.Shift <> ''O''
	and o.LocalOrder = 0
'

	if @M <> ''
	Begin
		set @sql = @sql + '
	and s.MDivisionID  = ''' + @M + '''' 
	End

	if @FactoryID <> ''
	Begin
		set @sql = @sql + '
	and s.FactoryID = ''' + @FactoryID + '''' 
	End
	
	if @CDCode <> ''
	Begin
		set @sql = @sql + '
	and o.CDCodeID = ''' + @CDCode + '''' 
	End
	
	if @Shift <> ''
	Begin
		set @sql = @sql + '
	and s.SHIFT = ''' + @Shift + '''' 
	End 

	
	set @sql = @sql + '
	Group by s.OutputDate, s.FactoryID, s.SewingLineID, s.Shift, o.StyleID, s.Manpower, o.CdCodeID, sd.ComboType, o.SeasonID, o.BrandID, o.Category, f.CountryID
' 

End
Else
Begin
	set @sql = N'
select s.OutputDate
	, s.FactoryID
	, s.SewingLineID
	, s.Shift
	, s.Category
	, s.StyleID
	, s.Manpower
	, s.ManHour
	, s.TotalOutput
	, s.CD
	, s.SeasonID
	, s.BrandID
	, [Fabrication] = ''''
	, [ProductGroup] = ''''
	, [ProductFabrication] = ''''
	, [GSD] = s.[GSDProsmv] * 1.1
	, [Earnedhours] = s.[TotalOutput] * (s.[GSDProsmv] * 1.1) / 60
	, s.[TotalWorkingHours] 
	, [CumulateDaysofDaysinProduction] = ''''
	, [EfficiencyLine] = iif(s.[TotalWorkingHours] = 0, 0, (s.[TotalOutput] * s.[GSDProsmv] / 60) / s.[TotalWorkingHours])
	, s.GSDProsmv
	, [Earnedhours2] = s.TotalOutput * s.GSDProsmv / 60
	, [EfficiencyLine2] = ''''
	, [NoofInlineDefects] = ''''
	, [NoofEndlineDefectiveGarments] = ''''
	, [WFT] = ''''
	, s.Country
	, s.Month
	, s.IsGSDPro
	, s.Orderseq
from 
(
	select s.OutputDate
		, s.FactoryID
		, s.SewingLineID
		, s.Shift
		, o.Category
		, o.StyleID
		, s.Manpower
		, [ManHour] = sum(sd.WorkHour)
		, [TotalOutput] = sum(sd.QAQty)
		, [CD] = concat(o.CdCodeID, ''-'', sd.ComboType)
		, o.SeasonID
		, o.BrandID 
		, [TotalWorkingHours] = sum(sd.WorkHour) * s.Manpower
		, [GSDProsmv] = iif(SUM(isnull(sd.QAQty, 0)) = 0, 0, SUM(o.CPU * o.CPUFactor * isnull([dbo].[GetOrderLocation_Rate](o.id, sd.ComboType), 100) / 100 * sd.QAQty * 1400) / SUM(sd.QAQty) / 60)
		, [Country] = case f.CountryID when ''PH'' then ''Philippines''
					   when ''VN'' then ''Vietnam''
					   when ''KH'' then ''Cambodia''
					   when ''CN'' then ''China''
				else f.CountryID end
		, [Month] = Left(DATENAME(m, s.OutputDate),3)
		, [IsGSDPro] = ''V''
		, [Orderseq] = case f.CountryID when ''PH'' then 1
					   when ''VN'' then 3
					   when ''KH'' then 2
					   when ''CN'' then 4
					else 5 end
	from [Production].[dbo].SewingOutput s with (nolock)
	Inner join [Production].[dbo].SewingOutput_Detail sd with (nolock) on s.ID = sd.ID
	Inner Join [Production].[dbo].Orders o with (nolock) on sd.OrderId = o.ID
	Inner join [Production].[dbo].Factory f with (nolock) on o.FactoryID=f.ID

	Where  s.Outputdate between ''' + FORMAT(@OutPutDateS, 'yyyyMMdd') + ''' and  ''' + FORMAT(@OutPutDateE, 'yyyyMMdd') + '''
	and o.BrandID in (''Adidas'', ''Reebok'') 
	and o.category in (''B'', ''S'') 
	and f.IsSampleRoom = 0
	and not (sd.WorkHour = 0 and sd.QAQty = 0)
	and s.Shift <> ''O''
	and o.LocalOrder = 0
'

	if @M <> ''
	Begin
		set @sql = @sql + '
	and s.MDivisionID  = ''' + @M + '''' 
	End

	if @FactoryID <> ''
	Begin
		set @sql = @sql + '
	and s.FactoryID = ''' + @FactoryID + '''' 
	End
	
	if @CDCode <> ''
	Begin
		set @sql = @sql + '
	and o.CDCodeID = ''' + @CDCode + '''' 
	End
	
	if @Shift <> ''
	Begin
		set @sql = @sql + '
	and s.SHIFT = ''' + @Shift + '''' 
	End 

	
	set @sql = @sql + '
	Group by s.OutputDate, s.FactoryID, s.SewingLineID, s.Shift, o.StyleID, s.Manpower, o.CdCodeID, sd.ComboType, o.SeasonID, o.BrandID, o.Category, f.CountryID
)s '
End

	--print @sql
	execute(@sql);

END
