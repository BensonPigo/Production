CREATE TABLE [dbo].[SewingSchedule_Detail] (
    [ID]           BIGINT       CONSTRAINT [DF_SewingSchedule_Detail_ID] DEFAULT ((0)) NOT NULL,
    [OrderID]      VARCHAR (13) CONSTRAINT [DF_SewingSchedule_Detail_OrderID] DEFAULT ('') NOT NULL,
    [ComboType]    VARCHAR (1)  CONSTRAINT [DF_SewingSchedule_Detail_ComboType] DEFAULT ('') NOT NULL,
    [SewingLineID] VARCHAR (5)  CONSTRAINT [DF_SewingSchedule_Detail_SewingLineID] DEFAULT ('') NOT NULL,
    [Article]      VARCHAR (8)  CONSTRAINT [DF_SewingSchedule_Detail_Article] DEFAULT ('') NOT NULL,
    [SizeCode]     VARCHAR (8)  CONSTRAINT [DF_SewingSchedule_Detail_SizeCode] DEFAULT ('') NOT NULL,
    [AlloQty]      INT          CONSTRAINT [DF_SewingSchedule_Detail_AlloQty] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_SewingSchedule_Detail] PRIMARY KEY CLUSTERED ([ID] ASC, [OrderID] ASC, [ComboType] ASC, [SewingLineID] ASC, [Article] ASC, [SizeCode] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Sewing Schedule Detail', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingSchedule_Detail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingSchedule_Detail', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingSchedule_Detail', @level2type = N'COLUMN', @level2name = N'OrderID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組合型態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingSchedule_Detail', @level2type = N'COLUMN', @level2name = N'ComboType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'車縫線', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingSchedule_Detail', @level2type = N'COLUMN', @level2name = N'SewingLineID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'色組', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingSchedule_Detail', @level2type = N'COLUMN', @level2name = N'Article';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'尺寸', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingSchedule_Detail', @level2type = N'COLUMN', @level2name = N'SizeCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'生產數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingSchedule_Detail', @level2type = N'COLUMN', @level2name = N'AlloQty';


GO
CREATE NONCLUSTERED INDEX [orderid_art_sizecode]
    ON [dbo].[SewingSchedule_Detail]([OrderID] ASC, [Article] ASC, [SizeCode] ASC);

