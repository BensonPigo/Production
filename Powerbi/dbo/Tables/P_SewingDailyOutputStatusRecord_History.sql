CREATE TABLE [dbo].[P_SewingDailyOutputStatusRecord_History](
		[Ukey] [bigint] IDENTITY(1,1) NOT NULL,
		[FactoryID] [varchar](4) NOT NULL,
		[SewingLineID] [varchar](5)NOT NULL,
		[SewingOutputDate] [date],
		[SPNo] [varchar](13)NOT NULL,
		[BIFactoryID] [varchar](8) Not NULL,
		[BIInsertDate] [datetime] NOT NULL,	
	 CONSTRAINT [PK_P_SewingDailyOutputStatusRecord_History] PRIMARY KEY CLUSTERED 
	(
		[Ukey] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[P_SewingDailyOutputStatusRecord_History] ADD  CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_History_APSNo]  DEFAULT ('') FOR [SewingLineID]
	ALTER TABLE [dbo].[P_SewingDailyOutputStatusRecord_History] ADD  CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_History_FactoryID]  DEFAULT ('') FOR [FactoryID]
	ALTER TABLE [dbo].[P_SewingDailyOutputStatusRecord_History] ADD  CONSTRAINT [DF_P_SewingDailyOutputStatusRecord_History_SPNo]  DEFAULT ('') FOR [SPNo]