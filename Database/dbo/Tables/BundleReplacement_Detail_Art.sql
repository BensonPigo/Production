CREATE TABLE [dbo].[BundleReplacement_Detail_Art] (
    [Bundleno]                    VARCHAR (10) DEFAULT ('') NOT NULL,
    [SubprocessId]                VARCHAR (10) DEFAULT ('') NOT NULL,
    [PatternCode]                 VARCHAR (20) DEFAULT ('') NOT NULL,
    [ID]                          VARCHAR (13) DEFAULT ('') NOT NULL,
    [Ukey]                        BIGINT       IDENTITY (1, 1) NOT NULL,
    [PostSewingSubProcess]        BIT          DEFAULT ((0)) NOT NULL,
    [NoBundleCardAfterSubprocess] BIT          DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_BundleReplacement_Detail_Art] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);



