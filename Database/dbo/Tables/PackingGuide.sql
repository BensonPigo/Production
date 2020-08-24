CREATE TABLE [dbo].[PackingGuide] (
    [Id]                 VARCHAR (13)   CONSTRAINT [DF_PackingGuide_Id] DEFAULT ('') NOT NULL,
    [MDivisionID]        VARCHAR (8)    CONSTRAINT [DF_PackingGuide_MDivisionID] DEFAULT ('') NOT NULL,
    [FactoryID]          VARCHAR (8)    CONSTRAINT [DF_PackingGuide_FactoryID] DEFAULT ('') NOT NULL,
    [OrderID]            VARCHAR (13)   CONSTRAINT [DF_PackingGuide_OrderID] DEFAULT ('') NOT NULL,
    [ShipModeID]         VARCHAR (10)   CONSTRAINT [DF_PackingGuide_ShipModeID] DEFAULT ('') NOT NULL,
    [CTNQty]             INT            CONSTRAINT [DF_PackingGuide_CTNQty] DEFAULT ((0)) NULL,
    [CTNStartNo]         INT            CONSTRAINT [DF_PackingGuide_CTNStartNo] DEFAULT ((0)) NULL,
    [SpecialInstruction] NVARCHAR (MAX) CONSTRAINT [DF_PackingGuide_SpecialInstruction] DEFAULT ('') NULL,
    [CBM]                NUMERIC (10, 3) CONSTRAINT [DF_PackingGuide_CBM] DEFAULT ((0)) NULL,
    [Remark]             NVARCHAR (150) CONSTRAINT [DF_PackingGuide_Remark] DEFAULT ('') NULL,
    [OrderShipmodeSeq]   VARCHAR (2)    CONSTRAINT [DF_PackingGuide_OrderShipmodeSeq] DEFAULT ('') NULL,
    [AddName]            VARCHAR (10)   CONSTRAINT [DF_PackingGuide_AddName] DEFAULT ('') NULL,
    [AddDate]            DATETIME       NULL,
    [EditName]           VARCHAR (10)   CONSTRAINT [DF_PackingGuide_EditName] DEFAULT ('') NULL,
    [EditDate]           DATETIME       NULL,
    [EstCTNBooking] DATE NULL, 
    [EstCTNArrive] DATE NULL, 
    CONSTRAINT [PK_PackingGuide] PRIMARY KEY CLUSTERED ([Id] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Packing Guide', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingGuide';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingGuide', @level2type = N'COLUMN', @level2name = N'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingGuide', @level2type = N'COLUMN', @level2name = N'FactoryID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingGuide', @level2type = N'COLUMN', @level2name = N'OrderID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'出貨方式', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingGuide', @level2type = N'COLUMN', @level2name = N'ShipModeID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'總箱數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingGuide', @level2type = N'COLUMN', @level2name = N'CTNQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'開始箱號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingGuide', @level2type = N'COLUMN', @level2name = N'CTNStartNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'裝箱說明', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingGuide', @level2type = N'COLUMN', @level2name = N'SpecialInstruction';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'材積', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingGuide', @level2type = N'COLUMN', @level2name = N'CBM';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingGuide', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Qty Breakdown Shipmode的Seq', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingGuide', @level2type = N'COLUMN', @level2name = N'OrderShipmodeSeq';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingGuide', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingGuide', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingGuide', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingGuide', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Manufacturing Division ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PackingGuide', @level2type = N'COLUMN', @level2name = N'MDivisionID';


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'箱子預計下單日',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'PackingGuide',
    @level2type = N'COLUMN',
    @level2name = N'EstCTNBooking'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'箱子預計到達日',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'PackingGuide',
    @level2type = N'COLUMN',
    @level2name = N'EstCTNArrive'