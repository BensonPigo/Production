CREATE TABLE [dbo].[Thread_Replace_Detail] (
    [Ukey]               BIGINT       IDENTITY (1, 1) NOT NULL,
    [Thread_ReplaceUkey] BIGINT       NOT NULL,
    [StartDate]          DATE         NOT NULL,
    [EndDate]            DATE         NULL,
    [AddName]            VARCHAR (10) NOT NULL,
    [AddDate]            DATETIME     NULL,
    [EditName]           VARCHAR (10) NULL,
    [EditDate]           DATETIME     NULL,
    CONSTRAINT [PK_Thread_Replace_Detail] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);

