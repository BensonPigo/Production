CREATE TABLE [dbo].[Factory] (
    [ID]                   VARCHAR (8)    CONSTRAINT [DF_Factory_ID] DEFAULT ('') NOT NULL,
    [MDivisionID]          VARCHAR (8)    CONSTRAINT [DF_Factory_MDivisionID] DEFAULT ('') NOT NULL,
    [Junk]                 BIT            CONSTRAINT [DF_Factory_Junk] DEFAULT ((0)) NOT NULL,
    [Abb]                  NVARCHAR (10)  CONSTRAINT [DF_Factory_Abb] DEFAULT ('') NOT NULL,
    [NameCH]               NVARCHAR (40)  CONSTRAINT [DF_Factory_NameCH] DEFAULT ('') NOT NULL,
    [NameEN]               NVARCHAR (80)  CONSTRAINT [DF_Factory_NameEN] DEFAULT ('') NOT NULL,
    [CountryID]            VARCHAR (2)    CONSTRAINT [DF_Factory_CountryID] DEFAULT ('') NOT NULL,
    [Tel]                  VARCHAR (30)   CONSTRAINT [DF_Factory_Tel] DEFAULT ('') NOT NULL,
    [Fax]                  VARCHAR (30)   CONSTRAINT [DF_Factory_Fax] DEFAULT ('') NOT NULL,
    [AddressCH]            NVARCHAR (50)  CONSTRAINT [DF_Factory_AddressCH] DEFAULT ('') NOT NULL,
    [AddressEN]            NVARCHAR (MAX) CONSTRAINT [DF_Factory_AddressEN] DEFAULT ('') NOT NULL,
    [CurrencyID]           VARCHAR (3)    CONSTRAINT [DF_Factory_CurrencyID] DEFAULT ('') NOT NULL,
    [CPU]                  INT            CONSTRAINT [DF_Factory_CPU] DEFAULT ((0)) NOT NULL,
    [ZipCode]              VARCHAR (6)    CONSTRAINT [DF_Factory_ZipCode] DEFAULT ('') NOT NULL,
    [PortSea]              VARCHAR (20)   CONSTRAINT [DF_Factory_PortSea] DEFAULT ('') NOT NULL,
    [PortAir]              VARCHAR (20)   CONSTRAINT [DF_Factory_PortAir] DEFAULT ('') NOT NULL,
    [KitId]                VARCHAR (8)    CONSTRAINT [DF_Factory_KitId] DEFAULT ('') NOT NULL,
    [ExpressGroup]         VARCHAR (8)    CONSTRAINT [DF_Factory_ExpressGroup] DEFAULT ('') NOT NULL,
    [IECode]               VARCHAR (1)    CONSTRAINT [DF_Factory_IECode] DEFAULT ('') NOT NULL,
    [NegoRegion]           VARCHAR (3)    CONSTRAINT [DF_Factory_NegoRegion] DEFAULT ('') NOT NULL,
    [AddName]              VARCHAR (10)   CONSTRAINT [DF_Factory_AddName] DEFAULT ('') NOT NULL,
    [AddDate]              DATETIME       NULL,
    [EditName]             VARCHAR (10)   CONSTRAINT [DF_Factory_EditName] DEFAULT ('') NOT NULL,
    [EditDate]             DATETIME       NULL,
    [KPICode]              VARCHAR (8)    CONSTRAINT [DF_Factory_KPICode] DEFAULT ('') NOT NULL,
    [FTYGroup]             VARCHAR (3)    CONSTRAINT [DF_Factory_FTYGroup] DEFAULT ('') NOT NULL,
    [KeyWord]              VARCHAR (3)    CONSTRAINT [DF_Factory_KeyWord] DEFAULT ('') NOT NULL,
    [TINNo]                VARCHAR (15)   CONSTRAINT [DF_Factory_TINNo] DEFAULT ('') NOT NULL,
    [VATAccNo]             VARCHAR (8)    CONSTRAINT [DF_Factory_VATAccNo] DEFAULT ('') NOT NULL,
    [WithholdingRateAccNo] VARCHAR (8)    CONSTRAINT [DF_Factory_WithholdingRateAccNo] DEFAULT ('') NOT NULL,
    [CreditBankAccNo]      VARCHAR (8)    CONSTRAINT [DF_Factory_CreditBankAccNo] DEFAULT ('') NOT NULL,
    [Manager]              VARCHAR (10)   CONSTRAINT [DF_Factory_Manager] DEFAULT ('') NOT NULL,
    [UseAPS]               BIT            CONSTRAINT [DF_Factory_UseAPS] DEFAULT ((0)) NOT NULL,
    [UseSBTS]              BIT            CONSTRAINT [DF_Factory_UseSBTS] DEFAULT ((0)) NOT NULL,
    [CheckDeclare]         BIT            CONSTRAINT [DF_Factory_CheckDeclare] DEFAULT ((0)) NOT NULL,
    [LocalCMT]             BIT            CONSTRAINT [DF_Factory_LocalCMT] DEFAULT ((0)) NOT NULL,
    [Type]                 VARCHAR (1)    CONSTRAINT [DF_Factory_Type] DEFAULT ('') NOT NULL,
    [Zone]                 VARCHAR (6)    CONSTRAINT [DF_Factory_Zone] DEFAULT ('') NOT NULL,
    [FactorySort]          VARCHAR (3)    CONSTRAINT [DF_Factory_FactorySort] DEFAULT ('') NOT NULL,
    [IsSampleRoom]         BIT            CONSTRAINT [DF_Factory_IsSampleRoom] DEFAULT ((0)) NOT NULL,
    [IsSCI]                BIT            CONSTRAINT [DF_Factory_IsSCI] DEFAULT ((0)) NOT NULL,
    [IsProduceFty]         BIT            CONSTRAINT [DF_Factory_IsProduceFty] DEFAULT ((0)) NOT NULL,
    [TestDocFactoryGroup]  VARCHAR (8)    CONSTRAINT [DF_Factory_TestDocFactoryGroup] DEFAULT ('') NOT NULL,
    [IsOriginalFty]        BIT            CONSTRAINT [DF_Factory_IsOriginalFty] DEFAULT ((0)) NOT NULL,
    [LastDownloadAPSDate]  DATETIME       NULL,
    [FtyZone]              VARCHAR (8)    CONSTRAINT [DF_Factory_FtyZone] DEFAULT ('') NOT NULL,
    [Foundry]              BIT            CONSTRAINT [DF_Factory_Foundry] DEFAULT ((0)) NOT NULL,
    [ProduceM]             VARCHAR (8)    CONSTRAINT [DF_Factory_ProduceM] DEFAULT ('') NOT NULL,
    [LoadingFactoryGroup]  VARCHAR (8)    CONSTRAINT [DF_Factory_LoadingFactoryGroup] DEFAULT ('') NOT NULL,
    [PadPrintGroup]        VARCHAR (3)    DEFAULT ('') NOT NULL,
    [IsSubcon]             BIT            DEFAULT ((0)) NOT NULL,
    [IsECFA]               BIT            CONSTRAINT [DF_Factory_IsECFA] DEFAULT ((0)) NOT NULL,
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


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'實際生產工廠的 MDivision', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Factory', @level2type = N'COLUMN', @level2name = N'ProduceM';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'loading歸屬於原工廠別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Factory', @level2type = N'COLUMN', @level2name = N'LoadingFactoryGroup';




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'PadPrintGroup', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Factory', @level2type = N'COLUMN', @level2name = N'PadPrintGroup';


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'是否為外發工廠',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Factory',
    @level2type = N'COLUMN',
    @level2name = N'IsSubcon'