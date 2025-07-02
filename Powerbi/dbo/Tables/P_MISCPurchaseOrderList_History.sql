	CREATE TABLE [dbo].[P_MISCPurchaseOrderList_History](
		[Ukey] [bigint] IDENTITY(1,1) NOT NULL,
		[Code] [varchar](23) NOT NULL,
		[PONo] [varchar](13) NOT NULL,
		[ReqNo] [varchar](13) NOT NULL,
		[BIFactoryID] [varchar](8) Not NULL,
		[BIInsertDate] [datetime] NOT NULL,	
	 CONSTRAINT [PK_P_MISCPurchaseOrderList_History] PRIMARY KEY CLUSTERED 
	(
		[Ukey] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[P_MISCPurchaseOrderList_History] ADD  CONSTRAINT [DF_P_MISCPurchaseOrderList_History_Code]  DEFAULT ('') FOR [Code]
	ALTER TABLE [dbo].[P_MISCPurchaseOrderList_History] ADD  CONSTRAINT [DF_P_MISCPurchaseOrderList_History_PONO]  DEFAULT ('') FOR [PONo]
	ALTER TABLE [dbo].[P_MISCPurchaseOrderList_History] ADD  CONSTRAINT [DF_P_MISCPurchaseOrderList_History_ReqNo]  DEFAULT ('') FOR [ReqNo]
