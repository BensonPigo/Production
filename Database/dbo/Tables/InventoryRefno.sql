CREATE TABLE [dbo].[InventoryRefno] (
    [ID]          BIGINT         CONSTRAINT [DF__InventoryRefno__ID] DEFAULT ((0)) NOT NULL,
    [Refno]       VARCHAR (36)   CONSTRAINT [DF__InventoryRefno__Refno] DEFAULT ('') NULL,
    [Width]       NUMERIC (5, 2) CONSTRAINT [DF__InventoryRefno__Width] DEFAULT ((0)) NULL,
    [BomArticle]  VARCHAR (8)    CONSTRAINT [DF__InventoryRefno__BomArticle] DEFAULT ('') NULL,
    [BomBuymonth] VARCHAR (10)   CONSTRAINT [DF__InventoryRefno__BomBuymonth] DEFAULT ('') NULL,
    [BomCountry]  VARCHAR (2)    CONSTRAINT [DF__InventoryRefno__BomCountry] DEFAULT ('') NULL,
    [BomCustCD]   VARCHAR (20)   CONSTRAINT [DF__InventoryRefno__BomCustCD] DEFAULT ('') NULL,
    [BomFactory]  VARCHAR (8)    CONSTRAINT [DF__InventoryRefno__BomFactory] DEFAULT ('') NULL,
    [BomStyle]    VARCHAR (15)   CONSTRAINT [DF__InventoryRefno__BomStyle] DEFAULT ('') NULL,
    [ProdID_Old]  VARCHAR (10)   CONSTRAINT [DF__InventoryRefno__ProdID_Old] DEFAULT ('') NULL,
    [Special_Old] NVARCHAR (MAX) CONSTRAINT [DF__InventoryRefno__Special_Old] DEFAULT ('') NULL,
    [Spec_Old]    NVARCHAR (MAX) CONSTRAINT [DF__InventoryRefno__Spec_Old] DEFAULT ('') NULL,
    [AddName]     VARCHAR (10)   CONSTRAINT [DF__InventoryRefno__AddName] DEFAULT ('') NULL,
    [AddDate]     DATETIME       NULL,
    CONSTRAINT [PK_InventoryRefno] PRIMARY KEY CLUSTERED ([ID] ASC)
);





