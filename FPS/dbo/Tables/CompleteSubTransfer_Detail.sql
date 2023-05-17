CREATE TABLE [dbo].[CompleteSubTransfer_Detail] (
    [ID]           VARCHAR (13) NOT NULL,
    [Barcode]      VARCHAR (13) NOT NULL,
    [Ukey]         BIGINT       DEFAULT ((0)) NOT NULL,
    [CompleteTime] DATETIME     NOT NULL,
    [SCIUpdate]    BIT          DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_CompleteSubTransfer_Detail] PRIMARY KEY CLUSTERED ([ID] ASC, [Ukey] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SCI是否已轉入', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CompleteSubTransfer_Detail', @level2type = N'COLUMN', @level2name = N'SCIUpdate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'完成時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CompleteSubTransfer_Detail', @level2type = N'COLUMN', @level2name = N'CompleteTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'布捲條碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CompleteSubTransfer_Detail', @level2type = N'COLUMN', @level2name = N'Barcode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'轉倉單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CompleteSubTransfer_Detail', @level2type = N'COLUMN', @level2name = N'ID';

