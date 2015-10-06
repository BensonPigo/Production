CREATE TABLE [dbo].[Style_BOF_Shell] (
    [StyleUkey]  BIGINT       CONSTRAINT [DF_Style_BOF_Shell_StyleUkey] DEFAULT ((0)) NOT NULL,
    [FabricCode] VARCHAR (3)  CONSTRAINT [DF_Style_BOF_Shell_FabricCode] DEFAULT ('') NOT NULL,
    [Refno]      VARCHAR (20) CONSTRAINT [DF_Style_BOF_Shell_Refno] DEFAULT ('') NOT NULL,
    [AddName]    VARCHAR (10) CONSTRAINT [DF_Style_BOF_Shell_AddName] DEFAULT ('') NULL,
    [AddDate]    DATETIME     NULL,
    [EditName]   VARCHAR (10) CONSTRAINT [DF_Style_BOF_Shell_EditName] DEFAULT ('') NULL,
    [EditDate]   DATETIME     NULL,
    CONSTRAINT [PK_Style_BOF_Shell] PRIMARY KEY CLUSTERED ([StyleUkey] ASC, [Refno] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Style - Bill of Fabric Shell', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_BOF_Shell';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'款式的唯一值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_BOF_Shell', @level2type = N'COLUMN', @level2name = N'StyleUkey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'布種', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_BOF_Shell', @level2type = N'COLUMN', @level2name = N'FabricCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'物料編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_BOF_Shell', @level2type = N'COLUMN', @level2name = N'Refno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_BOF_Shell', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_BOF_Shell', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_BOF_Shell', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_BOF_Shell', @level2type = N'COLUMN', @level2name = N'EditDate';

