CREATE TABLE [dbo].[P_DQSDefect_Detail](
	[FtyZon] [varchar](8) NULL,
	[BrandID] [varchar](8) NULL,
	[BuyerDelivery] [date] NULL,
	[Line] [varchar](5) NULL,
	[FactoryID] [varchar](8) NOT NULL,
	[Team] [varchar](8) NULL,
	[Shift] [varchar](5) NULL,
	[POID] [varchar](30) NULL,
	[StyleID] [varchar](15) NULL,
	[SPNO] [varchar](13) NULL,
	[Article] [varchar](8) NULL,
	[Status] [varchar](7) NULL,
	[FixType] [varchar](12) NULL,
	[FirstInspectDate] [date] NULL,
	[FirstInspectTime] [time](7) NULL,
	[InspectQCName] [nvarchar](30) NULL,
	[FixedTime] [varchar](16) NULL,
	[FixedQCName] [nvarchar](30) NULL,
	[ProductType] [varchar](6) NULL,
	[SizeCode] [varchar](8) NULL,
	[DefectTypeDesc] [nvarchar](60) NULL,
	[DefectCodeDesc] [nvarchar](100) NULL,
	[AreaCode] [varchar](50) NULL,
	[ReworkCardNo] [varchar](2) NULL,
	[GarmentDefectTypeID] [varchar](1) NULL,
	[GarmentDefectCodeID] [varchar](3) NULL,
	[Ukey] [bigint] IDENTITY(1,1) NOT NULL,
	[DefectCodeLocalDesc] [nvarchar](100) NOT NULL,
	[IsCriticalDefect] [varchar](1) NOT NULL,
	[BIFactoryID] [varchar](8) NOT NULL,
	[BIInsertDate] [datetime] NULL,
	[InspectionDetailUkey] [bigint] NOT NULL,
 CONSTRAINT [PK_P_DQSDefect_Detail] PRIMARY KEY CLUSTERED 
(
	[Ukey] ASC,
	[FactoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[P_DQSDefect_Detail] ADD  CONSTRAINT [DF_P_DQSDefect_Detail_DefectCodeLocalDesc]  DEFAULT ('') FOR [DefectCodeLocalDesc]
GO

ALTER TABLE [dbo].[P_DQSDefect_Detail] ADD  CONSTRAINT [DF_P_DQSDefect_Detail_IsCriticalDefect]  DEFAULT ('') FOR [IsCriticalDefect]
GO

ALTER TABLE [dbo].[P_DQSDefect_Detail] ADD  CONSTRAINT [DF_P_DQSDefect_Detail_BIFactoryID]  DEFAULT ('') FOR [BIFactoryID]
GO

ALTER TABLE [dbo].[P_DQSDefect_Detail] ADD  CONSTRAINT [DF_P_DQSDefect_Detail_InspectionDetailUkey]  DEFAULT ((0)) FOR [InspectionDetailUkey]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'DefectCode當地描述' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_DQSDefect_Detail', @level2type=N'COLUMN',@level2name=N'DefectCodeLocalDesc'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否為嚴重defect code' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_DQSDefect_Detail', @level2type=N'COLUMN',@level2name=N'IsCriticalDefect'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_DQSDefect_Detail', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_DQSDefect_Detail', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO