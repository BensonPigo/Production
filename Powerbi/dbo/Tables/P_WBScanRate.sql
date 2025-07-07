CREATE TABLE [dbo].[P_WBScanRate](
	[Date] [date] NOT NULL,
	[FactoryID] [varchar](8) NOT NULL,
	[WBScanRate] [numeric](5, 2) NOT NULL,
	[TTLRFIDSewInlineQty] [int] NOT NULL,
	[TTLSewQty] [int] NOT NULL,
	[BIFactoryID] [varchar](8) NOT NULL,
	[BIInsertDate] [datetime] NULL,
 CONSTRAINT [PK_P_WBScanRate] PRIMARY KEY CLUSTERED 
(
	[Date] ASC,
	[FactoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[P_WBScanRate] ADD  CONSTRAINT [DF_P_WBScanRate_WBScanRate]  DEFAULT ((0)) FOR [WBScanRate]
GO

ALTER TABLE [dbo].[P_WBScanRate] ADD  CONSTRAINT [DF_P_WBScanRate_TTLRFIDSewInlineQty]  DEFAULT ((0)) FOR [TTLRFIDSewInlineQty]
GO

ALTER TABLE [dbo].[P_WBScanRate] ADD  CONSTRAINT [DF_P_WBScanRate_TTLSewQty]  DEFAULT ((0)) FOR [TTLSewQty]
GO

ALTER TABLE [dbo].[P_WBScanRate] ADD  CONSTRAINT [DF_P_WBScanRate_BIFactoryID]  DEFAULT ('') FOR [BIFactoryID]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_WBScanRate', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_WBScanRate', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO