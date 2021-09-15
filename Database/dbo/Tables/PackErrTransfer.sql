CREATE TABLE [dbo].[PackErrTransfer] (
    [ID]             BIGINT       IDENTITY (1, 1) NOT NULL,
    [TransferDate]   DATE         NULL,
    [MDivisionID]    VARCHAR (8)  CONSTRAINT [DF_PackErrTransfer_MDivisionID] DEFAULT ('') NULL,
    [OrderID]        VARCHAR (13) CONSTRAINT [DF_PackErrTransfer_OrderID] DEFAULT ('') NULL,
    [PackingListID]  VARCHAR (13) CONSTRAINT [DF_PackErrTransfer_PackingListID] DEFAULT ('') NULL,
    [CTNStartNo]     VARCHAR (6)  CONSTRAINT [DF_PackErrTransfer_CTNStartNo] DEFAULT ('') NULL,
    [AddName]        VARCHAR (10) CONSTRAINT [DF_PackErrTransfer_AddName] DEFAULT ('') NULL,
    [AddDate]        DATETIME     NULL,
    [PackingErrorID] VARCHAR (8)  DEFAULT ('') NULL,
    [SCICtnNo]       VARCHAR (15) CONSTRAINT [DF_PackErrTransfer_SCICtnNo] DEFAULT ('') NOT NULL,
    [ErrQty]         SMALLINT     CONSTRAINT [DF_PackErrTransfer_ErrQty] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_PackErrTransfer] PRIMARY KEY CLUSTERED ([ID] ASC)
);





GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'錯誤ID(Type為TP)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackErrTransfer', @level2type = N'COLUMN', @level2name = N'PackingErrorID';

