CREATE TABLE [dbo].[Style_MarkerList_PatternPanel] (
    [StyleUkey]            BIGINT       CONSTRAINT [DF_Style_MarkerList_PatternPanel_StyleUkey] DEFAULT ((0)) NOT NULL,
    [Style_MarkerListUkey] BIGINT       CONSTRAINT [DF_Style_MarkerList_PatternPanel_Style_MarkerListUkey] DEFAULT ((0)) NOT NULL,
    [PatternPanel]          VARCHAR (2)  CONSTRAINT [DF_Style_MarkerList_PatternPanel_PatternPanel] DEFAULT ('') NOT NULL,
    [Lectracode]           VARCHAR (2)  CONSTRAINT [DF_Style_MarkerList_PatternPanel_Lectracode] DEFAULT ('') NOT NULL,
    [AddName]              VARCHAR (10) CONSTRAINT [DF_Style_MarkerList_PatternPanel_AddName] DEFAULT ('') NULL,
    [AddDate]              DATETIME     NULL,
    [EditName]             VARCHAR (10) CONSTRAINT [DF_Style_MarkerList_PatternPanel_EditName] DEFAULT ('') NULL,
    [EditDate]             DATETIME     NULL,
    CONSTRAINT [PK_Style_MarkerList_PatternPanel] PRIMARY KEY CLUSTERED ([Style_MarkerListUkey] ASC, [Lectracode] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Style_MarkerList_PatternPanel', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_MarkerList_PatternPanel';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'款式的唯一值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_MarkerList_PatternPanel', @level2type = N'COLUMN', @level2name = N'StyleUkey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'馬克的唯一值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_MarkerList_PatternPanel', @level2type = N'COLUMN', @level2name = N'Style_MarkerListUkey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'部位別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_MarkerList_PatternPanel', @level2type = N'COLUMN', @level2name = N'PatternPanel';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'布別+布種的代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_MarkerList_PatternPanel', @level2type = N'COLUMN', @level2name = N'Lectracode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_MarkerList_PatternPanel', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_MarkerList_PatternPanel', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_MarkerList_PatternPanel', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_MarkerList_PatternPanel', @level2type = N'COLUMN', @level2name = N'EditDate';

