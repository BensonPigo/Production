CREATE TABLE [dbo].[P_SewingLineSchedule](
	[APSNo] [int] NOT NULL,
	[SewingLineID] [varchar](5) NOT NULL,
	[SewingDay] [date] NOT NULL,
	[SewingStartTime] [datetime] NULL,
	[SewingEndTime] [datetime] NULL,
	[MDivisionID] [varchar](8) NULL,
	[FactoryID] [varchar](8) NOT NULL,
	[PO] [nvarchar](max) NULL,
	[POCount] [bigint] NULL,
	[SP] [nvarchar](max) NULL,
	[SPCount] [int] NULL,
	[EarliestSCIdelivery] [date] NULL,
	[LatestSCIdelivery] [date] NULL,
	[EarliestBuyerdelivery] [date] NULL,
	[LatestBuyerdelivery] [date] NULL,
	[Category] [nvarchar](max) NULL,
	[Colorway] [nvarchar](max) NULL,
	[ColorwayCount] [bigint] NULL,
	[CDCode] [nvarchar](max) NULL,
	[ProductionFamilyID] [nvarchar](max) NULL,
	[Style] [nvarchar](max) NULL,
	[StyleCount] [bigint] NULL,
	[OrderQty] [int] NULL,
	[AlloQty] [int] NULL,
	[StardardOutputPerDay] [float] NULL,
	[CPU] [float] NULL,
	[WorkHourPerDay] [float] NULL,
	[StardardOutputPerHour] [float] NULL,
	[Efficienycy] [numeric](10, 2) NULL,
	[ScheduleEfficiency] [numeric](11, 2) NULL,
	[LineEfficiency] [numeric](11, 2) NULL,
	[LearningCurve] [numeric](17, 2) NULL,
	[SewingInline] [datetime] NULL,
	[SewingOffline] [datetime] NULL,
	[PFRemark] [varchar](1) NULL,
	[MTLComplete] [varchar](1) NULL,
	[KPILETA] [date] NULL,
	[MTLETA] [date] NULL,
	[ArtworkType] [nvarchar](max) NULL,
	[InspectionDate] [date] NULL,
	[Remarks] [nvarchar](max) NULL,
	[CuttingOutput] [numeric](38, 2) NULL,
	[SewingOutput] [int] NULL,
	[ScannedQty] [int] NULL,
	[ClogQty] [int] NULL,
	[Sewer] [int] NOT NULL,
	[SewingCPU] [numeric](10, 2) NULL,
	[BrandID] [nvarchar](500) NULL,
	[Orig_WorkHourPerDay] [float] NULL,
	[New_SwitchTime] [float] NULL,
	[FirststCuttingOutputDate] [date] NULL,
	[TTL_PRINTING (PCS)] [numeric](38, 6) NULL,
	[TTL_PRINTING PPU (PPU)] [numeric](38, 6) NULL,
	[SubCon] [nvarchar](max) NULL,
	[CDCodeNew] [varchar](max) NULL,
	[ProductType] [nvarchar](max) NULL,
	[FabricType] [nvarchar](max) NULL,
	[Lining] [varchar](max) NULL,
	[Gender] [varchar](max) NULL,
	[Construction] [nvarchar](max) NULL,
	[Subcon Qty] [int] NULL,
	[Std Qty for printing] [int] NULL,
	[StyleName] [nvarchar](max) NULL,
	[StdQtyEMB] [varchar](50) NULL,
	[EMBStitch] [varchar](50) NULL,
	[EMBStitchCnt] [int] NULL,
	[TtlQtyEMB] [int] NULL,
	[PrintPcs] [int] NULL,
	[Ukey] [bigint] IDENTITY(1,1) NOT NULL,
	[InlineCategory] [varchar](80) NULL,
	[StyleSeason] [nvarchar](max) NOT NULL,
	[AddDate] [datetime] NULL,
	[EditDate] [datetime] NULL,
    [LastDownloadAPSDate] [datetime] NULL
 CONSTRAINT [PK_P_SewingLineSchedule] PRIMARY KEY CLUSTERED 
(
	[APSNo] ASC,
	[SewingDay] ASC,
	[SewingLineID] ASC,
	[Sewer] ASC,
	[FactoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY], 
    [SewingInlineCategory] VARCHAR(50) NOT NULL DEFAULT (''), 
    [BIFactoryID] VARCHAR(8) NOT NULL DEFAULT (''), 
    [BIInsertDate] DATETIME NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[P_SewingLineSchedule] ADD  CONSTRAINT [DF_P_SewingLineSchedule_StyleName]  DEFAULT ('') FOR [StyleName]
GO

ALTER TABLE [dbo].[P_SewingLineSchedule] ADD  CONSTRAINT [DF_P_SewingLineSchedule_StyleSeason]  DEFAULT ('') FOR [StyleSeason]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'款式名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingLineSchedule', @level2type=N'COLUMN',@level2name=N'StyleName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SewingLineSchedule.AddDate' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingLineSchedule', @level2type=N'COLUMN',@level2name=N'AddDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SewingLineSchedule.EditDate' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SewingLineSchedule', @level2type=N'COLUMN',@level2name=N'EditDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Sewing output InlineCategory By Style',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_SewingLineSchedule',
    @level2type = N'COLUMN',
    @level2name = N'SewingInlineCategory'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'記錄哪間工廠的資料，ex PH1, PH2',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_SewingLineSchedule',
    @level2type = N'COLUMN',
    @level2name = N'BIFactoryID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'時間戳記，紀錄寫入table時間',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_SewingLineSchedule',
    @level2type = N'COLUMN',
    @level2name = N'BIInsertDate'