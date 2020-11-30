CREATE TABLE [dbo].[FtyExport] (
    [ID]            VARCHAR (13)    CONSTRAINT [DF_FtyExport_ID] DEFAULT ('') NOT NULL,
    [Type]          TINYINT         CONSTRAINT [DF_FtyExport_Type] DEFAULT ((0)) NULL,
    [ExportCountry] VARCHAR (2)     CONSTRAINT [DF_FtyExport_ExportCountry] DEFAULT ('') NULL,
    [ImportCountry] VARCHAR (2)     CONSTRAINT [DF_FtyExport_ImportCountry] DEFAULT ('') NULL,
    [ExportPort]    VARCHAR (20)    CONSTRAINT [DF_FtyExport_ExportPort] DEFAULT ('') NULL,
    [ImportPort]    VARCHAR (20)    CONSTRAINT [DF_FtyExport_ImportPort] DEFAULT ('') NULL,
    [CYCFS]         VARCHAR (3)     CONSTRAINT [DF_FtyExport_CYCFS] DEFAULT ('') NULL,
    [ShipModeID]    VARCHAR (10)    CONSTRAINT [DF_FtyExport_ShipModeID] DEFAULT ('') NULL,
    [Consignee]     VARCHAR (8)     CONSTRAINT [DF_FtyExport_Consignee] DEFAULT ('') NULL,
    [Handle]        VARCHAR (10)    CONSTRAINT [DF_FtyExport_Handle] DEFAULT ('') NULL,
    [Forwarder]     VARCHAR (8)     CONSTRAINT [DF_FtyExport_Forwarder] DEFAULT ('') NULL,
    [Vessel]        NVARCHAR (30)   CONSTRAINT [DF_FtyExport_Vessel] DEFAULT ('') NULL,
    [Blno]          VARCHAR (20)    CONSTRAINT [DF_FtyExport_Blno] DEFAULT ('') NULL,
    [INVNo]         VARCHAR (25)    CONSTRAINT [DF_FtyExport_INVNo] DEFAULT ('') NULL,
    [Packages]      INT             CONSTRAINT [DF_FtyExport_Packages] DEFAULT ((0)) NULL,
    [WeightKg]      NUMERIC (9, 2)  CONSTRAINT [DF_FtyExport_WeightKg] DEFAULT ((0)) NULL,
    [NetKg]         NUMERIC (9, 2)  CONSTRAINT [DF_FtyExport_NetKg] DEFAULT ((0)) NULL,
    [Cbm]           NUMERIC (10, 3) CONSTRAINT [DF_FtyExport_Cbm] DEFAULT ((0)) NULL,
    [WhseArrival]   DATE            NULL,
    [PortArrival]   DATE            NULL,
    [DocArrival]    DATE            NULL,
    [AddName]       VARCHAR (10)    CONSTRAINT [DF_FtyExport_AddName] DEFAULT ('') NULL,
    [AddDate]       DATETIME        NULL,
    [EditName]      VARCHAR (10)    CONSTRAINT [DF_FtyExport_EditName] DEFAULT ('') NULL,
    [EditDate]      DATETIME        NULL,
    [SisFtyID]      VARCHAR (13)    NULL,
    [Shipper]       VARCHAR (8)     NULL,
    [OnBoard]       DATE            NULL,
    [NoCharges]     BIT             CONSTRAINT [DF_FtyExport_NoCharges] DEFAULT ((0)) NULL,
    [NonDeclare] BIT NOT NULL DEFAULT ((0)), 
    [ShipDate] DATE NULL, 
    [ETA] DATE NULL, 
    CONSTRAINT [PK_FtyExport] PRIMARY KEY CLUSTERED ([ID] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠之物料進出口工作底稿-表頭', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyExport';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'WK NO(Wkyyyymmxxxxx)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyExport', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'記錄資料來源', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyExport', @level2type = N'COLUMN', @level2name = N'Type';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'出口國別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyExport', @level2type = N'COLUMN', @level2name = N'ExportCountry';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'目的地', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyExport', @level2type = N'COLUMN', @level2name = N'ImportCountry';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'出口港口', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyExport', @level2type = N'COLUMN', @level2name = N'ExportPort';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'到達港口', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyExport', @level2type = N'COLUMN', @level2name = N'ImportPort';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'櫃型', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyExport', @level2type = N'COLUMN', @level2name = N'CYCFS';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Shipping mode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyExport', @level2type = N'COLUMN', @level2name = N'ShipModeID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'CONSIGNEE', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyExport', @level2type = N'COLUMN', @level2name = N'Consignee';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'船務負責人', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyExport', @level2type = N'COLUMN', @level2name = N'Handle';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'FORWARDER代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyExport', @level2type = N'COLUMN', @level2name = N'Forwarder';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'船名', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyExport', @level2type = N'COLUMN', @level2name = N'Vessel';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'提單號碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyExport', @level2type = N'COLUMN', @level2name = N'Blno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'INVOICE NO.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyExport', @level2type = N'COLUMN', @level2name = N'INVNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'總件數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyExport', @level2type = N'COLUMN', @level2name = N'Packages';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'毛重', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyExport', @level2type = N'COLUMN', @level2name = N'WeightKg';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'淨重', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyExport', @level2type = N'COLUMN', @level2name = N'NetKg';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'材積', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyExport', @level2type = N'COLUMN', @level2name = N'Cbm';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'到達W/H日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyExport', @level2type = N'COLUMN', @level2name = N'WhseArrival';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'到港日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyExport', @level2type = N'COLUMN', @level2name = N'PortArrival';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'文件收到日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyExport', @level2type = N'COLUMN', @level2name = N'DocArrival';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyExport', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyExport', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyExport', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyExport', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'出貨日',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'FtyExport',
    @level2type = N'COLUMN',
    @level2name = N'ShipDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'預計到達日',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'FtyExport',
    @level2type = N'COLUMN',
    @level2name = N'ETA'