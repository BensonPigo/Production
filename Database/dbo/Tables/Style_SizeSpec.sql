CREATE TABLE [dbo].[Style_SizeSpec] (
    [StyleUkey] BIGINT       CONSTRAINT [DF_Style_SizeSpec_StyleUkey] DEFAULT ((0)) NOT NULL,
    [SizeItem]  VARCHAR (3)  CONSTRAINT [DF_Style_SizeSpec_SizeItem] DEFAULT ('') NOT NULL,
    [SizeCode]  VARCHAR (8)  CONSTRAINT [DF_Style_SizeSpec_SizeCode] DEFAULT ('') NOT NULL,
    [SizeSpec]  VARCHAR (15) CONSTRAINT [DF_Style_SizeSpec_SizeSpec] DEFAULT ('') NULL,
    [UKey] BIGINT NOT NULL DEFAULT ((0)), 
    CONSTRAINT [PK_Style_SizeSpec] PRIMARY KEY CLUSTERED ([UKey])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SizeSpec的value', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_SizeSpec';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'款式的唯一值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_SizeSpec', @level2type = N'COLUMN', @level2name = N'StyleUkey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'量法順序(橫欄)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_SizeSpec', @level2type = N'COLUMN', @level2name = N'SizeItem';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SizeCode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_SizeSpec', @level2type = N'COLUMN', @level2name = N'SizeCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'應對尺寸', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_SizeSpec', @level2type = N'COLUMN', @level2name = N'SizeSpec';

