	CREATE TABLE [dbo].[P_InventoryStockListReport_History](
		[Ukey] [bigint] IDENTITY(1,1) NOT NULL,
		[Dyelot] [varchar](8) NOT NULL,
		[POID] [varchar](13) NOT NULL,
		[Roll] [varchar](8) NOT NULL,
		[SEQ1] [varchar](3) NOT NULL,
		[SEQ2] [varchar](2) NOT NULL,
		[StockType] [varchar](15) NOT NULL,
		[BIFactoryID] [varchar](8) Not NULL,
		[BIInsertDate] [datetime] NOT NULL,	
	 CONSTRAINT [PK_P_InventoryStockListReport_History] PRIMARY KEY CLUSTERED 
	(
		[Ukey] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[P_InventoryStockListReport_History] ADD  CONSTRAINT [DF_P_InventoryStockListReport_History_FactoryID]  DEFAULT ('') FOR [Dyelot]

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'缸' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport_History', @level2type=N'COLUMN',@level2name=N'Dyelot'
	go
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'卷' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport_History', @level2type=N'COLUMN',@level2name=N'Roll'
	go
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'倉別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport_History', @level2type=N'COLUMN',@level2name=N'StockType'
	go
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport_History', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
	go
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_InventoryStockListReport_History', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
	go
