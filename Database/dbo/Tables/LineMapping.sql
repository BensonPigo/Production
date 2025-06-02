CREATE TABLE [dbo].[LineMapping](
	[StyleID] [varchar](15) NOT NULL,
	[SeasonID] [varchar](10) NOT NULL,
	[BrandID] [varchar](8) NOT NULL,
	[StyleUKey] [bigint] NOT NULL,
	[Version] [tinyint] NOT NULL,
	[FactoryID] [varchar](8) NOT NULL,
	[ComboType] [varchar](1) NULL,
	[SewingLineID] [varchar](5) NULL,
	[Team] [varchar](5) NULL,
	[IdealOperators] [tinyint] NULL,
	[CurrentOperators] [smallint] NULL,
	[StandardOutput] [int] NULL,
	[DailyDemand] [int] NULL,
	[Workhour] [numeric](4, 1) NULL,
	[NetTime] [int] NULL,
	[TaktTime] [int] NULL,
	[TotalGSD] [int] NULL,
	[TotalCycle] [int] NULL,
	[HighestGSD] [numeric](12, 2) NULL,
	[HighestCycle] [numeric](12, 2) NULL,
	[ID] [bigint] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Status] [varchar](15) NULL,
	[IEReasonID] [varchar](5) NULL,
	[AddName] [varchar](10) NULL,
	[AddDate] [datetime] NULL,
	[EditName] [varchar](10) NULL,
	[EditDate] [datetime] NULL,
	[TimeStudyPhase] [varchar](10) NOT NULL,
	[TimeStudyVersion] [varchar](2) NOT NULL,
	[IEReasonLBRNotHit_1stUkey] [bigint] NULL,
	[Phase] [varchar](20) NOT NULL,
	[OriTotalGSD] INT NULL DEFAULT ((0)), 
	TimeStudyID bigint not NULL CONSTRAINT [DF_LineMapping_TimeStudyID] DEFAULT 0,
    [JukiIoTDataExchange] BIT NOT NULL DEFAULT 0, 
    [LMMachineConfirmName] VARCHAR(10) NOT NULL DEFAULT (''), 
    [LMMachineConfirmDate] DATETIME NULL, 
    [JukiStyleDataSubmitDate] DATETIME NULL, 
    [JukiProdPlanDataSubmitDate] DATETIME NULL, 
    [JukiLayoutDataSubmitDate] DATETIME NULL, 
    CONSTRAINT [PK_LineMapping] PRIMARY KEY CLUSTERED 
(
	[StyleUKey] ASC,
	[Version] ASC,
	[FactoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[LineMapping] ADD  CONSTRAINT [DF_LineMapping_StyleID]  DEFAULT ('') FOR [StyleID]
GO

ALTER TABLE [dbo].[LineMapping] ADD  CONSTRAINT [DF_LineMapping_SeasonID]  DEFAULT ('') FOR [SeasonID]
GO

ALTER TABLE [dbo].[LineMapping] ADD  CONSTRAINT [DF_LineMapping_BrandID]  DEFAULT ('') FOR [BrandID]
GO

ALTER TABLE [dbo].[LineMapping] ADD  CONSTRAINT [DF_LineMapping_StyleUKey]  DEFAULT ((0)) FOR [StyleUKey]
GO

ALTER TABLE [dbo].[LineMapping] ADD  CONSTRAINT [DF_LineMapping_Version]  DEFAULT ((0)) FOR [Version]
GO

ALTER TABLE [dbo].[LineMapping] ADD  CONSTRAINT [DF_LineMapping_FactoryID]  DEFAULT ('') FOR [FactoryID]
GO

ALTER TABLE [dbo].[LineMapping] ADD  CONSTRAINT [DF_LineMapping_ComboType]  DEFAULT ('') FOR [ComboType]
GO

ALTER TABLE [dbo].[LineMapping] ADD  CONSTRAINT [DF_LineMapping_SewingLineID]  DEFAULT ('') FOR [SewingLineID]
GO

ALTER TABLE [dbo].[LineMapping] ADD  CONSTRAINT [DF_LineMapping_Team]  DEFAULT ('') FOR [Team]
GO

ALTER TABLE [dbo].[LineMapping] ADD  CONSTRAINT [DF_LineMapping_IdealOperators]  DEFAULT ((0)) FOR [IdealOperators]
GO

ALTER TABLE [dbo].[LineMapping] ADD  CONSTRAINT [DF_LineMapping_CurrentOperators]  DEFAULT ((0)) FOR [CurrentOperators]
GO

ALTER TABLE [dbo].[LineMapping] ADD  CONSTRAINT [DF_LineMapping_StandardOutput]  DEFAULT ((0)) FOR [StandardOutput]
GO

ALTER TABLE [dbo].[LineMapping] ADD  CONSTRAINT [DF_LineMapping_DailyDemand]  DEFAULT ((0)) FOR [DailyDemand]
GO

ALTER TABLE [dbo].[LineMapping] ADD  CONSTRAINT [DF_LineMapping_Workhour]  DEFAULT ((0)) FOR [Workhour]
GO

ALTER TABLE [dbo].[LineMapping] ADD  CONSTRAINT [DF_LineMapping_NetTime]  DEFAULT ((0)) FOR [NetTime]
GO

ALTER TABLE [dbo].[LineMapping] ADD  CONSTRAINT [DF_LineMapping_TaktTime]  DEFAULT ((0)) FOR [TaktTime]
GO

ALTER TABLE [dbo].[LineMapping] ADD  CONSTRAINT [DF_LineMapping_TotalGSD]  DEFAULT ((0)) FOR [TotalGSD]
GO

ALTER TABLE [dbo].[LineMapping] ADD  CONSTRAINT [DF_LineMapping_TotalCycle]  DEFAULT ((0)) FOR [TotalCycle]
GO

ALTER TABLE [dbo].[LineMapping] ADD  CONSTRAINT [DF_LineMapping_HighestGSD]  DEFAULT ((0)) FOR [HighestGSD]
GO

ALTER TABLE [dbo].[LineMapping] ADD  CONSTRAINT [DF_LineMapping_HighestCycle]  DEFAULT ((0)) FOR [HighestCycle]
GO

ALTER TABLE [dbo].[LineMapping] ADD  CONSTRAINT [DF_LineMapping_Status]  DEFAULT ('') FOR [Status]
GO

ALTER TABLE [dbo].[LineMapping] ADD  CONSTRAINT [DF_LineMapping_IEReasonID]  DEFAULT ('') FOR [IEReasonID]
GO

ALTER TABLE [dbo].[LineMapping] ADD  CONSTRAINT [DF_LineMapping_AddName]  DEFAULT ('') FOR [AddName]
GO

ALTER TABLE [dbo].[LineMapping] ADD  CONSTRAINT [DF_LineMapping_EditName]  DEFAULT ('') FOR [EditName]
GO

ALTER TABLE [dbo].[LineMapping] ADD  CONSTRAINT [DF_LineMapping_TimeStudyPhase]  DEFAULT ('') FOR [TimeStudyPhase]
GO

ALTER TABLE [dbo].[LineMapping] ADD  CONSTRAINT [DF_LineMapping_TimeStudyVersion]  DEFAULT ('') FOR [TimeStudyVersion]
GO

ALTER TABLE [dbo].[LineMapping] ADD  CONSTRAINT [DF_LineMapping_Phase]  DEFAULT ('') FOR [Phase]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'款式' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LineMapping', @level2type=N'COLUMN',@level2name=N'StyleID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'季別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LineMapping', @level2type=N'COLUMN',@level2name=N'SeasonID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'品牌' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LineMapping', @level2type=N'COLUMN',@level2name=N'BrandID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'StyleUkey' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LineMapping', @level2type=N'COLUMN',@level2name=N'StyleUKey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'版本' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LineMapping', @level2type=N'COLUMN',@level2name=N'Version'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LineMapping', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'套裝部位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LineMapping', @level2type=N'COLUMN',@level2name=N'ComboType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'生產線' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LineMapping', @level2type=N'COLUMN',@level2name=N'SewingLineID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'生產組別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LineMapping', @level2type=N'COLUMN',@level2name=N'Team'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'預計投入人數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LineMapping', @level2type=N'COLUMN',@level2name=N'IdealOperators'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'現行投入人數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LineMapping', @level2type=N'COLUMN',@level2name=N'CurrentOperators'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'標準產出' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LineMapping', @level2type=N'COLUMN',@level2name=N'StandardOutput'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'一天產出件數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LineMapping', @level2type=N'COLUMN',@level2name=N'DailyDemand'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'每日工作時數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LineMapping', @level2type=N'COLUMN',@level2name=N'Workhour'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Net available Time' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LineMapping', @level2type=N'COLUMN',@level2name=N'NetTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'每產出一件所需秒數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LineMapping', @level2type=N'COLUMN',@level2name=N'TaktTime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'GSD上的總秒數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LineMapping', @level2type=N'COLUMN',@level2name=N'TotalGSD'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'全部Cycle的總秒數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LineMapping', @level2type=N'COLUMN',@level2name=N'TotalCycle'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'各站中最高的GSD秒數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LineMapping', @level2type=N'COLUMN',@level2name=N'HighestGSD'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'各站中最高的Cycle秒數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LineMapping', @level2type=N'COLUMN',@level2name=N'HighestCycle'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LineMapping', @level2type=N'COLUMN',@level2name=N'ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'狀態' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LineMapping', @level2type=N'COLUMN',@level2name=N'Status'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Not hit target reason' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LineMapping', @level2type=N'COLUMN',@level2name=N'IEReasonID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增者' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LineMapping', @level2type=N'COLUMN',@level2name=N'AddName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LineMapping', @level2type=N'COLUMN',@level2name=N'AddDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'編輯者' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LineMapping', @level2type=N'COLUMN',@level2name=N'EditName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'編輯時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LineMapping', @level2type=N'COLUMN',@level2name=N'EditDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Fty GSD的 Phase' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LineMapping', @level2type=N'COLUMN',@level2name=N'TimeStudyPhase'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Fty GSD的 version' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LineMapping', @level2type=N'COLUMN',@level2name=N'TimeStudyVersion'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'IEReasonLBRNotHit_1st Ukey' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LineMapping', @level2type=N'COLUMN',@level2name=N'IEReasonLBRNotHit_1stUkey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Line Mapping' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LineMapping'
GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'P01  TotalGSD',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LineMapping',
    @level2type = N'COLUMN',
    @level2name = N'OriTotalGSD'
Go

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Fty GSD編號OTP驗證功能' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LineMapping', @level2type=N'COLUMN',@level2name=N'TimeStudyID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'是否交換給Juki-JaNets系統',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LineMapping',
    @level2type = N'COLUMN',
    @level2name = N'JukiIoTDataExchange'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'LM機器確認人員',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LineMapping',
    @level2type = N'COLUMN',
    @level2name = N'LMMachineConfirmName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'LM機器確認日期',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LineMapping',
    @level2type = N'COLUMN',
    @level2name = N'LMMachineConfirmDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'傳給SCI中間庫的時間',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LineMapping',
    @level2type = N'COLUMN',
    @level2name = N'JukiStyleDataSubmitDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'傳給SCI中間庫的時間',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LineMapping',
    @level2type = N'COLUMN',
    @level2name = N'JukiProdPlanDataSubmitDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'傳給SCI中間庫的時間',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LineMapping',
    @level2type = N'COLUMN',
    @level2name = N'JukiLayoutDataSubmitDate'