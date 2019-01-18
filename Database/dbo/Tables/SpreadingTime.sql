CREATE TABLE [dbo].[SpreadingTime] (
    [MtlTypeID]            VARCHAR (20)   NOT NULL,
    [PreparationTime]      NUMERIC (8, 3) NULL,
    [ChangeOverRollTime]   NUMERIC (8, 3) NULL,
    [ChangeOverUnRollTime] NUMERIC (8, 3) NULL,
    [SetupTime]            NUMERIC (8, 3) NULL,
    [SeparatorTime]        NUMERIC (8, 3) NULL,
    [SpreadingTime]        NUMERIC (8, 3) NULL,
    [ForwardTime]          NUMERIC (8, 3) NULL,
    [AddName]              VARCHAR (10)   NULL,
    [AddDate]              DATETIME       NULL,
    [EditName]             VARCHAR (10)   NULL,
    [EditDate]             DATETIME       NULL,
    CONSTRAINT [PK_SpreadingTime] PRIMARY KEY CLUSTERED ([MtlTypeID] ASC)
);

