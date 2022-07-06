CREATE TABLE [dbo].[CTNPackingAudit_Detail] (
    [ID]              BIGINT      NOT NULL,
    [PackingReasonID] VARCHAR (5) CONSTRAINT [DF_CTNPackingAudit_Detail_PackingReasonID] DEFAULT ('') NOT NULL,
    [Qty]             INT         CONSTRAINT [DF_CTNPackingAudit_Detail_Qty] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_CTNPackingAudit_Detail] PRIMARY KEY CLUSTERED ([ID] ASC, [PackingReasonID] ASC)
);

