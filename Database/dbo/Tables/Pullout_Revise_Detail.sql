CREATE TABLE [dbo].[Pullout_Revise_Detail] (
    [ID]                      VARCHAR (13) CONSTRAINT [DF_Pullout_Revise_Detail_ID] DEFAULT ('') NOT NULL,
    [Pullout_DetailUKey]      BIGINT       CONSTRAINT [DF_Pullout_Revise_Detail_UKey] DEFAULT ((0)) NOT NULL,
    [Pullout_ReviseReviseKey] BIGINT       CONSTRAINT [DF_Pullout_Revise_Detail_ReviseKey] DEFAULT ((0)) NOT NULL,
    [OrderID]                 VARCHAR (13) CONSTRAINT [DF_Pullout_Revise_Detail_OrderID] DEFAULT ('') NOT NULL,
    [Article]                 VARCHAR (8)  CONSTRAINT [DF_Pullout_Revise_Detail_Article] DEFAULT ('') NOT NULL,
    [SizeCode]                VARCHAR (8)  CONSTRAINT [DF_Pullout_Revise_Detail_SizeCode] DEFAULT ('') NOT NULL,
    [OldShipQty]              INT          CONSTRAINT [DF_Pullout_Revise_Detail_OldShipQty] DEFAULT ((0)) NOT NULL,
    [NewShipQty]              INT          CONSTRAINT [DF_Pullout_Revise_Detail_NewShipQty] DEFAULT ((0)) NOT NULL,
    [OldUKey]                 VARCHAR (13) CONSTRAINT [DF_Pullout_Revise_Detail_OldUKey] DEFAULT ('') NULL,
    [OldReviseKey]            VARCHAR (13) CONSTRAINT [DF_Pullout_Revise_Detail_OldReviseKey] DEFAULT ('') NULL,
    CONSTRAINT [PK_Pullout_Revise_Detail] PRIMARY KEY CLUSTERED ([ID] ASC, [Pullout_DetailUKey] ASC, [Pullout_ReviseReviseKey] ASC, [OrderID] ASC, [Article] ASC, [SizeCode] ASC, [OldShipQty] ASC, [NewShipQty] ASC)
);






GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Pullout Revise Detail', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pullout_Revise_Detail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pullout_Revise_Detail', @level2type = N'COLUMN', @level2name = N'ID';


GO



GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pullout_Revise_Detail', @level2type = N'COLUMN', @level2name = N'OrderID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'色組', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pullout_Revise_Detail', @level2type = N'COLUMN', @level2name = N'Article';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'尺寸', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pullout_Revise_Detail', @level2type = N'COLUMN', @level2name = N'SizeCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'原出貨數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pullout_Revise_Detail', @level2type = N'COLUMN', @level2name = N'OldShipQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'修正後的出貨數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pullout_Revise_Detail', @level2type = N'COLUMN', @level2name = N'NewShipQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pullout_Revise_Detail', @level2type = N'COLUMN', @level2name = N'OldUKey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pullout_Revise_Detail', @level2type = N'COLUMN', @level2name = N'OldReviseKey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Revise Key', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pullout_Revise_Detail', @level2type = N'COLUMN', @level2name = N'Pullout_ReviseReviseKey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'UKey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pullout_Revise_Detail', @level2type = N'COLUMN', @level2name = N'Pullout_DetailUKey';

