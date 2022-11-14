CREATE TABLE [dbo].[Style_ColorCombo] (
    [StyleUkey]       BIGINT       CONSTRAINT [DF_Style_ColorCombo_StyleUkey] DEFAULT ((0)) NOT NULL,
    [Article]         VARCHAR (8)  CONSTRAINT [DF_Style_ColorCombo_Article] DEFAULT ('') NOT NULL,
    [ColorID]         VARCHAR (6)  CONSTRAINT [DF_Style_ColorCombo_ColorID] DEFAULT ('') NULL,
    [FabricCode]      VARCHAR (3)  CONSTRAINT [DF_Style_ColorCombo_FabricCode] DEFAULT ('') NULL,
    [FabricPanelCode] VARCHAR (2)  CONSTRAINT [DF_Style_ColorCombo_FabricPanelCode] DEFAULT ('') NOT NULL,
    [PatternPanel]    VARCHAR (2)  CONSTRAINT [DF_Style_ColorCombo_PatternPanel] DEFAULT ('') NULL,
    [AddName]         VARCHAR (10) CONSTRAINT [DF_Style_ColorCombo_AddName] DEFAULT ('') NULL,
    [AddDate]         DATETIME     NULL,
    [EditName]        VARCHAR (10) CONSTRAINT [DF_Style_ColorCombo_EditName] DEFAULT ('') NULL,
    [EditDate]        DATETIME     NULL,
    [FabricType]      VARCHAR (1)  DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_Style_ColorCombo] PRIMARY KEY CLUSTERED ([StyleUkey] ASC, [Article] ASC, [FabricPanelCode] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'款式基本檔- 配色表 Color Comb. (主料+副料-配色表)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_ColorCombo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'款式的唯一值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_ColorCombo', @level2type = N'COLUMN', @level2name = N'StyleUkey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'顏色組', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_ColorCombo', @level2type = N'COLUMN', @level2name = N'Article';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'顏色代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_ColorCombo', @level2type = N'COLUMN', @level2name = N'ColorID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'布種', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_ColorCombo', @level2type = N'COLUMN', @level2name = N'FabricCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'布別+部位的代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_ColorCombo', @level2type = N'COLUMN', @level2name = N'FabricPanelCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'部位別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_ColorCombo', @level2type = N'COLUMN', @level2name = N'PatternPanel';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_ColorCombo', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_ColorCombo', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_ColorCombo', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_ColorCombo', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fabric Type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_ColorCombo', @level2type = N'COLUMN', @level2name = N'FabricType';

