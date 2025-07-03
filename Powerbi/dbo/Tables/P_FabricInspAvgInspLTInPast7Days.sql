CREATE TABLE [dbo].[P_FabricInspAvgInspLTInPast7Days](
	[TransferDate] [date] NOT NULL,
	[FactoryID] [varchar](8) NOT NULL,
	[AvgInspLTInPast7Days] [numeric](6, 2) NOT NULL,
	[BIFactoryID] [varchar](8) NOT NULL,
	[BIInsertDate] [datetime] NULL,
 CONSTRAINT [PK_P_FabricInspAvgInspLTInPast7Days] PRIMARY KEY CLUSTERED 
(
	[TransferDate] ASC,
	[FactoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[P_FabricInspAvgInspLTInPast7Days] ADD  CONSTRAINT [DF_P_FabricInspAvgInspLTInPast7Days_FactoryID]  DEFAULT ('') FOR [FactoryID]
GO

ALTER TABLE [dbo].[P_FabricInspAvgInspLTInPast7Days] ADD  CONSTRAINT [DF_P_FabricInspAvgInspLTInPast7Days_AvgInspLTInPast7Days]  DEFAULT ((0)) FOR [AvgInspLTInPast7Days]
GO

ALTER TABLE [dbo].[P_FabricInspAvgInspLTInPast7Days] ADD  CONSTRAINT [DF_P_FabricInspAvgInspLTInPast7Days_BIFactoryID]  DEFAULT ('') FOR [BIFactoryID]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Addate' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspAvgInspLTInPast7Days', @level2type=N'COLUMN',@level2name=N'TransferDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'生產廠的FactoryID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspAvgInspLTInPast7Days', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'生產廠的FactoryID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspAvgInspLTInPast7Days', @level2type=N'COLUMN',@level2name=N'AvgInspLTInPast7Days'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspAvgInspLTInPast7Days', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspAvgInspLTInPast7Days', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO