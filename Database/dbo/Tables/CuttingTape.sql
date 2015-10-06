CREATE TABLE [dbo].[CuttingTape] (
    [ID]           VARCHAR (13) CONSTRAINT [DF_CuttingTape_ID] DEFAULT ('') NOT NULL,
    [FactoryId]    VARCHAR (10) CONSTRAINT [DF_CuttingTape_FactoryId] DEFAULT ('') NOT NULL,
    [OldEachcon]   DATE         NULL,
    [TapeInline]   DATE         NULL,
    [AddName]      VARCHAR (10) CONSTRAINT [DF_CuttingTape_AddName] DEFAULT ('') NULL,
    [AddDate]      DATETIME     NULL,
    [EditName]     VARCHAR (10) CONSTRAINT [DF_CuttingTape_EditName] DEFAULT ('') NULL,
    [EditDate]     DATETIME     NULL,
    [Sewinline]    DATE         NULL,
    [Sewoffline]   DATE         NULL,
    [Sewinglineid] VARCHAR (60) CONSTRAINT [DF_CuttingTape_Sewinglineid] DEFAULT ('') NULL,
    CONSTRAINT [PK_CuttingTape] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'外裁主檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CuttingTape';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CuttingTape', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CuttingTape', @level2type = N'COLUMN', @level2name = N'FactoryId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'舊each cons apv.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CuttingTape', @level2type = N'COLUMN', @level2name = N'OldEachcon';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'外裁最早上線日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CuttingTape', @level2type = N'COLUMN', @level2name = N'TapeInline';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CuttingTape', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CuttingTape', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CuttingTape', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CuttingTape', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最早車縫上線日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CuttingTape', @level2type = N'COLUMN', @level2name = N'Sewinline';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最晚車縫下線日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CuttingTape', @level2type = N'COLUMN', @level2name = N'Sewoffline';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'車縫線', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CuttingTape', @level2type = N'COLUMN', @level2name = N'Sewinglineid';

