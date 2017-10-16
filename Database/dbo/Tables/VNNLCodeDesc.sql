CREATE TABLE [dbo].[VNNLCodeDesc] (
    [NLCode]   VARCHAR (7)    CONSTRAINT [DF_VNNLCodeDesc_NLCode] DEFAULT ('') NOT NULL,
    [DescEN]   VARCHAR (100)  CONSTRAINT [DF_VNNLCodeDesc_DescEN] DEFAULT ('') NULL,
    [DescVI]   NVARCHAR (100) CONSTRAINT [DF_VNNLCodeDesc_DescVI] DEFAULT ('') NULL,
    [UnitVI]   NVARCHAR (50)  CONSTRAINT [DF_VNNLCodeDesc_UnitVI] DEFAULT ('') NULL,
    [AddName]  VARCHAR (10)   CONSTRAINT [DF_VNNLCodeDesc_AddName] DEFAULT ('') NULL,
    [AddDate]  DATETIME       NULL,
    [EditName] VARCHAR (10)   CONSTRAINT [DF_VNNLCodeDesc_EditName] DEFAULT ('') NULL,
    [EditDate] DATETIME       NULL,
    [WasteLower] NUMERIC(5, 3) NOT NULL, 
    [WasteUpper] NUMERIC(5, 3) NOT NULL, 
    CONSTRAINT [PK_VNNLCodeDesc] PRIMARY KEY CLUSTERED ([NLCode] ASC)
);






GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'VNNLCodeDesc', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'VNNLCodeDesc', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'VNNLCodeDesc', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'VNNLCodeDesc', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'越文單位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'VNNLCodeDesc', @level2type = N'COLUMN', @level2name = N'UnitVI';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'越文說明', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'VNNLCodeDesc', @level2type = N'COLUMN', @level2name = N'DescVI';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'英文說明', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'VNNLCodeDesc', @level2type = N'COLUMN', @level2name = N'DescEN';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'NL Code', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'VNNLCodeDesc', @level2type = N'COLUMN', @level2name = N'NLCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'NL Code說明翻譯檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'VNNLCodeDesc';

