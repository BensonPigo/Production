CREATE TABLE [dbo].[TransferExport]
(
	[ID] VARCHAR(13) NOT NULL, 
    [Eta] DATE NULL, 
	[Etd] DATE NULL, 
    [ExportCountry] VARCHAR(2) NOT NULL CONSTRAINT [DF_TransferExport_ExportCountry] DEFAULT (''), 
    [ImportCountry] VARCHAR(2) NOT NULL CONSTRAINT [DF_TransferExport_ImportCountry] DEFAULT (''), 
    [ExportPort] VARCHAR(20) NOT NULL CONSTRAINT [DF_TransferExport_ExportPort] DEFAULT (''), 
    [ImportPort] VARCHAR(20) NOT NULL CONSTRAINT [DF_TransferExport_ImportPort] DEFAULT (''), 
    [CYCFS] VARCHAR(6) NOT NULL CONSTRAINT [DF_TransferExport_CYCFS] DEFAULT (''), 
    [ShipModeID] VARCHAR(10) NOT NULL CONSTRAINT [DF_TransferExport_ShipModeID] DEFAULT (''), 
    [ShipmentTerm] varchar(5) NOT NULL CONSTRAINT [DF_TransferExport_ShipmentTerm] DEFAULT (''), 
    [FromFactoryID] VARCHAR(8) NOT NULL CONSTRAINT [DF_TransferExport_FromFactoryID] DEFAULT (''),  
    [FactoryID] VARCHAR(8) NOT NULL CONSTRAINT [DF_TransferExport_FactoryID] DEFAULT (''),  
    [ShipMark] VARCHAR(20) NOT NULL CONSTRAINT [DF_TransferExport_ShipMark] DEFAULT (''), 
    [ShipMarkDesc] NVARCHAR(MAX) NOT NULL CONSTRAINT [DF_TransferExport_ShipMarkDesc] DEFAULT (''), 
    [Consignee] VARCHAR(8) NOT NULL CONSTRAINT [DF_TransferExport_Consignee] DEFAULT (''), 
    [Handle] VARCHAR(10) NOT NULL CONSTRAINT [DF_TransferExport_Handle] DEFAULT (''), 
    [Payer] VARCHAR NOT NULL CONSTRAINT [DF_TransferExport_Payer] DEFAULT (''), 
    [CompanyID] NUMERIC(2) NOT NULL CONSTRAINT [DF_TransferExport_CompanyID] DEFAULT ((0)), 
    [Confirm] BIT NOT NULL CONSTRAINT [DF_TransferExport_Confirm] DEFAULT ((0)), 
    [ConfirmTime] DATETIME NULL,
    [Remark] NVARCHAR(MAX) NOT NULL CONSTRAINT [DF_TransferExport_Remark] DEFAULT (''),  
    [Ecfa] BIT NOT NULL CONSTRAINT [DF_TransferExport_Ecfa] DEFAULT ((0)), 
    [FormStatus] VARCHAR(2) NOT NULL CONSTRAINT [DF_TransferExport_FormStatus] DEFAULT (''), 
    [Carrier] VARCHAR(6) NOT NULL CONSTRAINT [DF_TransferExport_Carrier] DEFAULT (''), 
    [Forwarder] VARCHAR(6) NOT NULL CONSTRAINT [DF_TransferExport_Forwarder] DEFAULT (''), 
    [Vessel] NVARCHAR(30) NOT NULL CONSTRAINT [DF_TransferExport_Vessel] DEFAULT (''), 
    [ShipTo] VARCHAR(30) NOT NULL CONSTRAINT [DF_TransferExport_ShipTo] DEFAULT (''), 
    [Sono] VARCHAR(12) NOT NULL CONSTRAINT [DF_TransferExport_Sono] DEFAULT (''), 
    [Blno] VARCHAR(20) NOT NULL CONSTRAINT [DF_TransferExport_Blno] DEFAULT (''), 
    [InvNo] VARCHAR(25) NOT NULL CONSTRAINT [DF_TransferExport_InvNo] DEFAULT (''), 
    [Packages] NUMERIC(5) NOT NULL CONSTRAINT [DF_TransferExport_Packages] DEFAULT ((0)), 
    [WeightKg] NUMERIC(10, 2) NOT NULL CONSTRAINT [DF_TransferExport_WeightKg] DEFAULT ((0)), 
    [NetKg] NUMERIC(10, 2) NOT NULL CONSTRAINT [DF_TransferExport_NetKg] DEFAULT ((0)), 
    [Cbm] NUMERIC(10, 3) NOT NULL CONSTRAINT [DF_TransferExport_Cbm] DEFAULT ((0)), 
    [CbmFor] NUMERIC(10, 3) NOT NULL CONSTRAINT [DF_TransferExport_CbmFor] DEFAULT ((0)), 
    [PackingArrival] DATE NULL,
    [WhseArrival] DATE NULL, 
    [PortArrival] DATE NULL, 
    [DocArrival] DATE NULL, 
    [AddName] VARCHAR(10) NOT NULL CONSTRAINT [DF_TransferExport_AddName] DEFAULT (''),  
    [AddDate] DATETIME NULL, 
    [EditName] VARCHAR(10) NOT NULL CONSTRAINT [DF_TransferExport_EditName] DEFAULT (''), 
    [EditDate] DATETIME NULL, 
    [Junk] BIT NOT NULL CONSTRAINT [DF_TransferExport_Junk] DEFAULT ((0)), 
    [Airline] NVARCHAR(30) NOT NULL CONSTRAINT [DF_TransferExport_Airline] DEFAULT (''), 
    [Remark_Shipping] NVARCHAR(2000) NOT NULL CONSTRAINT [DF_TransferExport_Remark_Shipping] DEFAULT (''), 
	[EntryNo] nvarchar(40) NOT NULL CONSTRAINT [DF_TransferExport_EntryNo] DEFAULT (''), 
    [SLT] BIT NOT NULL CONSTRAINT [DF_TransferExport_SLT] DEFAULT ((0)), 
    [NoImportCharges] BIT NOT NULL CONSTRAINT [DF_TransferExport_NoImportCharges] DEFAULT ((0)), 
    [Replacement] BIT NOT NULL CONSTRAINT [DF_TransferExport_Replacement] DEFAULT ((0)), 
    [Delay] BIT NOT NULL CONSTRAINT [DF_TransferExport_Delay] DEFAULT ((0)), 
	[PrepaidFtyImportFee] Numeric (10, 2) NOT NULL CONSTRAINT [DF_TransferExport_PrepaidFtyImportFee] DEFAULT ((0)), 
    [FormE] BIT NOT NULL CONSTRAINT [DF_TransferExport_FormE] DEFAULT ((0)), 
    [SQCS] BIT NOT NULL CONSTRAINT [DF_TransferExport_SQCS] DEFAULT ((0)), 
    [OTFee] NUMERIC(10, 2) NOT NULL CONSTRAINT [DF_TransferExport_OTFee] DEFAULT ((0)), 
    [FtyOT] BIT NOT NULL CONSTRAINT [DF_TransferExport_FtyOT] DEFAULT ((0)), 
    [OTResponsibleFty1] VARCHAR(8) NOT NULL CONSTRAINT [DF_TransferExport_OTResponsibleFty1] DEFAULT (''), 
    [OTResponsibleFty2] VARCHAR(8) NOT NULL CONSTRAINT [DF_TransferExport_OTResponsibleFty2] DEFAULT (''), 
    [CIFTerms] BIT NOT NULL CONSTRAINT [DF_TransferExport_CIFTerms] DEFAULT ((0)), 
    [Sent] BIT NOT NULL CONSTRAINT [DF_TransferExport_Sent] DEFAULT ((0)), 
    [FtyStatus] VARCHAR(30) NOT NULL CONSTRAINT [DF_TransferExport_FtyStatus] DEFAULT (''), 
    [NoCharges] BIT NOT NULL CONSTRAINT [DF_TransferExport_NoCharges] DEFAULT ((0)), 
	[FtyTruckFee] numeric(10,2) NOT NULL CONSTRAINT [DF_TransferExport_FtyTruckFee] DEFAULT ((0)), 
	[MainWKID] varchar(13) NOT NULL CONSTRAINT [DF_TransferExport_MainWKID] DEFAULT ('') ,
	[CloseDate] date NULL, 
	[LoadDate] date NULL, 
    [NoExportCharge] BIT NOT NULL DEFAULT ((0)), 
    [TransferType] VARCHAR(12) NOT NULL DEFAULT (''), 
    [FtySendDate] DATETIME NULL, 
    [FtyConfirmDate] DATETIME NULL, 
    [Separated] BIT NOT NULL CONSTRAINT [DF_TransferExport_Separated] DEFAULT ((0)), 
    [FtyRequestSeparateDate] DATETIME NULL, 
    [TPESeparateApprovedDate] DATETIME NULL, 
    [WHSpearateConfirmDate] DATETIME NULL, 
    [ShippingSeparateConfirmDate] DATETIME NULL, 
    [Status] VARCHAR(20) NOT NULL CONSTRAINT [DF_TransferExport_Status] DEFAULT (''), 
    [Remark_Factory] NVARCHAR(2000) NOT NULL CONSTRAINT [DF_TransferExport_Remark_Factory] DEFAULT (''), 
    [OrderCompanyID]   NUMERIC(2, 0)    CONSTRAINT [DF_LocalDebit_OrderCompanyID] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_TransferExport] PRIMARY KEY CLUSTERED ([ID] ASC)
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'預計抵達日',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport',
    @level2type = N'COLUMN',
    @level2name = N'Eta'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'出口國',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport',
    @level2type = N'COLUMN',
    @level2name = N'ExportCountry'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'進口國',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport',
    @level2type = N'COLUMN',
    @level2name = N'ImportCountry'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'出口港口',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport',
    @level2type = N'COLUMN',
    @level2name = N'ExportPort'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'進口港口',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport',
    @level2type = N'COLUMN',
    @level2name = N'ImportPort'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'整櫃/散櫃',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport',
    @level2type = N'COLUMN',
    @level2name = N'CYCFS'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'運送方式',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport',
    @level2type = N'COLUMN',
    @level2name = N'ShipModeID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'付款條件',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport',
    @level2type = N'COLUMN',
    @level2name = N'ShipmentTerm'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'來源工廠',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport',
    @level2type = N'COLUMN',
    @level2name = N'FromFactoryID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'目的地工廠',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport',
    @level2type = N'COLUMN',
    @level2name = N'FactoryID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'嘜頭',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport',
    @level2type = N'COLUMN',
    @level2name = N'ShipMark'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'嘜頭描述',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport',
    @level2type = N'COLUMN',
    @level2name = N'ShipMarkDesc'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'收件人',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport',
    @level2type = N'COLUMN',
    @level2name = N'Consignee'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'船務負責人',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport',
    @level2type = N'COLUMN',
    @level2name = N'Handle'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'付款人',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport',
    @level2type = N'COLUMN',
    @level2name = N'Payer'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'公司',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport',
    @level2type = N'COLUMN',
    @level2name = N'CompanyID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'是否過帳',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport',
    @level2type = N'COLUMN',
    @level2name = N'Confirm'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'過帳時間',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport',
    @level2type = N'COLUMN',
    @level2name = N'ConfirmTime'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'備註',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport',
    @level2type = N'COLUMN',
    @level2name = N'Remark'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'是否為ECFA',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport',
    @level2type = N'COLUMN',
    @level2name = N'Ecfa'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'單據種類',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport',
    @level2type = N'COLUMN',
    @level2name = N'FormStatus'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'船公司',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport',
    @level2type = N'COLUMN',
    @level2name = N'Carrier'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'運送者',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport',
    @level2type = N'COLUMN',
    @level2name = N'Forwarder'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'船名',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport',
    @level2type = N'COLUMN',
    @level2name = N'Vessel'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'報單號碼',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport',
    @level2type = N'COLUMN',
    @level2name = N'Sono'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'提單號碼',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport',
    @level2type = N'COLUMN',
    @level2name = N'Blno'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'件數',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport',
    @level2type = N'COLUMN',
    @level2name = N'Packages'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'總重量',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport',
    @level2type = N'COLUMN',
    @level2name = N'WeightKg'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'淨重量',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport',
    @level2type = N'COLUMN',
    @level2name = N'NetKg'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'航空公司',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport',
    @level2type = N'COLUMN',
    @level2name = N'Airline'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'船務備註',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport',
    @level2type = N'COLUMN',
    @level2name = N'Remark_Shipping'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'是否短交期',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport',
    @level2type = N'COLUMN',
    @level2name = N'SLT'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'沒有進口費用',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport',
    @level2type = N'COLUMN',
    @level2name = N'NoImportCharges'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'工廠加班費',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport',
    @level2type = N'COLUMN',
    @level2name = N'OTFee'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'是否有工廠加班費',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport',
    @level2type = N'COLUMN',
    @level2name = N'FtyOT'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'加班費責任歸屬1',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport',
    @level2type = N'COLUMN',
    @level2name = N'OTResponsibleFty1'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'加班費責任歸屬2',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport',
    @level2type = N'COLUMN',
    @level2name = N'OTResponsibleFty2'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'是否送出',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport',
    @level2type = N'COLUMN',
    @level2name = N'Sent'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'台北 TK 的狀態(New, Sent, Request Separate, Separate Approved, Separate Reject, Fty Confirm, Confirm, Close, Junk)',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport',
    @level2type = N'COLUMN',
    @level2name = N'FtyStatus'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'沒有出口費用',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport',
    @level2type = N'COLUMN',
    @level2name = N'NoExportCharge'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'確認該單據屬於轉出還是轉入方 Transfer Out/Transfer In',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport',
    @level2type = N'COLUMN',
    @level2name = N'TransferType'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'工廠按下 Send 日期',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport',
    @level2type = N'COLUMN',
    @level2name = N'FtySendDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'工廠按下 Confirm 日期',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport',
    @level2type = N'COLUMN',
    @level2name = N'FtyConfirmDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'該單據是否曾經有過成功拆單',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport',
    @level2type = N'COLUMN',
    @level2name = N'Separated'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'工廠請求拆分 TK 的日期',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport',
    @level2type = N'COLUMN',
    @level2name = N'FtyRequestSeparateDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'台北核准工廠拆分 TK 的請求',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport',
    @level2type = N'COLUMN',
    @level2name = N'TPESeparateApprovedDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'工廠倉庫確認拆分後的結果',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport',
    @level2type = N'COLUMN',
    @level2name = N'WHSpearateConfirmDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'工廠船務確認拆分後的結果',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport',
    @level2type = N'COLUMN',
    @level2name = N'ShippingSeparateConfirmDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'台北 TK 的狀態(New, Sent, Request Separate, Separate Approved, Separate Reject, Fty Confirm, Confirm, Close, Junk)',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport',
    @level2type = N'COLUMN',
    @level2name = N'Status'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'工廠備註',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferExport',
    @level2type = N'COLUMN',
    @level2name = N'Remark_Factory'
GO	
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單公司別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TransferExport', @level2type = N'COLUMN', @level2name = N'OrderCompanyID';


GO