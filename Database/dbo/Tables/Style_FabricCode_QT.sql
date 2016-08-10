CREATE TABLE [dbo].[Style_FabricCode_QT] (
    [StyleUkey]    BIGINT         CONSTRAINT [DF_Style_FabricCode_QT_StyleUkey] DEFAULT ((0)) NOT NULL,
	[PatternPanel] VARCHAR(2) NULL DEFAULT (''),
    [FabricCode]   VARCHAR (3)    CONSTRAINT [DF_Style_FabricCode_QT_FabricCode] DEFAULT ('') NOT NULL,
    [LectraCode]   VARCHAR (2)    CONSTRAINT [DF_Style_FabricCode_QT_LectraCode] DEFAULT ('') NOT NULL,
    [SeqNO]        VARCHAR (2)    CONSTRAINT [DF_Style_FabricCode_QT_SeqNO] DEFAULT ('') NOT NULL,
	[QTPatternPanel] VARCHAR(2) NULL DEFAULT (''),
    [QTFabricCode] VARCHAR (3)    CONSTRAINT [DF_Style_FabricCode_QT_QTFabricCode] DEFAULT ('') NOT NULL,
    [QTLectraCode] VARCHAR (2)    CONSTRAINT [DF_Style_FabricCode_QT_QTLectraCode] DEFAULT ('') NULL,
    [QTWidth]      NUMERIC (3, 1) CONSTRAINT [DF_Style_FabricCode_QT_QTWidth] DEFAULT ((0)) NULL,
    [AddName]      VARCHAR (10)   CONSTRAINT [DF_Style_FabricCode_QT_AddName] DEFAULT ('') NULL,
    [AddDate]      DATETIME       NULL,
    [EditName]     VARCHAR (10)   CONSTRAINT [DF_Style_FabricCode_QT_EditName] DEFAULT ('') NULL,
    [EditDate]     DATETIME       NULL, 
    CONSTRAINT [PK_Style_FabricCode_QT] PRIMARY KEY CLUSTERED ([StyleUkey] ASC, [LectraCode] ASC, [SeqNO] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Style : Fabric Code (Bill Of Farbic) - QT', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_FabricCode_QT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'款式的唯一值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_FabricCode_QT', @level2type = N'COLUMN', @level2name = N'StyleUkey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'布種', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_FabricCode_QT', @level2type = N'COLUMN', @level2name = N'FabricCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'布別+布種的代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_FabricCode_QT', @level2type = N'COLUMN', @level2name = N'LectraCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_FabricCode_QT', @level2type = N'COLUMN', @level2name = N'SeqNO';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'QT布別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_FabricCode_QT', @level2type = N'COLUMN', @level2name = N'QTFabricCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'QT幅寬', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_FabricCode_QT', @level2type = N'COLUMN', @level2name = N'QTWidth';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_FabricCode_QT', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_FabricCode_QT', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_FabricCode_QT', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_FabricCode_QT', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'QT布別+布種的代碼',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Style_FabricCode_QT',
    @level2type = N'COLUMN',
    @level2name = N'QTLectraCode'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Pattern Panel',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Style_FabricCode_QT',
    @level2type = N'COLUMN',
    @level2name = N'PatternPanel'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'QT Pattern Panel',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Style_FabricCode_QT',
    @level2type = N'COLUMN',
    @level2name = N'QTPatternPanel'