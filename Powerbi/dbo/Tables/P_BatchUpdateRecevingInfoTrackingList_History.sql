﻿	CREATE TABLE [dbo].[P_BatchUpdateRecevingInfoTrackingList_History](
		[Ukey] [bigint] IDENTITY(1,1) NOT NULL,
		[Poid] [varchar] (13) NOT NULL CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_History_Poid]  DEFAULT '',
		[ReceivingID] [varchar] (13) NOT NULL CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_History_ReceivingID] DEFAULT '',
		[Roll] [varchar] (8) NOT NULL CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_History_Roll]  DEFAULT '',
		[Seq] [varchar] (6) NOT NULL CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_History_Seq]  DEFAULT '',
		[StockType] [varchar](1) NOT NULL CONSTRAINT [DF_P_BatchUpdateRecevingInfoTrackingList_History_StockType]  DEFAULT '',
		[BIFactoryID] [varchar](8) Not NULL,
		[BIInsertDate] [datetime] NOT NULL,	
	 CONSTRAINT [PK_P_BatchUpdateRecevingInfoTrackingList_History] PRIMARY KEY CLUSTERED 
	(
		[Ukey] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'採購單編號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_BatchUpdateRecevingInfoTrackingList_History', @level2type=N'COLUMN',@level2name=N'Poid'
	Go
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'收料單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_BatchUpdateRecevingInfoTrackingList_History', @level2type=N'COLUMN',@level2name=N'ReceivingID'
	Go
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'卷' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_BatchUpdateRecevingInfoTrackingList_History', @level2type=N'COLUMN',@level2name=N'Roll'
	Go
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'採購大小項' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_BatchUpdateRecevingInfoTrackingList_History', @level2type=N'COLUMN',@level2name=N'Seq'	
	Go
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'倉別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_BatchUpdateRecevingInfoTrackingList_History', @level2type=N'COLUMN',@level2name=N'StockType'
	Go
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_BatchUpdateRecevingInfoTrackingList_History', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
	Go
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_BatchUpdateRecevingInfoTrackingList_History', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
	Go
