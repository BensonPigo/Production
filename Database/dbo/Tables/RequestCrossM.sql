CREATE TABLE [dbo].[RequestCrossM] (
    [Id]            VARCHAR (13)  CONSTRAINT [DF_RequestCrossM_Id] DEFAULT ('') NOT NULL,
    [Type]          CHAR (1)      CONSTRAINT [DF_RequestCrossM_Type] DEFAULT ('') NOT NULL,
    [IssueDate]     DATE          NOT NULL,
    [MDivisionID]   VARCHAR (8)   CONSTRAINT [DF_RequestCrossM_MDivisionID] DEFAULT ('') NOT NULL,
    [ToMDivisionID] VARCHAR (8)   CONSTRAINT [DF_RequestCrossM_ToMDivisionID] DEFAULT ('') NOT NULL,
    [Status]        VARCHAR (15)  CONSTRAINT [DF_RequestCrossM_Status] DEFAULT ('') NULL,
    [ReferenceID]   VARCHAR (13)  NULL,
    [Remark]        NVARCHAR (60) CONSTRAINT [DF_RequestCrossM_Remark] DEFAULT ('') NULL,
    [AddName]       VARCHAR (10)  CONSTRAINT [DF_RequestCrossM_AddName] DEFAULT ('') NULL,
    [AddDate]       DATETIME      NULL,
    [EditName]      VARCHAR (10)  CONSTRAINT [DF_RequestCrossM_EditName] DEFAULT ('') NULL,
    [EditDate]      DATETIME      NULL,
    CONSTRAINT [PK_RequestCrossM] PRIMARY KEY CLUSTERED ([Id] ASC)
);

