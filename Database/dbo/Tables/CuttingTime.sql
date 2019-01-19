CREATE TABLE [dbo].[CuttingTime] (
    [MtlTypeID]    VARCHAR (20)   NOT NULL,
    [SetUpTime]    NUMERIC (8, 3) NULL,
    [WindowTime]   NUMERIC (8, 3) NULL,
    [WindowLength] NUMERIC (8, 3) NULL,
    CONSTRAINT [PK_CuttingTime] PRIMARY KEY CLUSTERED ([MtlTypeID] ASC)
);



