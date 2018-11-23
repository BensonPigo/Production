CREATE TABLE [dbo].[ECons_MNoticeFailed] (
    [ID]            VARCHAR (13)   NOT NULL,
    [Type]          VARCHAR (2)    NOT NULL,
    [KPIDate]       DATE           NULL,
    [KPIFailed]     VARCHAR (1)    NULL,
    [FailedComment] VARCHAR (100)  NULL,
    [ExpectApvDate] DATE           NULL,
    [ErrorMessage]  NVARCHAR (500) NULL,
    [AddName]       VARCHAR (10)   NULL,
    [AddDate]       DATETIME       NULL,
    [EditName]      VARCHAR (10)   NULL,
    [EditDate]      DATETIME       NULL,
    CONSTRAINT [PK_ECons_MNoticeFailed] PRIMARY KEY CLUSTERED ([ID] ASC, [Type] ASC)
);



