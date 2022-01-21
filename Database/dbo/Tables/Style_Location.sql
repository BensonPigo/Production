CREATE TABLE [dbo].[Style_Location] (
    [StyleUkey] BIGINT         CONSTRAINT [DF_Style_Location_StyleUkey] DEFAULT ((0)) NOT NULL,
    [Location]  VARCHAR (1)    CONSTRAINT [DF_Style_Location_Location] DEFAULT ('') NOT NULL,
    [Rate]      NUMERIC (5, 2) CONSTRAINT [DF_Style_Location_Rate] DEFAULT ((0)) NULL,
    [AddName]   VARCHAR (10)   CONSTRAINT [DF_Style_Location_AddName] DEFAULT ('') NULL,
    [AddDate]   DATETIME       NULL,
    [EditName]  VARCHAR (10)   CONSTRAINT [DF_Style_Location_EditName] DEFAULT ('') NULL,
    [EditDate]  DATETIME       NULL,
    [ApparelType] VARCHAR(5) NULL, 
    [FabricType] VARCHAR(5) NULL, 
    CONSTRAINT [PK_Style_Location] PRIMARY KEY CLUSTERED ([StyleUkey] ASC, [Location] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Location', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_Location';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'款式的唯一值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_Location', @level2type = N'COLUMN', @level2name = N'StyleUkey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'位置', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_Location', @level2type = N'COLUMN', @level2name = N'Location';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'比例', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_Location', @level2type = N'COLUMN', @level2name = N'Rate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_Location', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_Location', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_Location', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_Location', @level2type = N'COLUMN', @level2name = N'EditDate';

