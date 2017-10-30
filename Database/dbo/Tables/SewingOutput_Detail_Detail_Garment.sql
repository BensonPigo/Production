CREATE TABLE [dbo].[SewingOutput_Detail_Detail_Garment] (
    [ID]                      VARCHAR (13) NOT NULL,
    [SewingOutput_DetailUKey] BIGINT       NOT NULL,
    [OrderId]                 VARCHAR (13) NOT NULL,
    [ComboType]               VARCHAR (1)  NOT NULL,
    [Article]                 VARCHAR (8)  NOT NULL,
    [SizeCode]                VARCHAR (8)  NOT NULL,
    [OrderIDfrom]             VARCHAR (13) NOT NULL,
    [QAQty]                   INT          NOT NULL,
    CONSTRAINT [PK_SewingOutput_Detail_Detail_Garment_1] PRIMARY KEY CLUSTERED ([ID] ASC, [SewingOutput_DetailUKey] ASC, [OrderId] ASC, [ComboType] ASC, [Article] ASC, [SizeCode] ASC, [OrderIDfrom] ASC)
);



