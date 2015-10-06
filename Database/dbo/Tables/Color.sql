CREATE TABLE [dbo].[Color] (
    [BrandId]     VARCHAR (8)   CONSTRAINT [DF_Color_BrandId] DEFAULT ('') NOT NULL,
    [ID]          VARCHAR (6)   CONSTRAINT [DF_Color_ID] DEFAULT ('') NOT NULL,
    [Ukey]        BIGINT        CONSTRAINT [DF_Color_Ukey] DEFAULT ((0)) NULL,
    [Name]        NVARCHAR (90) CONSTRAINT [DF_Color_Name] DEFAULT ('') NULL,
    [Varicolored] TINYINT       CONSTRAINT [DF_Color_Varicolored] DEFAULT ((0)) NULL,
    [JUNK]        BIT           CONSTRAINT [DF_Color_JUNK] DEFAULT ((0)) NULL,
    [VIVID]       BIT           CONSTRAINT [DF_Color_VIVID] DEFAULT ((0)) NULL,
    [AddName]     VARCHAR (10)  CONSTRAINT [DF_Color_AddName] DEFAULT ('') NULL,
    [AddDate]     DATETIME      NULL,
    [EditName]    VARCHAR (10)  CONSTRAINT [DF_Color_EditName] DEFAULT ('') NULL,
    [EditDate]    DATETIME      NULL,
    CONSTRAINT [PK_Color] PRIMARY KEY CLUSTERED ([BrandId] ASC, [ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Color', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Color';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'品牌代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Color', @level2type = N'COLUMN', @level2name = N'BrandId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'顏色代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Color', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'唯一值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Color', @level2type = N'COLUMN', @level2name = N'Ukey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'顏色名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Color', @level2type = N'COLUMN', @level2name = N'Name';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'顏色組合數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Color', @level2type = N'COLUMN', @level2name = N'Varicolored';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'取消', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Color', @level2type = N'COLUMN', @level2name = N'JUNK';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'螢光色註記', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Color', @level2type = N'COLUMN', @level2name = N'VIVID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Color', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Color', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Color', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Color', @level2type = N'COLUMN', @level2name = N'EditDate';

