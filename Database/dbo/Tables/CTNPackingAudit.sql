﻿CREATE TABLE [dbo].[CTNPackingAudit] (
    [ID]               BIGINT       IDENTITY (1, 1) NOT NULL,
    [PackingAuditDate] DATE         NOT NULL,
    [MDivisionID]      VARCHAR (8)  CONSTRAINT [DF_CTNPackingAudit_MDivisionID] DEFAULT ('') NOT NULL,
    [OrderID]          VARCHAR (13) CONSTRAINT [DF_CTNPackingAudit_OrderID] DEFAULT ('') NOT NULL,
    [PackingListID]    VARCHAR (13) CONSTRAINT [DF_CTNPackingAudit_PackingListID] DEFAULT ('') NOT NULL,
    [CTNStartNo]       VARCHAR (6)  CONSTRAINT [DF_CTNPackingAudit_CTNStartNo] DEFAULT ('') NOT NULL,
    [SCICtnNo]         VARCHAR (16) CONSTRAINT [DF_CTNPackingAudit_SCICtnNo] DEFAULT ('') NOT NULL,
    [Qty]              INT          CONSTRAINT [DF_CTNPackingAudit_Qty] DEFAULT ((0)) NOT NULL,
    [Status]           VARCHAR (6)  CONSTRAINT [DF_CTNPackingAudit_Status] DEFAULT ('') NOT NULL,
    [AddName]          VARCHAR (10) CONSTRAINT [DF_CTNPackingAudit_AddName] DEFAULT ('') NOT NULL,
    [AddDate]          DATETIME     NOT NULL,
    [Remark] NVARCHAR(MAX) CONSTRAINT [DF_CTNPackingAudit_Remark] DEFAULT ('') NOT NULL, 
    CONSTRAINT [PK_CTNPackingAudit] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'退回原因備註',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'CTNPackingAudit',
    @level2type = N'COLUMN',
    @level2name = N'Remark'