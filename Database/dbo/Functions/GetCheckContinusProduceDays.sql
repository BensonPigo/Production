
Create FUNCTION [dbo].[GetCheckContinusProduceDays]
(
	@StyleUkey bigint ,
	@SewingLineID varchar(3) ,
	@FactoryID varchar(5),
	@Team varchar(3),
	@SewingDate date
)
RETURNS INT
AS
BEGIN
	declare  @Day nvarchar(256);

	;WITH tmpStyle AS 
	(
		SELECT ID, BrandID
		FROM Style WITH (NOLOCK)
		WHERE Ukey = @StyleUkey
	), tmpSewingOutputDay AS
	(
		select distinct top 30 so.OutputDate
		from SewingOutput so with (nolock)
		where   so.SewingLineID = @SewingLineID and
				so.FactoryID = @FactoryID and
				so.Team = @Team and
				so.OutputDate < @SewingDate and
				so.Shift <> 'O' and
				so.Category = 'O'
		order by    so.OutputDate desc
	), tmpSewingOutputID AS
	(
		select  so.ID, so.OutputDate
		from SewingOutput so with (nolock)
		where   so.SewingLineID = @SewingLineID and
				so.FactoryID = @FactoryID and
				so.Team = @Team and
				so.OutputDate in (select OutputDate from tmpSewingOutputDay) and
				so.Shift <> 'O' and
				so.Category = 'O'
	), tmpSewingOutputStyle AS
	(
		select  distinct
		so.OutputDate,
		o.StyleID,
		o.BrandID
		from  tmpSewingOutputID so
		inner join  SewingOutput_Detail sod with (nolock) on sod.ID = so.ID
		inner join  Orders o  with (nolock) on o.ID = sod.OrderID
	),tmpSewingSimlarStyle AS
	(
		select
		* 
		from 
		(
			select  OutputDate, StyleID, BrandID from tmpSewingOutputStyle
			union
			select  tso.OutputDate, [StyleID] = ss.ChildrenStyleID, [BrandID] = ss.ChildrenBrandID
			from tmpSewingOutputStyle tso
			inner join Style_SimilarStyle ss with (nolock) on   ss.MasterBrandID = tso.BrandID and
																ss.MasterStyleID = tso.StyleID
		) a
	),interruptDate AS
	(
		select   [interruptDate] = max(OutputDate)
		from    tmpSewingOutputDay
		where   OutputDate not in (
					select  distinct OutputDate
					from tmpSewingSimlarStyle tss
					where exists(select 1 from tmpStyle s where s.ID = tss.StyleID and s.BrandID = tss.BrandID))
	),tmpEnd AS
	(
		SELECT 
			[ContinusDays] = 
				CASE 
					WHEN (SELECT [interruptDate] FROM interruptDate) IS NULL 
						 AND EXISTS (
							 SELECT 1
							 FROM tmpSewingSimlarStyle tss
							 WHERE EXISTS (
								 SELECT 1 
								 FROM tmpStyle s 
								 WHERE s.ID = tss.StyleID AND s.BrandID = tss.BrandID
							 )
						 )
					THEN (SELECT COUNT(1) + 1 FROM tmpSewingOutputDay)

					WHEN (SELECT [interruptDate] FROM interruptDate) IS NULL THEN 1

					ELSE 
						(SELECT COUNT(1) + 1 
						 FROM tmpSewingOutputDay 
						 WHERE OutputDate > (SELECT [interruptDate] FROM interruptDate))
				END
	)

	SELECT @Day = [ContinusDays] FROM tmpEnd

	RETURN @Day
END
