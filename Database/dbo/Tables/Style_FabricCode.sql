CREATE TABLE [dbo].[Style_FabricCode] (
    [StyleUkey]       BIGINT         CONSTRAINT [DF_Style_FabricCode_StyleUkey] DEFAULT ((0)) NOT NULL,
    [FabricPanelCode] VARCHAR (2)    CONSTRAINT [DF_Style_FabricCode_FabricPanelCode] DEFAULT ('') NOT NULL,
    [FabricCode]      VARCHAR (3)    CONSTRAINT [DF_Style_FabricCode_FabricCode] DEFAULT ('') NOT NULL,
    [PatternPanel]    VARCHAR (2)    CONSTRAINT [DF_Style_FabricCode_PatternPanel] DEFAULT ('') NOT NULL,
    [AddName]         VARCHAR (10)   CONSTRAINT [DF_Style_FabricCode_AddName] DEFAULT ('') NULL,
    [AddDate]         DATETIME       NULL,
    [EditName]        VARCHAR (10)   CONSTRAINT [DF_Style_FabricCode_EditName] DEFAULT ('') NULL,
    [EditDate]        DATETIME       NULL,
    [QTWidth]         NUMERIC (3, 1) DEFAULT ((0)) NULL,
    CONSTRAINT [PK_Style_FabricCode] PRIMARY KEY CLUSTERED ([StyleUkey] ASC, [FabricPanelCode] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fabric Code', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_FabricCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'款式的唯一值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_FabricCode', @level2type = N'COLUMN', @level2name = N'StyleUkey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'布別+部位的代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_FabricCode', @level2type = N'COLUMN', @level2name = N'FabricPanelCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'布種', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_FabricCode', @level2type = N'COLUMN', @level2name = N'FabricCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'部位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_FabricCode', @level2type = N'COLUMN', @level2name = N'PatternPanel';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_FabricCode', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_FabricCode', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_FabricCode', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_FabricCode', @level2type = N'COLUMN', @level2name = N'EditDate';

