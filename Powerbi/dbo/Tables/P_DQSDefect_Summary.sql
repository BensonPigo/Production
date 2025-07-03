CREATE TABLE [dbo].[P_DQSDefect_Summary](
	[FirstInspectDate] [date] NOT NULL,
	[FactoryID] [varchar](8) NOT NULL,
	[BrandID] [varchar](8) NULL,
	[StyleID] [varchar](15) NULL,
	[POID] [varchar](30) NULL,
	[SPNO] [varchar](13) NOT NULL,
	[Article] [varchar](8) NOT NULL,
	[SizeCode] [varchar](8) NOT NULL,
	[Destination] [varchar](30) NULL,
	[CDCode] [varchar](6) NULL,
	[ProductionFamilyID] [varchar](20) NULL,
	[Team] [varchar](10) NULL,
	[QCName] [varchar](10) NOT NULL,
	[Shift] [varchar](5) NOT NULL,
	[Line] [varchar](5) NOT NULL,
	[Cell] [varchar](2) NULL,
	[InspectQty] [int] NULL,
	[RejectQty] [int] NULL,
	[WFT] [decimal](6, 3) NULL,
	[RFT] [decimal](10, 3) NULL,
	[CDCodeNew] [varchar](5) NULL,
	[ProductType] [nvarchar](500) NULL,
	[FabricType] [nvarchar](500) NULL,
	[Lining] [varchar](20) NULL,
	[Gender] [varchar](10) NULL,
	[Construction] [nvarchar](50) NULL,
	[Ukey] [bigint] IDENTITY(1,1) NOT NULL,
	[InspectionDate] [date] NOT NULL,
	[DefectQty] [int] NOT NULL,
	[BIFactoryID] [varchar](8) NOT NULL,
	[BIInsertDate] [datetime] NULL,
 CONSTRAINT [PK_P_DQSDefect_Summary] PRIMARY KEY CLUSTERED 
(
	[FactoryID] ASC,
	[FirstInspectDate] ASC,
	[SPNO] ASC,
	[Article] ASC,
	[SizeCode] ASC,
	[QCName] ASC,
	[Shift] ASC,
	[Line] ASC,
	[InspectionDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[P_DQSDefect_Summary] ADD  CONSTRAINT [DF_P_DQSDefect_Summary_DefectQty]  DEFAULT ((0)) FOR [DefectQty]
GO

ALTER TABLE [dbo].[P_DQSDefect_Summary] ADD  CONSTRAINT [DF_P_DQSDefect_Summary_BIFactoryID]  DEFAULT ('') FOR [BIFactoryID]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'實際產出日 (Last Inspection Date)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_DQSDefect_Summary', @level2type=N'COLUMN',@level2name=N'InspectionDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Defect數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_DQSDefect_Summary', @level2type=N'COLUMN',@level2name=N'DefectQty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_DQSDefect_Summary', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_DQSDefect_Summary', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO