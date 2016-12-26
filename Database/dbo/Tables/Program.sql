CREATE TABLE [dbo].[Program] (
    [ID]       VARCHAR (12)   CONSTRAINT [DF_Program_ID] DEFAULT ('') NOT NULL,
    [BrandID]  VARCHAR (8)    CONSTRAINT [DF_Program_BrandID] DEFAULT ('') NOT NULL,
    [RateCost] NUMERIC (3, 1) CONSTRAINT [DF_Program_RateCost] DEFAULT ((0)) NULL,
    [AddName]  VARCHAR (10)   CONSTRAINT [DF_Program_AddName] DEFAULT ('') NULL,
    [AddDate]  DATETIME       NULL,
    [EditName] VARCHAR (10)   CONSTRAINT [DF_Program_EditName] DEFAULT ('') NULL,
    [EditDate] DATETIME       NULL,
    [MiAdidas] BIT            NULL,
    CONSTRAINT [PK_Program] PRIMARY KEY CLUSTERED ([ID] ASC, [BrandID] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Program', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Program';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Program', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Program', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Brand', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Program', @level2type = N'COLUMN', @level2name = N'BrandID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Rate Cost', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Program', @level2type = N'COLUMN', @level2name = N'RateCost';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'AddName', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Program', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'AddDate', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Program', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'EditName', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Program', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'EditDate', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Program', @level2type = N'COLUMN', @level2name = N'EditDate';

