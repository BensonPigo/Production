CREATE TABLE [dbo].[P_SubprocessWIP_History](
	[HistoryUkey] [bigint] IDENTITY(1,1) NOT NULL,
	[Bundleno] [varchar](12) NOT NULL,
	[RFIDProcessLocationID] [varchar](15) NOT NULL,
	[Sp] [varchar](250) NOT NULL,
	[Pattern] [varchar](20) NOT NULL,
	[SubprocessID] [varchar](50) NOT NULL,
	[BIFactoryID] [varchar](8) NOT NULL,
	[BIInsertDate] [datetime] NOT NULL,
 CONSTRAINT [PK_P_SubprocessWIP_History] PRIMARY KEY CLUSTERED 
(
	[HistoryUkey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO