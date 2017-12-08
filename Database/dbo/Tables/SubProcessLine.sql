CREATE TABLE [dbo].[SubProcessLine]
(
	[Type] VARCHAR(10) NOT NULL DEFAULT ('') , 
    [ID] VARCHAR(10) NOT NULL DEFAULT (''), 
    [GroupID] VARCHAR(10) NULL DEFAULT (''), 
    [Description] NVARCHAR(100) NULL DEFAULT (''), 
    [Manpower] NUMERIC(5, 2) NULL DEFAULT ((0)), 
    [Remark] NVARCHAR(100) NULL DEFAULT (''), 
    [Junk] BIT NULL DEFAULT ((0)), 
    [AddName] VARCHAR(10) NULL DEFAULT (''), 
    [AddDate] DATETIME NULL, 
    [EditName] VARCHAR(10) NULL DEFAULT (''), 
    [EditDate] DATETIME NULL, 
    PRIMARY KEY ([Type], [ID], [Factory])
)
