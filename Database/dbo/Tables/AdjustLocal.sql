CREATE TABLE [dbo].[AdjustLocal]
(
	[Id] VARCHAR(13) NOT NULL PRIMARY KEY DEFAULT (''), 
    [MDivisionID] VARCHAR(8) NULL DEFAULT (''), 
    [FactoryID] VARCHAR(8) NULL DEFAULT (''), 
    [IssueDate] DATE NOT NULL, 
    [Remark] NVARCHAR(60) NULL DEFAULT (''), 
    [Status] VARCHAR(15) NULL DEFAULT (''), 
    [AddName] VARCHAR(10) NOT NULL DEFAULT ('') , 
    [AddDate] DATETIME NOT NULL, 
    [EditName] VARCHAR(10) NULL DEFAULT (''), 
    [EditDate] DATETIME NULL, 
    [Type] VARCHAR NOT NULL DEFAULT (''), 
    [StocktakingID] VARCHAR(13) NULL DEFAULT ('')
)
