CREATE TABLE [dbo].[VNImportDeclaration_Detail] (
    [ID]     VARCHAR (13)    NOT NULL,
    [HSCode] VARCHAR (11)    CONSTRAINT [DF_VNImportDeclaration_Detail_HSCode] DEFAULT ('') NULL,
    [NLCode] VARCHAR (5)     NOT NULL,
    [Qty]    NUMERIC (14, 3) CONSTRAINT [DF_VNImportDeclaration_Detail_Qty] DEFAULT ((0)) NULL,
    [UnitID] VARCHAR (8)     CONSTRAINT [DF_VNImportDeclaration_Detail_UnitID] DEFAULT ('') NULL,
    [Remark] NVARCHAR (60)   CONSTRAINT [DF_VNImportDeclaration_Detail_Remark] DEFAULT ('') NULL,
    [Price]  NUMERIC (14, 4) CONSTRAINT [DF_VNImportDeclaration_Detail_Price] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_VNImportDeclaration_Detail] PRIMARY KEY CLUSTERED ([ID] ASC, [NLCode] ASC)
);

