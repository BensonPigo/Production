CREATE TABLE [dbo].[Bundle_Detail] (
    [BundleNo]    VARCHAR (10)   CONSTRAINT [DF_Bundle_Detail_BundleNo] DEFAULT ('') NOT NULL,
    [Id]          VARCHAR (13)   CONSTRAINT [DF_Bundle_Detail_Id] DEFAULT ((0)) NOT NULL,
    [BundleGroup] NUMERIC (5)    CONSTRAINT [DF_Bundle_Detail_BundleGroup] DEFAULT ((0)) NULL,
    [Patterncode] VARCHAR (20)   CONSTRAINT [DF_Bundle_Detail_Patterncode] DEFAULT ('') NOT NULL,
    [PatternDesc] NVARCHAR (100) CONSTRAINT [DF_Bundle_Detail_PatternDesc] DEFAULT ('') NOT NULL,
    [SizeCode]    VARCHAR (8)    CONSTRAINT [DF_Bundle_Detail_SizeCode] DEFAULT ('') NULL,
    [Qty]         NUMERIC (5)    CONSTRAINT [DF_Bundle_Detail_Qty] DEFAULT ((0)) NULL,
    [Parts]       NUMERIC (5)    CONSTRAINT [DF_Bundle_Detail_Parts] DEFAULT ((0)) NULL,
    [Farmin]      NUMERIC (5)    CONSTRAINT [DF_Bundle_Detail_Farmin] DEFAULT ((0)) NULL,
    [FarmOut]     NUMERIC (5)    CONSTRAINT [DF_Bundle_Detail_FarmOut] DEFAULT ((0)) NULL,
    [PrintDate]   DATETIME       NULL,
    [IsPair]      BIT            NULL,
    [Location]    VARCHAR (1)    DEFAULT ('') NOT NULL,
    [RFUID] VARCHAR(20) NOT NULL CONSTRAINT [DF_Bundle_Detail_RFUID] DEFAULT (''), 
    [Tone]        VARCHAR (1)    CONSTRAINT [DF_Bundle_Detail_Tone] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_Bundle_Detail] PRIMARY KEY CLUSTERED ([BundleNo] ASC, [Id] ASC),
    CONSTRAINT [UK_BundleNo_Bundle_Detail] UNIQUE NONCLUSTERED ([BundleNo] ASC)
);
















GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Bundle Detail', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_Detail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'捆包號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_Detail', @level2type = N'COLUMN', @level2name = N'BundleNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_Detail', @level2type = N'COLUMN', @level2name = N'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Group', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_Detail', @level2type = N'COLUMN', @level2name = N'BundleGroup';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'版片編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_Detail', @level2type = N'COLUMN', @level2name = N'Patterncode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'版片敘述', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_Detail', @level2type = N'COLUMN', @level2name = N'PatternDesc';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'尺寸', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_Detail', @level2type = N'COLUMN', @level2name = N'SizeCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_Detail', @level2type = N'COLUMN', @level2name = N'Qty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Part 數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_Detail', @level2type = N'COLUMN', @level2name = N'Parts';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'外發收入數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_Detail', @level2type = N'COLUMN', @level2name = N'Farmin';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'外發發出數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_Detail', @level2type = N'COLUMN', @level2name = N'FarmOut';


GO
CREATE NONCLUSTERED INDEX [<Name of Missing Index, sysname,>]
    ON [dbo].[Bundle_Detail]([Id] ASC);

GO
CREATE NONCLUSTERED INDEX [IDX_Bundle_Detail_QtyBySetPerSubprocess]
    ON [dbo].[Bundle_Detail]([Patterncode],[SizeCode] ASC);
GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'for SNP RF Card UID, printer(CHP_1800)',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Bundle_Detail',
    @level2type = N'COLUMN',
    @level2name = N'RFUID'

GO
CREATE NONCLUSTERED INDEX [IDX_Bundle_Detail_PlanningR15]
    ON [dbo].[Bundle_Detail]([PatternDesc] ASC, [SizeCode] ASC);

