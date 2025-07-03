CREATE TABLE [dbo].[P_StationHourlyOutput](
	[FactoryID] [varchar](8) NOT NULL,
	[Date] [date] NULL,
	[Shift] [varchar](5) NOT NULL,
	[Team] [varchar](10) NOT NULL,
	[Line] [varchar](5) NOT NULL,
	[Station] [varchar](2) NOT NULL,
	[Capacity] [int] NOT NULL,
	[Target] [int] NOT NULL,
	[TotalOutput] [int] NOT NULL,
	[ProblemsEncounter] [nvarchar](500) NOT NULL,
	[ActionsTaken] [nvarchar](500) NOT NULL,
	[Problems4MS] [varchar](50) NOT NULL,
	[Ukey] [int] NOT NULL,
	[StyleID] [varchar](200) NOT NULL,
	[OrderID] [varchar](max) NOT NULL,
	[Problems4MSDesc] [varchar](max) NOT NULL,
	[BIFactoryID] [varchar](8) NOT NULL,
	[BIInsertDate] [datetime] NULL,
 CONSTRAINT [PK_P_StationHourlyOutput] PRIMARY KEY CLUSTERED 
(
	[FactoryID] ASC,
	[Ukey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[P_StationHourlyOutput] ADD  CONSTRAINT [DF_P_StationHourlyOutput_FactoryID]  DEFAULT ('') FOR [FactoryID]
GO

ALTER TABLE [dbo].[P_StationHourlyOutput] ADD  CONSTRAINT [DF_P_StationHourlyOutput_Shift]  DEFAULT ('') FOR [Shift]
GO

ALTER TABLE [dbo].[P_StationHourlyOutput] ADD  CONSTRAINT [DF_P_StationHourlyOutput_Team]  DEFAULT ('') FOR [Team]
GO

ALTER TABLE [dbo].[P_StationHourlyOutput] ADD  CONSTRAINT [DF_P_StationHourlyOutput_Line]  DEFAULT ('') FOR [Line]
GO

ALTER TABLE [dbo].[P_StationHourlyOutput] ADD  CONSTRAINT [DF_P_StationHourlyOutput_Station]  DEFAULT ('') FOR [Station]
GO

ALTER TABLE [dbo].[P_StationHourlyOutput] ADD  CONSTRAINT [DF_P_StationHourlyOutput_Capacity]  DEFAULT ((0)) FOR [Capacity]
GO

ALTER TABLE [dbo].[P_StationHourlyOutput] ADD  CONSTRAINT [DF_P_StationHourlyOutput_Target]  DEFAULT ((0)) FOR [Target]
GO

ALTER TABLE [dbo].[P_StationHourlyOutput] ADD  CONSTRAINT [DF_P_StationHourlyOutput_TotalOutput]  DEFAULT ((0)) FOR [TotalOutput]
GO

ALTER TABLE [dbo].[P_StationHourlyOutput] ADD  CONSTRAINT [DF_P_StationHourlyOutput_ProblemsEncounter]  DEFAULT ('') FOR [ProblemsEncounter]
GO

ALTER TABLE [dbo].[P_StationHourlyOutput] ADD  CONSTRAINT [DF_P_StationHourlyOutput_ActionsTaken]  DEFAULT ('') FOR [ActionsTaken]
GO

ALTER TABLE [dbo].[P_StationHourlyOutput] ADD  CONSTRAINT [DF_P_StationHourlyOutput_Problems4MS]  DEFAULT ('') FOR [Problems4MS]
GO

ALTER TABLE [dbo].[P_StationHourlyOutput] ADD  CONSTRAINT [DF_P_StationHourlyOutput_Ukey]  DEFAULT ('') FOR [Ukey]
GO

ALTER TABLE [dbo].[P_StationHourlyOutput] ADD  CONSTRAINT [DF_P_StationHourlyOutput_StyleID]  DEFAULT ('') FOR [StyleID]
GO

ALTER TABLE [dbo].[P_StationHourlyOutput] ADD  CONSTRAINT [DF_P_StationHourlyOutput_OrderID]  DEFAULT ('') FOR [OrderID]
GO

ALTER TABLE [dbo].[P_StationHourlyOutput] ADD  CONSTRAINT [DF_P_StationHourlyOutput_Problems4MSDesc]  DEFAULT ('') FOR [Problems4MSDesc]
GO

ALTER TABLE [dbo].[P_StationHourlyOutput] ADD  CONSTRAINT [DF_P_StationHourlyOutput_BIFactoryID]  DEFAULT ('') FOR [BIFactoryID]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StationHourlyOutput', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StationHourlyOutput', @level2type=N'COLUMN',@level2name=N'Date'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'班別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StationHourlyOutput', @level2type=N'COLUMN',@level2name=N'Shift'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Team' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StationHourlyOutput', @level2type=N'COLUMN',@level2name=N'Team'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Line' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StationHourlyOutput', @level2type=N'COLUMN',@level2name=N'Line'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Station' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StationHourlyOutput', @level2type=N'COLUMN',@level2name=N'Station'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'每小時最大能產出數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StationHourlyOutput', @level2type=N'COLUMN',@level2name=N'Capacity'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'每小時目標數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StationHourlyOutput', @level2type=N'COLUMN',@level2name=N'Target'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'總產量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StationHourlyOutput', @level2type=N'COLUMN',@level2name=N'TotalOutput'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'問題說明紀錄' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StationHourlyOutput', @level2type=N'COLUMN',@level2name=N'ProblemsEncounter'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'採取何種改善紀錄' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StationHourlyOutput', @level2type=N'COLUMN',@level2name=N'ActionsTaken'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Problems 4MS' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StationHourlyOutput', @level2type=N'COLUMN',@level2name=N'Problems4MS'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ukey' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StationHourlyOutput', @level2type=N'COLUMN',@level2name=N'Ukey'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'StyleID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StationHourlyOutput', @level2type=N'COLUMN',@level2name=N'StyleID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'OrderID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StationHourlyOutput', @level2type=N'COLUMN',@level2name=N'OrderID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N' 記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StationHourlyOutput', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_StationHourlyOutput', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO