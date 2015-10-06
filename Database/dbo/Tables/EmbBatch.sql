CREATE TABLE [dbo].[EmbBatch] (
    [BeginStitch] NUMERIC (7)    CONSTRAINT [DF_EmbBatch_BeginStitch] DEFAULT ((0)) NOT NULL,
    [EndStitch]   NUMERIC (7)    CONSTRAINT [DF_EmbBatch_EndStitch] DEFAULT ((0)) NOT NULL,
    [BatchNo]     NUMERIC (3, 1) CONSTRAINT [DF_EmbBatch_BatchNo] DEFAULT ((0)) NOT NULL,
    [AddName]     VARCHAR (10)   CONSTRAINT [DF_EmbBatch_AddName] DEFAULT ('') NULL,
    [AddDate]     DATETIME       NULL,
    [EditName]    VARCHAR (10)   CONSTRAINT [DF_EmbBatch_EditName] DEFAULT ('') NULL,
    [EditDate]    DATETIME       NULL,
    CONSTRAINT [PK_EmbBatch] PRIMARY KEY CLUSTERED ([BeginStitch] ASC, [EndStitch] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Embroidery Batch', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'EmbBatch';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'起始針數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'EmbBatch', @level2type = N'COLUMN', @level2name = N'BeginStitch';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'終止針數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'EmbBatch', @level2type = N'COLUMN', @level2name = N'EndStitch';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'每小時完成次數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'EmbBatch', @level2type = N'COLUMN', @level2name = N'BatchNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'EmbBatch', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'EmbBatch', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'EmbBatch', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'EmbBatch', @level2type = N'COLUMN', @level2name = N'EditDate';

