CREATE TABLE [dbo].[VNConsumption_SizeCode] (
    [ID]       VARCHAR (13) NOT NULL,
    [SizeCode] VARCHAR (8)  NOT NULL,
    CONSTRAINT [PK_Consumption_SizeCode] PRIMARY KEY CLUSTERED ([ID] ASC, [SizeCode] ASC)
);

