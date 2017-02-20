


CREATE FUNCTION [dbo].[GetStdQty]
(
	@orderid VARCHAR(13),
	@combotype VARCHAR(1),
	@sewingscheduleid BIGINT,
	@hourlystandoutput INT
)
RETURNS INT
AS
BEGIN
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
	DECLARE @output INT
	IF @combotype = 'A'
		BEGIN
			DECLARE @styleunit VARCHAR(6),
					@factory VARCHAR(8),
					@styleukey BIGINT
			select @styleunit = StyleUnit,@factory = FtyGroup,@styleukey = StyleUkey from Orders where ID = @orderid
			IF @styleunit = 'PCS'
				BEGIN
					select @output = sum(b.StdOutPut)
					from (select a.SewingLineID,MAX(a.AVGHour*a.StandardOutput) as StdOutPut
						  from (select s.ID,s.SewingLineID,isnull(AVG(w.Hours),0) as AVGHour,s.StandardOutput
								from SewingSchedule s, WorkHour w 
								where s.OrderID = @orderid
								and s.SewingLineID = w.SewingLineID
								and s.FactoryID = w.FactoryID
								and w.Date between CONVERT(date,s.Inline) and CONVERT(date,s.Offline)
								and w.Hours > 0
								group by s.ID,s.SewingLineID,s.StandardOutput) a
						  group by a.SewingLineID) b
				END
			ELSE
				BEGIN
					select @output = MIN(a.StdOutPut)
					from (select sl.Location,o.StdOutPut
						  from (select Location from Style_Location where StyleUkey = @styleukey) sl 
						  left join (select a.ComboType,SUM(a.StdOutPut) as StdOutPut
									 from (select a.SewingLineID,a.ComboType,MAX(a.AVGHour*a.StandardOutput) as StdOutPut
										   from (select s.ID,s.SewingLineID,s.ComboType,isnull(AVG(w.Hours),0) as AVGHour,s.StandardOutput
												 from SewingSchedule s, WorkHour w 
												 where s.OrderID = @orderid 
												 and s.SewingLineID = w.SewingLineID
												 and s.FactoryID = w.FactoryID
												 and w.Date between CONVERT(date,s.Inline) and CONVERT(date,s.Offline) 
												 and w.Hours > 0 
												 group by s.ID,s.SewingLineID,s.ComboType,s.StandardOutput) a
										   group by a.SewingLineID,a.ComboType) a
									 group by a.ComboType) o on o.ComboType = sl.Location) a
				END
			
		END
	ELSE
		BEGIN
			DECLARE @avghour DECIMAL,
					@standardoutput INT
			select @avghour = isnull(AVG(w.Hours),0)
			from SewingSchedule s, WorkHour w
			where s.ID = @sewingscheduleid
			and s.FactoryID = w.FactoryID
			and w.Date between CONVERT(date,s.Inline) and CONVERT(date,s.Offline)
			and w.Hours > 0

			SET @output = @hourlystandoutput*@avghour
		END
	return @output
END