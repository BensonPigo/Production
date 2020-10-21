CREATE PROCEDURE [dbo].[GetAdidasEfficiencyReport]
	@OutPutDateS as date,
	@OutPutDateE as date,
	@M as varchar(8) = '',
	@FactoryID as varchar(8) = '',
	@CDCode as varchar(6) = '',
	@Shift as varchar(1) = ''
AS
BEGIN
	declare @sql as varchar(max) = ''

	if @OutPutDateS = null or @OutPutDateE = null
	begin
		return
	end

	set @sql = N'
select s.OutputDate
	, s.FactoryID
	, s.SewingLineID
	, s.Shift
	, [Category] = ''Inline''
	, o.StyleID
	, s.Manpower
	, [ManHour] = sum(sd.WorkHour)
	, [TotalOutput] = sum(sd.QAQty)
	, [CD] = concat(o.CdCodeID, ''-'', sd.ComboType)
	, o.SeasonID
	, o.BrandID
	, [Fabrication] = ''=IFERROR(VLOOKUP(LEFT(INDIRECT(ADDRESS(ROW(), 10)), 2),''''Adidas data ''''!$A$2:$G$116, 4, FALSE), "")''
	, [ProductGroup] = ''=IFERROR(VLOOKUP(LEFT(INDIRECT(ADDRESS(ROW(), 10)), 2),''''Adidas data ''''!$A$2:$G$116, 7, FALSE), "")''
	, [ProductFabrication] = ''=INDIRECT(ADDRESS(ROW(), 14))&INDIRECT(ADDRESS(ROW(), 13))''
	, [GSD] = ''''
	, [Earnedhours] = ''=IF(INDIRECT(ADDRESS(ROW(), 9))="","",IFERROR((INDIRECT(ADDRESS(ROW(), 9))*INDIRECT(ADDRESS(ROW(), 16)))/60,""))''
	, [TotalWorkingHours] = ''=INDIRECT(ADDRESS(ROW(), 8))*INDIRECT(ADDRESS(ROW(), 7))''
	, [CumulateDaysofDaysinProduction] = (select cumulate from dbo.getSewingOutputCumulateOfDays(o.StyleID, s.SewingLineID, s.OutputDate, s.FactoryID))
	, [EfficiencyLine] = ''=INDIRECT(ADDRESS(ROW(), 17))/INDIRECT(ADDRESS(ROW(), 18))''
	, [NoofInlineDefects] = sum(isnull(InlineInspection.RejectWIP, 0))
	, [NoofEndlineDefectiveGarments] = sum(isnull(Inspection.cnt, 0))
	, [WFT] = ''=IFERROR((INDIRECT(ADDRESS(ROW(), 21))+INDIRECT(ADDRESS(ROW(), 22)))/INDIRECT(ADDRESS(ROW(), 9)),"")''
from [Production].[dbo].SewingOutput s with (nolock)
Inner join [Production].[dbo].SewingOutput_Detail sd with (nolock) on s.ID = sd.ID
Inner Join [Production].[dbo].Orders o with (nolock) on sd.OrderId = o.ID
Inner join [Production].[dbo].Factory f with (nolock) on o.FactoryID=f.ID
Outer apply (
	select [RejectWIP] = Sum(i.RejectWIP)
	from [ManufacturingExecution].[dbo].[InlineInspection] i with (nolock)
	where  s.OutputDate = i.InspectionDate 
	and s.SewingLineID = i.Line 
	and s.FactoryID = i.FactoryID 
	and s.Shift = iif(i.Shift = ''Day'', ''D'', ''N'')
	and s.Team = i.Team 
	and sd.OrderId = i.OrderID 
	and sd.ComboType = i.Location 
)InlineInspection
Outer apply (
	select cnt = Count(*)
	from [ManufacturingExecution].[dbo].[Inspection] i with (nolock) 
	where s.OutputDate = i.InspectionDate 
	and s.SewingLineID = i.Line 
	and s.FactoryID = i.FactoryID 
	and s.Shift = iif(i.Shift = ''Day'', ''D'', ''N'')
	and s.Team = i.Team 
	and sd.OrderId = i.OrderID 
	and sd.ComboType = i.Location 
	and sd.Article = i.Article
	and i.Status in (''Fixed'',''Dispose'',''Reject'')
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
Group by s.OutputDate, s.FactoryID, s.SewingLineID, s.Shift, o.StyleID, s.Manpower, o.CdCodeID, sd.ComboType, o.SeasonID, o.BrandID
order by s.OutputDate, s.FactoryID, s.SewingLineID, s.Shift, o.StyleID, o.SeasonID, o.BrandID' 

	--print @sql
	execute(@sql);


END
