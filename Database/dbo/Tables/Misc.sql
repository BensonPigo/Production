CREATE TABLE [dbo].[Misc] (
    [ID]           VARCHAR (23)    CONSTRAINT [DF_Misc_ID] DEFAULT ('') NOT NULL,
    [Model]        VARCHAR (50)    CONSTRAINT [DF_Misc_Model] DEFAULT ('') NULL,
    [MiscBrandID]  VARCHAR (10)    CONSTRAINT [DF_Misc_MiscBrandID] DEFAULT ('') NULL,
    [Description]  NVARCHAR (MAX)  CONSTRAINT [DF_Misc_Description] DEFAULT ('') NULL,
    [UnitID]       VARCHAR (8)     CONSTRAINT [DF_Misc_UnitID] DEFAULT ('') NULL,
    [CurrencyID]   VARCHAR (3)     CONSTRAINT [DF_Misc_CurrencyID] DEFAULT ('') NULL,
    [Price]        NUMERIC (12, 4) CONSTRAINT [DF_Misc_Price] DEFAULT ((0)) NULL,
    [Suppid]       VARCHAR (6)     CONSTRAINT [DF_Misc_Suppid] DEFAULT ('') NULL,
    [PutchaseFrom] VARCHAR (1)     CONSTRAINT [DF_Misc_PutchaseFrom] DEFAULT ('') NULL,
    [Inspect]      BIT             CONSTRAINT [DF_Misc_Inspect] DEFAULT ((0)) NULL,
    [IsMachine]    BIT             CONSTRAINT [DF_Misc_IsMachine] DEFAULT ((0)) NULL,
    [IsAsset]      BIT             CONSTRAINT [DF_Misc_IsAsset] DEFAULT ((0)) NULL,
    [PurchaseType] VARCHAR (20)    CONSTRAINT [DF_Misc_PurchaseType] DEFAULT ('') NULL,
    [Remark]       NVARCHAR (60)   CONSTRAINT [DF_Misc_Remark] DEFAULT ('') NULL,
    [PIC]          VARCHAR (100)   CONSTRAINT [DF_Misc_PIC] DEFAULT ('') NULL,
    [AddName]      VARCHAR (10)    CONSTRAINT [DF_Misc_AddName] DEFAULT ('') NULL,
    [AddDate]      DATETIME        NULL,
    [EditName]     VARCHAR (10)    CONSTRAINT [DF_Misc_EditName] DEFAULT ('') NULL,
    [EditDate]     DATETIME        NULL,
    [InspLeadTime] NUMERIC (2)     CONSTRAINT [DF_Misc_InspLeadTime] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_Misc] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Miscellaneous', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Misc';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'雜項編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Misc', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'模組名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Misc', @level2type = N'COLUMN', @level2name = N'Model';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'廠牌', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Misc', @level2type = N'COLUMN', @level2name = N'MiscBrandID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'描述', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Misc', @level2type = N'COLUMN', @level2name = N'Description';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'採購單位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Misc', @level2type = N'COLUMN', @level2name = N'UnitID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'幣別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Misc', @level2type = N'COLUMN', @level2name = N'CurrencyID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單價', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Misc', @level2type = N'COLUMN', @level2name = N'Price';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'廠商', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Misc', @level2type = N'COLUMN', @level2name = N'Suppid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'採購來源', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Misc', @level2type = N'COLUMN', @level2name = N'PutchaseFrom';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否檢驗', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Misc', @level2type = N'COLUMN', @level2name = N'Inspect';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否為機台', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Misc', @level2type = N'COLUMN', @level2name = N'IsMachine';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否為資產', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Misc', @level2type = N'COLUMN', @level2name = N'IsAsset';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'種類', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Misc', @level2type = N'COLUMN', @level2name = N'PurchaseType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Misc', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'圖片名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Misc', @level2type = N'COLUMN', @level2name = N'PIC';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Misc', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Misc', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後編輯人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Misc', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後編輯日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Misc', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'檢驗LeadTime', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Misc', @level2type = N'COLUMN', @level2name = N'InspLeadTime';

