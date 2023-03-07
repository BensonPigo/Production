CREATE TABLE [dbo].[Style_BOA_Location] (
    [StyleUkey]     BIGINT       CONSTRAINT [DF_Style_BOA_Location_StyleUkey] DEFAULT ((0)) NOT NULL,
    [Style_BOAUkey] BIGINT       NOT NULL,
    [Location]      VARCHAR (1)  NOT NULL,
    [AddName]       VARCHAR (10) CONSTRAINT [DF_Style_BOA_Location_AddName] DEFAULT ('') NOT NULL,
    [AddDate]       DATETIME     NULL,
    [EditName]      VARCHAR (10) CONSTRAINT [DF_Style_BOA_Location_EditName] DEFAULT ('') NOT NULL,
    [EditDate]      DATETIME     NULL,
    CONSTRAINT [PK_Style_BOA_Location] PRIMARY KEY CLUSTERED ([Style_BOAUkey] ASC, [Location] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_BOA_Location', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_BOA_Location', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_BOA_Location', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_BOA_Location', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'款式的唯一值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_BOA_Location', @level2type = N'COLUMN', @level2name = N'StyleUkey';

