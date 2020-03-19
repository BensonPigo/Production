CREATE TABLE [dbo].[BundleReplacement_Detail_Art] (
    [Bundleno]                    VARCHAR (10) NOT NULL,
    [SubprocessId]                VARCHAR (10) NOT NULL,
    [PatternCode]                 VARCHAR (20) NOT NULL,
    [ID]                          VARCHAR (13) NOT NULL,
    [Ukey]                        BIGINT       IDENTITY (1, 1) NOT NULL,
    [PostSewingSubProcess]        BIT          NOT NULL,
    [NoBundleCardAfterSubprocess] BIT          NOT NULL,
    CONSTRAINT [PK_BundleReplacement_Detail_Art] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);

