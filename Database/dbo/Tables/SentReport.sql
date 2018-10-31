CREATE TABLE [dbo].[SentReport] (
    [Export_DetailUkey]   BIGINT          NOT NULL,
    [InspectionReport]    DATE            NULL,
    [TPEInspectionReport] DATE            NULL,
    [TestReport]          DATE            NULL,
    [TPETestReport]       DATE            NULL,
    [ContinuityCard]      DATE            NULL,
    [TPEContinuityCard]   DATE            NULL,
    [T2InspYds]           NUMERIC (10, 2) CONSTRAINT [DF_SentReport_T2InspYds] DEFAULT ((0)) NOT NULL,
    [T2DefectPoint]       NUMERIC (5)     CONSTRAINT [DF_SentReport_T2DefectPoint] DEFAULT ((0)) NOT NULL,
    [T2Grade]             VARCHAR (1)     CONSTRAINT [DF_SentReport_T2Grade] DEFAULT ('') NOT NULL,
    [EditName]            VARCHAR (10)    NOT NULL,
    [EditDate]            DATETIME        NOT NULL,
    CONSTRAINT [PK_SentReport] PRIMARY KEY CLUSTERED ([Export_DetailUkey] ASC)
);

