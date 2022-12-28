	CREATE TABLE [dbo].[TransferToSubcon_Detail]
	(
		[ID] [varchar](13)			CONSTRAINT [DF_TransferToSubcon_Detail_ID]			DEFAULT ('') NOT NULL,
		[POID] [varchar](13)		CONSTRAINT [DF_TransferToSubcon_Detail_POID]		DEFAULT ('') NOT NULL,
		[Seq1] [varchar](3)			CONSTRAINT [DF_TransferToSubcon_Detail_Seq1]		DEFAULT ('') NOT NULL,
		[Seq2] [varchar](2)			CONSTRAINT [DF_TransferToSubcon_Detail_Seq2]		DEFAULT ('') NOT NULL,
		[Roll] [varchar](8)			CONSTRAINT [DF_TransferToSubcon_Detail_Roll]		DEFAULT ('') NOT NULL,
		[Dyelot] [varchar](8)		CONSTRAINT [DF_TransferToSubcon_Detail_Dyelot]		DEFAULT ('') NOT NULL,
		[StockType] [varchar](1)	CONSTRAINT [DF_TransferToSubcon_Detail_StockType]	DEFAULT ('') NOT NULL,
		[Qty] [numeric](11, 2)		CONSTRAINT [DF_TransferToSubcon_Detail_Qty]			DEFAULT ((0)) NOT NULL,
		[Ukey] [bigint] NOT NULL,
		CONSTRAINT [PK_TransferToSubcon_Detail] PRIMARY KEY CLUSTERED ([Ukey] ASC)
		WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	GO
	EXECUTE sys.sp_addextendedproperty @name=N'MS_Description', @value=N'外發單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferToSubcon_Detail', @level2type=N'COLUMN',@level2name=N'ID'
	GO

	EXECUTE sys.sp_addextendedproperty @name=N'MS_Description', @value=N'採購單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferToSubcon_Detail', @level2type=N'COLUMN',@level2name=N'POID'
	GO

	EXECUTE sys.sp_addextendedproperty @name=N'MS_Description', @value=N'物料大項' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferToSubcon_Detail', @level2type=N'COLUMN',@level2name=N'Seq1'
	GO

	EXECUTE sys.sp_addextendedproperty @name=N'MS_Description', @value=N'物料小項' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferToSubcon_Detail', @level2type=N'COLUMN',@level2name=N'Seq2'
	GO

	EXECUTE sys.sp_addextendedproperty @name=N'MS_Description', @value=N'捲號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferToSubcon_Detail', @level2type=N'COLUMN',@level2name=N'Roll'
	GO

	EXECUTE sys.sp_addextendedproperty @name=N'MS_Description', @value=N'缸號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferToSubcon_Detail', @level2type=N'COLUMN',@level2name=N'Dyelot'
	GO

	EXECUTE sys.sp_addextendedproperty @name=N'MS_Description', @value=N'倉庫代號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferToSubcon_Detail', @level2type=N'COLUMN',@level2name=N'StockType'
	GO

	EXECUTE sys.sp_addextendedproperty @name=N'MS_Description', @value=N'轉出當下的庫存數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferToSubcon_Detail', @level2type=N'COLUMN',@level2name=N'Qty'
	GO
