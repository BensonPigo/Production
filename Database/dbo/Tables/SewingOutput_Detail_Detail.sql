CREATE TABLE [dbo].[SewingOutput_Detail_Detail] (
    [ID]                      VARCHAR (13) CONSTRAINT [DF_SewingOutput_Detail_Detail_ID] DEFAULT ('') NOT NULL,
    [SewingOutput_DetailUKey] BIGINT       CONSTRAINT [DF_SewingOutput_Detail_Detail_DetailKey] DEFAULT ((0)) NOT NULL,
    [OrderId]                 VARCHAR (13) CONSTRAINT [DF_SewingOutput_Detail_Detail_OrderId] DEFAULT ('') NOT NULL,
    [ComboType]               VARCHAR (1)  CONSTRAINT [DF_SewingOutput_Detail_Detail_ComboType] DEFAULT ('') NOT NULL,
    [Article]                 VARCHAR (8)  CONSTRAINT [DF_SewingOutput_Detail_Detail_Article] DEFAULT ('') NOT NULL,
    [SizeCode]                VARCHAR (8)  CONSTRAINT [DF_SewingOutput_Detail_Detail_SizeCode] DEFAULT ('') NOT NULL,
    [QAQty]                   INT          CONSTRAINT [DF_SewingOutput_Detail_Detail_QAQty] DEFAULT ((0)) NOT NULL,
    [OldDetailKey]            VARCHAR (13) CONSTRAINT [DF_SewingOutput_Detail_Detail_OldDetailKey] DEFAULT ('') NULL,
    CONSTRAINT [PK_SewingOutput_Detail_Detail] PRIMARY KEY CLUSTERED ([ID] ASC, [SewingOutput_DetailUKey] ASC, [OrderId] ASC, [ComboType] ASC, [Article] ASC, [SizeCode] ASC)
);










GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Sewing Dailiy output(車縫日報明細檔)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingOutput_Detail_Detail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SewingOutput Id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingOutput_Detail_Detail', @level2type = N'COLUMN', @level2name = N'ID';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingOutput_Detail_Detail', @level2type = N'COLUMN', @level2name = N'OrderId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組合型態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingOutput_Detail_Detail', @level2type = N'COLUMN', @level2name = N'ComboType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'色組', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingOutput_Detail_Detail', @level2type = N'COLUMN', @level2name = N'Article';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Size Code', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingOutput_Detail_Detail', @level2type = N'COLUMN', @level2name = N'SizeCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'產出數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingOutput_Detail_Detail', @level2type = N'COLUMN', @level2name = N'QAQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingOutput_Detail_Detail', @level2type = N'COLUMN', @level2name = N'OldDetailKey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Detail Key', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingOutput_Detail_Detail', @level2type = N'COLUMN', @level2name = N'SewingOutput_DetailUKey';


GO
CREATE NONCLUSTERED INDEX [SewingOutput_DetailUkey]
    ON [dbo].[SewingOutput_Detail_Detail]([SewingOutput_DetailUKey] ASC, [Article] ASC, [SizeCode] ASC);


GO
CREATE NONCLUSTERED INDEX [OrderID]
    ON [dbo].[SewingOutput_Detail_Detail]([OrderId] ASC, [Article] ASC, [SizeCode] ASC, [ComboType] ASC);


GO
CREATE NONCLUSTERED INDEX [Shipping_R40]
    ON [dbo].[SewingOutput_Detail_Detail]([Article] ASC, [SizeCode] ASC)
    INCLUDE([OrderId], [ComboType], [QAQty]);

