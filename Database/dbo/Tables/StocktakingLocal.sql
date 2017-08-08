CREATE TABLE [dbo].[StocktakingLocal]
(
	[ID] VARCHAR(13) NOT NULL PRIMARY KEY DEFAULT (''), 
    [MDivisionID] VARCHAR(8) NOT NULL DEFAULT (''), 
    [FactoryID] VARCHAR(8) NOT NULL DEFAULT (''), 
    [IssueDate] DATE NOT NULL, 
    [Remark] NVARCHAR(60) NULL DEFAULT (''), 
    [Status] VARCHAR(15) NOT NULL DEFAULT (''), 
    [Type] VARCHAR NOT NULL DEFAULT (''), 
    [AdjustId] VARCHAR(13) NULL DEFAULT (''), 
    [Stocktype] VARCHAR NOT NULL DEFAULT (''), 
    [AddName] VARCHAR(10) NULL DEFAULT (''), 
    [AddDate] DATETIME NULL, 
    [EditName] VARCHAR(10) NULL DEFAULT (''), 
    [EditDate] DATETIME NULL
)
