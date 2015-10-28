CREATE TABLE [dbo].[CuttingTape] (
    [POID]            VARCHAR (13) CONSTRAINT [DF_CuttingTape_ID] DEFAULT ('') NOT NULL,
    [MDivisionID]     VARCHAR (8)  NOT NULL,
    [OldEachcon]      DATE         NULL,
    [TapeFirstInline] DATE         NULL,
    [AddName]         VARCHAR (10) CONSTRAINT [DF_CuttingTape_AddName] DEFAULT ('') NULL,
    [AddDate]         DATETIME     NULL,
    [EditName]        VARCHAR (10) CONSTRAINT [DF_CuttingTape_EditName] DEFAULT ('') NULL,
    [EditDate]        DATETIME     NULL,
    CONSTRAINT [PK_CuttingTape] PRIMARY KEY CLUSTERED ([POID] ASC, [MDivisionID] ASC) ON [SLAVE]
);






GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'外裁主檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CuttingTape';


GO



GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'舊each cons apv.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CuttingTape', @level2type = N'COLUMN', @level2name = N'OldEachcon';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CuttingTape', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CuttingTape', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CuttingTape', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CuttingTape', @level2type = N'COLUMN', @level2name = N'EditDate';


GO



GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'外裁最早上線日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CuttingTape', @level2type = N'COLUMN', @level2name = N'TapeFirstInline';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'POID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CuttingTape', @level2type = N'COLUMN', @level2name = N'POID';

