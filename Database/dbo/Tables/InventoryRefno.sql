CREATE TABLE [dbo].[InventoryRefno] (
    [ID]              BIGINT          NOT NULL,
    [Refno]           CHAR (20)       NOT NULL,
    [Width]           NUMERIC (3, 1)  NULL,
    [ColorID]         CHAR (70)       NULL,
    [SizeSpec]        CHAR (8)        NULL,
    [SizeUnit]        CHAR (8)        NULL,
    [BomArticle]      CHAR (8)        NULL,
    [BomBuymonth]     CHAR (10)       NULL,
    [BomCountry]      CHAR (2)        NULL,
    [BomCustCD]       CHAR (20)       NULL,
    [BomCustPONo]     CHAR (30)       NULL,
    [BomFactory]      CHAR (10)       NULL,
    [BomStyle]        CHAR (15)       NULL,
    [BomZipperInsert] CHAR (5)        NULL,
    [ProdID_Old]      CHAR (10)       NULL,
    [Special_Old]     NVARCHAR (60)   NULL,
    [Spec_Old]        NVARCHAR (1000) NULL,
    [AddName]         CHAR (10)       NULL,
    [AddDate]         DATETIME        NULL
);

