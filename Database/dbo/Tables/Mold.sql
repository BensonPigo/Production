CREATE TABLE [dbo].[Mold] (
    [ID]       VARCHAR (20)   CONSTRAINT [DF_Mold_ID] DEFAULT ('') NOT NULL,
    [Type]     VARCHAR (1)    CONSTRAINT [DF_Mold_Type] DEFAULT ('') NULL,
    [DescCH]   NVARCHAR (100) CONSTRAINT [DF_Mold_DescCH] DEFAULT ('') NULL,
    [DescEN]   NVARCHAR (100) CONSTRAINT [DF_Mold_DescEN] DEFAULT ('') NULL,
    [Junk]     BIT            CONSTRAINT [DF_Mold_Junk] DEFAULT ((0)) NULL,
    [AddName]  VARCHAR (10)   CONSTRAINT [DF_Mold_AddName] DEFAULT ('') NULL,
    [AddDate]  DATETIME       NULL,
    [EditName] VARCHAR (10)   CONSTRAINT [DF_Mold_EditName] DEFAULT ('') NULL,
    [EditDate] DATETIME       NULL,
    [IsAttachment] BIT CONSTRAINT [DF_Mold_IsAttachment] NOT NULL DEFAULT ((0)), 
    [IsTemplate] BIT CONSTRAINT [DF_Mold_IsTemplate] NOT NULL DEFAULT ((0)), 
    CONSTRAINT [PK_Mold] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'模具基本檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Mold';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Mold', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Mold', @level2type = N'COLUMN', @level2name = N'Type';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Description (Chinese)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Mold', @level2type = N'COLUMN', @level2name = N'DescCH';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Description (English)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Mold', @level2type = N'COLUMN', @level2name = N'DescEN';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Junk', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Mold', @level2type = N'COLUMN', @level2name = N'Junk';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Creator', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Mold', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Create time', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Mold', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Editor', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Mold', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Edit time', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Mold', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'是否為Attachment',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Mold',
    @level2type = N'COLUMN',
    @level2name = N'IsAttachment'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'是否為Template',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Mold',
    @level2type = N'COLUMN',
    @level2name = N'IsTemplate'