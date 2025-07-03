CREATE TABLE [dbo].[P_DailyRTLStatusByLineByStyle_History](
	[HistoryUkey] [bigint] IDENTITY(1,1) NOT NULL,
	[TransferDate] [date] NOT NULL,
	[FactoryID] [varchar](4) NOT NULL,
	[APSNo] [int] NOT NULL,
	[BIFactoryID] [varchar](8) NOT NULL,
	[BIInsertDate] [datetime] NOT NULL,
 CONSTRAINT [PK_P_DailyRTLStatusByLineByStyle_History] PRIMARY KEY CLUSTERED 
(
	[HistoryUkey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[P_DailyRTLStatusByLineByStyle_History] ADD  CONSTRAINT [DF_P_DailyRTLStatusByLineByStyle_History_FactoryID]  DEFAULT ('') FOR [FactoryID]
GO

ALTER TABLE [dbo].[P_DailyRTLStatusByLineByStyle_History] ADD  CONSTRAINT [DF_P_DailyRTLStatusByLineByStyle_History_APSNo]  DEFAULT ((0)) FOR [APSNo]
GO