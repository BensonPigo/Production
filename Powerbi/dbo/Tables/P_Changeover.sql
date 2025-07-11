CREATE TABLE [dbo].[P_Changeover] (
    [TransferDate]          DATE            NOT NULL,
    [FactoryID]             VARCHAR (8000)  CONSTRAINT [DF_P_Changeover_FactoryID_New] DEFAULT ('') NOT NULL,
    [ChgOverInTransferDate] INT             CONSTRAINT [DF_P_Changeover_ChgOverInTransferDate_New] DEFAULT ((0)) NOT NULL,
    [ChgOverIn1Day]         INT             CONSTRAINT [DF_P_Changeover_ChgOverIn1Day_New] DEFAULT ((0)) NOT NULL,
    [ChgOverIn7Days]        INT             CONSTRAINT [DF_P_Changeover_ChgOverIn7Days_New] DEFAULT ((0)) NOT NULL,
    [COTInPast1Day]         NUMERIC (38, 2) CONSTRAINT [DF_P_Changeover_COTInPast1Day_New] DEFAULT ((0)) NOT NULL,
    [COTInPast7Days]        NUMERIC (38, 2) CONSTRAINT [DF_P_Changeover_COTInPast7Days_New] DEFAULT ((0)) NOT NULL,
    [COPTInPast1Day]        NUMERIC (38, 2) CONSTRAINT [DF_P_Changeover_COPTInPast1Day_New] DEFAULT ((0)) NOT NULL,
    [COPTInPast7Days]       NUMERIC (38, 2) CONSTRAINT [DF_P_Changeover_COPTInPast7Days_New] DEFAULT ((0)) NOT NULL,
    [BIFactoryID]           VARCHAR (8000)  CONSTRAINT [DF_P_Changeover_BIFactoryID_New] DEFAULT ('') NOT NULL,
    [BIInsertDate]          DATETIME        NULL,
    [BIStatus]              VARCHAR (8000)  CONSTRAINT [DF_P_Changeover_BIStatus_New] DEFAULT (N'New') NULL,
    CONSTRAINT [PK_P_Changeover] PRIMARY KEY CLUSTERED ([TransferDate] ASC, [FactoryID] ASC)
);



GO


GO


GO


GO


GO


GO


GO


GO


GO


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

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N' 記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_Changeover', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_Changeover', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_Changeover', @level2type = N'COLUMN', @level2name = N'BIStatus';

