CREATE TABLE [dbo].[FtyStyleInnovationComebineSubprocess] (
    [Ukey]        BIGINT         IDENTITY (1, 1) NOT NULL,
    [MDivisionID] VARCHAR (8)    CONSTRAINT [DF_FtyStyleInnovationComebineSubprocess_MDivisionID] DEFAULT ('') NOT NULL,
    [StyleUkey]   BIGINT         CONSTRAINT [DF_FtyStyleInnovationComebineSubprocess_StyleUkey] DEFAULT ((0)) NOT NULL,
    [FabricCombo] VARCHAR (2)    CONSTRAINT [DF_FtyStyleInnovationComebineSubprocess_FabricCombo] DEFAULT ('') NOT NULL,
    [Article]     VARCHAR (8)    CONSTRAINT [DF_FtyStyleInnovationComebineSubprocess_Article] DEFAULT ('') NOT NULL,
    [Patterncode] VARCHAR (20)   CONSTRAINT [DF_FtyStyleInnovationComebineSubprocess_Patterncode] DEFAULT ('') NULL,
    [PatternDesc] NVARCHAR (100) CONSTRAINT [DF_FtyStyleInnovationComebineSubprocess_PatternDesc] DEFAULT ('') NULL,
    [Location]    VARCHAR (1)    CONSTRAINT [DF_FtyStyleInnovationComebineSubprocess_Location] DEFAULT ('') NULL,
    [Parts]       NUMERIC (5)    CONSTRAINT [DF_FtyStyleInnovationComebineSubprocess_Parts] DEFAULT ((0)) NULL,
    [IsPair]      BIT            CONSTRAINT [DF_FtyStyleInnovationComebineSubprocess_IsPair] DEFAULT ((0)) NOT NULL
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否為一對', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyStyleInnovationComebineSubprocess', @level2type = N'COLUMN', @level2name = N'IsPair';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Part數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyStyleInnovationComebineSubprocess', @level2type = N'COLUMN', @level2name = N'Parts';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'部位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyStyleInnovationComebineSubprocess', @level2type = N'COLUMN', @level2name = N'Location';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'版片敘述', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyStyleInnovationComebineSubprocess', @level2type = N'COLUMN', @level2name = N'PatternDesc';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'版片名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyStyleInnovationComebineSubprocess', @level2type = N'COLUMN', @level2name = N'Patterncode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'顏色組', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyStyleInnovationComebineSubprocess', @level2type = N'COLUMN', @level2name = N'Article';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'布種部位組', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyStyleInnovationComebineSubprocess', @level2type = N'COLUMN', @level2name = N'FabricCombo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'StyleUkey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyStyleInnovationComebineSubprocess', @level2type = N'COLUMN', @level2name = N'StyleUkey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'M', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyStyleInnovationComebineSubprocess', @level2type = N'COLUMN', @level2name = N'MDivisionID';

