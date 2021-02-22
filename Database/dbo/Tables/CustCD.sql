CREATE TABLE [dbo].[CustCD] (
    [BrandID]                        VARCHAR (8)    CONSTRAINT [DF_CustCD_BrandID] DEFAULT ('') NOT NULL,
    [Junk]                           BIT            CONSTRAINT [DF_CustCD_Junk] DEFAULT ((0)) NULL,
    [ID]                             VARCHAR (16)   CONSTRAINT [DF_CustCD_ID] DEFAULT ('') NOT NULL,
    [CountryID]                      VARCHAR (2)    CONSTRAINT [DF_CustCD_CountryID] DEFAULT ('') NULL,
    [City]                           VARCHAR (16)   CONSTRAINT [DF_CustCD_City] DEFAULT ('') NULL,
    [QuotaArea]                      VARCHAR (5)    CONSTRAINT [DF_CustCD_QuotaArea] DEFAULT ('') NULL,
    [ScanAndPack]                    BIT            CONSTRAINT [DF_CustCD_ScanAndPack] DEFAULT ((0)) NULL,
    [ZipperInsert]                   VARCHAR (5)    CONSTRAINT [DF_CustCD_ZipperInsert] DEFAULT ('') NULL,
    [SpecialCust]                    BIT            CONSTRAINT [DF_CustCD_SpecialCust] DEFAULT ((0)) NULL,
    [VasShas]                        BIT            CONSTRAINT [DF_CustCD_VasShas] DEFAULT ((0)) NULL,
    [Guid]                           NVARCHAR (MAX) CONSTRAINT [DF_CustCD_Guid] DEFAULT ('') NULL,
    [Factories]                      NVARCHAR (60)  CONSTRAINT [DF_CustCD_Factories] DEFAULT ('') NULL,
    [PayTermARIDBulk]                VARCHAR (10)   CONSTRAINT [DF_CustCD_PayTermARIDBulk] DEFAULT ('') NULL,
    [PayTermARIDSample]              VARCHAR (10)   CONSTRAINT [DF_CustCD_PayTermARIDSample] DEFAULT ('') NULL,
    [ProformaInvoice]                BIT            CONSTRAINT [DF_CustCD_ProformaInvoice] DEFAULT ((0)) NULL,
    [BankIDSample]                   VARCHAR (6)    CONSTRAINT [DF_CustCD_BankIDSample] DEFAULT ('') NULL,
    [BankIDBulk]                     VARCHAR (6)    CONSTRAINT [DF_CustCD_BankIDBulk] DEFAULT ('') NULL,
    [BrandLabel]                     NVARCHAR (MAX) CONSTRAINT [DF_CustCD_BrandLabel] DEFAULT ('') NULL,
    [MarkFront]                      NVARCHAR (MAX) CONSTRAINT [DF_CustCD_MarkFront] DEFAULT ('') NULL,
    [MarkBack]                       NVARCHAR (MAX) CONSTRAINT [DF_CustCD_MarkBack] DEFAULT ('') NULL,
    [MarkLeft]                       NVARCHAR (MAX) CONSTRAINT [DF_CustCD_MarkLeft] DEFAULT ('') NULL,
    [MarkRight]                      NVARCHAR (MAX) CONSTRAINT [DF_CustCD_MarkRight] DEFAULT ('') NULL,
    [BillTo]                         NVARCHAR (MAX) CONSTRAINT [DF_CustCD_BillTo] DEFAULT ('') NULL,
    [ShipTo]                         NVARCHAR (MAX) CONSTRAINT [DF_CustCD_ShipTo] DEFAULT ('') NULL,
    [Consignee]                      NVARCHAR (MAX) CONSTRAINT [DF_CustCD_Consignee] DEFAULT ('') NULL,
    [Notify]                         NVARCHAR (MAX) CONSTRAINT [DF_CustCD_Notify] DEFAULT ('') NULL,
    [Anotify]                        NVARCHAR (MAX) CONSTRAINT [DF_CustCD_Anotify] DEFAULT ('') NULL,
    [ShipRemark]                     NVARCHAR (MAX) CONSTRAINT [DF_CustCD_ShipRemark] DEFAULT ('') NULL,
    [Dest]                           NVARCHAR (50)  CONSTRAINT [DF_CustCD_Dest] DEFAULT ('') NULL,
    [AddName]                        VARCHAR (10)   CONSTRAINT [DF_CustCD_AddName] DEFAULT ('') NULL,
    [AddDate]                        DATETIME       NULL,
    [EditName]                       VARCHAR (10)   CONSTRAINT [DF_CustCD_EditName] DEFAULT ('') NULL,
    [EditDate]                       DATETIME       NULL,
    [Kit]                            VARCHAR (6)    DEFAULT ('') NULL,
    [BIRShipTo]                      VARCHAR (300)  NULL,
    [DiamondCustCD]                  VARCHAR (10)   DEFAULT ('') NULL,
    [DiamondCity]                    VARCHAR (100)  DEFAULT ('') NULL,
    [StickerCombinationUkey]         BIGINT         NULL,
    [StickerCombinationUkey_MixPack] BIGINT         NULL,
    [Need3rdInspect]                 BIT            DEFAULT ((0)) NOT NULL,
    [QAEditName]                     VARCHAR (10)   DEFAULT ('') NOT NULL,
    [QAEditDate]                     DATETIME       NULL,
    [StampCombinationUkey]           BIGINT         NULL,
    [HealthID]                       VARCHAR (10)   NULL,
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

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'��ؽX�˽c�K�ȲզX', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CustCD', @level2type = N'COLUMN', @level2name = N'StickerCombinationUkey';


GO
;
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�V�ؽX�˽c�K�ȲզX', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CustCD', @level2type = N'COLUMN', @level2name = N'StickerCombinationUkey_MixPack';


GO
;
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'���n�ĤT������', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CustCD', @level2type = N'COLUMN', @level2name = N'Need3rdInspect';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'QA �̫�s���', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CustCD', @level2type = N'COLUMN', @level2name = N'QAEditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'QA �̫�s��H��', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CustCD', @level2type = N'COLUMN', @level2name = N'QAEditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�Q�X�զX', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CustCD', @level2type = N'COLUMN', @level2name = N'StampCombinationUkey';


Go
