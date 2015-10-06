CREATE TABLE [dbo].[TransferToClog_Detail] (
    [Id]            VARCHAR (13) CONSTRAINT [DF_TransferToClog_Detail_Id] DEFAULT ('') NOT NULL,
    [PackingListID] VARCHAR (13) CONSTRAINT [DF_TransferToClog_Detail_PackingListID] DEFAULT ('') NOT NULL,
    [OrderID]       VARCHAR (13) CONSTRAINT [DF_TransferToClog_Detail_OrderID] DEFAULT ('') NOT NULL,
    [CTNStartNo]    VARCHAR (6)  CONSTRAINT [DF_TransferToClog_Detail_CTNStartNo] DEFAULT ('') NOT NULL,
    [AddName]       VARCHAR (10) CONSTRAINT [DF_TransferToClog_Detail_AddName] DEFAULT ('') NULL,
    [AddDate]       DATETIME     NULL,
    CONSTRAINT [PK_TransferToClog_Detail] PRIMARY KEY CLUSTERED ([Id] ASC, [PackingListID] ASC, [CTNStartNo] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Carton transfer to clog detail', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TransferToClog_Detail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TransferToClog_Detail', @level2type = N'COLUMN', @level2name = N'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Packing List Id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TransferToClog_Detail', @level2type = N'COLUMN', @level2name = N'PackingListID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TransferToClog_Detail', @level2type = N'COLUMN', @level2name = N'OrderID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'箱號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TransferToClog_Detail', @level2type = N'COLUMN', @level2name = N'CTNStartNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TransferToClog_Detail', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TransferToClog_Detail', @level2type = N'COLUMN', @level2name = N'AddDate';

