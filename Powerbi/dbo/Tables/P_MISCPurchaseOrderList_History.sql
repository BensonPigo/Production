CREATE TABLE [dbo].[P_MISCPurchaseOrderList_History](
	[HistoryUkey] [bigint] IDENTITY(1,1) NOT NULL,
	[PONo] [varchar](13) NOT NULL,
	[ReqNo] [varchar](13) NOT NULL,
	[Code] [varchar](23) NOT NULL,
	[BIFactoryID] [varchar](8) NOT NULL,
	[BIInsertDate] [datetime] NOT NULL,
 CONSTRAINT [PK_P_MISCPurchaseOrderList_History] PRIMARY KEY CLUSTERED 
(
	[HistoryUkey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO