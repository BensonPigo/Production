CREATE TABLE [dbo].[CDCode_Content] (
    [ID]                   VARCHAR (6)    CONSTRAINT [DF_CDCode_Content_ID] DEFAULT ('') NOT NULL,
    [TopProductionType]    NVARCHAR (100) CONSTRAINT [DF_CDCode_Content_TopProductionType] DEFAULT ('') NULL,
    [TopFabricType]        NVARCHAR (100) CONSTRAINT [DF_CDCode_Content_TopFabricType] DEFAULT ('') NULL,
    [BottomProductionType] NVARCHAR (100) CONSTRAINT [DF_CDCode_Content_BottomProductionType] DEFAULT ('') NULL,
    [BottomFabricType]     NVARCHAR (100) CONSTRAINT [DF_CDCode_Content_BottomFabricType] DEFAULT ('') NULL,
    [InnerProductionType]  NVARCHAR (100) CONSTRAINT [DF_CDCode_Content_InnerProductionType] DEFAULT ('') NULL,
    [InnerFabricType]      NVARCHAR (100) CONSTRAINT [DF_CDCode_Content_InnerFabricType] DEFAULT ('') NULL,
    [OuterProductionType]  NVARCHAR (100) CONSTRAINT [DF_CDCode_Content_OuterProductionType] DEFAULT ('') NULL,
    [OuterFabricType]      NVARCHAR (100) CONSTRAINT [DF_CDCode_Content_OuterFabricType] DEFAULT ('') NULL,
    [EditName]             VARCHAR (10)   CONSTRAINT [DF_CDCode_Content_EditName] DEFAULT ('') NULL,
    [EditDate]             DATETIME       NULL,
    CONSTRAINT [PK_CDCode_Content] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'各部位別的成分', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CDCode_Content';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'產能代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CDCode_Content', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Top Production Type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CDCode_Content', @level2type = N'COLUMN', @level2name = N'TopProductionType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Top Fabric Type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CDCode_Content', @level2type = N'COLUMN', @level2name = N'TopFabricType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Bottom Production Type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CDCode_Content', @level2type = N'COLUMN', @level2name = N'BottomProductionType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Bottom Fabric Type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CDCode_Content', @level2type = N'COLUMN', @level2name = N'BottomFabricType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Inner Production Type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CDCode_Content', @level2type = N'COLUMN', @level2name = N'InnerProductionType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Inner Fabric Type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CDCode_Content', @level2type = N'COLUMN', @level2name = N'InnerFabricType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Outer Production Type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CDCode_Content', @level2type = N'COLUMN', @level2name = N'OuterProductionType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Outer Fabric Type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CDCode_Content', @level2type = N'COLUMN', @level2name = N'OuterFabricType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CDCode_Content', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CDCode_Content', @level2type = N'COLUMN', @level2name = N'EditDate';

