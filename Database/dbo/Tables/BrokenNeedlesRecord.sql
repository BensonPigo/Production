CREATE TABLE [dbo].[BrokenNeedlesRecord] (
    [ID]             BIGINT       IDENTITY (1, 1) NOT NULL,
    [SP]             VARCHAR (13) CONSTRAINT [DF_BrokenNeedlesRecord_SP] DEFAULT ('') NOT NULL,
    [Line]           VARCHAR (5)  CONSTRAINT [DF_BrokenNeedlesRecord_Line] DEFAULT ('') NOT NULL,
    [Shift]          VARCHAR (5)  CONSTRAINT [DF_BrokenNeedlesRecord_Shift] DEFAULT ('') NOT NULL,
    [NeedleComplete] BIT          CONSTRAINT [DF_BrokenNeedlesRecord_NeedleComplete] DEFAULT ((0)) NOT NULL,
    [Operation]      VARCHAR (50) CONSTRAINT [DF_BrokenNeedlesRecord_Operation] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_BrokenNeedlesRecord] PRIMARY KEY CLUSTERED ([ID] ASC)
);

