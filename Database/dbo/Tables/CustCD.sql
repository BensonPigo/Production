CREATE TABLE [dbo].[CustCD] (
    [BrandID]                        VARCHAR (8)    CONSTRAINT [DF_CustCD_BrandID] DEFAULT ('') NOT NULL,
    [Junk]                           BIT            CONSTRAINT [DF_CustCD_Junk] DEFAULT ((0)) NOT NULL,
    [ID]                             VARCHAR (16)   CONSTRAINT [DF_CustCD_ID] DEFAULT ('') NOT NULL,
    [CountryID]                      VARCHAR (2)    CONSTRAINT [DF_CustCD_CountryID] DEFAULT ('') NOT NULL,
    [City]                           VARCHAR (16)   CONSTRAINT [DF_CustCD_City] DEFAULT ('') NOT NULL,
    [QuotaArea]                      VARCHAR (5)    CONSTRAINT [DF_CustCD_QuotaArea] DEFAULT ('') NOT NULL,
    [ScanAndPack]                    BIT            CONSTRAINT [DF_CustCD_ScanAndPack] DEFAULT ((0)) NOT NULL,
    [ZipperInsert]                   VARCHAR (5)    CONSTRAINT [DF_CustCD_ZipperInsert] DEFAULT ('') NOT NULL,
    [SpecialCust]                    BIT            CONSTRAINT [DF_CustCD_SpecialCust] DEFAULT ((0)) NOT NULL,
    [VasShas]                        BIT            CONSTRAINT [DF_CustCD_VasShas] DEFAULT ((0)) NOT NULL,
    [Guid]                           NVARCHAR (MAX) CONSTRAINT [DF_CustCD_Guid] DEFAULT ('') NOT NULL,
    [Factories]                      NVARCHAR (60)  CONSTRAINT [DF_CustCD_Factories] DEFAULT ('') NOT NULL,
    [PayTermARIDBulk]                VARCHAR (10)   CONSTRAINT [DF_CustCD_PayTermARIDBulk] DEFAULT ('') NOT NULL,
    [PayTermARIDSample]              VARCHAR (10)   CONSTRAINT [DF_CustCD_PayTermARIDSample] DEFAULT ('') NOT NULL,
    [ProformaInvoice]                BIT            CONSTRAINT [DF_CustCD_ProformaInvoice] DEFAULT ((0)) NOT NULL,
    [BankIDSample]                   VARCHAR (6)    CONSTRAINT [DF_CustCD_BankIDSample] DEFAULT ('') NOT NULL,
    [BankIDBulk]                     VARCHAR (6)    CONSTRAINT [DF_CustCD_BankIDBulk] DEFAULT ('') NOT NULL,
    [BrandLabel]                     NVARCHAR (MAX) CONSTRAINT [DF_CustCD_BrandLabel] DEFAULT ('') NOT NULL,
    [MarkFront]                      NVARCHAR (MAX) CONSTRAINT [DF_CustCD_MarkFront] DEFAULT ('') NOT NULL,
    [MarkBack]                       NVARCHAR (MAX) CONSTRAINT [DF_CustCD_MarkBack] DEFAULT ('') NOT NULL,
    [MarkLeft]                       NVARCHAR (MAX) CONSTRAINT [DF_CustCD_MarkLeft] DEFAULT ('') NOT NULL,
    [MarkRight]                      NVARCHAR (MAX) CONSTRAINT [DF_CustCD_MarkRight] DEFAULT ('') NOT NULL,
    [BillTo]                         NVARCHAR (MAX) CONSTRAINT [DF_CustCD_BillTo] DEFAULT ('') NOT NULL,
    [ShipTo]                         NVARCHAR (MAX) CONSTRAINT [DF_CustCD_ShipTo] DEFAULT ('') NOT NULL,
    [Consignee]                      NVARCHAR (MAX) CONSTRAINT [DF_CustCD_Consignee] DEFAULT ('') NOT NULL,
    [Notify]                         NVARCHAR (MAX) CONSTRAINT [DF_CustCD_Notify] DEFAULT ('') NOT NULL,
    [Anotify]                        NVARCHAR (MAX) CONSTRAINT [DF_CustCD_Anotify] DEFAULT ('') NOT NULL,
    [ShipRemark]                     NVARCHAR (MAX) CONSTRAINT [DF_CustCD_ShipRemark] DEFAULT ('') NOT NULL,
    [Dest]                           NVARCHAR (50)  CONSTRAINT [DF_CustCD_Dest] DEFAULT ('') NOT NULL,
    [AddName]                        VARCHAR (10)   CONSTRAINT [DF_CustCD_AddName] DEFAULT ('') NOT NULL,
    [AddDate]                        DATETIME       NULL,
    [EditName]                       VARCHAR (10)   CONSTRAINT [DF_CustCD_EditName] DEFAULT ('') NOT NULL,
    [EditDate]                       DATETIME       NULL,
    [Kit]                            VARCHAR (10)   CONSTRAINT [DF_CustCD_Kit] DEFAULT ('') NOT NULL,
    [BIRShipTo]                      VARCHAR (300)  CONSTRAINT [DF_CustCD_BIRShipTo] DEFAULT ('') NOT NULL,
    [DiamondCustCD]                  VARCHAR (10)   CONSTRAINT [DF_CustCD_DiamondCustCD] DEFAULT ('') NOT NULL,
    [DiamondCity]                    VARCHAR (100)  CONSTRAINT [DF_CustCD_DiamondCity] DEFAULT ('') NOT NULL,
    [StickerCombinationUkey]         BIGINT         CONSTRAINT [DF_CustCD_StickerCombinationUkey] DEFAULT ((0)) NOT NULL,
    [StickerCombinationUkey_MixPack] BIGINT         CONSTRAINT [DF_CustCD_StickerCombinationUkey_MixPack] DEFAULT ((0)) NOT NULL,
    [Need3rdInspect]                 BIT            DEFAULT ((0)) NOT NULL,
    [QAEditName]                     VARCHAR (10)   DEFAULT ('') NOT NULL,
    [QAEditDate]                     DATETIME       NULL,
    [StampCombinationUkey]           BIGINT         CONSTRAINT [DF_CustCD_StampCombinationUkey] DEFAULT ((0)) NOT NULL,
    [HealthID]                       VARCHAR (10)   CONSTRAINT [DF_CustCD_HealthID] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_CustCD] PRIMARY KEY CLUSTERED ([BrandID] ASC, [ID] ASC)
);








GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否成立Proforma Invoice', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CustCD', @level2type = N'COLUMN', @level2name = N'ProformaInvoice';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SCI-銀行(銷樣)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CustCD', @level2type = N'COLUMN', @level2name = N'BankIDSample';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SCI-銀行(大貨)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CustCD', @level2type = N'COLUMN', @level2name = N'BankIDBulk';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'商標說明', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CustCD', @level2type = N'COLUMN', @level2name = N'BrandLabel';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'大貨嘜頭(正面)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CustCD', @level2type = N'COLUMN', @level2name = N'MarkFront';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'大貨嘜頭(背面)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CustCD', @level2type = N'COLUMN', @level2name = N'MarkBack';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'大貨嘜頭(左面)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CustCD', @level2type = N'COLUMN', @level2name = N'MarkLeft';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'大貨嘜頭(右面)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CustCD', @level2type = N'COLUMN', @level2name = N'MarkRight';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'付款方資訊', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CustCD', @level2type = N'COLUMN', @level2name = N'BillTo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'運送地資訊', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CustCD', @level2type = N'COLUMN', @level2name = N'ShipTo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'收貨人資訊', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CustCD', @level2type = N'COLUMN', @level2name = N'Consignee';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'通知人資訊', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CustCD', @level2type = N'COLUMN', @level2name = N'Notify';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'第二通知人資訊', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CustCD', @level2type = N'COLUMN', @level2name = N'Anotify';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'船務註記', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CustCD', @level2type = N'COLUMN', @level2name = N'ShipRemark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'目的地', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CustCD', @level2type = N'COLUMN', @level2name = N'Dest';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CustCD', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CustCD', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CustCD', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CustCD', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Cust CD基本檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CustCD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'品牌代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CustCD', @level2type = N'COLUMN', @level2name = N'BrandID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'取消', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CustCD', @level2type = N'COLUMN', @level2name = N'Junk';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'客戶CD代碼( Cust CD)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CustCD', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'國別代碼(Ship 2 Country)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CustCD', @level2type = N'COLUMN', @level2name = N'CountryID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'城市(Customer)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CustCD', @level2type = N'COLUMN', @level2name = N'City';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Group/Quota', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CustCD', @level2type = N'COLUMN', @level2name = N'QuotaArea';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Scan & Pack', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CustCD', @level2type = N'COLUMN', @level2name = N'ScanAndPack';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'拉鏈左右拉', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CustCD', @level2type = N'COLUMN', @level2name = N'ZipperInsert';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Special Customer(自動轉單用)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CustCD', @level2type = N'COLUMN', @level2name = N'SpecialCust';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'VasShas', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CustCD', @level2type = N'COLUMN', @level2name = N'VasShas';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SHIPPING GUID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CustCD', @level2type = N'COLUMN', @level2name = N'Guid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Exclude Factories(排除工廠)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CustCD', @level2type = N'COLUMN', @level2name = N'Factories';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Payment Term for Bulk', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CustCD', @level2type = N'COLUMN', @level2name = N'PayTermARIDBulk';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Payment Term for Sample', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CustCD', @level2type = N'COLUMN', @level2name = N'PayTermARIDSample';
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單尺碼裝箱貼紙組合', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CustCD', @level2type = N'COLUMN', @level2name = N'StickerCombinationUkey';


GO
;
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'混尺碼裝箱貼紙組合', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CustCD', @level2type = N'COLUMN', @level2name = N'StickerCombinationUkey_MixPack';


GO
;
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'須要第三方檢驗', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CustCD', @level2type = N'COLUMN', @level2name = N'Need3rdInspect';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後編輯人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CustCD', @level2type = N'COLUMN', @level2name = N'QAEditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後編輯日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CustCD', @level2type = N'COLUMN', @level2name = N'QAEditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'噴碼組合', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CustCD', @level2type = N'COLUMN', @level2name = N'StampCombinationUkey';


Go
