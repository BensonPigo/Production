CREATE TABLE [dbo].[MNOrder_SizeSpec] (
    [ID]       VARCHAR (13) CONSTRAINT [DF_MNOrder_SizeSpec_ID] DEFAULT ('') NOT NULL,
    [SizeItem] VARCHAR (3)  CONSTRAINT [DF_MNOrder_SizeSpec_SizeItem] DEFAULT ('') NOT NULL,
    [Seq]      VARCHAR (2)  CONSTRAINT [DF_MNOrder_SizeSpec_Seq] DEFAULT ('') NULL,
    [SizeCode] VARCHAR (8)  CONSTRAINT [DF_MNOrder_SizeSpec_SizeCode] DEFAULT ('') NOT NULL,
    [SizeSpec] VARCHAR (15) CONSTRAINT [DF_MNOrder_SizeSpec_SizeSpec] DEFAULT ('') NULL,
    [Ukey]     BIGINT       NULL
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SizeSpec的value', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder_SizeSpec';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder_SizeSpec', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'量法順序(橫欄)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder_SizeSpec', @level2type = N'COLUMN', @level2name = N'SizeItem';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'尺寸順序(直欄)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder_SizeSpec', @level2type = N'COLUMN', @level2name = N'Seq';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'尺寸', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder_SizeSpec', @level2type = N'COLUMN', @level2name = N'SizeCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'應對尺寸', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder_SizeSpec', @level2type = N'COLUMN', @level2name = N'SizeSpec';


GO
CREATE NONCLUSTERED INDEX [IDX_MNOrder_SizeSpec]
    ON [dbo].[MNOrder_SizeSpec]([ID] ASC, [SizeItem] ASC, [SizeCode] ASC, [Ukey] ASC);

