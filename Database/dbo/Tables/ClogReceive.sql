CREATE TABLE [dbo].[ClogReceive] (
    [ID]             BIGINT       IDENTITY (1, 1) NOT NULL,
    [ReceiveDate]    DATE         NOT NULL,
    [MDivisionID]    VARCHAR (8)  CONSTRAINT [DF_ClogReceive_Detail_MDivisionID] DEFAULT ('') NOT NULL,
    [PackingListID]  VARCHAR (13) CONSTRAINT [DF_ClogReceive_Detail_PackingListId] DEFAULT ('') NOT NULL,
    [OrderID]        VARCHAR (13) CONSTRAINT [DF_ClogReceive_Detail_OrderId] DEFAULT ('') NOT NULL,
    [CTNStartNo]     VARCHAR (6)  CONSTRAINT [DF_ClogReceive_Detail_CTNStartNo] DEFAULT ('') NOT NULL,
    [ClogLocationID] VARCHAR (10) CONSTRAINT [DF_ClogReceive_Detail_ClogLocationId] DEFAULT ('') NULL,
    [AddDate]        DATETIME     NULL,
    [OldID]          VARCHAR (13) CONSTRAINT [DF_ClogReceive_Detail_OldID] DEFAULT ('') NULL,
    [AddName]        VARCHAR (10) CONSTRAINT [DF_ClogReceive_AddName] DEFAULT ('') NULL, 
    [SCICtnNo] VARCHAR(15) CONSTRAINT [DF_ClogReceive_SCICtnNo] DEFAULT ('') NOT NULL, 
    CONSTRAINT [PK_ClogReceive_Detail_1] PRIMARY KEY CLUSTERED ([ID] ASC)
);












GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Carton Receiving Detail', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ClogReceive';




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ClogReceive', @level2type = N'COLUMN', @level2name = N'ID';


GO



GO



GO



GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ClogReceive', @level2type = N'COLUMN', @level2name = N'AddDate';


GO



GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Packing List Id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ClogReceive', @level2type = N'COLUMN', @level2name = N'PackingListID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ClogReceive', @level2type = N'COLUMN', @level2name = N'OrderID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'箱號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ClogReceive', @level2type = N'COLUMN', @level2name = N'CTNStartNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'儲位編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ClogReceive', @level2type = N'COLUMN', @level2name = N'ClogLocationID';


GO
CREATE NONCLUSTERED INDEX [Index_ReadyDateIndex]
    ON [dbo].[ClogReceive]([ReceiveDate] ASC, [PackingListID] ASC, [OrderID] ASC, [CTNStartNo] ASC)
    INCLUDE([AddDate]);


GO
CREATE NONCLUSTERED INDEX [Index_SDP_Report]
    ON [dbo].[ClogReceive]([OrderID] ASC)
    INCLUDE([PackingListID], [CTNStartNo], [AddDate]);

