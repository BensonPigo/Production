CREATE TABLE [dbo].[Pullout_Revise] (
    [ID]            VARCHAR (13)  CONSTRAINT [DF_Pullout_Revise_ID] DEFAULT ('') NOT NULL,
    [Type]          VARCHAR (1)   CONSTRAINT [DF_Pullout_Revise_Type] DEFAULT ('') NOT NULL,
    [OrderID]       VARCHAR (13)  CONSTRAINT [DF_Pullout_Revise_OrderID] DEFAULT ('') NOT NULL,
    [OldShipQty]    INT           CONSTRAINT [DF_Pullout_Revise_OldShipQty] DEFAULT ((0)) NULL,
    [NewShipQty]    INT           CONSTRAINT [DF_Pullout_Revise_NewShipQty] DEFAULT ((0)) NULL,
    [OldStatus]     VARCHAR (1)   CONSTRAINT [DF_Pullout_Revise_OldStatus] DEFAULT ('') NULL,
    [NewStatus]     VARCHAR (1)   CONSTRAINT [DF_Pullout_Revise_NewStatus] DEFAULT ('') NULL,
    [PackingListID] VARCHAR (13)  CONSTRAINT [DF_Pullout_Revise_PackingListID] DEFAULT ('') NULL,
    [Remark]        NVARCHAR (30) CONSTRAINT [DF_Pullout_Revise_Remark] DEFAULT ('') NULL,
    [UKey]          BIGINT        CONSTRAINT [DF_Pullout_Revise_UKey] DEFAULT ((0)) NULL,
    [ReviseKey]     BIGINT        IDENTITY (1, 1) NOT NULL,
    [INVNo]         VARCHAR (25)  CONSTRAINT [DF_Pullout_Revise_INVNo] DEFAULT ('') NULL,
    [ShipModeID]    VARCHAR (5)   CONSTRAINT [DF_Pullout_Revise_ShipModeID] DEFAULT ('') NULL,
    [AddName]       VARCHAR (10)  CONSTRAINT [DF_Pullout_Revise_AddName] DEFAULT ('') NULL,
    [AddDate]       DATETIME      NULL,
    [OldUKey]       VARCHAR (13)  CONSTRAINT [DF_Pullout_Revise_OldUKey] DEFAULT ('') NULL,
    [OldReviseKey]  VARCHAR (13)  CONSTRAINT [DF_Pullout_Revise_OldReviseKey] DEFAULT ('') NULL,
    CONSTRAINT [PK_Pullout_Revise] PRIMARY KEY CLUSTERED ([ReviseKey] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Pullout Revise', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pullout_Revise';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pullout_Revise', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'修改類型', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pullout_Revise', @level2type = N'COLUMN', @level2name = N'Type';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pullout_Revise', @level2type = N'COLUMN', @level2name = N'OrderID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'原出貨數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pullout_Revise', @level2type = N'COLUMN', @level2name = N'OldShipQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'修正後的出貨數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pullout_Revise', @level2type = N'COLUMN', @level2name = N'NewShipQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'原狀態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pullout_Revise', @level2type = N'COLUMN', @level2name = N'OldStatus';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'修正後的狀態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pullout_Revise', @level2type = N'COLUMN', @level2name = N'NewStatus';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Packing List ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pullout_Revise', @level2type = N'COLUMN', @level2name = N'PackingListID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pullout_Revise', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'UKey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pullout_Revise', @level2type = N'COLUMN', @level2name = N'UKey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Revise Key', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pullout_Revise', @level2type = N'COLUMN', @level2name = N'ReviseKey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Garment Booking ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pullout_Revise', @level2type = N'COLUMN', @level2name = N'INVNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Ship Mode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pullout_Revise', @level2type = N'COLUMN', @level2name = N'ShipModeID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pullout_Revise', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pullout_Revise', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pullout_Revise', @level2type = N'COLUMN', @level2name = N'OldUKey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pullout_Revise', @level2type = N'COLUMN', @level2name = N'OldReviseKey';

