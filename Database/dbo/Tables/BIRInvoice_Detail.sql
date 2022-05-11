CREATE TABLE [dbo].[BIRInvoice_Detail] (
    [ID]    VARCHAR (13) CONSTRAINT [DF_BIRInvoice_Detail_ID] DEFAULT ('') NOT NULL,
    [InvNo] VARCHAR (25) CONSTRAINT [DF_BIRInvoice_Detail_InvNo] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_BIRInvoice_Detail] PRIMARY KEY CLUSTERED ([InvNo] ASC)
);

