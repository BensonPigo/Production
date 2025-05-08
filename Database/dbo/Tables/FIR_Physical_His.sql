CREATE TABLE [dbo].[FIR_Physical_His] (
    [DetailUkey] BIGINT NOT NULL,
    [InspSeq] INT NOT NULL,
    [ID] BIGINT CONSTRAINT [DF_FIR_Physical_His_ID] DEFAULT (0) NOT NULL,
    [Roll] VARCHAR(8) CONSTRAINT [DF_FIR_Physical_His_Roll] DEFAULT ('') NULL,
    [Dyelot] VARCHAR(8) CONSTRAINT [DF_FIR_Physical_His_Dyelot] DEFAULT ('') NULL,
    [POID] VARCHAR(13) CONSTRAINT [DF_FIR_Physical_His_POID] DEFAULT ('') NOT NULL,
    [Seq1] VARCHAR(3) CONSTRAINT [DF_FIR_Physical_His_Seq1] DEFAULT ('') NOT NULL,
    [Seq2] VARCHAR(2) CONSTRAINT [DF_FIR_Physical_His_Seq2] DEFAULT ('') NOT NULL,
    [ReceivingID] VARCHAR(13) CONSTRAINT [DF_FIR_Physical_His_ReceivingID] DEFAULT ('') NOT NULL,
    [TicketYds] NUMERIC(8,2) CONSTRAINT [DF_FIR_Physical_His_TicketYds] DEFAULT (0) NOT NULL,
    [ActualYds] NUMERIC(8,2) CONSTRAINT [DF_FIR_Physical_His_ActualYds] DEFAULT (0) NOT NULL,
    [FullWidth] NUMERIC(5,2) CONSTRAINT [DF_FIR_Physical_His_FullWidth] DEFAULT (0) NOT NULL,
    [ActualWidth] NUMERIC(5,2) CONSTRAINT [DF_FIR_Physical_His_ActualWidth] DEFAULT (0) NOT NULL,
    [TotalPoint] NUMERIC(6,0) CONSTRAINT [DF_FIR_Physical_His_TotalPoint] DEFAULT (0) NULL,
    [PointRate] NUMERIC(6,2) CONSTRAINT [DF_FIR_Physical_His_PointRate] DEFAULT (0) NULL,
    [Grade] VARCHAR(10) CONSTRAINT [DF_FIR_Physical_His_Grade] DEFAULT ('') NULL,
    [Result] VARCHAR(5) CONSTRAINT [DF_FIR_Physical_His_Result] DEFAULT ('') NULL,
    [Remark] NVARCHAR(60) CONSTRAINT [DF_FIR_Physical_His_Remark] DEFAULT ('') NULL,
    [InspDate] DATE NULL,
    [Inspector] VARCHAR(10) CONSTRAINT [DF_FIR_Physical_His_Inspector] DEFAULT ('') NULL,
    [AddName] VARCHAR(10) CONSTRAINT [DF_FIR_Physical_His_AddName] DEFAULT ('') NULL,
    [AddDate] DATETIME NULL,
    [EditName] VARCHAR(10) CONSTRAINT [DF_FIR_Physical_His_EditName] DEFAULT ('') NULL,
    [EditDate] DATETIME NULL,
    [QCTime] INT CONSTRAINT [DF_FIR_Physical_His_QCTime] DEFAULT (0) NOT NULL,
    [IsQMS] BIT CONSTRAINT [DF_FIR_Physical_His_IsQMS] DEFAULT (0) NOT NULL,
    [Issue_DetailUkey] BIGINT CONSTRAINT [DF_FIR_Physical_His_Issue_DetailUkey] DEFAULT (0) NOT NULL,
    [QMSMachineID] VARCHAR(20) CONSTRAINT [DF_FIR_Physical_His_QMSMachineID] DEFAULT ('') NULL,
    [ColorToneCheck] BIT CONSTRAINT [DF_FIR_Physical_His_ColorToneCheck] DEFAULT (0) NOT NULL,
    [GrandCcanUseReason] NVARCHAR(300) CONSTRAINT [DF_FIR_Physical_His_GrandCcanUseReason] DEFAULT ('') NOT NULL,
    [StartTime] DATETIME NULL,
    [TransactionID] VARCHAR(30) NULL,
    [QCstopQty] TINYINT CONSTRAINT [DF_FIR_Physical_His_QCstopQty] DEFAULT (0) NOT NULL,
    [Moisture] BIT CONSTRAINT [DF_FIR_Physical_His_Moisture] DEFAULT (0) NULL,
    [IsGrandCCanUse] BIT CONSTRAINT [DF_FIR_Physical_His_IsGrandCCanUse] DEFAULT (0) NOT NULL,
    CONSTRAINT [PK_FIR_Physical_His] PRIMARY KEY CLUSTERED ([DetailUkey] ASC, [InspSeq] ASC)
);
GO

-- 建立欄位描述
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'Detail Ukey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical_His', @level2type = N'COLUMN', @level2name = N'DetailUkey';
GO
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'稽核流水號 By FIRID, Roll, Dyelot', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical_His', @level2type = N'COLUMN', @level2name = N'InspSeq';
GO
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical_His', @level2type = N'COLUMN', @level2name = N'ID';
GO
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'捲號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical_His', @level2type = N'COLUMN', @level2name = N'Roll';
GO
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'缸號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical_His', @level2type = N'COLUMN', @level2name = N'Dyelot';
GO
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'採購單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical_His', @level2type = N'COLUMN', @level2name = N'POID';
GO
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'大項', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical_His', @level2type = N'COLUMN', @level2name = N'Seq1';
GO
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'小項', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical_His', @level2type = N'COLUMN', @level2name = N'Seq2';
GO
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'收料單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical_His', @level2type = N'COLUMN', @level2name = N'ReceivingID';
GO
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'帳面碼數碼長', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical_His', @level2type = N'COLUMN', @level2name = N'TicketYds';
GO
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'實際檢驗碼長', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical_His', @level2type = N'COLUMN', @level2name = N'ActualYds';
GO
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'全幅寬度', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical_His', @level2type = N'COLUMN', @level2name = N'FullWidth';
GO
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'實際幅寬', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical_His', @level2type = N'COLUMN', @level2name = N'ActualWidth';
GO
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'總瑕疵點數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical_His', @level2type = N'COLUMN', @level2name = N'TotalPoint';
GO
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'瑕疵點率', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical_His', @level2type = N'COLUMN', @level2name = N'PointRate';
GO
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'等級', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical_His', @level2type = N'COLUMN', @level2name = N'Grade';
GO
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'結果', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical_His', @level2type = N'COLUMN', @level2name = N'Result';
GO
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical_His', @level2type = N'COLUMN', @level2name = N'Remark';
GO
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'檢驗日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical_His', @level2type = N'COLUMN', @level2name = N'InspDate';
GO
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'檢驗人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical_His', @level2type = N'COLUMN', @level2name = N'Inspector';
GO
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical_His', @level2type = N'COLUMN', @level2name = N'AddName';
GO
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical_His', @level2type = N'COLUMN', @level2name = N'AddDate';
GO
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'最後編輯人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical_His', @level2type = N'COLUMN', @level2name = N'EditName';
GO
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'最後編輯時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical_His', @level2type = N'COLUMN', @level2name = N'EditDate';
GO
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'QC稽布時間(秒)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical_His', @level2type = N'COLUMN', @level2name = N'QCTime';
GO
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'是否為QMS操作', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical_His', @level2type = N'COLUMN', @level2name = N'IsQMS';
GO
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'Issue_Detail Ukey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical_His', @level2type = N'COLUMN', @level2name = N'Issue_DetailUkey';
GO
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'機器編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical_His', @level2type = N'COLUMN', @level2name = N'QMSMachineID';
GO
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'色差(By Physical)，default(0:正常/1:有問題)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical_His', @level2type = N'COLUMN', @level2name = N'ColorToneCheck';
GO
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'色差原因', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical_His', @level2type = N'COLUMN', @level2name = N'GrandCcanUseReason';
GO
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'開始時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical_His', @level2type = N'COLUMN', @level2name = N'StartTime';
GO
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'交易ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical_His', @level2type = N'COLUMN', @level2name = N'TransactionID';
GO
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'QC停止量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical_His', @level2type = N'COLUMN', @level2name = N'QCstopQty';
GO
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'水分', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical_His', @level2type = N'COLUMN', @level2name = N'Moisture';
GO
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'是否大色差', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical_His', @level2type = N'COLUMN', @level2name = N'IsGrandCCanUse';
GO