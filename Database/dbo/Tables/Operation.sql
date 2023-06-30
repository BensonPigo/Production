﻿CREATE TABLE [dbo].[Operation] (
    [ID]                  VARCHAR (20)   CONSTRAINT [DF_Operation_ID] DEFAULT ('') NOT NULL,
    [FromGSD]             BIT            CONSTRAINT [DF_Operation_FromGSD] DEFAULT ((0)) NOT NULL,
    [CalibratedCode]      BIT            CONSTRAINT [DF_Operation_CalibratedCode] DEFAULT ((0)) NOT NULL,
    [DescEN]              NVARCHAR (300) CONSTRAINT [DF_Operation_DescEN] DEFAULT ('') NOT NULL,
    [DescCH]              NVARCHAR (300) CONSTRAINT [DF_Operation_DescCH] DEFAULT ('') NOT NULL,
    [MachineTypeID]       VARCHAR (10)   CONSTRAINT [DF_Operation_MachineTypeID] DEFAULT ('') NOT NULL,
    [MtlFactorID]         VARCHAR (3)    CONSTRAINT [DF_Operation_MtlFactorID] DEFAULT ('') NOT NULL,
    [ISO]                 VARCHAR (10)   CONSTRAINT [DF_Operation_ISO] DEFAULT ('') NOT NULL,
    [RPM]                 INT            CONSTRAINT [DF_Operation_RPM] DEFAULT ((0)) NOT NULL,
    [MoldID]              VARCHAR (200)  CONSTRAINT [DF_Operation_MoldID] DEFAULT ('') NOT NULL,
    [OperationType]       VARCHAR (10)   CONSTRAINT [DF_Operation_OperationType] DEFAULT ('') NOT NULL,
    [CostCenter]          VARCHAR (10)   CONSTRAINT [DF_Operation_CostCenter] DEFAULT ('') NOT NULL,
    [Section]             VARCHAR (10)   CONSTRAINT [DF_Operation_Section] DEFAULT ('') NOT NULL,
    [SMV]                 DECIMAL (7, 4) CONSTRAINT [DF_Operation_SMV] DEFAULT ((0)) NOT NULL,
    [MachineTMU]          DECIMAL (8, 2) CONSTRAINT [DF_Operation_MachineTMU] DEFAULT ((0)) NOT NULL,
    [ManualTMU]           DECIMAL (8, 2) CONSTRAINT [DF_Operation_ManualTMU] DEFAULT ((0)) NOT NULL,
    [TotalTMU]            DECIMAL (8, 2) CONSTRAINT [DF_Operation_TotalTMU] DEFAULT ((0)) NOT NULL,
    [MachineAllowanceSMV] DECIMAL (8, 4) CONSTRAINT [DF_Operation_MachineAllowanceSMV] DEFAULT ((0)) NOT NULL,
    [ManualAllowanceSMV]  DECIMAL (8, 4) CONSTRAINT [DF_Operation_ManualAllowanceSMV] DEFAULT ((0)) NOT NULL,
    [StitchCM]            DECIMAL (5, 2) CONSTRAINT [DF_Operation_StitchCM] DEFAULT ((0)) NOT NULL,
    [SeamLength]          DECIMAL (9, 2) CONSTRAINT [DF_Operation_SeamLength] DEFAULT ((0)) NOT NULL,
    [Picture1]            NVARCHAR (60)  CONSTRAINT [DF_Operation_Picture1] DEFAULT ('') NOT NULL,
    [Picture2]            NVARCHAR (60)  CONSTRAINT [DF_Operation_Picture2] DEFAULT ('') NOT NULL,
    [NeedleThread]        NVARCHAR (40)  CONSTRAINT [DF_Operation_NeedleThread] DEFAULT ('') NOT NULL,
    [BottomThread]        NVARCHAR (40)  CONSTRAINT [DF_Operation_BottomThread] DEFAULT ('') NOT NULL,
    [CoverThread]         NVARCHAR (40)  CONSTRAINT [DF_Operation_CoverThread] DEFAULT ('') NOT NULL,
    [NeedleLength]        DECIMAL (6, 2) CONSTRAINT [DF_Operation_NeedleLength] DEFAULT ((0)) NOT NULL,
    [BottomLength]        DECIMAL (6, 2) CONSTRAINT [DF_Operation_BottomLength] DEFAULT ((0)) NOT NULL,
    [CoverLength]         DECIMAL (6, 2) CONSTRAINT [DF_Operation_CoverLength] DEFAULT ((0)) NOT NULL,
    [Junk]                BIT            CONSTRAINT [DF_Operation_Junk] DEFAULT ((0)) NOT NULL,
    [AddName]             VARCHAR (10)   CONSTRAINT [DF_Operation_AddName] DEFAULT ('') NOT NULL,
    [AddDate]             DATETIME       NULL,
    [EditName]            VARCHAR (10)   CONSTRAINT [DF_Operation_EditName] DEFAULT ('') NOT NULL,
    [EditDate]            DATETIME       NULL,
    [Ukey]                BIGINT         IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Annotation]          NVARCHAR (200) CONSTRAINT [DF_Operation_Annotation] DEFAULT ('') NOT NULL,
    [MasterPlusGroup]     VARCHAR (4)    DEFAULT ('') NOT NULL,
    [Hem]                 BIT            DEFAULT ((0)) NOT NULL,
    [Segment]             INT            CONSTRAINT [DF_Operation_Segment] DEFAULT ((0)) NOT NULL,
    [Tubular]             BIT            CONSTRAINT [DF_Operation_Tubular] DEFAULT ((0)) NOT NULL,
    [DescVN]              NVARCHAR (300) DEFAULT ('') NOT NULL,
    [DescKH]              NVARCHAR (300) DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_Operation] PRIMARY KEY CLUSTERED ([ID] ASC)
);








GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Operation 主檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Operation';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'小工段', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Operation', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否由GSD匯入', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Operation', @level2type = N'COLUMN', @level2name = N'FromGSD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'已準確', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Operation', @level2type = N'COLUMN', @level2name = N'CalibratedCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Description(英文)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Operation', @level2type = N'COLUMN', @level2name = N'DescEN';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Description (中文)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Operation', @level2type = N'COLUMN', @level2name = N'DescCH';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'機台', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Operation', @level2type = N'COLUMN', @level2name = N'MachineTypeID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'MaterialFactor', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Operation', @level2type = N'COLUMN', @level2name = N'MtlFactorID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ISO', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Operation', @level2type = N'COLUMN', @level2name = N'ISO';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'機器轉速', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Operation', @level2type = N'COLUMN', @level2name = N'RPM';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'模具', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Operation', @level2type = N'COLUMN', @level2name = N'MoldID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工段分類', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Operation', @level2type = N'COLUMN', @level2name = N'OperationType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'成本分類', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Operation', @level2type = N'COLUMN', @level2name = N'CostCenter';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'區域', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Operation', @level2type = N'COLUMN', @level2name = N'Section';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SMV(分)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Operation', @level2type = N'COLUMN', @level2name = N'SMV';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'機器 TMU (不含Allowance)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Operation', @level2type = N'COLUMN', @level2name = N'MachineTMU';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'人工 TMU (不含Allowance)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Operation', @level2type = N'COLUMN', @level2name = N'ManualTMU';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'TMU總合 (不含Allowance)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Operation', @level2type = N'COLUMN', @level2name = N'TotalTMU';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'機器 SMV (含Allowance)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Operation', @level2type = N'COLUMN', @level2name = N'MachineAllowanceSMV';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'人工+機器 SMV (含Allowance)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Operation', @level2type = N'COLUMN', @level2name = N'ManualAllowanceSMV';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'每公分針數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Operation', @level2type = N'COLUMN', @level2name = N'StitchCM';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'用線車縫長', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Operation', @level2type = N'COLUMN', @level2name = N'SeamLength';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'圖片一', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Operation', @level2type = N'COLUMN', @level2name = N'Picture1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'圖片二', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Operation', @level2type = N'COLUMN', @level2name = N'Picture2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'面線說明', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Operation', @level2type = N'COLUMN', @level2name = N'NeedleThread';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'底線說明', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Operation', @level2type = N'COLUMN', @level2name = N'BottomThread';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'圈線說明', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Operation', @level2type = N'COLUMN', @level2name = N'CoverThread';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'面線長度', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Operation', @level2type = N'COLUMN', @level2name = N'NeedleLength';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'底線長度', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Operation', @level2type = N'COLUMN', @level2name = N'BottomLength';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'圈線長度', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Operation', @level2type = N'COLUMN', @level2name = N'CoverLength';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Junk', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Operation', @level2type = N'COLUMN', @level2name = N'Junk';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Operation', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Operation', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Operation', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Operation', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'UKey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Operation', @level2type = N'COLUMN', @level2name = N'Ukey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'確認是否會用到圓筒輔助的機台，有無輔助的頭尾線耗損量不同。', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Operation', @level2type = N'COLUMN', @level2name = N'Tubular';


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Description(越南文)',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Operation',
    @level2type = N'COLUMN',
    @level2name = N'DescVN'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Description(高棉文)',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Operation',
    @level2type = N'COLUMN',
    @level2name = N'DescKH'