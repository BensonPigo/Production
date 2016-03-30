CREATE TABLE [dbo].[VNImportDeclaration] (
    [ID]                VARCHAR (13) NOT NULL,
    [CDate]             DATE         NULL,
    [BLNo]              VARCHAR (20) CONSTRAINT [DF_VNImportDeclaration_BLNo] DEFAULT ('') NULL,
    [WKNo]              VARCHAR (13) CONSTRAINT [DF_VNImportDeclaration_WKNo] DEFAULT ('') NULL,
    [VNContractID]      VARCHAR (15) CONSTRAINT [DF_VNImportDeclaration_ContractID] DEFAULT ('') NULL,
    [ShipModeID]        VARCHAR (10) CONSTRAINT [DF_VNImportDeclaration_ShipModeID] DEFAULT ('') NULL,
    [FromSite]          VARCHAR (2)  CONSTRAINT [DF_VNImportDeclaration_FromSite] DEFAULT ('') NULL,
    [IsSystemCalculate] BIT          CONSTRAINT [DF_VNImportDeclaration_IsSystemCalculate] DEFAULT ((0)) NULL,
    [DeclareNo]         VARCHAR (25) CONSTRAINT [DF_VNImportDeclaration_DeclareNo] DEFAULT ('') NULL,
    [Status]            VARCHAR (15) CONSTRAINT [DF_VNImportDeclaration_Status] DEFAULT ('') NULL,
    [IsFtyExport]       BIT          CONSTRAINT [DF_VNImportDeclaration_IsFtyExport] DEFAULT ((0)) NULL,
    [IsLocalPO]         BIT          CONSTRAINT [DF_VNImportDeclaration_IsLocalPO] DEFAULT ((0)) NULL,
    [AddName]           VARCHAR (10) CONSTRAINT [DF_VNImportDeclaration_AddName] DEFAULT ('') NULL,
    [AddDate]           DATETIME     NULL,
    [EditName]          VARCHAR (10) CONSTRAINT [DF_VNImportDeclaration_EditName] DEFAULT ('') NULL,
    [EditDate]          DATETIME     NULL,
    CONSTRAINT [PK_VNImportDeclaration] PRIMARY KEY CLUSTERED ([ID] ASC)
);

