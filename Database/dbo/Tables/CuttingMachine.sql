CREATE TABLE [dbo].[CuttingMachine] (
    [ID]          VARCHAR (10)  NOT NULL,
    [Description] NVARCHAR (60) NULL,
    [Junk]        BIT           NULL,
    [AddName]     VARCHAR (10)  NULL,
    [AddDate]     DATETIME      NULL,
    [EditName]    VARCHAR (10)  NULL,
    [EditDate]    DATETIME      NULL,
    CONSTRAINT [PK_CuttingMachine] PRIMARY KEY CLUSTERED ([ID] ASC)
);

