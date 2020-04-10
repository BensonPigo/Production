CREATE TABLE [dbo].[MachineType_ThreadRatio_Hem] (
    [ID]           VARCHAR (10)   NOT NULL,
    [Seq]          VARCHAR (2)    NOT NULL,
    [UseRatioRule] VARCHAR (1)    NOT NULL,
    [UseRatio]     NUMERIC (5, 2) NULL,
    [Ukey]         BIGINT         NOT NULL,
    CONSTRAINT [PK_MachineType_ThreadRatio_Hem] PRIMARY KEY CLUSTERED ([ID] ASC, [Seq] ASC, [UseRatioRule] ASC)
);

