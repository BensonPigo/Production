CREATE TABLE [dbo].[Bundle_Detail_Allpart_History] (
    [ID]          VARCHAR (13)   CONSTRAINT [DF_Bundle_Detail_Allpart_History_ID] DEFAULT ((0)) NOT NULL,
    [Patterncode] VARCHAR (20)   CONSTRAINT [DF_Bundle_Detail_Allpart_History_Patterncode] DEFAULT ('') NOT NULL,
    [PatternDesc] NVARCHAR (100) CONSTRAINT [DF_Bundle_Detail_Allpart_History_PatternDesc] DEFAULT ('') NULL,
    [parts]       NUMERIC (5)    CONSTRAINT [DF_Bundle_Detail_Allpart_History_parts] DEFAULT ((0)) NOT NULL,
    [Ukey]        BIGINT         IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [IsPair]      BIT            NULL,
    [Location]    VARCHAR (1)    DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_Bundle_Detail_Allpart_History] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_Detail_Allpart_History', @level2type = N'COLUMN', @level2name = N'parts';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'版片敘述', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_Detail_Allpart_History', @level2type = N'COLUMN', @level2name = N'PatternDesc';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'版片編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_Detail_Allpart_History', @level2type = N'COLUMN', @level2name = N'Patterncode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Bundle 單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_Detail_Allpart_History', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Bundle Detail Apll Part History', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_Detail_Allpart_History';

