CREATE TABLE [dbo].[Construction] (
    [Id]           VARCHAR (10)  CONSTRAINT [DF_Construction_Id] DEFAULT ('') NOT NULL,
    [Name]         NVARCHAR (50) CONSTRAINT [DF_Construction_Name] DEFAULT ('') NOT NULL,
    [CuttingLayer] SMALLINT      CONSTRAINT [DF_Construction_CuttingLayer] DEFAULT ((0)) NULL,
    [Junk]         BIT           CONSTRAINT [DF_Construction_Junk] DEFAULT ((0)) NULL,
    [AddName]      VARCHAR (10)  CONSTRAINT [DF_Construction_AddName] DEFAULT ('') NULL,
    [AddDate]      DATETIME      NULL,
    [EditName]     VARCHAR (10)  CONSTRAINT [DF_Construction_EditName] DEFAULT ('') NULL,
    [EditDate]     DATETIME      NULL,
    CONSTRAINT [PK_Construction] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Construction', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Construction';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組成代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Construction', @level2type = N'COLUMN', @level2name = N'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'說明', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Construction', @level2type = N'COLUMN', @level2name = N'Name';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'裁剪層數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Construction', @level2type = N'COLUMN', @level2name = N'CuttingLayer';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'刪除', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Construction', @level2type = N'COLUMN', @level2name = N'Junk';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Construction', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Construction', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Construction', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Construction', @level2type = N'COLUMN', @level2name = N'EditDate';

