	CREATE TABLE dbo.CFAInspectionRecord_OrderSEQ (
		Ukey Bigint NOT NULL IDENTITY(1,1),
		ID varchar(13) NOT NULL CONSTRAINT [DF_CFAInspectionRecord_OrderSEQ_ID] DEFAULT (''),
		OrderID varchar(13) NOT NULL CONSTRAINT [DF_CFAInspectionRecord_OrderSEQ_OrderID] DEFAULT (''),
		SEQ varchar(2) NOT NULL CONSTRAINT [DF_CFAInspectionRecord_OrderSEQ_Seq] DEFAULT (''),
		Carton Varchar(500) NOT NULL CONSTRAINT [DF_CFAInspectionRecord_OrderSEQ_Carton] DEFAULT '',
		CONSTRAINT [PK_CFAInspectionRecord_OrderSEQ] PRIMARY KEY CLUSTERED 
		(
			Ukey ASC
		)
	)
	GO
	;


CREATE NONCLUSTERED INDEX [IDX_CFAInspectionRecord_OrderSEQ_ID] ON [dbo].[CFAInspectionRecord_OrderSEQ]
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO




	EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'此次檢驗的紙箱箱號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CFAInspectionRecord_OrderSEQ', @level2type = N'COLUMN', @level2name = N'Carton';
	GO