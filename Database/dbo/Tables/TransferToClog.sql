CREATE TABLE [dbo].[TransferToClog] (
    [ID]            BIGINT       IDENTITY (1, 1) NOT NULL,
    [TransferDate]  DATE         NOT NULL,
    [MDivisionID]   VARCHAR (8)  CONSTRAINT [DF_TransferToClog_Detail_MDivisionID] DEFAULT ('') NOT NULL,
    [PackingListID] VARCHAR (13) CONSTRAINT [DF_TransferToClog_Detail_PackingListID] DEFAULT ('') NOT NULL,
    [OrderID]       VARCHAR (13) CONSTRAINT [DF_TransferToClog_Detail_OrderID] DEFAULT ('') NOT NULL,
    [CTNStartNo]    VARCHAR (6)  CONSTRAINT [DF_TransferToClog_Detail_CTNStartNo] DEFAULT ('') NOT NULL,
    [AddDate]       DATETIME     NULL,
    [OldID]         VARCHAR (13) CONSTRAINT [DF_TransferToClog_Detail_OldID] DEFAULT ('') NULL,
    [TransferSlipNo] VARCHAR(13) NULL, 
    [SCICtnNo] VARCHAR(16) CONSTRAINT [DF_TransferToClog_Detail_SCICtnNo] DEFAULT ('') NOT NULL, 
    CONSTRAINT [PK_TransferToClog_Detail_1] PRIMARY KEY CLUSTERED ([ID] ASC)
);






GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Carton transfer to clog detail', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TransferToClog';




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TransferToClog', @level2type = N'COLUMN', @level2name = N'Id';


GO



GO



GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TransferToClog', @level2type = N'COLUMN', @level2name = N'AddDate';


GO



GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Packing List Id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TransferToClog', @level2type = N'COLUMN', @level2name = N'PackingListID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TransferToClog', @level2type = N'COLUMN', @level2name = N'OrderID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'箱號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TransferToClog', @level2type = N'COLUMN', @level2name = N'CTNStartNo';

