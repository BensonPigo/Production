CREATE TABLE [dbo].[P_OutstandingPOStatus](
	[Buyerdelivery] [date] NOT NULL,
	[FTYGroup] [varchar](3) NOT NULL,
	[TotalCMPQty] [int] NOT NULL,
	[TotalClogCtn] [int] NOT NULL,
	[NotYet3rdSPCount] [int] NOT NULL,
	[BIFactoryID] [varchar](8) NOT NULL,
	[BIInsertDate] [datetime] NULL,
 CONSTRAINT [PK_P_OutstandingPOStatus] PRIMARY KEY CLUSTERED 
(
	[Buyerdelivery] ASC,
	[FTYGroup] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[P_OutstandingPOStatus] ADD  CONSTRAINT [DF_P_OutstandingPOStatus_TotalCMPQty]  DEFAULT ((0)) FOR [TotalCMPQty]
GO

ALTER TABLE [dbo].[P_OutstandingPOStatus] ADD  CONSTRAINT [DF_P_OutstandingPOStatus_TotalClogCtn]  DEFAULT ((0)) FOR [TotalClogCtn]
GO

ALTER TABLE [dbo].[P_OutstandingPOStatus] ADD  CONSTRAINT [DF_P_OutstandingPOStatus_NotYet3rdSPCount]  DEFAULT ((0)) FOR [NotYet3rdSPCount]
GO

ALTER TABLE [dbo].[P_OutstandingPOStatus] ADD  CONSTRAINT [DF_P_OutstandingPOStatus_BIFactoryID]  DEFAULT ('') FOR [BIFactoryID]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_OutstandingPOStatus', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_OutstandingPOStatus', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO