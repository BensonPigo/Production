
DECLARE @sql NVARCHAR(MAX);

IF @@SERVERNAME like '%tradedb%'
BEGIN
--formal tradedb
    SET @sql = 'CREATE VIEW [dbo].[View_DropdownList]
	AS SELECT * FROM tradedb.dbo.DropdownList;'
END
ELSE
BEGIN
--testing
    SET @sql = 'CREATE VIEW [dbo].[View_DropdownList]
	AS SELECT * FROM Production.dbo.DropdownList;'
END

EXEC sp_executesql @sql;