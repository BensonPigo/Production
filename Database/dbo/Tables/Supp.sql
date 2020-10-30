CREATE TABLE [dbo].[Supp] (
    [ID]           VARCHAR (6)    CONSTRAINT [DF_Supp_ID] DEFAULT ('') NOT NULL,
    [Junk]         BIT            CONSTRAINT [DF_Supp_Junk] DEFAULT ((0)) NULL,
    [AbbCH]        NVARCHAR (12)  CONSTRAINT [DF_Supp_AbbCH] DEFAULT ('') NULL,
    [AbbEN]        VARCHAR (12)   CONSTRAINT [DF_Supp_AbbEN] DEFAULT ('') NULL,
    [NameCH]       NVARCHAR (60)  CONSTRAINT [DF_Supp_NameCH] DEFAULT ('') NULL,
    [NameEN]       NVARCHAR (60)  CONSTRAINT [DF_Supp_NameEN] DEFAULT ('') NULL,
    [CountryID]    VARCHAR (2)    CONSTRAINT [DF_Supp_CountryID] DEFAULT ('') NULL,
    [ThirdCountry] BIT            CONSTRAINT [DF_Supp_ThirdCountry] DEFAULT ((0)) NULL,
    [Tel]          VARCHAR (30)   CONSTRAINT [DF_Supp_Tel] DEFAULT ('') NULL,
    [Fax]          VARCHAR (30)   CONSTRAINT [DF_Supp_Fax] DEFAULT ('') NULL,
    [AddressCH]    NVARCHAR (50)  CONSTRAINT [DF_Supp_AddressCH] DEFAULT ('') NULL,
    [AddressEN]    NVARCHAR (MAX) CONSTRAINT [DF_Supp_AddressEN] DEFAULT ('') NULL,
    [ZipCode]      VARCHAR (10)    CONSTRAINT [DF_Supp_ZipCode] DEFAULT ('') NULL,
    [Delay]        DATE           NULL,
    [DelayMemo]    NVARCHAR (MAX) CONSTRAINT [DF_Supp_DelayMemo] DEFAULT ('') NULL,
    [LockDate]     DATE           NULL,
    [LockMemo]     NVARCHAR (MAX) CONSTRAINT [DF_Supp_LockMemo] DEFAULT ('') NULL,
    [AddName]      VARCHAR (10)   CONSTRAINT [DF_Supp_AddName] DEFAULT ('') NULL,
    [AddDate]      DATETIME       NULL,
    [EditName]     VARCHAR (10)   CONSTRAINT [DF_Supp_EditName] DEFAULT ('') NULL,
    [EditDate]     DATETIME       NULL,
    [Currencyid]   VARCHAR (3)    NULL,
    CONSTRAINT [PK_Supp] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Supplier', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Supp';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'供應商代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Supp', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'取消', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Supp', @level2type = N'COLUMN', @level2name = N'Junk';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'簡稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Supp', @level2type = N'COLUMN', @level2name = N'AbbCH';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'英文簡稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Supp', @level2type = N'COLUMN', @level2name = N'AbbEN';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'中文全名', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Supp', @level2type = N'COLUMN', @level2name = N'NameCH';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'英文全名', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Supp', @level2type = N'COLUMN', @level2name = N'NameEN';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'國別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Supp', @level2type = N'COLUMN', @level2name = N'CountryID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'第三國廠商', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Supp', @level2type = N'COLUMN', @level2name = N'ThirdCountry';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'電話', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Supp', @level2type = N'COLUMN', @level2name = N'Tel';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'傳真', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Supp', @level2type = N'COLUMN', @level2name = N'Fax';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'中文地址', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Supp', @level2type = N'COLUMN', @level2name = N'AddressCH';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'英文地址', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Supp', @level2type = N'COLUMN', @level2name = N'AddressEN';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'郵遞區號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Supp', @level2type = N'COLUMN', @level2name = N'ZipCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否為延遲廠商', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Supp', @level2type = N'COLUMN', @level2name = N'Delay';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'延遲說明', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Supp', @level2type = N'COLUMN', @level2name = N'DelayMemo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'鎖碼日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Supp', @level2type = N'COLUMN', @level2name = N'LockDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'鎖碼說明', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Supp', @level2type = N'COLUMN', @level2name = N'LockMemo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Supp', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Supp', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Supp', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Supp', @level2type = N'COLUMN', @level2name = N'EditDate';

