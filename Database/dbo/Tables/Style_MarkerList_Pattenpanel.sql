CREATE TABLE [dbo].[Style_MarkerList_Pattenpanel] (
    [StyleUkey]            BIGINT       CONSTRAINT [DF_Style_MarkerList_Pattenpanel_StyleUkey] DEFAULT ((0)) NOT NULL,
    [Style_MarkerListUkey] BIGINT       CONSTRAINT [DF_Style_MarkerList_Pattenpanel_Style_MarkerListUkey] DEFAULT ((0)) NOT NULL,
    [PattenPanel]          VARCHAR (2)  CONSTRAINT [DF_Style_MarkerList_Pattenpanel_PattenPanel] DEFAULT ('') NOT NULL,
    [Lectracode]           VARCHAR (2)  CONSTRAINT [DF_Style_MarkerList_Pattenpanel_Lectracode] DEFAULT ('') NOT NULL,
    [AddName]              VARCHAR (10) CONSTRAINT [DF_Style_MarkerList_Pattenpanel_AddName] DEFAULT ('') NULL,
    [AddDate]              DATETIME     NULL,
    [EditName]             VARCHAR (10) CONSTRAINT [DF_Style_MarkerList_Pattenpanel_EditName] DEFAULT ('') NULL,
    [EditDate]             DATETIME     NULL,
    CONSTRAINT [PK_Style_MarkerList_Pattenpanel] PRIMARY KEY CLUSTERED ([Style_MarkerListUkey] ASC, [Lectracode] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Style_MarkerList_Pattenpanel', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_MarkerList_Pattenpanel';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'款式的唯一值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_MarkerList_Pattenpanel', @level2type = N'COLUMN', @level2name = N'StyleUkey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'馬克的唯一值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_MarkerList_Pattenpanel', @level2type = N'COLUMN', @level2name = N'Style_MarkerListUkey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'部位別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_MarkerList_Pattenpanel', @level2type = N'COLUMN', @level2name = N'PattenPanel';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'布別+布種的代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_MarkerList_Pattenpanel', @level2type = N'COLUMN', @level2name = N'Lectracode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_MarkerList_Pattenpanel', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_MarkerList_Pattenpanel', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_MarkerList_Pattenpanel', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_MarkerList_Pattenpanel', @level2type = N'COLUMN', @level2name = N'EditDate';

