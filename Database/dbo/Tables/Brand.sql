CREATE TABLE [dbo].[Brand] (
    [ID]                        VARCHAR (8)    CONSTRAINT [DF_Brand_ID] DEFAULT ('') NOT NULL,
    [NameCH]                    NVARCHAR (30)  CONSTRAINT [DF_Brand_NameCH] DEFAULT ('') NULL,
    [NameEN]                    NVARCHAR (40)  CONSTRAINT [DF_Brand_NameEN] DEFAULT ('') NULL,
    [CountryID]                 VARCHAR (2)    CONSTRAINT [DF_Brand_CountryID] DEFAULT ('') NULL,
    [BuyerID]                   VARCHAR (8)    CONSTRAINT [DF_Brand_BuyerID] DEFAULT ('') NULL,
    [Tel]                       VARCHAR (20)   CONSTRAINT [DF_Brand_Tel] DEFAULT ('') NULL,
    [Fax]                       VARCHAR (20)   CONSTRAINT [DF_Brand_Fax] DEFAULT ('') NULL,
    [Contact1]                  VARCHAR (20)   CONSTRAINT [DF_Brand_Contact1] DEFAULT ('') NULL,
    [Contact2]                  VARCHAR (20)   CONSTRAINT [DF_Brand_Contact2] DEFAULT ('') NULL,
    [AddressCH]                 NVARCHAR (50)  CONSTRAINT [DF_Brand_AddressCH] DEFAULT ('') NULL,
    [AddressEN]                 NVARCHAR (MAX) CONSTRAINT [DF_Brand_AddressEN] DEFAULT ('') NULL,
    [CurrencyID]                VARCHAR (3)    CONSTRAINT [DF_Brand_CurrencyID] DEFAULT ('') NULL,
    [Remark]                    NVARCHAR (MAX) CONSTRAINT [DF_Brand_Remark] DEFAULT ('') NULL,
    [Customize1]                VARCHAR (12)   CONSTRAINT [DF_Brand_Customize1] DEFAULT ('') NULL,
    [Customize2]                VARCHAR (12)   CONSTRAINT [DF_Brand_Customize2] DEFAULT ('') NULL,
    [Customize3]                VARCHAR (12)   CONSTRAINT [DF_Brand_Customize3] DEFAULT ('') NULL,
    [Commission]                SMALLINT       CONSTRAINT [DF_Brand_Commission] DEFAULT ((0)) NULL,
    [ZipCode]                   VARCHAR (6)    CONSTRAINT [DF_Brand_ZipCode] DEFAULT ('') NULL,
    [Email]                     NVARCHAR (50)  CONSTRAINT [DF_Brand_Email] DEFAULT ('') NULL,
    [MrTeam]                    VARCHAR (5)    CONSTRAINT [DF_Brand_MrTeam] DEFAULT ('') NULL,
    [BrandGroup]                VARCHAR (8)    CONSTRAINT [DF_Brand_BrandGroup] DEFAULT ('') NULL,
    [ApparelXlt]                VARCHAR (20)   CONSTRAINT [DF_Brand_ApparelXlt] DEFAULT ('') NULL,
    [LossSampleFabric]          NUMERIC (3, 1) CONSTRAINT [DF_Brand_LossSampleFabric] DEFAULT ((0)) NULL,
    [PayTermARIDBulk]           VARCHAR (10)    CONSTRAINT [DF_Brand_PayTermARIDBulk] DEFAULT ('') NULL,
    [PayTermARIDSample]         VARCHAR (10)    CONSTRAINT [DF_Brand_PayTermARIDSample] DEFAULT ('') NULL,
    [BrandFactoryAreaCaption]   VARCHAR (12)   CONSTRAINT [DF_Brand_BrandFactoryAreaCaption] DEFAULT ('') NULL,
    [BrandFactoryCodeCaption]   VARCHAR (12)   CONSTRAINT [DF_Brand_BrandFactoryCodeCaption] DEFAULT ('') NULL,
    [BrandFactoryVendorCaption] VARCHAR (12)   CONSTRAINT [DF_Brand_BrandFactoryVendorCaption] DEFAULT ('') NULL,
    [ShipCode]                  VARCHAR (3)    CONSTRAINT [DF_Brand_ShipCode] DEFAULT ('') NULL,
    [Junk]                      BIT            CONSTRAINT [DF_Brand_Junk] DEFAULT ((0)) NULL,
    [AddName]                   VARCHAR (10)   CONSTRAINT [DF_Brand_AddName] DEFAULT ('') NULL,
    [AddDate]                   DATETIME       NULL,
    [EditName]                  VARCHAR (10)   CONSTRAINT [DF_Brand_EditName] DEFAULT ('') NULL,
    [EditDate]                  DATETIME       NULL,
    [LossSampleAccessory]       NUMERIC (3, 1) NULL,
    [OTDExtension] INT NULL DEFAULT ((0)), 
    [UseRatioRule] VARCHAR CONSTRAINT [DF_Brand_UseRatioRule] DEFAULT ('') NULL, 
    [UseRatioRule_Thick] VARCHAR CONSTRAINT [DF_Brand_UseRatioRule_Thick] DEFAULT ('') NULL, 
    [Serial] TINYINT not NULL CONSTRAINT [DF_Brand_Serial] DEFAULT ((0)), 
    CONSTRAINT [PK_Brand] PRIMARY KEY CLUSTERED ([ID] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Brand', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Brand';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'客戶代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Brand', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'中文全名', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Brand', @level2type = N'COLUMN', @level2name = N'NameCH';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'英文全名', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Brand', @level2type = N'COLUMN', @level2name = N'NameEN';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'國別代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Brand', @level2type = N'COLUMN', @level2name = N'CountryID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'客戶', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Brand', @level2type = N'COLUMN', @level2name = N'BuyerID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'電話', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Brand', @level2type = N'COLUMN', @level2name = N'Tel';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'傳真', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Brand', @level2type = N'COLUMN', @level2name = N'Fax';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'聯絡人1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Brand', @level2type = N'COLUMN', @level2name = N'Contact1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'聯絡人2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Brand', @level2type = N'COLUMN', @level2name = N'Contact2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'中文地址', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Brand', @level2type = N'COLUMN', @level2name = N'AddressCH';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'英文地址', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Brand', @level2type = N'COLUMN', @level2name = N'AddressEN';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'交易幣別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Brand', @level2type = N'COLUMN', @level2name = N'CurrencyID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Brand', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單上的自訂欄位1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Brand', @level2type = N'COLUMN', @level2name = N'Customize1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單上的自訂欄位2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Brand', @level2type = N'COLUMN', @level2name = N'Customize2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單上的自訂欄位3', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Brand', @level2type = N'COLUMN', @level2name = N'Customize3';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'佣金%', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Brand', @level2type = N'COLUMN', @level2name = N'Commission';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'郵遞區號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Brand', @level2type = N'COLUMN', @level2name = N'ZipCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'E_Mail Address', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Brand', @level2type = N'COLUMN', @level2name = N'Email';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'業務組別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Brand', @level2type = N'COLUMN', @level2name = N'MrTeam';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Brand Group (用於Color,Frabic,Inventory)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Brand', @level2type = N'COLUMN', @level2name = N'BrandGroup';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Apparel 範本名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Brand', @level2type = N'COLUMN', @level2name = N'ApparelXlt';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Sample 耗損%(Fabric)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Brand', @level2type = N'COLUMN', @level2name = N'LossSampleFabric';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Payment Term for Bulk', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Brand', @level2type = N'COLUMN', @level2name = N'PayTermARIDBulk';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Payment Term for Sample', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Brand', @level2type = N'COLUMN', @level2name = N'PayTermARIDSample';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠AreaCode名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Brand', @level2type = N'COLUMN', @level2name = N'BrandFactoryAreaCaption';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠FactoryCode名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Brand', @level2type = N'COLUMN', @level2name = N'BrandFactoryCodeCaption';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠VendorCode名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Brand', @level2type = N'COLUMN', @level2name = N'BrandFactoryVendorCaption';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'大貨出貨三碼代碼(用於Invoice)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Brand', @level2type = N'COLUMN', @level2name = N'ShipCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'取消', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Brand', @level2type = N'COLUMN', @level2name = N'Junk';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Brand', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Brand', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Brand', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Brand', @level2type = N'COLUMN', @level2name = N'EditDate';

