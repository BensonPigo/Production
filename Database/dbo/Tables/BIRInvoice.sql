CREATE TABLE [dbo].[BIRInvoice] (
    [ID]           VARCHAR (13)   CONSTRAINT [DF__BIRInvoice__ID__049D3791] DEFAULT ('') NOT NULL,
    [AddName]      VARCHAR (10)   NULL,
    [AddDate]      DATETIME       NULL,
    [EditName]     VARCHAR (10)   NULL,
    [EditDate]     DATETIME       NULL,
    [ExVoucherID]  VARCHAR (16)   NULL,
    [Status]       VARCHAR (15)   NULL,
    [Approve]      VARCHAR (10)   NULL,
    [ApproveDate]  DATETIME       NULL,
    [InvDate]      DATE           NULL,
    [ExchangeRate] SMALLINT       NULL,
    [Remark]       NVARCHAR (500) NULL,
    CONSTRAINT [PK_BIRInvoice] PRIMARY KEY CLUSTERED ([ID] ASC)
);






GO



GO



GO



GO


