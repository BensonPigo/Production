	CREATE TABLE [dbo].[P_LoadingvsCapacity_History](
		[Ukey] [bigint] IDENTITY(1,1) NOT NULL,
		[ArtworkTypeID] [varchar](20) Not NULL,
		[FactoryID] [varchar](8) Not NULL,
		[Halfkey] [varchar](8) Not NULL,
		[Key] [varchar](6) Not NULL,
		[MDivisionID][varchar](8) Not NULL,
		[BIFactoryID] [varchar](8) Not NULL,
		[BIInsertDate] [datetime] NOT NULL,	
	 CONSTRAINT [PK_P_LoadingvsCapacity_History] PRIMARY KEY CLUSTERED 
	(
		[Ukey] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[P_LoadingvsCapacity_History] ADD  CONSTRAINT [DF_P_LoadingvsCapacity_History_FactoryID]  DEFAULT ('') FOR [FactoryID]
	ALTER TABLE [dbo].[P_LoadingvsCapacity_History] ADD  CONSTRAINT [DF_P_LoadingvsCapacity_History_ArtworkTypeID]  DEFAULT ('') FOR [ArtworkTypeID]
	ALTER TABLE [dbo].[P_LoadingvsCapacity_History] ADD  CONSTRAINT [DF_P_LoadingvsCapacity_History_Halfkey]  DEFAULT ('') FOR [Halfkey]
	ALTER TABLE [dbo].[P_LoadingvsCapacity_History] ADD  CONSTRAINT [DF_P_LoadingvsCapacity_History_Key]  DEFAULT ('') FOR [Key]
	ALTER TABLE [dbo].[P_LoadingvsCapacity_History] ADD  CONSTRAINT [DF_P_LoadingvsCapacity_History_MDivisionID]  DEFAULT ('') FOR [MDivisionID]