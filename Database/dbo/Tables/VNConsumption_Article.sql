CREATE TABLE [dbo].[VNConsumption_Article] (
    [ID]      VARCHAR (13) NOT NULL,
    [Article] VARCHAR (8)  CONSTRAINT [DF_Consumption_Article_Article] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_Consumption_Article] PRIMARY KEY CLUSTERED ([ID] ASC, [Article] ASC)
);

