CREATE TABLE [dbo].[LocalOrderLocationTrans_Detail]
(
	[ID]                VARCHAR(13)         CONSTRAINT [DF_LocalOrderLocationTrans_Detail_ID]                   DEFAULT ((''))  NOT NULL ,  
    [POID]              VARCHAR(13)         CONSTRAINT [DF_LocalOrderLocationTrans_Detail_POID]                 DEFAULT ((''))  NOT NULL , 
    [Seq1]              VARCHAR(3)          CONSTRAINT [DF_LocalOrderLocationTrans_Detail_Seq1]                 DEFAULT ((''))  NOT NULL ,
    [Seq2]              VARCHAR(2)          CONSTRAINT [DF_LocalOrderLocationTrans_Detail_Seq2]                 DEFAULT ((''))  NOT NULL ,
    [Roll]              VARCHAR(8)          CONSTRAINT [DF_LocalOrderLocationTrans_Detail_Roll]                 DEFAULT ((''))  NOT NULL ,
    [Dyelot]            VARCHAR(8)          CONSTRAINT [DF_LocalOrderLocationTrans_Detail_Dyelot]               DEFAULT ((''))  NOT NULL ,
    [StockType]         CHAR(1)             CONSTRAINT [DF_LocalOrderLocationTrans_Detail_StockType]            DEFAULT ((''))  NOT NULL , 
    [Qty]               NUMERIC(11, 2)      CONSTRAINT [DF_LocalOrderLocationTrans_Detail_Qty]                  DEFAULT ((0))   NOT NULL , 
    [FromLocation]      VARCHAR(60)         CONSTRAINT [DF_LocalOrderLocationTrans_Detail_FromLocation]         DEFAULT ((''))  NOT NULL , 
    [ToLocation]        VARCHAR(60)         CONSTRAINT [DF_LocalOrderLocationTrans_Detail_ToLocation]           DEFAULT ((''))  NOT NULL , 
    [SentToWMS]         BIT                 CONSTRAINT [DF_LocalOrderLocationTrans_Detail_SentToWMS]            DEFAULT ((0))   NOT NULL ,
    [CompleteTime]      DATETIME                                                                                                NULL, 
    [ToContainerCode]   NVARCHAR(100)       CONSTRAINT [DF_LocalOrderLocationTrans_Detail_ToContainerCode]      DEFAULT ((''))  NOT NULL , 
    [Ukey]              BIGINT                                                                                  IDENTITY(1,1)   NOT NULL , 
    
    CONSTRAINT [PK_LocalOrderLocationTrans_Detail] PRIMARY KEY ([Ukey]) 
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'調整單號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalOrderLocationTrans_Detail',
    @level2type = N'COLUMN',
    @level2name = N'ID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'訂單',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalOrderLocationTrans_Detail',
    @level2type = N'COLUMN',
    @level2name = N'POID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'大項',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalOrderLocationTrans_Detail',
    @level2type = N'COLUMN',
    @level2name = N'Seq1'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'小項',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalOrderLocationTrans_Detail',
    @level2type = N'COLUMN',
    @level2name = N'Seq2'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'捲號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalOrderLocationTrans_Detail',
    @level2type = N'COLUMN',
    @level2name = N'Roll'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'缸號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalOrderLocationTrans_Detail',
    @level2type = N'COLUMN',
    @level2name = N'Dyelot'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'倉別',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalOrderLocationTrans_Detail',
    @level2type = N'COLUMN',
    @level2name = N'StockType'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'調整庫存位置時的庫存餘數',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalOrderLocationTrans_Detail',
    @level2type = N'COLUMN',
    @level2name = N'Qty'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'調整前庫存位置',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalOrderLocationTrans_Detail',
    @level2type = N'COLUMN',
    @level2name = N'FromLocation'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'調整後庫存位置',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalOrderLocationTrans_Detail',
    @level2type = N'COLUMN',
    @level2name = N'ToLocation'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'是否有傳給 WMS',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalOrderLocationTrans_Detail',
    @level2type = N'COLUMN',
    @level2name = N'SentToWMS'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'WMS 完成指令的時間',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalOrderLocationTrans_Detail',
    @level2type = N'COLUMN',
    @level2name = N'CompleteTime'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'鐵框號 ( 主要針對主料 )',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalOrderLocationTrans_Detail',
    @level2type = N'COLUMN',
    @level2name = N'ToContainerCode'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'流水號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalOrderLocationTrans_Detail',
    @level2type = N'COLUMN',
    @level2name = N'Ukey'