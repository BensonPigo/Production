CREATE TABLE [dbo].[LocationTrans] (
    [ID]         VARCHAR (13) NOT NULL,
    [Barcode]    VARCHAR (13) NOT NULL,
    [ToLocation] VARCHAR (60) NOT NULL,
    [CmdTime]    DATETIME     NOT NULL,
    [SCIUpdate]  BIT          DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_LocationTrans] PRIMARY KEY CLUSTERED ([ID] ASC, [Barcode] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SCI是否已轉入', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocationTrans', @level2type = N'COLUMN', @level2name = N'SCIUpdate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'完成時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocationTrans', @level2type = N'COLUMN', @level2name = N'CmdTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新儲位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocationTrans', @level2type = N'COLUMN', @level2name = N'ToLocation';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'布捲條碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocationTrans', @level2type = N'COLUMN', @level2name = N'Barcode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocationTrans', @level2type = N'COLUMN', @level2name = N'ID';

