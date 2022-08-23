CREATE TABLE [dbo].[CFAReceive] (
    [ID]            BIGINT       IDENTITY (1, 1) NOT NULL,
    [ReceiveDate]   DATE         NOT NULL,
    [MDivisionID]   VARCHAR (8)  DEFAULT ('') NOT NULL,
    [OrderID]       VARCHAR (13) DEFAULT ('') NOT NULL,
    [PackingListID] VARCHAR (13) DEFAULT ('') NOT NULL,
    [CTNStartNo]    VARCHAR (6)  DEFAULT ('') NOT NULL,
    [AddName]       VARCHAR (10) DEFAULT ('') NOT NULL,
    [AddDate]       DATETIME     NOT NULL,
    [SCICtnNo]      VARCHAR (16) CONSTRAINT [DF_CFAReceive_SCICtnNo] DEFAULT ('') NOT NULL,
    [CFALocationID] VARCHAR (10) DEFAULT ('') NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);



GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Identity',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'CFAReceive',
    @level2type = N'COLUMN',
    @level2name = N'ID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'M',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'CFAReceive',
    @level2type = N'COLUMN',
    @level2name = N'MDivisionID'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�q�渹�X', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CFAReceive', @level2type = N'COLUMN', @level2name = N'OrderID';


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'PackID',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'CFAReceive',
    @level2type = N'COLUMN',
    @level2name = N'PackingListID'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�c��', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CFAReceive', @level2type = N'COLUMN', @level2name = N'CTNStartNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�s�W�H��', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CFAReceive', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�s�W�ɶ�', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CFAReceive', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�������', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CFAReceive', @level2type = N'COLUMN', @level2name = N'ReceiveDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'CFA儲位代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CFAReceive', @level2type = N'COLUMN', @level2name = N'CFALocationID';

