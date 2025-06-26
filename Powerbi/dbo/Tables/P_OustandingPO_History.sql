	CREATE TABLE [dbo].[P_OustandingPO_History](
		[Ukey] [bigint] IDENTITY(1,1) NOT NULL,
		[FactoryID] [varchar](8)NOT NULL,
		[OrderID] [varchar](13)NOT NULL,
		[Seq] [varchar](2)NOT NULL,
		[BIFactoryID] [varchar](8) Not NULL,
		[BIInsertDate] [datetime] NOT NULL,	
	 CONSTRAINT [PK_P_OustandingPO_History] PRIMARY KEY CLUSTERED 
	(
		[Ukey] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠編號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_OustandingPO_History', @level2type=N'COLUMN',@level2name=N'FactoryID'
	Go
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單編號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_OustandingPO_History', @level2type=N'COLUMN',@level2name=N'OrderID'
	Go
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'順序編號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_OustandingPO_History', @level2type=N'COLUMN',@level2name=N'Seq'
	Go
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_OustandingPO_History', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
	Go
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_OustandingPO_History', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
	Go