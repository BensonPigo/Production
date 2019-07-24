-- =============================================
-- Author:		Jack
-- Create date: 2019/07/05
-- Description:	GCD
-- =============================================
CREATE FUNCTION [dbo].[getGCD]
(
	@val as varchar(500)
)
RETURNS Int
AS
BEGIN
   DECLARE @tb TABLE (i INT identity, spdata NVARCHAR (max))
   DECLARE @strt INT
   DECLARE @end INT
   DECLARE @a INT
   DECLARE @b INT
   DECLARE @t INT
   DECLARE @cnt INT
   DECLARE @ind INT
  
   SELECT @strt = 1, @end = CHARINDEX (',', @val)
   WHILE @strt < LEN (@val) + 1
   BEGIN
      IF @end = 0 SET @end = LEN (@val) + 1
      INSERT INTO @tb (spdata) VALUES (SUBSTRING (@val, @strt, @end - @strt))
      SET @strt = @end + 1
      SET @end = CHARINDEX (',', @val, @strt)
   END
 
   SELECT @cnt = max (i) FROM @tb
   SELECT @a = convert (INT, spdata) FROM @tb WHERE i = 1
   SET @ind = 2
   WHILE @ind <= @cnt
   BEGIN
      SELECT @b = convert (INT, spdata) FROM @tb WHERE i = @ind
      WHILE (@b % @a) != 0
      BEGIN
         SET @t = @b % @a
         SET @b = @a
         SET @a = @t
      END
      SET @ind = @ind + 1
   END

   return @a
END
