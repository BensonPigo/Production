CREATE TABLE [dbo].[CuttingTime] (
    [WeaveTypeID]  VARCHAR (20)   NOT NULL,
    [SetUpTime]    NUMERIC (8, 3) NULL,
    [WindowTime]   NUMERIC (8, 3) NULL,
    [WindowLength] NUMERIC (8, 3) NULL,
    [EditName]     VARCHAR (10)   NULL,
    [EditDate]     DATETIME       NULL,
    CONSTRAINT [PK_CuttingTime] PRIMARY KEY CLUSTERED ([WeaveTypeID] ASC)
);







