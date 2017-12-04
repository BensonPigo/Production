CREATE TABLE [dbo].[SubProcessLearnCurve]
(
	[ukey] BIGINT NOT NULL , 
    [Type] VARCHAR(10) NOT NULL DEFAULT (''), 
    [ID] VARCHAR(10) NOT NULL DEFAULT (''), 
    [Description] NVARCHAR(100) NULL DEFAULT (''), 
    [Remark] NVARCHAR(100) NULL DEFAULT (''), 
    [Junk] BIT NULL DEFAULT (''), 
    [AddName] VARCHAR(10) NULL DEFAULT (''), 
    [AddDate] DATETIME NULL, 
    [EditName] VARCHAR(10) NULL DEFAULT (''), 
    [EditDate] DATETIME NULL, 
    CONSTRAINT [PK_SubProcessLearnCurve] PRIMARY KEY ([Type], [ID])
)
