CREATE TABLE [dbo].[SCIFty] (
    [ID]           VARCHAR (8)    CONSTRAINT [DF_SCIFty_ID] DEFAULT ('') NOT NULL,
    [Junk]         BIT            CONSTRAINT [DF_SCIFty_Junk] DEFAULT ((0)) NULL,
    [Abb]          NVARCHAR (10)  CONSTRAINT [DF_SCIFty_Abb] DEFAULT ('') NULL,
    [NameCH]       NVARCHAR (40)  CONSTRAINT [DF_SCIFty_NameCH] DEFAULT ('') NULL,
    [NameEN]       NVARCHAR (40)  CONSTRAINT [DF_SCIFty_NameEN] DEFAULT ('') NULL,
    [CountryID]    VARCHAR (2)    CONSTRAINT [DF_SCIFty_CountryID] DEFAULT ('') NULL,
    [Tel]          VARCHAR (30)   CONSTRAINT [DF_SCIFty_Tel] DEFAULT ('') NULL,
    [Fax]          VARCHAR (30)   CONSTRAINT [DF_SCIFty_Fax] DEFAULT ('') NULL,
    [AddressCH]    NVARCHAR (50)  CONSTRAINT [DF_SCIFty_AddressCH] DEFAULT ('') NULL,
    [AddressEN]    NVARCHAR (MAX) CONSTRAINT [DF_SCIFty_AddressEN] DEFAULT ('') NULL,
    [CurrencyID]   VARCHAR (3)    CONSTRAINT [DF_SCIFty_CurrencyID] DEFAULT ('') NULL,
    [ExpressGroup] VARCHAR (8)    CONSTRAINT [DF_SCIFty_ExpressGroup] DEFAULT ('') NULL,
    [PortAir]      VARCHAR (20)   CONSTRAINT [DF_SCIFty_PortAir] DEFAULT ('') NULL,
    [MDivisionID]  VARCHAR (8)    CONSTRAINT [DF_SCIFty_MDivisionID] DEFAULT ('') NULL,
    [AddName]      VARCHAR (10)   CONSTRAINT [DF_SCIFty_AddName] DEFAULT ('') NULL,
    [AddDate]      DATETIME       NULL,
    [EditName]     VARCHAR (10)   CONSTRAINT [DF_SCIFty_EditName] DEFAULT ('') NULL,
    [EditDate]     DATETIME       NULL,
    [Type]         VARCHAR (1)    DEFAULT ('') NOT NULL,
    [Zone]         VARCHAR (6)    DEFAULT ('') NULL,
    [FtyZone] VARCHAR(8) NULL, 
    CONSTRAINT [PK_SCIFty] PRIMARY KEY CLUSTERED ([ID] ASC)
);








GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SCI工廠基本資料', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SCIFty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SCIFty', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'取消', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SCIFty', @level2type = N'COLUMN', @level2name = N'Junk';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠簡稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SCIFty', @level2type = N'COLUMN', @level2name = N'Abb';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SCIFty', @level2type = N'COLUMN', @level2name = N'NameCH';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'英文名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SCIFty', @level2type = N'COLUMN', @level2name = N'NameEN';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'國別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SCIFty', @level2type = N'COLUMN', @level2name = N'CountryID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'電話', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SCIFty', @level2type = N'COLUMN', @level2name = N'Tel';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'傳真', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SCIFty', @level2type = N'COLUMN', @level2name = N'Fax';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'中文地址', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SCIFty', @level2type = N'COLUMN', @level2name = N'AddressCH';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'英文地址', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SCIFty', @level2type = N'COLUMN', @level2name = N'AddressEN';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'幣別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SCIFty', @level2type = N'COLUMN', @level2name = N'CurrencyID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SCIFty', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SCIFty', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SCIFty', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SCIFty', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠地區別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SCIFty', @level2type = N'COLUMN', @level2name = N'Zone';

