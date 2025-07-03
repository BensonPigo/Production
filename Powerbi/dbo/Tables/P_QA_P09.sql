CREATE TABLE [dbo].[P_QA_P09](
	[WK#] [varchar](13) NULL,
	[Invoice#] [varchar](max) NULL,
	[ATA] [date] NULL,
	[ETA] [date] NULL,
	[Season] [varchar](10) NULL,
	[SP#] [varchar](13) NULL,
	[Seq#] [varchar](10) NULL,
	[Brand] [varchar](8) NULL,
	[Supp] [varchar](6) NULL,
	[Supp Name] [varchar](12) NULL,
	[Ref#] [varchar](36) NULL,
	[Color] [nvarchar](150) NULL,
	[Qty] [numeric](12, 2) NULL,
	[Inspection Report_Fty Received Date] [date] NULL,
	[Inspection Report_Supp Sent Date] [date] NULL,
	[Test Report_Fty Received Date] [date] NULL,
	[Test Report_ Check Clima] [bit] NULL,
	[Test Report_Supp Sent Date] [date] NULL,
	[Continuity Card_Fty Received Date] [date] NULL,
	[Continuity Card_Supp Sent Date] [date] NULL,
	[Continuity Card_AWB#] [varchar](30) NULL,
	[1st Bulk Dyelot_Fty Received Date] [date] NULL,
	[1st Bulk Dyelot_Supp Sent Date] [varchar](100) NULL,
	[T2 Inspected Yards] [numeric](10, 2) NULL,
	[T2 Defect Points] [numeric](5, 0) NULL,
	[Grade] [varchar](1) NULL,
	[T1 Inspected Yards] [numeric](10, 2) NULL,
	[T1 Defect Points] [numeric](6, 0) NULL,
	[Fabric with clima] [bit] NULL,
	[FactoryID] [varchar](8) NOT NULL,
	[Ukey] [bigint] IDENTITY(1,1) NOT NULL,
	[Consignee] [varchar](8) NOT NULL,
	[BIFactoryID] [varchar](8) NOT NULL,
	[BIInsertDate] [datetime] NULL,
 CONSTRAINT [PK_P_QA_P09] PRIMARY KEY CLUSTERED 
(
	[Ukey] ASC,
	[FactoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[P_QA_P09] ADD  CONSTRAINT [DF_P_QA_P09_Consignee]  DEFAULT ('') FOR [Consignee]
GO

ALTER TABLE [dbo].[P_QA_P09] ADD  CONSTRAINT [DF_P_QA_P09_BIFactoryID]  DEFAULT ('') FOR [BIFactoryID]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_QA_P09', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_QA_P09', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO