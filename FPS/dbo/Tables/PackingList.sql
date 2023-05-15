CREATE TABLE [dbo].[PackingList] (
    [id]       VARCHAR (13) NOT NULL,
    [AddDate]  DATETIME     NOT NULL,
    [EditDate] DATETIME     NULL,
    [CmdTime]  DATETIME     NOT NULL,
    [junk]     BIT          DEFAULT ((0)) NULL,
    CONSTRAINT [PK_PackingList] PRIMARY KEY CLUSTERED ([id] ASC)
);

