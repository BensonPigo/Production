CREATE TABLE P_FabricInspDailyReport_Detail
(
    InspDate date NOT NULL,
    Inspector varchar(10) NOT NULL CONSTRAINT [CONSTRAINT_P_FabricInspDailyReport_Detail_Inspector] DEFAULT (''),
    InspectorName nvarchar(30) NOT NULL CONSTRAINT [CONSTRAINT_P_FabricInspDailyReport_Detail_InspectorName] DEFAULT (''),
    BrandID varchar(8) NOT NULL CONSTRAINT [CONSTRAINT_P_FabricInspDailyReport_Detail_BrandID] DEFAULT (''),
    Factory varchar(8) NOT NULL CONSTRAINT [CONSTRAINT_P_FabricInspDailyReport_Detail_Factory] DEFAULT (''),
    StyleID varchar(15) NOT NULL CONSTRAINT [CONSTRAINT_P_FabricInspDailyReport_Detail_StyleID] DEFAULT (''),
    POID varchar(13) NOT NULL CONSTRAINT [CONSTRAINT_P_FabricInspDailyReport_Detail_POID] DEFAULT (''),
    SEQ varchar(6) NOT NULL CONSTRAINT [CONSTRAINT_P_FabricInspDailyReport_Detail_SEQ] DEFAULT (''),
    StockType nvarchar(10) NOT NULL CONSTRAINT [CONSTRAINT_P_FabricInspDailyReport_Detail_StockType] DEFAULT (''),
    Wkno varchar(13) NOT NULL CONSTRAINT [CONSTRAINT_P_FabricInspDailyReport_Detail_Wkno] DEFAULT (''),
    SuppID varchar(6) NOT NULL CONSTRAINT [CONSTRAINT_P_FabricInspDailyReport_Detail_SuppID] DEFAULT (''),
    SuppName varchar(12) NOT NULL CONSTRAINT [CONSTRAINT_P_FabricInspDailyReport_Detail_SuppName] DEFAULT (''),
    ATA date not NULL,
    Roll varchar(8) NOT NULL CONSTRAINT [CONSTRAINT_P_FabricInspDailyReport_Detail_Roll] DEFAULT (''),
    Dyelot varchar(8) NOT NULL CONSTRAINT [CONSTRAINT_P_FabricInspDailyReport_Detail_Dyelot] DEFAULT (''),
    RefNo varchar(36) NOT NULL CONSTRAINT [CONSTRAINT_P_FabricInspDailyReport_Detail_RefNo] DEFAULT (''),
    Color varchar(500) NOT NULL CONSTRAINT [CONSTRAINT_P_FabricInspDailyReport_Detail_Color] DEFAULT (''),
    ArrivedYDS numeric(11, 2) NOT NULL CONSTRAINT [CONSTRAINT_P_FabricInspDailyReport_Detail_ArrivedYDS] DEFAULT (0),
    ActualYDS numeric(8, 2) NOT NULL CONSTRAINT [CONSTRAINT_P_FabricInspDailyReport_Detail_ActualYDS] DEFAULT (0),
    LthOfDiff numeric(8, 2) NOT NULL CONSTRAINT [CONSTRAINT_P_FabricInspDailyReport_Detail_LthOfDiff] DEFAULT (0),
    TransactionID varchar(30) NOT NULL CONSTRAINT [CONSTRAINT_P_FabricInspDailyReport_Detail_TransactionID] DEFAULT (''),
    QCIssueQty numeric(11, 2) NOT NULL CONSTRAINT [CONSTRAINT_P_FabricInspDailyReport_Detail_QCIssueQty] DEFAULT (0),
    QCIssueTransactionID varchar(13) NOT NULL CONSTRAINT [CONSTRAINT_P_FabricInspDailyReport_Detail_QCIssueTransactionID] DEFAULT (''),
    CutWidth numeric(5, 2) NOT NULL CONSTRAINT [CONSTRAINT_P_FabricInspDailyReport_Detail_CutWidth] DEFAULT (0),
    ActualWidth numeric(5, 2) NOT NULL CONSTRAINT [CONSTRAINT_P_FabricInspDailyReport_Detail_ActualWidth] DEFAULT (0),
    Speed numeric(10, 2) NOT NULL CONSTRAINT [CONSTRAINT_P_FabricInspDailyReport_Detail_Speed] DEFAULT (0),
    TotalDefectPoints numeric(6, 0) NOT NULL CONSTRAINT [CONSTRAINT_P_FabricInspDailyReport_Detail_TotalDefectPoints] DEFAULT (0),
    Grade varchar(10) NOT NULL CONSTRAINT [CONSTRAINT_P_FabricInspDailyReport_Detail_Grade] DEFAULT (''),
    ActInspTimeStart datetime NULL,
    CalculatedInspTimeStartFirstTime datetime NULL,
    ActInspTimeFinish datetime NULL,
    InspTimeFinishFirstTime datetime NULL,
    QCMachineStopTime int NOT NULL CONSTRAINT [CONSTRAINT_P_FabricInspDailyReport_Detail_QCMachineStopTime] DEFAULT (0),
    QCMachineRunTime int NOT NULL CONSTRAINT [CONSTRAINT_P_FabricInspDailyReport_Detail_QCMachineRunTime] DEFAULT (0),
    Remark nvarchar(60) NOT NULL CONSTRAINT [CONSTRAINT_P_FabricInspDailyReport_Detail_Remark] DEFAULT (''),
    MCHandle varchar(45) NOT NULL CONSTRAINT [CONSTRAINT_P_FabricInspDailyReport_Detail_MCHandle] DEFAULT (''),
    WeaveType varchar(20) NOT NULL CONSTRAINT [CONSTRAINT_P_FabricInspDailyReport_Detail_WeaveType] DEFAULT (''),
    AddDate datetime NULL,
    EditDate datetime NULL,
    [ReceivingID] VARCHAR(13) NOT NULL, 
    [BIFactoryID] VARCHAR(8) NOT NULL DEFAULT (''), 
    [BIInsertDate] DATETIME NULL, 
    CONSTRAINT [PK_P_FabricInspDailyReport_Detail] PRIMARY KEY (
        InspDate, 
        Inspector, 
        POID, 
        SEQ, 
        ATA, 
        Roll, 
        Dyelot
    )
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
    @value = N'Factory Group',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspDailyReport_Detail',
    @level2type = N'COLUMN',
    @level2name = N'Factory'
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
    @value = N'款式',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspDailyReport_Detail',
    @level2type = N'COLUMN',
    @level2name = N'StyleID'
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
    @value = N'Act Insp Time Start ',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspDailyReport_Detail',
    @level2type = N'COLUMN',
    @level2name = N'ActInspTimeStart'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Calculated Inspection Time Start',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspDailyReport_Detail',
    @level2type = N'COLUMN',
    @level2name = N'CalculatedInspTimeStartFirstTime'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Physical Insp最後編輯時間',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspDailyReport_Detail',
    @level2type = N'COLUMN',
    @level2name = N'ActInspTimeFinish'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Physical Insp新增時間',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspDailyReport_Detail',
    @level2type = N'COLUMN',
    @level2name = N'InspTimeFinishFirstTime'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'QC Machine Stop Time',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspDailyReport_Detail',
    @level2type = N'COLUMN',
    @level2name = N'QCMachineStopTime'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'QC驗布時間(秒)',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspDailyReport_Detail',
    @level2type = N'COLUMN',
    @level2name = N'QCMachineRunTime'
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
    @value = N'收料單號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_FabricInspDailyReport_Detail',
    @level2type = N'COLUMN',
    @level2name = N'ReceivingID'
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