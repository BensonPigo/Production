CREATE TABLE [dbo].[InventoryRefno] (
    [ID]              BIGINT          NOT NULL DEFAULT ((0)),
    [Refno]           VARCHAR(20)       NOT NULL DEFAULT (''),
    [Width]           NUMERIC (3, 1)  NULL DEFAULT ((0)),
    [ColorID]         VARCHAR(70)       NULL DEFAULT (''),
    [SizeSpec]        VARCHAR(8)        NULL DEFAULT (''),
    [SizeUnit]        VARCHAR(8)        NULL DEFAULT (''),
    [BomArticle]      VARCHAR(8)        NULL DEFAULT (''),
    [BomBuymonth]     VARCHAR(10)       NULL DEFAULT (''),
    [BomCountry]      VARCHAR(2)        NULL DEFAULT (''),
    [BomCustCD]       VARCHAR(20)       NULL DEFAULT (''),
    [BomCustPONo]     VARCHAR(30)       NULL DEFAULT (''),
    [BomFactory]      VARCHAR(8)       NULL DEFAULT (''),
    [BomStyle]        VARCHAR(15)       NULL DEFAULT (''),
    [BomZipperInsert] VARCHAR(5)        NULL DEFAULT (''),
    [ProdID_Old]      VARCHAR(10)       NULL DEFAULT (''),
    [Special_Old]     NVARCHAR (60)   NULL DEFAULT (''),
    [Spec_Old]        NVARCHAR (1000) NULL DEFAULT (''),
    [AddName]         VARCHAR(10)       NULL DEFAULT (''),
    [AddDate]         DATETIME        NULL, 
    CONSTRAINT [PK_InventoryRefno] PRIMARY KEY ([ID])
);

