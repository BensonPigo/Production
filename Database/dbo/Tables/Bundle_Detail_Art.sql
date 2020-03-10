CREATE TABLE [dbo].[Bundle_Detail_Art] (
    [Bundleno]                    VARCHAR (10) CONSTRAINT [DF_Bundle_Detail_Art_Bundleno] DEFAULT ('') NOT NULL,
    [SubprocessId]                VARCHAR (10) CONSTRAINT [DF_Bundle_Detail_Art_SubprocessId] DEFAULT ('') NOT NULL,
    [PatternCode]                 VARCHAR (20) CONSTRAINT [DF_Bundle_Detail_Art_PatternCode] DEFAULT ('') NOT NULL,
    [ID]                          VARCHAR (13) CONSTRAINT [DF_Bundle_Detail_Art_ID] DEFAULT ((0)) NOT NULL,
    [Ukey]                        BIGINT       IDENTITY (1, 1) NOT NULL,
    [PostSewingSubProcess]        BIT          DEFAULT ((0)) NOT NULL,
    [NoBundleCardAfterSubprocess] BIT          DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Bundle_Detail_Art] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);










GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'原bundle2.artwork轉成table', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_Detail_Art';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'BundleNo', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_Detail_Art', @level2type = N'COLUMN', @level2name = N'Bundleno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'加工項目', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_Detail_Art', @level2type = N'COLUMN', @level2name = N'SubprocessId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'版片名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_Detail_Art', @level2type = N'COLUMN', @level2name = N'PatternCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_Detail_Art', @level2type = N'COLUMN', @level2name = N'ID';


GO
CREATE NONCLUSTERED INDEX [ID_PC_Bundleno]
    ON [dbo].[Bundle_Detail_Art]([Bundleno] ASC, [PatternCode] ASC, [ID] ASC);

GO
CREATE NONCLUSTERED INDEX [ID_Bundleno_SubID]
    ON [dbo].[Bundle_Detail_Art]([Bundleno] ASC, [SubprocessId] ASC, [ID] ASC);