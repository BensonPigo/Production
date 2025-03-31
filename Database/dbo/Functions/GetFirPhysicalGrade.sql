
Create FUNCTION [dbo].[GetFirPhysicalGrade]
(
	@Point  NUMERIC(10, 2),
	@BrandID  varchar(15),
	@WeaveTypeID  varchar(15),
	@InspectionGroup varchar(15)
)
RETURNS @tmpFirPhysicalGrade Table
	(
		Grade varchar(10),
		Result varchar(10),
		IsColorFormat bit,
		ShowGrade varchar(10),
		isFormatInP01 bit
	)
AS
BEGIN
	
	insert into @tmpFirPhysicalGrade
	SELECT TOP 1    Grade,
                    [Result] = case Result	when 'P' then 'Pass'
											when 'F' then 'Fail'
											else Result
											end,
					IsColorFormat,
					ShowGrade,
					isFormatInP01
	FROM FIR_Grade WITH (NOLOCK) 
	WHERE WeaveTypeID = @WeaveTypeID
	AND Percentage >= IIF(@Point > 100, 100, @Point)
	AND BrandID = @BrandID
	and InspectionGroup = @InspectionGroup
	ORDER BY Percentage
                
	-- 若沒有找到等級，就用預設的等級
	if  not exists(select 1 from @tmpFirPhysicalGrade)
	begin
		INSERT INTO @tmpFirPhysicalGrade
	    SELECT TOP 1    Grade,
	                    [Result] = case Result	when 'P' then 'Pass'
											when 'F' then 'Fail'
											else Result
											end,
						IsColorFormat,
						ShowGrade,
						isFormatInP01
	    FROM FIR_Grade WITH (NOLOCK) 
	    WHERE WeaveTypeID = @WeaveTypeID
	    AND Percentage >= IIF(@Point > 100, 100, @Point)
	    AND BrandID = ''
	    ORDER BY Percentage
	end

	RETURN 
END