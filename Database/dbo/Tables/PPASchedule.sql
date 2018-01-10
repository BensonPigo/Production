CREATE TABLE [dbo].[PPASchedule] (
    [ID]                     BIGINT         IDENTITY (1, 1) NOT NULL,
    [OrderID]                VARCHAR (13)   NULL,
    [Group]                  VARCHAR (10)   NULL,
    [SubProcessLineID]       VARCHAR (10)   NULL,
    [OutputQty]              INT            NULL,
    [TargetQty]              INT            NULL,
    [Feature]                VARCHAR (100)  NULL,
    [SMV]                    NUMERIC (7, 4) NULL,
    [EarlyInline]            INT            NULL,
    [SubProcessLearnCurveID] VARCHAR (10)   NULL,
    [Inline]                 DATE           NULL,
    [Offline]                DATE           NULL,
    [AddDate]                DATETIME       NULL,
    [AddName]                VARCHAR (10)   NULL,
    [EditDate]               DATETIME       NULL,
    [EditName]               VARCHAR (10)   NULL,
    CONSTRAINT [PK_PPASchedule] PRIMARY KEY CLUSTERED ([ID] ASC)
);

