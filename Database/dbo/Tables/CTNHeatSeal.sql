CREATE TABLE [dbo].[CTNHeatSeal] (
    [ID]            BIGINT         IDENTITY (1, 1) NOT NULL,
    [ScanDate]      DATE           NULL,
    [MDivisionID]   VARCHAR (8)    CONSTRAINT [DF_CTNHeatSeal_MDivisionID] DEFAULT ('') NOT NULL,
    [PackingListID] VARCHAR (13)   CONSTRAINT [DF_CTNHeatSeal_PackingListID] DEFAULT ('') NOT NULL,
    [OrderID]       VARCHAR (13)   CONSTRAINT [DF_CTNHeatSeal_OrderID] DEFAULT ('') NOT NULL,
    [CTNStartNo]    VARCHAR (6)    CONSTRAINT [DF_CTNHeatSeal_CTNStartNo] DEFAULT ('') NOT NULL,
    [SCICtnNo]      VARCHAR (16)   CONSTRAINT [DF_CTNHeatSeal_SCICtnNo] DEFAULT ('') NOT NULL,
    [CartonQty]     INT            CONSTRAINT [DF_CTNHeatSeal_CartonQty] DEFAULT ((0)) NOT NULL,
    [Status]        VARCHAR (6)    CONSTRAINT [DF_CTNHeatSeal_Status] DEFAULT ('') NOT NULL,
    [Remark]        NVARCHAR (MAX) CONSTRAINT [DF_CTNHeatSeal_Remark] DEFAULT ('') NOT NULL,
    [AddName]       VARCHAR (10)   CONSTRAINT [DF_CTNHeatSeal_AddName] DEFAULT ('') NOT NULL,
    [AddDate]       DATETIME       NULL,
    CONSTRAINT [PK_CTNHeatSeal] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CTNHeatSeal', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CTNHeatSeal', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Return原因', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CTNHeatSeal', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'HP狀態 通過 : Pass 退回 : Return', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CTNHeatSeal', @level2type = N'COLUMN', @level2name = N'Status';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'總箱數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CTNHeatSeal', @level2type = N'COLUMN', @level2name = N'CartonQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SCI內部箱號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CTNHeatSeal', @level2type = N'COLUMN', @level2name = N'SCICtnNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'箱號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CTNHeatSeal', @level2type = N'COLUMN', @level2name = N'CTNStartNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CTNHeatSeal', @level2type = N'COLUMN', @level2name = N'OrderID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'PackingList ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CTNHeatSeal', @level2type = N'COLUMN', @level2name = N'PackingListID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'MDivision ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CTNHeatSeal', @level2type = N'COLUMN', @level2name = N'MDivisionID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'掃描日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CTNHeatSeal', @level2type = N'COLUMN', @level2name = N'ScanDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CTNHeatSeal', @level2type = N'COLUMN', @level2name = N'ID';

