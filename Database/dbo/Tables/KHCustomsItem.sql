CREATE TABLE [dbo].[KHCustomsItem] (
    [Ukey]                        BIGINT         IDENTITY (1, 1) NOT NULL,
    [RefNo]                       VARCHAR (30)   NOT NULL,
    [KHCustomsDescriptionCDCName] VARCHAR (50)   CONSTRAINT [DF_KHCustomsItem_KHCustomsDescriptionID] DEFAULT ('') NULL,
    [CDCUnitPrice]                NUMERIC (9, 4) CONSTRAINT [DF_KHCustomsItem_CDCUnitPrice] DEFAULT ((0)) NULL,
    [Description]                 VARCHAR (MAX)  CONSTRAINT [DF_KHCustomsItem_Description] DEFAULT ('') NULL,
    [Junk]                        BIT            CONSTRAINT [DF_KHCustomsItem_Junk] DEFAULT ((0)) NULL,
    [AddName]                     VARCHAR (10)   CONSTRAINT [DF_KHCustomsItem_AddName] DEFAULT ('') NULL,
    [AddDate]                     DATETIME       NULL,
    [EditName]                    VARCHAR (10)   CONSTRAINT [DF_KHCustomsItem_EditName] DEFAULT ('') NULL,
    [EditDate]                    DATETIME       NULL,
    CONSTRAINT [PK_KHCustomsItem] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);








GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHCustomsItem', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHCustomsItem', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHCustomsItem', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHCustomsItem', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Junk', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHCustomsItem', @level2type = N'COLUMN', @level2name = N'Junk';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'物料描述', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHCustomsItem', @level2type = N'COLUMN', @level2name = N'Description';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'此料號對應到海關單位的單價(USD)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHCustomsItem', @level2type = N'COLUMN', @level2name = N'CDCUnitPrice';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'料號
CustomsType為
1. Fabric/Accessory: Production..Fabric.SCIRefno
2. Machine: Machine..Machine.ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHCustomsItem', @level2type = N'COLUMN', @level2name = N'RefNo';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'KHCustomsItem Ukey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHCustomsItem', @level2type = N'COLUMN', @level2name = N'Ukey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'海關大類編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHCustomsItem', @level2type = N'COLUMN', @level2name = N'KHCustomsDescriptionCDCName';

