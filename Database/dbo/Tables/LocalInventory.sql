CREATE TABLE [dbo].[LocalInventory] (
    [MDivisionID]   VARCHAR (8)    NOT NULL,
    [OrderID]       VARCHAR (13)   CONSTRAINT [DF_LocalInventory_POID] DEFAULT ('') NOT NULL,
    [Refno]         VARCHAR (21)   CONSTRAINT [DF_LocalInventory_Refno] DEFAULT ('') NOT NULL,
    [ThreadColorID] VARCHAR (15)   CONSTRAINT [DF_LocalInventory_ColorID] DEFAULT ('') NOT NULL,
    [InQty]         NUMERIC (8, 2) CONSTRAINT [DF_LocalInventory_Qty] DEFAULT ((0)) NULL,
    [OutQty]        NUMERIC (8, 2) CONSTRAINT [DF_LocalInventory_OutQty] DEFAULT ((0)) NULL,
    [AdjustQty]     NUMERIC (8, 2) CONSTRAINT [DF_LocalInventory_AdjustQty] DEFAULT ((0)) NULL,
    [UnitId]        VARCHAR (8)    CONSTRAINT [DF_LocalInventory_UnitId] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_LocalInventory] PRIMARY KEY CLUSTERED ([MDivisionID] ASC, [OrderID] ASC, [Refno] ASC, [ThreadColorID] ASC)
);

