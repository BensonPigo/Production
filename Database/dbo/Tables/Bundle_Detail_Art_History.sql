CREATE TABLE [dbo].[Bundle_Detail_Art_History] (
    [Bundleno]                    VARCHAR (10) CONSTRAINT [DF_Bundle_Detail_Art_History_Bundleno] DEFAULT ('') NOT NULL,
    [SubprocessId]                VARCHAR (10) CONSTRAINT [DF_Bundle_Detail_Art_History_SubprocessId] DEFAULT ('') NOT NULL,
    [PatternCode]                 VARCHAR (20) CONSTRAINT [DF_Bundle_Detail_Art_History_PatternCode] DEFAULT ('') NOT NULL,
    [ID]                          VARCHAR (13) CONSTRAINT [DF_Bundle_Detail_Art_History_ID] DEFAULT ((0)) NOT NULL,
    [Ukey]                        BIGINT       IDENTITY (1, 1) NOT NULL,
    [PostSewingSubProcess]        BIT          DEFAULT ((0)) NOT NULL,
    [NoBundleCardAfterSubprocess] BIT          DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Bundle_Detail_Art_History] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_Detail_Art_History', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�����W��', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_Detail_Art_History', @level2type = N'COLUMN', @level2name = N'PatternCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�[�u����', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_Detail_Art_History', @level2type = N'COLUMN', @level2name = N'SubprocessId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'BundleNo', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_Detail_Art_History', @level2type = N'COLUMN', @level2name = N'Bundleno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'��bundle2.artwork�নtable', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Bundle_Detail_Art_History';

