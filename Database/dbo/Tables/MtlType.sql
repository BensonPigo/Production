CREATE TABLE [dbo].[MtlType] (
    [ID]                   VARCHAR (20)   CONSTRAINT [DF_MtlType_ID] DEFAULT ('') NOT NULL,
    [FullName]             NVARCHAR (100) CONSTRAINT [DF_MtlType_FullName] DEFAULT ('') NULL,
    [Type]                 VARCHAR (1)    CONSTRAINT [DF_MtlType_Type] DEFAULT ('') NULL,
    [Junk]                 BIT            CONSTRAINT [DF_MtlType_Junk] DEFAULT ((0)) NULL,
    [IrregularCost]        BIT            CONSTRAINT [DF_MtlType_IrregularCost] DEFAULT ((0)) NULL,
    [CheckZipper]          BIT            CONSTRAINT [DF_MtlType_CheckZipper] DEFAULT ((0)) NULL,
    [ProductionType]       VARCHAR (20)   CONSTRAINT [DF_MtlType_ProductionType] DEFAULT ('') NULL,
    [OutputUnit]           VARCHAR (1)    CONSTRAINT [DF_MtlType_OutputUnit] DEFAULT ('') NULL,
    [IsExtensionUnit]      BIT            CONSTRAINT [DF_MtlType_IsExtensionUnit] DEFAULT ((0)) NOT NULL,
    [IssueType]            VARCHAR (20)   CONSTRAINT [DF_MtlType_IssueType] DEFAULT ('') NULL,
    [IsTrimCardOther]      BIT            NULL,
    [AddName]              VARCHAR (10)   CONSTRAINT [DF_MtlType_AddName] DEFAULT ('') NULL,
    [AddDate]              DATETIME       NULL,
    [EditName]             VARCHAR (10)   CONSTRAINT [DF_MtlType_EditName] DEFAULT ('') NULL,
    [EditDate]             DATETIME       NULL,
    [IsThread]             BIT            NULL,
    [LossQtyCalculateType] VARCHAR (1)    NULL,
    CONSTRAINT [PK_MtlType] PRIMARY KEY CLUSTERED ([ID] ASC)
);








GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'主副料類別設定基本檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MtlType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MtlType', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'全名', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MtlType', @level2type = N'COLUMN', @level2name = N'FullName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'大分類', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MtlType', @level2type = N'COLUMN', @level2name = N'Type';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'取消', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MtlType', @level2type = N'COLUMN', @level2name = N'Junk';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否為「異常成本分類」的選項', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MtlType', @level2type = N'COLUMN', @level2name = N'IrregularCost';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'在副料基本檔中是否可勾選Chk_Zipper的選項', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MtlType', @level2type = N'COLUMN', @level2name = N'CheckZipper';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'生產分類(用於Pull forward)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MtlType', @level2type = N'COLUMN', @level2name = N'ProductionType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'設定1: Usage Unit ; 2: Purchase Unit', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MtlType', @level2type = N'COLUMN', @level2name = N'OutputUnit';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'屬於發料需做特定轉換的單位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MtlType', @level2type = N'COLUMN', @level2name = N'IsExtensionUnit';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'領料分類', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MtlType', @level2type = N'COLUMN', @level2name = N'IssueType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MtlType', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MtlType', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MtlType', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MtlType', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�O�_���u', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MtlType', @level2type = N'COLUMN', @level2name = N'IsThread';

