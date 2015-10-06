CREATE TABLE [dbo].[Style_FabricCode_Article] (
    [StyleUkey]     BIGINT       CONSTRAINT [DF_Style_FabricCode_Article_StyleUkey] DEFAULT ((0)) NOT NULL,
    [Style_BOFUkey] BIGINT       CONSTRAINT [DF_Style_FabricCode_Article_Style_BOFUkey] DEFAULT ((0)) NOT NULL,
    [LectraCode]    VARCHAR (2)  CONSTRAINT [DF_Style_FabricCode_Article_LectraCode] DEFAULT ('') NOT NULL,
    [Article]       VARCHAR (8)  CONSTRAINT [DF_Style_FabricCode_Article_Article] DEFAULT ('') NOT NULL,
    [AddName]       VARCHAR (10) CONSTRAINT [DF_Style_FabricCode_Article_AddName] DEFAULT ('') NULL,
    [AddDate]       DATETIME     NULL,
    [EditName]      VARCHAR (10) CONSTRAINT [DF_Style_FabricCode_Article_EditName] DEFAULT ('') NULL,
    [EditDate]      DATETIME     NULL,
    CONSTRAINT [PK_Style_FabricCode_Article] PRIMARY KEY CLUSTERED ([Style_BOFUkey] ASC, [LectraCode] ASC, [Article] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fabric Code', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_FabricCode_Article';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'款式的唯一值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_FabricCode_Article', @level2type = N'COLUMN', @level2name = N'StyleUkey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_FabricCode_Article', @level2type = N'COLUMN', @level2name = N'Style_BOFUkey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'布別+部位的代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_FabricCode_Article', @level2type = N'COLUMN', @level2name = N'LectraCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_FabricCode_Article', @level2type = N'COLUMN', @level2name = N'Article';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_FabricCode_Article', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_FabricCode_Article', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_FabricCode_Article', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_FabricCode_Article', @level2type = N'COLUMN', @level2name = N'EditDate';

