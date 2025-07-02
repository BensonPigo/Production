	CREATE TABLE [dbo].[P_RightFirstTimeDailyReport_History](
		[Ukey] [bigint] IDENTITY(1,1) NOT NULL,
		[CDate] [date],
		[FactoryID] [varchar](8) NOT NULL,
		[Line] [varchar](5) NOT NULL,
		[OrderID] [varchar](13) NOT NULL,
		[Shift] [varchar](1) NOT NULL,
		[Team] [varchar](5) NOT NULL,
		[ResponsibilityDept] [varchar](8) NOT NULL,
		[ResponsibilityFty] [varchar](8) NOT NULL,
		[BIFactoryID] [varchar](8) Not NULL,
		[BIInsertDate] [datetime] NOT NULL,	
	 CONSTRAINT [PK_P_RightFirstTimeDailyReport_History] PRIMARY KEY CLUSTERED 
	(
		[Ukey] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[P_RightFirstTimeDailyReport_History] ADD  CONSTRAINT [DF_P_RightFirstTimeDailyReport_History_FactoryID]  DEFAULT ('') FOR [FactoryID]
	ALTER TABLE [dbo].[P_RightFirstTimeDailyReport_History] ADD  CONSTRAINT [DF_P_RightFirstTimeDailyReport_History_Line]  DEFAULT ('') FOR [Line]
	ALTER TABLE [dbo].[P_RightFirstTimeDailyReport_History] ADD  CONSTRAINT [DF_P_RightFirstTimeDailyReport_History_OrderID]  DEFAULT ('') FOR [OrderID]
	ALTER TABLE [dbo].[P_RightFirstTimeDailyReport_History] ADD  CONSTRAINT [DF_P_RightFirstTimeDailyReport_History_Shift]  DEFAULT ('') FOR [Shift]
	ALTER TABLE [dbo].[P_RightFirstTimeDailyReport_History] ADD  CONSTRAINT [DF_P_RightFirstTimeDailyReport_History_Team]  DEFAULT ('') FOR [Team]