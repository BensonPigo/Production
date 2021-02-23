CREATE TABLE [dbo].[Bundle_Detail_CombineSubprocess] (
    [Ukey]        BIGINT         IDENTITY (1, 1) NOT NULL,
    [BundleNo]    VARCHAR (10)   CONSTRAINT [DF_Bundle_Detail_CombineSubprocess_BundleNo] DEFAULT ('') NOT NULL,
    [ID]          VARCHAR (13)   CONSTRAINT [DF_Bundle_Detail_CombineSubprocess_ID] DEFAULT ('') NOT NULL,
    [Patterncode] VARCHAR (20)   CONSTRAINT [DF_Bundle_Detail_CombineSubprocess_Patterncode] DEFAULT ('') NOT NULL,
    [PatternDesc] NVARCHAR (100) NULL,
    [Parts]       NUMERIC (5)    CONSTRAINT [DF_Bundle_Detail_CombineSubprocess_Parts] DEFAULT ((0)) NOT NULL,
    [Location]    VARCHAR (1)    CONSTRAINT [DF_Bundle_Detail_CombineSubprocess_Location] DEFAULT ('') NOT NULL,
    [IsPair]      BIT            CONSTRAINT [DF_Bundle_Detail_CombineSubprocess_IsPair] DEFAULT ((0)) NOT NULL,
    [IsMain]      BIT            CONSTRAINT [DF_Bundle_Detail_CombineSubprocess_IsMain] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Bundle_Detail_CombineSubprocess] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否為此combine subprocess的主main', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_Detail_CombineSubprocess', @level2type = N'COLUMN', @level2name = N'IsMain';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否成對 ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_Detail_CombineSubprocess', @level2type = N'COLUMN', @level2name = N'IsPair';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'B/T/I/O位置', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_Detail_CombineSubprocess', @level2type = N'COLUMN', @level2name = N'Location';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'版片數量 ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_Detail_CombineSubprocess', @level2type = N'COLUMN', @level2name = N'Parts';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'版片敘述 ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_Detail_CombineSubprocess', @level2type = N'COLUMN', @level2name = N'PatternDesc';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'版片編號 ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_Detail_CombineSubprocess', @level2type = N'COLUMN', @level2name = N'Patterncode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'BundleID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_Detail_CombineSubprocess', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'捆包號 ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_Detail_CombineSubprocess', @level2type = N'COLUMN', @level2name = N'BundleNo';

