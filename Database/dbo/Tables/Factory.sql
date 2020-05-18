CREATE TABLE [dbo].[Factory] (
    [ID]                   VARCHAR (8)    CONSTRAINT [DF_Factory_ID] DEFAULT ('') NOT NULL,
    [MDivisionID]          VARCHAR (8)    CONSTRAINT [DF_Factory_MDivisionID] DEFAULT ('') NULL,
    [Junk]                 BIT            CONSTRAINT [DF_Factory_Junk] DEFAULT ((0)) NULL,
    [Abb]                  NVARCHAR (10)  CONSTRAINT [DF_Factory_Abb] DEFAULT ('') NULL,
    [NameCH]               NVARCHAR (40)  CONSTRAINT [DF_Factory_NameCH] DEFAULT ('') NULL,
    [NameEN]               NVARCHAR (40)  CONSTRAINT [DF_Factory_NameEN] DEFAULT ('') NULL,
    [CountryID]            VARCHAR (2)    CONSTRAINT [DF_Factory_CountryID] DEFAULT ('') NULL,
    [Tel]                  VARCHAR (30)   CONSTRAINT [DF_Factory_Tel] DEFAULT ('') NULL,
    [Fax]                  VARCHAR (30)   CONSTRAINT [DF_Factory_Fax] DEFAULT ('') NULL,
    [AddressCH]            NVARCHAR (50)  CONSTRAINT [DF_Factory_AddressCH] DEFAULT ('') NULL,
    [AddressEN]            NVARCHAR (MAX) CONSTRAINT [DF_Factory_AddressEN] DEFAULT ('') NULL,
    [CurrencyID]           VARCHAR (3)    CONSTRAINT [DF_Factory_CurrencyID] DEFAULT ('') NULL,
    [CPU]                  INT            CONSTRAINT [DF_Factory_CPU] DEFAULT ((0)) NULL,
    [ZipCode]              VARCHAR (6)    CONSTRAINT [DF_Factory_ZipCode] DEFAULT ('') NULL,
    [PortSea]              VARCHAR (20)   CONSTRAINT [DF_Factory_PortSea] DEFAULT ('') NULL,
    [PortAir]              VARCHAR (20)   CONSTRAINT [DF_Factory_PortAir] DEFAULT ('') NULL,
    [KitId]                VARCHAR (8)    CONSTRAINT [DF_Factory_KitId] DEFAULT ('') NULL,
    [ExpressGroup]         VARCHAR (8)    CONSTRAINT [DF_Factory_ExpressGroup] DEFAULT ('') NULL,
    [IECode]               VARCHAR (1)    CONSTRAINT [DF_Factory_IECode] DEFAULT ('') NULL,
    [NegoRegion]           VARCHAR (3)    CONSTRAINT [DF_Factory_NegoRegion] DEFAULT ('') NULL,
    [AddName]              VARCHAR (10)   CONSTRAINT [DF_Factory_AddName] DEFAULT ('') NULL,
    [AddDate]              DATETIME       NULL,
    [EditName]             VARCHAR (10)   CONSTRAINT [DF_Factory_EditName] DEFAULT ('') NULL,
    [EditDate]             DATETIME       NULL,
    [KPICode]              VARCHAR (8)    CONSTRAINT [DF_Factory_KPICode] DEFAULT ('') NULL,
    [FTYGroup]             VARCHAR (3)    CONSTRAINT [DF_Factory_FTYGroup] DEFAULT ('') NULL,
    [KeyWord]              VARCHAR (3)    CONSTRAINT [DF_Factory_KeyWord] DEFAULT ('') NULL,
    [TINNo]                VARCHAR (15)   CONSTRAINT [DF_Factory_TINNo] DEFAULT ('') NULL,
    [VATAccNo]             VARCHAR (8)    CONSTRAINT [DF_Factory_VATAccNo] DEFAULT ('') NULL,
    [WithholdingRateAccNo] VARCHAR (8)    CONSTRAINT [DF_Factory_WithholdingRateAccNo] DEFAULT ('') NULL,
    [CreditBankAccNo]      VARCHAR (8)    CONSTRAINT [DF_Factory_CreditBankAccNo] DEFAULT ('') NULL,
    [Manager]              VARCHAR (10)   CONSTRAINT [DF_Factory_Manager] DEFAULT ('') NULL,
    [UseAPS]               BIT            CONSTRAINT [DF_Factory_UseAPS] DEFAULT ((0)) NULL,
    [UseSBTS]              BIT            CONSTRAINT [DF_Factory_UseSBTS] DEFAULT ((0)) NULL,
    [CheckDeclare]         BIT            CONSTRAINT [DF_Factory_CheckDeclare] DEFAULT ((0)) NULL,
    [LocalCMT]             BIT            CONSTRAINT [DF_Factory_LocalCMT] DEFAULT ((0)) NULL,
    [Type]                 VARCHAR (1)    CONSTRAINT [DF_Factory_Type] DEFAULT ('') NULL,
    [Zone]                 VARCHAR (6)    CONSTRAINT [DF_Factory_Zone] DEFAULT ('') NULL,
    [FactorySort]          VARCHAR (3)    CONSTRAINT [DF_Factory_FactorySort] DEFAULT ('') NULL,
    [IsSampleRoom]         BIT            CONSTRAINT [DF_Factory_IsSampleRoom] DEFAULT ((0)) NULL,
    [IsSCI]                BIT            DEFAULT ((0)) NULL,
    [IsProduceFty]         BIT            DEFAULT ((1)) NULL,
    [TestDocFactoryGroup]  VARCHAR (8)    CONSTRAINT [DF_Factory_TestDocFactoryGroup] DEFAULT ('') NULL,
    [IsOriginalFty]        BIT            DEFAULT ((0)) NULL,
    [LastDownloadAPSDate] DATETIME NULL, 
    [FtyZone] VARCHAR(8) NULL, 
    [Foundry] BIT NULL DEFAULT ((0)), 
    CONSTRAINT [PK_Factory] PRIMARY KEY CLUSTERED ([ID] ASC)
);






GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠基本資料', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Factory';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Factory', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'取消', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Factory', @level2type = N'COLUMN', @level2name = N'Junk';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠簡稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Factory', @level2type = N'COLUMN', @level2name = N'Abb';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Factory', @level2type = N'COLUMN', @level2name = N'NameCH';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'英文名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Factory', @level2type = N'COLUMN', @level2name = N'NameEN';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'國別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Factory', @level2type = N'COLUMN', @level2name = N'CountryID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'電話', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Factory', @level2type = N'COLUMN', @level2name = N'Tel';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'傳真', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Factory', @level2type = N'COLUMN', @level2name = N'Fax';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'中文地址', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Factory', @level2type = N'COLUMN', @level2name = N'AddressCH';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'英文地址', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Factory', @level2type = N'COLUMN', @level2name = N'AddressEN';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'幣別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Factory', @level2type = N'COLUMN', @level2name = N'CurrencyID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'月總產值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Factory', @level2type = N'COLUMN', @level2name = N'CPU';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'郵遞區號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Factory', @level2type = N'COLUMN', @level2name = N'ZipCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'PORT OF SEA(First sale)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Factory', @level2type = N'COLUMN', @level2name = N'PortSea';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Port of AIR (First AIR)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Factory', @level2type = N'COLUMN', @level2name = N'PortAir';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Production Kits group', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Factory', @level2type = N'COLUMN', @level2name = N'KitId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'國際快遞group', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Factory', @level2type = N'COLUMN', @level2name = N'ExpressGroup';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'IE operation 編碼代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Factory', @level2type = N'COLUMN', @level2name = N'IECode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Region Code用於Nego 產生', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Factory', @level2type = N'COLUMN', @level2name = N'NegoRegion';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Factory', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Factory', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Factory', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Factory', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠kpi統計群組', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Factory', @level2type = N'COLUMN', @level2name = N'KPICode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Factory Group', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Factory', @level2type = N'COLUMN', @level2name = N'FTYGroup';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Key Word', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Factory', @level2type = N'COLUMN', @level2name = N'KeyWord';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'稅籍編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Factory', @level2type = N'COLUMN', @level2name = N'TINNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'增值稅會計科目', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Factory', @level2type = N'COLUMN', @level2name = N'VATAccNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'預扣稅會計科目', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Factory', @level2type = N'COLUMN', @level2name = N'WithholdingRateAccNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'扣款銀行會計科目', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Factory', @level2type = N'COLUMN', @level2name = N'CreditBankAccNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'廠長', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Factory', @level2type = N'COLUMN', @level2name = N'Manager';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'使用APS系統', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Factory', @level2type = N'COLUMN', @level2name = N'UseAPS';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'使用Subcon Bundle Tracking System', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Factory', @level2type = N'COLUMN', @level2name = N'UseSBTS';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Garment booking amend時是否需要檢查出口報關申請', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Factory', @level2type = N'COLUMN', @level2name = N'CheckDeclare';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Factory CMT包含Local Purchase', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Factory', @level2type = N'COLUMN', @level2name = N'LocalCMT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Type :Bulk ,Sample ,MMS', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Factory', @level2type = N'COLUMN', @level2name = N'Type';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠地區別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Factory', @level2type = N'COLUMN', @level2name = N'Zone';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠排序碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Factory', @level2type = N'COLUMN', @level2name = N'FactorySort';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組織代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Factory', @level2type = N'COLUMN', @level2name = N'MDivisionID';

