CREATE TABLE [dbo].[SubProcessSeq_Detail] (
    [StyleUkey]    BIGINT       DEFAULT ((0)) NOT NULL,
    [SubProcessID] VARCHAR (15) DEFAULT ('') NOT NULL,
    [Seq]          TINYINT      DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_SubProcessSeq_Detail] PRIMARY KEY CLUSTERED ([StyleUkey] ASC, [SubProcessID] ASC)
);

