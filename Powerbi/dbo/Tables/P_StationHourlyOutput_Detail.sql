CREATE TABLE [dbo].[P_StationHourlyOutput_Detail](
	[Ukey] [int] NOT NULL,
	[FactoryID] [varchar](8) NOT NULL,
	[StationHourlyOutputUkey] [bigint] NOT NULL,
	[Oclock] [tinyint] NOT NULL,
	[Qty] [int] NOT NULL,
	[BIFactoryID] [varchar](8) NOT NULL,
	[BIInsertDate] [datetime] NULL,
 CONSTRAINT [PK_P_StationHourlyOutput_Detail] PRIMARY KEY CLUSTERED 
(
	[FactoryID] ASC,
	[Ukey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[P_StationHourlyOutput_Detail] ADD  CONSTRAINT [DF_P_StationHourlyOutput_Detail_Ukey]  DEFAULT ((0)) FOR [Ukey]
GO

ALTER TABLE [dbo].[P_StationHourlyOutput_Detail] ADD  CONSTRAINT [DF_P_StationHourlyOutput_Detail_FactoryID]  DEFAULT ('') FOR [FactoryID]
GO

ALTER TABLE [dbo].[P_StationHourlyOutput_Detail] ADD  CONSTRAINT [DF_P_StationHourlyOutput_Detail_StationHourlyOutputUkey]  DEFAULT ((0)) FOR [StationHourlyOutputUkey]
GO

ALTER TABLE [dbo].[P_StationHourlyOutput_Detail] ADD  CONSTRAINT [DF_P_StationHourlyOutput_Detail_Oclock]  DEFAULT ((0)) FOR [Oclock]
GO

ALTER TABLE [dbo].[P_StationHourlyOutput_Detail] ADD  CONSTRAINT [DF_P_StationHourlyOutput_Detail_Qty]  DEFAULT ((0)) FOR [Qty]
GO

ALTER TABLE [dbo].[P_StationHourlyOutput_Detail] ADD  CONSTRAINT [DF_P_StationHourlyOutput_Detail_BIFactoryID]  DEFAULT ('') FOR [BIFactoryID]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ukey' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StationHourlyOutput_Detail', @level2type=N'COLUMN',@level2name=N'Ukey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StationHourlyOutput_Detail', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'StationHourlyOutputUkey' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StationHourlyOutput_Detail', @level2type=N'COLUMN',@level2name=N'StationHourlyOutputUkey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間區間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StationHourlyOutput_Detail', @level2type=N'COLUMN',@level2name=N'Oclock'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'成衣件數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StationHourlyOutput_Detail', @level2type=N'COLUMN',@level2name=N'Qty'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N' 記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StationHourlyOutput_Detail', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StationHourlyOutput_Detail', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO