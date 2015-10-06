CREATE TABLE [dbo].[Style_WeightData] (
    [StyleUkey] BIGINT         CONSTRAINT [DF_Style_WeightData_StyleUkey] DEFAULT ((0)) NOT NULL,
    [Article]   VARCHAR (8)    CONSTRAINT [DF_Style_WeightData_Article] DEFAULT ('') NOT NULL,
    [SizeCode]  VARCHAR (8)    CONSTRAINT [DF_Style_WeightData_SizeCode] DEFAULT ('') NOT NULL,
    [NW]        NUMERIC (5, 3) CONSTRAINT [DF_Style_WeightData_NW] DEFAULT ((0)) NULL,
    [NNW]       NUMERIC (5, 3) CONSTRAINT [DF_Style_WeightData_NNW] DEFAULT ((0)) NULL,
    [AddName]   VARCHAR (10)   CONSTRAINT [DF_Style_WeightData_AddName] DEFAULT ('') NULL,
    [AddDate]   DATETIME       NULL,
    [EditName]  VARCHAR (10)   CONSTRAINT [DF_Style_WeightData_EditName] DEFAULT ('') NULL,
    [EditDate]  DATETIME       NULL,
    CONSTRAINT [PK_Style_WeightData] PRIMARY KEY CLUSTERED ([StyleUkey] ASC, [Article] ASC, [SizeCode] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'款式資料基本檔-衣服重量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_WeightData';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'UKey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_WeightData', @level2type = N'COLUMN', @level2name = N'StyleUkey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'色組', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_WeightData', @level2type = N'COLUMN', @level2name = N'Article';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'尺寸', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_WeightData', @level2type = N'COLUMN', @level2name = N'SizeCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'淨重', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_WeightData', @level2type = N'COLUMN', @level2name = N'NW';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'淨淨重', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_WeightData', @level2type = N'COLUMN', @level2name = N'NNW';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_WeightData', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_WeightData', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_WeightData', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_WeightData', @level2type = N'COLUMN', @level2name = N'EditDate';

