CREATE TABLE [dbo].[P_SewingDailyOutputStatusRecord_History](
	[HistoryUkey] [bigint] IDENTITY(1,1) NOT NULL,
	[SewingLineID] [varchar](5) NOT NULL,
	[SewingOutputDate] [date] NOT NULL,
	[FactoryID] [varchar](4) NOT NULL,
	[SPNo] [varchar](13) NOT NULL,
	[BIFactoryID] [varchar](8) NOT NULL,
	[BIInsertDate] [datetime] NOT NULL,
 CONSTRAINT [PK_P_SewingDailyOutputStatusRecord_History] PRIMARY KEY CLUSTERED 
(
	[HistoryUkey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
