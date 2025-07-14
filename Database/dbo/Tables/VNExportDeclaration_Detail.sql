CREATE TABLE [dbo].[VNExportDeclaration_Detail] (
    [ID]        VARCHAR (13) CONSTRAINT [DF_VNExportDeclaration_Detail_ID] DEFAULT ('') NOT NULL,
    [OrderID]   VARCHAR (13) CONSTRAINT [DF_VNExportDeclaration_Detail_OrderID] DEFAULT ('') NOT NULL,
    [StyleID]   VARCHAR (15) CONSTRAINT [DF_VNExportDeclaration_Detail_StyleID] DEFAULT ('') NULL,
    [SeasonID]  VARCHAR (10) CONSTRAINT [DF_VNExportDeclaration_Detail_SeasonID] DEFAULT ('') NULL,
    [BrandID]   VARCHAR (8)  CONSTRAINT [DF_VNExportDeclaration_Detail_BrandID] DEFAULT ('') NULL,
    [Category]  VARCHAR (1)  CONSTRAINT [DF_VNExportDeclaration_Detail_Category] DEFAULT ('') NULL,
    [CustomSP]  VARCHAR (12)  CONSTRAINT [DF_VNExportDeclaration_Detail_CustomSP] DEFAULT ('') NULL,
    [Article]   VARCHAR (8)  CONSTRAINT [DF_VNExportDeclaration_Detail_Article] DEFAULT ('') NOT NULL,
    [SizeCode]  VARCHAR (8)  CONSTRAINT [DF_VNExportDeclaration_Detail_SizeCode] DEFAULT ('') NOT NULL,
    [ExportQty] INT          CONSTRAINT [DF_VNExportDeclaration_Detail_OrderQty] DEFAULT ((0)) NULL,
    [StyleUKey] BIGINT       CONSTRAINT [DF_VNExportDeclaration_Detail_StyleUKey] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_VNExportDeclaration_Detail] PRIMARY KEY CLUSTERED ([ID] ASC, [OrderID] ASC, [Article] ASC, [SizeCode] ASC)
);

