	CREATE TABLE [dbo].[P_SewingDailyOutput_History](
		[Ukey] [bigint] IDENTITY(1,1) NOT NULL,
		[MDivisionID] [varchar](20) NOT NULL,
		[BIFactoryID] [varchar](8) Not NULL,
		[BIInsertDate] [datetime] NOT NULL,	
	 CONSTRAINT [PK_P_SewingDailyOutput_History] PRIMARY KEY CLUSTERED 
	(
		[Ukey] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[P_SewingDailyOutput_History] ADD  CONSTRAINT [DF_P_SewingDailyOutput_History_MDivisionID]  DEFAULT ('') FOR [MDivisionID]
