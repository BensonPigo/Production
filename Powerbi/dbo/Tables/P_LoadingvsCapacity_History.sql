CREATE TABLE [dbo].[P_LoadingvsCapacity_History](
	[HistoryUkey] [bigint] IDENTITY(1,1) NOT NULL,
	[MDivisionID] [varchar](8) NOT NULL,
	[FactoryID] [varchar](8) NOT NULL,
	[Key] [varchar](6) NOT NULL,
	[Halfkey] [varchar](8) NOT NULL,
	[ArtworkTypeID] [varchar](20) NOT NULL,
	[BIFactoryID] [varchar](8) NOT NULL,
	[BIInsertDate] [datetime] NOT NULL,
 CONSTRAINT [PK_P_LoadingvsCapacity_History] PRIMARY KEY CLUSTERED 
(
	[HistoryUkey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO