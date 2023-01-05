CREATE TABLE [dbo].[InventoryRefNo_Spec] (
    [InventoryRefNoID] BIGINT       NOT NULL,
    [SpecColumnID]     VARCHAR (50) CONSTRAINT [DF_InventoryRefNo_Spec_SpecColumnID] DEFAULT ('') NOT NULL,
    [SpecValue]        VARCHAR (50) CONSTRAINT [DF_InventoryRefNo_Spec_SpecValue] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_InventoryRefNo_Spec] PRIMARY KEY CLUSTERED ([InventoryRefNoID] ASC, [SpecColumnID] ASC)
);

