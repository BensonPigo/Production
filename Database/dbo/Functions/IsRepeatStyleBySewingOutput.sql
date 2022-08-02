
Create FUNCTION [dbo].[IsRepeatStyleBySewingOutput]
(
	@FactoryID varchar(8),
	@OutputDate date,
	@SewingLineID varchar(5),
	@Team varchar(5),
	@StyleUkey bigint
)
RETURNS varchar(20)
AS
BEGIN
	if Exists	(
					select	1
					from	(	select	[StyleID] = s.ID, s.BrandID
								from Style s with (nolock)
								where s.Ukey = @StyleUkey
								union 
								select	[StyleID] = ssm.ChildrenStyleID, [BrandID] = ssm.ChildrenBrandID
								from Style s with (nolock)
								inner join Style_SimilarStyle ssm with (nolock) on ssm.MasterStyleID = s.ID and ssm.MasterBrandID = s.BrandID
								where s.Ukey = @StyleUkey
							) OriStyleInfo
					where exists(	select	1
									from SewingOutput_Detail sod with (nolock)
									inner join Orders o with (nolock) on o.ID = sod.OrderId
									where sod.ID in (	select so.ID
														from SewingOutput so with (nolock)
														where	so.SewingLineID = @SewingLineID and
																so.FactoryID = @FactoryID and
																so.Team = @Team and
																so.OutputDate in (select distinct top 30 so.OutputDate
																				  from SewingOutput so with (nolock)
																				  where   so.SewingLineID = @SewingLineID and
																				          so.FactoryID = @FactoryID and
																				          so.Team = @Team and
																				          so.OutputDate < @OutputDate
																				  order by    so.OutputDate desc)
														) and
									o.BrandID = OriStyleInfo.BrandID and
									o.StyleID = OriStyleInfo.StyleID
								)
				)
	begin
		return 'Repeat Style'
	end
	
	return 'New Style'
	
END