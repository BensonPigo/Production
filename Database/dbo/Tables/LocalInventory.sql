CREATE TABLE [dbo].[LocalInventory] (
    [OrderID]       VARCHAR (13)   CONSTRAINT [DF_LocalInventory_POID] DEFAULT ('') NOT NULL,
    [Refno]         VARCHAR (21)   CONSTRAINT [DF_LocalInventory_Refno] DEFAULT ('') NOT NULL,
    [ThreadColorID] VARCHAR (15)   CONSTRAINT [DF_LocalInventory_ColorID] DEFAULT ('') NOT NULL,
    [InQty]         NUMERIC (8, 2) CONSTRAINT [DF_LocalInventory_Qty] DEFAULT ((0)) NULL,
    [OutQty]        NUMERIC (8, 2) CONSTRAINT [DF_LocalInventory_OutQty] DEFAULT ((0)) NULL,
    [AdjustQty]     NUMERIC (8, 2) CONSTRAINT [DF_LocalInventory_AdjustQty] DEFAULT ((0)) NULL,
    [UnitId]        VARCHAR (8)    CONSTRAINT [DF_LocalInventory_UnitId] DEFAULT ('') NOT NULL,
    [LobQty] NUMERIC(11, 2) NULL DEFAULT ((0)), 
    [ALocation] VARCHAR(100) NULL DEFAULT (''), 
    [CLocation] VARCHAR(100) NULL DEFAULT (''), 
    CONSTRAINT [PK_LocalInventory_1] PRIMARY KEY CLUSTERED ([OrderID] ASC, [Refno] ASC, [ThreadColorID] ASC)
);






GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'C倉數量',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LocalInventory',
    @level2type = N'COLUMN',
    @level2name = N'LobQty'