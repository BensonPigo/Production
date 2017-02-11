CREATE TABLE [dbo].[InventoryRefno] (
    [ID]              BIGINT         CONSTRAINT [DF__InventoryRef__ID__78C9C9BA] DEFAULT ((0)) NOT NULL,
    [Refno]           VARCHAR (20)   CONSTRAINT [DF__Inventory__Refno__79BDEDF3] DEFAULT ('') NOT NULL,
    [Width]           NUMERIC (5, 1) CONSTRAINT [DF__Inventory__Width__7AB2122C] DEFAULT ((0)) NULL,
    [ColorID]         VARCHAR (70)   CONSTRAINT [DF__Inventory__Color__7BA63665] DEFAULT ('') NULL,
    [SizeSpec]        VARCHAR (8)    CONSTRAINT [DF__Inventory__SizeS__7C9A5A9E] DEFAULT ('') NULL,
    [SizeUnit]        VARCHAR (8)    CONSTRAINT [DF__Inventory__SizeU__7D8E7ED7] DEFAULT ('') NULL,
    [BomArticle]      VARCHAR (8)    CONSTRAINT [DF__Inventory__BomAr__7E82A310] DEFAULT ('') NULL,
    [BomBuymonth]     VARCHAR (10)   CONSTRAINT [DF__Inventory__BomBu__7F76C749] DEFAULT ('') NULL,
    [BomCountry]      VARCHAR (2)    CONSTRAINT [DF__Inventory__BomCo__006AEB82] DEFAULT ('') NULL,
    [BomCustCD]       VARCHAR (20)   CONSTRAINT [DF__Inventory__BomCu__015F0FBB] DEFAULT ('') NULL,
    [BomCustPONo]     VARCHAR (30)   CONSTRAINT [DF__Inventory__BomCu__025333F4] DEFAULT ('') NULL,
    [BomFactory]      VARCHAR (8)    CONSTRAINT [DF__Inventory__BomFa__0347582D] DEFAULT ('') NULL,
    [BomStyle]        VARCHAR (15)   CONSTRAINT [DF__Inventory__BomSt__043B7C66] DEFAULT ('') NULL,
    [BomZipperInsert] VARCHAR (5)    CONSTRAINT [DF__Inventory__BomZi__052FA09F] DEFAULT ('') NULL,
    [ProdID_Old]      VARCHAR (10)   CONSTRAINT [DF__Inventory__ProdI__0623C4D8] DEFAULT ('') NULL,
    [Special_Old]     NVARCHAR (60)  CONSTRAINT [DF__Inventory__Speci__0717E911] DEFAULT ('') NULL,
    [Spec_Old]        NVARCHAR (MAX) CONSTRAINT [DF__Inventory__Spec___080C0D4A] DEFAULT ('') NULL,
    [AddName]         VARCHAR (10)   CONSTRAINT [DF__Inventory__AddNa__09003183] DEFAULT ('') NULL,
    [AddDate]         DATETIME       NULL,
    CONSTRAINT [PK_InventoryRefno] PRIMARY KEY CLUSTERED ([ID] ASC)
);



