CREATE TABLE [dbo].[SubProcessSeq] (
    [StyleUkey] BIGINT       DEFAULT ((0)) NOT NULL,
    [AddName]   VARCHAR (10) DEFAULT ('') NOT NULL,
    [AddDate]   DATETIME     NULL,
    [EditName]  VARCHAR (10) DEFAULT ('') NOT NULL,
    [EditDate]  DATETIME     NOT NULL,
    CONSTRAINT [PK_SubProcessSeq] PRIMARY KEY CLUSTERED ([StyleUkey] ASC)
);

