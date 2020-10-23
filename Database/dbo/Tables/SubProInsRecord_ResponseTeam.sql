CREATE TABLE [dbo].[SubProInsRecord_ResponseTeam] (
    [Ukey]                 BIGINT       IDENTITY (1, 1) NOT NULL,
    [SubProResponseTeamID] VARCHAR (50) NOT NULL,
    [SubProInsRecordUkey]  BIGINT       NOT NULL,
    [StartResolveDate]     DATETIME     NULL,
    [EndResolveDate]       DATETIME     NULL,
    [AddName]              VARCHAR (10) NULL,
    [Editname]             VARCHAR (10) NULL,
    CONSTRAINT [PK_SubProInsRecord_ResponseTeam] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);

