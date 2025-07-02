	CREATE TABLE [dbo].[P_SDP_History](
		[Ukey] [bigint] IDENTITY(1,1) NOT NULL,
		[FactoryID] [varchar](8) NOT NULL,
		[Pullouttimes] [INT],
		[Seq] [varchar](2) Not NULL,
		[SPNo] [varchar](13) Not NULL,
		[Style] [varchar](15) Not NULL,
		[BIFactoryID] [varchar](8) Not NULL,
		[BIInsertDate] [datetime] NOT NULL,	
	 CONSTRAINT [PK_P_SDP_History] PRIMARY KEY CLUSTERED 
	(
		[Ukey] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[P_SDP_History] ADD  CONSTRAINT [DF_P_SDP_History_FactoryID]  DEFAULT ('') FOR [FactoryID]
	ALTER TABLE [dbo].[P_SDP_History] ADD  CONSTRAINT [DF_P_SDP_History_Seq]  DEFAULT ('') FOR [Seq]
	ALTER TABLE [dbo].[P_SDP_History] ADD  CONSTRAINT [DF_P_SDP_History_SPNo]  DEFAULT ('') FOR [SPNo]
	ALTER TABLE [dbo].[P_SDP_History] ADD  CONSTRAINT [DF_P_SDP_History_Style]  DEFAULT ('') FOR [Style]
