CREATE TABLE [dbo].[ThreadColorGroup]
(
	[Id] VARCHAR(50) NOT NULL , 
    [Description] NVARCHAR(100) CONSTRAINT [DF_ThreadColorGroup_Description] DEFAULT ('') NULL,
    [Junk] BIT CONSTRAINT [DF_ThreadColorGroup_Junk] DEFAULT ((0)) NULL,
    [AddName] VARCHAR(10) CONSTRAINT [DF_ThreadColorGroup_AddName] DEFAULT ('') NULL,
    [AddDate] DATETIME NULL, 
    [EditName] VARCHAR(10) CONSTRAINT [DF_ThreadColorGroup_EditName] DEFAULT ('') NULL,
    [EditDate] DATETIME NULL,
    CONSTRAINT [PK_ThreadColorGroup] PRIMARY KEY CLUSTERED ([id] ASC)
)

