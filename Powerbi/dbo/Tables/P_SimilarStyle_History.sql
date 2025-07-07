CREATE TABLE [dbo].[P_SimilarStyle_History](
	[HistoryUkey] [bigint] IDENTITY(1,1) NOT NULL,
	[OutputDate] [date] NOT NULL,
	[FactoryID] [varchar](8) NOT NULL,
	[StyleID] [varchar](15) NOT NULL,
	[BrandID] [varchar](8) NOT NULL,
	[BIFactoryID] [varchar](8) NOT NULL,
	[BIInsertDate] [datetime] NOT NULL,
 CONSTRAINT [PK_P_SimilarStyle_History] PRIMARY KEY CLUSTERED 
(
	[HistoryUkey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO