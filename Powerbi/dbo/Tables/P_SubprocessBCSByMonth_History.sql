CREATE TABLE [dbo].[P_SubprocessBCSByMonth_History](
	[HistoryUkey] [bigint] IDENTITY(1,1) NOT NULL,
	[Month] [varchar](6) NOT NULL,
	[Factory] [varchar](8) NOT NULL,
	[BIFactoryID] [varchar](8) NOT NULL,
	[BIInsertDate] [datetime] NOT NULL,
 CONSTRAINT [PK_P_SubprocessBCSByMonth_History] PRIMARY KEY CLUSTERED 
(
	[HistoryUkey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[P_SubprocessBCSByMonth_History] ADD  CONSTRAINT [DF_P_SubprocessBCSByMonth_History_Month]  DEFAULT ('') FOR [Month]
GO

ALTER TABLE [dbo].[P_SubprocessBCSByMonth_History] ADD  CONSTRAINT [DF_P_SubprocessBCSByMonth_History_Factory]  DEFAULT ('') FOR [Factory]
GO