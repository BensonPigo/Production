	CREATE TABLE [dbo].[P_AccessoryInspLabStatus_History](
		[Ukey] [bigint] IDENTITY(1,1) NOT NULL,
		[POID] [varchar](13) Not NULL,
		[ReceivingID] [varchar](13) Not NULL,
		[SEQ] [varchar](6) Not NULL,
		[BIFactoryID] [varchar](8) Not NULL,
		[BIInsertDate] [datetime] NOT NULL,	
	 CONSTRAINT [PK_P_AccessoryInspLabStatus_History] PRIMARY KEY CLUSTERED 
	(
		[Ukey] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[P_AccessoryInspLabStatus_History] ADD  CONSTRAINT [DF_P_AccessoryInspLabStatus_History_POID]  DEFAULT ('') FOR [POID]
	ALTER TABLE [dbo].[P_AccessoryInspLabStatus_History] ADD  CONSTRAINT [DF_P_AccessoryInspLabStatus_History_ReceivingID]  DEFAULT ('') FOR [ReceivingID]
	ALTER TABLE [dbo].[P_AccessoryInspLabStatus_History] ADD  CONSTRAINT [DF_P_AccessoryInspLabStatus_History_SEQ]  DEFAULT ('') FOR [SEQ]