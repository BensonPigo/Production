CREATE TABLE [dbo].[P_ProdctionStatus](
	[SewingLineID] [varchar](5) NOT NULL,
	[FactoryID] [varchar](8) NOT NULL,
	[SPNO] [varchar](13) NOT NULL,
	[StyleID] [varchar](15) NOT NULL,
	[StyleName] [nvarchar](50) NOT NULL,
	[ComboType] [varchar](1) NOT NULL,
	[SPCategory] [varchar](1) NOT NULL,
	[SCIDelivery] [date] NULL,
	[BuyerDelivery] [date] NULL,
	[InlineDate] [datetime] NOT NULL,
	[OfflineDate] [datetime] NOT NULL,
	[OrderQty] [int] NOT NULL,
	[AlloQty] [int] NOT NULL,
	[SewingQty] [int] NOT NULL,
	[SewingBalance] [int] NOT NULL,
	[TtlSewingQtyByComboType] [int] NOT NULL,
	[TtlSewingQtyBySP] [int] NOT NULL,
	[ClogQty] [int] NOT NULL,
	[TtlClogBalance] [int] NOT NULL,
	[DaysOffToDDSched] [varchar](8) NOT NULL,
	[DaysTodayToDD] [varchar](8) NOT NULL,
	[NeedQtyByStdOut] [varchar](8) NOT NULL,
	[Pending] [varchar](1) NOT NULL,
	[TotalStandardOutput] [numeric](11, 6) NOT NULL,
	[DaysToDrainByStdOut] [varchar](8) NOT NULL,
	[OfflineDateByStdOut] [datetime] NULL,
	[DaysOffToDDByStdOut] [varchar](8) NOT NULL,
	[MaxOutput] [varchar](8) NOT NULL,
	[DaysToDrainByMaxOut] [varchar](8) NOT NULL,
	[OfflineDateByMaxOut] [datetime] NULL,
	[DaysOffToDDByMaxOut] [varchar](8) NOT NULL,
	[TightByMaxOut] [varchar](1) NOT NULL,
	[TightByStdOut] [varchar](1) NOT NULL,
	[BIFactoryID] [varchar](8) NOT NULL,
	[BIInsertDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[SewingLineID] ASC,
	[FactoryID] ASC,
	[SPNO] ASC,
	[ComboType] ASC,
	[InlineDate] ASC,
	[OfflineDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[P_ProdctionStatus] ADD  CONSTRAINT [DF_P_ProdctionStatus_SewingLineID]  DEFAULT ('') FOR [SewingLineID]
GO

ALTER TABLE [dbo].[P_ProdctionStatus] ADD  CONSTRAINT [DF_P_ProdctionStatus_FactoryID]  DEFAULT ('') FOR [FactoryID]
GO

ALTER TABLE [dbo].[P_ProdctionStatus] ADD  CONSTRAINT [DF_P_ProdctionStatus_SPNO]  DEFAULT ('') FOR [SPNO]
GO

ALTER TABLE [dbo].[P_ProdctionStatus] ADD  CONSTRAINT [DF_P_ProdctionStatus_StyleID]  DEFAULT ('') FOR [StyleID]
GO

ALTER TABLE [dbo].[P_ProdctionStatus] ADD  CONSTRAINT [DF_P_ProdctionStatus_StyleName]  DEFAULT ('') FOR [StyleName]
GO

ALTER TABLE [dbo].[P_ProdctionStatus] ADD  CONSTRAINT [DF_P_ProdctionStatus_ComboType]  DEFAULT ('') FOR [ComboType]
GO

ALTER TABLE [dbo].[P_ProdctionStatus] ADD  CONSTRAINT [DF_P_ProdctionStatus_SPCategory]  DEFAULT ('') FOR [SPCategory]
GO

ALTER TABLE [dbo].[P_ProdctionStatus] ADD  CONSTRAINT [DF_P_ProdctionStatus_OrderQty]  DEFAULT ((0)) FOR [OrderQty]
GO

ALTER TABLE [dbo].[P_ProdctionStatus] ADD  CONSTRAINT [DF_P_ProdctionStatus_AlloQty]  DEFAULT ((0)) FOR [AlloQty]
GO

ALTER TABLE [dbo].[P_ProdctionStatus] ADD  CONSTRAINT [DF_P_ProdctionStatus_SewingQty]  DEFAULT ((0)) FOR [SewingQty]
GO

ALTER TABLE [dbo].[P_ProdctionStatus] ADD  CONSTRAINT [DF_P_ProdctionStatus_SewingBalance]  DEFAULT ((0)) FOR [SewingBalance]
GO

ALTER TABLE [dbo].[P_ProdctionStatus] ADD  CONSTRAINT [DF_P_ProdctionStatus_TtlSewingQtyByComboType]  DEFAULT ((0)) FOR [TtlSewingQtyByComboType]
GO

ALTER TABLE [dbo].[P_ProdctionStatus] ADD  CONSTRAINT [DF_P_ProdctionStatus_TtlSewingQtyBySP]  DEFAULT ((0)) FOR [TtlSewingQtyBySP]
GO

ALTER TABLE [dbo].[P_ProdctionStatus] ADD  CONSTRAINT [DF_P_ProdctionStatus_ClogQty]  DEFAULT ((0)) FOR [ClogQty]
GO

ALTER TABLE [dbo].[P_ProdctionStatus] ADD  CONSTRAINT [DF_P_ProdctionStatus_TtlClogBalance]  DEFAULT ((0)) FOR [TtlClogBalance]
GO

ALTER TABLE [dbo].[P_ProdctionStatus] ADD  CONSTRAINT [DF_P_ProdctionStatus_DaysOffToDDSched]  DEFAULT ('') FOR [DaysOffToDDSched]
GO

ALTER TABLE [dbo].[P_ProdctionStatus] ADD  CONSTRAINT [DF_P_ProdctionStatus_DaysTodayToDD]  DEFAULT ('') FOR [DaysTodayToDD]
GO

ALTER TABLE [dbo].[P_ProdctionStatus] ADD  CONSTRAINT [DF_P_ProdctionStatus_NeedQtyByStdOut]  DEFAULT ('') FOR [NeedQtyByStdOut]
GO

ALTER TABLE [dbo].[P_ProdctionStatus] ADD  CONSTRAINT [DF_P_ProdctionStatus_Pending]  DEFAULT ('') FOR [Pending]
GO

ALTER TABLE [dbo].[P_ProdctionStatus] ADD  CONSTRAINT [DF_P_ProdctionStatus_TotalStandardOutput]  DEFAULT ((0)) FOR [TotalStandardOutput]
GO

ALTER TABLE [dbo].[P_ProdctionStatus] ADD  CONSTRAINT [DF_P_ProdctionStatus_DaysToDrainByStdOut]  DEFAULT ('') FOR [DaysToDrainByStdOut]
GO

ALTER TABLE [dbo].[P_ProdctionStatus] ADD  CONSTRAINT [DF_P_ProdctionStatus_DaysOffToDDByStdOut]  DEFAULT ('') FOR [DaysOffToDDByStdOut]
GO

ALTER TABLE [dbo].[P_ProdctionStatus] ADD  CONSTRAINT [DF_P_ProdctionStatus_MaxOutput]  DEFAULT ('') FOR [MaxOutput]
GO

ALTER TABLE [dbo].[P_ProdctionStatus] ADD  CONSTRAINT [DF_P_ProdctionStatus_DaysToDrainByMaxOut]  DEFAULT ('') FOR [DaysToDrainByMaxOut]
GO

ALTER TABLE [dbo].[P_ProdctionStatus] ADD  CONSTRAINT [DF_P_ProdctionStatus_DaysOffToDDByMaxOut]  DEFAULT ('') FOR [DaysOffToDDByMaxOut]
GO

ALTER TABLE [dbo].[P_ProdctionStatus] ADD  CONSTRAINT [DF_P_ProdctionStatus_TightByMaxOut]  DEFAULT ('') FOR [TightByMaxOut]
GO

ALTER TABLE [dbo].[P_ProdctionStatus] ADD  CONSTRAINT [DF_P_ProdctionStatus_TightByStdOut]  DEFAULT ('') FOR [TightByStdOut]
GO

ALTER TABLE [dbo].[P_ProdctionStatus] ADD  CONSTRAINT [DF_P_ProdctionStatus_BIFactoryID]  DEFAULT ('') FOR [BIFactoryID]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProdctionStatus', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProdctionStatus', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO