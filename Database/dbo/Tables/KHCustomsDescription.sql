CREATE TABLE [dbo].[KHCustomsDescription] (
    [CDCCode]          VARCHAR (5)   NOT NULL,
    [CDCName]          VARCHAR (50)  NOT NULL,
    [CustomsType]      VARCHAR (10)  NOT NULL,
    [CDCUnit]          NVARCHAR (50) NOT NULL,
    [IsDeclareByNetKg] BIT           CONSTRAINT [DF_KHCustomsDescription_IsDeclareByNetKg] DEFAULT ((0)) NOT NULL,
    [Junk]             BIT           CONSTRAINT [DF_KHCustomsDescription_Junk] DEFAULT ((0)) NOT NULL,
    [AddName]          VARCHAR (10)  CONSTRAINT [DF_KHCustomsDescription_AddName] DEFAULT ('') NULL,
    [AddDate]          DATETIME      NULL,
    [EditName]         VARCHAR (10)  CONSTRAINT [DF_KHCustomsDescription_EditName] DEFAULT ('') NULL,
    [EditDate]         DATETIME      NULL,
    CONSTRAINT [PK_KHCustomsDescription_1] PRIMARY KEY CLUSTERED ([CDCName] ASC)
);










GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHCustomsDescription', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHCustomsDescription', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHCustomsDescription', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHCustomsDescription', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Junk', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHCustomsDescription', @level2type = N'COLUMN', @level2name = N'Junk';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'海關單位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHCustomsDescription', @level2type = N'COLUMN', @level2name = N'CDCUnit';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'報關內容分類如下
1. Fabric
2. Accessory
3. Machine
4. Chemical', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHCustomsDescription', @level2type = N'COLUMN', @level2name = N'CustomsType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'海關物料大類', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHCustomsDescription', @level2type = N'COLUMN', @level2name = N'CDCName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'海關大類編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHCustomsDescription', @level2type = N'COLUMN', @level2name = N'CDCCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否根據N.W.報關', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'KHCustomsDescription', @level2type = N'COLUMN', @level2name = N'IsDeclareByNetKg';

