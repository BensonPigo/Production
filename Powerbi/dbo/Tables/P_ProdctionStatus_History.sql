CREATE TABLE [dbo].[P_ProdctionStatus_History](
	[HistoryUkey] [bigint] IDENTITY(1,1) NOT NULL,
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
 CONSTRAINT [PK_P_ProdctionStatus_History] PRIMARY KEY CLUSTERED 
(
	[HistoryUkey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[P_ProdctionStatus_History] ADD  CONSTRAINT [DF_P_ProdctionStatus_History_SewingLineID]  DEFAULT ('') FOR [SewingLineID]
GO

ALTER TABLE [dbo].[P_ProdctionStatus_History] ADD  CONSTRAINT [DF_P_ProdctionStatus_History_FactoryID]  DEFAULT ('') FOR [FactoryID]
GO

ALTER TABLE [dbo].[P_ProdctionStatus_History] ADD  CONSTRAINT [DF_P_ProdctionStatus_History_SPNO]  DEFAULT ('') FOR [SPNO]
GO

ALTER TABLE [dbo].[P_ProdctionStatus_History] ADD  CONSTRAINT [DF_P_ProdctionStatus_History_StyleID]  DEFAULT ('') FOR [StyleID]
GO

ALTER TABLE [dbo].[P_ProdctionStatus_History] ADD  CONSTRAINT [DF_P_ProdctionStatus_History_StyleName]  DEFAULT ('') FOR [StyleName]
GO

ALTER TABLE [dbo].[P_ProdctionStatus_History] ADD  CONSTRAINT [DF_P_ProdctionStatus_History_ComboType]  DEFAULT ('') FOR [ComboType]
GO

ALTER TABLE [dbo].[P_ProdctionStatus_History] ADD  CONSTRAINT [DF_P_ProdctionStatus_History_SPCategory]  DEFAULT ('') FOR [SPCategory]
GO

ALTER TABLE [dbo].[P_ProdctionStatus_History] ADD  CONSTRAINT [DF_P_ProdctionStatus_History_OrderQty]  DEFAULT ((0)) FOR [OrderQty]
GO

ALTER TABLE [dbo].[P_ProdctionStatus_History] ADD  CONSTRAINT [DF_P_ProdctionStatus_History_AlloQty]  DEFAULT ((0)) FOR [AlloQty]
GO

ALTER TABLE [dbo].[P_ProdctionStatus_History] ADD  CONSTRAINT [DF_P_ProdctionStatus_History_SewingQty]  DEFAULT ((0)) FOR [SewingQty]
GO

ALTER TABLE [dbo].[P_ProdctionStatus_History] ADD  CONSTRAINT [DF_P_ProdctionStatus_History_SewingBalance]  DEFAULT ((0)) FOR [SewingBalance]
GO

ALTER TABLE [dbo].[P_ProdctionStatus_History] ADD  CONSTRAINT [DF_P_ProdctionStatus_History_TtlSewingQtyByComboType]  DEFAULT ((0)) FOR [TtlSewingQtyByComboType]
GO

ALTER TABLE [dbo].[P_ProdctionStatus_History] ADD  CONSTRAINT [DF_P_ProdctionStatus_History_TtlSewingQtyBySP]  DEFAULT ((0)) FOR [TtlSewingQtyBySP]
GO

ALTER TABLE [dbo].[P_ProdctionStatus_History] ADD  CONSTRAINT [DF_P_ProdctionStatus_History_ClogQty]  DEFAULT ((0)) FOR [ClogQty]
GO

ALTER TABLE [dbo].[P_ProdctionStatus_History] ADD  CONSTRAINT [DF_P_ProdctionStatus_History_TtlClogBalance]  DEFAULT ((0)) FOR [TtlClogBalance]
GO

ALTER TABLE [dbo].[P_ProdctionStatus_History] ADD  CONSTRAINT [DF_P_ProdctionStatus_History_DaysOffToDDSched]  DEFAULT ('') FOR [DaysOffToDDSched]
GO

ALTER TABLE [dbo].[P_ProdctionStatus_History] ADD  CONSTRAINT [DF_P_ProdctionStatus_History_DaysTodayToDD]  DEFAULT ('') FOR [DaysTodayToDD]
GO

ALTER TABLE [dbo].[P_ProdctionStatus_History] ADD  CONSTRAINT [DF_P_ProdctionStatus_History_NeedQtyByStdOut]  DEFAULT ('') FOR [NeedQtyByStdOut]
GO

ALTER TABLE [dbo].[P_ProdctionStatus_History] ADD  CONSTRAINT [DF_P_ProdctionStatus_History_Pending]  DEFAULT ('') FOR [Pending]
GO

ALTER TABLE [dbo].[P_ProdctionStatus_History] ADD  CONSTRAINT [DF_P_ProdctionStatus_History_TotalStandardOutput]  DEFAULT ((0)) FOR [TotalStandardOutput]
GO

ALTER TABLE [dbo].[P_ProdctionStatus_History] ADD  CONSTRAINT [DF_P_ProdctionStatus_History_DaysToDrainByStdOut]  DEFAULT ('') FOR [DaysToDrainByStdOut]
GO

ALTER TABLE [dbo].[P_ProdctionStatus_History] ADD  CONSTRAINT [DF_P_ProdctionStatus_History_DaysOffToDDByStdOut]  DEFAULT ('') FOR [DaysOffToDDByStdOut]
GO

ALTER TABLE [dbo].[P_ProdctionStatus_History] ADD  CONSTRAINT [DF_P_ProdctionStatus_History_MaxOutput]  DEFAULT ('') FOR [MaxOutput]
GO

ALTER TABLE [dbo].[P_ProdctionStatus_History] ADD  CONSTRAINT [DF_P_ProdctionStatus_History_DaysToDrainByMaxOut]  DEFAULT ('') FOR [DaysToDrainByMaxOut]
GO

ALTER TABLE [dbo].[P_ProdctionStatus_History] ADD  CONSTRAINT [DF_P_ProdctionStatus_History_DaysOffToDDByMaxOut]  DEFAULT ('') FOR [DaysOffToDDByMaxOut]
GO

ALTER TABLE [dbo].[P_ProdctionStatus_History] ADD  CONSTRAINT [DF_P_ProdctionStatus_History_TightByMaxOut]  DEFAULT ('') FOR [TightByMaxOut]
GO

ALTER TABLE [dbo].[P_ProdctionStatus_History] ADD  CONSTRAINT [DF_P_ProdctionStatus_History_TightByStdOut]  DEFAULT ('') FOR [TightByStdOut]
GO

ALTER TABLE [dbo].[P_ProdctionStatus_History] ADD  CONSTRAINT [DF_P_ProdctionStatus_History_BIFactoryID]  DEFAULT ('') FOR [BIFactoryID]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProdctionStatus_History', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_ProdctionStatus_History', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
