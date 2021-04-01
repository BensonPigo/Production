CREATE TABLE [dbo].[SemiFinished]
(
	[Refno] VARCHAR(21) NOT NULL, 
    [Description] NVARCHAR(MAX) CONSTRAINT [DF_SemiFinished_Desc] DEFAULT ('') NOT NULL, 
    [Unit] VARCHAR(8) CONSTRAINT [DF_SemiFinished_Unit] DEFAULT ('') NOT NULL,
    [AddName] VARCHAR(10) CONSTRAINT [DF_SemiFinished_AddName] DEFAULT ('') NOT NULL,
    [AddDate] DATETIME NULL, 
    [EditName] VARCHAR(10) CONSTRAINT [DF_SemiFinished_EditName] DEFAULT ('') NOT NULL,
    [EditDate] DATETIME NULL,
	CONSTRAINT [PK_SemiFinished] PRIMARY KEY CLUSTERED  ([Refno] ASC)
)
