CREATE TABLE [dbo].[Style_SizeCode] (
    [StyleUkey] BIGINT      CONSTRAINT [DF_Style_SizeCode_StyleUkey] DEFAULT ((0)) NOT NULL,
    [Seq]       VARCHAR (2) CONSTRAINT [DF_Style_SizeCode_Seq] DEFAULT ('') NOT NULL,
    [SizeGroup] VARCHAR (1) CONSTRAINT [DF_Style_SizeCode_SizeGroup] DEFAULT ('') NULL,
    [SizeCode]  VARCHAR (8) CONSTRAINT [DF_Style_SizeCode_SizeCode] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_Style_SizeCode] PRIMARY KEY CLUSTERED ([StyleUkey] ASC, [Seq] ASC, [SizeCode] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SizeSpec的上方標題', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_SizeCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'款式的唯一值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_SizeCode', @level2type = N'COLUMN', @level2name = N'StyleUkey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'順序', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_SizeCode', @level2type = N'COLUMN', @level2name = N'Seq';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SizeGroup', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_SizeCode', @level2type = N'COLUMN', @level2name = N'SizeGroup';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SizeCode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_SizeCode', @level2type = N'COLUMN', @level2name = N'SizeCode';

