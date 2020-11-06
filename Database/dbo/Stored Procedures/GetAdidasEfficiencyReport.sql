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
	, [Fabrication] = ''=IFERROR(VLOOKUP(LEFT(INDIRECT(ADDRESS(ROW(), 10)), 2),''''Adidas data ''''!$A$2:$G$116, 4, FALSE), "")''
	, [ProductGroup] = ''=IFERROR(VLOOKUP(LEFT(INDIRECT(ADDRESS(ROW(), 10)), 2),''''Adidas data ''''!$A$2:$G$116, 7, FALSE), "")''
	, [ProductFabrication] = ''=INDIRECT(ADDRESS(ROW(), 14))&INDIRECT(ADDRESS(ROW(), 13))''
	, [GSD] = ''=INDIRECT(ADDRESS(ROW(),21)) * 1.1''
	, [Earnedhours] = ''=IF(INDIRECT(ADDRESS(ROW(), 9))="","",IFERROR((INDIRECT(ADDRESS(ROW(), 9))*INDIRECT(ADDRESS(ROW(), 16)))/60,""))''
	, [TotalWorkingHours] = ''=INDIRECT(ADDRESS(ROW(), 8))*INDIRECT(ADDRESS(ROW(), 7))''
	, [CumulateDaysofDaysinProduction] = (select cumulate from dbo.getSewingOutputCumulateOfDays(o.StyleID, s.SewingLineID, s.OutputDate, s.FactoryID))
	, [EfficiencyLine] = ''=INDIRECT(ADDRESS(ROW(), 17))/INDIRECT(ADDRESS(ROW(), 18))''	
	, [GSDProsmv] = SUM(iif(isnull(sd.QAQty, 0) = 0, 0, (o.CPU * o.CPUFactor * isnull([dbo].[GetOrderLocation_Rate](o.id, sd.ComboType), 100)) / sd.QAQty))
	, [Earnedhours2] = ''=IF(INDIRECT(ADDRESS(ROW(),9))="","",IFERROR(INDIRECT(ADDRESS(ROW(),9))*INDIRECT(ADDRESS(ROW(),21))/60,""))''
	, [EfficiencyLine2] = ''=INDIRECT(ADDRESS(ROW(),22))/INDIRECT(ADDRESS(ROW(),18))'''

	if @IsSintexEffReportCompare = 0
	begin
		set @sql = @sql+ N'
		, [NoofInlineDefects] = sum(isnull(InlineInspection.RejectWIP, 0))
		, [NoofEndlineDefectiveGarments] = sum(isnull(Inspection.cnt, 0))
		, [WFT] = ''=IFERROR((INDIRECT(ADDRESS(ROW(), 24))+INDIRECT(ADDRESS(ROW(), 25)))/INDIRECT(ADDRESS(ROW(), 9)),"")'''
	end
	else
	begin
		set @sql = @sql+ N'
		, [NoofInlineDefects] = 0
		, [NoofEndlineDefectiveGarments] = 0
		, [WFT] = '''''
	end


	set @sql = @sql+ N'
	, [Country] = case f.CountryID when ''PH'' then ''Philippines''
					   when ''VN'' then ''Vietnam''
					   when ''KH'' then ''Cambodia''
					   when ''CN'' then ''China''
				else f.CountryID end
	  ,[Month] = Left(DATENAME(m, s.OutputDate),3)
	  ,[IsGSDPro] = ''V''
	  ,[Orderseq] = case f.CountryID when ''PH'' then 1
					   when ''VN'' then 3
					   when ''KH'' then 2
					   when ''CN'' then 4
					else 5 end
from [Production].[dbo].SewingOutput s with (nolock)
Inner join [Production].[dbo].SewingOutput_Detail sd with (nolock) on s.ID = sd.ID
Inner Join [Production].[dbo].Orders o with (nolock) on sd.OrderId = o.ID
Inner join [Production].[dbo].Factory f with (nolock) on o.FactoryID=f.ID'

	if @IsSintexEffReportCompare = 0
	begin
		set @sql = @sql+ N'
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
			)Inspection'
	end

set @sql = @sql+ N'
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
order by s.OutputDate, s.FactoryID, s.SewingLineID, s.Shift, o.StyleID, o.SeasonID, o.BrandID' 

	--print @sql
	execute(@sql);


END
