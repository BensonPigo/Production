CREATE TABLE [dbo].[Keyword_BomType] (
    [ID]               VARCHAR (30)  CONSTRAINT [DF_Keyword_BomType_ID] DEFAULT ('') NOT NULL,
    [BomType]          VARCHAR (20)  CONSTRAINT [DF_Table_1_BomTypeID] DEFAULT ('') NOT NULL,
    [BomTypeFieldName] VARCHAR (100) CONSTRAINT [DF_Table_1_BOMTypeFieldName] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_Keyword_BomType] PRIMARY KEY CLUSTERED ([ID] ASC, [BomType] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'顯示名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Keyword_BomType', @level2type = N'COLUMN', @level2name = N'ID';

