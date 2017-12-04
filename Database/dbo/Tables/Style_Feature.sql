CREATE TABLE [dbo].[Style_Feature]
(
	[ukey] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [styleUkey] BIGINT NULL, 
    [Type] VARCHAR(10) NULL DEFAULT (''), 
    [Feature] VARCHAR(30) NULL DEFAULT (''), 
    [SMV] NUMERIC(7, 4) NULL DEFAULT ((0)), 
    [Remark] NVARCHAR(100) NULL DEFAULT (''), 
    [AddName] VARCHAR(10) NULL DEFAULT (''), 
    [AddDate] DATETIME NULL, 
    [EditName] VARCHAR(10) NULL DEFAULT (''), 
    [EditDate] DATETIME NULL
)
