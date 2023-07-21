CREATE TABLE [dbo].[MachineType_ThreadRatio_Regular] (
    [ID]           VARCHAR (10)   NOT NULL,
    [Seq]          VARCHAR (2)    NOT NULL,
    [UseRatioRule] VARCHAR (1)    NOT NULL,
    [UseRatio]     DECIMAL (5, 2) CONSTRAINT [DF_MachineType_ThreadRatio_Regular_UseRatio] DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC, [Seq] ASC, [UseRatioRule] ASC)
);

