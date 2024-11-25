CREATE TABLE [dbo].[SewingDailyOutputStatusRecord] (
    [SewingLineID]         VARCHAR (5)  NOT NULL,
    [SewingOutputDate]     DATE         NOT NULL,
    [FactoryID]            VARCHAR (8)  NOT NULL,
    [OrderID]              VARCHAR (13) CONSTRAINT [DF_SewingDailyOutputStatusRecord_OrderID] DEFAULT ('') NOT NULL,
    [SewingInLine]         DATETIME     NULL,
    [SewingOffLine]        DATETIME     NULL,
    [StandardOutputPerDay] INT          CONSTRAINT [DF_SewingDailyOutputStatusRecord_StandardOutputPerDay] DEFAULT ((0)) NOT NULL,
    [CuttingRemark]        VARCHAR (30) CONSTRAINT [DF_SewingDailyOutputStatusRecord_CuttingRemark] DEFAULT ('') NOT NULL,
    [LoadingRemark]        VARCHAR (30) CONSTRAINT [DF_SewingDailyOutputStatusRecord_LoadingRemark] DEFAULT ('') NOT NULL,
    [LoadingExclusion]     BIT          CONSTRAINT [DF_SewingDailyOutputStatusRecord_LoadingExclusion] DEFAULT ((0)) NOT NULL,
    [ATRemark]             VARCHAR (30) CONSTRAINT [DF_SewingDailyOutputStatusRecord_ATRemark] DEFAULT ('') NOT NULL,
    [ATExclusion]          BIT          CONSTRAINT [DF_SewingDailyOutputStatusRecord_ATExclusion] DEFAULT ((0)) NOT NULL,
    [AUTRemark]            VARCHAR (30) CONSTRAINT [DF_SewingDailyOutputStatusRecord_AUTRemark] DEFAULT ('') NOT NULL,
    [AUTExclusion]         BIT          CONSTRAINT [DF_SewingDailyOutputStatusRecord_AUTExclusion] DEFAULT ((0)) NOT NULL,
    [HTRemark]             VARCHAR (30) CONSTRAINT [DF_SewingDailyOutputStatusRecord_HTRemark] DEFAULT ('') NOT NULL,
    [HTExclusion]          BIT          CONSTRAINT [DF_SewingDailyOutputStatusRecord_HTExclusion] DEFAULT ((0)) NOT NULL,
    [BORemark]             VARCHAR (30) CONSTRAINT [DF_SewingDailyOutputStatusRecord_BORemark] DEFAULT ('') NOT NULL,
    [BOExclusion]          BIT          CONSTRAINT [DF_SewingDailyOutputStatusRecord_BOExclusion] DEFAULT ((0)) NOT NULL,
    [FMRemark]             VARCHAR (30) CONSTRAINT [DF_SewingDailyOutputStatusRecord_FMRemark] DEFAULT ('') NOT NULL,
    [FMExclusion]          BIT          CONSTRAINT [DF_SewingDailyOutputStatusRecord_FMExclusion] DEFAULT ((0)) NOT NULL,
    [PRTRemark]            VARCHAR (30) CONSTRAINT [DF_SewingDailyOutputStatusRecord_PRTRemark] DEFAULT ('') NOT NULL,
    [PRTExclusion]         BIT  CONSTRAINT [DF_SewingDailyOutputStatusRecord_PRTExclusion] DEFAULT ((0)) NOT NULL,
    [AddName]              VARCHAR (10) CONSTRAINT [DF_SewingDailyOutputStatusRecord_AddName] DEFAULT ('') NOT NULL,
    [AddDate]              DATETIME     NULL,
    [EditName]             VARCHAR (10) CONSTRAINT [DF_SewingDailyOutputStatusRecord_EditName] DEFAULT ('') NOT NULL,
    [EditDate]             DATETIME     NULL,
    CONSTRAINT [PK_SewingDailyOutputStatusRecord] PRIMARY KEY CLUSTERED ([SewingLineID] ASC, [SewingOutputDate] ASC, [OrderID] ASC, [FactoryID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'加工段PRT完成率計算排除欄位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingDailyOutputStatusRecord', @level2type = N'COLUMN', @level2name = N'PRTExclusion';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'加工段PRT供應量不足原因', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingDailyOutputStatusRecord', @level2type = N'COLUMN', @level2name = N'PRTRemark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'加工段FM完成率計算排除欄位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingDailyOutputStatusRecord', @level2type = N'COLUMN', @level2name = N'FMExclusion';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'加工段FM供應量不足原因', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingDailyOutputStatusRecord', @level2type = N'COLUMN', @level2name = N'FMRemark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'加工段BO完成率計算排除欄位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingDailyOutputStatusRecord', @level2type = N'COLUMN', @level2name = N'BOExclusion';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'加工段BO供應量不足原因', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingDailyOutputStatusRecord', @level2type = N'COLUMN', @level2name = N'BORemark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'加工段HT完成率計算排除欄位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingDailyOutputStatusRecord', @level2type = N'COLUMN', @level2name = N'HTExclusion';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'加工段HT供應量不足原因', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingDailyOutputStatusRecord', @level2type = N'COLUMN', @level2name = N'HTRemark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'加工段AUT完成率計算排除欄位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingDailyOutputStatusRecord', @level2type = N'COLUMN', @level2name = N'AUTExclusion';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'加工段AUT供應量不足原因', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingDailyOutputStatusRecord', @level2type = N'COLUMN', @level2name = N'AUTRemark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'加工段AT完成率計算排除欄位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingDailyOutputStatusRecord', @level2type = N'COLUMN', @level2name = N'ATExclusion';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'加工段AT供應量不足原因', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingDailyOutputStatusRecord', @level2type = N'COLUMN', @level2name = N'ATRemark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'加工段Loading完成率計算排除欄位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingDailyOutputStatusRecord', @level2type = N'COLUMN', @level2name = N'LoadingExclusion';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'加工段Loading供應量不足原因', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingDailyOutputStatusRecord', @level2type = N'COLUMN', @level2name = N'LoadingRemark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'加工段Cutting供應量不足原因', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingDailyOutputStatusRecord', @level2type = N'COLUMN', @level2name = N'CuttingRemark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'每日標準產出', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingDailyOutputStatusRecord', @level2type = N'COLUMN', @level2name = N'StandardOutputPerDay';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'排程結束日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingDailyOutputStatusRecord', @level2type = N'COLUMN', @level2name = N'SewingOffLine';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'排程開始日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingDailyOutputStatusRecord', @level2type = N'COLUMN', @level2name = N'SewingInLine';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingDailyOutputStatusRecord', @level2type = N'COLUMN', @level2name = N'OrderID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingDailyOutputStatusRecord', @level2type = N'COLUMN', @level2name = N'FactoryID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'產出日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingDailyOutputStatusRecord', @level2type = N'COLUMN', @level2name = N'SewingOutputDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'產線ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingDailyOutputStatusRecord', @level2type = N'COLUMN', @level2name = N'SewingLineID';

