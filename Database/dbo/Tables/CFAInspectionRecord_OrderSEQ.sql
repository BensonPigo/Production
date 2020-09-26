	CREATE TABLE dbo.CFAInspectionRecord_OrderSEQ (
		Ukey Bigint NOT NULL IDENTITY(1,1),
		ID varchar(13) NOT NULL CONSTRAINT [DF_CFAInspectionRecord_OrderSEQ_ID] DEFAULT (''),
		OrderID varchar(13) NOT NULL CONSTRAINT [DF_CFAInspectionRecord_OrderSEQ_OrderID] DEFAULT (''),
		SEQ varchar(2) NOT NULL CONSTRAINT [DF_CFAInspectionRecord_OrderSEQ_Seq] DEFAULT (''),
		CONSTRAINT [PK_CFAInspectionRecord_OrderSEQ] PRIMARY KEY CLUSTERED 
		(
			Ukey ASC
		)
	)
	GO
	;