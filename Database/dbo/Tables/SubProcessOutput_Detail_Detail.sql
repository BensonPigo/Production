CREATE TABLE [dbo].[SubProcessOutput_Detail_Detail] (
    [ID]                          VARCHAR (15) NOT NULL,
    [SubprocessOutput_DetailUKey] BIGINT       NOT NULL,
    [OrderId]                     VARCHAR (13) NOT NULL,
    [Article]                     VARCHAR (8)  NOT NULL,
    [SizeCode]                    VARCHAR (8)  NOT NULL,
    [QAQty]                       INT          NULL,
    CONSTRAINT [PK_SubProcessOutput_Detail_Detail] PRIMARY KEY CLUSTERED ([ID] ASC, [SubprocessOutput_DetailUKey] ASC, [OrderId] ASC, [Article] ASC, [SizeCode] ASC)
);

