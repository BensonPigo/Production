CREATE TABLE [dbo].[LossRateAccessory_Limit] (
    [MtltypeId] VARCHAR (20)   NOT NULL,
    [UsageUnit] VARCHAR (8)    NOT NULL,
    [LimitUp]   DECIMAL (7, 2) CONSTRAINT [DF_LossRateAccessory_Limit_LimitUp] DEFAULT ((0)) NOT NULL,
    [AddName]   VARCHAR (10)   CONSTRAINT [DF_LossRateAccessory_Limit_AddName] DEFAULT ('') NOT NULL,
    [AddDate]   DATETIME       NULL,
    [EditName]  VARCHAR (10)   CONSTRAINT [DF_LossRateAccessory_Limit_EditName] DEFAULT ('') NOT NULL,
    [EditDate]  DATETIME       NULL,
    CONSTRAINT [PK_LossRateAccessory_Limit] PRIMARY KEY CLUSTERED ([MtltypeId] ASC, [UsageUnit] ASC)
);



