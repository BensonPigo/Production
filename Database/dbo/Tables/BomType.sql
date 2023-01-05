CREATE TABLE [dbo].[BomType] (
    [ID]                VARCHAR (20) CONSTRAINT [DF_BomType_ID] DEFAULT ('') NOT NULL,
    [Name]              VARCHAR (50) CONSTRAINT [DF_BomType_Name] DEFAULT ('') NULL,
    [Seq]               INT          CONSTRAINT [DF_BomType_Seq] DEFAULT ((0)) NULL,
    [IsInformationSpec] BIT          CONSTRAINT [DF_BomType_IsInformationSpec] DEFAULT ((0)) NULL,
    [Junk]              BIT          CONSTRAINT [DF_BomType_Junk] DEFAULT ((0)) NULL,
    [AddName]           VARCHAR (10) CONSTRAINT [DF_BomType_AddName] DEFAULT ('') NOT NULL,
    [AddDate]           DATETIME     NULL,
    [EditName]          VARCHAR (10) CONSTRAINT [DF_BomType_EditName] DEFAULT ('') NULL,
    [EditDate]          DATETIME     NULL,
    CONSTRAINT [PK_BomType] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'修改日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BomType', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BomType', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BomType', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BomType', @level2type = N'COLUMN', @level2name = N'AddName';

