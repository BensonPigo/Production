CREATE TABLE [dbo].[Pullout_Detail_Detail] (
    [ID]                 VARCHAR (13) CONSTRAINT [DF_Pullout_Detail_Detail_ID] DEFAULT ('') NOT NULL,
    [Pullout_DetailUKey] BIGINT       CONSTRAINT [DF_Pullout_Detail_Detail_UKey] DEFAULT ((0)) NOT NULL,
    [OrderID]            VARCHAR (13) CONSTRAINT [DF_Pullout_Detail_Detail_OrderID] DEFAULT ('') NOT NULL,
    [Article]            VARCHAR (8)  CONSTRAINT [DF_Pullout_Detail_Detail_Article] DEFAULT ('') NOT NULL,
    [SizeCode]           VARCHAR (8)  CONSTRAINT [DF_Pullout_Detail_Detail_SizeCode] DEFAULT ('') NOT NULL,
    [ShipQty]            INT          CONSTRAINT [DF_Pullout_Detail_Detail_ShipQty] DEFAULT ((0)) NULL,
    [OldUkey]            VARCHAR (13) CONSTRAINT [DF_Pullout_Detail_Detail_OldUkey] DEFAULT ('') NULL,
    CONSTRAINT [PK_Pullout_Detail_Detail] PRIMARY KEY CLUSTERED ([ID] ASC, [Pullout_DetailUKey] ASC, [OrderID] ASC, [Article] ASC, [SizeCode] ASC)
);






GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Pullout Report Detail', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pullout_Detail_Detail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pullout_Detail_Detail', @level2type = N'COLUMN', @level2name = N'ID';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pullout_Detail_Detail', @level2type = N'COLUMN', @level2name = N'OrderID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'色組', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pullout_Detail_Detail', @level2type = N'COLUMN', @level2name = N'Article';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'尺寸', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pullout_Detail_Detail', @level2type = N'COLUMN', @level2name = N'SizeCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'出貨數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pullout_Detail_Detail', @level2type = N'COLUMN', @level2name = N'ShipQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pullout_Detail_Detail', @level2type = N'COLUMN', @level2name = N'OldUkey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'UKey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pullout_Detail_Detail', @level2type = N'COLUMN', @level2name = N'Pullout_DetailUKey';

