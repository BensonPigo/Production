CREATE TABLE [dbo].[RequestCrossM_Detail] (
    [Id]              VARCHAR (13)    CONSTRAINT [DF_RequestCrossM_Summary_Id] DEFAULT ('') NOT NULL,
    [Ukey]            BIGINT          IDENTITY (1, 1) NOT NULL,
    [FromMDivisionID] VARCHAR (8)     CONSTRAINT [DF_RequestCrossM_Summary_FromMDivisionID] DEFAULT ('') NOT NULL,
    [FromPOID]        VARCHAR (13)    CONSTRAINT [DF_RequestCrossM_Summary_FromPOID] DEFAULT ('') NOT NULL,
    [FromSeq1]        VARCHAR (3)     CONSTRAINT [DF_RequestCrossM_Summary_FromSeq1] DEFAULT ('') NOT NULL,
    [FromSeq2]        VARCHAR (2)     CONSTRAINT [DF_RequestCrossM_Summary_FromSeq2] DEFAULT ('') NOT NULL,
    [ToMDivisionID]   VARCHAR (8)     CONSTRAINT [DF_RequestCrossM_Summary_ToMDivisionID] DEFAULT ('') NOT NULL,
    [ToPOID]          VARCHAR (13)    CONSTRAINT [DF_RequestCrossM_Summary_ToPOID] DEFAULT ('') NOT NULL,
    [ToSeq1]          VARCHAR (3)     CONSTRAINT [DF_RequestCrossM_Summary_ToSeq1] DEFAULT ('') NOT NULL,
    [ToSeq2]          VARCHAR (2)     CONSTRAINT [DF_RequestCrossM_Summary_ToSeq2] DEFAULT ('') NOT NULL,
    [Qty]             NUMERIC (10, 2) CONSTRAINT [DF_RequestCrossM_Summary_Qty] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_RequestCrossM_Summary_1] PRIMARY KEY CLUSTERED ([Id] ASC, [Ukey] ASC),
    CONSTRAINT [FK_RequestCrossM_Summary_RequestCrossM] FOREIGN KEY ([Id]) REFERENCES [dbo].[RequestCrossM] ([Id]) NOT FOR REPLICATION
);


GO
ALTER TABLE [dbo].[RequestCrossM_Detail] NOCHECK CONSTRAINT [FK_RequestCrossM_Summary_RequestCrossM];

