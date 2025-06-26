
CREATE TABLE [dbo].[P_Changeover](
	[TransferDate] [date] NOT NULL,
	[FactoryID] [varchar](8) NOT NULL,
	[ChgOverInTransferDate] [int] NOT NULL,
	[ChgOverIn1Day] [int] NOT NULL,
	[ChgOverIn7Days] [int] NOT NULL,
	[COTInPast1Day] [numeric](8, 2) NOT NULL,
	[COTInPast7Days] [numeric](8, 2) NOT NULL,
	[COPTInPast1Day] [numeric](8, 2) NOT NULL,
	[COPTInPast7Days] [numeric](8, 2) NOT NULL,
 [BIFactoryID] VARCHAR(8) NOT NULL DEFAULT (''), 
    [BIInsertDate] DATETIME NULL, 
    CONSTRAINT [PK_P_Changeover] PRIMARY KEY CLUSTERED 
(
	[TransferDate] ASC,
	[FactoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[P_Changeover] ADD  CONSTRAINT [DF_P_Changeover_FactoryID]  DEFAULT ('') FOR [FactoryID]
GO

ALTER TABLE [dbo].[P_Changeover] ADD  CONSTRAINT [DF_P_Changeover_ChgOverInTransferDate]  DEFAULT ((0)) FOR [ChgOverInTransferDate]
GO

ALTER TABLE [dbo].[P_Changeover] ADD  CONSTRAINT [DF_P_Changeover_ChgOverIn1Day]  DEFAULT ((0)) FOR [ChgOverIn1Day]
GO

ALTER TABLE [dbo].[P_Changeover] ADD  CONSTRAINT [DF_P_Changeover_ChgOverIn7Days]  DEFAULT ((0)) FOR [ChgOverIn7Days]
GO

ALTER TABLE [dbo].[P_Changeover] ADD  CONSTRAINT [DF_P_Changeover_COTInPast1Day]  DEFAULT ((0)) FOR [COTInPast1Day]
GO

ALTER TABLE [dbo].[P_Changeover] ADD  CONSTRAINT [DF_P_Changeover_COTInPast7Days]  DEFAULT ((0)) FOR [COTInPast7Days]
GO

ALTER TABLE [dbo].[P_Changeover] ADD  CONSTRAINT [DF_P_Changeover_COPTInPast1Day]  DEFAULT ((0)) FOR [COPTInPast1Day]
GO

ALTER TABLE [dbo].[P_Changeover] ADD  CONSTRAINT [DF_P_Changeover_COPTInPast7Days]  DEFAULT ((0)) FOR [COPTInPast7Days]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'資料轉換當日的日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_Changeover', @level2type=N'COLUMN',@level2name=N'TransferDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠別' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_Changeover', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'計算當日的換款次數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_Changeover', @level2type=N'COLUMN',@level2name=N'ChgOverInTransferDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'計算當日+1的換款次數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_Changeover', @level2type=N'COLUMN',@level2name=N'ChgOverIn1Day'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'計算當日+1至當日+7的換款次數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_Changeover', @level2type=N'COLUMN',@level2name=N'ChgOverIn7Days'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'昨天的平均COT(min)
COT：從前一款式到新款式所花費的總時間
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_Changeover', @level2type=N'COLUMN',@level2name=N'COTInPast1Day'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'過去7天內(不含當日)的平均COT(min)
COT：從前一款式到新款式所花費的總時間
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_Changeover', @level2type=N'COLUMN',@level2name=N'COTInPast7Days'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'昨天的平均COPT(min)
COPT：生產一件良品所花費的總時間
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_Changeover', @level2type=N'COLUMN',@level2name=N'COPTInPast1Day'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'過去7天內(不含當日)的平均COPT(min)
COPT：生產一件良品所花費的總時間
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_Changeover', @level2type=N'COLUMN',@level2name=N'COPTInPast7Days'
GO



EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'記錄哪間工廠的資料，ex PH1, PH2',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_Changeover',
    @level2type = N'COLUMN',
    @level2name = N'BIFactoryID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'時間戳記，紀錄寫入table時間',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_Changeover',
    @level2type = N'COLUMN',
    @level2name = N'BIInsertDate'