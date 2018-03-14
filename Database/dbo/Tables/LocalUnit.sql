CREATE TABLE [dbo].[LocalUnit]
(
	[Id] VARCHAR(8) CONSTRAINT [DF_LocalUnit_ID]    DEFAULT ('') NOT NULL, 
    [Description] NVARCHAR(120) CONSTRAINT [DF_LocalUnit_Description]   DEFAULT ('') NULL, 
    [Junk] BIT CONSTRAINT [DF_LocalUnit_Junk]   DEFAULT ((0)) NULL, 
    [AddName] VARCHAR(10) CONSTRAINT [DF_LocalUnit_AddName]   DEFAULT ('') NULL, 
    [AddDate] DATETIME  NULL, 
    [EditName] VARCHAR(10) CONSTRAINT [DF_LocalUnit_EditName]   DEFAULT ('') NULL, 
    [EditDate] DATETIME  NULL,
	CONSTRAINT [PK_LocalUnit] PRIMARY KEY CLUSTERED ([ID] ASC)
);

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Local Unit 基本檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalUnit';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單位代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocalUnit', @level2type = N'COLUMN', @level2name = N'ID';

