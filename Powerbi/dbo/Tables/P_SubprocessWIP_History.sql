	CREATE TABLE [dbo].[P_SubprocessWIP_History](
		[Ukey] [bigint] IDENTITY(1,1) NOT NULL,
		[Bundleno] [varchar](12) NOT NULL,
		[Pattern] [varchar](20) NOT NULL,
		[RFIDProcessLocationID] [varchar](15) NOT NULL,
		[Sp] [varchar](250) NOT NULL,
		[SubprocessID] [varchar](50) NOT NULL,
		[BIFactoryID] [varchar](8) Not NULL,
		[BIInsertDate] [datetime] NOT NULL,	
	 CONSTRAINT [PK_P_SubprocessWIP_History] PRIMARY KEY CLUSTERED 
	(
		[Ukey] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[P_SubprocessWIP_History] ADD  CONSTRAINT [DF_P_SubprocessWIP_History_Bundleno]  DEFAULT ('') FOR [Bundleno]
	ALTER TABLE [dbo].[P_SubprocessWIP_History] ADD  CONSTRAINT [DF_P_SubprocessWIP_History_Pattern]  DEFAULT ('') FOR [Pattern]
	ALTER TABLE [dbo].[P_SubprocessWIP_History] ADD  CONSTRAINT [DF_P_SubprocessWIP_History_RFIDProcessLocationID]  DEFAULT ('') FOR [RFIDProcessLocationID]
	ALTER TABLE [dbo].[P_SubprocessWIP_History] ADD  CONSTRAINT [DF_P_SubprocessWIP_History_Sp]  DEFAULT ('') FOR [Sp]
	ALTER TABLE [dbo].[P_SubprocessWIP_History] ADD  CONSTRAINT [DF_P_SubprocessWIP_History_SubprocessID]  DEFAULT ('') FOR [SubprocessID]
