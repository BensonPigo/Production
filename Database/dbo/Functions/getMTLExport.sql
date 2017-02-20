
CREATE FUNCTION [dbo].[getMTLExport](@poid varchar(13), @mtlexport varchar(2))
RETURNS varchar(3)
BEGIN
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
/*
	DECLARE @string varchar(3) --要回傳的字串
	IF @mtlexport <> ''
		BEGIN
			SET @string = @mtlexport
		END
	ELSE
		BEGIN
			Select @string = Count(Distinct ID) from Export_Detail where POID = @poid
		END*/

	RETURN iif(@mtlexport<>''
							,@mtlexport
							,convert(varchar(3),(select Count(Distinct ID) from Export_Detail where POID = @poid)))
END