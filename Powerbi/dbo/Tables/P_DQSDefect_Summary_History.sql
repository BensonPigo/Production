CREATE TABLE [dbo].[P_DQSDefect_Summary_History](
	[HistoryUkey] [bigint] IDENTITY(1,1) NOT NULL,
	[FirstInspectDate] [date] NOT NULL,
	[FactoryID] [varchar](8) NOT NULL,
	[SPNO] [varchar](13) NOT NULL,
	[Article] [varchar](8) NOT NULL,
	[SizeCode] [varchar](8) NOT NULL,
	[QCName] [varchar](10) NOT NULL,
	[Shift] [varchar](5) NOT NULL,
	[Line] [varchar](5) NOT NULL,
	[InspectionDate] [date] NOT NULL,
	[BIFactoryID] [varchar](8) NOT NULL,
	[BIInsertDate] [datetime] NOT NULL,
 CONSTRAINT [PK_P_DQSDefect_Summary_History] PRIMARY KEY CLUSTERED 
(
	[HistoryUkey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO