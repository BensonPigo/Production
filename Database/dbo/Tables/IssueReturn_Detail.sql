CREATE TABLE [dbo].[IssueReturn_Detail] (
    [Id]               VARCHAR (13)    CONSTRAINT [DF_IssueReturn_Detail_Id] DEFAULT ('') NOT NULL,
    [FtyInventoryUkey] BIGINT          NULL,
    [MDivisionID]      VARCHAR (8)     CONSTRAINT [DF_IssueReturn_Detail_MDivisionID] DEFAULT ('') NULL,
    [POID]             VARCHAR (13)    CONSTRAINT [DF_IssueReturn_Detail_POID] DEFAULT ('') NULL,
    [Seq1]             VARCHAR (3)     CONSTRAINT [DF_IssueReturn_Detail_Seq1] DEFAULT ('') NULL,
    [Seq2]             VARCHAR (2)     CONSTRAINT [DF_IssueReturn_Detail_Seq2] DEFAULT ('') NULL,
    [Roll]             VARCHAR (8)     CONSTRAINT [DF_IssueReturn_Detail_Roll] DEFAULT ('') NULL,
    [Dyelot]           VARCHAR (8)     CONSTRAINT [DF_IssueReturn_Detail_Dyelot] DEFAULT ('') NULL,
    [StockType]        CHAR (1)        CONSTRAINT [DF_IssueReturn_Detail_StockType] DEFAULT ('') NULL,
    [Qty]              NUMERIC (10, 2) CONSTRAINT [DF_IssueReturn_Detail_Qty] DEFAULT ((0)) NULL,
    [Ukey]             BIGINT          IDENTITY (1, 1) NOT NULL,
    [CompleteTime] DATETIME NULL, 
    [Location ] VARCHAR(500) NULL DEFAULT (''), 
    [SentToWMS] BIT NOT NULL DEFAULT ((0)), 
    CONSTRAINT [PK_IssueReturn_Detail] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'發料退回明細', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IssueReturn_Detail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'發料退回單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IssueReturn_Detail', @level2type = N'COLUMN', @level2name = N'Id';


GO



GO



GO



GO



GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'發料數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IssueReturn_Detail', @level2type = N'COLUMN', @level2name = N'Qty';


GO



EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'儲位',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'IssueReturn_Detail',
    @level2type = N'COLUMN',
    @level2name = N'Location '