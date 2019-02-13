CREATE TABLE [dbo].[VNImportDeclaration_Detail_Detail] (
    [ID]         VARCHAR (13)    NOT NULL,
    [Refno]      VARCHAR (21)    NOT NULL,
    [FabricType] VARCHAR (20)    NOT NULL,
    [NLCode]     VARCHAR (5)     NOT NULL,
    [Qty]        NUMERIC (14, 3) NULL,
    [Remark]     NVARCHAR (60)   NULL,
    CONSTRAINT [PK_VNImportDeclaration_Detail_Detail] PRIMARY KEY CLUSTERED ([ID] ASC, [Refno] ASC, [FabricType] ASC, [NLCode] ASC)
);







