CREATE TABLE [dbo].[CompleteTransferToCFA] (
    [ID]        BIGINT       NOT NULL,
    [SCICtnNo]  VARCHAR (16) DEFAULT ('') NOT NULL,
    [Time]      DATETIME     NOT NULL,
    [SCIUpdate] BIT          DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_CompleteTransferToCFA] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SCI箱號(已轉到驗貨區的箱號)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CompleteTransferToCFA', @level2type = N'COLUMN', @level2name = N'SCICtnNo';

