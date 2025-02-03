CREATE TABLE [dbo].[Export] (
    [ID]                  VARCHAR (13)    CONSTRAINT [DF_Export_ID] DEFAULT ('') NOT NULL,
    [ScheduleID]          VARCHAR (13)    CONSTRAINT [DF_Export_ScheduleID] DEFAULT ('') NOT NULL,
    [ScheduleDate]        DATE            NULL,
    [LoadDate]            DATE            NOT NULL,
    [CloseDate]           DATE            NOT NULL,
    [Etd]                 DATE            NOT NULL,
    [Eta]                 DATE            NOT NULL,
    [ExportCountry]       VARCHAR (2)     CONSTRAINT [DF_Export_ExportCountry] DEFAULT ('') NOT NULL,
    [ImportCountry]       VARCHAR (2)     CONSTRAINT [DF_Export_ImportCountry] DEFAULT ('') NOT NULL,
    [ExportPort]          VARCHAR (20)    CONSTRAINT [DF_Export_ExportPort] DEFAULT ('') NOT NULL,
    [ImportPort]          VARCHAR (20)    CONSTRAINT [DF_Export_ImportPort] DEFAULT ('') NOT NULL,
    [CYCFS]               VARCHAR (6)     CONSTRAINT [DF_Export_CYCFS] DEFAULT ('') NOT NULL,
    [ShipModeID]          VARCHAR (10)    CONSTRAINT [DF_Export_ShipModeID] DEFAULT ('') NOT NULL,
    [ShipmentTerm]        VARCHAR (5)     CONSTRAINT [DF_Export_ShipmentTerm] DEFAULT ('') NOT NULL,
    [FactoryID]           VARCHAR (8)     CONSTRAINT [DF_Export_FactoryID] DEFAULT ('') NOT NULL,
    [ShipMark]            VARCHAR (20)    CONSTRAINT [DF_Export_ShipMark] DEFAULT ('') NOT NULL,
    [ShipMarkDesc]        NVARCHAR (MAX)  CONSTRAINT [DF_Export_ShipMarkDesc] DEFAULT ('') NOT NULL,
    [Consignee]           VARCHAR (8)     CONSTRAINT [DF_Export_Consignee] DEFAULT ('') NOT NULL,
    [Handle]              VARCHAR (10)    CONSTRAINT [DF_Export_Handle] DEFAULT ('') NOT NULL,
    [Posting]             BIT             CONSTRAINT [DF_Export_Posting] DEFAULT ((0)) NOT NULL,
    [Payer]               VARCHAR (1)     CONSTRAINT [DF_Export_Payer] DEFAULT ('') NOT NULL,
    [CompanyID]           NUMERIC(2)     CONSTRAINT [DF_Export_CompanyID] DEFAULT ((0)) NOT NULL,
    [Confirm]             BIT             CONSTRAINT [DF_Export_Confirm] DEFAULT ((0)) NOT NULL,
    [LastEdit]            DATETIME        NULL,
    [Remark]              NVARCHAR (MAX)  CONSTRAINT [DF_Export_Remark] DEFAULT ('') NOT NULL,
    [Ecfa]                BIT             CONSTRAINT [DF_Export_Ecfa] DEFAULT ((0)) NOT NULL,
    [FormStatus]          VARCHAR (2)     CONSTRAINT [DF_Export_FormStatus] DEFAULT ('') NOT NULL,
    [Carrier]             VARCHAR (6)     CONSTRAINT [DF_Export_Carrier] DEFAULT ('') NOT NULL,
    [Forwarder]           VARCHAR (6)     CONSTRAINT [DF_Export_Forwarder] DEFAULT ('') NOT NULL,
    [Vessel]              NVARCHAR (60)   CONSTRAINT [DF_Export_Vessel] DEFAULT ('') NOT NULL,
    [ShipTo]              VARCHAR (30)    CONSTRAINT [DF_Export_ShipTo] DEFAULT ('') NOT NULL,
    [Sono]                VARCHAR (12)    CONSTRAINT [DF_Export_Sono] DEFAULT ('') NOT NULL,
    [Blno]                VARCHAR (20)    CONSTRAINT [DF_Export_Blno] DEFAULT ('') NOT NULL,
    [InvNo]               VARCHAR (25)    CONSTRAINT [DF_Export_InvNo] DEFAULT ('') NOT NULL,
    [Exchange]            NUMERIC (5, 3)  CONSTRAINT [DF_Export_Exchange] DEFAULT ((0)) NOT NULL,
    [Packages]            DECIMAL (5)     CONSTRAINT [DF_Export_Packages] DEFAULT ((0)) NOT NULL,
    [WeightKg]            DECIMAL (10, 2) CONSTRAINT [DF_Export_WeightKg] DEFAULT ((0)) NOT NULL,
    [NetKg]               DECIMAL (10, 2) CONSTRAINT [DF_Export_NetKg] DEFAULT ((0)) NOT NULL,
    [Cbm]                 DECIMAL (10, 3) CONSTRAINT [DF_Export_Cbm] DEFAULT ((0)) NOT NULL,
    [CbmFor]              DECIMAL (10, 3) CONSTRAINT [DF_Export_CbmFor] DEFAULT ((0)) NOT NULL,
    [Takings]             DECIMAL (2)     CONSTRAINT [DF_Export_Takings] DEFAULT ((0)) NOT NULL,
    [TakingFee]           DECIMAL (10, 2) CONSTRAINT [DF_Export_TakingFee] DEFAULT ((0)) NOT NULL,
    [PackingArrival]      DATE            NULL,
    [WhseArrival]         DATE            NULL,
    [PortArrival]         DATE            NULL,
    [DocArrival]          DATE            NULL,
    [Broker]              VARCHAR (6)     CONSTRAINT [DF_Export_Broker] DEFAULT ('') NOT NULL,
    [Insurer]             VARCHAR (6)     CONSTRAINT [DF_Export_Insurer] DEFAULT ('') NOT NULL,
    [Trailer1]            VARCHAR (6)     CONSTRAINT [DF_Export_Trailer1] DEFAULT ('') NOT NULL,
    [Trailer2]            VARCHAR (6)     CONSTRAINT [DF_Export_Trailer2] DEFAULT ('') NOT NULL,
    [Freight]             DECIMAL (10, 2) CONSTRAINT [DF_Export_Freight] DEFAULT ((0)) NOT NULL,
    [Insurance]           DECIMAL (10, 2) CONSTRAINT [DF_Export_Insurance] DEFAULT ((0)) NOT NULL,
    [TPEEditName]         VARCHAR (10)    CONSTRAINT [DF_Export_TPEEditName] DEFAULT ('') NOT NULL,
    [TPEEditDate]         DATETIME        NULL,
    [Junk]                BIT             CONSTRAINT [DF_Export_Junk] DEFAULT ((0)) NOT NULL,
    [AddName]             VARCHAR (10)    CONSTRAINT [DF_Export_AddName] DEFAULT ('') NOT NULL,
    [AddDate]             DATETIME        NULL,
    [EditName]            VARCHAR (10)    CONSTRAINT [DF_Export_EditName] DEFAULT ('') NOT NULL,
    [EditDate]            DATETIME        NULL,
    [MainExportID]        VARCHAR (13)    CONSTRAINT [DF_Export_MainExportID] DEFAULT ('') NOT NULL,
    [Remark_Factory]      NVARCHAR (2000) CONSTRAINT [DF_Export_Remark_Factory] DEFAULT ('') NOT NULL,
    [NoImportCharges]     BIT             CONSTRAINT [DF_Export_NoImportCharges] DEFAULT ((0)) NOT NULL,
    [Replacement]         BIT             DEFAULT ((0)) NOT NULL,
    [Delay]               BIT             DEFAULT ((0)) NOT NULL,
    [PrepaidFtyImportFee] NUMERIC (10, 2) DEFAULT ((0)) NOT NULL,
    [MainExportID08]      VARCHAR (13)    DEFAULT ('') NOT NULL,
    [NonDeclare]          BIT             DEFAULT ((0)) NOT NULL,
    [FormE]               BIT             DEFAULT ((0)) NOT NULL,
    [SQCS]                BIT             DEFAULT ((0)) NOT NULL,
    [FtyTruckFee]         NUMERIC (10, 2) DEFAULT ((0)) NOT NULL,
    [FtyTrucker]          VARCHAR (6)     DEFAULT ('') NOT NULL,
    [CustomOT]            BIT             DEFAULT ((0)) NOT NULL,
    [CustomOTRespFty1]    VARCHAR (8)     DEFAULT ('') NOT NULL,
    [CustomOTRespFty2]    VARCHAR (8)     DEFAULT ('') NOT NULL,
    [OTFee]               NUMERIC (10, 2) DEFAULT ((0)) NOT NULL,
    [CIFTerms]            BIT             CONSTRAINT [DF_Export_CIFTerms] DEFAULT ((0)) NOT NULL,
    [FtyDisburseSD]       VARCHAR (13)    CONSTRAINT [DF_Export_FtyDisburseSD] DEFAULT ('') NOT NULL,
    [MainWKID]            VARCHAR (13)    CONSTRAINT [DF_Export_MainWK] DEFAULT ('') NOT NULL,
    [MainWKID08]          VARCHAR (13)    DEFAULT ('') NOT NULL,
    [OrderCompanyID] NUMERIC(2)      CONSTRAINT [DF_Export_OrderCompanyID] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Export] PRIMARY KEY CLUSTERED ([ID] ASC)
);










GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'船務出貨檔表頭', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'WK NO(Wkyyyymmxxxxx)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'預估船期編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export', @level2type = N'COLUMN', @level2name = N'ScheduleID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'排船表日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export', @level2type = N'COLUMN', @level2name = N'ScheduleDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'上櫃日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export', @level2type = N'COLUMN', @level2name = N'LoadDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'結關日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export', @level2type = N'COLUMN', @level2name = N'CloseDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'裝船日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export', @level2type = N'COLUMN', @level2name = N'Etd';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'到達日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export', @level2type = N'COLUMN', @level2name = N'Eta';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'出口國別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export', @level2type = N'COLUMN', @level2name = N'ExportCountry';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'目的地', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export', @level2type = N'COLUMN', @level2name = N'ImportCountry';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'出口港口', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export', @level2type = N'COLUMN', @level2name = N'ExportPort';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'到達港口', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export', @level2type = N'COLUMN', @level2name = N'ImportPort';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'整櫃或散櫃', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export', @level2type = N'COLUMN', @level2name = N'CYCFS';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Shipping mode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export', @level2type = N'COLUMN', @level2name = N'ShipModeID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Shipment Term', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export', @level2type = N'COLUMN', @level2name = N'ShipmentTerm';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Factory', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export', @level2type = N'COLUMN', @level2name = N'FactoryID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Shipping Mark', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export', @level2type = N'COLUMN', @level2name = N'ShipMark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'嘜頭', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export', @level2type = N'COLUMN', @level2name = N'ShipMarkDesc';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'收件人', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export', @level2type = N'COLUMN', @level2name = N'Consignee';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'船務負責人', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export', @level2type = N'COLUMN', @level2name = N'Handle';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'補過帳', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export', @level2type = N'COLUMN', @level2name = N'Posting';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'付費者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export', @level2type = N'COLUMN', @level2name = N'Payer';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'公司別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export', @level2type = N'COLUMN', @level2name = N'CompanyID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'數量過入採購單', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export', @level2type = N'COLUMN', @level2name = N'Confirm';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改日期 for kpi', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export', @level2type = N'COLUMN', @level2name = N'LastEdit';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'此工作底稿下的採購項是否含有HSCODE品項', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export', @level2type = N'COLUMN', @level2name = N'Ecfa';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'formE & 多國拼裝辨識用', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export', @level2type = N'COLUMN', @level2name = N'FormStatus';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'船公司', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export', @level2type = N'COLUMN', @level2name = N'Carrier';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'FORWARDER代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export', @level2type = N'COLUMN', @level2name = N'Forwarder';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'船名', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export', @level2type = N'COLUMN', @level2name = N'Vessel';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'出貨地', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export', @level2type = N'COLUMN', @level2name = N'ShipTo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'S/O', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export', @level2type = N'COLUMN', @level2name = N'Sono';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'提單號碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export', @level2type = N'COLUMN', @level2name = N'Blno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'INVOICE NO.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export', @level2type = N'COLUMN', @level2name = N'InvNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'匯率', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export', @level2type = N'COLUMN', @level2name = N'Exchange';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'總件/箱數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export', @level2type = N'COLUMN', @level2name = N'Packages';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'毛重', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export', @level2type = N'COLUMN', @level2name = N'WeightKg';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'淨重', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export', @level2type = N'COLUMN', @level2name = N'NetKg';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'材積', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export', @level2type = N'COLUMN', @level2name = N'Cbm';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'WK屬於FOR的CY部分材積', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export', @level2type = N'COLUMN', @level2name = N'CbmFor';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'HK結帳時填入的提貨次數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export', @level2type = N'COLUMN', @level2name = N'Takings';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'提貨費用', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export', @level2type = N'COLUMN', @level2name = N'TakingFee';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Packing List 收到日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export', @level2type = N'COLUMN', @level2name = N'PackingArrival';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'到達W/H日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export', @level2type = N'COLUMN', @level2name = N'WhseArrival';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'到港日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export', @level2type = N'COLUMN', @level2name = N'PortArrival';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'文件收到日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export', @level2type = N'COLUMN', @level2name = N'DocArrival';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'報關行代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export', @level2type = N'COLUMN', @level2name = N'Broker';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'保險公司', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export', @level2type = N'COLUMN', @level2name = N'Insurer';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'拖車行1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export', @level2type = N'COLUMN', @level2name = N'Trailer1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'拖車行2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export', @level2type = N'COLUMN', @level2name = N'Trailer2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'運費', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export', @level2type = N'COLUMN', @level2name = N'Freight';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'保險費', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export', @level2type = N'COLUMN', @level2name = N'Insurance';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export', @level2type = N'COLUMN', @level2name = N'TPEEditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export', @level2type = N'COLUMN', @level2name = N'TPEEditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Cancel', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export', @level2type = N'COLUMN', @level2name = N'Junk';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'台北代墊工廠進口費用', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export', @level2type = N'COLUMN', @level2name = N'PrepaidFtyImportFee';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'代墊工廠進口費母單', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export', @level2type = N'COLUMN', @level2name = N'MainExportID08';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'廠商因為貨量較小，不安排海運或空運，而改成廠商付費快遞出貨。', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export', @level2type = N'COLUMN', @level2name = N'SQCS';


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'台北船務可能會合併不同Shipment Term 到同一筆WK#, 當Shipment Term非CIF時, 工廠務仍可由 "CIF terms by supplier" flag 辨識該 Shipment 可能為 CIF',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Export',
    @level2type = N'COLUMN',
    @level2name = N'CIFTerms'

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'台北代墊進口費母單單號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Export',
    @level2type = N'COLUMN',
    @level2name = N'MainWKID08'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單公司別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Export', @level2type = N'COLUMN', @level2name = N'OrderCompanyID';


GO
CREATE NONCLUSTERED INDEX [WHR21]
    ON [dbo].[Export]([Blno] ASC)
    INCLUDE([Packages]);

