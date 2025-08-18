CREATE TABLE [dbo].[P_FabricInspDailyReport_Detail] (
    [InspectionStatus]     VARCHAR (8000)  CONSTRAINT [DF_P_FabricInspDailyReport_Detail_InspectionStatus] DEFAULT ('') NOT NULL,
    [InspDate]             DATE            NOT NULL,
    [Inspector]            VARCHAR (8000)  CONSTRAINT [DF_P_FabricInspDailyReport_Detail_Inspector] DEFAULT ('') NOT NULL,
    [InspectorName]        VARCHAR (8000)  CONSTRAINT [DF_P_FabricInspDailyReport_Detail_InspectorName] DEFAULT ('') NOT NULL,
    [BrandID]              VARCHAR (8000)  CONSTRAINT [DF_P_FabricInspDailyReport_Detail_BrandID] DEFAULT ('') NOT NULL,
    [FactoryID]            VARCHAR (8000)  CONSTRAINT [DF_P_FabricInspDailyReport_Detail_FactoryID] DEFAULT ('') NOT NULL,
    [Style]                VARCHAR (8000)  CONSTRAINT [DF_P_FabricInspDailyReport_Detail_Style] DEFAULT ('') NOT NULL,
    [POID]                 VARCHAR (8000)  CONSTRAINT [DF_P_FabricInspDailyReport_Detail_POID] DEFAULT ('') NOT NULL,
    [SEQ]                  VARCHAR (8000)  CONSTRAINT [DF_P_FabricInspDailyReport_Detail_SEQ] DEFAULT ('') NOT NULL,
    [StockType]            VARCHAR (8000)  CONSTRAINT [DF_P_FabricInspDailyReport_Detail_StockType] DEFAULT ('') NOT NULL,
    [WKNo]                 VARCHAR (8000)  CONSTRAINT [DF_P_FabricInspDailyReport_Detail_WKNo] DEFAULT ('') NOT NULL,
    [SuppID]               VARCHAR (8000)  CONSTRAINT [DF_P_FabricInspDailyReport_Detail_SuppID] DEFAULT ('') NOT NULL,
    [SuppName]             VARCHAR (8000)  CONSTRAINT [DF_P_FabricInspDailyReport_Detail_SuppName] DEFAULT ('') NOT NULL,
    [ATA]                  DATE            NOT NULL,
    [Roll]                 VARCHAR (8000)  CONSTRAINT [DF_P_FabricInspDailyReport_Detail_Roll] DEFAULT ('') NOT NULL,
    [Dyelot]               VARCHAR (8000)  CONSTRAINT [DF_P_FabricInspDailyReport_Detail_Dyelot] DEFAULT ('') NOT NULL,
    [RefNo]                VARCHAR (8000)  CONSTRAINT [DF_P_FabricInspDailyReport_Detail_RefNo] DEFAULT ('') NOT NULL,
    [Color]                VARCHAR (8000)  CONSTRAINT [DF_P_FabricInspDailyReport_Detail_Color] DEFAULT ('') NOT NULL,
    [ArrivedYDS]           NUMERIC (38, 6) CONSTRAINT [DF_P_FabricInspDailyReport_Detail_ArrivedYDS] DEFAULT ((0)) NOT NULL,
    [ActualYDS]            NUMERIC (38, 6) CONSTRAINT [DF_P_FabricInspDailyReport_Detail_ActualYDS] DEFAULT ((0)) NOT NULL,
    [LthOfDiff]            NUMERIC (38, 6) CONSTRAINT [DF_P_FabricInspDailyReport_Detail_LthOfDiff] DEFAULT ((0)) NOT NULL,
    [TransactionID]        VARCHAR (8000)  CONSTRAINT [DF_P_FabricInspDailyReport_Detail_TransactionID] DEFAULT ('') NOT NULL,
    [QCIssueQty]           NUMERIC (38, 6) CONSTRAINT [DF_P_FabricInspDailyReport_Detail_QCIssueQty] DEFAULT ((0)) NOT NULL,
    [QCIssueTransactionID] VARCHAR (8000)  CONSTRAINT [DF_P_FabricInspDailyReport_Detail_QCIssueTransactionID] DEFAULT ('') NOT NULL,
    [CutWidth]             NUMERIC (38, 6) CONSTRAINT [DF_P_FabricInspDailyReport_Detail_CutWidth] DEFAULT ((0)) NOT NULL,
    [ActualWidth]          NUMERIC (38, 6) CONSTRAINT [DF_P_FabricInspDailyReport_Detail_ActualWidth] DEFAULT ((0)) NOT NULL,
    [Speed]                NUMERIC (38, 6) CONSTRAINT [DF_P_FabricInspDailyReport_Detail_Speed] DEFAULT ((0)) NOT NULL,
    [TotalDefectPoints]    NUMERIC (38, 6) CONSTRAINT [DF_P_FabricInspDailyReport_Detail_TotalDefectPoints] DEFAULT ((0)) NOT NULL,
    [PointRatePerRoll]     NUMERIC (38, 6) CONSTRAINT [DF_P_FabricInspDailyReport_Detail_PointRatePerRoll] DEFAULT ((0)) NOT NULL,
    [Grade]                VARCHAR (8000)  CONSTRAINT [DF_P_FabricInspDailyReport_Detail_Grade] DEFAULT ('') NOT NULL,
    [SortOut]              VARCHAR (8000)  CONSTRAINT [DF_P_FabricInspDailyReport_Detail_SortOut] DEFAULT ('') NOT NULL,
    [InspectionStartTime]  DATETIME        NULL,
    [InspectionFinishTime] DATETIME        NULL,
    [MachineDownTime]      INT             CONSTRAINT [DF_P_FabricInspDailyReport_Detail_MachineDownTime] DEFAULT ((0)) NOT NULL,
    [MachineRunTime]       INT             CONSTRAINT [DF_P_FabricInspDailyReport_Detail_MachineRunTime] DEFAULT ((0)) NOT NULL,
    [Remark]               VARCHAR (8000)  CONSTRAINT [DF_P_FabricInspDailyReport_Detail_Remark] DEFAULT ('') NOT NULL,
    [MCHandle]             VARCHAR (8000)  CONSTRAINT [DF_P_FabricInspDailyReport_Detail_MCHandle] DEFAULT ('') NOT NULL,
    [WeaveType]            VARCHAR (8000)  CONSTRAINT [DF_P_FabricInspDailyReport_Detail_WeaveType] DEFAULT ('') NOT NULL,
    [MachineID]            VARCHAR (8000)  CONSTRAINT [DF_P_FabricInspDailyReport_Detail_MachineID] DEFAULT ('') NOT NULL,
    [ReceivingID]		   VARCHAR (8000)  CONSTRAINT [DF_P_FabricInspDailyReport_Detail_ReceivingID] DEFAULT ('') NOT NULL,
	[InspSeq]              INT             CONSTRAINT [DF_P_FabricInspDailyReport_Detail_InspSeq] DEFAULT ((0)) NOT NULL,
    [AddDate]              DATETIME        NULL,
    [BIFactoryID]          VARCHAR (8000)  CONSTRAINT [DF_P_FabricInspDailyReport_Detail_BIFactoryID] DEFAULT ('') NOT NULL,
    [BIInsertDate]         DATETIME        CONSTRAINT [DF_P_FabricInspDailyReport_Detail_BIInsertDate] DEFAULT (getdate()) NOT NULL,
    [BIStatus]             VARCHAR (8000)  CONSTRAINT [DF_P_FabricInspDailyReport_Detail_BIStatus] DEFAULT ('New') NOT NULL,
    CONSTRAINT [PK_P_FabricInspDailyReport_Detail] PRIMARY KEY CLUSTERED ([POID] ASC, [ReceivingID] ASC, [SEQ] ASC, [Roll] ASC, [Dyelot] ASC, [InspSeq] ASC)
);

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'檢驗日期',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspDailyReport_Detail',
    @level2type = N'COLUMN',
    @level2name = N'InspDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'檢驗人員',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspDailyReport_Detail',
    @level2type = N'COLUMN',
    @level2name = N'Inspector'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'檢驗人員名',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspDailyReport_Detail',
    @level2type = N'COLUMN',
    @level2name = N'InspectorName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'採購單號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspDailyReport_Detail',
    @level2type = N'COLUMN',
    @level2name = N'POID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'品牌',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspDailyReport_Detail',
    @level2type = N'COLUMN',
    @level2name = N'BrandID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'大項-小項',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspDailyReport_Detail',
    @level2type = N'COLUMN',
    @level2name = N'SEQ'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Stock Type',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspDailyReport_Detail',
    @level2type = N'COLUMN',
    @level2name = N'StockType'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'廠商代號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspDailyReport_Detail',
    @level2type = N'COLUMN',
    @level2name = N'SuppID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'廠商英文簡稱',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspDailyReport_Detail',
    @level2type = N'COLUMN',
    @level2name = N'SuppName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Arrive W/H Date',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspDailyReport_Detail',
    @level2type = N'COLUMN',
    @level2name = N'ATA'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'捲號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspDailyReport_Detail',
    @level2type = N'COLUMN',
    @level2name = N'Roll'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'缸號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspDailyReport_Detail',
    @level2type = N'COLUMN',
    @level2name = N'Dyelot'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'物料編號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspDailyReport_Detail',
    @level2type = N'COLUMN',
    @level2name = N'RefNo'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Color',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspDailyReport_Detail',
    @level2type = N'COLUMN',
    @level2name = N'Color'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Arrived YDS',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspDailyReport_Detail',
    @level2type = N'COLUMN',
    @level2name = N'ArrivedYDS'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'實際檢驗碼長',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspDailyReport_Detail',
    @level2type = N'COLUMN',
    @level2name = N'ActualYDS'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Lth. Of Diff',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspDailyReport_Detail',
    @level2type = N'COLUMN',
    @level2name = N'LthOfDiff'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'TransactionID',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspDailyReport_Detail',
    @level2type = N'COLUMN',
    @level2name = N'TransactionID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'發料數量',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspDailyReport_Detail',
    @level2type = N'COLUMN',
    @level2name = N'QCIssueQty'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'發料單號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspDailyReport_Detail',
    @level2type = N'COLUMN',
    @level2name = N'QCIssueTransactionID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'幅寬',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspDailyReport_Detail',
    @level2type = N'COLUMN',
    @level2name = N'CutWidth'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'實際幅寬',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspDailyReport_Detail',
    @level2type = N'COLUMN',
    @level2name = N'ActualWidth'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Speed(Yds/Minutes)',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspDailyReport_Detail',
    @level2type = N'COLUMN',
    @level2name = N'Speed'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'總瑕疵點數',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspDailyReport_Detail',
    @level2type = N'COLUMN',
    @level2name = N'TotalDefectPoints'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'等級',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspDailyReport_Detail',
    @level2type = N'COLUMN',
    @level2name = N'Grade'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'備註',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspDailyReport_Detail',
    @level2type = N'COLUMN',
    @level2name = N'Remark'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'MC Handle',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspDailyReport_Detail',
    @level2type = N'COLUMN',
    @level2name = N'MCHandle'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'織法',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspDailyReport_Detail',
    @level2type = N'COLUMN',
    @level2name = N'WeaveType'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'記錄哪間工廠的資料，ex PH1, PH2',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspDailyReport_Detail',
    @level2type = N'COLUMN',
    @level2name = N'BIFactoryID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'時間戳記，紀錄寫入table時間',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspDailyReport_Detail',
    @level2type = N'COLUMN',
    @level2name = N'BIInsertDate'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', 
    @value = N'款式', 
    @level0type = N'SCHEMA', 
    @level0name = N'dbo', 
    @level1type = N'TABLE', 
    @level1name = N'P_FabricInspDailyReport_Detail', 
    @level2type = N'COLUMN', 
    @level2name = N'Style';
GO

