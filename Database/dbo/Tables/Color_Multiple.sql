CREATE TABLE [dbo].[Color_Multiple] (
    [ID]        VARCHAR (6)  CONSTRAINT [DF_Color_Multiple_ID] DEFAULT ('') NULL,
    [ColorUkey] BIGINT       CONSTRAINT [DF_Color_Multiple_ColorUkey] DEFAULT ((0)) NOT NULL,
    [BrandID]   VARCHAR (8)  CONSTRAINT [DF_Color_Multiple_BrandID] DEFAULT ('') NULL,
    [Seqno]     VARCHAR (2)  CONSTRAINT [DF_Color_Multiple_Seqno] DEFAULT ('') NOT NULL,
    [ColorID]   VARCHAR (6)  CONSTRAINT [DF_Color_Multiple_ColorID] DEFAULT ('') NULL,
    [AddName]   VARCHAR (10) CONSTRAINT [DF_Color_Multiple_AddName] DEFAULT ('') NULL,
    [AddDate]   DATETIME     NULL,
    [EditName]  VARCHAR (10) CONSTRAINT [DF_Color_Multiple_EditName] DEFAULT ('') NULL,
    [EditDate]  DATETIME     NULL,
    CONSTRAINT [PK_Color_Multiple] PRIMARY KEY CLUSTERED ([ColorUkey] ASC, [Seqno] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'顏色基本檔-複合色清單', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Color_Multiple';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'顏色編碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Color_Multiple', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'顏色的唯一值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Color_Multiple', @level2type = N'COLUMN', @level2name = N'ColorUkey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'品牌', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Color_Multiple', @level2type = N'COLUMN', @level2name = N'BrandID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'順序', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Color_Multiple', @level2type = N'COLUMN', @level2name = N'Seqno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'顏色編碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Color_Multiple', @level2type = N'COLUMN', @level2name = N'ColorID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Color_Multiple', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Color_Multiple', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Color_Multiple', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Color_Multiple', @level2type = N'COLUMN', @level2name = N'EditDate';

