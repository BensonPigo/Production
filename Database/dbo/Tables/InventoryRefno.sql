CREATE TABLE [dbo].[InventoryRefno] (
    [ID]              BIGINT          DEFAULT ((0)) NOT NULL,
    [Refno]           VARCHAR (20)    DEFAULT ('') NOT NULL,
    [Width]           NUMERIC (5, 1)  DEFAULT ((0)) NULL,
    [ColorID]         VARCHAR (70)    DEFAULT ('') NULL,
    [SizeSpec]        VARCHAR (8)     DEFAULT ('') NULL,
    [SizeUnit]        VARCHAR (8)     DEFAULT ('') NULL,
    [BomArticle]      VARCHAR (8)     DEFAULT ('') NULL,
    [BomBuymonth]     VARCHAR (10)    DEFAULT ('') NULL,
    [BomCountry]      VARCHAR (2)     DEFAULT ('') NULL,
    [BomCustCD]       VARCHAR (20)    DEFAULT ('') NULL,
    [BomCustPONo]     VARCHAR (30)    DEFAULT ('') NULL,
    [BomFactory]      VARCHAR (8)     DEFAULT ('') NULL,
    [BomStyle]        VARCHAR (15)    DEFAULT ('') NULL,
    [BomZipperInsert] VARCHAR (5)     DEFAULT ('') NULL,
    [ProdID_Old]      VARCHAR (10)    DEFAULT ('') NULL,
    [Special_Old]     NVARCHAR (60)   DEFAULT ('') NULL,
    [Spec_Old]        NVARCHAR (MAX)  DEFAULT ('') NULL,
    [AddName]         VARCHAR (10)    DEFAULT ('') NULL,
    [AddDate]         DATETIME       NULL,
    CONSTRAINT [PK_InventoryRefno] PRIMARY KEY CLUSTERED ([ID] ASC)
);



