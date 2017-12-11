CREATE TABLE [dbo].[SubProcessFeature]
(
	[Type] VARCHAR(10) NOT NULL  DEFAULT (''), 
    [Feature] VARCHAR(30) NOT NULL DEFAULT (''), 
    [Remark] NVARCHAR(100) NULL DEFAULT (''), 
    [Junk] BIT NULL DEFAULT ((0)), 
    [AddName] VARCHAR(10) NULL DEFAULT (''), 
    [AddDate] DATETIME NULL, 
    [EditName] VARCHAR(10) NULL DEFAULT (''), 
    [EditDate] DATETIME NULL, 
    PRIMARY KEY ([Feature], [Type])
)
