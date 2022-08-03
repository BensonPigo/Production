CREATE TABLE [dbo].[InventoryRefno] (
    [ID]          BIGINT         CONSTRAINT [DF__InventoryRef__ID__3A978D17] DEFAULT ((0)) NOT NULL,
    [Refno]       VARCHAR (36)   CONSTRAINT [DF__Inventory__Refno__3B8BB150] DEFAULT ('') NULL,
    [Width]       NUMERIC (5, 2) CONSTRAINT [DF__Inventory__Width__3C7FD589] DEFAULT ((0)) NULL,
    [BomArticle]  VARCHAR (8)    CONSTRAINT [DF__Inventory__BomAr__4050666D] DEFAULT ('') NULL,
    [BomBuymonth] VARCHAR (10)   CONSTRAINT [DF__Inventory__BomBu__41448AA6] DEFAULT ('') NULL,
    [BomCountry]  VARCHAR (2)    CONSTRAINT [DF__Inventory__BomCo__4238AEDF] DEFAULT ('') NULL,
    [BomCustCD]   VARCHAR (20)   CONSTRAINT [DF__Inventory__BomCu__432CD318] DEFAULT ('') NULL,
    [BomFactory]  VARCHAR (8)    CONSTRAINT [DF__Inventory__BomFa__45151B8A] DEFAULT ('') NULL,
    [BomStyle]    VARCHAR (15)   CONSTRAINT [DF__Inventory__BomSt__46093FC3] DEFAULT ('') NULL,
    [ProdID_Old]  VARCHAR (10)   CONSTRAINT [DF__Inventory__ProdI__47F18835] DEFAULT ('') NULL,
    [Special_Old] NVARCHAR (MAX) CONSTRAINT [DF__Inventory__Speci__48E5AC6E] DEFAULT ('') NULL,
    [Spec_Old]    NVARCHAR (MAX) CONSTRAINT [DF__Inventory__Spec___49D9D0A7] DEFAULT ('') NULL,
    [AddName]     VARCHAR (10)   CONSTRAINT [DF__Inventory__AddNa__4ACDF4E0] DEFAULT ('') NULL,
    [AddDate]     DATETIME       NULL,
    CONSTRAINT [PK_InventoryRefno] PRIMARY KEY CLUSTERED ([ID] ASC)
);





