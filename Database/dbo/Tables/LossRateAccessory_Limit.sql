CREATE TABLE [dbo].[LossRateAccessory_Limit] (
    [MtltypeId] VARCHAR (20)   NOT NULL,
    [UsageUnit] VARCHAR (8)    NOT NULL,
    [LimitUp]   NUMERIC (7, 2) NULL,
    [AddName]   VARCHAR (10)   NULL,
    [AddDate]   DATETIME       NULL,
    [EditName]  VARCHAR (10)   NULL,
    [EditDate]  DATETIME       NULL,
    CONSTRAINT [PK_LossRateAccessory_Limit] PRIMARY KEY CLUSTERED ([MtltypeId] ASC, [UsageUnit] ASC)
);

