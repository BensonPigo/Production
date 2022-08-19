CREATE TABLE [dbo].[TransferToCFA] (
    [ID]             BIGINT       IDENTITY (1, 1) NOT NULL,
    [TransferDate]   DATE         NOT NULL,
    [MDivisionID]    VARCHAR (8)  DEFAULT ('') NOT NULL,
    [OrderID]        VARCHAR (13) DEFAULT ('') NOT NULL,
    [PackingListID]  VARCHAR (13) DEFAULT ('') NOT NULL,
    [CTNStartNo]     VARCHAR (6)  DEFAULT ('') NOT NULL,
    [AddName]        VARCHAR (10) DEFAULT ('') NOT NULL,
    [AddDate]        DATETIME     NOT NULL,
    [OrigLoactionID] VARCHAR (10) DEFAULT ('') NULL,
    [CompleteTime]   DATETIME     NULL,
    [SCICtnNo] VARCHAR(16) CONSTRAINT [DF_TransferToCFA_SCICtnNo] DEFAULT ('') NOT NULL, 
    PRIMARY KEY CLUSTERED ([ID] ASC)
);





GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Identity',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferToCFA',
    @level2type = N'COLUMN',
    @level2name = N'ID'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'������', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TransferToCFA', @level2type = N'COLUMN', @level2name = N'TransferDate';


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'M',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferToCFA',
    @level2type = N'COLUMN',
    @level2name = N'MDivisionID'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�q�渹�X', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TransferToCFA', @level2type = N'COLUMN', @level2name = N'OrderID';


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'PackID',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TransferToCFA',
    @level2type = N'COLUMN',
    @level2name = N'PackingListID'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�c��', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TransferToCFA', @level2type = N'COLUMN', @level2name = N'CTNStartNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�s�W�H��', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TransferToCFA', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�s�W�ɶ�', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TransferToCFA', @level2type = N'COLUMN', @level2name = N'AddDate';

