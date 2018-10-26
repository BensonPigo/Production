CREATE TABLE [dbo].[MockupWash_Detail] (
    [ID]                  VARCHAR (13)  NOT NULL,
    [ReportNo]            VARCHAR (13)  NOT NULL,
    [No]                  INT           NOT NULL,
    [SubmitDate]          DATE          NULL,
    [CombineStyle]        VARCHAR (120) DEFAULT ('') NULL,
    [Result]              VARCHAR (4)   DEFAULT ('') NULL,
    [ReceivedDate]        DATETIME      NULL,
    [ReleasedDate]        DATETIME      NULL,
    [Technician]          VARCHAR (10)  DEFAULT ('') NULL,
    [MR]                  VARCHAR (10)  DEFAULT ('') NULL,
    [TestingMethod]       VARCHAR (250) DEFAULT ('') NULL,
    [AddDate]             DATETIME      NULL,
    [AddName]             VARCHAR (10)  DEFAULT ('') NULL,
    [EditDate]            DATETIME      NULL,
    [EditName]            VARCHAR (10)  DEFAULT ('') NULL,
    [HTPlate]             INT           NULL,
    [HTFlim]              INT           NULL,
    [HTTime]              INT           NULL,
    [HTPressure]          INT           NULL,
    [HTPellOff]           VARCHAR (5)   NULL,
    [HT2ndPressnoreverse] INT           NULL,
    [HT2ndPressreversed]  INT           NULL,
    [HTCoolingTime]       INT           NULL,
    PRIMARY KEY CLUSTERED ([ReportNo] ASC)
);


