	CREATE TABLE [dbo].[P_SimilarStyle_History](
		[Ukey] [bigint] IDENTITY(1,1) NOT NULL,
		[BrandID] [varchar](8) NOT NULL,
		[FactoryID] [varchar](8) NOT NULL,
		[OutputDate] [date] NOT NULL,
		[StyleID] [varchar](15) NOT NULL,
		[WeekNo] [int],
		[Year] [int] ,
		[BIFactoryID] [varchar](8) Not NULL,
		[BIInsertDate] [datetime] NOT NULL,	
	 CONSTRAINT [PK_P_SimilarStyle_History] PRIMARY KEY CLUSTERED 
	(
		[Ukey] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[P_SimilarStyle_History] ADD  CONSTRAINT [DF_P_SimilarStyle_History_FactoryID]  DEFAULT ('') FOR [BrandID]
	ALTER TABLE [dbo].[P_SimilarStyle_History] ADD  CONSTRAINT [DF_P_SimilarStyle_History_FactoryID]  DEFAULT ('') FOR [FactoryID]
	ALTER TABLE [dbo].[P_SimilarStyle_History] ADD  CONSTRAINT [DF_P_SimilarStyle_History_FactoryID]  DEFAULT ('') FOR [StyleID]