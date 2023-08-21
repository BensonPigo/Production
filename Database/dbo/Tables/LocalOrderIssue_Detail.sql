CREATE TABLE [dbo].[LocalOrderIssue_Detail]
(
	[ID]            VARCHAR(13)         CONSTRAINT [DF_LocalOrderIssue_Detail_ID]               DEFAULT ((''))  NOT NULL , 
    [POID]          VARCHAR(13)         CONSTRAINT [DF_LocalOrderIssue_Detail_POID]             DEFAULT ((''))  NOT NULL , 
    [Seq1]          VARCHAR(3)          CONSTRAINT [DF_LocalOrderIssue_Detail_Seq1]             DEFAULT ((''))  NOT NULL , 
    [Seq2]          VARCHAR(2)          CONSTRAINT [DF_LocalOrderIssue_Detail_Seq2]             DEFAULT ((''))  NOT NULL , 
    [Roll]          VARCHAR(8)          CONSTRAINT [DF_LocalOrderIssue_Detail_Roll]             DEFAULT ((''))  NOT NULL , 
    [Dyelot]        VARCHAR(8)          CONSTRAINT [DF_LocalOrderIssue_Detail_Dyelot]           DEFAULT ((''))  NOT NULL , 
    [StockType]     CHAR(1)             CONSTRAINT [DF_LocalOrderIssue_Detail_StockType]        DEFAULT ((''))  NOT NULL , 
    [Qty]           NUMERIC(11, 2)      CONSTRAINT [DF_LocalOrderIssue_Detail_Qty]              DEFAULT ((''))  NOT NULL , 
    [SentToWMS]     BIT                 CONSTRAINT [DF_LocalOrderIssue_Detail_SentToWMS]        DEFAULT ((0))   NOT NULL , 
    [CompleteTime]  DATETIME                                                                                    NULL, 
    [Ukey]          BIGINT                                                                      IDENTITY(1,1)   NOT NULL , 
    CONSTRAINT [PK_LocalOrderIssue_Detail] PRIMARY KEY ([Ukey])
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'發料單號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalOrderIssue_Detail',
    @level2type = N'COLUMN',
    @level2name = N'ID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'訂單',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalOrderIssue_Detail',
    @level2type = N'COLUMN',
    @level2name = N'POID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'大項',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalOrderIssue_Detail',
    @level2type = N'COLUMN',
    @level2name = N'Seq1'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'小項',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalOrderIssue_Detail',
    @level2type = N'COLUMN',
    @level2name = N'Seq2'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'捲號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalOrderIssue_Detail',
    @level2type = N'COLUMN',
    @level2name = N'Roll'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'缸號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalOrderIssue_Detail',
    @level2type = N'COLUMN',
    @level2name = N'Dyelot'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'倉別',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalOrderIssue_Detail',
    @level2type = N'COLUMN',
    @level2name = N'StockType'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'發料數量',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalOrderIssue_Detail',
    @level2type = N'COLUMN',
    @level2name = N'Qty'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'是否有傳給 WMS',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalOrderIssue_Detail',
    @level2type = N'COLUMN',
    @level2name = N'SentToWMS'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'WMS 完成指令的時間',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalOrderIssue_Detail',
    @level2type = N'COLUMN',
    @level2name = N'CompleteTime'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'流水號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalOrderIssue_Detail',
    @level2type = N'COLUMN',
    @level2name = N'Ukey'