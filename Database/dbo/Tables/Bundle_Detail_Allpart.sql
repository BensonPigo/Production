CREATE TABLE [dbo].[Bundle_Detail_Allpart] (
    [ID]          VARCHAR (13)  CONSTRAINT [DF_Bundle_Detail_Allpart_ID] DEFAULT ((0)) NOT NULL,
    [BundleNo]    VARCHAR (10)  CONSTRAINT [DF_Bundle_Detail_Allpart_BundleNo] DEFAULT ('') NOT NULL,
    [Patterncode] VARCHAR (20)  CONSTRAINT [DF_Bundle_Detail_Allpart_Patterncode] DEFAULT ('') NOT NULL,
    [PatternDesc] NVARCHAR (40) CONSTRAINT [DF_Bundle_Detail_Allpart_PatternDesc] DEFAULT ('') NOT NULL,
    [parts]       NUMERIC (5)   CONSTRAINT [DF_Bundle_Detail_Allpart_parts] DEFAULT ((0)) NOT NULL,
    [Ukey]        BIGINT        IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_Bundle_Detail_Allpart_1] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Bundle Detail Apll Part', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_Detail_Allpart';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Bundle 單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_Detail_Allpart', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'捆包號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_Detail_Allpart', @level2type = N'COLUMN', @level2name = N'BundleNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'版片編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_Detail_Allpart', @level2type = N'COLUMN', @level2name = N'Patterncode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'版片敘述', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_Detail_Allpart', @level2type = N'COLUMN', @level2name = N'PatternDesc';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_Detail_Allpart', @level2type = N'COLUMN', @level2name = N'parts';


GO


