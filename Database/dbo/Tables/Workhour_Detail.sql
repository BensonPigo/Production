CREATE TABLE [dbo].[Workhour_Detail] (
    [SewingLineID] VARCHAR (5) NULL,
    [FactoryID]    VARCHAR (8) NULL,
    [Date]         DATE        NULL,
    [StartHour]    FLOAT (53)  NULL,
    [EndHour]      FLOAT (53)  NULL,
    [UKey]         BIGINT      IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    CONSTRAINT [PK_Workhour_Detail] PRIMARY KEY CLUSTERED ([UKey] ASC)
);


GO
CREATE NONCLUSTERED INDEX [WDforP_SewingLineSchedule]
    ON [dbo].[Workhour_Detail]([SewingLineID] ASC, [FactoryID] ASC, [Date] ASC)
    INCLUDE([StartHour], [EndHour]);

