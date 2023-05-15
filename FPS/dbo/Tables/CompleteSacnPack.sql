CREATE TABLE [dbo].[CompleteSacnPack] (
    [ID]           BIGINT       NOT NULL,
    [SCICtnNo]     VARCHAR (16) NOT NULL,
    [Article]      VARCHAR (8)  NOT NULL,
    [SizeCode]     VARCHAR (8)  NOT NULL,
    [ScanQty]      SMALLINT     NOT NULL,
    [ScanName]     VARCHAR (10) NOT NULL,
    [ScanEditDate] DATETIME     NOT NULL,
    [Time]         DATETIME     NOT NULL,
    [SCIUpdate]    BIT          NOT NULL,
    CONSTRAINT [PK_CompleteSacnPack] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SCI是否已轉製', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CompleteSacnPack', @level2type = N'COLUMN', @level2name = N'SCIUpdate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Sunrise完成時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CompleteSacnPack', @level2type = N'COLUMN', @level2name = N'Time';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'掃描日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CompleteSacnPack', @level2type = N'COLUMN', @level2name = N'ScanEditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'掃描人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CompleteSacnPack', @level2type = N'COLUMN', @level2name = N'ScanName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'掃描數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CompleteSacnPack', @level2type = N'COLUMN', @level2name = N'ScanQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'尺寸', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CompleteSacnPack', @level2type = N'COLUMN', @level2name = N'SizeCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'顏色', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CompleteSacnPack', @level2type = N'COLUMN', @level2name = N'Article';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SCI箱號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CompleteSacnPack', @level2type = N'COLUMN', @level2name = N'SCICtnNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'完成Sacn & Pack單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CompleteSacnPack', @level2type = N'COLUMN', @level2name = N'ID';

