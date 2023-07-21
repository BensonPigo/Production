CREATE TABLE [dbo].[InventoryRefno] (
    [ID]          BIGINT         CONSTRAINT [DF__InventoryRef__ID__3A978D17] DEFAULT ((0)) NOT NULL,
    [Refno]       VARCHAR (36)   CONSTRAINT [DF__Inventory__Refno__3B8BB150] DEFAULT ('') NULL,
    [Width]       DECIMAL (5, 2) CONSTRAINT [DF_InventoryRefno_Width] DEFAULT ((0)) NOT NULL,
    [ProdID_Old]  VARCHAR (10)   CONSTRAINT [DF_InventoryRefno_ProdID_Old] DEFAULT ('') NOT NULL,
    [Special_Old] NVARCHAR (MAX) CONSTRAINT [DF_InventoryRefno_Special_Old] DEFAULT ('') NOT NULL,
    [Spec_Old]    NVARCHAR (MAX) CONSTRAINT [DF_InventoryRefno_Spec_Old] DEFAULT ('') NOT NULL,
    [AddName]     VARCHAR (10)   CONSTRAINT [DF_InventoryRefno_AddName] DEFAULT ('') NOT NULL,
    [AddDate]     DATETIME       NULL,
    CONSTRAINT [PK_InventoryRefno] PRIMARY KEY CLUSTERED ([ID] ASC)
);







