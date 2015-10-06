CREATE TABLE [dbo].[SewingOutput_Detail_Detail] (
    [ID]           VARCHAR (13) CONSTRAINT [DF_SewingOutput_Detail_Detail_ID] DEFAULT ('') NOT NULL,
    [DetailKey]    BIGINT       CONSTRAINT [DF_SewingOutput_Detail_Detail_DetailKey] DEFAULT ((0)) NOT NULL,
    [OrderId]      VARCHAR (13) CONSTRAINT [DF_SewingOutput_Detail_Detail_OrderId] DEFAULT ('') NOT NULL,
    [ComboType]    VARCHAR (1)  CONSTRAINT [DF_SewingOutput_Detail_Detail_ComboType] DEFAULT ('') NOT NULL,
    [Article]      VARCHAR (8)  CONSTRAINT [DF_SewingOutput_Detail_Detail_Article] DEFAULT ('') NOT NULL,
    [SizeCode]     VARCHAR (8)  CONSTRAINT [DF_SewingOutput_Detail_Detail_SizeCode] DEFAULT ('') NOT NULL,
    [QAQty]        INT          CONSTRAINT [DF_SewingOutput_Detail_Detail_QAQty] DEFAULT ((0)) NOT NULL,
    [OldDetailKey] VARCHAR (10) CONSTRAINT [DF_SewingOutput_Detail_Detail_OldDetailKey] DEFAULT ('') NULL,
    CONSTRAINT [PK_SewingOutput_Detail_Detail] PRIMARY KEY CLUSTERED ([ID] ASC, [DetailKey] ASC, [OrderId] ASC, [ComboType] ASC, [Article] ASC, [SizeCode] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Sewing Dailiy output(車縫日報明細檔)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingOutput_Detail_Detail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SewingOutput Id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingOutput_Detail_Detail', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Detail Key', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingOutput_Detail_Detail', @level2type = N'COLUMN', @level2name = N'DetailKey';


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

