CREATE TABLE [dbo].[FtyStdRate_PRT] (
    [Region]    VARCHAR (4)     CONSTRAINT [DF_FtyStdRate_PRT_Region] DEFAULT ('') NOT NULL,
    [SeasonID]  VARCHAR (10)    CONSTRAINT [DF_FtyStdRate_PRT_SeasonID] DEFAULT ('') NOT NULL,
    [InkType]   VARCHAR (50)    CONSTRAINT [DF_FtyStdRate_PRT_InkType] DEFAULT ('') NOT NULL,
    [Colors]    VARCHAR (100)   CONSTRAINT [DF_FtyStdRate_PRT_Colors] DEFAULT ('') NOT NULL,
    [Area]      DECIMAL (12, 2) CONSTRAINT [DF_FtyStdRate_PRT_Area] DEFAULT ((0)) NOT NULL,
    [Length]    DECIMAL (5, 1)  CONSTRAINT [DF_FtyStdRate_PRT_Length] DEFAULT ((0)) NOT NULL,
    [Width]     DECIMAL (5, 1)  CONSTRAINT [DF_FtyStdRate_PRT_Width] DEFAULT ((0)) NOT NULL,
    [Surcharge] DECIMAL (5, 2)  CONSTRAINT [DF_FtyStdRate_PRT_Surcharge] DEFAULT ((0)) NOT NULL,
    [Price]     NUMERIC (16, 4) CONSTRAINT [DF_FtyStdRate_PRT_Price] DEFAULT ((0)) NOT NULL,
    [Ratio]     DECIMAL (5, 2)  CONSTRAINT [DF_FtyStdRate_PRT_Ratio] DEFAULT ((0)) NOT NULL,
    [SEQ]       VARCHAR (4)     CONSTRAINT [DF_FtyStdRate_PRT_SEQ] DEFAULT ('') NOT NULL,
    [AddName]   VARCHAR (10)    CONSTRAINT [DF_FtyStdRate_PRT_AddName] DEFAULT ('') NOT NULL,
    [AddDate]   DATETIME        NULL,
    [EditName]  VARCHAR (10)    CONSTRAINT [DF_FtyStdRate_PRT_EditName] DEFAULT ('') NOT NULL,
    [EditDate]  DATETIME        NULL,
    CONSTRAINT [PK_FtyStdRate_PRT] PRIMARY KEY CLUSTERED ([Region] ASC, [SeasonID] ASC, [InkType] ASC, [Colors] ASC, [Area] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyStdRate_PRT', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyStdRate_PRT', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyStdRate_PRT', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyStdRate_PRT', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'排序', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyStdRate_PRT', @level2type = N'COLUMN', @level2name = N'SEQ';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'保留利潤(%)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyStdRate_PRT', @level2type = N'COLUMN', @level2name = N'Ratio';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'價格', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyStdRate_PRT', @level2type = N'COLUMN', @level2name = N'Price';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'額外費用(%)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyStdRate_PRT', @level2type = N'COLUMN', @level2name = N'Surcharge';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'邊寬', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyStdRate_PRT', @level2type = N'COLUMN', @level2name = N'Width';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'邊長', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyStdRate_PRT', @level2type = N'COLUMN', @level2name = N'Length';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'面積', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyStdRate_PRT', @level2type = N'COLUMN', @level2name = N'Area';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Colors', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyStdRate_PRT', @level2type = N'COLUMN', @level2name = N'Colors';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'印花種類', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyStdRate_PRT', @level2type = N'COLUMN', @level2name = N'InkType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'季度', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyStdRate_PRT', @level2type = N'COLUMN', @level2name = N'SeasonID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'地區', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyStdRate_PRT', @level2type = N'COLUMN', @level2name = N'Region';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'COP - 工廠標準成本(Printing)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyStdRate_PRT';

