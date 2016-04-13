CREATE TABLE [dbo].[VNExportDeclaration] (
    [ID]             VARCHAR (13) NOT NULL,
    [CDate]          DATE         NULL,
    [VNContractID]   VARCHAR (15) CONSTRAINT [DF_VNExportDeclaration_VNContractID] DEFAULT ('') NULL,
    [InvNo]          VARCHAR (25) CONSTRAINT [DF_VNExportDeclaration_InvNo] DEFAULT ('') NULL,
    [VNExportPortID] VARCHAR (10) CONSTRAINT [DF_VNExportDeclaration_VNExportPortID] DEFAULT ('') NULL,
    [DeclareNo]      VARCHAR (25) CONSTRAINT [DF_VNExportDeclaration_DeclareNo] DEFAULT ('') NULL,
    [Status]         VARCHAR (15) CONSTRAINT [DF_VNExportDeclaration_Status] DEFAULT ('') NULL,
    [DataFrom]       VARCHAR (11) CONSTRAINT [DF_VNExportDeclaration_DataFrom] DEFAULT ('') NULL,
    [AddName]        VARCHAR (10) CONSTRAINT [DF_VNExportDeclaration_AddName] DEFAULT ('') NULL,
    [AddDate]        DATETIME     NULL,
    [EditName]       VARCHAR (10) CONSTRAINT [DF_VNExportDeclaration_EditName] DEFAULT ('') NULL,
    [EditDate]       DATETIME     NULL,
    CONSTRAINT [PK_VNExportDeclaration] PRIMARY KEY CLUSTERED ([ID] ASC)
);

