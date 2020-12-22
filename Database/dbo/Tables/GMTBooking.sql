CREATE TABLE [dbo].[GMTBooking] (
    [ID]                       VARCHAR (25)    CONSTRAINT [DF_GMTBooking_ID] DEFAULT ('') NOT NULL,
    [Shipper]                  VARCHAR (8)     CONSTRAINT [DF_GMTBooking_Shipper] DEFAULT ('') NOT NULL,
    [InvSerial]                VARCHAR (10)    CONSTRAINT [DF_GMTBooking_InvSerial] DEFAULT ('') NOT NULL,
    [InvDate]                  DATE            NOT NULL,
    [BrandID]                  VARCHAR (8)     CONSTRAINT [DF_GMTBooking_BrandID] DEFAULT ('') NOT NULL,
    [CustCDID]                 VARCHAR (16)    CONSTRAINT [DF_GMTBooking_CustCDID] DEFAULT ('') NOT NULL,
    [Dest]                     VARCHAR (2)     CONSTRAINT [DF_GMTBooking_Dest] DEFAULT ('') NOT NULL,
    [ShipModeID]               VARCHAR (10)    CONSTRAINT [DF_GMTBooking_ShipModeID] DEFAULT ('') NOT NULL,
    [ShipTermID]               VARCHAR (5)     CONSTRAINT [DF_GMTBooking_ShipTermID] DEFAULT ('') NOT NULL,
    [PayTermARID]              VARCHAR (10)     CONSTRAINT [DF_GMTBooking_PayTermARID] DEFAULT ('') NOT NULL,
    [Forwarder]                VARCHAR (6)     CONSTRAINT [DF_GMTBooking_Forwarder] DEFAULT ('') NOT NULL,
    [FCRDate]                  DATE            NULL,
    [Vessel]                   VARCHAR (60)    CONSTRAINT [DF_GMTBooking_Vessel] DEFAULT ('') NULL,
    [CutOffDate]               DATETIME        NULL,
    [ETD]                      DATE            NULL,
    [ETA]                      DATE            NULL,
    [SONo]                     VARCHAR (16)    CONSTRAINT [DF_GMTBooking_SONo] DEFAULT ('') NULL,
    [SOCFMDate]                DATE            NULL,
    [ForwarderWhse_DetailUKey] BIGINT          CONSTRAINT [DF_GMTBooking_ForwarderWhseID] DEFAULT ((0)) NULL,
    [Remark]                   NVARCHAR (60)   CONSTRAINT [DF_GMTBooking_Remark] DEFAULT ('') NULL,
    [TotalShipQty]             INT             CONSTRAINT [DF_GMTBooking_TotalShipQty] DEFAULT ((0)) NULL,
    [TotalCTNQty]              INT             CONSTRAINT [DF_GMTBooking_TotalCTNQty] DEFAULT ((0)) NULL,
    [TotalNW]                  NUMERIC (10, 3) CONSTRAINT [DF_GMTBooking_TotalNW] DEFAULT ((0)) NULL,
    [TotalGW]                  NUMERIC (10, 3) CONSTRAINT [DF_GMTBooking_TotalGW] DEFAULT ((0)) NULL,
    [TotalNNW]                 NUMERIC (10, 3) CONSTRAINT [DF_GMTBooking_TotalNNW] DEFAULT ((0)) NULL,
    [TotalCBM]                 NUMERIC (11, 4) CONSTRAINT [DF_GMTBooking_TotalCBM] DEFAULT ((0)) NULL,
    [Status]                   VARCHAR (15)    CONSTRAINT [DF_GMTBooking_Status] DEFAULT ('') NULL,
    [Handle]                   VARCHAR (10)    CONSTRAINT [DF_GMTBooking_Handle] DEFAULT ('') NOT NULL,
    [Description]              NVARCHAR (80)   CONSTRAINT [DF_GMTBooking_Description] DEFAULT ('') NULL,
    [SendToTPE]                DATE            NULL,
    [ShipPlanID]               VARCHAR (13)    CONSTRAINT [DF_GMTBooking_ShipPlanID] DEFAULT ('') NULL,
    [CYCFS]                    VARCHAR (7)     CONSTRAINT [DF_GMTBooking_CYCFS] DEFAULT ('') NULL,
    [AddName]                  VARCHAR (10)    CONSTRAINT [DF_GMTBooking_AddName] DEFAULT ('') NULL,
    [AddDate]                  DATETIME        NULL,
    [EditName]                 VARCHAR (10)    CONSTRAINT [DF_GMTBooking_EditName] DEFAULT ('') NULL,
    [EditDate]                 DATETIME        NULL,
    [NoExportCharges]          BIT             DEFAULT ((0)) NULL,
    [BIRID]                    INT             NULL,
    [BL2No]                    VARCHAR (20)    DEFAULT ('') NULL,
    [BLNo]                     VARCHAR (20)    DEFAULT ('') NULL,
	[ActFCRDate] [date] NULL,
	[FBDate] [date] NULL,
    [InvoiceApproveDate] DATE NULL, 
    [DocumentRefNo] VARCHAR(15) CONSTRAINT [DF_GMTBooking_DocumentRefNo] DEFAULT ('') NULL,
    [IntendDeliveryDate] DATE NULL, 
    [TotalAPPBookingVW] NUMERIC(21, 2) NULL DEFAULT ((0)), 
    [TotalAPPEstAmtVW] NUMERIC(21, 2) NULL DEFAULT ((0)), 
    [NonDeclare] BIT NOT NULL DEFAULT (0), 
    [Foundry] BIT NULL DEFAULT ((0)), 
    [DischargePortID] VARCHAR(20) NULL, 
    CONSTRAINT [PK_GMTBooking] PRIMARY KEY CLUSTERED ([ID] ASC)
);












GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Garment Booking', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GMTBooking';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GMTBooking', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'出貨人(工廠)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GMTBooking', @level2type = N'COLUMN', @level2name = N'Shipper';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Invoice Serial #', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GMTBooking', @level2type = N'COLUMN', @level2name = N'InvSerial';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Invoice Date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GMTBooking', @level2type = N'COLUMN', @level2name = N'InvDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Brand', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GMTBooking', @level2type = N'COLUMN', @level2name = N'BrandID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'CustCD', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GMTBooking', @level2type = N'COLUMN', @level2name = N'CustCDID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'目的地', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GMTBooking', @level2type = N'COLUMN', @level2name = N'Dest';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'出貨方式', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GMTBooking', @level2type = N'COLUMN', @level2name = N'ShipModeID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'出貨條件', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GMTBooking', @level2type = N'COLUMN', @level2name = N'ShipTermID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'付款條件', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GMTBooking', @level2type = N'COLUMN', @level2name = N'PayTermARID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'船公司', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GMTBooking', @level2type = N'COLUMN', @level2name = N'Forwarder';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'貨運承攬商收據', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GMTBooking', @level2type = N'COLUMN', @level2name = N'FCRDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'船名/航次', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GMTBooking', @level2type = N'COLUMN', @level2name = N'Vessel';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'結關日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GMTBooking', @level2type = N'COLUMN', @level2name = N'CutOffDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'預定開航日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GMTBooking', @level2type = N'COLUMN', @level2name = N'ETD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'預計到達日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GMTBooking', @level2type = N'COLUMN', @level2name = N'ETA';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'艙位訂單', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GMTBooking', @level2type = N'COLUMN', @level2name = N'SONo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'艙位確認日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GMTBooking', @level2type = N'COLUMN', @level2name = N'SOCFMDate';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GMTBooking', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'總數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GMTBooking', @level2type = N'COLUMN', @level2name = N'TotalShipQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'總箱數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GMTBooking', @level2type = N'COLUMN', @level2name = N'TotalCTNQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'總淨重', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GMTBooking', @level2type = N'COLUMN', @level2name = N'TotalNW';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'總重量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GMTBooking', @level2type = N'COLUMN', @level2name = N'TotalGW';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'總淨淨重(不含包裝的重量)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GMTBooking', @level2type = N'COLUMN', @level2name = N'TotalNNW';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'總材積', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GMTBooking', @level2type = N'COLUMN', @level2name = N'TotalCBM';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'狀態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GMTBooking', @level2type = N'COLUMN', @level2name = N'Status';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'負責人', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GMTBooking', @level2type = N'COLUMN', @level2name = N'Handle';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'說明', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GMTBooking', @level2type = N'COLUMN', @level2name = N'Description';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'傳回台北日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GMTBooking', @level2type = N'COLUMN', @level2name = N'SendToTPE';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Ship Plan ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GMTBooking', @level2type = N'COLUMN', @level2name = N'ShipPlanID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'運輸類型', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GMTBooking', @level2type = N'COLUMN', @level2name = N'CYCFS';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GMTBooking', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GMTBooking', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GMTBooking', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GMTBooking', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'貨代倉庫', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GMTBooking', @level2type = N'COLUMN', @level2name = N'ForwarderWhse_DetailUKey';


GO
CREATE NONCLUSTERED INDEX [shipplanid]
    ON [dbo].[GMTBooking]([ShipPlanID] ASC);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'提單號碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GMTBooking', @level2type = N'COLUMN', @level2name = N'BLNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'提單號碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GMTBooking', @level2type = N'COLUMN', @level2name = N'BL2No';


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'確認 Invoice# 是否包含請姊妹廠代工的訂單',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'GMTBooking',
    @level2type = N'COLUMN',
    @level2name = N'Foundry'