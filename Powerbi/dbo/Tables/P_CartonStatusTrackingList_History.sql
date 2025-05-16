	CREATE TABLE [dbo].[P_CartonStatusTrackingList_History](
		[Ukey] [bigint] IDENTITY(1,1) NOT NULL,
		[CtnNo] [varchar](6) NOT NULL,
		[FactoryID] [varchar](8)NOT NULL,
		[PackingListID] [varchar](13)NOT NULL,
		[SeqNo] [varchar](2)NOT NULL,
		[SP] [varchar](13)NOT NULL,
		[BIFactoryID] [varchar](8) Not NULL,
		[BIInsertDate] [datetime] NOT NULL,	
	 CONSTRAINT [PK_P_CartonStatusTrackingList_History] PRIMARY KEY CLUSTERED 
CREATE TABLE [dbo].[P_CartonStatusTrackingList_History]
(
		[Ukey] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	[Ukey] BIGINT NOT NULL PRIMARY KEY, 
    [FactoryID] VARCHAR(8) NOT NULL DEFAULT (''), 
    [SP] VARCHAR(13) NOT NULL DEFAULT (''), 
    [SeqNo] VARCHAR(2) NOT NULL DEFAULT (''), 
    [PackingListID] VARCHAR(13) NOT NULL DEFAULT (''), 
    [CtnNo] VARCHAR(6) NOT NULL DEFAULT (''), 
    [BIFactoryID] VARCHAR(8) NOT NULL DEFAULT (''), 
    [BIInsertDate] DATETIME NULL
)

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'訂單號碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CartonStatusTrackingList_History', @level2type=N'COLUMN',@level2name=N'SP'
GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'CTN號碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CartonStatusTrackingList_History', @level2type=N'COLUMN',@level2name=N'CtnNo'
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Order No',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_CartonStatusTrackingList_History',
    @level2type = N'COLUMN',
    @level2name = N'SP'
GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠編號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CartonStatusTrackingList_History', @level2type=N'COLUMN',@level2name=N'FactoryID'
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Shipment Seq',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_CartonStatusTrackingList_History',
    @level2type = N'COLUMN',
    @level2name = N'SeqNo'
GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'撿料單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CartonStatusTrackingList_History', @level2type=N'COLUMN',@level2name=N'PackingListID'
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Packing List #',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_CartonStatusTrackingList_History',
    @level2type = N'COLUMN',
    @level2name = N'PackingListID'
GO

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'記錄哪間工廠的資料，ex PH1, PH2',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_CartonStatusTrackingList_History',
    @level2type = N'COLUMN',
    @level2name = N'BIFactoryID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'時間戳記，紀錄寫入table時間',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_CartonStatusTrackingList_History',
    @level2type = N'COLUMN',
    @level2name = N'BIInsertDate'
GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CartonStatusTrackingList_History', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Carton#',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_CartonStatusTrackingList_History',
    @level2type = N'COLUMN',
    @level2name = N'BIFactory'
GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_CartonStatusTrackingList_History', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Order Factory',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_CartonStatusTrackingList_History',
    @level2type = N'COLUMN',
    @level2name = N'FactoryID'
GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Carton#',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_CartonStatusTrackingList_History',
    @level2type = N'COLUMN',
    @level2name = N'CtnNo'