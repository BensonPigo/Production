CREATE TABLE [dbo].[P_LineMapping_History](
	[HistoryUkey] [bigint] IDENTITY(1,1) NOT NULL,
	[FactoryID] [varchar](8) NOT NULL,
	[StyleUKey] [bigint] NOT NULL,
	[ComboType] [varchar](1) NOT NULL,
	[Version] [tinyint] NOT NULL,
	[Phase] [varchar](7) NOT NULL,
	[SewingLine] [varchar](8) NOT NULL,
	[IsFrom] [varchar](6) NOT NULL,
	[Team] [varchar](8) NOT NULL,
	[BIFactoryID] [varchar](8) NOT NULL,
	[BIInsertDate] [datetime] NOT NULL,
 CONSTRAINT [PK_P_LineMapping_History] PRIMARY KEY CLUSTERED 
(
	[HistoryUkey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping_History', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'串Style.Ukey' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping_History', @level2type=N'COLUMN',@level2name=N'StyleUKey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'套裝部位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping_History', @level2type=N'COLUMN',@level2name=N'ComboType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ALM版號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping_History', @level2type=N'COLUMN',@level2name=N'Version'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ALM階段' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping_History', @level2type=N'COLUMN',@level2name=N'Phase'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'生產線' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping_History', @level2type=N'COLUMN',@level2name=N'SewingLine'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'資料來源為IE P03或IE P06' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping_History', @level2type=N'COLUMN',@level2name=N'IsFrom'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'生產組別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping_History', @level2type=N'COLUMN',@level2name=N'Team'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping_History', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_LineMapping_History', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO