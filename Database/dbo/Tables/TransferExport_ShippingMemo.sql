CREATE TABLE [dbo].[TransferExport_ShippingMemo]
(
		[Ukey] [bigint]						IDENTITY(1,1) NOT NULL,
		[ID] [varchar](13)				CONSTRAINT [DF_TransferExport_ShippingMemo_ID]  DEFAULT ('')		NOT NULL,
		[ShippingExpense] [bit]			CONSTRAINT [DF_TransferExport_ShippingMemo_ShippingExpense]  DEFAULT ((0))	NOT NULL,
		[Subject] [nvarchar](100)		CONSTRAINT [DF_TransferExport_ShippingMemo_Subject]  DEFAULT ('')	NOT NULL,
		[Description] [nvarchar](500)	CONSTRAINT [DF_TransferExport_ShippingMemo_Description]  DEFAULT ('') NOT NULL,
		[AddName] [varchar](10)			CONSTRAINT [DF_TransferExport_ShippingMemo_AddName]  DEFAULT ('') NOT NULL,
		[AddDate] [datetime] NULL,
		[EditName] [varchar](10)		CONSTRAINT [DF_TransferExport_ShippingMemo_EditName]  DEFAULT ('') NOT NULL,
		[EditDate] [datetime] NULL,
	CONSTRAINT [PK_TransferExport_ShippingMemo] PRIMARY KEY CLUSTERED ([Ukey] ASC)
	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
