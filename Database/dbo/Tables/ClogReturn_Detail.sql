CREATE TABLE [dbo].[ClogReturn_Detail] (
    [ID]               VARCHAR (13) CONSTRAINT [DF_ClogReturn_Detail_ID] DEFAULT ('') NOT NULL,
    [TransferToClogId] VARCHAR (13) CONSTRAINT [DF_ClogReturn_Detail_TransferToClogId] DEFAULT ('') NOT NULL,
    [PackingListId]    VARCHAR (13) CONSTRAINT [DF_ClogReturn_Detail_PackingListId] DEFAULT ('') NOT NULL,
    [OrderId]          VARCHAR (13) CONSTRAINT [DF_ClogReturn_Detail_OrderId] DEFAULT ('') NOT NULL,
    [CTNStartNo]       VARCHAR (6)  CONSTRAINT [DF_ClogReturn_Detail_CTNStartNo] DEFAULT ('') NOT NULL,
    [AddName]          VARCHAR (10) CONSTRAINT [DF_ClogReturn_Detail_AddName] DEFAULT ('') NULL,
    [AddDate]          DATETIME     NULL,
    CONSTRAINT [PK_ClogReturn_Detail] PRIMARY KEY CLUSTERED ([ID] ASC, [TransferToClogId] ASC, [PackingListId] ASC, [OrderId] ASC, [CTNStartNo] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Carton Return Detail', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ClogReturn_Detail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ClogReturn_Detail', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Transfer To Clog Id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ClogReturn_Detail', @level2type = N'COLUMN', @level2name = N'TransferToClogId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Packing List Id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ClogReturn_Detail', @level2type = N'COLUMN', @level2name = N'PackingListId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ClogReturn_Detail', @level2type = N'COLUMN', @level2name = N'OrderId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'箱號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ClogReturn_Detail', @level2type = N'COLUMN', @level2name = N'CTNStartNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ClogReturn_Detail', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ClogReturn_Detail', @level2type = N'COLUMN', @level2name = N'AddDate';

