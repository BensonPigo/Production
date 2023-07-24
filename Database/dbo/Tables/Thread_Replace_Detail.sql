CREATE TABLE [dbo].[Thread_Replace_Detail] (
    [Ukey]               BIGINT       IDENTITY (1, 1) NOT NULL,
    [Thread_ReplaceUkey] BIGINT       NOT NULL,
    [StartDate]          DATE         NOT NULL,
    [EndDate]            DATE         NULL,
    [AddName]            VARCHAR (10) NOT NULL,
    [AddDate]            DATETIME     NULL,
    [EditName]           VARCHAR (10) CONSTRAINT [DF_Thread_Replace_Detail_EditName] DEFAULT ('') NOT NULL,
    [EditDate]           DATETIME     NULL,
    CONSTRAINT [PK_Thread_Replace_Detail] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);



