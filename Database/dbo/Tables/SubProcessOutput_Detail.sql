CREATE TABLE [dbo].[SubProcessOutput_Detail] (
    [Ukey]             BIGINT         IDENTITY (1, 1) NOT NULL,
    [ID]               VARCHAR (15)   NULL,
    [SubprocessLineID] VARCHAR (15)   NULL,
    [OrderId]          VARCHAR (13)   NULL,
    [Article]          VARCHAR (8)    NULL,
    [Color]            VARCHAR (6)    NULL,
    [QAQty]            INT            NULL,
    [ProdQty]          INT            NULL,
    [DefectQty]        INT            NULL,
    [Feature]          VARCHAR (100)  NULL,
    [SMV]              INT            NULL,
    [FeatureCPU]       NUMERIC (6, 4) NULL,
    [Manpower]         NUMERIC (5, 2) NULL,
    [Workinghours]     NUMERIC (5, 2) NULL,
    [TTLWorkinghours]  NUMERIC (7, 2) NULL,
    [EFF]              INT            NULL,
    [TotalCPU]         NUMERIC (6, 2) NULL,
    [PPH]              NUMERIC (6, 3) NULL,
    [Remark]           NVARCHAR (100) NULL,
    CONSTRAINT [PK_SubProcessOutput_Detail] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);



