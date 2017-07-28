CREATE TABLE [dbo].[LocationTransLocal]
(
	[ID] VARCHAR(13) NOT NULL PRIMARY KEY DEFAULT (''), 
    [MDivisionID] VARCHAR(8) NOT NULL DEFAULT (''), 
    [FactoryID] VARCHAR(8) NOT NULL DEFAULT (''), 
    [IssueDate] DATE NOT NULL, 
    [StockType] VARCHAR NOT NULL DEFAULT (''), 
    [Status] VARCHAR(15) NOT NULL DEFAULT (''), 
    [Remark] NVARCHAR(60) NULL DEFAULT (''), 
    [AddName] VARCHAR(10) NULL DEFAULT (''), 
    [AddDate] DATETIME NULL, 
    [EditName] VARCHAR(10) NULL DEFAULT (''), 
    [EditDate] DATETIME NULL
)
