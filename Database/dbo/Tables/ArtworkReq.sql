CREATE TABLE [dbo].[ArtworkReq] (
    [ID]               VARCHAR (13)  DEFAULT ('') NOT NULL,
    [MDivisionID]      VARCHAR (8)   DEFAULT ('') NOT NULL,
    [FactoryID]        VARCHAR (8)   DEFAULT ('') NOT NULL,
    [LocalSuppID]      VARCHAR (8)   DEFAULT ('') NOT NULL,
    [ReqDate]          DATE          DEFAULT ('') NOT NULL,
    [ArtworkTypeID]    VARCHAR (20)  DEFAULT ('') NOT NULL,
    [Handle]           VARCHAR (10)  DEFAULT ('') NOT NULL,
    [DeptApvName]      VARCHAR (10)  DEFAULT ('') NOT NULL,
    [DeptApvDate]      DATETIME      NULL,
    [MgApvName]        VARCHAR (10)  DEFAULT ('') NOT NULL,
    [MgApvDate]        DATETIME      NULL,
    [CloseUnCloseName] VARCHAR (10)  DEFAULT ('') NOT NULL,
    [CloseUnCloseDate] DATETIME      NULL,
    [Remark]           NVARCHAR (60) DEFAULT ('') NOT NULL,
    [Status]           VARCHAR (15)  DEFAULT ('') NOT NULL,
    [OriStatus]        VARCHAR (15)  DEFAULT ('') NOT NULL,
    [Exceed]           BIT           DEFAULT ((0)) NOT NULL,
    [AddName]          VARCHAR (10)  DEFAULT ('') NOT NULL,
    [AddDate]          DATETIME      NOT NULL,
    [EditName]         VARCHAR (10)  DEFAULT ('') NOT NULL,
    [EditDate]         DATETIME      NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);


